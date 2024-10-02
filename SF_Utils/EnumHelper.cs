﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SF_Utils
{
    public class EnumHelper
    {
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        public static T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            T val;
            return Enum.TryParse<T>(str, true, out val) ? val : default(T);
        }

        public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }

            return (T)Enum.ToObject(enumType, intValue);
        }

        public string GetMonth(string param)
        {
            string tmpMth = "";
            switch (param)
            {
                case "01":
                    {
                        tmpMth = "JAN";
                        break;
                    }

                case "02":
                    {
                        tmpMth = "FEB";
                        break;
                    }

                case "03":
                    {
                        tmpMth = "MAR";
                        break;
                    }

                case "04":
                    {
                        tmpMth = "APR";
                        break;
                    }

                case "05":
                    {
                        tmpMth = "MAY";
                        break;
                    }

                case "06":
                    {
                        tmpMth = "JUN";
                        break;
                    }

                case "07":
                    {
                        tmpMth = "JUL";
                        break;
                    }

                case "08":
                    {
                        tmpMth = "AUG";
                        break;
                    }

                case "09":
                    {
                        tmpMth = "SEP";
                        break;
                    }

                case "10":
                    {
                        tmpMth = "OCT";
                        break;
                    }

                case "11":
                    {
                        tmpMth = "NOV";
                        break;
                    }

                case "12":
                    {
                        tmpMth = "DEC";
                        break;
                    }
            }
            return tmpMth;
        }

    }
}
