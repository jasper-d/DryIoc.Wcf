﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace DryIoc.Wcf {
    public class DryIocServiceBehavior : IServiceBehavior {
        private readonly IContainer _container;

        internal Type ServiceType { get; set; }

        public DryIocServiceBehavior(IContainer container) {
            if(container == null) {
                throw new ArgumentNullException(nameof(container));
            }

            _container = container;
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters) {}

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) {
            if(serviceDescription == null) {
                throw new ArgumentNullException(nameof(serviceDescription));
            }

            if(serviceHostBase == null) {
                throw new ArgumentNullException(nameof(serviceHostBase));
            }

            var instanceProvider = new DryIocInstanceProvider(_container, ServiceType ?? serviceDescription.ServiceType);
            var endpointDispatchers = GetEndpointDispatchersForImplementedContracts(serviceDescription, serviceHostBase);

            foreach (var endpointDispatcher in endpointDispatchers) {
                endpointDispatcher.DispatchRuntime.InstanceProvider = instanceProvider;
            }

        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) {}

        private IEnumerable<EndpointDispatcher> GetEndpointDispatchersForImplementedContracts(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) {
            var implementedContracts = (
                from serviceEndpoint in serviceDescription.Endpoints
                where serviceEndpoint.Contract.ContractType.IsAssignableFrom(serviceDescription.ServiceType)
                select serviceEndpoint.Contract.Name)
                .ToList();

            return
                from channelDispatcher in serviceHostBase.ChannelDispatchers.OfType<ChannelDispatcher>()
                from endpointDispatcher in channelDispatcher.Endpoints
                where implementedContracts.Contains(endpointDispatcher.ContractName)
                select endpointDispatcher;
        }
    }
}
