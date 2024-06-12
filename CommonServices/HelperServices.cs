using System;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.IO;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CatalogDB;
using System.Globalization;
using System.Text.RegularExpressions;

using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using System.Net;
namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    public class HelperServices
    {
        /*********************************** DECLARATION ***********************************/
        ErrorHandler objErrorHandler = new ErrorHandler();
     HelperDB objHelper = new HelperDB();
        string _tempstring = "";
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"];
        Regex _wordRegex = new Regex(@"[^0-9a-zA-Z]+", RegexOptions.Compiled);
        /*********************************** DECLARATION ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO STORE THR MAIL LOG INTO THE MAIL LOG TABLE  ***/
        /********************************************************************************/
        public int Mail_Log(string mail_type, int order_id, string mailto, string mail_error)
        {
            try
            {
                //string sSQL = " INSERT INTO TBWC_ORDER(USER_ID,ORDER_STATUS,IS_SHIPPED,ORDER_EMAIL_SENT,ORDER_INVOICE_SENT,CREATED_USER,MODIFIED_USER)";
                //sSQL = sSQL + " VALUES( " + oInfo.UserID + ",1,0,0,0," + oInfo.UserID + "," + oInfo.UserID + " )";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"];

                string sSQL;
                sSQL = "exec STP_TBWC_POP_MAIL_LOG ";
                sSQL = sSQL + websiteid + ",'" + mail_type + "'," + order_id + ",'" + mailto + "','" + mail_error + "'";
                return objHelper.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }

        }


        public int Mail_Error_Log(string mail_type, int order_id, string mailto, string mail_error,int resend,int user_id,int user_role,int is_site)
        {
           try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"];
                string sSQL;
                sSQL = "exec STP_TBWC_POP_MAIL_ERROR_LOG ";
                sSQL = sSQL + websiteid + ",'" + mail_type + "'," + order_id + ",'" + mailto + "','" + mail_error.Replace("'","") + "'," + resend + "," + user_id +"," + user_role + ","+is_site+"";
                return objHelper.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }


        public int User_Browse_Item(int product_id, int family_id,int user_id,string family_name,string desc,string short_desc,string img_url,string url,string cost)
        {
            try
            {
                //string websiteid = ConfigurationManager.AppSettings["WEBSITEID"];
                string sSQL;
                sSQL = "exec STP_TBWC_POP_USER_BROWSE_ITEM ";
                sSQL = sSQL + product_id + "," + family_id + "," + user_id + ",'" + family_name + "','" + desc + "','" + short_desc + "','" + img_url + "','" + url + "','"+cost+"'";
                return objHelper.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        public DataTable CheckUserBI()
        {
            try
            {
                return (DataTable)objHelper.GetGenericDataDB("", "", "GET_USERID_CHECK_BROWSE_ITEM", HelperDB.ReturnType.RTTable);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        public DataSet GetBrowseItem(string user_id)
        {
            try
            {
                return (DataSet)objHelper.GetGenericDataDB("", user_id, "GET_BROWSE_ITEM_SINGLE_USER", HelperDB.ReturnType.RTDataSet);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        public DataSet CheckUserBrowseItem(string product_id, string family_id, string user_id)
        {
            try
            {
                return (DataSet)objHelper.GetGenericDataDB("", product_id, family_id, user_id, "GET_USER_BROWSE_ITEM", HelperDB.ReturnType.RTDataSet);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        public string DeleteUserBrowseItem(int product_id, int family_id, string user_id)
        {
            try
            {
                return (string)objHelper.GetGenericDataDB("", product_id.ToString(), family_id.ToString(), user_id, "DELETE_USER_BROWSE_ITEM", HelperDB.ReturnType.RTString);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return "-1";

            }
        }


    


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE OPTION NAME VALUES  ***/
        /********************************************************************************/
        public string GetSupplierOptionValues(string SupplierID, string oName)
        {
            try
            {
                DataSet objoptName_all;
                bool isfirst = false;
                // first Attempt
                _tempstring = "";
                objoptName_all = GetSupplierOptionValuesAll(false);
                if (objoptName_all != null)
                {
                    DataRow[] dr = objoptName_all.Tables[0].Select("SUPPLIER_OPTION='" + oName + "' And SUPPLIER_CATEGORY_ID='" + SupplierID + "'");
                    if (dr.Length > 0)
                    {
                        _tempstring = dr.CopyToDataTable().Rows[0]["SUPPLIER_VALUE"].ToString();
                        isfirst = true;
                    }
                }
                // second Attempt
                if (!(isfirst))
                {
                    objoptName_all = GetSupplierOptionValuesAll(true);
                    if (objoptName_all != null)
                    {
                        DataRow[] dr = objoptName_all.Tables[0].Select("SUPPLIER_OPTION='" + oName + "' And SUPPLIER_CATEGORY_ID='" + SupplierID + "'");
                        if (dr.Length > 0)
                        {
                            _tempstring = dr.CopyToDataTable().Rows[0]["SUPPLIER_VALUE"].ToString();
                        }
                    }
                }

                //_tempstring = (string)objHelper.GetGenericDataDB(oName, "OPTION_NAME", HelperDB.ReturnType.RTString);

                return _tempstring;
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return "";
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE OPTION NAME VALUES  ***/
        /********************************************************************************/
        public string GetOptionValues(string oName)
        {

            if (oName != "COURIER CHARGE")
            {
                try
                {
                    DataSet objoptName_all;
                    bool isfirst = false;
                    // first Attempt
                    _tempstring = "";
                    objoptName_all = GetOptionValuesAll(false);
                    if (objoptName_all != null)
                    {
                        DataRow[] dr = objoptName_all.Tables[0].Select("OPTION_NAME='" + oName + "'");
                        if (dr.Length > 0)
                        {
                            _tempstring = dr.CopyToDataTable().Rows[0]["OPTION_VALUE"].ToString();
                            isfirst = true;
                        }
                    }
                    // second Attempt
                    if (!(isfirst))
                    {
                        objoptName_all = GetOptionValuesAll(true);
                        if (objoptName_all != null)
                        {
                            DataRow[] dr = objoptName_all.Tables[0].Select("OPTION_NAME='" + oName + "'");
                            if (dr.Length > 0)
                            {
                                _tempstring = dr.CopyToDataTable().Rows[0]["OPTION_VALUE"].ToString();
                            }
                        }
                    }

                    //_tempstring = (string)objHelper.GetGenericDataDB(oName, "OPTION_NAME", HelperDB.ReturnType.RTString);

                    return _tempstring;
                }
                catch (Exception objException)
                {
                    objErrorHandler.ErrorMsg = objException;
                    objErrorHandler.CreateLog();
                    return "";
                }
             
            }
            else
            {
                  string price="0";
                HelperDB objHelperDB = new HelperDB();
                decimal x = objHelperDB.GetProductPrice_Exc(83480, 1, "999");
                //if (x == 0) {                                                           //comment the line after testing
                //    x = 9.00;
                //}
                price = x.ToString();
              return price;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO SET THE FOLDER PATH FOR PRODUCT IMAGES  ***/
        /********************************************************************************/
        //public string SetImageFolderPath_old(string SourcePath, string FindString, string ConvertTo)
        //{
        //    try
        //    {
        //        SourcePath = SourcePath.ToLower();
        //        FindString = FindString.ToLower();
        //        ConvertTo = ConvertTo.ToLower();
        //        string returnval = "";
        //        string tempstr = "";
        //        string tempstr1 = "";
        //        string strfile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
        //        if (ConvertTo.ToLower() != "_images")
        //        {
        //            if (SourcePath.Contains(FindString) == true)
        //                returnval = SourcePath.Replace(FindString, ConvertTo);
        //            else
        //            {
        //                string[] temp = SourcePath.Split(new string[] { "/" }, StringSplitOptions.None);
        //                if (temp.Length >= 2)
        //                {
        //                    tempstr = temp[temp.Length - 2] + ConvertTo;
        //                    //returnval = SourcePath.Replace(temp[temp.Length - 2].ToString(), temp[temp.Length - 2].ToString() + ConvertTo);
        //                    temp[temp.Length - 2] = temp[temp.Length - 2].ToString() + ConvertTo;
        //                    for (int inx = 0; inx <= temp.Length - 1; inx++)
        //                    {
        //                        if (temp[inx] == "")
        //                            returnval = returnval + "";
        //                        else
        //                            returnval = returnval + "/" + temp[inx];

        //                    }
        //                }
        //                else
        //                    returnval = SourcePath;
        //            }
        //        }
        //        else
        //        {

        //            tempstr = strfile.Replace("\\", "/") + SourcePath.Replace(FindString, "");
        //            tempstr1 = strfile.Replace("\\", "/") + SourcePath.Replace(FindString, ConvertTo);

        //            if (File.Exists(tempstr))
        //            {
        //                returnval = SourcePath.Replace(FindString, "");
        //            }
        //            else if (File.Exists(tempstr1))
        //            {
        //                returnval = SourcePath.Replace(FindString, ConvertTo);
        //            }
        //            else
        //                returnval = SourcePath;

        //        }

        //        return returnval;
        //    }
        //    catch (Exception objException)
        //    {
        //        objErrorHandler.ErrorMsg = objException;
        //        objErrorHandler.CreateLog();
        //        return null;
        //    }
        //}

        public string SetImageFolderPath(string SourcePath, string FindString, string ConvertTo)
        {
            try
            {

                if (SourcePath.ToLower().Contains("noimage"))
                    return SourcePath;

                SourcePath = SourcePath.ToLower().Replace("\\", "/").Replace(@"\", "/");
                FindString = FindString.ToLower();
                if (FindString == "_th")
                {
                    FindString = "_images";
                }
                ConvertTo = ConvertTo.ToLower();
                string returnval = string.Empty;
                string tempstr = string.Empty;
                string tempstr1 = string.Empty;
                string strfile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
                if (ConvertTo.ToLower() != "_images")
                {
                    if ((SourcePath.Contains(FindString)))
                    {
                        // returnval = SourcePath.Replace(FindString, ConvertTo);
                        string[] temp1 = SourcePath.Split(new string[] { "/" }, StringSplitOptions.None);
                        if (temp1.Length >= 2)
                        {
                            if ((temp1[temp1.Length - 2].EndsWith(FindString)))
                                temp1[temp1.Length - 2] = temp1[temp1.Length - 2].Replace(FindString, ConvertTo);

                            else
                                temp1[temp1.Length - 2] = temp1[temp1.Length - 2].ToString() + ConvertTo;


                            for (int inx = 0; inx <= temp1.Length - 1; inx++)
                            {
                                if (temp1[inx] == "")
                                    returnval = returnval + "";
                                else
                                    returnval = returnval + "/" + temp1[inx];

                            }
                        }
                        else
                            returnval = SourcePath;

                    }
                    else
                    {
                        string[] temp = SourcePath.Split(new string[] { "/" }, StringSplitOptions.None);
                        if (temp.Length >= 2)
                        {
                            tempstr = temp[temp.Length - 2] + ConvertTo;
                            //returnval = SourcePath.Replace(temp[temp.Length - 2].ToString(), temp[temp.Length - 2].ToString() + ConvertTo);
                            temp[temp.Length - 2] = temp[temp.Length - 2].ToString() + ConvertTo;
                            for (int inx = 0; inx <= temp.Length - 1; inx++)
                            {
                                if (temp[inx] == "")
                                    returnval = returnval + "";
                                else
                                    returnval = returnval + "/" + temp[inx];

                            }
                        }
                        else
                            returnval = SourcePath;
                    }
                }
                else
                {

                    tempstr = strfile.Replace("\\", "/") + SourcePath.Replace(FindString, "");
                    tempstr1 = strfile.Replace("\\", "/") + SourcePath.Replace(FindString, ConvertTo);

                    if (File.Exists(tempstr))
                    {
                        returnval = SourcePath.Replace(FindString, "");
                    }
                    else if (File.Exists(tempstr1))
                    {
                        returnval = SourcePath.Replace(FindString, ConvertTo);
                    }
                    else
                        returnval = SourcePath;

                }
                returnval = returnval.Replace("_th_th", "_th");
                return returnval;
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ALL THE Supplier OPTION VALUES  ***/
        /********************************************************************************/
        public DataSet GetSupplierOptionValuesAll(bool Reset)
        {
            try
            {
                DataSet objoptName_all;
                if (Reset == true)
                {
                    objoptName_all = (DataSet)objHelper.GetGenericDataDB("", "SUPPLIER_OPTION_NAME_ALL", HelperDB.ReturnType.RTDataSet);
                    HttpContext.Current.Session["SUPPLIER_OPTION_NAME_ALL"] = objoptName_all;
                }
                else
                {
                    if (HttpContext.Current.Session["SUPPLIER_OPTION_NAME_ALL"] == null)
                    {
                        objoptName_all = (DataSet)objHelper.GetGenericDataDB("", "SUPPLIER_OPTION_NAME_ALL", HelperDB.ReturnType.RTDataSet);
                        HttpContext.Current.Session["SUPPLIER_OPTION_NAME_ALL"] = objoptName_all;
                    }
                    else
                        objoptName_all = (DataSet)HttpContext.Current.Session["SUPPLIER_OPTION_NAME_ALL"];
                }

                return objoptName_all;
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ALL THE OPTION VALUES  ***/
        /********************************************************************************/
        public DataSet GetOptionValuesAll(bool Reset)
        {
            try
            {
                DataSet objoptName_all;
                if (Reset == true)
                {
                    objoptName_all = (DataSet)objHelper.GetGenericDataDB("", "OPTION_NAME_ALL", HelperDB.ReturnType.RTDataSet);
                    HttpContext.Current.Session["OPTION_NAME_ALL"] = objoptName_all;
                }
                else
                {
                    if (HttpContext.Current.Session["OPTION_NAME_ALL"] == null)
                    {
                        objoptName_all = (DataSet)objHelper.GetGenericDataDB("", "OPTION_NAME_ALL", HelperDB.ReturnType.RTDataSet);
                        HttpContext.Current.Session["OPTION_NAME_ALL"] = objoptName_all;
                    }
                    else
                        objoptName_all = (DataSet)HttpContext.Current.Session["OPTION_NAME_ALL"];
                }

                return objoptName_all;
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public bool GetIsEcomEnabled(string userid)
        //{
        //    bool retvalue = false;
        //    DataTable objDt = new DataTable();
        //    try
        //    {

        //        //string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //        if (!string.IsNullOrEmpty(userid))
        //        {
        //            string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //            //string sSQL = "SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
        //            //objHelperService.SQLString = sSQL;
        //            //int iROLE = objHelperService.CI(objHelperService.GetValue("USER_ROLE"));
        //            int iROLE = 0;
        //            if (HttpContext.Current.Session["ECOM_LOGIN_COMP"] == null)
        //            {
        //                objDt = (DataTable)objHelper.GetGenericDataDB(WesCatalogId, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
        //                if (objDt != null && objDt.Rows.Count > 0)
        //                {
        //                    HttpContext.Current.Session["ECOM_LOGIN_COMP"] = objDt;
        //                    iROLE = CI(objDt.Rows[0]["USER_ROLE"].ToString());
        //                }
        //            }
        //            else
        //            {
        //                objDt = (DataTable)HttpContext.Current.Session["ECOM_LOGIN_COMP"];
        //                if (objDt != null && objDt.Rows.Count > 0)
        //                {
        //                    iROLE = CI(objDt.Rows[0]["USER_ROLE"].ToString());
        //                }
        //            }
        //            if (iROLE <= 3)
        //                retvalue = true;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //    }
        //    finally
        //    {
        //        objDt.Dispose();
        //        objDt = null;

        //    }
        //    return retvalue;
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER THE ECOM OPTION IS ENABLED OR NOT  ***/
        /********************************************************************************/
        public bool GetIsEcomEnabled(string userid)
        {
            bool retvalue = false;
            DataTable objDt = new DataTable();
            try
            {
                if (userid == string.Empty)
                    userid = "0";

                //string userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (!string.IsNullOrEmpty(userid) && Convert.ToInt32(userid) != 0)
                {
                    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"];
                    //string sSQL = "SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
                    //objHelperService.SQLString = sSQL;
                    //int iROLE = objHelperService.CI(objHelperService.GetValue("USER_ROLE"));
                    int iROLE = 0;
                    if (HttpContext.Current.Session["ECOM_LOGIN_COMP"] == null)
                    {
                        objDt = (DataTable)objHelper.GetGenericDataDB(WesCatalogId, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                        if (objDt != null && objDt.Rows.Count > 0)
                        {
                            HttpContext.Current.Session["ECOM_LOGIN_COMP"] = objDt;
                            iROLE = CI(objDt.Rows[0]["USER_ROLE"].ToString());
                        }
                    }
                    else
                    {
                        objDt = (DataTable)HttpContext.Current.Session["ECOM_LOGIN_COMP"];
                        if (objDt != null && objDt.Rows.Count > 0)
                        {
                            iROLE = CI(objDt.Rows[0]["USER_ROLE"].ToString());
                        }
                    }
                    if (iROLE <= 3)
                        retvalue = true;
                    else
                    {
                        if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString() == "Retailer")
                            retvalue = true;
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            finally
            {
                if (objDt != null)
                    objDt.Dispose();
                objDt = null;

            }
            return retvalue;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE POWER SEARCH PRODUCT DETAILS  ***/
        /********************************************************************************/
        //public DataSet GetPowerSearchProducts(string Searchtxt, int price_code, int user_id)
        //{
        //    return objHelper.GetPowerSearchProducts(Searchtxt, price_code, user_id);
        //}


        /*********************************** OLD CODE ***********************************/
        //public string connection()
        //{
        //    try
        //    {
        //        ConnectionDB objConnection = new ConnectionDB();

        //        return objConnection.ConnectionString.Replace("\\", @"\");
        //    }
        //    catch (Exception objException)
        //    {
        //        objErrorHandler.ErrorMsg = objException;
        //        objErrorHandler.CreateLog();
        //        return "";
        //    }
        //}
        /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO TRIM  ***/
        /********************************************************************************/
        //public string StringTrim(string sValue, int Trimlen, bool trimspace)
        //{
        //    try
        //    {
        //        string sReturnValue = "";
        //        sValue = sValue.Replace(" ", "");

        //        if (sValue.Length > Trimlen)
        //            sReturnValue = sValue.Substring(0, Trimlen) + "...";
        //        else
        //            sReturnValue = sValue;

        //        return sReturnValue;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        return "";
        //    }
        //}


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE UPDATED VALUES  ***/
        /********************************************************************************/
        public string Prepare(string sValue)
        {
            try
            {
                string sReturnValue;
                sReturnValue = sValue.Replace("'", " ");
                return sReturnValue;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return "";
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CONVERT THE OBJECT INTO STRING  ***/
        /********************************************************************************/
        public string CS(object obj)
        {
            try
            {
                string sRetValue = "";
                if (obj != null && obj != DBNull.Value)
                {
                    sRetValue = Convert.ToString(obj);
                }
                return sRetValue;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return "";
            }
        }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO SET OBJECT VALUE TO STRING ***/
        /********************************************************************************/
        public string FixDecPlace(decimal obj)
        {
            try
            {
                string sRetValue = null;
                sRetValue = obj.ToString("N2");
                return sRetValue;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return "";
            }
        }
        /// <summary>
        ///  This is used to convert the object into integer
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>integer</returns>
        /// <example>
        /// <code>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO COVERT THE OBJECT VALUE INTO INTEGER VALUE  ***/
        /********************************************************************************/
        public int CI(object obj)
        {
            int retValue = 0;
            try
            {
                
                if (obj != null && obj != DBNull.Value && obj.ToString() != "")
                {
                    retValue = Convert.ToInt32(obj);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog("error field" + obj);
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        /// <summary>
        ///  This is used to convert the object into Long Integer
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>Int64</returns>
        /// <example>
        /// <code>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO COVERT THE OBJECT VALUE INTO LONG INTEGER VALUE  ***/
        /********************************************************************************/
        public Int64 CLI(object obj)
        {
            try
            {
                Int64 retValue = 0;
                if (obj != null && obj != DBNull.Value && obj.ToString() != "")
                {
                    retValue = Convert.ToInt64(obj);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        /// <summary>
        ///  This is used to convert the object into Double
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>double</returns>
        /// <example>
        /// <code>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO COVERT THE OBJECT VALUE INTO DOUBLE VALUE  ***/
        /********************************************************************************/
        public double CD(object obj)
        {
            try
            {
                double retValue = 0.0;
                if (obj != null && obj != DBNull.Value)
                {
                    retValue = Convert.ToDouble(obj);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        /// <summary>
        ///  This is used to convert the object into Decimal Values
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>decimal</returns>
        /// <example>
        /// <code>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO COVERT THE OBJECT VALUE INTO DECIMAL VALUE  ***/
        /********************************************************************************/
        public decimal CDEC(object obj)
        {
            try
            {
                decimal retValue = 0;
                if (obj != null && obj != DBNull.Value && obj.ToString() != "")
                {
                    retValue = Convert.ToDecimal(obj);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        /// <summary>
        ///  This is used to convert the object into Bool Value(T/F)
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>integer</returns> 
        /// <example>
        /// <code>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO COVERT THE OBJECT VALUE INTO BOOLEAN VALUE  ***/
        /********************************************************************************/
        public int CB(object obj)
        {
            try
            {
                int retValue = 0;
                if ((bool)obj)
                {
                    retValue = 1;
                }
                return retValue;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK THE EMAIL ID IS VALID OR NOT  ***/
        /********************************************************************************/
        public bool ValidateEmail(string emailAddress)
        {

            Regex regex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            Match match = regex.Match(emailAddress);
            if (match.Success)
                return true;
            else
                return false;
        }

   
        public void writelog(string newurl)
        {
            try
            {
                System.IO.StreamWriter log;
                //  string path = HttpContext.Current.Server.MapPath("log_palani.txt");

                log = System.IO.File.AppendText(HttpContext.Current.Server.MapPath("logfile_2.txt"));
                // log = System.IO.File.AppendText("C:\\WES WEBSITE\\WAGNER1.0\\logfile.txt");


                // log = System.IO.File.AppendText(path);
                log.WriteLine(newurl);
                log.Close();

                //System.IO.StreamWriter log;


                //log = System.IO.File.AppendText("C:\\WES WEBSITE\\Wiretek\\logfile_2.txt");
                //// log = System.IO.File.AppendText("C:\\WES WEBSITE\\WAGNER1.0\\testlog\\logfile_2.txt");
                //// log = System.IO.File.AppendText("E:\\CatalogStudio\\Wagner_2_0\\WAGNER\\logfile_2.txt");
                //log.WriteLine(newurl);
                //log.Close();
            }
            catch (Exception ex)
            {

            }
        }
        public string viewebook(string ebookpath)
        {
            try
            {

                string ebookpath_updated = ebookpath;
                if (ebookpath.Contains("\\attachments"))
                {
                    ebookpath_updated = ebookpath.Replace("\\attachments", "");
                }

                else
                {
                    ebookpath = "\\attachments" + ebookpath;
                }
                if (ebookpath.Contains(".htm") || ebookpath.Contains(".html"))
                {
                    return ebookpath_updated;
                }
                else
                {

                    string folderPath = HttpContext.Current.Server.MapPath(ebookpath);   // or whatever folder you want to load..



                    var htmlFiles = new DirectoryInfo(folderPath).GetFiles("*.html");


                    if (htmlFiles.Length > 0)
                    {
                        var firsthtmlFilename = htmlFiles[0].Name;
                        ebookpath_updated = ebookpath_updated.Replace("\\", "/") + "/" + firsthtmlFilename;
                        return ebookpath_updated;
                    }

                    else
                    {
                        var htmFiles = new DirectoryInfo(folderPath).GetFiles("*.htm");
                        if (htmFiles.Length > 0)
                        {
                            var firsthtmFilename = htmFiles[0].Name;
                            ebookpath_updated = ebookpath_updated.Replace("\\", "/") + "/" + firsthtmFilename;
                            return ebookpath_updated;
                        }
                    }

                }
                return ebookpath;
            }
            catch
            {
                return ebookpath;
            }
        }
        public string CheckPriceValueDecimal(string cost)
        {
            string[] price = null;
            try
            {
                price = cost.ToString().Split('.');

                if (price.Length > 1)
                {
                    if (price[1].Length >= 4)
                    {
                        if ((price.Length > 0 && Convert.ToInt32(price[1].Substring(2, 2)) > 1))
                        {
                            cost = cost.ToString();
                        }
                        else
                        {
                            cost = Convert.ToDouble(cost).ToString("#0.00");
                        }

                    }
                }

                else
                {
                    cost = Convert.ToDouble(cost)+".00";
                }

            }
            catch (Exception ex)
            {


            }
            return cost;
        }

     
        public string CheckCredential()
        {
            bool returnvalue = false;
            bool validUser;
            string username;
            string password;
            string login;
            string userID = "";
            int UserID;
            TradingBell.WebCat.Helpers.Security objSecurity = new TradingBell.WebCat.Helpers.Security();
            UserServices objUserServices = new UserServices();
            //TradingBell.WebCat.Helpers.ErrorHandler objErrorHandler = new TradingBell.WebCat.Helpers.ErrorHandler();
            CompanyGroupServices objCompanyGroupServices = new CompanyGroupServices();

            if (HttpContext.Current.Request.Cookies["LoginInfoCL"] != null && HttpContext.Current.Request.Cookies["LoginInfoCL"].Value.ToString().Trim() != "")
            {
                try
                {
                    login = "False";
                    username = "";
                    password = "";
                    HttpCookie LoginInfoCookie = HttpContext.Current.Request.Cookies["LoginInfoCL"];

                    if (LoginInfoCookie["UserName"] != null)
                        username = objSecurity.StringDeCrypt(LoginInfoCookie["UserName"].ToString());

                    if (LoginInfoCookie["Password"] != null)
                        password = objSecurity.StringDeCrypt(LoginInfoCookie["Password"].ToString());

                    if (LoginInfoCookie["Login"] != null)
                        login = objSecurity.StringDeCrypt(LoginInfoCookie["Login"].ToString());

                    validUser = objUserServices.CheckUserName(username);
                    UserID = objUserServices.GetUserID(username);
                    if (UserID != -1 && username != string.Empty && login == "True")
                    {
                        if (objCompanyGroupServices.CheckCompanyStatus(UserID) == CompanyGroupServices.CompanyStatus.ACTIVE.ToString())
                        {
                            if (validUser == true)
                            {
                                bool HasAdminUser = objUserServices.HasAdmin(UserID);
                                if (objUserServices.IsUserActive(UserID.ToString()))
                                {
                                    password = objSecurity.StringEnCrypt_password(password);
                                    if (objUserServices.CheckUser(username, password))
                                    {
                                        string Role;
                                        Role = objUserServices.GetRole(UserID);

                                        if (Role != null)
                                        {
                                            returnvalue = true;
                                            objUserServices.OnLineFlag(true, UserID);
                                            // HttpContext.Current.Session["USER_NAME"] = username;
                                            userID = UserID.ToString();
                                            HttpContext.Current.Session["USER_ID"] = UserID;
                                            // HttpContext.Current.Session["USER_ROLE"] = Role;
                                            //  HttpContext.Current.Session["COMPANY_ID"] = objUserServices.GetCompanyID(UserID);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        HttpContext.Current.Session["USER_ID"] = "";
                        userID = "0";

                    }

                }
                catch (Exception ex)
                {
                    userID = "0";
                    // objErrorHandler.ErrorMsg = ex;
                    // objErrorHandler.CreateLog();
                }

            }
            else
            {
                //HttpContext.Current.Session["USER_NAME"] = "";
                HttpContext.Current.Session["USER_ID"] = "";
                //HttpContext.Current.Session["USER_ROLE"] = "0";
                //HttpContext.Current.Session["COMPANY_ID"] = "";
                userID = "0";
            }



            return userID;

        }
        public string MetaTagProductkeyword(DataTable dt)
        {
            string strkeyword = string.Empty;
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                if (i == 0)
                {
                    strkeyword = dt.Rows[i][0].ToString();
                }
                else
                {
                    strkeyword = strkeyword + "," + dt.Rows[i]["Product Tags"].ToString();
                }
            }
            GetmetadataFromEA objgetmetadata = new GetmetadataFromEA();
            strkeyword = objgetmetadata.Replace_SpecialChar(strkeyword);
            return strkeyword;

        }
  
        public string StripWhitespace(string body)
        {
            string html = body;
            //string[] lines = body.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            //StringBuilder emptyLines = new StringBuilder();
            //foreach (string line in lines)
            //{
            //    string s = line.Trim();
            //    if (s.Length > 0 && !s.StartsWith("//"))
            //        emptyLines.AppendLine(s.Trim());
            //}

            //body = emptyLines.ToString();

            //// remove C styles comments
            //body = Regex.Replace(body, "/\\*.*?\\*/", String.Empty, RegexOptions.Compiled | RegexOptions.Singleline);
            ////// trim left
            //body = Regex.Replace(body, "^\\s*", String.Empty, RegexOptions.Compiled | RegexOptions.Multiline);
            ////// trim right
            //body = Regex.Replace(body, "\\s*[\\r\\n]", "\r\n", RegexOptions.Compiled | RegexOptions.ECMAScript);
            //// remove whitespace beside of left curly braced
            //body = Regex.Replace(body, "\\s*{\\s*", "{", RegexOptions.Compiled | RegexOptions.ECMAScript);
            //// remove whitespace beside of coma
            //body = Regex.Replace(body, "\\s*,\\s*", ",", RegexOptions.Compiled | RegexOptions.ECMAScript);
            //// remove whitespace beside of semicolon
            //body = Regex.Replace(body, "\\s*;\\s*", ";", RegexOptions.Compiled | RegexOptions.ECMAScript);
            //// remove newline after keywords
            //body = Regex.Replace(body, "\\r\\n(?<=\\b(abstract|boolean|break|byte|case|catch|char|class|const|continue|default|delete|do|double|else|extends|false|final|finally|float|for|function|goto|if|implements|import|in|instanceof|int|interface|long|native|new|null|package|private|protected|public|return|short|static|super|switch|synchronized|this|throw|throws|transient|true|try|typeof|var|void|while|with)\\r\\n)", " ", RegexOptions.Compiled | RegexOptions.ECMAScript);
            //body = Regex.Replace(body, "[\\r\\n]", "", RegexOptions.Compiled | RegexOptions.ECMAScript);

            /// Solution A
            //html = Regex.Replace(html, @"\n|\t", " ");
            //html = Regex.Replace(html, @">\s+<", "><").Trim();
            //html = Regex.Replace(html, @"\s{2,}", " ");

            ///// Solution B
            //html = Regex.Replace(html, @"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}", "");
            //html = Regex.Replace(html, @"[ \f\r\t\v]?([\n\xFE\xFF/{}[\];,<>*%&|^!~?:=])[\f\r\t\v]?", "$1");
            //html = html.Replace(";\n", ";");

            ///// Solution C
            //html = Regex.Replace(html, @"[a-zA-Z]+#", "#");
            //html = Regex.Replace(html, @"[\n\r]+\s*", string.Empty);
            //html = Regex.Replace(html, @"\s+", " ");
            //html = Regex.Replace(html, @"\s?([:,;{}])\s?", "$1");
            //html = html.Replace(";}", "}");
            //html = Regex.Replace(html, @"([\s:]0)(px|pt|%|em)", "$1");

            ///// Remove comments
            //html = Regex.Replace(html, @"/\*[\d\D]*?\*/", string.Empty);

            // Regex reg = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}|\<\!--[^\[].*?--\>", RegexOptions.Compiled);
            // html = reg.Replace(html, string.Empty);
            return html;
        }
        public string SimpleURL_Attr(StringTemplate _stmpl_records, string EAPATH, string AttributeValue, string FormName, string AttrID, string AttributeType, bool noconcat)
        {


            string ORGURL = string.Empty;
            string stlistprod = string.Empty;

            string newformname = FormName.ToUpper();
            try
            {
                if (noconcat == false)
                {
                    string strEApath = EAPATH.Replace("AttribSelect=", "");

                    string[] ConsURL = strEApath.Split(new string[] { "////" }, StringSplitOptions.None);
                    ConsURL = ConsURL.Where(c => c.Trim() != "").ToArray();


                    if (ConsURL.Length >= 2)
                    {
                        if (ConsURL[0].ToLower().Contains("allproducts") || ConsURL[0].ToLower().Contains("all products"))
                            ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                        if (ConsURL[0].ToUpper().Contains("WESAUSTRALASIA"))
                            ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();

                    }



                    for (int i = 0; i <= ConsURL.Length - 1; i++)
                    {
                        if (i == 0)
                        {

                            stlistprod = URLstringreplace_Simple(ConsURL[i]);
                        }
                        else
                        {


                            stlistprod = URLstringreplace_Simple(ConsURL[i]) + "/" + stlistprod;

                        }
                    }
                }
                else
                {
                    stlistprod = EAPATH.Replace("////", "/");
                }
                // AddDomainname()
                if (AttrID != "")
                {
                    if (AttrID != "384")
                    {
                        stlistprod = "/" + URLstringreplace_Simple(AttributeValue) + "_" + AttrID + "/" + stlistprod.Replace("//", "/");
                    }
                    else 
                    {
                        stlistprod = "/" + URLstringreplace_Simple(AttributeValue) + "_" +  "m/" + stlistprod.Replace("//", "/");
                    }
}
                else
                {

                    if (AttributeType == "BRAND")
                    {
                        stlistprod = "/" + URLstringreplace_Simple(AttributeValue) + "_" + "b/" + stlistprod.Replace("//", "/");
                    }

                    else if (AttributeType == "MODEL")
                    {
                        stlistprod = "/" + URLstringreplace_Simple(AttributeValue) + "_" + "m/" + stlistprod.Replace("//", "/");
                    }
                    else if (AttributeType == "PRODUCT TAGS")
                    {
                        stlistprod = "/" + URLstringreplace_Simple(AttributeValue) + "_" + "p/" + stlistprod.Replace("//", "/");
                    }
                    else if (AttributeType == "PRICE RANGE")
                    {
                        stlistprod = "/" + URLstringreplace_Simple(AttributeValue) + "_" + "pr/" + stlistprod.Replace("//", "/");
                    }

                    else
                    {

                        stlistprod = "/" + URLstringreplace_Simple(AttributeValue) + "/" + stlistprod.Replace("//", "/");
                    }

                }
                if (FormName.Contains("-"))
                {
                    string[] FormNamenew = FormName.Split('-');
                    string formname = FormNamenew[1];
                    if (!(formname.ToLower().Contains("home.aspx")))
                    {

                        formname = formname.Replace(".aspx", "");

                        if (FormNamenew[0] == "BCRemoveurl")
                            //FormNamenew[1] + "?"
                            _stmpl_records.SetAttribute("TBT_REMOVEREWRITEURL", (stlistprod + "/" + formname + "/"));
                        else
                            //FormNamenew[1] + "?" + 
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", (stlistprod + "/" + formname + "/"));

                    }
                    else
                    {
                        if (FormNamenew[0] == "BCRemoveurl")
                            //FormNamenew[1] + "?"
                            _stmpl_records.SetAttribute("TBT_REMOVEREWRITEURL", "Home.aspx");
                        else
                            //FormNamenew[1] + "?" + 
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", "Home.aspx");

                    }

                }

                else
                {

                    _stmpl_records.SetAttribute("TBT_REWRITEURL", stlistprod.Replace("//", "/"));



                }


                return stlistprod;
            }
            catch (Exception ex)
            {
                // objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl");

                return ORGURL;
            }


        }
        public string SimpleURL(StringTemplate _stmpl_records, string EAPATH, string FormName)
        {


            string ORGURL = string.Empty;
            string newformname = FormName.ToUpper();
            try
            {

                string strEApath = EAPATH.Replace("AttribSelect=", "");

                string[] ConsURL = strEApath.Split(new string[] { "////" }, StringSplitOptions.None);
                ConsURL = ConsURL.Where(c => c.Trim() != "").ToArray();


                if (ConsURL.Length >= 2)
                {
                    if (ConsURL[0].ToLower().Contains("allproducts") || ConsURL[0].ToLower().Contains("all products"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                    if (ConsURL[0].ToUpper().Contains("WESAUSTRALASIA"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();

                }


                string stlistprod = string.Empty;

                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 0)
                    {

                        stlistprod = URLstringreplace_Simple(ConsURL[i]);
                    }
                    else
                    {


                        stlistprod = URLstringreplace_Simple(ConsURL[i]) + "/" + stlistprod;

                    }
                }
                stlistprod = AddDomainname() + stlistprod.Replace("//", "/");
                if (FormName.Contains("-"))
                {
                    string[] FormNamenew = FormName.Split('-');
                    string formname = FormNamenew[1];
                    if (!(formname.ToLower().Contains("home.aspx")))
                    {

                        formname = formname.Replace(".aspx", "");

                        if (FormNamenew[0] == "BCRemoveurl")
                            //FormNamenew[1] + "?"
                            _stmpl_records.SetAttribute("TBT_REMOVEREWRITEURL", (stlistprod + "/" + formname + "/"));
                        else
                            //FormNamenew[1] + "?" + 
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", (stlistprod + "/" + formname + "/"));

                    }
                    else
                    {
                        if (FormNamenew[0] == "BCRemoveurl")
                            //FormNamenew[1] + "?"
                            _stmpl_records.SetAttribute("TBT_REMOVEREWRITEURL", "Home.aspx");
                        else
                            //FormNamenew[1] + "?" + 
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", "Home.aspx");

                    }

                }

                else
                {

                    _stmpl_records.SetAttribute("TBT_REWRITEURL", stlistprod);



                }

              
                return stlistprod;
            }
            catch (Exception ex)
            {
               // objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl");

                return ORGURL;
            }


        }
        public string SimpleURL_Str(string EAPATH, string FormName,bool withhttp)
        {


            string ORGURL = string.Empty;
            string newformname = FormName.ToUpper();
            try
            {

                string strEApath = EAPATH.Replace("AttribSelect=", "");

                string[] ConsURL = strEApath.Split(new string[] { "////" }, StringSplitOptions.None);
                ConsURL = ConsURL.Where(c => c.Trim() != "").ToArray();


                if (ConsURL.Length >= 2)
                {
                    if (ConsURL[0].ToLower().Contains("allproducts") || ConsURL[0].ToLower().Contains("all products"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                    if (ConsURL[0].ToUpper().Contains("WESAUSTRALASIA"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();

                }


                string stlistprod = string.Empty;

                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 0)
                    {

                        stlistprod = URLstringreplace_Simple(ConsURL[i]);
                    }
                    else
                    {


                        stlistprod = URLstringreplace_Simple(ConsURL[i]) + "/" + stlistprod;

                    }
                }
                if (withhttp == true)
                {
                    stlistprod = AddDomainname() + stlistprod;
                    return stlistprod;
                }
                else
                {
                    return stlistprod;
                }
            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl");

                return ORGURL;
            }


        }
        public string SimpleURL_MS(StringTemplate _stmpl_records, string EAPATH, string FormName,bool ado)
        {


            string ORGURL = string.Empty;
            string newformname = FormName.ToUpper();
            try
            {

                string strEApath = EAPATH.Replace("AttribSelect=", "");

                string[] ConsURL = strEApath.Split(new string[] { "////" }, StringSplitOptions.None);
                ConsURL = ConsURL.Where(c => c.Trim() != "").ToArray();


                if (ConsURL.Length >= 2)
                {
                    if (ConsURL[0].ToLower().Contains("allproducts") || ConsURL[0].ToLower().Contains("all products"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                    if (ConsURL[0].ToUpper().Contains("WESAUSTRALASIA"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();

                }


                string stlistprod = string.Empty;

                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 0)
                    {

                        stlistprod = URLstringreplace_Simple(ConsURL[i]);
                    }
                    else
                    {

                        stlistprod = stlistprod + "/" + URLstringreplace_Simple(ConsURL[i]);

                    }
                }
                stlistprod =stlistprod.Replace("//", "/");
                 if (stlistprod.StartsWith("/") == false)
                {

                    stlistprod = "/" + stlistprod;
                }
                if (FormName.Contains("-"))
                {
                    string[] FormNamenew = FormName.Split('-');
                    string formname = FormNamenew[1];
                    if (!(formname.ToLower().Contains("home.aspx")))
                    {

                        formname = formname.Replace(".aspx", "");

                        if (FormNamenew[0] == "BCRemoveurl")
                            //FormNamenew[1] + "?"
                            _stmpl_records.SetAttribute("TBT_REMOVEREWRITEURL", (stlistprod + "/" + formname + "/"));
                        else
                            //FormNamenew[1] + "?" + 
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", (stlistprod + "/" + formname + "/"));

                    }
                    else
                    {
                        if (FormNamenew[0] == "BCRemoveurl")
                            //FormNamenew[1] + "?"
                            _stmpl_records.SetAttribute("TBT_REMOVEREWRITEURL", "Home.aspx");
                        else
                            //FormNamenew[1] + "?" + 
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", "Home.aspx");

                    }

                }

                else
                {

                    _stmpl_records.SetAttribute("TBT_REWRITEURL", stlistprod);



                }

               
                return stlistprod;
            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl");

                return ORGURL;
            }


        }
        public string SimpleURL_MS(StringTemplate _stmpl_records, string EAPATH, string FormName)
        {


            string ORGURL = string.Empty;
            string newformname = FormName.ToUpper();
            try
            {

                string strEApath = EAPATH.Replace("AttribSelect=", "");

                string[] ConsURL = strEApath.Split(new string[] { "////" }, StringSplitOptions.None);
                ConsURL = ConsURL.Where(c => c.Trim() != "").ToArray();


                if (ConsURL.Length >= 2)
                {
                    if (ConsURL[0].ToLower().Contains("allproducts") || ConsURL[0].ToLower().Contains("all products"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                    if (ConsURL[0].ToUpper().Contains("WESAUSTRALASIA"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                   
                }


                string stlistprod = string.Empty;

                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 0)
                    {
                      
                        stlistprod = URLstringreplace_Simple(ConsURL[i]);
                    }
                    else
                    {

                        stlistprod = stlistprod + "/" + URLstringreplace_Simple(ConsURL[i]);

                    }
                }
                stlistprod = AddDomainname() + stlistprod.Replace("//", "/");
                if (FormName.Contains("-"))
                {
                    string[] FormNamenew = FormName.Split('-');
                    string formname = FormNamenew[1];
                    if (!(formname.ToLower().Contains("home.aspx")))
                    {

                        formname = formname.Replace(".aspx", "");

                        if (FormNamenew[0] == "BCRemoveurl")
                            //FormNamenew[1] + "?"
                            _stmpl_records.SetAttribute("TBT_REMOVEREWRITEURL", (stlistprod + "/" + formname + "/"));
                        else
                            //FormNamenew[1] + "?" + 
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", (stlistprod + "/" + formname + "/"));

                    }
                    else
                    {
                        if (FormNamenew[0] == "BCRemoveurl")
                            //FormNamenew[1] + "?"
                            _stmpl_records.SetAttribute("TBT_REMOVEREWRITEURL", "Home.aspx");
                        else
                            //FormNamenew[1] + "?" + 
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", "Home.aspx");

                    }

                }

                else
                {

                    _stmpl_records.SetAttribute("TBT_REWRITEURL", stlistprod);



                }
                return stlistprod;
            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl");

                return ORGURL;
            }


        }
        public string SimpleURL_MS_Str( string EAPATH, string FormName,bool withhttp)
        {


            string ORGURL = string.Empty;
            string newformname = FormName.ToUpper();
            try
            {

                string strEApath = EAPATH.Replace("AttribSelect=", "");

                string[] ConsURL = strEApath.Split(new string[] { "////" }, StringSplitOptions.None);
                ConsURL = ConsURL.Where(c => c.Trim() != "").ToArray();


                if (ConsURL.Length >= 2)
                {
                    if (ConsURL[0].ToLower().Contains("allproducts") || ConsURL[0].ToLower().Contains("all products"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                    if (ConsURL[0].ToUpper().Contains("WESAUSTRALASIA"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();

                }


                string stlistprod = string.Empty;

                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 0)
                    {

                        stlistprod = URLstringreplace_Simple(ConsURL[i]);
                    }
                    else
                    {


                        stlistprod = stlistprod + "/" + URLstringreplace_Simple(ConsURL[i]);

                    }
                }
                if (withhttp == true)
                {
                    stlistprod = AddDomainname() + stlistprod;
                    return stlistprod;
                }
                else
                {
                    return stlistprod;
                }
              
            }
            catch (Exception ex)
            {
               // objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl");

                return ORGURL;
            }


        }


        //public string checkspecialchar_express(string value)
        //{
        //    TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        //    Regex _wordRegex = new Regex(@"[^0-9a-zA-Z *.#$%^&!]+", RegexOptions.Compiled);
        //    string Itemnew = _wordRegex.Replace(value, "");
        //    if (Itemnew.Contains("-") == false)
        //    {
        //        return textInfo.ToTitleCase(value.ToLower());
        //    }
        //    else
        //    {
        //        return value;
        //    }
        //}

        public string checkspecialchar(string value)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            Regex _wordRegex = new Regex(@"[^0-9a-zA-Z *.#$%^&!]+", RegexOptions.Compiled);
            string Itemnew = _wordRegex.Replace(value, "-");
            if (Itemnew.Contains("-") == false)
            {
                return textInfo.ToTitleCase(value.ToLower());
            }
            else
            {
                return value;
            }
        }
        public string URLstringreplace_Simple(string skeyword)
        {
            //skeyword = "USB-mA fe Female";
            //This key is the logic of special character replacement(eg:909)
            string repkey = string.Empty;
            //This key is the logic of =(eg:Brand=)Moved to end
            string equalskey = string.Empty;
            try
            {

                skeyword = HttpUtility.UrlDecode(skeyword).Trim().ToLower();
                if ((skeyword.Contains('=')))
                {
                    string[] splitkeyword = skeyword.Split(new string[] { "=" }, StringSplitOptions.None);
                    skeyword = splitkeyword[1];
                    //Keywords such as product tag space is replaced and - is also replaced
                    equalskey = splitkeyword[0].Replace("-", "~-").Replace(' ', '~').Replace("%", "-~");
                    equalskey = "-" + equalskey;
                }
               
                 skeyword = _wordRegex.Replace(skeyword, "-");
                skeyword = skeyword.ToString();

                if (skeyword.EndsWith("-"))
                {

                    skeyword = skeyword.Substring(0, skeyword.Length - 1);    
                }
                if (skeyword.StartsWith("-"))
                {

                    skeyword = skeyword.Substring(1, skeyword.Length - 1);
                }

                return skeyword + equalskey.Replace("-category", "");
            }
            catch
            {
                return skeyword;

            }
        }

        //public string Change_MPD_PD(string StrRawurl)
        //{
        //    string[] mainstrrawurl = StrRawurl.Split(new string[] { "/" }, StringSplitOptions.None);
        //    string url=string.Empty;
        //    try
        //    {
        //        if (mainstrrawurl.Length == 8)
        //        {
        //            url = mainstrrawurl[3] + "/" + mainstrrawurl[1] + "/" + mainstrrawurl[2] + "/" + mainstrrawurl[4] + "/" + mainstrrawurl[5] + "/" + "pd" + "/";
        //        }
        //        else if (mainstrrawurl.Length == 7)
        //        {
        //            url = mainstrrawurl[2] + "/" + mainstrrawurl[1] + "/" + mainstrrawurl[3] + "/" + mainstrrawurl[4] + "/" + mainstrrawurl[5] + "/" + "pd" + "/";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.CreateLog(ex.ToString());  
        //    }
        //    return url;
        //}



        public string URlStringReverse(string StrRawurl)
        {
            //if (StrRawurl.ToString().EndsWith("/") == false)
            //{
            //    StrRawurl = StrRawurl + "/";
            //}
            //try
            //{

            //    if ((!(StrRawurl.EndsWith("/ct/"))) && (StrRawurl.Contains("/ct/")))
            //    {

            //        string[] sturl = StrRawurl.Split(new string[] { "/ct/" }, StringSplitOptions.None);
            //        StrRawurl ="/"+ sturl[0] + "/ct/";
            //        HttpContext.Current.Response.RedirectPermanent(StrRawurl);
            //    }

            //    else if ((StrRawurl.Contains("/bb/")) && (!(StrRawurl.EndsWith("/bb/"))))
            //    {



            //        string[] sturl = StrRawurl.Split(new string[] { "/bb/" }, StringSplitOptions.None);
            //        StrRawurl = "/" + sturl[0] + "/bb/";
            //        HttpContext.Current.Response.RedirectPermanent(StrRawurl);

            //    }
            //    else if ((StrRawurl.Contains("/ps/")) && (!(StrRawurl.EndsWith("/ps/"))))
            //    {



            //        string[] sturl = StrRawurl.Split(new string[] { "/ps/" }, StringSplitOptions.None);
            //        StrRawurl = "/" + sturl[0] + "/ps/";

            //        HttpContext.Current.Response.RedirectPermanent(StrRawurl);
            //    }
            //    else if ((StrRawurl.Contains("/fl/")) && (!(StrRawurl.EndsWith("/fl/"))))
            //    {


            //        string[] sturl = StrRawurl.Split(new string[] { "/fl/" }, StringSplitOptions.None);
            //        StrRawurl = "/" + sturl[0] + "/fl/";

            //        HttpContext.Current.Response.RedirectPermanent(StrRawurl);
            //    }
            //    else if ((StrRawurl.Contains("/pd/")) && (!(StrRawurl.EndsWith("/pd/"))))
            //    {


            //        string[] sturl = StrRawurl.Split(new string[] { "/pd/" }, StringSplitOptions.None);
            //        StrRawurl = "/" + sturl[0] + "/pd/";

            //        HttpContext.Current.Response.RedirectPermanent(StrRawurl);
            //    }
            //    else if ((StrRawurl.Contains("/pl/")) && (!(StrRawurl.EndsWith("/pl/"))))
            //    {



            //        string[] sturl = StrRawurl.Split(new string[] { "/pl/" }, StringSplitOptions.None);
            //        StrRawurl = "/" + sturl[0] + "/pl/";

            //        HttpContext.Current.Response.RedirectPermanent(StrRawurl);
            //    }

            //}
            //catch (System.Threading.ThreadAbortException)
            //{
            //    // ignore it
            //}
            //catch (Exception ex)
            //{
            //    // ignore it
            //}











            string formname = string.Empty;
            string ynewurl = string.Empty;
            try
            {
                if (StrRawurl.EndsWith("/") == true)
                {
                    StrRawurl = StrRawurl.Substring(0, StrRawurl.Length - 1);
                }
                if (StrRawurl.EndsWith(".") == true)
                {
                    StrRawurl = StrRawurl.Substring(0, StrRawurl.Length - 1);
                }
                StrRawurl = StrRawurl.Replace(".~", ".");
                string[] mainstrrawurl = StrRawurl.Split(new string[] { "xx" }, StringSplitOptions.None);

                string mainurl = string.Empty;
                if (mainstrrawurl.Length > 2)
                {
                    string[] getformname = mainstrrawurl[mainstrrawurl.Length - 1].Split(new string[] { "/" }, StringSplitOptions.None);
                    formname = getformname[getformname.Length - 1];
                    string main_url = string.Empty;
                    for (int i = 0; i <= mainstrrawurl.Length - 2; i++)
                    {
                        main_url = main_url + mainstrrawurl[i];
                    }


                    mainurl = main_url + "/" + formname;
                }
                else if (mainstrrawurl.Length > 1)
                {
                    string[] getformname = mainstrrawurl[1].Split(new string[] { "/" }, StringSplitOptions.None);
                    formname = getformname[getformname.Length - 1];
                    mainurl = mainstrrawurl[0] + "/" + formname;
                }

                else
                {
                    mainurl = mainstrrawurl[0];
                }


                string[] strrawurl = mainurl.Split(new string[] { "/" }, StringSplitOptions.None);
                string NewRawUrl = string.Empty;
                string urlkey = string.Empty;
                string[] surlkey = null;
                int j = 0;
                for (int i = strrawurl.Length - 1; i > 0; i--)
                {


                    if (i == strrawurl.Length - 1)
                    {
                        NewRawUrl = strrawurl[i];
                    }
                    if (i == strrawurl.Length - 2)
                    {
                        urlkey = strrawurl[i];
                        surlkey = strrawurl[i].Split('-');
                        //for (int r = 0; r > 0; r--)
                        //{ 

                        //}

                    }
                    if (i < strrawurl.Length - 2)
                    {
                        string strurl = strrawurl[i];
                        bool ishypen_st = false;
                        if (strrawurl[i].StartsWith("-"))
                        {
                            strurl = strurl.Substring(1, strurl.Length - 1);
                            ishypen_st = true;
                        }
                        string[] hypenstr = strurl.Split('-');
                        string removestrkey = surlkey[j];
                        StringBuilder newSBskeyword = new StringBuilder(removestrkey);
                        // newSBskeyword.Replace("9", "");

                        newSBskeyword.Replace("a0", "_");
                        newSBskeyword.Replace("a1", ":");
                        newSBskeyword.Replace("a2", "\"");
                        newSBskeyword.Replace("a3", "+");
                        newSBskeyword.Replace("a4", "(");
                        newSBskeyword.Replace("a5", ")");
                        newSBskeyword.Replace("a6", "?");
                        newSBskeyword.Replace("a7", "“");
                        newSBskeyword.Replace("a8", "”");
                        newSBskeyword.Replace("a9", "™");
                        newSBskeyword.Replace("b1", "*");
                        newSBskeyword.Replace("b2", "?");

                        newSBskeyword.Replace("b3", "°");
                        newSBskeyword.Replace("b4", "®");
                        newSBskeyword.Replace("b5", "©");
                        newSBskeyword.Replace("b6", "€");
                        newSBskeyword.Replace("b7", "«");
                        //newSBskeyword.Replace("b8", "µ");
                        newSBskeyword.Replace("b9", "±");
                        newSBskeyword.Replace("c1", "²");
                        newSBskeyword.Replace("c2", "¾");
                        newSBskeyword.Replace("c3", "¼");
                        newSBskeyword.Replace("c4", "Ø");
                        newSBskeyword.Replace("c5", "Þ");
                        newSBskeyword.Replace("c6", "ß");
                        newSBskeyword.Replace("c7", "ç");

                        newSBskeyword.Replace("c8", "²");









                        newSBskeyword.Replace("0", " ");
                        newSBskeyword.Replace("1", "-");
                        newSBskeyword.Replace("2", "=");
                        newSBskeyword.Replace("3", "#");
                        // newSBskeyword.Replace("4", "(");
                        newSBskeyword.Replace("5", "%");
                        newSBskeyword.Replace("6", "/");
                        newSBskeyword.Replace("7", "&");
                        newSBskeyword.Replace("8", "\\");
                        string[] snewSBskeyword = newSBskeyword.ToString().Split('9');
                        string orgstring = string.Empty;
                        //replaced string is converted to original string
                        for (int k = 0; k <= hypenstr.Length - 1; k++)
                        {
                            if (k == 0)
                            {
                                if (ishypen_st == true)
                                {
                                    orgstring = snewSBskeyword[k] + hypenstr[k];
                                }
                                else
                                {
                                    orgstring = hypenstr[k];
                                }
                            }
                            else
                            {
                                orgstring = orgstring + snewSBskeyword[k] + hypenstr[k];
                            }
                        }
                        //if (orgstring.Contains('=') == true)
                        //{
                        //    string[] splitkeyword = orgstring.Split(new string[] { "=" }, StringSplitOptions.None);
                        //    orgstring = splitkeyword[1] + "=" + splitkeyword[0];
                        //}
                        //orgstring = orgstring.Replace("~”~", "”");  
                        orgstring.Replace("?", "<ars>g</ars>");
                        orgstring.Replace("?", "&rarr;");
                        orgstring.Replace("?", "rarr;");
                        if (orgstring.Contains('/') == true)
                        {
                            orgstring = orgstring.Replace("/", "~~");
                        }
                        NewRawUrl = NewRawUrl + "/" + orgstring;
                        j = j + 1;
                    }
                }
                if (NewRawUrl.StartsWith("/"))
                {
                    NewRawUrl = NewRawUrl.Substring(1, NewRawUrl.Length - 1);
                }
                string[] mainnewurl = NewRawUrl.Split('/');
                string newmainnewurl = string.Empty;

                if (mainstrrawurl.Length > 1)
                {
                    string[] equalssplit = mainstrrawurl[mainstrrawurl.Length - 1].Split(new string[] { "/" }, StringSplitOptions.None);

                    for (int y = 1; y <= mainnewurl.Length - 1; y++)
                    {

                        for (int i = 0; i <= equalssplit.Length - 1; i++)
                        {

                            string[] hypensplit = equalssplit[i].Replace(".~", ".").Replace("~-", "~_").Replace("-~", "%").Replace("~", " ").Split(new string[] { "-" }, StringSplitOptions.None);
                            if (hypensplit.Length > 1)
                            {
                                newmainnewurl = "";
                                if (y == 1)
                                {
                                    newmainnewurl = mainnewurl[1];
                                }
                                if (y == 1 && Convert.ToInt32(hypensplit[0]) == y - 1)
                                {
                                    newmainnewurl = hypensplit[1].Replace("~_", "-").Replace("_~", "%").Replace("~", " ") + "=" + newmainnewurl;
                                    break;
                                }
                                else if (Convert.ToInt32(hypensplit[0]) == y - 1)
                                {
                                    newmainnewurl = hypensplit[1].Replace("~_", "-").Replace("~", " ").Replace("_~", "%") + "=" + mainnewurl[y];
                                    break;
                                }
                                else
                                {
                                    newmainnewurl = mainnewurl[y];
                                }

                            }


                        }

                        if (y == 0)
                        {
                            ynewurl = newmainnewurl;
                        }
                        else
                        {
                            ynewurl = ynewurl + "/" + newmainnewurl;
                        }
                    }
                }
                else
                {
                    ynewurl = NewRawUrl;
                }
                return formname + ynewurl.ToString();
            }
            catch
            {
                // HttpContext.Current.Response.Redirect("/404New.htm");
                string newstrrawurl = string.Empty;
                if (StrRawurl.StartsWith("/") == true)
                {
                    StrRawurl = StrRawurl.Substring(1, StrRawurl.Length - 1);
                }
                if (StrRawurl.Contains("/ct/"))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/ct/" }, StringSplitOptions.None);


                    newstrrawurl = "/ct" + sturl[0];
                }
                else if (StrRawurl.Contains("/bb/"))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/bb/" }, StringSplitOptions.None);
                    newstrrawurl = "/bb" + sturl[0];
                }
                else if (StrRawurl.Contains("/ps/"))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/ps/" }, StringSplitOptions.None);
                    newstrrawurl = "/ps" + sturl[0];
                }
                else if (StrRawurl.Contains("/fl/"))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/fl/" }, StringSplitOptions.None);
                    newstrrawurl = "/fl" + sturl[0];
                }
                else if (StrRawurl.Contains("/pd/"))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/pd/" }, StringSplitOptions.None);
                    newstrrawurl = "/pd" + sturl[0];
                }
                else if (StrRawurl.Contains("/pl/"))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/pl/" }, StringSplitOptions.None);
                    newstrrawurl = "/pl" + sturl[0];
                }
                else
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/" }, StringSplitOptions.None);
                    string strvalue = SimpleURL_Str( sturl[0], "ps.aspx",true);
                    strvalue = "/" + strvalue + "/ps/";
                    HttpContext.Current.Response.RedirectPermanent(strvalue,false);
                }
                return newstrrawurl;
            }
        }
        public string URlStringReverse_MS(string StrRawurl)
        {
            //if (StrRawurl.ToString().EndsWith("/") == false)
            //{
            //    StrRawurl = StrRawurl + "/";
            //}




            string formname = string.Empty;
            string ynewurl = string.Empty;
            try
            {
                if (StrRawurl.EndsWith("/") == true)
                {
                    StrRawurl = StrRawurl.Substring(0, StrRawurl.Length - 1);
                }
                if (StrRawurl.EndsWith(".") == true)
                {
                    StrRawurl = StrRawurl.Substring(0, StrRawurl.Length - 1);
                }
                StrRawurl = StrRawurl.Replace(".~", ".");
                string[] mainstrrawurl = StrRawurl.Split(new string[] { "xx" }, StringSplitOptions.None);

                string mainurl = string.Empty;
                if (mainstrrawurl.Length > 2)
                {
                    string[] getformname = mainstrrawurl[mainstrrawurl.Length - 1].Split(new string[] { "/" }, StringSplitOptions.None);
                    formname = getformname[getformname.Length - 1];
                    string main_url = string.Empty;
                    for (int i = 0; i <= mainstrrawurl.Length - 2; i++)
                    {
                        main_url = main_url + mainstrrawurl[i];
                    }


                    mainurl = main_url + "/" + formname;
                }
                else if (mainstrrawurl.Length > 1)
                {
                    string[] getformname = mainstrrawurl[1].Split(new string[] { "/" }, StringSplitOptions.None);
                    formname = getformname[getformname.Length - 1];
                    mainurl = mainstrrawurl[0] + "/" + formname;
                }
                else if (mainstrrawurl.Length == 1)
                {
                    string[] getformname = mainstrrawurl[0].Split(new string[] { "/" }, StringSplitOptions.None);
                    formname = getformname[getformname.Length - 1];
                    mainurl = mainstrrawurl[0];
                }
                else
                {
                    mainurl = mainstrrawurl[0];
                }


                string[] strrawurl = mainurl.Split(new string[] { "/" }, StringSplitOptions.None);
                string NewRawUrl = string.Empty;
                string urlkey = string.Empty;
                string[] surlkey = null;
                int j = 0;
                for (int i = 1; i < strrawurl.Length; i++)
                {

                    if (i == 1)
                    {

                        // NewRawUrl = strrawurl[strrawurl.Length - 1];
                        urlkey = strrawurl[strrawurl.Length - 2];
                        surlkey = strrawurl[strrawurl.Length - 2].Split('-');
                    }


                    //for (int r = 0; r > 0; r--)
                    //{ 

                    //}


                    if (i < strrawurl.Length - 2)
                    {
                        string strurl = strrawurl[i];

                        bool ishypen_st = false;
                        if (strrawurl[i].StartsWith("-"))
                        {
                            strurl = strurl.Substring(1, strurl.Length - 1);
                            ishypen_st = true;
                        }
                        string[] hypenstr = null;
                        if (strurl != "")
                        {
                            hypenstr = strurl.Split('-');
                        }




                        string removestrkey = surlkey[j];
                        StringBuilder newSBskeyword = new StringBuilder(removestrkey);
                        // newSBskeyword.Replace("9", "");

                        newSBskeyword.Replace("a0", "_");
                        newSBskeyword.Replace("a1", ":");
                        newSBskeyword.Replace("a2", "\"");
                        newSBskeyword.Replace("a3", "+");
                        newSBskeyword.Replace("a4", "(");
                        newSBskeyword.Replace("a5", ")");
                        newSBskeyword.Replace("a6", "?");
                        newSBskeyword.Replace("a7", "“");
                        newSBskeyword.Replace("a8", "”");
                        newSBskeyword.Replace("a9", "™");
                        newSBskeyword.Replace("b1", "*");
                        newSBskeyword.Replace("b2", "?");
                        newSBskeyword.Replace("0", " ");
                        newSBskeyword.Replace("1", "-");
                        newSBskeyword.Replace("2", "=");
                        newSBskeyword.Replace("3", "#");
                        // newSBskeyword.Replace("4", "(");
                        newSBskeyword.Replace("5", "%");
                        newSBskeyword.Replace("6", "/");
                        newSBskeyword.Replace("7", "&");
                        newSBskeyword.Replace("8", "\\");
                        string[] snewSBskeyword = newSBskeyword.ToString().Split('9');
                        string orgstring = string.Empty;
                        //replaced string is converted to original string
                        for (int k = 0; k <= hypenstr.Length - 1; k++)
                        {
                            if (k == 0)
                            {
                                if (ishypen_st == true)
                                {
                                    orgstring = snewSBskeyword[k] + hypenstr[k];
                                }
                                else
                                {
                                    orgstring = hypenstr[k];
                                }
                            }
                            else
                            {
                                orgstring = orgstring + snewSBskeyword[k] + hypenstr[k];
                            }
                        }
                        //if (orgstring.Contains('=') == true)
                        //{
                        //    string[] splitkeyword = orgstring.Split(new string[] { "=" }, StringSplitOptions.None);
                        //    orgstring = splitkeyword[1] + "=" + splitkeyword[0];
                        //}
                        //orgstring = orgstring.Replace("~”~", "”");  
                        orgstring.Replace("?", "<ars>g</ars>");
                        orgstring.Replace("?", "&rarr;");
                        orgstring.Replace("?", "rarr;");
                        if (orgstring.Contains('/') == true)
                        {
                            orgstring = orgstring.Replace("/", "~~");
                        }


                        NewRawUrl = NewRawUrl + "/" + orgstring;
                        j = j + 1;
                    }
                }
                if (NewRawUrl.StartsWith("/"))
                {
                    NewRawUrl = NewRawUrl.Substring(1, NewRawUrl.Length - 1);
                }
                string[] mainnewurl = NewRawUrl.Split('/');
                string newmainnewurl = string.Empty;

                if (mainstrrawurl.Length > 1)
                {
                    string[] equalssplit = mainstrrawurl[mainstrrawurl.Length - 1].Split(new string[] { "/" }, StringSplitOptions.None);

                    for (int y = 0; y <= mainnewurl.Length - 1; y++)
                    {

                        for (int i = 0; i <= equalssplit.Length - 1; i++)
                        {

                            string[] hypensplit = equalssplit[i].Replace(".~", ".").Replace("~-", "~_").Replace("-~", "%").Replace("~", " ").Split(new string[] { "-" }, StringSplitOptions.None);
                            if (hypensplit.Length > 1)
                            {
                                newmainnewurl = "";
                                //if (y == 1)
                                //{
                                //    newmainnewurl = mainnewurl[1];
                                //}
                                //y == 1 && 
                                if (Convert.ToInt32(hypensplit[0]) == y)
                                {
                                    newmainnewurl = hypensplit[1].Replace("~_", "-").Replace("_~", "%").Replace("~", " ") + "=" + mainnewurl[y];
                                    break;
                                }
                                //else if (Convert.ToInt32(hypensplit[0]) == y - 1)
                                //{
                                //    newmainnewurl = hypensplit[1].Replace("~_", "-").Replace("~", " ").Replace("_~", "%") + "=" + mainnewurl[y];
                                //    break;
                                //}
                                else
                                {
                                    newmainnewurl = mainnewurl[y];
                                }

                            }


                        }

                        if (y == 0)
                        {
                            ynewurl = newmainnewurl;
                        }
                        else
                        {
                            ynewurl = ynewurl + "/" + newmainnewurl;
                        }
                    }
                }
                else
                {
                    ynewurl = NewRawUrl;
                }
                return formname + "/" + ynewurl.ToString();
            }
            catch
            {
                // HttpContext.Current.Response.Redirect("/404New.htm");
                string newstrrawurl = string.Empty;
                if (StrRawurl.StartsWith("/") == true)
                {
                    StrRawurl = StrRawurl.Substring(1, StrRawurl.Length - 1);
                }
                if (StrRawurl.Contains("/ct/"))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/ct/" }, StringSplitOptions.None);


                    newstrrawurl = "/ct" + sturl[0];
                }
                else if (StrRawurl.Contains("/bb/"))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/bb/" }, StringSplitOptions.None);
                    newstrrawurl = "/bb" + sturl[0];
                }
                else if (StrRawurl.Contains("/ps/"))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/ps/" }, StringSplitOptions.None);
                    newstrrawurl = "/ps" + sturl[0];
                }
                else if (StrRawurl.Contains("/fl/"))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/fl/" }, StringSplitOptions.None);
                    newstrrawurl = "/fl" + sturl[0];
                }
                else if (StrRawurl.Contains("/pd/"))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/pd/" }, StringSplitOptions.None);
                    newstrrawurl = "/pd" + sturl[0];
                }
                else if (StrRawurl.Contains("/pl/"))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/pl/" }, StringSplitOptions.None);
                    newstrrawurl = "/pl" + sturl[0];
                }
                else
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/" }, StringSplitOptions.None);
                    string strvalue = SimpleURL_MS_Str( sturl[0], "ps.aspx",true);
                    strvalue = strvalue + "/ps/";
                    HttpContext.Current.Response.RedirectPermanent(strvalue,false);
                }
                return newstrrawurl;
            }
        }

        public string AddDomainname()
        {
            string stlistprod = string.Empty;
            //try
            //{

            string currenturl = System.Configuration.ConfigurationManager.AppSettings["CssScriptsSDUrl"];
               // stlistprod = currenturl.Replace("https://", "http://");
                return currenturl;

                //if (System.Web.HttpContext.Current.Session != null) //if (HttpContext.Current.Session.Count > 0)
                //{
                //    if (System.Web.HttpContext.Current.Session["USER_ID"] != null
                //        && System.Web.HttpContext.Current.Session["USER_ID"].ToString() != "")
                //    {
                //        if (Convert.ToInt32(System.Web.HttpContext.Current.Session["USER_ID"].ToString()) > 0
                //        && Convert.ToInt32(System.Web.HttpContext.Current.Session["USER_ID"].ToString()) != 999)
                //        {
                //            stlistprod = "/";
                //        }
                //        // stlistprod = "https://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port+"/";
                //    }
                //}
            //}
            //catch
            //{

            //    stlistprod = "/";
            //}
                //else
                //{
                //    stlistprod = currenturl.Replace("https://", "http://");
                //    // stlistprod = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port+"/";
                //}
           // }

        // stlistprod = "http://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port + "/";
            return stlistprod;
        }

        public void microsite_redirect()
        {
            if (HttpContext.Current.Session["SUPPLIER_NAME"] != null)
            {

                string str = SimpleURL_MS_Str(HttpContext.Current.Session["SUPPLIER_NAME"].ToString(), "mct.aspx", false);
               HttpContext.Current.Response.Redirect(str + "/mct/", false);
            }
            else
            {
                HttpContext.Current.Response.Redirect("/home.aspx", false);
            }
        }
        public string ProdimageRreplaceImages(string shtml, string _Package)
        {
            string _RenderedHTML = shtml.ToString().Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

            if (_Package == "NEWPRODUCT" || _Package == "CATEGORYLISTIMG" || _Package == "CSFAMILYPAGE" || _Package == "CSFAMILYPAGEWITHSUBFAMILY" || _Package == "PRODUCT")
            {
                if (shtml.ToString().Contains("data-original=\"/prodimages\""))
                {
                    if (_Package == "CSFAMILYPAGE")
                        _RenderedHTML = shtml.ToString().Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages/images/noimage.gif\"");
                    else
                        _RenderedHTML = shtml.ToString().Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages/images/noimage.gif\"");
                    _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                }
                if (shtml.ToString().Contains("data-original=\"\""))
                {
                    _RenderedHTML = shtml.ToString().Replace("data-original=\"\"", "data-original=\"/prodimages/images/noimage.gif\"");
                    _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                }
            }
            else
            {
                if (shtml.ToString().Contains("src=\"/prodimages\""))
                {
                    if (_Package == "CSFAMILYPAGE")
                        _RenderedHTML = shtml.ToString().Replace("src=\"/prodimages\"", "src=\"/prodimages/images/noimage.gif\" style=\"display:none;\"");
                    else
                        _RenderedHTML = shtml.ToString().Replace("src=\"/prodimages\"", "src=\"/prodimages/images/noimage.gif\"");
                    _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                }
                if (shtml.ToString().Contains("src=\"\""))
                {
                    _RenderedHTML = shtml.ToString().Replace("src=\"\"", "src=\"/prodimages/images/noimage.gif\"");
                    _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                }

            }
            return _RenderedHTML;
        }


        public bool CheckImageExistCDN(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "image/jpeg";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode.ToString() == "OK")
                {
                    return true;
                }
                else
                {
                    return false;
                }
               
            }
            catch (Exception ex)
            {

                return false;
            }
        }

    }

  
    /*********************************** J TECH CODE ***********************************/
}