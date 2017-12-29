
using DesignGyakorlas.ViewModels.Commands;
using System.Windows.Input;

namespace DesignGyakorlas.Models
{
   public class DialogDataModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string OkButtonText { get; set; }
        public bool SecondButtonNeeded { get; set; }
        public string SecondButtonText { get; set; }
        public CommandHandler SecondButtonCommand { get; set; }
    }
}
