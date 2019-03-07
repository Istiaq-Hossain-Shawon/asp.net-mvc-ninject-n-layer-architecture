using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.MODEL
{
    public partial class TEAM
    {
        public string TEAM_ID { set; get; }
        public string BRANCH_ID { set; get; }
        public string TEAM_NAME { set; get; }
        public string EMPLOYEE_ID { set; get; }
        public string EMPLOYEE_NAME { set; get; }
        public DateTime ENTRY_DATE { set; get; }
        public string ENTRY_BY { set; get; }
        public string DELETE_STATUS { set; get; }
    }
}
