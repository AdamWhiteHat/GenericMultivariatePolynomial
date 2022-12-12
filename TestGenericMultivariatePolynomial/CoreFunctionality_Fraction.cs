using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(Fraction))]
	[TestFixture(Category = "CoreFunctionality - Fraction")]
	public class CoreFunctionality_Fraction : CoreFunctionality<Fraction>
	{
	}
}
