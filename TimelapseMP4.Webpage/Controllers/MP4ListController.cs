using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using TimelapseMP4.Webpage.Models;

namespace TimelapseMP4.Webpage.Controllers
{
	[Route("api/[controller]/[action]")]
	public class MP4ListController : Controller
	{
		private readonly WebpageSettings _webpageSettings;

		public MP4ListController(IOptions<WebpageSettings> webpageSettings)
		{
			_webpageSettings = webpageSettings.Value;
		}

		[HttpGet]
		public IActionResult GetList()
		{
			var videos = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), _webpageSettings.MP4Path));
			var mp4Details = new List<MP4Details>();

			foreach (var video in videos)
			{
				var name = Path.GetFileNameWithoutExtension(video);
				mp4Details.Add(new MP4Details
				{
					Name = name,
					Date = DateTime.TryParse(name, out var date) ? date : (DateTime?)null
				});
			}

			return Json(mp4Details);
		}
	}

	internal class MP4Details
	{
		public string Name { get; set; }
		public DateTime? Date { get; set; }

	}
}
