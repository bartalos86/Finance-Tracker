using Caliburn.Micro;
using DesignGyakorlas.Models;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using DesignGyakorlas.ViewModels.Commands;
using System.Text;
using System;
using System.Linq;

namespace DesignGyakorlas.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        #region Valtozok
        //Letrehoz egy listat amit majd áttad a bindelt listanak
        BindableCollection<ItemViewModel> temporary = new BindableCollection<ItemViewModel>();
        BindableCollection<ItemViewModel> fulltemporary = new BindableCollection<ItemViewModel>();
        string savePath = $"{Directory.GetCurrentDirectory()}/Saves/";
        int? previousItemIndex;
        int subIndexCounter = 0;
        #endregion
        //Constructor
        public ShellViewModel()
        {
           
            _windowHeight = 450;
            Task.Factory.StartNew(LoadSavedItems);

          //  temporary.Add(new ItemViewModel("Kifli",5,0.05,0,new ItemTypeModel(null,null,itemType.DrinkAndFood),false));
            Items = temporary;
            FullItems = fulltemporary;
            //   CurrentMoney = CalculateBalance();
           // CalculateBalance();
          

        }

       void Teszt()
        {
            MessageBox.Show("shell");
        }

        //Mikor megnyomjuk az AddItems gombot, megnyitja a menut majd onnan lekeri az objectumot es berakja a listaba
        public void AddItemsButton()  
        {
            
            ItemViewModel itemToAdd = new ItemViewModel("empty", 0, 0, 0,false);
            AddItemsMenuViewModel form = new AddItemsMenuViewModel(itemToAdd);

            IWindowManager manager = new WindowManager();
            
            if (manager.ShowDialog(form).Value == true)
            {
                if (form.CanAddItem)
                {
                    itemToAdd = form.ReturnModel;
                    itemToAdd.WalletID = inputData.SelectedWalletID;
                    foreach (var item in Items)
                    {

                        if ((item.ItemName) == (itemToAdd.ItemName) && (item.Money) == (itemToAdd.Money) && (item.Sale) == (itemToAdd.Sale) && (item.ItemType.TypeEnumID) == (itemToAdd.ItemType.TypeEnumID) && item.IsIncome == false)
                        {
                            MessageBox.Show("talalt");
                            itemToAdd.Count += temporary[temporary.IndexOf(item)].Count;
                            temporary.RemoveAt(temporary.IndexOf(item));
                            break;
                        }
                        
                    }
                    temporary.Add(itemToAdd);
                    CalculateBalance();

                    //Logika
                    //  CurrentMoney = CalculateBalance();
                    //  CalculateBalance();
                } 
            }
        }

        //Jovedelem hozzaadasa
       public void AddIncomeButton()
        {
            IWindowManager manager = new WindowManager();
            IncomeViewModel incomeItemToAdd = new IncomeViewModel("Fusi",2.0);
            AddIncomeViewModel form_AddIncome = new AddIncomeViewModel(incomeItemToAdd);

            ItemTypeModel typeWithMoneyIcon = new ItemTypeModel("/DesignGyakorlas;component/Images/Icons/income.png",null,itemType.Other);
            if(manager.ShowDialog(form_AddIncome).Value == true)
            {
                if (form_AddIncome.CanAddIncome)
                {
                    incomeItemToAdd = form_AddIncome.ReturnModel;
                    
                    temporary.Add(new ItemViewModel(incomeItemToAdd.IncomeName, 1, (double)incomeItemToAdd.SimpleMoney, 0, typeWithMoneyIcon, true)
                    { WalletID =  inputData.SelectedWalletID });//I didnt made a constructor for it but its the same
                   
                    CalculateBalance();
                }
            }
                    

        }

        public void StatisticsShowButton()
        {
            StatisticsWindowViewModel form_Statistics = new StatisticsWindowViewModel(CalculateStatistics());
            WindowManager windowManager = new WindowManager();
            if(windowManager.ShowDialog(form_Statistics) == true)
            {

            }
        }

        public void RemoveItemButton()
        {
            previousItemIndex = _selectedIndex - 1;
            if (SelectedIndex != null)
            {
                _items.RemoveAt((int)SelectedIndex);
               
                if (previousItemIndex == -1 && Items.Count <= 1)
                    SelectedIndex = previousItemIndex + 1;
                else if (SelectedIndex > 1)
                    SelectedIndex = previousItemIndex;
                else if (previousItemIndex == -1 && Items.Count > 1 )
                    SelectedIndex = previousItemIndex+1;
                else
                    SelectedIndex = previousItemIndex;
            } else
                MessageBox.Show("Kerlek Jelolj ki valamit");
            
        }
        SettingsDataModel inputData = new SettingsDataModel(); //TODO eltuntentni ezt innen
        public void SettingsShowButton()
        {

            SettingsMenuViewModel SettingsWindow = new SettingsMenuViewModel();
            WindowManager manager = new WindowManager();
            if(manager.ShowDialog(SettingsWindow) == true)
            {
                if (SettingsWindow.IsWalletChanged)
                {
                  

                    ChangeItems();
                }
                    
            }
        }

        //TODO: kommentelesre szorul
        public void ExitButton()
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            var filesInFolder = Directory.GetFiles(savePath);
            foreach(string file in filesInFolder)
            {
               File.Delete(file);
            }

            StreamWriter writer;
            int itemIndex = 0;

            foreach (var item in Items)
            {
                fulltemporary.Add(item);
            }

            foreach (var item in FullItems)
            {

                writer = new StreamWriter($"{savePath}{item.ItemName}{itemIndex}.mrk");
                itemIndex++;
                string serializedItemText = JsonConvert.SerializeObject(item, Formatting.Indented);
   
                 writer.Write(serializedItemText);
               
               writer.Close();
            }

            this.TryClose();
        }


        #region Metodusok

        public void ChangeItems()
        {
            foreach(var item in Items)
            {
             //   Items.Where(i => i.WalletID != inputData.SelectedWalletID);
            }
        }

        public void LoadSettings()
        {
            string saveDir = $"{Directory.GetCurrentDirectory()}\\Settings\\settings.json";
            string rawJson;

            if (File.Exists(saveDir))
            {
                StreamReader stRead = new StreamReader(saveDir);
                rawJson = stRead.ReadToEnd();
                stRead.Dispose();
            }
            else { rawJson = null; }



            if (rawJson != null)
            {
                inputData = JsonConvert.DeserializeObject<SettingsDataModel>(rawJson);
            }
            else { inputData = null; }
        }

        //TODO: kommentelesre szorul, megcsinalni hogy csak a .mrk nevu fajlokat vegye figyelembe
        private void LoadSavedItems()
        {

            LoadSettings();

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            StreamReader dataReader;
            var filesInFolder = Directory.GetFiles(savePath,"*.mrk");

            foreach (string file in filesInFolder)
            {
                dataReader = new StreamReader(file);
                string rawJson = dataReader.ReadToEnd();

                ItemViewModel tempBeforeAdd;
                try
                {
                     tempBeforeAdd = JsonConvert.DeserializeObject<ItemViewModel>(rawJson);
                    string imageLocation = tempBeforeAdd.ItemType.IconSource.Substring(26);

                    if (!File.Exists($"{Directory.GetCurrentDirectory()}{imageLocation}"))
                        throw new FileLoadException();

                    if ((int)tempBeforeAdd.ItemType.TypeEnumID > 7)
                        throw new System.Exception();

                    if (tempBeforeAdd.ItemName != null && tempBeforeAdd.Money != 0 && tempBeforeAdd.ItemType.IconSource != string.Empty)
                    {


                        if (!tempBeforeAdd.IsSubscribtion)
                        {
                            if(inputData.SelectedWalletID == tempBeforeAdd.WalletID)
                            temporary.Add(tempBeforeAdd);
                            else
                            fulltemporary.Add(tempBeforeAdd);
                        }
                          
                       //changed
                        else
                        {
                            if (inputData.SelectedWalletID == tempBeforeAdd.WalletID)
                                temporary.Insert(subIndexCounter, tempBeforeAdd);
                            else
                            fulltemporary.Insert(subIndexCounter, tempBeforeAdd);
                            subIndexCounter++;
                        }
                            
                    }
                    else
                       throw new Exception();
                   
                    CalculateBalance();

                    //! Ez a Subscribtion rendsyer a penzhozzaadashoz
                    if (tempBeforeAdd.DateManager != null)
                    {
                        if (tempBeforeAdd.DateManager.SubscriptionLastDate.Date < DateTime.Now.Date && tempBeforeAdd.DateManager.DayAdded <= DateTime.Now.Day
                            &&( tempBeforeAdd.DateManager.SubscriptionLastDate.Month < DateTime.Now.Month || tempBeforeAdd.DateManager.SubscriptionLastDate.Year < DateTime.Now.Year) )
                        {
                            //levonni a penzbol tempBeforeAdd.money
                            CurrentMoney += tempBeforeAdd.Money * tempBeforeAdd.Count;

                            tempBeforeAdd.DateManager.MonthAdded = DateTime.Now.Month;
                            
                            
                            TimeSpan monthCountSpan = DateTime.Now.Date - tempBeforeAdd.DateManager.SubscriptionLastDate.Date;
                            int elapsedExtraMonths = (int) Math.Floor(monthCountSpan.TotalDays / 30) -1 ;

                            //levonni a penzbol meg elapsedMonths-szor
                            if(elapsedExtraMonths > 0)
                                CurrentMoney += tempBeforeAdd.Money * tempBeforeAdd.Count * elapsedExtraMonths;

                            MessageBox.Show(elapsedExtraMonths.ToString());
                            CalculateBalance();
                            tempBeforeAdd.DateManager.SubscriptionLastDate = DateTime.Now.Date;
                            MessageBox.Show("money has been added or subtracted");
                        }
                    }
                }
                catch
                {
                    IWindowManager manager1 = new WindowManager();
                    App.Current.Dispatcher.Invoke(delegate
                    {
                        manager1.ShowWindow(new CustomDialogueBoxViewModel(new DialogDataModel()
                        {
                            Title = "Warning!",
                            Message = "Bad item file detected it wont be initialized and will be deleted upon exit",
                            OkButtonText = "Ok",
                            SecondButtonNeeded = true,
                            SecondButtonText = "Retry",
                            SecondButtonCommand = new CommandHandler(() => Teszt())
                        }));
                    });
                   

                  //  MessageBox.Show("Bad item file detected it wont be initialized and will be deleted upon exit");
                }

               

                dataReader.Close();
                Thread.Sleep(200);
            }

        }

        //Oszzeszamolja az osszes penzt
        private void CalculateBalance()
        {
            double? totalBalance = 0;
            double? moneySaved = 0;
          
            foreach (ItemViewModel item in Items)
            {
                //if(item.IsIncome)
                //    totalBalance -= item.Money * (double)item.Count;
                //else
                    totalBalance += item.Money * (double)item.Count;

                //TODO: Ezt lehet hogz ki kene torolni
                if (item.Sale != null && item.Sale != 0)
                moneySaved += ((((1 + (double)(item.Sale /10)) * item.Money) - item.Money) *item.Count)/10;
                
            }
            MoneySaved = moneySaved;
            CurrentMoney = totalBalance;
        }
        //TODO: kommentelesre szorul
        private StatisticsDataModel CalculateStatistics()
        {
           
            double? totalMoneySaved = 0;
            double? totalMoneyCost = 0;
            double totalMoneyIncome = 0;
            itemType mostUsedCategory = itemType.Other; //other csak mert deklarálni kellett
            int drinkAndFoodCounter = 0;
            int healthAndFitnessCounter = 0;
            int entertainmentCounter = 0;
            int shoppingCounter = 0;
            int billCounter = 0;
            int electronicsCounter = 0;
            int otherCounter = 0;

            foreach (ItemViewModel item in Items)
            {
                if (!item.IsIncome)
                    totalMoneyCost += item.Money * (double)item.Count;
                else
                    totalMoneyIncome += (double)item.Money;

                //TODO: Ezt lehet hogz ki kene torolni
                if (item.Sale != null && item.Sale != 0)
                    totalMoneySaved += ((((1 + (double)(item.Sale / 10)) * item.Money) - item.Money) * item.Count) / 10;

                switch (item.ItemType.TypeEnumID)
                {
                    case itemType.DrinkAndFood:
                        drinkAndFoodCounter += item.Count;
                        break;
                    case itemType.HealthAndFitness:
                        healthAndFitnessCounter += item.Count;
                        break;
                    case itemType.Entertainment:
                        entertainmentCounter += item.Count;
                        break;
                    case itemType.Shopping:
                        shoppingCounter += item.Count;
                        break;
                    case itemType.Bill:
                        billCounter += item.Count;
                        break;
                    case itemType.Electronics:
                        electronicsCounter += item.Count;
                        break;
                    case itemType.Other:
                        otherCounter += item.Count;
                        break;
                }

            }
            int[] counters = new int[] { healthAndFitnessCounter, drinkAndFoodCounter, entertainmentCounter, shoppingCounter, billCounter, electronicsCounter, otherCounter };
            int largestNumIndex = 0;
            int largestNum = 0;
            for(int i = 0; i < counters.Length; i++)
            {
               
                if (largestNum <= counters[i])
                    {
                        largestNumIndex = i;
                        largestNum = counters[i];
                }
   
            }

            return new StatisticsDataModel((double)totalMoneyCost, (double)totalMoneySaved, totalMoneyIncome,(itemType)largestNumIndex);
        }

        public void SelectedItemChanged()
        {
            StringBuilder message = new StringBuilder();


            if (SelectedItem != null && SelectedItem.DateManager != null)
            {
                message.AppendLine(SelectedItem.DateManager.SubscriptionTimeSpanDate.ToShortDateString());
                //message.AppendLine(SelectedItem.DateManager.SubscriptionTimeSpan.ToString());
                //message.AppendLine(SelectedItem.DateManager.SubscriptionLastDate.ToShortDateString());
                //message.AppendLine(SelectedItem.DateManager.MonthAdded.ToString());
                message.AppendLine(SelectedItem.IsSubscribtion.ToString());
            }
            else { message.AppendLine("nothing"); }

            IWindowManager manager = new WindowManager();
            if (SelectedItem != null && SelectedItem.DateManager != null)
                manager.ShowWindow(new CustomDialogueBoxViewModel(new Models.DialogDataModel
            {
                Title = "Error",
                Message = message.ToString(),
                OkButtonText = "Ok",
                SecondButtonNeeded = false

            }));

        }


