using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using NUnit;
using NUnit.Framework;
using ExtendedArithmetic;

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
		[TestCase("X^3 + 7*X^2 + 14*X + 8", "X + 1", "X^2 + 6*X + 8", "X^2 + 2")]
		public virtual void TestSubtract6(string dividend, string unit, string expectedQuotient, string expected)
		{
			MultivariatePolynomial<T> polyDividend = MultivariatePolynomial<T>.Parse(dividend);
			MultivariatePolynomial<T> polyUnit = MultivariatePolynomial<T>.Parse(unit);
			MultivariatePolynomial<T> quotient = MultivariatePolynomial<T>.Parse(expectedQuotient);

			MultivariatePolynomial<T> specialQuotient = MultivariatePolynomial<T>.Divide(polyDividend, polyUnit);
			MultivariatePolynomial<T> difference = MultivariatePolynomial<T>.Subtract(specialQuotient, polyUnit);
			difference = MultivariatePolynomial<T>.Subtract(difference, polyUnit);
			difference = MultivariatePolynomial<T>.Subtract(difference, polyUnit);
			difference = MultivariatePolynomial<T>.Subtract(difference, polyUnit);
			difference = MultivariatePolynomial<T>.Subtract(difference, polyUnit);
			difference = MultivariatePolynomial<T>.Subtract(difference, polyUnit);

			string actual = difference.ToString();

			TestContext.WriteLine($"Difference: {difference}");

			TestContext.WriteLine("");
			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"A strange edge-case situation: {dividend} / {unit} = 'special' quotient who's constant has the variable X.\nWhereas a polynomial from parse has no such variable for the constant.\nThis becomes noticeable if we subtract a constant from the 'special' polynomial form:\nThe result will appear to have two constants!\nObserve:\n{expectedQuotient} SUBTRACT {unit}\n= {difference}.");
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
		[TestCase("1", "a", "2")]
		public virtual void TestDivide_UserSubmittedIssueNo1(string expected1, string expected2, string expected3)
		{
			Indeterminate[] ind1 = new Indeterminate[] { new Indeterminate('a', 1) };
			Indeterminate[] ind2 = new Indeterminate[0];

			Term<T>[] terms = new Term<T>[] { new Term<T>(GenericArithmetic<T>.Two, ind1) };
			MultivariatePolynomial<T> pol1 = new MultivariatePolynomial<T>(terms); // MultivariatePolynomial.Parse("2*a");

			Term<T>[] terms2 = new Term<T>[] { new Term<T>(GenericArithmetic<T>.Two, ind2) };
			MultivariatePolynomial<T> pol2 = new MultivariatePolynomial<T>(terms2); // MultivariatePolynomial.Parse("2");

			Term<T>[] terms3 = new Term<T>[] { new Term<T>(GenericArithmetic<T>.One, ind1) };
			MultivariatePolynomial<T> pol3 = new MultivariatePolynomial<T>(terms3); // MultivariatePolynomial.Parse("a");

			TestContext.WriteLine($"pol1 = {pol1}");
			TestContext.WriteLine($"pol2 = {pol2}");
			TestContext.WriteLine($"pol3 = {pol3}");
			TestContext.WriteLine("");

			MultivariatePolynomial<T> quotient1 = MultivariatePolynomial<T>.Divide(pol1, pol1);
			MultivariatePolynomial<T> quotient2 = MultivariatePolynomial<T>.Divide(pol1, pol2);
			MultivariatePolynomial<T> quotient3 = MultivariatePolynomial<T>.Divide(pol1, pol3);

			TestContext.WriteLine($"pol1/pol1 = {quotient1}");
			TestContext.WriteLine($"pol1/pol2 = {quotient2}");
			TestContext.WriteLine($"pol1/pol3 = {quotient3}");
			TestContext.WriteLine("");

			string actual1 = quotient1.ToString();
			string actual2 = quotient2.ToString();
			string actual3 = quotient3.ToString();

			Assert.AreEqual(expected1, actual1);
			Assert.AreEqual(expected2, actual2);
			Assert.AreEqual(expected3, actual3);
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
		[TestCase("X^4 + 8*X^3 + 21*X^2 + 22*X + 8", "X + 1", "X + 1", "X + 2", "X + 4")]
		public virtual void TestFactorization1(string polynomialToFactor, params string[] expected)
		{
			MultivariatePolynomial<T> poly = MultivariatePolynomial<T>.Parse(polynomialToFactor);

			List<MultivariatePolynomial<T>> results = MultivariatePolynomial<T>.Factor(poly);

			string[] actual = results.Select(p => p.ToString()).ToArray();

			TestContext.WriteLine($"Expected: {{ {string.Join(", ", expected)} }}");
			TestContext.WriteLine($"Actual:   {{ {string.Join(", ", actual)} }}");
			TestContext.WriteLine("");

			Assert.AreEqual(expected.Length, actual.Length, $"expected.Length ({expected.Length}) == actual.Length ({actual.Length})");

			foreach (string search in expected)
			{
				if (!actual.Contains(search))
				{
					Assert.Fail($"{{ {string.Join(", ", actual)} }} does not contain the factor \"{search}\".");
				}
			}
		}

		[Test]
		[TestCase("X^3 + 6*X^2 + 11*X + 6", "X + 1", "X + 2", "X + 3")]
		public virtual void TestFactorization2(string polynomialToFactor, params string[] expected)
		{
			MultivariatePolynomial<T> poly = MultivariatePolynomial<T>.Parse(polynomialToFactor);

			List<MultivariatePolynomial<T>> results = MultivariatePolynomial<T>.Factor(poly);

			string[] actual = results.Select(p => p.ToString()).ToArray();

			TestContext.WriteLine($"Expected: {{ {string.Join(", ", expected)} }}");
			TestContext.WriteLine($"Actual:   {{ {string.Join(", ", actual)} }}");
			TestContext.WriteLine("");

			Assert.AreEqual(expected.Length, actual.Length, $"expected.Length ({expected.Length}) == actual.Length ({actual.Length})");

			foreach (string search in expected)
			{
				if (!actual.Contains(search))
				{
					Assert.Fail($"{{ {string.Join(", ", actual)} }} does not contain the factor \"{search}\".");
				}
			}
		}

		[Test]
		[TestCase("X^2 + X + 1")]
		public virtual void TestFactorization_Irreducible(string irreduciblePolynomial)
		{
			MultivariatePolynomial<T> poly = MultivariatePolynomial<T>.Parse(irreduciblePolynomial);

			List<MultivariatePolynomial<T>> results = MultivariatePolynomial<T>.Factor(poly);

			string[] actual = results.Select(p => p.ToString()).ToArray();

			string[] expected = new string[] { };

			TestContext.WriteLine($"Expected: {{ {string.Join(", ", expected)} }}");
			TestContext.WriteLine($"Actual:   {{ {string.Join(", ", actual)} }}");
			TestContext.WriteLine("");

			Assert.AreEqual(expected.Length, actual.Length, $"expected.Length ({expected.Length}) == actual.Length ({actual.Length})");

			foreach (string search in expected)
			{
				if (!actual.Contains(search))
				{
					Assert.Fail($"{{ {string.Join(", ", actual)} }} does not contain the factor \"{search}\".");
				}
			}
		}

		[Test]
		[TestCase("X^4 + 8*X^3 + 21*X^2 + 22*X + 8", "X^3 + 6*X^2 + 11*X + 6", "X^2 + 3*X + 2")]
		public virtual void TestGCD_Univarite_1(string polyString1, string polyString2, string expected)
		{
			TestContext.WriteLine("<EXPECTING>");
			TestContext.WriteLine("Left.Factors():");
			TestContext.WriteLine("X + 1");
			TestContext.WriteLine("X + 1");
			TestContext.WriteLine("X + 2");
			TestContext.WriteLine("X + 4");
			TestContext.WriteLine("");
			TestContext.WriteLine("Right.Factors():");
			TestContext.WriteLine("X + 1");
			TestContext.WriteLine("X + 2");
			TestContext.WriteLine("X + 3");
			TestContext.WriteLine("</EXPECTING>");
			TestContext.WriteLine("");
			TestContext.WriteLine("");

			MultivariatePolynomial<T> poly1 = MultivariatePolynomial<T>.Parse(polyString1);
			MultivariatePolynomial<T> poly2 = MultivariatePolynomial<T>.Parse(polyString2);
			MultivariatePolynomial<T> gcd = MultivariatePolynomial<T>.GCD(poly1, poly2);

			string actual = gcd.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GCD({polyString1}, {polyString2});");
		}

		[Test]
		[TestCase("X^5 + 25*X^4 + 230*X^3 + 950*X^2 + 1689*X + 945", "X^3 + 14*X^2 + 56*X + 64", "1")]
		public virtual void TestGCD_Univarite_2(string polyString1, string polyString2, string expected)
		{
			MultivariatePolynomial<T> poly1 = MultivariatePolynomial<T>.Parse(polyString1);
			MultivariatePolynomial<T> poly2 = MultivariatePolynomial<T>.Parse(polyString2);
			MultivariatePolynomial<T> gcd = MultivariatePolynomial<T>.GCD(poly1, poly2);

			string actual = gcd.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GCD({polyString1}, {polyString2});");
		}

		[Test]
		[TestCase("X^5 + 9*X^4 + 29*X^3 + 43*X^2 + 30*X + 8", "X^4 + 7*X^3 + 17*X^2 + 17*X + 6", "X^3 + 4*X^2 + 5*X + 2")]
		public virtual void TestGCD_Univarite_3(string polyString1, string polyString2, string expected)
		{
			TestContext.WriteLine("<EXPECTING>");
			TestContext.WriteLine("Left.Factors():");
			TestContext.WriteLine("X + 1");
			TestContext.WriteLine("X + 1");
			TestContext.WriteLine("X + 1");
			TestContext.WriteLine("X + 2");
			TestContext.WriteLine("X + 4");
			TestContext.WriteLine("");
			TestContext.WriteLine("Right.Factors():");
			TestContext.WriteLine("X + 1");
			TestContext.WriteLine("X + 1");
			TestContext.WriteLine("X + 2");
			TestContext.WriteLine("X + 3");
			TestContext.WriteLine("</EXPECTING>");
			TestContext.WriteLine("");
			TestContext.WriteLine("");
			TestContext.WriteLine("<ACTUAL>");

			MultivariatePolynomial<T> poly1 = MultivariatePolynomial<T>.Parse(polyString1);
			MultivariatePolynomial<T> poly2 = MultivariatePolynomial<T>.Parse(polyString2);
			MultivariatePolynomial<T> gcd = MultivariatePolynomial<T>.GCD(poly1, poly2);

			TestContext.WriteLine("</ACTUAL>");

			string actual = gcd.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GCD({polyString1}, {polyString2});");
		}

		[Ignore("Work in progress")]
		[TestCase("3*X*Y + 3*X + 2*Y + 2", "4*X*Y + 4*X + 3*Y + 3", "Y + 1")]
		public void TestGCD_Multivarite_1(string polyString1, string polyString2, string expected)
		{
			MultivariatePolynomial<T> poly1 = MultivariatePolynomial<T>.Parse(polyString1);
			MultivariatePolynomial<T> poly2 = MultivariatePolynomial<T>.Parse(polyString2);
			MultivariatePolynomial<T> gcd = MultivariatePolynomial<T>.GCD(poly1, poly2);

			string actual = gcd.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GCD({polyString1}, {polyString2});");
		}

		[Ignore("Work in progress")]
		[TestCase("6*X^2 + 3*X*Y + 2*X + Y", "10*Z^2*X + 5*Z^2*Y + 2*Z*X + Z*Y - 2*X - Y", "2*X + Y")]
		public void TestGCD_Multivarite_2(string polyString1, string polyString2, string expected)
		{
			MultivariatePolynomial<T> poly1 = MultivariatePolynomial<T>.Parse(polyString1);
			MultivariatePolynomial<T> poly2 = MultivariatePolynomial<T>.Parse(polyString2);
			MultivariatePolynomial<T> gcd = MultivariatePolynomial<T>.GCD(poly1, poly2);

			string actual = gcd.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GCD({polyString1}, {polyString2});");
		}

		[Ignore("Work in progress")]
		[TestCase("3*X^2*Y - Y", "2*Y^3 + 3*Y*Z", "Y")]
		public void TestGCD_Multivarite_3(string polyString1, string polyString2, string expected)
		{
			MultivariatePolynomial<T> poly1 = MultivariatePolynomial<T>.Parse(polyString1);
			MultivariatePolynomial<T> poly2 = MultivariatePolynomial<T>.Parse(polyString2);
			MultivariatePolynomial<T> gcd = MultivariatePolynomial<T>.GCD(poly1, poly2);

			string actual = gcd.ToString();

			TestContext.WriteLine($"Expected: \"{expected}\"");
			TestContext.WriteLine($"Actual: \"{actual}\"");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial.GCD({polyString1}, {polyString2});");
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
