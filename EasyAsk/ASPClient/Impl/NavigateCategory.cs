namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class NavigateCategory : INavigateCategory
    {
        private readonly string ATTR_IDS = "EA_IDs";
        private readonly string ATTR_NODE_STRING = "EA_NodeString";
        private readonly string ATTR_PRODUCT_COUNT = "EA_ProductCount";
        private readonly string ATTR_SEO_PATH = "EA_SEOPath";
        private IList<string> m_ids = null;
        private string m_name;
        private string m_nodeString = null;
        private int m_productCount = -1;
        private string m_seoPath = null;
        private IList<INavigateCategory> m_subCategories = null;
        private readonly string RELATIVE_PATH_SUBCATEORIES = "EA_SubCategories/EA_Category";

        internal NavigateCategory(XmlNode node)
        {
            this.m_name = node.FirstChild.InnerText;
            this.m_productCount = DOMUtilities.getIntegerAttribute(node, this.ATTR_PRODUCT_COUNT);
            this.m_nodeString = DOMUtilities.getStringAttribute(node, this.ATTR_NODE_STRING);
            this.m_seoPath = DOMUtilities.getStringAttribute(node, this.ATTR_SEO_PATH);
            string str = DOMUtilities.getStringAttribute(node, this.ATTR_IDS, null);
            if (null != str)
            {
                string[] strArray = str.Split(new char[] { ',' });
                this.m_ids = new List<string>(strArray.Length);
                foreach (string str2 in strArray)
                {
                    this.m_ids.Add(str2);
                }
            }
            XmlNodeList list = node.SelectNodes(this.RELATIVE_PATH_SUBCATEORIES);
            if (null != list)
            {
                this.m_subCategories = new List<INavigateCategory>(list.Count);
                foreach (XmlNode node2 in list)
                {
                    this.m_subCategories.Add(new NavigateCategory(node2));
                }
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

        public string getNodeString()
        {
            return this.m_nodeString;
        }

        public int getProductCount()
        {
            return this.m_productCount;
        }

        public string getSEOPath()
        {
            return this.m_seoPath;
        }

        public IList<INavigateCategory> getSubCategories()
        {
            return this.m_subCategories;
        }
    }
}

