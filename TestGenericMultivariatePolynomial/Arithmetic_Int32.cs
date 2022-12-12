using System;
using System.Numerics;
using ExtendedArithmetic;
using NUnit.Framework;
using TestMultivariatePolynomial;

namespace TestGenericPolynomial
{
	[TestOf(typeof(Int32))]
	[TestFixture(Category = "Arithmetic - Int32")]
	public class Arithmetic_Int32 : Arithmetic<Int32>
	{
	}
}
