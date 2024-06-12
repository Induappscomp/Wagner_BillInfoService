namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Xml;

    internal class NavigateNode : INavigateNode
    {
        private String m_englishName;
        private String m_path;
        private String m_purePath;
        private String m_SEOPath;
        private int m_type;
        private String m_value;
        private static readonly string RELATIVE_PATH_TO_ENGLISH_NAME = "EA_EnglishName";
        private static readonly string RELATIVE_PATH_TO_PATH = "EA_Path";
        private static readonly string RELATIVE_PATH_TO_PATH_TYPE = "EA_NavNodePathType";
        private static readonly string RELATIVE_PATH_TO_PURE_PATH = "EA_PurePath";
        private static readonly string RELATIVE_PATH_TO_SEO_PATH = "EA_SEOPath";
        private static readonly string RELATIVE_PATH_TO_VALUE = "EA_Value";

        internal NavigateNode(XmlNode node)
        {
            m_value = DOMUtilities.getString(node, RELATIVE_PATH_TO_VALUE);
            m_path = DOMUtilities.getString(node, RELATIVE_PATH_TO_PATH);
            m_purePath = DOMUtilities.getString(node, RELATIVE_PATH_TO_PURE_PATH);
            m_SEOPath = DOMUtilities.getString(node, RELATIVE_PATH_TO_SEO_PATH);
            m_type = DOMUtilities.getInteger(node, RELATIVE_PATH_TO_PATH_TYPE);
            m_englishName = DOMUtilities.getString(node, RELATIVE_PATH_TO_ENGLISH_NAME);
        }

        public string getEnglishName()
        {
            return m_englishName;
        }

        public string getLabel()
        {
            if (EasyAskConstants.NODE_TYPE_ATTRIBUTE == this.m_type)
            {
                return m_englishName.Substring(1, m_englishName.IndexOf(" = ") - 1).Trim();
            }
            if (EasyAskConstants.NODE_TYPE_CATEGORY == m_type)
            {
               // return "Item Category";
                return "ITEM CATEGORY";
            }
            if (EasyAskConstants.NODE_TYPE_USER_SEARCH == m_type)
            {
                return "User Search";
            }
            return null;
        }

        public string getPath()
        {
            return m_path;
        }

        public string getPurePath()
        {
            return m_purePath;
        }

        public string getSEOPath()
        {
            return m_SEOPath;
        }

        public int getType()
        {
            return m_type;
        }

        public string getValue()
        {
            return m_value;
        }
    }
}

