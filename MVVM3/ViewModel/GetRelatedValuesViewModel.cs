﻿using FTN.Common;
using MVVM3.Commands;
using MVVM3.Helpers;
using MVVM3.Model;
using MVVMLight.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;

namespace MVVM3.ViewModel
{
    public class GetRelatedValuesViewModel : BindableBase
    {
        public MyICommand ClearProperties { get; set; }
        public MyICommand ResetAll { get; set; }
        public MyICommand GetValuesCriteria { get; set; }
        private GetValuesCommands commands = new GetValuesCommands();

        private List<DMSType> types = new List<DMSType>();
        private ObservableCollection<long> gids = new ObservableCollection<long>();
        private ObservableCollection<ModelCode> properties = new ObservableCollection<ModelCode>();
        private ObservableCollection<PropertyView> listedProperties = new ObservableCollection<PropertyView>();

        private ObservableCollection<ModelCode> references = new ObservableCollection<ModelCode>();
        private ModelCode selectedReference;

        private DMSType selectedType;
        private long selectedGid = -1;

        public ObservableCollection<ModelCode> ModelList { get; set; }
        public ObservableCollection<ModelCode> SelectedModels { get; set; }

        public GetRelatedValuesViewModel()
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
            ResetAll = new MyICommand(ResetAllForm);
            // GetValuesCriteria = new MyICommand(GetValuesFromNMSCriteria);

            ResetAllForm();
        }

        private void ClearPropertiesCollection()
        {
            SelectedModels.Clear();
        }

        private void ResetAllForm()
        {
            SelectedModels.Clear();
            SelectedType = (DMSType)1;
            SelectedGid = -1;
            ListedProperties.Clear();
            References.Clear();
            References = GetReferences();
            Messenger.Default.Send(new StatusMessage("Criteria has been resetted.", "SteelBlue"));
        }

        private ObservableCollection<ModelCode> GetReferences()
        {
            ObservableCollection<ModelCode> references = new ObservableCollection<ModelCode>();
            foreach (var mc in Properties)
            {
                if ( (((long)mc & 0x0000000000000009) == 0x0000000000000009) 
                    || ((long)mc & 0x0000000000000019) == 0x0000000000000019)
                {
                    references.Add(mc);
                }
            }
            return references;
        }

        public ModelCode SelectedReferenceModelCode
        {
            get => selectedReference;
            set
            {
                if (selectedReference != value)
                {
                    selectedReference = value;
                    OnPropertyChanged("SelectedReferenceModelCode");
                }
            }
        }

        public ObservableCollection<ModelCode> References
        {
            get => references;
            set
            {
                if (value != references)
                {
                    references = value;
                    OnPropertyChanged("References");
                }
            }
        }


        public ObservableCollection<PropertyView> ListedProperties
        {
            get => listedProperties;
            set
            {
                if (value != listedProperties)
                {
                    listedProperties = value;
                    OnPropertyChanged("ListedProperties");
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
                    if (!SelectedModels.Contains(model))
                    {
                        SelectedModels.Add(model);
                    }
                }
            }
        }

        public List<DMSType> Types
        {
            get => types;
            set
            {
                if (types != value)
                {
                    types = value;
                    OnPropertyChanged("Types");
                }

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
                        SelectedGid = -1;
                        ListedProperties.Clear();
                        SelectedReferenceModelCode = 0;


                        // List all references
                        References = GetReferences();
                    }
                    catch
                    {
                        if (Gids != null)
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
                    ListedProperties.Clear();
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
                if (properties != value)
                {
                    properties = value;
                    OnPropertyChanged("Properties");
                }
            }
        }
    }
}
