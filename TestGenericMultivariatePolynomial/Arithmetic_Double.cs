using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;
using TestGenericMultivariatePolynomial;

namespace TestGenericMultivariatePolynomial
{
	[TestOf(typeof(double))]
	[TestFixture(Category = "Arithmetic - Double")]
	public class Arithmetic_Double : Arithmetic<double>
	{
	}
}
