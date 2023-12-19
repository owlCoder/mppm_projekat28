using MVVM3.Helpers;

namespace MVVM3.ViewModel
{
    public class HomeViewModel : BindableBase
    {
        private string applicationTitle;
        private string applicationSubtitle;

        public HomeViewModel()
        {
            ApplicationTitle = "GDA Client";
            ApplicationSubtitle = "Use navigation to access advanced options";
        }

        public string ApplicationTitle
        {
            get { return applicationTitle; }
            set
            {
                applicationTitle = value;
                OnPropertyChanged("ApplicationTitle");
            }
        }

        public string ApplicationSubtitle
        {
            get { return applicationSubtitle; }
            set
            {
                applicationSubtitle = value;
                OnPropertyChanged("ApplicationSubtitle");
            }
        }
    }
}
