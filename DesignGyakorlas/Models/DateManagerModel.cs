using System;


namespace DesignGyakorlas.Models
{
    public class DateManagerModel
    {
        private DateTime _subscriptionLastDate;

        public DateTime SubscriptionLastDate
        {
            get { return _subscriptionLastDate; }
            set { _subscriptionLastDate = value; }
        }

        private TimeSpan _subscriptionSpanToEnd;

        public TimeSpan SubscriptionSpanToEnd
        {
            get { return _subscriptionSpanToEnd; }
            set { _subscriptionSpanToEnd = value; }
        }

        private DateTime _subscriptionTimeSpanDate;

        public DateTime SubscriptionTimeSpanDate
        {
            get { return _subscriptionTimeSpanDate; }
            set { _subscriptionTimeSpanDate = value; }
        }

        private TimeSpan _subscriptionTimeSpan;

        public TimeSpan SubscriptionTimeSpan
        {
            get { return _subscriptionTimeSpan; }
            set { _subscriptionTimeSpan = value; }
        }

        private bool _isAddedAlready;

        public bool IsAddedAlready
        {
            get { return _isAddedAlready; }
            set { _isAddedAlready = value; }
        }

        private int _monthAdded;

        public int MonthAdded
        {
            get { return _monthAdded; }
            set { _monthAdded = value; }
        }

        private int _dayAdded;

        public int DayAdded
        {
            get { return _dayAdded; }
            set { _dayAdded = value; }
        }

    }
}
