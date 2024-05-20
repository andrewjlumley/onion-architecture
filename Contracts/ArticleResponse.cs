using System.Collections.ObjectModel;

namespace Contracts
{
    public class ArticleResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
        public DateTime Created { get; set; }
        public DateTime? Published { get; set; }
        public ICollection<ArticleCommentResponse> Comments { get; set; } = new Collection<ArticleCommentResponse>();
    }

    public class ArticleCommentResponse
    {
		public Guid Id { get; set; }
		public Guid ArticleId { get; set; }
		public DateTime Created { get; set; }
		public string Comment { get; set; } = string.Empty;
	}
}
