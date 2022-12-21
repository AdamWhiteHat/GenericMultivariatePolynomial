using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.VisualStudio.TestPlatform;
using NUnit;
using ExtendedArithmetic;
using NUnit.Framework;

namespace TestMultivariatePolynomial
{
	[TestFixture(Category = "Arithmetic")]
	public class Arithmetic<T>
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[Test]
		[TestCase("X^2 + 2*X - 1", "2*X^2 - 3*X + 6", "3*X^2 - X + 5")]
		public virtual void TestAdd(string augend, string addend, string expected)
		{
			MultivariatePolynomial<T> polyAugend = MultivariatePolynomial<T>.Parse(augend);
			MultivariatePolynomial<T> polyAddend = MultivariatePolynomial<T>.Parse(addend);

			MultivariatePolynomial<T> sum = MultivariatePolynomial<T>.Add(polyAugend, polyAddend);
			string actual = sum.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Add({augend}, {addend});");
		}

		[Test]
		[TestCase("X^2", "6", "X^2 + 6")]
		public virtual void TestAddUnlikeTerms(string augend, string addend, string expected)
		{
			MultivariatePolynomial<T> polyAugend = MultivariatePolynomial<T>.Parse(augend);
			MultivariatePolynomial<T> polyAddend = MultivariatePolynomial<T>.Parse(addend);

			MultivariatePolynomial<T> sum = MultivariatePolynomial<T>.Add(polyAugend, polyAddend);
			string actual = sum.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Add({augend}, {addend});");
		}

		[Test]
		[TestCase("X^2", "6", "X^2 - 6")]
		public virtual void TestSubtractUnlikeTerms(string minuend, string subtrahend, string expected)
		{
			MultivariatePolynomial<T> polyMinuend = MultivariatePolynomial<T>.Parse(minuend);
			MultivariatePolynomial<T> polySubtrahend = MultivariatePolynomial<T>.Parse(subtrahend);

			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Subtract: ({minuend}) - ({subtrahend})");
		}


		[Test]
		[TestCase("3*X", "X + 2", "2*X - 2")]
		public virtual void TestSubtract1(string minuend, string subtrahend, string expected)
		{
			MultivariatePolynomial<T> polyMinuend = MultivariatePolynomial<T>.Parse(minuend);
			MultivariatePolynomial<T> polySubtrahend = MultivariatePolynomial<T>.Parse(subtrahend);

			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Subtract: ({minuend}) - ({subtrahend})");
		}

		[Test]
		[TestCase("36*X*Y + 6*X + 6*Y + 1", "36*X*Y + 1", "6*X + 6*Y")]
		public virtual void TestSubtract2(string minuend, string subtrahend, string expected)
		{
			MultivariatePolynomial<T> polyMinuend = MultivariatePolynomial<T>.Parse(minuend);
			MultivariatePolynomial<T> polySubtrahend = MultivariatePolynomial<T>.Parse(subtrahend);

			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Subtract: ({minuend}) - ({subtrahend})");
		}

		[Test]
		[TestCase("2*X^3 + 2*X - 1", "2*X^2 - 5*X - 6", "2*X^3 - 2*X^2 + 7*X + 5")]
		public virtual void TestSubtract3(string minuend, string subtrahend, string expected)
		{
			MultivariatePolynomial<T> polyMinuend = MultivariatePolynomial<T>.Parse(minuend);
			MultivariatePolynomial<T> polySubtrahend = MultivariatePolynomial<T>.Parse(subtrahend);

			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Subtract: ({minuend}) - ({subtrahend})");
		}

		[Test]
		[TestCase("3*X^2*Y^3 + 2*X^3*Y^2 + 6*X*Y^2 + 4*X^3 - 6*X^2*Y + 3*X*Y - 2*X^2 + 12*X - 6", "X^3*Y^2 + 3*X^2 - 3*Y^2 - 12*X - 2", "3*X^2*Y^3 + X^3*Y^2 + 4*X^3 + 6*X*Y^2 - 6*X^2*Y + 3*Y^2 - 5*X^2 + 3*X*Y + 24*X - 4")]
		public virtual void TestSubtract4(string minuend, string subtrahend, string expected)
		{
			MultivariatePolynomial<T> polyMinuend = MultivariatePolynomial<T>.Parse(minuend);
			MultivariatePolynomial<T> polySubtrahend = MultivariatePolynomial<T>.Parse(subtrahend);

			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Subtract: ({minuend}) - ({subtrahend})");
		}

		[Test]
		[TestCase("504*X*Y*Z^2 + 216*X*Y - 42*X*Z^2 - 18*X + 84*Y*Z^2 + 36*Y - 7*Z^2 - 3", "X*Y*Z^2 + 42*X*Z^2 - 8*X - X^2 - 3", "503*X*Y*Z^2 + 84*Y*Z^2 - 84*X*Z^2 + X^2 - 7*Z^2 + 216*X*Y + 36*Y - 10*X")]
		public virtual void TestSubtract5(string minuend, string subtrahend, string expected)
		{
			MultivariatePolynomial<T> polyMinuend = MultivariatePolynomial<T>.Parse(minuend);
			MultivariatePolynomial<T> polySubtrahend = MultivariatePolynomial<T>.Parse(subtrahend);

			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Subtract: ({minuend}) - ({subtrahend})");
		}

