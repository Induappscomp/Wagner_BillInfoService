namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class NavigateHierarchy : INavigateHierarchy
    {
        private static readonly string ATTR_IDS = "EA_IDs";
        private static readonly string ATTR_IS_SELECTED = "EA_IsSelected";
        private static readonly string ATTR_PATH = "EA_NavHierPath";
        private IList<string> m_ids = null;
        private bool m_isSelected;
        private string m_name;
        private string m_path;
        private IList<INavigateHierarchy> m_subNodes;
        private static readonly string RELATIVE_PATH_SUBNODES = "EA_NavSubNodes/EA_NavHierNode";

        internal NavigateHierarchy(XmlNode node)
        {
            this.m_name = node.FirstChild.InnerText;
            this.m_path = DOMUtilities.getStringAttribute(node, ATTR_PATH, "");
            this.m_isSelected = DOMUtilities.getBooleanAttribute(node, ATTR_IS_SELECTED);
            XmlNodeList list = node.SelectNodes(RELATIVE_PATH_SUBNODES);
            if (null != list)
            {
                this.m_subNodes = new List<INavigateHierarchy>(list.Count);
                foreach (XmlNode node2 in list)
                {
                    this.m_subNodes.Add(new NavigateHierarchy(node2));
                }
            }
            else
            {
                this.m_subNodes = new List<INavigateHierarchy>(0);
            }
            string str = DOMUtilities.getStringAttribute(node, ATTR_IDS, null);
            if (null != str)
            {
                string[] strArray = str.Split(new char[] { ',' });
                this.m_ids = new List<string>(strArray.Length);
                foreach (string str2 in strArray)
                {
                    this.m_ids.Add(str2);
                }
            }
            else
            {
                this.m_ids = new List<string>(0);
            }
        }

        public IList<string> getIDs()
        {
            return this.m_ids;
        }

        public string getName()
        {
            return this.m_name;
        }

        public string getPath()
        {
            return this.m_path;
        }

        public IList<INavigateHierarchy> getSubNodes()
        {
            return this.m_subNodes;
        }

        public bool isSelected()
        {
            return this.m_isSelected;
        }
    }
}

