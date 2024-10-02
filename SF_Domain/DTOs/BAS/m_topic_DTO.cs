using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class m_topic_DTO
    {
        public int topic_id { get; set; }
        public string topic_title { get; set; }
        public string topic_group_product { get; set; }
        public string topic_filepath { get; set; }
        public string topic_file_name { get; set; }
        public Nullable<byte> topic_status { get; set; }
    }
}
