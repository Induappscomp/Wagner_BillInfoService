namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class CategoriesInfo
    {
        private static readonly string ATTR_IDS = "EA_IDs";
        private static readonly string ATTR_NODE_STRING = "EA_NodeString";
        private static readonly string ATTR_PRODUCT_COUNT = "EA_ProductCount";
        private static readonly string ATTR_SEO_PATH = "EA_SEOPath";
        private List<INavigateCategory> m_categories = new List<INavigateCategory>();
        private string m_detailedSuggestedCategory = "Categories";
        private string m_detailedSuggestedIDs = "";
        private string m_detailedSuggestedNodeString = "";
        private int m_detailedSuggestedProductCount = -1;
        private string m_detailedSuggestedSEOPath = "";
        private List<INavigateCategory> m_initialCategories = new List<INavigateCategory>();
        private static XmlNode m_node;
        private string m_suggestedCategoryID = "";
        private string m_suggestedCategoryTitle = "Categories";
        private static readonly string RELATIVE_PATH_ALL_CATS = "EA_CategoryList/EA_Category";
        private static readonly string RELATIVE_PATH_CATEGORIES = "EA_Categories";
        private static readonly string RELATIVE_PATH_DETAILED_SUGGESTED = "EA_DetailSuggestedCategory";
        private static readonly string RELATIVE_PATH_INITIAL_CATS = "EA_InitialCategoryList/EA_Category";
        private static readonly string RELATIVE_PATH_SUGGESTED_ID = "EA_SuggestedCategoryID";
        private static readonly string RELATIVE_PATH_SUGGESTED_TITLE = "EA_SuggestedCategoryTitle";

        public CategoriesInfo(XmlNode node)
        {
            m_node = node;
            this.processCategories();
        }

        public IList<INavigateCategory> getDetailedCategories()
        {
            return this.getDetailedCategories(EasyAskConstants.CATEGORY_DISPLAY_MODE_FULL);
        }

        public IList<INavigateCategory> getDetailedCategories(int nDisplayMode)
        {
            if (EasyAskConstants.CATEGORY_DISPLAY_MODE_INITIAL == nDisplayMode)
            {
                return this.m_initialCategories;
            }
            return this.m_categories;
        }

        public string getDetailedSuggestedCategory()
        {
            return this.m_detailedSuggestedCategory;
        }

        public string getDetailedSuggestedIDs()
        {
            return this.m_detailedSuggestedIDs;
        }

        public string getDetailedSuggestedNodeString()
        {
            return this.m_detailedSuggestedNodeString;
        }

        public int getDetailedSuggestedProductCount()
        {
            return this.m_detailedSuggestedProductCount;
        }

        public string getDetailedSuggestedSEOPath()
        {
            return this.m_detailedSuggestedSEOPath;
        }

        public string getSuggestedCategoryID()
        {
            return this.m_suggestedCategoryID;
        }

        public string getSuggestedCategoryTitle()
        {
            return this.m_suggestedCategoryTitle;
        }

        private void processCategories()
        {
            if (null != m_node)
            {
                XmlNode node = m_node.SelectSingleNode(RELATIVE_PATH_CATEGORIES);
                if (null != node)
                {
                    XmlNodeList list = node.SelectNodes(RELATIVE_PATH_ALL_CATS);
                    foreach (XmlNode node2 in list)
                    {
                        this.m_categories.Add(new NavigateCategory(node2));
                    }
                    list = node.SelectNodes(RELATIVE_PATH_INITIAL_CATS);
                    if ((list != null) && (0 < list.Count))
                    {
                        foreach (XmlNode node2 in list)
                        {
                            this.m_initialCategories.Add(new NavigateCategory(node2));
                        }
                    }
                    XmlNode node3 = node.SelectSingleNode(RELATIVE_PATH_SUGGESTED_TITLE);
                    if (null != node3)
                    {
                        this.m_suggestedCategoryTitle = node3.InnerText;
                    }
                    node3 = node.SelectSingleNode(RELATIVE_PATH_SUGGESTED_ID);
                    if (null != node3)
                    {
                        this.m_suggestedCategoryID = node3.InnerText;
                    }
                    node3 = node.SelectSingleNode(RELATIVE_PATH_DETAILED_SUGGESTED);
                    if (null != node3)
                    {
                        this.m_detailedSuggestedCategory = node3.InnerText;
                        this.m_detailedSuggestedProductCount = DOMUtilities.getIntegerAttribute(node3, ATTR_PRODUCT_COUNT);
                        this.m_detailedSuggestedIDs = DOMUtilities.getStringAttribute(node3, ATTR_IDS);
                        this.m_detailedSuggestedNodeString = DOMUtilities.getStringAttribute(node3, ATTR_NODE_STRING);
                        this.m_detailedSuggestedSEOPath = DOMUtilities.getStringAttribute(node3, ATTR_SEO_PATH);
                    }
                }
            }
        }
    }
}

