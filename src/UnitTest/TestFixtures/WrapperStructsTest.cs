using System;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using FreeImageAPI;
using FreeImageAPI.IO;
using FreeImageAPI.Metadata;
using NUnit.Framework;

namespace FreeImageNETUnitTest.TestFixtures
{
    [TestFixture]
    public class WrapperStructsTest
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
        public void FIRational()
        {
            FIRational rational1 = new FIRational();
            FIRational rational2 = new FIRational();
            FIRational rational3 = new FIRational();

            //
            // Constructors
            //

            Assert.That(rational1.Numerator == 0);
            Assert.That(rational1.Denominator == 0);

            rational1 = new FIRational(412, 33);
            Assert.That(rational1.Numerator == 412);
            Assert.That(rational1.Denominator == 33);

            rational2 = new FIRational(rational1);
            Assert.That(rational2.Numerator == 412);
            Assert.That(rational2.Denominator == 33);

            rational3 = new FIRational(5.75m);
            Assert.That(rational3.Numerator == 23);
            Assert.That(rational3.Denominator == 4);

            //
            // == !=
            //

            rational1 = new FIRational(421, 51);
            rational2 = rational1;
            Assert.That(rational1 == rational2);
            Assert.That(!(rational1 != rational2));

            rational2 = new FIRational(1, 7);
            Assert.That(rational1 != rational2);
            Assert.That(!(rational1 == rational2));

            //
            // > >= < <=
            //

            rational1 = new FIRational(51, 4);
            rational2 = new FIRational(27, 9);
            Assert.That(rational1 != rational2);
            Assert.That(rational1 > rational2);
            Assert.That(rational1 >= rational2);

            rational1 = new FIRational(-412, 4);
            Assert.That(rational1 != rational2);
            Assert.That(rational1 < rational2);
            Assert.That(rational1 <= rational2);

            //
            // + / -
            //

            rational1 = new FIRational(41, 3);
            rational2 = new FIRational(612, 412);
            rational3 = rational1 - rational2;
            Assert.That((rational3 + rational2) == rational1);

            rational1 = new FIRational(-7852, 63);
            rational2 = new FIRational(666111, -7654);
            rational3 = rational1 - rational2;
            Assert.That((rational3 + rational2) == rational1);

            rational1 = new FIRational(-513, 88);
            rational2 = new FIRational(413, 5);
            rational3 = rational1 - rational2;
            Assert.That((rational3 + rational2) == rational1);

            rational1 = new FIRational(-513, 88);
            rational2 = new FIRational(413, 5);
            rational3 = rational1 - rational2;
            Assert.That((rational3 + rational2) == rational1);

            rational1 = new FIRational(7531, 23144);
            rational2 = new FIRational(-412, 78777);
            rational3 = rational1 - rational2;
            Assert.That((rational3 + rational2) == rational1);

            rational1 = new FIRational(513, -42123);
            rational2 = new FIRational(-42, 77);
            rational3 = rational1 - rational2;
            Assert.That((rational3 + rational2) == rational1);

            rational1 = new FIRational(44, 11);
            rational1 = -rational1;
            Assert.That(rational1.Numerator == -4 && rational1.Denominator == 1);

            //
            // %
            //

            rational1 = new FIRational(23, 8);
            rational2 = new FIRational(77, 777);
            Assert.That((rational1 % rational2) == 0);

            rational2 = -rational2;
            Assert.That((rational1 % rational2) == 0);

            rational2 = new FIRational(7, 4);
            rational3 = new FIRational(9, 8);
            Assert.That((rational1 % rational2) == rational3);

            rational2 = -rational2;
            Assert.That((rational1 % rational2) == rational3);

            //
            // ~
            //

            rational1 = new FIRational(41, 77);
            rational1 = ~rational1;
            Assert.That(rational1.Numerator == 77 && rational1.Denominator == 41);

            //
            // -
            //

            rational1 = new FIRational(52, 4);
            rational1 = -rational1;
            Assert.That(rational1 < 0);

            //
            // ++ --
            //

            rational1 = new FIRational(5, 3);
            rational1++;
            rational2 = new FIRational(8, 3);
            Assert.That(rational1 == rational2);

            rational1 = new FIRational(41, -43);
            rational1++;
            Assert.That(rational1 > 0.0f);

            rational1--;
            Assert.That(rational1 == new FIRational(41, -43));

            rational1 = new FIRational(8134, 312);
            Assert.That(rational1 != 26);

            //
            // Direct assigns
            //

            rational1 = (FIRational)0.75m;
            Assert.That(rational1.Numerator == 3 && rational1.Denominator == 4);
            rational1 = (FIRational)0.33;
            Assert.That(rational1.Numerator == 33 && rational1.Denominator == 100);
            rational1 = (FIRational)62.975m;
            Assert.That(((decimal)rational1.Numerator / (decimal)rational1.Denominator) == 62.975m);
            rational1 = (FIRational)(-73.0975m);
            Assert.That(((decimal)rational1.Numerator / (decimal)rational1.Denominator) == -73.0975m);
            rational1 = (FIRational)(7m / 9m);
            Assert.That(rational1.Numerator == 7 && rational1.Denominator == 9);
            rational1 = (FIRational)(-15m / 9m);
            Assert.That(rational1.Numerator == -5 && rational1.Denominator == 3);
            rational1 = (FIRational)(0.7777m);
            Assert.That(rational1.Denominator != 9);

            //
            // Properties
            //

            rational1 = new FIRational(515, 5);
            Assert.That(rational1.IsInteger);

            rational1 = new FIRational(876, 77);
            Assert.That(rational1.Truncate() == (876 / 77));

            //
            // Special cases
            //

            rational1 = new FIRational(0, 10000);
            Assert.That(rational1 == 0m);

            rational1 = new FIRational(10000, 0);
            Assert.That(rational1 == 0f);

            rational1 = new FIRational(0, 0);
            Assert.That(rational1 == 0d);

            rational1 = new FIRational(-1, 0);
            Assert.That(rational1 == 0);

            rational1 = new FIRational(0, -1);
            Assert.That(rational1 == 0);
        }

