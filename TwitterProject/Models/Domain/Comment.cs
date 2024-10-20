namespace TwitterProject.Models.Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public string TweetId { get; set; }
    }
}
