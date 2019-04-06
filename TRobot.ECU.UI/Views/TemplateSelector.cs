using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TRobot.ECU.UI.Views
{
   public class TemplateSelector : DataTemplateSelector
    {
        public DataTemplate ItemTemplate { get; set; }
        public DataTemplate NewButtonTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == CollectionView.NewItemPlaceholder)
            {
                return NewButtonTemplate;
            }
            else
            {
                return ItemTemplate;
            }
        }
    }
}
