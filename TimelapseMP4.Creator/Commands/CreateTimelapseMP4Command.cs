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
	public static class CreateTimelapseMP4Command
	{
		public static async Task CreateTimelapseMP4(AppSettings appSettings, string localImageDirectory, string filename)
		{
			if (!Directory.Exists(appSettings.MP4OutputDirectory))
			{
				Directory.CreateDirectory(appSettings.MP4OutputDirectory);
			}

			if (!Directory.Exists(localImageDirectory))
			{
				Console.WriteLine($"LocalImageDirectory {localImageDirectory} does not exist");
				return;
			}

			var savePath = $"{appSettings.MP4OutputDirectory}\\{filename}.mp4";
			if (File.Exists(savePath))
			{
				Console.WriteLine($"Movie already exists for savePath {savePath}");
				return;
			}

			if (!Directory.EnumerateFiles(localImageDirectory, "*.jpg").Any())
			{
				Console.WriteLine($"No files to create movie in directory {localImageDirectory}");
				return;
			}

			var stopwatch = Stopwatch.StartNew();
			var result = string.Empty;
			if (IsLinux())
			{
				string command = $"ffmpeg -framerate 30 -i {localImageDirectory}/image_%04d.jpg -c:v libx264 -r 30 {savePath}";
				Console.WriteLine(command);

				result = $"{command}\r\n";
				using (var proc = new Process())
				{
					proc.StartInfo.FileName = "/bin/bash";
					proc.StartInfo.Arguments = "-c \" " + command + " \"";
					proc.StartInfo.UseShellExecute = false;
					proc.StartInfo.RedirectStandardOutput = true;
					proc.StartInfo.RedirectStandardError = true;
					proc.Start();

					result += proc.StandardOutput.ReadToEnd();
					result += proc.StandardError.ReadToEnd();

					proc.WaitForExit();
				}
			}
			else
			{
				string command = $"\"{appSettings.WindowsFfmpegLocation}\" -framerate 30 -i {localImageDirectory}\\image_%04d.jpg -c:v libx264 -r 30 {savePath}";
				Console.WriteLine(command);
				await File.AppendAllTextAsync($"windowsCommands.log", $"{command}\r\n");

				result = $"{command}\r\n";
				//using (var proc = new Process())
				//{
				//	proc.StartInfo.FileName = "cmd.exe";
				//	proc.StartInfo.Arguments = "/C " + command;
				//	proc.StartInfo.UseShellExecute = false;
				//	proc.StartInfo.RedirectStandardOutput = true;
				//	proc.StartInfo.RedirectStandardError = true;

				//	proc.Start();

				//	result += proc.StandardOutput.ReadToEnd();
				//	result += proc.StandardError.ReadToEnd();

				//	proc.WaitForExit();
				//}
			}

			var elapsedMillis = stopwatch.ElapsedMilliseconds;
			Console.WriteLine($"Finished creating movie in {elapsedMillis} ms");
			result += $"\r\nelapsedMillis: {elapsedMillis}\r\n";
			//await LogCreateOutput(result, filename);
		}

		private static async Task LogCreateOutput(string result, string filename)
		{
			await File.AppendAllTextAsync($"createOutput_{filename}.log", result);
		}

		private static bool IsLinux()
		{
			int p = (int)Environment.OSVersion.Platform;
			return p == 4 || p == 6;
		}
	}
}