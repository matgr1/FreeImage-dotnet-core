using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;
using FreeImageAPI;
using FreeImageAPI.Metadata;
using NUnit.Framework;

namespace FreeImageNETUnitTest.TestFixtures
{
    [TestFixture]
    public class WrapperFunctionsTest
    {
        ImageManager iManager = new ImageManager();
        FIBITMAP dib = new FIBITMAP();        

        [SetUp]
        public void InitEachTime()
        {
        }

        [TearDown]
        public void DeInitEachTime()
        {
        }

        public bool EqualColors(Color color1, Color color2)
        {
            if (color1.A != color2.A) return false;
            if (color1.R != color2.R) return false;
            if (color1.G != color2.G) return false;
            if (color1.B != color2.B) return false;
            return true;
        }

        //
        // Tests
        //

        [Test]
        public void FreeImage_GetWrapperVersion()
        {
            //Assert.That(FreeImage.GetWrapperVersion() ==
            //    String.Format("{0}.{1}.{2}",
            //    FreeImage.FREEIMAGE_MAJOR_VERSION,
            //    FreeImage.FREEIMAGE_MINOR_VERSION,
            //    FreeImage.FREEIMAGE_RELEASE_SERIAL));
        }

        [Test]
        public void FreeImage_IsAvailable()
        {
            Assert.DoesNotThrow(() => FreeImage.ValidateAvailability());
        }

        [Test]
        public void FreeImage_GetBitmap()
        {
            Bitmap bitmap = null;

            try
            {
                bitmap = FreeImage.GetBitmap(new FIBITMAP());
            }
            catch
            {
            }
            Assert.IsNull(bitmap);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
            Assert.That(!dib.IsNull);

            bitmap = FreeImage.GetBitmap(dib);
            Assert.IsNotNull(bitmap);
            Assert.AreEqual((int)FreeImage.GetHeight(dib), bitmap.Height);
            Assert.AreEqual((int)FreeImage.GetWidth(dib), bitmap.Width);

            bitmap.Dispose();
            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_CreateFromBitmap()
        {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(iManager.GetBitmapPath(ImageType.Odd, ImageColorType.Type_24));
            Assert.IsNotNull(bitmap);

            dib = FreeImage.CreateFromBitmap(bitmap);
            Assert.That(!dib.IsNull);

            Assert.AreEqual((int)FreeImage.GetHeight(dib), bitmap.Height);
            Assert.AreEqual((int)FreeImage.GetWidth(dib), bitmap.Width);

            bitmap.Dispose();
            FreeImage.UnloadEx(ref dib);

            try
            {
                dib = FreeImage.CreateFromBitmap(null);
                Assert.Fail();
            }
            catch
            {
            }
        }

        [Test]
        public void FreeImage_SaveBitmap()
        {
            Bitmap bitmap = (Bitmap)Bitmap.FromFile(iManager.GetBitmapPath(ImageType.Odd, ImageColorType.Type_24));
            Assert.IsNotNull(bitmap);

            Assert.IsTrue(FreeImage.SaveBitmap(bitmap, @"test.png", FREE_IMAGE_FORMAT.FIF_PNG, FREE_IMAGE_SAVE_FLAGS.DEFAULT));
            bitmap.Dispose();

            Assert.IsTrue(File.Exists(@"test.png"));

            dib = FreeImage.Load(FREE_IMAGE_FORMAT.FIF_PNG, @"test.png", FREE_IMAGE_LOAD_FLAGS.DEFAULT);
            Assert.That(!dib.IsNull);

            FreeImage.UnloadEx(ref dib);

            File.Delete(@"test.png");
            Assert.IsFalse(File.Exists(@"test.png"));
            bitmap.Dispose();
        }

        [Test]
        public void FreeImage_LoadEx()
        {
            dib = FreeImage.LoadEx(iManager.GetBitmapPath(ImageType.Odd, ImageColorType.Type_16_555));
            Assert.That(!dib.IsNull);
            FreeImage.UnloadEx(ref dib);

            FREE_IMAGE_FORMAT format = FREE_IMAGE_FORMAT.FIF_TIFF;

            dib = FreeImage.LoadEx(iManager.GetBitmapPath(ImageType.Odd, ImageColorType.Type_16_565), ref format);
            Assert.That(dib.IsNull);
            Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_TIFF, format);
            FreeImage.UnloadEx(ref dib);

            format = FREE_IMAGE_FORMAT.FIF_UNKNOWN;
            dib = FreeImage.LoadEx(iManager.GetBitmapPath(ImageType.JPEG, ImageColorType.Type_16_565),
                FREE_IMAGE_LOAD_FLAGS.DEFAULT, ref format);
            Assert.That(!dib.IsNull);
            Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_JPEG, format);

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_UnloadEx()
        {
            Assert.That(dib.IsNull);
            FreeImage.UnloadEx(ref dib);
            Assert.That(dib.IsNull);

            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_16_555);
            Assert.That(!dib.IsNull);

            FreeImage.UnloadEx(ref dib);
            Assert.That(dib.IsNull);
        }

        [Test]
        public void FreeImage_SaveEx()
        {
            FREE_IMAGE_FORMAT format;
            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_08);
            Assert.That(!dib.IsNull);

