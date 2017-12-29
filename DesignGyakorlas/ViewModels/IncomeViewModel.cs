using Caliburn.Micro;


namespace DesignGyakorlas.ViewModels
{
    class IncomeViewModel : PropertyChangedBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="incomeName">Bevetel neve hogz mibol van</param>
        /// <param name="simpleMoney">Maga az osszeg egyszeru osszeg formajaban</param>
        public IncomeViewModel(string incomeName,double? simpleMoney)
        {
            _simpleMoney = simpleMoney;
            _incomeName = incomeName;
        }


        //Bevétel neve,hogy miből van
        private string _incomeName;

        public string IncomeName
        {
            get { return _incomeName; }
            set { _incomeName = value; NotifyOfPropertyChange(() => IncomeName); }
        }


        //Bevétel üsszege
        private double? _simpleMoney;

        public double? SimpleMoney
        {
            get { return _simpleMoney; }
            set { _simpleMoney = value; NotifyOfPropertyChange(() => SimpleMoney); }
        }


    }
}
