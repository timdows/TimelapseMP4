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
		
		public static void GetFilesAndSaveResized(string sourceDirectory, string destinationDirectory)
		{
			// Check if source directory exists
			if (!Directory.Exists(sourceDirectory))
			{
				throw new Exception($"SourceImageLocation {sourceDirectory} does not exist");
			}

			// Create directory if needed
			if (!Directory.Exists(destinationDirectory))
			{
				Console.WriteLine($"Creating destination directory {destinationDirectory}");
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

				if (File.Exists(destinationPath))
				{
					Console.WriteLine($"File {fileToCopy.FileName} already exists in save directory {destinationPath}");
					continue;
				}

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
		}
	}
}