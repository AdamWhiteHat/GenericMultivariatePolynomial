using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(BigRational))]
	[TestFixture(Category = "Arithmetic - BigRational")]
	public class Arithmetic_BigRational : Arithmetic<BigRational>
	{
	}
}
