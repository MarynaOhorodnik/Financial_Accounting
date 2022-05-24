using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financial_Accounting
{
    public static class Value_Total
    {
        private static double income_total = 1;
        private static double outcome_total = 1;

        public static double Income_total
        {
            get { return income_total; }
            set { income_total = value; }
        }

        public static double Outcome_total
        {
            get { return outcome_total; }
            set { outcome_total = value; }
        }
    }
}
