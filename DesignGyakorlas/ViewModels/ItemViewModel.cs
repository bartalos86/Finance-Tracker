using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using System.Threading.Tasks;
using DesignGyakorlas.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Windows;

namespace DesignGyakorlas.ViewModels
{
    [JsonObject]
    public class ItemViewModel : PropertyChangedBase

    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemName">A megvett tárgy(vagy bevétel) neve</param>
        /// <param name="count">Hogy hány darabot vettél belőle</param>
        /// <param name="money">mennyibe kerult, vagy mennyi pent kaptal(jovedelem esetén)</param>
        /// <param name="sale">Hány százalékos akció volt rá</param>
        /// <param name="isIncome">Hogy bevételnek vegye-e vagy mit költség</param>
        [JsonConstructor]
        public ItemViewModel(string itemName, int count, double money, int sale, bool isIncome)
        {
            _itemName = itemName;
            _count = count;
            _money = money;
            _sale = sale;
            _isIncome = isIncome;
          


        }

        public ItemViewModel(string itemName, int count, double money, int sale, ItemTypeModel itemType, bool isIncome)
        {
            _itemName = itemName;
            _count = count;
            _money = money;
            _sale = sale;
            _isIncome = isIncome;
            _itemType = itemType;

        }

        public ItemViewModel(string itemName, int count, double money, int sale, ItemTypeModel itemType, bool isIncome,DateManagerModel dateManager)
        {
            _itemName = itemName;
            _count = count;
            _money = money;
            _sale = sale;
            _isIncome = isIncome;
            _itemType = itemType;
            _dateManager = dateManager;
            

        }

        public ItemViewModel()
        {
            
        }

        public void ItemTypeSetterUsingDatabase()
        {
            _itemType = new ItemTypeModel(ItemTypeNumber);
        }

        private string _itemName;

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; NotifyOfPropertyChange(() => ItemName); }
        }

        private int _count = 1;

        public int Count
        {
            get { return _count; }
            set { _count = value; NotifyOfPropertyChange(() => Count); }
        }

        private double? _money;

        public double? Money
        {
            get { return _money; }
            set { _money = value; NotifyOfPropertyChange(() => Money); }
        }

        private int? _sale;

        public int? Sale
        {
            get {
                if (_sale == null)
                {
                    return 0;
                }
                else { return _sale; }

              }
            set { _sale = value; NotifyOfPropertyChange(() => Sale); }
        }

        private bool _isIncome;

        public bool IsIncome
        {
            get { return _isIncome; }
            set { _isIncome = value; NotifyOfPropertyChange(() => IsIncome); }
        }

        private ItemTypeModel _itemType;

        public ItemTypeModel ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }

        private DateManagerModel _dateManager;

        public DateManagerModel DateManager
        {
            get { return _dateManager; }
            set { _dateManager = value;if(value != null)
                    _isSubscribtion = true; }
        }

        private bool _isSubscribtion = false;

        public bool IsSubscribtion
        {
            get {
                return _isSubscribtion;
            }

            set { _isSubscribtion = value; }
            
        }

        private int _walletID;

        public int WalletID
        {
            get { return _walletID; }
            set { _walletID = value; }
        }


        [JsonIgnore]
        private int _itemTypeNumber;
        [JsonIgnore]
        public int ItemTypeNumber
        {
            get { return _itemTypeNumber; }
            set { _itemTypeNumber = value; ItemTypeSetterUsingDatabase(); }
        }

        [JsonIgnore]
        private int _itemUseCount;
        [JsonIgnore]
        public int ItemUseCount
        {
            get { return _itemUseCount; }
            set { _itemUseCount = value; }
        }

        [JsonIgnore]
        private int _id;
        [JsonIgnore]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        [JsonIgnore]
        private string _itemAddedDatabaseDateString = DateTime.Now.ToString();
        [JsonIgnore]
        public string ItemAddedDatabaseDateString 
        {
            get { return _itemAddedDatabaseDateString; }
            set { _itemAddedDatabaseDateString = value; }
        }

        [JsonIgnore]
        public DateTime ItemAddedDatabaseDate
        {
            get { return DateTime.Parse(_itemAddedDatabaseDateString); }
        }

        private DateTime _itemAddedDate;

        public DateTime ItemAddedDate
        {
            get { return _itemAddedDate; }
            set { _itemAddedDate = value; NotifyOfPropertyChange(() => ItemAddedDate); }
        }
        [JsonIgnore]
        private Visibility _isVisible;
        [JsonIgnore]
        public Visibility IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }



        public static implicit operator BindableCollection<object>(ItemViewModel v)
        {
            throw new NotImplementedException();
        }
    }
}
