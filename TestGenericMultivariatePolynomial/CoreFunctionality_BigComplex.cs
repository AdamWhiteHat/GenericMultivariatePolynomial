using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(BigComplex))]
	[TestFixture(Category = "CoreFunctionality - BigComplex")]
	public class CoreFunctionality_BigComplex : CoreFunctionality<BigComplex>
	{
	}
}
