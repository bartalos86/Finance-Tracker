using Caliburn.Micro;
using DesignGyakorlas.ViewModels;
using Newtonsoft.Json;

namespace DesignGyakorlas.Models
{
    [JsonObject]
    public class ItemTypeModel : PropertyChangedBase
    {
        [JsonConstructor]
        public ItemTypeModel(string iconSource,string itemText, itemType typeEnumId)
        {     
            _iconSource = iconSource;
            _itemText = itemText;
            _typeEnumId = typeEnumId;
        }

        public ItemTypeModel(itemType typeEnumId)
        {
            _typeEnumId = typeEnumId;

        }

        public ItemTypeModel(int TypeEnumIdInNumber)
        {
            _typeEnumId = (itemType) TypeEnumIdInNumber;
            switch (_typeEnumId)
            {
                case itemType.DrinkAndFood:
                    _iconSource = "/DesignGyakorlas;component/Images/Icons/food2.png";
                    _itemText = "Drink And Food";
                    break;
                case itemType.HealthAndFitness:
                    _iconSource = "/DesignGyakorlas;component/Images/Icons/health3.png";
                    _itemText = "Health And Fitness";
                    break;
                case itemType.Entertainment:
                    _iconSource = "/DesignGyakorlas;component/Images/Icons/entertainment.png";
                    _itemText = "Entertainment";
                    break;
                case itemType.Shopping:
                    _iconSource = "/DesignGyakorlas;component/Images/Icons/shopping.png";
                    _itemText = "Shopping";
                    break;
                case itemType.Bill:
                    _iconSource = "/DesignGyakorlas;component/Images/Icons/bill.png";
                    _itemText = "Bill";
                    break;
                case itemType.Electronics:
                    _iconSource = "/DesignGyakorlas;component/Images/Icons/electronics.png";
                    _itemText = "Electronics";
                    break;
                case itemType.Other:
                    _iconSource = "/DesignGyakorlas;component/Images/Icons/other.png";
                    _itemText = "Other";
                    break;
            }
        }

        private string _iconSource;

        public string IconSource
        {
            get { return _iconSource; }
            set { _iconSource = value; NotifyOfPropertyChange(() => IconSource); }
        }

        private string _itemText;

        public string ItemText
        {
            get { return _itemText; }
            set { _itemText = value;NotifyOfPropertyChange(() => ItemText); }
        }

        private itemType _typeEnumId;

        public itemType TypeEnumID
        {
            get { return _typeEnumId; }
            set { _typeEnumId = value; NotifyOfPropertyChange(() => TypeEnumID); }
        }




    }
}
