// ==========================================================
// FreeImage 3 .NET wrapper
// Original FreeImage 3 functions and .NET compatible derived functions
//
// Design and implementation by
// - Jean-Philippe Goerke (jpgoerke@users.sourceforge.net)
// - Carsten Klein (cklein05@users.sourceforge.net)
//
// Contributors:
// - David Boland (davidboland@vodafone.ie)
//
// Main reference : MSDN Knowlede Base
//
// This file is part of FreeImage 3
//
// COVERED CODE IS PROVIDED UNDER THIS LICENSE ON AN "AS IS" BASIS, WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING, WITHOUT LIMITATION, WARRANTIES
// THAT THE COVERED CODE IS FREE OF DEFECTS, MERCHANTABLE, FIT FOR A PARTICULAR PURPOSE
// OR NON-INFRINGING. THE ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE COVERED
// CODE IS WITH YOU. SHOULD ANY COVERED CODE PROVE DEFECTIVE IN ANY RESPECT, YOU (NOT
// THE INITIAL DEVELOPER OR ANY OTHER CONTRIBUTOR) ASSUME THE COST OF ANY NECESSARY
// SERVICING, REPAIR OR CORRECTION. THIS DISCLAIMER OF WARRANTY CONSTITUTES AN ESSENTIAL
// PART OF THIS LICENSE. NO USE OF ANY COVERED CODE IS AUTHORIZED HEREUNDER EXCEPT UNDER
// THIS DISCLAIMER.
//
// Use at your own risk!
// ==========================================================

// ==========================================================
// CVS
// $Revision: 1.9 $
// $Date: 2009/09/15 11:41:37 $
// $Id: FreeImageStaticImports.cs,v 1.9 2009/09/15 11:41:37 cklein05 Exp $
// ==========================================================

using System;
using System.Runtime.InteropServices;
using FreeImageAPI.Plugins;
using FreeImageAPI.IO;

namespace FreeImageAPI
{
    public static partial class FreeImage
    {
        #region Constants

        private static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// Filename of the FreeImage library.
        /// </summary>
        private const string FreeImageLibrary = "FreeImage";

        /// <summary>
        /// Number of bytes to shift left within a 4 byte block.
        /// </summary>
        public const int FI_RGBA_RED = 2;

        /// <summary>
        /// Number of bytes to shift left within a 4 byte block.
        /// </summary>
        public const int FI_RGBA_GREEN = 1;

        /// <summary>
        /// Number of bytes to shift left within a 4 byte block.
        /// </summary>
        public const int FI_RGBA_BLUE = 0;

        /// <summary>
        /// Number of bytes to shift left within a 4 byte block.
        /// </summary>
        public const int FI_RGBA_ALPHA = 3;

        /// <summary>
        /// Mask indicating the position of the given color.
        /// </summary>
        public const uint FI_RGBA_RED_MASK = 0x00FF0000;

        /// <summary>
        /// Mask indicating the position of the given color.
        /// </summary>
        public const uint FI_RGBA_GREEN_MASK = 0x0000FF00;

        /// <summary>
        /// Mask indicating the position of the given color.
        /// </summary>
        public const uint FI_RGBA_BLUE_MASK = 0x000000FF;

        /// <summary>
        /// Mask indicating the position of the given color.
        /// </summary>
        public const uint FI_RGBA_ALPHA_MASK = 0xFF000000;

        /// <summary>
        /// Number of bits to shift left within a 32 bit block.
        /// </summary>
        public const int FI_RGBA_RED_SHIFT = 16;

        /// <summary>
        /// Number of bits to shift left within a 32 bit block.
        /// </summary>
        public const int FI_RGBA_GREEN_SHIFT = 8;

        /// <summary>
        /// Number of bits to shift left within a 32 bit block.
        /// </summary>
        public const int FI_RGBA_BLUE_SHIFT = 0;

        /// <summary>
        /// Number of bits to shift left within a 32 bit block.
        /// </summary>
        public const int FI_RGBA_ALPHA_SHIFT = 24;

        /// <summary>
        /// Mask indicating the position of color components of a 32 bit color.
        /// </summary>
        public const uint FI_RGBA_RGB_MASK = (FI_RGBA_RED_MASK | FI_RGBA_GREEN_MASK | FI_RGBA_BLUE_MASK);

        /// <summary>
        /// Mask indicating the position of the given color.
        /// </summary>
        public const int FI16_555_RED_MASK = 0x7C00;

        /// <summary>
        /// Mask indicating the position of the given color.
        /// </summary>
        public const int FI16_555_GREEN_MASK = 0x03E0;

        /// <summary>
        /// Mask indicating the position of the given color.
        /// </summary>
        public const int FI16_555_BLUE_MASK = 0x001F;

        /// <summary>
        /// Number of bits to shift left within a 16 bit block.
        /// </summary>
        public const int FI16_555_RED_SHIFT = 10;

        /// <summary>
        /// Number of bits to shift left within a 16 bit block.
        /// </summary>
        public const int FI16_555_GREEN_SHIFT = 5;

        /// <summary>
        /// Number of bits to shift left within a 16 bit block.
        /// </summary>
        public const int FI16_555_BLUE_SHIFT = 0;

        /// <summary>
        /// Mask indicating the position of the given color.
        /// </summary>
        public const int FI16_565_RED_MASK = 0xF800;

        /// <summary>
        /// Mask indicating the position of the given color.
        /// </summary>
        public const int FI16_565_GREEN_MASK = 0x07E0;

        /// <summary>
        /// Mask indicating the position of the given color.
        /// </summary>
        public const int FI16_565_BLUE_MASK = 0x001F;

        /// <summary>
        /// Number of bits to shift left within a 16 bit block.
        /// </summary>
        public const int FI16_565_RED_SHIFT = 11;

        /// <summary>
        /// Number of bits to shift left within a 16 bit block.
        /// </summary>
        public const int FI16_565_GREEN_SHIFT = 5;

        /// <summary>
        /// Number of bits to shift left within a 16 bit block.
        /// </summary>
        public const int FI16_565_BLUE_SHIFT = 0;

        #endregion

        #region General functions

        /// <summary>
        /// Initialises the library.
        /// </summary>
        /// <param name="load_local_plugins_only">
        /// When the <paramref name="load_local_plugins_only"/> is true, FreeImage won't make use of al plugins.
        /// </param>
        private static void Initialise(bool load_local_plugins_only)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.Initialise(load_local_plugins_only);
            else
                FreeImageStaticImportsLinux.Initialise(load_local_plugins_only);
        }

