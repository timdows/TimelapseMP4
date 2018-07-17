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
	public static class InitCommand
	{
		public static void DoInit(AppSettings appSettings)
		{
			if (IsLinux())
			{
				Console.WriteLine("Executing InitCommand.DoInit()");
				var result = string.Empty;

				using (var proc = new Process())
				{
					proc.StartInfo.FileName = "/bin/bash";
					proc.StartInfo.Arguments = "-c \" " + appSettings.MountCommand + " \"";
					proc.StartInfo.UseShellExecute = false;
					proc.StartInfo.RedirectStandardOutput = true;
					proc.StartInfo.RedirectStandardError = true;
					proc.Start();

					result += proc.StandardOutput.ReadToEnd();
					result += proc.StandardError.ReadToEnd();

					proc.WaitForExit();
				}

				Console.WriteLine($"DoInit result: {result}");
			}
		}

		private static bool IsLinux()
		{
			int p = (int)Environment.OSVersion.Platform;
			return p == 4 || p == 6;
		}
	}
}