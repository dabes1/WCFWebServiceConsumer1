using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Added usings
using System.IO;                            // Stream, StreamReader
using System.Net;                           // WebRequest, HttpWebResponse, HttpStatusCode, WebClient
using System.Xml;                           // XmlDocument
using System.Runtime.Serialization;         // DataContractSerializer
using System.Runtime.Serialization.Json;    // DataContractJsonSerializer
//using WCFWebServiceConsumer1.DataObjects;
using WCFWebServiceApplication1.DataObjects;

// WCFWebServiceConsumer1
// This console application demonstrates a simple consumer of WCFWebServiceApplication1
// RESt formats consumed include:
//  -Simple types: string, int
//  -XML formats
//  -JSON formats
//  -Object passed for XML/JSON method formats is a complex data object "StateObject"
//   StateObject contains:
//      Id          -int
//      Abrv        -string
//      Name        -string
//      StateList   -List<string>

namespace WCFWebServiceConsumer1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WCFWebServiceConsumer1" + '\n');
            Console.WriteLine("This console app demonstrates the consumation of WCFWebServiceApplication1");
            Console.WriteLine("RESt formats consumed include:");
            Console.WriteLine(" -Simple types: string, int");
            Console.WriteLine(" -XML formats");
            Console.WriteLine(" -JSON formats");
            Console.WriteLine("Object passed for XML/JSON method formats is a complex data object \"StateObject\"");
            Console.WriteLine("StateObject contains:");
            Console.WriteLine(" Id          -int");
            Console.WriteLine(" Abrv        -string");
            Console.WriteLine(" Name        -string");
            Console.WriteLine(" StateList   -List<string>");

            Console.WriteLine("Several test processes are used:");
            Console.WriteLine("1.Simple standard process");
            Console.WriteLine("2.Simple more streamlined process");
            Console.WriteLine("3.XML Format response and parsing - simple examples");
            Console.WriteLine("4.JSON(JavaScript Object Notation) Format response/parsing");
            Console.WriteLine(" -simple example");
            Console.WriteLine(" -DataContractJsonSerializer correctly creates the client's StateObject");
                
            Console.WriteLine("");
            Console.WriteLine("");


            #region - The original simple method to consume, works for simple types (string, int, etc)
            Console.WriteLine("*****************************************************************************-");
            Console.WriteLine("1.Simple standard process");
            Console.WriteLine("This section uses:");
            Console.WriteLine("-WebRequest");
            Console.WriteLine("-HttpWebResponse");
            Console.WriteLine("-Stream");
            Console.WriteLine("-StreamReader");
            Console.WriteLine("Consumed Object: StateObject");
            Console.WriteLine("Message formats: not applied.");
            Console.WriteLine("RESt call to: http://localhost:57531/Service1.svc/State/10");
            Console.WriteLine("The returned StateObject:");

            WebRequest req = WebRequest.Create(@"http://localhost:57531/Service1.svc/State/10");
            req.Method = "GET";
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                using (Stream respStream = resp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream, Encoding.UTF8);
                    Console.WriteLine(reader.ReadToEnd() + '\n');
                }                
            }
            Console.WriteLine("");
            Console.WriteLine("");
            #endregion


            /*
            #region - Simple steps to consume the RESt StateObject from WCFWebServiceApplication1
            Console.WriteLine("*****************************************************************************-");
            Console.WriteLine("2.Simple more streamlined process");
            Console.WriteLine("This section uses:");
            Console.WriteLine("-WebRequest");
            Console.WriteLine("-WebResponse");
            Console.WriteLine("-StreamReader");
            Console.WriteLine("Consumed Object: StateObject");
            Console.WriteLine("Message formatting not applied.");
            Console.WriteLine("RESt call to: http://localhost:57531/Service1.svc/State/10");
            Console.WriteLine("The returned StateObject:");

            WebRequest req3 = WebRequest.Create(@"http://localhost:57531/Service1.svc/State/10");
            WebResponse resp3 = req3.GetResponse();
            StreamReader resp3Stream = new StreamReader(resp3.GetResponseStream());
            string resp3Str = resp3Stream.ReadToEnd();
            resp3Stream.Close();
            Console.WriteLine(resp3Str + '\n');
            Console.WriteLine("");
            Console.WriteLine("");
            #endregion
            */

            /*
            #region - The original simple method to consume, works for simple types, example from: http://stackoverflow.com/questions/14058992/wcf-rest-service-consumption-in-c-sharp-console-application
            Console.WriteLine("*****************************************************************************-");
            Console.WriteLine("3.XML Format response and parsing - simple examples");
            Console.WriteLine("This section uses:");
            Console.WriteLine("-WebRequest (ContentType = @\"application / xml; charset = utf - 8\",");
            Console.WriteLine("-HttpWebResponse");
            Console.WriteLine("-XmlDocument (GetElementsByTagName)");
            Console.WriteLine("-XmlReader");
            Console.WriteLine("-XmlNodeList");
            Console.WriteLine("Message formats: WebMessageFormat.XML");
            Console.WriteLine("RESt call to: http://localhost:57531/Service1.svc/StateXML/15");
            Console.WriteLine("The returned StateObject:");

            WebRequest reqXML = WebRequest.Create(@"http://localhost:57531/Service1.svc/StateXML/15");
            reqXML.Method = "GET";
            reqXML.ContentType = @"application/xml; charset=utf-8";
            HttpWebResponse respXML = reqXML.GetResponse() as HttpWebResponse;

            if (respXML.StatusCode == HttpStatusCode.OK)
            {
                // Well this works, but it only displays InnerText
                Console.WriteLine("Using XmlDocument.InnerText:");
                XmlDocument xmlDoc = new XmlDocument();
                XmlReader xmlRdr = new XmlTextReader(respXML.GetResponseStream());
                xmlDoc.Load(xmlRdr);
                Console.WriteLine(xmlDoc.InnerText + '\n');


                // Using GetElementsByTagName allows you to specifically target a specific StateObject property name
                Console.WriteLine("Using XmlDocument.GetElementsByTagName:");
                XmlDocument xmlDoc2 = new XmlDocument();
                XmlReader xmlRdr2 = new XmlTextReader(respXML.GetResponseStream());
                xmlDoc2.LoadXml(xmlDoc.InnerXml);

                Console.WriteLine("Id:");
                XmlNodeList list = xmlDoc2.GetElementsByTagName("Id");
                foreach (XmlNode node in list)
                {
                    Console.WriteLine(node.InnerText);
                }

                Console.WriteLine("Abrv:");
                list = xmlDoc2.GetElementsByTagName("Abrv");
                foreach (XmlNode node in list)
                {
                    Console.WriteLine(node.InnerText);
                }

                Console.WriteLine("Name:");
                list = xmlDoc2.GetElementsByTagName("Name");
                foreach (XmlNode node in list)
                {
                    Console.WriteLine(node.InnerText);
                }

                Console.WriteLine("State List:");  
                list = xmlDoc2.GetElementsByTagName("a:string");//Check against 'a:string' because StateList is List<string>
                foreach (XmlNode node in list)
                {
                    Console.WriteLine(" -" + node.InnerText);
                }
            }

            StateObject stObj;
            var url = new Uri("http://localhost:57531/Service1.svc/StateXML/15");
            var reqXmlHttp = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            var respXmlHttp = (HttpWebResponse)reqXmlHttp.GetResponse();
            var dataContractSerializer = new DataContractSerializer(typeof(StateObject));
            using (var respStr = respXmlHttp.GetResponseStream())
            {
                stObj = (StateObject)dataContractSerializer.ReadObject(respStr);
            }

            Console.WriteLine("");
            Console.WriteLine("");
            #endregion
            */

            /*
            #region - Example to consume a Data object, example from: http://stackoverflow.com/questions/14058992/wcf-rest-service-consumption-in-c-sharp-console-application
            Console.WriteLine("*****************************************************************************-");
            Console.WriteLine("4.JSON(JavaScript Object Notation) Format response/parsing - simple examples");
            Console.WriteLine("  This has multiple sections:");
            Console.WriteLine("  Section A: simple example");
            Console.WriteLine("  Section B: DataContractJsonSerializer  example");
            Console.WriteLine("");


            Console.WriteLine("Section A uses:");
            Console.WriteLine(" -WebRequest (ContentType = @\"application/json; charset=utf-8\"");
            Console.WriteLine(" -HttpWebResponse");
            Console.WriteLine(" -StreamReader");
            Console.WriteLine(" Message formats: WebMessageFormat.JSON");
            Console.WriteLine(" RESt call to: http://localhost:57531/Service1.svc/StateJSON/20");
            Console.WriteLine(" The returned StateObject:");

            WebRequest reqJSON = WebRequest.Create(@"http://localhost:57531/Service1.svc/StateJSON/20");
            reqJSON.Method = "GET";
            reqJSON.ContentType = @"application/json; charset=utf-8";
            HttpWebResponse respJSON = reqJSON.GetResponse() as HttpWebResponse;
            string strJSON = string.Empty;
            using (StreamReader sr = new StreamReader(respJSON.GetResponseStream()))
            {
                Console.WriteLine("Using StreamReader to read the JSON response:");
                strJSON = sr.ReadToEnd();
                Console.WriteLine(strJSON);
                Console.WriteLine("");
            }

            Console.WriteLine("Section B uses:");
            Console.WriteLine(" -HttpWebRequest");
            Console.WriteLine(" -HttpWebResponse");
            Console.WriteLine(" -DataContractJsonSerializer");
            Console.WriteLine(" Message formats: WebMessageFormat.JSON");
            Console.WriteLine(" RESt call to: http://localhost:57531/Service1.svc/StateJSON/5");
            Console.WriteLine(" The returned StateObject:");
            HttpWebRequest reqJSON_Http = WebRequest.Create("http://localhost:57531/Service1.svc/StateJSON/5") as HttpWebRequest;
            using (HttpWebResponse respJSON_Http = reqJSON_Http.GetResponse() as HttpWebResponse)
            {
                if (respJSON_Http.StatusCode != HttpStatusCode.OK)
                {
                    // error
                }
                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(StateObject));                
                object objResp = jsonSerializer.ReadObject(respJSON_Http.GetResponseStream());
                StateObject StObj = objResp as StateObject;

                // DataTable not being correctly received from Service
                //System.Data.DataTable dt = StObj.DataTable;
            }
            #endregion
            */


            ///*
            #region - POST Methods
            
            // Restful service URL, example from: "http://www.c-sharpcorner.com/UploadFile/surya_bg2000/developing-wcf-restful-services-with-get-and-post-methods/"
            string url = "http://localhost:57531/Service1.svc/PostSampleMethod/New";
           
            ASCIIEncoding encoding = new ASCIIEncoding();                       // declare ascii encoding
            string strResult = string.Empty;            
            string SampleXml = @"<parent>" +                                    // sample xml sent to Service & this data is sent in POST
                   "<child>" +
                      "<username>username</username>" +
                        "<password>password</password>" +
                       "</child>" +
                    "</parent>";

            string postData = SampleXml.ToString();            
            byte[] data = encoding.GetBytes(postData);                          // convert xmlstring to byte using ascii encoding            
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url); // declare httpwebrequet wrt url defined above\            
            webrequest.Method = "POST";// set method as post            
            webrequest.ContentType = "application/x-www-form-urlencoded";       // set content type\            
            webrequest.ContentLength = data.Length;                             // set content length
            
            Stream newStream = webrequest.GetRequestStream();                   // get stream data out of webrequest object
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            
            HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();// declare & read response from service
            Encoding enc = Encoding.UTF8;                                       // set utf8 encoding
            StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);// read response stream from response object            
            strResult = loResponseStream.ReadToEnd();                           // read string from stream data            
            loResponseStream.Close();                                           // close the stream object
            webresponse.Close();                                                // close the response object
            strResult = strResult.Replace("</string>", "");                     // below steps remove unwanted data from response string
            


            /*  -- this did not work
            string urlPost = "http://localhost:57531/Service1.svc/ObjLoad";
            ASCIIEncoding encoding = new ASCIIEncoding();
            string strResult = string.Empty;

            LoadObjects load = new LoadObjects();
            load.Id = 1;
            load.Value1 = 250;
            load.Value2 = 25;
            load.Desc1 = "Load Obj Desc 1 - Tester";
            load.Desc2 = "Load Obj Desc 2 - Tester";

            byte[] data = encoding.GetBytes(load.ToString());

            HttpWebRequest reqPost = WebRequest.Create(urlPost) as HttpWebRequest;
            reqPost.Method = "POST";
            reqPost.ContentType = "application/x-www-form-urlencoded";
            reqPost.ContentLength = data.Length;

            Stream newStream = reqPost.GetRequestStream();
            newStream.Write(data, 0, 1);
            newStream.Close();

            HttpWebResponse rspPost = reqPost.GetResponse() as HttpWebResponse;
            Encoding enc = System.Text.Encoding.GetEncoding("utf-8?");
            StreamReader IoResponseStream = new StreamReader(rspPost.GetResponseStream(), enc);
            strResult = IoResponseStream.ReadToEnd();
            IoResponseStream.Close();
            rspPost.Close();
            */



            #endregion
            //*/



        }
    }
}


