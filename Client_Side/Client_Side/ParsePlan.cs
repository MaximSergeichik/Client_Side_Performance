using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Client_Side
{
    class ParsePlan
    {

        private static string PathToPlan;
        private static List<TestAction> list = new List<TestAction>();

        static void ReadNode(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "Test")
                {
                    ReadNode(node.ChildNodes);
                }
                else if (node.Name != "xml")
                {
                    TestAction o = new TestAction();
                    o.type = node.Name;
                    o.objectXPath = node.InnerText;
                    if (node.Attributes != null)
                    {
                        XmlAttributeCollection attr = node.Attributes;
                        foreach (XmlAttribute a in attr)
                        {
                            switch(a.Name)
                            {
                                case "name": { o.name = a.Value; break; }
                                case "text": { o.text = a.Value; break; }
                                case "measure": { o.measure = a.Value; break; }
                                case "waitForXPath": { o.waitForXPath = a.Value; break; }
                                case "waitForClassName": { o.waitForClassName = a.Value; break; }
                                case "waitForTitle": { o.waitForTitle = a.Value; break; }
                                case "waitForTag": { o.waitForTag = a.Value; break; }
                                case "waitForInTag": { o.waitForInTag = a.Value; break; }
                                case "waitForinLabel": { o.waitForInLabel = a.Value; break; }
                                case "waitForInText": { o.waitForInText = a.Value; break; }
                                case "waitForInValue": { o.waitForInValue = a.Value; break; }
                                case "objectClassName": { o.objectClassName = a.Value; break; }
                                case "objectTitle": { o.objectTitle = a.Value; break; }
                                case "objectTag": { o.objectTag = a.Value; break; }
                                case "objectInTag": { o.objectInTag = a.Value; break; }
                                case "objectInLabel": { o.objectInLabel = a.Value; break; }
                                case "objectInText": { o.objectInText = a.Value; break; }
                                case "objectInValue": { o.objectInValue = a.Value; break; }
                                case "sendInLog": { o.sendInLog = a.Value; break; }
                                default: { o.attribute = a.Value; break; }
                            }

                            //if (a.Name == "name")
                            //    o.name = a.Value;
                            //else
                            //    o.attribute = a.Value;

                        }
                    }
                    list.Add(o);
                }
            }
        }

        public static List<TestAction> Plan()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(PathToPlan);
            XmlNodeList nodes = doc.ChildNodes;
            ReadNode(nodes);

            return list;
        }

        public static string SetPathToPlan
        {
            set
            {
                PathToPlan = value;
            }
        }
    }
}
