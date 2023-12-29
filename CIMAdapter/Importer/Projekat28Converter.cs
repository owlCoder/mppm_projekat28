namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
    using FTN.Common;

    public static class Projekat28Converter
    {
        #region Populate ResourceDescription
        public static void PopulateIdentifiedObjectProperties(FTN.IdentifiedObject cim, ResourceDescription rd)
        {
            if ((cim != null) && (rd != null))
            {
                if (cim.AliasNameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_ALIASNAME, cim.AliasName));
                }
                if (cim.MRIDHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_MRID, cim.MRID));
                }
                if (cim.NameHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.IDOBJ_NAME, cim.Name));
                }
            }
        }

        public static void PopulateRegularTimePointsProperties(FTN.RegularTimePoint cim, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cim != null) && (rd != null))
            {
                PopulateIdentifiedObjectProperties(cim, rd);

                if (cim.SequenceNumberHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGTIMEPNT_SEQ_NUM, cim.SequenceNumber));
                }
                if(cim.Value1HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGTIMEPNT_VALUE1, cim.Value1));
                }
                if (cim.Value2HasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGTIMEPNT_VALUE2, cim.Value2));
                }

                if (cim.IntervalScheduleHasValue)
                {
                    long gid = importHelper.GetMappedGID(cim.IntervalSchedule.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cim.GetType().ToString()).Append(" rdfID = \"").Append(cim.ID);
                        report.Report.Append("\" - Failed to set reference to RegularTimePoint: rdfID \"").Append(cim.IntervalSchedule.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.REGTIMEPNT_RISCHDL, gid));
                }
            }
        }

        public static void PopulateBasicIntervalScheduleProperties(FTN.BasicIntervalSchedule cim, ResourceDescription rd)
        {
            if ((cim != null) && (rd != null))
            {
                PopulateIdentifiedObjectProperties(cim, rd);

                if (cim.StartTimeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTSCH_STIME, cim.StartTime));
                }
                if (cim.Value1MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTSCH_VAL1_MUL, (short)GetDMSUnitMultiplier(cim.Value1Multiplier)));
                }
                if (cim.Value1UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTSCH_VAL1_UNT, (short)GetDMSUnitSymbol(cim.Value1Unit)));
                }
                if (cim.Value2MultiplierHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTSCH_VAL2_MUL, (short)GetDMSUnitMultiplier(cim.Value2Multiplier)));
                }
                if (cim.Value2UnitHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.BASICINTSCH_VAL2_UNT, (short)GetDMSUnitSymbol(cim.Value2Unit)));
                }
            }
        }

        public static void PopulateRegularIntervalScheduleProperties(FTN.RegularIntervalSchedule cim, ResourceDescription rd)
        {
            if ((cim != null) && (rd != null))
            {
                PopulateBasicIntervalScheduleProperties(cim, rd);
            }
        }

        public static void PopulateSeasonDayTypeScheduleProperties(FTN.SeasonDayTypeSchedule cim, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cim != null) && (rd != null))
            {
                PopulateRegularIntervalScheduleProperties(cim, rd);

                if (cim.SeasonHasValue)
                {
                    long gid = importHelper.GetMappedGID(cim.Season.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cim.GetType().ToString()).Append(" rdfID = \"").Append(cim.ID);
                        report.Report.Append("\" - Failed to set reference to Season: rdfID \"").Append(cim.Season.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.SEASONDTSCHDL_SEASON, gid));
                }
                if (cim.DayTypeHasValue)
                {
                    long gid = importHelper.GetMappedGID(cim.DayType.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cim.GetType().ToString()).Append(" rdfID = \"").Append(cim.ID);
                        report.Report.Append("\" - Failed to set reference to DayType: rdfID \"").Append(cim.DayType.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.SEASONDTSCHDL_DAYTYPE, gid));
                }
            }

        }

        public static void PopulateRegulationScheduleProperties(FTN.RegulationSchedule cim, ResourceDescription rd, ImportHelper importHelper, TransformAndLoadReport report)
        {
            if ((cim != null) && (rd != null))
            {
                PopulateSeasonDayTypeScheduleProperties(cim, rd, importHelper, report);

                if (cim.RegulatingControlHasValue)
                {
                    long gid = importHelper.GetMappedGID(cim.RegulatingControl.ID);
                    if (gid < 0)
                    {
                        report.Report.Append("WARNING: Convert ").Append(cim.GetType().ToString()).Append(" rdfID = \"").Append(cim.ID);
                        report.Report.Append("\" - Failed to set reference to RegulatingControl: rdfID \"").Append(cim.RegulatingControl.ID).AppendLine(" \" is not mapped to GID!");
                    }
                    rd.AddProperty(new Property(ModelCode.REGSCHDL_REGCTRL, gid));
                }
            }

        }

        public static void PopulatePowerSystemResourceProperties(FTN.PowerSystemResource cim, ResourceDescription rd)
        {
            if ((cim != null) && (rd != null))
            {
                PopulateIdentifiedObjectProperties(cim, rd);
            }
        }

        public static void PopulateRegulatingControlProperties(FTN.RegulatingControl cim, ResourceDescription rd)
        {
            if ((cim != null) && (rd != null))
            {
                PopulatePowerSystemResourceProperties(cim, rd);

                if (cim.DiscreteHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGCTRL_DISCRETE, cim.Discrete));
                }
                if (cim.ModeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGCTRL_MODE, (short)GetDMSRegulatingControlModeKind(cim.Mode)));
                }
                if (cim.MonitoredPhaseHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGCTRL_MONITPHASE, (short)GetDMSPhaseCode(cim.MonitoredPhase)));
                }
                if (cim.TargetRangeHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGCTRL_TARGRANGE, cim.TargetRange));
                }
                if (cim.TargetValueHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.REGCTRL_TARG_VALUE, cim.TargetValue));
                }
            }
        }

        public static void PopulateEquipmentProperties(FTN.Equipment cim, ResourceDescription rd)
        {
            if ((cim != null) && (rd != null))
            {
                PopulatePowerSystemResourceProperties(cim, rd);

                if (cim.AggregateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_AGGREGATE, cim.Aggregate));
                }
                if (cim.NormallyInServiceHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.EQUIPMENT_IN_SERVICE, cim.NormallyInService));
                }
            }
        }

        public static void PopulateConductingEquipmentProperties(FTN.ConductingEquipment cim, ResourceDescription rd)
        {
            if ((cim != null) && (rd != null))
            {
                PopulateEquipmentProperties(cim, rd);
            }
        }

        public static void PopulateSwitchProperties(FTN.Switch cim, ResourceDescription rd)
        {
            if ((cim != null) && (rd != null))
            {
                PopulateConductingEquipmentProperties(cim, rd);

                if (cim.NormalOpenHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_NORMAL_OPEN, cim.NormalOpen));
                }
                if (cim.RatedCurrentHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_RATCURRENT, cim.RatedCurrent));
                }
                if (cim.RetainedHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_RETAINED, cim.Retained));
                }
                if (cim.SwitchOnCountHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_S_ON_COUNT, cim.SwitchOnCount));
                }
                if (cim.SwitchOnDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SWITCH_S_ON_DATE, cim.SwitchOnDate));
                }
            }
        }

        public static void PopulateFusesProperties(FTN.Fuse cim, ResourceDescription rd)
        {
            if ((cim != null) && (rd != null))
            {
                PopulateSwitchProperties(cim, rd);
            }
        }

        public static void PopulateDisconnectorsProperties(FTN.Disconnector cim, ResourceDescription rd)
        {
            if ((cim != null) && (rd != null))
            {
                PopulateSwitchProperties(cim, rd);
            }
        }

        public static void PopulateSeasonProperties(FTN.Season cim, ResourceDescription rd)
        {
            if ((cim != null) && (rd != null))
            {
                PopulateIdentifiedObjectProperties(cim, rd);

                if (cim.StartDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SEASON_START_DATE, cim.StartDate));
                }
                if (cim.EndDateHasValue)
                {
                    rd.AddProperty(new Property(ModelCode.SEASON_END_DATE, cim.EndDate));
                }
            }
        }

        public static void PopulateDayTypeProperties(FTN.DayType cim, ResourceDescription rd)
        {
            if ((cim != null) && (rd != null))
            {
                PopulateIdentifiedObjectProperties(cim, rd);
            }
        }

        #endregion Populate ResourceDescription

        #region Enums convert
        public static PhaseCode GetDMSPhaseCode(FTN.PhaseCode phases)
        {
            switch (phases)
            {
                case FTN.PhaseCode.A:
                    return PhaseCode.A;
                case FTN.PhaseCode.AB:
                    return PhaseCode.AB;
                case FTN.PhaseCode.ABC:
                    return PhaseCode.ABC;
                case FTN.PhaseCode.ABCN:
                    return PhaseCode.ABCN;
                case FTN.PhaseCode.ABN:
                    return PhaseCode.ABN;
                case FTN.PhaseCode.AC:
                    return PhaseCode.AC;
                case FTN.PhaseCode.ACN:
                    return PhaseCode.ACN;
                case FTN.PhaseCode.AN:
                    return PhaseCode.AN;
                case FTN.PhaseCode.B:
                    return PhaseCode.B;
                case FTN.PhaseCode.BC:
                    return PhaseCode.BC;
                case FTN.PhaseCode.BCN:
                    return PhaseCode.BCN;
                case FTN.PhaseCode.BN:
                    return PhaseCode.BN;
                case FTN.PhaseCode.C:
                    return PhaseCode.C;
                case FTN.PhaseCode.CN:
                    return PhaseCode.CN;
                case FTN.PhaseCode.N:
                    return PhaseCode.N;
                case FTN.PhaseCode.s1:
                    return PhaseCode.s1;
                case FTN.PhaseCode.s12:
                    return PhaseCode.s12;
                case FTN.PhaseCode.s12N:
                    return PhaseCode.s12N;
                case FTN.PhaseCode.s1N:
                    return PhaseCode.s1N;
                case FTN.PhaseCode.s2:
                    return PhaseCode.s2;
                case FTN.PhaseCode.s2N:
                    return PhaseCode.s2N;
                default: return PhaseCode.A;
            }
        }

        public static UnitMultiplier GetDMSUnitMultiplier(FTN.UnitMultiplier unitMultiplier)
        {
            switch (unitMultiplier)
            {
                case FTN.UnitMultiplier.c:
                    return UnitMultiplier.c;
                case FTN.UnitMultiplier.d:
                    return UnitMultiplier.d;
                case FTN.UnitMultiplier.G:
                    return UnitMultiplier.G;
                case FTN.UnitMultiplier.k:
                    return UnitMultiplier.k;
                case FTN.UnitMultiplier.m:
                    return UnitMultiplier.m;
                case FTN.UnitMultiplier.M:
                    return UnitMultiplier.M;
                case FTN.UnitMultiplier.micro:
                    return UnitMultiplier.micro;
                case FTN.UnitMultiplier.n:
                    return UnitMultiplier.n;
                case FTN.UnitMultiplier.none:
                    return UnitMultiplier.none;
                case FTN.UnitMultiplier.p:
                    return UnitMultiplier.p;
                case FTN.UnitMultiplier.T:
                    return UnitMultiplier.T;
                default:
                    return UnitMultiplier.c;
            }
        }

        public static UnitSymbol GetDMSUnitSymbol(FTN.UnitSymbol unitSymbol)
        {
            switch (unitSymbol)
            {
                case FTN.UnitSymbol.A:
                    return UnitSymbol.A;
                case FTN.UnitSymbol.deg:
                    return UnitSymbol.deg;
                case FTN.UnitSymbol.degC:
                    return UnitSymbol.degC;
                case FTN.UnitSymbol.F:
                    return UnitSymbol.F;
                case FTN.UnitSymbol.g:
                    return UnitSymbol.g;
                case FTN.UnitSymbol.h:
                    return UnitSymbol.h;
                case FTN.UnitSymbol.H:
                    return UnitSymbol.H;
                case FTN.UnitSymbol.Hz:
                    return UnitSymbol.Hz;
                case FTN.UnitSymbol.J:
                    return UnitSymbol.J;
                case FTN.UnitSymbol.m:
                    return UnitSymbol.m;
                case FTN.UnitSymbol.m2:
                    return UnitSymbol.m2;
                case FTN.UnitSymbol.m3:
                    return UnitSymbol.m3;
                case FTN.UnitSymbol.min:
                    return UnitSymbol.min;
                case FTN.UnitSymbol.N:
                    return UnitSymbol.N;
                case FTN.UnitSymbol.none:
                    return UnitSymbol.none;
                case FTN.UnitSymbol.ohm:
                    return UnitSymbol.ohm;
                case FTN.UnitSymbol.Pa:
                    return UnitSymbol.Pa;
                case FTN.UnitSymbol.rad:
                    return UnitSymbol.rad;
                case FTN.UnitSymbol.s:
                    return UnitSymbol.s;
                case FTN.UnitSymbol.S:
                    return UnitSymbol.S;
                case FTN.UnitSymbol.V:
                    return UnitSymbol.V;
                case FTN.UnitSymbol.VA:
                    return UnitSymbol.VA;
                case FTN.UnitSymbol.VAh:
                    return UnitSymbol.VAh;
                case FTN.UnitSymbol.VAr:
                    return UnitSymbol.VAr;
                case FTN.UnitSymbol.VArh:
                    return UnitSymbol.VArh;
                case FTN.UnitSymbol.W:
                    return UnitSymbol.W;
                case FTN.UnitSymbol.Wh:
                    return UnitSymbol.Wh;
                default:
                    return UnitSymbol.A;
            }
        }

        public static RegulatingControlModeKind GetDMSRegulatingControlModeKind(FTN.RegulatingControlModeKind regulatingControlModeKind)
        {
            switch (regulatingControlModeKind)
            {
                case FTN.RegulatingControlModeKind.activePower:
                    return RegulatingControlModeKind.activePower;
                case FTN.RegulatingControlModeKind.admittance:
                    return RegulatingControlModeKind.admittance;
                case FTN.RegulatingControlModeKind.currentFlow:
                    return RegulatingControlModeKind.currentFlow;
                case FTN.RegulatingControlModeKind.@fixed:
                    return RegulatingControlModeKind.@fixed_;
                case FTN.RegulatingControlModeKind.powerFactor:
                    return RegulatingControlModeKind.powerFactor;
                case FTN.RegulatingControlModeKind.reactivePower:
                    return RegulatingControlModeKind.reactivePower;
                case FTN.RegulatingControlModeKind.temperature:
                    return RegulatingControlModeKind.temperature;
                case FTN.RegulatingControlModeKind.timeScheduled:
                    return RegulatingControlModeKind.timeScheduled;
                case FTN.RegulatingControlModeKind.voltage:
                    return RegulatingControlModeKind.voltage;
                default:
                    return RegulatingControlModeKind.activePower;
            }
        }
        #endregion Enums convert
    }
}