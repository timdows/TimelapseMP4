using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using TimelapseMP4.Creator.Models;
using TimelapseMP4.Creator.Services;

namespace TimelapseMP4.Creator.Commands
{
	public class Get1400HourFileCommand
	{
		private readonly AppSettings _appSettings;

		public Get1400HourFileCommand(AppSettings appSettings)
		{
			_appSettings = appSettings;
		}
		
		public async Task Get1400HourFile(string sourceDirectory)
		{
			var saveDir = "Files/Hour1400";
			if (!Directory.Exists(saveDir))
			{
				Directory.CreateDirectory(saveDir);
			}

			var fileGroup = Directory.GetFiles(sourceDirectory, "*.jpg", SearchOption.AllDirectories)
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

				Console.WriteLine($"Hour1400 file: {saveFile.Path}");

				try
				{
					using (var client = new TimelapseMP4Webpage
					{
						BaseUri = new Uri(_appSettings.ServiceApiLocation)
					})
					{
						// Load the image and save a resized version
						using (var image = Image.Load(saveFile.Path))
						{
							var fileName = $"{saveFile.DateTimeTaken.ToString("yyyy-MM-ddTHHmmss")}.jpg";
							image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
							image.Save($"{saveDir}/{fileName}");

							var uploadResponse = client.ApiHour1400UploadPost(new Services.Models.Hour1400UploadRequest
							{
								FileName = fileName,
								Secret = _appSettings.Hour1400UploadSecret,
								Bytes = File.ReadAllBytes($"{saveDir}/{fileName}")
							});
						}

						// Load the image and save a thumbnail
						using (var image = Image.Load(saveFile.Path))
						{
							var height = 200;
							decimal widthRatio = image.Height / height;
							int width = (int)Math.Round(image.Width / widthRatio, 0);
							var fileName = $"{saveFile.DateTimeTaken.ToString("yyyy-MM-ddTHHmmss")}_thumb.jpg";

							image.Mutate(x => x.Resize(width, height));
							image.Save($"{saveDir}/{fileName}");

							var uploadResponse = client.ApiHour1400UploadPost(new Services.Models.Hour1400UploadRequest
							{
								FileName = fileName,
								Secret = _appSettings.Hour1400UploadSecret,
								Bytes = File.ReadAllBytes($"{saveDir}/{fileName}")
							});
						}
					}
				}
				catch (Exception excep)
				{
					Console.WriteLine(excep.Message);
				}
			}
		}
	}
}