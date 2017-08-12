using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
using FreeImageAPI;
using FreeImageAPI.IO;
using FreeImageAPI.Plugins;
using NUnit.Framework;

namespace FreeImageNETUnitTest.TestFixtures
{
	[TestFixture]
	public class ImportedFunctionsTest
	{
		ImageManager iManager = new ImageManager();
		FIBITMAP dib;

        [SetUp]
		public void InitEachTime()
        {
        }

		[TearDown]
		public void DeInitEachTime()
		{
		}

		[Test]
		public void FreeImage_GetVersion()
		{
			string version = FreeImage.GetVersion();
			Assert.IsNotEmpty(version);
		}

		[Test]
		public void FreeImage_GetCopyrightMessage()
		{
			string copyright = FreeImage.GetCopyrightMessage();
			Assert.IsNotEmpty(copyright);
		}

        string freeImageCallback = null;

        [Test]
		public void FreeImage_OutputMessageProc_SetOutputMessage()
		{
			Assert.IsNull(freeImageCallback);
			FreeImage.SetOutputMessage(new OutputMessageFunction(FreeImage_Message));
			FreeImage.OutputMessageProc(FREE_IMAGE_FORMAT.FIF_UNKNOWN, "unit test");
			FreeImage.SetOutputMessage(null);
			Assert.IsNotNull(freeImageCallback);
			freeImageCallback = null;
        }

        void FreeImage_Message(FREE_IMAGE_FORMAT fif, string message)
        {
            freeImageCallback = message;
        }

