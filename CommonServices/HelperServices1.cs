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
namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    public class HelperServices
    {
        /*********************************** DECLARATION ***********************************/      
        ErrorHandler objErrorHandler = new ErrorHandler();
        CatalogDB.HelperDB objHelper = new CatalogDB.HelperDB();        
        string _tempstring = "";
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        /*********************************** DECLARATION ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE OPTION NAME VALUES  ***/
        /********************************************************************************/
        public string GetSupplierOptionValues(string SupplierID,string oName)
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
                    DataRow[] dr = objoptName_all.Tables[0].Select("SUPPLIER_OPTION='" + oName + "' And SUPPLIER_CATEGORY_ID='" + SupplierID  + "'");
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
            try
            {   DataSet objoptName_all;
                bool isfirst=false;
                // first Attempt
                _tempstring = "";
                objoptName_all = GetOptionValuesAll(false);   
                if (objoptName_all!=null)
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

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO SET THE FOLDER PATH FOR PRODUCT IMAGES  ***/
        /********************************************************************************/
        public string  SetImageFolderPath_old(string SourcePath,string FindString, string ConvertTo)
        {
            try
            {
                SourcePath = SourcePath.ToLower();
                FindString = FindString.ToLower();
                ConvertTo = ConvertTo.ToLower();
                string returnval="";
                string tempstr = "";
                string tempstr1 = "";
                string strfile = HttpContext.Current.Server.MapPath("ProdImages");
                if (ConvertTo.ToLower() != "_images")
                {
                    if ((SourcePath.Contains(FindString)))
                        returnval = SourcePath.Replace(FindString, ConvertTo);
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
                    
                    tempstr = strfile.Replace("\\", "/") + SourcePath.Replace(FindString,"");
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

                return returnval;
            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        public string SetImageFolderPath(string SourcePath, string FindString, string ConvertTo)
        {
            try
            {

                if (SourcePath.ToLower().Contains("noimage"))
                    return SourcePath;

                SourcePath = SourcePath.ToLower().Replace("\\", "/").Replace(@"\", "/");
                FindString = FindString.ToLower();
                ConvertTo = ConvertTo.ToLower();
                string returnval = string.Empty;
                string tempstr = string.Empty;
                string tempstr1 = string.Empty;
                string strfile = HttpContext.Current.Server.MapPath("ProdImages");
                if (ConvertTo.ToLower() != "_images")
                {
                    if ((SourcePath.Contains(FindString)))
                    {
                        // returnval = SourcePath.Replace(FindString, ConvertTo);
                        string[] temp1 = SourcePath.Split(new string[] { "/" }, StringSplitOptions.None);
                        if (temp1.Length >= 2)
                        {
                            if ((temp1[temp1.Length - 2].Contains(FindString)))
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
                if ((Reset))
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
                if ((Reset))
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
                    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
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
        public DataSet GetPowerSearchProducts(string Searchtxt, int price_code, int user_id)
        {
            return objHelper.GetPowerSearchProducts(Searchtxt, price_code, user_id);
        }


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
        public string StringTrim(string sValue, int Trimlen, bool trimspace)
        {
            try
            {
                string sReturnValue = "";
                sValue = sValue.Replace(" ", "");

                if (sValue.Length > Trimlen)
                    sReturnValue = sValue.Substring(0, Trimlen) + "...";
                else
                    sReturnValue = sValue;

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
        /*** PURPOSE      : TO RETRIVE THE UPDATED VALUES  ***/
        /********************************************************************************/
        public string Prepare(string sValue)
        {
            try
            {
                string sReturnValue;
                sReturnValue = sValue.Replace("'", "''");
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
            try
            {
                int retValue = 0;
                if (obj != null && obj != DBNull.Value && obj.ToString() != "")
                {
                    retValue = Convert.ToInt32(obj);
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

        //public string URL_Rewrite_New(string url, int iskey, string Formname)
        //{

        //    try
        //    {
        //        string val = string.Empty;
        //        DataTable td = null;


        //        if (HttpContext.Current.Session["URLINI"] != null)
        //        {
        //            DataTable dt = (DataTable)HttpContext.Current.Session["URLINI"];
        //            DataRow[] foundRows;
        //            foundRows = dt.Select("NEWURL='" + url + "'");
        //            if (foundRows.Length == 1)
        //            {
        //                //  objErrorHandler.CreateLog("SelectStart-DT");
        //                val = foundRows[0]["ORGURL"].ToString();
        //                //  objErrorHandler.CreateLog("SelectEnd-DT");
        //                dt.Dispose();

        //            }
        //            else if (foundRows.Length >= 2)
        //            {
        //                DataRow[] foundRows1;
        //                foundRows1 = dt.Select("NEWURL='" + url + "' and ORGURL like '" + Formname.Replace("/", "") + "%'");
        //                if (foundRows1.Length > 0)
        //                {
        //                    //  objErrorHandler.CreateLog("SelectStart-DT");
        //                    val = foundRows1[0]["ORGURL"].ToString();


        //                }
        //            }

        //            else
        //            {

        //                DataTable dtHOME = (DataTable)HttpContext.Current.Session["URLINIHOME"];
        //                DataRow[] foundRowsHOME;
        //                foundRowsHOME = dtHOME.Select("NEWURL='" + url + "'");
        //                if (foundRowsHOME.Length > 0)
        //                {

        //                    val = foundRowsHOME[0]["ORGURL"].ToString();

        //                    dtHOME.Dispose();

        //                }

        //                else
        //                {

        //                    DataTable dtLOGIN = (DataTable)HttpContext.Current.Session["URLINILOGIN"];
        //                    DataRow[] foundRowsLOGIN;
        //                    foundRowsLOGIN = dtHOME.Select("NEWURL='" + url + "'");
        //                    if (foundRowsLOGIN.Length > 0)
        //                    {

        //                        val = foundRowsLOGIN[0]["ORGURL"].ToString();

        //                        dtLOGIN.Dispose();

        //                    }
        //                    else
        //                    {


        //                        DataTable dtdynamic = new DataTable();
        //                        DataRow[] foundRowsDYNAMIC = null;
        //                        if (HttpContext.Current.Session["URLINIDYNAMIC"] != null)
        //                        {
        //                            dtdynamic = (DataTable)HttpContext.Current.Session["URLINIDYNAMIC"];

        //                            foundRowsDYNAMIC = dtdynamic.Select("NEWURL='" + url + "'");
        //                            if (foundRowsDYNAMIC.Length == 1)
        //                            {

        //                                val = foundRowsDYNAMIC[0]["ORGURL"].ToString();

        //                                dtdynamic.Dispose();

        //                            }
        //                            else if (foundRowsDYNAMIC.Length >= 2)
        //                            {
        //                                DataRow[] foundRows1;
        //                                foundRows1 = dtdynamic.Select("NEWURL='" + url + "' and ORGURL like '" + Formname.Replace("/", "") + "%'");
        //                                if (foundRows1.Length > 0)
        //                                {
        //                                    //  objErrorHandler.CreateLog("SelectStart-DT");
        //                                    val = foundRows1[0]["ORGURL"].ToString();


        //                                }
        //                            }
        //                            else
        //                            {


        //                                td = objHelper.GetDataTableDB("Exec STP_PICK_URLRW_WAGNER '" + url + "','" + Formname.ToUpper().Replace("/", "") + "', " + iskey + " ");

        //                                if (td != null && td.Rows.Count == 1)
        //                                {
        //                                    if (iskey == 1)
        //                                        val = td.Rows[0]["ORGURL"].ToString();
        //                                    else
        //                                        val = td.Rows[0]["NEWURL"].ToString();
        //                                }
        //                                else if (td.Rows.Count >= 2)
        //                                {
        //                                    DataRow[] foundRows1;
        //                                    foundRows1 = td.Select("ORGURL like '" + Formname.Replace("/", "") + "%'");
        //                                    if (foundRows1.Length > 0)
        //                                    {
        //                                        //  objErrorHandler.CreateLog("SelectStart-DT");
        //                                        val = foundRows1[0]["ORGURL"].ToString();


        //                                    }
        //                                }
        //                            }
        //                        }

        //                        else
        //                        {
        //                            td = objHelper.GetDataTableDB("Exec STP_PICK_URLRW_WAGNER '" + url + "','" + Formname.ToUpper().Replace("/", "") + "', " + iskey + " ");

        //                            if (td != null && td.Rows.Count == 1)
        //                            {
        //                                if (iskey == 1)
        //                                    val = td.Rows[0]["ORGURL"].ToString();
        //                                else
        //                                    val = td.Rows[0]["NEWURL"].ToString();
        //                            }
        //                            else if (td.Rows.Count >= 2)
        //                            {
        //                                DataRow[] foundRows1;
        //                                foundRows1 = td.Select("ORGURL like '" + Formname.Replace("/", "") + "%'");
        //                                if (foundRows1.Length > 0)
        //                                {
        //                                    //  objErrorHandler.CreateLog("SelectStart-DT");
        //                                    val = foundRows1[0]["ORGURL"].ToString();


        //                                }
        //                            }
        //                        }

        //                    }


        //                }
                        
        //            }


        //            HttpContext.Current.Session["URLINI"] = null;

        //        }
        //        else
        //        {
        //            td = objHelper.GetDataTableDB("Exec STP_PICK_URLRW_WAGNER '" + url + "','"+ Formname.ToUpper().Replace("/","") +"', " + iskey+" ");

        //            if (td != null && td.Rows.Count == 1)
        //            {
        //                if (iskey == 1)
        //                    val = td.Rows[0]["ORGURL"].ToString();
        //                else
        //                    val = td.Rows[0]["NEWURL"].ToString();
        //            }
        //            else if (td.Rows.Count >= 2)
        //            {
        //                DataRow[] foundRows1;
        //                foundRows1 = td.Select("ORGURL like '" + Formname.Replace("/", "") + "%'");
        //                if (foundRows1.Length > 0)
        //                {
        //                    //  objErrorHandler.CreateLog("SelectStart-DT");
        //                    val = foundRows1[0]["ORGURL"].ToString();


        //                }
        //            }
        //        }
        //        return val;

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.CreateLog(ex.ToString() + "-" + "URL_Rewrite_New");

        //        return string.Empty;
        //    }


        //}



        //public string URL_Rewrite_New(string url, int iskey, string Formname, string AtrType)
        //{

        //    try
        //    {
        //        string val = string.Empty;
        //        DataTable td = null;


        //        if (HttpContext.Current.Session["URLINI"] != null)
        //        {
        //            DataTable dt = (DataTable)HttpContext.Current.Session["URLINI"];
        //            DataRow[] foundRows;
        //            foundRows = dt.Select("NEWURL='" + url + "' and atrtype='" + AtrType.ToUpper() + "' ");
        //            if (foundRows.Length > 0)
        //            {
        //                //  objErrorHandler.CreateLog("SelectStart-DT");
        //                val = foundRows[0]["ORGURL"].ToString();
        //                //  objErrorHandler.CreateLog("SelectEnd-DT");
        //                dt.Dispose();
        //                HttpContext.Current.Session["URLINI"] = null;
        //            }
        //            else
        //            {

        //                foundRows = dt.Select("NEWURL='" + url + "' ");
        //                if (foundRows.Length > 0)
        //                {
        //                    //  objErrorHandler.CreateLog("SelectStart-DT");
        //                    val = foundRows[0]["ORGURL"].ToString();
        //                    //  objErrorHandler.CreateLog("SelectEnd-DT");
        //                    dt.Dispose();
        //                    HttpContext.Current.Session["URLINI"] = null;
        //                }
        //                else
        //                {
        //                    //  objErrorHandler.CreateLog("OrgSelectStart-DT");
        //                    td = objHelper.GetDataTableDB("Exec STP_PICK_URLRW_WAGNER_Type '" + url + "','" + AtrType.ToUpper() + "' ,'" + Formname.ToUpper().Replace("/", "") + "' ,  " + iskey + "");
        //                    // objErrorHandler.CreateLog("OrgSelectEnd-DT");

        //                    if (td != null && td.Rows.Count == 1)
        //                    {
        //                        if (iskey == 1)
        //                            val = td.Rows[0]["ORGURL"].ToString();
        //                        else
        //                            val = td.Rows[0]["NEWURL"].ToString();
        //                    }
        //                    else if (td.Rows.Count >= 2)
        //                    {
        //                        DataRow[] foundRows1;
        //                        foundRows1 = td.Select("ORGURL like '" + Formname.Replace("/", "") + "%'");
        //                        if (foundRows1.Length > 0)
        //                        {
        //                            //  objErrorHandler.CreateLog("SelectStart-DT");
        //                            val = foundRows1[0]["ORGURL"].ToString();


        //                        }
        //                    }
        //                    else
        //                    {

        //                        val = URL_Rewrite_New(url, 1, Formname.Replace("/", ""));
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {


        //            //  objErrorHandler.CreateLog("OrgSelectStart-DT");
        //            td = objHelper.GetDataTableDB("Exec STP_PICK_URLRW_WAGNER_Type '" + url + "','" + AtrType.ToUpper() + "' ,'" + Formname.ToUpper().Replace("/", "") + "' ,  " + iskey + "");
        //            // objErrorHandler.CreateLog("OrgSelectEnd-DT");
        //            if (td != null && td.Rows.Count == 1)
        //            {
        //                if (iskey == 1)
        //                    val = td.Rows[0]["ORGURL"].ToString();
        //                else
        //                    val = td.Rows[0]["NEWURL"].ToString();
        //            }
        //            else if (td.Rows.Count >= 2)
        //            {
        //                DataRow[] foundRows1;
        //                foundRows1 = td.Select("ORGURL like '" + Formname.Replace("/", "") + "%'");
        //                if (foundRows1.Length > 0)
        //                {

        //                    val = foundRows1[0]["ORGURL"].ToString();


        //                }
        //            }
        //            else
        //            {

        //                val = URL_Rewrite_New(url, 1, Formname.Replace("/", ""));
        //            }
        //        }

        //        return val;

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.CreateLog(ex.ToString() + "-" + "URL_Rewrite_New");

        //        return string.Empty;
        //    }


        //}
        public string URL_Rewrite_New(string url, int iskey, string Formname)
        {

            try
            {
                string val = string.Empty;
                DataTable td = null;

                Formname = Formname.Replace("/", "") + "%";
                if (HttpContext.Current.Session["URLINI"] != null)
                {
                    DataTable dt = (DataTable)HttpContext.Current.Session["URLINI"];
                    DataRow[] foundRows;

                    foundRows = dt.Select("NEWURL='" + url + "' and ORGURL like '" + Formname + "'");
                    if (foundRows.Length > 0)
                    {
                        //  objErrorHandler.CreateLog("SelectStart-DT");
                        val = foundRows[0]["ORGURL"].ToString();
                        dt.Dispose();
                        HttpContext.Current.Session["URLINI"] = null;

                        return val;
                    }
                }
                if (HttpContext.Current.Session["URLINIDYNAMIC"] != null)
                {


                    DataTable dtdynamic = (DataTable)HttpContext.Current.Session["URLINIDYNAMIC"];

                    DataRow[] foundRowsDYNAMIC = dtdynamic.Select("NEWURL='" + url + "'  and ORGURL like '" + Formname + "'");
                    if (foundRowsDYNAMIC.Length > 0)
                    {

                        val = foundRowsDYNAMIC[0]["ORGURL"].ToString();

                        dtdynamic.Dispose();
                        HttpContext.Current.Session["URLINIDYNAMIC"] = null;
                        return val;
                    }

                }



                if (HttpContext.Current.Session["URLINIHOME"] != null)
                {
                    DataTable dtHOME = (DataTable)HttpContext.Current.Session["URLINIHOME"];
                    DataRow[] foundRowsHOME;
                    foundRowsHOME = dtHOME.Select("NEWURL='" + url + "'  and ORGURL like '" + Formname + "'");
                    if (foundRowsHOME.Length > 0)
                    {

                        val = foundRowsHOME[0]["ORGURL"].ToString();

                        dtHOME.Dispose();
                        return val;
                    }
                }

                if (HttpContext.Current.Session["URLINILOGIN"] != null)
                {

                    DataTable dtLOGIN = (DataTable)HttpContext.Current.Session["URLINILOGIN"];
                    DataRow[] foundRowsLOGIN;
                    foundRowsLOGIN = dtLOGIN.Select("NEWURL='" + url + "' and ORGURL like '" + Formname + "'");
                    if (foundRowsLOGIN.Length > 0)
                    {

                        val = foundRowsLOGIN[0]["ORGURL"].ToString();

                        dtLOGIN.Dispose();
                        return val;
                    }

                }






                return val;
            }
            catch
            {
                return "";
            }
        }
        public string Cons_NewURl(StringTemplate _stmpl_records, string EAPATH, string FormName, bool Insertdt, string Atrtype, bool isdynamicpage)
        {
            if (FormName.Contains("/mct/") || FormName.Contains("mct.aspx") ||
                         FormName.Contains("/mfl/") || FormName.Contains("mfl.aspx")
                || FormName.Contains("/mpl/") || FormName.Contains("mpl.aspx")
                || FormName.Contains("/mpd/") || FormName.Contains("mpd.aspx")
|| FormName.Contains("/mps/") || FormName.Contains("mps.aspx"))
            {
                return Cons_NewURl_MS(_stmpl_records, EAPATH, FormName, Insertdt, Atrtype, true, false);
            
            }
            else
            {
                return Cons_NewURl(_stmpl_records, EAPATH, FormName, Insertdt, Atrtype, true, false);

            }
          
        
        }
        public string Cons_NewURl(StringTemplate _stmpl_records, string EAPATH, string FormName, bool Insertdt, string Atrtype)
        {

            if (FormName.Contains("/mct/") || FormName.Contains("mct.aspx") ||
                         FormName.Contains("/mfl/") || FormName.Contains("mfl.aspx")
                || FormName.Contains("/mpl/") || FormName.Contains("mpl.aspx")
                || FormName.Contains("/mpd/") || FormName.Contains("mpd.aspx")
|| FormName.Contains("/mps/") || FormName.Contains("mps.aspx"))
            {
                return Cons_NewURl_MS(_stmpl_records, EAPATH, FormName, Insertdt, Atrtype, true, false);

            }
            else
            {
                return Cons_NewURl(_stmpl_records, EAPATH, FormName, Insertdt, Atrtype, false, false);

            }
            }
        public string Cons_NewURl_MS(StringTemplate _stmpl_records, string EAPATH, string FormName, bool Insertdt, string Atrtype, bool isdynamicpage, bool isnewfamily)
        {
            // string ORIGINALURL = string.Empty;
            string ORGURL = string.Empty;
            string newformname = FormName.ToUpper();
            try
            {
                // string stlistprod = string.Empty;
                string strValue = _stmpl_records.ToString();
                Match m2 = Regex.Match(strValue, @"rel=\""(.*?)\""", RegexOptions.Singleline);
                if (FormName.Contains("-"))
                {
                    string[] FormNamenew = FormName.Split('-');
                    newformname = FormNamenew[1].ToUpper();
                    if (FormNamenew[0] == "BCRemoveurl")
                        m2 = Regex.Match(strValue, @"relrem=\""(.*?)\""", RegexOptions.Singleline);
                }



                ORGURL = m2.Groups[1].ToString().Replace("amp;", "");



                string strEApath = EAPATH.Replace("AttribSelect=", "");



                string[] ConsURL = strEApath.Split(new string[] { "////" }, StringSplitOptions.None);
                ConsURL = ConsURL.Where(c => c.Trim() != "").ToArray();


                if (ConsURL.Length >= 2)
                {
                    if (ConsURL[0].ToLower().Contains("allproducts") || ConsURL[0].ToLower().Contains("all products"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                    if (ConsURL[0].ToUpper().Contains("WESAUSTRALASIA"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();



                    // Array.Reverse(ConsURL);

                }
                //int i = Array.IndexOf(ConsURL, ConsURL.Where(x => x.Contains(":")).FirstOrDefault());
                //if (i > 0)
                //{
                //    string[] tempcons = ConsURL[i].Split(':');
                //    ConsURL[i] = tempcons[1];
                //}


                //string stlistprod = URLstringreplace(string.Join("/", ConsURL));
                string stlistprod = string.Empty;
                string lastkey = string.Empty;
                string equalskey = string.Empty;
                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        string replacedword = URLstringreplace_New(ConsURL[i]);
                        string[] srepword = replacedword.Split('/');
                        stlistprod = srepword[0];
                        lastkey = srepword[1];
                        if (srepword[2] != "")
                        {
                            equalskey = "0-" + srepword[2];
                        }
                    }
                    else
                    {
                        string repconsurl = string.Empty;
                        string replacedword = URLstringreplace_New(ConsURL[i]);
                        string[] srepword = replacedword.Split('/');
                        // stlistprod =stlistprod+"/"+ srepword[0];
                        lastkey = lastkey + "-" + srepword[1];
                        repconsurl = srepword[0];
                        if (srepword[2] != "")
                        {
                            if (equalskey != "")
                            {
                                equalskey = equalskey + "/" + i + "-" + srepword[2];
                            }
                            else
                            {
                                equalskey = i + "-" + srepword[2];
                            }
                        }
                        try
                        {
                            string frmnamelwr = string.Empty;
                            frmnamelwr = FormName.ToLower();

                            if (frmnamelwr.Contains("fl.aspx") || frmnamelwr.Contains("mfl.aspx"))
                            {
                                if (i == ConsURL.Length - 1)
                                {
                                    repconsurl = repconsurl.Replace("__", "_");
                                }
                            }
                            if (frmnamelwr.Contains("pd.aspx") || frmnamelwr.Contains("mpd.aspx"))
                            {
                                if (i == ConsURL.Length - 2)
                                {
                                    repconsurl = repconsurl.Replace("__", "_");
                                }
                            }
                        }
                        catch
                        { }

                        stlistprod = stlistprod+"/"+ repconsurl ;


                    }
                }


                if (ORGURL != string.Empty)
                {

                    //if (Insertdt == true)
                    //{

                    //    //InsertToDt(stlistprod, ORGURL, newformname);
                    //    //InsertToDt(newformname + "?" + stlistprod, ORGURL);

                    //    if (isdynamicpage == true)
                    //    {
                    //        InsertToDt_DYNAMIC(stlistprod, ORGURL, Atrtype);
                    //    }
                    //    else
                    //    {
                    //        InsertToDt(stlistprod, ORGURL, Atrtype);
                    //    }
                    //}



                }


                if ((stlistprod.ToString().EndsWith(".")))
                {
                    stlistprod = stlistprod.Substring(0, stlistprod.Length - 1);
                }
                if (equalskey != "")
                {
                    equalskey = "xx" + equalskey;
                }
                if (stlistprod.StartsWith("/"))
                {
                    stlistprod = stlistprod.Substring(1, stlistprod.Length - 1);
                }
                stlistprod = stlistprod.ToLower();
                if (FormName.Contains("-"))
                {
                    string[] FormNamenew = FormName.Split('-');
                    string formname = FormNamenew[1];
                    if (!(formname.ToLower().Contains("home.aspx")))
                    {

                        formname = formname.Replace(".aspx", "");

                        if (FormNamenew[0] == "BCRemoveurl")
                            //FormNamenew[1] + "?"
                            _stmpl_records.SetAttribute("TBT_REMOVEREWRITEURL", (stlistprod.ToLower() + "/" + lastkey + equalskey + "/" + formname + "/").Replace("//", "/"));
                        else
                            //FormNamenew[1] + "?" + 
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", (stlistprod.ToLower() + "/" + lastkey + equalskey + "/" + formname + "/").Replace("//", "/"));

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
                else if (!(isnewfamily))
                {
                    _stmpl_records.SetAttribute("TBT_REWRITEURL", stlistprod.ToLower() + "/" + lastkey + equalskey);
                    // _stmpl_records.SetAttribute("TBT_REWRITEURL_new", stlistprod.ToUpper());
                }

                if ((isnewfamily))
                {
                    _stmpl_records.SetAttribute("TBT_REWRITEURL_FAMILY", stlistprod.ToLower() + "/" + lastkey + equalskey);
                }


                stlistprod = stlistprod + "/" + lastkey + equalskey;

                return stlistprod;
            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl");

                return ORGURL;
            }

        }












        public string Cons_NewURlASPX_MS(string _stmpl_records, string EAPATH, string FormName, bool Insertdt, string Atrtype, bool isdynamicpage, bool isnewfamily)
        {
            // string ORIGINALURL = string.Empty;
            string ORGURL = string.Empty;
            string newformname = FormName.ToUpper();
            try
            {
                // string stlistprod = string.Empty;
                //string strValue = _stmpl_records.ToString();
                //Match m2 = Regex.Match(strValue, @"rel=\""(.*?)\""", RegexOptions.Singleline);
                //if (FormName.Contains("-"))
                //{
                //    string[] FormNamenew = FormName.Split('-');
                //    newformname = FormNamenew[1].ToUpper();
                //    if (FormNamenew[0] == "BCRemoveurl")
                //        m2 = Regex.Match(strValue, @"relrem=\""(.*?)\""", RegexOptions.Singleline);
                //}



                //ORGURL = m2.Groups[1].ToString().Replace("amp;", "");



                string strEApath = EAPATH.Replace("AttribSelect=", "");



                string[] ConsURL = strEApath.Split(new string[] { "////" }, StringSplitOptions.None);
                ConsURL = ConsURL.Where(c => c.Trim() != "").ToArray();


                if (ConsURL.Length >= 2)
                {
                    if (ConsURL[0].ToLower().Contains("allproducts") || ConsURL[0].ToLower().Contains("all products"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                    if (ConsURL[0].ToUpper().Contains("WESAUSTRALASIA"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();



                    // Array.Reverse(ConsURL);

                }
                //int i = Array.IndexOf(ConsURL, ConsURL.Where(x => x.Contains(":")).FirstOrDefault());
                //if (i > 0)
                //{
                //    string[] tempcons = ConsURL[i].Split(':');
                //    ConsURL[i] = tempcons[1];
                //}


                //string stlistprod = URLstringreplace(string.Join("/", ConsURL));
                string stlistprod = string.Empty;
                string lastkey = string.Empty;
                string equalskey = string.Empty;
                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        string replacedword = URLstringreplace_New(ConsURL[i]);
                        string[] srepword = replacedword.Split('/');
                        stlistprod = srepword[0];
                        lastkey = srepword[1];
                        if (srepword[2] != "")
                        {
                            equalskey = "0-" + srepword[2];
                        }
                    }
                    else
                    {
                        string repconsurl = string.Empty;
                        string replacedword = URLstringreplace_New(ConsURL[i]);
                        string[] srepword = replacedword.Split('/');
                        // stlistprod =stlistprod+"/"+ srepword[0];
                        lastkey = lastkey + "-" + srepword[1];
                        repconsurl = srepword[0];
                        if (srepword[2] != "")
                        {
                            if (equalskey != "")
                            {
                                equalskey = equalskey + "/" + i + "-" + srepword[2];
                            }
                            else
                            {
                                equalskey = i + "-" + srepword[2];
                            }
                        }
                        try
                        {
                            if (FormName.ToLower().Contains("fl.aspx") || FormName.ToLower().Contains("mfl.aspx"))
                            {
                                if (i == ConsURL.Length - 1)
                                {
                                    repconsurl = repconsurl.Replace("__", "_");
                                }
                            }
                            if (FormName.ToLower().Contains("pd.aspx") || FormName.ToLower().Contains("mpd.aspx"))
                            {
                                if (i == ConsURL.Length - 2)
                                {
                                    repconsurl = repconsurl.Replace("__", "_");
                                }
                            }
                        }
                        catch
                        { }

                        stlistprod = stlistprod + "/" + repconsurl;


                    }
                }


                if (ORGURL != string.Empty)
                {

                    //if (Insertdt == true)
                    //{

                    //    //InsertToDt(stlistprod, ORGURL, newformname);
                    //    //InsertToDt(newformname + "?" + stlistprod, ORGURL);

                    //    if (isdynamicpage == true)
                    //    {
                    //        InsertToDt_DYNAMIC(stlistprod, ORGURL, Atrtype);
                    //    }
                    //    else
                    //    {
                    //        InsertToDt(stlistprod, ORGURL, Atrtype);
                    //    }
                    //}



                }


                if ((stlistprod.ToString().EndsWith(".")))
                {
                    stlistprod = stlistprod.Substring(0, stlistprod.Length - 1);
                }
                if (equalskey != "")
                {
                    equalskey = "xx" + equalskey;
                }
                if (stlistprod.StartsWith("/"))
                {
                    stlistprod = stlistprod.Substring(1, stlistprod.Length - 1);
                }
                stlistprod = stlistprod.ToLower();

                if (_stmpl_records == null)
                {
                    if (FormName.Contains("-"))
                    {
                        string[] FormNamenew = FormName.Split('-');
                        string formname = FormNamenew[1];
                        if (formname.ToLower().Contains("home.aspx") == false)
                        {

                            formname = formname.Replace(".aspx", "");

                            if (FormNamenew[0] == "BCRemoveurl")
                                //FormNamenew[1] + "?"
                                return (stlistprod.ToLower() + "/" + lastkey + equalskey + "/" + formname + "/").Replace("//", "/");
                            else
                                //FormNamenew[1] + "?" + 
                                return (stlistprod.ToLower() + "/" + lastkey + equalskey + "/" + formname + "/").Replace("//", "/");

                        }
                        else
                        {
                            if (FormNamenew[0] == "BCRemoveurl")
                                //FormNamenew[1] + "?"
                                return "Home.aspx";
                            else
                                //FormNamenew[1] + "?" + 
                                return "Home.aspx";

                        }

                    }
                    else if (isnewfamily == false)
                    {
                        return stlistprod.ToLower() + "/" + lastkey + equalskey;
                        // _stmpl_records.SetAttribute("TBT_REWRITEURL_new", stlistprod.ToUpper());
                    }
                }
                else
                {
















                }
                if ((isnewfamily))
                {
                    return stlistprod.ToLower() + "/" + lastkey + equalskey;
                }


                stlistprod = stlistprod + "/" + lastkey + equalskey;

                return stlistprod;
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl");

                return ORGURL;
            }

        }  
        
        
        
        public string Cons_NewURl(StringTemplate _stmpl_records, string EAPATH, string FormName, bool Insertdt, string Atrtype, bool isdynamicpage,bool isnewfamily)
        {
            // string ORIGINALURL = string.Empty;
            string ORGURL = string.Empty;
            string newformname = FormName.ToUpper();
            string formnamelwr = string.Empty;
            formnamelwr = FormName.ToLower();
            try
            {
                // string stlistprod = string.Empty;
                string strValue = _stmpl_records.ToString();
                Match m2 = Regex.Match(strValue, @"rel=\""(.*?)\""", RegexOptions.Singleline);
                if (FormName.Contains("-"))
                {
                    string[] FormNamenew = FormName.Split('-');
                    newformname = FormNamenew[1].ToUpper();
                    if (FormNamenew[0] == "BCRemoveurl")
                        m2 = Regex.Match(strValue, @"relrem=\""(.*?)\""", RegexOptions.Singleline);
                }


              
                ORGURL = m2.Groups[1].ToString().Replace("amp;", "");



                string strEApath = EAPATH.Replace("AttribSelect=", "");



                string[] ConsURL = strEApath.Split(new string[] { "////" }, StringSplitOptions.None);
                ConsURL = ConsURL.Where(c => c.Trim() != "").ToArray();


                if (ConsURL.Length >= 2)
                {
                    if (ConsURL[0].ToLower().Contains("allproducts") || ConsURL[0].ToLower().Contains("all products"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                    if (ConsURL[0].ToUpper().Contains("WESAUSTRALASIA"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                   


                    // Array.Reverse(ConsURL);

                }
                //int i = Array.IndexOf(ConsURL, ConsURL.Where(x => x.Contains(":")).FirstOrDefault());
                //if (i > 0)
                //{
                //    string[] tempcons = ConsURL[i].Split(':');
                //    ConsURL[i] = tempcons[1];
                //}


                //string stlistprod = URLstringreplace(string.Join("/", ConsURL));
                string stlistprod = string.Empty;
                string lastkey = string.Empty;
                string equalskey = string.Empty;
                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        string replacedword=URLstringreplace_New(ConsURL[i]);
                        string[] srepword=replacedword.Split('/');
                        stlistprod = srepword[0];
                        lastkey = srepword[1];
                        if (srepword[2] != "")
                        {
                            equalskey = "0-" + srepword[2];
                        }
                    }
                    else
                    {
                        string repconsurl=string.Empty;
                        string replacedword = URLstringreplace_New(ConsURL[i]);
                        string[] srepword = replacedword.Split('/');
                       // stlistprod =stlistprod+"/"+ srepword[0];

                       

                        lastkey =lastkey +"-"+ srepword[1];
                        repconsurl = srepword[0];
                        if (srepword[2] != "")
                        {
                            if (equalskey != "")
                            {
                                equalskey = equalskey + "/" + i + "-" + srepword[2];
                            }
                            else
                            {
                                equalskey = i + "-" + srepword[2];
                            }
                        }
                        try
                        {
                            if (formnamelwr.Contains("fl.aspx") || formnamelwr.Contains("mfl.aspx"))
                            {
                                if (i == ConsURL.Length - 1)
                                {
                                    repconsurl = repconsurl.Replace("__", "_");
                                }
                            }
                            if (formnamelwr.Contains("pd.aspx") || formnamelwr.Contains("mpd.aspx"))
                            {
                                if (i == ConsURL.Length - 2)
                                {
                                    repconsurl = repconsurl.Replace("__", "_");
                                }
                            }
                        }
                        catch
                        { }

                        stlistprod = repconsurl + "/" + stlistprod;
                   
                        
                    }
                }

               
                if (ORGURL != string.Empty)
                {

                    //if (Insertdt == true)
                    //{

                    //    //InsertToDt(stlistprod, ORGURL, newformname);
                    //    //InsertToDt(newformname + "?" + stlistprod, ORGURL);

                    //    if (isdynamicpage == true)
                    //    {
                    //        InsertToDt_DYNAMIC(stlistprod, ORGURL, Atrtype);
                    //    }
                    //    else
                    //    {
                    //        InsertToDt(stlistprod, ORGURL, Atrtype);
                    //    }
                    //}



                }


                if ((stlistprod.EndsWith(".")))
                {
                    stlistprod = stlistprod.Substring(0, stlistprod.Length - 1);
                }
                if (equalskey != "")
                {
                    equalskey = "xx" + equalskey;
                }
                 if (stlistprod.StartsWith("/"))
                        {
                            stlistprod = stlistprod.Substring(1, stlistprod.Length - 1);
                        }
                 stlistprod = stlistprod.ToLower();
                if (FormName.Contains("-"))
                {
                    string[] FormNamenew = FormName.Split('-');
                    string formname = FormNamenew[1];
                    if (!(formname.ToLower().Contains("home.aspx")))
                    {

                        formname = formname.Replace(".aspx", "");
                       
                        if (FormNamenew[0] == "BCRemoveurl")
                            //FormNamenew[1] + "?"
                            _stmpl_records.SetAttribute("TBT_REMOVEREWRITEURL", (stlistprod + "/" + lastkey  +equalskey +"/"+ formname+"/").Replace("//", "/"));
                        else
                            //FormNamenew[1] + "?" + 
                            _stmpl_records.SetAttribute("TBT_REWRITEURL", (stlistprod + "/" + lastkey + equalskey + "/" + formname+"/").Replace("//", "/"));

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
                else if (!(isnewfamily))
                {
                    _stmpl_records.SetAttribute("TBT_REWRITEURL", stlistprod + "/" + lastkey+equalskey );
                   // _stmpl_records.SetAttribute("TBT_REWRITEURL_new", stlistprod.ToUpper());
                }

                if ((isnewfamily))
                {
                    _stmpl_records.SetAttribute("TBT_REWRITEURL_FAMILY", stlistprod+ "/" + lastkey +equalskey );
                }

               
                    stlistprod = stlistprod + "/" + lastkey + equalskey;
               
                return stlistprod ;
            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl");

                return ORGURL;
            }

        }
        public string Cons_NewURlASPX(string _stmpl_records, string EAPATH, string FormName, bool Insertdt, string Atrtype, bool isdynamicpage)
        {
            if (FormName.Contains("/mct/") || FormName.Contains("mct.aspx") ||
                           FormName.Contains("/mfl/") || FormName.Contains("mfl.aspx")
                  || FormName.Contains("/mpl/") || FormName.Contains("mpl.aspx")
                  || FormName.Contains("/mpd/") || FormName.Contains("mpd.aspx")
               || FormName.Contains("/mps/") || FormName.Contains("mps.aspx"))
            {
                return Cons_NewURlASPX_MS(_stmpl_records, EAPATH, FormName, Insertdt, Atrtype, true, false);
            }
            else
            {
                return Cons_NewURlASPX(_stmpl_records, EAPATH, FormName, Insertdt, Atrtype, true, false);
            }
             }
        public string Cons_NewURlASPX(string _stmpl_records, string EAPATH, string FormName, bool Insertdt, string Atrtype)
        {
            if (FormName.Contains("/mct/") || FormName.Contains("mct.aspx") ||
                         FormName.Contains("/mfl/") || FormName.Contains("mfl.aspx")
                || FormName.Contains("/mpl/") || FormName.Contains("mpl.aspx")
                || FormName.Contains("/mpd/") || FormName.Contains("mpd.aspx")
             || FormName.Contains("/mps/") || FormName.Contains("mps.aspx"))
            {
                return Cons_NewURlASPX_MS(_stmpl_records, EAPATH, FormName, Insertdt, Atrtype, false, false);
            }
            else
            {
                return Cons_NewURlASPX(_stmpl_records, EAPATH, FormName, Insertdt, Atrtype, false, false);
            }
            }
        public string Cons_NewURlASPX(string _stmpl_records, string EAPATH, string FormName, bool Insertdt, string Atrtype, bool isdynamicpage, bool isnewfamily)
        {
            // string ORIGINALURL = string.Empty;
            string ORGURL = string.Empty;
            string newformname = FormName.ToUpper();
            try
            {
                // string stlistprod = string.Empty;
                //string strValue = _stmpl_records.ToString();
                //Match m2 = Regex.Match(strValue, @"rel=\""(.*?)\""", RegexOptions.Singleline);
                //if (FormName.Contains("-"))
                //{
                //    string[] FormNamenew = FormName.Split('-');
                //    newformname = FormNamenew[1].ToUpper();
                //    if (FormNamenew[0] == "BCRemoveurl")
                //        m2 = Regex.Match(strValue, @"relrem=\""(.*?)\""", RegexOptions.Singleline);
                //}



                //ORGURL = m2.Groups[1].ToString().Replace("amp;", "");



                string strEApath = EAPATH.Replace("AttribSelect=", "");



                string[] ConsURL = strEApath.Split(new string[] { "////" }, StringSplitOptions.None);
                ConsURL = ConsURL.Where(c => c.Trim() != "").ToArray();


                if (ConsURL.Length >= 2)
                {
                    if (ConsURL[0].ToLower().Contains("allproducts") || ConsURL[0].ToLower().Contains("all products"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                    if (ConsURL[0].ToUpper().Contains("WESAUSTRALASIA"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();



                    // Array.Reverse(ConsURL);

                }
                //int i = Array.IndexOf(ConsURL, ConsURL.Where(x => x.Contains(":")).FirstOrDefault());
                //if (i > 0)
                //{
                //    string[] tempcons = ConsURL[i].Split(':');
                //    ConsURL[i] = tempcons[1];
                //}


                //string stlistprod = URLstringreplace(string.Join("/", ConsURL));
                string stlistprod = string.Empty;
                string lastkey = string.Empty;
                string equalskey = string.Empty;
                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        string replacedword = URLstringreplace_New(ConsURL[i]);
                        string[] srepword = replacedword.Split('/');
                        stlistprod = srepword[0];
                        lastkey = srepword[1];
                        if (srepword[2] != "")
                        {
                            equalskey = "0-" + srepword[2];
                        }
                    }
                    else
                    {
                        string repconsurl = string.Empty;
                        string replacedword = URLstringreplace_New(ConsURL[i]);
                        string[] srepword = replacedword.Split('/');
                        // stlistprod =stlistprod+"/"+ srepword[0];
                        lastkey = lastkey + "-" + srepword[1];
                        repconsurl = srepword[0];
                        if (srepword[2] != "")
                        {
                            if (equalskey != "")
                            {
                                equalskey = equalskey + "/" + i + "-" + srepword[2];
                            }
                            else
                            {
                                equalskey = i + "-" + srepword[2];
                            }
                        }
                        try
                        {
                            string frmnamelwr = string.Empty;
                            frmnamelwr = FormName.ToLower();
                            if (frmnamelwr.Contains("fl.aspx") || frmnamelwr.Contains("mfl.aspx"))
                            {
                                if (i == ConsURL.Length - 1)
                                {
                                    repconsurl = repconsurl.Replace("__", "_");
                                }
                            }
                            if (frmnamelwr.Contains("pd.aspx") || frmnamelwr.Contains("mpd.aspx"))
                            {
                                if (i == ConsURL.Length - 2)
                                {
                                    repconsurl = repconsurl.Replace("__", "_");
                                }
                            }
                        }
                        catch
                        { }

                        stlistprod = repconsurl + "/" + stlistprod;


                    }
                }


                if (ORGURL != string.Empty)
                {

                    //if (Insertdt == true)
                    //{

                    //    //InsertToDt(stlistprod, ORGURL, newformname);
                    //    //InsertToDt(newformname + "?" + stlistprod, ORGURL);

                    //    if (isdynamicpage == true)
                    //    {
                    //        InsertToDt_DYNAMIC(stlistprod, ORGURL, Atrtype);
                    //    }
                    //    else
                    //    {
                    //        InsertToDt(stlistprod, ORGURL, Atrtype);
                    //    }
                    //}



                }


                if ((stlistprod.ToString().EndsWith(".")))
                {
                    stlistprod = stlistprod.Substring(0, stlistprod.Length - 1);
                }
                if (equalskey != string.Empty)
                {
                    equalskey = "xx" + equalskey;
                }
                if (stlistprod.StartsWith("/"))
                {
                    stlistprod = stlistprod.Substring(1, stlistprod.Length - 1);
                }
                stlistprod = stlistprod.ToLower();
                if (FormName.Contains("-"))
                {
                    string[] FormNamenew = FormName.Split('-');
                    string formname = FormNamenew[1];
                    if (!(formname.ToLower().Contains("home.aspx")))
                    {

                        formname = formname.Replace(".aspx", "");

                        if (FormNamenew[0] == "BCRemoveurl")
                            //FormNamenew[1] + "?"
                            return (stlistprod + "/" + lastkey + equalskey + "/" + formname + "/").Replace("//", "/");
                        else
                            //FormNamenew[1] + "?" + 
                            return (stlistprod + "/" + lastkey + equalskey + "/" + formname + "/").Replace("//", "/");

                    }
                    else
                    {
                        if (FormNamenew[0] == "BCRemoveurl")
                            //FormNamenew[1] + "?"
                            return "Home.aspx";
                        else
                            //FormNamenew[1] + "?" + 
                            return "Home.aspx";

                    }

                }
                else if (!(isnewfamily))
                {
                    return stlistprod + "/" + lastkey + equalskey;
                    // _stmpl_records.SetAttribute("TBT_REWRITEURL_new", stlistprod.ToUpper());
                }

                if ((isnewfamily))
                {
                    return stlistprod + "/" + lastkey + equalskey;
                }


                stlistprod = stlistprod + "/" + lastkey + equalskey;

                return stlistprod;
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl");

                return ORGURL;
            }

        }
        public string cons_NEWURL_ORG(StringTemplate _stmpl_records, string EAPATH, string FormName, bool Insertdt, string Atrtype)
        {
            return cons_NEWURL_ORG(_stmpl_records, EAPATH, FormName, Insertdt, Atrtype, false, false);
        }
        public string cons_NEWURL_ORG(StringTemplate _stmpl_records, string EAPATH, string FormName, bool Insertdt, string Atrtype, bool isdynamicpage, bool isnewfamily)
        {
            // string ORIGINALURL = string.Empty;
            string ORGURL = string.Empty;
            string newformname = FormName.ToUpper();
            try
            {
                // string stlistprod = string.Empty;
                string strValue = _stmpl_records.ToString();
                Match m2 = Regex.Match(strValue, @"rel=\""(.*?)\""", RegexOptions.Singleline);
                if (FormName.Contains("-"))
                {
                    string[] FormNamenew = FormName.Split('-');
                    newformname = FormNamenew[1].ToUpper();
                    if (FormNamenew[0] == "BCRemoveurl")
                        m2 = Regex.Match(strValue, @"relrem=\""(.*?)\""", RegexOptions.Singleline);
                }



                ORGURL = m2.Groups[1].ToString().Replace("amp;", "");





                return ORGURL;
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl");

                return ORGURL;
            }
        }




  
        public void InsertToDt_DYNAMIC(string stlistprod, string ORGURL, string ATRTYPE)
        {
            //, string FormName
            try
            {
                if (HttpContext.Current.Session["URLINIDYNAMIC"] == null)
                {
                    DataTable dtDYNAMIC = new DataTable();
                    dtDYNAMIC.Columns.Add("NewURL");
                    dtDYNAMIC.Columns.Add("ORGURL");
                    dtDYNAMIC.Columns.Add("ATRTYPE");
                    //dt.Columns.Add("FormName");
                    HttpContext.Current.Session["URLINIDYNAMIC"] = dtDYNAMIC;
                }
                 DataTable dtcheck = (DataTable)HttpContext.Current.Session["URLINI"];
                    DataRow[] foundRows;

                    foundRows = dtcheck.Select("NEWURL='" + stlistprod + "' ");
                    if (foundRows.Length <= 0)
                    {
                        DataTable dt = (DataTable)HttpContext.Current.Session["URLINIDYNAMIC"];
                        DataRow dr = dt.NewRow();
                        dr["NewURL"] = stlistprod;
                        dr["ORGURL"] = ORGURL;
                        dr["ATRTYPE"] = ATRTYPE;
                        //FormName = FormName.Replace(".ASPX", ""); 
                        //dr["FormName"] = FormName;
                        dt.Rows.Add(dr);
                        HttpContext.Current.Session["URLINIDYNAMIC"] = dt;
                    }
                //}


            }
            catch (Exception ex)
            {
                //objErrorHandler.ErrorMsg = objException;
                //objErrorHandler.CreateLog(ex.ToString() + "InsertToDt");

            }


        }


        //public string URLstringreplace(string skeyword)
        //{


        //    try
        //    {

        //        skeyword = HttpUtility.UrlDecode(skeyword).Trim().ToLower();
        //        skeyword = skeyword.Replace(" - ", "-").Replace("  ", " ").Replace("  ", " ").
        //            Replace("<ars>g</ars>", "-").Replace("&rarr;", "-").
        //            Replace("rarr;", "-").Replace("<", "-").Replace(">", "-").
        //            Replace("bol", "-").Replace("\n", "-").Replace("\r", "-").Replace("!", "-").Replace("®", "-").Replace("[", "-").Replace("]", "-").Replace("--", "-").Replace("&", "-").Replace(" & ", "-").Replace("+-+", "-").ToString().Replace(" ", "-").Replace('"', ' ').Replace("---", "-").Replace("---", "-").Replace("--", "-").Replace("-/-", "/").Replace("+", "-/").Replace("-/", "-").Replace("/-", "/").Replace("-&-", "-");

        //        if (skeyword.EndsWith("-") == true)
        //        {
        //            skeyword = skeyword.Remove(skeyword.Length - 1, 1);
        //        }
        //        skeyword = skeyword.ToUpper();


        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.CreateLog(ex.ToString() + "URLstringreplace");
        //    }
        //    return skeyword;
        //}
        public string URLstringreplace(string skeyword)
        {

           
            try
            {
               
                skeyword = HttpUtility.UrlDecode(skeyword).Trim().ToLower();
                StringBuilder SBskeyword = new StringBuilder(skeyword);
                SBskeyword.Replace("<ars> g</ars>", "_");
                SBskeyword.Replace("<ars> g </ars>", "_");
                SBskeyword.Replace("<ars>g </ars>", "_");
                SBskeyword.Replace("<ars>g</ars>", "_");
                 SBskeyword.Replace("&rarr;", "_");
                 SBskeyword.Replace("rarr;", "_");
                 SBskeyword.Replace("bol", "_");
                     SBskeyword.Replace("\n", "_");
                     SBskeyword.Replace("\r", "_");
                     SBskeyword.Replace(' ', '_');
                 SBskeyword.Replace("\"", "``");
                
                 
                skeyword = SBskeyword.ToString();
                if (skeyword.Contains("_/_") == false && skeyword.Contains("_/") == false || skeyword.Contains("/_") == false)
                {
                    skeyword = skeyword.Replace("/", "`/`");
                }


                //Replace("/", "//")
                //.Replace("+", "||").Replace("\"", "``").Replace("&", "^^").Replace(":", "~`").Replace(".","~^");
         // string      skeyword1 = HttpUtility.UrlEncode(skeyword).Trim();
          //if (skeyword1.Contains("%E2%80%9C") == true)
          //{
          //    skeyword = skeyword1.ToUpper().Replace("%E2%80%9C", "``").Replace("%E2%80%9D", "``");
          //    skeyword = HttpUtility.UrlDecode(skeyword).Trim();
          //}
                if ((skeyword.EndsWith("_")))
                {
                    skeyword = skeyword.Remove(skeyword.Length - 1, 1);
                }
                skeyword = skeyword.ToUpper();


            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog(ex.ToString() + "URLstringreplace");
            }
            return skeyword;
        }
        public string URLstringreplace_New(string skeyword)
        {
            //skeyword = "USB-mA fe Female";
            //This key is the logic of special character replacement(eg:909)
            string repkey = string.Empty;
     
            string equalskey = string.Empty;
            try
            {
              
                skeyword = HttpUtility.UrlDecode(skeyword).Trim().ToLower();
                if ((skeyword.Contains('=')))
                {
                    string[] splitkeyword = skeyword.Split(new string[] { "=" }, StringSplitOptions.None);
                    skeyword = splitkeyword[1];
                    //Keywords such as product tag space is replaced and - is also replaced
                    equalskey = splitkeyword[0].Replace("-", "~-").Replace(' ', '~').Replace("%","-~");
                }
               string newkeyword=skeyword;
                StringBuilder SBskeyword = new StringBuilder(skeyword);
                SBskeyword.Replace("<ars>g</ars>", "→");
                SBskeyword.Replace("&rarr;", "→");
                SBskeyword.Replace("rarr;", "→");               
                SBskeyword.Replace('_', '-');
                SBskeyword.Replace(' ', '-');
                SBskeyword.Replace('=', '-');
                SBskeyword.Replace("/", "-"); 
                SBskeyword.Replace("\\", "-");               
                SBskeyword.Replace("&", "-");
                SBskeyword.Replace("#", "-");            
                SBskeyword.Replace("%", "-");
                SBskeyword.Replace(":", "-");
                SBskeyword.Replace("\"", "-");
                SBskeyword.Replace("+", "-");
                SBskeyword.Replace("(", "-");
                SBskeyword.Replace(")", "-");
                SBskeyword.Replace("ω", "-");
                SBskeyword.Replace("”", "-");
                SBskeyword.Replace("“", "-");
                SBskeyword.Replace("™", "-");
                SBskeyword.Replace("*", "-");
                SBskeyword.Replace("→", "-");  
                SBskeyword.Replace("%E2%84%A2", "-");
                SBskeyword.Replace("-------", "-");
                SBskeyword.Replace("-------", "-");
                SBskeyword.Replace("------", "-");
                SBskeyword.Replace("-----", "-");
                SBskeyword.Replace("----", "-");
                SBskeyword.Replace("---", "-");
                SBskeyword.Replace("--", "-");              
                skeyword = SBskeyword.ToString();
                skeyword = skeyword.ToUpper();

                string[] arr = newkeyword.Split(new char[] { ' ', '/','=', '\\', '&', '#',  '%' ,'-',':','"','+','(',')','ω','”','“','™','*','→'},
                   StringSplitOptions.RemoveEmptyEntries);
                 repkey = newkeyword;
                 StringBuilder newSBskeyword = new StringBuilder(repkey);
                 string missedwords = string.Empty;
                //Loop that change the given string with special character into 909..
                //***Start 
                for (int i = 0; i <= arr.Length - 1; i++)
                 {
                     string[] checksameword = newSBskeyword.ToString().Split(new string[] { arr[i] }, StringSplitOptions.None);
                     if (checksameword.Length <= 2)
                     {
                         newSBskeyword.Replace(arr[i], "9");
                     }
                     else
                     {
                         if (missedwords == string.Empty)
                         {
                             missedwords = arr[i];
                         }
                         else
                         {
                             missedwords = missedwords+":"+arr[i];
                         }
                     }
                 }
                if(missedwords!="")
                {
                    string[] secondrep = missedwords.Split(':');
                    for (int i = 0; i <= secondrep.Length - 1; i++)
                    {

                        newSBskeyword = newSBskeyword.Replace(secondrep[i], "9"); 
                    }
                }
                    newSBskeyword.Replace("_", "a0");
                     newSBskeyword.Replace(":", "a1");
                     newSBskeyword.Replace("\"", "a2");
                     newSBskeyword.Replace("+", "a3");
                     newSBskeyword.Replace("(", "a4");
                     newSBskeyword.Replace(")", "a5");
                     newSBskeyword.Replace("ω", "a6");
                     newSBskeyword.Replace("“", "a7");
                    newSBskeyword.Replace("”", "a8");                
                     newSBskeyword.Replace("™", "a9");
                     newSBskeyword.Replace("*", "b1");
                     newSBskeyword.Replace("→", "b2");                  
                     newSBskeyword.Replace(" ", "0");
                     newSBskeyword.Replace("-", "1");
                     newSBskeyword.Replace("=", "2");
                     newSBskeyword.Replace("#", "3");                   
                     newSBskeyword.Replace("%", "5");
                     newSBskeyword.Replace("/", "6");
                     newSBskeyword.Replace("&", "7");
                     newSBskeyword.Replace("\\", "8");    
                     repkey = newSBskeyword.ToString();

 
                
            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog(ex.ToString() + "URLstringreplace");
            }
            //If the word ends with (.) then ./ will throw error show that is replaced with .~
             if ((skeyword.EndsWith(".")))
                {
                  
                    skeyword = skeyword.Remove(skeyword.Length - 1, 1)+".~";
                }
             repkey = repkey.Replace("th", "").Replace("a9","a4");
             if ((equalskey.EndsWith(".")))
                {
                  
                    equalskey = equalskey.Remove(equalskey.Length - 1, 1)+".~";
                }
             skeyword = skeyword.ToLower();
             if (skeyword== "fl")
             { 
             skeyword = skeyword.Replace("fl", "~fl~");
             }
             else if(skeyword == "pl")
             {
              skeyword = skeyword.Replace("pl", "~pl~");
             }
             else if(skeyword == "ct")
             {
             
               skeyword = skeyword.Replace("ct", "~ct~");
             }
            else if(skeyword == "pd")
             {
             
               skeyword = skeyword.Replace("pd", "~pd~");
             }
            else if(skeyword== "bb")
             {
             
               skeyword = skeyword.Replace("bb", "~bb~");
             }
            else if(skeyword == "ps")
             {
             
               skeyword = skeyword.Replace("ps", "~ps~");
             }
             else if (skeyword == "bp")
             {

                 skeyword = skeyword.Replace("bp", "~bp~");
             }
             else if (skeyword== "bk")
             {

                 skeyword = skeyword.Replace("bk", "~bk~");
             }
            return skeyword + "/" + repkey + "/" + equalskey;
        }
        public string URlStringReverse(string StrRawurl)
        {
           
            try
            {

                if ((StrRawurl.EndsWith("/ct/")))
                {

                    string[] sturl = StrRawurl.Split(new string[] { "/ct/" }, StringSplitOptions.None);
                    StrRawurl = sturl[0] + "/ct/";
                    HttpContext.Current.Response.RedirectPermanent(StrRawurl,false);
                }

                else if ( (StrRawurl.EndsWith("/bb/")))
                {



                    string[] sturl = StrRawurl.Split(new string[] { "/bb/" }, StringSplitOptions.None);
                    StrRawurl = sturl[0] + "/bb/";
                    HttpContext.Current.Response.RedirectPermanent(StrRawurl,false);

                }
                else if ((StrRawurl.EndsWith("/ps/")))
                {



                    string[] sturl = StrRawurl.Split(new string[] { "/ps/" }, StringSplitOptions.None);
                    StrRawurl = sturl[0] + "/ps/";

                    HttpContext.Current.Response.RedirectPermanent(StrRawurl,false);
                }
                else if ( (StrRawurl.EndsWith("/fl/")))
                {


                    string[] sturl = StrRawurl.Split(new string[] { "/fl/" }, StringSplitOptions.None);
                    StrRawurl = sturl[0] + "/fl/";

                    HttpContext.Current.Response.RedirectPermanent(StrRawurl,false);
                }
                else if ( (StrRawurl.EndsWith("/pd/")))
                {


                    string[] sturl = StrRawurl.Split(new string[] { "/pd/" }, StringSplitOptions.None);
                    StrRawurl = sturl[0] + "/pd/";

                    HttpContext.Current.Response.RedirectPermanent(StrRawurl,false);
                }
                else if ( (StrRawurl.EndsWith("/pl/")))
                {



                    string[] sturl = StrRawurl.Split(new string[] { "/pl/" }, StringSplitOptions.None);
                    StrRawurl = sturl[0] + "/pl/";

                    HttpContext.Current.Response.RedirectPermanent(StrRawurl,false);
                }

            }
            catch (System.Threading.ThreadAbortException)
            {
                // ignore it
            }
            catch (Exception ex)
            {
                // ignore it
            }



            string formname = string.Empty;
            string ynewurl = string.Empty;
            try
            {
                if ((StrRawurl.EndsWith("/")))
                {
                    StrRawurl = StrRawurl.Substring(0, StrRawurl.Length - 1);
                }
                if ((StrRawurl.EndsWith(".")))
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
                      
                        newSBskeyword.Replace("a0", "_");
                        newSBskeyword.Replace("a1", ":");
                        newSBskeyword.Replace("a2", "\"");
                        newSBskeyword.Replace("a3", "+");
                        newSBskeyword.Replace("a4", "(");
                        newSBskeyword.Replace("a5", ")");
                        newSBskeyword.Replace("a6", "ω");
                        newSBskeyword.Replace("a7", "“");
                        newSBskeyword.Replace("a8", "”");
                        newSBskeyword.Replace("a9", "™");
                        newSBskeyword.Replace("b1", "*");
                        newSBskeyword.Replace("b2", "→");
                        newSBskeyword.Replace("0", " ");
                        newSBskeyword.Replace("1", "-");
                        newSBskeyword.Replace("2", "=");
                        newSBskeyword.Replace("3", "#");
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
                                if ((ishypen_st))
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
                      
                        orgstring.Replace("→", "<ars>g</ars>");
                        orgstring.Replace("→", "&rarr;");
                        orgstring.Replace("→", "rarr;");
                        if ((orgstring.Contains('/')))
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

                            string[] hypensplit = equalssplit[i].Replace(".~", ".").Replace("~-", "~_").Replace("-~","%").Replace("~", " ").Split(new string[] { "-" }, StringSplitOptions.None);
                            if (hypensplit.Length > 1)
                            {
                                newmainnewurl = "";
                                if (y == 1)
                                {
                                    newmainnewurl = mainnewurl[1];
                                }
                                if (y == 1 && Convert.ToInt32(hypensplit[0]) == y - 1)
                                {
                                    newmainnewurl = hypensplit[1].Replace("~_", "-").Replace("_~","%").Replace("~", " ") + "=" + newmainnewurl;
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
              
                string newstrrawurl=string.Empty  ;
                if ((StrRawurl.StartsWith("/")))
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
                    string strvalue = Cons_NewURl_bybrand("ps.aspx?srctext=" + HttpUtility.UrlEncode(sturl[0]) + "", sturl[0], "ps.aspx", "");
                    strvalue = "/" + strvalue + "/ps/";
                    HttpContext.Current.Response.RedirectPermanent(strvalue,false); 
                }
                return newstrrawurl;
            }
        }



        public string URlStringReverse_MS(string StrRawurl)
        {
            
            string formname = string.Empty;
            string ynewurl = string.Empty;
            try
            {
                if ((StrRawurl.EndsWith("/")))
                {
                    StrRawurl = StrRawurl.Substring(0, StrRawurl.Length - 1);
                }
                else  if ((StrRawurl.EndsWith(".")))
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

                      
                        urlkey = strrawurl[strrawurl.Length - 2];
                        surlkey = strrawurl[strrawurl.Length - 2].Split('-');
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
                        string[] hypenstr = null;
                        if (strurl != "")
                        {
                         hypenstr=   strurl.Split('-');
                        }




                        string removestrkey = surlkey[j];
                        StringBuilder newSBskeyword = new StringBuilder(removestrkey);
                       
                        newSBskeyword.Replace("a0", "_");
                        newSBskeyword.Replace("a1", ":");
                        newSBskeyword.Replace("a2", "\"");
                        newSBskeyword.Replace("a3", "+");
                        newSBskeyword.Replace("a4", "(");
                        newSBskeyword.Replace("a5", ")");
                        newSBskeyword.Replace("a6", "ω");
                        newSBskeyword.Replace("a7", "“");
                        newSBskeyword.Replace("a8", "”");
                        newSBskeyword.Replace("a9", "™");
                        newSBskeyword.Replace("b1", "*");
                        newSBskeyword.Replace("b2", "→");
                        newSBskeyword.Replace("0", " ");
                        newSBskeyword.Replace("1", "-");
                        newSBskeyword.Replace("2", "=");
                        newSBskeyword.Replace("3", "#");                
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
                                if ((ishypen_st))
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
                      
                        orgstring.Replace("→", "<ars>g</ars>");
                        orgstring.Replace("→", "&rarr;");
                        orgstring.Replace("→", "rarr;");
                        if ((orgstring.Contains('/')))
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
                              
                                if (Convert.ToInt32(hypensplit[0]) == y )
                                {
                                    newmainnewurl = hypensplit[1].Replace("~_", "-").Replace("_~", "%").Replace("~", " ") + "=" + mainnewurl[y];
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
                return formname +"/"+ ynewurl.ToString();
            }
            catch
            {
              
                string newstrrawurl = string.Empty;
                if ((StrRawurl.StartsWith("/")))
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
                    string strvalue = Cons_NewURl_bybrand("ps.aspx?srctext=" + HttpUtility.UrlEncode(sturl[0]) + "", sturl[0], "ps.aspx", "");
                    strvalue = "/" + strvalue + "/ps/";
                    HttpContext.Current.Response.RedirectPermanent(strvalue);
                }
                return newstrrawurl;
            }
        }
        
   
        public void Createnewdt()
        {


            DataTable dt = new DataTable();
            dt.Columns.Add("NewURL");
            dt.Columns.Add("ORGURL");
            dt.Columns.Add("ATRTYPE");
            HttpContext.Current.Session["URLINI"] = dt;


        }

        public void Createnewdt_Home()
        {


            DataTable dt = new DataTable();
            dt.Columns.Add("NewURL");
            dt.Columns.Add("ORGURL");
            dt.Columns.Add("ATRTYPE");
                HttpContext.Current.Session["URLINIHOME"] = dt;

        }
        public void Createnewdt_Login()
        {


            DataTable dt = new DataTable();
            dt.Columns.Add("NewURL");
            dt.Columns.Add("ORGURL");
            dt.Columns.Add("ATRTYPE");
                  HttpContext.Current.Session["URLINILOGIN"] = dt;

        }
        //Added by :Indu
        //For URLREwrite
        public void InsertToDt(string stlistprod, string ORGURL, string ATRTYPE)
        {

            try
            {
                 DataTable dtcheck = (DataTable)HttpContext.Current.Session["URLINI"];
                    DataRow[] foundRows;

                    foundRows = dtcheck.Select("NEWURL='" + stlistprod + "' ");
                    if (foundRows.Length <= 0)
                    {

                        DataTable dt = (DataTable)HttpContext.Current.Session["URLINI"];
                        DataRow dr = dt.NewRow();
                        dr["NewURL"] = stlistprod;
                        dr["ORGURL"] = ORGURL;
                        dr["ATRTYPE"] = ATRTYPE;

                        dt.Rows.Add(dr);
                        HttpContext.Current.Session["URLINI"] = dt;
                    }


                if ((HttpContext.Current.Request.Url.ToString().ToUpper().Contains("/Home.aspx")))
                {
                    DataTable dtHOME = (DataTable)HttpContext.Current.Session["URLINIHOME"];
                    DataRow drHOME = dtHOME.NewRow();
                    drHOME["NewURL"] = stlistprod;
                    drHOME["ORGURL"] = ORGURL;
                    drHOME["ATRTYPE"] = ATRTYPE;

                    dtHOME.Rows.Add(drHOME);
                    HttpContext.Current.Session["URLINIHOME"] = dtHOME;
                }

                if ((HttpContext.Current.Request.Url.ToString().ToUpper().Contains("/Login.aspx")))
                {
                    DataTable dtLOGIN = (DataTable)HttpContext.Current.Session["URLINILOGIN"];
                    DataRow dRLOGIN = dtLOGIN.NewRow();
                    dRLOGIN["NewURL"] = stlistprod;
                    dRLOGIN["ORGURL"] = ORGURL;
                    dRLOGIN["ATRTYPE"] = ATRTYPE;

                    dtLOGIN.Rows.Add(dRLOGIN);
                    HttpContext.Current.Session["URLINIHOME"] = dtLOGIN;
                }
            }
            catch (Exception ex)
            {

                //objErrorHandler.CreateLog(ex.ToString() + "InsertToDt");

            }


        }
        public void Insert_CIDTopDT(string cid,string cname)
        {
            if (HttpContext.Current.Session["dtcid"] == null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("cid");
                dt.Columns.Add("cname");
                DataRow dr = dt.NewRow();
                dr["cid"] = cid;
                dr["cname"] = cname;


                dt.Rows.Add(dr);
                HttpContext.Current.Session["dtcid"] = dt;
               
            }
            else
            {
                DataTable dt = (DataTable)HttpContext.Current.Session["dtcid"];

                DataRow[] foundRows;

                foundRows = dt.Select("cid='" + cid + "' ");
                    if (foundRows.Length <= 0)
                    {
                        DataRow dr = dt.NewRow();
                        dr["cid"] = cid;
                        dr["cname"] = cname;


                        dt.Rows.Add(dr);
                        HttpContext.Current.Session["dtcid"] = dt;
                    }
               
            }
           
        
        }
        public string Cons_NewURl_bybrand(string originalurl, string EAPATH, string FormName, string Atrtype)
        {
            try
            {


                //string[] ConsURL = EAPATH.Replace("UserSearch1=", "").Replace("UserSearch=", "").
                //  Replace("UserSearch", "").Replace("=", "/").Replace("'", "").
                //  Replace("AttribSelect=", "").
                //  Split(new string[] { "////" }, StringSplitOptions.None);
                string[] ConsURL = EAPATH.Replace("//// //// ////","").
                 Replace("AttribSelect=", "").Replace("UserSearch=", "").
                 Split(new string[] { "////" }, StringSplitOptions.None);

                if (ConsURL.Length >= 2)
                {
                    if (ConsURL[0].ToLower().Contains("allproducts") || ConsURL[0].ToLower().Contains("all products"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                    if (ConsURL[0].ToUpper().Contains("WESAUSTRALASIA"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                   


                }
             
                string stlistprod = string.Empty;
                
                string lastkey = string.Empty;
                string equalskey = string.Empty;
                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 0)
                    {
                         string replacedword= URLstringreplace_New(ConsURL[i]);
                        string[] srepword = replacedword.Split('/');
                        stlistprod = srepword[0];
                        lastkey = srepword[1];
                        if (srepword[2] != "")
                        {
                            equalskey = "0-" + srepword[2];
                        }
                    }
                    else
                    {
                        
            
                        string repconsurl = string.Empty;
                        string replacedword = URLstringreplace_New(ConsURL[i]);
                        string[] srepword = replacedword.Split('/');
                        // stlistprod =stlistprod+"/"+ srepword[0];
                        lastkey = lastkey + "-" + srepword[1];
                        repconsurl = srepword[0];
                        stlistprod = repconsurl + "/" + stlistprod;
                        if (srepword[2] != "")
                        {
                            if (equalskey != "")
                            {
                                equalskey = equalskey + "/" + i + "-" + srepword[2];
                            }
                            else
                            {
                                equalskey = i + "-" + srepword[2];
                            }
                        }
                 
                    }
                }
                stlistprod = stlistprod.Replace("///", "").ToLower();
               

                if ((stlistprod.ToString().EndsWith(".")))
                {
                    stlistprod = stlistprod.Substring(0, stlistprod.Length - 1);
                }
                if (equalskey != string.Empty)
                {
                    equalskey = "xx" + equalskey;
                }
                 if (stlistprod.StartsWith("/"))
                        {
                            stlistprod = stlistprod.Substring(1, stlistprod.Length - 1);
                        }
                return stlistprod + "/" + lastkey + equalskey;
            }
            catch (Exception ex)
            {
                
                return originalurl;
            }
        }

        public string Cons_NewURl_bybrand_MS(string originalurl, string EAPATH, string FormName, string Atrtype)
        {
            try
            {


       
                string[] ConsURL = EAPATH.Replace("//// //// ////", "").
                 Replace("AttribSelect=", "").Replace("UserSearch=", "").
                 Split(new string[] { "////" }, StringSplitOptions.None);

                if (ConsURL.Length >= 2)
                {
                    if (ConsURL[0].ToLower().Contains("allproducts") || ConsURL[0].ToLower().Contains("all products"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();
                    if (ConsURL[0].ToUpper().Contains("WESAUSTRALASIA"))
                        ConsURL = ConsURL.Where(w => w != ConsURL[0]).ToArray();



                }
                //int i = Array.IndexOf(ConsURL, ConsURL.Where(x => x.Contains(":")).FirstOrDefault());
                //if (i > 0)
                //{
                //    string[] tempcons = ConsURL[i].Split(':');
                //    ConsURL[i] = tempcons[1];
                //}

                //string stlistprod = URLstringreplace(string.Join("/", ConsURL));
                string stlistprod = string.Empty;

                string lastkey = string.Empty;
                string equalskey = string.Empty;
                for (int i = 0; i <= ConsURL.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        string replacedword = URLstringreplace_New(ConsURL[i]);
                        string[] srepword = replacedword.Split('/');
                        stlistprod = srepword[0];
                        lastkey = srepword[1];
                        if (srepword[2] != "")
                        {
                            equalskey = "0-" + srepword[2];
                        }
                    }
                    else
                    {

                        //if (ConsURL[i].ToLower().Contains("brand") == true)
                        //{
                        //    strmodel = ConsURL[i].ToLower().Replace("brand=", "");
                        //}
                        //if (ConsURL[i].ToLower().Contains("model") == true)
                        //{
                        //    ConsURL[i] = ConsURL[i].ToLower().Replace(strmodel + "..", "");
                        //}
                        string repconsurl = string.Empty;
                        string replacedword = URLstringreplace_New(ConsURL[i]);
                        string[] srepword = replacedword.Split('/');
                        // stlistprod =stlistprod+"/"+ srepword[0];
                        lastkey = lastkey + "-" + srepword[1];
                        repconsurl = srepword[0];
                        stlistprod =  stlistprod+"/"+repconsurl ;
                        if (srepword[2] != "")
                        {
                            if (equalskey != "")
                            {
                                equalskey = equalskey + "/" + i + "-" + srepword[2];
                            }
                            else
                            {
                                equalskey = i + "-" + srepword[2];
                            }
                        }
                        // stlistprod = stlistprod + "/" + URLstringreplace_New(ConsURL[i]);
                    }
                }
                stlistprod = stlistprod.Replace("///", "").ToLower();
                //if (originalurl != string.Empty)
                //{

                //    InsertToDt(stlistprod, originalurl.Replace("amp;", ""), Atrtype);
                //}



                if ((stlistprod.ToString().EndsWith(".")))
                {
                    stlistprod = stlistprod.Substring(0, stlistprod.Length - 1);
                }
                if (equalskey != "")
                {
                    equalskey = "xx" + equalskey;
                }
                if (stlistprod.StartsWith("/"))
                {
                    stlistprod = stlistprod.Substring(1, stlistprod.Length - 1);
                }
                return stlistprod + "/" + lastkey + equalskey;
            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl_bybrand");
                return originalurl;
            }
        }

        public string Cons_NewURl_POWERSEARCH(string originalurl, string EAPATH, string FormName, string Atrtype)
        {
            try
            {

               
                string stlistprod = string.Empty;
                string lastkey = string.Empty;
                string equalskey = string.Empty;
               
                        string replacedword = URLstringreplace_New(EAPATH );
                        string[] srepword = replacedword.Split('/');
                        stlistprod = srepword[0];
                        lastkey = srepword[1];
                       
 return stlistprod.ToLower() + "/" + lastkey ;
                      
                }
               
               
               
            
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog(ex.ToString() + "Cons_NewURl_POWERSEARCH");
                return originalurl;
            }
        }
        //public void urlbulkinsert()
        //{

        //    try
        //    {


        //        if (HttpContext.Current.Session["URLINI"] != null)
        //        {
        //            // Int32 c = 0;
        //            string val = string.Empty;
        //            DataTable dt = (DataTable)HttpContext.Current.Session["URLINI"];

        //            dt = dt.DefaultView.ToTable(true, "NewURL", "ORGURL", "ATRTYPE").Copy();

        //            if (HttpContext.Current.Session["URLINIHOME"] == null)
        //            {
        //                HttpContext.Current.Session["URLINIHOME"] = dt;
        //                HttpContext.Current.Session["URLINILOGIN"] = dt;
        //            }



        //            objHelper.BulkExecuteSQLQueryDB(dt);

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.CreateLog(ex.ToString() + "URLRewriteToAddressBarNew");
        //    }

        //}
        public void writelog(string newurl)
        {
            try
            {
                //  System.IO.StreamWriter log;
                ////  string path = HttpContext.Current.Server.MapPath("log_palani.txt");

                //  //log = System.IO.File.AppendText(HttpContext.Current.Server.MapPath("logfile_2.txt"));
                //  // log = System.IO.File.AppendText("C:\\WES WEBSITE\\WAGNER1.0\\logfile.txt");


                //   // log = System.IO.File.AppendText(path);
                //  log.WriteLine(newurl);
                //  log.Close();

                System.IO.StreamWriter log;


                log = System.IO.File.AppendText(HttpContext.Current.Server.MapPath("logfile_2.txt"));
                // log = System.IO.File.AppendText("C:\\WES WEBSITE\\WAGNER1.0\\testlog\\logfile_2.txt");
               // log = System.IO.File.AppendText("E:\\CatalogStudio\\Wagner_2_0\\WAGNER\\logfile_2.txt");
                log.WriteLine(newurl);
                log.Close();
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
                else
                    cost = Convert.ToDouble(cost).ToString("#0.00");

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
                            if ((validUser))
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
            for (int i = 0; i <= dt.Rows.Count-1; i++)
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


        public  string StripWhitespace(string body)
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

    }
    /*********************************** J TECH CODE ***********************************/
}