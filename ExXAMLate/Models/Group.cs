using System.Collections.ObjectModel;
using ExXAMLate.Common;

namespace ExXAMLate.Models
{
    public class Group<T> : BindableBase, IGroup
    {
        private ObservableCollection<T> _items = new ObservableCollection<T>();
        private string _title = string.Empty;

        public Group(string title)
        {
            Title = title;
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<T> Items
        {
            get { return _items; }
            set { _items = value; }
        }
    }
}