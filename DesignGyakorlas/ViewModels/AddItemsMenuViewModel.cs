using Caliburn.Micro;
using DesignGyakorlas.Models;
using DesignGyakorlas.ViewModels.Commands;
using System.Collections.Generic;

using System.Text;
using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using DesignGyakorlas.Functions;
using System.Threading.Tasks;
using System.Windows.Media;
using System;

namespace DesignGyakorlas.ViewModels
{

    public enum itemType
    {
        HealthAndFitness,
        DrinkAndFood,
        Entertainment,
        Shopping,
        Bill,
        Electronics,
        Other
    }

    public class AddItemsMenuViewModel : Screen
    {

        BindableCollection<ItemViewModel> tempCollection = new BindableCollection<ItemViewModel>();
        List<ItemTypeModel> tempComboBoxItems = new List<ItemTypeModel>();
        List<string> temp2 = new List<string>();
        DataAcess DataAc = new DataAcess();


        //Constructor
        public AddItemsMenuViewModel(ItemViewModel itemToAdd)
        {
            ReturnModel = itemToAdd;

            //Lekerdezi a lekerdezi az 5 leghasznaltabb itemet es beloadolja az autocomplete checkbox listájába hogy indulaskor-
            //-is mar be legyen toltve
            Task.Factory.StartNew(delegate 
            {
                
                List<ItemViewModel> retrievedItems = DataAc.GetSearchedItemWithQuery("SELECT * FROM AutocompleteItems ORDER BY ItemUseCount DESC LIMIT 5");
                if(retrievedItems.Count != 0)
                {
                    foreach(var item in retrievedItems)
                    {
                        temp2.Add(item.ItemName);                      
                    }
                }
            });


            AutocompleteCommand = new CommandHandler(AutocompleteCommandMethod);
            SubmitButtonWithEnter = new CommandHandler(() => SubmitItemsButton());

            
            //ComboBox kategoriakat hozzaadja
            tempComboBoxItems.Add(new ItemTypeModel((int)itemType.HealthAndFitness));
            tempComboBoxItems.Add(new ItemTypeModel((int)itemType.DrinkAndFood));
            tempComboBoxItems.Add(new ItemTypeModel((int)itemType.Entertainment));
            tempComboBoxItems.Add(new ItemTypeModel((int)itemType.Bill));
            tempComboBoxItems.Add(new ItemTypeModel((int)itemType.Electronics));
            ItemTypeModel otherItem = new ItemTypeModel((int)itemType.Other);
            tempComboBoxItems.Add(otherItem);

            //Alapértelmezett valatsztott item
            _comboboxSelectedItem = otherItem;

            //Combobox itemjeit betolti mien tipusi itemek vannak
            _itemTypeComboBox = tempComboBoxItems;

           

            //Ez az autocomplete adatbazisa
            DatabaseNameList = temp2;
        }

       //Ha TAB-ot nyomsz
        public void AutocompleteCommandMethod()
        {
          
            List<ItemViewModel> retrievedItems = DataAc.GetSearchedItem(ProductNameText);
            Task.Factory.StartNew(() => IsItemInDatabaseTextUpdater());

            //Ha az az item benne van az adatbazisba akkor betolti a talat itemek kozul az elsot
            if (retrievedItems.Count != 0)
            {
                //Hozzaad +1-et hogy hanyszor lett hasznallva
                int itemUseCount = retrievedItems[0].ItemUseCount + 1;
                //Bealllitja a textboxot erteket az item parametereire
                ProductPriceText = retrievedItems[0].Money;
                ProductCountText = retrievedItems[0].Count;
                ProductSaleText = retrievedItems[0].Sale;
                ComboboxSelectedItem = tempComboBoxItems.Find(i => i.TypeEnumID == retrievedItems[0].ItemType.TypeEnumID);
              
                //Upadateli az adatbazisba hogy az az item hanyszor lett hasznalva a 'ITemUseCount' valtoztatasaval
                    Task.Factory.StartNew(delegate
                    {
                        DataAc.RunQuery($"UPDATE AutocompleteItems SET ItemUseCount = {itemUseCount} WHERE Id = {retrievedItems[0].ID}");
                    });
                   
            }
        }
        // Az ItemName Textboxhoz van bindelve es egy gomb lenyomasakor fut te
        public void LoadAutocompleteItems()
        {
            //Az ActionExecitionEventContext-bol kiveszi a lenyomott gombot
            // KeyEventArgs keyArgs = input.EventArgs as KeyEventArgs;
            Task.Factory.StartNew(() => IsItemInDatabaseTextUpdater());

            //Rakeres az adatbazisban az adott szovegre es hozzadja az autocomplete listajahoz
            Task.Factory.StartNew(delegate
            {
                List<ItemViewModel> returnedItems = DataAc.GetSearchedItemWithQuery($"select * from AutocompleteItems where ItemName Like '%{ProductNameText}%'");
                foreach (var item in returnedItems)
                {
                    temp2.Add(item.ItemName);
                }
            });
        }


