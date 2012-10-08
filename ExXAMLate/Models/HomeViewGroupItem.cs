using System.Windows.Input;

namespace ExXAMLate.Models
{
    public class HomeViewGroupItem
    {
        public string Background { get; set; }
        public string Title { get; set; }
        public ICommand Clicked { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}