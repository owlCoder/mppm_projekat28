using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System.Collections.Generic;

namespace FTN.Services.NetworkModelService.DataModel.LoadModel
{
    public class SeasonDayTypeSchedule : RegularIntervalSchedule
    {
        private long dayType = 0;
        private long season = 0;

        public SeasonDayTypeSchedule(long globalId) : base(globalId) { }

        public long DayType { get => dayType; set => dayType = value; }
        public long Season { get => season; set => season = value; }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                SeasonDayTypeSchedule x = (SeasonDayTypeSchedule)obj;
                return (x.dayType == this.DayType && x.season == this.Season);
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
                case ModelCode.SEASONDTSCHDL_DAYTYPE:
                case ModelCode.SEASONDTSCHDL_SEASON:
                    return true;

                default:
                    return base.HasProperty(t);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.SEASONDTSCHDL_DAYTYPE:
                    prop.SetValue(dayType);
                    break;
                case ModelCode.SEASONDTSCHDL_SEASON:
                    prop.SetValue(season);
                    break;

                default:
                    base.GetProperty(prop);
                    break;
            }
        }

        public override void SetProperty(Property property)
        {
            switch (property.Id)
            {
                case ModelCode.SEASONDTSCHDL_DAYTYPE:
                    dayType = property.AsReference();
                    break;

                case ModelCode.SEASONDTSCHDL_SEASON:
                    season = property.AsReference();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            if (dayType != 0&& (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
            {
                references[ModelCode.SEASONDTSCHDL_DAYTYPE] = new List<long> { dayType };
            }

            if (season != 0 && (refType != TypeOfReference.Reference || refType != TypeOfReference.Both))
            {
                references[ModelCode.SEASONDTSCHDL_SEASON] = new List<long> { season };
            }

            base.GetReferences(references, refType);
        }
    }
}
