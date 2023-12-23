using System;
using System.Collections.Generic;
using CIM.Model;
using FTN.Common;
using FTN.ESI.SIMES.CIM.CIMAdapter.Manager;

namespace FTN.ESI.SIMES.CIM.CIMAdapter.Importer
{
	public class Projekat28Importer
	{
		private static Projekat28Importer ptImporter = null;
		private static object singletoneLock = new object();

		private ConcreteModel concreteModel;
		private Delta delta;
		private ImportHelper importHelper;
		private TransformAndLoadReport report;


		#region Properties
		public static Projekat28Importer Instance
		{
			get
			{
				if (ptImporter == null)
				{
					lock (singletoneLock)
					{
						if (ptImporter == null)
						{
							ptImporter = new Projekat28Importer();
							ptImporter.Reset();
						}
					}
				}
				return ptImporter;
			}
		}

		public Delta NMSDelta
		{
			get 
			{
				return delta;
			}
		}
		#endregion Properties


		public void Reset()
		{
			concreteModel = null;
			delta = new Delta();
			importHelper = new ImportHelper();
			report = null;
		}

		public TransformAndLoadReport CreateNMSDelta(ConcreteModel cimConcreteModel)
		{
			LogManager.Log("Importing PowerTransformer Elements...", LogLevel.Info);
			report = new TransformAndLoadReport();
			concreteModel = cimConcreteModel;
			delta.ClearDeltaOperations();

			if ((concreteModel != null) && (concreteModel.ModelMap != null))
			{
				try
				{
					// convert into DMS elements
					ConvertModelAndPopulateDelta();
				}
				catch (Exception ex)
				{
					string message = string.Format("{0} - ERROR in data import - {1}", DateTime.Now, ex.Message);
					LogManager.Log(message);
					report.Report.AppendLine(ex.Message);
					report.Success = false;
				}
			}
			LogManager.Log("Importing PowerTransformer Elements - END.", LogLevel.Info);
			return report;
		}

		/// <summary>
		/// Method performs conversion of network elements from CIM based concrete model into DMS model.
		/// </summary>
		private void ConvertModelAndPopulateDelta()
		{
			LogManager.Log("Loading elements and creating delta...", LogLevel.Info);

            //// import all concrete model types (DMSType enum)
            ImportSeasons();
            ImportDayTypes();
            ImportRegulatingControls();
            ImportRegulationSchedules();
            ImportFuses();
            ImportDisconnectors();
            ImportRegularTimePoints();

			LogManager.Log("Loading elements and creating delta completed.", LogLevel.Info);
		}

		#region Import
		private void ImportRegularTimePoints()
		{
			SortedDictionary<string, object> cim = concreteModel.GetAllObjectsOfType("FTN.RegularTimePoint");
			if (cim != null)
			{
				foreach (KeyValuePair<string, object> cimRegularTimePointPair in cim)
				{
					FTN.RegularTimePoint cimRegularTimePoint = cimRegularTimePointPair.Value as FTN.RegularTimePoint;

					ResourceDescription rd = CreateRegularTimePointResourceDescription(cimRegularTimePoint);
					if (rd != null)
					{
						delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
						report.Report.Append("RegularTimePoint ID = ").Append(cimRegularTimePoint.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
					}
					else
					{
						report.Report.Append("RegularTimePoint ID = ").Append(cimRegularTimePoint.ID).AppendLine(" FAILED to be converted");
					}
				}
				report.Report.AppendLine();
			}
		}

		private ResourceDescription CreateRegularTimePointResourceDescription(FTN.RegularTimePoint cimRegularTimePoint)
		{
			ResourceDescription rd = null;
			if (cimRegularTimePoint != null)
			{
				long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULARTIMEPOINT, importHelper.CheckOutIndexForDMSType(DMSType.REGULARTIMEPOINT));
				rd = new ResourceDescription(gid);
				importHelper.DefineIDMapping(cimRegularTimePoint.ID, gid);

				Projekat28Converter.PopulateRegularTimePointsProperties(cimRegularTimePoint, rd, importHelper, report);
			}
			return rd;
		}

