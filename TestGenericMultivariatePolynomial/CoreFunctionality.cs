using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestPlatform;
using NUnit;
using ExtendedArithmetic;
using NUnit.Framework;

namespace TestMultivariatePolynomial
{
	[TestFixture(Category = "CoreFunctionality")]
	public class CoreFunctionality<T>
	{
		private TestContext m_testContext;
		public TestContext TestContext { get { return m_testContext; } set { m_testContext = value; } }

		[Test]
		[TestCase("X*Y*Z^2 + X*Y + X*Z + Y*Z - 1", "X*Y*Z^2 + X*Y + X*Z + Y*Z - 1")]
		public virtual void TestParse001(string toTest, string expected)
		{
			MultivariatePolynomial<T> testPolynomial = MultivariatePolynomial<T>.Parse(toTest);

			string actual = testPolynomial.ToString();//.Replace(" ", "");
			bool isMatch = (expected == actual);
			string passFailString = isMatch ? "PASS" : "FAIL";
			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{passFailString}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial<T>.Parse(\"{toTest}\").ToString();");
		}

		[Test]
		[TestCase("12", "12")]
		public virtual void TestParse_ConstantPolynomial001(string toTest, string expected)
		{
			MultivariatePolynomial<T> testPolynomial = MultivariatePolynomial<T>.Parse(toTest);

			string actual = testPolynomial.ToString();//.Replace(" ", "");
			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{((expected == actual) ? "PASS" : "FAIL")}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial<T>.Parse(\"{toTest}\").ToString();");
		}

		[Test]
		[TestCase("-12", "-12")]
		public virtual void TestParse_ConstantPolynomial002(string toTest, string expected)
		{
			MultivariatePolynomial<T> testPolynomial = MultivariatePolynomial<T>.Parse(toTest);

			string actual = testPolynomial.ToString();//.Replace(" ", "");
			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{((expected == actual) ? "PASS" : "FAIL")}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial<T>.Parse(\"{toTest}\").ToString();");
		}

		[Test]
		[TestCase("X")]
		public virtual void TestParse_ConstantPolynomial003(string toTest)
		{
			MultivariatePolynomial<T> testPolynomial = MultivariatePolynomial<T>.Parse(toTest);

			string expected = toTest;//.Replace(" ", "");
			string actual = testPolynomial.ToString();//.Replace(" ", "");
			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{((expected == actual) ? "PASS" : "FAIL")}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial<T>.Parse(\"{toTest}\").ToString();");
		}

		[Test]
		[TestCase("-X")]
		public virtual void TestParse_ConstantPolynomial004(string toTest)
		{
			MultivariatePolynomial<T> testPolynomial = MultivariatePolynomial<T>.Parse(toTest);

			string expected = toTest;//.Replace(" ", "");
			string actual = testPolynomial.ToString();//.Replace(" ", "");
			TestContext.WriteLine($"Expected: \"{expected}\"; Actual: \"{actual}\"");
			TestContext.WriteLine($"Pass/Fail: \"{((expected == actual) ? "PASS" : "FAIL")}\"");
			Assert.AreEqual(expected, actual, $"MultivariatePolynomial<T>.Parse(\"{toTest}\").ToString();");
		}

		[Test]
		[TestCase("-8*X^2 - X + 8", "-8*X^2 - X + 8", "-8*X^2", "-X")]
		public virtual void TestParseNegativeLeadingCoefficient(string polynomial, string polynomialExpected, string leadingTermExpected, string secondTermExpected)
		{
			MultivariatePolynomial<T> testPolynomial = MultivariatePolynomial<T>.Parse(polynomial);
			string polynomialActual = testPolynomial.ToString();
			string leadingTermActual = testPolynomial.Terms[0].ToString();
			string secondTermActual = testPolynomial.Terms[1].ToString();

			Assert.AreEqual(polynomialExpected, polynomialActual, $"Expected: \"{polynomialExpected}\"; Actual: \"{polynomialActual}\"");
			Assert.AreEqual(leadingTermExpected, leadingTermActual, $"Expected: \"{leadingTermExpected}\"; Actual: \"{leadingTermActual}\"");
			Assert.AreEqual(secondTermExpected, secondTermActual, $"Expected: \"{secondTermExpected}\"; Actual: \"{secondTermActual}\"");
		}

