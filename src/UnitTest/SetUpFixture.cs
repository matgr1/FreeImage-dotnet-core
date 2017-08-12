using NUnit.Framework;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace FreeImageNETUnitTest
{
    [SetUpFixture]
    public class SetUpFixture
    {
        [OneTimeSetUp]
        public void Init()
        {
            string dir = Path.GetDirectoryName(typeof(SetUpFixture).Assembly.Location);
            Environment.CurrentDirectory = dir;

            CopyFreeImageNativeDll();
        }

        [OneTimeTearDown]
        public void DeInit()
        {
        }

        private static void CopyFreeImageNativeDll()
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

            if (File.Exists(targetFreeImagePath))
            {
                File.Delete(targetFreeImagePath);
            }

            File.Copy(sourceFreeImagePath, targetFreeImagePath, false);
        }
    }
}
