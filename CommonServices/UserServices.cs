using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;

/// <summary>
/// 
/// </summary>

namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    /// <summary>
    /// This is used to get and return all the User Information
    /// </summary>
    /// <remarks>
    /// Implementing the users login informations.
    /// Updation,Creation of records (Users).
    /// Getting User Information like Shipping Details,Billing Details,Status,Role,Security Questions....
    /// </remarks>
    /// <example>
    /// User oUser=new User();
    /// </example>
    public class UserServices
    {
        /*********************************** DECLARATION ***********************************/
        #region "Declarations..."

        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperService = new HelperServices();
       // OrderServices objOrderServices = new OrderServices();
        /// <summary>
        /// Create Structure for User Related Information 
        /// </summary>
        /// <example>
        ///  User.UserInfo oUsrInfo = new User.UserInfo();
        /// oUsrInfo.FirstName.ToString();
        /// </example>
        public struct UserInfo
        {
            /// <summary>
            /// User's ID
            /// </summary>
            public int UserID;
            /// <summary>
            /// User's Company ID
            /// </summary>
            public string CompanyID;
            /// <summary>
            /// User's Email
            /// </summary>
            public string Email;
            /// <summary>
            /// User's Password
            /// </summary>
            public string Password;
            /// <summary>
            /// User's Prefix
            /// </summary>
            public string Prefix;
            /// <summary>
            /// User's First Name
            /// </summary>
            public string FirstName;
            /// <summary>
            /// User's Last Name
            /// </summary>
            public string LastName;
            /// <summary>
            /// User's Middle Name
            /// </summary>
            public string MiddleName;
            /// <summary>
            /// User's Suffix
            /// </summary>
            public string Suffix;
            /// <summary>
            /// User's Address 1
            /// </summary>
            public string Address1;
            /// <summary>
            /// User's Address 2
            /// </summary>
            public string Address2;
            /// <summary>
            /// User's Address 3
            /// </summary>
            public string Address3;
            /// <summary>
            /// User's City
            /// </summary>
            public string City;
            /// <summary>
            /// User's State
            /// </summary>
            public string State;
            /// <summary>
            ///  User's Zip Code
            /// </summary>
            public string Zip;
            /// <summary>
            /// User's Country
            /// </summary>
            public string Country;
            /// <summary>
            /// User's Alternate Email Id
            /// </summary>
            public string AlternateEmail;
            /// <summary>
            /// Home Phone Number
            /// </summary>
            public string Phone;
            /// <summary>
            /// Mobile Number
            /// </summary>
            public string MobilePhone;
            /// <summary>
            /// Fax
            /// </summary>
            public string Fax;
            /// <summary>
            /// User Status(Active / InActive)
            /// </summary>
            public int Status;
            /// <summary>
            /// Comments
            /// </summary>
            public string Comments;
            /// <summary>
            /// User's Online Status
            /// </summary>
            public int isOnline;
            /// <summary>
            /// Password Question
            /// </summary>
            public string Pwd_Question1;
            /// <summary>
            /// Password Answer
            /// </summary>
            public string Pwd_Answer1;
            /// <summary>
            /// Purchase Order
            /// </summary>
            public int isPO_Payment;
            /// <summary>
            /// Credit Card
            /// </summary>
            public int isCC_Payment;
            /// <summary>
            /// Cash On Delivery
            /// </summary>
            public int isCOD_Payment;
            /// <summary>
            /// User's Role
            /// </summary>
            public UserRole oUserRole;

            //For Shipping Details
            /// <summary>
            ///  Shipping Address 1
            /// </summary>
            public string ShipAddress1;
            /// <summary>
            ///  Shipping Address 2
            /// </summary>
            public string ShipAddress2;
            /// <summary>
            /// Shipping Address 3
            /// </summary>
            public string ShipAddress3;
            /// <summary>
            /// Shipping City
            /// </summary>
            public string ShipCity;
            /// <summary>
            /// Shipping State
            /// </summary>
            public string ShipState;
            /// <summary>
            /// Shipping Zip Code
            /// </summary>
            public string ShipZip;
            /// <summary>
            /// Shipping Country
            /// </summary>
            public string ShipCountry;
            /// <summary>
            /// Shipping Phone
            /// </summary>
            public string ShipPhone;
            //For Billing Details
            /// <summary>
            /// Billing Address 1
            /// </summary>
            public string BillAddress1;
            /// <summary>
            /// Billing Address 2
            /// </summary>
            public string BillAddress2;
            /// <summary>
            /// Billing Address 3
            /// </summary>
            public string BillAddress3;
            /// <summary>
            /// Billing City
            /// </summary>
            public string BillCity;
            /// <summary>
            /// Billing State
            /// </summary>
            public string BillState;
            /// <summary>
            /// Billing Zip Code
            /// </summary>
            public string BillZip;
            /// <summary>
            /// Billing Country
            /// </summary>
            public string BillCountry;
            /// <summary>
            /// Billing Phone
            /// </summary>
            public string BillPhone;
            /// <summary>
            /// Billing Phone
            /// </summary>
            public string Contact;
            /// <summary>
            /// Contact
            /// </summary>
            public string LoginName;
            /// <summary>
            /// Login_Name
            /// </summary>
            public int USERROLE;
            /// <summary>
            /// CUSTOMER_TYPE
            /// </summary>
            public string CUSTOMER_TYPE;
            /// <summary>
            /// COMPANY_NAME
            /// </summary>
            public string COMPANY_NAME;
            /// <summary>
            /// SUbscribe
            /// </summary>
            public int Subscribe;

            public string Receiver_Contact;
            public string Bill_Company;
            public string Bill_Name;
            public string DELIVERYINST;
            public string Ship_Phone;

        }


        public struct RegistrationInfo
        {

            public string Customer_Type;
            public string SiteID;

            public string CustStatus;

            public string CompanyName;

            public string Address1;

            public string Address2;

            public string SubCity;

            public string State;

            public string PostZipcode;

            public string Country;

            public string AbnAcn;

            public string Fname;

            public string Lname;

            public string Position;

            public string Phone;

            public string Fax;

            public string Email;

            public string Mobile;

            public string WesAccNo;

            public string LastInvNo;

            public string BusinessType;

            public string BusinessDsc;

            public string Status;

            public string RegType;

            public string IpAddr;
            public string Password;
        }


        /// <summary>
        /// Create Enum Values for User Status Information
        /// </summary>
        /// <example>
        /// User.UserStatus.MANUALVERIFY;
        /// </example>
        public enum UserStatus
        {
            /// <summary>
            /// New User
            /// </summary>
            NEWUSER = 0,
            /// <summary>
            /// Active User
            /// </summary>
            ACTIVE = 1,
            /// <summary>
            /// InActive User
            /// </summary>
            INACTIVE = 2,
            /// <summary>
            /// Locked User
            /// </summary>
            LOCKED = 3,
            //RESET = 4,
            /// <summary>
            /// Manual Verified User
            /// </summary>
            MANUALVERIFY = 4,

            /// DELETED 
            DELETED = 5,

            ///RESET = 6
            RESET = 6
        }
        /// <summary>
        /// Create Enum for User Roles
        /// </summary>
        /// <example>
        /// User.UserRole.COMPANYADMIN;
        /// </example>
        public enum UserRole
        {
            /// <summary>
            /// Company Administrator
            /// </summary>
            COMPANYADMIN = 0,

            /// <summary>
            /// Store Administrator
            /// </summary>
            STOREADMIN = 1,
            /// <summary>
            /// Shopper 
            /// </summary>
            SHOPPER = 2
        }
        /// <summary>
        /// Store Administrator's Email ID
        /// </summary>
        public static int _StoreAdminID = 999;

        #endregion
        /*********************************** DECLARATION ***********************************/


        #region "Functions..."
        /// <summary>
        /// Default Constructor
        /// </summary>
        /*********************************** CONSTRUCTOR ***********************************/
        public UserServices()
        {
        }
        /*********************************** CONSTRUCTOR ***********************************/
        /// <summary>
        /// This is used to create the User Record
        /// </summary>
        /// <param name="oUser">UserInfo</param>
        /// <returns>integer</returns>
        /// <example>
        /// <code>


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CREATE NEW USER DETAILS  ***/
        /********************************************************************************/
        public int CreateUser(UserInfo oUser)
        {
            try
            {
                //string sSQL = " INSERT INTO TBWC_USER(USER_EMAIL,USER_PASSWORD,USER_PREFIX,USER_FNAME,USER_LNAME,USER_MNAME,";
                //sSQL = sSQL + " USER_SUFFIX,ADDRESS_LINE1,ADDRESS_LINE2,ADDRESS_LINE3,CITY,STATE,ZIP,COUNTRY,ALTERNATE_EMAIL,PHONE,";
                //sSQL = sSQL + " MOBILE_PHONE,FAX,USER_STATUS,COMMENTS,IS_ONLINE,PWD_QUESTION1,PWD_ANSWER1,";
                //sSQL = sSQL + " CREATED_USER,MODIFIED_USER,PO_PAYMENT_APPROVED,";
                //sSQL = sSQL + " CC_PAYMENT_APPROVED,COD_PAYMENT_APPROVED,USER_ROLE,";
                //sSQL = sSQL + " SHIP_ADDRESS_LINE1,SHIP_ADDRESS_LINE2,SHIP_ADDRESS_LINE3,SHIP_CITY,SHIP_STATE,SHIP_ZIP,SHIP_COUNTRY,";
                //sSQL = sSQL + " SHIP_PHONE,BILL_ADDRESS_LINE1,BILL_ADDRESS_LINE2,BILL_ADDRESS_LINE3,BILL_CITY,BILL_STATE,BILL_ZIP,BILL_COUNTRY,";
                //sSQL = sSQL + " BILL_PHONE )";
                //sSQL = sSQL + " VALUES('" + oUser.Email + "','" + oUser.Password + "','" + oUser.Prefix + "','" + oUser.FirstName.Replace("'", "''") + "','";
                //sSQL = sSQL + oUser.LastName.Replace("'", "''") + "','" + oUser.MiddleName.Replace("'", "''") + "','" + oUser.Suffix + "','" + oUser.Address1.Replace("'", "''") + "','" + oUser.Address2.Replace("'", "''") + "','";
                //sSQL = sSQL + oUser.Address3.ToString().Replace("'", "''") + "','" + oUser.City.Replace("'", "''") + "','" + oUser.State.Replace("'", "''") + "','" + oUser.Zip + "','" + oUser.Country.Replace("'", "''") + "','" + oUser.AlternateEmail + "','";
                //sSQL = sSQL + oUser.Phone + "','" + oUser.MobilePhone + "','" + oUser.Fax + "','" + oUser.Status + "','" + oUser.Comments + "',";
                //sSQL = sSQL + oUser.isOnline + ",'" + oUser.Pwd_Question1.Replace("'", "''") + "','" + oUser.Pwd_Answer1.Replace("'", "''") + "',";
                //sSQL = sSQL + _StoreAdminID + "," + _StoreAdminID + "," + oUser.isPO_Payment + ",";
                //sSQL = sSQL + oUser.isCC_Payment + "," + oUser.isCOD_Payment + ",'" + oUser.oUserRole + "','" + oUser.ShipAddress1.Replace("'", "''") + "','";
                //sSQL = sSQL + oUser.ShipAddress2.Replace("'", "''") + "','" + oUser.ShipAddress3.Replace("'", "''") + "','" + oUser.ShipCity.Replace("'", "''") + "','" + oUser.ShipState.Replace("'", "''") + "','" + oUser.ShipZip + "','" + oUser.ShipCountry.Replace("'", "''") + "','" + oUser.ShipPhone + "','";
                //sSQL = sSQL + oUser.BillAddress1.Replace("'", "''") + "','" + oUser.BillAddress2.Replace("'", "''") + "','" + oUser.BillAddress3.Replace("'", "''") + "','" + oUser.BillCity.Replace("'", "''") + "','" + oUser.BillState.Replace("'", "''") + "','" + oUser.BillZip + "','" + oUser.BillCountry.Replace("'", "''") + "','" + oUser.BillPhone + "')";

                string sSQL =" Exec STP_TBWC_POPSaveUser ";
                sSQL = sSQL + " '" + oUser.Email + "','" + oUser.Password + "','" + oUser.Prefix + "','" + oUser.FirstName.Replace("'", "''") + "','";
                sSQL = sSQL + oUser.LastName.Replace("'", "''") + "','" + oUser.MiddleName.Replace("'", "''") + "','" + oUser.Suffix + "','" + oUser.Address1.Replace("'", "''") + "','" + oUser.Address2.Replace("'", "''") + "','";
                sSQL = sSQL + oUser.Address3.ToString().Replace("'", "''") + "','" + oUser.City.Replace("'", "''") + "','" + oUser.State.Replace("'", "''") + "','" + oUser.Zip + "','" + oUser.Country.Replace("'", "''") + "','" + oUser.AlternateEmail + "','";
                sSQL = sSQL + oUser.Phone + "','" + oUser.MobilePhone + "','" + oUser.Fax + "','" + oUser.Status + "','" + oUser.Comments + "',";
                sSQL = sSQL + oUser.isOnline + ",'" + oUser.Pwd_Question1.Replace("'", "''") + "','" + oUser.Pwd_Answer1.Replace("'", "''") + "',";
                sSQL = sSQL + _StoreAdminID + "," + _StoreAdminID + "," + oUser.isPO_Payment + ",";
                sSQL = sSQL + oUser.isCC_Payment + "," + oUser.isCOD_Payment + ",'" + oUser.oUserRole + "','" + oUser.ShipAddress1.Replace("'", "''") + "','";
                sSQL = sSQL + oUser.ShipAddress2.Replace("'", "''") + "','" + oUser.ShipAddress3.Replace("'", "''") + "','" + oUser.ShipCity.Replace("'", "''") + "','" + oUser.ShipState.Replace("'", "''") + "','" + oUser.ShipZip + "','" + oUser.ShipCountry.Replace("'", "''") + "','" + oUser.ShipPhone + "','";
                sSQL = sSQL + oUser.BillAddress1.Replace("'", "''") + "','" + oUser.BillAddress2.Replace("'", "''") + "','" + oUser.BillAddress3.Replace("'", "''") + "','" + oUser.BillCity.Replace("'", "''") + "','" + oUser.BillState.Replace("'", "''") + "','" + oUser.BillZip + "','" + oUser.BillCountry.Replace("'", "''") + "','" + oUser.BillPhone + "'";

                //oHelper.SQLString = sSQL;
                return objHelperDB.ExecuteSQLQueryDB(sSQL);      
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }

        }

        /// <summary>
        /// This is used to set the User's Communication Information
        /// </summary>
        /// <param name="oUser">UserInfo</param>
        /// <returns>integer</returns>
        /// <example>
        /// <code>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CREATE REGISTRATION DETAILS FOR USERS  ***/
        /********************************************************************************/
        public int  CreateRegistration(RegistrationInfo RegInfo)
        {
            try
            {
                //string sSQL = "INSERT INTO WES_CUST_REGFORM(SITE_ID,CUST_STATUS,COMPANY_NAME,ADDRESS_1,ADDRESS_2,SUB_CITY,STATE,";
                //sSQL += "POST_ZIPCODE,COUNTRY,ABN_ACN,FNAME,LNAME,POSITION,PHONE,FAX,EMAIL,WES_ACC_NO,LAST_INV_NO,BUSINESS_TYPE,";
                //sSQL += "BUSINESS_DSC,IP_ADDR,STATUS,MOBILE,REG_TYPE)VALUES('" + RegInfo.SiteID.ToString() + "','" + RegInfo.CustStatus.ToString() + "','" + RegInfo.CompanyName.ToString() + "','" + RegInfo.Address1.ToString() + "','" + RegInfo.Address2.ToString() + "', '" + RegInfo.SubCity.ToString() + "','" + RegInfo.State.ToString() + "',";
                //sSQL += "'" + RegInfo.PostZipcode.ToString() + "','" + RegInfo.Country.ToString() + "','" + RegInfo.AbnAcn.ToString() + "','" + RegInfo.Fname.ToString() + "','" + RegInfo.Lname.ToString() + "','" + RegInfo.Position.ToString() + "','" + RegInfo.Phone.ToString() + "','" + RegInfo.Fax.ToString() + "','" + RegInfo.Email.ToString() + "','" + RegInfo.WesAccNo.ToString() + "','" + RegInfo.LastInvNo.ToString() + "','" + RegInfo.BusinessType.ToString() + "',";
                //sSQL += "'" + RegInfo.BusinessDsc.ToString() + "', '" + RegInfo.IpAddr.ToString() + "', '" + RegInfo.Status.ToString() + "','" + RegInfo.Mobile + "','" + RegInfo.RegType + "')";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
               
                string sSQL = "Exec STP_TBWC_POPSAVE_CUST_REGFORM ";
                sSQL += "'" + RegInfo.SiteID.ToString() + "','" + RegInfo.CustStatus.ToString() + "','" + RegInfo.CompanyName.ToString() + "','" + RegInfo.Address1.ToString() + "','" + RegInfo.Address2.ToString() + "', '" + RegInfo.SubCity.ToString() + "','" + RegInfo.State.ToString() + "',";
                sSQL += "'" + RegInfo.PostZipcode.ToString() + "','" + RegInfo.Country.ToString() + "','" + RegInfo.AbnAcn.ToString() + "','" + RegInfo.Fname.ToString() + "','" + RegInfo.Lname.ToString() + "','" + RegInfo.Position.ToString() + "','" + RegInfo.Phone.ToString() + "','" + RegInfo.Fax.ToString() + "','" + RegInfo.Email.ToString() + "','" + RegInfo.WesAccNo.ToString() + "','" + RegInfo.LastInvNo.ToString() + "','" + RegInfo.BusinessType.ToString() + "',";
                sSQL += "'" + RegInfo.BusinessDsc.ToString() + "', '" + RegInfo.IpAddr.ToString() + "', '" + RegInfo.Status.ToString() + "','" + RegInfo.Mobile + "','" + RegInfo.RegType + "','','','','" + RegInfo.Customer_Type + "','" + RegInfo.Password + "'";
                return objHelperDB.ExecuteSQLQueryDBRtnIdentity(sSQL);


            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }

        }
        public int UpdateRegistrationExpress(RegistrationInfo RegInfo)
        {
            try
            {
                string sSQL = "Exec STP_TBWC_RENEW_CUST_REGFORM_EXPRESS ";
                sSQL += "'" + RegInfo.SiteID.ToString() + "',";
                sSQL += "'" + RegInfo.Fname.ToString() + "','" + RegInfo.Lname.ToString() + "','" + RegInfo.Phone.ToString() + "','" + RegInfo.Email.ToString() + "',";
                sSQL += "'" + RegInfo.Password + "','" + RegInfo.IpAddr.ToString() + "','" + RegInfo.Mobile.ToString() + "'";
                return objHelperDB.ExecuteSQLQueryDBRtnIdentity(sSQL);


            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }



        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO DELETE REGISTRATION DETAILS  ***/
        /********************************************************************************/
        public int DeleteRegistration(double reg_id)
        {
            try
            {

                string sSQL = "Exec STP_TBWC_CANCEL_CUST_REGFORM " + reg_id.ToString();
                return objHelperDB.ExecuteSQLQueryDB(sSQL);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }

        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO SET USERS ROLE  ***/
        /********************************************************************************/
        public int SetUserRole(string  User_ID,int User_role)
        {
            try
            {


                string sSQL = "Exec STP_TBWC_RENEW_COMPANY_BUYERS ";
                sSQL = sSQL + "'',0," + User_ID + ",'','','UPDATE_USER_ROLE',0,'',0,0," + User_role.ToString() ;
                return objHelperDB.ExecuteSQLQueryDB(sSQL);  

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }

        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO SET USERS Subscribe  ***/
        /********************************************************************************/
        public int SetUserSubscriber(string User_ID, int Subscribe)
        {
            try
            {


                string sSQL = "Exec STP_TBWC_RENEW_COMPANY_BUYERS_SUBSCRIBE ";
                sSQL = sSQL + Subscribe.ToString() + "," + User_ID;
                return objHelperDB.ExecuteSQLQueryDB(sSQL);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }

        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE CUSTOMER REGISTRATION DETAILS BASED ON E-MAIL ADDRESS  ***/
        /********************************************************************************/
        public DataSet GetCustomerRegistrationDetails(string EmailAddress)
        {
            try
            {
                //string sSQL = "SELECT * FROM WES_CUST_REGFORM WHERE EMAIL = '" + EmailAddress + "'";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objHelperDB.GetGenericDataDB(EmailAddress, "GET_CUST_REGFORM", HelperDB.ReturnType.RTDataSet);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK CUSTOMER ALREADY REGISTERED OR NOT  ***/
        /********************************************************************************/
        public DataSet CheckCustomerRegistrationExists(string EmailAddress,string DealerRetailerType)
        {
            try
            {
                //string sSQL = "SELECT * FROM WES_CUST_REGFORM WHERE EMAIL = '" + EmailAddress + "'";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objHelperDB.GetGenericDataDB("",EmailAddress, DealerRetailerType, "GET_CUST_REGFORM_CHECK", HelperDB.ReturnType.RTDataSet);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        public DataSet CheckCustomerRegistrationExists_MYACCOUNT(string EmailAddress, string DealerRetailerType ,string USER_ID)
        {
            try
            {
                //string sSQL = "SELECT * FROM WES_CUST_REGFORM WHERE EMAIL = '" + EmailAddress + "'";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                //objErrorHandler.CreateLog(EmailAddress + USER_ID + "GET_CUST_REGFORM_CHECK_MYACCOUNT");
                return (DataSet)objHelperDB.GetGenericDataDB("", EmailAddress, DealerRetailerType,USER_ID, "GET_CUST_REGFORM_CHECK_MYACCOUNT", HelperDB.ReturnType.RTDataSet);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }

  

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE REGISTRATION DETAILS OF CUSTOMERS ***/
        /********************************************************************************/
        public RegistrationInfo GetRegistrationInfo(string EmailAddress)
        {
            DataSet dsCustomer = new DataSet();
            RegistrationInfo oRI = new RegistrationInfo();
            try
            {
                string siteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                dsCustomer = GetCustomerRegistrationDetails(EmailAddress);

                foreach (DataRow oDR in dsCustomer.Tables[0].Rows)
                {
                    oRI.SiteID = oDR["SITE_ID"].ToString();
                    oRI.CustStatus = oDR["CUST_STATUS"].ToString();
                    oRI.CompanyName = oDR["COMPANY_NAME"].ToString();
                    oRI.Address1 = oDR["ADDRESS_1"].ToString();
                    oRI.Address2 = oDR["ADDRESS_2"].ToString();
                    oRI.Fname = oDR["FNAME"].ToString();
                    oRI.Lname = oDR["LNAME"].ToString();
                    oRI.SubCity = oDR["SUB_CITY"].ToString();
                    oRI.State = oDR["STATE"].ToString();
                    oRI.PostZipcode = oDR["POST_ZIPCODE"].ToString();
                    oRI.Country = oDR["COUNTRY"].ToString();
                    oRI.AbnAcn = oDR["ABN_ACN"].ToString();
                    oRI.Position = oDR["POSITION"].ToString();
                    oRI.Email = oDR["EMAIL"].ToString();
                    oRI.Phone = oDR["PHONE"].ToString();
                    oRI.Mobile = oDR["MOBILE"].ToString();
                    oRI.Fax = oDR["FAX"].ToString();
                    oRI.WesAccNo = oDR["WES_ACC_NO"].ToString();
                    oRI.LastInvNo = oDR["LAST_INV_NO"].ToString();
                    oRI.BusinessType = oDR["BUSINESS_TYPE"].ToString();
                    oRI.BusinessDsc = oDR["BUSINESS_DSC"].ToString();
                    oRI.IpAddr = oDR["IP_ADDR"].ToString();
                }

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }

            return oRI;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE COMPANY BUYER DETAILS  ***/
        /********************************************************************************/
        public int UpdateUserInfo(UserInfo oUser)
        {
            try
            {

                string sSQL = "Exec STP_TBWC_RENEW_UPDATE_COMPANY_BUYERS '" + oUser.FirstName + " " + oUser.LastName + "'";                
                sSQL = sSQL + ",'" + oUser.Address1 + "','" + oUser.Address2 + "','" + oUser.Address3 + "','" + oUser.City + "','" + oUser.State + "','" + oUser.Zip + "',";
                sSQL = sSQL + "'" + oUser.Country + "','" + oUser.Phone + "','" + oUser.MobilePhone + "',";
                sSQL = sSQL + "'" + oUser.Fax + "',";
                sSQL = sSQL + "'" + oUser.ShipAddress1 + "','" + oUser.ShipAddress2 + "','" + oUser.ShipAddress3 + "',";
                sSQL = sSQL + "'" + oUser.ShipCity + "','" + oUser.ShipState + "','" + oUser.ShipZip + "','" + oUser.ShipCountry + "','" + oUser.ShipPhone + "',";
                sSQL = sSQL + "'" + oUser.BillAddress1 + "','" + oUser.BillAddress2 + "','" + oUser.BillAddress3 + "','" + oUser.BillCity + "',";
                sSQL = sSQL + "'" + oUser.BillState + "','" + oUser.BillZip + "','" + oUser.BillCountry + "','" + oUser.BillPhone + "',";
               sSQL = sSQL + " '" + oUser.COMPANY_NAME + "'," + oUser.UserID + ",'" + oUser.FirstName + "','" + oUser.LastName+"','1','"+ oUser.Email +"' ";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }


          public int UpdateUserInfo_loginExp(UserInfo oUser)
        {
            try
            {

                string sSQL = "Exec STP_TBWC_RENEW_UPDATE_COMPANY_BUYERS_LOGIN_Exp ";

                sSQL = sSQL + " " + oUser.UserID + ",'" + oUser.FirstName + "','" + oUser.LastName + "','" + oUser.Phone + "','" + oUser.Password + "'  ";

                sSQL = sSQL + " Exec STP_TBWC_UPDATE_MOBILE_NUMBER_USER ";
                sSQL = sSQL + "'" + oUser.MobilePhone + "'," + oUser.UserID + "";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

    

        public int UpdateUserInfoExpress(UserInfo oUser,string BusType,string ABN)
        {
            try
            {

                //string sSQL = "Exec STP_TBWC_RENEW_UPDATE_COMPANY_BUYERS_EXPRESS '" + oUser.ShipAddress1 + "','" + oUser.ShipAddress2 + "',";
                //sSQL = sSQL + "'" + oUser.ShipCity + "','" + oUser.ShipState + "','" + oUser.ShipZip + "','" + oUser.ShipCountry + "',";
                //sSQL = sSQL + "'" + oUser.BillAddress1 + "','" + oUser.BillAddress2 + "','" + oUser.BillCity + "',";
                //sSQL = sSQL + "'" + oUser.BillState + "','" + oUser.BillZip + "','" + oUser.BillCountry + "',";
                //sSQL = sSQL + " '" + oUser.COMPANY_NAME + "'," + oUser.UserID + ",'" + BusType + "','" + ABN + "' ,'"+oUser.Receiver_Contact +"','"+ oUser.Bill_Company +"','"+ oUser.Bill_Name+"','"+ oUser.DELIVERYINST +"'";
                //return objHelperDB.ExecuteSQLQueryDB(sSQL);
                string sSQL = "Exec STP_TBWC_RENEW_UPDATE_COMPANY_BUYERS_EXPRESS '" + oUser.ShipAddress1 + "','" + oUser.ShipAddress2 + "' ,";
                sSQL = sSQL + "'" + oUser.ShipCity + "','" + oUser.ShipState + "','" + oUser.ShipZip + "','" + oUser.ShipCountry + "',";
                sSQL = sSQL + "'" + oUser.BillAddress1 + "','" + oUser.BillAddress2 + "','" + oUser.BillCity + "',";
                sSQL = sSQL + "'" + oUser.BillState + "','" + oUser.BillZip + "','" + oUser.BillCountry + "',";
                sSQL = sSQL + " '" + oUser.COMPANY_NAME + "'," + oUser.UserID + ",'" + BusType + "','" + ABN + "' ,'" + oUser.Receiver_Contact + "','" + oUser.Bill_Company + "','" + oUser.Bill_Name + "','" + oUser.DELIVERYINST + "' ";
                objErrorHandler.CreateLog(sSQL);
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        /// <summary>
        /// This is used to set the User's Shipping Information
        /// </summary>
        /// <param name="oUser">UserInfo</param>
        /// <returns>integer</returns>
        /// <example>
        /// <code>

        /*********************************** CODE NOT USED ***********************************/   
        public int UpdateShippingInfo(UserInfo oUser)
        {
            try
            {
                /*string sSQL = " UPDATE TBWC_USER SET ";
                sSQL = sSQL + "SHIP_ADDRESS_LINE1='" + oUser.ShipAddress1 + "',SHIP_ADDRESS_LINE2='" + oUser.ShipAddress2 + "',SHIP_ADDRESS_LINE3='" + oUser.ShipAddress3 + "',";
                sSQL = sSQL + "SHIP_CITY='" + oUser.ShipCity + "',SHIP_STATE='" + oUser.ShipState + "',SHIP_ZIP='" + oUser.ShipZip + "',SHIP_COUNTRY='" + oUser.ShipCountry + "',SHIP_PHONE='" + oUser.ShipPhone + "'";
                sSQL = sSQL + "WHERE USER_ID=" + oUser.UserID;
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();*/
                return 1;
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        /*********************************** CODE NOT USED ***********************************/   
        /// <summary>
        /// This is used to set the User's Billing Information
        /// </summary>
        /// <param name="oUser">UserInfo</param>
        /// <returns>integer</returns>
        /// <example>
        /// <code>

        /*********************************** CODE NOT USED ***********************************/   
        public int UpdateBillingInfo(UserInfo oUser)
        {
            try
            {
                /*string sSQL = " UPDATE TBWC_USER SET ";
                sSQL = sSQL + "USER_FNAME='" + oUser.FirstName + "',USER_LNAME='" + oUser.LastName + "',BILL_ADDRESS_LINE1='" + oUser.BillAddress1 + "',BILL_ADDRESS_LINE2='" + oUser.BillAddress2 + "',BILL_ADDRESS_LINE3='" + oUser.BillAddress3 + "',BILL_CITY='" + oUser.BillCity + "',";
                sSQL = sSQL + "BILL_STATE='" + oUser.BillState + "',BILL_ZIP='" + oUser.BillZip + "',BILL_COUNTRY='" + oUser.BillCountry + "',BILL_PHONE='" + oUser.BillPhone + "' WHERE USER_ID=" + oUser.UserID;
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();*/
                return 1;
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        /*********************************** CODE NOT USED ***********************************/   


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE USER NAME  ***/
        /********************************************************************************/
        public int UpdateUserName(string Userpw, int UserID, string LoginName, string EmailAddress)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //string sSQL = " UPDATE TBWC_COMPANY_BUYERS SET ";
                //sSQL = sSQL + " PASSWORD='" + Userpw + "', USER_STATUS=6 ";
                //sSQL = sSQL + " WHERE WEBSITE_ID = " + websiteid + " AND USER_ID=" + UserID + " AND LOGIN_NAME='" + LoginName + "' AND ";
                //sSQL = sSQL + " EMAILADDR='" + EmailAddress + "'";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string sSQL = "Exec STP_TBWC_RENEW_COMPANY_BUYERS ";
                sSQL = sSQL + "'" + Userpw + "'," + websiteid + "," + UserID.ToString() + ",'" + LoginName + "','" + EmailAddress + "','UPDATE_PWD'";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);  
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE COMPANY BUYERS NEW PASSWORD  ***/
        /********************************************************************************/

        public int UpdateNewPassword(string LoginName, int UserID, int CompanyAccountNo, string EmailAddress, string TempPassword, string NewPassword)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //string sSQL = " UPDATE TBWC_COMPANY_BUYERS SET ";
                //sSQL = sSQL + " PASSWORD='" + NewPassword + "', USER_STATUS=1";
                //sSQL = sSQL + " WHERE WEBSITE_ID = " + websiteid + " AND USER_ID=" + UserID + " AND LOGIN_NAME='" + LoginName + "' AND ";
                //sSQL = sSQL + " PASSWORD='" + TempPassword + "' AND ";
                //sSQL = sSQL + " COMPANY_ID=" + CompanyAccountNo + " AND EMAILADDR='" + EmailAddress + "' AND USER_STATUS=6";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string sSQL = "Exec STP_TBWC_RENEW_COMPANY_BUYERS ";
                sSQL = sSQL + "'" + NewPassword + "'," + websiteid + "," + UserID.ToString() + ",'" + LoginName + "','" + EmailAddress + "','UPDATE_NEW_PWD'," + CompanyAccountNo + ",'" + TempPassword +"'";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);  
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to check the User's Email ID and Password are Correct or Wrong
        /// </summary>
        /// <param name="UserName">string</param>
        /// <param name="Password">string</param>
        /// <returns>bool</returns>
        /// <example>
        /// <code>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK USER LOGIN DETAILS ARE VALID OR NOT ***/
        /********************************************************************************/
        public bool CheckUser(string UserName, string Password)
        {
            bool isValidUser = false;
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                /*DataSet dsUser = new DataSet();
                
                string sSQL = "SELECT * FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND LOGIN_NAME = '" + UserName + "' ";

                // Old string sSQL = " SELECT * FROM TBWC_USER ";
               // sSQL = sSQL + "where USER_EMAIL = '" + UserName + "'";


                 sSQL = sSQL + "AND BINARY_CHECKSUM(PASSWORD) = BINARY_CHECKSUM('" + Password + "')";

                // Old sSQL = sSQL + "OR BINARY_CHECKSUM(USER_PASSWORD)=BINARY_CHECKSUM('" + Password + "'+'W@9$'))";
                oHelper.SQLString = sSQL;
                dsUser = oHelper.GetDataSet("ValidUser");*/
                DataSet dsUser = new DataSet();
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("",websiteid, UserName, Password, "GET_CEHECK_USER", HelperDB.ReturnType.RTDataSet);  

                if (dsUser != null)
                {
                    dsUser.Tables[0].TableName = "ValidUser";  
                    if (dsUser.Tables["ValidUser"].Rows.Count > 0)
                    {
                        isValidUser = true;
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                isValidUser = false;
            }
            return isValidUser;

        }
        /// <summary>
        /// This is used to set the New Password
        /// </summary>
        /// <param name="UserID">int</param>
        /// <param name="NewPassword">string</param>
        /// <returns>integer</returns>
        /// <example>
        /// <code>
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE USERS PASSWORD  ***/
        /********************************************************************************/
        public int ChangePassword(int UserID, string NewPassword)
        {
            try
            {
                //string sSQL;
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //sSQL = "UPDATE TBWC_COMPANY_BUYERS SET USER_STATUS = 1, PASSWORD='" + NewPassword + "' WHERE WEBSITE_ID = " + websiteid + " AND USER_ID=" + UserID;
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();

                string sSQL = "Exec STP_TBWC_RENEW_COMPANY_BUYERS ";
                sSQL = sSQL + "'" + NewPassword + "'," + websiteid + "," + UserID.ToString() + ",'','','CHANGE_PWD',0,''";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);  
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE USER NAME BASED ON USER ID  ***/
        /********************************************************************************/
        public int ChangeLoginName(int UserID, string NewLoginName)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

                string sSQL = "Exec STP_TBWC_RENEW_COMPANY_BUYERS ";
                sSQL = sSQL + "''," + websiteid + "," + UserID.ToString() + ",'"+ NewLoginName +"','','UPDATE_USER_NAME',0,''";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        /// <summary>
        /// This is used to get the Password for the User
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PASSWORD FROM COMPANY BUYERS  ***/
        /********************************************************************************/
        public string GetPassword(int UserID)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //string sSQL = "SELECT PASSWORD FROM TBWC_COMPANY_BUYERS WHERE USER_ID =" + UserID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetValue("PASSWORD");
                return (string)objHelperDB.GetGenericDataDB(UserID.ToString(), "GET_PASSWORD", HelperDB.ReturnType.RTString); 
                
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return "-1";

            }
        }
        /// <summary>
        /// This is used to get the User's Communication details
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>UserInfo</returns>
        /// <example>
        /// <code>

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public UserInfo GetUserInfo(int UserID)
        //{
        //    string sSQL;
        //    DataSet dsUser = new DataSet();
        //    UserInfo oUI = new UserInfo();

        //    try
        //    {
        //        string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //        //sSQL = "select * from TBWC_COMPANY TC, TBWC_COMPANY_BUYERS TCB WHERE TC.COMPANY_ID = TCB.COMPANY_ID AND TCB.WEBSITE_ID = " + websiteid + " AND TCB.USER_ID = " + UserID;
        //        //old sSQL = "SELECT * FROM TBWC_USER WHERE USER_ID=" + UserID;
        //        //oHelper.SQLString = sSQL;
        //        //dsUser = oHelper.GetDataSet("User");
        //        dsUser = (DataSet)objHelperDB.GetGenericDataDB("", websiteid, UserID.ToString(), "GET_USER_INFO", HelperDB.ReturnType.RTDataSet);
        //        dsUser.Tables[0].TableName = "User";
        //        foreach (DataRow oDR in dsUser.Tables["User"].Rows)
        //        {
        //            oUI.Prefix = ""; // oDR["USER_PREFIX"].ToString();
        //            oUI.FirstName = oDR["CONTACT"].ToString();
        //            oUI.LastName = " "; // oDR["USER_LNAME"].ToString();
        //            oUI.MiddleName = " "; // oDR["USER_MNAME"].ToString();
        //            oUI.Suffix = " "; // oDR["USER_SUFFIX"].ToString();
        //            oUI.Address1 = oDR["ADDRESS_LINE1"].ToString();
        //            oUI.Address2 = oDR["ADDRESS_LINE2"].ToString();
        //            oUI.Address3 = oDR["ADDRESS_LINE3"].ToString();
        //            oUI.City = oDR["CITY"].ToString();
        //            oUI.State = oDR["STATE"].ToString();
        //            oUI.Zip = oDR["ZIP"].ToString();
        //            oUI.Country = oDR["COUNTRY"].ToString();
        //            oUI.AlternateEmail = oDR["EMAILADDR"].ToString();
        //            oUI.Phone = oDR["PHONE"].ToString();
        //            oUI.MobilePhone = oDR["PHONE1"].ToString();
        //            oUI.Fax = oDR["FAX"].ToString();
        //            oUI.Status = (int)oDR["USER_STATUS"];
        //            //SHIPPING DETAILS
        //            oUI.ShipAddress1 = oDR["ADDRESS_LINE1"].ToString();
        //            oUI.ShipAddress2 = oDR["ADDRESS_LINE2"].ToString();
        //            oUI.ShipAddress3 = oDR["ADDRESS_LINE3"].ToString();
        //            oUI.ShipCity = oDR["CITY"].ToString();
        //            oUI.ShipState = oDR["STATE"].ToString();
        //            oUI.ShipZip = oDR["ZIP"].ToString();
        //            oUI.ShipCountry = oDR["COUNTRY"].ToString();
        //            oUI.ShipPhone = oDR["PHONE"].ToString();

        //            oUI.BillAddress1 = oDR["ADDRESS_LINE1"].ToString();
        //            oUI.BillAddress2 = oDR["ADDRESS_LINE2"].ToString();
        //            oUI.BillAddress3 = oDR["ADDRESS_LINE3"].ToString();
        //            oUI.BillCity = oDR["CITY"].ToString();
        //            oUI.BillState = oDR["STATE"].ToString();
        //            oUI.BillZip = oDR["ZIP"].ToString();
        //            oUI.BillCountry = oDR["COUNTRY"].ToString();
        //            oUI.BillPhone = oDR["PHONE1"].ToString();

        //            oUI.Comments = "";   // oDR["COMMENTS"].ToString();
        //            oUI.Contact = oDR["CONTACT"].ToString().ToUpper();
        //            oUI.LoginName = oDR["LOGIN_NAME"].ToString();
        //            oUI.USERROLE = objHelperService.CI(oDR["USER_ROLE"]);

        //            /*oUI.Prefix = oDR["USER_PREFIX"].ToString();
        //            oUI.FirstName = oDR["USER_FNAME"].ToString();
        //            oUI.LastName = oDR["USER_LNAME"].ToString();
        //            oUI.MiddleName = oDR["USER_MNAME"].ToString();
        //            oUI.Suffix = oDR["USER_SUFFIX"].ToString();
        //            oUI.Address1 = oDR["ADDRESS_LINE1"].ToString();
        //            oUI.Address2 = oDR["ADDRESS_LINE2"].ToString(); 
        //            oUI.Address3 = oDR["ADDRESS_LINE3"].ToString(); 
        //            oUI.City = oDR["CITY"].ToString(); 
        //            oUI.State = oDR["STATE"].ToString(); 
        //            oUI.Zip = oDR["ZIP"].ToString(); 
        //            oUI.Country = oDR["COUNTRY"].ToString(); 
        //            oUI.AlternateEmail = oDR["ALTERNATE_EMAIL"].ToString(); 
        //            oUI.Phone = oDR["PHONE"].ToString();
        //            oUI.MobilePhone = oDR["MOBILE_PHONE"].ToString();
        //            oUI.Fax = oDR["FAX"].ToString();
        //            oUI.Status = (int)oDR["USER_STATUS"];
        //            //SHIPPING DETAILS
        //            oUI.ShipAddress1 = oDR["SHIP_ADDRESS_LINE1"].ToString();
        //            oUI.ShipAddress2 = oDR["SHIP_ADDRESS_LINE2"].ToString();
        //            oUI.ShipAddress3 = oDR["SHIP_ADDRESS_LINE3"].ToString();
        //            oUI.ShipCity = oDR["SHIP_CITY"].ToString();
        //            oUI.ShipState = oDR["SHIP_STATE"].ToString();
        //            oUI.ShipZip = oDR["SHIP_ZIP"].ToString();
        //            oUI.ShipCountry = oDR["SHIP_COUNTRY"].ToString();
        //            oUI.ShipPhone = oDR["SHIP_PHONE"].ToString();

        //            oUI.BillAddress1 = oDR["BILL_ADDRESS_LINE1"].ToString();
        //            oUI.BillAddress2 = oDR["BILL_ADDRESS_LINE2"].ToString();
        //            oUI.BillAddress3 = oDR["BILL_ADDRESS_LINE3"].ToString();
        //            oUI.BillCity = oDR["BILL_CITY"].ToString();
        //            oUI.BillState = oDR["BILL_STATE"].ToString();
        //            oUI.BillZip = oDR["BILL_ZIP"].ToString();
        //            oUI.BillCountry = oDR["BILL_COUNTRY"].ToString();
        //            oUI.BillPhone = oDR["BILL_PHONE"].ToString();

        //            oUI.Comments = oDR["COMMENTS"].ToString(); */
        //            if (objHelperService.CI(oDR["USER_ONLINE"]) == 1)
        //            {
        //                oUI.isOnline = 1;
        //            }
        //            else
        //            {
        //                oUI.isOnline = 0;
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return oUI;

        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER FULL DETAILS ***/
        /********************************************************************************/
        public UserInfo GetUserInfo(int UserID)
        {
           // string sSQL;
            DataSet dsUser = new DataSet();
            UserInfo oUI = new UserInfo();

            try
            {
               // objErrorHandler.CreateLog(UserID.ToString() + DateTime.Now.ToString());

                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //sSQL = "select * from TBWC_COMPANY TC, TBWC_COMPANY_BUYERS TCB WHERE TC.COMPANY_ID = TCB.COMPANY_ID AND TCB.WEBSITE_ID = " + websiteid + " AND TCB.USER_ID = " + UserID;
                //old sSQL = "SELECT * FROM TBWC_USER WHERE USER_ID=" + UserID;
                //oHelper.SQLString = sSQL;
                //dsUser = oHelper.GetDataSet("User");
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("",websiteid, UserID.ToString(), "GET_USER_INFO", HelperDB.ReturnType.RTDataSet);
                if (dsUser != null)
                {
                    dsUser.Tables[0].TableName = "User";
                    foreach (DataRow oDR in dsUser.Tables["User"].Rows)
                    {
                        oUI.Prefix = ""; // oDR["USER_PREFIX"].ToString();
                        oUI.FirstName = oDR["FIRST_NAME"].ToString();   //oDR["CONTACT"].ToString();
                        oUI.LastName = oDR["LAST_NAME"].ToString(); ; // oDR["USER_LNAME"].ToString();
                        oUI.COMPANY_NAME = oDR["COMPANY_NAME"].ToString();
                        oUI.MiddleName = ""; // oDR["USER_MNAME"].ToString();
                        oUI.Suffix = ""; // oDR["USER_SUFFIX"].ToString();
                        oUI.Password = oDR["password"].ToString();
                        oUI.Email = oDR["EMAILADDR"].ToString();
                        oUI.Address1 = oDR["ADDRESS_LINE1"].ToString();
                        oUI.Address2 = oDR["ADDRESS_LINE2"].ToString();
                        oUI.Address3 = oDR["ADDRESS_LINE3"].ToString();
                        oUI.City = oDR["CITY"].ToString();
                        oUI.State = oDR["STATE"].ToString();
                        oUI.Zip = oDR["ZIP"].ToString();
                        oUI.Country = oDR["COUNTRY"].ToString();
                        oUI.AlternateEmail = oDR["EMAILADDR"].ToString();
                        oUI.Phone = oDR["PHONE"].ToString();
                        oUI.MobilePhone = oDR["MOBILE_PHONE"].ToString();
                        oUI.Fax = oDR["FAX"].ToString();
                        oUI.Status = (int)oDR["USER_STATUS"];
                        //SHIPPING DETAILS
                        oUI.ShipAddress1 = oDR["SHIP_ADDRESS_LINE1"].ToString();
                        oUI.ShipAddress2 = oDR["SHIP_ADDRESS_LINE2"].ToString();
                        oUI.ShipAddress3 = oDR["SHIP_ADDRESS_LINE3"].ToString();
                        oUI.ShipCity = oDR["SHIP_CITY"].ToString();
                        oUI.ShipState = oDR["SHIP_STATE"].ToString();
                        oUI.ShipZip = oDR["SHIP_ZIP"].ToString();
                        oUI.ShipCountry = oDR["SHIP_COUNTRY"].ToString();
                        oUI.ShipPhone = oDR["SHIP_PHONE"].ToString();

                        oUI.BillAddress1 = oDR["BILL_ADDRESS_LINE1"].ToString();
                        oUI.BillAddress2 = oDR["BILL_ADDRESS_LINE2"].ToString();
                        oUI.BillAddress3 = oDR["BILL_ADDRESS_LINE3"].ToString();
                        oUI.BillCity = oDR["BILL_CITY"].ToString();
                        oUI.BillState = oDR["BILL_STATE"].ToString();
                        oUI.BillZip = oDR["BILL_ZIP"].ToString();
                        oUI.BillCountry = oDR["BILL_COUNTRY"].ToString();
                        oUI.BillPhone = oDR["BILL_PHONE"].ToString();
                        oUI.Bill_Company = oDR["Bill_Company"].ToString();
                        oUI.Bill_Name = oDR["Bill_Name"].ToString();

                        oUI.Comments = "";   // oDR["COMMENTS"].ToString();
                        oUI.Contact = oDR["CONTACT"].ToString().ToUpper();
                        oUI.LoginName = oDR["LOGIN_NAME"].ToString();
                        oUI.USERROLE = objHelperService.CI(oDR["USER_ROLE"]);
                        oUI.CUSTOMER_TYPE = oDR["CUSTOMER_TYPE"].ToString();
                        oUI.CompanyID = oDR["COMPANY_ID"].ToString();
                        oUI.Receiver_Contact = oDR["Receiver_Contact"].ToString();
                        oUI.DELIVERYINST = oDR["DELIVERYINST"].ToString();
                        oUI.UserID = UserID;
                        /*oUI.Prefix = oDR["USER_PREFIX"].ToString();
                        oUI.FirstName = oDR["USER_FNAME"].ToString();
                        oUI.LastName = oDR["USER_LNAME"].ToString();
                        oUI.MiddleName = oDR["USER_MNAME"].ToString();
                        oUI.Suffix = oDR["USER_SUFFIX"].ToString();
                        oUI.Address1 = oDR["ADDRESS_LINE1"].ToString();
                        oUI.Address2 = oDR["ADDRESS_LINE2"].ToString(); 
                        oUI.Address3 = oDR["ADDRESS_LINE3"].ToString(); 
                        oUI.City = oDR["CITY"].ToString(); 
                        oUI.State = oDR["STATE"].ToString(); 
                        oUI.Zip = oDR["ZIP"].ToString(); 
                        oUI.Country = oDR["COUNTRY"].ToString(); 
                        oUI.AlternateEmail = oDR["ALTERNATE_EMAIL"].ToString(); 
                        oUI.Phone = oDR["PHONE"].ToString();
                        oUI.MobilePhone = oDR["MOBILE_PHONE"].ToString();
                        oUI.Fax = oDR["FAX"].ToString();
                        oUI.Status = (int)oDR["USER_STATUS"];
                        //SHIPPING DETAILS
                        oUI.ShipAddress1 = oDR["SHIP_ADDRESS_LINE1"].ToString();
                        oUI.ShipAddress2 = oDR["SHIP_ADDRESS_LINE2"].ToString();
                        oUI.ShipAddress3 = oDR["SHIP_ADDRESS_LINE3"].ToString();
                        oUI.ShipCity = oDR["SHIP_CITY"].ToString();
                        oUI.ShipState = oDR["SHIP_STATE"].ToString();
                        oUI.ShipZip = oDR["SHIP_ZIP"].ToString();
                        oUI.ShipCountry = oDR["SHIP_COUNTRY"].ToString();
                        oUI.ShipPhone = oDR["SHIP_PHONE"].ToString();

                        oUI.BillAddress1 = oDR["BILL_ADDRESS_LINE1"].ToString();
                        oUI.BillAddress2 = oDR["BILL_ADDRESS_LINE2"].ToString();
                        oUI.BillAddress3 = oDR["BILL_ADDRESS_LINE3"].ToString();
                        oUI.BillCity = oDR["BILL_CITY"].ToString();
                        oUI.BillState = oDR["BILL_STATE"].ToString();
                        oUI.BillZip = oDR["BILL_ZIP"].ToString();
                        oUI.BillCountry = oDR["BILL_COUNTRY"].ToString();
                        oUI.BillPhone = oDR["BILL_PHONE"].ToString();

                        oUI.Comments = oDR["COMMENTS"].ToString(); */
                        oUI.Subscribe = Convert.ToInt32(oDR["Subscribe"]);
                        if (objHelperService.CI(oDR["USER_ONLINE"]) == 1)
                        {
                            oUI.isOnline = 1;
                        }
                        else
                        {
                            oUI.isOnline = 0;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return oUI;

        }
        /// <summary>
        /// This is used to get the User's Shipping Information
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>UserInfo</returns>
        /// <example>
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USERS SHIPPING DETAILS ***/
        /********************************************************************************/
        public UserInfo GetUserShipInfo(int UserID)
        {
           // string sSQL;
            DataSet dsUser = new DataSet();
            UserInfo oUI = new UserInfo();
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //sSQL = "select * from TBWC_COMPANY TC, TBWC_COMPANY_BUYERS TCB WHERE TC.COMPANY_ID = TCB.COMPANY_ID AND TCB.WEBSITE_ID = " + websiteid + " AND TCB.USER_ID = " + UserID;
                // old sSQL = "SELECT * FROM TBWC_USER WHERE USER_ID=" + UserID;
                //oHelper.SQLString = sSQL;
                //dsUser = oHelper.GetDataSet("User");
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("",websiteid, UserID.ToString(), "GET_USER_INFO", HelperDB.ReturnType.RTDataSet);
                dsUser.Tables[0].TableName = "User";  
                foreach (DataRow oDR in dsUser.Tables["User"].Rows)
                {

                    oUI.FirstName = oDR["FIRST_NAME"].ToString();
                    oUI.LastName = oDR["LAST_NAME"].ToString();
                 //   oUI.MiddleName =   oDR["USER_MNAME"].ToString();
                    oUI.ShipAddress1 = oDR["SHIP_ADDRESS_LINE1"].ToString();
                    oUI.ShipAddress2 = oDR["SHIP_ADDRESS_LINE2"].ToString();
                    oUI.ShipAddress3 = oDR["SHIP_ADDRESS_LINE3"].ToString();
                    oUI.ShipCity = oDR["SHIP_CITY"].ToString();
                    oUI.ShipState = oDR["SHIP_STATE"].ToString();
                    oUI.ShipZip = oDR["SHIP_ZIP"].ToString();
                    oUI.ShipCountry = oDR["SHIP_COUNTRY"].ToString();
                    oUI.ShipPhone = oDR["SHIP_PHONE"].ToString();
                    oUI.COMPANY_NAME = oDR["COMPANY_NAME"].ToString();
                    oUI.Receiver_Contact = oDR["Receiver_Contact"].ToString();
                    oUI.DELIVERYINST = oDR["DELIVERYINST"].ToString();
                    oUI.Ship_Phone = oDR["SHIP_PHONE"].ToString();
                    /*oUI.FirstName = oDR["USER_FNAME"].ToString();
                    oUI.LastName = oDR["USER_LNAME"].ToString();
                    oUI.MiddleName = oDR["USER_MNAME"].ToString();
                    oUI.ShipAddress1 = oDR["SHIP_ADDRESS_LINE1"].ToString();
                    oUI.ShipAddress2 = oDR["SHIP_ADDRESS_LINE2"].ToString();
                    oUI.ShipAddress3 = oDR["SHIP_ADDRESS_LINE3"].ToString();
                    oUI.ShipCity = oDR["SHIP_CITY"].ToString();
                    oUI.ShipState = oDR["SHIP_STATE"].ToString();
                    oUI.ShipZip = oDR["SHIP_ZIP"].ToString();
                    oUI.ShipCountry = oDR["SHIP_COUNTRY"].ToString();
                    oUI.ShipPhone = oDR["SHIP_PHONE"].ToString();*/
                }

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();               
            }
            return oUI;
        }
        /// <summary>
        /// This is used to get the User's Billing Information
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>UserInfo</returns>
        /// <example>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USERS BILLING DETAILS FROM COMPANY BUYERS ***/
        /********************************************************************************/
        public UserInfo GetUserBillInfo(int UserID)
        {
           // string sSQL;
            DataSet dsUser = new DataSet();
            UserInfo oUI = new UserInfo();
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //sSQL = "select * from TBWC_COMPANY TC, TBWC_COMPANY_BUYERS TCB WHERE TC.COMPANY_ID = (select BILLTO_ID FROM WES_CUSTOMER WHERE WES_CUSTOMER_ID = TCB.COMPANY_ID) AND TCB.WEBSITE_ID = " + websiteid + " AND TCB.USER_ID = " + UserID;
                // old sSQL = "SELECT * FROM TBWC_USER WHERE USER_ID=" + UserID;
                //oHelper.SQLString = sSQL;
                //dsUser = oHelper.GetDataSet("User");
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("",websiteid, UserID.ToString(), "GET_USER_BILL_INFO", HelperDB.ReturnType.RTDataSet);
                dsUser.Tables[0].TableName = "User";  
                foreach (DataRow oDR in dsUser.Tables["User"].Rows)
                {
                    oUI.FirstName = oDR["FIRST_NAME"].ToString();
                    oUI.LastName = oDR["LAST_NAME"].ToString();
                   // oUI.MiddleName =oDR["USER_MNAME"].ToString();
                    oUI.BillAddress1 = oDR["BILL_ADDRESS_LINE1"].ToString();
                    oUI.BillAddress2 = oDR["BILL_ADDRESS_LINE2"].ToString();
                    oUI.BillAddress3 = oDR["BILL_ADDRESS_LINE3"].ToString();
                    oUI.BillCity = oDR["BILL_CITY"].ToString();
                    oUI.BillState = oDR["BILL_STATE"].ToString();
                    oUI.BillZip = oDR["BILL_ZIP"].ToString();
                    oUI.BillCountry = oDR["BILL_COUNTRY"].ToString();
                    oUI.BillPhone = oDR["BILL_PHONE"].ToString();
                    oUI.Bill_Company = oDR["Bill_Company"].ToString();
                    oUI.Bill_Name = oDR["Bill_Name"].ToString();
                    /*oUI.FirstName = oDR["USER_FNAME"].ToString();
                    oUI.LastName = oDR["USER_LNAME"].ToString();
                    oUI.MiddleName = oDR["USER_MNAME"].ToString();
                    oUI.BillAddress1 = oDR["BILL_ADDRESS_LINE1"].ToString();
                    oUI.BillAddress2 = oDR["BILL_ADDRESS_LINE2"].ToString();
                    oUI.BillAddress3 = oDR["BILL_ADDRESS_LINE3"].ToString();
                    oUI.BillCity = oDR["BILL_CITY"].ToString();
                    oUI.BillState = oDR["BILL_STATE"].ToString();
                    oUI.BillZip = oDR["BILL_ZIP"].ToString();
                    oUI.BillCountry = oDR["BILL_COUNTRY"].ToString();
                    oUI.BillPhone = oDR["BILL_PHONE"].ToString();*/
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();                
            }
            return oUI;
        }
       

        /// <summary>
        /// This is used to get whether the User Is based on Email or not
        /// </summary>
        /// <param name="UserEmail">string</param>
        /// <returns>integer</returns>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER ID BASED ON EMAIL ID  ***/
        /********************************************************************************/
        public int GetUser_ID(string UserEmail)
        {
           // string sSQL;
            int UserID = -1;
            DataSet dsUID = new DataSet();
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //sSQL = "SELECT USER_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND EMAILADDR='" + UserEmail + "'";
                // Old sSQL = "SELECT USER_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND LOGIN_NAME='" + UserEmail + "'";                
                // old sSQL = "SELECT USER_ID FROM TBWC_USER WHERE USER_EMAIL='" + UserEmail + "'";
                //oHelper.SQLString = sSQL;
                //dsUID = oHelper.GetDataSet("UID");
                dsUID = (DataSet)objHelperDB.GetGenericDataDB("",websiteid, UserEmail, "GET_COMPANY_BUYERS_DETAIL_EMAIL", HelperDB.ReturnType.RTDataSet);
                dsUID.Tables[0].TableName = "UID";
                if (dsUID != null)
                {
                    foreach (DataRow oDR in dsUID.Tables["UID"].Rows)
                    {
                        UserID = (int)oDR["USER_ID"];
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                UserID = -1;
            }
            return UserID;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER ID BASED ON CUSTOMERS LOGIN NAME  ***/
        /********************************************************************************/
        public int GetUserID(string LOGIN_NAME)
        {
           // string sSQL;
            int UserID = -1;
            DataSet dsUID = new DataSet();
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //sSQL = "SELECT USER_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND LOGIN_NAME='" + UserEmail + "'";
                // Old sSQL = "SELECT USER_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND EMAILADDR='" + UserEmail + "'";
                // old sSQL = "SELECT USER_ID FROM TBWC_USER WHERE USER_EMAIL='" + UserEmail + "'";
                //oHelper.SQLString = sSQL;
                //dsUID = oHelper.GetDataSet("UID");
                dsUID = (DataSet)objHelperDB.GetGenericDataDB("", websiteid, LOGIN_NAME, "GET_COMPANY_BUYERS_DETAIL_LOGIN", HelperDB.ReturnType.RTDataSet);
               // dsUID.Tables[0].TableName = "UID"; 
                if (dsUID != null)
                {
                    dsUID.Tables[0].TableName = "UID"; 

                    foreach (DataRow oDR in dsUID.Tables["UID"].Rows)
                    {
                        UserID = (int)oDR["USER_ID"];
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                UserID = -1;
            }
            return UserID;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER DETAILS  ***/
        /********************************************************************************/
        public DataSet  GetUserDateSet(int Cust_reg_id)
        {                     
            DataSet dsUID = new DataSet();
            try
            {
                dsUID = (DataSet)objHelperDB.GetGenericDataDB(Cust_reg_id.ToString() , "GET_COMPANY_BUYERS_DETAILS_CUSTOMER_REG", HelperDB.ReturnType.RTDataSet);
               
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                dsUID = null;
            }
            return dsUID;
        }
        /// <summary>
        /// This is used to get role of the User(Admin/Shopper)
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USERS ROLE FROM COMPANY BUYERS ***/
        /********************************************************************************/
        public string GetRole(int UserID)
        {
            string retVal = string.Empty;
            DataSet dsUID = new DataSet();
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //string sSQL = " SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS";
                // old string sSQL = " SELECT USER_ROLE FROM TBWC_USER";
                //sSQL = sSQL + " WHERE WEBSITE_ID = " + websiteid + " AND USER_ID =" + UserID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.GetValue("USER_ROLE");

                dsUID = (DataSet)objHelperDB.GetGenericDataDB("",websiteid, UserID.ToString(), "GET_COMPANY_BUYERS_DETAIL_USERID", HelperDB.ReturnType.RTDataSet);
                dsUID.Tables[0].TableName = "UID";
                if (dsUID != null)
                {
                    foreach (DataRow oDR in dsUID.Tables["UID"].Rows)
                    {
                        retVal = oDR["USER_ROLE"].ToString();
                    }
                }

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retVal = null;
            }
            return retVal;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE CUSTOMER TYPE ***/
        /********************************************************************************/
        public string GetCustomerType(int UserID)
        {
            //string retVal = string.Empty;
            //DataSet ds = new DataSet();
            //try
            //{
            //    ds = (DataSet)objHelperDB.GetGenericDataDB("", UserID.ToString(), "GET_CUSTOMER_TYPE", HelperDB.ReturnType.RTDataSet);
            //    if (ds != null)
            //    {
            //        foreach (DataRow oDR in ds.Tables[0].Rows)
            //        {
            //            retVal = oDR["CUSTOMER_TYPE"].ToString();
            //        }
            //    }

            //}
            //catch (Exception e)
            //{
            //    objErrorHandler.ErrorMsg = e;
            //    objErrorHandler.CreateLog();
            //    retVal = "";
            //}

            //modified by indu -25-Sep-2015 Reason:Customer type is Retailer

            return "Retailer";
        }
        /// <summary>
        /// This is used to get the User's Status
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>integer</returns>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER STATUS DETAILS***/
        /********************************************************************************/
        public int GetUserStatus(int UserID)
        {
            //string sSQL;
            int StatusValue = 0;
            DataSet dsUS = new DataSet();
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //sSQL = "SELECT USER_STATUS FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND USER_ID= '" + UserID + "'";
                // old sSQL = "SELECT USER_STATUS FROM TBWC_USER WHERE USER_ID= '" + UserID + "'";
                //oHelper.SQLString = sSQL;
                //dsUS = oHelper.GetDataSet("UserStatus");
                dsUS = (DataSet)objHelperDB.GetGenericDataDB("",websiteid, UserID.ToString(), "GET_COMPANY_BUYERS_DETAIL_USERID", HelperDB.ReturnType.RTDataSet);
                dsUS.Tables[0].TableName = "UserStatus";
                if (dsUS != null)
                {
                    foreach (DataRow oDR in dsUS.Tables["UserStatus"].Rows)
                    {
                        StatusValue = (int)oDR["USER_STATUS"];
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                StatusValue = -1;
            }
            return StatusValue;
        }
        /// <summary>
        /// This is used to get the User's Email Id
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>string</returns>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER EMAIL ADDRESS ***/
        /********************************************************************************/
        public string GetUserEmailAdd(int UserID)
        {
           // string sSQL;
            string sEmail = string.Empty;
            DataSet dsUser = new DataSet();
            try
            {
                string websiteid = ConfigurationManager.AppSettings["websiteid"].ToString();
                //sSQL = "SELECT EMAILADDR [USER_EMAIL] FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND USER_ID = " + UserID;
                //old sSQL = "SELECT USER_EMAIL FROM TBWC_USER WHERE USER_ID =" + UserID;
                //oHelper.SQLString = sSQL;
                //dsUser = oHelper.GetDataSet("UserName");
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("",websiteid, UserID.ToString(), "GET_COMPANY_BUYERS_DETAIL_USERID", HelperDB.ReturnType.RTDataSet);
                dsUser.Tables[0].TableName = "UserName";
                if (dsUser != null)
                {
                    foreach (DataRow oDR in dsUser.Tables["UserName"].Rows)
                    {
                        sEmail = oDR["USER_EMAIL"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                sEmail = "-1";
            }
            return sEmail;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USERS LOGIN NAME ***/
        /********************************************************************************/
        public string GetUserLoginName(int UserID)
        {
            //string sSQL;
            string LoginName = string.Empty;
            DataSet dsUser = new DataSet();
            try
            {
                string websiteid = ConfigurationManager.AppSettings["websiteid"].ToString();
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("", websiteid, UserID.ToString(), "GET_COMPANY_BUYERS_DETAIL_USERID", HelperDB.ReturnType.RTDataSet);
                dsUser.Tables[0].TableName = "UserName";
                if (dsUser != null)
                {
                    foreach (DataRow oDR in dsUser.Tables["UserName"].Rows)
                    {
                        LoginName = oDR["LOGIN_NAME"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                LoginName = "";
            }
            return LoginName;
        }
        /// <summary>
        /// This is used to get the full name of the User(First Name + Last Name)
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>string</returns>


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE FULL NAME OF THE USER ***/
        /********************************************************************************/
        public string GetUserFullName(int UserID)
        {
            //string sSQL;
            string sUsrFName = string.Empty;
            DataSet dsUser = new DataSet();
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //sSQL = "SELECT CONTACT AS USER_FULLNAME FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + "and USER_ID =" + UserID + "";
                //old sSQL = "SELECT USER_FNAME + ' ' + USER_LNAME AS USER_FULLNAME FROM TBWC_USER WHERE USER_ID =" + UserID;
                //oHelper.SQLString = sSQL;
                //sUsrFName = oHelper.GetValue("USER_FULLNAME");
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("",websiteid, UserID.ToString(), "GET_COMPANY_BUYERS_DETAIL_USERID", HelperDB.ReturnType.RTDataSet);
                dsUser.Tables[0].TableName = "UserName";
                if (dsUser != null)
                {
                    foreach (DataRow oDR in dsUser.Tables["UserName"].Rows)
                    {
                        sUsrFName = oDR["USER_FULLNAME"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                sUsrFName = "-1";
            }
            return sUsrFName;
        }
        /// <summary>
        /// This is used to check the Username is Already Exist in the Table.
        /// </summary>
        /// <param name="UserEmail">string</param>
        /// <returns>bool</returns>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK USERS E MAIL IS VALID OR NOT ***/
        /********************************************************************************/
        public bool CheckUserEmail(String UserEmail,string DealerRetailerType)
        {
           // string sSQL;
            DataSet dsUser = new DataSet();
            bool isAvaliable = false;
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //sSQL = "SELECT * FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND EMAILADDR = '" + UserEmail + "'";
                //old sSQL = "SELECT * FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND LOGIN_NAME = '" + UserEmail + "'";                
                // old sSQL = "SELECT * FROM TBWC_USER WHERE USER_EMAIL='" + UserEmail + "'";
                //oHelper.SQLString = sSQL;
                //dsUser = oHelper.GetDataSet();
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("", websiteid, UserEmail.ToString(), DealerRetailerType, "GET_COMPANY_BUYERS_DETAIL_EMAIL", HelperDB.ReturnType.RTDataSet);                
                if (dsUser == null)
                {
                    isAvaliable = false;
                }
                else
                {
                    isAvaliable = true;
                }


            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                isAvaliable = true;
            }
            return isAvaliable;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK USERS REGISTERED E-MAIL ID IS AVAILABLE OR NOT  ***/
        /********************************************************************************/
        public bool CheckUserRegisterEmail(String UserEmail, string DealerRetailerType)
        {
           // string sSQL;
            DataSet dsUser = new DataSet();
            bool isAvaliable = false;
            try
            {
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("", UserEmail.ToString(), DealerRetailerType, "GET_COMPANY_BUYERS_DETAIL_EMAIL_CHECK", HelperDB.ReturnType.RTDataSet);
                if (dsUser == null)
                {
                    isAvaliable = false;
                }
                else
                {
                    isAvaliable = true;
                }


            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                isAvaliable = true;
            }
            return isAvaliable;
        }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK USER ID IS AVAILABLE FOR USERS E-MAIL ID OR NOT  ***/
        /********************************************************************************/
        public bool CheckForgotUserEmail(String LoginName, String UserEmail)
        {
           // string sSQL;
            DataSet dsUser = new DataSet();
            bool isAvaliable = false;
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //sSQL = "SELECT * FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND EMAILADDR = '" + UserEmail + "' AND LOGIN_NAME='" + LoginName + "'";
                //oHelper.SQLString = sSQL;
                //dsUser = oHelper.GetDataSet();
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("", websiteid, UserEmail.ToString(), LoginName.ToString(), "GET_COMPANY_BUYERS_DETAIL_EMAIL_LOGIN", HelperDB.ReturnType.RTDataSet);                
                if (dsUser == null)
                {
                    isAvaliable = false;
                }
                else
                {
                    isAvaliable = true;
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            return isAvaliable;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK USER IS ALLOWED TO USE FORGET PASSOWRD OR NOT  ***/
        /********************************************************************************/
        public bool CheckValidUserForForgetPassword(String LoginName, String UserEmail, String TempPassword)
        {
           // string sSQL;
            DataSet dsUser = new DataSet();
            bool isAvaliable = false;
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //sSQL = "SELECT * FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND EMAILADDR = '" + UserEmail + "' AND ";
                //sSQL = sSQL + " LOGIN_NAME='" + LoginName + "' AND PASSWORD='" + TempPassword + "' AND USER_STATUS=6";
                //oHelper.SQLString = sSQL;
                //dsUser = oHelper.GetDataSet();
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("", websiteid, UserEmail.ToString(), LoginName.ToString(), TempPassword, "GET_COMPANY_BUYERS_CHECK_CALID_USER", HelperDB.ReturnType.RTDataSet);                
               
                if (dsUser == null)
                {
                    isAvaliable = false;
                }
                else
                {
                    isAvaliable = true;
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            return isAvaliable;
        }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK USER ID AVAILABLE FOR THE USER EMAIL OR NOT ***/
        /********************************************************************************/
        public bool CheckUserName(String UserEmail)
        {
            //string sSQL;
            DataSet dsUser = new DataSet();
            bool isAvaliable = false;
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //sSQL = "SELECT * FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND LOGIN_NAME = '" + UserEmail + "'";
                //old sSQL = "SELECT * FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " AND EMAILADDR = '" + UserEmail + "'";
                //old sSQL = "SELECT * FROM TBWC_USER WHERE USER_EMAIL='" + UserEmail + "'";
                //oHelper.SQLString = sSQL;
                //dsUser = oHelper.GetDataSet();
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("", websiteid, UserEmail.ToString(), "GET_COMPANY_BUYERS_DETAIL_LOGIN", HelperDB.ReturnType.RTDataSet);                
               
                if (dsUser == null)
                {
                    isAvaliable = false;
                }
                else
                {
                    isAvaliable = true;
                }


            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                isAvaliable = true;
            }
            return isAvaliable;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK MULTIPLE USER EMAIL ***/
        /********************************************************************************/
        public DataSet CheckMultipleUserMail(String UserEmail)
        {
            // string sSQL;
            DataSet dsUser = new DataSet();     
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();                
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("", websiteid, UserEmail.ToString(), "GET_COMPANY_BUYERS_DETAIL_LOGIN", HelperDB.ReturnType.RTDataSet);


            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
            return dsUser;
        }
      
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK MULTIPLE USER EMAIL ***/
        /********************************************************************************/
        public DataSet CheckMultipleUserMail(String UserEmail,String customertype)
        {
            //string sSQL;
            DataSet dsUser = new DataSet();
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("", websiteid, UserEmail.ToString(),customertype.ToString(), "GET_COMPANY_BUYERS_DETAIL_LOGIN_RETAILER", HelperDB.ReturnType.RTDataSet);


            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
            return dsUser;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK USER NAME USED FOR MULTIPLE LOGIN ***/
        /********************************************************************************/
        public DataSet CheckMultipleLoginName(String LoginName, string UserId)
        {
           // string sSQL;
            DataSet dsUser = new DataSet();
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("", websiteid, LoginName.ToString(),UserId.ToString(), "GET_COMPANY_BUYERS_DETAIL_LOGIN", HelperDB.ReturnType.RTDataSet);


            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
            return dsUser;
        }
        /// <summary>
        /// This is used to generate the Next User ID..
        /// </summary>        
        /// <returns>integer</returns>


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER MAXIMUM USER ID OR NEXT USER ID ***/
        /********************************************************************************/
        public int GetMaxUserId()
        {
           // string sSQL;
            int Userid = 0;
            DataSet odsUserid = new DataSet();
            try
            {
                //sSQL = " SELECT MAX(USER_ID) + 1 AS 'MAXID' FROM TBWC_USER";
                //oHelper.SQLString = sSQL;
                //odsUserid = oHelper.GetDataSet();
                odsUserid = (DataSet)objHelperDB.GetGenericDataDB("", "GET_USER_MAX_ID", HelperDB.ReturnType.RTDataSet);                
               
                if (odsUserid != null)
                {
                    foreach (DataRow rUserid in odsUserid.Tables[0].Rows)
                    {
                        Userid = objHelperService.CI(rUserid["MAXID"]);
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                Userid = -1;
            }
            return Userid;

        }
        /// <summary>
        /// This is used to insert the Company User in the Company Buyers Table
        /// </summary>
        /// <param name="UserID">int</param>
        /// <param name="CompanyID">int</param>
        /// <param name="CDUser">int</param>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO INSERT COMPANY DETAILS INTO COMPANY BUYERS  ***/
        /********************************************************************************/
        public int InsertCompanyUser(int UserID, string CompanyID, int CDUser)
        {
            try
            {
                //string sSQL = " INSERT INTO TBWC_COMPANY_BUYERS";
                //sSQL = sSQL + " (COMPANY_ID,USER_ID,";
                //sSQL = sSQL + " CREATED_USER,MODIFIED_USER)";
                //sSQL = sSQL + " VALUES('" + CompanyID + "'," + UserID + ",";
                //sSQL = sSQL + CDUser + "," + CDUser + ")";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string sSQL = "Exec STP_TBWC_POPSaveCOMPANY_BUYERS ";
                sSQL = sSQL + "'" + CompanyID + "'," + UserID + "," + CDUser + "," + CDUser ;

                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        /// <summary>
        /// This is used to get the User's Locked Status
        /// </summary>
        /// <param name="UserName">string</param>
        /// <returns>integer</returns>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER ID FROM COMPANY BUYERS ***/
        /********************************************************************************/
        public int GetUserWebSite_id(string User_id)
        {
            //string sSQL;
            int UserID = 0;
            DataSet dsUID = new DataSet();
            try
            {

                dsUID = (DataSet)objHelperDB.GetGenericDataDB(User_id, "GET_COMPANY_BUYERS_DETAILS", HelperDB.ReturnType.RTDataSet);
                dsUID.Tables[0].TableName = "UID";
                if (dsUID != null)
                {
                    foreach (DataRow oDR in dsUID.Tables["UID"].Rows)
                    {
                        UserID = (int)oDR["WEBSITE_ID"];
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                UserID = 0;
            }
            return UserID;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE OPTION VALUE AND WEB SITE NAME USING WEB SITE ID ***/
        /********************************************************************************/
        public string GetWebTitle(string website_id)
        {

            string WebTitle = string.Empty;
            DataSet dsUID = new DataSet();
            try
            {

                dsUID = (DataSet)objHelperDB.GetGenericDataDB(website_id, "GET_WEBSITE_TITLE", HelperDB.ReturnType.RTDataSet);
                if (dsUID != null && dsUID.Tables.Count > 0 && dsUID.Tables[0].Rows.Count > 0)
                {

                    WebTitle = dsUID.Tables[0].Rows[0]["OPTION_VALUE"].ToString();
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                WebTitle = "";
            }
            return WebTitle;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO LOCK USER ON INCORRECT LOGIN ***/
        /********************************************************************************/
        public int LockUser(string UserName)
        {
            try
            {
                //string sSQL = " UPDATE TBWC_COMPANY_BUYERS SET ";
                //sSQL = sSQL + " USER_STATUS = " + (int)UserStatus.LOCKED;
                //sSQL = sSQL + " WHERE LOGIN_NAME = '" + UserName + "'";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string sSQL = "Exec STP_TBWC_RENEW_COMPANY_BUYERS ";
                sSQL = sSQL + "'',0,0,'" + UserName + "','','UPDATE_USER_STATUS',0,''," + (int)UserStatus.LOCKED + ",0";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);  

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        /// <summary>
        /// This is used to get status of the User(Active/Inactive)
        /// </summary>
        /// <param name="UserName">string</param>
        /// <returns>bool</returns>
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK USER STATUS WEATHER THE USER IS ACTIVE OR NOT ***/
        /********************************************************************************/
        public bool IsUserActive(string Userid)
        {
            bool isActive = false;
            string user_status = string.Empty;
            try
            {
                //string sSQL = " SELECT USER_STATUS FROM TBWC_COMPANY_BUYERS ";
                //sSQL = sSQL + " WHERE LOGIN_NAME = '" + UserName + "'";
                //oHelper.SQLString = sSQL;
                //user_status = (string)objHelperDB.GetGenericDataDB(UserName, "GET_USER_STATUS", HelperDB.ReturnType.RTString);
                user_status = (string)objHelperDB.GetGenericDataDB(Userid, "GET_USER_STATUS", HelperDB.ReturnType.RTString);
                if (objHelperService.CI(user_status) == (int)UserStatus.ACTIVE || objHelperService.CI(user_status) == (int)UserStatus.MANUALVERIFY)
                {
                    isActive = true;
                }

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return isActive;

        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE COMPANY ID BASED ON USER ID ***/
        /********************************************************************************/
        public int GetCompanyID(int UserID)
        {
            int retval = -1;
            string tempstr = string.Empty;
            try
            {
                //string sSQL = string.Format(" SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID= {0}", UserID);
                //oHelper.SQLString = sSQL;
                //retval = oHelper.CI(oHelper.GetValue("COMPANY_ID"));
                tempstr = (string)objHelperDB.GetGenericDataDB(UserID.ToString(), "GET_COMPANY_ID", HelperDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    retval = objHelperService.CI(tempstr);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return retval;
            }
            return retval;
        }
        /// <summary>
        /// This is used to get status of the User(Online/Offline)
        /// </summary>
        /// <param name="flag">bool</param>
        /// <param name="UserID">int</param>
        /// <returns>integer</returns>


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE USER ONLINE STATUS  ***/
        /********************************************************************************/
        public int OnLineFlag(bool flag, int UserID)
        {
            int onValue = 0;
            try
            {
                if (flag)
                {
                    onValue = 1;
                }
                //string sSQL = " UPDATE TBWC_COMPANY_BUYERS ";
                //sSQL = sSQL + " SET USER_ONLINE=" + onValue;
                //sSQL = sSQL + " WHERE USER_ID = " + UserID;
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();

                string sSQL = "Exec STP_TBWC_RENEW_COMPANY_BUYERS ";
                sSQL = sSQL + "'',0," + UserID.ToString() + ",'','','UPDATE_USER_ONLINE',0,'',0," + onValue;
                return objHelperDB.ExecuteSQLQueryDB(sSQL);  
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }

        }
        /// <summary>
        ///  This is used to get the User's First Name
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <returns>string</returns>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USERS FIRST NAME ***/
        /********************************************************************************/
        public string UserFirstName(int OrderID)
        {
            string retval = string.Empty;
            try
            {
                //string sSQL = " SELECT WCU.USER_FNAME FROM TBWC_USER WCU,TBWC_ORDER WCO";
                //sSQL = sSQL + " WHERE WCU.USER_ID = WCO.USER_ID";
                //sSQL = sSQL + " AND WCO.ORDER_ID = " + OrderID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetValue("USER_FNAME");
                string tempstr = (string)objHelperDB.GetGenericDataDB(OrderID.ToString(), "GET_ORDER_USER_FNAME", HelperDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    retval = tempstr;
                return retval;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }

        }
        /// <summary>
        /// This is used to get the User's Payment Options
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>DataSet</returns>
        /// <example>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER PAYMENT OPTION DETAILS ***/
        /********************************************************************************/
        public DataSet GetUserPaymentOptions(int UserID)
        {
            try
            {
                //string sSQL = " SELECT  PO_PAYMENT_APPROVED,CC_PAYMENT_APPROVED,COD_PAYMENT_APPROVED FROM TBWC_USER";
                //sSQL = sSQL + " WHERE USER_ID =" + UserID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objHelperDB.GetGenericDataDB(UserID.ToString(), "GET_USER_PAYMENT_OPTION", HelperDB.ReturnType.RTDataSet);
                
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
            
        }
        /// <summary>
        /// This is used to get the Security Question
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>string</returns>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PASSWORD SECURITY QUESTIONS BASED ON USER ID ***/
        /********************************************************************************/
        public string GetSecurityQuestion(int UserID)
        {
            string retval = string.Empty;
            DataSet tempds = new DataSet();
            try
            {
                //string sSQL = " SELECT PWD_QUESTION1 FROM TBWC_USER";
                //sSQL = sSQL + " WHERE USER_ID =" + UserID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetValue("PWD_QUESTION1");                
                tempds = (DataSet)objHelperDB.GetGenericDataDB(UserID.ToString(), "GET_USER_DETAILS", HelperDB.ReturnType.RTDataSet);
                if (tempds != null && tempds.Tables.Count > 0 && tempds.Tables[0].Rows.Count > 0)
                {
                    retval = tempds.Tables[0].Rows[0]["PWD_QUESTION1"].ToString();  
                }

               
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
            finally
            {
                tempds = null;
            }
            return retval;
        }
        /// <summary>
        /// This is used to get the Security Answer
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>string</returns>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PASSWORD SECURITY ANSWER ***/
        /********************************************************************************/
        public string GetSecurityAnswer(int UserID)
        {
            string retval = string.Empty;
            DataSet tempds = new DataSet();
            try
            {
                //string sSQL = " SELECT PWD_ANSWER1 FROM TBWC_USER";
                //sSQL = sSQL + " WHERE USER_ID =" + UserID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetValue("PWD_ANSWER1");
                tempds = (DataSet)objHelperDB.GetGenericDataDB(UserID.ToString(), "GET_USER_DETAILS", HelperDB.ReturnType.RTDataSet);
                if (tempds != null && tempds.Tables.Count > 0 && tempds.Tables[0].Rows.Count > 0)
                {
                    retval = tempds.Tables[0].Rows[0]["PWD_ANSWER1"].ToString();
                }

               
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
            finally
            {
                tempds = null;
            }
            return retval;
        }
        /// <summary>
        /// This is used to get the User's Tax Exempt Value(T/F)
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>bool</returns>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE TAX EXEMPT DETAILS BASED ON USER ID ***/
        /********************************************************************************/
        public bool GetTaxExempt(int UserID,int orderid)
        {
           // string sSQL;
            string TaxExp;
            bool retVal = false;
            DataSet tempds = new DataSet();
            try
            {
                
                tempds = (DataSet)objHelperDB.GetGenericDataDB(UserID.ToString(), "GET_USER_DETAILS", HelperDB.ReturnType.RTDataSet);
                if (tempds != null && tempds.Tables.Count > 0 && tempds.Tables[0].Rows.Count > 0)
                {
                    TaxExp = tempds.Tables[0].Rows[0]["TAX_EXEMPT"].ToString();
                    if (TaxExp.ToUpper() == "TRUE")
                    {
                        retVal = true;
                    }
                }
               

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retVal = false;
            }
            finally
            {
                tempds = null;
            }
            return retVal;

        }
      
        /// <summary>
        /// This is used to get the User's Billing state Code
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>string</returns>


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE BILL STATE CODE DETAILS ***/
        /********************************************************************************/

        public string GetUserBillStateCode(int UserID)
        {
           // string sSQL;
            string retVal="-1";
            DataSet tempds = new DataSet();
            try
            {
                //sSQL = "SELECT BILL_STATE FROM TBWC_USER WHERE USER_ID = " + UserID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.GetValue("BILL_STATE");
                tempds = (DataSet)objHelperDB.GetGenericDataDB(UserID.ToString(), "GET_USER_INFO", HelperDB.ReturnType.RTDataSet);
                if (tempds != null && tempds.Tables.Count > 0 && tempds.Tables[0].Rows.Count > 0)
                {
                    retVal = tempds.Tables[0].Rows[0]["STATE"].ToString();
                }                
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retVal = "-1";
            }
            finally
            {
                tempds = null;
            }
            return retVal;

        }
        /// <summary>
        /// This is used to get the User's Billing Country Code
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>string</returns>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USERS BILL COUNTRY CODE ***/
        /********************************************************************************/
        public string GetUserBillCountryCode(int UserID)
        {
           // string sSQL;
            string retVal="";
            DataSet tempds = new DataSet();
            try
            {
                //sSQL = "SELECT BILL_COUNTRY FROM TBWC_USER WHERE USER_ID =" + UserID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.GetValue("BILL_COUNTRY");
                tempds = (DataSet)objHelperDB.GetGenericDataDB(UserID.ToString(), "GET_USER_INFO", HelperDB.ReturnType.RTDataSet);
                if (tempds != null && tempds.Tables.Count > 0 && tempds.Tables[0].Rows.Count > 0)
                {
                    retVal = tempds.Tables[0].Rows[0]["COUNTRY"].ToString();
                }
                
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retVal = "-1";
            }
            finally
            {
                tempds = null;
            }
            return retVal;

        }

        public string GetUserBillCountryCode_EC(int UserID)
        {
         // string sSQL;
            string retVal="";
            DataSet tempds = new DataSet();
            try
            {
                //sSQL = "SELECT BILL_COUNTRY FROM TBWC_USER WHERE USER_ID =" + UserID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.GetValue("BILL_COUNTRY");
                tempds = (DataSet)objHelperDB.GetGenericDataDB("","",UserID.ToString(), "GET_USER_INFO_EC", HelperDB.ReturnType.RTDataSet);
                if (tempds != null && tempds.Tables.Count > 0 && tempds.Tables[0].Rows.Count > 0)
                {
                    retVal = tempds.Tables[0].Rows[0]["COUNTRY"].ToString();
                }
                
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retVal = "-1";
            }
            finally
            {
                tempds = null;
            }
            return retVal;
}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USERS COUNTRY CODE ***/
        /********************************************************************************/
        public string GetUserCountryCode(string CountryName)
        {
            string sSQL;
            string retVal;
            try
            {
               // sSQL = "SELECT COUNTRY_CODE FROM TBWC_COUNTRY WHERE COUNTRY_NAME ='" + CountryName + "'";
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.GetValue("COUNTRY_CODE");
                retVal = (string)objHelperDB.GetGenericDataDB(CountryName, "GET_COUNTRY_CODE", HelperDB.ReturnType.RTString);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retVal = "-1";
            }
            return retVal;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE COMPANY NAME COMAPNY TABLE ***/
        /********************************************************************************/
        public string GetCompanyName(int UserID)
        {
            //string sSQL;
            string sCompname = string.Empty;
            DataSet dsUser = new DataSet();
            try
            {
                //sSQL = "SELECT DISTINCT COMPANY_NAME FROM TBWC_COMPANY WHERE COMPANY_ID=(SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID=" + UserID + ")";
                //oHelper.SQLString = sSQL;
                //dsUser = oHelper.GetDataSet("COMPANY_NAME");
                dsUser = (DataSet)objHelperDB.GetGenericDataDB(UserID.ToString(), "GET_COMPANY_DETAILS", HelperDB.ReturnType.RTDataSet);

                if (dsUser != null)
                {
                    dsUser.Tables[0].TableName = "COMPANY_NAME";
                    if (dsUser.Tables["COMPANY_NAME"].Rows.Count > 0)
                    {
                        sCompname = dsUser.Tables[0].Rows[0]["COMPANY_NAME"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                sCompname = "-1";
            }
            finally
            {
                dsUser = null;
            }
            return sCompname;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE BILL TO COMPANY NAME ***/
        /********************************************************************************/
        public string GetBillToCompanyName(int UserID)
        {
            // string sSQL;
            string sCompname = string.Empty;
            DataSet dsUser = new DataSet();
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

                //sSQL = "SELECT DISTINCT COMPANY_NAME FROM TBWC_COMPANY WHERE COMPANY_ID=(SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID=" + UserID + ")";
                //oHelper.SQLString = sSQL;
                //dsUser = oHelper.GetDataSet("COMPANY_NAME");
                dsUser = (DataSet)objHelperDB.GetGenericDataDB("", websiteid, UserID.ToString(), "GET_USER_BILL_INFO", HelperDB.ReturnType.RTDataSet);

                if (dsUser != null)
                {
                    dsUser.Tables[0].TableName = "COMPANY_NAME";
                    if (dsUser.Tables["COMPANY_NAME"].Rows.Count > 0)
                    {
                        if (dsUser.Tables[0].Rows[0]["Bill_Company"].ToString() == "")
                        {
                            sCompname = dsUser.Tables[0].Rows[0]["COMPANY_NAME"].ToString();
                        }
                        else
                        {
                            sCompname = dsUser.Tables[0].Rows[0]["Bill_Company"].ToString();
                        }


                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                sCompname = "-1";
            }
            finally
            {
                dsUser = null;
            }
            return sCompname;
        }
        #endregion
        public void SaveContact(string FULLNAME, string EMAIL, string CONTACTNUMBER, string COMMENTS)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                string sSQL = "Exec STP_TBWC_POPSaveContact ";
                sSQL = sSQL + "'" + FULLNAME + "','" + EMAIL + "','" + CONTACTNUMBER + "','" + COMMENTS + "'," + websiteid +"";
                objHelperDB.ExecuteSQLQueryDB(sSQL);

            }
            catch (Exception objException)
            {
                objErrorHandler.ErrorMsg = objException;
                objErrorHandler.CreateLog();

            }
            finally
            {
                //objConnection.CloseConnectionstate();
            }

        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK COMPANY BUYERS COUNT  ***/
        /********************************************************************************/
        public bool HasAdmin(int UserID)
        {
            bool retval = false;
            DataSet dsUser = new DataSet();
            try
            {
                //string sSQL = string.Format("SELECT COUNT(*) CNT FROM TBWC_COMPANY_BUYERS WHERE USER_ROLE = 1 AND COMPANY_ID = {0}", GetCompanyID(UserID));
                //oHelper.SQLString = sSQL;

                //if (System.Convert.ToInt32(oHelper.GetValue("CNT")) > 0)
                //    retval = true;
                dsUser = (DataSet)objHelperDB.GetGenericDataDB(UserID.ToString(), "GET_COMPANY_BUYERS_COUNT", HelperDB.ReturnType.RTDataSet);

                if (dsUser != null)
                {
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dsUser.Tables[0].Rows[0]["CNT"].ToString()) > 0)
                            retval = true;
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retval = false;
            }
            finally
            {
                dsUser = null;
            }
            return retval;
        }

        public string Get_ADMIN_APPROVED_UserEmils(string user_id)
        {
            DataSet oDs = new DataSet();
            string emails = string.Empty;

            string userid = user_id;//Session["USER_ID"].ToString();
            if (userid == string.Empty)
                userid = "0";
            //Commented by Indu
            //This Process will be applicat

            //try
            //{
            //    CompanyGroupDB objCompanyGroupDB = new CompanyGroupDB();

            //    oDs = (DataSet)objCompanyGroupDB.GetGenericDataDB(userid, "GET_COMPANY_USER_ADMIN_APPROVED_EMAILS", CompanyGroupDB.ReturnType.RTDataSet);
            //    if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
            //    {

            //        oDs.Tables[0].TableName = "Users";

            //        foreach (DataRow rItem in oDs.Tables["Users"].Rows)
            //        {
            //            if (rItem["EMAILADDR"].ToString() != "")
            //                emails = emails + rItem["EMAILADDR"].ToString() + ",";

            //        }
            //        if (emails != "")
            //            emails = emails.Substring(0, emails.Length - 1) + "";
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            return emails;
        }
    
    
    
    
  

   
    }
    /*********************************** J TECH CODE ***********************************/
}