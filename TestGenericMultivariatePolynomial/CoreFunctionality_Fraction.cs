using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using TestGenericMultivariatePolynomial;

namespace TestGenericMultivariatePolynomial
{
	[TestOf(typeof(Fraction))]
	[TestFixture(Category = "CoreFunctionality - Fraction")]
	public class CoreFunctionality_Fraction : CoreFunctionality<Fraction>
	{
	}
}
