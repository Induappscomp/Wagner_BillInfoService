using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Security.Cryptography;
using TradingBell.WebCat.Helpers;

namespace TradingBell.WebCat.CatalogDB
{
    #region OLD CODE TRADING BELL
     //ConnectionDB objConnection = new ConnectionDB();
     //   private string sSqlStr;
     //   ErrorHandler objErrorHandler = new ErrorHandler();
     //   SqlCommand objSqlCommand;
     //   DataSet objDataSet = new DataSet();
     //   SqlDataReader objDataR = null;
     //   DataTable objDatatbl = new DataTable();
     //   SqlDataAdapter objDataAdapter;
     //   string ReturnValue = string.Empty;
     //   string WebSiteID = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
     //   public enum ReturnType
     //   {
     //       RTString = 1,
     //       RTTable = 2,
     //       RTDataSet = 3
     //   }
     //   public CountryDB()
     //   {
     //   }
     //   public object GetGenericDataDB(string Param1, string ReturnOption, ReturnType ReturnType)
     //   {
     //       return GetGenericDataDB("", Param1, "", "", "", ReturnOption, ReturnType);
     //   }
     //   public object GetGenericDataDB(string Catalog_ID, string Param1, string ReturnOption, ReturnType ReturnType)
     //   {
     //       return GetGenericDataDB(Catalog_ID, Param1, "", "", "", ReturnOption, ReturnType);
     //   }
     //   public object GetGenericDataDB(string Catalog_ID, string Param1, string Param2, string ReturnOption, ReturnType ReturnType)
     //   {
     //       return GetGenericDataDB(Catalog_ID, Param1, Param2, "", "", ReturnOption, ReturnType);
     //   }
     //   public object GetGenericDataDB(string Catalog_ID, string Param1, string Param2, string Param3, string ReturnOption, ReturnType ReturnType)
     //   {
     //       return GetGenericDataDB(Catalog_ID, Param1, Param2, Param3, "", ReturnOption, ReturnType);
     //   }

     //   public object GetGenericDataDB(string Catalog_ID, string Param1, string Param2, string Param3, string Param4, string ReturnOption, ReturnType ReturnType)
     //   {
     //       object TempReturn = null;
     //       try
     //       {
               
     //           objDataSet = new DataSet();
     //           objDatatbl = new DataTable();
     //           objSqlCommand = new SqlCommand("STP_TBWC_PICK_COUNTRY_DB_DATA", objConnection.GetConnection());
     //           objSqlCommand.CommandType = CommandType.StoredProcedure;
     //           objSqlCommand.Parameters.Add(new SqlParameter("@Catalog_ID", Catalog_ID));
     //           objSqlCommand.Parameters.Add(new SqlParameter("@Param1", Param1));
     //           objSqlCommand.Parameters.Add(new SqlParameter("@Param2", Param2));
     //           objSqlCommand.Parameters.Add(new SqlParameter("@Param3", Param3));
     //           objSqlCommand.Parameters.Add(new SqlParameter("@Param4", Param4));
     //           objSqlCommand.Parameters.Add(new SqlParameter("@ReturnOption", ReturnOption));
     //           objSqlCommand.Parameters.Add(new SqlParameter("@WebSiteID", WebSiteID));

     //           objDataR = objSqlCommand.ExecuteReader();
     //           objDatatbl.Load(objDataR);

     //           if (objDatatbl != null && objDatatbl.Rows.Count > 0)
     //           {
     //               if (ReturnType == CountryDB.ReturnType.RTString)
     //               {
     //                   TempReturn = objDatatbl.Rows[0][0].ToString();
     //               }
     //               else if (ReturnType == CountryDB.ReturnType.RTTable)
     //               {
     //                   TempReturn = objDatatbl;
     //               }
     //               else if (ReturnType == CountryDB.ReturnType.RTDataSet)
     //               {
     //                   objDataSet.Tables.Add(objDatatbl.Copy());
     //                   TempReturn = objDataSet;
     //               }
     //           }
     //           else
     //           {
     //               if (ReturnType == CountryDB.ReturnType.RTString)
     //               {
     //                   TempReturn = "";
     //               }
     //               else if (ReturnType == CountryDB.ReturnType.RTTable)
     //               {
     //                   TempReturn = null;
     //               }
     //               else if (ReturnType == CountryDB.ReturnType.RTDataSet)
     //               {
     //                   TempReturn = null;
     //               }
     //           }

               
               
     //       }
     //       catch (Exception objException)
     //       {
     //           objErrorHandler.ErrorMsg = objException;
     //           objErrorHandler.CreateLog();
     //           return null;

     //       }
     //       finally
     //       {
     //           objConnection.CloseConnection();
     //           objSqlCommand.Dispose();
     //           objSqlCommand = null;
     //           objDataSet.Dispose();
     //           objDataSet = null;
     //           objDatatbl.Dispose();
     //           objDatatbl = null;
     //           objDataR.Close();
                
