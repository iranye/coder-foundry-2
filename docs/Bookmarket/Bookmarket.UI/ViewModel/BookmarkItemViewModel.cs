using Bookmarket.Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Bookmarket.UI.ViewModel
{
    public class BookmarkItemViewModel : ViewModelBase
    {
        private readonly Bookmark _model;

        public BookmarkItemViewModel(Bookmark model)
        {
            _model = model;
            if (_model.Tags is not null)
            {
                foreach (var tag in _model.Tags)
                {
                    Tags.Add(new TagViewModel(tag));
                }
            }
        }

        public BookmarkItemViewModel()
        {
            _model = new Bookmark();
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

        public string? Desc
        {
            get => _model.Desc;
            set
            {
                if (value is not null && value != _model.Desc)
                {
                    _model.Desc = value;
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
            if ((Title != null && Title.ToLower().Contains(filter))
                || (Href != null && Href.ToLower().Contains(filter))
                || (Desc != null && Desc.ToLower().Contains(filter)))
            {
                ret = true;
            }
            return ret;
        }

        internal void ApplyTags(List<TagViewModel> tagViewModels)
        {
            foreach (var tagViewModel in tagViewModels)
            {
                if (!Tags.Any(t => t.Id == tagViewModel.Id))
                {
                    Tags.Add(tagViewModel);
                }
            }
            RaisePropertyChanged("TagsCsv");
        }

        public ObservableCollection<TagViewModel> Tags { get; set; } = new ObservableCollection<TagViewModel>();

        public string? _tagsCsv = String.Empty;
        public string TagsCsv
        {
            get
            {
                if (Tags is null || Tags.Count == 0)
                {
                    return String.Empty;
                }
                var tagTitles = Tags.Select(t => t.ToString()).ToList();
                _tagsCsv = string.Join(",", tagTitles);
                return _tagsCsv;
            }
            set
            {
                _tagsCsv = value;
                if (Tags is null)
                {
                    Tags = new ObservableCollection<TagViewModel>();
                }
                else
                {
                    Tags.Clear();
                }
                if (!String.IsNullOrWhiteSpace(_tagsCsv))
                {
                    var tagsArr = _tagsCsv.Split(",");
                    foreach (var tag in tagsArr)
                    {
                        // TODO: probably have to inject _tagsDataProvider
                        if (tag.Contains(":"))
                        {
                            var tagsAgg = tag.Split(':');
                            if (Int32.TryParse(tagsAgg[0], out var tagId))
                            {
                                Tags.Add
                                (
                                    new TagViewModel
                                    (
                                        new Tag { Id = tagId, Name = tagsAgg[1] }
                                    )
                                );
                            }
                        }
                    }
                }
                RaisePropertyChanged("TagsCsv");
            }
        }
    }
}
