using SF_Domain.DTOs.BAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_DAL.BAS;
using System.Data.SqlClient;

namespace SF_BusinessLogics.Dashboard
{
    public class DashboardBLL : IDashboardBLL
    {
        // author : Hakim
        // date : 2021-08-12
        // description : [Support] add role for PE at line 71, 276, 317, 347, 405 & 434


        public List<TopPercentageDTO> getTopPercentage(int year, int month, string rep, string rep_pos, bool? isValidPosition = false)
        {
            bas_trialEntities bas = new bas_trialEntities();
            bas.Database.CommandTimeout = 5000;
            List<TopPercentageDTO> tops = new List<TopPercentageDTO>();
            if(isValidPosition.Value)
            {
                TopPercentageDTO top = new TopPercentageDTO();
                top.name = "Sales";
                top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetSalesMR @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                tops.Add(top);

                top = new TopPercentageDTO();
                top.name = "Call";
                top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetCallMR @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                tops.Add(top);

                top = new TopPercentageDTO();
                top.name = "User";
                top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetUserMR @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                tops.Add(top);

                top = new TopPercentageDTO();
                top.name = "Coverage";
                top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetCoverageMR @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                tops.Add(top);
            }
            else
            {
                if (rep_pos.Equals("RM"))
                {
                    TopPercentageDTO top = new TopPercentageDTO();
                    top.name = "Sales";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetSalesRM @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                    tops.Add(top);

                    top = new TopPercentageDTO();
                    top.name = "Call";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetCallRM @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                    tops.Add(top);

                    top = new TopPercentageDTO();
                    top.name = "User";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetUserRM @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                    tops.Add(top);

                    top = new TopPercentageDTO();
                    top.name = "Coverage";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetCoverageRM @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                    tops.Add(top);
                }
                else if (rep_pos.Equals("AM"))
                {
                    TopPercentageDTO top = new TopPercentageDTO();
                    top.name = "Sales";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetSalesAM @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                    tops.Add(top);

                    top = new TopPercentageDTO();
                    top.name = "Call";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetCallAM @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                    tops.Add(top);

                    top = new TopPercentageDTO();
                    top.name = "User";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetUserAM @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                    tops.Add(top);

                    top = new TopPercentageDTO();
                    top.name = "Coverage";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetCoverageAM @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                    tops.Add(top);
                }
                else if (rep_pos.Equals("PPM"))
                {
                    TopPercentageDTO top = new TopPercentageDTO();
                    top.name = "Sales";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetSalesPPM @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                    tops.Add(top);

                    top = new TopPercentageDTO();
                    top.name = "Call";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetCallPPM @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                    tops.Add(top);

                    top = new TopPercentageDTO();
                    top.name = "User";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetUserPPM @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                    tops.Add(top);

                    top = new TopPercentageDTO();
                    top.name = "Coverage";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetCoveragePPM @mth, @yrs, @rep", new SqlParameter("mth", month), new SqlParameter("yrs", year), new SqlParameter("rep", rep)).FirstOrDefault();
                    tops.Add(top);
                }
                else
                {
                    TopPercentageDTO top = new TopPercentageDTO();
                    top.name = "Sales";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetSalesAll @mth, @yrs", new SqlParameter("mth", month), new SqlParameter("yrs", year)).FirstOrDefault();
                    tops.Add(top);

                    top = new TopPercentageDTO();
                    top.name = "Call";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetCallAll @mth, @yrs", new SqlParameter("mth", month), new SqlParameter("yrs", year)).FirstOrDefault();
                    tops.Add(top);

                    top = new TopPercentageDTO();
                    top.name = "User";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetUserAll @mth, @yrs", new SqlParameter("mth", month), new SqlParameter("yrs", year)).FirstOrDefault();
                    tops.Add(top);

                    top = new TopPercentageDTO();
                    top.name = "Coverage";
                    top.range = String.Format(new System.Globalization.CultureInfo("en-US"), "{0:MMMM}", DateTime.Now);
                    top.value = bas.Database.SqlQuery<string>("EXEC DB_SP_GetCoverageAll @mth, @yrs", new SqlParameter("mth", month), new SqlParameter("yrs", year)).FirstOrDefault();
                    tops.Add(top);
                }
            }
            

            return tops;
        }

