namespace SikuliWrapper
{
    using System;
	using System.Diagnostics;
    using System.IO;
    using SikuliWrapper.Factories;
    using SikuliWrapper.Interfaces;

    public static class Sikuli
    {
	    private static SikuliRuntime CreateRuntime()
	    {
		    return new SikuliRuntime(
				new AsyncDuplexStreamsHandlerFactory(),
			    new SikuliScriptProcessFactory()
		    );
	    }

        public static IScreen CreateSession()
        {
            return new Screen(CreateRuntime());
        }

        public static string RunProject(string projectPath)
        {
            return RunProject(projectPath, null);
        }

        private static string RunProject(string projectPath, string args)
        {
			if (projectPath == null)
			{
				throw new ArgumentNullException(nameof(projectPath));
			}

			if (!Directory.Exists(projectPath))
			{
				throw new DirectoryNotFoundException($"Project not found in path '{projectPath}'");
			}

			var processFactory = new SikuliScriptProcessFactory();
            using (Process process = processFactory.Start($"-r {projectPath} {args}"))
            {
				process.WaitForExit();
				string output = process.StandardOutput.ReadToEnd();
				return output;
			}
        }
    }
}
