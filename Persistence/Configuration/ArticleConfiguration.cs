using Domain.Entities;
using Domain.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System.Reflection.Emit;

namespace Persistence.Configurations
{
	internal sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
	{
		public void Configure(EntityTypeBuilder<Article> builder)
		{
			builder.ToTable("Articles");
			builder.HasKey(article => article.Id);
			builder.Property(article => article.Id).IsRequired();
			builder.Property(article => article.Title).HasMaxLength(50);
			builder.Property(article => article.ArticleType).HasConversion(
				v => v.Serialise(),
				v => ArticleType.Unserialise(v)
			);
			builder.Property(article => article.Content).HasMaxLength(200);
			builder.Property(article => article.Tags).HasConversion(
				v => JsonConvert.SerializeObject(v),
				v => JsonConvert.DeserializeObject<List<String>>(v) ?? new List<String>()
			);
			builder.Property(article => article.Created).IsRequired();
			builder.Property(article => article.Published);
			builder.Property(article => article.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
			builder.HasMany(article => article.Comments)
				.WithOne(article => article.Article)
				.HasForeignKey(articleComment => articleComment.ArticleId)
				.HasPrincipalKey(article => article.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}

	internal sealed class ArticleCommentConfiguration : IEntityTypeConfiguration<ArticleComment>
	{
		public void Configure(EntityTypeBuilder<ArticleComment> builder)
		{
			builder.ToTable("ArticleComments");
			builder.HasKey(articleComment => articleComment.Id);
			builder.Property(articleComment => articleComment.Id).ValueGeneratedNever().IsRequired();
			builder.Property(articleComment => articleComment.ArticleId).IsRequired(); 
			builder.Property(articleComment => articleComment.Created).IsRequired();
			builder.Property(articleComment => articleComment.Comment).HasMaxLength(250);
		}
	}
}