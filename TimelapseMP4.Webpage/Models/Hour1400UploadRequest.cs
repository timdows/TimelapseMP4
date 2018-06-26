using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace TimelapseMP4Webpage.Models
{
	public class Hour1400UploadRequest
	{
		public IFormFile File { get; set; }
		public string FileName { get; set; }
		public string Secret { get; set; }
	}
}
