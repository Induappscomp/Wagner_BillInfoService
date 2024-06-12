namespace TradingBell.WebCat.EasyAsk
{
    using System;

    public interface IDataDescription
    {
        string getColName();
        string getColType();
        bool getDecoded();
        bool getDisplayable();
        string getFormat();
        string getHTMLType();
        string getTagName();
    }
}

