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
                            if (a.Name == "name")
                                o.name = a.Value;
                            else
                                o.attribute = a.Value;

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
