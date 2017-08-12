using System;
using System.Collections.Generic;
using FreeImageNETUnitTest.TestFixtures;

namespace FreeImageNETUnitTest
{
	public class Program
	{
		static ImageManager iManager = new ImageManager();
		static ImportedFunctionsTest ift = new ImportedFunctionsTest();
		static ImportedStructsTest ist = new ImportedStructsTest();
		static WrapperStructsTest wst = new WrapperStructsTest();
		static WrapperFunctionsTest wft = new WrapperFunctionsTest();
		static FreeImageBitmapTest fib = new FreeImageBitmapTest();

		public static void Main()
		{
			List<TestClass> classList = new List<TestClass>(5);
			classList.Add(new TestClass(ift));
			classList.Add(new TestClass(ist));
			classList.Add(new TestClass(wst));
			classList.Add(new TestClass(wft));
			classList.Add(new TestClass(fib));

			for (int i = 0; i < 10000; )
			{
				for (int j = 0; j < classList.Count; j++)
					classList[j].ExecuteTests();
				Console.WriteLine("Loop {0}", ++i);
				//GC.Collect();
			}
		}
	}
}