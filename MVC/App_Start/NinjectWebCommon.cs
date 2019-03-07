using System;
using System.Web;
using System.Web.Http;
using MVC.WEB.App_Start;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using MVC.INTERFACE;
using MVC.Service;
using System.Collections.Generic;

namespace MVC.WEB.App_Start
{
    public class NinjectResolver : System.Web.Mvc.IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectResolver()
        {
            _kernel = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            this._kernel.Bind<ITeamService>().To<TeamService>(); // Registering Types    

        }
    }

} 