        [Test]
		public void FreeImage_Allocate()
		{
			dib = FreeImage.Allocate(
				133,
				77,
				8,
				FreeImage.FI_RGBA_RED_MASK,
				FreeImage.FI_RGBA_GREEN_MASK,
				FreeImage.FI_RGBA_BLUE_MASK);

			Assert.That(!dib.IsNull);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_AllocateT()
		{
			dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_RGBA16, 31, 555, 64, 0, 0, 0);

			Assert.That(!dib.IsNull);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_Clone()
		{
			dib = FreeImage.Allocate(1, 1, 32, 0, 0, 0);
			Assert.That(!dib.IsNull);

			FIBITMAP temp = FreeImage.Clone(dib);
			Assert.AreNotEqual(0, temp);

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_Load()
		{
			Assert.That(dib.IsNull);
			dib = FreeImage.Load(FREE_IMAGE_FORMAT.FIF_JPEG, Path.Combine(iManager.baseDirectory, "JPEG", "Image.jpg"), FREE_IMAGE_LOAD_FLAGS.DEFAULT);
			Assert.That(!dib.IsNull);
			FreeImage.UnloadEx(ref dib);
			Assert.That(dib.IsNull);
		}

		[Test]
		public void FreeImage_Unload()
		{
			Assert.That(dib.IsNull);
			dib = FreeImage.Load(FREE_IMAGE_FORMAT.FIF_JPEG, Path.Combine(iManager.baseDirectory, "JPEG", "Image.jpg"), FREE_IMAGE_LOAD_FLAGS.DEFAULT);
			Assert.That(!dib.IsNull);
			FreeImage.Unload(dib);
			dib.SetNull();
		}

		[Test]
		public void FreeImage_LoadFromHandle()
		{
			byte[] data = File.ReadAllBytes(iManager.GetBitmapPath(ImageType.Even, ImageColorType.Type_16_555));
			MemoryStream mStream = new MemoryStream(data);
			FreeImageIO io = FreeImageStreamIO.io;

			using (fi_handle handle = new fi_handle(mStream))
			{
				dib = FreeImage.LoadFromHandle(FREE_IMAGE_FORMAT.FIF_BMP, ref io, handle, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
				Assert.That(!dib.IsNull);

				FreeImage.UnloadEx(ref dib);
			}
		}

		[Test]
		public void FreeImage_Save()
		{
			string filename = @"test.bmp";
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_08);
			Assert.That(!dib.IsNull);

			Assert.IsTrue(FreeImage.Save(FREE_IMAGE_FORMAT.FIF_BMP, dib, filename, FREE_IMAGE_SAVE_FLAGS.DEFAULT));
			Assert.IsTrue(File.Exists(filename));
			File.Delete(filename);
			Assert.IsFalse(File.Exists(filename));

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SaveToHandle()
		{
			FreeImageIO io = new FreeImageIO();
			FreeImage.SaveToHandle(FREE_IMAGE_FORMAT.FIF_BMP, dib, ref io, new fi_handle(), FREE_IMAGE_SAVE_FLAGS.DEFAULT);
		}

		[Test]
		public void FreeImage_Memory()
		{
			dib = FreeImage.Allocate(1, 1, 1, 0, 0, 0);
			Assert.That(!dib.IsNull);
			FIMEMORY mem = FreeImage.OpenMemory(IntPtr.Zero, 0);
			Assert.AreNotEqual(0, mem);
			FreeImage.SaveToMemory(FREE_IMAGE_FORMAT.FIF_TIFF, dib, mem, FREE_IMAGE_SAVE_FLAGS.DEFAULT);
			Assert.AreNotEqual(0, FreeImage.TellMemory(mem));
			Assert.IsTrue(FreeImage.SeekMemory(mem, 0, System.IO.SeekOrigin.Begin));

			FIBITMAP temp = FreeImage.LoadFromMemory(FREE_IMAGE_FORMAT.FIF_TIFF, mem, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
			Assert.AreNotEqual(0, temp);
			FreeImage.UnloadEx(ref temp);

			uint size = 0;
			byte[] ptr = new byte[1];
			IntPtr buffer = IntPtr.Zero;
			Assert.IsTrue(FreeImage.AcquireMemory(mem, ref buffer, ref size));
			Assert.AreNotEqual(IntPtr.Zero, ptr);
			Assert.AreNotEqual(0, size);

			Assert.AreEqual(1, FreeImage.WriteMemory(ptr, 1, 1, mem));
			FreeImage.SeekMemory(mem, 1, System.IO.SeekOrigin.Begin);
			Assert.AreEqual(1, FreeImage.TellMemory(mem));
			Assert.AreEqual(2, FreeImage.ReadMemory(ptr, 1, 2, mem));
			FreeImage.CloseMemory(mem);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_RegisterLocalPlugin()
		{
			InitProc proc = null;
			Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_UNKNOWN, FreeImage.RegisterLocalPlugin(proc, "", "", "", ""));
		}

		[Test]
		public void FreeImage_RegisterExternalPlugin()
		{
			Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_UNKNOWN, FreeImage.RegisterExternalPlugin("", "", "", "", ""));
		}

		[Test]
		public void FreeImage_GetFIFCount()
		{
			Assert.AreNotEqual(0, FreeImage.GetFIFCount());
		}

		[Test]
		public void FreeImage_SetPluginEnabled_IsPluginEnabled()
		{
			FreeImage.SetPluginEnabled(FREE_IMAGE_FORMAT.FIF_PNG, false);
			Assert.AreEqual(0, FreeImage.IsPluginEnabled(FREE_IMAGE_FORMAT.FIF_PNG));
			FreeImage.SetPluginEnabled(FREE_IMAGE_FORMAT.FIF_PNG, true);
			Assert.AreEqual(1, FreeImage.IsPluginEnabled(FREE_IMAGE_FORMAT.FIF_PNG));
		}

		[Test]
		public void FreeImage_GetFIFFromFormat()
		{
			Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_UNKNOWN, FreeImage.GetFIFFromFormat(""));
			Assert.AreNotEqual(FREE_IMAGE_FORMAT.FIF_UNKNOWN, FreeImage.GetFIFFromFormat("TIFF"));
		}

		[Test]
		public void FreeImage_GetFIFFromMime()
		{
			Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_UNKNOWN, FreeImage.GetFIFFromMime(""));
			Assert.AreNotEqual(FREE_IMAGE_FORMAT.FIF_UNKNOWN, FreeImage.GetFIFFromMime("image/jpeg"));
		}

		[Test]
		public void FreeImage_GetFormatFromFIF()
		{
			Assert.IsNotEmpty(FreeImage.GetFormatFromFIF(FREE_IMAGE_FORMAT.FIF_JNG));
		}

		[Test]
		public void FreeImage_GetFIFExtensionList()
		{
			Assert.IsNotEmpty(FreeImage.GetFIFExtensionList(FREE_IMAGE_FORMAT.FIF_PGM));
		}

		[Test]
		public void FreeImage_GetFIFDescription()
		{
			Assert.IsNotEmpty(FreeImage.GetFIFDescription(FREE_IMAGE_FORMAT.FIF_PBM));
		}

		[Test]
		public void FreeImage_GetFIFRegExpr()
		{
			Assert.IsNotEmpty(FreeImage.GetFIFRegExpr(FREE_IMAGE_FORMAT.FIF_JPEG));
		}

		[Test]
		public void FreeImage_GetFIFMimeType()
		{
			Assert.IsNotEmpty(FreeImage.GetFIFMimeType(FREE_IMAGE_FORMAT.FIF_ICO));
		}

		[Test]
		public void FreeImage_GetFIFFromFilename()
		{
			Assert.AreNotEqual(FREE_IMAGE_FORMAT.FIF_UNKNOWN, FreeImage.GetFIFFromFilename("test.bmp"));
			Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_UNKNOWN, FreeImage.GetFIFFromFilename("test.000"));
		}

		[Test]
		public void FreeImage_FIFSupportsReading()
		{
			Assert.IsTrue(FreeImage.FIFSupportsReading(FREE_IMAGE_FORMAT.FIF_TIFF));
		}

		[Test]
		public void FreeImage_FIFSupportsWriting()
		{
			Assert.IsTrue(FreeImage.FIFSupportsWriting(FREE_IMAGE_FORMAT.FIF_GIF));
		}