        [Ignore("Ignoring StreamWrapper")]
        public void StreamWrapper()
        {
            string url = @"http://freeimage.sourceforge.net/images/logo.jpg";

            //
            // Non blocking
            //

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            Assert.IsNotNull(req);

            req.Timeout = 1000;
            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch
            {
                return;
            }
            Assert.IsNotNull(resp);

            Stream stream = resp.GetResponseStream();
            Assert.IsNotNull(stream);

            StreamWrapper wrapper = new StreamWrapper(stream, false);
            Assert.IsNotNull(wrapper);
            Assert.IsTrue(wrapper.CanRead && wrapper.CanSeek && !wrapper.CanWrite);

            byte[] buffer = new byte[1024 * 100];
            int read;
            int count = 0;

            do
            {
                read = wrapper.Read(buffer, count, buffer.Length - count);
                count += read;
            } while (read != 0);

            Assert.AreEqual(7972, count);
            Assert.AreEqual(7972, wrapper.Length);

            wrapper.Position = 0;
            Assert.AreEqual(0, wrapper.Position);

            byte[] test = new byte[buffer.Length];
            int countTest = 0;

            do
            {
                read = wrapper.Read(test, countTest, test.Length - countTest);
                countTest += read;
            } while (read != 0);

            Assert.AreEqual(count, countTest);

            for (int i = 0; i < countTest; i++)
                if (buffer[i] != test[i])
                    Assert.Fail();

            resp.Close();
            wrapper.Dispose();
            stream.Dispose();

            //
            // Blocking
            //

            req = (HttpWebRequest)HttpWebRequest.Create(url);
            Assert.IsNotNull(req);

            resp = (HttpWebResponse)req.GetResponse();
            Assert.IsNotNull(resp);

            stream = resp.GetResponseStream();
            Assert.IsNotNull(stream);

            wrapper = new StreamWrapper(stream, true);
            Assert.IsNotNull(wrapper);
            Assert.IsTrue(wrapper.CanRead && wrapper.CanSeek && !wrapper.CanWrite);

            buffer = new byte[1024 * 100];
            count = 0;

            count = wrapper.Read(buffer, 0, buffer.Length);
            Assert.AreEqual(7972, count);

            resp.Close();
            stream.Dispose();
            wrapper.Dispose();

            //
            // Position & Read byte
            //

            buffer = new byte[] { 0x00, 0x01, 0x02, 0xFF, 0xFE, 0xFD };
            stream = new MemoryStream(buffer);
            wrapper = new StreamWrapper(stream, false);

            Assert.That(0x00 == wrapper.ReadByte());
            Assert.That(0x01 == wrapper.ReadByte());
            Assert.That(0x02 == wrapper.ReadByte());
            Assert.That(0xFF == wrapper.ReadByte());
            Assert.That(0xFE == wrapper.ReadByte());
            Assert.That(0xFD == wrapper.ReadByte());
            Assert.That(-1 == wrapper.ReadByte());

            Assert.That(wrapper.Length == buffer.Length);

            wrapper.Seek(0, SeekOrigin.Begin);
            Assert.That(0x00 == wrapper.ReadByte());
            wrapper.Seek(3, SeekOrigin.Begin);
            Assert.That(0xFF == wrapper.ReadByte());
            wrapper.Seek(0, SeekOrigin.End);
            Assert.That(-1 == wrapper.ReadByte());
            wrapper.Seek(-2, SeekOrigin.End);
            Assert.That(0xFE == wrapper.ReadByte());
            wrapper.Seek(0, SeekOrigin.Begin);
            Assert.That(0x00 == wrapper.ReadByte());
            wrapper.Seek(2, SeekOrigin.Current);
            Assert.That(0xFF == wrapper.ReadByte());
            wrapper.Seek(1, SeekOrigin.Current);
            Assert.That(0xFD == wrapper.ReadByte());
            Assert.That(wrapper.Position != 0);
            wrapper.Reset();
            Assert.That(wrapper.Position == 0);

            wrapper.Dispose();
            stream.Position = 0;
            wrapper = new StreamWrapper(stream, false);

            wrapper.Seek(10, SeekOrigin.Begin);
            Assert.That(wrapper.Position == buffer.Length);

            wrapper.Dispose();
            stream.Dispose();
        }

