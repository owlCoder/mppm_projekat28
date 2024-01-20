using FTN.Common;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class ConductingEquipment : Equipment
    {
        public ConductingEquipment(long globalId) : base(globalId) { }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool HasProperty(ModelCode property)
        {
            return base.HasProperty(property);
        }

        public override void GetProperty(Property prop)
        {
            base.GetProperty(prop);
        }

        public override void SetProperty(Property property)
        {
            base.SetProperty(property);
        }
    }
}