            Assert.IsTrue(FreeImage.SaveEx(dib, @"test.png"));
            Assert.IsTrue(File.Exists(@"test.png"));
            format = FreeImage.GetFileType(@"test.png", 0);
            Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_PNG, format);
            File.Delete(@"test.png");
            Assert.IsFalse(File.Exists(@"test.png"));

            Assert.IsTrue(FreeImage.SaveEx(ref dib, @"test.tiff", FREE_IMAGE_SAVE_FLAGS.DEFAULT, false));
            Assert.IsTrue(File.Exists(@"test.tiff"));
            format = FreeImage.GetFileType(@"test.tiff", 0);
            Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_TIFF, format);
            File.Delete(@"test.tiff");
            Assert.IsFalse(File.Exists(@"test.tiff"));

            Assert.IsTrue(FreeImage.SaveEx(
                            ref dib,
                            @"test.gif",
                            FREE_IMAGE_FORMAT.FIF_UNKNOWN,
                            FREE_IMAGE_SAVE_FLAGS.DEFAULT,
                            FREE_IMAGE_COLOR_DEPTH.FICD_08_BPP,
                            false));

            Assert.IsTrue(File.Exists(@"test.gif"));
            format = FreeImage.GetFileType(@"test.gif", 0);
            Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_GIF, format);
            File.Delete(@"test.gif");
            Assert.IsFalse(File.Exists(@"test.gif"));

            Assert.IsFalse(FreeImage.SaveEx(dib, @""));
            Assert.IsFalse(FreeImage.SaveEx(dib, @"test.test"));

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_LoadFromStream()
        {
            FREE_IMAGE_FORMAT format;
            FileStream fStream;

            fStream = new FileStream(iManager.GetBitmapPath(ImageType.Odd, ImageColorType.Type_16_565), FileMode.Open);
            Assert.IsNotNull(fStream);

            dib = FreeImage.LoadFromStream(fStream);
            Assert.That(!dib.IsNull);
            Assert.That(FreeImage.GetBPP(dib) == 16);
            Assert.That(FreeImage.GetGreenMask(dib) == FreeImage.FI16_565_GREEN_MASK);

            FreeImage.UnloadEx(ref dib);
            fStream.Close();

            fStream = new FileStream(iManager.GetBitmapPath(ImageType.Metadata, ImageColorType.Type_01_Dither), FileMode.Open);
            Assert.IsNotNull(fStream);

            format = FREE_IMAGE_FORMAT.FIF_UNKNOWN;
            dib = FreeImage.LoadFromStream(fStream, FREE_IMAGE_LOAD_FLAGS.DEFAULT, ref format);
            Assert.That(!dib.IsNull);
            Assert.That(FreeImage.GetBPP(dib) == 24);
            Assert.That(format == FREE_IMAGE_FORMAT.FIF_JPEG);
            FreeImage.UnloadEx(ref dib);
            fStream.Close();

            fStream = new FileStream(iManager.GetBitmapPath(ImageType.Even, ImageColorType.Type_32), FileMode.Open);
            format = FREE_IMAGE_FORMAT.FIF_TIFF;
            dib = FreeImage.LoadFromStream(fStream, FREE_IMAGE_LOAD_FLAGS.DEFAULT, ref format);
            Assert.That(!dib.IsNull);
            Assert.That(FreeImage.GetBPP(dib) == 32);
            Assert.That(format == FREE_IMAGE_FORMAT.FIF_TIFF);

            FreeImage.UnloadEx(ref dib);

            Assert.That(dib.IsNull);
            Assert.Throws<IOException>(() => FreeImage.LoadFromStream(new MemoryStream(new byte[] { })));
            Assert.That(dib.IsNull);

            format = FREE_IMAGE_FORMAT.FIF_BMP;
            fStream.Position = 0;
            dib = FreeImage.LoadFromStream(fStream, ref format);
            Assert.That(dib.IsNull);
            Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_BMP, format);

            fStream.Close();
        }

        [Test]
        public void FreeImage_SaveToStream()
        {
            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_08_Greyscale_MinIsBlack);
            Assert.That(!dib.IsNull);

            Stream stream = new FileStream(@"out_stream.bmp", FileMode.Create);
            Assert.IsNotNull(stream);

            Assert.IsTrue(FreeImage.SaveEx(ref dib, @"out_file.bmp", FREE_IMAGE_FORMAT.FIF_BMP, false));
            Assert.IsTrue(FreeImage.SaveToStream(dib, stream, FREE_IMAGE_FORMAT.FIF_BMP));
            stream.Flush();
            stream.Dispose();

            Assert.IsTrue(File.Exists(@"out_stream.bmp"));
            Assert.IsTrue(File.Exists(@"out_file.bmp"));
            byte[] buffer1 = File.ReadAllBytes(@"out_stream.bmp");
            byte[] buffer2 = File.ReadAllBytes(@"out_file.bmp");
            Assert.AreEqual(buffer1.Length, buffer2.Length);
            for (int i = 0; i < buffer1.Length; i++)
                if (buffer1[i] != buffer2[i])
                    Assert.Fail();

            File.Delete(@"out_stream.bmp");
            File.Delete(@"out_file.bmp");
            Assert.IsFalse(File.Exists(@"out_stream.bmp"));
            Assert.IsFalse(File.Exists(@"out_file.bmp"));

            stream = new MemoryStream();
            Assert.IsFalse(FreeImage.SaveToStream(dib, stream, FREE_IMAGE_FORMAT.FIF_FAXG3));
            stream.Dispose();
            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_IsExtensionValidForFIF()
        {
            Assert.IsTrue(FreeImage.IsExtensionValidForFIF(FREE_IMAGE_FORMAT.FIF_BMP, "bmp", StringComparison.CurrentCultureIgnoreCase));
            Assert.IsTrue(FreeImage.IsExtensionValidForFIF(FREE_IMAGE_FORMAT.FIF_BMP, "BMP", StringComparison.CurrentCultureIgnoreCase));
            Assert.IsFalse(FreeImage.IsExtensionValidForFIF(FREE_IMAGE_FORMAT.FIF_BMP, "DUMMY", StringComparison.CurrentCultureIgnoreCase));
            Assert.IsTrue(FreeImage.IsExtensionValidForFIF(FREE_IMAGE_FORMAT.FIF_PCX, "pcx", StringComparison.CurrentCultureIgnoreCase));
            Assert.IsTrue(FreeImage.IsExtensionValidForFIF(FREE_IMAGE_FORMAT.FIF_TIFF, "tif", StringComparison.CurrentCultureIgnoreCase));
            Assert.IsTrue(FreeImage.IsExtensionValidForFIF(FREE_IMAGE_FORMAT.FIF_TIFF, "TIFF", StringComparison.CurrentCultureIgnoreCase));
            Assert.IsFalse(FreeImage.IsExtensionValidForFIF(FREE_IMAGE_FORMAT.FIF_ICO, "ICO", StringComparison.CurrentCulture));
        }

        [Test]
        public void FreeImage_IsFilenameValidForFIF()
        {
            Assert.IsTrue(FreeImage.IsFilenameValidForFIF(FREE_IMAGE_FORMAT.FIF_JPEG, "file.jpg"));
            Assert.IsTrue(FreeImage.IsFilenameValidForFIF(FREE_IMAGE_FORMAT.FIF_JPEG, "file.jpeg"));
            Assert.IsFalse(FreeImage.IsFilenameValidForFIF(FREE_IMAGE_FORMAT.FIF_JPEG, "file.bmp"));
            Assert.IsTrue(FreeImage.IsFilenameValidForFIF(FREE_IMAGE_FORMAT.FIF_GIF, "file.gif"));
            Assert.IsTrue(FreeImage.IsFilenameValidForFIF(FREE_IMAGE_FORMAT.FIF_GIF, "file.GIF"));
            Assert.IsTrue(FreeImage.IsFilenameValidForFIF(FREE_IMAGE_FORMAT.FIF_GIF, "file.GIF", StringComparison.CurrentCultureIgnoreCase));
            Assert.IsFalse(FreeImage.IsFilenameValidForFIF(FREE_IMAGE_FORMAT.FIF_GIF, "file.txt"));
            Assert.IsFalse(FreeImage.IsFilenameValidForFIF(FREE_IMAGE_FORMAT.FIF_UNKNOWN, "file.jpg"));
            Assert.IsFalse(FreeImage.IsFilenameValidForFIF(FREE_IMAGE_FORMAT.FIF_UNKNOWN, "file.bmp"));
            Assert.IsFalse(FreeImage.IsFilenameValidForFIF(FREE_IMAGE_FORMAT.FIF_UNKNOWN, "file.tif"));
        }

        [Test]
        public void FreeImage_GetPrimaryExtensionFromFIF()
        {
            Assert.AreEqual("gif", FreeImage.GetPrimaryExtensionFromFIF(FREE_IMAGE_FORMAT.FIF_GIF));
            Assert.AreEqual("tif", FreeImage.GetPrimaryExtensionFromFIF(FREE_IMAGE_FORMAT.FIF_TIFF));
            Assert.AreNotEqual("tiff", FreeImage.GetPrimaryExtensionFromFIF(FREE_IMAGE_FORMAT.FIF_TIFF));
            Assert.AreEqual("psd", FreeImage.GetPrimaryExtensionFromFIF(FREE_IMAGE_FORMAT.FIF_PSD));
            Assert.AreEqual("iff", FreeImage.GetPrimaryExtensionFromFIF(FREE_IMAGE_FORMAT.FIF_IFF));
            Assert.IsNull(FreeImage.GetPrimaryExtensionFromFIF(FREE_IMAGE_FORMAT.FIF_UNKNOWN));
        }

        [Test]
        public void FreeImage_OpenMultiBitmapEx()
        {
            FIMULTIBITMAP dib = FreeImage.OpenMultiBitmapEx(iManager.GetBitmapPath(ImageType.Multipaged, ImageColorType.Type_01_Dither));
            Assert.IsFalse(dib.IsNull);
            Assert.AreEqual(4, FreeImage.GetPageCount(dib));
            FreeImage.CloseMultiBitmap(dib, FREE_IMAGE_SAVE_FLAGS.DEFAULT);

            FREE_IMAGE_FORMAT format = FREE_IMAGE_FORMAT.FIF_UNKNOWN;
            dib = FreeImage.OpenMultiBitmapEx(
                iManager.GetBitmapPath(ImageType.Multipaged, ImageColorType.Type_04), ref format, FREE_IMAGE_LOAD_FLAGS.DEFAULT,
                false, true, true);
            Assert.IsFalse(dib.IsNull);
            Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_TIFF, format);
            FreeImage.CloseMultiBitmap(dib, FREE_IMAGE_SAVE_FLAGS.DEFAULT);
        }

        [Test]
        public void FreeImage_GetLockedPageCount()
        {
            FIMULTIBITMAP dib = FreeImage.OpenMultiBitmapEx(iManager.GetBitmapPath(ImageType.Multipaged, ImageColorType.Type_01_Dither));
            FIBITMAP page1, page2, page3;
            Assert.IsFalse(dib.IsNull);
            Assert.AreEqual(4, FreeImage.GetPageCount(dib));
            Assert.AreEqual(0, FreeImage.GetLockedPageCount(dib));

            page1 = FreeImage.LockPage(dib, 0);
            Assert.AreEqual(1, FreeImage.GetLockedPageCount(dib));

            page2 = FreeImage.LockPage(dib, 1);
            Assert.AreEqual(2, FreeImage.GetLockedPageCount(dib));

            page3 = FreeImage.LockPage(dib, 2);
            Assert.AreEqual(3, FreeImage.GetLockedPageCount(dib));

            FreeImage.UnlockPage(dib, page3, true);
            Assert.AreEqual(2, FreeImage.GetLockedPageCount(dib));

            FreeImage.UnlockPage(dib, page2, true);
            Assert.AreEqual(1, FreeImage.GetLockedPageCount(dib));

            FreeImage.UnlockPage(dib, page1, true);
            Assert.AreEqual(0, FreeImage.GetLockedPageCount(dib));

            FreeImage.CloseMultiBitmapEx(ref dib);
        }

        [Test]
        public void FreeImage_GetLockedPages()
        {
            FIMULTIBITMAP dib = FreeImage.OpenMultiBitmapEx(iManager.GetBitmapPath(ImageType.Multipaged, ImageColorType.Type_01_Dither));
            FIBITMAP page1, page2, page3;
            int[] lockedList;
            Assert.IsFalse(dib.IsNull);
            Assert.AreEqual(4, FreeImage.GetPageCount(dib));
            Assert.AreEqual(0, FreeImage.GetLockedPageCount(dib));

            page1 = FreeImage.LockPage(dib, 0);
            Assert.AreEqual(1, FreeImage.GetLockedPageCount(dib));
            lockedList = FreeImage.GetLockedPages(dib);
            Assert.Contains(0, lockedList);

            page2 = FreeImage.LockPage(dib, 1);
            Assert.AreEqual(2, FreeImage.GetLockedPageCount(dib));
            lockedList = FreeImage.GetLockedPages(dib);
            Assert.Contains(0, lockedList);
            Assert.Contains(1, lockedList);

            page3 = FreeImage.LockPage(dib, 3);
            Assert.AreEqual(3, FreeImage.GetLockedPageCount(dib));
            lockedList = FreeImage.GetLockedPages(dib);
            Assert.Contains(0, lockedList);
            Assert.Contains(1, lockedList);
            Assert.Contains(3, lockedList);

            FreeImage.UnlockPage(dib, page2, true);
            Assert.AreEqual(2, FreeImage.GetLockedPageCount(dib));
            lockedList = FreeImage.GetLockedPages(dib);
            Assert.Contains(0, lockedList);
            Assert.Contains(3, lockedList);

            FreeImage.UnlockPage(dib, page1, true);
            Assert.AreEqual(1, FreeImage.GetLockedPageCount(dib));
            lockedList = FreeImage.GetLockedPages(dib);
            Assert.Contains(3, lockedList);

            FreeImage.UnlockPage(dib, page3, true);
            Assert.AreEqual(0, FreeImage.GetLockedPageCount(dib));
            lockedList = FreeImage.GetLockedPages(dib);
            Assert.AreEqual(0, lockedList.Length);

            FreeImage.CloseMultiBitmapEx(ref dib);
        }

        [Test]
        public void FreeImage_GetFileTypeFromStream()
        {
            FileStream fStream = new FileStream(iManager.GetBitmapPath(ImageType.JPEG, ImageColorType.Type_01_Dither), FileMode.Open);
            Assert.IsNotNull(fStream);

            Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_JPEG, FreeImage.GetFileTypeFromStream(fStream));
            fStream.Dispose();

            fStream = new FileStream(iManager.GetBitmapPath(ImageType.Odd, ImageColorType.Type_16_565), FileMode.Open);
            Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_BMP, FreeImage.GetFileTypeFromStream(fStream));
            fStream.Close();
        }

        [Test]
        public void FreeImage_GetHbitmap()
        {
            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_24);
            Assert.IsFalse(dib.IsNull);

            IntPtr hBitmap = FreeImage.GetHbitmap(dib, IntPtr.Zero, false);
            Bitmap bitmap = Bitmap.FromHbitmap(hBitmap);
            Assert.IsNotNull(bitmap);
            Assert.AreEqual(FreeImage.GetWidth(dib), bitmap.Width);
            Assert.AreEqual(FreeImage.GetHeight(dib), bitmap.Height);

            bitmap.Dispose();
            FreeImage.FreeHbitmap(hBitmap);
            FreeImage.UnloadEx(ref dib);

            try
            {
                hBitmap = FreeImage.GetHbitmap(dib, IntPtr.Zero, false);
                Assert.Fail();
            }
            catch
            {
            }
        }

        [Test]
        public void FreeImage_GetResolutionX()
        {
            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_24);
            Assert.IsFalse(dib.IsNull);

            Assert.AreEqual(72, FreeImage.GetResolutionX(dib));

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_GetResolutionY()
        {
            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_24);
            Assert.IsFalse(dib.IsNull);

            Assert.AreEqual(72, FreeImage.GetResolutionY(dib));

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_SetResolutionX()
        {
            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_24);
            Assert.IsFalse(dib.IsNull);

            Assert.AreEqual(72, FreeImage.GetResolutionX(dib));

            FreeImage.SetResolutionX(dib, 12u);
            Assert.AreEqual(12, FreeImage.GetResolutionX(dib));

            FreeImage.SetResolutionX(dib, 1u);
            Assert.AreEqual(1, FreeImage.GetResolutionX(dib));

            FreeImage.SetResolutionX(dib, 66u);
            Assert.AreEqual(66, FreeImage.GetResolutionX(dib));

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_SetResolutionY()
        {
            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_24);
            Assert.IsFalse(dib.IsNull);

            Assert.AreEqual(72, FreeImage.GetResolutionY(dib));

            FreeImage.SetResolutionY(dib, 12u);
            Assert.AreEqual(12, FreeImage.GetResolutionY(dib));

            FreeImage.SetResolutionY(dib, 1u);
            Assert.AreEqual(1, FreeImage.GetResolutionY(dib));

            FreeImage.SetResolutionY(dib, 66u);
            Assert.AreEqual(66, FreeImage.GetResolutionY(dib));

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_IsGreyscaleImage()
        {
            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
            Assert.IsFalse(FreeImage.IsGreyscaleImage(dib));
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_04_Greyscale_Unordered);
            Assert.IsTrue(FreeImage.IsGreyscaleImage(dib));
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_04_Greyscale_MinIsBlack);
            Assert.IsTrue(FreeImage.IsGreyscaleImage(dib));
            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_GetPaletteEx()
        {
            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
            Palette palette = null;

            try
            {
                palette = FreeImage.GetPaletteEx(dib);
                Assert.Fail();
            }
            catch
            {
            }
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_08_Greyscale_MinIsBlack);
            try
            {
                palette = FreeImage.GetPaletteEx(dib);
            }
            catch
            {
                Assert.Fail();
            }
            Assert.AreEqual(256, palette.Length);
            for (int index = 0; index < 256; index++)
            {
                Assert.AreEqual(index, palette[index].rgbRed);
                Assert.AreEqual(index, palette[index].rgbGreen);
                Assert.AreEqual(index, palette[index].rgbBlue);
            }

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_GetInfoHeaderEx()
        {
            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_04);
            Assert.IsFalse(dib.IsNull);

            BITMAPINFOHEADER iHeader = FreeImage.GetInfoHeaderEx(dib);
            Assert.AreEqual(4, iHeader.biBitCount);
            Assert.AreEqual(FreeImage.GetWidth(dib), iHeader.biWidth);
            Assert.AreEqual(FreeImage.GetHeight(dib), iHeader.biHeight);
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_01_Dither);
            Assert.IsFalse(dib.IsNull);

            iHeader = FreeImage.GetInfoHeaderEx(dib);
            Assert.AreEqual(1, iHeader.biBitCount);
            Assert.AreEqual(FreeImage.GetWidth(dib), iHeader.biWidth);
            Assert.AreEqual(FreeImage.GetHeight(dib), iHeader.biHeight);
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
            Assert.IsFalse(dib.IsNull);

            iHeader = FreeImage.GetInfoHeaderEx(dib);
            Assert.AreEqual(24, iHeader.biBitCount);
            Assert.AreEqual(FreeImage.GetWidth(dib), iHeader.biWidth);
            Assert.AreEqual(FreeImage.GetHeight(dib), iHeader.biHeight);
            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_GetInfoEx()
        {
            BITMAPINFO info;

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_01_Dither);
            Assert.That(!dib.IsNull);
            info = FreeImage.GetInfoEx(dib);
            Assert.AreEqual(FreeImage.GetBPP(dib), info.bmiHeader.biBitCount);
            Assert.AreEqual(FreeImage.GetWidth(dib), info.bmiHeader.biWidth);
            Assert.AreEqual(FreeImage.GetHeight(dib), info.bmiHeader.biHeight);
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_04_Greyscale_MinIsBlack);
            Assert.That(!dib.IsNull);
            info = FreeImage.GetInfoEx(dib);
            Assert.AreEqual(FreeImage.GetBPP(dib), info.bmiHeader.biBitCount);
            Assert.AreEqual(FreeImage.GetWidth(dib), info.bmiHeader.biWidth);
            Assert.AreEqual(FreeImage.GetHeight(dib), info.bmiHeader.biHeight);
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_08_Greyscale_Unordered);
            Assert.That(!dib.IsNull);
            info = FreeImage.GetInfoEx(dib);
            Assert.AreEqual(FreeImage.GetBPP(dib), info.bmiHeader.biBitCount);
            Assert.AreEqual(FreeImage.GetWidth(dib), info.bmiHeader.biWidth);
            Assert.AreEqual(FreeImage.GetHeight(dib), info.bmiHeader.biHeight);
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_16_555);
            Assert.That(!dib.IsNull);
            info = FreeImage.GetInfoEx(dib);
            Assert.AreEqual(FreeImage.GetBPP(dib), info.bmiHeader.biBitCount);
            Assert.AreEqual(FreeImage.GetWidth(dib), info.bmiHeader.biWidth);
            Assert.AreEqual(FreeImage.GetHeight(dib), info.bmiHeader.biHeight);
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_16_565);
            Assert.That(!dib.IsNull);
            info = FreeImage.GetInfoEx(dib);
            Assert.AreEqual(FreeImage.GetBPP(dib), info.bmiHeader.biBitCount);
            Assert.AreEqual(FreeImage.GetWidth(dib), info.bmiHeader.biWidth);
            Assert.AreEqual(FreeImage.GetHeight(dib), info.bmiHeader.biHeight);
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
            Assert.That(!dib.IsNull);
            info = FreeImage.GetInfoEx(dib);
            Assert.AreEqual(FreeImage.GetBPP(dib), info.bmiHeader.biBitCount);
            Assert.AreEqual(FreeImage.GetWidth(dib), info.bmiHeader.biWidth);
            Assert.AreEqual(FreeImage.GetHeight(dib), info.bmiHeader.biHeight);
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
            Assert.That(!dib.IsNull);
            info = FreeImage.GetInfoEx(dib);
            Assert.AreEqual(FreeImage.GetBPP(dib), info.bmiHeader.biBitCount);
            Assert.AreEqual(FreeImage.GetWidth(dib), info.bmiHeader.biWidth);
            Assert.AreEqual(FreeImage.GetHeight(dib), info.bmiHeader.biHeight);
            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_GetPixelFormat()
        {
            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_01_Threshold);
            Assert.IsFalse(dib.IsNull);

            Assert.AreEqual(PixelFormat.Format1bppIndexed, FreeImage.GetPixelFormat(dib));
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_04_Greyscale_Unordered);
            Assert.IsFalse(dib.IsNull);

            Assert.AreEqual(PixelFormat.Format4bppIndexed, FreeImage.GetPixelFormat(dib));
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_16_555);
            Assert.IsFalse(dib.IsNull);

            Assert.AreEqual(PixelFormat.Format16bppRgb555, FreeImage.GetPixelFormat(dib));
            FreeImage.UnloadEx(ref dib);

            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_16_565);
            Assert.IsFalse(dib.IsNull);

            Assert.AreEqual(PixelFormat.Format16bppRgb565, FreeImage.GetPixelFormat(dib));
            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_GetFormatParameters()
        {
            uint bpp, red, green, blue;
            FREE_IMAGE_TYPE type;

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format16bppArgb1555, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(16, bpp);
            Assert.AreEqual(red, FreeImage.FI16_555_RED_MASK);
            Assert.AreEqual(green, FreeImage.FI16_555_GREEN_MASK);
            Assert.AreEqual(blue, FreeImage.FI16_555_BLUE_MASK);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_BITMAP);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format16bppGrayScale, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(16, bpp);
            Assert.AreEqual(red, 0);
            Assert.AreEqual(green, 0);
            Assert.AreEqual(blue, 0);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_UINT16);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format16bppRgb555, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(16, bpp);
            Assert.AreEqual(red, FreeImage.FI16_555_RED_MASK);
            Assert.AreEqual(green, FreeImage.FI16_555_GREEN_MASK);
            Assert.AreEqual(blue, FreeImage.FI16_555_BLUE_MASK);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_BITMAP);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format16bppRgb565, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(16, bpp);
            Assert.AreEqual(red, FreeImage.FI16_565_RED_MASK);
            Assert.AreEqual(green, FreeImage.FI16_565_GREEN_MASK);
            Assert.AreEqual(blue, FreeImage.FI16_565_BLUE_MASK);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_BITMAP);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format1bppIndexed, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(1, bpp);
            Assert.AreEqual(red, 0);
            Assert.AreEqual(green, 0);
            Assert.AreEqual(blue, 0);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_BITMAP);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format24bppRgb, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(24, bpp);
            Assert.AreEqual(red, FreeImage.FI_RGBA_RED_MASK);
            Assert.AreEqual(green, FreeImage.FI_RGBA_GREEN_MASK);
            Assert.AreEqual(blue, FreeImage.FI_RGBA_BLUE_MASK);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_BITMAP);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format32bppArgb, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(32, bpp);
            Assert.AreEqual(red, FreeImage.FI_RGBA_RED_MASK);
            Assert.AreEqual(green, FreeImage.FI_RGBA_GREEN_MASK);
            Assert.AreEqual(blue, FreeImage.FI_RGBA_BLUE_MASK);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_BITMAP);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format32bppPArgb, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(32, bpp);
            Assert.AreEqual(red, FreeImage.FI_RGBA_RED_MASK);
            Assert.AreEqual(green, FreeImage.FI_RGBA_GREEN_MASK);
            Assert.AreEqual(blue, FreeImage.FI_RGBA_BLUE_MASK);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_BITMAP);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format32bppRgb, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(32, bpp);
            Assert.AreEqual(red, FreeImage.FI_RGBA_RED_MASK);
            Assert.AreEqual(green, FreeImage.FI_RGBA_GREEN_MASK);
            Assert.AreEqual(blue, FreeImage.FI_RGBA_BLUE_MASK);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_BITMAP);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format48bppRgb, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(48, bpp);
            Assert.AreEqual(red, 0);
            Assert.AreEqual(green, 0);
            Assert.AreEqual(blue, 0);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_RGB16);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format4bppIndexed, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(4, bpp);
            Assert.AreEqual(red, 0);
            Assert.AreEqual(green, 0);
            Assert.AreEqual(blue, 0);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_BITMAP);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format64bppArgb, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(64, bpp);
            Assert.AreEqual(red, 0);
            Assert.AreEqual(green, 0);
            Assert.AreEqual(blue, 0);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_RGBA16);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format64bppPArgb, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(64, bpp);
            Assert.AreEqual(red, 0);
            Assert.AreEqual(green, 0);
            Assert.AreEqual(blue, 0);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_RGBA16);

            Assert.IsTrue(FreeImage.GetFormatParameters(PixelFormat.Format8bppIndexed, out type, out bpp, out red, out green, out blue));
            Assert.AreEqual(8, bpp);
            Assert.AreEqual(red, 0);
            Assert.AreEqual(green, 0);
            Assert.AreEqual(blue, 0);
            Assert.AreEqual(type, FREE_IMAGE_TYPE.FIT_BITMAP);
        }

        [Test]
        public void FreeImage_Compare()
        {
            FIBITMAP dib2;

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_01_Dither);
            Assert.IsFalse(dib.IsNull);
            dib2 = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_01_Threshold);
            Assert.IsFalse(dib2.IsNull);

            Assert.IsFalse(FreeImage.Compare(dib, dib2, FREE_IMAGE_COMPARE_FLAGS.COMPLETE));
            Assert.IsTrue(FreeImage.Compare(dib, dib2, FREE_IMAGE_COMPARE_FLAGS.HEADER));

            FreeImage.UnloadEx(ref dib);
            FreeImage.UnloadEx(ref dib2);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_04_Greyscale_MinIsBlack);
            dib2 = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_04_Greyscale_Unordered);

            Assert.IsFalse(FreeImage.Compare(dib, dib2, FREE_IMAGE_COMPARE_FLAGS.COMPLETE));

            FreeImage.UnloadEx(ref dib);
            FreeImage.UnloadEx(ref dib2);
            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
            dib2 = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
            Assert.IsTrue(FreeImage.Compare(dib, dib2, FREE_IMAGE_COMPARE_FLAGS.COMPLETE));

            FreeImage.UnloadEx(ref dib);
            FreeImage.UnloadEx(ref dib2);

            dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
            Assert.IsFalse(dib.IsNull);
            dib2 = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
            Assert.IsFalse(dib2.IsNull);

            Assert.IsTrue(FreeImage.Compare(dib, dib2, FREE_IMAGE_COMPARE_FLAGS.COMPLETE));

            FreeImage.UnloadEx(ref dib);
            FreeImage.UnloadEx(ref dib2);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_16_555);
            Assert.IsFalse(dib.IsNull);
            dib2 = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_16_555);
            Assert.IsFalse(dib2.IsNull);

            Assert.IsTrue(FreeImage.Compare(dib, dib2, FREE_IMAGE_COMPARE_FLAGS.COMPLETE));

            FreeImage.UnloadEx(ref dib);
            FreeImage.UnloadEx(ref dib2);

            dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_16_565);
            Assert.IsFalse(dib.IsNull);
            dib2 = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_16_565);
            Assert.IsFalse(dib2.IsNull);

            Assert.IsTrue(FreeImage.Compare(dib, dib2, FREE_IMAGE_COMPARE_FLAGS.COMPLETE));

            FreeImage.UnloadEx(ref dib);
            FreeImage.UnloadEx(ref dib2);
        }

        [Test]
        public void FreeImage_CreateICCProfileEx()
        {
            FIICCPROFILE prof;
            byte[] data = new byte[173];
            Random rand = new Random(DateTime.Now.Millisecond);
            dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_BITMAP, 1, 1, 1, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);

            prof = FreeImage.GetICCProfileEx(dib);
            Assert.That(prof.DataPointer == IntPtr.Zero);

            rand.NextBytes(data);
            prof = FreeImage.CreateICCProfileEx(dib, data);
            Assert.That(prof.Size == data.Length);
            for (int i = 0; i < data.Length; i++)
                if (prof.Data[i] != data[i])
                    Assert.Fail();

            FreeImage.DestroyICCProfile(dib);
            prof = FreeImage.GetICCProfileEx(dib);
            Assert.That(prof.DataPointer == IntPtr.Zero);

            FreeImage.CreateICCProfileEx(dib, new byte[0], 0);
            prof = FreeImage.GetICCProfileEx(dib);
            Assert.That(prof.DataPointer == IntPtr.Zero);

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_ConvertColorDepth()
        {
            int bpp = 1;
            FIBITMAP dib2;

            dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_BITMAP, 80, 80, 1, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);

            do
            {
                dib2 = FreeImage.ConvertColorDepth(dib, (FREE_IMAGE_COLOR_DEPTH)bpp);
                Assert.IsFalse(dib2.IsNull);
                Assert.AreEqual(bpp, FreeImage.GetBPP(dib2));
                if (dib != dib2)
                    FreeImage.UnloadEx(ref dib2);
            } while (0 != (bpp = FreeImage.GetNextColorDepth(bpp)));

            FreeImage.UnloadEx(ref dib);

            dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_BITMAP, 80, 80, 32,
                FreeImage.FI_RGBA_RED_MASK, FreeImage.FI_RGBA_GREEN_MASK, FreeImage.FI_RGBA_BLUE_MASK);
            Assert.IsFalse(dib.IsNull);
            bpp = 32;

            do
            {
                dib2 = FreeImage.ConvertColorDepth(dib, (FREE_IMAGE_COLOR_DEPTH)bpp);
                Assert.IsFalse(dib2.IsNull);
                Assert.AreEqual(bpp, FreeImage.GetBPP(dib2));
                if (dib != dib2)
                    FreeImage.UnloadEx(ref dib2);
            } while (0 != (bpp = FreeImage.GetPrevousColorDepth(bpp)));

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_GetNextColorDepth()
        {
            Assert.AreEqual(0, FreeImage.GetNextColorDepth(5));
            Assert.AreEqual(0, FreeImage.GetNextColorDepth(0));
            Assert.AreEqual(4, FreeImage.GetNextColorDepth(1));
            Assert.AreEqual(8, FreeImage.GetNextColorDepth(4));
            Assert.AreEqual(16, FreeImage.GetNextColorDepth(8));
            Assert.AreEqual(24, FreeImage.GetNextColorDepth(16));
            Assert.AreEqual(32, FreeImage.GetNextColorDepth(24));
            Assert.AreEqual(0, FreeImage.GetNextColorDepth(32));
        }

        [Test]
        public void FreeImage_GetPrevousColorDepth()
        {
            Assert.AreEqual(0, FreeImage.GetNextColorDepth(5));
            Assert.AreEqual(0, FreeImage.GetNextColorDepth(0));
            Assert.AreEqual(4, FreeImage.GetNextColorDepth(1));
            Assert.AreEqual(8, FreeImage.GetNextColorDepth(4));
            Assert.AreEqual(16, FreeImage.GetNextColorDepth(8));
            Assert.AreEqual(24, FreeImage.GetNextColorDepth(16));
            Assert.AreEqual(32, FreeImage.GetNextColorDepth(24));
            Assert.AreEqual(0, FreeImage.GetNextColorDepth(32));
        }

        [Test]
        public unsafe void FreeImage_PtrToStr()
        {
            string testString;
            string resString;
            IntPtr buffer;
            int index;

            testString = "Test string";
            buffer = Marshal.AllocHGlobal(testString.Length + 1);

            for (index = 0; index < testString.Length; index++)
            {
                Marshal.WriteByte(buffer, index, (byte)testString[index]);
            }
            Marshal.WriteByte(buffer, index, (byte)0);

            resString = FreeImage.PtrToStr((byte*)buffer);
            Assert.That(resString == testString);

            Marshal.FreeHGlobal(buffer);

            testString = @"äöü?=§%/!)§(%&)(§";
            buffer = Marshal.AllocHGlobal(testString.Length + 1);

            for (index = 0; index < testString.Length; index++)
            {
                Marshal.WriteByte(buffer, index, (byte)testString[index]);
            }
            Marshal.WriteByte(buffer, index, (byte)0);

            resString = FreeImage.PtrToStr((byte*)buffer);
            Assert.That(resString == testString);

            Marshal.FreeHGlobal(buffer);
        }

        [Test]
        public void FreeImage_CopyMetadata()
        {
            dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
            Assert.IsFalse(dib.IsNull);
            int mdCount = 0;

            FIBITMAP dib2 = FreeImage.Allocate(1, 1, 1, 0, 0, 0);
            Assert.IsFalse(dib2.IsNull);

            FREE_IMAGE_MDMODEL[] modelList = (FREE_IMAGE_MDMODEL[])Enum.GetValues(typeof(FREE_IMAGE_MDMODEL));
            FITAG tag, tag2;
            FIMETADATA mdHandle;

            foreach (FREE_IMAGE_MDMODEL model in modelList)
            {
                mdHandle = FreeImage.FindFirstMetadata(model, dib2, out tag);
                Assert.IsTrue(mdHandle.IsNull);
                mdCount += (int)FreeImage.GetMetadataCount(model, dib);
            }

            Assert.AreEqual(mdCount, FreeImage.CloneMetadataEx(dib, dib2, FREE_IMAGE_METADATA_COPY.CLEAR_EXISTING));

            foreach (FREE_IMAGE_MDMODEL model in modelList)
            {
                mdHandle = FreeImage.FindFirstMetadata(model, dib, out tag);
                if (!mdHandle.IsNull)
                {
                    do
                    {
                        Assert.IsTrue(FreeImage.GetMetadata(model, dib2, FreeImage.GetTagKey(tag), out tag2));
                        Assert.That(FreeImage.GetTagCount(tag) == FreeImage.GetTagCount(tag2));
                        Assert.That(FreeImage.GetTagDescription(tag) == FreeImage.GetTagDescription(tag2));
                        Assert.That(FreeImage.GetTagID(tag) == FreeImage.GetTagID(tag2));
                        Assert.That(FreeImage.GetTagKey(tag) == FreeImage.GetTagKey(tag2));
                        Assert.That(FreeImage.GetTagLength(tag) == FreeImage.GetTagLength(tag2));
                        Assert.That(FreeImage.GetTagType(tag) == FreeImage.GetTagType(tag2));
                    }
                    while (FreeImage.FindNextMetadata(mdHandle, out tag));
                    FreeImage.FindCloseMetadata(mdHandle);
                }
            }

            Assert.AreEqual(0, FreeImage.CloneMetadataEx(dib, dib2, FREE_IMAGE_METADATA_COPY.KEEP_EXISITNG));

            foreach (FREE_IMAGE_MDMODEL model in modelList)
            {
                mdHandle = FreeImage.FindFirstMetadata(model, dib, out tag);
                if (!mdHandle.IsNull)
                {
                    do
                    {
                        Assert.IsTrue(FreeImage.GetMetadata(model, dib2, FreeImage.GetTagKey(tag), out tag2));
                        Assert.AreEqual(FreeImage.GetTagCount(tag), FreeImage.GetTagCount(tag2));
                        Assert.AreEqual(FreeImage.GetTagDescription(tag), FreeImage.GetTagDescription(tag2));
                        Assert.AreEqual(FreeImage.GetTagID(tag), FreeImage.GetTagID(tag2));
                        Assert.AreEqual(FreeImage.GetTagKey(tag), FreeImage.GetTagKey(tag2));
                        Assert.AreEqual(FreeImage.GetTagLength(tag), FreeImage.GetTagLength(tag2));
                        Assert.AreEqual(FreeImage.GetTagType(tag), FreeImage.GetTagType(tag2));
                    }
                    while (FreeImage.FindNextMetadata(mdHandle, out tag));
                    FreeImage.FindCloseMetadata(mdHandle);
                }
            }

            const string newTagDescription = "NEW_TAG_DESCRIPTION";

            Assert.IsTrue(FreeImage.GetMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_MAIN, dib, "Copyright", out tag));
            Assert.IsTrue(FreeImage.SetTagDescription(tag, newTagDescription));
            Assert.AreEqual(mdCount, FreeImage.CloneMetadataEx(dib, dib2, FREE_IMAGE_METADATA_COPY.REPLACE_EXISTING));
            Assert.IsTrue(FreeImage.GetMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_MAIN, dib2, "Copyright", out tag2));
            Assert.AreEqual(newTagDescription, FreeImage.GetTagDescription(tag2));

            FreeImage.UnloadEx(ref dib2);
            FreeImage.UnloadEx(ref dib);

            dib2 = FreeImage.Allocate(1, 1, 1, 0, 0, 0);
            dib = FreeImage.Allocate(1, 1, 1, 0, 0, 0);

            Assert.AreEqual(0, FreeImage.CloneMetadataEx(dib, dib2, FREE_IMAGE_METADATA_COPY.CLEAR_EXISTING));

            FreeImage.UnloadEx(ref dib2);
            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_GetImageComment()
        {
            dib = FreeImage.Allocate(1, 1, 1, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);
            const string comment = "C O M M E N T";

            Assert.IsNull(FreeImage.GetImageComment(dib));
            Assert.IsTrue(FreeImage.SetImageComment(dib, comment));
            Assert.AreEqual(comment, FreeImage.GetImageComment(dib));
            Assert.IsTrue(FreeImage.SetImageComment(dib, null));
            Assert.IsNull(FreeImage.GetImageComment(dib));
            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_SetImageComment()
        {
            dib = FreeImage.Allocate(1, 1, 1, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);
            const string comment = "C O M M E N T";

            Assert.IsNull(FreeImage.GetImageComment(dib));
            Assert.IsTrue(FreeImage.SetImageComment(dib, comment));
            Assert.AreEqual(comment, FreeImage.GetImageComment(dib));
            Assert.IsTrue(FreeImage.SetImageComment(dib, null));
            Assert.IsNull(FreeImage.GetImageComment(dib));
            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_GetMetadata()
        {
            MetadataTag tag;

            dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
            Assert.IsFalse(dib.IsNull);

            Assert.IsFalse(FreeImage.GetMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_MAIN, dib, "~~~~~", out tag));
            Assert.IsNull(tag);

            Assert.IsTrue(FreeImage.GetMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_MAIN, dib, "Artist", out tag));
            Assert.IsFalse(tag.tag.IsNull);

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_SetMetadata()
        {
            MetadataTag tag;
            Random rand = new Random();

            dib = FreeImage.Allocate(1, 1, 1, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);

            ushort value = (ushort)rand.Next();

            tag = new MetadataTag(FREE_IMAGE_MDMODEL.FIMD_CUSTOM);
            tag.ID = value;

            Assert.IsTrue(FreeImage.SetMetadata(FREE_IMAGE_MDMODEL.FIMD_CUSTOM, dib, "~~~~~", tag));
            Assert.IsTrue(FreeImage.GetMetadata(FREE_IMAGE_MDMODEL.FIMD_CUSTOM, dib, "~~~~~", out tag));
            Assert.AreEqual(value, tag.ID);

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_FindFirstMetadata()
        {
            MetadataTag tag;
            FIMETADATA mdHandle;
            dib = FreeImage.Allocate(1, 1, 1, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);

            FREE_IMAGE_MDMODEL[] models = (FREE_IMAGE_MDMODEL[])Enum.GetValues(typeof(FREE_IMAGE_MDMODEL));
            foreach (FREE_IMAGE_MDMODEL model in models)
            {
                mdHandle = FreeImage.FindFirstMetadata(model, dib, out tag);
                Assert.IsTrue(mdHandle.IsNull);
            }

            tag = new MetadataTag(FREE_IMAGE_MDMODEL.FIMD_COMMENTS);
            tag.Key = "KEY";
            tag.Value = 12345;
            tag.AddToImage(dib);

            mdHandle = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_COMMENTS, dib, out tag);
            Assert.IsFalse(mdHandle.IsNull);

            FreeImage.FindCloseMetadata(mdHandle);
            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_FindNextMetadata()
        {
            MetadataTag tag;
            FIMETADATA mdHandle;
            dib = FreeImage.Allocate(1, 1, 1, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);

            FREE_IMAGE_MDMODEL[] models = (FREE_IMAGE_MDMODEL[])Enum.GetValues(typeof(FREE_IMAGE_MDMODEL));
            foreach (FREE_IMAGE_MDMODEL model in models)
            {
                mdHandle = FreeImage.FindFirstMetadata(model, dib, out tag);
                Assert.IsTrue(mdHandle.IsNull);
            }

            tag = new MetadataTag(FREE_IMAGE_MDMODEL.FIMD_COMMENTS);
            tag.Key = "KEY1";
            tag.Value = 12345;
            tag.AddToImage(dib);

            tag = new MetadataTag(FREE_IMAGE_MDMODEL.FIMD_COMMENTS);
            tag.Key = "KEY2";
            tag.Value = 54321;
            tag.AddToImage(dib);

            mdHandle = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_COMMENTS, dib, out tag);
            Assert.IsFalse(mdHandle.IsNull);

            Assert.IsTrue(FreeImage.FindNextMetadata(mdHandle, out tag));
            Assert.IsFalse(FreeImage.FindNextMetadata(mdHandle, out tag));

            FreeImage.FindCloseMetadata(mdHandle);
            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_SetGetTransparencyTableEx()
        {
            dib = FreeImage.Allocate(10, 10, 6, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);

            byte[] transTable = FreeImage.GetTransparencyTableEx(dib);
            Assert.That(transTable.Length == 0);

            Random rand = new Random();
            transTable = new byte[rand.Next(0, 255)];
            int length = transTable.Length;

            for (int i = 0; i < transTable.Length; i++)
                transTable[i] = (byte)i;

            FreeImage.SetTransparencyTable(dib, transTable);
            transTable = null;
            transTable = FreeImage.GetTransparencyTableEx(dib);
            Assert.That(transTable.Length == length);
            for (int i = 0; i < transTable.Length; i++)
                Assert.That(transTable[i] == i);

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_GetUniqueColors()
        {
            Palette palette;

            //
            // 1bpp
            //

            dib = FreeImage.Allocate(10, 1, 1, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);

            palette = new Palette(dib);
            palette[0] = Color.FromArgb(43, 255, 255, 255);
            palette[1] = Color.FromArgb(222, 0, 0, 0);

            Scanline<FI1BIT> sl1bit = new Scanline<FI1BIT>(dib, 0);
            for (int x = 0; x < sl1bit.Length; x++)
            {
                sl1bit[x] = 0;
            }

            Assert.AreEqual(1, FreeImage.GetUniqueColors(dib));

            sl1bit[5] = 1;
            Assert.AreEqual(2, FreeImage.GetUniqueColors(dib));

            palette[1] = Color.FromArgb(222, 255, 255, 255);
            Assert.AreEqual(1, FreeImage.GetUniqueColors(dib));

            sl1bit[5] = 0;
            Assert.AreEqual(1, FreeImage.GetUniqueColors(dib));

            FreeImage.UnloadEx(ref dib);

            //
            // 4bpp
            //

            dib = FreeImage.Allocate(10, 1, 4, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);

            palette = new Palette(dib);
            palette[0] = new RGBQUAD(Color.FromArgb(43, 255, 255, 255));
            palette[1] = new RGBQUAD(Color.FromArgb(222, 51, 2, 211));
            palette[2] = new RGBQUAD(Color.FromArgb(29, 25, 31, 52));
            palette[3] = new RGBQUAD(Color.FromArgb(173, 142, 61, 178));
            palette[4] = new RGBQUAD(Color.FromArgb(143, 41, 67, 199));
            palette[5] = new RGBQUAD(Color.FromArgb(2, 0, 2, 221));

            Scanline<FI4BIT> sl4bit = new Scanline<FI4BIT>(dib, 0);

            for (int x = 0; x < sl4bit.Length; x++)
            {
                sl4bit[x] = 0;
            }

            Assert.AreEqual(1, FreeImage.GetUniqueColors(dib));

            sl4bit[1] = 1;
            Assert.AreEqual(2, FreeImage.GetUniqueColors(dib));

            sl4bit[2] = 1;
            Assert.AreEqual(2, FreeImage.GetUniqueColors(dib));

            sl4bit[3] = 2;
            Assert.AreEqual(3, FreeImage.GetUniqueColors(dib));

            sl4bit[4] = 3;
            Assert.AreEqual(4, FreeImage.GetUniqueColors(dib));

            sl4bit[5] = 4;
            Assert.AreEqual(5, FreeImage.GetUniqueColors(dib));

            sl4bit[6] = 5;
            Assert.AreEqual(6, FreeImage.GetUniqueColors(dib));

            sl4bit[7] = 6;
            Assert.AreEqual(7, FreeImage.GetUniqueColors(dib));

            sl4bit[8] = 7;
            Assert.AreEqual(7, FreeImage.GetUniqueColors(dib));

            sl4bit[9] = 7;
            Assert.AreEqual(7, FreeImage.GetUniqueColors(dib));

            FreeImage.UnloadEx(ref dib);

            //
            // 8bpp
            //

            dib = FreeImage.Allocate(10, 1, 8, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);

            palette = new Palette(dib);
            palette[0] = new RGBQUAD(Color.FromArgb(43, 255, 255, 255));
            palette[1] = new RGBQUAD(Color.FromArgb(222, 51, 2, 211));
            palette[2] = new RGBQUAD(Color.FromArgb(29, 25, 31, 52));
            palette[3] = new RGBQUAD(Color.FromArgb(173, 142, 61, 178));
            palette[4] = new RGBQUAD(Color.FromArgb(143, 41, 67, 199));
            palette[5] = new RGBQUAD(Color.FromArgb(2, 0, 2, 221));

            Scanline<byte> sl8bit = new Scanline<byte>(dib, 0);

            for (int x = 0; x < sl8bit.Length; x++)
            {
                sl8bit[x] = 0;
            }

            Assert.AreEqual(1, FreeImage.GetUniqueColors(dib));

            sl8bit[1] = 1;
            Assert.AreEqual(2, FreeImage.GetUniqueColors(dib));

            sl8bit[2] = 2;
            Assert.AreEqual(3, FreeImage.GetUniqueColors(dib));

            sl8bit[3] = 3;
            Assert.AreEqual(4, FreeImage.GetUniqueColors(dib));

            sl8bit[4] = 4;
            Assert.AreEqual(5, FreeImage.GetUniqueColors(dib));

            sl8bit[5] = 6;
            Assert.AreEqual(6, FreeImage.GetUniqueColors(dib));

            sl8bit[5] = 7;
            Assert.AreEqual(6, FreeImage.GetUniqueColors(dib));

            sl8bit[5] = 8;
            Assert.AreEqual(6, FreeImage.GetUniqueColors(dib));

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_CreateShrunkenPaletteLUT()
        {
            Random rand = new Random();
            dib = FreeImage.Allocate(1, 1, 8, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);

            Palette palette = new Palette(dib);
            byte[] lut;
            int colors;

            for (int x = 0; x < palette.Length; x++)
            {
                palette[x] = 0xFF000000;
            }

            lut = FreeImage.CreateShrunkenPaletteLUT(dib, out colors);
            Assert.AreEqual(1, colors);

            for (int x = 0; x < palette.Length; x++)
            {
                Assert.AreEqual(0, lut[x]);
            }

            palette[1] = 0x00000001;
            lut = FreeImage.CreateShrunkenPaletteLUT(dib, out colors);
            Assert.AreEqual(2, colors);

            Assert.AreEqual(0, lut[0]);
            Assert.AreEqual(1, lut[1]);

            for (int x = 2; x < palette.Length; x++)
            {
                Assert.AreEqual(0, lut[x]);
            }

            for (int x = 0; x < palette.Length; x++)
            {
                palette[x] = (uint)x;
            }

            lut = FreeImage.CreateShrunkenPaletteLUT(dib, out colors);
            Assert.AreEqual(256, colors);

            for (int x = 0; x < palette.Length; x++)
            {
                Assert.AreEqual(x, lut[x]);
            }

            uint[] testColors = new uint[] { 0xFF4F387C, 0xFF749178, 0xFF84D51A, 0xFF746B71, 0x74718163, 0x91648106 };
            palette[0] = testColors[0];
            palette[1] = testColors[1];
            palette[2] = testColors[2];
            palette[3] = testColors[3];
            palette[4] = testColors[4];
            palette[5] = testColors[5];

            for (int x = testColors.Length; x < palette.Length; x++)
            {
                palette[x] = testColors[rand.Next(0, testColors.Length - 1)];
            }

            lut = FreeImage.CreateShrunkenPaletteLUT(dib, out colors);
            Assert.AreEqual(testColors.Length, colors);

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void FreeImage_Rotate4bit()
        {
            Palette orgPal, rotPal;
            FIBITMAP rotated;
            byte index;
            dib = FreeImage.Allocate(2, 3, 4, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);

            index = 1; if (!FreeImage.SetPixelIndex(dib, 0, 0, ref index)) throw new Exception();
            index = 2; if (!FreeImage.SetPixelIndex(dib, 1, 0, ref index)) throw new Exception();
            index = 3; if (!FreeImage.SetPixelIndex(dib, 0, 1, ref index)) throw new Exception();
            index = 4; if (!FreeImage.SetPixelIndex(dib, 1, 1, ref index)) throw new Exception();
            index = 5; if (!FreeImage.SetPixelIndex(dib, 0, 2, ref index)) throw new Exception();
            index = 6; if (!FreeImage.SetPixelIndex(dib, 1, 2, ref index)) throw new Exception();

            //
            // 90 deg
            //

            rotated = FreeImage.Rotate4bit(dib, 90d);
            Assert.IsFalse(rotated.IsNull);
            Assert.AreEqual(3, FreeImage.GetWidth(rotated));
            Assert.AreEqual(2, FreeImage.GetHeight(rotated));
            Assert.AreEqual(FREE_IMAGE_TYPE.FIT_BITMAP, FreeImage.GetImageType(rotated));
            Assert.AreEqual(4, FreeImage.GetBPP(rotated));
            orgPal = new Palette(dib);
            rotPal = new Palette(rotated);
            Assert.IsNotNull(orgPal);
            Assert.IsNotNull(rotPal);
            Assert.AreEqual(orgPal.Length, rotPal.Length);
            for (int i = 0; i < orgPal.Length; i++)
            {
                Assert.AreEqual(orgPal[i], rotPal[i]);
            }

            FreeImage.GetPixelIndex(rotated, 0, 0, out index);
            Assert.AreEqual(5, index);
            FreeImage.GetPixelIndex(rotated, 1, 0, out index);
            Assert.AreEqual(3, index);
            FreeImage.GetPixelIndex(rotated, 2, 0, out index);
            Assert.AreEqual(1, index);
            FreeImage.GetPixelIndex(rotated, 0, 1, out index);
            Assert.AreEqual(6, index);
            FreeImage.GetPixelIndex(rotated, 1, 1, out index);
            Assert.AreEqual(4, index);
            FreeImage.GetPixelIndex(rotated, 2, 1, out index);
            Assert.AreEqual(2, index);
            FreeImage.UnloadEx(ref rotated);

            //
            // 180 deg
            //

            rotated = FreeImage.Rotate4bit(dib, 180d);
            Assert.IsFalse(rotated.IsNull);
            Assert.AreEqual(FreeImage.GetWidth(dib), FreeImage.GetWidth(rotated));
            Assert.AreEqual(FreeImage.GetHeight(dib), FreeImage.GetHeight(rotated));
            Assert.AreEqual(FREE_IMAGE_TYPE.FIT_BITMAP, FreeImage.GetImageType(rotated));
            Assert.AreEqual(4, FreeImage.GetBPP(rotated));
            orgPal = new Palette(dib);
            rotPal = new Palette(rotated);
            Assert.IsNotNull(orgPal);
            Assert.IsNotNull(rotPal);
            Assert.AreEqual(orgPal.Length, rotPal.Length);
            for (int i = 0; i < orgPal.Length; i++)
            {
                Assert.AreEqual(orgPal[i], rotPal[i]);
            }

            FreeImage.GetPixelIndex(rotated, 0, 0, out index);
            Assert.AreEqual(6, index);
            FreeImage.GetPixelIndex(rotated, 1, 0, out index);
            Assert.AreEqual(5, index);
            FreeImage.GetPixelIndex(rotated, 0, 1, out index);
            Assert.AreEqual(4, index);
            FreeImage.GetPixelIndex(rotated, 1, 1, out index);
            Assert.AreEqual(3, index);
            FreeImage.GetPixelIndex(rotated, 0, 2, out index);
            Assert.AreEqual(2, index);
            FreeImage.GetPixelIndex(rotated, 1, 2, out index);
            Assert.AreEqual(1, index);
            FreeImage.UnloadEx(ref rotated);

            //
            // 270 deg
            //

            rotated = FreeImage.Rotate4bit(dib, 270d);
            Assert.IsFalse(rotated.IsNull);
            Assert.AreEqual(3, FreeImage.GetWidth(rotated));
            Assert.AreEqual(2, FreeImage.GetHeight(rotated));
            Assert.AreEqual(FREE_IMAGE_TYPE.FIT_BITMAP, FreeImage.GetImageType(rotated));
            Assert.AreEqual(4, FreeImage.GetBPP(rotated));
            orgPal = new Palette(dib);
            rotPal = new Palette(rotated);
            Assert.IsNotNull(orgPal);
            Assert.IsNotNull(rotPal);
            Assert.AreEqual(orgPal.Length, rotPal.Length);
            for (int i = 0; i < orgPal.Length; i++)
            {
                Assert.AreEqual(orgPal[i], rotPal[i]);
            }

            FreeImage.GetPixelIndex(rotated, 0, 0, out index);
            Assert.AreEqual(2, index);
            FreeImage.GetPixelIndex(rotated, 1, 0, out index);
            Assert.AreEqual(4, index);
            FreeImage.GetPixelIndex(rotated, 2, 0, out index);
            Assert.AreEqual(6, index);
            FreeImage.GetPixelIndex(rotated, 0, 1, out index);
            Assert.AreEqual(1, index);
            FreeImage.GetPixelIndex(rotated, 1, 1, out index);
            Assert.AreEqual(3, index);
            FreeImage.GetPixelIndex(rotated, 2, 1, out index);
            Assert.AreEqual(5, index);
            FreeImage.UnloadEx(ref rotated);

            FreeImage.UnloadEx(ref dib);
        }
    }
}