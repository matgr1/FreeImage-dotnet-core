using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.Serialization.Formatters.Binary;
using FreeImageAPI;
using NUnit.Framework;

namespace FreeImageNETUnitTest.TestFixtures
{
	[TestFixture]
	public class FreeImageBitmapTest
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

		[Test]
		public void FreeImageBitmapConstructors()
		{
			Image bitmap;
			FreeImageBitmap fib, fib2;
			Stream stream;
			Graphics g;
			string filename = iManager.GetBitmapPath(ImageType.Odd, ImageColorType.Type_24);
			Assert.IsNotNull(filename);
			Assert.IsTrue(File.Exists(filename));

			bitmap = new Bitmap(filename);
			Assert.IsNotNull(bitmap);

			fib = new FreeImageBitmap(bitmap);
			Assert.AreEqual(bitmap.Width, fib.Width);
			Assert.AreEqual(bitmap.Height, fib.Height);
			fib.Dispose();
			fib.Dispose();

			fib = new FreeImageBitmap(bitmap, new Size(100, 100));
			Assert.AreEqual(100, fib.Width);
			Assert.AreEqual(100, fib.Height);
			fib.Dispose();
			bitmap.Dispose();

			fib = new FreeImageBitmap(filename);
			fib.Dispose();

			fib = new FreeImageBitmap(filename, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
			fib.Dispose();

			fib = new FreeImageBitmap(filename, FREE_IMAGE_FORMAT.FIF_UNKNOWN);
			fib.Dispose();

			fib = new FreeImageBitmap(filename, FREE_IMAGE_FORMAT.FIF_UNKNOWN, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
			fib.Dispose();

			stream = new FileStream(filename, FileMode.Open);
			Assert.IsNotNull(stream);

			fib = new FreeImageBitmap(stream);
			fib.Dispose();
			stream.Seek(0, SeekOrigin.Begin);

			fib = new FreeImageBitmap(stream, FREE_IMAGE_FORMAT.FIF_UNKNOWN);
			fib.Dispose();
			stream.Seek(0, SeekOrigin.Begin);

			fib = new FreeImageBitmap(stream, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
			fib.Dispose();
			stream.Seek(0, SeekOrigin.Begin);

			fib = new FreeImageBitmap(stream, FREE_IMAGE_FORMAT.FIF_UNKNOWN, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
			fib.Dispose();
			stream.Dispose();

			fib = new FreeImageBitmap(100, 100);
			Assert.AreEqual(24, fib.ColorDepth);
			Assert.AreEqual(100, fib.Width);
			Assert.AreEqual(100, fib.Height);
			fib.Dispose();

			using (bitmap = new Bitmap(filename))
			{
				Assert.IsNotNull(bitmap);
				using (g = Graphics.FromImage(bitmap))
				{
					Assert.IsNotNull(g);
					fib = new FreeImageBitmap(100, 100, g);
				}
			}
			fib.Dispose();

			fib = new FreeImageBitmap(100, 100, PixelFormat.Format1bppIndexed);
			Assert.AreEqual(PixelFormat.Format1bppIndexed, fib.PixelFormat);
			Assert.AreEqual(100, fib.Width);
			Assert.AreEqual(100, fib.Height);
			fib.Dispose();

			fib = new FreeImageBitmap(100, 100, PixelFormat.Format4bppIndexed);
			Assert.AreEqual(PixelFormat.Format4bppIndexed, fib.PixelFormat);
			Assert.AreEqual(100, fib.Width);
			Assert.AreEqual(100, fib.Height);
			fib.Dispose();

			fib = new FreeImageBitmap(100, 100, PixelFormat.Format8bppIndexed);
			Assert.AreEqual(PixelFormat.Format8bppIndexed, fib.PixelFormat);
			Assert.AreEqual(100, fib.Width);
			Assert.AreEqual(100, fib.Height);
			fib.Dispose();

			fib = new FreeImageBitmap(100, 100, PixelFormat.Format16bppRgb555);
			Assert.AreEqual(PixelFormat.Format16bppRgb555, fib.PixelFormat);
			Assert.AreEqual(100, fib.Width);
			Assert.AreEqual(100, fib.Height);
			fib.Dispose();

			fib = new FreeImageBitmap(100, 100, PixelFormat.Format16bppRgb565);
			Assert.AreEqual(PixelFormat.Format16bppRgb565, fib.PixelFormat);
			Assert.AreEqual(100, fib.Width);
			Assert.AreEqual(100, fib.Height);
			fib.Dispose();

			fib = new FreeImageBitmap(100, 100, PixelFormat.Format24bppRgb);
			Assert.AreEqual(PixelFormat.Format24bppRgb, fib.PixelFormat);
			Assert.AreEqual(100, fib.Width);
			Assert.AreEqual(100, fib.Height);
			fib.Dispose();

			fib = new FreeImageBitmap(100, 100, PixelFormat.Format32bppArgb);
			Assert.AreEqual(PixelFormat.Format32bppArgb, fib.PixelFormat);
			Assert.AreEqual(100, fib.Width);
			Assert.AreEqual(100, fib.Height);

			stream = new MemoryStream();
			BinaryFormatter formatter = new BinaryFormatter();

			formatter.Serialize(stream, fib);
			Assert.Greater(stream.Length, 0);
			stream.Position = 0;

			fib2 = formatter.Deserialize(stream) as FreeImageBitmap;
			stream.Dispose();
			fib.Dispose();
			fib2.Dispose();

			fib = new FreeImageBitmap(filename);
			fib2 = new FreeImageBitmap(fib);
			fib2.Dispose();

			fib2 = new FreeImageBitmap(fib, new Size(31, 22));
			Assert.AreEqual(31, fib2.Width);
			Assert.AreEqual(22, fib2.Height);
			fib2.Dispose();
			fib.Dispose();

			dib = FreeImage.Allocate(1000, 800, 24, 0xFF0000, 0xFF00, 0xFF);
			Assert.IsFalse(dib.IsNull);

			fib = new FreeImageBitmap(1000, 800, -(int)FreeImage.GetPitch(dib), FreeImage.GetPixelFormat(dib), FreeImage.GetScanLine(dib, 0));
			fib.Dispose();
			FreeImage.UnloadEx(ref dib);
		}

		[Test]
		public unsafe void Properties()
		{
			string filename = iManager.GetBitmapPath(ImageType.Even, ImageColorType.Type_32);
			Assert.IsNotNull(filename);
			Assert.IsTrue(File.Exists(filename));

			FreeImageBitmap fib = new FreeImageBitmap(filename);
			Assert.IsFalse(fib.HasPalette);

			try
			{
				Palette palette = fib.Palette;
				Assert.Fail();
			}
			catch
			{
			}

			Assert.IsFalse(fib.HasBackgroundColor);
			fib.BackgroundColor = Color.LightSeaGreen;
			Assert.IsTrue(fib.HasBackgroundColor);
			Assert.That(
				Color.LightSeaGreen.B == fib.BackgroundColor.Value.B &&
				Color.LightSeaGreen.G == fib.BackgroundColor.Value.G &&
				Color.LightSeaGreen.R == fib.BackgroundColor.Value.R);
			fib.BackgroundColor = null;
			Assert.IsFalse(fib.HasBackgroundColor);
			fib.Dispose();

			fib = new FreeImageBitmap(100, 100, PixelFormat.Format1bppIndexed);
			ImageFlags flags = (ImageFlags)fib.Flags;
			Assert.That((flags & ImageFlags.ColorSpaceRgb) == ImageFlags.ColorSpaceRgb);
			Assert.That((flags & ImageFlags.HasAlpha) != ImageFlags.HasAlpha);
			Assert.That((flags & ImageFlags.HasRealDpi) != ImageFlags.HasRealDpi);
			Assert.That((flags & ImageFlags.HasTranslucent) != ImageFlags.HasTranslucent);
			fib.Dispose();

			dib = FreeImage.Allocate(100, 100, 32, 0xFF0000, 0xFF00, 0xFF);
			FIICCPROFILE* prof = (FIICCPROFILE*)FreeImage.CreateICCProfile(dib, new byte[] { 0, 1, 2, 3 }, 4);
			fib = new FreeImageBitmap(dib);
			Scanline<RGBQUAD> sc = (Scanline<RGBQUAD>)fib.GetScanline(0);
			RGBQUAD rgbq = sc[0];
			rgbq.rgbReserved = 127;
			sc[0] = rgbq;
			flags = (ImageFlags)fib.Flags;
			Assert.That((flags & ImageFlags.HasAlpha) == ImageFlags.HasAlpha);
			Assert.That((flags & ImageFlags.HasRealDpi) != ImageFlags.HasRealDpi);
			Assert.That((flags & ImageFlags.HasTranslucent) == ImageFlags.HasTranslucent);
			fib.Dispose();
			fib = null;
			GC.Collect(2, GCCollectionMode.Forced);
			GC.WaitForPendingFinalizers();

			fib = new FreeImageBitmap(iManager.GetBitmapPath(ImageType.Metadata, ImageColorType.Type_01_Dither));
			int[] propList = fib.PropertyIdList;
			Assert.IsNotNull(propList);
			Assert.Greater(propList.Length, 0);
			PropertyItem[] propItemList = fib.PropertyItems;
			Assert.IsNotNull(propItemList);
			Assert.Greater(propItemList.Length, 0);
			Assert.IsNotNull(fib.RawFormat);
			fib.Dispose();

			fib = new FreeImageBitmap(iManager.GetBitmapPath(ImageType.Multipaged, ImageColorType.Type_01_Dither));
			Assert.Greater(fib.FrameCount, 1);
			fib.Dispose();
		}

		[Test]
		public void GetBounds()
		{
			Random rand = new Random();
			int height = rand.Next(1, 100), width = rand.Next(1, 100);
			FreeImageBitmap fib = new FreeImageBitmap(width, height, PixelFormat.Format24bppRgb);

			Assert.AreEqual(fib.VerticalResolution, fib.HorizontalResolution);
			GraphicsUnit unit;
			RectangleF rect;

			unit = GraphicsUnit.Display;
			rect = fib.GetBounds(ref unit);

			Assert.AreEqual(GraphicsUnit.Pixel, unit);
			Assert.AreEqual(width, (int)rect.Width);
			Assert.AreEqual(height, (int)rect.Height);
			fib.Dispose();
		}

		[Test]
		public void GetPropertyItem()
		{
			FreeImageBitmap fib = new FreeImageBitmap(iManager.GetBitmapPath(ImageType.Metadata, ImageColorType.Type_01_Dither));
			int[] list = fib.PropertyIdList;
			Assert.IsNotNull(list);
			Assert.Greater(list.Length, 0);

			for (int i = 0; i < list.Length; i++)
			{
				PropertyItem item = fib.GetPropertyItem(list[i]);
				Assert.IsNotNull(item);
			}
			fib.Dispose();
		}

		[Test]
		public void RemovePropertyItem()
		{
			FreeImageBitmap fib = new FreeImageBitmap(iManager.GetBitmapPath(ImageType.Metadata, ImageColorType.Type_01_Dither));
			Random rand = new Random();
			int[] list = fib.PropertyIdList;
			int length = list.Length;
			Assert.Greater(list.Length, 0);

			int id = list[rand.Next(0, list.Length - 1)];
			Assert.IsNotNull(fib.GetPropertyItem(id));
			fib.RemovePropertyItem(id);
			list = fib.PropertyIdList;
			Assert.That((list.Length + 1) == length);
			fib.Dispose();
		}

		[Test]
		public unsafe void RotateFlip()
		{
			FreeImageBitmap fib = new FreeImageBitmap(2, 2, PixelFormat.Format32bppArgb);

			ResetRotateBitmap(fib);
			fib.RotateFlip(RotateFlipType.RotateNoneFlipX);
			Assert.AreEqual(0x00000002, ((int*)fib.GetScanlinePointer(0))[0]);
			Assert.AreEqual(0x00000001, ((int*)fib.GetScanlinePointer(0))[1]);
			Assert.AreEqual(0x00000004, ((int*)fib.GetScanlinePointer(1))[0]);
			Assert.AreEqual(0x00000003, ((int*)fib.GetScanlinePointer(1))[1]);

			ResetRotateBitmap(fib);
			fib.RotateFlip(RotateFlipType.RotateNoneFlipY);
			Assert.AreEqual(0x00000003, ((int*)fib.GetScanlinePointer(0))[0]);
			Assert.AreEqual(0x00000004, ((int*)fib.GetScanlinePointer(0))[1]);
			Assert.AreEqual(0x00000001, ((int*)fib.GetScanlinePointer(1))[0]);
			Assert.AreEqual(0x00000002, ((int*)fib.GetScanlinePointer(1))[1]);

			ResetRotateBitmap(fib);
			fib.RotateFlip(RotateFlipType.RotateNoneFlipXY);
			Assert.AreEqual(0x00000004, ((int*)fib.GetScanlinePointer(0))[0]);
			Assert.AreEqual(0x00000003, ((int*)fib.GetScanlinePointer(0))[1]);
			Assert.AreEqual(0x00000002, ((int*)fib.GetScanlinePointer(1))[0]);
			Assert.AreEqual(0x00000001, ((int*)fib.GetScanlinePointer(1))[1]);

			ResetRotateBitmap(fib);
			fib.RotateFlip(RotateFlipType.Rotate90FlipNone);
			Assert.AreEqual(0x00000003, ((int*)fib.GetScanlinePointer(0))[0]);
			Assert.AreEqual(0x00000001, ((int*)fib.GetScanlinePointer(0))[1]);
			Assert.AreEqual(0x00000004, ((int*)fib.GetScanlinePointer(1))[0]);
			Assert.AreEqual(0x00000002, ((int*)fib.GetScanlinePointer(1))[1]);

			ResetRotateBitmap(fib);
			fib.RotateFlip(RotateFlipType.Rotate90FlipX);
			Assert.AreEqual(0x00000001, ((int*)fib.GetScanlinePointer(0))[0]);
			Assert.AreEqual(0x00000003, ((int*)fib.GetScanlinePointer(0))[1]);
			Assert.AreEqual(0x00000002, ((int*)fib.GetScanlinePointer(1))[0]);
			Assert.AreEqual(0x00000004, ((int*)fib.GetScanlinePointer(1))[1]);

			ResetRotateBitmap(fib);
			fib.RotateFlip(RotateFlipType.Rotate90FlipY);
			Assert.AreEqual(0x00000004, ((int*)fib.GetScanlinePointer(0))[0]);
			Assert.AreEqual(0x00000002, ((int*)fib.GetScanlinePointer(0))[1]);
			Assert.AreEqual(0x00000003, ((int*)fib.GetScanlinePointer(1))[0]);
			Assert.AreEqual(0x00000001, ((int*)fib.GetScanlinePointer(1))[1]);

			fib.Dispose();
		}

		private unsafe void ResetRotateBitmap(FreeImageBitmap fib)
		{
			((int*)fib.GetScanlinePointer(0))[0] = 0x00000001;
			((int*)fib.GetScanlinePointer(0))[1] = 0x00000002;
			((int*)fib.GetScanlinePointer(1))[0] = 0x00000003;
			((int*)fib.GetScanlinePointer(1))[1] = 0x00000004;
		}

		[Test]
		public unsafe void GetSetPixel()
		{
			Random rand = new Random();
			FreeImageBitmap fib = new FreeImageBitmap(2, 1, PixelFormat.Format1bppIndexed);
			Palette palette = fib.Palette;
			for (int i = 0; i < palette.Length; i++)
			{
				palette[i] = (uint)rand.Next(int.MinValue, int.MaxValue);
				fib.SetPixel(i, 0, palette[i]);
			}
			for (int i = 0; i < palette.Length; i++)
			{
				Assert.AreEqual(fib.GetPixel(i, 0), palette[i].Color);
			}
			fib.Dispose();

			fib = new FreeImageBitmap(16, 1, PixelFormat.Format4bppIndexed);
			palette = fib.Palette;
			for (int i = 0; i < palette.Length; i++)
			{
				palette[i] = (uint)rand.Next(int.MinValue, int.MaxValue);
				fib.SetPixel(i, 0, palette[i]);
			}
			for (int i = 0; i < palette.Length; i++)
			{
				Assert.AreEqual(fib.GetPixel(i, 0), palette[i].Color);
			}
			fib.Dispose();

			fib = new FreeImageBitmap(256, 1, PixelFormat.Format8bppIndexed);
			palette = fib.Palette;
			for (int i = 0; i < palette.Length; i++)
			{
				palette[i] = (uint)rand.Next(int.MinValue, int.MaxValue);
				fib.SetPixel(i, 0, palette[i]);
			}
			for (int i = 0; i < palette.Length; i++)
			{
				Assert.AreEqual(fib.GetPixel(i, 0), palette[i].Color);
			}
			fib.Dispose();

			fib = new FreeImageBitmap(1000, 1, PixelFormat.Format16bppRgb555);
			for (int i = 0; i < 1000; i++)
			{
				Color orgColor = Color.FromArgb(rand.Next(int.MinValue, int.MaxValue));
				fib.SetPixel(i, 0, orgColor);
				Color newColor = fib.GetPixel(i, 0);
				Assert.That(Math.Abs(orgColor.B - newColor.B) <= 8);
				Assert.That(Math.Abs(orgColor.G - newColor.G) <= 8);
				Assert.That(Math.Abs(orgColor.R - newColor.R) <= 8);
			}
			fib.Dispose();

			fib = new FreeImageBitmap(1000, 1, PixelFormat.Format24bppRgb);
			for (int i = 0; i < 1000; i++)
			{
				Color orgColor = Color.FromArgb(rand.Next(int.MinValue, int.MaxValue));
				fib.SetPixel(i, 0, orgColor);
				Color newColor = fib.GetPixel(i, 0);
				Assert.AreEqual(orgColor.B, newColor.B);
				Assert.AreEqual(orgColor.G, newColor.G);
				Assert.AreEqual(orgColor.R, newColor.R);
			}
			fib.Dispose();

			fib = new FreeImageBitmap(1000, 1, PixelFormat.Format32bppArgb);
			for (int i = 0; i < 1000; i++)
			{
				Color orgColor = Color.FromArgb(rand.Next(int.MinValue, int.MaxValue));
				fib.SetPixel(i, 0, orgColor);
				Color newColor = fib.GetPixel(i, 0);
				Assert.AreEqual(orgColor.B, newColor.B);
				Assert.AreEqual(orgColor.G, newColor.G);
				Assert.AreEqual(orgColor.R, newColor.R);
				Assert.AreEqual(orgColor.A, newColor.A);
			}
			fib.Dispose();
		}

		[Test]
		public void SaveAdd()
		{
			string filename = @"saveadd.tif";
			FreeImageBitmap fib = new FreeImageBitmap(100, 100, PixelFormat.Format24bppRgb);
			try
			{
				fib.SaveAdd();
				Assert.Fail();
			}
			catch { }
			Assert.IsFalse(File.Exists(filename));
			fib.Save(filename);
			fib.AdjustBrightness(0.3d);
			fib.SaveAdd();
			FreeImageBitmap other = new FreeImageBitmap(100, 100, PixelFormat.Format24bppRgb);
			foreach (Scanline<RGBTRIPLE> scanline in other)
			{
				for (int i = 0; i < scanline.Length; i++)
				{
					scanline[i] = new RGBTRIPLE(Color.FromArgb(0x339955));
				}
			}
			fib.SaveAdd(other);
			other.SaveAdd(filename);
			other.Dispose();
			fib.Dispose();

			fib = new FreeImageBitmap(filename);
			Assert.AreEqual(4, fib.FrameCount);
			fib.Dispose();
			File.Delete(filename);
			Assert.IsFalse(File.Exists(filename));
		}

		[Test]
		public void Clone()
		{
			FreeImageBitmap fib = new FreeImageBitmap(iManager.GetBitmapPath(ImageType.Even, ImageColorType.Type_24));
			object obj = new object();
			fib.Tag = obj;
			FreeImageBitmap clone = fib.Clone() as FreeImageBitmap;
			Assert.IsNotNull(clone);
			Assert.AreEqual(fib.Width, clone.Width);
			Assert.AreEqual(fib.Height, clone.Height);
			Assert.AreEqual(fib.ColorDepth, clone.ColorDepth);
			Assert.AreSame(fib.Tag, clone.Tag);
			Assert.AreEqual(fib.ImageFormat, clone.ImageFormat);
			clone.Dispose();
			fib.Dispose();
		}

		[Ignore("Ignoring LockBits")]
		public void LockBits()
		{
		}

		[Ignore("Ignoring UnlockBits")]
		public void UnlockBits()
		{
		}

		[Test]
		public void GetTypeConvertedInstance()
		{
			using (FreeImageBitmap fib = new FreeImageBitmap(10, 10, PixelFormat.Format8bppIndexed))
			{
				Assert.AreEqual(FREE_IMAGE_TYPE.FIT_BITMAP, fib.ImageType);
				using (FreeImageBitmap conv = fib.GetTypeConvertedInstance(FREE_IMAGE_TYPE.FIT_DOUBLE, true))
				{
					Assert.IsNotNull(conv);
					Assert.AreEqual(FREE_IMAGE_TYPE.FIT_DOUBLE, conv.ImageType);
				}
			}
		}

		[Test]
		public void GetColorConvertedInstance()
		{
			using (FreeImageBitmap fib = new FreeImageBitmap(10, 10, PixelFormat.Format32bppArgb))
			{
				Assert.AreEqual(32, fib.ColorDepth);
				using (FreeImageBitmap conv = fib.GetColorConvertedInstance(FREE_IMAGE_COLOR_DEPTH.FICD_24_BPP))
				{
					Assert.IsNotNull(conv);
					Assert.AreEqual(24, conv.ColorDepth);
				}
			}
		}

		[Test]
		public void GetScaledInstance()
		{
			using (FreeImageBitmap fib = new FreeImageBitmap(100, 80, PixelFormat.Format32bppArgb))
			{
				Assert.AreEqual(100, fib.Width);
				Assert.AreEqual(80, fib.Height);
				using (FreeImageBitmap conv = fib.GetScaledInstance(80, 60, FREE_IMAGE_FILTER.FILTER_BICUBIC))
				{
					Assert.IsNotNull(conv);
					Assert.AreEqual(80, conv.Width);
					Assert.AreEqual(60, conv.Height);
				}
			}
		}

		[Test]
		public unsafe void GetRotatedInstance()
		{
			using (FreeImageBitmap fib = new FreeImageBitmap(2, 2, PixelFormat.Format32bppArgb))
			{
				((int*)fib.GetScanlinePointer(0))[0] = 0x1;
				((int*)fib.GetScanlinePointer(0))[1] = 0x2;
				((int*)fib.GetScanlinePointer(1))[0] = 0x3;
				((int*)fib.GetScanlinePointer(1))[1] = 0x4;
				using (FreeImageBitmap conv = fib.GetRotatedInstance(90d))
				{
					Assert.IsNotNull(conv);
					Assert.AreEqual(((int*)conv.GetScanlinePointer(0))[0], 0x3);
					Assert.AreEqual(((int*)conv.GetScanlinePointer(0))[1], 0x1);
					Assert.AreEqual(((int*)conv.GetScanlinePointer(1))[0], 0x4);
					Assert.AreEqual(((int*)conv.GetScanlinePointer(1))[1], 0x2);
				}
			}
		}

		[Test]
		public void GetScanline()
		{
			FreeImageBitmap fib;

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format1bppIndexed);
			Scanline<FI1BIT> scanline1 = (Scanline<FI1BIT>)fib.GetScanline(0);
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format4bppIndexed);
			Scanline<FI4BIT> scanline2 = (Scanline<FI4BIT>)fib.GetScanline(0);
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format8bppIndexed);
			Scanline<Byte> scanline3 = (Scanline<Byte>)fib.GetScanline(0);
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format16bppRgb555);
			Scanline<FI16RGB555> scanline4 = (Scanline<FI16RGB555>)fib.GetScanline(0);
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format16bppRgb565);
			Scanline<FI16RGB565> scanline5 = (Scanline<FI16RGB565>)fib.GetScanline(0);
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format24bppRgb);
			Scanline<RGBTRIPLE> scanline6 = (Scanline<RGBTRIPLE>)fib.GetScanline(0);
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format32bppArgb);
			Scanline<RGBQUAD> scanline7 = (Scanline<RGBQUAD>)fib.GetScanline(0);
			fib.Dispose();
		}

		[Test]
		public void GetScanlines()
		{
			FreeImageBitmap fib;

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format1bppIndexed);
			IList<Scanline<FI1BIT>> scanline01 = (IList<Scanline<FI1BIT>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format4bppIndexed);
			IList<Scanline<FI4BIT>> scanline02 = (IList<Scanline<FI4BIT>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format8bppIndexed);
			IList<Scanline<Byte>> scanline03 = (IList<Scanline<Byte>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format16bppRgb555);
			IList<Scanline<FI16RGB555>> scanline04 = (IList<Scanline<FI16RGB555>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format16bppRgb565);
			IList<Scanline<FI16RGB565>> scanline05 = (IList<Scanline<FI16RGB565>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format24bppRgb);
			IList<Scanline<RGBTRIPLE>> scanline06 = (IList<Scanline<RGBTRIPLE>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, PixelFormat.Format32bppArgb);
			IList<Scanline<RGBQUAD>> scanline07 = (IList<Scanline<RGBQUAD>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, FREE_IMAGE_TYPE.FIT_COMPLEX);
			IList<Scanline<FICOMPLEX>> scanline08 = (IList<Scanline<FICOMPLEX>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, FREE_IMAGE_TYPE.FIT_DOUBLE);
			IList<Scanline<Double>> scanline09 = (IList<Scanline<Double>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, FREE_IMAGE_TYPE.FIT_FLOAT);
			IList<Scanline<Single>> scanline10 = (IList<Scanline<Single>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, FREE_IMAGE_TYPE.FIT_INT16);
			IList<Scanline<Int16>> scanline11 = (IList<Scanline<Int16>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, FREE_IMAGE_TYPE.FIT_INT32);
			IList<Scanline<Int32>> scanline12 = (IList<Scanline<Int32>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, FREE_IMAGE_TYPE.FIT_RGB16);
			IList<Scanline<FIRGB16>> scanline13 = (IList<Scanline<FIRGB16>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, FREE_IMAGE_TYPE.FIT_RGBA16);
			IList<Scanline<FIRGBA16>> scanline14 = (IList<Scanline<FIRGBA16>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, FREE_IMAGE_TYPE.FIT_RGBAF);
			IList<Scanline<FIRGBAF>> scanline15 = (IList<Scanline<FIRGBAF>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, FREE_IMAGE_TYPE.FIT_RGBF);
			IList<Scanline<FIRGBF>> scanline16 = (IList<Scanline<FIRGBF>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, FREE_IMAGE_TYPE.FIT_UINT16);
			IList<Scanline<UInt16>> scanline17 = (IList<Scanline<UInt16>>)fib.GetScanlines();
			fib.Dispose();

			fib = new FreeImageBitmap(10, 10, FREE_IMAGE_TYPE.FIT_UINT32);
			IList<Scanline<UInt32>> scanline18 = (IList<Scanline<UInt32>>)fib.GetScanlines();
			fib.Dispose();
		}

		[Test]
		public void Operators()
		{
			FreeImageBitmap fib1 = null, fib2 = null;
			Assert.IsTrue(fib1 == fib2);
			Assert.IsFalse(fib1 != fib2);
			Assert.IsTrue(fib1 == null);
			Assert.IsFalse(fib1 != null);

			fib1 = new FreeImageBitmap(10, 10, PixelFormat.Format24bppRgb);
			Assert.IsFalse(fib1 == fib2);
			Assert.IsTrue(fib1 != fib2);

			fib2 = fib1;
			fib1 = null;
			Assert.IsFalse(fib1 == fib2);
			Assert.IsTrue(fib1 != fib2);

			fib1 = new FreeImageBitmap(10, 9, PixelFormat.Format24bppRgb);
			Assert.IsFalse(fib1 == fib2);
			Assert.IsTrue(fib1 != fib2);

			fib2.Dispose();
			fib2 = fib1;

			Assert.IsTrue(fib1 == fib2);
			Assert.IsFalse(fib1 != fib2);

			fib2 = fib1.Clone() as FreeImageBitmap;
			Assert.IsTrue(fib1 == fib2);
			Assert.IsFalse(fib1 != fib2);

			fib1.Dispose();
			fib2.Dispose();
		}
	}
}