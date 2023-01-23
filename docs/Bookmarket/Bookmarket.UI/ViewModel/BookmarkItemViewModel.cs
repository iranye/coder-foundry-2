using Bookmarket.Domain.Models;

namespace Bookmarket.UI.ViewModel
{
    public class BookmarkItemViewModel : ViewModelBase
    {
        private readonly Bookmark _model;

        public BookmarkItemViewModel(Bookmark model)
        {
            _model = model;
        }

        public int Id
        {
            get => _model.Id;
            set => _model.Id = value;
        }

        public string? Title
        {
            get => _model.Title;
            set
            {
                if (value is not null && value != _model.Title)
                {
                    _model.Title = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string? Href
        {
            get => _model.Href;
            set
            {
                if (value is not null && value != _model.Href)
                {
                    _model.Href = value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool HasFilterString(string filter)
        {
            bool ret = false;
            if ((Title != null && Title.ToLower().Contains(filter)) || (Href != null && Href.ToLower().Contains(filter)))
            {
                ret = true;
            }
            return ret;
        }
    }
}