        public List<TopRankDTO> getTopRank(string month, string year)
        {
            bas_trialEntities bas = new bas_trialEntities();
            bas.Database.CommandTimeout = 5000;
            List<TopRankDTO> tops = new List<TopRankDTO>();
            
            bas.Database.CommandTimeout = 5000;
            List<tempItemMR> result = bas.Database.SqlQuery<tempItemMR>("EXEC SP_SELECT_DB_ACHIEVMENT_MR @yrs = "+year+", @mth = "+month+"").ToList();
            foreach (tempItemMR item in result)
            {
                TopRankDTO top = new TopRankDTO();
                top.type = "mr";
                top.name = item.mr;
                top.tsv_nmr = item.tsv_nmr+"";
                top.tsv_value = item.tsv_value+"";
                tops.Add(top);
            }

            bas.Database.CommandTimeout = 5000;
            List<tempItemBO> resultBO = bas.Database.SqlQuery<tempItemBO>("EXEC SP_SELECT_DB_ACHIEVMENT_BO @yrs = " + year + ", @mth = " + month + "").ToList();
            foreach (tempItemBO item in resultBO)
            {
                TopRankDTO top = new TopRankDTO();
                top.type = "bo";
                top.name = item.tsv_bo;
                top.tsv_nmr = item.tsv_nmr + "";
                top.tsv_value = item.tsv_value + "";
                tops.Add(top);
            }

            bas.Database.CommandTimeout = 5000;
            List<tempItemREG> resultREG = bas.Database.SqlQuery<tempItemREG>("EXEC SP_SELECT_DB_ACHIEVMENT_REG @yrs = " + year + ", @mth = " + month + "").ToList();
            foreach (tempItemREG item in resultREG)
            {
                TopRankDTO top = new TopRankDTO();
                top.type = "region";
                top.name = item.tsv_region;
                top.tsv_nmr = item.tsv_nmr + "";
                top.tsv_value = item.tsv_value + "";
                tops.Add(top);
            }

            return tops;
        }

        public List<TopRankDTO> getTopRankByDate(string startdate, string enddate)
        {
            bas_trialEntities bas = new bas_trialEntities();
            bas.Database.CommandTimeout = 5000;
            List<TopRankDTO> tops = new List<TopRankDTO>();

            bas.Database.CommandTimeout = 5000;
            List<tempItemMR> result = bas.Database.SqlQuery<tempItemMR>("EXEC DB_SP_GetMRTop_ByDate @dateStart, @dateEnd", new SqlParameter("dateStart", startdate), new SqlParameter("dateEnd", enddate)).ToList();
            foreach (tempItemMR item in result.OrderByDescending(x => x.tsv_value))
            {
                TopRankDTO top = new TopRankDTO();
                top.type = "mr";
                top.name = item.mr;
                top.tsv_nmr = item.tsv_nmr + "";
                top.tsv_value = item.tsv_value + "";
                tops.Add(top);
            }

            bas.Database.CommandTimeout = 5000;
            List<tempItemBO> resultBO = bas.Database.SqlQuery<tempItemBO>("EXEC DB_SP_GetBOTop_ByDate @dateStart, @dateEnd", new SqlParameter("dateStart", startdate), new SqlParameter("dateEnd", enddate)).ToList();
            foreach (tempItemBO item in resultBO.OrderByDescending(x => x.tsv_value))
            {
                TopRankDTO top = new TopRankDTO();
                top.type = "bo";
                top.name = item.tsv_bo;
                top.tsv_nmr = item.tsv_nmr + "";
                top.tsv_value = item.tsv_value + "";
                tops.Add(top);
            }

            bas.Database.CommandTimeout = 5000;
            List<tempItemREG> resultREG = bas.Database.SqlQuery<tempItemREG>("EXEC DB_SP_GetRegTop_ByDate @dateStart, @dateEnd", new SqlParameter("dateStart", startdate), new SqlParameter("dateEnd", enddate)).ToList();
            foreach (tempItemREG item in resultREG.OrderByDescending(x => x.tsv_value))
            {
                TopRankDTO top = new TopRankDTO();
                if (!item.tsv_region.Contains("ANSD"))
                {
                    top.type = "region";
                    top.name = item.tsv_region;
                    top.tsv_nmr = item.tsv_nmr + "";
                    top.tsv_value = item.tsv_value + "";
                    tops.Add(top);    
                }
            }

            return tops;
        }

