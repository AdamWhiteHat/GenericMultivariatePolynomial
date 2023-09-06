using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using ExtendedNumerics.ExtensionMethods;
using NUnit.Framework;
using TestGenericMultivariatePolynomial;

namespace TestGenericMultivariatePolynomial
{
	[TestOf(typeof(BigComplex))]
	[TestFixture(Category = "Arithmetic - BigComplex")]
	public class Arithmetic_BigComplex : Arithmetic<BigComplex>
	{
		[Test]
		[TestCase("36*X*Y + 6*X + 6*Y + 1", "6*X + 1", "6*Y + 1")]
		public override void TestDivide1(string dividend, string divisor, string expected)
		{
			//base.TestDivide1(dividend, divisor, expected);
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
	}
}
