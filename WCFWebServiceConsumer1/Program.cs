using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Added usings
using System.IO;                            // Stream, StreamReader
using System.Net;                           // WebRequest, HttpWebResponse, HttpStatusCode, WebClient
using System.Xml;                           // XmlDocument
using System.Runtime.Serialization.Json;    // DataContractJsonSerializer
using WCFWebServiceConsumer1.DataObjects;

namespace WCFWebServiceConsumer1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WCFWebServiceConsumer1" + '\n');
            Console.WriteLine("This console app demonstrates the consumation of WCFWebServiceApplication1" + '\n');
            Console.WriteLine("Several test processes are used:");
            Console.WriteLine("1.Simple standard process");
            Console.WriteLine("2.Simple more streamlined process");
            Console.WriteLine("3.XML Format response and parsing - simple examples");
            Console.WriteLine("4.JSON Format response and parsing - simple examples");
            Console.WriteLine("");
            Console.WriteLine("");


            #region - The original simple method to consume, works for simple types (string, int, etc)
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("This section uses:");
            Console.WriteLine("WebRequest, HttpWebResponse, Stream and StreamReader");
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




            #region - Simple steps to consume the RESt StateObject from WCFWebServiceApplication1
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("This section uses:");
            Console.WriteLine("WebRequest, WebResponse and StreamReader");
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



            #region - The original simple method to consume, works for simple types, example from: http://stackoverflow.com/questions/14058992/wcf-rest-service-consumption-in-c-sharp-console-application
            Console.WriteLine("********************************************************************************");
            Console.WriteLine("This section uses:");
            Console.WriteLine("WebRequest (ContentType = @\"application / xml; charset = utf - 8\",");
            Console.WriteLine("HttpWebResponse, XmlDocument (GetElementsByTagName), XmlReader, XmlNodeList to");
            Console.WriteLine("consume the StateoObject from the WCFWebServiceApplication1");
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
                Console.WriteLine("Using XmlDocument.InnerText:" + '\n');
                XmlDocument xmlDoc = new XmlDocument();
                XmlReader xmlRdr = new XmlTextReader(respXML.GetResponseStream());
                xmlDoc.Load(xmlRdr);
                Console.WriteLine(xmlDoc.InnerText);


                // Using GetElementsByTagName allows you to specifically target a specific StateObject property name
                Console.WriteLine("Using XmlDocument.GetElementsByTagName:");
                XmlDocument xmlDoc2 = new XmlDocument();
                XmlReader xmlRdr2 = new XmlTextReader(respXML.GetResponseStream());
                xmlDoc2.LoadXml(xmlDoc.InnerXml);

                Console.WriteLine("Id:");
                XmlNodeList list2 = xmlDoc2.GetElementsByTagName("Id");
                foreach (XmlNode node in list2)
                {
                    Console.WriteLine(node.InnerText);
                }

                Console.WriteLine("Abrv:");
                list2 = xmlDoc2.GetElementsByTagName("Abrv");
                foreach (XmlNode node in list2)
                {
                    Console.WriteLine(node.InnerText);
                }

                Console.WriteLine("Name:");
                list2 = xmlDoc2.GetElementsByTagName("Name");
                foreach (XmlNode node in list2)
                {
                    Console.WriteLine(node.InnerText);
                }


                // This was not working
                /*
                XmlNodeList list = xmlDoc2.SelectNodes("/StateObject[@*]");
                foreach (XmlNode node in list)
                {
                    XmlNode xnode = node.SelectSingleNode("/Abrv");

                    if (xnode != null)
                    {
                        string value = xnode["Abrv"].InnerText;
                        Console.WriteLine("Node:" + value + '\n');
                    }
                }
                */

            }
            Console.WriteLine("");
            Console.WriteLine("");

            #endregion


            #region - Example to consume a Data object, example from: http://stackoverflow.com/questions/14058992/wcf-rest-service-consumption-in-c-sharp-console-application
            Console.WriteLine("********************************************************************************");
            WebRequest reqJSON = WebRequest.Create(@"http://localhost:57531/Service1.svc/StateJSON/5");
            reqJSON.Method = "GET";
            reqJSON.ContentType = @"application/json; charset=utf-8";

            // original version that failed
            //HttpWebResponse respJSON = (HttpWebResponse)reqJSON.GetResponse();
            //string jResp = string.Empty;
            //using (StreamReader sr = new StreamReader(respJSON.GetResponseStream()))
            //{
            //    jResp = sr.ReadToEnd();
            //    Console.WriteLine(jResp);
            //}

            // 2nd try
            //HttpWebResponse respJSON = reqJSON.GetResponse() as HttpWebResponse;
            HttpWebResponse respJSON2 = (HttpWebResponse)reqJSON.GetResponse();
            string jResp = string.Empty;
            using (StreamReader sr = new StreamReader(respJSON2.GetResponseStream()))
            {
                jResp = sr.ReadToEnd();
                Console.WriteLine(jResp);
            }

            #endregion


        }
    }
}


