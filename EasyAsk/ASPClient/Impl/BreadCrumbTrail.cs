namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class BreadCrumbTrail : IBreadCrumbTrail
    {
        private string m_fullPath = null;
        private IList<INavigateNode> m_navNodes = new List<INavigateNode>(0);
        private string m_pureCategoryPath = null;
        private static readonly string RELATIVE_PATH_TO_FULL_PATH = "EA_FullPath";
        private static readonly string RELATIVE_PATH_TO_NODES = "EA_NavPathNodeList/EA_NavPathNode";
        private static readonly string RELATIVE_PATH_TO_PURE_CATEGORY_PATH = "EA_PureCategoryPath";

        internal BreadCrumbTrail(XmlNode node)
        {
            if (null != node)
            {
                this.m_fullPath = DOMUtilities.getString(node, RELATIVE_PATH_TO_FULL_PATH);
                this.m_pureCategoryPath = DOMUtilities.getString(node, RELATIVE_PATH_TO_PURE_CATEGORY_PATH);
                XmlNodeList list = node.SelectNodes(RELATIVE_PATH_TO_NODES);
                if (null != list)
                {
                    foreach (XmlNode node2 in list)
                    {
                        this.m_navNodes.Add(new NavigateNode(node2));
                    }
                }
            }
        }

        public string getFullPath()
        {
            return this.m_fullPath;
        }

        public string getPureCategoryPath()
        {
            return this.m_pureCategoryPath;
        }

        public IList<INavigateNode> getSearchPath()
        {
            return this.m_navNodes;
        }
    }
}

