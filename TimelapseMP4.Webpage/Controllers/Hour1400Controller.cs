using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TimelapseMP4Webpage.Models;

namespace TimelapseMP4Webpage.Controllers
{
	[Route("api/[controller]/[action]")]
	public class Hour1400Controller : Controller
	{
		const string Hour1400Path = "Hour1400Files";
		private readonly WebpageSettings _webpageSettings;

		public Hour1400Controller(IOptions<WebpageSettings> webpageSettings)
		{
			_webpageSettings = webpageSettings.Value;
		}

		[HttpGet]
		[Produces(typeof(List<Hour1400File>))]
		public IActionResult GetList()
		{
			var images = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), Hour1400Path))
				.Select(item => Hour1400File.CreateImageFileDetails(item))
				.ToList();
			return Json(images);
		}

		[HttpGet]
		[Produces(typeof(List<Hour1400File>))]
		public IActionResult GetThumbnailList()
		{
			var images = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), Hour1400Path))
				.Select(item => Hour1400File.CreateImageFileDetails(item))
				.Where(item => item.FileName.EndsWith("_thumb.jpg"))
				.OrderByDescending(item => item.DateTaken)
				.ToList();
			return Json(images);
		}

		[HttpGet]
		public IActionResult GetImage(string fileName)
		{
			var fullPath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), Hour1400Path), fileName);

			if (System.IO.File.Exists(fullPath))
			{
				return new PhysicalFileResult(fullPath, "image/jpg");
			}

			return NotFound();
		}

		[HttpPost]
		[Produces(typeof(object))]
		public async Task<IActionResult> Upload([FromBody] Hour1400UploadRequest request)
		{
			if (!string.Equals(_webpageSettings.Hour1400UploadSecret, request.Secret))
			{
				return Unauthorized();
			}

			// full path to file in temp location
			var filePath = Path.GetTempFileName();

			if (request.File.Length > 0)
			{
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await request.File.CopyToAsync(stream);
				}
			}

			// process uploaded files
			// Don't rely on or trust the FileName property without validation.

			return Ok(new { request.File.Length, filePath });
		}
	}
}
