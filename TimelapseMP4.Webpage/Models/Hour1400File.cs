using Newtonsoft.Json;
using System;

namespace TimelapseMP4.Webpage.Models
{
	public class Hour1400File
	{
		[JsonIgnore]
		public string Path { get; set; }
		public string FileName { get; set; }
		public DateTime DateTaken { get; set; }


		public static Hour1400File CreateImageFileDetails(string path)
		{
			var fileName = System.IO.Path.GetFileName(path);

			DateTime dateTaken;

			if (!DateTime.TryParseExact(
				fileName.Replace("_thumb", string.Empty).Replace(".jpg", string.Empty), 
				"yyyy-MM-ddTHHmmss", 
				System.Globalization.CultureInfo.InvariantCulture, 
				System.Globalization.DateTimeStyles.None,
				out dateTaken))
			{
				return null;
			}

			return new Hour1400File
			{
				Path = path,
				FileName = fileName,
				DateTaken = dateTaken
			};
		}
	}
}
