using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace WcfSample.Service {
    [ServiceContract]
    public interface ISampleService {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        Guid GetIdOfFoo();

        [OperationContract]
        Guid GetIdOfBar();

        [OperationContract]
        Guid GetIdOfSingleton();

        [OperationContract]
        Guid GetIdOfTransient();

        [OperationContract]
        Guid GetIdOfSelf();

        [OperationContract]
        bool BarEqualsFooBar();

        [OperationContract]
        Task<bool> ResolutionWorksForAsyncOperations();
    }
}
