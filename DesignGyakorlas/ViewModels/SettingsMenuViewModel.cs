using Caliburn.Micro;
using DesignGyakorlas.Models;
using DesignGyakorlas.ViewModels.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace DesignGyakorlas.ViewModels
{
    public enum CurrencyType
    {
        USD,
        EUR,
        BTC
    };

    public class SettingsMenuViewModel : Screen
    {
        BindableCollection<CurrencyTypeModel> tempCurrencies = new BindableCollection<CurrencyTypeModel>();
        BindableCollection<WalletItemModel> tempWallets = new BindableCollection<WalletItemModel>();
        SettingsDataModel  inputDataLocal = new SettingsDataModel();
        bool IsInputDataValid;
       
        public SettingsMenuViewModel(SettingsDataModel inputData)
        {

            //Base
            AddWalletCommand = new CommandHandler(() => AddWallet());

            tempCurrencies.Add(new CurrencyTypeModel(CurrencyType.EUR));
            tempCurrencies.Add(new CurrencyTypeModel(CurrencyType.USD));
            tempCurrencies.Add(new CurrencyTypeModel(CurrencyType.BTC));

        
            if (inputData != null && inputData.Wallets != null)
                IsInputDataValid = true;

            if (IsInputDataValid)
            {
                foreach (var wallet in inputData.Wallets)
                {
                    tempWallets.Add(wallet);
                }
                inputDataLocal = inputData;
              
               
                _selectedWallet = tempWallets.Single(wallet => wallet.WalletID == inputData.SelectedWalletID);
                _selectedCurrency = tempCurrencies.Single(i => i.Type == (CurrencyType)inputData.CurrencyTypeNum);
            }


            inputData = inputDataLocal;
            _currencyTypeComboBox = tempCurrencies;
            _wallets = tempWallets;

        }

        public void AddWalletButton()
        {
            IsAddingWallet = Visibility.Visible;
        }

        public void AddWallet()
        {
            if (!string.IsNullOrEmpty(WalletNameText))
            {
                tempWallets.Add(new WalletItemModel() {
                    ImageSource = "/DesignGyakorlas;component/Images/Icons/wallet.png",
                    ItemText = WalletNameText,
                    WalletID = new Random().Next(Wallets.Count, 150),
                    Money = 0
                    
                });
                Wallets = tempWallets;
                IsAddingWallet = Visibility.Hidden;
            }
        }

        public void RemoveWalletButton()
        {
            // MessageBox.Show(SelectedWalletIndex.ToString());
            if (SelectedWalletIndex != -1 && Wallets.Count != 1)
            {
                 DeletedWalletId = tempWallets[SelectedWalletIndex].WalletID;
                tempWallets.RemoveAt(SelectedWalletIndex);
              
                string savePath = $"{Directory.GetCurrentDirectory()}/Saves/";
                StreamReader dataReader;
                var filesInFolder = Directory.GetFiles(savePath, "*.mrk");
                foreach (var file in filesInFolder)
                {
                    dataReader = new StreamReader(file);
                    string rawJson = dataReader.ReadToEnd();
                    dataReader.Dispose();
                    ItemViewModel tempBeforeDel = JsonConvert.DeserializeObject<ItemViewModel>(rawJson);

                    if (tempBeforeDel.WalletID == deletedWalletId)
                        File.Delete(file);
            }
            } 
           
            Wallets = tempWallets;

           

        }

        public void OkButton()
        {
            if (SelectedWalletIndex == -1)
                SelectedWalletIndex = 0;

            string saveDir = $"{Directory.GetCurrentDirectory()}\\Settings";
            string saveItem = $"{Directory.GetCurrentDirectory()}\\Settings\\settings.json";
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            if (!File.Exists(saveItem))
                File.Create(saveItem).Dispose();

            StreamWriter saveWriter = new StreamWriter(saveItem);
            SettingsDataModel dataForSerialization = new SettingsDataModel()
            {
                Wallets = tempWallets.ToArray(),
                CurrencyTypeNum = (int)SelectedCurrency.Type,
                SelectedWalletID = SelectedWallet.WalletID,
                DeletedWalletID = DeletedWalletId
            };

            ReturnData = dataForSerialization;

            string serializedData = JsonConvert.SerializeObject(dataForSerialization);

            if (inputDataLocal.SelectedWalletID != SelectedWallet.WalletID)
                IsWalletChanged = true;
            
            saveWriter.Write(serializedData);
            saveWriter.Dispose();
            //Show Dialog
            //if (IsWalletChanged)
            //{
            //    IWindowManager manager1 = new WindowManager();
            //    manager1.ShowWindow(new CustomDialogueBoxViewModel(new DialogDataModel()
            //    {
            //        Title = "Warning!",
            //        Message = "If you add item now it will go to the old wallet, you can add new items only after resatart",
            //        OkButtonText = "I agree",
            //        SecondButtonNeeded = false,

            //    }));
            //}

            //if (IsWalletChanged)
            //    MessageBox.Show("valtozott");
            this.TryClose();
        }

        public void ExitButton()
        {
           
            this.TryClose();
        }
     

        #region Properties
        private BindableCollection<CurrencyTypeModel> _currencyTypeComboBox;

        public BindableCollection<CurrencyTypeModel> CurrencyTypeComboBox
        {
            get { return _currencyTypeComboBox; }
            set { _currencyTypeComboBox = value;NotifyOfPropertyChange(() => CurrencyTypeComboBox); }
        }

        private BindableCollection<WalletItemModel> _wallets;

        public BindableCollection<WalletItemModel> Wallets
        {
            get { return _wallets; }
            set { _wallets = value;NotifyOfPropertyChange(() => Wallets); }
        }


        private CurrencyTypeModel _selectedCurrency;

        public CurrencyTypeModel SelectedCurrency
        {
            get { return _selectedCurrency; }
            set { _selectedCurrency = value;NotifyOfPropertyChange(() => SelectedCurrency); }
        }

        private WalletItemModel _selectedWallet;

        public WalletItemModel SelectedWallet
        {
            get { return _selectedWallet; }
            set { _selectedWallet = value;NotifyOfPropertyChange(() => SelectedWallet); }
        }

        private Visibility _isAddingWallet = Visibility.Hidden;

        public Visibility IsAddingWallet
        {
            get { return _isAddingWallet; }
            set { _isAddingWallet = value;NotifyOfPropertyChange(() => IsAddingWallet); }
        }

        private string _walletNameText;

        public string WalletNameText
        {
            get { return _walletNameText; }
            set { _walletNameText = value;NotifyOfPropertyChange(() => WalletNameText); }
        }

        private int _selectedWalletIndex = 0;

        public int SelectedWalletIndex
        {
            get { return _selectedWalletIndex; }
            set { _selectedWalletIndex = value;NotifyOfPropertyChange(() => SelectedWalletIndex); }
        }

        public bool IsWalletChanged { get; set; }

        private int deletedWalletId = -1;

        public int DeletedWalletId
        {
            get { return deletedWalletId; }
            set { deletedWalletId = value; }
        }

        private SettingsDataModel _returnData;

        public SettingsDataModel ReturnData
        {
            get { return _returnData; }
            set { _returnData = value; }
        }


        public CommandHandler AddWalletCommand { get; set; }
        public CommandHandler OkButtonCommand { get; set; }
        #endregion
    }
}
