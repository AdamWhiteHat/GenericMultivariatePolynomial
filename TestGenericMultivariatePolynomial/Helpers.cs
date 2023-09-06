using System.Globalization;
using System.Linq;
using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;

namespace TestGenericMultivariatePolynomial
{
	public static class Helpers
	{
		public static class Complex
		{
			public static string AutoConvertStringFormat(string complexToString)
			{
				bool useNewStringFormat = TargetFramework.IsNet7_0OrGreater();

				string result = complexToString;
				if (useNewStringFormat)
				{
					result = ConvertToNewStringFormat(result);
				}
				else
				{
					result = ConvertToOldStringFormat(result);
				}
				return result;
			}

			public static string ConvertToNewStringFormat(string complexToString)
			{
				string result = complexToString;
				result = result.Replace("(", "<");
				result = result.Replace(")", ">");
				result = result.Replace(",", ";");
				return result;
			}

			public static string ConvertToOldStringFormat(string complexToString)
			{
				string result = complexToString;
				result = result.Replace("<", "(");
				result = result.Replace(">", ")");
				result = result.Replace(";", ",");
				return result;
			}
		}


		public static class TargetFramework
		{
			public static bool IsNet7_0OrGreater()
			{
				bool result = false;
#if NET7_0_OR_GREATER
				result = true;
#endif
				return result;
			}

			public static bool IsNetCore3_1OrGreater()
			{
				bool result = false;
#if NETCOREAPP3_1_OR_GREATER
				result = true;
#endif
				return result;
			}
		}
	}
}
