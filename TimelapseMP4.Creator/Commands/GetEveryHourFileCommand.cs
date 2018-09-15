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
	public class GetEveryHourFileCommand
	{
		private readonly AppSettings _appSettings;

		public GetEveryHourFileCommand(AppSettings appSettings)
		{
			_appSettings = appSettings;
		}
		
		public async Task GetEveryHourFile(string sourceDirectory)
		{
			var saveDir = "Files/EveryHour";
			if (!Directory.Exists(saveDir))
			{
				Directory.CreateDirectory(saveDir);
			}

			var fileGroups = Directory.GetFiles(sourceDirectory, "*.jpg", SearchOption.AllDirectories)
				.Select(item => ImageFileDetails.CreateImageFileDetails(item))
				.Where(item => item != null)
				.GroupBy(item => item.DateTimeTaken.Hour)
				.ToList();

			foreach (var fileGroup in fileGroups)
			{
				var hour = fileGroup.Key;
				var picture = fileGroup.OrderBy(item => item.DateTimeTaken).First();
			}
			
		}
	}
}