        [Ignore("Ignoring LocalPlugin")]
        public void LocalPlugin()
        {
        }

        [Test]
        public void FreeImageStreamIO()
        {
            Random rand = new Random();
            byte[] bBuffer = new byte[256];
            IntPtr buffer = Marshal.AllocHGlobal(256);

            MemoryStream stream = new MemoryStream();
            Assert.IsNotNull(stream);
            using (fi_handle handle = new fi_handle(stream))
            {

                FreeImageIO io = FreeImageAPI.IO.FreeImageStreamIO.io;
                Assert.IsNotNull(io.readProc);
                Assert.IsNotNull(io.writeProc);
                Assert.IsNotNull(io.seekProc);
                Assert.IsNotNull(io.tellProc);

                //
                // Procs
                //

                rand.NextBytes(bBuffer);

                stream.Write(bBuffer, 0, bBuffer.Length);
                Assert.That(io.tellProc(handle) == stream.Position);
                Assert.That(io.seekProc(handle, 0, SeekOrigin.Begin) == 0);
                Assert.That(io.tellProc(handle) == 0);
                Assert.That(io.tellProc(handle) == stream.Position);

                // Read one block
                Assert.That(io.readProc(buffer, (uint)bBuffer.Length, 1, handle) == 1);
                for (int i = 0; i < bBuffer.Length; i++)
                    Assert.That(Marshal.ReadByte(buffer, i) == bBuffer[i]);

                Assert.That(io.tellProc(handle) == stream.Position);
                Assert.That(io.seekProc(handle, 0, SeekOrigin.Begin) == 0);
                Assert.That(io.tellProc(handle) == stream.Position);

                // Read 1 byte block
                Assert.That(io.readProc(buffer, 1, (uint)bBuffer.Length, handle) == bBuffer.Length);
                for (int i = 0; i < bBuffer.Length; i++)
                    Assert.That(Marshal.ReadByte(buffer, i) == bBuffer[i]);

                Assert.That(io.tellProc(handle) == stream.Position);
                Assert.That(io.seekProc(handle, 0, SeekOrigin.Begin) == 0);
                Assert.That(io.tellProc(handle) == stream.Position);

                rand.NextBytes(bBuffer);
                for (int i = 0; i < bBuffer.Length; i++)
                    Marshal.WriteByte(buffer, i, bBuffer[i]);

                // Write one block

                Assert.That(io.writeProc(buffer, (uint)bBuffer.Length, 1, handle) == 1);
                for (int i = 0; i < bBuffer.Length; i++)
                    Assert.That(Marshal.ReadByte(buffer, i) == bBuffer[i]);
                Assert.That(io.tellProc(handle) == stream.Position);

                Assert.That(io.seekProc(handle, 0, SeekOrigin.Begin) == 0);
                Assert.That(io.tellProc(handle) == 0);

                // write 1 byte block

                Assert.That(io.writeProc(buffer, 1, (uint)bBuffer.Length, handle) == bBuffer.Length);
                for (int i = 0; i < bBuffer.Length; i++)
                    Assert.That(Marshal.ReadByte(buffer, i) == bBuffer[i]);
                Assert.That(io.tellProc(handle) == stream.Position);

                // Seek and tell

                Assert.That(io.seekProc(handle, 0, SeekOrigin.Begin) == 0);
                Assert.That(io.tellProc(handle) == 0);

                Assert.That(io.seekProc(handle, 127, SeekOrigin.Current) == 0);
                Assert.That(io.tellProc(handle) == 127);

                Assert.That(io.seekProc(handle, 0, SeekOrigin.End) == 0);
                Assert.That(io.tellProc(handle) == 256);

                Marshal.FreeHGlobal(buffer);
                stream.Dispose();
            }
        }

