namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class GroupInfo : IGroupedResult
    {
        private static readonly string ATTR_NAME = "EA_Name";
        private static readonly string ATTR_PRODUCT_COUNT = "EA_ProductCount";
        private static readonly string ATTR_TOTAL_ROWS = "EA_TotalRows";
        private IList<AttributeInfo> m_attributes = null;
        private CategoriesInfo m_catInfo = null;
        private int m_count;
        private IList<ItemRow> m_items;
        private string m_name;
        private XmlNode m_node = null;
        private INavigateResults m_res = null;
        private GroupedSetInfo m_set = null;
        private int m_totalRows;
        private static readonly string RELATIVE_PATH_ATTRIBUTE = "EA_Attribute";
        private static readonly string RELATIVE_PATH_TO_ITEM = "EA_Item";

        internal GroupInfo(INavigateResults res, XmlNode node, GroupedSetInfo set)
        {
            this.m_res = res;
            this.m_node = node;
            this.m_set = set;
            this.m_name = DOMUtilities.getStringAttribute(node, ATTR_NAME);
            this.m_count = DOMUtilities.getIntegerAttribute(node, ATTR_PRODUCT_COUNT);
            this.m_totalRows = DOMUtilities.getIntegerAttribute(node, ATTR_TOTAL_ROWS);
        }

        public IList<INavigateCategory> getAllCategoryDetails()
        {
            return this.getDetailedCategories(EasyAskConstants.CATEGORY_DISPLAY_MODE_FULL);
        }

        public IList<string> getAttributeNames()
        {
            this.processAttributes();
            List<string> list = new List<string>(this.m_attributes.Count);
            foreach (AttributeInfo info in this.m_attributes)
            {
                list.Add(info.getName());
            }
            return list;
        }

        public int getAttributeValueCount(string attr, string val)
        {
            this.processAttributes();
            foreach (INavigateAttribute attribute in this.getDetailedAttributeValues(attr, EasyAskConstants.ATTR_DISPLAY_MODE_FULL))
            {
                if (attribute.getValue().Equals(val))
                {
                    return attribute.getProductCount();
                }
            }
            return -1;
        }

        private AttributeInfo getAttrInfo(string attrName)
        {
            this.processAttributes();
            foreach (AttributeInfo info in this.m_attributes)
            {
                if (0 == string.Compare(attrName, info.getName(), true))
                {
                    return info;
                }
            }
            return null;
        }

        public IList<string> getCategories()
        {
            IList<string> list = new List<string>();
            foreach (INavigateCategory category in this.getAllCategoryDetails())
            {
                list.Add(category.getName());
            }
            return list;
        }

        public string getCellData(int row, int col)
        {
            this.processItems();
            return this.m_items[row].getFormattedText(col);
        }

        public IList<INavigateAttribute> getDetailedAttributeValues(string attrName, int displayMode)
        {
            AttributeInfo info = this.getAttrInfo(attrName);
            if (null != info)
            {
                if (!((EasyAskConstants.ATTR_DISPLAY_MODE_INITIAL != displayMode) || info.getIsLimited()))
                {
                    displayMode = EasyAskConstants.ATTR_DISPLAY_MODE_FULL;
                }
                return ((EasyAskConstants.ATTR_DISPLAY_MODE_FULL == displayMode) ? info.getFullList() : info.getInitialList());
            }
            return new List<INavigateAttribute>(0);
        }

        protected IList<INavigateCategory> getDetailedCategories(int nDisplayMode)
        {
            this.processCategories();
            return this.m_catInfo.getDetailedCategories(nDisplayMode);
        }

        public INavigateCategory getDetailedSuggestedCategory()
        {
            return null;
        }

        public int getEndRow()
        {
            return this.m_set.getGroupEndRow(this);
        }

        public string getGroupValue()
        {
            return this.m_name;
        }

        public IResultRow getItem(int row)
        {
            this.processItems();
            return this.m_items[row];
        }

        public int getNumberOfRows()
        {
            this.processItems();
            return this.m_items.Count;
        }

        public int getStartRow()
        {
            return this.m_set.getGroupStartRow(this);
        }

        public string getSuggestedCategoryID()
        {
            return "";
        }

        public string getSuggestedCategoryTitle()
        {
            this.processCategories();
            return this.m_catInfo.getSuggestedCategoryTitle();
        }

        public int getTotalNumberOfRows()
        {
            return this.m_totalRows;
        }

        internal void processAttributes()
        {
            if (null == this.m_attributes)
            {
                XmlNodeList list = this.m_node.SelectNodes(RELATIVE_PATH_ATTRIBUTE);
                this.m_attributes = new List<AttributeInfo>(list.Count);
                foreach (XmlNode node in list)
                {
                    this.m_attributes.Add(new AttributeInfo(node));
                }
            }
        }

        internal void processCategories()
        {
            if (null == this.m_catInfo)
            {
                this.m_catInfo = new CategoriesInfo(this.m_node);
            }
        }

        private void processItems()
        {
            if (null == this.m_items)
            {
                this.m_items = new List<ItemRow>();
                XmlNodeList list = this.m_node.SelectNodes(RELATIVE_PATH_TO_ITEM);
                if (null != list)
                {
                    IList<IDataDescription> desc = this.m_res.getDataDescriptions();
                    foreach (XmlNode node in list)
                    {
                        this.m_items.Add(new ItemRow(desc, node));
                    }
                }
            }
        }
    }
}

