using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace FreeImageNETUnitTest
{
    [SetUpFixture]
    public class SetUpFixture
    {

        [OneTimeSetUp]
        public void Init()
        {
            string dir = Path.GetDirectoryName(typeof(SetUpFixture).GetTypeInfo().Assembly.Location);
            Directory.SetCurrentDirectory(dir);

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

            const string freeImageLibraryName = "FreeImage";

            string libraryPath = GetPlatformLibraryPath(runtimesFolder, freeImageLibraryName);
            string libraryFileExtension = Path.GetExtension(libraryPath);

            if (false == File.Exists(libraryPath))
            {
                throw new FileNotFoundException(libraryPath);
            }

            string executingFolder = Utility.GetExecutingFolder();
            string targetLibraryPath = Path.Combine(executingFolder, $"{freeImageLibraryName}{libraryFileExtension}");

            if (File.Exists(targetLibraryPath))
            {
                File.Delete(targetLibraryPath);
            }

            File.Copy(libraryPath, targetLibraryPath, false);
        }

        private static string GetPlatformLibraryPath(string runtimesFolder, string libraryName)
        {
            string runtimeFolderName;
            string libraryFileName;

#if NET461
            runtimeFolderName = GetWindowsRuntimeFolder();
            libraryFileName = $"{libraryName}.dll";
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                runtimeFolderName = GetWindowsRuntimeFolder();
                libraryFileName = $"{libraryName}.dll";
            }

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                runtimeFolderName = "osx.10.10-x64";
                libraryFileName = "libfreeimage.3.17.0.dylib";
            }

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                runtimeFolderName = "ubuntu.16.04-x64";
                libraryFileName = "libfreeimage-3.17.0.so";
            }
            else
            {
                throw new Exception($"Unsupported platform");
            }
#endif

            return Path.Combine(runtimesFolder, runtimeFolderName, "native", libraryFileName);
        }

        private static string GetWindowsRuntimeFolder()
        {
            int ptrSize = Marshal.SizeOf(new IntPtr());
            return (ptrSize == 4) ? "win7-x86" : "win7-x64";
        }
    }
}
