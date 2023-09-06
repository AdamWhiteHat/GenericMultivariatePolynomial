using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;
using TestGenericMultivariatePolynomial;

namespace TestGenericMultivariatePolynomial
{
	[TestOf(typeof(BigInteger))]
	[TestFixture(Category = "Arithmetic - BigInteger")]
	public class Arithmetic_BigInteger : Arithmetic<BigInteger>
	{
	}
}
