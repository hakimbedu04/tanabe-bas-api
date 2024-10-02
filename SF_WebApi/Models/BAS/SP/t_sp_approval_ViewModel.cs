using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SF_WebApi.Models.BAS.SP
{
    public class t_sp_approval_ViewModel
    {
        public int spa_id { get; set; }
        public string spr_id { get; set; }
        public string spa_position { get; set; }
        public string spa_functionary { get; set; }
        public string spa_nik { get; set; }
        public Nullable<byte> spa_level { get; set; }
        public string spa_approval { get; set; }
        public Nullable<System.DateTime> spa_date_sign { get; set; }
        public string spa_comment { get; set; }
        public string spa_approval_realization { get; set; }
        public Nullable<System.DateTime> spa_date_sign_realization { get; set; }
        public string spa_comment_realization { get; set; }
        public string spa_email { get; set; }
        public string spa_action { get; set; }
        public Nullable<byte> spa_ready { get; set; }
        public Nullable<byte> spa_ready_realization { get; set; }
    }
}