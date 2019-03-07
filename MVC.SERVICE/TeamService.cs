using System;
using System.Collections.Generic;
using System.Linq;
using MVC.DAL;
using MVC.DAL.Repositories;
using MVC.INTERFACE;
using MVC.MODEL;
using MVC.Service;


namespace MVC.Service
{
    public class TeamService : ITeamService
    {
        private IConnectionFactory connectionFactory;

        public IList<TEAM> GetList(string rowFrom, string rowTo, string searchText,string user)
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var context = new DbContext(connectionFactory);

            try
            {
               
                var rep = new TeamRepository(context);

                return rep.GetList(rowFrom, rowTo, searchText, user).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Dispose();
            }
        }

        public double GetTotalItems(string pageNumber, string pageSize, string searchText, string user)
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var context = new DbContext(connectionFactory);

            try
            {

                var rep = new TeamRepository(context);

                return rep.GetTotalItems(pageNumber, pageSize, searchText, user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Dispose();
            }
        }

        public bool Delete(TEAM user)
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var context = new DbContext(connectionFactory);

            try
            {

                var rep = new TeamRepository(context);

                return rep.Delete(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Dispose();
            }
        }

        public TEAM Get(string noteId)
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var context = new DbContext(connectionFactory);

            try
            {

                var rep = new TeamRepository(context);

                return rep.Get(noteId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Dispose();
            }
        }
        
        public bool Save(TEAM note)
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var context = new DbContext(connectionFactory);
            try
            {

                var rep = new TeamRepository(context);

                return rep.Save(note);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Dispose();
            }
            
        }

        public System.Data.DataTable GetEmployeeInfoByTeam(string note_id)
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var context = new DbContext(connectionFactory);

            try
            {

                var rep = new TeamRepository(context);

                return rep.GetEmployeeInfoByTeam(note_id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Dispose();
            }
        }
        public bool Update(TEAM note)
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var context = new DbContext(connectionFactory);
            try
            {
                var rep = new TeamRepository(context);

                return rep.Update(note);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Dispose();
            }

        }

        public bool IsDuplicate(TEAM user)
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var context = new DbContext(connectionFactory);

            try
            {

                var rep = new TeamRepository(context);

                return rep.IsDuplicate(user);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Dispose();
            }
        }
        
    }
}
