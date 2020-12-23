using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SpendTracker
{
    public class DailySpend
    {
        public string Date { get; set; }
        public int Food { get; set; }
        public int Transportation { get; set; }
        public int Other { get; set; }
        public int TotalAmount { get; set; }
        public string Remarks { get; set; }
    }
}
