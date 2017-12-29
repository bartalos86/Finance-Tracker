
namespace DesignGyakorlas.Models
{
   
   public class SettingsDataModel
    {
        public WalletItemModel[] Wallets { get; set; }
        public int SelectedWalletID { get; set; }
        public int CurrencyTypeNum { get; set; }
       
    }
}
