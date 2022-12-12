using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(BigDecimal))]
	[TestFixture(Category = "Arithmetic - BigDecimal")]
	public class Arithmetic_BigDecimal : Arithmetic<BigDecimal>
	{
	}
}