        [Test]
        public void MetadataTag()
        {
            FITAG tag;
            MetadataTag metaTag;

            Random rand = new Random();
            dib = iManager.GetBitmap(ImageType.Metadata, ImageColorType.Type_01_Dither);
            Assert.IsFalse(dib.IsNull);

            Assert.That(FreeImage.GetMetadataCount(FREE_IMAGE_MDMODEL.FIMD_EXIF_MAIN, dib) > 0);

            FIMETADATA mData = FreeImage.FindFirstMetadata(FREE_IMAGE_MDMODEL.FIMD_EXIF_MAIN, dib, out tag);
            Assert.IsFalse(tag.IsNull);
            Assert.IsFalse(mData.IsNull);

            //
            // Constructors
            //

            metaTag = new MetadataTag(tag, dib);
            Assert.That(metaTag.Model == FREE_IMAGE_MDMODEL.FIMD_EXIF_MAIN);

            metaTag = new MetadataTag(tag, FREE_IMAGE_MDMODEL.FIMD_EXIF_MAIN);
            Assert.That(metaTag.Model == FREE_IMAGE_MDMODEL.FIMD_EXIF_MAIN);

            //
            // Properties
            //

            metaTag.ID = ushort.MinValue;
            Assert.That(metaTag.ID == ushort.MinValue);

            metaTag.ID = ushort.MaxValue;
            Assert.That(metaTag.ID == ushort.MaxValue);

            metaTag.ID = ushort.MaxValue / 2;
            Assert.That(metaTag.ID == ushort.MaxValue / 2);

            metaTag.Description = "";
            Assert.That(metaTag.Description == "");

            metaTag.Description = "A";
            Assert.That(metaTag.Description == "A");

            metaTag.Description = "ABCDEFG";
            Assert.That(metaTag.Description == "ABCDEFG");

            metaTag.Key = "";
            Assert.That(metaTag.Key == "");

            metaTag.Key = "A";
            Assert.That(metaTag.Key == "A");

            metaTag.Key = "ABCDEFG";
            Assert.That(metaTag.Key == "ABCDEFG");

            //
            // SetValue
            //

            try
            {
                metaTag.SetValue(null, FREE_IMAGE_MDTYPE.FIDT_ASCII);
                Assert.Fail();
            }
            catch
            {
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_ASCII
            //

            string testString = "";

            Assert.IsTrue(metaTag.SetValue(testString, FREE_IMAGE_MDTYPE.FIDT_ASCII));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((string)metaTag.Value).Length == 0);

            testString = "X";

            Assert.IsTrue(metaTag.SetValue(testString, FREE_IMAGE_MDTYPE.FIDT_ASCII));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((string)metaTag.Value).Length == testString.Length);
            Assert.That(((string)metaTag.Value) == testString);

            testString = "TEST-STRING";

