using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SF_Domain.DTOs.BAS
{
    public class DetailExpenseModel
    {
        private int _id;
        public int id
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

        private string _detail_description;
        public string detail_description
        {
            get
            {
                return _detail_description;
            }
            set
            {
                _detail_description = value;
            }
        }

        private string _detail_curr;
        public string detail_curr
        {
            get
            {
                return _detail_curr;
            }
            set
            {
                _detail_curr = value;
            }
        }

        private double _detail_value;
        public double detail_value
        {
            get
            {
                return _detail_value;
            }
            set
            {
                _detail_value = value;
            }
        }
    }
}
