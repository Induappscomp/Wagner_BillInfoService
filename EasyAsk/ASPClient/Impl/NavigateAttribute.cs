namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Text;
    using System.Xml;

    internal class NavigateAttribute : INavigateAttribute
    {
        private static readonly string ATTR_DISPLAY_AS_LINK = "EA_DisplayAsLink";
        private static readonly string ATTR_NODE_STRING = "EA_NodeString";
        private static readonly string ATTR_PRODUCT_COUNT = "EA_ProductCount";
        private static readonly string ATTR_TYPE = "EA_AttrType";
        private bool m_displayAsLink = false;
        private string m_name;
        private string m_nodeString;
        private int m_productCount;
        private int m_type;
        private string m_value;
        private static readonly string[] splitPathSep = new string[] { "////" };
        private static readonly string[] splitValSep = new string[] { ";;;;" };

        internal NavigateAttribute(string name, XmlNode node)
        {
            this.m_name = name;
            this.m_displayAsLink = DOMUtilities.getBooleanAttribute(node, ATTR_DISPLAY_AS_LINK);
            this.m_productCount = DOMUtilities.getIntegerAttribute(node, ATTR_PRODUCT_COUNT);
            this.m_type = DOMUtilities.getIntegerAttribute(node, ATTR_TYPE, EasyAskConstants.ATTR_TYPE_NORMAL);
            this.m_nodeString = DOMUtilities.getStringAttribute(node, ATTR_NODE_STRING);
            this.m_value = node.FirstChild.InnerText;
        }

        public bool getDisplayAsLink()
        {
            return this.m_displayAsLink;
        }

        public string getName()
        {
            return this.m_name;
        }

        public string getNodeString()
        {
            return this.m_nodeString;
        }

        public int getProductCount()
        {
            return this.m_productCount;
        }

        public int getType()
        {
            return this.m_type;
        }

        public string getValue()
        {
            return this.m_value;
        }

        public string removeFromPath(string path)
        {
            string[] strArray = path.Split(splitPathSep, StringSplitOptions.None);
            StringBuilder builder = new StringBuilder();
            string str = this.getName() + " = '" + this.getValue() + "'";
            for (int i = 0; i < strArray.Length; i++)
            {
                if (strArray[i].StartsWith("AttribSelect="))
                {
                    StringBuilder builder2 = new StringBuilder();
                    string[] strArray2 = strArray[i].Substring("AttribSelect=".Length).Split(splitValSep, StringSplitOptions.None);
                    for (int j = 0; j < strArray2.Length; j++)
                    {
                        if (!strArray2[j].Equals(str))
                        {
                            if (0 < builder2.Length)
                            {
                                builder2.Append(";;;;");
                            }
                            builder2.Append(strArray2[j]);
                        }
                    }
                    if (0 < builder2.Length)
                    {
                        if (0 < builder.Length)
                        {
                            builder.Append("////");
                        }
                        builder.Append("AttribSelect=");
                        builder.Append(builder2);
                    }
                }
                else
                {
                    if (0 < builder.Length)
                    {
                        builder.Append("////");
                    }
                    builder.Append(strArray[i]);
                }
            }
            return builder.ToString();
        }
    }
}

