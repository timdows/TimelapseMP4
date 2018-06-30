﻿using Microsoft.Extensions.Configuration;
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
		public static async Task Main(string[] args)
		{
			//Get1400HourFiles();

			while (true)
			{
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

			//UnsortedFiles(appSettings);
			//await CreateTimelapseMP4FromUnsortedFiles(appSettings);

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

				await GetFilesAndSaveResizedCommand.GetFilesAndSaveResized(sourceDirectory, destinationDirectory);
				//await CreateTimelapseMP4(appSettings, destinationDirectory, date);
			}
		}

		// public static async Task CreateTimelapseMP4FromUnsortedFiles(AppSettings appSettings)
		// {
		// 	var sources = Directory.EnumerateDirectories(appSettings.LocalImageLocation);
		// 	foreach (var source in sources)
		// 	{
		// 		var date = Path.GetFileName(source);
		// 		await CreateTimelapseMP4(appSettings, source, date);
		// 	}
		// }

		
	}
}
