using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TimelapseMP4.Creator.Commands;
using TimelapseMP4.Creator.Models;

namespace TimelapseMP4.Creator
{
	public class Program
	{
		const string FinishedPathsLogFile = "finishedPaths.log";

		public static async Task Main(string[] args)
		{
			while (true)
			{
				Console.WriteLine($"Running commands, start at {DateTime.Now.ToShortTimeString()}");
				await Task.WhenAll(
					Run(),
					Task.Delay(60 * 60 * 1000));
			}
		}

		public static async Task Run()
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var appSettings = config.GetSection("AppSettings").Get<AppSettings>();
			var get1400HourFileCommand = new Get1400HourFileCommand(appSettings);

			var directories = Directory.EnumerateDirectories(appSettings.SourceImageLocation);
			foreach (var directory in directories)
			{
				var date = Path.GetFileName(directory);
				if (date.Equals(DateTime.Today.ToString("yyyy-MM-dd"), StringComparison.CurrentCultureIgnoreCase))
				{
					continue;
				}

				var sourceDirectory = Path.Combine(appSettings.SourceImageLocation, date);
				var destinationDirectory = Path.Combine(appSettings.LocalImageLocation, date);

				if (await IsPathInFinishedFile(sourceDirectory))
				{
					Console.WriteLine($"Skipping copy files and resize for directory {sourceDirectory}");
					return;
				}

				Console.WriteLine($"Working with directory {sourceDirectory}");

				await GetFilesAndSaveResizedCommand.GetFilesAndSaveResized(sourceDirectory, destinationDirectory);
				await get1400HourFileCommand.Get1400HourFile(sourceDirectory);
				//await CreateTimelapseMP4(appSettings, destinationDirectory, date);

				await AddPathToFinishedFile(sourceDirectory);
			}
		}

		private static async Task<bool> IsPathInFinishedFile(string path)
		{
			if (!File.Exists(FinishedPathsLogFile))
			{
				return false;
			}

			var lines = await File.ReadAllLinesAsync(FinishedPathsLogFile);
			return lines.Contains(path);
		}

		private static async Task AddPathToFinishedFile(string path)
		{
			await File.AppendAllTextAsync(FinishedPathsLogFile, $"{path}\r\n");
		}
	}
}
