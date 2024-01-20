using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System.Collections.Generic;
namespace FTN.Services.NetworkModelService.DataModel.LoadModel
{
    public class DayType : IdentifiedObject
    {
        private List<long> seasonDayTypeSchedules = new List<long>();

        public DayType(long globalId) : base(globalId) { }

        public List<long> SeasonDayTypeSchedules { get => seasonDayTypeSchedules; set => seasonDayTypeSchedules = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                DayType x = (DayType)obj;
                return CompareHelper.CompareLists(x.seasonDayTypeSchedules, this.SeasonDayTypeSchedules);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool HasProperty(ModelCode t)
        {
            switch (t)
            {
                case ModelCode.DAYTYPE_SEASONDTSCHDL:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.DAYTYPE_SEASONDTSCHDL:
                    prop.SetValue(seasonDayTypeSchedules);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            base.SetProperty(property);
        }

        public override bool IsReferenced
        {
            get
            {
                return seasonDayTypeSchedules.Count > 0 || base.IsReferenced;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (seasonDayTypeSchedules != null && seasonDayTypeSchedules.Count > 0 && (refType == TypeOfReference.Target || refType == TypeOfReference.Both))
            {
                references[ModelCode.DAYTYPE_SEASONDTSCHDL] = seasonDayTypeSchedules.GetRange(0, seasonDayTypeSchedules.Count);
            }

            base.GetReferences(references, refType);
        }

        public override void AddReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.SEASONDTSCHDL_DAYTYPE:
                    seasonDayTypeSchedules.Add(globalId);
                    break;

                default:
                    base.AddReference(referenceId, globalId);
                    break;
            }
        }

        public override void RemoveReference(ModelCode referenceId, long globalId)
        {
            switch (referenceId)
            {
                case ModelCode.REGCTRL_REGSCHDL:

                    if (seasonDayTypeSchedules.Contains(globalId))
                    {
                        seasonDayTypeSchedules.Remove(globalId);
                    }
                    else
                    {
                        CommonTrace.WriteTrace(CommonTrace.TraceWarning, "Entity (GID = 0x{0:x16}) doesn't contain reference 0x{1:x16}.", this.GlobalId, globalId);
                    }

                    break;

                default:
                    base.RemoveReference(referenceId, globalId);
                    break;
            }
        }
    }
}
