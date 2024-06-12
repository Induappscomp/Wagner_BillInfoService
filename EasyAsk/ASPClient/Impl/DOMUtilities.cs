namespace TradingBell.WebCat.EasyAsk.Impl
{
    using System;
    using System.Xml;

    internal class DOMUtilities
    {
        internal static int getInteger(XmlNode node, String path)
        {
            XmlNode val = null == node ? null : node.SelectSingleNode(path);
            if (null != val)
            {
                try
                {
                    return Int32.Parse(val.FirstChild.InnerText);
                }
                catch (FormatException fe) { }
            }
            return -1;
        }

        internal static String getString(XmlNode node, String path)
        {
            XmlNode val = null == node ? null : node.SelectSingleNode(path);
            if (null != val)
            {
                return val.FirstChild.InnerText;
            }
            return null;
        }

        internal static Boolean getBoolean(XmlNode node, String path)
        {
            XmlNode val = null == node ? null : node.SelectSingleNode(path);
            if (null != val)
            {
                return 0 == String.Compare(val.FirstChild.InnerText, "true", true);
            }
            return false;
        }

        internal static Boolean getBooleanAttribute(XmlNode node, String attrName)
        {
            XmlAttribute val = null == node ? null : node.Attributes[attrName];
            if (null != val)
            {
                return 0 == String.Compare(val.InnerText, "true", true);
            }
            return false;
        }
        internal static string getStringAttribute(XmlNode node, string attrName)
        {
           XmlAttribute attribute = (node == null) ? null : node.Attributes[attrName];
            if (null != attribute)
            {
                return attribute.InnerText;
            }
            return "";
        }
        internal static String getStringAttribute(XmlNode node, String attrName, String defVal)
        {
            XmlAttribute val = null == node ? null : node.Attributes[attrName];
            if (null != val)
            {
                return val.InnerText;
            }
            return defVal;
        }

        internal static int getIntegerAttribute(XmlNode node, String attrName)
        {
            XmlAttribute val = null == node ? null : node.Attributes[attrName];
            if (null != val)
            {
                try
                {
                    return Int32.Parse(val.InnerText);
                }
                catch (FormatException fe) { }
            }
            return -1;
        }

        internal static int getIntegerAttribute(XmlNode node, String attrName, int defVal)
        {
            XmlAttribute val = null == node ? null : node.Attributes[attrName];
            if (null != val)
            {
                try
                {
                    return Int32.Parse(val.InnerText);
                }
                catch (FormatException fe) { }
            }
            return defVal;
        }
        //internal static bool getBoolean(XmlNode node, string path)
        //{
        //    XmlNode node2 = (node == null) ? null : node.SelectSingleNode(path);
        //    return ((null != node2) && (0 == string.Compare(node2.FirstChild.InnerText, "true", true)));
        //}

        //internal static bool getBooleanAttribute(XmlNode node, string attrName)
        //{
        //    XmlAttribute attribute = (node == null) ? null : node.Attributes[attrName];
        //    return ((null != attribute) && (0 == string.Compare(attribute.InnerText, "true", true)));
        //}

        //internal static int getInteger(XmlNode node, string path)
        //{
        //    XmlNode node2 = (node == null) ? null : node.SelectSingleNode(path);
        //    if (null != node2)
        //    {
        //        try
        //        {
        //            return int.Parse(node2.FirstChild.InnerText);
        //        }
        //        catch (FormatException)
        //        {
        //        }
        //    }
        //    return -1;
        //}

        //internal static int getIntegerAttribute(XmlNode node, string attrName)
        //{
        //    XmlAttribute attribute = (node == null) ? null : node.Attributes[attrName];
        //    if (null != attribute)
        //    {
        //        try
        //        {
        //            return int.Parse(attribute.InnerText);
        //        }
        //        catch (FormatException)
        //        {
        //        }
        //    }
        //    return -1;
        //}

        //internal static int getIntegerAttribute(XmlNode node, string attrName, int defVal)
        //{
        //    XmlAttribute attribute = (node == null) ? null : node.Attributes[attrName];
        //    if (null != attribute)
        //    {
        //        try
        //        {
        //            return int.Parse(attribute.InnerText);
        //        }
        //        catch (FormatException)
        //        {
        //        }
        //    }
        //    return defVal;
        //}

        //internal static string getString(XmlNode node, string path)
        //{
        //    XmlNode node2 = (node == null) ? null : node.SelectSingleNode(path);
        //    if (null != node2)
        //    {
        //        return node2.FirstChild.InnerText;
        //    }
        //    return null;
        //}

        //internal static string getStringAttribute(XmlNode node, string attrName)
        //{
        //    XmlAttribute attribute = (node == null) ? null : node.Attributes[attrName];
        //    if (null != attribute)
        //    {
        //        return attribute.InnerText;
        //    }
        //    return "";
        //}

        //internal static string getStringAttribute(XmlNode node, string attrName, string defVal)
        //{
        //    XmlAttribute attribute = (node == null) ? null : node.Attributes[attrName];
        //    if (null != attribute)
        //    {
        //        return attribute.InnerText;
        //    }
        //    return defVal;
        //}
    }
}