        private class tempItemMR {
            public string mr {get; set;}
            public double tsv_value {get;set;}
            public int tsv_nmr {get;set;}
        }

        private class tempItemBO
        {
            public string tsv_bo { get; set; }
            public double tsv_value { get; set; }
            public int tsv_nmr { get; set; }
        }

        private class tempItemREG
        {
            public string tsv_region { get; set; }
            public double tsv_value { get; set; }
            public int tsv_nmr { get; set; }
        }

        public List<TrendSalesDTO> getTrendSales(string stripMonth, string rep_position, string idRef, bool? isValidPosition = false)
        {
            bas_trialEntities bas = new bas_trialEntities();
            bas.Database.CommandTimeout = 5000;
            List<TrendSalesDTO> trendSaless = new List<TrendSalesDTO>();
            if (isValidPosition.Value)
            {
                trendSaless = bas.Database.SqlQuery<TrendSalesDTO>("EXEC DB_SP_GetTsvMR @range, @rep", new SqlParameter("range", stripMonth), new SqlParameter("rep", idRef)).ToList();
            }
            else
            {
                if (rep_position == "RM")
                {
                    trendSaless = bas.Database.SqlQuery<TrendSalesDTO>("EXEC DB_SP_GetTsvRM @range, @rep", new SqlParameter("range", stripMonth), new SqlParameter("rep", idRef)).ToList();
                }
                else if (rep_position == "AM")
                {
                    trendSaless = bas.Database.SqlQuery<TrendSalesDTO>("EXEC DB_SP_GetTsvAM @range, @rep", new SqlParameter("range", stripMonth), new SqlParameter("rep", idRef)).ToList();
                }
                else if (rep_position == "PPM")
                {
                    trendSaless = bas.Database.SqlQuery<TrendSalesDTO>("EXEC DB_SP_GetTsvPPM @range, @rep", new SqlParameter("range", stripMonth), new SqlParameter("rep", idRef)).ToList();
                }
                else
                {
                    trendSaless = bas.Database.SqlQuery<TrendSalesDTO>("EXEC SP_SELECT_DB_TSV_ALL @range, @rep", new SqlParameter("range", stripMonth), new SqlParameter("rep", idRef)).ToList();
                }
            }


            string[] arr = stripMonth.Split('|').ToArray();
            int counter = 0;
            int counter2 = 0;
            foreach (TrendSalesDTO item in trendSaless)
            {
                trendSaless[counter2].tsv_date = arr[counter];
                counter++;
                counter2++;

                if (counter == 12)
                    counter = 0;
            }

            return trendSaless;
        }

        public List<AchievemtnDTO> getAchievement(string startDate, string endDate, string rep_position, string rep_id, bool? isValidPosition = false)
        {
            bas_trialEntities bas = new bas_trialEntities();
            bas.Database.CommandTimeout = 5000;
            List<AchievemtnDTO> achievemtn = new List<AchievemtnDTO>();
            if (isValidPosition.Value)
            {
                achievemtn = bas.Database.SqlQuery<AchievemtnDTO>("EXEC DB_SP_GetAchievmentMRByDate @dateStart, @dateEnd, @rep", new SqlParameter("dateStart", startDate), new SqlParameter("dateEnd", endDate), new SqlParameter("rep", rep_id)).ToList();
            }
            else
            {
                if (rep_position == "RM")
                {
                    achievemtn = bas.Database.SqlQuery<AchievemtnDTO>("EXEC DB_SP_GetAchievmentRMByDate @dateStart, @dateEnd, @rep", new SqlParameter("dateStart", startDate), new SqlParameter("dateEnd", endDate), new SqlParameter("rep", rep_id)).ToList();
                }
                else if (rep_position == "AM")
                {
                    achievemtn = bas.Database.SqlQuery<AchievemtnDTO>("EXEC DB_SP_GetAchievmentAMByDate @dateStart, @dateEnd, @rep", new SqlParameter("dateStart", startDate), new SqlParameter("dateEnd", endDate), new SqlParameter("rep", rep_id)).ToList();
                }

                else if (rep_position == "PPM")
                {
                    achievemtn = bas.Database.SqlQuery<AchievemtnDTO>("EXEC DB_SP_GetAchievmentPPMByDate @dateStart, @dateEnd, @rep", new SqlParameter("dateStart", startDate), new SqlParameter("dateEnd", endDate), new SqlParameter("rep", rep_id)).ToList();
                }
                else
                {
                    achievemtn = bas.Database.SqlQuery<AchievemtnDTO>("EXEC DB_SP_GetAchievmentByDate @dateStart, @dateEnd", new SqlParameter("dateStart", startDate), new SqlParameter("dateEnd", endDate)).ToList();
                }
            }
            return achievemtn;
        }

