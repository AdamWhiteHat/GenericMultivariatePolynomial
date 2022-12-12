using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(Complex))]
	[TestFixture(Category = "Arithmetic - Complex")]
	public class Arithmetic_Complex : Arithmetic<Complex>
	{
	}
}
