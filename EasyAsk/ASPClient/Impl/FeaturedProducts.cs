namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class FeaturedProducts : IFeaturedProducts
    {
        private static readonly string ATTR_PRODUCT_COUNT = "EA_ProductCount";
        private List<IResultRow> m_items = null;
        private XmlNode m_node = null;
        private int m_productCount = -1;
        private INavigateResults m_res;
        private static readonly string RELATIVE_PATH_TO_ITEMS = "EA_Item";

        public FeaturedProducts(INavigateResults res, XmlNode node)
        {
            this.m_node = node;
            this.m_res = res;
            if (null != this.m_node)
            {
                this.m_productCount = DOMUtilities.getIntegerAttribute(node, ATTR_PRODUCT_COUNT);
            }
        }

        public IList<IResultRow> getItems()
        {
            this.processItems();
            return this.m_items;
        }

        public int getProductCount()
        {
            return this.m_productCount;
        }

        private void processItems()
        {
            if (null == this.m_items)
            {
                this.m_items = new List<IResultRow>();
                if (null != this.m_node)
                {
                    foreach (XmlNode node in this.m_node.SelectNodes(RELATIVE_PATH_TO_ITEMS))
                    {
                        this.m_items.Add(new ItemRow(this.m_res.getDataDescriptions(), node));
                    }
                    this.m_items.TrimExcess();
                }
            }
        }
    }
}

