using Caliburn.Micro;
using DesignGyakorlas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DesignGyakorlas.ViewModels
{
   public class StatisticsWindowViewModel : Screen
    {

        public StatisticsWindowViewModel(StatisticsDataModel inputStatisticsData)
        {
            _totalCostLabelText = $"{inputStatisticsData.TotalCost}$";
            _savedCostLabelText = $"{inputStatisticsData.TotalSaved}$";
            _totalIncomeLabelText = $"{inputStatisticsData.TotalIncome}$";
            switch (inputStatisticsData.MostUsedCategory)
            {
                case itemType.DrinkAndFood:
                    _mostUsedCategoryLabelText = "Drink And Food";
                    break;
                case itemType.HealthAndFitness:
                    _mostUsedCategoryLabelText = "Healt And Fitness";
                    break;
                case itemType.Entertainment:
                    _mostUsedCategoryLabelText = "Entertainment";
                    break;
                case itemType.Shopping:
                    _mostUsedCategoryLabelText = "Shopping";
                    break;
                case itemType.Bill:
                    _mostUsedCategoryLabelText = "Bills";
                    break;
                case itemType.Electronics:
                    _mostUsedCategoryLabelText = "Electronics";
                    break;
                case itemType.Other:
                    _mostUsedCategoryLabelText = "Other";
                    break;

            }
        }

        public void ExitButton()
        {
            this.TryClose();
        }

        //Properties
        private string _totalCostLabelText;

        public string TotalCostLabelText
        {
            get { return _totalCostLabelText; }
            set { _totalCostLabelText = value; NotifyOfPropertyChange(() => TotalCostLabelText); }
        }

        private string _savedCostLabelText;

        public string SavedCostLabelText
        {
            get { return _savedCostLabelText; }
            set { _savedCostLabelText = value; NotifyOfPropertyChange(() => SavedCostLabelText); }
        }

        private string _totalIncomeLabelText;

        public string TotalIncomeLabelText
        {
            get { return _totalIncomeLabelText; }
            set { _totalIncomeLabelText = value; NotifyOfPropertyChange(() => TotalIncomeLabelText); }
        }

        private string _mostUsedCategoryLabelText;

        public string MostUsedCategoryLabelText
        {
            get { return _mostUsedCategoryLabelText; }
            set { _mostUsedCategoryLabelText = value;NotifyOfPropertyChange(() => MostUsedCategoryLabelText); }
        }



    }
}
