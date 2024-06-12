namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class CarveOut : ICarveOut
    {
        private static readonly string ATTR_DISPLAY_FORMAT = "EA_DisplayFormat";
        private static readonly string ATTR_MAXIMUM = "EA_Maximum";
        private static readonly string ATTR_PRODUCT_COUNT = "EA_ProductCount";
        private string m_displayFormat = null;
        private List<IResultRow> m_items = null;
        private int m_maximum = -1;
        private XmlNode m_node = null;
        private int m_productCount = -1;
        private INavigateResults m_res = null;
        private static readonly string RELATIVE_PATH_TO_ITEMS = "EA_Products/EA_Item";
        private static readonly string RELATIVE_PATH_TO_PRODUCTS = "EA_Products";

        public CarveOut(INavigateResults res, XmlNode node)
        {
            this.m_node = node;
            this.m_res = res;
            this.m_maximum = DOMUtilities.getIntegerAttribute(this.m_node, ATTR_MAXIMUM);
            this.m_displayFormat = DOMUtilities.getStringAttribute(this.m_node, ATTR_DISPLAY_FORMAT);
            this.m_productCount = DOMUtilities.getIntegerAttribute(this.m_node.SelectSingleNode(RELATIVE_PATH_TO_PRODUCTS), ATTR_PRODUCT_COUNT);
        }

        public string getDisplayFormat()
        {
            return this.m_displayFormat;
        }

        public IList<IResultRow> getItems()
        {
            this.processItems();
            return this.m_items;
        }

        public int getMaximum()
        {
            return this.m_maximum;
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
                foreach (XmlNode node in this.m_node.SelectNodes(RELATIVE_PATH_TO_ITEMS))
                {
                    this.m_items.Add(new ItemRow(this.m_res.getDataDescriptions(), node));
                }
            }
        }
    }
}

