using ExXAMLate.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace ExXAMLate.Common
{
    public class ContainerStyleSelector : StyleSelector
    {
        public Style AnatomyOfButtons { get; set; }

        public Style DefaultTemplate { get; set; }
        
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            var group = (ICollectionViewGroup)item;
            if (group != null)
            {
                var g = ((IGroup)group.Group);
                if (g is ButtonAnatomyGroup)
                    return AnatomyOfButtons;
            }

            return base.SelectStyleCore(item, container);
        }
    }
}