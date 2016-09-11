using System;

namespace FreeImageAPI
{
	public class FreeImageException : Exception
	{
		public FreeImageException()
			: this(FreeImageEngine.LastErrorMessage)
		{
		}

		public FreeImageException(string message)
			: this(message, null)
		{
		}

		public FreeImageException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}