        //AddItemGomb Kattintasa, ami beallitja a ReturnModel-t és leellenorzi a hibakat, ha nincs hiba hozzadja
        public void SubmitItemsButton()
        {
            ReturnModel.ItemName = $"{ProductNameText} (-)";
            ReturnModel.Money = -ProductPriceText;
            ReturnModel.Count = ProductCountText;
            ReturnModel.Sale = ProductSaleText;
            ReturnModel.ItemType = ComboboxSelectedItem;
            ReturnModel.IsIncome = false;
            ReturnModel.ItemAddedDate = DateTime.Now;
            

            //Itt ellenorzi le a van-e hiba
            if (string.IsNullOrWhiteSpace(ProductNameText) || ProductPriceText == null || ProductCountText <= 0)
            {
                StringBuilder messageBuilder = new StringBuilder();

                if (string.IsNullOrWhiteSpace(ProductNameText))
                    messageBuilder.Append("A targy neve nem lehet ures! ");

                if (ProductPriceText == null)
                    messageBuilder.Append("A targy ara nem lehet ures! ");

                if (ProductCountText <= 0)
                    messageBuilder.Append("Nem vehetsz valamibol nullat vagy kevesebbet! ");

                IWindowManager manager = new WindowManager();
                manager.ShowWindow(new CustomDialogueBoxViewModel(new Models.DialogDataModel
                {
                    Title = "Error",
                    Message = messageBuilder.ToString(),
                    OkButtonText = "Ok",
                    SecondButtonNeeded = false

                }));
               // MessageBox.Show(messageBuilder.ToString());
            }
            else
            {
                //Ha a checkbox be van pipalva megprobalja hozzaadni az adatbazishoz vagy updatelni ha mar benne van
                if(CanAddToDatabase)
                Task.Factory.StartNew(() => AddOrUpdateItemInDatabase());

                //Local
                DateTime SubscriptionLastDate = DateTime.Now;
                TimeSpan SubscriptionTimeSpan = TimeSpan.FromDays(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

                if (IsSubscribtion)
                {

                    ReturnModel.DateManager = new DateManagerModel()
                    {
                        SubscriptionLastDate = DateTime.Now,
                        SubscriptionTimeSpan = TimeSpan.FromDays(DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)),
                        SubscriptionTimeSpanDate = SubscriptionLastDate + SubscriptionTimeSpan,
                        MonthAdded = (int)DateTime.Now.Month,
                        DayAdded = (int)DateTime.Now.Day
                    };
                  
                }

                //Ha nincs hiba hozzaadja es bezarja
                CanAddItem = true;
                TryClose(true);
            }
         
        }

        //Kilepes gomb itt nem adja hozza a jovedelmet
        public void ExitButton()
        {
            CanAddItem = false;
            
            TryClose(true);
        }

