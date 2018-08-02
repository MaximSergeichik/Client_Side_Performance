using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Client_Side
{
    class ParsePlan
    {
        #region Data

        private static string PathToPlan;
        private static List<TestAction> TestActions = new List<TestAction>();

        #endregion

        #region Mathods

        static TestAction ReadNode(XmlNode node)
        {
            TestAction o = new TestAction();

            o.Type = node.Name;
            o.InnerNodeText = node.InnerText;
            if (node.Attributes != null)
            {
                XmlAttributeCollection attr = node.Attributes;
                foreach (XmlAttribute a in attr)
                {
                    switch (a.Name)
                    {
                        case "name": { o.Name = a.Value; break; }
                        case "text": { o.Text = a.Value; break; }
                        case "measure": { o.Measure = a.Value; break; }
                        case "measureType": { o.MeasureType = a.Value; break; }
                        case "wait": { o.Wait = a.Value; break; }
                        case "waitForXPath": { o.WaitForXPath = a.Value; break; }
                        case "waitForClassName": { o.WaitForClassName = a.Value; break; }
                        case "waitForTitle": { o.WaitForTitle = a.Value; break; }
                        case "waitForTag": { o.WaitForTag = a.Value; break; }
                        case "waitForInTag": { o.WaitForInTag = a.Value; break; }
                        case "waitForinLabel": { o.WaitForInLabel = a.Value; break; }
                        case "waitForInText": { o.WaitForInText = a.Value; break; }
                        case "waitForInValue": { o.WaitForInValue = a.Value; break; }
                        case "waitForId": { o.WaitForId = a.Value; break; }
                        case "objectClassName": { o.ObjectClassName = a.Value; break; }
                        case "objectTitle": { o.ObjectTitle = a.Value; break; }
                        case "objectTag": { o.ObjectTag = a.Value; break; }
                        case "objectInTag": { o.ObjectInTag = a.Value; break; }
                        case "objectInLabel": { o.ObjectInLabel = a.Value; break; }
                        case "objectInText": { o.ObjectInText = a.Value; break; }
                        case "objectInValue": { o.ObjectInValue = a.Value; break; }
                        case "objectId": { o.ObjectId = a.Value; break; }
                        case "objectXPath": { o.ObjectXPath = a.Value; break; }
                        case "targetClassName": { o.TargetClassName = a.Value; break; }
                        case "targetTitle": { o.TargetTitle = a.Value; break; }
                        case "targetTag": { o.TargetTag = a.Value; break; }
                        case "targetInTag": { o.TargetInTag = a.Value; break; }
                        case "targetInLabel": { o.TargetInLabel = a.Value; break; }
                        case "targetInText": { o.TargetInText = a.Value; break; }
                        case "targetInValue": { o.TargetInValue = a.Value; break; }
                        case "targetId": { o.TargetId = a.Value; break; }
                        case "targetXPath": { o.TargetXPath = a.Value; break; }
                        case "value": { o.Value = a.Value; break; }
                        case "iterations": { o.Iterations = a.Value; break; }
                        default: { o.UndefinedAttribute = a.Value; break; }
                    }
                }
            }

            return o;
        }

        static List<TestAction> ReadNodes(XmlNodeList nodes)
        {
            List<TestAction> list = new List<TestAction>();
            foreach (XmlNode node in nodes)
            {
                if (node.Name == "Test")
                {
                    list = ReadNodes(node.ChildNodes);
                }
                if (node.Name == "if")
                {
                    TestAction o = ReadNode(node);
                    o.InnerActions = ReadNodes(node.ChildNodes);
                    list.Add(o);
                }
                if (node.Name == "for")
                {
                    TestAction o = ReadNode(node);
                    o.InnerActions = ReadNodes(node.ChildNodes);
                    list.Add(o);
                }
                else if (node.Name != "xml" && node.Name != "Test")
                {
                    list.Add(ReadNode(node));
                }
            }
            return list;
        }

        public static List<TestAction> Plan()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(PathToPlan);
            XmlNodeList nodes = doc.ChildNodes;
            TestActions = ReadNodes(nodes);

            return TestActions;
        }

        #endregion

        #region Properties

        public static string SetPathToPlan
        {
            set
            {
                PathToPlan = value;
            }
        }

        #endregion
    }
}
