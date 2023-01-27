namespace Bookmarket.Domain.Models
{
    //
    // "id": "001",
    // "href": "https://gist.github.com/dahlsailrunner/1765b807940e29951ea6bdfb36cd85dd",
    // "title": "VS Code Configuring for C#"
    public class Bookmark
    {
        public int Id { get; set; }

        public string? Href { get; set; }

        public string Title { get; set; } = String.Empty;

        public ICollection<Tag>? Tags { get; set; } = null;
    }
}
