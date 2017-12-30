
namespace DesignGyakorlas.Models
{
   
   public class SettingsDataModel
    {
        public WalletItemModel[] Wallets { get; set; }
        public int SelectedWalletID { get; set; }
        public int CurrencyTypeNum { get; set; }
        public int DeletedWalletID { get; set; } = -1;
    }
}
