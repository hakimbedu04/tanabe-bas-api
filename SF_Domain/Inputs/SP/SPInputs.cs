using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SF_Domain.Inputs.SP
{
    public class SPInputs : BaseInput
    {
        public int SpfId { get; set; }
        public int SppId { get; set; }
        public string SprId { get; set; }
        public string SpProduct { get; set; }
        public int SpId { get; set; }
        public string SpType { get; set; }
        public int DrCode { get; set; }
        public int SponsorId { get; set; }
        public double BudgetPlanValue { get; set; }
        public int SpdsId { get; set; }
        public string SpType2 { get; set; }
        public string EventBudget { get; set; }
        public TypeColl Coll { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string EName { get; set; }
        public string ETopic { get; set; }
        public string EPlace { get; set; }
        public int GeneralSponsor { get; set; }
        public double BAmount { get; set; }
        public string Participant { get; set; }
        public string Notes { get; set; }
        public XDocument ProductXmlDoc { get; set; }
        public List<TempProduct> ListProducts { get; set; }
        public string Products { get; set; }
        public int SprStatus { get; set; }
        public string SpBa { get; set; }
        public double BudgetRealValue { get; set; }
        public Codes ListDoctors { get; set; }
        public List<Collection> BatchValues { get; set; }
        public DateTime RealDateTime { get; set; }
        public int RealizationState { get; set; }
        public string Host { get; set; }
    }

    public class TypeColl
    {
        public string SpType { get; set; }
    }

    public class Codes
    {
        public string DrCode { get; set; }
    }

    public class Collection
    {
        public int SpdsId { get; set; }
        public int DrActual { get; set; }
        public int SponsorId { get; set; }
        public double BudgetRealValue { get; set; }
    }
}
