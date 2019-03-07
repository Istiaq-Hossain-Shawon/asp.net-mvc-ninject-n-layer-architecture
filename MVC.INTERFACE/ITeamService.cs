using MVC.MODEL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.INTERFACE
{
    public interface ITeamService 
    {
        IList<TEAM> GetList(string rowFrom, string rowTo, string searchText, string user);
        TEAM Get(string ID);
        bool Save(TEAM OFFICE_NOTE);
        bool Update(TEAM OFFICE_NOTE);
        bool Delete(TEAM EMPLOYEE);
        bool IsDuplicate(TEAM EMPLOYEE);
        double GetTotalItems(string pageNumber, string pageSize, string searchText, string user);
        DataTable GetEmployeeInfoByTeam(string note_id);


    }
}
