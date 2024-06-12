namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class AttributesInfo
    {
        private static readonly string ATTR_ATTR_TYPE = "EA_AttrType";
        private static readonly string ATTR_INIT_DISP_LIMIT = "EA_InitDispLimit";
        private static readonly string ATTR_IS_INIT_DISP_LIMITED = "EA_IsInitDispLimited";
        private List<AttributeInfo> m_attributes = new List<AttributeInfo>();
        private bool m_bInitialDispLimitedForAttrNames = false;
        private Dictionary<int, List<string>> m_initialAttributeNames = new Dictionary<int, List<string>>();
        private int m_initialDispLimitForAttrNames = -1;
        private static readonly string RELATIVE_PATH_ATTRIBUTE = "EA_Attribute";
        private static readonly string RELATIVE_PATH_ATTRIBUTE_NAME = "EA_AttributeName";
        private static readonly string RELATIVE_PATH_INITIAL_NAME_ORDER = "EA_InitialAttrNameOrder";

        public AttributesInfo(XmlNode node)
        {
            if (null != node)
            {
                this.m_attributes = this.getAttributeInfo(node);
                this.m_bInitialDispLimitedForAttrNames = DOMUtilities.getBooleanAttribute(node, ATTR_IS_INIT_DISP_LIMITED);
                this.m_initialDispLimitForAttrNames = DOMUtilities.getIntegerAttribute(node, ATTR_INIT_DISP_LIMIT);
                foreach (XmlNode node2 in node.SelectNodes(RELATIVE_PATH_INITIAL_NAME_ORDER))
                {
                    int key = DOMUtilities.getIntegerAttribute(node2, ATTR_ATTR_TYPE);
                    XmlNodeList list = node2.SelectNodes(RELATIVE_PATH_ATTRIBUTE_NAME);
                    if (0 < list.Count)
                    {
                        List<string> list2 = new List<string>(list.Count);
                        foreach (XmlNode node3 in list)
                        {
                            list2.Add(node3.FirstChild.InnerText);
                        }
                        this.m_initialAttributeNames.Add(key, list2);
                    }
                }
            }
            this.m_attributes.TrimExcess();
        }

        private List<AttributeInfo> getAttributeInfo(XmlNode node)
        {
            List<AttributeInfo> list = new List<AttributeInfo>();
            if (null != node)
            {
                XmlNodeList list2 = node.SelectNodes(RELATIVE_PATH_ATTRIBUTE);
                list = new List<AttributeInfo>(list2.Count);
                foreach (XmlNode node2 in list2)
                {
                    list.Add(new AttributeInfo(node2));
                }
            }
            return list;
        }

        public IList<string> getAttributeNames(int attrFilter, int displayMode)
        {
            if (EasyAskConstants.ATTR_DISPLAY_MODE_INITIAL == displayMode)
            {
                return this.m_initialAttributeNames[attrFilter];
            }
            List<string> list = new List<string>();
            foreach (AttributeInfo info in this.m_attributes)
            {
                if (0 != (info.getAttrType() & attrFilter))
                {
                    list.Add(info.getName());
                }
            }
            return list;
        }

        private AttributeInfo getAttrInfo(string attrName)
        {
            foreach (AttributeInfo info in this.m_attributes)
            {
                if (0 == string.Compare(attrName, info.getName(), true))
                {
                    return info;
                }
            }
            return null;
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
            return new List<INavigateAttribute>();
        }

        public IList<string> getInitialDisplayList(int attrType)
        {
            return this.m_initialAttributeNames[attrType];
        }

        public int getInitialDispLimitForAttrNames()
        {
            return this.m_initialDispLimitForAttrNames;
        }

        public int getInitialDispLimitForAttrValues(string attrName)
        {
            AttributeInfo info = this.getAttrInfo(attrName);
            return ((info == null) ? -1 : info.getLimit());
        }

        public bool isInitialDispLimitedForAttrNames()
        {
            return this.m_bInitialDispLimitedForAttrNames;
        }

        public bool isInitialDispLimitedForAttrValues(string attrName)
        {
            AttributeInfo info = this.getAttrInfo(attrName);
            return ((info != null) && info.getIsLimited());
        }
    }
}

