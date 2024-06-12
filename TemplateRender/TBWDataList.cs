using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;

namespace TradingBell.WebCat.TemplateRender
{
    /*********************************** J TECH CODE ***********************************/
    public class TBWDataList
    {
        string _TBWDataListItem;
        public TBWDataList(string TBWDataListItem)
        {
            this._TBWDataListItem = TBWDataListItem;
        }
        public string TBWDataListItem { get { return _TBWDataListItem; } }
    }
    /*********************************** CLASS DECLARATION *****************************/
    public class TBWDataList1
    {
        string _TBWDataListItem1;
        public TBWDataList1(string TBWDataListItem1)
        {
            this._TBWDataListItem1 = TBWDataListItem1;
        }
        public string TBWDataListItem1 { get { return _TBWDataListItem1; } }
    }
    public class TBWDataList2
    {
        string _TBWDataListItem2;
        public TBWDataList2(string TBWDataListItem2)
        {
            this._TBWDataListItem2 = TBWDataListItem2;
        }
        public string TBWDataListItem2 { get { return _TBWDataListItem2; } }
    }
    /*********************************** CLASS DECLARATION *****************************/

    /*********************************** J TECH CODE ***********************************/
}
