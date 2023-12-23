using FTN.Common;
using FTN.ServiceContracts;
using MVVM3.Model;
using MVVMLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVM3.Commands
{
    public class GetValuesCommands
    {
        private ModelResourcesDesc modelResourcesDesc = new ModelResourcesDesc();

        public GetValuesCommands() { }

        protected INetworkModelGDAContract Proxy
        {
            get { return ProxyConnector.Instance.GetProxy(); }
        }

        // Method to get gids per dms model code
        public ObservableCollection<long> GetGIDs(DMSType modelCode)
        {
            Messenger.Default.Send(new StatusMessage("Getting global identificators method started", "CadetBlue"));

            int iteratorId = 0;
            List<long> ids = new List<long>();

            try
            {
                int numberOfResources = 2;
                int resourcesLeft = 0;

                List<ModelCode> properties = modelResourcesDesc.GetAllPropertyIds(modelCode);

                iteratorId = Proxy.GetExtentValues(modelResourcesDesc.GetModelCodeFromType(modelCode), properties);
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

                Messenger.Default.Send(new StatusMessage("Getting extent values for " + modelCode + " method successfully finished. Fetched " + ids.Count + " samples.", "SeaGreen"));
            }
            catch (Exception e)
            {
                string message = string.Format("Getting extent values method failed for {0}. {1}", modelCode, e.Message);
                Messenger.Default.Send(new StatusMessage(message, "Crimson"));
            }

            ObservableCollection<long> gids = new ObservableCollection<long>(ids);

            return gids;
        }

        // Method to get by values
        public ObservableCollection<PropertyView> GetValues(long globalId, List<ModelCode> props)
        {
            try
            {
                ResourceDescription rd = Proxy.GetValues(globalId, props);
                ObservableCollection<PropertyView> data = new ObservableCollection<PropertyView>();

                foreach (Property p in rd.Properties)
                {
                    if (p.Type == PropertyType.ReferenceVector)
                    {
                        if (p.AsReferences().Count == 0)
                        {
                            data.Add(new PropertyView(p.Id, "Not referenced"));
                        }
                        else
                        {
                            data.Add(new PropertyView(p.Id, "Referenced to other entities"));
                        }
                    }
                    else
                    {
                        data.Add(new PropertyView(p.Id, p.ToString()));
                    }
                }

                Messenger.Default.Send(new StatusMessage("Get values method fetched " + data.Count + " " +
                                                         (data.Count <= 1 ? "property" : "properties") + " from service.", "CadetBlue"));

                return data;
            }
            catch (Exception)
            {
                Messenger.Default.Send(new StatusMessage("Service can't fetch values right now!", "Crimson"));
                return new ObservableCollection<PropertyView>();
            }
        }
    }
}
