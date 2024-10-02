using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF_Domain.DTOs.BAS;
using SF_Domain.Inputs.User;
using SF_Domain.Inputs.Visit;

namespace SF_Repositories.VisitRepo
{
    public interface IVisitRepo
    {
        void DeleteProductTopic(VisitInputs inputs);
        IEnumerable<int> GetTopicFeedback(int vptId);
        bool UpdateFeedbackTopic(VisitInputs inputs);
        List<m_event_DTO> GetEventNameList(string budget);
        void DeleteDetailProduct(int vdid);
        bool isHaveRemainingToSendMail(UserInputs inputs);
    }
}
