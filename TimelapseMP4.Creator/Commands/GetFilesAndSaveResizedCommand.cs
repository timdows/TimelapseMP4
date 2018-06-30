using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using TimelapseMP4.Creator.Models;

namespace TimelapseMP4.Creator.Commands
{
	public static class GetFilesAndSaveResizedCommand
	{
		const string FinishedPathsLogFile = "finishedPaths.log";
		public static async Task GetFilesAndSaveResized(string sourceDirectory, string destinationDirectory)
		{
			if (await IsPathInFinishedFile(sourceDirectory))
			{
				Console.WriteLine($"Skipping copy files and resize for directory {sourceDirectory}");
				return;
			}

			// Check if source directory exists
			if (!Directory.Exists(sourceDirectory))
			{
				throw new Exception($"SourceImageLocation {sourceDirectory} does not exist");
			}

			// Remove content if the directory exists
			if (Directory.Exists(destinationDirectory))
			{
				var directoryInfo = new DirectoryInfo(destinationDirectory);
				foreach (var file in directoryInfo.EnumerateFiles())
				{
					file.Delete();
				}
			}
			else
			{
				Directory.CreateDirectory(destinationDirectory);
			}
			
			var filesToCopy = Directory.GetFiles(sourceDirectory, "*.jpg")
				.Select(item => ImageFileDetails.CreateImageFileDetails(item))
				.ToList();
			filesToCopy.RemoveAll(item => item == null);

			// Sort the files so that renaming with index is possible
			filesToCopy = filesToCopy.OrderBy(item => item.DateTimeTaken).ToList();

			Console.WriteLine($"Total files in source directory {sourceDirectory}: {filesToCopy.Count}");

			var index = 0;
			var stopwatch = Stopwatch.StartNew();
			foreach (var fileToCopy in filesToCopy)
			{
				stopwatch.Restart();

				var localFileName = $"image_{index++.ToString("D4")}.jpg";
				var destinationPath = Path.Combine(destinationDirectory, localFileName);
				long downloadTimeInSeconds = 0;

				// Load the image and save a resized version
				using (var image = Image.Load(fileToCopy.Path))
				{
					downloadTimeInSeconds = stopwatch.ElapsedMilliseconds;
					image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
					image.Save(destinationPath);
				}

				var info = $"Finished copying and resizing file: {fileToCopy.FileName}. File {index}/{filesToCopy.Count}. Statistics {downloadTimeInSeconds} - {stopwatch.ElapsedMilliseconds}";
				Console.WriteLine(info);
			}

			await AddPathToFinishedFile(sourceDirectory);
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