		[Test]
		public void FreeImage_FIFSupportsExportBPP()
		{
			Assert.IsTrue(FreeImage.FIFSupportsExportBPP(FREE_IMAGE_FORMAT.FIF_BMP, 32));
			Assert.IsFalse(FreeImage.FIFSupportsExportBPP(FREE_IMAGE_FORMAT.FIF_GIF, 32));
		}

		[Test]
		public void FreeImage_FIFSupportsExportType()
		{
			Assert.IsTrue(FreeImage.FIFSupportsExportType(FREE_IMAGE_FORMAT.FIF_BMP, FREE_IMAGE_TYPE.FIT_BITMAP));
			Assert.IsFalse(FreeImage.FIFSupportsExportType(FREE_IMAGE_FORMAT.FIF_BMP, FREE_IMAGE_TYPE.FIT_COMPLEX));
		}

		[Test]
		public void FreeImage_FIFSupportsICCProfiles()
		{
			Assert.IsTrue(FreeImage.FIFSupportsICCProfiles(FREE_IMAGE_FORMAT.FIF_JPEG));
			Assert.IsFalse(FreeImage.FIFSupportsICCProfiles(FREE_IMAGE_FORMAT.FIF_BMP));
		}

		[Test]
		public void FreeImage_MultiBitmap()
		{
			FIBITMAP temp;
			FIMULTIBITMAP mdib = FreeImage.OpenMultiBitmap(
				FREE_IMAGE_FORMAT.FIF_TIFF,
				@"test.tif",
				true,
				false,
				true,
				FREE_IMAGE_LOAD_FLAGS.DEFAULT);
			Assert.AreNotEqual(0, mdib);
			Assert.AreEqual(0, FreeImage.GetPageCount(mdib));
			dib = FreeImage.Allocate(10, 10, 8, 0, 0, 0);
			FreeImage.AppendPage(mdib, dib);
			Assert.AreEqual(1, FreeImage.GetPageCount(mdib));
			FreeImage.AppendPage(mdib, dib);
			Assert.AreEqual(2, FreeImage.GetPageCount(mdib));
			FreeImage.AppendPage(mdib, dib);
			Assert.AreEqual(3, FreeImage.GetPageCount(mdib));
			FreeImage.CloseMultiBitmapEx(ref mdib);
			FreeImage.UnloadEx(ref dib);
			mdib.SetNull();
			mdib = FreeImage.OpenMultiBitmap(FREE_IMAGE_FORMAT.FIF_TIFF, @"test.tif", false, false, true, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
			Assert.AreNotEqual(0, mdib);
			Assert.AreEqual(3, FreeImage.GetPageCount(mdib));
			dib = FreeImage.LockPage(mdib, 1);
			temp = FreeImage.LockPage(mdib, 2);

			int[] pages = null;
			int count = 0;
			FreeImage.GetLockedPageNumbers(mdib, pages, ref count);
			Assert.AreEqual(2, count);
			pages = new int[count];
			FreeImage.GetLockedPageNumbers(mdib, pages, ref count);
			Assert.AreEqual(2, pages.Length);
			FreeImage.UnlockPage(mdib, dib, false);
			FreeImage.UnlockPage(mdib, temp, true);
			dib.SetNull();
			Assert.IsTrue(FreeImage.MovePage(mdib, 0, 1));
			FreeImage.CloseMultiBitmapEx(ref mdib);
			Assert.IsTrue(System.IO.File.Exists("test.tif"));
			System.IO.File.Delete("test.tif");
		}

		[Test]
		public void FreeImage_GetFileType()
		{
			Assert.AreNotEqual(FREE_IMAGE_FORMAT.FIF_UNKNOWN, FreeImage.GetFileType(iManager.GetBitmapPath(ImageType.Even, ImageColorType.Type_08_Greyscale_Unordered), 0));
		}

		[Test]
		public void FreeImage_GetFileTypeFromHandle()
		{
			FreeImageIO io = FreeImageStreamIO.io;
			Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_UNKNOWN, FreeImage.GetFileTypeFromHandle(ref io, new fi_handle(), 0));
		}

		[Test]
		public void FreeImage_GetFileTypeFromMemory()
		{
			Assert.AreEqual(FREE_IMAGE_FORMAT.FIF_UNKNOWN, FreeImage.GetFileTypeFromMemory(new FIMEMORY(), 0));
		}

		[Test]
		public void FreeImage_IsLittleEndian()
		{
			Assert.IsTrue(FreeImage.IsLittleEndian());
		}

		[Test]
		public void FreeImage_LookupX11Color()
		{
			byte red, green, blue;
			FreeImage.LookupX11Color("lawngreen", out red, out green, out blue);
			Assert.AreEqual(124, red);
			Assert.AreEqual(252, green);
			Assert.AreEqual(0, blue);
		}

		[Test]
		public void FreeImage_LookupSVGColor()
		{
			byte red, green, blue;
			FreeImage.LookupX11Color("orchid", out red, out green, out blue);
			Assert.AreEqual(218, red);
			Assert.AreEqual(112, green);
			Assert.AreEqual(214, blue);
		}