     //       }
     //       return TempReturn;
     //   }
    #endregion

    /*********************************** J TECH CODE ***********************************/

    public class CountryDB
    {
        /*********************************** DECLARATION ***********************************/
        ConnectionDB objConnection = new ConnectionDB();
        private string sSqlStr;
        ErrorHandler objErrorHandler = new ErrorHandler();
        SqlCommand objSqlCommand;
        DataSet objDataSet = new DataSet();
        SqlDataReader objDataR = null;
        DataTable objDatatbl = new DataTable();
        SqlDataAdapter objDataAdapter;
        string ReturnValue = string.Empty;
        string WebSiteID = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        public enum ReturnType
        {
            RTString = 1,
            RTTable = 2,
            RTDataSet = 3
        }
        /*********************************** DECLARATION ***********************************/

        /*********************************** CONSTRUCTOR ***********************************/
        public CountryDB()
        {
        }
        /*********************************** CONSTRUCTOR ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE COUNTRY RELATED DETAILS FROM DATABASE BASED ON PARAMETERS ***/
        /********************************************************************************/

        public object GetGenericDataDB(string Param1, string ReturnOption, ReturnType ReturnType)
        {
            return GetGenericDataDB("", Param1, "", "", "", ReturnOption, ReturnType);
        }
        public object GetGenericDataDB(string Catalog_ID, string Param1, string ReturnOption, ReturnType ReturnType)
        {
            return GetGenericDataDB(Catalog_ID, Param1, "", "", "", ReturnOption, ReturnType);
        }
        public object GetGenericDataDB(string Catalog_ID, string Param1, string Param2, string ReturnOption, ReturnType ReturnType)
        {
            return GetGenericDataDB(Catalog_ID, Param1, Param2, "", "", ReturnOption, ReturnType);
        }
        public object GetGenericDataDB(string Catalog_ID, string Param1, string Param2, string Param3, string ReturnOption, ReturnType ReturnType)
        {
            return GetGenericDataDB(Catalog_ID, Param1, Param2, Param3, "", ReturnOption, ReturnType);
        }

        public object GetGenericDataDB(string Catalog_ID, string Param1, string Param2, string Param3, string Param4, string ReturnOption, ReturnType ReturnType)
        {
            object TempReturn = null;
            try
            {
               
                objDataSet = new DataSet();
                objDatatbl = new DataTable();
                objSqlCommand = new SqlCommand("STP_TBWC_PICK_COUNTRY_DB_DATA", objConnection.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.Add(new SqlParameter("@Catalog_ID", Catalog_ID));
                objSqlCommand.Parameters.Add(new SqlParameter("@Param1", Param1));
                objSqlCommand.Parameters.Add(new SqlParameter("@Param2", Param2));
                objSqlCommand.Parameters.Add(new SqlParameter("@Param3", Param3));
                objSqlCommand.Parameters.Add(new SqlParameter("@Param4", Param4));
                objSqlCommand.Parameters.Add(new SqlParameter("@ReturnOption", ReturnOption));
                objSqlCommand.Parameters.Add(new SqlParameter("@WebSiteID", WebSiteID));

                objDataR = objSqlCommand.ExecuteReader();
                objDatatbl.Load(objDataR);

                if (objDatatbl != null && objDatatbl.Rows.Count > 0)
                {
                    if (ReturnType.Equals(CountryDB.ReturnType.RTString))
                    {
                        TempReturn = objDatatbl.Rows[0][0].ToString();
                    }
                    else if (ReturnType.Equals(CountryDB.ReturnType.RTTable))
                    {
                        TempReturn = objDatatbl;
                    }
                    else if (ReturnType.Equals(CountryDB.ReturnType.RTDataSet))
                    {
                        objDataSet.Tables.Add(objDatatbl.Copy());
                        TempReturn = objDataSet;
                    }
                }
                else
                {
                    if (ReturnType.Equals(CountryDB.ReturnType.RTString))
                    {
                        TempReturn = "";
                    }
                    else if (ReturnType.Equals(CountryDB.ReturnType.RTTable))
                    {
                        TempReturn = null;
                    }
                    else if (ReturnType.Equals(CountryDB.ReturnType.RTDataSet))
                    {
                        TempReturn = null;
                    }
                }

               
               
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;

            }
            finally
            {
                objConnection.CloseConnection();
                objSqlCommand.Dispose();
                objSqlCommand = null;
                objDataSet.Dispose();
                objDataSet = null;
                objDatatbl.Dispose();
                objDatatbl = null;
                objDataR.Close();
                
            }
            return TempReturn;
        }
    }
    /*********************************** J TECH CODE ***********************************/
}
