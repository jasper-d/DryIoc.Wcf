﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IntegrationTests.ServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.ISampleService")]
    public interface ISampleService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/GetData", ReplyAction="http://tempuri.org/ISampleService/GetDataResponse")]
        string GetData(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/GetData", ReplyAction="http://tempuri.org/ISampleService/GetDataResponse")]
        System.Threading.Tasks.Task<string> GetDataAsync(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/GetHashCodeOfFoo", ReplyAction="http://tempuri.org/ISampleService/GetHashCodeOfFooResponse")]
        System.Guid GetHashCodeOfFoo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/GetHashCodeOfFoo", ReplyAction="http://tempuri.org/ISampleService/GetHashCodeOfFooResponse")]
        System.Threading.Tasks.Task<System.Guid> GetHashCodeOfFooAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/GetHashCodeOfBar", ReplyAction="http://tempuri.org/ISampleService/GetHashCodeOfBarResponse")]
        System.Guid GetHashCodeOfBar();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/GetHashCodeOfBar", ReplyAction="http://tempuri.org/ISampleService/GetHashCodeOfBarResponse")]
        System.Threading.Tasks.Task<System.Guid> GetHashCodeOfBarAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/GetHashCodeOfSingleton", ReplyAction="http://tempuri.org/ISampleService/GetHashCodeOfSingletonResponse")]
        System.Guid GetHashCodeOfSingleton();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/GetHashCodeOfSingleton", ReplyAction="http://tempuri.org/ISampleService/GetHashCodeOfSingletonResponse")]
        System.Threading.Tasks.Task<System.Guid> GetHashCodeOfSingletonAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/GetHashCodeOfTransient", ReplyAction="http://tempuri.org/ISampleService/GetHashCodeOfTransientResponse")]
        System.Guid GetHashCodeOfTransient();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/GetHashCodeOfTransient", ReplyAction="http://tempuri.org/ISampleService/GetHashCodeOfTransientResponse")]
        System.Threading.Tasks.Task<System.Guid> GetHashCodeOfTransientAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/GetHashCodeOfSelf", ReplyAction="http://tempuri.org/ISampleService/GetHashCodeOfSelfResponse")]
        System.Guid GetHashCodeOfSelf();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/GetHashCodeOfSelf", ReplyAction="http://tempuri.org/ISampleService/GetHashCodeOfSelfResponse")]
        System.Threading.Tasks.Task<System.Guid> GetHashCodeOfSelfAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/BarEqualsFooBar", ReplyAction="http://tempuri.org/ISampleService/BarEqualsFooBarResponse")]
        bool BarEqualsFooBar();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/BarEqualsFooBar", ReplyAction="http://tempuri.org/ISampleService/BarEqualsFooBarResponse")]
        System.Threading.Tasks.Task<bool> BarEqualsFooBarAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/ResolutionWorksForAsyncOperations", ReplyAction="http://tempuri.org/ISampleService/ResolutionWorksForAsyncOperationsResponse")]
        bool ResolutionWorksForAsyncOperations();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ISampleService/ResolutionWorksForAsyncOperations", ReplyAction="http://tempuri.org/ISampleService/ResolutionWorksForAsyncOperationsResponse")]
        System.Threading.Tasks.Task<bool> ResolutionWorksForAsyncOperationsAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISampleServiceChannel : IntegrationTests.ServiceReference.ISampleService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SampleServiceClient : System.ServiceModel.ClientBase<IntegrationTests.ServiceReference.ISampleService>, IntegrationTests.ServiceReference.ISampleService {
        
        public SampleServiceClient() {
        }
        
        public SampleServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SampleServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SampleServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SampleServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetData(int value) {
            return base.Channel.GetData(value);
        }
        
        public System.Threading.Tasks.Task<string> GetDataAsync(int value) {
            return base.Channel.GetDataAsync(value);
        }
        
        public System.Guid GetHashCodeOfFoo() {
            return base.Channel.GetHashCodeOfFoo();
        }
        
        public System.Threading.Tasks.Task<System.Guid> GetHashCodeOfFooAsync() {
            return base.Channel.GetHashCodeOfFooAsync();
        }
        
        public System.Guid GetHashCodeOfBar() {
            return base.Channel.GetHashCodeOfBar();
        }
        
        public System.Threading.Tasks.Task<System.Guid> GetHashCodeOfBarAsync() {
            return base.Channel.GetHashCodeOfBarAsync();
        }
        
        public System.Guid GetHashCodeOfSingleton() {
            return base.Channel.GetHashCodeOfSingleton();
        }
        
        public System.Threading.Tasks.Task<System.Guid> GetHashCodeOfSingletonAsync() {
            return base.Channel.GetHashCodeOfSingletonAsync();
        }
        
        public System.Guid GetHashCodeOfTransient() {
            return base.Channel.GetHashCodeOfTransient();
        }
        
        public System.Threading.Tasks.Task<System.Guid> GetHashCodeOfTransientAsync() {
            return base.Channel.GetHashCodeOfTransientAsync();
        }
        
        public System.Guid GetHashCodeOfSelf() {
            return base.Channel.GetHashCodeOfSelf();
        }
        
        public System.Threading.Tasks.Task<System.Guid> GetHashCodeOfSelfAsync() {
            return base.Channel.GetHashCodeOfSelfAsync();
        }
        
        public bool BarEqualsFooBar() {
            return base.Channel.BarEqualsFooBar();
        }
        
        public System.Threading.Tasks.Task<bool> BarEqualsFooBarAsync() {
            return base.Channel.BarEqualsFooBarAsync();
        }
        
        public bool ResolutionWorksForAsyncOperations() {
            return base.Channel.ResolutionWorksForAsyncOperations();
        }
        
        public System.Threading.Tasks.Task<bool> ResolutionWorksForAsyncOperationsAsync() {
            return base.Channel.ResolutionWorksForAsyncOperationsAsync();
        }
    }
}
