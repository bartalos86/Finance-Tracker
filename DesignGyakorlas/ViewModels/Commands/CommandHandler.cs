using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesignGyakorlas.ViewModels.Commands
{

    /// <summary>
    /// Ez a class torodik a sima parancsok lefuttatásával
    /// </summary>
   public class CommandHandler : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action whatToDo;

        public CommandHandler(Action inputMethod)
        {
            whatToDo = inputMethod;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            whatToDo.Invoke();
        }

        public void ExecuteWithoutParameter()
        {
            whatToDo.Invoke();
        }
    }
}
