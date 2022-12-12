using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(BigInteger))]
	[TestFixture(Category = "Arithmetic - BigInteger")]
	public class Arithmetic_BigInteger : Arithmetic<BigInteger>
	{
	}
}
