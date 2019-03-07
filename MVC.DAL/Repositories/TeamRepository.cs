using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MVC.MODEL;
using System.Data.OracleClient;

namespace MVC.DAL.Repositories
{
    public class TeamRepository : Repository<TEAM>
    {
        private DbContext _context;

        public TeamRepository(DbContext context)
            : base(context)
        {
            _context = context;
        }

        public IList<TEAM> GetList(string rowFrom, string rowTo, string searchText, string User)
        {
            using (var command = _context.CreateCommand())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"SELECT * FROM TEAM");
                command.CommandText = sb.ToString();
                return this.ToList(command).ToList();
            }
        }

        public DataTable GetEmployeeInfoByTeam(string teamid)
        {
            using (var command = _context.CreateCommand())
            {
                StringBuilder sb = new StringBuilder();


                sb.Append(@"select TEAM.TEAM_ID,TEAM_NAME,EMPLOYEE_ID from TEAM inner join TEAM_EMPLOYEE
on TEAM.TEAM_ID =TEAM_EMPLOYEE.TEAM_ID where  TEAM.TEAM_ID='" + teamid + "'");
                command.CommandText = sb.ToString();

                return this.Select(command);
            }
        }

        public double GetTotalItems(string pageNumber, string pageSize, string searchText,string User)
        {
            using (var command = _context.CreateCommand())
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@" SELECT count(*) as totalItems  FROM (
     SELECT distinct TEAM_ID,TEAM_NAME,
ENTRY_DATE,ENTRY_BY,EMPLOYEE_ID,EMPLOYEE_NAME,DELETE_STATUS,
                      row_number() over (ORDER BY TEAM_ID DESC) line_number
                    FROM TEAM 
where  NVL(DELETE_STATUS,'N')='N' and    (((lower(concat(TEAM_NAME)) like lower('%" + searchText + "%')) AND (lower('" + searchText + "') <> 'all')) OR (lower('" + searchText + "') = 'all'))) ORDER BY line_number ");
                
                command.CommandText = sb.ToString();
                DataTable aDataTable=this.Select(command);

                string value= aDataTable.Rows[0]["totalItems"].ToString();
                return Convert.ToDouble(value);
            }
        }

        public IList<TEAM> Get(string noteId)
        {
            using (var command = _context.CreateCommand())
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@"Select * from TEAM where TEAM_ID='" + noteId + "' ");

                command.CommandText = sb.ToString();

                return this.ToList(command).ToList();
            }
        }

        public bool Update(TEAM team)
        {
            StringBuilder sb = new StringBuilder();
            using (var command = _context.CreateCommand())
            {
                sb.Append(" begin  ");

                sb.Append(@"  UPDATE TEAM SET TEAM_NAME='" + team.TEAM_NAME + "'  where TEAM_ID='" + team.TEAM_ID + "' ; ");

                sb.Append(@"  delete from  TEAM_EMPLOYEE   where TEAM_ID='" + team.TEAM_ID + "' ; ");
                
                foreach (var employee in team.EMPLOYEE_LIST)
                {

                    sb.Append(@" Insert into TEAM_EMPLOYEE(TEAM_ID,EMPLOYEE_ID,ENTRY_DATE,ENTRY_BY,DELETE_STATUS) "
                        + " Values('" + team.TEAM_ID + "','" + employee.EMPLOYEE_ID + "',sysdate," + team.ENTRY_BY + ",'N') ;  ");


                }
                sb.Append(" end ;  ");

                command.CommandText = sb.ToString();
                this.ExecuteNonQuery(command);
            }
            return true;
        }
        
        public bool Save(TEAM team)
        {
            StringBuilder sb = new StringBuilder();
            using (var command = _context.CreateCommand())
            {
                sb.Append(" begin  ");

                sb.Append("Insert into TEAM(TEAM_ID,TEAM_NAME,ENTRY_DATE,ENTRY_BY,BRANCH_ID) "
                + "values('" + team.TEAM_ID + "','" + team.TEAM_NAME + "',sysdate,'" + team.ENTRY_BY + "','" + team.BRANCH_ID + "') ;");


                foreach (var employee in team.EMPLOYEE_LIST)
                {

                    sb.Append(@" Insert into TEAM_EMPLOYEE(TEAM_ID,EMPLOYEE_ID,ENTRY_DATE,ENTRY_BY,DELETE_STATUS) "
                        + " Values('" + team.TEAM_ID + "','" + employee.EMPLOYEE_ID + "',sysdate," + team.ENTRY_BY + ",'N') ;  ");


                }

                sb.Append(" end ;  ");

                command.CommandText = sb.ToString();
                this.ExecuteNonQuery(command);
            }
            
            return true;


        }

        public bool Delete(TEAM TEAM)
        {
            StringBuilder sb = new StringBuilder();

            using (var command = _context.CreateCommand())
            {
                sb = new StringBuilder();

                sb.Append("Delete from TEAM where TEAM_ID ='" + TEAM.TEAM_ID + "'");

                command.CommandText = sb.ToString();

                this.ExecuteNonQuery(command);

            }

            return true;
        }

        public bool IsDuplicate(TEAM EMPLOYEE)
        {
            using (var command = _context.CreateCommand())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select To_char(sysdate,'DD-MON-YYY') from   TEAM ");
                command.CommandText = sb.ToString();
                var data = ToList(command).ToList();
                if (data.Count == 0)
                    return false;
                else return true;
            }
        }
       
    }
}
