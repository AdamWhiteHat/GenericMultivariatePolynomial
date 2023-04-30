using System;
using System.Linq;
using System.Numerics;
using System.Globalization;
using System.Collections.Generic;

namespace ExtendedArithmetic
{
	/// <summary>
	/// Class Indeterminate.
	/// Implements the <see cref="ExtendedArithmetic.ICloneable{ExtendedArithmetic.Indeterminate}" />
	/// Implements the <see cref="System.IEquatable{ExtendedArithmetic.Indeterminate}" />
	/// Implements the <see cref="System.Collections.Generic.IEqualityComparer{ExtendedArithmetic.Indeterminate}" />
	/// </summary>
	/// <seealso cref="ExtendedArithmetic.ICloneable{ExtendedArithmetic.Indeterminate}" />
	/// <seealso cref="System.IEquatable{ExtendedArithmetic.Indeterminate}" />
	/// <seealso cref="System.Collections.Generic.IEqualityComparer{ExtendedArithmetic.Indeterminate}" />
	public class Indeterminate : ICloneable<Indeterminate>, IEquatable<Indeterminate>, IEqualityComparer<Indeterminate>
	{
		/// <summary>
		/// Gets the indeterminate's symbol.
		/// </summary>
		public char Symbol { get; }

		/// <summary>
		/// Gets the indeterminate's exponent.
		/// </summary>
		public int Exponent { get; }

		/// <summary>
		/// Gets the empty indeterminate.
		/// </summary>
		internal static Indeterminate[] Empty = new Indeterminate[0];

		/// <summary>
		/// Gets the zero indeterminate.
		/// </summary>
		internal static Indeterminate[] Zero = new Indeterminate[] { new Indeterminate('X', 0) };

		private UnicodeCategory[] AllowedSymbolCategories = new UnicodeCategory[]
		{
			UnicodeCategory.LowercaseLetter,
			UnicodeCategory.UppercaseLetter,
			UnicodeCategory.ModifierLetter,
			UnicodeCategory.MathSymbol
		};

		#region Constructor & Parse

		/// <summary>
		/// Initializes a new instance of the <see cref="Indeterminate"/> class, given the symbol and the exponent.
		/// </summary>
		/// <param name="symbol">The symbol.</param>
		/// <param name="exponent">The exponent.</param>
		/// <exception cref="System.ArgumentException">Parameter {nameof(symbol)} must be a letter character.</exception>
		public Indeterminate(char symbol, int exponent)
		{
			var symbolCategory = CharUnicodeInfo.GetUnicodeCategory(symbol);
			if (!AllowedSymbolCategories.Contains(symbolCategory))
			{
				throw new ArgumentException($"Parameter {nameof(symbol)} must be a letter character.");
			}
			Symbol = symbol;
			Exponent = exponent;
		}

		/// <summary>
		/// Parses the string representation of an indeterminate into a new instance.
		/// </summary>
		/// <exception cref="System.FormatException"></exception>
		internal static Indeterminate Parse(string input)
		{
			int exponent = 1;

			string[] parts = input.Split(new char[] { '^' });

			if (parts[0].Length != 1)
			{
				throw new FormatException();
			}

			char symbol = parts[0][0];

			if (!char.IsLetter(symbol))
			{
				throw new FormatException();
			}

			if (parts.Length == 2)
			{
				if (!parts[1].All(c => char.IsDigit(c)))
				{
					throw new FormatException();
				}
				exponent = int.Parse(parts[1]);
			}

			return new Indeterminate(symbol, exponent);
		}

		#endregion

		#region Overrides and Interface implementations

		/// <summary>
		/// Clones this instance.
		/// </summary>
		public Indeterminate Clone()
		{
			return new Indeterminate(this.Symbol, this.Exponent);
		}

		/// <summary>
		/// Returns true if the two indeterminates are equal.
		/// </summary>
		/// <returns><see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.</returns>
		public bool Equals(Indeterminate other)
		{
			return this.Equals(this, other);
		}

		/// <summary>
		/// Returns true if the two indeterminates are equal.
		/// </summary>
		/// <returns><see langword="true" /> if the specified objects are equal; otherwise, <see langword="false" />.</returns>
		public bool Equals(Indeterminate x, Indeterminate y)
		{
			if (x == null) { return (y == null) ? true : false; }
			if (x.Symbol != y.Symbol) { return false; }
			if (x.Exponent != y.Exponent) { return false; }
			return true;
		}

		/// <summary>
		/// Returns true if the two indeterminates share the same exponent and symbol.
		/// </summary>
		internal static bool AreCompatable(Indeterminate left, Indeterminate right)
		{
			if (left.Exponent == 0 || right.Exponent == 0)
			{
				return left.Exponent == right.Exponent;
			}
			return left.Symbol == right.Symbol;
		}

		/// <summary>
		/// Returns true if the two indeterminates are equal.
		/// </summary>
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Indeterminate);
		}

		/// <summary>
		/// Returns a hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		public int GetHashCode(Indeterminate obj)
		{
			return obj.GetHashCode();
		}

		/// <summary>
		/// Returns a hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		public override int GetHashCode()
		{
			if (Exponent == 0)
			{
				return new Tuple<char, int>('X', 0).GetHashCode();
			}
			return new Tuple<char, int>(Symbol, Exponent).GetHashCode();
		}

		/// <summary>
		/// Returns the <see cref="System.String" /> equivalent of thisindeterminate.
		/// </summary>
		public override string ToString()
		{
			//return (Exponent == 1) ? Symbol.ToString() : $"{Symbol}^{Exponent}";
			if (Exponent == 0)
			{
				return string.Empty;
			}
			else if (Exponent == 1)
			{
				return Symbol.ToString();
			}
			else
			{
				return $"{Symbol}^{Exponent}";
			}
		}

		#endregion
	}
}
