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
	public static class UnsortedFilesCommand
	{
		public static void UnsortedFiles(AppSettings appSettings)
		{
			var filesToCopy = Directory.GetFiles(appSettings.UnsortedImagesDirectory, "*.jpg", SearchOption.AllDirectories)
				.Select(item => ImageFileDetails.CreateImageFromEpochFile(item))
				.ToList();
			filesToCopy.RemoveAll(item => item == null);
			filesToCopy = filesToCopy.Distinct().ToList();

			// Sort the files so that renaming with index is possible
			filesToCopy = filesToCopy.OrderBy(item => item.DateTimeTaken).ToList();

			var finishedDates = Directory.GetDirectories(appSettings.LocalImageLocation)
				.Select(item => {
					DateTime.TryParseExact(
						Path.GetFileName(item),
						"yyyy-MM-dd",
						System.Globalization.CultureInfo.InvariantCulture,
						System.Globalization.DateTimeStyles.None, out var parsedDate);
					return parsedDate;
				})
				.ToList();
			filesToCopy.RemoveAll(item => finishedDates.Contains(item.DateTimeTaken.Date));

			Console.WriteLine($"Total files in directory {appSettings.UnsortedImagesDirectory}: {filesToCopy.Count}");

			var index = 0;
			var overallIndex = 0;
			var stopwatch = Stopwatch.StartNew();

			foreach (var fileToCopy in filesToCopy)
			{
				stopwatch.Restart();

				// Create destinationDirectory if needed and reset index (files are sorted)
				var destinationDirectory = Path.Combine(appSettings.LocalImageLocation, fileToCopy.DateTimeTaken.ToString("yyyy-MM-dd"));
				if (!Directory.Exists(destinationDirectory))
				{
					Directory.CreateDirectory(destinationDirectory);
					index = 0;
				}

				var localFileName = $"image_{index++.ToString("D4")}.jpg";
				var destinationPath = Path.Combine(destinationDirectory, localFileName);
				long downloadTimeInSeconds = 0;
				try
				{
					// Load the image and save a resized version
					using (var image = Image.Load(fileToCopy.Path))
					{
						downloadTimeInSeconds = stopwatch.ElapsedMilliseconds;
						image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
						image.Save(destinationPath);
					}

					var info = $"Finished copying and resizing file: {fileToCopy.FileName} date {fileToCopy.DateTimeTaken.ToString("yyyy-MM-dd HH:mm:ss")}. " +
						$"File {overallIndex++}/{filesToCopy.Count}. " +
						$"Statistics {downloadTimeInSeconds} - {stopwatch.ElapsedMilliseconds}";
					Console.WriteLine(info);
				}
				catch (Exception excep)
				{
					index--;
					// Oops, 0 kb file?
				}
			}
		}
	}
}