using FTN.Common;
using FTN.ServiceContracts;
using MVVM3.Model;
using MVVMLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MVVM3.Commands
{
    public class GetExtentValuesCommands
    {
        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

        public GetExtentValuesCommands() { }

        protected INetworkModelGDAContract Proxy
        {
            get { return ProxyConnector.Instance.GetProxy(); }
        }



        // Method to get by all elements by model code
        public ObservableCollection<PropertiesView> GetExtentValues(DMSType dmsModelCode, List<ModelCode> props)
        {
            Messenger.Default.Send(new StatusMessage("Getting extent values method started", "CadetBlue"));
            
            int iteratorId = 0;
            List<long> ids = new List<long>();
            ObservableCollection<PropertiesView> data = new ObservableCollection<PropertiesView>();
            ModelCode modelCode = modelResourcesDesc.GetModelCodeFromType(dmsModelCode);

            try
            {
                int numberOfResources = 2;
                int resourcesLeft = 0;

                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(modelCode);

                iteratorId = Proxy.GetExtentValues(modelCode, properties);
                resourcesLeft = Proxy.IteratorResourcesLeft(iteratorId);

                while (resourcesLeft > 0)
                {
                    List<ResourceDescription> rds = Proxy.IteratorNext(numberOfResources, iteratorId);

                    for (int i = 0; i < rds.Count; i++)
                    {
                        ids.Add(rds[i].Id);
                    }

                    resourcesLeft = Proxy.IteratorResourcesLeft(iteratorId);
                }

                Proxy.IteratorClose(iteratorId);

                foreach(long gid in  ids)
                {
                    PropertiesView currentLoad = new PropertiesView() { ParentElementName = modelCode };
                    ResourceDescription rd = Proxy.GetValues(gid, props);

                    foreach (Property p in rd.Properties)
                    {
                        if (p.Type == PropertyType.ReferenceVector)
                        {
                            if (p.AsReferences().Count == 0)
                            {
                                currentLoad.Properties.Add(new PropertyView(p.Id, "Not referenced"));
                            }
                            else
                            {
                                currentLoad.Properties.Add(new PropertyView(p.Id, "Referenced to other entities"));
                            }
                        }
                        else
                        {
                            currentLoad.Properties.Add(new PropertyView(p.Id, p.ToString()));
                        }
                    }

                    // add current class into collection of loads
                    data.Add(currentLoad);
                }

                Messenger.Default.Send(new StatusMessage("Getting extent values for " + modelCode + " method successfully finished. Fetched " + ids.Count + " samples.", "SeaGreen"));

            }
            catch (Exception)
            {
                Messenger.Default.Send(new StatusMessage("Service can't fetch extent values right now!", "Crimson"));
            }

            return data;
        }
    }
}
