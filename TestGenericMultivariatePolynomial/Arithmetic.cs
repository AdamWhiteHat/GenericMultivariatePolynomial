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
		public virtual void TestAdd()
		{
			string augend = "X^2 + 2*X - 1";
			string addend = "2*X^2 - 3*X + 6";

			string expected = "3*X^2 - X + 5";

			MultivariatePolynomial<T> polyAugend = MultivariatePolynomial<T>.Parse(augend);
			MultivariatePolynomial<T> polyAddend = MultivariatePolynomial<T>.Parse(addend);

			MultivariatePolynomial<T> sum = MultivariatePolynomial<T>.Add(polyAugend, polyAddend);
			string actual = sum.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Add({augend}, {addend});");
		}

		[Test]
		public virtual void TestAddUnlikeTerms()
		{
			string augend = "X^2";
			string addend = "6";

			string expected = "X^2 + 6";

			MultivariatePolynomial<T> polyAugend = MultivariatePolynomial<T>.Parse(augend);
			MultivariatePolynomial<T> polyAddend = MultivariatePolynomial<T>.Parse(addend);

			MultivariatePolynomial<T> sum = MultivariatePolynomial<T>.Add(polyAugend, polyAddend);
			string actual = sum.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Add({augend}, {addend});");
		}

		[Test]
		public virtual void TestSubtractUnlikeTerms()
		{
			string minuend = "X^2";
			string subtrahend = "6";

			string expected = "X^2 - 6";

			MultivariatePolynomial<T> polyMinuend = MultivariatePolynomial<T>.Parse(minuend);
			MultivariatePolynomial<T> polySubtrahend = MultivariatePolynomial<T>.Parse(subtrahend);

			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Subtract: ({minuend}) - ({subtrahend})");
		}


		[Test]
		public virtual void TestSubtract1()
		{
			string minuend = "3*X";
			string subtrahend = "X + 2";

			string expected = "2*X - 2";

			MultivariatePolynomial<T> polyMinuend = MultivariatePolynomial<T>.Parse(minuend);
			MultivariatePolynomial<T> polySubtrahend = MultivariatePolynomial<T>.Parse(subtrahend);

			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Subtract: ({minuend}) - ({subtrahend})");
		}

		[Test]
		public virtual void TestSubtract2()
		{
			string minuend = "36*X*Y + 6*X + 6*Y + 1";
			string subtrahend = "36*X*Y + 1";

			string expected = "6*X + 6*Y";

			MultivariatePolynomial<T> polyMinuend = MultivariatePolynomial<T>.Parse(minuend);
			MultivariatePolynomial<T> polySubtrahend = MultivariatePolynomial<T>.Parse(subtrahend);

			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Subtract: ({minuend}) - ({subtrahend})");
		}

		[Test]
		public virtual void TestSubtract3()
		{
			string minuend = "2*X^3 + 2*X - 1";
			string subtrahend = "2*X^2 - 5*X - 6";

			string expected = "2*X^3 - 2*X^2 + 7*X + 5";

			MultivariatePolynomial<T> polyMinuend = MultivariatePolynomial<T>.Parse(minuend);
			MultivariatePolynomial<T> polySubtrahend = MultivariatePolynomial<T>.Parse(subtrahend);

			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Subtract: ({minuend}) - ({subtrahend})");
		}

		[Test]
		public virtual void TestSubtract4()
		{
			string minuend = "3*X^2*Y^3 + 2*X^3*Y^2 + 6*X*Y^2 + 4*X^3 - 6*X^2*Y + 3*X*Y - 2*X^2 + 12*X - 6";
			string subtrahend = "X^3*Y^2 + 3*X^2 - 3*Y^2 - 12*X - 2";

			string expected = "3*X^2*Y^3 + X^3*Y^2 + 4*X^3 + 6*X*Y^2 - 6*X^2*Y + 3*Y^2 - 5*X^2 + 3*X*Y + 24*X - 4";

			MultivariatePolynomial<T> polyMinuend = MultivariatePolynomial<T>.Parse(minuend);
			MultivariatePolynomial<T> polySubtrahend = MultivariatePolynomial<T>.Parse(subtrahend);

			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Subtract: ({minuend}) - ({subtrahend})");
		}

		[Test]
		public virtual void TestSubtract5()
		{
			string minuend = "504*X*Y*Z^2 + 216*X*Y - 42*X*Z^2 - 18*X + 84*Y*Z^2 + 36*Y - 7*Z^2 - 3";
			string subtrahend = "X*Y*Z^2 + 42*X*Z^2 - 8*X - X^2 - 3";

			string expected = "503*X*Y*Z^2 + 84*Y*Z^2 - 84*X*Z^2 + X^2 - 7*Z^2 + 216*X*Y + 36*Y - 10*X";

			MultivariatePolynomial<T> polyMinuend = MultivariatePolynomial<T>.Parse(minuend);
			MultivariatePolynomial<T> polySubtrahend = MultivariatePolynomial<T>.Parse(subtrahend);

			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(polyMinuend, polySubtrahend);
			string actual = difference.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Subtract: ({minuend}) - ({subtrahend})");
		}

		[Test]
		public virtual void TestMultiply1()
		{
			string lhs = "6*X + 1";
			string rhs = "6*Y + 1";
			string expected = "36*X*Y + 6*X + 6*Y + 1";

			MultivariatePolynomial<T> polylhs = MultivariatePolynomial<T>.Parse(lhs);
			MultivariatePolynomial<T> polyrhs = MultivariatePolynomial<T>.Parse(rhs);

			MultivariatePolynomial<T> polyProdcut = MultivariatePolynomial<T>.Multiply(polylhs, polyrhs);

			string actual = polyProdcut.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Multiply({lhs}, {rhs});");
		}

		[Test]
		public virtual void TestMultiplySameSymbols()
		{
			string lhs = "6*X + 1";
			string rhs = "6*X - 1";
			string expected = "36*X^2 - 1";

			MultivariatePolynomial<T> polylhs = MultivariatePolynomial<T>.Parse(lhs);
			MultivariatePolynomial<T> polyrhs = MultivariatePolynomial<T>.Parse(rhs);

			MultivariatePolynomial<T> polyProdcut = MultivariatePolynomial<T>.Multiply(polylhs, polyrhs);

			string actual = polyProdcut.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Multiply({lhs}, {rhs});");
		}

		[Test]
		public virtual void TestDivide1()
		{
			string dividend = "36*X*Y + 6*X + 6*Y + 1";
			string divisor = "6*X + 1";

			string expected = "6*Y + 1";

			MultivariatePolynomial<T> polyDivedend = MultivariatePolynomial<T>.Parse(dividend);
			MultivariatePolynomial<T> polyDivisor = MultivariatePolynomial<T>.Parse(divisor);

			MultivariatePolynomial<T> quotient = MultivariatePolynomial<T>.Divide(polyDivedend, polyDivisor);
			string actual = quotient.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Divide({dividend}, {divisor});");
		}

		[Test]
		public virtual void TestDivide2()
		{
			string dividend = "2*X*Y^2 + 3*X*Y + 4*Y^2 + 6*Y";
			string divisor = "X + 2";

			string expected = "2*Y^2 + 3*Y";

			MultivariatePolynomial<T> polyDivedend = MultivariatePolynomial<T>.Parse(dividend);
			MultivariatePolynomial<T> polyDivisor = MultivariatePolynomial<T>.Parse(divisor);

			MultivariatePolynomial<T> quotient = MultivariatePolynomial<T>.Divide(polyDivedend, polyDivisor);
			string actual = quotient.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Divide({dividend}, {divisor});");
		}

		[Test]
		public virtual void TestPow()
		{
			string polyBaseString = "2*X*Y^2 - 1";
			int exponent = 2;

			string expected = "4*X^2*Y^4 - 4*X*Y^2 + 1";

			MultivariatePolynomial<T> polyBase = MultivariatePolynomial<T>.Parse(polyBaseString);
			MultivariatePolynomial<T> power = MultivariatePolynomial<T>.Pow(polyBase, exponent);

			string actual = power.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Pow({polyBaseString}, {exponent});");
		}

		[Test]
		public virtual void TestGetDerivative1()
		{
			string polyString = "132*X*Y + 77*X + 55*Y + 1";
			string expected = "132*Y + 77";

			MultivariatePolynomial<T> poly = MultivariatePolynomial<T>.Parse(polyString);
			MultivariatePolynomial<T> derivative = MultivariatePolynomial<T>.GetDerivative(poly, 'X');

			string actual = derivative.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.GetDerivative({polyString});");
		}

		[Test]
		public virtual void TestGetDerivative2()
		{
			string polyString = "4*X^2*Y^4 - 4*X*Y^2 + 1";
			string expected = "8*X*Y^4 - 4*Y^2";

			MultivariatePolynomial<T> poly = MultivariatePolynomial<T>.Parse(polyString);
			MultivariatePolynomial<T> derivative = MultivariatePolynomial<T>.GetDerivative(poly, 'X');

			string actual = derivative.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.GetDerivative({polyString});");
		}

		[Test]
		public virtual void TestGCD()
		{
			//throw new NotImplementedException();

			string polyString1 = "X^4 + 8*X^3 + 21*X^2 + 22*X + 8";     //"X^4 + 8*X^3 + 21*X^2 + 22*X + 8";
			string polyString2 = "X^3 + 6*X^2 + 11*X + 6";              //"X^3 + 6*X^2 + 11*X + 6";
			string expected = "X^2 + 3*X + 2";                          //"X^2 + 3*X + 2";

			MultivariatePolynomial<T> poly1 = MultivariatePolynomial<T>.Parse(polyString1);
			MultivariatePolynomial<T> poly2 = MultivariatePolynomial<T>.Parse(polyString2);
			MultivariatePolynomial<T> gcd = MultivariatePolynomial<T>.GCD(poly1, poly2);

			string actual = gcd.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.GCD({polyString1}, {polyString2});");
		}

		[Test]
		public virtual void TestFunctionalComposition001()
		{
			MultivariatePolynomial<T> indeterminateX = MultivariatePolynomial<T>.Parse("6*X + 1");

			MultivariatePolynomial<T> zero = MultivariatePolynomial<T>.Parse("0");
			MultivariatePolynomial<T> minusOne = MultivariatePolynomial<T>.Parse("-1");
			MultivariatePolynomial<T> one = MultivariatePolynomial<T>.Parse("1");
			MultivariatePolynomial<T> X = MultivariatePolynomial<T>.Parse("X");

			MultivariatePolynomial<T> even = MultivariatePolynomial<T>.Parse("2*Y");
			MultivariatePolynomial<T> odd = MultivariatePolynomial<T>.Parse("2*X + 1");


			string expecting1 = "1";
			string expecting2 = "6*X + 1";
			string expecting3 = "12*Y + 1";
			//string expecting4 = "";
			//string expecting5 = "";		


			string actual1 = MultivariatePolynomial<T>.Pow(indeterminateX, 0).ToString();
			string actual2 = MultivariatePolynomial<T>.Pow(indeterminateX, 1).ToString();
			string actual3 = indeterminateX.FunctionalComposition(new List<Tuple<char, MultivariatePolynomial<T>>>() { new Tuple<char, MultivariatePolynomial<T>>('X', even) }).ToString();
			//string actual4 = composition4.ToString();
			//string actual5 = composition5.ToString();

			Assert.AreEqual(expecting1, actual1);
			Assert.AreEqual(expecting2, actual2);
			Assert.AreEqual(expecting3, actual3);
			//Assert.AreEqual(expecting4, actual4);
			//Assert.AreEqual(expecting5, actual5);
		}
		[Test]
		public virtual void TestFunctionalComposition002()
		{
			MultivariatePolynomial<T> indeterminateX = MultivariatePolynomial<T>.Parse("6*X + 1");
			MultivariatePolynomial<T> indeterminateY = MultivariatePolynomial<T>.Parse("6*Y - 1");
			MultivariatePolynomial<T> polyn = MultivariatePolynomial<T>.Multiply(indeterminateX, indeterminateY);

			MultivariatePolynomial<T> zero = MultivariatePolynomial<T>.Parse("0");
			MultivariatePolynomial<T> minusOne = MultivariatePolynomial<T>.Parse("-1");
			MultivariatePolynomial<T> six = MultivariatePolynomial<T>.Parse("6");

			MultivariatePolynomial<T> even = MultivariatePolynomial<T>.Parse("2*Y");
			MultivariatePolynomial<T> odd = MultivariatePolynomial<T>.Parse("2*X + 1");

			MultivariatePolynomial<T> inversePolyn = MultivariatePolynomial<T>.Multiply(polyn, minusOne); // -36*X*Y + 6*X - 6*Y + 1

			List<Tuple<char, MultivariatePolynomial<T>>> indeterminantsOddEven = new List<Tuple<char, MultivariatePolynomial<T>>>()
			{
				new Tuple<char, MultivariatePolynomial<T>>('X', odd),
				new Tuple<char, MultivariatePolynomial<T>>('Y', even),
			};

			List<Tuple<char, MultivariatePolynomial<T>>> indeterminantsConstants = new List<Tuple<char, MultivariatePolynomial<T>>>()
			{
				new Tuple<char, MultivariatePolynomial<T>>('X', zero),
				new Tuple<char, MultivariatePolynomial<T>>('Y', minusOne),
			};

			List<Tuple<char, MultivariatePolynomial<T>>> indeterminantsInverse = new List<Tuple<char, MultivariatePolynomial<T>>>()
			{
				new Tuple<char, MultivariatePolynomial<T>>('X', inversePolyn),
				new Tuple<char, MultivariatePolynomial<T>>('Y', inversePolyn),
			};

			List<Tuple<char, MultivariatePolynomial<T>>> indeterminantSix = new List<Tuple<char, MultivariatePolynomial<T>>>()
			{
				new Tuple<char, MultivariatePolynomial<T>>('X', six)
			};

			MultivariatePolynomial<T> composition1 = polyn.FunctionalComposition(indeterminantsOddEven); // 36*X*Y + 6*Y - 6*X - 1
			MultivariatePolynomial<T> composition2 = polyn.FunctionalComposition(indeterminantsInverse);
			MultivariatePolynomial<T> composition3 = polyn.FunctionalComposition(indeterminantsConstants);
			MultivariatePolynomial<T> composition4 = minusOne.FunctionalComposition(indeterminantSix);
			MultivariatePolynomial<T> composition5 = indeterminateX.FunctionalComposition(indeterminantsConstants);

			string expecting1 = "144*X*Y - 12*X + 84*Y - 7";
			string expecting2 = "46656*X^2*Y^2 + 1296*X^2 - 15552*X^2*Y + 15552*X*Y^2 + 432*X - 5184*X*Y + 1296*Y^2 - 432*Y + 35";
			string expecting3 = "-7";
			string expecting4 = "-1";
			string expecting5 = "1";

			string actual1 = composition1.ToString();
			string actual2 = composition2.ToString();
			string actual3 = composition3.ToString();
			string actual4 = composition4.ToString();
			string actual5 = composition5.ToString();

			//Assert.AreEqual(expecting1, actual1);
			//Assert.AreEqual(expecting2, actual2);
			Assert.AreEqual(expecting3, actual3);
			Assert.AreEqual(expecting4, actual4);
			Assert.AreEqual(expecting5, actual5);
		}
	}
}
