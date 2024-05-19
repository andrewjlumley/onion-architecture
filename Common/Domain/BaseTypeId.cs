using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;

namespace Common.Domain
{
	public abstract class BaseTypeId<TPrimitiveType> : IEquatable<BaseTypeId<TPrimitiveType>>, IComparable<BaseTypeId<TPrimitiveType>>, IComparable
		where TPrimitiveType : notnull
	{
		public BaseTypeId(TPrimitiveType id)
		{
			Value = id;
		}

		public TPrimitiveType Value { get; set; }

		#region Equals

		public override bool Equals(object? other)
		{
			return other is BaseTypeId<TPrimitiveType> id && Value.Equals(id.Value);
		}

		public bool Equals(BaseTypeId<TPrimitiveType>? other)
		{
			return Equals((object?)other);
		}

		public static bool operator ==(BaseTypeId<TPrimitiveType> lhs, BaseTypeId<TPrimitiveType> rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(BaseTypeId<TPrimitiveType> lhs, BaseTypeId<TPrimitiveType> rhs)
		{
			return !(lhs == rhs);
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}

		#endregion

		#region Comparison

		public int CompareTo(object? other)
		{
			var id = other as BaseTypeId<TPrimitiveType>;
			return Comparer<TPrimitiveType>.Default.Compare(Value!, id!.Value);
		}

		public int CompareTo(BaseTypeId<TPrimitiveType>? other)
		{
			return CompareTo((object?)other);
		}

		#endregion

		#region Enum list and serialisation

		protected static TTypeObject[] AllTypes<TType, TTypeObject>()
		{
			return EnumHelper.GetEnumList<TType>().Select(f => (TTypeObject)Activator.CreateInstance(typeof(TTypeObject), f)!).ToArray();
		}

		public TTypeId Serialise<TType, TTypeId>() 
			where TType : Enum
		{
			var enumType = (Enum)(object)Value;
			return enumType.GetAttributeOfType<SerialisationTypeIdAttribute>()!.GetTypeId<TTypeId>();
		}

		public static TTypeObject Unserialise<TType, TTypeId, TTypeObject>(TTypeId id)
			where TTypeObject : BaseTypeId<TPrimitiveType>
			where TType : Enum
		{
			var matches = BaseTypeId<TPrimitiveType>.AllTypes<TType, TTypeObject>().Where(f => f.Serialise<TType, TTypeId>()!.Equals(id));
			if (matches.IsNullOrEmpty())
				throw new Exception($"Cannot unserialise object type '{typeof(TTypeObject).ToString()}' from Id: {id}.");

			return (TTypeObject)matches.First()!;
		}

		#endregion

		public override string ToString()
		{
			return Value?.ToString() ?? String.Empty;
		}
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class SerialisationTypeIdAttribute : Attribute
	{
		object _id;

		public SerialisationTypeIdAttribute(object id)
		{
			_id = id;
		}

		public TValue GetTypeId<TValue>() => (TValue)_id;
	}
}
