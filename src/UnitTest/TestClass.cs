using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace FreeImageNETUnitTest
{
	public class TestClass
	{
		private object classMember = null;

		private MethodInfo classSetUp = null;
		private MethodInfo classTearDown = null;

		private MethodInfo testSetUp = null;
		private MethodInfo testTearDown = null;

		private List<MethodInfo> methodList = null;

		private static object[] parameters = { };

		public TestClass(object classMember)
		{
			this.classMember = classMember;
			MethodInfo[] infos = classMember.GetType().GetTypeInfo().GetMethods(BindingFlags.Public | BindingFlags.Instance);
			methodList = new List<MethodInfo>(infos.Length);

			foreach (MethodInfo info in infos)
			{
				IEnumerable<object> attributes = info.GetCustomAttributes(false);
				foreach (Attribute attribute in attributes)
				{
					if (attribute.GetType() == typeof(TestAttribute))
					{
						methodList.Add(info);
						break;
					}
					else if (attribute.GetType() == typeof(TestFixtureSetUpAttribute))
					{
						classSetUp = info;
						break;
					}
					else if (attribute.GetType() == typeof(TestFixtureTearDownAttribute))
					{
						classTearDown = info;
						break;
					}
					else if (attribute.GetType() == typeof(SetUpAttribute))
					{
						testSetUp = info;
						break;
					}
					else if (attribute.GetType() == typeof(TearDownAttribute))
					{
						testTearDown = info;
						break;
					}
				}
			}
		}

		public void ExecuteTests()
		{
			if (classSetUp != null)
				classSetUp.Invoke(classMember, parameters);

			foreach (MethodInfo method in methodList)
			{
				if (testSetUp != null)
					testSetUp.Invoke(classMember, parameters);

				try
				{
					Console.WriteLine(method.ToString());
					method.Invoke(classMember, parameters);
				}
				catch (Exception ex)
				{
					while (ex.InnerException != null)
						ex = ex.InnerException;
					Console.WriteLine(ex.ToString());
					Environment.Exit(99);
				}

				if (testTearDown != null)
					testTearDown.Invoke(classMember, parameters);
			}

			if (classTearDown != null)
				classTearDown.Invoke(classMember, parameters);
		}
	}
}