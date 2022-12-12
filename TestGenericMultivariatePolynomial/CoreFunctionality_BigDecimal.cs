using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(BigDecimal))]
	[TestFixture(Category = "CoreFunctionality - BigDecimal")]
	public class CoreFunctionality_BigDecimal : CoreFunctionality<BigDecimal>
	{
	}
}
