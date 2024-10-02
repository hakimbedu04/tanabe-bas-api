using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SF_Domain.DTOs.BAS;
using SF_Domain.DTOs.BAS.OfflineInputs;

namespace SF_Domain.Inputs.Visit
{
    public class RealizationOfflineInputs : BaseInput
    {
        public List<XDocument> ListProductXml { get; set; }
        public List<XDocument> ListTopicXml { get; set; }
        public List<ListObject> ListObject { get; set; }
        public string Product { get; set; }
        public string Topic { get; set; }
        public string Sign { get; set; }
    }

    public class ListObject
    {
        public string VisitId { get; set; }
        public List<t_visit_product_offline> ListProduct { get; set; }
        public List<t_visit_product_topic_offline> ListTopic { get; set; }
        public List<sp_offline_t_signature_mobile_DTO> ListSign { get; set; }
    }
}
