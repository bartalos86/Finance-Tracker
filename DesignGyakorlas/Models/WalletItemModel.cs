using DesignGyakorlas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignGyakorlas.Models
{
   public class WalletItemModel : BaseComboItem
    {

        public WalletItemModel()
        {

            
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
