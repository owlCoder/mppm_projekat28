using FTN.Common;
using FTN.Services.NetworkModelService.DataModel.Core;
using System;

namespace FTN.Services.NetworkModelService.DataModel.Wires
{
    public class Switch : ConductingEquipment
    {
        private bool normalOpen;
        private float ratedCurrent;
        private bool retained;
        private long switchOnCount;
        private DateTime switchOnDate;

        public Switch(long globalId) : base(globalId) { }

        public bool NormalOpen { get => normalOpen; set => normalOpen = value; }
        public float RatedCurrent { get => ratedCurrent; set => ratedCurrent = value; }
        public bool Retained { get => retained; set => retained = value; }
        public long SwitchOnCount { get => switchOnCount; set => switchOnCount = value; }
        public DateTime SwitchOnDate { get => switchOnDate; set => switchOnDate = value; }

        public override bool Equals(object obj)
        {
            return obj is Switch @switch &&
                   base.Equals(obj) &&
                   normalOpen == @switch.normalOpen &&
                   ratedCurrent == @switch.ratedCurrent &&
                   retained == @switch.retained &&
                   switchOnCount == @switch.switchOnCount &&
                   switchOnDate == @switch.switchOnDate;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.SWITCH_NORMAL_OPEN:
                case ModelCode.SWITCH_RATCURRENT:
                case ModelCode.SWITCH_RETAINED:
                case ModelCode.SWITCH_S_ON_COUNT:
                case ModelCode.SWITCH_S_ON_DATE:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.SWITCH_NORMAL_OPEN:
                    prop.SetValue(normalOpen);
                    break;

                case ModelCode.SWITCH_RATCURRENT:
                    prop.SetValue(ratedCurrent);
                    break;

                case ModelCode.SWITCH_RETAINED:
                    prop.SetValue(retained);
                    break;

                case ModelCode.SWITCH_S_ON_COUNT:
                    prop.SetValue(switchOnCount);
                    break;

                case ModelCode.SWITCH_S_ON_DATE:
                    prop.SetValue(switchOnDate);
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
                case ModelCode.SWITCH_NORMAL_OPEN:
                    normalOpen = property.AsBool();
                    break;

                case ModelCode.SWITCH_RATCURRENT:
                    ratedCurrent = property.AsFloat();
                    break;

                case ModelCode.SWITCH_RETAINED:
                    retained = property.AsBool();
                    break;

                case ModelCode.SWITCH_S_ON_COUNT:
                    switchOnCount = property.AsLong();
                    break;

                case ModelCode.SWITCH_S_ON_DATE:
                    switchOnDate = property.AsDateTime();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }
    }
}
