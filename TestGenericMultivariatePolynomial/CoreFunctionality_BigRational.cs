using System.Numerics;
using ExtendedArithmetic;
using ExtendedNumerics;
using NUnit.Framework;
using TestGenericMultivariatePolynomial;

namespace TestGenericMultivariatePolynomial
{
	[TestOf(typeof(BigRational))]
	[TestFixture(Category = "CoreFunctionality - BigRational")]
	public class CoreFunctionality_BigRational : CoreFunctionality<BigRational>
	{
	}
}
