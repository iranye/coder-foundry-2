using Bookmarket.Domain.Models;
using System;

namespace Bookmarket.UI.ViewModel
{
    public class TagViewModel : ViewModelBase
    {
        public TagViewModel(Tag tag)
        {
            this.Id = tag.Id;
            this.Name = tag.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    RaisePropertyChanged();
                }
            }
        }

        public override string ToString()
        {
            return $"{Id}:{Name}";
        }
    }
}
