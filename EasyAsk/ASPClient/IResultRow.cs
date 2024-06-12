namespace TradingBell.WebCat.EasyAsk
{
    using System;

    public interface IResultRow
    {
        string getCellData(int col);
        int size();
    }
}

