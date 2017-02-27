using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dialog.Behoerdenloesung.Sitzungen.UI.Web.Models
{
    public class ComboBoxItem<T>
    {
        public T Value { get; set; }
        public string Text { get; set; }

        public ComboBoxItem()
        {
            Value = default(T);
            Text = "";
        }
    }
}