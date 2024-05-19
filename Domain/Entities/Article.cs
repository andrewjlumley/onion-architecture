using Common.Domain;
using Domain.Types;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public sealed class Article : DomainBase<Article>, IValidatableObject
{
	private string _content = string.Empty;

	public Article()
	{
		ArticleType = ArticleType.Type.Unspecified;
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

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? PublishedOnUtc { get; set; }

	public byte[] Version { get; set; } = null!;

	#endregion

	#region Recalculate

	[NotifiedBy(nameof(Content))]
	public static void RecalculateOnContentChange(Article article)
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
