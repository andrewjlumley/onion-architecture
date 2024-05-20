namespace Contracts
{
    public class ArticleRequest
	{
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new();
    }

    public class ArticleCommentRequest
    {
		public string Comment { get; set; } = string.Empty;
	}
}