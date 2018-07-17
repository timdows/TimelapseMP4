namespace TimelapseMP4.Creator.Models
{
	public class AppSettings
	{
		public string SourceImageLocation { get; set; }
		public string LocalImageLocation { get; set; }
		public string WindowsFfmpegLocation { get; set; }
		public string MP4OutputDirectory { get; set; }
		public string UnsortedImagesDirectory { get; set; }
		public string Hour1400UploadSecret { get; set; }
		public string ServiceApiLocation { get; set; }
		public string MountCommand { get; set; }
	}
}
