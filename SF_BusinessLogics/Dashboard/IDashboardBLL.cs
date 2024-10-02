using SF_Domain.DTOs.BAS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_BusinessLogics.Dashboard
{
    public interface IDashboardBLL
    {
        List<TopPercentageDTO> getTopPercentage(int year, int month, string rep, string rep_pos, bool? isValidPosition = false);

        List<TopRankDTO> getTopRank(string month, string year);

        List<TopRankDTO> getTopRankByDate(string startdate, string enddate);

        List<TrendSalesDTO> getTrendSales(string stripMonth, string posRef, string idRef, bool? isValidPosition = false);

        List<AchievemtnDTO>getAchievement(string startDate, string endDate, string rep_position, string rep_id, bool? isValidPosition = false);

        List<SalesWDDTO> getSWD(DateTime today, int posDt, string repPos, string repId, bool? isValidPosition = false);

        List<CallDoctorDTO> getCallDoctor(string startDate, string endDate, string rep_position, string rep_id, bool? isValidPosition = false);
    }
}
