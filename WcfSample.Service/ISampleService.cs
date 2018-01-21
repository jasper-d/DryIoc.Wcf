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
        Guid GetHashCodeOfFoo();

        [OperationContract]
        Guid GetHashCodeOfBar();

        [OperationContract]
        Guid GetHashCodeOfSingleton();

        [OperationContract]
        Guid GetHashCodeOfTransient();

        [OperationContract]
        Guid GetHashCodeOfSelf();

        [OperationContract]
        bool BarEqualsFooBar();

        [OperationContract]
        Task<bool> ResolutionWorksForAsyncOperations();
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