        private void ImportDayTypes()
        {
            SortedDictionary<string, object> cimDayTypes = concreteModel.GetAllObjectsOfType("FTN.DayType");
            if (cimDayTypes != null)
            {
                foreach (KeyValuePair<string, object> cimDayTypePair in cimDayTypes)
                {
                    FTN.DayType cimDayType = cimDayTypePair.Value as FTN.DayType;

                    ResourceDescription rd = CreateDayTypeResourceDescription(cimDayType);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("DayType ID = ").Append(cimDayType.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("DayType ID = ").Append(cimDayType.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateDayTypeResourceDescription(FTN.DayType cimDayType)
        {
            ResourceDescription rd = null;
            if (cimDayType != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DAYTYPE, importHelper.CheckOutIndexForDMSType(DMSType.DAYTYPE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimDayType.ID, gid);

                ////populate ResourceDescription
                Projekat28Converter.PopulateDayTypeProperties(cimDayType, rd);
            }
            return rd;
        }

        private void ImportSeasons()
        {
            SortedDictionary<string, object> cimSeasons = concreteModel.GetAllObjectsOfType("FTN.Season");
            if (cimSeasons != null)
            {
                foreach (KeyValuePair<string, object> cimSeasonPair in cimSeasons)
                {
                    FTN.Season cimSeason = cimSeasonPair.Value as FTN.Season;

                    ResourceDescription rd = CreateSeasonResourceDescription(cimSeason);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Season ID = ").Append(cimSeason.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Season ID = ").Append(cimSeason.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateSeasonResourceDescription(FTN.Season cimSeason)
        {
            ResourceDescription rd = null;
            if (cimSeason != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.SEASON, importHelper.CheckOutIndexForDMSType(DMSType.SEASON));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimSeason.ID, gid);

                ////populate ResourceDescription
                Projekat28Converter.PopulateSeasonProperties(cimSeason, rd);
            }
            return rd;
        }

        private void ImportRegulatingControls()
        {
            SortedDictionary<string, object> cimRegulatingControls = concreteModel.GetAllObjectsOfType("FTN.RegulatingControl");
            if (cimRegulatingControls != null)
            {
                foreach (KeyValuePair<string, object> cimTRegulatingControlPair in cimRegulatingControls)
                {
                    FTN.RegulatingControl cimRegulatingControl = cimTRegulatingControlPair.Value as FTN.RegulatingControl;

                    ResourceDescription rd = CreateRegulatingControlResourceDescription(cimRegulatingControl);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("RegulatingControl ID = ").Append(cimRegulatingControl.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("RegulatingControl ID = ").Append(cimRegulatingControl.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateRegulatingControlResourceDescription(FTN.RegulatingControl cimRegulatingControl)
        {
            ResourceDescription rd = null;
            if (cimRegulatingControl != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULATINGCONTROL, importHelper.CheckOutIndexForDMSType(DMSType.REGULATINGCONTROL));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimRegulatingControl.ID, gid);

                ////populate ResourceDescription
                Projekat28Converter.PopulateRegulatingControlProperties(cimRegulatingControl, rd);
            }
            return rd;

        }

        private void ImportRegulationSchedules()
        {
            SortedDictionary<string, object> cimRegulationSchedules = concreteModel.GetAllObjectsOfType("FTN.RegulationSchedule");
            if (cimRegulationSchedules != null)
            {
                foreach (KeyValuePair<string, object> RegulationSchedulePair in cimRegulationSchedules)
                {
                    FTN.RegulationSchedule cimRegulationSchedule = RegulationSchedulePair.Value as FTN.RegulationSchedule;

                    ResourceDescription rd = CreateRegulationScheduleResourceDescription(cimRegulationSchedule);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("RegulationSchedule ID = ").Append(cimRegulationSchedule.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("RegulationSchedule ID = ").Append(cimRegulationSchedule.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateRegulationScheduleResourceDescription(FTN.RegulationSchedule cimRegulationSchedule)
        {
            ResourceDescription rd = null;
            if (cimRegulationSchedule != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.REGULATIONSCHEDULE, importHelper.CheckOutIndexForDMSType(DMSType.REGULATIONSCHEDULE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimRegulationSchedule.ID, gid);

                ////populate ResourceDescription
                Projekat28Converter.PopulateRegulationScheduleProperties(cimRegulationSchedule, rd, importHelper, report);
            }
            return rd;
        }


        private void ImportFuses()
        {
            SortedDictionary<string, object> cimFuses = concreteModel.GetAllObjectsOfType("FTN.Fuse");
            if (cimFuses != null)
            {
                foreach (KeyValuePair<string, object> cimFusesPair in cimFuses)
                {
                    FTN.Fuse cimFuseValue = cimFusesPair.Value as FTN.Fuse;

                    ResourceDescription rd = CreateFuseResourceDescription(cimFuseValue);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("Fuse ID = ").Append(cimFuseValue.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("Fuse ID = ").Append(cimFuseValue.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateFuseResourceDescription(FTN.Fuse cimFuse)
        {
            ResourceDescription rd = null;
            if (cimFuse != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.FUSE, importHelper.CheckOutIndexForDMSType(DMSType.FUSE));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimFuse.ID, gid);

                ////populate ResourceDescription
                Projekat28Converter.PopulateFusesProperties(cimFuse, rd);
            }
            return rd;
        }

        private void ImportDisconnectors()
        {
            SortedDictionary<string, object> cimDisconnectors = concreteModel.GetAllObjectsOfType("FTN.Disconnector");
            if (cimDisconnectors != null)
            {
                foreach (KeyValuePair<string, object> DisconnectorsPair in cimDisconnectors)
                {
                    FTN.Disconnector disconnectroValue = DisconnectorsPair.Value as FTN.Disconnector;

                    ResourceDescription rd = CreateDisconnectorResourceDescription(disconnectroValue);
                    if (rd != null)
                    {
                        delta.AddDeltaOperation(DeltaOpType.Insert, rd, true);
                        report.Report.Append("RegulationSchedule ID = ").Append(disconnectroValue.ID).Append(" SUCCESSFULLY converted to GID = ").AppendLine(rd.Id.ToString());
                    }
                    else
                    {
                        report.Report.Append("RegulationSchedule ID = ").Append(disconnectroValue.ID).AppendLine(" FAILED to be converted");
                    }
                }
                report.Report.AppendLine();
            }
        }

        private ResourceDescription CreateDisconnectorResourceDescription(FTN.Disconnector cimDisconnector)
        {
            ResourceDescription rd = null;
            if (cimDisconnector != null)
            {
                long gid = ModelCodeHelper.CreateGlobalId(0, (short)DMSType.DISCONNECTOR, importHelper.CheckOutIndexForDMSType(DMSType.DISCONNECTOR));
                rd = new ResourceDescription(gid);
                importHelper.DefineIDMapping(cimDisconnector.ID, gid);

                ////populate ResourceDescription
                Projekat28Converter.PopulateDisconnectorsProperties(cimDisconnector, rd);
            }
            return rd;
        }

        #endregion Import
    }
}

