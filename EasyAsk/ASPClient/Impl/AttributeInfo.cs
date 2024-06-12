namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class AttributeInfo
    {
        private static readonly string ATTR_IS_LIMITED = "EA_IsInitDispLimited";
        private static readonly string ATTR_LIMIT = "EA_InitDispLimit";
        private static readonly string ATTR_TYPE = "EA_AttrType";
        private int m_attrType = EasyAskConstants.ATTR_TYPE_NORMAL;
        private List<INavigateAttribute> m_fullList = null;
        private List<INavigateAttribute> m_initialList = null;
        private bool m_isLimited = false;
        private int m_limit = -1;
        private string m_name;
        private XmlNode m_xmlNode = null;
        private static readonly string RELATIVE_PATH_FULL_LIST = "EA_AttributeValueList/EA_AttributeValue";
        private static readonly string RELATIVE_PATH_INITIAL_LIST = "EA_InitialAttributeValueList/EA_AttributeValue";
        private static readonly string RELATIVE_PATH_NAME = "EA_AttributeName";

        internal AttributeInfo(XmlNode node)
        {
            this.m_xmlNode = node;
            this.m_name = DOMUtilities.getString(node, RELATIVE_PATH_NAME);
            this.m_isLimited = DOMUtilities.getBooleanAttribute(node, ATTR_IS_LIMITED);
            this.m_limit = DOMUtilities.getIntegerAttribute(node, ATTR_LIMIT);
            this.m_attrType = DOMUtilities.getIntegerAttribute(node.SelectSingleNode(RELATIVE_PATH_FULL_LIST), ATTR_TYPE, EasyAskConstants.ATTR_TYPE_NORMAL);
        }

        private List<INavigateAttribute> formList(XmlNodeList attrVals)
        {
            List<INavigateAttribute> list = new List<INavigateAttribute>();
            foreach (XmlNode node in attrVals)
            {
                list.Add(new NavigateAttribute(this.m_name, node));
            }
            list.TrimExcess();
            return list;
        }

        public int getAttrType()
        {
            return this.m_attrType;
        }

        public IList<INavigateAttribute> getFullList()
        {
            if ((this.m_fullList == null) && (null != this.m_xmlNode))
            {
                this.m_fullList = this.formList(this.m_xmlNode.SelectNodes(RELATIVE_PATH_FULL_LIST));
            }
            return this.m_fullList;
        }

        public IList<INavigateAttribute> getInitialList()
        {
            if ((this.m_initialList == null) && (null != this.m_xmlNode))
            {
                this.m_initialList = this.formList(this.m_xmlNode.SelectNodes(RELATIVE_PATH_INITIAL_LIST));
            }
            return this.m_initialList;
        }

        public bool getIsLimited()
        {
            return this.m_isLimited;
        }

        public int getLimit()
        {
            return this.m_limit;
        }

        public string getName()
        {
            return this.m_name;
        }
    }
}

