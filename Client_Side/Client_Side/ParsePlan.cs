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
                    o.value = node.InnerText;
                    if (node.Attributes != null)
                    {
                        XmlAttributeCollection attr = node.Attributes;
                        foreach (XmlAttribute a in attr)
                        {
                            switch(a.Name)
                            {
                                case "name": { o.name = a.Value; break; }
                                case "waitFor": { o.waitFor = a.Value; break; }
                                case "className": { o.className = a.Value; break; }
                                case "title": { o.title = a.Value; break; }
                                case "text": { o.text = a.Value; break; }
                                case "measure": { o.measure = a.Value; break; }
                                case "tag": { o.tag = a.Value; break; }
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
