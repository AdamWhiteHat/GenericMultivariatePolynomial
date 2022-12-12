using System;
using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(Int64))]
	[TestFixture(Category = "CoreFunctionality - UInt64")]
	public class CoreFunctionality_Int64 : CoreFunctionality<Int64>
	{
		[Test]
		[TestCase("2*x^4 + 13*y^3 + 29*x^2 + 29*y + 13", "4546", "63570", "4193819859020819")]
		public override void TestEvaluate(string polyString, string xValue, string yValue, string expected)
		{
			base.TestEvaluate(polyString, xValue, yValue, expected);
		}
	}
}
