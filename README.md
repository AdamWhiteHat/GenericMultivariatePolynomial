# GenericMultivariatePolynomial
A multivariate, sparse, generic polynomial arithmetic library. That is, a polynomial in only one indeterminate, X, that only tracks terms with non-zero coefficients. This generic implementation has been tested and supports performing arithmetic on numeric types such as BigInteger, Complex, Decimal, Double, BigComplex, BigDecimal, BigRational, Int32, Int64 and more.

All arithmetic is done __symbolically__. That means the result of an arithmetic operation on two polynomials is another polynomial, not the result of evaluating those two polynomials and performing arithmetic on the results.

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
   * Polynomial evaluation by assigning to the invariant (X in this case) a value.

#


### Polynomial Rings over a Finite Field

* **Polynomial.Field** supports addition, multiplication, division/modulus and inverse of a polynomial ring over a finite field. These operations do not support Complex, BigComplex, BigDecimal, or BigRational types.
   * What this effectively means in less-technical terms is that the polynomial arithmetic is performed in the usual way, but the result is then taken modulus two things:
      * Modulo an integer: All coefficients are reduced modulo an integer.
      * Modulo a polynomial: The whole polynomial is reduced modulo another, smaller, polynomial. This notion works much the same as regular modulus; The modulus polynomial, let's call it g, is declared to be equivalent to zero, and so every multiple of g is reduced to zero. You can think of it this way (although this is not how it's actually carried out): From a large polynomial, g is repeatedly subtracted from that polynomial until it can't subtract g anymore without crossing zero. The result is a polynomial that lies between 0 and g. Just like regular modulus, the result is always less than your modulus, or zero if the polynomial was a multiple of the modulus.
      * Effectively forms a quotient ring
   
* You can instantiate a polynomial in various ways:
   * From a string
      * This is the most massively-useful way and is the quickest way to start working with a particular polynomial you had in mind.
   * From its roots (Not all types supported)
      * Build a polynomial that has, as its roots, all of the numbers in the supplied array. If you want multiplicity of roots, include that number in the array multiple times.
   * From the base-m expansion of a number
      * Given a large number and a radix (base), call it m, a polynomial will be generated that is that number represented in the number base m.
   

* Other methods of interest that are related to, but not necessarily performed on a polynomial (Not all types supported):
   * Euler's Criterion
   * Legendre Symbol and Legendre Symbol Search
   * Tonelli-Shanks
   * Chinese Remainder Theorem
   * Polynomial evaluation by assigning to the invariant (X in this case) a value.

#