            Assert.IsTrue(metaTag.SetValue(testString, FREE_IMAGE_MDTYPE.FIDT_ASCII));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((string)metaTag.Value).Length == testString.Length);
            Assert.That(((string)metaTag.Value) == testString);

            //
            // FREE_IMAGE_MDTYPE.FIDT_BYTE
            //			

            byte testByte;
            byte[] testByteArray;

            Assert.IsTrue(metaTag.SetValue(byte.MinValue, FREE_IMAGE_MDTYPE.FIDT_BYTE));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((byte[])metaTag.Value).Length == 1);
            Assert.That(((byte[])metaTag.Value)[0] == byte.MinValue);

            Assert.IsTrue(metaTag.SetValue(byte.MaxValue, FREE_IMAGE_MDTYPE.FIDT_BYTE));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((byte[])metaTag.Value).Length == 1);
            Assert.That(((byte[])metaTag.Value)[0] == byte.MaxValue);

            for (int i = 0; i < 10; i++)
            {
                testByte = (byte)rand.Next(byte.MinValue, byte.MaxValue);
                testByteArray = new byte[rand.Next(0, 31)];

                for (int j = 0; j < testByteArray.Length; j++)
                    testByteArray[j] = (byte)rand.Next();

                Assert.IsTrue(metaTag.SetValue(testByte, FREE_IMAGE_MDTYPE.FIDT_BYTE));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((byte[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 1);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_BYTE);
                Assert.That(((byte[])metaTag.Value)[0] == testByte);

                Assert.IsTrue(metaTag.SetValue(testByteArray, FREE_IMAGE_MDTYPE.FIDT_BYTE));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((byte[])metaTag.Value).Length == testByteArray.Length);
                Assert.That(metaTag.Count == testByteArray.Length);
                Assert.That(metaTag.Length == testByteArray.Length * 1);

                byte[] value = (byte[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testByteArray[j] == value[j]);
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_DOUBLE
            //

            double testDouble;
            double[] testDoubleArray;

            Assert.IsTrue(metaTag.SetValue(double.MinValue, FREE_IMAGE_MDTYPE.FIDT_DOUBLE));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((double[])metaTag.Value).Length == 1);
            Assert.That(((double[])metaTag.Value)[0] == double.MinValue);

            Assert.IsTrue(metaTag.SetValue(double.MaxValue, FREE_IMAGE_MDTYPE.FIDT_DOUBLE));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((double[])metaTag.Value).Length == 1);
            Assert.That(((double[])metaTag.Value)[0] == double.MaxValue);

            for (int i = 0; i < 10; i++)
            {
                testDouble = (double)rand.NextDouble();
                testDoubleArray = new double[rand.Next(0, 31)];

                for (int j = 0; j < testDoubleArray.Length; j++)
                    testDoubleArray[j] = rand.NextDouble();

                Assert.IsTrue(metaTag.SetValue(testDouble, FREE_IMAGE_MDTYPE.FIDT_DOUBLE));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((double[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 8);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_DOUBLE);
                Assert.That(((double[])metaTag.Value)[0] == testDouble);

                Assert.IsTrue(metaTag.SetValue(testDoubleArray, FREE_IMAGE_MDTYPE.FIDT_DOUBLE));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((double[])metaTag.Value).Length == testDoubleArray.Length);
                Assert.That(metaTag.Count == testDoubleArray.Length);
                Assert.That(metaTag.Length == testDoubleArray.Length * 8);

                double[] value = (double[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testDoubleArray[j] == value[j]);
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_FLOAT
            //

            float testfloat;
            float[] testFloatArray;

            Assert.IsTrue(metaTag.SetValue(float.MinValue, FREE_IMAGE_MDTYPE.FIDT_FLOAT));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((float[])metaTag.Value).Length == 1);
            Assert.That(((float[])metaTag.Value)[0] == float.MinValue);

            Assert.IsTrue(metaTag.SetValue(float.MaxValue, FREE_IMAGE_MDTYPE.FIDT_FLOAT));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((float[])metaTag.Value).Length == 1);
            Assert.That(((float[])metaTag.Value)[0] == float.MaxValue);

            for (int i = 0; i < 10; i++)
            {
                testfloat = (float)rand.NextDouble();
                testFloatArray = new float[rand.Next(0, 31)];

                for (int j = 0; j < testFloatArray.Length; j++)
                    testFloatArray[j] = (float)rand.NextDouble();

                Assert.IsTrue(metaTag.SetValue(testfloat, FREE_IMAGE_MDTYPE.FIDT_FLOAT));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((float[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 4);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_FLOAT);
                Assert.That(((float[])metaTag.Value)[0] == testfloat);

                Assert.IsTrue(metaTag.SetValue(testFloatArray, FREE_IMAGE_MDTYPE.FIDT_FLOAT));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((float[])metaTag.Value).Length == testFloatArray.Length);
                Assert.That(metaTag.Count == testFloatArray.Length);
                Assert.That(metaTag.Length == testFloatArray.Length * 4);

                float[] value = (float[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testFloatArray[j] == value[j]);
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_IFD
            //

            uint testUint;
            uint[] testUintArray;

            Assert.IsTrue(metaTag.SetValue(uint.MinValue, FREE_IMAGE_MDTYPE.FIDT_IFD));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((uint[])metaTag.Value).Length == 1);
            Assert.That(((uint[])metaTag.Value)[0] == uint.MinValue);

            Assert.IsTrue(metaTag.SetValue(uint.MaxValue, FREE_IMAGE_MDTYPE.FIDT_IFD));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((uint[])metaTag.Value).Length == 1);
            Assert.That(((uint[])metaTag.Value)[0] == uint.MaxValue);

            for (int i = 0; i < 10; i++)
            {
                testUint = (uint)rand.NextDouble();
                testUintArray = new uint[rand.Next(0, 31)];

                for (int j = 0; j < testUintArray.Length; j++)
                    testUintArray[j] = (uint)rand.Next();

                Assert.IsTrue(metaTag.SetValue(testUint, FREE_IMAGE_MDTYPE.FIDT_IFD));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((uint[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 4);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_IFD);
                Assert.That(((uint[])metaTag.Value)[0] == testUint);

                Assert.IsTrue(metaTag.SetValue(testUintArray, FREE_IMAGE_MDTYPE.FIDT_IFD));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((uint[])metaTag.Value).Length == testUintArray.Length);
                Assert.That(metaTag.Count == testUintArray.Length);
                Assert.That(metaTag.Length == testUintArray.Length * 4);

                uint[] value = (uint[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testUintArray[j] == value[j]);
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_LONG
            //

            Assert.IsTrue(metaTag.SetValue(uint.MinValue, FREE_IMAGE_MDTYPE.FIDT_LONG));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((uint[])metaTag.Value).Length == 1);
            Assert.That(((uint[])metaTag.Value)[0] == uint.MinValue);

            Assert.IsTrue(metaTag.SetValue(uint.MaxValue, FREE_IMAGE_MDTYPE.FIDT_LONG));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((uint[])metaTag.Value).Length == 1);
            Assert.That(((uint[])metaTag.Value)[0] == uint.MaxValue);

            for (int i = 0; i < 10; i++)
            {
                testUint = (uint)rand.NextDouble();
                testUintArray = new uint[rand.Next(0, 31)];

                for (int j = 0; j < testUintArray.Length; j++)
                    testUintArray[j] = (uint)rand.Next();

                Assert.IsTrue(metaTag.SetValue(testUint, FREE_IMAGE_MDTYPE.FIDT_LONG));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((uint[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 4);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_LONG);
                Assert.That(((uint[])metaTag.Value)[0] == testUint);

                Assert.IsTrue(metaTag.SetValue(testUintArray, FREE_IMAGE_MDTYPE.FIDT_LONG));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((uint[])metaTag.Value).Length == testUintArray.Length);
                Assert.That(metaTag.Count == testUintArray.Length);
                Assert.That(metaTag.Length == testUintArray.Length * 4);

                uint[] value = (uint[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testUintArray[j] == value[j]);
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_NOTYPE
            //

            try
            {
                metaTag.SetValue(new object(), FREE_IMAGE_MDTYPE.FIDT_NOTYPE);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_PALETTE
            //

            RGBQUAD testRGBQUAD;
            RGBQUAD[] testRGBQUADArray;

            for (int i = 0; i < 10; i++)
            {
                testRGBQUAD = new RGBQUAD(Color.FromArgb(rand.Next()));
                testRGBQUADArray = new RGBQUAD[rand.Next(0, 31)];

                for (int j = 0; j < testRGBQUADArray.Length; j++)
                    testRGBQUADArray[j] = new RGBQUAD(Color.FromArgb(rand.Next()));

                Assert.IsTrue(metaTag.SetValue(testRGBQUAD, FREE_IMAGE_MDTYPE.FIDT_PALETTE));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((RGBQUAD[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 4);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_PALETTE);
                Assert.That(((RGBQUAD[])metaTag.Value)[0] == testRGBQUAD);

                Assert.IsTrue(metaTag.SetValue(testRGBQUADArray, FREE_IMAGE_MDTYPE.FIDT_PALETTE));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((RGBQUAD[])metaTag.Value).Length == testRGBQUADArray.Length);
                Assert.That(metaTag.Count == testRGBQUADArray.Length);
                Assert.That(metaTag.Length == testRGBQUADArray.Length * 4);

                RGBQUAD[] value = (RGBQUAD[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testRGBQUADArray[j] == value[j]);
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_RATIONAL
            //

            FIURational testFIURational;
            FIURational[] testFIURationalArray;

            for (int i = 0; i < 10; i++)
            {
                testFIURational = new FIURational((uint)rand.Next(), (uint)rand.Next());
                testFIURationalArray = new FIURational[rand.Next(0, 31)];

                for (int j = 0; j < testFIURationalArray.Length; j++)
                    testFIURationalArray[j] = new FIURational((uint)rand.Next(), (uint)rand.Next());

                Assert.IsTrue(metaTag.SetValue(testFIURational, FREE_IMAGE_MDTYPE.FIDT_RATIONAL));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((FIURational[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 8);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_RATIONAL);
                Assert.That(((FIURational[])metaTag.Value)[0] == testFIURational);

                Assert.IsTrue(metaTag.SetValue(testFIURationalArray, FREE_IMAGE_MDTYPE.FIDT_RATIONAL));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((FIURational[])metaTag.Value).Length == testFIURationalArray.Length);
                Assert.That(metaTag.Count == testFIURationalArray.Length);
                Assert.That(metaTag.Length == testFIURationalArray.Length * 8);

                FIURational[] value = (FIURational[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testFIURationalArray[j] == value[j]);
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_SBYTE
            //

            sbyte testSByte;
            sbyte[] testSByteArray;

            Assert.IsTrue(metaTag.SetValue(sbyte.MinValue, FREE_IMAGE_MDTYPE.FIDT_SBYTE));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((sbyte[])metaTag.Value).Length == 1);
            Assert.That(((sbyte[])metaTag.Value)[0] == sbyte.MinValue);

            Assert.IsTrue(metaTag.SetValue(sbyte.MaxValue, FREE_IMAGE_MDTYPE.FIDT_SBYTE));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((sbyte[])metaTag.Value).Length == 1);
            Assert.That(((sbyte[])metaTag.Value)[0] == sbyte.MaxValue);

            for (int i = 0; i < 10; i++)
            {
                testSByte = (sbyte)rand.Next(sbyte.MinValue, sbyte.MaxValue);
                testSByteArray = new sbyte[rand.Next(0, 31)];

                for (int j = 0; j < testSByteArray.Length; j++)
                    testSByteArray[j] = (sbyte)rand.Next();

                Assert.IsTrue(metaTag.SetValue(testSByte, FREE_IMAGE_MDTYPE.FIDT_SBYTE));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((sbyte[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 1);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_SBYTE);
                Assert.That(((sbyte[])metaTag.Value)[0] == testSByte);

                Assert.IsTrue(metaTag.SetValue(testSByteArray, FREE_IMAGE_MDTYPE.FIDT_SBYTE));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((sbyte[])metaTag.Value).Length == testSByteArray.Length);
                Assert.That(metaTag.Count == testSByteArray.Length);
                Assert.That(metaTag.Length == testSByteArray.Length * 1);

                sbyte[] value = (sbyte[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testSByteArray[j] == value[j]);
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_SHORT
            //

            ushort testUShort;
            ushort[] testUShortArray;

            Assert.IsTrue(metaTag.SetValue(ushort.MinValue, FREE_IMAGE_MDTYPE.FIDT_SHORT));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((ushort[])metaTag.Value).Length == 1);
            Assert.That(((ushort[])metaTag.Value)[0] == ushort.MinValue);

            Assert.IsTrue(metaTag.SetValue(ushort.MaxValue, FREE_IMAGE_MDTYPE.FIDT_SHORT));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((ushort[])metaTag.Value).Length == 1);
            Assert.That(((ushort[])metaTag.Value)[0] == ushort.MaxValue);

            for (int i = 0; i < 10; i++)
            {
                testUShort = (ushort)rand.Next(ushort.MinValue, sbyte.MaxValue);
                testUShortArray = new ushort[rand.Next(0, 31)];

                for (int j = 0; j < testUShortArray.Length; j++)
                    testUShortArray[j] = (ushort)rand.Next();

                Assert.IsTrue(metaTag.SetValue(testUShort, FREE_IMAGE_MDTYPE.FIDT_SHORT));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((ushort[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 2);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_SHORT);
                Assert.That(((ushort[])metaTag.Value)[0] == testUShort);

                Assert.IsTrue(metaTag.SetValue(testUShortArray, FREE_IMAGE_MDTYPE.FIDT_SHORT));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((ushort[])metaTag.Value).Length == testUShortArray.Length);
                Assert.That(metaTag.Count == testUShortArray.Length);
                Assert.That(metaTag.Length == testUShortArray.Length * 2);

                ushort[] value = (ushort[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testUShortArray[j] == value[j]);
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_SLONG
            //

            int testInt;
            int[] testIntArray;

            Assert.IsTrue(metaTag.SetValue(int.MinValue, FREE_IMAGE_MDTYPE.FIDT_SLONG));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((int[])metaTag.Value).Length == 1);
            Assert.That(((int[])metaTag.Value)[0] == int.MinValue);

            Assert.IsTrue(metaTag.SetValue(int.MaxValue, FREE_IMAGE_MDTYPE.FIDT_SLONG));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((int[])metaTag.Value).Length == 1);
            Assert.That(((int[])metaTag.Value)[0] == int.MaxValue);

            for (int i = 0; i < 10; i++)
            {
                testInt = (int)rand.NextDouble();
                testIntArray = new int[rand.Next(0, 31)];

                for (int j = 0; j < testIntArray.Length; j++)
                    testIntArray[j] = rand.Next();

                Assert.IsTrue(metaTag.SetValue(testInt, FREE_IMAGE_MDTYPE.FIDT_SLONG));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((int[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 4);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_SLONG);
                Assert.That(((int[])metaTag.Value)[0] == testInt);

                Assert.IsTrue(metaTag.SetValue(testIntArray, FREE_IMAGE_MDTYPE.FIDT_SLONG));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((int[])metaTag.Value).Length == testIntArray.Length);
                Assert.That(metaTag.Count == testIntArray.Length);
                Assert.That(metaTag.Length == testIntArray.Length * 4);

                int[] value = (int[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testIntArray[j] == value[j]);
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_SRATIONAL
            //

            FIRational testFIRational;
            FIRational[] testFIRationalArray;

            for (int i = 0; i < 10; i++)
            {
                testFIRational = new FIRational(rand.Next(), rand.Next());
                testFIRationalArray = new FIRational[rand.Next(0, 31)];

                for (int j = 0; j < testFIRationalArray.Length; j++)
                    testFIRationalArray[j] = new FIRational(rand.Next(), rand.Next());

                Assert.IsTrue(metaTag.SetValue(testFIRational, FREE_IMAGE_MDTYPE.FIDT_SRATIONAL));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((FIRational[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 8);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_SRATIONAL);
                Assert.That(((FIRational[])metaTag.Value)[0] == testFIRational);

                Assert.IsTrue(metaTag.SetValue(testFIRationalArray, FREE_IMAGE_MDTYPE.FIDT_SRATIONAL));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((FIRational[])metaTag.Value).Length == testFIRationalArray.Length);
                Assert.That(metaTag.Count == testFIRationalArray.Length);
                Assert.That(metaTag.Length == testFIRationalArray.Length * 8);

                FIRational[] value = (FIRational[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testFIRationalArray[j] == value[j]);
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_SSHORT
            //

            short testShort;
            short[] testShortArray;

            Assert.IsTrue(metaTag.SetValue(short.MinValue, FREE_IMAGE_MDTYPE.FIDT_SSHORT));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((short[])metaTag.Value).Length == 1);
            Assert.That(((short[])metaTag.Value)[0] == short.MinValue);

            Assert.IsTrue(metaTag.SetValue(short.MaxValue, FREE_IMAGE_MDTYPE.FIDT_SSHORT));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((short[])metaTag.Value).Length == 1);
            Assert.That(((short[])metaTag.Value)[0] == short.MaxValue);

            for (int i = 0; i < 10; i++)
            {
                testShort = (short)rand.Next(short.MinValue, short.MaxValue);
                testShortArray = new short[rand.Next(0, 31)];

                for (int j = 0; j < testShortArray.Length; j++)
                    testShortArray[j] = (short)rand.Next();

                Assert.IsTrue(metaTag.SetValue(testShort, FREE_IMAGE_MDTYPE.FIDT_SSHORT));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((short[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 2);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_SSHORT);
                Assert.That(((short[])metaTag.Value)[0] == testShort);

                Assert.IsTrue(metaTag.SetValue(testShortArray, FREE_IMAGE_MDTYPE.FIDT_SSHORT));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((short[])metaTag.Value).Length == testShortArray.Length);
                Assert.That(metaTag.Count == testShortArray.Length);
                Assert.That(metaTag.Length == testShortArray.Length * 2);

                short[] value = (short[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testShortArray[j] == value[j]);
            }

            //
            // FREE_IMAGE_MDTYPE.FIDT_UNDEFINED
            //

            Assert.IsTrue(metaTag.SetValue(byte.MinValue, FREE_IMAGE_MDTYPE.FIDT_UNDEFINED));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((byte[])metaTag.Value).Length == 1);
            Assert.That(((byte[])metaTag.Value)[0] == byte.MinValue);

            Assert.IsTrue(metaTag.SetValue(byte.MaxValue, FREE_IMAGE_MDTYPE.FIDT_UNDEFINED));
            Assert.IsNotNull(metaTag.Value);
            Assert.That(((byte[])metaTag.Value).Length == 1);
            Assert.That(((byte[])metaTag.Value)[0] == byte.MaxValue);

            for (int i = 0; i < 10; i++)
            {
                testByte = (byte)rand.Next(byte.MinValue, byte.MaxValue);
                testByteArray = new byte[rand.Next(0, 31)];

                for (int j = 0; j < testByteArray.Length; j++)
                    testByteArray[j] = (byte)rand.Next();

                Assert.IsTrue(metaTag.SetValue(testByte, FREE_IMAGE_MDTYPE.FIDT_UNDEFINED));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((byte[])metaTag.Value).Length == 1);
                Assert.That(metaTag.Count == 1);
                Assert.That(metaTag.Length == 1);
                Assert.That(metaTag.Type == FREE_IMAGE_MDTYPE.FIDT_UNDEFINED);
                Assert.That(((byte[])metaTag.Value)[0] == testByte);

                Assert.IsTrue(metaTag.SetValue(testByteArray, FREE_IMAGE_MDTYPE.FIDT_UNDEFINED));
                Assert.IsNotNull(metaTag.Value);
                Assert.That(((byte[])metaTag.Value).Length == testByteArray.Length);
                Assert.That(metaTag.Count == testByteArray.Length);
                Assert.That(metaTag.Length == testByteArray.Length * 1);

                byte[] value = (byte[])metaTag.Value;

                for (int j = 0; j < value.Length; j++)
                    Assert.That(testByteArray[j] == value[j]);
            }

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void MetadataModel()
        {
            MetadataTag tag;
            dib = FreeImage.Allocate(1, 1, 1, 0, 0, 0);
            Assert.IsFalse(dib.IsNull);

            MetadataModel model = new MDM_GEOTIFF(dib);
            Assert.AreEqual(0, model.Count);
            Assert.IsFalse(model.Exists);
            Assert.IsEmpty(model.List);
            Assert.AreEqual(model.Model, FREE_IMAGE_MDMODEL.FIMD_GEOTIFF);
            Assert.IsTrue(model.DestoryModel());
            foreach (MetadataTag m in model) Assert.Fail();

            tag = new MetadataTag(FREE_IMAGE_MDMODEL.FIMD_GEOTIFF);
            tag.Key = "KEY";
            tag.Value = 54321f;
            Assert.IsTrue(model.AddTag(tag));

            Assert.AreEqual(1, model.Count);
            Assert.IsTrue(model.Exists);
            Assert.IsNotEmpty(model.List);
            Assert.AreEqual(model.Model, FREE_IMAGE_MDMODEL.FIMD_GEOTIFF);

            Assert.IsTrue(model.DestoryModel());
            Assert.AreEqual(0, model.Count);
            Assert.IsFalse(model.Exists);
            Assert.IsEmpty(model.List);
            Assert.AreEqual(model.Model, FREE_IMAGE_MDMODEL.FIMD_GEOTIFF);

            FreeImage.UnloadEx(ref dib);
        }

        [Test]
        public void ImageMetadata()
        {
            ImageMetadata metadata;
            List<MetadataModel> modelList;
            MetadataTag tag = new MetadataTag(FREE_IMAGE_MDMODEL.FIMD_COMMENTS);
            tag.Key = "KEY";
            tag.ID = 11;
            tag.Value = new double[] { 0d, 41d, -523d, -0.41d };

            dib = FreeImage.Allocate(1, 1, 1, 1, 0, 0);
            Assert.IsFalse(dib.IsNull);

            metadata = new ImageMetadata(dib, true);
            Assert.AreEqual(0, metadata.Count);
            Assert.IsTrue(metadata.HideEmptyModels);
            Assert.IsEmpty(metadata.List);

            metadata = new ImageMetadata(dib, false);
            Assert.AreEqual(FreeImage.FREE_IMAGE_MDMODELS.Length, metadata.Count);
            Assert.IsFalse(metadata.HideEmptyModels);
            Assert.IsNotEmpty(metadata.List);

            metadata.HideEmptyModels = true;
            metadata.AddTag(tag);

            Assert.AreEqual(1, metadata.Count);
            Assert.IsNotEmpty(metadata.List);

            modelList = metadata.List;
            Assert.AreEqual(FREE_IMAGE_MDMODEL.FIMD_COMMENTS, modelList[0].Model);

            System.Collections.IEnumerator enumerator = metadata.GetEnumerator();
            Assert.IsTrue(enumerator.MoveNext());
            Assert.IsNotNull((MetadataModel)enumerator.Current);
            Assert.IsFalse(enumerator.MoveNext());

            FreeImage.UnloadEx(ref dib);
        }
    }
}