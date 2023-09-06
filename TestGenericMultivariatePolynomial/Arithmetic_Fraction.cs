using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using TestGenericMultivariatePolynomial;

namespace TestGenericMultivariatePolynomial
{
	[TestOf(typeof(Fraction))]
	[TestFixture(Category = "Arithmetic - Fraction")]
	public class Arithmetic_Fraction : Arithmetic<Fraction>
	{
	}
}