		[Test]
		public void FreeImage_GetBits()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_01_Threshold);
			Assert.That(!dib.IsNull);
			Assert.AreNotEqual(IntPtr.Zero, FreeImage.GetBits(dib));
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetScanLine()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_04_Greyscale_MinIsBlack);
			Assert.That(!dib.IsNull);
			Assert.AreNotEqual(IntPtr.Zero, FreeImage.GetScanLine(dib, 0));
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetPixelIndex_SetPixelIndex()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_04_Greyscale_Unordered);
			Assert.That(!dib.IsNull);
			byte index_old, index_new;
			Assert.IsTrue(FreeImage.GetPixelIndex(dib, 31, 10, out index_old));
			index_new = index_old == byte.MaxValue ? (byte)0 : (byte)(index_old + 1);
			Assert.IsTrue(FreeImage.SetPixelIndex(dib, 31, 10, ref index_new));
			Assert.IsTrue(FreeImage.GetPixelIndex(dib, 31, 10, out index_old));
			Assert.AreEqual(index_new, index_old);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetPixelColor_SetPixelColor()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.That(!dib.IsNull);
			RGBQUAD value_old, value_new;
			Assert.IsTrue(FreeImage.GetPixelColor(dib, 77, 61, out value_old));
			value_new = (value_old == (RGBQUAD)Color.White) ? Color.Black : Color.White;
			Assert.IsTrue(FreeImage.SetPixelColor(dib, 77, 61, ref value_new));
			Assert.IsTrue(FreeImage.GetPixelColor(dib, 77, 61, out value_old));
			Assert.AreEqual(value_new, value_old);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_Bitmap_information_functions()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_08_Greyscale_MinIsBlack);
			Assert.That(!dib.IsNull);
			Assert.AreEqual(FREE_IMAGE_TYPE.FIT_BITMAP, FreeImage.GetImageType(dib));
			Assert.AreNotEqual(0, FreeImage.GetColorsUsed(dib));
			Assert.AreEqual(8, FreeImage.GetBPP(dib));
			Assert.AreNotEqual(0, FreeImage.GetWidth(dib));
			Assert.AreNotEqual(0, FreeImage.GetHeight(dib));
			Assert.AreNotEqual(0, FreeImage.GetLine(dib));
			Assert.AreNotEqual(0, FreeImage.GetPitch(dib));
			Assert.AreNotEqual(0, FreeImage.GetDIBSize(dib));
			Assert.AreNotEqual(IntPtr.Zero, FreeImage.GetPalette(dib));
			FreeImage.SetDotsPerMeterX(dib, 1234);
			FreeImage.SetDotsPerMeterY(dib, 4321);
			Assert.AreEqual(1234, FreeImage.GetDotsPerMeterX(dib));
			Assert.AreEqual(4321, FreeImage.GetDotsPerMeterY(dib));
			Assert.AreNotEqual(IntPtr.Zero, FreeImage.GetInfoHeader(dib));
			Assert.AreNotEqual(IntPtr.Zero, FreeImage.GetInfo(dib));
			Assert.AreEqual(FREE_IMAGE_COLOR_TYPE.FIC_MINISBLACK, FreeImage.GetColorType(dib));
			Assert.AreEqual(0, FreeImage.GetRedMask(dib));
			Assert.AreEqual(0, FreeImage.GetGreenMask(dib));
			Assert.AreEqual(0, FreeImage.GetBlueMask(dib));
			Assert.AreEqual(0, FreeImage.GetTransparencyCount(dib));
			Assert.AreNotEqual(IntPtr.Zero, FreeImage.GetTransparencyTable(dib));
			FreeImage.SetTransparent(dib, true);
			Assert.IsTrue(FreeImage.IsTransparent(dib));
			FreeImage.SetTransparencyTable(dib, new byte[] { });
			Assert.IsFalse(FreeImage.IsTransparent(dib));
			Assert.IsFalse(FreeImage.HasBackgroundColor(dib));
			RGBQUAD rgb = Color.Teal;
			Assert.IsTrue(FreeImage.SetBackgroundColor(dib, ref rgb));
			Assert.IsTrue(FreeImage.GetBackgroundColor(dib, out rgb));
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetICCProfile()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);
			new FIICCPROFILE(dib, new byte[] { 0xFF, 0xAA, 0x00, 0x33 });
			FIICCPROFILE p = FreeImage.GetICCProfileEx(dib);
			Assert.AreEqual(4, p.Size);
			Assert.AreEqual(0xAA, p.Data[1]);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_CreateICCProfile()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);
			byte[] data = new byte[256];
			Assert.AreNotEqual(IntPtr.Zero, FreeImage.CreateICCProfile(dib, data, 256));
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_DestroyICCProfile()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);
			FreeImage.DestroyICCProfile(dib);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ConvertTo4Bits()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.ConvertTo4Bits(dib);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(4, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ConvertTo8Bits()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.ConvertTo8Bits(dib);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(8, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ConvertToGreyscale()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.ConvertToGreyscale(dib);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(8, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ConvertTo16Bits555()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.ConvertTo16Bits555(dib);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(16, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ConvertTo16Bits565()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.ConvertTo16Bits565(dib);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(16, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ConvertTo24Bits()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.ConvertTo24Bits(dib);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(24, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ConvertTo32Bits()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.ConvertTo32Bits(dib);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(32, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ColorQuantize()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.ColorQuantize(dib, FREE_IMAGE_QUANTIZE.FIQ_WUQUANT);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(8, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ColorQuantizeEx()
		{
			FIBITMAP paletteDib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_08);
			Assert.IsFalse(paletteDib.IsNull);
			Palette palette = FreeImage.GetPaletteEx(paletteDib);
			RGBQUAD[] table = palette.Data;

			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			FIBITMAP temp = FreeImage.ColorQuantizeEx(dib, FREE_IMAGE_QUANTIZE.FIQ_WUQUANT, (int)palette.Length, (int)palette.Length, table);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(8, FreeImage.GetBPP(temp));

			FreeImage.UnloadEx(ref paletteDib);
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_Threshold()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.Threshold(dib, 128);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(1, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_Dither()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.Dither(dib, FREE_IMAGE_DITHER.FID_FS);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(1, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_RawBits()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.That(!dib.IsNull);
			IntPtr buffer = Marshal.AllocHGlobal((int)FreeImage.GetDIBSize(dib));
			FreeImage.ConvertToRawBits(
				buffer,
				dib,
				(int)FreeImage.GetPitch(dib),
				FreeImage.GetBPP(dib),
				FreeImage.GetRedMask(dib),
				FreeImage.GetGreenMask(dib),
				FreeImage.GetBlueMask(dib),
				true);
			FIBITMAP temp = FreeImage.ConvertFromRawBits(
				buffer,
				(int)FreeImage.GetWidth(dib),
				(int)FreeImage.GetHeight(dib),
				(int)FreeImage.GetPitch(dib),
				FreeImage.GetBPP(dib),
				FreeImage.GetRedMask(dib),
				FreeImage.GetGreenMask(dib),
				FreeImage.GetBlueMask(dib),
				true);

			Assert.AreNotEqual(0, temp);

			Marshal.FreeHGlobal(buffer);
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ConvertToRGBF()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.ConvertToRGBF(dib);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(FREE_IMAGE_TYPE.FIT_RGBF, FreeImage.GetImageType(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ConvertToStandardType()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_04_Greyscale_MinIsBlack);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.ConvertToStandardType(dib, true);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(FREE_IMAGE_COLOR_TYPE.FIC_PALETTE, FreeImage.GetColorType(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ConvertToType()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_08_Greyscale_Unordered);
			Assert.That(!dib.IsNull);
			FIBITMAP temp = FreeImage.ConvertToType(dib, FREE_IMAGE_TYPE.FIT_UINT32, true);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(FREE_IMAGE_TYPE.FIT_UINT32, FreeImage.GetImageType(temp));
			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ToneMapping()
		{
			FIBITMAP temp;
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			FIBITMAP rgbf = FreeImage.ConvertToRGBF(dib);
			Assert.AreNotEqual(0, rgbf);
			Assert.AreEqual(FREE_IMAGE_TYPE.FIT_RGBF, FreeImage.GetImageType(rgbf));
			Assert.AreEqual(96, FreeImage.GetBPP(rgbf));

			temp = FreeImage.ToneMapping(rgbf, FREE_IMAGE_TMO.FITMO_REINHARD05, 1f, 1.1f);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(24, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);

			FreeImage.UnloadEx(ref rgbf);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_TmoDrago03()
		{
			FIBITMAP temp;
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			FIBITMAP rgbf = FreeImage.ConvertToRGBF(dib);
			Assert.AreNotEqual(0, rgbf);
			Assert.AreEqual(FREE_IMAGE_TYPE.FIT_RGBF, FreeImage.GetImageType(rgbf));
			Assert.AreEqual(96, FreeImage.GetBPP(rgbf));

			temp = FreeImage.TmoDrago03(rgbf, 1f, 1.2f);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(24, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);

			FreeImage.UnloadEx(ref rgbf);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_TmoReinhard05()
		{
			FIBITMAP temp;
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			FIBITMAP rgbf = FreeImage.ConvertToRGBF(dib);
			Assert.AreNotEqual(0, rgbf);
			Assert.AreEqual(FREE_IMAGE_TYPE.FIT_RGBF, FreeImage.GetImageType(rgbf));
			Assert.AreEqual(96, FreeImage.GetBPP(rgbf));

			temp = FreeImage.TmoReinhard05(rgbf, 0f, 0.25f);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(24, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);

			FreeImage.UnloadEx(ref rgbf);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_TmoFattal02()
		{
			FIBITMAP temp;
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			FIBITMAP rgbf = FreeImage.ConvertToRGBF(dib);
			Assert.AreNotEqual(0, rgbf);
			Assert.AreEqual(FREE_IMAGE_TYPE.FIT_RGBF, FreeImage.GetImageType(rgbf));
			Assert.AreEqual(96, FreeImage.GetBPP(rgbf));

			temp = FreeImage.TmoFattal02(rgbf, 1f, 0.79f);
			Assert.AreNotEqual(0, temp);
			Assert.AreEqual(24, FreeImage.GetBPP(temp));
			FreeImage.UnloadEx(ref temp);

			FreeImage.UnloadEx(ref rgbf);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ZLibCompress_ZLibUncompress()
		{
			Random rand = new Random(DateTime.Now.Millisecond);
			byte[] source = new byte[10240];
			byte[] compressed = new byte[(int)(10355f * 1.01 + 12f)];
			byte[] uncompressed = new byte[10240];
			rand.NextBytes(source);
			Assert.AreNotEqual(0, FreeImage.ZLibCompress(compressed, (uint)compressed.Length, source, (uint)source.Length));
			Assert.AreNotEqual(0, FreeImage.ZLibUncompress(uncompressed, (uint)source.Length, compressed, (uint)compressed.Length));
			for (int i = 0; i < source.Length; i++)
				if (source[i] != uncompressed[i])
					Assert.Fail();
		}

		[Test]
		public void FreeImage_ZLibGZip_ZLibGUnzip()
		{
			Random rand = new Random(DateTime.Now.Millisecond);
			byte[] source = new byte[10240];
			byte[] compressed = new byte[(int)(10355f * 1.01 + 24f)];
			byte[] uncompressed = new byte[10240];
			rand.NextBytes(source);
			Assert.AreNotEqual(0, FreeImage.ZLibGZip(compressed, (uint)compressed.Length, source, (uint)source.Length));
			Assert.AreNotEqual(0, FreeImage.ZLibGUnzip(uncompressed, (uint)source.Length, compressed, (uint)compressed.Length));
			for (int i = 0; i < source.Length; i++)
				if (source[i] != uncompressed[i])
					Assert.Fail();
		}

		[Test]
		public void FreeImage_ZLibCRC32()
		{
			byte[] buffer = new byte[0];
			Assert.AreEqual(0xFEBCA008, FreeImage.ZLibCRC32(0xFEBCA008, buffer, 0));
		}

		[Test]
		public void FreeImage_CreateTag()
		{
			FITAG tag = FreeImage.CreateTag();
			Assert.AreNotEqual(0, tag);
			FITAG tag_clone = FreeImage.CloneTag(tag);
			Assert.AreNotEqual(0, tag_clone);
			FreeImage.DeleteTag(tag);
			FreeImage.DeleteTag(tag_clone);
		}

		[Test]
		public void FreeImage_Tag_accessors()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			Assert.IsTrue(FreeImage.FindNextMetadata(mData, out tag));
			Assert.AreNotEqual(0, tag);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetTagKey()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.GetTagKey(tag);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetTagDescription()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.GetTagDescription(tag);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetTagID()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.GetTagID(tag);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetTagType()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.GetTagType(tag);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetTagCount()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.GetTagCount(tag);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetTagLength()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.GetTagLength(tag);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetTagValue()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.GetTagValue(tag);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SetTagKey()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.SetTagKey(tag, "");

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SetTagDescription()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.SetTagDescription(tag, "");

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SetTagID()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.SetTagID(tag, 44);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SetTagType()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.SetTagType(tag, FREE_IMAGE_MDTYPE.FIDT_ASCII);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SetTagCount()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.SetTagCount(tag, 3);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SetTagLength()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			FreeImage.SetTagLength(tag, 6);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SetTagValue()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			int length = (int)FreeImage.GetTagLength(tag);
			FREE_IMAGE_MDTYPE type = FreeImage.GetTagType(tag);
			int count = (int)FreeImage.GetTagCount(tag);

			byte[] buffer = new byte[length * count];

			FreeImage.SetTagValue(tag, buffer);

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetMetadataCount()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			Assert.AreNotEqual(0, FreeImage.GetMetadataCount(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib));
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_TagToString()
		{
			dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
			Assert.That(!dib.IsNull);

			FITAG tag;
			FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, dib, out tag);
			Assert.AreNotEqual(0, mData);
			Assert.AreNotEqual(0, tag);

			Assert.IsNotEmpty(FreeImage.TagToString(FREE_IMAGE_MDMODEL.FIMD_EXIF_EXIF, tag, 0));

			FreeImage.FindCloseMetadata(mData);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_RotateClassic()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			FIBITMAP temp = FreeImage.RotateClassic(dib, 45d);
			Assert.AreNotEqual(0, temp);

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_RotateEx()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			FIBITMAP temp = FreeImage.RotateEx(dib, 261d, 0d, 33d, 51d, 9d, true);
			Assert.AreNotEqual(0, temp);

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_FlipHorizontal()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			Assert.IsTrue(FreeImage.FlipHorizontal(dib));

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_FlipVertical()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			Assert.IsTrue(FreeImage.FlipVertical(dib));

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_JPEGTransform()
		{
			string filename = iManager.GetBitmapPath(ImageType.JPEG, ImageColorType.Type_24);
			string filenameOut = filename + ".out.jpg";
			Assert.IsTrue(File.Exists(filename));

			Assert.IsTrue(FreeImage.JPEGTransform(filename, filenameOut, FREE_IMAGE_JPEG_OPERATION.FIJPEG_OP_FLIP_V, false));
			Assert.IsTrue(File.Exists(filenameOut));

			FIBITMAP temp = FreeImage.Load(FREE_IMAGE_FORMAT.FIF_JPEG, filenameOut, FREE_IMAGE_LOAD_FLAGS.JPEG_ACCURATE);
			Assert.AreNotEqual(0, temp);

			File.Delete(filenameOut);
			Assert.IsFalse(File.Exists(filenameOut));

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_Rescale()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_16_555);
			Assert.That(!dib.IsNull);

			FIBITMAP temp = FreeImage.Rescale(dib, 100, 100, FREE_IMAGE_FILTER.FILTER_BICUBIC);
			Assert.AreNotEqual(0, temp);

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_MakeThumbnail()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_16_555);
			Assert.That(!dib.IsNull);

			FIBITMAP temp = FreeImage.MakeThumbnail(dib, 50, false);
			Assert.AreNotEqual(0, temp);

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_AdjustCurve()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			byte[] lut = new byte[256];
			Assert.IsTrue(FreeImage.AdjustCurve(dib, lut, FREE_IMAGE_COLOR_CHANNEL.FICC_GREEN));

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_AdjustGamma()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			Assert.IsTrue(FreeImage.AdjustGamma(dib, 1.3d));

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_AdjustBrightness()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			Assert.IsTrue(FreeImage.AdjustBrightness(dib, 1.3d));

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_AdjustContrast()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			Assert.IsTrue(FreeImage.AdjustContrast(dib, 1.3d));

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_Invert()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			Assert.IsTrue(FreeImage.Invert(dib));

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetHistogram()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			int[] histo = new int[256];
			Assert.IsTrue(FreeImage.GetHistogram(dib, histo, FREE_IMAGE_COLOR_CHANNEL.FICC_RED));

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetChannel()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.That(!dib.IsNull);

			FIBITMAP temp = FreeImage.GetChannel(dib, FREE_IMAGE_COLOR_CHANNEL.FICC_GREEN);
			Assert.AreNotEqual(0, temp);

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SetChannel()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.That(!dib.IsNull);
			FIBITMAP dib8 = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_08_Greyscale_MinIsBlack);
			Assert.AreNotEqual(0, dib8);
            Assert.AreEqual(FreeImage.GetWidth(dib), FreeImage.GetWidth(dib8));
            Assert.AreEqual(FreeImage.GetHeight(dib), FreeImage.GetHeight(dib8));

			Assert.IsTrue(FreeImage.SetChannel(dib, dib8, FREE_IMAGE_COLOR_CHANNEL.FICC_BLUE));

			FreeImage.UnloadEx(ref dib8);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetComplexChannel()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_08);
			Assert.That(!dib.IsNull);

			FIBITMAP temp = FreeImage.ConvertToType(dib, FREE_IMAGE_TYPE.FIT_COMPLEX, true);
			Assert.AreNotEqual(0, temp);

			FIBITMAP temp2 = FreeImage.GetComplexChannel(temp, FREE_IMAGE_COLOR_CHANNEL.FICC_IMAG);
			Assert.AreNotEqual(0, temp2);

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref temp2);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SetComplexChannel()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_08_Greyscale_Unordered);
			Assert.That(!dib.IsNull);

			FIBITMAP temp = FreeImage.ConvertToType(dib, FREE_IMAGE_TYPE.FIT_COMPLEX, true);
			Assert.AreNotEqual(0, temp);

			FIBITMAP temp2 = FreeImage.GetComplexChannel(temp, FREE_IMAGE_COLOR_CHANNEL.FICC_IMAG);
			Assert.AreNotEqual(0, temp2);

			Assert.IsTrue(FreeImage.SetComplexChannel(temp, temp2, FREE_IMAGE_COLOR_CHANNEL.FICC_IMAG));

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref temp2);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_Copy()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_08_Greyscale_MinIsBlack);
			Assert.That(!dib.IsNull);

			FIBITMAP temp = FreeImage.Copy(dib, 5, 9, 44, 2);
			Assert.AreNotEqual(0, temp);

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_Paste()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_08_Greyscale_MinIsBlack);
			Assert.That(!dib.IsNull);

			FIBITMAP temp = FreeImage.Allocate(3, 3, 8, 0, 0, 0);
			Assert.IsTrue(FreeImage.Paste(dib, temp, 31, 3, 256));

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_Composite()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_08_Greyscale_MinIsBlack);
			Assert.That(!dib.IsNull);
			RGBQUAD rgbq = new RGBQUAD();

			FIBITMAP temp = FreeImage.Composite(dib, false, ref rgbq, new FIBITMAP());
			Assert.AreNotEqual(0, temp);

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_JPEGCrop()
		{
			string filename = iManager.GetBitmapPath(ImageType.JPEG, ImageColorType.Type_01_Dither);
			Assert.IsTrue(File.Exists(filename));
			string filenameOut = filename + ".out.jpg";

			Assert.IsTrue(FreeImage.JPEGCrop(filename, filenameOut, 3, 2, 1, 5));
			Assert.IsTrue(File.Exists(filenameOut));

			FIBITMAP temp = FreeImage.Load(FREE_IMAGE_FORMAT.FIF_JPEG, filenameOut, FREE_IMAGE_LOAD_FLAGS.JPEG_ACCURATE);
			Assert.AreNotEqual(0, temp);

			File.Delete(filenameOut);
			Assert.IsFalse(File.Exists(filenameOut));

			FreeImage.UnloadEx(ref temp);
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_PreMultiplyWithAlpha()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			Assert.IsFalse(dib.IsNull);

			Assert.IsTrue(FreeImage.PreMultiplyWithAlpha(dib));
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_MultigridPoissonSolver()
		{
			dib = FreeImage.AllocateT(FREE_IMAGE_TYPE.FIT_FLOAT, 10, 10, 32, 0, 0, 0);
			Assert.IsFalse(dib.IsNull);

			FIBITMAP dib2 = FreeImage.MultigridPoissonSolver(dib, 2);

			FreeImage.UnloadEx(ref dib);
			FreeImage.UnloadEx(ref dib2);
		}

		[Test]
		public void FreeImage_GetAdjustColorsLookupTable()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_24);
			Assert.IsFalse(dib.IsNull);

			byte[] lut = new byte[256];
			FreeImage.GetAdjustColorsLookupTable(lut, 55d, 0d, 2.1d, false);

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_AdjustColors()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_24);
			Assert.IsFalse(dib.IsNull);

			Assert.IsTrue(FreeImage.AdjustColors(dib, -4d, 22d, 1.1d, false));

			FreeImage.UnloadEx(ref dib);
		}

		[Ignore("Ignoring FreeImage_ApplyColorMapping")]
		public void FreeImage_ApplyColorMapping()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			FreeImage_ApplyColorMapping2(dib);

			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			FreeImage_ApplyColorMapping2(dib);

			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			FreeImage_ApplyColorMapping2(dib);

			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			FreeImage_ApplyColorMapping2(dib);

			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			FreeImage_ApplyColorMapping2(dib);

			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			FreeImage_ApplyColorMapping2(dib);

			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			FreeImage_ApplyColorMapping2(dib);

			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			FreeImage_ApplyColorMapping2(dib);

			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_32);
			FreeImage_ApplyColorMapping2(dib);
		}

		private void FreeImage_ApplyColorMapping2(FIBITMAP dib)
		{
			Assert.IsFalse(dib.IsNull);

			Scanline<RGBQUAD> rgbqa = new Scanline<RGBQUAD>(dib, 0);

			RGBQUAD[] src = new RGBQUAD[1];
			RGBQUAD[] dst = new RGBQUAD[1];
			src[0] = rgbqa[0];
			dst[0].Color = src[0].Color == Color.White ? Color.Thistle : Color.White;

			uint count = FreeImage.ApplyColorMapping(dib, src, dst, 1, true, false); // Memory
			Assert.That(count > 0);

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SwapColors()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_08);
			Assert.IsFalse(dib.IsNull);

			RGBQUAD src = new RGBQUAD(Color.FromArgb(93, 119, 170));
			RGBQUAD dst = new RGBQUAD(Color.FromArgb(90, 130, 148));

			uint count = FreeImage.SwapColors(dib, ref src, ref dst, true);

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_ApplyPaletteIndexMapping()
		{
			// alle farbtiefen

			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_04);
			Assert.IsFalse(dib.IsNull);

			byte[] src = { 0, 3, 1 };
			byte[] dst = { 3, 1, 0 };

			uint count = FreeImage.ApplyPaletteIndexMapping(dib, src, dst, 3, false);
			Assert.That(count > 0);

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SwapPaletteIndices()
		{
			dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_04);
			Assert.IsFalse(dib.IsNull);

			byte src = 0;
			byte dst = 3;

			uint count = FreeImage.SwapPaletteIndices(dib, ref src, ref dst);
			Assert.That(count > 0);

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_SetTransparentIndex()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_04);
			Assert.IsFalse(dib.IsNull);

			FreeImage.SetTransparentIndex(dib, 0);

			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public void FreeImage_GetTransparentIndex()
		{
			dib = iManager.GetBitmap(ImageType.Even, ImageColorType.Type_04);
			Assert.IsFalse(dib.IsNull);

			int i = FreeImage.GetTransparentIndex(dib);

			FreeImage.UnloadEx(ref dib);
		}
	}
}