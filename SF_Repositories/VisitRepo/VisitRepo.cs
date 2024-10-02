using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SF_DAL.BAS;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.User;
using SF_Domain.Inputs.Visit;

namespace SF_Repositories.VisitRepo
{
    public class VisitRepo : IVisitRepo
    {
        private bas_trialEntities _basContext;
        public void DeleteProductTopic(VisitInputs inputs)
        {
            using (var basContext = new bas_trialEntities())
            {
                var dbResult = basContext.t_visit_product_topic.Find(inputs.VptId);
                if (dbResult != null)
                {
                    basContext.Entry(dbResult).State = EntityState.Deleted;
                    basContext.SaveChanges();
                }
            }
        }

        public IEnumerable<int> GetTopicFeedback(int vptId)
        {
            using (var basContext = new bas_trialEntities())
            {
                return basContext.t_visit_product_topic.AsNoTracking().Where(x => x.vpt_id == vptId)
                        .Select(x => new
                        {
                            vpt_feedback = x.vpt_feedback.HasValue? x.vpt_feedback.Value : 0
                        })
                        .Distinct()
                        .Select(x => x.vpt_feedback)
                        .ToList();
            }
        }

        public bool UpdateFeedbackTopic(VisitInputs inputs)
        {
            using (var basContext = new bas_trialEntities())
            {
                var dbResult = basContext.t_visit_product_topic.Find(inputs.VptId);
                if (dbResult != null)
                {
                    dbResult.vpt_feedback = inputs.VptFeedBack;
                    dbResult.vpt_feedback_date = DateTime.Now;
                    dbResult.note_feedback = inputs.Info;
                    dbResult.info_feedback_id = inputs.info_feedback_id;
                    basContext.Entry(dbResult).State = EntityState.Modified;
                    basContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public List<m_event_DTO> GetEventNameList(string budget)
        {
            var dbResult =
                _basContext.m_event.Where(x => x.event_budget == budget && x.event_sp == "SP2").GroupBy(x => new m_event()
                {
                    event_description = x.event_description,
                    event_detail_description = x.event_detail_description
                })
                .Select(x => new m_event()
                {
                    event_description = x.Key.event_description,
                    event_detail_description = x.Key.event_detail_description
                }).OrderBy(x => x.event_description).ToList();
            var viewMapper = Mapper.Map<List<m_event_DTO>>(dbResult);
            return viewMapper;
        }

        public void DeleteDetailProduct(int vdid)
        {
            var dbResult = _basContext.t_visit_product.Find(vdid);
            if (dbResult != null)
            {
                _basContext.Entry(dbResult).State = EntityState.Deleted;
                _basContext.SaveChanges();
            }
        }

        public bool isHaveRemainingToSendMail(UserInputs inputs)
        {
            var transactionid = "RSVR";
            var currentDate = DateTime.Now;
            var currMonth = inputs.Month;
            var currYear = inputs.Year;
            var dateSent = Convert.ToDateTime(currentDate.ToString("yyyy-MM-dd"));
            var dbResult = _basContext.SP_SELECT_TRANSACT_EMAIL(currMonth, currYear, transactionid, inputs.RepId, dateSent).FirstOrDefault();
            return Convert.ToBoolean(dbResult.Value);
        }
    }
}
