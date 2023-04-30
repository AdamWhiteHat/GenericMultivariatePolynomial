using System.Linq;
using System.Collections.Generic;

namespace ExtendedArithmetic
{
	/// <summary>
	/// Interface ICloneable
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICloneable<T>
	{

		/// <summary>
		/// Clones this instance.
		/// </summary>
		T Clone();
	}

	/// <summary>
	/// Class CloneHelper.
	/// </summary>
	public static class CloneHelper<T>
	{

		/// <summary>
		/// Clones the collection.
		/// </summary>
		public static IEnumerable<T> CloneCollection(IEnumerable<ICloneable<T>> list)
		{
			return list.Select(t => t.Clone());
		}
	}
}
