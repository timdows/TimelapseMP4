using Microsoft.AspNetCore.Http;

namespace TimelapseMP4.Webpage.Models
{
	public class Hour1400UploadRequest
	{
		public IFormFile File { get; set; }
		public string FileName { get; set; }
		public string Secret { get; set; }
	}
}
