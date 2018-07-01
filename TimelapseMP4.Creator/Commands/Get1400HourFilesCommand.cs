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
	public static class Get1400HourFilesCommand
	{
		public static void Get1400HourFiles()
		{
			var saveDir = "1400HourFiles";
			if (!Directory.Exists(saveDir))
			{
				Directory.CreateDirectory(saveDir);
			}

			var fileGroup = Directory.GetFiles(@"\\192.168.1.14\projects\VWP Timelapse\timelapse construction", "*.jpg", SearchOption.AllDirectories)
				.Select(item => ImageFileDetails.CreateImageFileDetails(item))
				.Where(item => item != null)
				.GroupBy(item => item.DateTimeTaken.Date)
				.ToList();

			foreach (var files in fileGroup)
			{
				var fileAfter = files
					.OrderBy(item => item.DateTimeTaken)
					.FirstOrDefault(item => item.DateTimeTaken.Hour >= 14);

				var fileBefore = files
					.OrderByDescending(item => item.DateTimeTaken)
					.FirstOrDefault(item => item.DateTimeTaken.Hour < 14);

				ImageFileDetails saveFile = null;
				if ((fileAfter?.DateTimeTaken.Hour ?? 23) - 14 <= 14 - (fileBefore?.DateTimeTaken.Hour ?? 0))
				{
					saveFile = fileAfter;
				}

				if ((fileAfter?.DateTimeTaken.Hour ?? 23) - 14 >= 14 - (fileBefore?.DateTimeTaken.Hour ?? 0))
				{
					saveFile = fileBefore;
				}

				if (saveFile == null && fileBefore != null)
				{
					saveFile = fileBefore;
				}

				if (saveFile == null)
				{
					continue;
				}

				try
				{
					// Load the image and save a resized version
					using (var image = Image.Load(saveFile.Path))
					{
						image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
						image.Save($"{saveDir}\\{saveFile.DateTimeTaken.ToString("yyyy-MM-ddTHHmmss")}.jpg");
					}

					// Load the image and save a thumbnail
					using (var image = Image.Load(saveFile.Path))
					{
						var height = 200;
						decimal widthRatio = image.Height / height;
						int width = (int)Math.Round(image.Width / widthRatio, 0);
						image.Mutate(x => x.Resize(width, height));
						image.Save($"{saveDir}\\{saveFile.DateTimeTaken.ToString("yyyy-MM-ddTHHmmss")}_thumb.jpg");
					}
				}
				catch (Exception excep)
				{
					var a = excep.Message;
				}
			}
		}
	}
}