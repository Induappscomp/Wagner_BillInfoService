namespace TradingBell.WebCat.EasyAsk
{
    using System;

    public interface IRemoteEasyAsk
    {
        IOptions getOptions();
        void setOptions(IOptions val);
        INavigateResults userAttributeClick(string path, string attr);
        INavigateResults userAttributeClick_Brand(string path, string attr);
        INavigateResults userBreadCrumbClick(string path);
        //INavigateResults userBreadCrumbClick1(string path);
        INavigateResults userCategoryClick(string path, string cat);
        INavigateResults userCategoryClick(string path);
        INavigateResults userGoToPage(string path, string page);
        INavigateResults userPageOp(string path, string curPage, string pageOp);
        INavigateResults userSearch(string path, string question);
    }
}

