using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace FreeImageNETUnitTest
{
	internal enum ImageType : byte
	{
		Even,
		Odd,
		JPEG,
		Metadata,
		Multipaged
	}

	internal enum ImageColorType : byte
	{
		Type_01_Dither,
		Type_01_Threshold,
		Type_04,
		Type_04_Greyscale_MinIsBlack,
		Type_04_Greyscale_Unordered,
		Type_08,
		Type_08_Greyscale_MinIsBlack,
		Type_08_Greyscale_Unordered,
		Type_16_555,
		Type_16_565,
		Type_24,
		Type_32,
	}

	internal class ImageManager
	{
		public readonly string baseDirectory = null;

		public ImageManager()
			: this(GetImageDataFolder())
		{
		}

		public ImageManager(string baseDirectory)
		{
			if (false == Directory.Exists(baseDirectory))
			{
				throw new DirectoryNotFoundException();
			}

			this.baseDirectory = baseDirectory;
		}

		public string GetBitmapPath(ImageType type, ImageColorType colorType)
		{
			string filename = null;

			switch (type)
			{
				case ImageType.Even:
					switch (colorType)
					{
						case ImageColorType.Type_01_Dither:
							filename = Path.Combine(baseDirectory, "Even", "Image_01_dither.tif");
							break;
						case ImageColorType.Type_01_Threshold:
							filename = Path.Combine(baseDirectory, "Even", "Image_01_threshold.tif");
							break;
						case ImageColorType.Type_04:
							filename = Path.Combine(baseDirectory, "Even", "Image_04.tif");
							break;
						case ImageColorType.Type_04_Greyscale_MinIsBlack:
							filename = Path.Combine(baseDirectory, "Even", "Image_04_gs_minisblack.tif");
							break;
						case ImageColorType.Type_04_Greyscale_Unordered:
							filename = Path.Combine(baseDirectory, "Even", "Image_04_gs_unordered.tif");
							break;
						case ImageColorType.Type_08:
							filename = Path.Combine(baseDirectory, "Even", "Image_08.tif");
							break;
						case ImageColorType.Type_08_Greyscale_MinIsBlack:
							filename = Path.Combine(baseDirectory, "Even", "Image_08_gs_minisblack.tif");
							break;
						case ImageColorType.Type_08_Greyscale_Unordered:
							filename = Path.Combine(baseDirectory, "Even", "Image_08_gs_unordered.tif");
							break;
						case ImageColorType.Type_16_555:
							filename = Path.Combine(baseDirectory, "Even", "Image_16_555.bmp");
							break;
						case ImageColorType.Type_16_565:
							filename = Path.Combine(baseDirectory, "Even", "Image_16_565.bmp");
							break;
						case ImageColorType.Type_24:
							filename = Path.Combine(baseDirectory, "Even", "Image_24.tif");
							break;
						case ImageColorType.Type_32:
							filename = Path.Combine(baseDirectory, "Even", "Image_32.tif");
							break;
					}
					break;
				case ImageType.Odd:
					switch (colorType)
					{
						case ImageColorType.Type_01_Dither:
							filename = Path.Combine(baseDirectory, "Odd", "Image_01_dither.tif");
							break;
						case ImageColorType.Type_01_Threshold:
							filename = Path.Combine(baseDirectory, "Odd", "Image_01_threshold.tif");
							break;
						case ImageColorType.Type_04:
							filename = Path.Combine(baseDirectory, "Odd", "Image_04.tif");
							break;
						case ImageColorType.Type_04_Greyscale_MinIsBlack:
							filename = Path.Combine(baseDirectory, "Odd", "Image_04_gs_minisblack.tif");
							break;
						case ImageColorType.Type_04_Greyscale_Unordered:
							filename = Path.Combine(baseDirectory, "Odd", "Image_04_gs_unordered.tif");
							break;
						case ImageColorType.Type_08:
							filename = Path.Combine(baseDirectory, "Odd", "Image_08.tif");
							break;
						case ImageColorType.Type_08_Greyscale_MinIsBlack:
							filename = Path.Combine(baseDirectory, "Odd", "Image_08_gs_minisblack.tif");
							break;
						case ImageColorType.Type_08_Greyscale_Unordered:
							filename = Path.Combine(baseDirectory, "Odd", "Image_08_gs_unordered.tif");
							break;
						case ImageColorType.Type_16_555:
							filename = Path.Combine(baseDirectory, "Odd", "Image_16_555.bmp");
							break;
						case ImageColorType.Type_16_565:
							filename = Path.Combine(baseDirectory, "Odd", "Image_16_565.bmp");
							break;
						case ImageColorType.Type_24:
							filename = Path.Combine(baseDirectory, "Odd", "Image_24.tif");
							break;
					}
					break;
				case ImageType.JPEG:
					filename = Path.Combine(baseDirectory, "JPEG", "Image.jpg");
					break;
				case ImageType.Metadata:
					filename = Path.Combine(baseDirectory, "Metadata", "exif.jpg");
					break;
				case ImageType.Multipaged:
					filename = Path.Combine(baseDirectory, "Multipaged", "Image.tif");
					break;
			}
			return filename;
		}

		public FreeImageAPI.FIBITMAP GetBitmap(ImageType type, ImageColorType colorType)
		{
			FreeImageAPI.FIBITMAP result = new FreeImageAPI.FIBITMAP();
			string filename = GetBitmapPath(type, colorType);
			if (!String.IsNullOrEmpty(filename) && File.Exists(filename))
				result = FreeImageAPI.FreeImage.LoadEx(filename);
			return result;
		}

		private static string GetImageDataFolder()
		{			
			string solutionFolder = Utility.GetSolutionFolder();
			string imagesFolder = Path.Combine(solutionFolder, "UnitTestData", "Images");

			return Path.GetFullPath(imagesFolder);
		}
	}
}