#endregion

            //TODO:megtisztitani a folosleges propertiesektol
            #region Properties

        private double? _currentMoney;

        public double? CurrentMoney
        {
            get
            {
                return _currentMoney;
            }
            set { _currentMoney = value; NotifyOfPropertyChange(() => CurrentMoney); }
        }


        private double? _moneySaved = 0;

        public double? MoneySaved
        {
            get { return _moneySaved; }
            set { _moneySaved = value; NotifyOfPropertyChange(() => MoneySaved); }
        }


        //Ehhet van hozzakapcsolva a Lista nétzet
        private BindableCollection<ItemViewModel> _items;
        
        public BindableCollection<ItemViewModel> Items
        {
            get {return _items;}
            set
            {
                _items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        private BindableCollection<ItemViewModel> _fullItems;

        public BindableCollection<ItemViewModel> FullItems
        {
            get { return _fullItems; }
            set
            {
                _fullItems = value;
                NotifyOfPropertyChange(() => FullItems);
            }
        }

        //A kivalasztott item van mindig WPF-bol idekapcsolva
        private ItemViewModel _selectedItem;

        public ItemViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value;NotifyOfPropertyChange(() => SelectedItem); SelectedItemChanged(); }
        }

        private int? _selectedIndex;

        public int? SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set { _selectedIndex = value; NotifyOfPropertyChange(() => SelectedIndex);  }
        }


        private int _windowHeight;

        public int WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; NotifyOfPropertyChange(() => WindowHeight); }
        }
      

        #endregion
    }
}