        /// <summary>
        /// Deinitialises the library.
        /// </summary>
        private static void DeInitialise()
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.DeInitialise();
            else
                FreeImageStaticImportsLinux.DeInitialise();
        }

        /// <summary>
        /// Returns a string containing the current version of the library.
        /// </summary>
        /// <returns>The current version of the library.</returns>
        public static unsafe string GetVersion() { return PtrToStr(GetVersion_()); }
        private static unsafe byte* GetVersion_()
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetVersion_();
            else
                return FreeImageStaticImportsLinux.GetVersion_();
        }

        /// <summary>
        /// Returns a string containing a standard copyright message.
        /// </summary>
        /// <returns>A standard copyright message.</returns>
        public static unsafe string GetCopyrightMessage() { return PtrToStr(GetCopyrightMessage_()); }
        private static unsafe byte* GetCopyrightMessage_()
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetCopyrightMessage_();
            else
                return FreeImageStaticImportsLinux.GetCopyrightMessage_();
        }

        /// <summary>
        /// Calls the set error message function in FreeImage.
        /// </summary>
        /// <param name="fif">Format of the bitmaps.</param>
        /// <param name="message">The error message.</param>
        public static void OutputMessageProc(FREE_IMAGE_FORMAT fif, string message)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.OutputMessageProc(fif, message);
            else
                FreeImageStaticImportsLinux.OutputMessageProc(fif, message);
        }

        /// <summary>
        /// You use the function FreeImage_SetOutputMessage to capture the log string
        /// so that you can show it to the user of the program.
        /// The callback is implemented in the <see cref="FreeImageEngine.Message"/> event of this class.
        /// </summary>
        /// <remarks>The function is private because FreeImage can only have a single
        /// callback function. To use the callback use the <see cref="FreeImageEngine.Message"/>
        /// event of this class.</remarks>
        /// <param name="omf">Handler to the callback function.</param>
        internal static void SetOutputMessage(OutputMessageFunction omf)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.SetOutputMessage(omf);
            else
                FreeImageStaticImportsLinux.SetOutputMessage(omf);
        }

        #endregion

        #region Bitmap management functions

        /// <summary>
        /// Creates a new bitmap in memory.
        /// </summary>
        /// <param name="width">Width of the new bitmap.</param>
        /// <param name="height">Height of the new bitmap.</param>
        /// <param name="bpp">Bit depth of the new Bitmap.
        /// Supported pixel depth: 1-, 4-, 8-, 16-, 24-, 32-bit per pixel for standard bitmap</param>
        /// <param name="red_mask">Red part of the color layout.
        /// eg: 0xFF0000</param>
        /// <param name="green_mask">Green part of the color layout.
        /// eg: 0x00FF00</param>
        /// <param name="blue_mask">Blue part of the color layout.
        /// eg: 0x0000FF</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP Allocate(int width, int height, int bpp,
            uint red_mask, uint green_mask, uint blue_mask)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Allocate(width, height, bpp, red_mask, green_mask, blue_mask);
            else
                return FreeImageStaticImportsLinux.Allocate(width, height, bpp, red_mask, green_mask, blue_mask);
        }

        /// <summary>
        /// Creates a new bitmap in memory.
        /// </summary>
        /// <param name="type">Type of the image.</param>
        /// <param name="width">Width of the new bitmap.</param>
        /// <param name="height">Height of the new bitmap.</param>
        /// <param name="bpp">Bit depth of the new Bitmap.
        /// Supported pixel depth: 1-, 4-, 8-, 16-, 24-, 32-bit per pixel for standard bitmap</param>
        /// <param name="red_mask">Red part of the color layout.
        /// eg: 0xFF0000</param>
        /// <param name="green_mask">Green part of the color layout.
        /// eg: 0x00FF00</param>
        /// <param name="blue_mask">Blue part of the color layout.
        /// eg: 0x0000FF</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP AllocateT(FREE_IMAGE_TYPE type, int width, int height, int bpp,
            uint red_mask, uint green_mask, uint blue_mask)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.AllocateT(type, width, height, bpp, red_mask, green_mask, blue_mask);
            else
                return FreeImageStaticImportsLinux.AllocateT(type, width, height, bpp, red_mask, green_mask, blue_mask);
        }

        internal static FIBITMAP AllocateEx(int width, int height, int bpp,
            IntPtr color, FREE_IMAGE_COLOR_OPTIONS options, RGBQUAD[] palette,
            uint red_mask, uint green_mask, uint blue_mask)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.AllocateEx(width, height, bpp, color, options, palette, red_mask, green_mask, blue_mask);
            else
                return FreeImageStaticImportsLinux.AllocateEx(width, height, bpp, color, options, palette, red_mask, green_mask, blue_mask);
        }

        internal static FIBITMAP AllocateExT(FREE_IMAGE_TYPE type, int width, int height, int bpp,
            IntPtr color, FREE_IMAGE_COLOR_OPTIONS options, RGBQUAD[] palette,
            uint red_mask, uint green_mask, uint blue_mask)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.AllocateExT(type, width, height, bpp, color, options, palette, red_mask, green_mask, blue_mask);
            else
                return FreeImageStaticImportsLinux.AllocateExT(type, width, height, bpp, color, options, palette, red_mask, green_mask, blue_mask);
        }

        /// <summary>
        /// Makes an exact reproduction of an existing bitmap, including metadata and attached profile if any.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP Clone(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Clone(dib);
            else
                return FreeImageStaticImportsLinux.Clone(dib);
        }

        /// <summary>
        /// Deletes a previously loaded FIBITMAP from memory.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        public static void Unload(FIBITMAP dib)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.Unload(dib);
            else
                FreeImageStaticImportsLinux.Unload(dib);
        }

        /// <summary>
        /// Decodes a bitmap, allocates memory for it and returns it as a FIBITMAP.
        /// </summary>
        /// <param name="fif">Type of the bitmap.</param>
        /// <param name="filename">Name of the file to decode.</param>
        /// <param name="flags">Flags to enable or disable plugin-features.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP Load(FREE_IMAGE_FORMAT fif, string filename, FREE_IMAGE_LOAD_FLAGS flags)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Load(fif, filename, flags);
            else
                return FreeImageStaticImportsLinux.Load(fif, filename, flags);
        }

        /// <summary>
        /// Decodes a bitmap, allocates memory for it and returns it as a FIBITMAP.
        /// The filename supports UNICODE.
        /// </summary>
        /// <param name="fif">Type of the bitmap.</param>
        /// <param name="filename">Name of the file to decode.</param>
        /// <param name="flags">Flags to enable or disable plugin-features.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        private static FIBITMAP LoadU(FREE_IMAGE_FORMAT fif, string filename, FREE_IMAGE_LOAD_FLAGS flags)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.LoadU(fif, filename, flags);
            else
                return FreeImageStaticImportsLinux.LoadU(fif, filename, flags);
        }

        /// <summary>
        /// Loads a bitmap from an arbitrary source.
        /// </summary>
        /// <param name="fif">Type of the bitmap.</param>
        /// <param name="io">A FreeImageIO structure with functionpointers to handle the source.</param>
        /// <param name="handle">A handle to the source.</param>
        /// <param name="flags">Flags to enable or disable plugin-features.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP LoadFromHandle(FREE_IMAGE_FORMAT fif, ref FreeImageIO io, fi_handle handle, FREE_IMAGE_LOAD_FLAGS flags)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.LoadFromHandle(fif, ref io, handle, flags);
            else
                return FreeImageStaticImportsLinux.LoadFromHandle(fif, ref io, handle, flags);
        }

        /// <summary>
        /// Saves a previosly loaded FIBITMAP to a file.
        /// </summary>
        /// <param name="fif">Type of the bitmap.</param>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="filename">Name of the file to save to.</param>
        /// <param name="flags">Flags to enable or disable plugin-features.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool Save(FREE_IMAGE_FORMAT fif, FIBITMAP dib, string filename, FREE_IMAGE_SAVE_FLAGS flags)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Save(fif, dib, filename, flags);
            else
                return FreeImageStaticImportsLinux.Save(fif, dib, filename, flags);
        }

        /// <summary>
        /// Saves a previosly loaded FIBITMAP to a file.
        /// The filename supports UNICODE.
        /// </summary>
        /// <param name="fif">Type of the bitmap.</param>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="filename">Name of the file to save to.</param>
        /// <param name="flags">Flags to enable or disable plugin-features.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        private static bool SaveU(FREE_IMAGE_FORMAT fif, FIBITMAP dib, string filename, FREE_IMAGE_SAVE_FLAGS flags)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SaveU(fif, dib, filename, flags);
            else
                return FreeImageStaticImportsLinux.SaveU(fif, dib, filename, flags);
        }

        /// <summary>
        /// Saves a bitmap to an arbitrary source.
        /// </summary>
        /// <param name="fif">Type of the bitmap.</param>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="io">A FreeImageIO structure with functionpointers to handle the source.</param>
        /// <param name="handle">A handle to the source.</param>
        /// <param name="flags">Flags to enable or disable plugin-features.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SaveToHandle(FREE_IMAGE_FORMAT fif, FIBITMAP dib, ref FreeImageIO io, fi_handle handle,
                                        FREE_IMAGE_SAVE_FLAGS flags)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SaveToHandle(fif, dib, ref io, handle, flags);
            else
                return FreeImageStaticImportsLinux.SaveToHandle(fif, dib, ref io, handle, flags);
        }

        #endregion

        #region Memory I/O streams

        /// <summary>
        /// Open a memory stream.
        /// </summary>
        /// <param name="data">Pointer to the data in memory.</param>
        /// <param name="size_in_bytes">Length of the data in byte.</param>
        /// <returns>Handle to a memory stream.</returns>
        public static FIMEMORY OpenMemory(IntPtr data, uint size_in_bytes)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.OpenMemory(data, size_in_bytes);
            else
                return FreeImageStaticImportsLinux.OpenMemory(data, size_in_bytes);
        }

        internal static FIMEMORY OpenMemoryEx(byte[] data, uint size_in_bytes)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.OpenMemoryEx(data, size_in_bytes);
            else
                return FreeImageStaticImportsLinux.OpenMemoryEx(data, size_in_bytes);
        }

        /// <summary>
        /// Close and free a memory stream.
        /// </summary>
        /// <param name="stream">Handle to a memory stream.</param>
        public static void CloseMemory(FIMEMORY stream)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.CloseMemory(stream);
            else
                FreeImageStaticImportsLinux.CloseMemory(stream);
        }

        /// <summary>
        /// Decodes a bitmap from a stream, allocates memory for it and returns it as a FIBITMAP.
        /// </summary>
        /// <param name="fif">Type of the bitmap.</param>
        /// <param name="stream">Handle to a memory stream.</param>
        /// <param name="flags">Flags to enable or disable plugin-features.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP LoadFromMemory(FREE_IMAGE_FORMAT fif, FIMEMORY stream, FREE_IMAGE_LOAD_FLAGS flags)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.LoadFromMemory(fif, stream, flags);
            else
                return FreeImageStaticImportsLinux.LoadFromMemory(fif, stream, flags);
        }

        /// <summary>
        /// Saves a previosly loaded FIBITMAP to a stream.
        /// </summary>
        /// <param name="fif">Type of the bitmap.</param>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="stream">Handle to a memory stream.</param>
        /// <param name="flags">Flags to enable or disable plugin-features.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SaveToMemory(FREE_IMAGE_FORMAT fif, FIBITMAP dib, FIMEMORY stream, FREE_IMAGE_SAVE_FLAGS flags)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SaveToMemory(fif, dib, stream, flags);
            else
                return FreeImageStaticImportsLinux.SaveToMemory(fif, dib, stream, flags);
        }

        /// <summary>
        /// Gets the current position of a memory handle.
        /// </summary>
        /// <param name="stream">Handle to a memory stream.</param>
        /// <returns>The current file position if successful, -1 otherwise.</returns>
        public static int TellMemory(FIMEMORY stream)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.TellMemory(stream);
            else
                return FreeImageStaticImportsLinux.TellMemory(stream);
        }

        /// <summary>
        /// Moves the memory handle to a specified location.
        /// </summary>
        /// <param name="stream">Handle to a memory stream.</param>
        /// <param name="offset">Number of bytes from origin.</param>
        /// <param name="origin">Initial position.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SeekMemory(FIMEMORY stream, int offset, System.IO.SeekOrigin origin)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SeekMemory(stream, offset, origin);
            else
                return FreeImageStaticImportsLinux.SeekMemory(stream, offset, origin);
        }

        /// <summary>
        /// Provides a direct buffer access to a memory stream.
        /// </summary>
        /// <param name="stream">The target memory stream.</param>
        /// <param name="data">Pointer to the data in memory.</param>
        /// <param name="size_in_bytes">Size of the data in bytes.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool AcquireMemory(FIMEMORY stream, ref IntPtr data, ref uint size_in_bytes)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.AcquireMemory(stream, ref data, ref size_in_bytes);
            else
                return FreeImageStaticImportsLinux.AcquireMemory(stream, ref data, ref size_in_bytes);
        }

        /// <summary>
        /// Reads data from a memory stream.
        /// </summary>
        /// <param name="buffer">The buffer to store the data in.</param>
        /// <param name="size">Size in bytes of the items.</param>
        /// <param name="count">Number of items to read.</param>
        /// <param name="stream">The stream to read from.
        /// The memory pointer associated with stream is increased by the number of bytes actually read.</param>
        /// <returns>The number of full items actually read.
        /// May be less than count on error or stream-end.</returns>
        public static uint ReadMemory(byte[] buffer, uint size, uint count, FIMEMORY stream)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ReadMemory(buffer, size, count, stream);
            else
                return FreeImageStaticImportsLinux.ReadMemory(buffer, size, count, stream);
        }

        /// <summary>
        /// Writes data to a memory stream.
        /// </summary>
        /// <param name="buffer">The buffer to read the data from.</param>
        /// <param name="size">Size in bytes of the items.</param>
        /// <param name="count">Number of items to write.</param>
        /// <param name="stream">The stream to write to.
        /// The memory pointer associated with stream is increased by the number of bytes actually written.</param>
        /// <returns>The number of full items actually written.
        /// May be less than count on error or stream-end.</returns>
        public static uint WriteMemory(byte[] buffer, uint size, uint count, FIMEMORY stream)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.WriteMemory(buffer, size, count, stream);
            else
                return FreeImageStaticImportsLinux.WriteMemory(buffer, size, count, stream);
        }

        /// <summary>
        /// Open a multi-page bitmap from a memory stream.
        /// </summary>
        /// <param name="fif">Type of the bitmap.</param>
        /// <param name="stream">The stream to decode.</param>
        /// <param name="flags">Flags to enable or disable plugin-features.</param>
        /// <returns>Handle to a FreeImage multi-paged bitmap.</returns>
        public static FIMULTIBITMAP LoadMultiBitmapFromMemory(FREE_IMAGE_FORMAT fif, FIMEMORY stream, FREE_IMAGE_LOAD_FLAGS flags)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.LoadMultiBitmapFromMemory(fif, stream, flags);
            else
                return FreeImageStaticImportsLinux.LoadMultiBitmapFromMemory(fif, stream, flags);
        }

        #endregion

        #region Plugin functions

        /// <summary>
        /// Registers a new plugin to be used in FreeImage.
        /// </summary>
        /// <param name="proc_address">Pointer to the function that initialises the plugin.</param>
        /// <param name="format">A string describing the format of the plugin.</param>
        /// <param name="description">A string describing the plugin.</param>
        /// <param name="extension">A string witha comma sperated list of extensions. f.e: "pl,pl2,pl4"</param>
        /// <param name="regexpr">A regular expression used to identify the bitmap.</param>
        /// <returns>The format idientifier assigned by FreeImage.</returns>
        public static FREE_IMAGE_FORMAT RegisterLocalPlugin(InitProc proc_address,
                                                            string format, string description, string extension, string regexpr)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.RegisterLocalPlugin(proc_address, format, description, extension, regexpr);
            else
                return FreeImageStaticImportsLinux.RegisterLocalPlugin(proc_address, format, description, extension, regexpr);
        }

        /// <summary>
        /// Registers a new plugin to be used in FreeImage. The plugin is residing in a DLL.
        /// The Init function must be called “Init” and must use the stdcall calling convention.
        /// </summary>
        /// <param name="path">Complete path to the dll file hosting the plugin.</param>
        /// <param name="format">A string describing the format of the plugin.</param>
        /// <param name="description">A string describing the plugin.</param>
        /// <param name="extension">A string witha comma sperated list of extensions. f.e: "pl,pl2,pl4"</param>
        /// <param name="regexpr">A regular expression used to identify the bitmap.</param>
        /// <returns>The format idientifier assigned by FreeImage.</returns>
        public static FREE_IMAGE_FORMAT RegisterExternalPlugin(string path,
                                                               string format, string description, string extension, string regexpr)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.RegisterExternalPlugin(path, format, description, extension, regexpr);
            else
                return FreeImageStaticImportsLinux.RegisterExternalPlugin(path, format, description, extension, regexpr);
        }

        /// <summary>
        /// Retrieves the number of FREE_IMAGE_FORMAT identifiers being currently registered.
        /// </summary>
        /// <returns>The number of registered formats.</returns>
        public static int GetFIFCount()
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFIFCount();
            else
                return FreeImageStaticImportsLinux.GetFIFCount();
        }

        /// <summary>
        /// Enables or disables a plugin.
        /// </summary>
        /// <param name="fif">The plugin to enable or disable.</param>
        /// <param name="enable">True: enable the plugin. false: disable the plugin.</param>
        /// <returns>The previous state of the plugin.
        /// 1 - enabled. 0 - disables. -1 plugin does not exist.</returns>
        public static int SetPluginEnabled(FREE_IMAGE_FORMAT fif, bool enable)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetPluginEnabled(fif, enable);
            else
                return FreeImageStaticImportsLinux.SetPluginEnabled(fif, enable);
        }

        /// <summary>
        /// Retrieves the state of a plugin.
        /// </summary>
        /// <param name="fif">The plugin to check.</param>
        /// <returns>1 - enabled. 0 - disables. -1 plugin does not exist.</returns>
        public static int IsPluginEnabled(FREE_IMAGE_FORMAT fif)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.IsPluginEnabled(fif);
            else
                return FreeImageStaticImportsLinux.IsPluginEnabled(fif);
        }

        /// <summary>
        /// Returns a <see cref="FREE_IMAGE_FORMAT"/> identifier from the format string that was used to register the FIF.
        /// </summary>
        /// <param name="format">The string that was used to register the plugin.</param>
        /// <returns>A <see cref="FREE_IMAGE_FORMAT"/> identifier from the format.</returns>
        public static FREE_IMAGE_FORMAT GetFIFFromFormat(string format)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFIFFromFormat(format);
            else
                return FreeImageStaticImportsLinux.GetFIFFromFormat(format);
        }

        /// <summary>
        /// Returns a <see cref="FREE_IMAGE_FORMAT"/> identifier from a MIME content type string
        /// (MIME stands for Multipurpose Internet Mail Extension).
        /// </summary>
        /// <param name="mime">A MIME content type.</param>
        /// <returns>A <see cref="FREE_IMAGE_FORMAT"/> identifier from the MIME.</returns>
        public static FREE_IMAGE_FORMAT GetFIFFromMime(string mime)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFIFFromMime(mime);
            else
                return FreeImageStaticImportsLinux.GetFIFFromMime(mime);
        }

        /// <summary>
        /// Returns the string that was used to register a plugin from the system assigned <see cref="FREE_IMAGE_FORMAT"/>.
        /// </summary>
        /// <param name="fif">The assigned <see cref="FREE_IMAGE_FORMAT"/>.</param>
        /// <returns>The string that was used to register the plugin.</returns>
        public static unsafe string GetFormatFromFIF(FREE_IMAGE_FORMAT fif) { return PtrToStr(GetFormatFromFIF_(fif)); }
        private static unsafe byte* GetFormatFromFIF_(FREE_IMAGE_FORMAT fif)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFormatFromFIF_(fif);
            else
                return FreeImageStaticImportsLinux.GetFormatFromFIF_(fif);
        }

        /// <summary>
        /// Returns a comma-delimited file extension list describing the bitmap formats the given plugin can read and/or write.
        /// </summary>
        /// <param name="fif">The desired <see cref="FREE_IMAGE_FORMAT"/>.</param>
        /// <returns>A comma-delimited file extension list.</returns>
        public static unsafe string GetFIFExtensionList(FREE_IMAGE_FORMAT fif) { return PtrToStr(GetFIFExtensionList_(fif)); }
        private static unsafe byte* GetFIFExtensionList_(FREE_IMAGE_FORMAT fif)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFIFExtensionList_(fif);
            else
                return FreeImageStaticImportsLinux.GetFIFExtensionList_(fif);
        }

        /// <summary>
        /// Returns a descriptive string that describes the bitmap formats the given plugin can read and/or write.
        /// </summary>
        /// <param name="fif">The desired <see cref="FREE_IMAGE_FORMAT"/>.</param>
        /// <returns>A descriptive string that describes the bitmap formats.</returns>
        public static unsafe string GetFIFDescription(FREE_IMAGE_FORMAT fif) { return PtrToStr(GetFIFDescription_(fif)); }
        private static unsafe byte* GetFIFDescription_(FREE_IMAGE_FORMAT fif)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFIFDescription_(fif);
            else
                return FreeImageStaticImportsLinux.GetFIFDescription_(fif);
        }

        /// <summary>
        /// Returns a regular expression string that can be used by a regular expression engine to identify the bitmap.
        /// FreeImageQt makes use of this function.
        /// </summary>
        /// <param name="fif">The desired <see cref="FREE_IMAGE_FORMAT"/>.</param>
        /// <returns>A regular expression string.</returns>
        public static unsafe string GetFIFRegExpr(FREE_IMAGE_FORMAT fif) { return PtrToStr(GetFIFRegExpr_(fif)); }
        private static unsafe byte* GetFIFRegExpr_(FREE_IMAGE_FORMAT fif)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFIFRegExpr_(fif);
            else
                return FreeImageStaticImportsLinux.GetFIFRegExpr_(fif);
        }

        /// <summary>
        /// Given a <see cref="FREE_IMAGE_FORMAT"/> identifier, returns a MIME content type string (MIME stands for Multipurpose Internet Mail Extension).
        /// </summary>
        /// <param name="fif">The desired <see cref="FREE_IMAGE_FORMAT"/>.</param>
        /// <returns>A MIME content type string.</returns>
        public static unsafe string GetFIFMimeType(FREE_IMAGE_FORMAT fif) { return PtrToStr(GetFIFMimeType_(fif)); }
        private static unsafe byte* GetFIFMimeType_(FREE_IMAGE_FORMAT fif)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFIFMimeType_(fif);
            else
                return FreeImageStaticImportsLinux.GetFIFMimeType_(fif);
        }

        /// <summary>
        /// This function takes a filename or a file-extension and returns the plugin that can
        /// read/write files with that extension in the form of a <see cref="FREE_IMAGE_FORMAT"/> identifier.
        /// </summary>
        /// <param name="filename">The filename or -extension.</param>
        /// <returns>The <see cref="FREE_IMAGE_FORMAT"/> of the plugin.</returns>
        public static FREE_IMAGE_FORMAT GetFIFFromFilename(string filename)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFIFFromFilename(filename);
            else
                return FreeImageStaticImportsLinux.GetFIFFromFilename(filename);
        }

        /// <summary>
        /// This function takes a filename or a file-extension and returns the plugin that can
        /// read/write files with that extension in the form of a <see cref="FREE_IMAGE_FORMAT"/> identifier.
        /// Supports UNICODE filenames.
        /// </summary>
        /// <param name="filename">The filename or -extension.</param>
        /// <returns>The <see cref="FREE_IMAGE_FORMAT"/> of the plugin.</returns>
        private static FREE_IMAGE_FORMAT GetFIFFromFilenameU(string filename)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFIFFromFilenameU(filename);
            else
                return FreeImageStaticImportsLinux.GetFIFFromFilenameU(filename);
        }

        /// <summary>
        /// Checks if a plugin can load bitmaps.
        /// </summary>
        /// <param name="fif">The <see cref="FREE_IMAGE_FORMAT"/> of the plugin.</param>
        /// <returns>True if the plugin can load bitmaps, else false.</returns>
        public static bool FIFSupportsReading(FREE_IMAGE_FORMAT fif)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.FIFSupportsReading(fif);
            else
                return FreeImageStaticImportsLinux.FIFSupportsReading(fif);
        }

        /// <summary>
        /// Checks if a plugin can save bitmaps.
        /// </summary>
        /// <param name="fif">The <see cref="FREE_IMAGE_FORMAT"/> of the plugin.</param>
        /// <returns>True if the plugin can save bitmaps, else false.</returns>
        public static bool FIFSupportsWriting(FREE_IMAGE_FORMAT fif)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.FIFSupportsWriting(fif);
            else
                return FreeImageStaticImportsLinux.FIFSupportsWriting(fif);
        }

        /// <summary>
        /// Checks if a plugin can save bitmaps in the desired bit depth.
        /// </summary>
        /// <param name="fif">The <see cref="FREE_IMAGE_FORMAT"/> of the plugin.</param>
        /// <param name="bpp">The desired bit depth.</param>
        /// <returns>True if the plugin can save bitmaps in the desired bit depth, else false.</returns>
        public static bool FIFSupportsExportBPP(FREE_IMAGE_FORMAT fif, int bpp)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.FIFSupportsExportBPP(fif, bpp);
            else
                return FreeImageStaticImportsLinux.FIFSupportsExportBPP(fif, bpp);
        }

        /// <summary>
        /// Checks if a plugin can save a bitmap in the desired data type.
        /// </summary>
        /// <param name="fif">The <see cref="FREE_IMAGE_FORMAT"/> of the plugin.</param>
        /// <param name="type">The desired image type.</param>
        /// <returns>True if the plugin can save bitmaps as the desired type, else false.</returns>
        public static bool FIFSupportsExportType(FREE_IMAGE_FORMAT fif, FREE_IMAGE_TYPE type)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.FIFSupportsExportType(fif, type);
            else
                return FreeImageStaticImportsLinux.FIFSupportsExportType(fif, type);
        }

        /// <summary>
        /// Checks if a plugin can load or save an ICC profile.
        /// </summary>
        /// <param name="fif">The <see cref="FREE_IMAGE_FORMAT"/> of the plugin.</param>
        /// <returns>True if the plugin can load or save an ICC profile, else false.</returns>
        public static bool FIFSupportsICCProfiles(FREE_IMAGE_FORMAT fif)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.FIFSupportsICCProfiles(fif);
            else
                return FreeImageStaticImportsLinux.FIFSupportsICCProfiles(fif);
        }

        #endregion

        #region Multipage functions

        /// <summary>
        /// Loads a FreeImage multi-paged bitmap.
        /// Load flags can be provided by the flags parameter.
        /// </summary>
        /// <param name="fif">Format of the image.</param>
        /// <param name="filename">The complete name of the file to load.</param>
        /// <param name="create_new">When true a new bitmap is created.</param>
        /// <param name="read_only">When true the bitmap will be loaded read only.</param>
        /// <param name="keep_cache_in_memory">When true performance is increased at the cost of memory.</param>
        /// <param name="flags">Flags to enable or disable plugin-features.</param>
        /// <returns>Handle to a FreeImage multi-paged bitmap.</returns>
        public static FIMULTIBITMAP OpenMultiBitmap(FREE_IMAGE_FORMAT fif, string filename, bool create_new,
                                                    bool read_only, bool keep_cache_in_memory, FREE_IMAGE_LOAD_FLAGS flags)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.OpenMultiBitmap(fif, filename, create_new, read_only, keep_cache_in_memory, flags);
            else
                return FreeImageStaticImportsLinux.OpenMultiBitmap(fif, filename, create_new, read_only, keep_cache_in_memory, flags);
        }

        /// <summary>
        /// Loads a FreeImage multi-pages bitmap from the specified handle
        /// using the specified functions.
        /// Load flags can be provided by the flags parameter.
        /// </summary>
        /// <param name="fif">Format of the image.</param>
        /// <param name="io">IO functions used to read from the specified handle.</param>
        /// <param name="handle">The handle to load the bitmap from.</param>
        /// <param name="flags">Flags to enable or disable plugin-features.</param>
        /// <returns>Handle to a FreeImage multi-paged bitmap.</returns>
        public static FIMULTIBITMAP OpenMultiBitmapFromHandle(FREE_IMAGE_FORMAT fif, ref FreeImageIO io,
                                                              fi_handle handle, FREE_IMAGE_LOAD_FLAGS flags)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.OpenMultiBitmapFromHandle(fif, ref io, handle, flags);
            else
                return FreeImageStaticImportsLinux.OpenMultiBitmapFromHandle(fif, ref io, handle, flags);
        }

        /// <summary>
        /// Closes a previously opened multi-page bitmap and, when the bitmap was not opened read-only, applies any changes made to it.
        /// </summary>
        /// <param name="bitmap">Handle to a FreeImage multi-paged bitmap.</param>
        /// <param name="flags">Flags to enable or disable plugin-features.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        private static bool CloseMultiBitmap_(FIMULTIBITMAP bitmap, FREE_IMAGE_SAVE_FLAGS flags)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.CloseMultiBitmap_(bitmap, flags);
            else
                return FreeImageStaticImportsLinux.CloseMultiBitmap_(bitmap, flags);
        }

        /// <summary>
        /// Returns the number of pages currently available in the multi-paged bitmap.
        /// </summary>
        /// <param name="bitmap">Handle to a FreeImage multi-paged bitmap.</param>
        /// <returns>Number of pages.</returns>
        public static int GetPageCount(FIMULTIBITMAP bitmap)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetPageCount(bitmap);
            else
                return FreeImageStaticImportsLinux.GetPageCount(bitmap);
        }

        /// <summary>
        /// Appends a new page to the end of the bitmap.
        /// </summary>
        /// <param name="bitmap">Handle to a FreeImage multi-paged bitmap.</param>
        /// <param name="data">Handle to a FreeImage bitmap.</param>
        public static void AppendPage(FIMULTIBITMAP bitmap, FIBITMAP data)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.AppendPage(bitmap, data);
            else
                FreeImageStaticImportsLinux.AppendPage(bitmap, data);
        }

        /// <summary>
        /// Inserts a new page before the given position in the bitmap.
        /// </summary>
        /// <param name="bitmap">Handle to a FreeImage multi-paged bitmap.</param>
        /// <param name="page">Page has to be a number smaller than the current number of pages available in the bitmap.</param>
        /// <param name="data">Handle to a FreeImage bitmap.</param>
        public static void InsertPage(FIMULTIBITMAP bitmap, int page, FIBITMAP data)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.InsertPage(bitmap, page, data);
            else
                FreeImageStaticImportsLinux.InsertPage(bitmap, page, data);
        }

        /// <summary>
        /// Deletes the page on the given position.
        /// </summary>
        /// <param name="bitmap">Handle to a FreeImage multi-paged bitmap.</param>
        /// <param name="page">Number of the page to delete.</param>
        public static void DeletePage(FIMULTIBITMAP bitmap, int page)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.DeletePage(bitmap, page);
            else
                FreeImageStaticImportsLinux.DeletePage(bitmap, page);
        }

        /// <summary>
        /// Locks a page in memory for editing.
        /// </summary>
        /// <param name="bitmap">Handle to a FreeImage multi-paged bitmap.</param>
        /// <param name="page">Number of the page to lock.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP LockPage(FIMULTIBITMAP bitmap, int page)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.LockPage(bitmap, page);
            else
                return FreeImageStaticImportsLinux.LockPage(bitmap, page);
        }

        /// <summary>
        /// Unlocks a previously locked page and gives it back to the multi-page engine.
        /// </summary>
        /// <param name="bitmap">Handle to a FreeImage multi-paged bitmap.</param>
        /// <param name="data">Handle to a FreeImage bitmap.</param>
        /// <param name="changed">If true, the page is applied to the multi-page bitmap.</param>
        public static void UnlockPage(FIMULTIBITMAP bitmap, FIBITMAP data, bool changed)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.UnlockPage(bitmap, data, changed);
            else
                FreeImageStaticImportsLinux.UnlockPage(bitmap, data, changed);
        }

        /// <summary>
        /// Moves the source page to the position of the target page.
        /// </summary>
        /// <param name="bitmap">Handle to a FreeImage multi-paged bitmap.</param>
        /// <param name="target">New position of the page.</param>
        /// <param name="source">Old position of the page.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool MovePage(FIMULTIBITMAP bitmap, int target, int source)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.MovePage(bitmap, target, source);
            else
                return FreeImageStaticImportsLinux.MovePage(bitmap, target, source);
        }

        /// <summary>
        /// Returns an array of page-numbers that are currently locked in memory.
        /// When the pages parameter is null, the size of the array is returned in the count variable.
        /// </summary>
        /// <example>
        /// <code>
        /// int[] lockedPages = null;
        /// int count = 0;
        /// GetLockedPageNumbers(dib, lockedPages, ref count);
        /// lockedPages = new int[count];
        /// GetLockedPageNumbers(dib, lockedPages, ref count);
        /// </code>
        /// </example>
        /// <param name="bitmap">Handle to a FreeImage multi-paged bitmap.</param>
        /// <param name="pages">The list of locked pages in the multi-pages bitmap.
        /// If set to null, count will contain the number of pages.</param>
        /// <param name="count">If <paramref name="pages"/> is set to null count will contain the number of locked pages.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool GetLockedPageNumbers(FIMULTIBITMAP bitmap, int[] pages, ref int count)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetLockedPageNumbers(bitmap, pages, ref count);
            else
                return FreeImageStaticImportsLinux.GetLockedPageNumbers(bitmap, pages, ref count);
        }

        #endregion

        #region Filetype functions

        /// <summary>
        /// Orders FreeImage to analyze the bitmap signature.
        /// </summary>
        /// <param name="filename">Name of the file to analyze.</param>
        /// <param name="size">Reserved parameter - use 0.</param>
        /// <returns>Type of the bitmap.</returns>
        public static FREE_IMAGE_FORMAT GetFileType(string filename, int size)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFileType(filename, size);
            else
                return FreeImageStaticImportsLinux.GetFileType(filename, size);
        }


        /// <summary>
        /// Orders FreeImage to analyze the bitmap signature.
        /// Supports UNICODE filenames.
        /// </summary>
        /// <param name="filename">Name of the file to analyze.</param>
        /// <param name="size">Reserved parameter - use 0.</param>
        /// <returns>Type of the bitmap.</returns>
        private static FREE_IMAGE_FORMAT GetFileTypeU(string filename, int size)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFileTypeU(filename, size);
            else
                return FreeImageStaticImportsLinux.GetFileTypeU(filename, size);
        }

        /// <summary>
        /// Uses the <see cref="FreeImageIO"/> structure as described in the topic bitmap management functions
        /// to identify a bitmap type.
        /// </summary>
        /// <param name="io">A <see cref="FreeImageIO"/> structure with functionpointers to handle the source.</param>
        /// <param name="handle">A handle to the source.</param>
        /// <param name="size">Size in bytes of the source.</param>
        /// <returns>Type of the bitmap.</returns>
        public static FREE_IMAGE_FORMAT GetFileTypeFromHandle(ref FreeImageIO io, fi_handle handle, int size)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFileTypeFromHandle(ref io, handle, size);
            else
                return FreeImageStaticImportsLinux.GetFileTypeFromHandle(ref io, handle, size);
        }

        /// <summary>
        /// Uses a memory handle to identify a bitmap type.
        /// </summary>
        /// <param name="stream">Pointer to the stream.</param>
        /// <param name="size">Size in bytes of the source.</param>
        /// <returns>Type of the bitmap.</returns>
        public static FREE_IMAGE_FORMAT GetFileTypeFromMemory(FIMEMORY stream, int size)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetFileTypeFromMemory(stream, size);
            else
                return FreeImageStaticImportsLinux.GetFileTypeFromMemory(stream, size);
        }

        #endregion

        #region Helper functions

        /// <summary>
        /// Returns whether the platform is using Little Endian.
        /// </summary>
        /// <returns>Returns true if the platform is using Litte Endian, else false.</returns>
        public static bool IsLittleEndian()
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.IsLittleEndian();
            else
                return FreeImageStaticImportsLinux.IsLittleEndian();
        }

        /// <summary>
        /// Converts a X11 color name into a corresponding RGB value.
        /// </summary>
        /// <param name="szColor">Name of the color to convert.</param>
        /// <param name="nRed">Red component.</param>
        /// <param name="nGreen">Green component.</param>
        /// <param name="nBlue">Blue component.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool LookupX11Color(string szColor, out byte nRed, out byte nGreen, out byte nBlue)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.LookupX11Color(szColor, out nRed, out nGreen, out nBlue);
            else
                return FreeImageStaticImportsLinux.LookupX11Color(szColor, out nRed, out nGreen, out nBlue);
        }

        /// <summary>
        /// Converts a SVG color name into a corresponding RGB value.
        /// </summary>
        /// <param name="szColor">Name of the color to convert.</param>
        /// <param name="nRed">Red component.</param>
        /// <param name="nGreen">Green component.</param>
        /// <param name="nBlue">Blue component.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool LookupSVGColor(string szColor, out byte nRed, out byte nGreen, out byte nBlue)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.LookupSVGColor(szColor, out nRed, out nGreen, out nBlue);
            else
                return FreeImageStaticImportsLinux.LookupSVGColor(szColor, out nRed, out nGreen, out nBlue);
        }

        #endregion

        #region Pixel access functions

        /// <summary>
        /// Returns a pointer to the data-bits of the bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Pointer to the data-bits.</returns>
        public static IntPtr GetBits(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetBits(dib);
            else
                return FreeImageStaticImportsLinux.GetBits(dib);
        }

        /// <summary>
        /// Returns a pointer to the start of the given scanline in the bitmap's data-bits.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="scanline">Number of the scanline.</param>
        /// <returns>Pointer to the scanline.</returns>
        public static IntPtr GetScanLine(FIBITMAP dib, int scanline)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetScanLine(dib, scanline);
            else
                return FreeImageStaticImportsLinux.GetScanLine(dib, scanline);
        }

        /// <summary>
        /// Get the pixel index of a palettized image at position (x, y), including range check (slow access).
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="x">Pixel position in horizontal direction.</param>
        /// <param name="y">Pixel position in vertical direction.</param>
        /// <param name="value">The pixel index.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool GetPixelIndex(FIBITMAP dib, uint x, uint y, out byte value)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetPixelIndex(dib, x, y, out value);
            else
                return FreeImageStaticImportsLinux.GetPixelIndex(dib, x, y, out value);
        }

        /// <summary>
        /// Get the pixel color of a 16-, 24- or 32-bit image at position (x, y), including range check (slow access).
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="x">Pixel position in horizontal direction.</param>
        /// <param name="y">Pixel position in vertical direction.</param>
        /// <param name="value">The pixel color.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool GetPixelColor(FIBITMAP dib, uint x, uint y, out RGBQUAD value)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetPixelColor(dib, x, y, out value);
            else
                return FreeImageStaticImportsLinux.GetPixelColor(dib, x, y, out value);
        }

        /// <summary>
        /// Set the pixel index of a palettized image at position (x, y), including range check (slow access).
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="x">Pixel position in horizontal direction.</param>
        /// <param name="y">Pixel position in vertical direction.</param>
        /// <param name="value">The new pixel index.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SetPixelIndex(FIBITMAP dib, uint x, uint y, ref byte value)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetPixelIndex(dib, x, y, ref value);
            else
                return FreeImageStaticImportsLinux.SetPixelIndex(dib, x, y, ref value);
        }

        /// <summary>
        /// Set the pixel color of a 16-, 24- or 32-bit image at position (x, y), including range check (slow access).
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="x">Pixel position in horizontal direction.</param>
        /// <param name="y">Pixel position in vertical direction.</param>
        /// <param name="value">The new pixel color.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SetPixelColor(FIBITMAP dib, uint x, uint y, ref RGBQUAD value)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetPixelColor(dib, x, y, ref value);
            else
                return FreeImageStaticImportsLinux.SetPixelColor(dib, x, y, ref value);
        }

        #endregion

        #region Bitmap information functions

        /// <summary>
        /// Retrieves the type of the bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Type of the bitmap.</returns>
        public static FREE_IMAGE_TYPE GetImageType(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetImageType(dib);
            else
                return FreeImageStaticImportsLinux.GetImageType(dib);
        }

        /// <summary>
        /// Returns the number of colors used in a bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Palette-size for palletised bitmaps, and 0 for high-colour bitmaps.</returns>
        public static uint GetColorsUsed(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetColorsUsed(dib);
            else
                return FreeImageStaticImportsLinux.GetColorsUsed(dib);
        }

        /// <summary>
        /// Returns the size of one pixel in the bitmap in bits.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Size of one pixel in the bitmap in bits.</returns>
        public static uint GetBPP(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetBPP(dib);
            else
                return FreeImageStaticImportsLinux.GetBPP(dib);
        }

        /// <summary>
        /// Returns the width of the bitmap in pixel units.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>With of the bitmap.</returns>
        public static uint GetWidth(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetWidth(dib);
            else
                return FreeImageStaticImportsLinux.GetWidth(dib);
        }

        /// <summary>
        /// Returns the height of the bitmap in pixel units.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Height of the bitmap.</returns>
        public static uint GetHeight(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetHeight(dib);
            else
                return FreeImageStaticImportsLinux.GetHeight(dib);
        }

        /// <summary>
        /// Returns the width of the bitmap in bytes.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>With of the bitmap in bytes.</returns>
        public static uint GetLine(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetLine(dib);
            else
                return FreeImageStaticImportsLinux.GetLine(dib);
        }

        /// <summary>
        /// Returns the width of the bitmap in bytes, rounded to the next 32-bit boundary,
        /// also known as pitch or stride or scan width.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>With of the bitmap in bytes.</returns>
        public static uint GetPitch(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetPitch(dib);
            else
                return FreeImageStaticImportsLinux.GetPitch(dib);
        }

        /// <summary>
        /// Returns the size of the DIB-element of a FIBITMAP in memory.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Size of the DIB-element</returns>
        public static uint GetDIBSize(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetDIBSize(dib);
            else
                return FreeImageStaticImportsLinux.GetDIBSize(dib);
        }

        /// <summary>
        /// Returns a pointer to the bitmap's palette.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Pointer to the bitmap's palette.</returns>
        public static IntPtr GetPalette(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetPalette(dib);
            else
                return FreeImageStaticImportsLinux.GetPalette(dib);
        }

        /// <summary>
        /// Returns the horizontal resolution, in pixels-per-meter, of the target device for the bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>The horizontal resolution, in pixels-per-meter.</returns>
        public static uint GetDotsPerMeterX(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetDotsPerMeterX(dib);
            else
                return FreeImageStaticImportsLinux.GetDotsPerMeterX(dib);
        }

        /// <summary>
        /// Returns the vertical resolution, in pixels-per-meter, of the target device for the bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>The vertical resolution, in pixels-per-meter.</returns>
        public static uint GetDotsPerMeterY(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetDotsPerMeterY(dib);
            else
                return FreeImageStaticImportsLinux.GetDotsPerMeterY(dib);
        }

        /// <summary>
        /// Set the horizontal resolution, in pixels-per-meter, of the target device for the bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="res">The new horizontal resolution.</param>
        public static void SetDotsPerMeterX(FIBITMAP dib, uint res)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.SetDotsPerMeterX(dib, res);
            else
                FreeImageStaticImportsLinux.SetDotsPerMeterX(dib, res);
        }

        /// <summary>
        /// Set the vertical resolution, in pixels-per-meter, of the target device for the bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="res">The new vertical resolution.</param>
        public static void SetDotsPerMeterY(FIBITMAP dib, uint res)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.SetDotsPerMeterY(dib, res);
            else
                FreeImageStaticImportsLinux.SetDotsPerMeterY(dib, res);
        }

        /// <summary>
        /// Returns a pointer to the <see cref="BITMAPINFOHEADER"/> of the DIB-element in a FIBITMAP.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Poiter to the header of the bitmap.</returns>
        public static IntPtr GetInfoHeader(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetInfoHeader(dib);
            else
                return FreeImageStaticImportsLinux.GetInfoHeader(dib);
        }

        /// <summary>
        /// Alias for FreeImage_GetInfoHeader that returns a pointer to a <see cref="BITMAPINFO"/>
        /// rather than to a <see cref="BITMAPINFOHEADER"/>.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Pointer to the <see cref="BITMAPINFO"/> structure for the bitmap.</returns>
        public static IntPtr GetInfo(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetInfo(dib);
            else
                return FreeImageStaticImportsLinux.GetInfo(dib);
        }

        /// <summary>
        /// Investigates the color type of the bitmap by reading the bitmap's pixel bits and analysing them.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>The color type of the bitmap.</returns>
        public static FREE_IMAGE_COLOR_TYPE GetColorType(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetColorType(dib);
            else
                return FreeImageStaticImportsLinux.GetColorType(dib);
        }

        /// <summary>
        /// Returns a bit pattern describing the red color component of a pixel in a FreeImage bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>The bit pattern for RED.</returns>
        public static uint GetRedMask(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetRedMask(dib);
            else
                return FreeImageStaticImportsLinux.GetRedMask(dib);
        }

        /// <summary>
        /// Returns a bit pattern describing the green color component of a pixel in a FreeImage bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>The bit pattern for green.</returns>
        public static uint GetGreenMask(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetGreenMask(dib);
            else
                return FreeImageStaticImportsLinux.GetGreenMask(dib);
        }

        /// <summary>
        /// Returns a bit pattern describing the blue color component of a pixel in a FreeImage bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>The bit pattern for blue.</returns>
        public static uint GetBlueMask(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetBlueMask(dib);
            else
                return FreeImageStaticImportsLinux.GetBlueMask(dib);
        }

        /// <summary>
        /// Returns the number of transparent colors in a palletised bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>The number of transparent colors in a palletised bitmap.</returns>
        public static uint GetTransparencyCount(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetTransparencyCount(dib);
            else
                return FreeImageStaticImportsLinux.GetTransparencyCount(dib);
        }

        /// <summary>
        /// Returns a pointer to the bitmap's transparency table.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Pointer to the bitmap's transparency table.</returns>
        public static IntPtr GetTransparencyTable(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetTransparencyTable(dib);
            else
                return FreeImageStaticImportsLinux.GetTransparencyTable(dib);
        }

        /// <summary>
        /// Tells FreeImage if it should make use of the transparency table
        /// or the alpha channel that may accompany a bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="enabled">True to enable the transparency, false to disable.</param>
        public static void SetTransparent(FIBITMAP dib, bool enabled)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.SetTransparent(dib, enabled);
            else
                FreeImageStaticImportsLinux.SetTransparent(dib, enabled);
        }

        /// <summary>
        /// Set the bitmap's transparency table. Only affects palletised bitmaps.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="table">Pointer to the bitmap's new transparency table.</param>
        /// <param name="count">The number of transparent colors in the new transparency table.</param>
        internal static void SetTransparencyTable(FIBITMAP dib, byte[] table, int count)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.SetTransparencyTable(dib, table, count);
            else
                FreeImageStaticImportsLinux.SetTransparencyTable(dib, table, count);
        }

        /// <summary>
        /// Returns whether the transparency table is enabled.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Returns true when the transparency table is enabled (1-, 4- or 8-bit images)
        /// or when the input dib contains alpha values (32-bit images). Returns false otherwise.</returns>
        public static bool IsTransparent(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.IsTransparent(dib);
            else
                return FreeImageStaticImportsLinux.IsTransparent(dib);
        }

        /// <summary>
        /// Returns whether the bitmap has a file background color.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Returns true when the image has a file background color, false otherwise.</returns>
        public static bool HasBackgroundColor(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.HasBackgroundColor(dib);
            else
                return FreeImageStaticImportsLinux.HasBackgroundColor(dib);
        }

        /// <summary>
        /// Returns the file background color of an image.
        /// For 8-bit images, the color index in the palette is returned in the
        /// rgbReserved member of the bkcolor parameter.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="bkcolor">The background color.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool GetBackgroundColor(FIBITMAP dib, out RGBQUAD bkcolor)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetBackgroundColor(dib, out bkcolor);
            else
                return FreeImageStaticImportsLinux.GetBackgroundColor(dib, out bkcolor);
        }

        /// <summary>
        /// Set the file background color of an image.
        /// When saving an image to PNG, this background color is transparently saved to the PNG file.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="bkcolor">The new background color.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static unsafe bool SetBackgroundColor(FIBITMAP dib, ref RGBQUAD bkcolor)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetBackgroundColor(dib, ref bkcolor);
            else
                return FreeImageStaticImportsLinux.SetBackgroundColor(dib, ref bkcolor);
        }

        /// <summary>
        /// Set the file background color of an image.
        /// When saving an image to PNG, this background color is transparently saved to the PNG file.
        /// When the bkcolor parameter is null, the background color is removed from the image.
        /// <para>
        /// This overloaded version of the function with an array parameter is provided to allow
        /// passing <c>null</c> in the <paramref name="bkcolor"/> parameter. This is similar to the
        /// original C/C++ function. Passing <c>null</c> as <paramref name="bkcolor"/> parameter will
        /// unset the dib's previously set background color.
        /// </para> 
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="bkcolor">The new background color.
        /// The first entry in the array is used.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        /// <example>
        /// <code>
        /// // create a RGBQUAD color
        /// RGBQUAD color = new RGBQUAD(Color.Green);
        /// 
        /// // set the dib's background color (using the other version of the function)
        /// FreeImage.SetBackgroundColor(dib, ref color);
        /// 
        /// // remove it again (this only works due to the array parameter RGBQUAD[])
        /// FreeImage.SetBackgroundColor(dib, null);
        /// </code>
        /// </example>
        public static unsafe bool SetBackgroundColor(FIBITMAP dib, RGBQUAD[] bkcolor)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetBackgroundColor(dib, bkcolor);
            else
                return FreeImageStaticImportsLinux.SetBackgroundColor(dib, bkcolor);
        }

        /// <summary>
        /// Sets the index of the palette entry to be used as transparent color
        /// for the image specified. Does nothing on high color images.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="index">The index of the palette entry to be set as transparent color.</param>
        public static void SetTransparentIndex(FIBITMAP dib, int index)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.SetTransparentIndex(dib, index);
            else
                FreeImageStaticImportsLinux.SetTransparentIndex(dib, index);
        }

        /// <summary>
        /// Returns the palette entry used as transparent color for the image specified.
        /// Works for palletised images only and returns -1 for high color
        /// images or if the image has no color set to be transparent.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>the index of the palette entry used as transparent color for
        /// the image specified or -1 if there is no transparent color found
        /// (e.g. the image is a high color image).</returns>
        public static int GetTransparentIndex(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetTransparentIndex(dib);
            else
                return FreeImageStaticImportsLinux.GetTransparentIndex(dib);
        }

        #endregion

        #region ICC profile functions

        /// <summary>
        /// Retrieves the <see cref="FIICCPROFILE"/> data of the bitmap.
        /// This function can also be called safely, when the original format does not support profiles.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>The <see cref="FIICCPROFILE"/> data of the bitmap.</returns>
        public static FIICCPROFILE GetICCProfileEx(FIBITMAP dib) { unsafe { return *(FIICCPROFILE*)GetICCProfile(dib); } }

        /// <summary>
        /// Retrieves a pointer to the <see cref="FIICCPROFILE"/> data of the bitmap.
        /// This function can also be called safely, when the original format does not support profiles.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Pointer to the <see cref="FIICCPROFILE"/> data of the bitmap.</returns>
        public static IntPtr GetICCProfile(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetICCProfile(dib);
            else
                return FreeImageStaticImportsLinux.GetICCProfile(dib);
        }

        /// <summary>
        /// Creates a new <see cref="FIICCPROFILE"/> block from ICC profile data previously read from a file
        /// or built by a color management system. The profile data is attached to the bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="data">Pointer to the new <see cref="FIICCPROFILE"/> data.</param>
        /// <param name="size">Size of the <see cref="FIICCPROFILE"/> data.</param>
        /// <returns>Pointer to the created <see cref="FIICCPROFILE"/> structure.</returns>
        public static IntPtr CreateICCProfile(FIBITMAP dib, byte[] data, int size)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.CreateICCProfile(dib, data, size);
            else
                return FreeImageStaticImportsLinux.CreateICCProfile(dib, data, size);
        }

        /// <summary>
        /// This function destroys an <see cref="FIICCPROFILE"/> previously created by <see cref="CreateICCProfile(FIBITMAP,byte[],int)"/>.
        /// After this call the bitmap will contain no profile information.
        /// This function should be called to ensure that a stored bitmap will not contain any profile information.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        public static void DestroyICCProfile(FIBITMAP dib)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.DestroyICCProfile(dib);
            else
                FreeImageStaticImportsLinux.DestroyICCProfile(dib);
        }

        #endregion

        #region Conversion functions

        /// <summary>
        /// Converts a bitmap to 4 bits.
        /// If the bitmap was a high-color bitmap (16, 24 or 32-bit) or if it was a
        /// monochrome or greyscale bitmap (1 or 8-bit), the end result will be a
        /// greyscale bitmap, otherwise (1-bit palletised bitmaps) it will be a palletised bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ConvertTo4Bits(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ConvertTo4Bits(dib);
            else
                return FreeImageStaticImportsLinux.ConvertTo4Bits(dib);
        }

        /// <summary>
        /// Converts a bitmap to 8 bits. If the bitmap was a high-color bitmap (16, 24 or 32-bit)
        /// or if it was a monochrome or greyscale bitmap (1 or 4-bit), the end result will be a
        /// greyscale bitmap, otherwise (1 or 4-bit palletised bitmaps) it will be a palletised bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ConvertTo8Bits(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ConvertTo8Bits(dib);
            else
                return FreeImageStaticImportsLinux.ConvertTo8Bits(dib);
        }

        /// <summary>
        /// Converts a bitmap to a 8-bit greyscale image with a linear ramp.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ConvertToGreyscale(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ConvertToGreyscale(dib);
            else
                return FreeImageStaticImportsLinux.ConvertToGreyscale(dib);
        }

        /// <summary>
        /// Converts a bitmap to 16 bits, where each pixel has a color pattern of
        /// 5 bits red, 5 bits green and 5 bits blue. One bit in each pixel is unused.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ConvertTo16Bits555(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ConvertTo16Bits555(dib);
            else
                return FreeImageStaticImportsLinux.ConvertTo16Bits555(dib);
        }
        /// <summary>
        /// Converts a bitmap to 16 bits, where each pixel has a color pattern of
        /// 5 bits red, 6 bits green and 5 bits blue.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ConvertTo16Bits565(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ConvertTo16Bits565(dib);
            else
                return FreeImageStaticImportsLinux.ConvertTo16Bits565(dib);
        }

        /// <summary>
        /// Converts a bitmap to 24 bits. A clone of the input bitmap is returned for 24-bit bitmaps.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ConvertTo24Bits(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ConvertTo24Bits(dib);
            else
                return FreeImageStaticImportsLinux.ConvertTo24Bits(dib);
        }

        /// <summary>
        /// Converts a bitmap to 32 bits. A clone of the input bitmap is returned for 32-bit bitmaps.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ConvertTo32Bits(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ConvertTo32Bits(dib);
            else
                return FreeImageStaticImportsLinux.ConvertTo32Bits(dib);
        }

        /// <summary>
        /// Quantizes a high-color 24-bit bitmap to an 8-bit palette color bitmap.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="quantize">Specifies the color reduction algorithm to be used.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ColorQuantize(FIBITMAP dib, FREE_IMAGE_QUANTIZE quantize)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ColorQuantize(dib, quantize);
            else
                return FreeImageStaticImportsLinux.ColorQuantize(dib, quantize);
        }

        /// <summary>
        /// ColorQuantizeEx is an extension to the <see cref="ColorQuantize(FIBITMAP, FREE_IMAGE_QUANTIZE)"/> method that
        /// provides additional options used to quantize a 24-bit image to any
        /// number of colors (up to 256), as well as quantize a 24-bit image using a
        /// partial or full provided palette.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="quantize">Specifies the color reduction algorithm to be used.</param>
        /// <param name="PaletteSize">Size of the desired output palette.</param>
        /// <param name="ReserveSize">Size of the provided palette of ReservePalette.</param>
        /// <param name="ReservePalette">The provided palette.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ColorQuantizeEx(FIBITMAP dib, FREE_IMAGE_QUANTIZE quantize, int PaletteSize, int ReserveSize, RGBQUAD[] ReservePalette)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ColorQuantizeEx(dib, quantize, PaletteSize, ReserveSize, ReservePalette);
            else
                return FreeImageStaticImportsLinux.ColorQuantizeEx(dib, quantize, PaletteSize, ReserveSize, ReservePalette);
        }

        /// <summary>
        /// Converts a bitmap to 1-bit monochrome bitmap using a threshold T between [0..255].
        /// The function first converts the bitmap to a 8-bit greyscale bitmap.
        /// Then, any brightness level that is less than T is set to zero, otherwise to 1.
        /// For 1-bit input bitmaps, the function clones the input bitmap and builds a monochrome palette.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="t">The threshold.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP Threshold(FIBITMAP dib, byte t)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Threshold(dib, t);
            else
                return FreeImageStaticImportsLinux.Threshold(dib, t);
        }

        /// <summary>
        /// Converts a bitmap to 1-bit monochrome bitmap using a dithering algorithm.
        /// For 1-bit input bitmaps, the function clones the input bitmap and builds a monochrome palette.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="algorithm">The dithering algorithm to use.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP Dither(FIBITMAP dib, FREE_IMAGE_DITHER algorithm)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Dither(dib, algorithm);
            else
                return FreeImageStaticImportsLinux.Dither(dib, algorithm);
        }

        /// <summary>
        /// Converts a raw bitmap to a FreeImage bitmap.
        /// </summary>
        /// <param name="bits">Pointer to the memory block containing the raw bitmap.</param>
        /// <param name="width">The width in pixels of the raw bitmap.</param>
        /// <param name="height">The height in pixels of the raw bitmap.</param>
        /// <param name="pitch">Defines the total width of a scanline in the raw bitmap,
        /// including padding bytes.</param>
        /// <param name="bpp">The bit depth (bits per pixel) of the raw bitmap.</param>
        /// <param name="red_mask">The bit mask describing the bits used to store a single 
        /// pixel's red component in the raw bitmap. This is only applied to 16-bpp raw bitmaps.</param>
        /// <param name="green_mask">The bit mask describing the bits used to store a single
        /// pixel's green component in the raw bitmap. This is only applied to 16-bpp raw bitmaps.</param>
        /// <param name="blue_mask">The bit mask describing the bits used to store a single
        /// pixel's blue component in the raw bitmap. This is only applied to 16-bpp raw bitmaps.</param>
        /// <param name="topdown">If true, the raw bitmap is stored in top-down order (top-left pixel first)
        /// and in bottom-up order (bottom-left pixel first) otherwise.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ConvertFromRawBits(IntPtr bits, int width, int height, int pitch,
                                                  uint bpp, uint red_mask, uint green_mask, uint blue_mask, bool topdown)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ConvertFromRawBits(bits, width, height, pitch, bpp, red_mask, green_mask, blue_mask, topdown);
            else
                return FreeImageStaticImportsLinux.ConvertFromRawBits(bits, width, height, pitch, bpp, red_mask, green_mask, blue_mask, topdown);
        }

        /// <summary>
        /// Converts a raw bitmap to a FreeImage bitmap.
        /// </summary>
        /// <param name="bits">Array of bytes containing the raw bitmap.</param>
        /// <param name="width">The width in pixels of the raw bitmap.</param>
        /// <param name="height">The height in pixels of the raw bitmap.</param>
        /// <param name="pitch">Defines the total width of a scanline in the raw bitmap,
        /// including padding bytes.</param>
        /// <param name="bpp">The bit depth (bits per pixel) of the raw bitmap.</param>
        /// <param name="red_mask">The bit mask describing the bits used to store a single 
        /// pixel's red component in the raw bitmap. This is only applied to 16-bpp raw bitmaps.</param>
        /// <param name="green_mask">The bit mask describing the bits used to store a single
        /// pixel's green component in the raw bitmap. This is only applied to 16-bpp raw bitmaps.</param>
        /// <param name="blue_mask">The bit mask describing the bits used to store a single
        /// pixel's blue component in the raw bitmap. This is only applied to 16-bpp raw bitmaps.</param>
        /// <param name="topdown">If true, the raw bitmap is stored in top-down order (top-left pixel first)
        /// and in bottom-up order (bottom-left pixel first) otherwise.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ConvertFromRawBits(byte[] bits, int width, int height, int pitch,
                                                  uint bpp, uint red_mask, uint green_mask, uint blue_mask, bool topdown)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ConvertFromRawBits(bits, width, height, pitch, bpp, red_mask, green_mask, blue_mask, topdown);
            else
                return FreeImageStaticImportsLinux.ConvertFromRawBits(bits, width, height, pitch, bpp, red_mask, green_mask, blue_mask, topdown);
        }

        /// <summary>
        /// Converts a FreeImage bitmap to a raw bitmap, that is a raw piece of memory.
        /// </summary>
        /// <param name="bits">Pointer to the memory block receiving the raw bitmap.</param>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="pitch">The desired total width in bytes of a scanline in the raw bitmap,
        /// including any padding bytes.</param>
        /// <param name="bpp">The desired bit depth (bits per pixel) of the raw bitmap.</param>
        /// <param name="red_mask">The desired bit mask describing the bits used to store a single 
        /// pixel's red component in the raw bitmap. This is only applied to 16-bpp raw bitmaps.</param>
        /// <param name="green_mask">The desired bit mask describing the bits used to store a single
        /// pixel's green component in the raw bitmap. This is only applied to 16-bpp raw bitmaps.</param>
        /// <param name="blue_mask">The desired bit mask describing the bits used to store a single
        /// pixel's blue component in the raw bitmap. This is only applied to 16-bpp raw bitmaps.</param>
        /// <param name="topdown">If true, the raw bitmap will be stored in top-down order (top-left pixel first)
        /// and in bottom-up order (bottom-left pixel first) otherwise.</param>
        public static void ConvertToRawBits(IntPtr bits, FIBITMAP dib, int pitch, uint bpp,
                                            uint red_mask, uint green_mask, uint blue_mask, bool topdown)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.ConvertToRawBits(bits, dib, pitch, bpp, red_mask, green_mask, blue_mask, topdown);
            else
                FreeImageStaticImportsLinux.ConvertToRawBits(bits, dib, pitch, bpp, red_mask, green_mask, blue_mask, topdown);
        }

        /// <summary>
        /// Converts a FreeImage bitmap to a raw bitmap, that is a raw piece of memory.
        /// </summary>
        /// <param name="bits">Array of bytes receiving the raw bitmap.</param>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="pitch">The desired total width in bytes of a scanline in the raw bitmap,
        /// including any padding bytes.</param>
        /// <param name="bpp">The desired bit depth (bits per pixel) of the raw bitmap.</param>
        /// <param name="red_mask">The desired bit mask describing the bits used to store a single 
        /// pixel's red component in the raw bitmap. This is only applied to 16-bpp raw bitmaps.</param>
        /// <param name="green_mask">The desired bit mask describing the bits used to store a single
        /// pixel's green component in the raw bitmap. This is only applied to 16-bpp raw bitmaps.</param>
        /// <param name="blue_mask">The desired bit mask describing the bits used to store a single
        /// pixel's blue component in the raw bitmap. This is only applied to 16-bpp raw bitmaps.</param>
        /// <param name="topdown">If true, the raw bitmap will be stored in top-down order (top-left pixel first)
        /// and in bottom-up order (bottom-left pixel first) otherwise.</param>
        public static void ConvertToRawBits(byte[] bits, FIBITMAP dib, int pitch, uint bpp,
                                            uint red_mask, uint green_mask, uint blue_mask, bool topdown)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.ConvertToRawBits(bits, dib, pitch, bpp, red_mask, green_mask, blue_mask, topdown);
            else
                FreeImageStaticImportsLinux.ConvertToRawBits(bits, dib, pitch, bpp, red_mask, green_mask, blue_mask, topdown);
        }

        /// <summary>
        /// Converts a 24- or 32-bit RGB(A) standard image or a 48-bit RGB image to a FIT_RGBF type image.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ConvertToRGBF(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ConvertToRGBF(dib);
            else
                return FreeImageStaticImportsLinux.ConvertToRGBF(dib);
        }

        /// <summary>
        /// Converts a non standard image whose color type is FIC_MINISBLACK
        /// to a standard 8-bit greyscale image.
        /// </summary>
        /// <param name="src">Handle to a FreeImage bitmap.</param>
        /// <param name="scale_linear">When true the conversion is done by scaling linearly
        /// each pixel value from [min, max] to an integer value between [0..255],
        /// where min and max are the minimum and maximum pixel values in the image.
        /// When false the conversion is done by rounding each pixel value to an integer between [0..255].
        ///
        /// Rounding is done using the following formula:
        ///
        /// dst_pixel = (BYTE) MIN(255, MAX(0, q)) where int q = int(src_pixel + 0.5);</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ConvertToStandardType(FIBITMAP src, bool scale_linear)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ConvertToStandardType(src, scale_linear);
            else
                return FreeImageStaticImportsLinux.ConvertToStandardType(src, scale_linear);
        }

        /// <summary>
        /// Converts an image of any type to type dst_type.
        /// </summary>
        /// <param name="src">Handle to a FreeImage bitmap.</param>
        /// <param name="dst_type">Destination type.</param>
        /// <param name="scale_linear">True to scale linear, else false.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ConvertToType(FIBITMAP src, FREE_IMAGE_TYPE dst_type, bool scale_linear)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ConvertToType(src, dst_type, scale_linear);
            else
                return FreeImageStaticImportsLinux.ConvertToType(src, dst_type, scale_linear);
        }

        #endregion

        #region Tone mapping operators

        /// <summary>
        /// Converts a High Dynamic Range image (48-bit RGB or 96-bit RGBF) to a 24-bit RGB image, suitable for display.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="tmo">The tone mapping operator to be used.</param>
        /// <param name="first_param">Parmeter depending on the used algorithm</param>
        /// <param name="second_param">Parmeter depending on the used algorithm</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP ToneMapping(FIBITMAP dib, FREE_IMAGE_TMO tmo, double first_param, double second_param)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ToneMapping(dib, tmo, first_param, second_param);
            else
                return FreeImageStaticImportsLinux.ToneMapping(dib, tmo, first_param, second_param);
        }

        /// <summary>
        /// Converts a High Dynamic Range image to a 24-bit RGB image using a global
        /// operator based on logarithmic compression of luminance values, imitating the human response to light.
        /// </summary>
        /// <param name="src">Handle to a FreeImage bitmap.</param>
        /// <param name="gamma">A gamma correction that is applied after the tone mapping.
        /// A value of 1 means no correction.</param>
        /// <param name="exposure">Scale factor allowing to adjust the brightness of the output image.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP TmoDrago03(FIBITMAP src, double gamma, double exposure)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.TmoDrago03(src, gamma, exposure);
            else
                return FreeImageStaticImportsLinux.TmoDrago03(src, gamma, exposure);
        }

        /// <summary>
        /// Converts a High Dynamic Range image to a 24-bit RGB image using a global operator inspired
        /// by photoreceptor physiology of the human visual system.
        /// </summary>
        /// <param name="src">Handle to a FreeImage bitmap.</param>
        /// <param name="intensity">Controls the overall image intensity in the range [-8, 8].</param>
        /// <param name="contrast">Controls the overall image contrast in the range [0.3, 1.0[.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP TmoReinhard05(FIBITMAP src, double intensity, double contrast)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.TmoReinhard05(src, intensity, contrast);
            else
                return FreeImageStaticImportsLinux.TmoReinhard05(src, intensity, contrast);
        }

        /// <summary>
        /// Apply the Gradient Domain High Dynamic Range Compression to a RGBF image and convert to 24-bit RGB.
        /// </summary>
        /// <param name="src">Handle to a FreeImage bitmap.</param>
        /// <param name="color_saturation">Color saturation (s parameter in the paper) in [0.4..0.6]</param>
        /// <param name="attenuation">Atenuation factor (beta parameter in the paper) in [0.8..0.9]</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP TmoFattal02(FIBITMAP src, double color_saturation, double attenuation)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.TmoFattal02(src, color_saturation, attenuation);
            else
                return FreeImageStaticImportsLinux.TmoFattal02(src, color_saturation, attenuation);
        }

        #endregion

        #region Compression functions

        /// <summary>
        /// Compresses a source buffer into a target buffer, using the ZLib library.
        /// </summary>
        /// <param name="target">Pointer to the target buffer.</param>
        /// <param name="target_size">Size of the target buffer.
        /// Must be at least 0.1% larger than source_size plus 12 bytes.</param>
        /// <param name="source">Pointer to the source buffer.</param>
        /// <param name="source_size">Size of the source buffer.</param>
        /// <returns>The actual size of the compressed buffer, or 0 if an error occurred.</returns>
        public static uint ZLibCompress(byte[] target, uint target_size, byte[] source, uint source_size)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ZLibCompress(target, target_size, source, source_size);
            else
                return FreeImageStaticImportsLinux.ZLibCompress(target, target_size, source, source_size);
        }

        /// <summary>
        /// Decompresses a source buffer into a target buffer, using the ZLib library.
        /// </summary>
        /// <param name="target">Pointer to the target buffer.</param>
        /// <param name="target_size">Size of the target buffer.
        /// Must have been saved outlide of zlib.</param>
        /// <param name="source">Pointer to the source buffer.</param>
        /// <param name="source_size">Size of the source buffer.</param>
        /// <returns>The actual size of the uncompressed buffer, or 0 if an error occurred.</returns>
        public static uint ZLibUncompress(byte[] target, uint target_size, byte[] source, uint source_size)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ZLibCompress(target, target_size, source, source_size);
            else
                return FreeImageStaticImportsLinux.ZLibCompress(target, target_size, source, source_size);
        }

        /// <summary>
        /// Compresses a source buffer into a target buffer, using the ZLib library.
        /// </summary>
        /// <param name="target">Pointer to the target buffer.</param>
        /// <param name="target_size">Size of the target buffer.
        /// Must be at least 0.1% larger than source_size plus 24 bytes.</param>
        /// <param name="source">Pointer to the source buffer.</param>
        /// <param name="source_size">Size of the source buffer.</param>
        /// <returns>The actual size of the compressed buffer, or 0 if an error occurred.</returns>
        public static uint ZLibGZip(byte[] target, uint target_size, byte[] source, uint source_size)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ZLibGZip(target, target_size, source, source_size);
            else
                return FreeImageStaticImportsLinux.ZLibGZip(target, target_size, source, source_size);
        }

        /// <summary>
        /// Decompresses a source buffer into a target buffer, using the ZLib library.
        /// </summary>
        /// <param name="target">Pointer to the target buffer.</param>
        /// <param name="target_size">Size of the target buffer.
        /// Must have been saved outlide of zlib.</param>
        /// <param name="source">Pointer to the source buffer.</param>
        /// <param name="source_size">Size of the source buffer.</param>
        /// <returns>The actual size of the uncompressed buffer, or 0 if an error occurred.</returns>
        public static uint ZLibGUnzip(byte[] target, uint target_size, byte[] source, uint source_size)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ZLibGUnzip(target, target_size, source, source_size);
            else
                return FreeImageStaticImportsLinux.ZLibGUnzip(target, target_size, source, source_size);
        }

        /// <summary>
        /// Generates a CRC32 checksum.
        /// </summary>
        /// <param name="crc">The CRC32 checksum to begin with.</param>
        /// <param name="source">Pointer to the source buffer.
        /// If the value is 0, the function returns the required initial value for the crc.</param>
        /// <param name="source_size">Size of the source buffer.</param>
        /// <returns></returns>
        public static uint ZLibCRC32(uint crc, byte[] source, uint source_size)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ZLibCRC32(crc, source, source_size);
            else
                return FreeImageStaticImportsLinux.ZLibCRC32(crc, source, source_size);
        }

        #endregion

        #region Tag creation and destruction

        /// <summary>
        /// Allocates a new <see cref="FITAG"/> object.
        /// This object must be destroyed with a call to
        /// <see cref="FreeImageAPI.FreeImage.DeleteTag(FITAG)"/> when no longer in use.
        /// </summary>
        /// <returns>The new <see cref="FITAG"/>.</returns>
        public static FITAG CreateTag()
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.CreateTag();
            else
                return FreeImageStaticImportsLinux.CreateTag();
        }

        /// <summary>
        /// Delete a previously allocated <see cref="FITAG"/> object.
        /// </summary>
        /// <param name="tag">The <see cref="FITAG"/> to destroy.</param>
        public static void DeleteTag(FITAG tag)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.DeleteTag(tag);
            else
                FreeImageStaticImportsLinux.DeleteTag(tag);
        }

        /// <summary>
        /// Creates and returns a copy of a <see cref="FITAG"/> object.
        /// </summary>
        /// <param name="tag">The <see cref="FITAG"/> to clone.</param>
        /// <returns>The new <see cref="FITAG"/>.</returns>
        public static FITAG CloneTag(FITAG tag)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.CloneTag(tag);
            else
                return FreeImageStaticImportsLinux.CloneTag(tag);
        }

        #endregion

        #region Tag accessors

        /// <summary>
        /// Returns the tag field name (unique inside a metadata model).
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <returns>The field name.</returns>
        public static unsafe string GetTagKey(FITAG tag) { return PtrToStr(GetTagKey_(tag)); }
        private static unsafe byte* GetTagKey_(FITAG tag)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetTagKey_(tag);
            else
                return FreeImageStaticImportsLinux.GetTagKey_(tag);
        }

        /// <summary>
        /// Returns the tag description.
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <returns>The description or NULL if unavailable.</returns>
        public static unsafe string GetTagDescription(FITAG tag) { return PtrToStr(GetTagDescription_(tag)); }
        private static unsafe byte* GetTagDescription_(FITAG tag)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetTagDescription_(tag);
            else
                return FreeImageStaticImportsLinux.GetTagDescription_(tag);
        }

        /// <summary>
        /// Returns the tag ID.
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <returns>The ID or 0 if unavailable.</returns>
        public static ushort GetTagID(FITAG tag)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetTagID(tag);
            else
                return FreeImageStaticImportsLinux.GetTagID(tag);
        }

        /// <summary>
        /// Returns the tag data type.
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <returns>The tag type.</returns>
        public static FREE_IMAGE_MDTYPE GetTagType(FITAG tag)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetTagType(tag);
            else
                return FreeImageStaticImportsLinux.GetTagType(tag);
        }

        /// <summary>
        /// Returns the number of components in the tag (in tag type units).
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <returns>The number of components.</returns>
        public static uint GetTagCount(FITAG tag)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetTagCount(tag);
            else
                return FreeImageStaticImportsLinux.GetTagCount(tag);
        }

        /// <summary>
        /// Returns the length of the tag value in bytes.
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <returns>The length of the tag value.</returns>
        public static uint GetTagLength(FITAG tag)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetTagLength(tag);
            else
                return FreeImageStaticImportsLinux.GetTagLength(tag);
        }

        /// <summary>
        /// Returns the tag value.
        /// It is up to the programmer to interpret the returned pointer correctly,
        /// according to the results of GetTagType and GetTagCount.
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <returns>Pointer to the value.</returns>
        public static IntPtr GetTagValue(FITAG tag)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetTagValue(tag);
            else
                return FreeImageStaticImportsLinux.GetTagValue(tag);
        }

        /// <summary>
        /// Sets the tag field name.
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <param name="key">The new name.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SetTagKey(FITAG tag, string key)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetTagKey(tag, key);
            else
                return FreeImageStaticImportsLinux.SetTagKey(tag, key);
        }

        /// <summary>
        /// Sets the tag description.
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <param name="description">The new description.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SetTagDescription(FITAG tag, string description)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetTagDescription(tag, description);
            else
                return FreeImageStaticImportsLinux.SetTagDescription(tag, description);
        }

        /// <summary>
        /// Sets the tag ID.
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <param name="id">The new ID.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SetTagID(FITAG tag, ushort id)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetTagID(tag, id);
            else
                return FreeImageStaticImportsLinux.SetTagID(tag, id);
        }

        /// <summary>
        /// Sets the tag data type.
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <param name="type">The new type.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SetTagType(FITAG tag, FREE_IMAGE_MDTYPE type)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetTagType(tag, type);
            else
                return FreeImageStaticImportsLinux.SetTagType(tag, type);
        }

        /// <summary>
        /// Sets the number of data in the tag.
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <param name="count">New number of data.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SetTagCount(FITAG tag, uint count)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetTagCount(tag, count);
            else
                return FreeImageStaticImportsLinux.SetTagCount(tag, count);
        }

        /// <summary>
        /// Sets the length of the tag value in bytes.
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <param name="length">The new length.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SetTagLength(FITAG tag, uint length)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetTagLength(tag, length);
            else
                return FreeImageStaticImportsLinux.SetTagLength(tag, length);
        }

        /// <summary>
        /// Sets the tag value.
        /// </summary>
        /// <param name="tag">The tag field.</param>
        /// <param name="value">Pointer to the new value.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SetTagValue(FITAG tag, byte[] value)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetTagValue(tag, value);
            else
                return FreeImageStaticImportsLinux.SetTagValue(tag, value);
        }

        #endregion

        #region Metadata iterator

        /// <summary>
        /// Provides information about the first instance of a tag that matches the metadata model.
        /// </summary>
        /// <param name="model">The model to match.</param>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="tag">Tag that matches the metadata model.</param>
        /// <returns>Unique search handle that can be used to call FindNextMetadata or FindCloseMetadata.
        /// Null if the metadata model does not exist.</returns>
        public static FIMETADATA FindFirstMetadata(FREE_IMAGE_MDMODEL model, FIBITMAP dib, out FITAG tag)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.FindFirstMetadata(model, dib, out tag);
            else
                return FreeImageStaticImportsLinux.FindFirstMetadata(model, dib, out tag);
        }

        /// <summary>
        /// Find the next tag, if any, that matches the metadata model argument in a previous call
        /// to FindFirstMetadata, and then alters the tag object contents accordingly.
        /// </summary>
        /// <param name="mdhandle">Unique search handle provided by FindFirstMetadata.</param>
        /// <param name="tag">Tag that matches the metadata model.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool FindNextMetadata(FIMETADATA mdhandle, out FITAG tag)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.FindNextMetadata(mdhandle, out tag);
            else
                return FreeImageStaticImportsLinux.FindNextMetadata(mdhandle, out tag);
        }

        /// <summary>
        /// Closes the specified metadata search handle and releases associated resources.
        /// </summary>
        /// <param name="mdhandle">The handle to close.</param>
        private static void FindCloseMetadata_(FIMETADATA mdhandle)
        {
            if (!IsLinux)
                FreeImageStaticImportsDefault.FindCloseMetadata_(mdhandle);
            else
                FreeImageStaticImportsLinux.FindCloseMetadata_(mdhandle);
        }

        #endregion

        #region Metadata setter and getter

        /// <summary>
        /// Retrieve a metadata attached to a dib.
        /// </summary>
        /// <param name="model">The metadata model to look for.</param>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="key">The metadata field name.</param>
        /// <param name="tag">A FITAG structure returned by the function.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool GetMetadata(FREE_IMAGE_MDMODEL model, FIBITMAP dib, string key, out FITAG tag)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetMetadata(model, dib, key, out tag);
            else
                return FreeImageStaticImportsLinux.GetMetadata(model, dib, key, out tag);
        }

        /// <summary>
        /// Attach a new FreeImage tag to a dib.
        /// </summary>
        /// <param name="model">The metadata model used to store the tag.</param>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="key">The tag field name.</param>
        /// <param name="tag">The FreeImage tag to be attached.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SetMetadata(FREE_IMAGE_MDMODEL model, FIBITMAP dib, string key, FITAG tag)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetMetadata(model, dib, key, tag);
            else
                return FreeImageStaticImportsLinux.SetMetadata(model, dib, key, tag);
        }

        #endregion

        #region Metadata helper functions

        /// <summary>
        /// Returns the number of tags contained in the model metadata model attached to the input dib.
        /// </summary>
        /// <param name="model">The metadata model.</param>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Number of tags contained in the metadata model.</returns>
        public static uint GetMetadataCount(FREE_IMAGE_MDMODEL model, FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetMetadataCount(model, dib);
            else
                return FreeImageStaticImportsLinux.GetMetadataCount(model, dib);
        }

        /// <summary>
        /// Copies the metadata of FreeImage bitmap to another.
        /// </summary>
        /// <param name="dst">The FreeImage bitmap to copy the metadata to.</param>
        /// <param name="src">The FreeImage bitmap to copy the metadata from.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool CloneMetadata(FIBITMAP dst, FIBITMAP src)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.CloneMetadata(dst, src);
            else
                return FreeImageStaticImportsLinux.CloneMetadata(dst, src);
        }

        /// <summary>
        /// Converts a FreeImage tag structure to a string that represents the interpreted tag value.
        /// The function is not thread safe.
        /// </summary>
        /// <param name="model">The metadata model.</param>
        /// <param name="tag">The interpreted tag value.</param>
        /// <param name="Make">Reserved.</param>
        /// <returns>The representing string.</returns>
        public static unsafe string TagToString(FREE_IMAGE_MDMODEL model, FITAG tag, uint Make) { return PtrToStr(TagToString_(model, tag, Make)); }
        private static unsafe byte* TagToString_(FREE_IMAGE_MDMODEL model, FITAG tag, uint Make)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.TagToString_(model, tag, Make);
            else
                return FreeImageStaticImportsLinux.TagToString_(model, tag, Make);
        }

        #endregion

        #region Rotation and flipping

        /// <summary>
        /// This function rotates a 1-, 8-bit greyscale or a 24-, 32-bit color image by means of 3 shears.
        /// 1-bit images rotation is limited to integer multiple of 90°.
        /// <c>null</c> is returned for other values.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="angle">The angle of rotation.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        [Obsolete("RotateClassic is deprecated (use Rotate instead).")]
        public static FIBITMAP RotateClassic(FIBITMAP dib, double angle)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.RotateClassic(dib, angle);
            else
                return FreeImageStaticImportsLinux.RotateClassic(dib, angle);
        }

        internal static FIBITMAP Rotate(FIBITMAP dib, double angle, IntPtr backgroundColor)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Rotate(dib, angle, backgroundColor);
            else
                return FreeImageStaticImportsLinux.Rotate(dib, angle, backgroundColor);
        }

        /// <summary>
        /// This function performs a rotation and / or translation of an 8-bit greyscale,
        /// 24- or 32-bit image, using a 3rd order (cubic) B-Spline.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="angle">The angle of rotation.</param>
        /// <param name="x_shift">Horizontal image translation.</param>
        /// <param name="y_shift">Vertical image translation.</param>
        /// <param name="x_origin">Rotation center x-coordinate.</param>
        /// <param name="y_origin">Rotation center y-coordinate.</param>
        /// <param name="use_mask">When true the irrelevant part of the image is set to a black color,
        /// otherwise, a mirroring technique is used to fill irrelevant pixels.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP RotateEx(FIBITMAP dib, double angle,
                                        double x_shift, double y_shift, double x_origin, double y_origin, bool use_mask)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.RotateEx(dib, angle, x_shift, y_shift, x_origin, y_origin, use_mask);
            else
                return FreeImageStaticImportsLinux.RotateEx(dib, angle, x_shift, y_shift, x_origin, y_origin, use_mask);
        }

        /// <summary>
        /// Flip the input dib horizontally along the vertical axis.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool FlipHorizontal(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.FlipHorizontal(dib);
            else
                return FreeImageStaticImportsLinux.FlipHorizontal(dib);
        }

        /// <summary>
        /// Flip the input dib vertically along the horizontal axis.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool FlipVertical(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.FlipVertical(dib);
            else
                return FreeImageStaticImportsLinux.FlipVertical(dib);
        }

        /// <summary>
        /// Performs a lossless rotation or flipping on a JPEG file.
        /// </summary>
        /// <param name="src_file">Source file.</param>
        /// <param name="dst_file">Destination file; can be the source file; will be overwritten.</param>
        /// <param name="operation">The operation to apply.</param>
        /// <param name="perfect">To avoid lossy transformation, you can set the perfect parameter to true.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool JPEGTransform(string src_file, string dst_file,
                                         FREE_IMAGE_JPEG_OPERATION operation, bool perfect)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.JPEGTransform(src_file, dst_file, operation, perfect);
            else
                return FreeImageStaticImportsLinux.JPEGTransform(src_file, dst_file, operation, perfect);
        }

        #endregion

        #region Upsampling / downsampling

        /// <summary>
        /// Performs resampling (or scaling, zooming) of a greyscale or RGB(A) image
        /// to the desired destination width and height.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="dst_width">Destination width.</param>
        /// <param name="dst_height">Destination height.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP Rescale(FIBITMAP dib, int dst_width, int dst_height, FREE_IMAGE_FILTER filter)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Rescale(dib, dst_width, dst_height, filter);
            else
                return FreeImageStaticImportsLinux.Rescale(dib, dst_width, dst_height, filter);
        }

        /// <summary>
        /// Creates a thumbnail from a greyscale or RGB(A) image, keeping aspect ratio.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="max_pixel_size">Thumbnail square size.</param>
        /// <param name="convert">When true HDR images are transperantly converted to standard images.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP MakeThumbnail(FIBITMAP dib, int max_pixel_size, bool convert)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.MakeThumbnail(dib, max_pixel_size, convert);
            else
                return FreeImageStaticImportsLinux.MakeThumbnail(dib, max_pixel_size, convert);
        }

        internal static FIBITMAP EnlargeCanvas(FIBITMAP dib,
                                               int left, int top, int right, int bottom, IntPtr color, FREE_IMAGE_COLOR_OPTIONS options)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.EnlargeCanvas(dib, left, top, right, bottom, color, options);
            else
                return FreeImageStaticImportsLinux.EnlargeCanvas(dib, left, top, right, bottom, color, options);
        }

        #endregion

        #region Color manipulation

        /// <summary>
        /// Perfoms an histogram transformation on a 8-, 24- or 32-bit image.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="lookUpTable">The lookup table.
        /// It's size is assumed to be 256 in length.</param>
        /// <param name="channel">The color channel to be transformed.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool AdjustCurve(FIBITMAP dib, byte[] lookUpTable, FREE_IMAGE_COLOR_CHANNEL channel)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.AdjustCurve(dib, lookUpTable, channel);
            else
                return FreeImageStaticImportsLinux.AdjustCurve(dib, lookUpTable, channel);
        }

        /// <summary>
        /// Performs gamma correction on a 8-, 24- or 32-bit image.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="gamma">The parameter represents the gamma value to use (gamma > 0).
        /// A value of 1.0 leaves the image alone, less than one darkens it, and greater than one lightens it.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool AdjustGamma(FIBITMAP dib, double gamma)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.AdjustGamma(dib, gamma);
            else
                return FreeImageStaticImportsLinux.AdjustGamma(dib, gamma);
        }

        /// <summary>
        /// Adjusts the brightness of a 8-, 24- or 32-bit image by a certain amount.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="percentage">A value 0 means no change,
        /// less than 0 will make the image darker and greater than 0 will make the image brighter.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool AdjustBrightness(FIBITMAP dib, double percentage)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.AdjustBrightness(dib, percentage);
            else
                return FreeImageStaticImportsLinux.AdjustBrightness(dib, percentage);
        }

        /// <summary>
        /// Adjusts the contrast of a 8-, 24- or 32-bit image by a certain amount.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="percentage">A value 0 means no change,
        /// less than 0 will decrease the contrast and greater than 0 will increase the contrast of the image.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool AdjustContrast(FIBITMAP dib, double percentage)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.AdjustContrast(dib, percentage);
            else
                return FreeImageStaticImportsLinux.AdjustContrast(dib, percentage);
        }

        /// <summary>
        /// Inverts each pixel data.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool Invert(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Invert(dib);
            else
                return FreeImageStaticImportsLinux.Invert(dib);
        }

        /// <summary>
        /// Computes the image histogram.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="histo">Array of integers with a size of 256.</param>
        /// <param name="channel">Channel to compute from.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool GetHistogram(FIBITMAP dib, int[] histo, FREE_IMAGE_COLOR_CHANNEL channel)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetHistogram(dib, histo, channel);
            else
                return FreeImageStaticImportsLinux.GetHistogram(dib, histo, channel);
        }

        #endregion

        #region Channel processing

        /// <summary>
        /// Retrieves the red, green, blue or alpha channel of a 24- or 32-bit image.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="channel">The color channel to extract.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP GetChannel(FIBITMAP dib, FREE_IMAGE_COLOR_CHANNEL channel)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetChannel(dib, channel);
            else
                return FreeImageStaticImportsLinux.GetChannel(dib, channel);
        }

        /// <summary>
        /// Insert a 8-bit dib into a 24- or 32-bit image.
        /// Both images must have to same width and height.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="dib8">Handle to the bitmap to insert.</param>
        /// <param name="channel">The color channel to replace.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SetChannel(FIBITMAP dib, FIBITMAP dib8, FREE_IMAGE_COLOR_CHANNEL channel)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetChannel(dib, dib8, channel);
            else
                return FreeImageStaticImportsLinux.SetChannel(dib, dib8, channel);
        }

        /// <summary>
        /// Retrieves the real part, imaginary part, magnitude or phase of a complex image.
        /// </summary>
        /// <param name="src">Handle to a FreeImage bitmap.</param>
        /// <param name="channel">The color channel to extract.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP GetComplexChannel(FIBITMAP src, FREE_IMAGE_COLOR_CHANNEL channel)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetComplexChannel(src, channel);
            else
                return FreeImageStaticImportsLinux.GetComplexChannel(src, channel);
        }

        /// <summary>
        /// Set the real or imaginary part of a complex image.
        /// Both images must have to same width and height.
        /// </summary>
        /// <param name="dst">Handle to a FreeImage bitmap.</param>
        /// <param name="src">Handle to a FreeImage bitmap.</param>
        /// <param name="channel">The color channel to replace.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool SetComplexChannel(FIBITMAP dst, FIBITMAP src, FREE_IMAGE_COLOR_CHANNEL channel)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SetComplexChannel(dst, src, channel);
            else
                return FreeImageStaticImportsLinux.SetComplexChannel(dst, src, channel);
        }

        #endregion

        #region Copy / Paste / Composite routines

        /// <summary>
        /// Copy a sub part of the current dib image.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="left">Specifies the left position of the cropped rectangle.</param>
        /// <param name="top">Specifies the top position of the cropped rectangle.</param>
        /// <param name="right">Specifies the right position of the cropped rectangle.</param>
        /// <param name="bottom">Specifies the bottom position of the cropped rectangle.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP Copy(FIBITMAP dib, int left, int top, int right, int bottom)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Copy(dib, left, top, right, bottom);
            else
                return FreeImageStaticImportsLinux.Copy(dib, left, top, right, bottom);
        }

        /// <summary>
        /// Alpha blend or combine a sub part image with the current dib image.
        /// The bit depth of the dst bitmap must be greater than or equal to the bit depth of the src.
        /// </summary>
        /// <param name="dst">Handle to a FreeImage bitmap.</param>
        /// <param name="src">Handle to a FreeImage bitmap.</param>
        /// <param name="left">Specifies the left position of the sub image.</param>
        /// <param name="top">Specifies the top position of the sub image.</param>
        /// <param name="alpha">alpha blend factor.
        /// The source and destination images are alpha blended if alpha=0..255.
        /// If alpha > 255, then the source image is combined to the destination image.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool Paste(FIBITMAP dst, FIBITMAP src, int left, int top, int alpha)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Paste(dst, src, left, top, alpha);
            else
                return FreeImageStaticImportsLinux.Paste(dst, src, left, top, alpha);
        }

        /// <summary>
        /// This function composite a transparent foreground image against a single background color or
        /// against a background image.
        /// </summary>
        /// <param name="fg">Handle to a FreeImage bitmap.</param>
        /// <param name="useFileBkg">When true the background of fg is used if it contains one.</param>
        /// <param name="appBkColor">The application background is used if useFileBkg is false.</param>
        /// <param name="bg">Image used as background when useFileBkg is false or fg has no background
        /// and appBkColor is null.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP Composite(FIBITMAP fg, bool useFileBkg, ref RGBQUAD appBkColor, FIBITMAP bg)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Composite(fg, useFileBkg, ref appBkColor, bg);
            else
                return FreeImageStaticImportsLinux.Composite(fg, useFileBkg, ref appBkColor, bg);
        }

        /// <summary>
        /// This function composite a transparent foreground image against a single background color or
        /// against a background image.
        /// </summary>
        /// <param name="fg">Handle to a FreeImage bitmap.</param>
        /// <param name="useFileBkg">When true the background of fg is used if it contains one.</param>
        /// <param name="appBkColor">The application background is used if useFileBkg is false
        /// and 'appBkColor' is not null.</param>
        /// <param name="bg">Image used as background when useFileBkg is false or fg has no background
        /// and appBkColor is null.</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP Composite(FIBITMAP fg, bool useFileBkg, RGBQUAD[] appBkColor, FIBITMAP bg)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.Composite(fg, useFileBkg, appBkColor, bg);
            else
                return FreeImageStaticImportsLinux.Composite(fg, useFileBkg, appBkColor, bg);
        }

        /// <summary>
        /// Performs a lossless crop on a JPEG file.
        /// </summary>
        /// <param name="src_file">Source filename.</param>
        /// <param name="dst_file">Destination filename.</param>
        /// <param name="left">Specifies the left position of the cropped rectangle.</param>
        /// <param name="top">Specifies the top position of the cropped rectangle.</param>
        /// <param name="right">Specifies the right position of the cropped rectangle.</param>
        /// <param name="bottom">Specifies the bottom position of the cropped rectangle.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool JPEGCrop(string src_file, string dst_file, int left, int top, int right, int bottom)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.JPEGCrop(src_file, dst_file, left, top, right, bottom);
            else
                return FreeImageStaticImportsLinux.JPEGCrop(src_file, dst_file, left, top, right, bottom);
        }

        /// <summary>
        /// Applies the alpha value of each pixel to its color components.
        /// The aplha value stays unchanged.
        /// Only works with 32-bits color depth.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool PreMultiplyWithAlpha(FIBITMAP dib)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.PreMultiplyWithAlpha(dib);
            else
                return FreeImageStaticImportsLinux.PreMultiplyWithAlpha(dib);
        }

        #endregion

        #region Miscellaneous algorithms

        /// <summary>
        /// Solves a Poisson equation, remap result pixels to [0..1] and returns the solution.
        /// </summary>
        /// <param name="Laplacian">Handle to a FreeImage bitmap.</param>
        /// <param name="ncycle">Number of cycles in the multigrid algorithm (usually 2 or 3)</param>
        /// <returns>Handle to a FreeImage bitmap.</returns>
        public static FIBITMAP MultigridPoissonSolver(FIBITMAP Laplacian, int ncycle)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.MultigridPoissonSolver(Laplacian, ncycle);
            else
                return FreeImageStaticImportsLinux.MultigridPoissonSolver(Laplacian, ncycle);
        }

        #endregion

        #region Colors

        /// <summary>
        /// Creates a lookup table to be used with <see cref="AdjustCurve"/> which may adjusts brightness and
        /// contrast, correct gamma and invert the image with a single call to <see cref="AdjustCurve"/>.
        /// </summary>
        /// <param name="lookUpTable">Output lookup table to be used with <see cref="AdjustCurve"/>.
        /// The size of 'lookUpTable' is assumed to be 256.</param>
        /// <param name="brightness">Percentage brightness value where -100 &lt;= brightness &lt;= 100.
        /// <para>A value of 0 means no change, less than 0 will make the image darker and greater
        /// than 0 will make the image brighter.</para></param>
        /// <param name="contrast">Percentage contrast value where -100 &lt;= contrast &lt;= 100.
        /// <para>A value of 0 means no change, less than 0 will decrease the contrast
        /// and greater than 0 will increase the contrast of the image.</para></param>
        /// <param name="gamma">Gamma value to be used for gamma correction.
        /// <para>A value of 1.0 leaves the image alone, less than one darkens it,
        /// and greater than one lightens it.</para></param>
        /// <param name="invert">If set to true, the image will be inverted.</param>
        /// <returns>The number of adjustments applied to the resulting lookup table
        /// compared to a blind lookup table.</returns>
        /// <remarks>
        /// This function creates a lookup table to be used with <see cref="AdjustCurve"/> which may adjust
        /// brightness and contrast, correct gamma and invert the image with a single call to
        /// <see cref="AdjustCurve"/>. If more than one of these image display properties need to be adjusted,
        /// using a combined lookup table should be preferred over calling each adjustment function
        /// separately. That's particularly true for huge images or if performance is an issue. Then,
        /// the expensive process of iterating over all pixels of an image is performed only once and
        /// not up to four times.
        /// <para/>
        /// Furthermore, the lookup table created does not depend on the order, in which each single
        /// adjustment operation is performed. Due to rounding and byte casting issues, it actually
        /// matters in which order individual adjustment operations are performed. Both of the following
        /// snippets most likely produce different results:
        /// <para/>
        /// <code>
        /// // snippet 1: contrast, brightness
        /// AdjustContrast(dib, 15.0);
        /// AdjustBrightness(dib, 50.0); 
        /// </code>
        /// <para/>
        /// <code>
        /// // snippet 2: brightness, contrast
        /// AdjustBrightness(dib, 50.0);
        /// AdjustContrast(dib, 15.0);
        /// </code>
        /// <para/>
        /// Better and even faster would be snippet 3:
        /// <para/>
        /// <code>
        /// // snippet 3:
        /// byte[] lut = new byte[256];
        /// GetAdjustColorsLookupTable(lut, 50.0, 15.0, 1.0, false);
        /// AdjustCurve(dib, lut, FREE_IMAGE_COLOR_CHANNEL.FICC_RGB);
        /// </code>
        /// <para/>
        /// This function is also used internally by <see cref="AdjustColors"/>, which does not return the
        /// lookup table, but uses it to call <see cref="AdjustCurve"/> on the passed image.
        /// </remarks>
        public static int GetAdjustColorsLookupTable(byte[] lookUpTable, double brightness, double contrast, double gamma, bool invert)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.GetAdjustColorsLookupTable(lookUpTable, brightness, contrast, gamma, invert);
            else
                return FreeImageStaticImportsLinux.GetAdjustColorsLookupTable(lookUpTable, brightness, contrast, gamma, invert);
        }

        /// <summary>
        /// Adjusts an image's brightness, contrast and gamma as well as it may
        /// optionally invert the image within a single operation.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="brightness">Percentage brightness value where -100 &lt;= brightness &lt;= 100.
        /// <para>A value of 0 means no change, less than 0 will make the image darker and greater
        /// than 0 will make the image brighter.</para></param>
        /// <param name="contrast">Percentage contrast value where -100 &lt;= contrast &lt;= 100.
        /// <para>A value of 0 means no change, less than 0 will decrease the contrast
        /// and greater than 0 will increase the contrast of the image.</para></param>
        /// <param name="gamma">Gamma value to be used for gamma correction.
        /// <para>A value of 1.0 leaves the image alone, less than one darkens it,
        /// and greater than one lightens it.</para>
        /// This parameter must not be zero or smaller than zero.
        /// If so, it will be ignored and no gamma correction will be performed on the image.</param>
        /// <param name="invert">If set to true, the image will be inverted.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        /// <remarks>
        /// This function adjusts an image's brightness, contrast and gamma as well as it
        /// may optionally invert the image within a single operation. If more than one of
        /// these image display properties need to be adjusted, using this function should
        /// be preferred over calling each adjustment function separately. That's particularly
        /// true for huge images or if performance is an issue.
        /// <para/>
        /// This function relies on <see cref="GetAdjustColorsLookupTable"/>,
        /// which creates a single lookup table, that combines all adjustment operations requested.
        /// <para/>
        /// Furthermore, the lookup table created by <see cref="GetAdjustColorsLookupTable"/> does
        /// not depend on the order, in which each single adjustment operation is performed.
        /// Due to rounding and byte casting issues, it actually matters in which order individual
        /// adjustment operations are performed. Both of the following snippets most likely produce
        /// different results:
        /// <para/>
        /// <code>
        /// // snippet 1: contrast, brightness
        /// AdjustContrast(dib, 15.0);
        /// AdjustBrightness(dib, 50.0);
        /// </code>
        /// <para/>
        /// <code>
        /// // snippet 2: brightness, contrast
        /// AdjustBrightness(dib, 50.0);
        /// AdjustContrast(dib, 15.0);
        /// </code>
        /// <para/>
        /// Better and even faster would be snippet 3:
        /// <para/>
        /// <code>
        /// // snippet 3:
        /// AdjustColors(dib, 50.0, 15.0, 1.0, false);
        /// </code>
        /// </remarks>
        public static bool AdjustColors(FIBITMAP dib, double brightness, double contrast, double gamma, bool invert)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.AdjustColors(dib, brightness, contrast, gamma, invert);
            else
                return FreeImageStaticImportsLinux.AdjustColors(dib, brightness, contrast, gamma, invert);
        }

        /// <summary>
        /// Applies color mapping for one or several colors on a 1-, 4- or 8-bit
        /// palletized or a 16-, 24- or 32-bit high color image.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="srccolors">Array of colors to be used as the mapping source.</param>
        /// <param name="dstcolors">Array of colors to be used as the mapping destination.</param>
        /// <param name="count">The number of colors to be mapped. This is the size of both
        /// srccolors and dstcolors.</param>
        /// <param name="ignore_alpha">If true, 32-bit images and colors are treated as 24-bit.</param>
        /// <param name="swap">If true, source and destination colors are swapped, that is,
        /// each destination color is also mapped to the corresponding source color.</param>
        /// <returns>The total number of pixels changed.</returns>
        /// <remarks>
        /// This function maps up to <paramref name="count"/> colors specified in
        /// <paramref name="srccolors"/> to these specified in <paramref name="dstcolors"/>.
        /// Thereby, color <i>srccolors[N]</i>, if found in the image, will be replaced by color
        /// <i>dstcolors[N]</i>. If <paramref name="swap"/> is <b>true</b>, additionally all colors
        /// specified in <paramref name="dstcolors"/> are also mapped to these specified
        /// in <paramref name="srccolors"/>. For high color images, the actual image data will be
        /// modified whereas, for palletized images only the palette will be changed.
        /// <para/>
        /// The function returns the number of pixels changed or zero, if no pixels were changed. 
        /// <para/>
        /// Both arrays <paramref name="srccolors"/> and <paramref name="dstcolors"/> are assumed
        /// not to hold less than <paramref name="count"/> colors.
        /// <para/>
        /// For 16-bit images, all colors specified are transparently converted to their 
        /// proper 16-bit representation (either in RGB555 or RGB565 format, which is determined
        /// by the image's red- green- and blue-mask).
        /// <para/>
        /// <b>Note, that this behaviour is different from what <see cref="ApplyPaletteIndexMapping"/> does,
        /// which modifies the actual image data on palletized images.</b>
        /// </remarks>
        public static uint ApplyColorMapping(FIBITMAP dib, RGBQUAD[] srccolors, RGBQUAD[] dstcolors, uint count, bool ignore_alpha, bool swap)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ApplyColorMapping(dib, srccolors, dstcolors, count, ignore_alpha, swap);
            else
                return FreeImageStaticImportsLinux.ApplyColorMapping(dib, srccolors, dstcolors, count, ignore_alpha, swap);
        }

        /// <summary>
        /// Swaps two specified colors on a 1-, 4- or 8-bit palletized
        /// or a 16-, 24- or 32-bit high color image.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="color_a">One of the two colors to be swapped.</param>
        /// <param name="color_b">The other of the two colors to be swapped.</param>
        /// <param name="ignore_alpha">If true, 32-bit images and colors are treated as 24-bit.</param>
        /// <returns>The total number of pixels changed.</returns>
        /// <remarks>
        /// This function swaps the two specified colors <paramref name="color_a"/> and
        /// <paramref name="color_b"/> on a palletized or high color image.
        /// For high color images, the actual image data will be modified whereas, for palletized
        /// images only the palette will be changed.
        /// <para/>
        /// <b>Note, that this behaviour is different from what <see cref="SwapPaletteIndices"/> does,
        /// which modifies the actual image data on palletized images.</b>
        /// <para/>
        /// This is just a thin wrapper for <see cref="ApplyColorMapping"/> and resolves to:
        /// <para/>
        /// <code>
        /// return ApplyColorMapping(dib, color_a, color_b, 1, ignore_alpha, true);
        /// </code>
        /// </remarks>
        public static uint SwapColors(FIBITMAP dib, ref RGBQUAD color_a, ref RGBQUAD color_b, bool ignore_alpha)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SwapColors(dib, ref color_a, ref color_b, ignore_alpha);
            else
                return FreeImageStaticImportsLinux.SwapColors(dib, ref color_a, ref color_b, ignore_alpha);
        }

        /// <summary>
        /// Applies palette index mapping for one or several indices
        /// on a 1-, 4- or 8-bit palletized image.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="srcindices">Array of palette indices to be used as the mapping source.</param>
        /// <param name="dstindices">Array of palette indices to be used as the mapping destination.</param>
        /// <param name="count">The number of palette indices to be mapped. This is the size of both
        /// srcindices and dstindices</param>
        /// <param name="swap">If true, source and destination palette indices are swapped, that is,
        /// each destination index is also mapped to the corresponding source index.</param>
        /// <returns>The total number of pixels changed.</returns>
        /// <remarks>
        /// This function maps up to <paramref name="count"/> palette indices specified in
        /// <paramref name="srcindices"/> to these specified in <paramref name="dstindices"/>.
        /// Thereby, index <i>srcindices[N]</i>, if present in the image, will be replaced by index
        /// <i>dstindices[N]</i>. If <paramref name="swap"/> is <b>true</b>, additionally all indices
        /// specified in <paramref name="dstindices"/> are also mapped to these specified in 
        /// <paramref name="srcindices"/>.
        /// <para/>
        /// The function returns the number of pixels changed or zero, if no pixels were changed.
        /// Both arrays <paramref name="srcindices"/> and <paramref name="dstindices"/> are assumed not to
        /// hold less than <paramref name="count"/> indices.
        /// <para/>
        /// <b>Note, that this behaviour is different from what <see cref="ApplyColorMapping"/> does, which
        /// modifies the actual image data on palletized images.</b>
        /// </remarks>
        public static uint ApplyPaletteIndexMapping(FIBITMAP dib, byte[] srcindices, byte[] dstindices, uint count, bool swap)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.ApplyPaletteIndexMapping(dib, srcindices, dstindices, count, swap);
            else
                return FreeImageStaticImportsLinux.ApplyPaletteIndexMapping(dib, srcindices, dstindices, count, swap);
        }

        /// <summary>
        /// Swaps two specified palette indices on a 1-, 4- or 8-bit palletized image.
        /// </summary>
        /// <param name="dib">Handle to a FreeImage bitmap.</param>
        /// <param name="index_a">One of the two palette indices to be swapped.</param>
        /// <param name="index_b">The other of the two palette indices to be swapped.</param>
        /// <returns>The total number of pixels changed.</returns>
        /// <remarks>
        /// This function swaps the two specified palette indices <i>index_a</i> and
        /// <i>index_b</i> on a palletized image. Therefore, not the palette, but the
        /// actual image data will be modified.
        /// <para/>
        /// <b>Note, that this behaviour is different from what <see cref="SwapColors"/> does on palletized images,
        /// which only swaps the colors in the palette.</b>
        /// <para/>
        /// This is just a thin wrapper for <see cref="ApplyColorMapping"/> and resolves to:
        /// <para/>
        /// <code>
        /// return ApplyPaletteIndexMapping(dib, index_a, index_b, 1, true);
        /// </code>
        /// </remarks>
        public static uint SwapPaletteIndices(FIBITMAP dib, ref byte index_a, ref byte index_b)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.SwapPaletteIndices(dib, ref index_a, ref index_b   );
            else
                return FreeImageStaticImportsLinux.SwapPaletteIndices(dib, ref index_a, ref index_b);
        }

        internal static bool FillBackground(FIBITMAP dib, IntPtr color, FREE_IMAGE_COLOR_OPTIONS options)
        {
            if (!IsLinux)
                return FreeImageStaticImportsDefault.FillBackground(dib, color, options);
            else
                return FreeImageStaticImportsLinux.FillBackground(dib, color, options);
        }

        #endregion
    }
}