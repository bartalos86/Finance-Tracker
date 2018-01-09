using Caliburn.Micro;
using DesignGyakorlas.Models;
using DesignGyakorlas.ViewModels.Commands;
using System.Windows;

namespace DesignGyakorlas.ViewModels
{
   public class CustomDialogueBoxViewModel : Screen
    {

        public CustomDialogueBoxViewModel(DialogDataModel inputData)
        {
            _messageText = inputData.Message;
            _okButtonText = inputData.OkButtonText;
            _titleText = inputData.Title;

            _secondButtonCommandProp = inputData.SecondButtonCommand;

            if (inputData.SecondButtonNeeded)
            {
                _secondButtonText = inputData.SecondButtonText;
                _isSecondButtonActivated = Visibility.Visible;
            }else { _isSecondButtonActivated = Visibility.Hidden; }
        }

        public void OkButton()
        {
            this.TryClose();
        }

        public void SecondButtonCommand()
        {
            if(SecondButtonCommandProp != null)
            SecondButtonCommandProp.ExecuteWithoutParameter();

            this.TryClose();
        }

        public void ExitButton()
        {
            this.TryClose();
        }

        #region Properties

        private string _titleText;

        public string TitleText
        {
            get { return _titleText; }
            set { _titleText = value;NotifyOfPropertyChange(() => TitleText); }
        }


        private string _messageText;

        public string MessageText
        {
            get { return _messageText; }
            set { _messageText = value; NotifyOfPropertyChange(() => MessageText); }
        }

        private string _okButtonText;

        public string OkButtonText
        {
            get { return _okButtonText; }
            set { _okButtonText = value;NotifyOfPropertyChange(() => OkButtonText); }
        }

        private string _secondButtonText;

        public string SecondButtonText
        {
            get { return _secondButtonText; }
            set { _secondButtonText = value; NotifyOfPropertyChange(() => SecondButtonText); }
        }

        private Visibility _isSecondButtonActivated;

        public Visibility IsSecondButtonActivated
        {
            get { return _isSecondButtonActivated; }
            set { _isSecondButtonActivated = value;NotifyOfPropertyChange(() => IsSecondButtonActivated); }
        }

        private CommandHandler _secondButtonCommandProp;

        public CommandHandler SecondButtonCommandProp
        {
            get { return _secondButtonCommandProp; }
            set { _secondButtonCommandProp = value; NotifyOfPropertyChange(() => SecondButtonCommandProp); }
        }




        #endregion

    }
}
