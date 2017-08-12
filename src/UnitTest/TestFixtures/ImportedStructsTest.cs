using System;
using System.Drawing;
using FreeImageAPI;
using FreeImageAPI.IO;
using NUnit.Framework;

namespace FreeImageNETUnitTest.TestFixtures
{
    [TestFixture]
    public class ImportedStructsTest
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

        [Test]
        public void RGBQUAD()
        {
            RGBQUAD rgbq = new RGBQUAD();
            Assert.AreEqual(0, rgbq.rgbBlue);
            Assert.AreEqual(0, rgbq.rgbGreen);
            Assert.AreEqual(0, rgbq.rgbRed);
            Assert.AreEqual(0, rgbq.rgbReserved);

            rgbq = new RGBQUAD(Color.Chartreuse);
            Assert.That(EqualColors(Color.Chartreuse, rgbq.Color));

            rgbq = new RGBQUAD(Color.FromArgb(133, 83, 95, 173));
            Assert.AreEqual(173, rgbq.rgbBlue);
            Assert.AreEqual(95, rgbq.rgbGreen);
            Assert.AreEqual(83, rgbq.rgbRed);
            Assert.AreEqual(133, rgbq.rgbReserved);

            rgbq.Color = Color.Crimson;
            Assert.That(EqualColors(Color.Crimson, rgbq.Color));

            rgbq.Color = Color.MidnightBlue;
            Assert.That(EqualColors(Color.MidnightBlue, rgbq.Color));

            rgbq.Color = Color.White;
            Assert.AreEqual(255, rgbq.rgbBlue);
            Assert.AreEqual(255, rgbq.rgbGreen);
            Assert.AreEqual(255, rgbq.rgbRed);
            Assert.AreEqual(255, rgbq.rgbReserved);

            rgbq.Color = Color.Black;
            Assert.AreEqual(0, rgbq.rgbBlue);
            Assert.AreEqual(0, rgbq.rgbGreen);
            Assert.AreEqual(0, rgbq.rgbRed);
            Assert.AreEqual(255, rgbq.rgbReserved);

            rgbq = Color.DarkGoldenrod;
            Color color = rgbq;
            Assert.That(EqualColors(Color.DarkGoldenrod, color));
        }

        [Test]
        public void RGBTRIPLE()
        {
            RGBTRIPLE rgbt = new RGBTRIPLE();
            Assert.AreEqual(0, rgbt.rgbtBlue);
            Assert.AreEqual(0, rgbt.rgbtGreen);
            Assert.AreEqual(0, rgbt.rgbtRed);

            rgbt = new RGBTRIPLE(Color.Chartreuse);
            Assert.That(EqualColors(Color.Chartreuse, rgbt.Color));

            rgbt = new RGBTRIPLE(Color.FromArgb(133, 83, 95, 173));
            Assert.AreEqual(173, rgbt.rgbtBlue);
            Assert.AreEqual(95, rgbt.rgbtGreen);
            Assert.AreEqual(83, rgbt.rgbtRed);

            rgbt.Color = Color.Crimson;
            Assert.That(EqualColors(Color.Crimson, rgbt.Color));

            rgbt.Color = Color.MidnightBlue;
            Assert.That(EqualColors(Color.MidnightBlue, rgbt.Color));

            rgbt.Color = Color.White;
            Assert.AreEqual(255, rgbt.rgbtBlue);
            Assert.AreEqual(255, rgbt.rgbtGreen);
            Assert.AreEqual(255, rgbt.rgbtRed);

            rgbt.Color = Color.Black;
            Assert.AreEqual(0, rgbt.rgbtBlue);
            Assert.AreEqual(0, rgbt.rgbtGreen);
            Assert.AreEqual(0, rgbt.rgbtRed);

            rgbt = Color.DarkGoldenrod;
            Color color = rgbt;
            Assert.That(EqualColors(Color.DarkGoldenrod, color));
        }

