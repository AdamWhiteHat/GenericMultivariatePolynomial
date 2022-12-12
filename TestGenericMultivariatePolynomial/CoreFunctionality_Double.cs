using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(double))]
	[TestFixture(Category = "CoreFunctionality - Double")]
	public class CoreFunctionality_Double : CoreFunctionality<double>
	{
		[Test]
		[TestCase("2*x^4 + 13*y^3 + 29*x^2 + 29*y + 13", "45468", "63570", "8.551120982818029E+18")]
		public override void TestEvaluate(string polyString, string xValue, string yValue, string expected)
		{
			base.TestEvaluate(polyString, xValue, yValue, expected);
		}
	}
}
