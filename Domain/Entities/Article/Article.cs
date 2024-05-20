using Common.Domain;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Domain.Entities.Article;

public sealed class Article : DomainBase<Article>, IValidatableObject
{
	private string _content = string.Empty;

	public Article()
	{
		ArticleType = ArticleType.Type.Unspecified;
		((ObservableCollection<ArticleComment>)Comments).CollectionChanged += (s, e) =>
		{
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
			{
				Register(this.GetType(), (IDomainObject)e.NewItems![0]!);
			}
		};
	}

	public static Article Create()
	{
		return new Article { Id = Guid.NewGuid(), Created = DateTime.Now };
	}

	#region Article members

	public Guid Id { get; set; }

	[Required]
	[StringLength(50)]
	public string Title { get; set; } = string.Empty;

	public ArticleType ArticleType { get; set; }

	[Required(ErrorMessage = "You must assign Content here")]
    [StringLength(200, ErrorMessage = "The length musn't exceed {1} characters")]
    public string Content
	{
        get { return _content; }
		set { Setter<string>(() => _content, (v) => _content = v, value, nameof(Content)); }
	}

	public List<string> Tags { get; set; } = new();

    public DateTime Created { get; set; }

    public DateTime? Published { get; set; }

	public byte[] Version { get; set; } = null!;

	public ICollection<ArticleComment> Comments { get; set; } = new ObservableCollection<ArticleComment>();

	#endregion

	#region Recalculate

	[NotifiedBy(nameof(Content))]
	public static void RecalculateOnContentChange(Article article)
	{
		Console.WriteLine();
	}

	[NotifiedBy(nameof(ArticleComment.Comment))]
	public static void RecalculateOnArticleCommentChange(Article article, ArticleComment comment)
	{
		Console.WriteLine();
	}

	#endregion

	#region IValidatableObject members

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (String.IsNullOrWhiteSpace(Title))
			yield return new ValidationResult("No title assigned",new[] { nameof(Title) });
	}

	#endregion
}
