using DesignGyakorlas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignGyakorlas.Models
{
   public class WalletItemModel
    {

        public WalletItemModel()
        {

            
        }

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

        private CurrencyType _type;

        public CurrencyType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private int _walletID;

        public int WalletID
        {
            get { return _walletID; }
            set { _walletID = value; }
        }

        private double? _money;

        public double? Money
        {
            get { return _money; }
            set { _money = value; }
        }


    }
}
