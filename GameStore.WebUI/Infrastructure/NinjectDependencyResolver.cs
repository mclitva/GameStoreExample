using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using Moq;
using GameStore.Domain.Entities;
using GameStore.Domain.Abstract;
using GameStore.Domain.Concrete;
using GameStore.WebUI.Infrastructure.Abstract;
using GameStore.WebUI.Infrastructure.Concrete;
using System.Configuration;

namespace GameStore.WebUI.Infrastructure
{


    public class NinjectDependencyResolver : IDependencyResolver
    {
        private Ninject.IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            //configuration of our container
            kernel.Bind<IGameRepository>().To<EFGameRepository>();

            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager
                    .AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("eSettings", emailSettings);
            kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }

    }
}