        #region Metodusok
        //Ez intezi el aza adatbazisban az adott ReturnModel belerakását vagy updatelesét ha meg van hívva
        private void AddOrUpdateItemInDatabase()
        {
            //Formátozza es eltavoltit mindenfele folosleges karaktert a vegerol
            string ReturnModelItemNameFormatted = ReturnModel.ItemName.TrimEnd("(-) ".ToCharArray()).ToLower();

            //Betolti azokat az itemeket az adatbazisbol amiknek ugyanaz a nevuk mint a Returnmondellnek fotmatozva
            List<ItemViewModel> lista = DataAc.GetSearchedItemWithQuery($"select * from AutocompleteItems where ItemName = '{ReturnModelItemNameFormatted}'");
           
            //Ha a nemtalat olyna itemet aminek ugyanaz a neve hozzaadja vagy ha megvaltozott akkor csak updateli
            if (lista.Count > 0)
            {
                //Ha benne van de megvaltozott valamilye updateli
                if (lista[0].ItemName.ToLower() == ReturnModelItemNameFormatted && lista[0].Count != ReturnModel.Count || lista[0].Money != ReturnModel.Money || lista[0].Sale != ReturnModel.Sale || lista[0].ItemTypeNumber != (int)ReturnModel.ItemType.TypeEnumID)
                {
                    DataAc.RunQuery($"Update autocompleteitems SET  Count = {ReturnModel.Count}, Money = {ReturnModel.Money}, Sale = {ReturnModel.Sale}, ItemTypeNumber = {(int)ReturnModel.ItemType.TypeEnumID} WHERE Id = {lista[0].ID};");
                }
            }
            else
            {
                  //Itt adja hozza
                DataAc.RunQuery($"INSERT INTO `autocompleteitems` (`Id`, `ItemName`, `Count`, `Money`, `Sale`, `ItemTypeNumber`, `ItemUseCount`, `ItemAddedDatabaseDateString`) VALUES (NULL, '{ReturnModelItemNameFormatted}', '{ReturnModel.Count}', '{ReturnModel.Money}', '{ReturnModel.Sale}', '{(int)ReturnModel.ItemType.TypeEnumID}', NULL, '{DateTime.Now}');");
            }
        }

        private void IsItemInDatabaseTextUpdater()
        {
            if (DataAc.IsConnectionSuccesful)
            {
                List<ItemViewModel> returnedItemsWithName = DataAc.GetSearchedItemWithQuery($"select * from AutocompleteItems where ItemName = '{ProductNameText}'");
                if (returnedItemsWithName.Count > 0)
                {
                    IsInDatabaseText = "Yes!";
                    IsInDatabaseTextColor = Brushes.Green;
                    TextBoxDatabaseCheckImage = "/DesignGyakorlas;component/Images/Styling/checkmark.png";
                    // MessageBox.Show(returnedItemsWithName[0].ItemAddedDate.ToString());
                    returnedItemsWithName.Clear();
                }
                else
                {
                    TextBoxDatabaseCheckImage = "/DesignGyakorlas;component/Images/Styling/redx.png";
                    IsInDatabaseText = "No!";
                    IsInDatabaseTextColor = Brushes.Red;
                    returnedItemsWithName.Clear();
                }
            }
            else
            {
                TextBoxDatabaseCheckImage = "/DesignGyakorlas;component/Images/Styling/redx.png";
                    IsInDatabaseText = "Error!";
                    IsInDatabaseTextColor = Brushes.Red;
                

            }
        }

        #endregion

        #region Properties

        //ItemViewModel amit visszapasszol a main windownak
        private ItemViewModel _returnModel;

        public ItemViewModel ReturnModel
        {
            get { return _returnModel; }
            set { _returnModel = value; NotifyOfPropertyChange(() => ReturnModel); }
        }

        //Ettol fugg hogy a masik window regiszrtalje-e
        private bool _canAddItem;

        public bool CanAddItem
        {
            get { return _canAddItem; }
            set { _canAddItem = value; NotifyOfPropertyChange(() => CanAddItem); }
        }


        //Nev textboxhoz van bidelve
        private string _productNameText = null;

