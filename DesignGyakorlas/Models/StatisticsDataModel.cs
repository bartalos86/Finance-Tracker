
using DesignGyakorlas.ViewModels;

namespace DesignGyakorlas.Models
{
   public class StatisticsDataModel
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="totalCost">Az osszes kolcseg osszeadva</param>
        /// <param name="totalSaved">Az osszeg amit megsporolt a felhasznalo</param>
        /// <param name="totalIncome">Az osszed bevetel</param>
        public StatisticsDataModel(double totalCost,double totalSaved,double totalIncome,itemType mostUsedCategory)
        {
            TotalCost = totalCost;
            TotalSaved = totalSaved;
            TotalIncome = totalIncome;
            MostUsedCategory = mostUsedCategory;
        }

        public double TotalCost { get; set; }
        public double TotalSaved { get; set; }
        public double TotalIncome { get; set; }
        public itemType MostUsedCategory { get; set; }
    }
}
