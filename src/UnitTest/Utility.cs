using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FreeImageNETUnitTest
{
    public static class Utility
	{
		public static void CopyFreeImageNativeDll()
		{
			string solutionFolder = Utility.GetSolutionFolder();
			string runtimesFolder = Path.Combine(solutionFolder, "runtimes");

			int ptrSize = Marshal.SizeOf(new IntPtr());

			const string freeImageDllName = "FreeImage.dll";

			string runTimeFolderName = (ptrSize == 4) ? "win7-x86" : "win7-x64";
			string sourceFreeImagePath = Path.Combine(runtimesFolder, runTimeFolderName, "native", freeImageDllName);

			if (false == File.Exists(sourceFreeImagePath))
			{
				throw new FileNotFoundException(sourceFreeImagePath);
			}

			string executingFolder = Utility.GetExecutingFolder();
			string targetFreeImagePath = Path.Combine(executingFolder, freeImageDllName);

			if (false == File.Exists(targetFreeImagePath))
			{
				File.Copy(sourceFreeImagePath, targetFreeImagePath, false);
			}
		}

		public static string GetExecutingFolder()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Locati‌​on);
		}

		public static string GetSolutionFolder()
		{
			// TODO: this is pretty hokie...
			string executingFolder = GetExecutingFolder();
			return Path.GetFullPath(Path.Combine(executingFolder, "..", "..", "..", "..", ".."));
		}
	}
}
