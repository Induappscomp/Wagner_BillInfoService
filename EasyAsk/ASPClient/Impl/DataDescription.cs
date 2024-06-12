namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Xml;

    internal class DataDescription : IDataDescription
    {
        private static readonly string ATTRIBUTE_COL_TYPE = "EA_ColType";
        private static readonly string ATTRIBUTE_DECODED = "EA_Decoded";
        private static readonly string ATTRIBUTE_DISPLAYABLE = "EA_Displayable";
        private static readonly string ATTRIBUTE_FORMAT = "EA_Format";
        private static readonly string ATTRIBUTE_HTML_TYPE = "EA_HTMLType";
        private string m_colName;
        private string m_colType;
        private bool m_decoded;
        private bool m_displayable;
        private string m_format;
        private string m_htmlType;
        private string m_tagName;
        private static readonly string RELATIVE_PATH_TO_COLUMN_NAME = "EA_ColumnName";
        private static readonly string RELATIVE_PATH_TO_TAG_NAME = "EA_TagName";

        internal DataDescription(XmlNode node)
        {
            this.m_colType = DOMUtilities.getStringAttribute(node, ATTRIBUTE_COL_TYPE, "A");
            this.m_htmlType = DOMUtilities.getStringAttribute(node, ATTRIBUTE_HTML_TYPE, "A");
            this.m_displayable = DOMUtilities.getBooleanAttribute(node, ATTRIBUTE_DISPLAYABLE);
            this.m_decoded = DOMUtilities.getBooleanAttribute(node, ATTRIBUTE_DECODED);
            this.m_format = DOMUtilities.getStringAttribute(node, ATTRIBUTE_FORMAT, null);
            this.m_tagName = DOMUtilities.getString(node, RELATIVE_PATH_TO_TAG_NAME);
            this.m_colName = DOMUtilities.getString(node, RELATIVE_PATH_TO_COLUMN_NAME);
        }

        public string getColName()
        {
            return this.m_colName;
        }

        public string getColType()
        {
            return this.m_colType;
        }

        public bool getDecoded()
        {
            return this.m_decoded;
        }

        public bool getDisplayable()
        {
            return this.m_displayable;
        }

        public string getFormat()
        {
            return this.m_format;
        }

        public string getHTMLType()
        {
            return this.m_htmlType;
        }

        public string getTagName()
        {
            return this.m_tagName;
        }
    }
}

