using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    /// <summary>
    /// This is used to get the Company Details
    /// </summary>
    /// <remarks>
    /// Used to get the Company Details like Company Name,Status of Company,Get Company Admin Name List...
    /// </remarks>
    /// <example>
    /// CompanyGroup oCom = new CompanyGroup();
    /// </example>
    public class CompanyGroupServices
    {

        /*********************************** DECLARATION ***********************************/        
        /// <summary>
        /// Create the Structure for Company Related Information
        /// </summary>
        /// <example>
        /// CompanyGroup.CompanyInfo oCI = new CompanyGroup.CompanyInfo();
        /// oCI.CompanyName;
        /// </example>
        public struct CompanyInfo
        {
            /// <summary>
            /// Company ID
            /// </summary>
            public string CompanyID;
            /// <summary>
            /// Company Name
            /// </summary>
            public string CompanyName;
            /// <summary>
            /// Company TaxID
            /// </summary>
            public string TaxID;
            /// <summary>
            /// Company Address 1
            /// </summary>
            public string Address1;
            /// <summary>
            /// Company Address 2
            /// </summary>
            public string Address2;
            /// <summary>
            /// Company Address 3
            /// </summary>
            public string Address3;
            /// <summary>
            /// Company City
            /// </summary>
            public string City;
            /// <summary>
            /// Company State
            /// </summary>
            public string State;
            /// <summary>
            /// Company Zip Code
            /// </summary>
            public string Zip;
            /// <summary>
            /// Company Country
            /// </summary>
            public string Country;
            /// <summary>
            /// Company Phone Number 1
            /// </summary>
            public string Phone1;
            /// <summary>
            /// Company Phone Number 2
            /// </summary>
            public string Phone2;
            /// <summary>
            /// Company TollFree Number
            /// </summary>
            public string TollFree;
            /// <summary>
            /// Company Fax Number
            /// </summary>
            public string Fax;
            /// <summary>
            /// Company Status
            /// </summary>
            public CompanyStatus Status;
            /// <summary>
            /// Company Email Id
            /// </summary>
            public string Email;
            /// <summary>
            /// Company CSC Code
            /// </summary>
            public string CSC;
            /// <summary>
            /// Company WebSite Address
            /// </summary>
            public string Web;
            /// <summary>
            /// Company Contact Person 
            /// </summary>
            public string ContactName;
            /// <summary>
            /// Company Pay Method
            /// </summary>
            public string PayMethod;
            /// <summary>
            /// Buyer's Group
            /// </summary>
            public string BuyerGroup;

        }
        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        CompanyGroupDB objCompanyGroupDB = new CompanyGroupDB();
        HelperServices objHelperServices = new HelperServices();
        private int _mCompanyUser;
        private string _CompID_Type = "custom";

        /// <summary>
        /// Create Enum for Company Status Information
        /// </summary>
        public enum CompanyStatus
        {
            /// <summary>
            /// New Comapny
            /// </summary>
            NEWCOMPANY = 0,
            /// <summary>
            /// Active Company
            /// </summary>
            ACTIVE = 1,
            /// <summary>
            /// InActive Company
            /// </summary>
            INACTIVE = 2

        }

        /*********************************** DECLARATION ***********************************/      

        /// <summary>
        /// Default Constructor
        /// </summary>

        /*********************************** CONSTRUCTOR ***********************************/
        public CompanyGroupServices()
        {

        }
        /*********************************** CONSTRUCTOR ***********************************/

        #region "Properties"

        /// <summary>
        /// Set the Property for Company User 
        /// </summary>
        public int CompanyUser
        {
            get
            {
                return _mCompanyUser;
            }
            set
            {
                _mCompanyUser = value;
            }
        }
        public string CompID_Type
        {
            get
            {
                return _CompID_Type;
            }
            set
            {
                _CompID_Type = value;
            }
        }
        #endregion

        #region "Functions..."
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to create Profile of the Company.
        /// </summary>
        /// <param name="oCI">CompanyInfo</param>
        /// <returns>int</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     CompanyGroup.CompanyInfo oCI;
        ///     int retVal;
        ///     ...
        ///     retVal = oCom.CreateProfile(oCI);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CREATE NEW COMPANY DETAILS ***/
        /********************************************************************************/

        public int CreateProfile(CompanyInfo oCI)
        {
            try
            {
                //string sSQL = " INSERT INTO TBWC_COMPANY(";
                //sSQL = sSQL + " COMPANY_ID,COMPANY_NAME,EIN_OR_TAXID,ADDRESS_LINE1,";
                //sSQL = sSQL + " ADDRESS_LINE2,ADDRESS_LINE3,CITY,STATE,";
                //sSQL = sSQL + " ZIP,COUNTRY,PHONE1,PHONE2,";
                //sSQL = sSQL + " TOLLFREE_PHONE,FAX,EMAIL,";
                //sSQL = sSQL + " COMPANY_STATUS,CORP_SECURITY_CODE,WEB,";
                //sSQL = sSQL + " CREATED_USER,MODIFIED_USER,";
                //sSQL = sSQL + " PRIMARY_CONTACT_NAME,PREFFERED_PAY_MTHD,BUYER_GROUP)";
                //sSQL = sSQL + " VALUES('" + oCI.CompanyID + "','" + oCI.CompanyName.Replace("'","''") + "','" + oCI.TaxID + "','" + oCI.Address1.Replace("'","''") + "','";
                //sSQL = sSQL + oCI.Address2.Replace("'", "''") + "','" + oCI.Address3.Replace("'", "''") + "','" + oCI.City.Replace("'", "''") + "','" + oCI.State.Replace("'", "''") + "','";
                //sSQL = sSQL + oCI.Zip + "','" + oCI.Country.Replace("'", "''") + "','" + oCI.Phone1 + "','" + oCI.Phone2 + "','";
                //sSQL = sSQL + oCI.TollFree + "','" + oCI.Fax + "','" + oCI.Email.Replace("'", "''") + "','";
                //sSQL = sSQL + oCI.Status + "','" + oCI.CSC + "','" + oCI.Web + "',";
                //sSQL = sSQL + _mCompanyUser + "," + _mCompanyUser + ",'";
                //sSQL = sSQL + oCI.ContactName.Replace("'", "''") + "','" + oCI.PayMethod + "','" + oCI.BuyerGroup + "')";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string sSQL = "Exec STP_TBWC_POP_COMPANY ";
                sSQL = sSQL + "'" + oCI.CompanyID + "','" + oCI.CompanyName.Replace("'","''") + "','" + oCI.TaxID + "','" + oCI.Address1.Replace("'","''") + "','";
                sSQL = sSQL + oCI.Address2.Replace("'", "''") + "','" + oCI.Address3.Replace("'", "''") + "','" + oCI.City.Replace("'", "''") + "','" + oCI.State.Replace("'", "''") + "','";
                sSQL = sSQL + oCI.Zip + "','" + oCI.Country.Replace("'", "''") + "','" + oCI.Phone1 + "','" + oCI.Phone2 + "','";
                sSQL = sSQL + oCI.TollFree + "','" + oCI.Fax + "','" + oCI.Email.Replace("'", "''") + "','";
                sSQL = sSQL + oCI.Status + "','" + oCI.CSC + "','" + oCI.Web + "',";
                sSQL = sSQL + _mCompanyUser + "," + _mCompanyUser + ",'";
                sSQL = sSQL + oCI.ContactName.Replace("'", "''") + "','" + oCI.PayMethod + "','" + oCI.BuyerGroup + "'";
                return objHelperDB.ExecuteSQLQueryDB(sSQL); 

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Company Id
        /// </summary>
        /// <param name="CompanyName">string</param>
        /// <returns>int</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompanyName;
        ///     string CompanyID;
        ///     ...
        ///     CompanyID = oCom.GetCompanyID(CompanyName);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE COMPANY ID ***/
        /********************************************************************************/
        public string GetCompanyID(string CompanyName)
        {
            string tempstr="";
            try
            {
                string CompanyID = "";
                DataTable dtCompany = new DataTable();
                //string sSQL = " SELECT COMPANY_ID FROM TBWC_COMPANY";
                //sSQL = sSQL + " WHERE COMPANY_NAME='" + CompanyName + "'";
                //oHelper.SQLString = sSQL;
                //dtCompany = oHelper.GetDataTable();
                //if (dtCompany.Rows.Count > 0)
                //{
                //    CompanyID = dtCompany.Rows[0].ItemArray[0].ToString();
                //}
                tempstr = (string)objCompanyGroupDB.GetGenericDataDB(CompanyName, "GET_COMPANY_ID", CompanyGroupDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    CompanyID = tempstr;
  
                
                return CompanyID;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return "";
            }
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to check whether the Company Exists or Not.
        /// </summary>
        /// <param name="CompanyID">string</param>
        /// <returns>bool</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompanyID;
        ///     bool isValid;
        ///      ...
        ///     isValid = oCom.IsCompanyExist(CompanyID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER THE COMPANY ALREADY EXIST OR NOT ***/
        /********************************************************************************/
        public bool IsCompanyExist(string CompanyID)
        {
            bool isValid = false;
            try
            {
                DataTable dtCompany = new DataTable();
                //string sSQL = " SELECT COMPANY_NAME FROM TBWC_COMPANY";
                //sSQL = sSQL + " WHERE COMPANY_ID='" + CompanyID + "'";
                //sSQL = sSQL + " AND COMPANY_STATUS ='" + CompanyStatus.ACTIVE.ToString() + "'";
                //oHelper.SQLString = sSQL;
                //dtCompany = oHelper.GetDataTable();
                //if (dtCompany.Rows.Count > 0)
                //{
                    //isValid = true;
                //}

                dtCompany = (DataTable)objCompanyGroupDB.GetGenericDataDB("", CompanyID, CompanyStatus.ACTIVE.ToString(), "GET_COMPANY_EXIST", CompanyGroupDB.ReturnType.RTTable);
                if (dtCompany != null && dtCompany.Rows.Count>0   )
                    isValid = true;

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return isValid;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to check the User of the Company Exist or Not
        /// </summary>
        /// <param name="CompanyID">string</param>
        /// <returns>bool</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompanyID;
        ///     bool isExist;
        ///      ...
        ///     isExist = oCom.IsCompanyUserExist(CompanyID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK COMPANY USER ALREADY EXIST OR NOT ***/
        /********************************************************************************/
        public bool IsCompanyUserExist(string CompanyID)
        {
            bool isValid = false;
            try
            {
                DataTable dtCompany = new DataTable();
                //string sSQL = " SELECT USER_ID FROM TBWC_COMPANY_BUYERS";
                //sSQL = sSQL + " WHERE COMPANY_ID='" + CompanyID + "'";
                //oHelper.SQLString = sSQL;
                //dtCompany = oHelper.GetDataTable();
                //if (dtCompany.Rows.Count > 0)
                //{
                //    isValid = true;
                //}
                dtCompany = (DataTable)objCompanyGroupDB.GetGenericDataDB(CompanyID, "GET_COMPANY_USER_EXIST", CompanyGroupDB.ReturnType.RTTable);
                if (dtCompany != null && dtCompany.Rows.Count > 0)
                    isValid = true;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return isValid;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to check the Company Status
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     int Userid;
        ///     string chkStatus; 
        ///      ...
        ///     chkStatus = oCom.CheckCompanyStatus(Userid);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE COMPANY STATUS ***/
        /********************************************************************************/
        public string CheckCompanyStatus(int UserID)
        {
            string CompanyStatus = "";
            try
            {
                //string sSQL = " SELECT COMPANY_STATUS FROM TBWC_COMPANY WHERE COMPANY_ID =(";
                //sSQL = sSQL + " SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = " + UserID + ")";
                //oHelper.SQLString = sSQL;
                //CompanyStatus = oHelper.GetValue("COMPANY_STATUS");
                CompanyStatus = (string)objCompanyGroupDB.GetGenericDataDB(UserID.ToString() , "GET_COMPANY_STATUS", CompanyGroupDB.ReturnType.RTString);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return CompanyStatus;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Company Informations using Company Name
        /// </summary>
        /// <param name="CompanyName">string</param>
        /// <returns>CompanyInfo</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompName;
        ///     CompanyInfo oCI;
        ///     ...
        ///     oCI = oCom.GetCompanyInfo(CompName);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE COMPANY DETAILS ***/
        /********************************************************************************/
        public CompanyInfo GetCompanyInfo(string CompanyName)
        {
            DataSet dsCom = new DataSet();
            CompanyInfo oCI = new CompanyInfo();
            try
            {
                //string sSQL = " SELECT * FROM TBWC_COMPANY";
                //sSQL = sSQL + " WHERE COMPANY_NAME ='" + CompanyName.Replace("'","''") + "'";
                //oHelper.SQLString = sSQL;
                //dsCom = oHelper.GetDataSet("Company");
                dsCom = (DataSet)objCompanyGroupDB.GetGenericDataDB(CompanyName.Replace("'", "''"), "GET_COMPANY", CompanyGroupDB.ReturnType.RTDataSet);
                if (dsCom != null)
                {
                    dsCom.Tables[0].TableName = "Company";
                    foreach (DataRow row in dsCom.Tables["Company"].Rows)
                    {
                        oCI.CompanyID = row["COMPANY_ID"].ToString();
                        oCI.CompanyName = objHelperServices.CS(row["COMPANY_NAME"]);
                        oCI.Address1 = objHelperServices.CS(row["ADDRESS_LINE1"]);
                        oCI.Address2 = objHelperServices.CS(row["ADDRESS_LINE2"]);
                        oCI.Address3 = objHelperServices.CS(row["ADDRESS_LINE3"]);
                        oCI.City = objHelperServices.CS(row["CITY"]);
                        oCI.State = objHelperServices.CS(row["STATE"]);
                        oCI.Zip = objHelperServices.CS(row["ZIP"]);
                        oCI.Country = objHelperServices.CS(row["COUNTRY"]);
                        oCI.Phone1 = objHelperServices.CS(row["PHONE1"]);
                        oCI.Phone2 = objHelperServices.CS(row["PHONE2"]);
                        oCI.TollFree = objHelperServices.CS(row["TOLLFREE_PHONE"]);
                        oCI.Fax = objHelperServices.CS(row["FAX"]);
                        oCI.Web = objHelperServices.CS(row["WEB"]);
                        oCI.Email = objHelperServices.CS(row["EMAIL"]);
                        oCI.ContactName = objHelperServices.CS(row["PRIMARY_CONTACT_NAME"]);
                        oCI.CSC = objHelperServices.CS(row["CORP_SECURITY_CODE"]);

                        oCI.PayMethod = objHelperServices.CS(row["PREFFERED_PAY_MTHD"]);
                        oCI.Status = (CompanyStatus)row["COMPANY_STATUS"];
                        oCI.BuyerGroup = objHelperServices.CS(row["BUYER_GROUP"]);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
            return oCI;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Company Name
        /// </summary>
        /// <param name="CompanyID">string</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompanyID;
        ///     string CompGroup;
        ///     ...
        ///     CompGroup = oCom.GetCompanyName(CompanyID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE COMPANY NAME  ***/
        /********************************************************************************/

        public string GetCompanyName(string CompanyID)
        {
            string RetVal = "";
            try
            {
                //string sSQL = " SELECT COMPANY_NAME FROM TBWC_COMPANY";
                //sSQL = sSQL + " WHERE COMPANY_ID='" + CompanyID + "'";
                //oHelper.SQLString = sSQL;
                //RetVal = oHelper.GetValue("COMPANY_NAME");
                RetVal = (string)objCompanyGroupDB.GetGenericDataDB(CompanyID, "GET_COMPANY_NAME", CompanyGroupDB.ReturnType.RTString); 

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                RetVal = "-1";
            }
            return RetVal;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Details of the Company Administrator
        /// </summary>
        /// <param name="CompanyID">int</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompanyID;
        ///     string RetVal;
        ///     ...
        ///     RetVal = oCom.GetCompanyAdmin(CompanyID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE COMPANY ADMINISTRATOR DETAILS  ***/
        /********************************************************************************/
        public string GetCompanyAdmin(string CompanyID)
        {
            string RetVal = "";
            string tempstr = "";
            try
            {
                //string sSQL = " SELECT WCU.USER_EMAIL FROM TBWC_USER WCU,TBWC_COMPANY_BUYERS WCC";
                //sSQL = sSQL + " WHERE WCU.USER_ID = WCC.USER_ID  ";
                //sSQL = sSQL + " AND WCU.USER_ROLE ='" + TradingBell.Common.User.UserRole.COMPANYADMIN.ToString() + "'";
                //sSQL = sSQL + " AND  WCC.COMPANY_ID ='" + CompanyID + "'";
                //oHelper.SQLString = sSQL;
                //RetVal = oHelper.GetValue("USER_EMAIL");
                tempstr = (string)objCompanyGroupDB.GetGenericDataDB("", UserServices.UserRole.COMPANYADMIN.ToString(), CompanyID, "GET_COMPANY_USER_EMAIL", CompanyGroupDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    RetVal = tempstr;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                RetVal = "-1";
            }
            return RetVal;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to check whether the Company Details Exists or Not
        /// </summary>
        /// <param name="CompanyName">string</param>
        /// <returns>bool</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     CompanyGroup oCom = new CompanyGroup();
        ///     string CompanyName;
        ///     bool isExists;
        ///     ...
        ///     isExists = oCom.isCompanyNameExists(CompanyName);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER THE COMPANY NAME ALREADY EXIST OR NOT  ***/
        /********************************************************************************/
        public bool isCompanyNameExists(string CompanyName)
        {
            bool RetVal = false;
            string tempstr = "";
            string ComName = "";
            try
            {
                //string sSQL = " SELECT COMPANY_NAME FROM TBWC_COMPANY";
                //sSQL = sSQL + " WHERE COMPANY_Name='" + CompanyName.Replace("'","''") + "'";
                //oHelper.SQLString = sSQL;
                //ComName = oHelper.GetValue("COMPANY_NAME");
                //if (ComName != string.Empty)
                //{
                //    RetVal = true;
                //}
                tempstr = (string)objCompanyGroupDB.GetGenericDataDB(CompanyName, "GET_COMPANY_ID", CompanyGroupDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    RetVal = true;
  

                
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                RetVal = true;
            }
            return RetVal;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER THE COMPANY ID ALREADY EXIST OR NOT  ***/
        /********************************************************************************/
        public bool isCompanyIDExists(string CompanyID)
        {
            bool RetVal = false;
            //string ComID = "";

            try
            {
                //string sSQL = " SELECT COMPANY_ID FROM TBWC_COMPANY";
                //sSQL = sSQL + " WHERE COMPANY_ID='" + CompanyID + "'";
                //oHelper.SQLString = sSQL;
                //ComID = oHelper.GetValue("COMPANY_ID");
                //if (ComID != string.Empty)
                //{
                //    RetVal = true;
                //}
                string tempstr = (string)objCompanyGroupDB.GetGenericDataDB(CompanyID, "GET_COMPANY_NAME", CompanyGroupDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    RetVal = true;

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                RetVal = true;
            }
            return RetVal;
        }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO GENERATE NEW COMPANY ID BASED ON OLD COMPANY ID  ***/
        /********************************************************************************/
        public string GenerateCompanyID()
        {
            int TempCompID = 0;
            //string CompanyID = "";
            if (_CompID_Type == "auto")
            {
                //string sSql = "SELECT MAX(CONVERT(INT,COMPANY_ID)) AS COMPANY_ID FROM TBWC_COMPANY";
                //oHelper.SQLString = sSql;

                //if (oHelper.GetValue("COMPANY_ID") == null)
                //{
                //    TempCompID = 10000;
                //}
                //else
                //{
                //    TempCompID = oHelper.CI(oHelper.GetValue("COMPANY_ID").ToString());
                //}
                string tempstr = (string)objCompanyGroupDB.GetGenericDataDB("", "GET_COMPANY_MAX_ID", CompanyGroupDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    TempCompID = objHelperServices.CI(tempstr);
                else
                    TempCompID = 10000;

                TempCompID = TempCompID + 1;
            }
            return TempCompID.ToString();
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER ID  ***/
        /********************************************************************************/
        public DataTable GetForgotUserId(string EmailAddr,string CusType)
        {
            DataTable dt = new DataTable();
            try
            {

                dt = (DataTable)objCompanyGroupDB.GetGenericDataDB("", EmailAddr.ToString(), CompanyStatus.ACTIVE.ToString(),CusType.ToString(), "GET_COMPANY_STATUS_FORGOT_USER_ID", CompanyGroupDB.ReturnType.RTTable);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return dt;
        }
        #endregion

    }
    /*********************************** J TECH CODE ***********************************/
}