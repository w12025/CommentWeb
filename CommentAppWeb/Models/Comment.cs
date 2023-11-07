namespace CommentAppWeb.Models;

public class Comment
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Username { get; set; }
}