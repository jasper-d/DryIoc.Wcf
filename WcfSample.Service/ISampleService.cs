using System.Runtime.Serialization;
using System.ServiceModel;

namespace WcfSample.Service {
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ISampleService {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        int GetHashCodeOfFoo();

        [OperationContract]
        int GetHashCodeOfBar();

        [OperationContract]
        int GetHashCodeOfSingleton();

        [OperationContract]
        int GetHashCodeOfTransient();

        [OperationContract]
        int GetHashCodeOfSelf();

        [OperationContract]
        bool BarEqualsFooBar();
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
