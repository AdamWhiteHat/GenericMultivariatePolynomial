using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(BigComplex))]
	[TestFixture(Category = "Arithmetic - BigComplex")]
	public class Arithmetic_BigComplex : Arithmetic<BigComplex>
	{
	}
}
