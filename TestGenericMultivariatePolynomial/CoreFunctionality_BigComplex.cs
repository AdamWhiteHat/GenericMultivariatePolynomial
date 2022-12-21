using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(BigComplex))]
	[TestFixture(Category = "CoreFunctionality - BigComplex")]
	public class CoreFunctionality_BigComplex : CoreFunctionality<BigComplex>
	{
		[Test]
		[TestCase("2*x^4 + 13*y^3 + 29*x^2 + 29*y + 13", "45468", "63570", "8551120982818028879")]
		public override void TestEvaluate(string polyString, string xValue, string yValue, string expected)
		{
			base.TestEvaluate(polyString, xValue, yValue, expected);
		}
	}
}
