using System;

namespace DataMart.Input
{
    public class OFFTIME_INFO
    {
        public string RULE_TYPE { get; set; }
        public string START_TIME { get; set; }
        public string END_TIME { get; set; }
        public string DAYS_OF_WEEK { get; set; }
        public DateTime START_DATE_TIME { get; set; }
        public DateTime END_DATE_TIME { get; set; }
    }
}
