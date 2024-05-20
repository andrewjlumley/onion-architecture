using Common.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
	public sealed class ArticleComment : DomainBase<ArticleComment>, IValidatableObject
	{
		private string _comment = string.Empty;

		public static ArticleComment Create(Guid articleId)
		{
			return new ArticleComment { Id = Guid.NewGuid(), ArticleId = articleId, Created = DateTime.Now };
		}

		#region ArticleComment members

		public Guid Id { get; set; }

		public Guid ArticleId { get; set; }

		public DateTime Created { get; set; }

		[Required]
		[StringLength(250)]
		public String Comment
		{
			get { return _comment; }
			set { Setter<string>(() => _comment, (v) => _comment = v, value, nameof(Comment)); }
		}

		public Article Article { get; } = null!;

		#endregion

		#region IValidatableObject members

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			yield break;
		}

		#endregion
	}
}
