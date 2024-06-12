namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class ItemRow : IResultRow
    {
        private List<string> m_items;

        internal ItemRow(IList<IDataDescription> desc, XmlNode item)
        {
            this.m_items = new List<string>(desc.Count);
            foreach (DataDescription description in desc)
            {
                XmlNode node = item.SelectSingleNode(description.getTagName());
                if (null != node)
                {
                    this.m_items.Add(node.FirstChild.InnerText);
                }
                else
                {
                    this.m_items.Add("");
                }
            }
        }

        public string getCellData(int col)
        {
            return this.m_items[col];
        }

        internal string getFormattedText(int col)
        {
            return this.m_items[col];
        }

        public int size()
        {
            return this.m_items.Count;
        }
    }
}

