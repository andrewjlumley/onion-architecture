using Common.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Article
{
	public sealed class ArticleType : BaseTypeId<ArticleType.Type>
	{
		public enum Type
		{
			[SerialisationTypeId(0)]
			Unspecified = 0,
			[SerialisationTypeId(10)]
			[Description("Original Research")]
			OriginalResearch,
			[SerialisationTypeId(20)]
			[Description("Review Article")]
			ReviewArticle,
			[SerialisationTypeId(30)]
			[Description("Clinical Case Study")]
			ClinicalCaseStudy,
			[SerialisationTypeId(40)]
			[Description("Clinical Trial")]
			ClinicalTrial,
			[SerialisationTypeId(50)]
			[Description("Opinion")]
			Opinion,
			[SerialisationTypeId(60)]
			[Description("Review")]
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
