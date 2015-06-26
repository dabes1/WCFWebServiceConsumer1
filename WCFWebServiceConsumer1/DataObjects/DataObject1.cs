using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Added usings
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace WCFWebServiceConsumer1.DataObjects
{
    class DataObject1
    {
    }

    [DataContract]
    public class StateObject
    {
        private int _id;
        private string _abrv;
        private string _name;
        private List<string> _stateList;

        [DataMember]
        public int Id { get { return _id; } set { _id = value; } }

        [DataMember]
        public string Abrv { get { return _abrv; } set { _abrv = value; } }

        [DataMember]
        public string Name { get { return _name; } set { _name = value; } }

        [DataMember]
        public List<string> StateList { get { return _stateList; } set { _stateList = value; } }
    }

}
