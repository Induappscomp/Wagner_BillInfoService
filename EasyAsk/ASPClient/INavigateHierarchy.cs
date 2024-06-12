namespace TradingBell.WebCat.EasyAsk
{
    using System;
    using System.Collections.Generic;

    public interface INavigateHierarchy
    {
        IList<string> getIDs();
        string getName();
        string getPath();
        IList<INavigateHierarchy> getSubNodes();
        bool isSelected();
    }
}

