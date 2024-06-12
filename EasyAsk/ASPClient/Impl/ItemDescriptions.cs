namespace TradingBell.WebCat.EasyAsk.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class ItemDescriptions
    {
        private int m_currentPage;
        private IList<IDataDescription> m_descs;
        private int m_firstItem;
        private bool m_isDrillDownActive;
        private int m_lastItem;
        private int m_pageCount;
        private int m_resultsPerPage;
        private string m_sortOrder;
        private int m_totalItems;
        private static readonly string RELATIVE_PATH_TO_CURRENT_PAGE = "EA_CurrentPage";
        private static readonly string RELATIVE_PATH_TO_DATA_DESCRIPTION = "EA_DataDescription";
        private static readonly string RELATIVE_PATH_TO_FIRST_ITEM = "EA_FirstItem";
        private static readonly string RELATIVE_PATH_TO_IS_DRILL_DOWN_ACTIVE = "EA_IsDrillDownActive";
        private static readonly string RELATIVE_PATH_TO_LAST_ITEM = "EA_LastItem";
        private static readonly string RELATIVE_PATH_TO_PAGE_COUNT = "EA_PageCount";
        private static readonly string RELATIVE_PATH_TO_RESULTS_PER_PAGE = "EA_ResultsPerPage";
        private static readonly string RELATIVE_PATH_TO_SORT_ORDER = "EA_SortOrder";
        private static readonly string RELATIVE_PATH_TO_TOTAL_ITEMS = "EA_TotalItems";

        public ItemDescriptions()
        {
            this.m_isDrillDownActive = false;
            this.m_pageCount = -1;
            this.m_currentPage = -1;
            this.m_totalItems = -1;
            this.m_resultsPerPage = -1;
            this.m_firstItem = -1;
            this.m_lastItem = -1;
            this.m_sortOrder = "";
            this.m_descs = null;
            this.m_descs = new List<IDataDescription>(0);
        }

        public ItemDescriptions(XmlNode node)
        {
            this.m_isDrillDownActive = false;
            this.m_pageCount = -1;
            this.m_currentPage = -1;
            this.m_totalItems = -1;
            this.m_resultsPerPage = -1;
            this.m_firstItem = -1;
            this.m_lastItem = -1;
            this.m_sortOrder = "";
            this.m_descs = null;
            this.m_isDrillDownActive = DOMUtilities.getBoolean(node, RELATIVE_PATH_TO_IS_DRILL_DOWN_ACTIVE);
            this.m_pageCount = DOMUtilities.getInteger(node, RELATIVE_PATH_TO_PAGE_COUNT);
            this.m_currentPage = DOMUtilities.getInteger(node, RELATIVE_PATH_TO_CURRENT_PAGE);
            this.m_totalItems = DOMUtilities.getInteger(node, RELATIVE_PATH_TO_TOTAL_ITEMS);
            this.m_resultsPerPage = DOMUtilities.getInteger(node, RELATIVE_PATH_TO_RESULTS_PER_PAGE);
            this.m_firstItem = DOMUtilities.getInteger(node, RELATIVE_PATH_TO_FIRST_ITEM);
            this.m_lastItem = DOMUtilities.getInteger(node, RELATIVE_PATH_TO_LAST_ITEM);
            this.m_sortOrder = DOMUtilities.getString(node, RELATIVE_PATH_TO_SORT_ORDER);
            XmlNodeList list = node.SelectNodes(RELATIVE_PATH_TO_DATA_DESCRIPTION);
            this.m_descs = new List<IDataDescription>();
            foreach (XmlNode node2 in list)
            {
                this.m_descs.Add(new DataDescription(node2));
            }
        }

        public int getColumnIndex(string colName)
        {
            for (int i = 0; i < this.m_descs.Count; i++)
            {
                DataDescription description = (DataDescription) this.m_descs[i];
                if (0 == string.Compare(colName, description.getColName(), true))
                {
                    return i;
                }
            }
            return -1;
        }

        public int getCurrentPage()
        {
            return this.m_currentPage;
        }

        public IList<IDataDescription> getDataDescriptions()
        {
            return this.m_descs;
        }

        public int getFirstItem()
        {
            return this.m_firstItem;
        }

        public bool getIsDrillDown()
        {
            return this.m_isDrillDownActive;
        }

        public int getLastItem()
        {
            return this.m_lastItem;
        }

        public int getPageCount()
        {
            return this.m_pageCount;
        }

        public int getResultsPerPage()
        {
            return this.m_resultsPerPage;
        }

        public string getSortOrder()
        {
            return this.m_sortOrder;
        }

        public int getTotalItems()
        {
            return this.m_totalItems;
        }
    }
}

