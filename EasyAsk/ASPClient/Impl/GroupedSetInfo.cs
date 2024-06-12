namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class GroupedSetInfo : IGroupedResultSet
    {
        private static readonly string ATTR_BREAK_GROUPS = "EA_BreakGroups";
        private static readonly string ATTR_CRITERIA = "EA_Name";
        private static readonly string ATTR_MAX_ROWS = "EA_MaxRows";
        private static readonly string ATTR_PAGINATE = "EA_Paginate";
        private static readonly string ATTR_TOTAL_GROUPS = "EA_TotalGroups";
        private static readonly string ATTR_TYPE = "EA_Type";
        private bool m_breakGroups = false;
        private string m_criteria = null;
        private List<IGroupedResult> m_groups = null;
        private IList<GroupedPageLayout> m_layout = null;
        private int m_maxRowsPerGroup = -1;
        private XmlNode m_node;
        private bool m_paginate = false;
        private INavigateResults m_res = null;
        private int m_totalGroups;
        private int m_totalRows = 0;
        private int m_type;
        private static readonly string RELATIVE_PATH_GROUPS = "EA_Group";

        internal GroupedSetInfo(XmlNode node, INavigateResults res)
        {
            this.m_node = node;
            this.m_res = res;
            this.m_type = DOMUtilities.getIntegerAttribute(this.m_node, ATTR_TYPE);
            this.m_criteria = DOMUtilities.getStringAttribute(this.m_node, ATTR_CRITERIA);
            this.m_totalGroups = DOMUtilities.getIntegerAttribute(this.m_node, ATTR_TOTAL_GROUPS);
            this.m_paginate = DOMUtilities.getBooleanAttribute(this.m_node, ATTR_PAGINATE);
            this.m_breakGroups = DOMUtilities.getBooleanAttribute(this.m_node, ATTR_BREAK_GROUPS);
            this.m_maxRowsPerGroup = DOMUtilities.getIntegerAttribute(this.m_node, ATTR_MAX_ROWS);
        }

        public int getEndGroup()
        {
            this.processGroups();
            int num = Math.Min(this.m_res.getCurrentPage(), this.m_layout.Count);
            IGroupedResult item = this.m_layout[num - 1].getEndGroup();
            return this.m_groups.IndexOf(item);
        }

        public IGroupedResult getGroup(int i)
        {
            this.processGroups();
            return this.m_groups[i];
        }

        public string getGroupCriteria()
        {
            return this.m_criteria;
        }

        public int getGroupCriteriaType()
        {
            return this.m_type;
        }

        public int getGroupEndRow(IGroupedResult group)
        {
            this.processGroups();
            int num = this.m_res.getCurrentPage();
            if (this.m_layout[num - 1].getEndGroup() == group)
            {
                return this.m_layout[num - 1].getEndRow();
            }
            return group.getNumberOfRows();
        }

        public int getGroupStartRow(IGroupedResult group)
        {
            this.processGroups();
            int num = this.m_res.getCurrentPage();
            if (this.m_layout[num - 1].getStartGroup() == group)
            {
                return this.m_layout[num - 1].getStartRow();
            }
            return 1;
        }

        public int getMaximumRowsPerGroup()
        {
            return this.m_maxRowsPerGroup;
        }

        public string getNodeString(IGroupedResult group)
        {
            string str = group.getGroupValue();
            if (1 == this.m_type)
            {
                foreach (INavigateCategory category in this.m_res.getDetailedCategories())
                {
                    if (str.Equals(category.getName(), StringComparison.OrdinalIgnoreCase))
                    {
                        return category.getNodeString();
                    }
                }
            }
            else if (2 == this.m_type)
            {
                foreach (INavigateAttribute attribute in this.m_res.getDetailedAttributeValues(this.getGroupCriteria()))
                {
                    if (str.Equals(attribute.getValue(), StringComparison.OrdinalIgnoreCase))
                    {
                        return attribute.getNodeString();
                    }
                }
            }
            return "";
        }

        public int getNumberOfGroups()
        {
            return this.m_totalGroups;
        }

        public int getPageCount()
        {
            this.processGroups();
            return this.m_layout.Count;
        }

        public int getStartGroup()
        {
            this.processGroups();
            int num = Math.Min(this.m_res.getCurrentPage(), this.m_layout.Count);
            IGroupedResult item = this.m_layout[num - 1].getStartGroup();
            return this.m_groups.IndexOf(item);
        }

        public int getTotalNumberOfRows()
        {
            this.processGroups();
            return this.m_totalRows;
        }

        private void processGroups()
        {
            if (null == this.m_groups)
            {
                this.m_groups = new List<IGroupedResult>();
                foreach (XmlNode node in this.m_node.SelectNodes(RELATIVE_PATH_GROUPS))
                {
                    this.m_groups.Add(new GroupInfo(this.m_res, node, this));
                }
                this.m_groups.TrimExcess();
                this.m_layout = GroupedPageLayout.layoutPages(this.m_paginate ? this.m_res.getResultsPerPage() : -1, this.m_groups, this.m_breakGroups);
                this.m_totalRows = 0;
                foreach (IGroupedResult result in this.m_groups)
                {
                    this.m_totalRows += result.getTotalNumberOfRows();
                }
            }
        }
    }
}

