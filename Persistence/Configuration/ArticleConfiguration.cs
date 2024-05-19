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
			builder.Property(article => article.Id).HasMaxLength(20).IsRequired();
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
			builder.Property(article => article.CreatedOnUtc).IsRequired();
			builder.Property(article => article.PublishedOnUtc);
			builder.Property(a => a.Version).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
		}
    }
}
