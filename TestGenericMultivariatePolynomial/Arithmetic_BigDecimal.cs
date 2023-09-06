using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using TestGenericMultivariatePolynomial;

namespace TestGenericMultivariatePolynomial
{
	[TestOf(typeof(BigDecimal))]
	[TestFixture(Category = "Arithmetic - BigDecimal")]
	public class Arithmetic_BigDecimal : Arithmetic<BigDecimal>
	{
		public override void TestFactorization1(string polynomialToFactor, string[] expected)
		{
			//base.TestFactorization1(polynomialToFactor, expected);
		}

		public override void TestFactorization2(string polynomialToFactor, string[] expected)
		{
			//base.TestFactorization2(polynomialToFactor, expected);
		}
	}
}
