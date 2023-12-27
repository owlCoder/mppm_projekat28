using FTN.Common;
using System;
using System.Collections.Generic;

namespace FTN.Services.NetworkModelService.DataModel.Core
{
    public class BasicIntervalSchedule : IdentifiedObject
    {
        private DateTime startTime;
        private UnitMultiplier value1Multiplier;
        private UnitSymbol value1Unit;
        private UnitMultiplier value2Multiplier;
        private UnitSymbol value2Unit;

        public DateTime StartTime { get => startTime; set => startTime = value; }
        public UnitMultiplier Value1Multiplier { get => value1Multiplier; set => value1Multiplier = value; }
        public UnitSymbol Value1Unit { get => value1Unit; set => value1Unit = value; }
        public UnitMultiplier Value2Multiplier { get => value2Multiplier; set => value2Multiplier = value; }
        public UnitSymbol Value2Unit { get => value2Unit; set => value2Unit = value; }

        public BasicIntervalSchedule(long globalId) : base(globalId) { }

        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
            {
                BasicIntervalSchedule x = (BasicIntervalSchedule)obj;

                return (
                    x.startTime == this.startTime &&
                    x.value1Multiplier == this.value1Multiplier &&
                    x.value1Unit == this.value1Unit &&
                    x.value2Multiplier == this.value2Multiplier &&
                    x.value2Unit == this.value2Unit);

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

        public override bool HasProperty(ModelCode property)
        {
            switch (property)
            {
                case ModelCode.BASICINTSCH_STIME:
                case ModelCode.BASICINTSCH_VAL1_MUL:
                case ModelCode.BASICINTSCH_VAL1_UNT:
                case ModelCode.BASICINTSCH_VAL2_MUL:
                case ModelCode.BASICINTSCH_VAL2_UNT:
                    return true;

                default:
                    return base.HasProperty(property);
            }
        }

        public override void GetProperty(Property prop)
        {
            switch (prop.Id)
            {
                case ModelCode.BASICINTSCH_STIME:
                    prop.SetValue(startTime);
                    break;

                case ModelCode.BASICINTSCH_VAL1_MUL:
                    prop.SetValue((short)value1Multiplier);
                    break;

                case ModelCode.BASICINTSCH_VAL1_UNT:
                    prop.SetValue((short)value1Unit);
                    break;

                case ModelCode.BASICINTSCH_VAL2_MUL:
                    prop.SetValue((short)value2Multiplier);
                    break;

                case ModelCode.BASICINTSCH_VAL2_UNT:
                    prop.SetValue((short)value2Unit);
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
                case ModelCode.BASICINTSCH_STIME:
                    startTime = property.AsDateTime();
                    break;

                case ModelCode.BASICINTSCH_VAL1_MUL:
                    value1Multiplier = (UnitMultiplier)property.AsEnum();
                    break;

                case ModelCode.BASICINTSCH_VAL1_UNT:
                    value1Unit = (UnitSymbol)property.AsEnum();
                    break;

                case ModelCode.BASICINTSCH_VAL2_MUL:
                    value2Multiplier = (UnitMultiplier)property.AsEnum();
                    break;

                case ModelCode.BASICINTSCH_VAL2_UNT:
                    value2Unit = (UnitSymbol)property.AsEnum();
                    break;

                default:
                    base.SetProperty(property);
                    break;
            }
        }

        public override void GetReferences(Dictionary<ModelCode, List<long>> references, TypeOfReference refType)
        {
            base.GetReferences(references, refType);
        }
    }
}
