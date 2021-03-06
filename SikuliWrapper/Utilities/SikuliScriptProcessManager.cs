﻿namespace SikuliWrapper.Utilities
{
	using System;
	using System.Diagnostics;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
	using Microsoft.Win32;
	using SikuliWrapper.Interfaces;

	[ExcludeFromCodeCoverage]
	public class SikuliScriptProcessManager : ISikuliScriptProcessManager
	{
		public virtual Process Start(string args)
		{
			var javaPath = GuessJavaPath();
			var sikuliHome = GuessSikuliPath();
			var javaArguments = $"-jar \"{sikuliHome}\" {args}";
			
			var process = new Process
			{
				StartInfo =
				{
					FileName = javaPath,
					Arguments = javaArguments,
					CreateNoWindow = true,
					WindowStyle = ProcessWindowStyle.Hidden,
					UseShellExecute = false,
					RedirectStandardInput = true,
					RedirectStandardError = true,
					RedirectStandardOutput = true
				}
			};

			process.Start();

			return process;
		}

		private string GuessSikuliPath()
		{
			var solutionFolder = Path.GetFullPath(@"..\..\..\..\");
			foreach (var file in Directory.GetFiles(solutionFolder, "*", SearchOption.AllDirectories))
			{
				if (file.Contains("sikulix.jar"))
				{
					return file;
				}
			}

			var sikuliHome = MakeEmptyNull(Environment.GetEnvironmentVariable("SIKULI_HOME"));
			if (sikuliHome != null)
			{
				return sikuliHome;
			}

			throw new FileNotFoundException("sikulix.jar not found in the solution. If you install sikuli on other place please add SIKULI_HOME environment variable.");
		}

		private string GuessJavaPath()
		{
			var javaHome = MakeEmptyNull(Environment.GetEnvironmentVariable("JAVA_HOME"))
						   ?? MakeEmptyNull(GetJavaPathFromRegistry(RegistryView.Registry64))
						   ?? MakeEmptyNull(GetJavaPathFromRegistry(RegistryView.Registry32));

			if (string.IsNullOrEmpty(javaHome))
			{
				throw new Exception("Java path not found. Is it installed? If yes, set the JAVA_HOME environment vairable.");
			}

			var javaPath = Path.Combine(javaHome, "bin", "java.exe");

			if (!File.Exists(javaPath))
			{
				throw new Exception($"Java executable not found in expected folder: {javaPath}. If you have multiple Java installations, you may want to set the JAVA_HOME environment variable.");
			}

			return javaPath;
		}

		private string GetJavaPathFromRegistry(RegistryView view)
		{
			const string jreKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment";
			using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view).OpenSubKey(jreKey))
			{
				if (baseKey == null)
				{
					return null;
				}

				var currentVersion = baseKey.GetValue("CurrentVersion").ToString();
				using (var homeKey = baseKey.OpenSubKey(currentVersion))
				{
					if (homeKey != null)
					{
						return homeKey.GetValue("JavaHome").ToString();
					}
				}
			}
			return null;
		}

		private string MakeEmptyNull(string value)
		{
			return value == "" ? null : value;
		}
	}
}
