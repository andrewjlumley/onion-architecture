using Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Types
{
	public sealed class ArticleType : BaseTypeId<ArticleType.Type>
	{
		public enum Type
		{
			[SerialisationTypeId(0)]
			Unspecified = 0,
			[SerialisationTypeId(10)]
			OriginalResearch,
			[SerialisationTypeId(20)]
			ReviewArticle,
			[SerialisationTypeId(30)]
			ClinicalCaseStudy,
			[SerialisationTypeId(40)]
			ClinicalTrial,
			[SerialisationTypeId(50)]
			Opinion,
			[SerialisationTypeId(60)]
			Review
		}

		public ArticleType(Type type) : base(type)
		{ }

		public static implicit operator Type(ArticleType id)
		{
			return id.Value;
		}

		public static implicit operator ArticleType(Type type)
		{
			return new ArticleType(type);
		}

		public static ArticleType[] AllTypes()
		{
			return AllTypes<Type, ArticleType>();
		}

		public Int32 Serialise()
		{
			return Serialise<Type, Int32>();
		}

		public static ArticleType Unserialise(Int32 id)
		{
			return Unserialise<Type, Int32, ArticleType>(id);
		}
	}
}
