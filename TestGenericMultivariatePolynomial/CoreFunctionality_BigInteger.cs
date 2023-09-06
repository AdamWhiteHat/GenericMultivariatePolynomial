using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;
using TestGenericMultivariatePolynomial;

namespace TestGenericMultivariatePolynomial
{
	[TestOf(typeof(BigInteger))]
	[TestFixture(Category = "CoreFunctionality - BigInteger")]
	public class CoreFunctionality_BigInteger : CoreFunctionality<BigInteger>
	{
	}
}
