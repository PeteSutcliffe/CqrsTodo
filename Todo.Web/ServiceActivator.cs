using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using StructureMap;

namespace Todo.Web
{
    public class ServiceActivator : IHttpControllerActivator
    {
        private readonly IContainer _container;

        public ServiceActivator(IContainer container)
        {
            _container = container;
        }

        public ServiceActivator(HttpConfiguration configuration, IContainer container) : this(container) { }

        public IHttpController Create(HttpRequestMessage request
            , HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller = _container
                .GetInstance(controllerType) as IHttpController;
            return controller;
        }
    }
}