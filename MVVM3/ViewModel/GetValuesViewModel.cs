using FTN.Common;
using MVVM3.Commands;
using MVVM3.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace MVVM3.ViewModel
{
    public class GetValuesViewModel : BindableBase
    {
        public MyICommand ClearProperties {  get; set; }
        private Visibility show;
        private GetValuesCommands commands = new GetValuesCommands();

        private List<DMSType> types = new List<DMSType>();
        private ObservableCollection<long> gids = new ObservableCollection<long>();
        private ObservableCollection<ModelCode> properties = new ObservableCollection<ModelCode>();

        private DMSType selectedType;
        private long selectedGid = -1;

        public ObservableCollection<ModelCode> ModelList { get; set; }
        public ObservableCollection<ModelCode> SelectedModels { get; set; }

        public GetValuesViewModel()
        {
            // Show all dms types in combobox
            if (types != null && types.Count == 0)
            {
                Types = MainWindowViewModel.modelResourcesDesc.AllDMSTypes.ToList();
                Types.Remove(DMSType.MASK_TYPE);
            }

            ModelList = new ObservableCollection<ModelCode>();
            SelectedModels = new ObservableCollection<ModelCode>();

            ClearProperties = new MyICommand(ClearPropertiesCollection);
            Show = Visibility.Hidden;
        }

        private void ClearPropertiesCollection() 
        {
            SelectedModels.Clear();
        }

        public Visibility Show
        {
            get => show;
            set
            {
                if(show != value)
                {
                    show = value;
                    OnPropertyChanged("Show");
                }
            }
        }

        public System.Collections.IList SelectedItems
        {
            get
            {
                return SelectedModels;
            }
            set
            {
                foreach (ModelCode model in value)
                {
                    if(!SelectedModels.Contains(model))
                        SelectedModels.Add(model);
                }
            }
        }

        public List<DMSType> Types
        {
            get => types;
            set
            {
                types = value;
                OnPropertyChanged("Types");
            }
        }

        public DMSType SelectedType
        {
            get => selectedType;
            set
            {
                if (selectedType != value)
                {
                    selectedType = value;
                    OnPropertyChanged("SelectedType");

                    // Get all global ids from service via proxy
                    try
                    {
                        Gids.Clear();
                        Gids = commands.GetGIDs(selectedType);
                        Properties = new ObservableCollection<ModelCode>(
                            MainWindowViewModel.modelResourcesDesc.GetAllPropertyIds(SelectedType));
                        SelectedModels.Clear();
                        Show = Visibility.Visible;
                    }
                    catch 
                    { 
                        if(Gids != null)
                        {
                            Gids.Clear();
                            SelectedGid = -1;
                        }
                    }
                }
            }
        }

        public long SelectedGid
        {
            get => selectedGid;
            set
            {
                if (selectedGid != value)
                {
                    selectedGid = value;
                    OnPropertyChanged("SelectedGid");
                }
            }
        }

        public ObservableCollection<long> Gids
        {
            get => gids;
            private set
            {
                if (gids != value)
                {
                    gids = value;
                    OnPropertyChanged("Gids");
                }
            }
        }

        public ObservableCollection<ModelCode> Properties
        {
            get => properties;
            set
            {
                if(properties != value)
                {
                    properties = value;
                    OnPropertyChanged("Properties");
                }
            }
        }
    }
}
