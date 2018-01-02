using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignGyakorlas.Models
{
   public class BaseComboItem
    {
        private string _imageSource;

        public string ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; }
        }

        private string _itemText;

        public string ItemText
        {
            get { return _itemText; }
            set { _itemText = value; }
        }

    }
}
