namespace TradingBell.WebCat.EasyAsk
{
    using System;

    public interface IGroupedResultSet
    {
        int getEndGroup();
        IGroupedResult getGroup(int i);
        string getGroupCriteria();
        int getGroupCriteriaType();
        int getMaximumRowsPerGroup();
        string getNodeString(IGroupedResult group);
        int getNumberOfGroups();
        int getPageCount();
        int getStartGroup();
        int getTotalNumberOfRows();
    }
}

