namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;

    internal class Options : IOptions
    {
        private string m_calloutParam;
        private string m_dictionary;
        private string m_grouping;
        private bool m_navigateHierarcy;
        private int m_resultsPerPage;
        private bool m_returnSKUs;
        private string m_sortOrder;
        private bool m_subCategories;
        private bool m_toplevelProducts;

        internal Options()
        {
            this.m_dictionary = null;
            this.m_navigateHierarcy = false;
            this.m_subCategories = false;
            this.m_toplevelProducts = false;
            this.m_returnSKUs = false;
            this.m_resultsPerPage = -1;
            this.m_sortOrder = null;
            this.m_grouping = null;
            this.m_calloutParam = null;
        }

        internal Options(string dictionary)
        {
            this.m_dictionary = null;
            this.m_navigateHierarcy = false;
            this.m_subCategories = false;
            this.m_toplevelProducts = false;
            this.m_returnSKUs = false;
            this.m_resultsPerPage = -1;
            this.m_sortOrder = null;
            this.m_grouping = null;
            this.m_calloutParam = null;
            this.m_dictionary = dictionary;
        }

        public string getCallOutParam()
        {
            return this.m_calloutParam;
        }

        public string getDictionary()
        {
            return this.m_dictionary;
        }

        public string getGrouping()
        {
            return this.m_grouping;
        }

        public bool getNavigateHierarchy()
        {
            return this.m_navigateHierarcy;
        }

        public int getResultsPerPage()
        {
            return this.m_resultsPerPage;
        }

        public bool getReturnSKUs()
        {
            return this.m_returnSKUs;
        }

        public string getSortOrder()
        {
            return this.m_sortOrder;
        }

        public bool getSubCategories()
        {
            return this.m_subCategories;
        }

        public bool getToplevelProducts()
        {
            return this.m_toplevelProducts;
        }

        public void setCallOutParam(string val)
        {
            this.m_calloutParam = val;
        }

        public void setDictionary(string name)
        {
            this.m_dictionary = name;
        }

        public void setGrouping(string val)
        {
            this.m_grouping = val;
        }

        public void setNavigateHierarchy(bool val)
        {
            this.m_navigateHierarcy = val;
        }

        public void setResultsPerPage(int val)
        {
            this.m_resultsPerPage = val;
        }

        public void setResultsPerPage(string val)
        {
            if ((val != null) && (0 < val.Length))
            {
                try
                {
                    this.setResultsPerPage(int.Parse(val));
                }
                catch (FormatException)
                {
                }
            }
        }

        public void setReturnSKUs(bool val)
        {
            this.m_returnSKUs = val;
        }

        public void setSortOrder(string val)
        {
            this.m_sortOrder = val;
        }

        public void setSubCategories(bool val)
        {
            this.m_subCategories = val;
        }

        public void setToplevelProducts(bool val)
        {
            this.m_toplevelProducts = val;
        }
    }
}

