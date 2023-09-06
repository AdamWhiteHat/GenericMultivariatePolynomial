using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using TestGenericMultivariatePolynomial;

namespace TestGenericMultivariatePolynomial
{
	[TestOf(typeof(BigRational))]
	[TestFixture(Category = "Arithmetic - BigRational")]
	public class Arithmetic_BigRational : Arithmetic<BigRational>
	{
	}
}
