namespace TwitterProject.Models.Domain
{
    public class Tweet
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public int Upvote { get; set; }
        public bool Liked { get; set; } 

    }
}