		[Test]
		public virtual void TestInstantiateZeroCoefficient()
		{
			Indeterminate indt = new Indeterminate('X', 2);
			Term<T> term = new Term<T>(GenericArithmetic<T>.Zero, new Indeterminate[] { indt });

			MultivariatePolynomial<T> testPolynomial = new MultivariatePolynomial<T>(new Term<T>[] { term });
			string expected = "0";
			string actual = testPolynomial.ToString();
			Assert.AreEqual(expected, actual, $"Expected: \"{expected}\"; Actual: \"{actual}\"");
		}

		[Test]
		public virtual void TestInstantiateEmpty()
		{
			MultivariatePolynomial<T> testPolynomial = new MultivariatePolynomial<T>(new Term<T>[0]);
			string expected = "0";
			string actual = testPolynomial.ToString();
			Assert.AreEqual(expected, actual, $"Expected: \"{expected}\"; Actual: \"{actual}\"");
		}

		[Test]
		public virtual void TestInstantiateNull()
		{
			MultivariatePolynomial<T> testPolynomial = new MultivariatePolynomial<T>(null);
			string expected = "0";
			string actual = testPolynomial.ToString();
			Assert.AreEqual(expected, actual, $"Expected: \"{expected}\"; Actual: \"{actual}\"");
		}

		[Test]

		[TestCase("2*x^4 + 13*y^3 + 29*x^2 + 29*y + 13", "45468", "63570", "8551120982818029391")]
		public virtual void TestEvaluate(string polyString, string xValue, string yValue, string expected)
		{
			MultivariatePolynomial<T> poly = MultivariatePolynomial<T>.Parse(polyString);
			List<Tuple<char, T>> indeterminants = new List<Tuple<char, T>>()
			{
				new Tuple<char, T>('x', GenericArithmetic<T>.Parse(xValue)),
				new Tuple<char, T>('y', GenericArithmetic<T>.Parse(yValue)),
			};

			T result = poly.Evaluate(indeterminants);
			string actual = result.ToString();

			TestContext.WriteLine($"Result: \"{actual}\".");
			Assert.AreEqual(expected, actual, $"Test of: MultivariatePolynomial<T>.Evaluate({polyString}) where {string.Join(" and ", indeterminants.Select(tup => $"{tup.Item1} = {tup.Item2}"))}");
		}


		[Test]
		[TestCase("3*X^2*Y^3 + 6*X* Y^4 + X^3*Y^2 + 4*X^5 - 6*X^2*Y + 3*X* Y*Z - 5*X^2 + 3*Y^3 + 24*X* Y - 4", "4*X^5 + 6*X*Y^4 + 3*X^2*Y^3 + X^3*Y^2 + 3*Y^3 - 6*X^2*Y + 3*X*Y*Z - 5*X^2 + 24*X*Y - 4")]
		public virtual void TestMonomialOrdering(string toParse, string expected)
		{
			MultivariatePolynomial<T> poly = MultivariatePolynomial<T>.Parse(toParse);
			string actual = poly.ToString();

			string debug = string.Join(Environment.NewLine,
				poly.Terms.Select(
					   trm =>
							$"Deg:{trm.Degree} Var.Cnt:{trm.VariableCount()} CoEff:{trm.CoEfficient} {string.Join("", trm.Variables.Select(ind => ind.Symbol).OrderBy(c => c).ToList())} => {trm.ToString()}"));
			TestContext.WriteLine($"Term Info:");
			TestContext.WriteLine(debug);

			TestContext.WriteLine($"Result: \"{actual}\".");
			Assert.AreEqual(expected, actual, $"Test of: Monomial Ordering");
		}
	}
}
