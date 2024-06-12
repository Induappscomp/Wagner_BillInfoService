namespace TradingBell.WebCat.EasyAsk
{
    using System;

    public interface IOptions
    {
        string getCallOutParam();
        string getDictionary();
        string getGrouping();
        bool getNavigateHierarchy();
        int getResultsPerPage();
        bool getReturnSKUs();
        string getSortOrder();
        bool getSubCategories();
        bool getToplevelProducts();
        void setCallOutParam(string val);
        void setDictionary(string name);
        void setGrouping(string val);
        void setNavigateHierarchy(bool val);
        void setResultsPerPage(int val);
        void setResultsPerPage(string val);
        void setReturnSKUs(bool val);
        void setSortOrder(string val);
        void setSubCategories(bool val);
        void setToplevelProducts(bool val);
    }
}