        public List<SalesWDDTO> getSWD(DateTime today, int posDt, string rep_position, string repId, bool? isValidPosition = false)
        {
            bas_trialEntities bas = new bas_trialEntities();
            bas.Database.CommandTimeout = 5000;
            List<SalesWDDTO> salesWDs = new List<SalesWDDTO>();
            if (isValidPosition.Value)
            {
                salesWDs = bas.Database.SqlQuery<SalesWDDTO>("EXEC DB_SP_GetHeadSwdlEditMR @rep, @dt", new SqlParameter("rep", repId), new SqlParameter("dt", posDt)).ToList();
            }
            else
            {
                if (rep_position == "RM")
                {
                    salesWDs = bas.Database.SqlQuery<SalesWDDTO>("EXEC DB_SP_GetHeadSwdlEditRM @rep, @dt", new SqlParameter("rep", repId), new SqlParameter("dt", posDt)).ToList();
                }
                else if (rep_position == "AM")
                {
                    salesWDs = bas.Database.SqlQuery<SalesWDDTO>("EXEC DB_SP_GetHeadSwdlEditAM @rep, @dt", new SqlParameter("rep", repId), new SqlParameter("dt", posDt)).ToList();
                }
                else if (rep_position == "PPM")
                {
                    salesWDs = bas.Database.SqlQuery<SalesWDDTO>("EXEC DB_SP_GetHeadSwdlEditPPM @rep, @dt", new SqlParameter("rep", repId), new SqlParameter("dt", posDt)).ToList();
                }
                else
                {
                    salesWDs = bas.Database.SqlQuery<SalesWDDTO>("EXEC DB_SP_GetHeadSwdlEditAll @dt", new SqlParameter("dt", posDt)).ToList();
                }
            }

            var yesterday = today.AddMonths(0);
            yesterday = today.AddMonths(0);
            string tempThisYear = string.Format("{0:yyyy}", yesterday);
            string tempThisMonth = string.Format("{0:MMM}", yesterday);

            yesterday = today.AddMonths(-1);
            string tempLastYear = string.Format("{0:yyyy}", yesterday);
            string tempLastMonth = string.Format("{0:MMM}", yesterday);

            yesterday = today.AddMonths(-2);
            string tempAvgMonthSt = string.Format("{0:MMM}", yesterday);

            yesterday = today.AddMonths(-4);
            string tempAvgMonthEd = string.Format("{0:MMM}", yesterday);

            List<SalesWDDTO> salesWDs2 = new List<SalesWDDTO>();
            foreach (var item in salesWDs)
            {
                SalesWDDTO salesWDDTO = new SalesWDDTO();
                salesWDDTO.dateSum = 0;
                salesWDDTO.sumDateAvg = item.sumDateAvg;
                salesWDDTO.sumDateLast = item.sumDateLast;
                salesWDDTO.sumDateThis = item.sumDateThis;
                salesWDDTO.tempAvgMonthEd = tempAvgMonthEd;
                salesWDDTO.tempAvgMonthSt = tempAvgMonthSt;
                salesWDDTO.tempLastMonth = tempLastMonth;
                salesWDDTO.tempLastYear = tempLastYear;
                salesWDDTO.tempSwdAvg = item.tempSwdAvg;
                salesWDDTO.tempSwdLast = item.tempSwdLast;
                salesWDDTO.tempSwdlNow = item.tempSwdlNow;
                salesWDDTO.tempThisMonth = tempThisMonth;
                salesWDDTO.tempThisYear = tempThisYear;

                salesWDs2.Add(salesWDDTO);
            }

            List<SalesWDDTO> salesWDs3 = new List<SalesWDDTO>();
            if (isValidPosition.Value)
            {
                salesWDs3 = bas.Database.SqlQuery<SalesWDDTO>("EXEC DB_SP_GetDetailSwdlMR @rep", new SqlParameter("rep", repId)).ToList();

            }
            else
            {
                if (rep_position == "RM")
                {
                    salesWDs3 = bas.Database.SqlQuery<SalesWDDTO>("EXEC DB_SP_GetDetailSwdlRM @rep", new SqlParameter("rep", repId)).ToList();
                }
                else if (rep_position == "AM")
                {
                    salesWDs3 = bas.Database.SqlQuery<SalesWDDTO>("EXEC DB_SP_GetDetailSwdlAM @rep", new SqlParameter("rep", repId)).ToList();
                }
                else if (rep_position == "PPM")
                {
                    salesWDs3 = bas.Database.SqlQuery<SalesWDDTO>("EXEC DB_SP_GetDetailSwdlPPM @rep", new SqlParameter("rep", repId)).ToList();
                }
                else
                {
                    salesWDs3 = bas.Database.SqlQuery<SalesWDDTO>("EXEC DB_SP_GetDetailSwdlAll").ToList();
                }
            }

            return salesWDs2.Union(salesWDs3).ToList();
        }

