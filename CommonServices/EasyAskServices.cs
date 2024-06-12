using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers ;
using System.Data;
using System.Data.SqlClient;

namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    public class EasyAskServices
    {
        /*********************************** DECLARATION ***********************************/
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperDB objHelperDb = new HelperDB();
        //string _tempstring = "";
        DataTable objDataTable = new DataTable();
        /*********************************** DECLARATION ***********************************/
        //public int GetPriceCode(string userid)
        //{
        //    int _return = -1;
        //    try
        //    {
        //        _tempstring = (string)objHelperDb.GetGenericDataDB(userid, "PROCE_CODE", HelperDB.ReturnType.RTString);
        //        if (_tempstring != null && _tempstring != "")
        //            _return = Convert.ToInt32(_tempstring);
        //    }
        //    catch (Exception objException)
        //    {
        //        objErrorHandler.ErrorMsg = objException;
        //        objErrorHandler.CreateLog();
        //        return -1;
        //    }
        //    finally
        //    {

        //    }           
        //    return _return;
        //}

        
    }

    /*********************************** J TECH CODE ***********************************/
}