        [Test]
        public void FIRGB16()
        {
            FIRGB16 rgb = new FIRGB16();
            Assert.AreEqual(0 * 256, rgb.blue);
            Assert.AreEqual(0 * 256, rgb.green);
            Assert.AreEqual(0 * 256, rgb.red);

            rgb = new FIRGB16(Color.Chartreuse);
            Assert.That(EqualColors(Color.Chartreuse, rgb.Color));

            rgb = new FIRGB16(Color.FromArgb(133, 83, 95, 173));
            Assert.AreEqual(173 * 256, rgb.blue);
            Assert.AreEqual(95 * 256, rgb.green);
            Assert.AreEqual(83 * 256, rgb.red);

            rgb.Color = Color.Crimson;
            Assert.That(EqualColors(Color.Crimson, rgb.Color));

            rgb.Color = Color.MidnightBlue;
            Assert.That(EqualColors(Color.MidnightBlue, rgb.Color));

            rgb.Color = Color.White;
            Assert.AreEqual(255 * 256, rgb.blue);
            Assert.AreEqual(255 * 256, rgb.green);
            Assert.AreEqual(255 * 256, rgb.red);

            rgb.Color = Color.Black;
            Assert.AreEqual(0 * 256, rgb.blue);
            Assert.AreEqual(0 * 256, rgb.green);
            Assert.AreEqual(0 * 256, rgb.red);

            rgb = Color.DarkGoldenrod;
            Color color = rgb;
            Assert.That(EqualColors(Color.DarkGoldenrod, color));
        }

        [Test]
        public void FIRGBA16()
        {
            FIRGBA16 rgb = new FIRGBA16();
            Assert.AreEqual(0 * 256, rgb.blue);
            Assert.AreEqual(0 * 256, rgb.green);
            Assert.AreEqual(0 * 256, rgb.red);
            Assert.AreEqual(0 * 256, rgb.alpha);

            rgb = new FIRGBA16(Color.Chartreuse);
            Assert.That(EqualColors(Color.Chartreuse, rgb.Color));

            rgb = new FIRGBA16(Color.FromArgb(133, 83, 95, 173));
            Assert.AreEqual(173 * 256, rgb.blue);
            Assert.AreEqual(95 * 256, rgb.green);
            Assert.AreEqual(83 * 256, rgb.red);
            Assert.AreEqual(133 * 256, rgb.alpha);

            rgb.Color = Color.Crimson;
            Assert.That(EqualColors(Color.Crimson, rgb.Color));

            rgb.Color = Color.MidnightBlue;
            Assert.That(EqualColors(Color.MidnightBlue, rgb.Color));

            rgb.Color = Color.White;
            Assert.AreEqual(255 * 256, rgb.blue);
            Assert.AreEqual(255 * 256, rgb.green);
            Assert.AreEqual(255 * 256, rgb.red);
            Assert.AreEqual(255 * 256, rgb.alpha);

            rgb.Color = Color.Black;
            Assert.AreEqual(0 * 256, rgb.blue);
            Assert.AreEqual(0 * 256, rgb.green);
            Assert.AreEqual(0 * 256, rgb.red);
            Assert.AreEqual(255 * 256, rgb.alpha);

            rgb = Color.DarkGoldenrod;
            Color color = rgb;
            Assert.That(EqualColors(Color.DarkGoldenrod, color));
        }

        [Test]
        public void FIRGBF()
        {
            FIRGBF rgb = new FIRGBF();
            Assert.AreEqual(0 / 255f, rgb.blue);
            Assert.AreEqual(0 / 255f, rgb.green);
            Assert.AreEqual(0 / 255f, rgb.red);

            rgb = new FIRGBF(Color.Chartreuse);
            Assert.That(EqualColors(Color.Chartreuse, rgb.Color));

            rgb = new FIRGBF(Color.FromArgb(133, 83, 95, 173));
            Assert.AreEqual(173 / 255f, rgb.blue);
            Assert.AreEqual(95 / 255f, rgb.green);
            Assert.AreEqual(83 / 255f, rgb.red);

            rgb.Color = Color.Crimson;
            Assert.That(EqualColors(Color.Crimson, rgb.Color));

            rgb.Color = Color.MidnightBlue;
            Assert.That(EqualColors(Color.MidnightBlue, rgb.Color));

            rgb.Color = Color.White;
            Assert.AreEqual(255 / 255f, rgb.blue);
            Assert.AreEqual(255 / 255f, rgb.green);
            Assert.AreEqual(255 / 255f, rgb.red);

            rgb.Color = Color.Black;
            Assert.AreEqual(0 / 255f, rgb.blue);
            Assert.AreEqual(0 / 255f, rgb.green);
            Assert.AreEqual(0 / 255f, rgb.red);

            rgb = Color.DarkGoldenrod;
            Color color = rgb;
            Assert.That(EqualColors(Color.DarkGoldenrod, color));
        }

