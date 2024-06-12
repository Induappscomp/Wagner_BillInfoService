namespace TradingBell.WebCat.EasyAsk
{
    using System;

    public interface INavigateAttribute
    {
        bool getDisplayAsLink();
        string getName();
        string getNodeString();
        int getProductCount();
        int getType();
        string getValue();
        string removeFromPath(string path);
    }
}