		[Test]
		[TestCase("6*X + 1", "6*Y + 1", "36*X*Y + 6*X + 6*Y + 1")]
		public virtual void TestMultiply1(string lhs, string rhs, string expected)
		{
			MultivariatePolynomial<T> polylhs = MultivariatePolynomial<T>.Parse(lhs);
			MultivariatePolynomial<T> polyrhs = MultivariatePolynomial<T>.Parse(rhs);

			MultivariatePolynomial<T> polyProdcut = MultivariatePolynomial<T>.Multiply(polylhs, polyrhs);

			string actual = polyProdcut.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Multiply({lhs}, {rhs});");
		}

		[Test]
		[TestCase("6*X + 1", "6*X - 1", "36*X^2 - 1")]
		public virtual void TestMultiplySameSymbols(string lhs, string rhs, string expected)
		{
			MultivariatePolynomial<T> polylhs = MultivariatePolynomial<T>.Parse(lhs);
			MultivariatePolynomial<T> polyrhs = MultivariatePolynomial<T>.Parse(rhs);

			MultivariatePolynomial<T> polyProdcut = MultivariatePolynomial<T>.Multiply(polylhs, polyrhs);

			string actual = polyProdcut.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Multiply({lhs}, {rhs});");
		}

		[Test]
		[TestCase("36*X*Y + 6*X + 6*Y + 1", "6*X + 1", "6*Y + 1")]
		public virtual void TestDivide1(string dividend, string divisor, string expected)
		{
			MultivariatePolynomial<T> polyDivedend = MultivariatePolynomial<T>.Parse(dividend);
			MultivariatePolynomial<T> polyDivisor = MultivariatePolynomial<T>.Parse(divisor);

			MultivariatePolynomial<T> quotient = MultivariatePolynomial<T>.Divide(polyDivedend, polyDivisor);
			string actual = quotient.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Divide({dividend}, {divisor});");
		}

		[Test]
		[TestCase("2*X*Y^2 + 3*X*Y + 4*Y^2 + 6*Y", "X + 2", "2*Y^2 + 3*Y")]
		public virtual void TestDivide2(string dividend, string divisor, string expected)
		{
			MultivariatePolynomial<T> polyDivedend = MultivariatePolynomial<T>.Parse(dividend);
			MultivariatePolynomial<T> polyDivisor = MultivariatePolynomial<T>.Parse(divisor);

			MultivariatePolynomial<T> quotient = MultivariatePolynomial<T>.Divide(polyDivedend, polyDivisor);
			string actual = quotient.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Divide({dividend}, {divisor});");
		}

		[Test]
		[TestCase("2*X*Y^2 - 1", "2", "4*X^2*Y^4 - 4*X*Y^2 + 1")]
		public virtual void TestPow(string powerBase, string exponent, string expected)
		{
			int exponentInt = int.Parse(exponent);

			MultivariatePolynomial<T> polyBase = MultivariatePolynomial<T>.Parse(powerBase);
			MultivariatePolynomial<T> power = MultivariatePolynomial<T>.Pow(polyBase, exponentInt);

			string actual = power.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Pow({powerBase}, {exponent});");
		}

		[Test]
		[TestCase("132*X*Y + 77*X + 55*Y + 1", "132*Y + 77")]
		public virtual void TestGetDerivative1(string polynomial, string expected)
		{
			MultivariatePolynomial<T> poly = MultivariatePolynomial<T>.Parse(polynomial);
			MultivariatePolynomial<T> derivative = MultivariatePolynomial<T>.GetDerivative(poly, 'X');

			string actual = derivative.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.GetDerivative({polynomial});");
		}

		[Test]
		[TestCase("4*X^2*Y^4 - 4*X*Y^2 + 1", "8*X*Y^4 - 4*Y^2")]
		public virtual void TestGetDerivative2(string polynomial, string expected)
		{
			MultivariatePolynomial<T> poly = MultivariatePolynomial<T>.Parse(polynomial);
			MultivariatePolynomial<T> derivative = MultivariatePolynomial<T>.GetDerivative(poly, 'X');

			string actual = derivative.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.GetDerivative({polynomial});");
		}