        [Test]
        public void FIRGBAF()
        {
            FIRGBAF rgb = new FIRGBAF();
            Assert.AreEqual(0 / 255f, rgb.blue);
            Assert.AreEqual(0 / 255f, rgb.green);
            Assert.AreEqual(0 / 255f, rgb.red);
            Assert.AreEqual(0 / 255f, rgb.alpha);

            rgb = new FIRGBAF(Color.Chartreuse);
            Assert.That(EqualColors(Color.Chartreuse, rgb.Color));

            rgb = new FIRGBAF(Color.FromArgb(133, 83, 95, 173));
            Assert.AreEqual(173 / 255f, rgb.blue);
            Assert.AreEqual(95 / 255f, rgb.green);
            Assert.AreEqual(83 / 255f, rgb.red);
            Assert.AreEqual(133 / 255f, rgb.alpha);

            rgb.Color = Color.Crimson;
            Assert.That(EqualColors(Color.Crimson, rgb.Color));

            rgb.Color = Color.MidnightBlue;
            Assert.That(EqualColors(Color.MidnightBlue, rgb.Color));

            rgb.Color = Color.White;
            Assert.AreEqual(255 / 255f, rgb.blue);
            Assert.AreEqual(255 / 255f, rgb.green);
            Assert.AreEqual(255 / 255f, rgb.red);
            Assert.AreEqual(255 / 255f, rgb.alpha);

            rgb.Color = Color.Black;
            Assert.AreEqual(0 / 255f, rgb.blue);
            Assert.AreEqual(0 / 255f, rgb.green);
            Assert.AreEqual(0 / 255f, rgb.red);
            Assert.AreEqual(255 / 255f, rgb.alpha);

            rgb = Color.DarkGoldenrod;
            Color color = rgb;
            Assert.That(EqualColors(Color.DarkGoldenrod, color));
        }

        [Ignore("Ignoring FICOMPLEX")]
        public void FICOMPLEX()
        {
        }

        [Test]
        public void FIBITMAP()
        {
            FIBITMAP var = new FIBITMAP();
            Assert.IsTrue(var.IsNull);
        }

        [Test]
        public void fi_handle()
        {
            fi_handle var = new fi_handle();
            Assert.IsTrue(var.IsNull);

            string test = "hello word!";
            using (var = new fi_handle(test))
            {
                Assert.IsFalse(var.IsNull);

                object obj = var.GetObject();
                Assert.That(obj is string);
                Assert.AreSame(obj, test);
            }
        }

        [Test]
        public void FIICCPROFILE()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            FIICCPROFILE var = new FIICCPROFILE();
            Assert.AreEqual(0, var.Data.Length);
            Assert.AreEqual(IntPtr.Zero, var.DataPointer);
            Assert.AreEqual(0, var.Size);

            dib = iManager.GetBitmap(ImageType.Odd, ImageColorType.Type_24);
            Assert.That(!dib.IsNull);

            byte[] data = new byte[512];
            rand.NextBytes(data);

            var = FreeImage.GetICCProfileEx(dib);
            Assert.AreEqual(0, var.Size);

            var = new FIICCPROFILE(dib, data, 256);
            Assert.AreEqual(256, var.Data.Length);
            Assert.AreNotEqual(IntPtr.Zero, var.DataPointer);
            Assert.AreEqual(256, var.Size);
            byte[] dataComp = var.Data;
            for (int i = 0; i < data.Length && i < dataComp.Length; i++)
                if (data[i] != dataComp[i])
                    Assert.Fail();

            FreeImage.DestroyICCProfile(dib);
            var = FreeImage.GetICCProfileEx(dib);
            Assert.AreEqual(0, var.Size);

            var = new FIICCPROFILE(dib, data);
            Assert.AreEqual(512, var.Data.Length);
            Assert.AreNotEqual(IntPtr.Zero, var.DataPointer);
            Assert.AreEqual(512, var.Size);
            dataComp = var.Data;
            for (int i = 0; i < data.Length && i < dataComp.Length; i++)
                if (data[i] != dataComp[i])
                    Assert.Fail();

            var = FreeImage.GetICCProfileEx(dib);
            Assert.AreEqual(512, var.Data.Length);
            Assert.AreNotEqual(IntPtr.Zero, var.DataPointer);
            Assert.AreEqual(512, var.Size);

            FreeImage.DestroyICCProfile(dib);
            var = FreeImage.GetICCProfileEx(dib);
            Assert.AreEqual(0, var.Size);

            FreeImage.UnloadEx(ref dib);
        }
    }
}