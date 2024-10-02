using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class t_spAttachmentModel
    {
        private Int32 _id;
        public Int32 id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private Int32 _spf_id;
        public Int32 spf_id
        {
            get
            {
                return _spf_id;
            }
            set
            {
                _spf_id = value;
            }
        }

        private string _spr_id;
        public string spr_id
        {
            get
            {
                return _spr_id;
            }
            set
            {
                _spr_id = value;
            }
        }

        private string _spf_file_name;
        public string spf_file_name
        {
            get
            {
                return _spf_file_name;
            }
            set
            {
                _spf_file_name = value;
            }
        }

        private string _spf_file_path;
        public string spf_file_path
        {
            get
            {
                return _spf_file_path;
            }
            set
            {
                _spf_file_path = value;
            }
        }
    }
}
