using DesignGyakorlas.ViewModels;


namespace DesignGyakorlas.Models
{
    public class CurrencyTypeModel : BaseComboItem
    {

        public CurrencyTypeModel(CurrencyType type)
        {
            
            _type = type;

            switch (type)
            {
                case CurrencyType.USD:
                    ItemText = "USD";
                    ImageSource = "/DesignGyakorlas;component/Images/Icons/dollar.png";
                    break;
                case CurrencyType.EUR:
                    ItemText = "EUR";
                    ImageSource = "/DesignGyakorlas;component/Images/Icons/euro.jpg";
                    break;
                case CurrencyType.BTC:
                    ItemText = "BTC";
                    ImageSource = "/DesignGyakorlas;component/Images/Icons/bitcoin.png";
                    break;
            }
           
        }

     

        private CurrencyType _type;

        public CurrencyType Type
        {
            get { return _type; }
            set { _type = value; }
        }



    }
}
