using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesignGyakorlas.ViewModels
{
    class AddIncomeViewModel : Screen
    {

        
        //Begyujti a temp ViewModelt
        public AddIncomeViewModel(IncomeViewModel inputItem)
        {
            ReturnModel = inputItem;
        }

        //Leellenorzi hogy az a textboxokban minden jo-e, ha igen akkor beteszi a penzt
        public void SubmitIncomeButton()
        {
            ReturnModel.IncomeName = $"{IncomeNameText} (+)";
            ReturnModel.SimpleMoney = IncomeBaseMoneyText;

       

            if (string.IsNullOrWhiteSpace(IncomeNameText) || IncomeBaseMoneyText == null || IncomeBaseMoneyText <= 0)
            {
                //Ebbe lesz osszerakva a vegso uzenet hogy mi a hiba
                StringBuilder messageBuilder = new StringBuilder();

                //Itt megkeresik hol a hiba
                if (string.IsNullOrWhiteSpace(IncomeNameText))
                    messageBuilder.Append("A bevetel neve nem lehet ures! ");

                if (IncomeBaseMoneyText == null)
                    messageBuilder.Append("A bevetel osszege nem lehet ures! ");

                if (IncomeBaseMoneyText <= 0)
                    messageBuilder.Append("Nem lehet a beveteled 0 vagy kevesebb es szamnak kell lennie! ");

                IWindowManager manager = new WindowManager();
                manager.ShowWindow(new CustomDialogueBoxViewModel(new Models.DialogDataModel {
                    Title = "Error",
                    Message = messageBuilder.ToString(),
                    OkButtonText = "Ok",
                    SecondButtonNeeded = false

                }));


             //   MessageBox.Show(messageBuilder.ToString());


            }
            else
            {
                //Ha nincs semmi gaz hozzaadja a jovedelmet
                CanAddIncome = true;
                TryClose(true);
            }


        }

        public void ExitButton()
        {
            CanAddIncome = false;
            TryClose(true);
        }

        //Ezt returnolja mint jovedelem
        private IncomeViewModel _returnModel;

        public IncomeViewModel ReturnModel
        {
            get { return _returnModel; }
            set { _returnModel = value; NotifyOfPropertyChange(() => ReturnModel); }
        }

        //Ha true akkor a masik classban hozza lesz adva jovedelemkent
        private bool _canAddIncome = true;

        public bool CanAddIncome
        {
            get { return _canAddIncome; }
            set { _canAddIncome = value; NotifyOfPropertyChange(() => CanAddIncome); }
        }

        //Jovedelem neve ami hozza van bindelve a Design-hoz
        private string _incomeNameText;

        public string IncomeNameText
        {
            get { return _incomeNameText; }
            set { _incomeNameText = value; NotifyOfPropertyChange(() => IncomeNameText); }

        }

        //Ez a penz osszeg,ami szinten a design-hoz van bindelve
        private double? _incomeBaseMoneyText;

        public double? IncomeBaseMoneyText
        {
            get { return _incomeBaseMoneyText; }
            set { _incomeBaseMoneyText = value; NotifyOfPropertyChange(() => _incomeBaseMoneyText); }
        }




    }
}
