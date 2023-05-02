using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(Complex))]
	[TestFixture(Category = "Arithmetic - Complex")]
	public class Arithmetic_Complex : Arithmetic<Complex>
	{

		[Test]
		[TestCase("X^2 + 2*X - 1", "2*X^2 - 3*X + 6", "(3, 0)*X^2 - X + (5, 0)")]
		public override void TestAdd(string augend, string addend, string expected)
		{
			base.TestAdd(augend, addend, expected);
		}

		[Test]
		[TestCase("X^2", "6", "X^2 + (6, 0)")]
		public override void TestAddUnlikeTerms(string augend, string addend, string expected)
		{
			base.TestAddUnlikeTerms(augend, addend, expected);
		}

		[Test]
		[TestCase("X^2", "6", "X^2 + (-6, 0)")]
		public override void TestSubtractUnlikeTerms(string minuend, string subtrahend, string expected)
		{
			base.TestSubtractUnlikeTerms(minuend, subtrahend, expected);
		}

		[Test]
		[TestCase("3*X", "X + 2", "(2, 0)*X + (-2, 0)")]
		public override void TestSubtract1(string minuend, string subtrahend, string expected)
		{
			base.TestSubtract1(minuend, subtrahend, expected);
		}

		[Test]
		[TestCase("36*X*Y + 6*X + 6*Y + 1", "36*X*Y + 1", "(6, 0)*X + (6, 0)*Y")]
		public override void TestSubtract2(string minuend, string subtrahend, string expected)
		{
			base.TestSubtract2(minuend, subtrahend, expected);
		}

		[Test]
		[TestCase("2*X^3 + 2*X - 1", "2*X^2 - 5*X - 6", "(2, 0)*X^3 + (-2, 0)*X^2 + (7, 0)*X + (5, 0)")]
		public override void TestSubtract3(string minuend, string subtrahend, string expected)
		{
			base.TestSubtract3(minuend, subtrahend, expected);
		}

		[Test]
		[TestCase("3*X^2*Y^3 + 2*X^3*Y^2 + 6*X*Y^2 + 4*X^3 - 6*X^2*Y + 3*X*Y - 2*X^2 + 12*X - 6", "X^3*Y^2 + 3*X^2 - 3*Y^2 - 12*X - 2", "(3, 0)*X^2*Y^3 + X^3*Y^2 + (4, 0)*X^3 + (6, 0)*X*Y^2 + (-6, 0)*X^2*Y + (3, 0)*Y^2 + (-5, 0)*X^2 + (3, 0)*X*Y + (24, 0)*X + (-4, 0)")]
		public override void TestSubtract4(string minuend, string subtrahend, string expected)
		{
			base.TestSubtract4(minuend, subtrahend, expected);
		}

		[Test]
		[TestCase("504*X*Y*Z^2 + 216*X*Y - 42*X*Z^2 - 18*X + 84*Y*Z^2 + 36*Y - 7*Z^2 - 3", "X*Y*Z^2 + 42*X*Z^2 - 8*X - X^2 - 3", "(503, 0)*X*Y*Z^2 + (84, 0)*Y*Z^2 + (-84, 0)*X*Z^2 + X^2 + (-7, 0)*Z^2 + (216, 0)*X*Y + (36, 0)*Y + (-10, 0)*X")]
		public override void TestSubtract5(string minuend, string subtrahend, string expected)
		{
			base.TestSubtract5(minuend, subtrahend, expected);
		}

		[Test]
		[TestCase("X^3 + 7*X^2 + 14*X + 8", "X + 1", "X^2 + 6*X + (8, 0)", "X^2 + (2, 0)")]
		public override void TestSubtract6(string dividend, string unit, string expectedQuotient, string expected)
		{
			base.TestSubtract6(dividend, unit, expectedQuotient, expected);
		}

		[Test]
		[TestCase("6*X + 1", "6*Y + 1", "(36, 0)*X*Y + (6, 0)*X + (6, 0)*Y + (1, 0)")]
		public override void TestMultiply1(string lhs, string rhs, string expected)
		{
			base.TestMultiply1(lhs, rhs, expected);
		}

		[Test]
		[TestCase("6*X + 1", "6*X - 1", "(36, 0)*X^2 + (-1, 0)")]
		public override void TestMultiplySameSymbols(string lhs, string rhs, string expected)
		{
			base.TestMultiplySameSymbols(lhs, rhs, expected);
		}

		[Test]
		[TestCase("36*X*Y + 6*X + 6*Y + 1", "6*X + 1", "(6, 0)*Y + (1, 0)")]
		public override void TestDivide1(string dividend, string divisor, string expected)
		{
			//base.TestDivide1(dividend, divisor, expected);
		}

		[Test]
		[TestCase("2*X*Y^2 + 3*X*Y + 4*Y^2 + 6*Y", "X + 2", "(2, 0)*Y^2 + (3, 0)*Y")]
		public override void TestDivide2(string dividend, string divisor, string expected)
		{
			//base.TestDivide2(dividend, divisor, expected);
		}

		[Test]
		[TestCase("(1, 0)", "a", "(2, 0)")]
		public override void TestDivide_UserSubmittedIssueNo1(string expected1, string expected2, string expected3)
		{
			base.TestDivide_UserSubmittedIssueNo1(expected1, expected2, expected3);
		}

		[Test]
		[TestCase("2*X*Y^2 - 1", "2", "(4, 0)*X^2*Y^4 + (-4, 0)*X*Y^2 + (1, 0)")]
		public override void TestPow(string powerBase, string exponent, string expected)
		{
			base.TestPow(powerBase, exponent, expected);
		}

		[Test]
		[TestCase("132*X*Y + 77*X + 55*Y + 1", "(132, 0)*Y + (77, 0)")]
		public override void TestGetDerivative1(string polynomial, string expected)
		{
			base.TestGetDerivative1(polynomial, expected);
		}

		[Test]
		[TestCase("4*X^2*Y^4 - 4*X*Y^2 + 1", "(8, 0)*X*Y^4 + (-4, 0)*Y^2")]
		public override void TestGetDerivative2(string polynomial, string expected)
		{
			base.TestGetDerivative2(polynomial, expected);
		}

		public override void TestFactorization1(string polynomialToFactor, params string[] expected)
		{
			base.TestFactorization1(polynomialToFactor, expected);
		}

		public override void TestFactorization2(string polynomialToFactor, params string[] expected)
		{
			base.TestFactorization2(polynomialToFactor, expected);
		}

		public override void TestFactorization_Irreducible(string irreduciblePolynomial)
		{
			base.TestFactorization_Irreducible(irreduciblePolynomial);
		}

		public override void TestGCD_Univarite_1(string polyString1, string polyString2, string expected)
		{
			base.TestGCD_Univarite_1(polyString1, polyString2, expected);
		}

		public override void TestGCD_Univarite_2(string polyString1, string polyString2, string expected)
		{
			base.TestGCD_Univarite_2(polyString1, polyString2, expected);
		}

		public override void TestGCD_Univarite_3(string polyString1, string polyString2, string expected)
		{
			base.TestGCD_Univarite_3(polyString1, polyString2, expected);
		}

		public override void TestGCD(string polynomial1, string polynomial2, string expected)
		{
			base.TestGCD(polynomial1, polynomial2, expected);
		}

		//[Test]
		//[TestCase("(6, 0)*X + (1, 0)", "(2, 0)*Y", "(12, 0)*Y + (1, 0)")]
		public override void TestFunctionalComposition_f_x_001(string f, string x, string expecting)
		{
			//base.TestFunctionalComposition_f_x_001(f, x, expecting);
		}

		//[Test]
		//[TestCase("6*X + 1", "6*Y - 1", "(36, 0)*Y + (-5, 0)")]
		public override void TestFunctionalComposition_f_x_002(string f, string x, string expecting)
		{
			//base.TestFunctionalComposition_f_x_002(f, x, expecting);
		}

		//[Test]
		//[TestCase("(36, 0)*X*Y - (6, 0)*X + (6, 0)*Y - (1, 0)", "(0, 0)", "(-1, 0)", "(-7, 0)")]
		public override void TestFunctionalComposition_f_xy_001(string f, string x, string y, string expecting)
		{
			//base.TestFunctionalComposition_f_xy_001(f, x, y, expecting);
		}

		//[Test]
		//[TestCase("-1*X^2 + 5", "2*X + 3", "X + 3", "(-4, 0)*X^2 + (-36, 0)*X + (-76, 0)")]
		public override void TestFunctionalComposition_fg_x_001(string f, string g, string x, string expecting)
		{
			//base.TestFunctionalComposition_fg_x_001(f, g, x, expecting);
		}

		[Test]
		public void TestRewriteComplexPolynomial()
		{
			string input = "-(3, 0)*X - Y - (1, 0)";
			string expected = "(-3,0)*X+(-1,0)*Y+(-1,0)";

			string actual = MultivariatePolynomial<Complex>.RewriteComplexPolynomial(input);

			TestContext.WriteLine($"Expected: {expected}");
			TestContext.WriteLine($"  Actual: {actual}");

			var polynomial = MultivariatePolynomial<Complex>.Parse(actual);
			string polynomialString = polynomial.ToString();

			TestContext.WriteLine();
			TestContext.WriteLine($"Polynomial.ToString(): {polynomialString}");

			Assert.AreEqual(expected, actual);
		}
	}
}
