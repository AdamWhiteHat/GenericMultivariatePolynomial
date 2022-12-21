# GenericMultivariatePolynomial
A multivariate, sparse, generic polynomial arithmetic library.

This generic implementation has been tested and supports performing arithmetic on numeric types such as BigInteger, Complex, Decimal, Double, BigComplex, BigDecimal, BigRational, Int32, Int64 and more.

All arithmetic is done __symbolically__. That means the result of an arithmetic operation on two polynomials is another polynomial, not the result of evaluating those two polynomials and performing arithmetic on the results.


An example of a multivariate polynomial would be:

16\*X^2\*Y + 12\*X\*Y - 4\*Y - 1


#


### Generic Arithmetic Type Polynomial

* All classes and methods support a generic type T, which can be any data type that has arithmetic operator overloads. Explicitly, the following types have tests exercising all the standard arithmetic operations (Addition, Subtraction, Multiplication, Division, Exponentiation, Modulus, Square Root, Equality and Comparison (where applicable--Complex numbers are not orderable)) and so are well supported and come with verifiable proof that they work:
   * Every .NET CLR value type (Byte, Int16, Int32, Int64, UInt16, UInt32, UInt64, Float, Double, Decimal)
   * System.Numerics.BigInteger
   * System.Numerics.Complex
   * [ExtendedNumerics.BigComplex](https://github.com/AdamWhiteHat/BigComplex) (My arbitrary-precision complex number type library)
   * [ExtendedNumerics.BigDecimal](https://github.com/AdamWhiteHat/BigDecimal) (My arbitrary-precision floating-point number type library.)
   * [ExtendedNumerics.BigRational](https://github.com/AdamWhiteHat/BigRational)(My arbitrary precision rational number type library.)
   * [ExtendedNumerics.Fraction](https://github.com/AdamWhiteHat/BigRational/blob/master/BigRational/Fraction.cs) (My arbitrary-precision rational number type library.)
   * Any type that has the Addition, Multiplication and Exponentiation operator overloads (at a minimum).

 
* Supports **symbolic** multivariate polynomial (generic) arithmetic including:
   * Addition
   * Subtraction
   * Multiplication
   * Division
   * Modulus
   * Exponentiation
   * GCD of polynomials
   * Derivative
   * Integral
   * Reciprocal
   * Irreducibility checking
   * Polynomial evaluation by assigning values to the invariants.

#

