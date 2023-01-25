using Bookmarket.Domain.Models;
using System;

namespace Bookmarket.UI.ViewModel
{
    public class TagViewModel
    {
        public TagViewModel(Tag tag)
        {
            this.Id = tag.Id;
            this.Name = tag.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        public override string ToString()
        {
            return $"{Id}:{Name}";
        }
    }
}