        public string ProductNameText
        {
            get { return _productNameText; }
            set { _productNameText = value; NotifyOfPropertyChange(() => ProductNameText); }
        }

        //Ar textboxhoz van bindelve
        private double? _productPriceText = null;

        public double? ProductPriceText
        {
            get { return _productPriceText; }
            set { _productPriceText = value; NotifyOfPropertyChange(() => ProductPriceText); }
        }

        //Darab TextBoxhoz van bindelve
        private int _productCountText = 1;

        public int ProductCountText
        {
            get { return _productCountText; }
            set { _productCountText = value; NotifyOfPropertyChange(() => ProductCountText); }

        }
        
        //Akcio TextBoxhoz van bindelve
        private int? _productSaleText = null;

        public int? ProductSaleText
        {
            get { return _productSaleText; }
            set { _productSaleText = value; NotifyOfPropertyChange(() => ProductSaleText); }
        }

        private string _isInDatabaseText;

        public string IsInDatabaseText
        {
            get { return _isInDatabaseText; }
            set { _isInDatabaseText = value; NotifyOfPropertyChange(() => IsInDatabaseText); }
        }

        private Brush _isInDatabaseTextColor;

        public Brush IsInDatabaseTextColor
        {
            get { return _isInDatabaseTextColor; }
            set { _isInDatabaseTextColor = value; NotifyOfPropertyChange(() => IsInDatabaseTextColor); }
        }



        //Ez az az adatbazis ami az autocomplete TextBoxhoz kell, ide vannak beleteve az arak ...stb
        private BindableCollection<ItemViewModel> _itemDatabase;

        public BindableCollection<ItemViewModel> ItemDatabase
        {
            get { return _itemDatabase; }
            set { _itemDatabase = value; NotifyOfPropertyChange(() => ItemDatabase); }
        }

        //Ez az Autocomplethez van bindolva es ebben vannak az Itemek nevei.
        private List<string> _databaseNameList;

        public List<string> DatabaseNameList
        {
            get { return _databaseNameList; }
            set { _databaseNameList = value; NotifyOfPropertyChange(() => DatabaseNameList); }
        }

        //Itemek tipusai a Comboboxban (Combobox itemei)
        private List<ItemTypeModel> _itemTypeComboBox;
         
        public List<ItemTypeModel> ItemTypeComboBox
        {
            get { return _itemTypeComboBox; }
            set { _itemTypeComboBox = value; NotifyOfPropertyChange(() => ItemTypeComboBox); }
        }

        //A combobox altal kivalasztott item
        private ItemTypeModel _comboboxSelectedItem;

        public ItemTypeModel ComboboxSelectedItem
        {
            get { return _comboboxSelectedItem; }
            set { _comboboxSelectedItem = value;NotifyOfPropertyChange(() => ComboboxSelectedItem); }
        }

        //Ez van hozzakotve az 'AddToDatabase' CheckBox-hoz 
        private bool _canAddToDatabase;

        public bool CanAddToDatabase
        {
            get { return _canAddToDatabase; }
            set { _canAddToDatabase = value;NotifyOfPropertyChange(() => CanAddToDatabase); }
        }

        private string _textBoxDatabaseCheckImage;

        public string TextBoxDatabaseCheckImage
        {
            get { return _textBoxDatabaseCheckImage; }
            set { _textBoxDatabaseCheckImage = value;NotifyOfPropertyChange(() => TextBoxDatabaseCheckImage); }
        }

        private bool _isSubscribtion;

        public bool IsSubscribtion
        {
            get { return _isSubscribtion; }
            set { _isSubscribtion = value; NotifyOfPropertyChange(() => IsSubscribtion); }
        }


        #endregion

        #region Commands

        //Ez az a Parncs amit a TAB gombra van bindelve és lefuttatja a beallitott Metódust
        public CommandHandler AutocompleteCommand { get; private set; }

        //Ez a Parancs ami az ENTER gombra van bindelve es lefuttatja a beallitott Metodust
        public CommandHandler SubmitButtonWithEnter { get; private set; }

        #endregion
    }
}
