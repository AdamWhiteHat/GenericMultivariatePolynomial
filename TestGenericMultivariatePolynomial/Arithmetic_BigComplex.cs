using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using ExtendedNumerics.ExtensionMethods;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
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
	}
}