		[Test]
		[TestCase("6*X + 1", "2*Y", "12*Y + 1")]
		public virtual void TestFunctionalComposition_f_x_001(string f, string x, string expecting)
		{
			MultivariatePolynomial<T> polyF = MultivariatePolynomial<T>.Parse(f);
			MultivariatePolynomial<T> xPoly = MultivariatePolynomial<T>.Parse(x);

			List<Tuple<char, MultivariatePolynomial<T>>> indeterminantsConstants = new List<Tuple<char, MultivariatePolynomial<T>>>()
			{
				new Tuple<char, MultivariatePolynomial<T>>('X', xPoly)
			};

			MultivariatePolynomial<T> composition = polyF.FunctionalComposition(indeterminantsConstants);
			string actual = composition.ToString();

			TestContext.WriteLine($"f(X) = {f}");
			TestContext.WriteLine($"f({x}) = {actual}");
			Assert.AreEqual(expecting, actual);
		}

		[Test]
		[TestCase("6*X + 1", "6*Y - 1", "36*Y - 5")]
		public virtual void TestFunctionalComposition_f_x_002(string f, string x, string expecting)
		{
			MultivariatePolynomial<T> polyF = MultivariatePolynomial<T>.Parse(f);
			MultivariatePolynomial<T> xPoly = MultivariatePolynomial<T>.Parse(x);

			List<Tuple<char, MultivariatePolynomial<T>>> indeterminantsConstants = new List<Tuple<char, MultivariatePolynomial<T>>>()
			{
				new Tuple<char, MultivariatePolynomial<T>>('X', xPoly)
			};

			MultivariatePolynomial<T> composition = polyF.FunctionalComposition(indeterminantsConstants);
			string actual = composition.ToString();

			TestContext.WriteLine($"f(X) = {f}");
			TestContext.WriteLine($"f({x}) = {actual}");
			Assert.AreEqual(expecting, actual);
		}

		[Test]
		[TestCase("36*X*Y - 6*X + 6*Y - 1", "0", "-1", "-7")]
		public virtual void TestFunctionalComposition_f_xy_001(string f, string x, string y, string expecting)
		{
			MultivariatePolynomial<T> polyF = MultivariatePolynomial<T>.Parse(f);
			MultivariatePolynomial<T> xPoly = MultivariatePolynomial<T>.Parse(x);
			MultivariatePolynomial<T> yPoly = MultivariatePolynomial<T>.Parse(y);

			List<Tuple<char, MultivariatePolynomial<T>>> indeterminantsConstants = new List<Tuple<char, MultivariatePolynomial<T>>>()
			{
				new Tuple<char, MultivariatePolynomial<T>>('X', xPoly),
				new Tuple<char, MultivariatePolynomial<T>>('Y', yPoly),
			};

			MultivariatePolynomial<T> composition = polyF.FunctionalComposition(indeterminantsConstants);
			string actual = composition.ToString();


			TestContext.WriteLine($"f(X, Y) = {f}");
			TestContext.WriteLine($"f({x}, {y}) = {actual}");
			Assert.AreEqual(expecting, actual);
		}

		[Test]
		[TestCase("-X^2 + 5", "2*X + 3", "X + 3", "-4*X^2 - 36*X - 76")]
		public virtual void TestFunctionalComposition_fg_x_001(string f, string g, string x, string expecting)
		{
			MultivariatePolynomial<T> polyF = MultivariatePolynomial<T>.Parse(f);
			MultivariatePolynomial<T> polyG = MultivariatePolynomial<T>.Parse(g);
			MultivariatePolynomial<T> xPoly = MultivariatePolynomial<T>.Parse(x);

			List<Tuple<char, MultivariatePolynomial<T>>> fIndeterminants = new List<Tuple<char, MultivariatePolynomial<T>>>()
			{
				new Tuple<char, MultivariatePolynomial<T>>('X', polyG)
			};

			List<Tuple<char, MultivariatePolynomial<T>>> gIndeterminants = new List<Tuple<char, MultivariatePolynomial<T>>>()
			{
				new Tuple<char, MultivariatePolynomial<T>>('X', xPoly),
			};

			MultivariatePolynomial<T> fComposition = polyF.FunctionalComposition(fIndeterminants);
			MultivariatePolynomial<T> gComposition = fComposition.FunctionalComposition(gIndeterminants);
			string actual = gComposition.ToString();


			TestContext.WriteLine($"f(X) = {f}");
			TestContext.WriteLine($"g(X) = {g}");
			TestContext.WriteLine($"f(g(X)) = {fComposition}");
			TestContext.WriteLine($"f(g({x})) = {actual}");
			Assert.AreEqual(expecting, actual);
		}

		//[Test]
		//[TestCase("X^4 + 8*X^3 + 21*X^2 + 22*X + 8", "X^3 + 6*X^2 + 11*X + 6", "X^2 + 3*X + 2")]
		public virtual void TestGCD(string polynomial1, string polynomial2, string expected)
		{
			//throw new NotImplementedException();

			MultivariatePolynomial<T> poly1 = MultivariatePolynomial<T>.Parse(polynomial1);
			MultivariatePolynomial<T> poly2 = MultivariatePolynomial<T>.Parse(polynomial2);
			MultivariatePolynomial<T> gcd = MultivariatePolynomial<T>.GCD(poly1, poly2);

			string actual = gcd.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.GCD({polynomial1}, {polynomial2});");
		}
	}
}