        public List<CallDoctorDTO> getCallDoctor(string startDate, string endDate, string rep_position, string rep_id, bool? isValidPosition = false)
        {
            bas_trialEntities bas = new bas_trialEntities();
            bas.Database.CommandTimeout = 5000;
            List<CallDoctorDTO> callDoctor = new List<CallDoctorDTO>();

            if (isValidPosition.Value)
            {
                callDoctor = bas.Database.SqlQuery<CallDoctorDTO>("EXEC DB_SP_GetDoctorCallMRByDate @dateStart, @dateEnd, @nik", new SqlParameter("dateStart", startDate), new SqlParameter("dateEnd", endDate), new SqlParameter("nik", rep_id)).ToList();
            }
            else
            {
                if (rep_position == "RM")
                {
                    callDoctor = bas.Database.SqlQuery<CallDoctorDTO>("EXEC DB_SP_GetDoctorCallRMByDate @dateStart, @dateEnd, @nik", new SqlParameter("dateStart", startDate), new SqlParameter("dateEnd", endDate), new SqlParameter("nik", rep_id)).ToList();
                }
                else if (rep_position == "AM")
                {
                    callDoctor = bas.Database.SqlQuery<CallDoctorDTO>("EXEC DB_SP_GetDoctorCallAMByDate @dateStart, @dateEnd, @nik", new SqlParameter("dateStart", startDate), new SqlParameter("dateEnd", endDate), new SqlParameter("nik", rep_id)).ToList();
                }
                else if (rep_position == "PPM")
                {
                    callDoctor = bas.Database.SqlQuery<CallDoctorDTO>("EXEC DB_SP_GetDoctorCallPPMByDate @dateStart, @dateEnd, @nik", new SqlParameter("dateStart", startDate), new SqlParameter("dateEnd", endDate), new SqlParameter("nik", rep_id)).ToList();
                }
                else
                {
                    callDoctor = bas.Database.SqlQuery<CallDoctorDTO>("EXEC [DB_SP_GetDoctorCallByDate @dateStart, @dateEnd, @nik", new SqlParameter("dateStart", startDate), new SqlParameter("dateEnd", endDate), new SqlParameter("nik", rep_id)).ToList();
                }
            }

            return callDoctor;
        }
    }
}
