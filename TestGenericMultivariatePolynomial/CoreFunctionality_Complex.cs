using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(Complex))]
	[TestFixture(Category = "CoreFunctionality - Complex")]
	public class CoreFunctionality_Complex : CoreFunctionality<Complex>
	{
		[Test]
		[TestCase("X*Y*Z^2 + X*Y + X*Z + Y*Z - 1", "X*Y*Z^2 + X*Y + X*Z + Y*Z + (-1, 0)")]
		public override void TestParse001(string toTest, string expected)
		{
			base.TestParse001(toTest, expected);
		}

		[Test]
		[TestCase("(6, 0)*Y", "Y + (6, 0)", "(6, 0)*Y + (6, 0)")]

		public override void TestParse002(string expected1, string expected2, string expected3)
		{
			base.TestParse002(expected1, expected2, expected3);
		}


		[Test]
		[TestCase("12", "(12, 0)")]
		public override void TestParse_ConstantPolynomial001(string toTest, string expected)
		{
			base.TestParse_ConstantPolynomial001(toTest, expected);
		}

		[Test]
		[TestCase("-12", "(-12, 0)")]
		public override void TestParse_ConstantPolynomial002(string toTest, string expected)
		{
			base.TestParse_ConstantPolynomial002(toTest, expected);
		}

		[Test]
		[TestCase("-8*X^2 - X + 8", "(-8, 0)*X^2 - X + (8, 0)", "(-8, 0)*X^2", "-X")]
		public override void TestParseNegativeLeadingCoefficient(string polynomial, string polynomialExpected, string leadingTermExpected, string secondTermExpected)
		{
			base.TestParseNegativeLeadingCoefficient(polynomial, polynomialExpected, leadingTermExpected, secondTermExpected);
		}

		[Test]
		[TestCase("2*x^4 + 13*y^3 + 29*x^2 + 29*y + 13", "45468", "63570", "(8.551120982818029E+18, 0)")]
		public override void TestEvaluate(string polyString, string xValue, string yValue, string expected)
		{
			base.TestEvaluate(polyString, xValue, yValue, expected);
		}

		[Test]
		[TestCase("3*X^2*Y^3 + 6*X* Y^4 + X^3*Y^2 + 4*X^5 - 6*X^2*Y + 3*X* Y*Z - 5*X^2 + 3*Y^3 + 24*X* Y - 4", "(4, 0)*X^5 + (6, 0)*X*Y^4 + (3, 0)*X^2*Y^3 + X^3*Y^2 + (3, 0)*Y^3 + (-6, 0)*X^2*Y + (3, 0)*X*Y*Z + (-5, 0)*X^2 + (24, 0)*X*Y + (-4, 0)")]
		public override void TestMonomialOrdering(string toParse, string expected)
		{
			base.TestMonomialOrdering(toParse, expected);
		}
	}
}
