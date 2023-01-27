namespace Bookmarket.Domain.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public override string ToString()
        {
            return $"{Id}:{Name}";
        }
    }
}
