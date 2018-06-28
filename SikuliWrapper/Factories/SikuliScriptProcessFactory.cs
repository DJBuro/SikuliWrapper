namespace SikuliWrapper.Factories
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using Microsoft.Win32;
	using SikuliWrapper.Interfaces;

	public class SikuliScriptProcessFactory : ISikuliScriptProcessFactory
	{
		public Process Start(string args)
		{
			string javaPath = GuessJavaPath();
			string sikuliHome = GuessSikuliPath();
			string javaArguments = $"-jar \"{sikuliHome}\" {args}";
			
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
			string solutionFolder = Path.GetFullPath(@"..\..\..\..\");
			foreach (string file in Directory.GetFiles(solutionFolder, "*", SearchOption.AllDirectories))
			{
				
				if (file.Contains("sikulix.jar"))
				{
					return file;
				}
			}

			string sikuliHome = MakeEmptyNull(Environment.GetEnvironmentVariable("SIKULI_HOME"));
			if (sikuliHome != null)
			{
				return sikuliHome;
			}

			throw new FileNotFoundException($"sikulix.jar not found in the solution. If you install sikuli on other place please add SIKULI_HOME environment variable.");
		}

		private static string DetectSikuliPath(string sikuliHome)
		{
			string sikuliScript101JarPath = Path.Combine(sikuliHome, "sikuli-script.jar");
			if (File.Exists(sikuliScript101JarPath))
			{
				return sikuliScript101JarPath;
			}

			string sikuliScript110JarPath = Path.Combine(sikuliHome, "sikulix.jar");
			if (File.Exists(sikuliScript110JarPath))
			{
				return sikuliScript110JarPath;
			}

			throw new FileNotFoundException($"Neither sikuli-script.jar nor sikulix.jar were found in the path referenced in SIKULI_HOME environment variable \"{sikuliHome}\"");
		}

		private static string GuessJavaPath()
		{
			string javaHome = MakeEmptyNull(Environment.GetEnvironmentVariable("JAVA_HOME"))
						   ?? MakeEmptyNull(GetJavaPathFromRegistry(RegistryView.Registry64))
						   ?? MakeEmptyNull(GetJavaPathFromRegistry(RegistryView.Registry32));

			if (String.IsNullOrEmpty(javaHome))
			{
				throw new Exception("Java path not found. Is it installed? If yes, set the JAVA_HOME environment vairable.");
			}

			string javaPath = Path.Combine(javaHome, "bin", "java.exe");

			if (!File.Exists(javaPath))
			{
				throw new Exception($"Java executable not found in expected folder: {javaPath}. If you have multiple Java installations, you may want to set the JAVA_HOME environment variable.");
			}

			return javaPath;
		}

		private static string GetJavaPathFromRegistry(RegistryView view)
		{
			const string jreKey = "SOFTWARE\\JavaSoft\\Java Runtime Environment";
			using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view).OpenSubKey(jreKey))
			{
				if (baseKey == null)
				{
					return null;
				}

				string currentVersion = baseKey.GetValue("CurrentVersion").ToString();
				using (RegistryKey homeKey = baseKey.OpenSubKey(currentVersion))
				{
					if (homeKey != null)
					{
						return homeKey.GetValue("JavaHome").ToString();
					}
				}
			}
			return null;
		}

		private static string MakeEmptyNull(string value)
		{
			return value == "" ? null : value;
		}
	}
}
