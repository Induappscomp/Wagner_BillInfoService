namespace TradingBell.WebCat.EasyAsk
{
    using System;
    using System.Collections.Generic;

    public interface IGroupedResult
    {
        IList<INavigateCategory> getAllCategoryDetails();
        IList<string> getAttributeNames();
        int getAttributeValueCount(string attrName, string attrValue);
        IList<string> getCategories();
        string getCellData(int row, int col);
        INavigateCategory getDetailedSuggestedCategory();
        int getEndRow();
        string getGroupValue();
        IResultRow getItem(int row);
        int getNumberOfRows();
        int getStartRow();
        string getSuggestedCategoryID();
        string getSuggestedCategoryTitle();
        int getTotalNumberOfRows();
    }
}

