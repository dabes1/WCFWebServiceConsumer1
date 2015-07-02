using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Added usings
using System.Data;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

// WCF data objects defined
// - to correctly receive the WCF Response messsage, the following namespace needs to be 
//   correctly defined based on the namespace of the WCF service [DataContract]
namespace WCFWebServiceApplication1.DataObjects
{
    class DataObject1
    {
    }

    public class StateObject
    {
        private int _id;
        private string _abrv;
        private string _name;
        private List<string> _stateList;
        //private DataTable _dt;

        public int Id { get { return _id; } set { _id = value; } }

        public string Abrv { get { return _abrv; } set { _abrv = value; } }

        public string Name { get { return _name; } set { _name = value; } }

        public List<string> StateList { get { return _stateList; } set { _stateList = value; } }

        /*  -- DataTable not receiveing correctly from service
        [DataMember]
        public DataTable DataTable { get { return _dt; } set { _dt = value; } }
        */
    }

}
