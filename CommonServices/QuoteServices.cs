using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;

namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    /// <summary>
    /// This is used to Get and Return all the Quote related Methods and Functions
    /// </summary>
    /// <remarks>
    /// Used to get Quote Details like Data Retrieve Functions,Quote Retrieve Functions,Invoice Functions..
    /// </remarks>
    /// <example>
    /// Quote oQuote = new Quote();
    /// </example>
   
    public class QuoteServices
    {
        /*********************************** DECLARATION ***********************************/
        HelperDB ObjHelperDB = new HelperDB();
        HelperServices ObjHelperServices = new HelperServices();
        QuoteDB ObjQuoteDB = new QuoteDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
       
        #region "Structure for Quote"
        public struct QuoteInfo
        {
            /// <summary>
            /// QuoteID
            /// </summary>
            public int QuoteID;
            /// <summary>
            /// UserID
            /// </summary>
            public int UserID;
            /// <summary>
            /// QuoteFName
            /// </summary>
            public string QuoteFName;
            /// <summary>
            /// QuoteLName
            /// </summary>
            public string QuoteLName;
            /// <summary>
            /// QuoteAdd1
            /// </summary>
            public string QuoteAdd1;
            /// <summary>
            /// QuoteAdd2
            /// </summary>
            public string QuoteAdd2;
            /// <summary>
            /// QuoteAdd3
            /// </summary>
            public string QuoteAdd3;
            /// <summary>
            /// QuoteCity
            /// </summary>
            public string QuoteCity;
            /// <summary>
            /// QuoteState
            /// </summary>
            public string QuoteState;
            /// <summary>
            /// QuoteZip
            /// </summary>
            public string QuoteZip;
            /// <summary>
            /// QuoteCountry
            /// </summary>
            public string QuoteCountry;
            /// <summary>
            /// QuotePhone
            /// </summary>
            public string QuotePhone;
            /// <summary>
            /// QuoteEmail
            /// </summary>
            public string QuoteEmail;
            /// <summary>
            /// QuoteComments
            /// </summary>
            public string QuoteComments;
            /// <summary>
            /// QuoteStatus
            /// </summary>
            public int QuoteStatus;
            /// <summary>
            /// ProdTotalPrice
            /// </summary>
            public decimal ProdTotalPrice;
            /// <summary>
            /// TotalAmount
            /// </summary>
            public decimal TotalAmount;
            /// <summary>
            /// TaxAmount
            /// </summary>
            public decimal TaxAmount;
            /// <summary>
            /// Quote Created Date
            /// </summary>
            public DateTime CreatedDate;

           //public string ShipPrefix;
            //public string ShipFName;
            //public string ShipLName;
            //public string ShipMName;
            //public string ShipSuffix;
            //public string ShipAdd1;
            //public string ShipAdd2;
            //public string ShipAdd3;
            //public string ShipCity;
            //public string ShipState;
            //public string ShipZip;
            //public string ShipCountry;
            //public string ShipPhone;
            //public string ShipNotes;

            //public string BillFName;
            //public string BillLName;
            //public string BillMName;
            //public string BillAdd1;
            //public string BillAdd2;
            //public string BillAdd3;
            //public string BillCity;
            //public string BillState;
            //public string BillZip;
            //public string BillCountry;
            //public string BillPhone;

            //public decimal ShipCost;
            //public string ShipMethod;
            //public bool IsShipped;
            //public string TrackingNo;
            //public string EstDelivery;
            //public string ShipCompany;
            //public string ShipConf;
            //public bool isEmailSent;
            //public bool isInvoiceSent;
        }
        /// <summary>
        /// Create Structure for Quote Item Information
        /// </summary>
        /// <example>
        /// Quote.QuoteItemInfo oItemInFo = new  Quote.QuoteItemInfo();
        /// oItemInFo.QuoteID
        /// </example>
        public struct QuoteItemInfo
        {
            /// <summary>
            /// UserID
            /// </summary>
            public int UserID;
            /// <summary>
            /// QuoteID
            /// </summary>
            public int QuoteID;
            /// <summary>
            /// ProductID
            /// </summary>
            public int ProductID;
            /// <summary>
            /// Quantity
            /// </summary>
            public decimal Quantity;
            /// <summary>
            /// PriceApplied
            /// </summary>
            public decimal PriceApplied;
            /// <summary>
            /// PriceCalcMethod
            /// </summary>
            public string PriceCalcMethod;

        }
        /// <summary>
        /// Create Enum Values for Quote Status Details
        /// </summary>
        /// <example>
        /// /// Quote.QuoteStatus.OPEN;
        /// </example>
        public enum QuoteStatus
        {
            /// <summary>
            /// Open Quote Status
            /// </summary>
            OPEN = 1,
            /// <summary>
            /// REQUESTQUOTE Quote Status
            /// </summary>
            REQUESTQUOTE = 2,
            /// <summary>
            /// RESPONSEQUOTE Quote Status
            /// </summary>
            RESPONSEQUOTE = 3,
            /// <summary>
            /// CLOSED Quote Status
            /// </summary>
            CLOSED = 4,
            /// <summary>
            /// CANCELED Quote Status
            /// </summary>
            CANCELED = 5,
            /// <summary>
            /// QUOTEPLACED Status
            /// </summary>
            QUOTEUPDATEFLAG =7
            
        }
        public enum QuoteUpdateFlag
        {
            /// <summary>
            ///Used to Set the flag when Quote is Updated
            /// </summary>
            QUOTEUPDATE =1
        }
        #endregion
        /*********************************** DECLARATION ***********************************/

        #region "Quote Retrieve Functions"
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to create the New Quote in the Quote Table
        /// </summary>
        /// <param name="qInfo">QuoteInfo</param>
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
        ///     Quote oQuote = new Quote();
        ///     Quote.QuoteInfo oInfo = new  Quote.QuoteInfo();
        ///     int quote;
        ///     ...
        ///     quote = oQuote.CreateQuote(qInfo);
        /// }
        /// </code>
        /// </example>
        /// /*********************************** OLD CODE ***********************************/
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO ADD NEW QUOTE DETAILS INTO QUOTE HISTORY  ***/
        /********************************************************************************/
        public int CreateQuote(QuoteInfo qInfo)
        {
            try
            {

                string sSQL;
                //sSQL = "INSERT INTO TBWC_QUOTE(USER_ID,QUOTE_FNAME,QUOTE_LNAME,QUOTE_ADDRESS1,QUOTE_ADDRESS2,QUOTE_ADDRESS3,";
                //sSQL = sSQL + "QUOTE_CITY,QUOTE_STATE,QUOTE_ZIP,QUOTE_COUNTRY,QUOTE_PHONE,QUOTE_COMMENTS,QUOTE_STATUS,QUOTE_EMAIL,PRODUCT_TOTAL_PRICE,TOTAL_AMOUNT,";
                //sSQL = sSQL + "CREATED_DATE,CREATED_USER,MODIFIED_DATE,MODIFIED_USER) ";
                //sSQL = sSQL + "VALUES(" + qInfo.UserID + ",'" + qInfo.QuoteFName + "','" + qInfo.QuoteLName + "','" + qInfo.QuoteAdd1 + "','" + qInfo.QuoteAdd2 + "','" + qInfo.QuoteAdd3 + "','";
                //sSQL = sSQL + qInfo.QuoteCity + "','" + qInfo.QuoteState + "','" + qInfo.QuoteZip + "','" + qInfo.QuoteCountry + "','" + qInfo.QuotePhone + "','" + qInfo.QuoteComments + "'," + qInfo.QuoteStatus + ",'" + qInfo.QuoteEmail + "'," + qInfo.ProdTotalPrice + "," + qInfo.TotalAmount + ",";
                //sSQL = sSQL + "{fn now()}" + "," + qInfo.UserID + ",{fn now()}," + qInfo.UserID + ")";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                sSQL = "Exec STP_TBWC_POP_QUOTE ";
                sSQL = sSQL + "" + qInfo.UserID + ",'" + qInfo.QuoteFName + "','" + qInfo.QuoteLName + "','" + qInfo.QuoteAdd1 + "','" + qInfo.QuoteAdd2 + "','" + qInfo.QuoteAdd3 + "','";
                sSQL = sSQL + qInfo.QuoteCity + "','" + qInfo.QuoteState + "','" + qInfo.QuoteZip + "','" + qInfo.QuoteCountry + "','" + qInfo.QuotePhone + "','" + qInfo.QuoteComments + "'," + qInfo.QuoteStatus + ",'" + qInfo.QuoteEmail + "'," + qInfo.ProdTotalPrice + "," + qInfo.TotalAmount + ",'";
                sSQL = sSQL + DateTime.Now  + "'," + qInfo.UserID + ",'" +  DateTime.Now+ "'," + qInfo.UserID ;
                return ObjHelperDB.ExecuteSQLQueryDB(sSQL);   

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
        /// This is used to create the New Quote in the QuoteHistory
        /// </summary>
        /// <param name="qInfo">QuoteInfo</param>
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
        ///     Quote oQuote = new Quote();
        ///     Quote.QuoteInfo oInfo = new  Quote.QuoteInfo ();
        ///     int quote;
        ///     ...
        ///     quote = oQuote.CreateQuoteHistory(qInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO ADD QUOTE DETAILS INTO QUOTE HISTORY ITEMS  ***/
        /********************************************************************************/
        public int CreateQuoteHistory(QuoteInfo qInfo)
        {
            try
            {
                string sSQL;
                //sSQL = "INSERT INTO TBWC_QUOTE_HISTORY(QUOTE_ID,USER_ID,QUOTE_HISTORY_FNAME,QUOTE_HISTORY_LNAME,QUOTE_HISTORY_ADDRESS1,QUOTE_HISTORY_ADDRESS2,QUOTE_HISTORY_ADDRESS3,";
                //sSQL = sSQL + "QUOTE_HISTORY_CITY,QUOTE_HISTORY_STATE,QUOTE_HISTORY_ZIP,QUOTE_HISTORY_COUNTRY,QUOTE_HISTORY_PHONE,QUOTE_HISTORY_EMAIL,QUOTE_HISTORY_COMMENTS,QUOTE_HISTORY_STATUS,PRODUCT_TOTAL_PRICE,TOTAL_AMOUNT,";
                //sSQL = sSQL + "CREATED_DATE,CREATED_USER,MODIFIED_DATE,MODIFIED_USER) ";
                //sSQL = sSQL + "VALUES(" + qInfo.QuoteID + "," + qInfo.UserID + ",'" + qInfo.QuoteFName + "','" + qInfo.QuoteLName + "','" + qInfo.QuoteAdd1 + "','" + qInfo.QuoteAdd2 + "','" + qInfo.QuoteAdd3 + "','";
                //sSQL = sSQL + qInfo.QuoteCity + "','" + qInfo.QuoteState + "','" + qInfo.QuoteZip + "','" + qInfo.QuoteCountry + "','" + qInfo.QuotePhone + "','" + qInfo.QuoteEmail + "','" + qInfo.QuoteComments + "'," + qInfo.QuoteStatus + "," + qInfo.ProdTotalPrice + "," + qInfo.TotalAmount + ",";
                //sSQL = sSQL + "{fn now()}," + qInfo.UserID + ",{fn now()}," + qInfo.UserID + ")";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                sSQL = "Exec STP_TBWC_POP_QUOTE_HISTORY ";
                sSQL = sSQL + "" + qInfo.QuoteID + "," + qInfo.UserID + ",'" + qInfo.QuoteFName + "','" + qInfo.QuoteLName + "','" + qInfo.QuoteAdd1 + "','" + qInfo.QuoteAdd2 + "','" + qInfo.QuoteAdd3 + "','";
                sSQL = sSQL + qInfo.QuoteCity + "','" + qInfo.QuoteState + "','" + qInfo.QuoteZip + "','" + qInfo.QuoteCountry + "','" + qInfo.QuotePhone + "','" + qInfo.QuoteEmail + "','" + qInfo.QuoteComments + "'," + qInfo.QuoteStatus + "," + qInfo.ProdTotalPrice + "," + qInfo.TotalAmount + ",'";
                sSQL = sSQL + DateTime.Now + "'," + qInfo.UserID + ",'" + DateTime.Now + "'," + qInfo.UserID ;
                return ObjHelperDB.ExecuteSQLQueryDB(sSQL);   

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
        /// This is used to create the New Quote in the QuoteHistoryItems
        /// </summary>
        /// <param name="qInfo">QuoteItemInfo</param>
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
        ///     Quote oQuote = new Quote();
        ///     Quote.QuoteInfo oInfo = new  Quote.QuoteInfo ();
        ///     int quote;
        ///     ...
        ///     quote = oQuote.CreateQuoteHistoryItems(qInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO ADD QUOTE DETAILS INTO QUOTE HISTORY ITEMS  ***/
        /********************************************************************************/
        public int CreateQuoteHistoryItems(QuoteItemInfo qInfo)
        {
            try
            {
                string sSQL;
                //sSQL = "INSERT INTO TBWC_QUOTE_HISTORY_ITEMS(QUOTE_ID,PRODUCT_ID,QTY,PRICE_APPLIED,MODIFIED_DATE,MODIFIED_USER)";
                //sSQL = sSQL + "VALUES(" + qInfo.QuoteID + "," + qInfo.ProductID + ", " + qInfo.Quantity + "," + qInfo.PriceApplied + "";
                //sSQL = sSQL + ",{fn now()}," + qInfo.UserID + ")";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                sSQL = "Exec STP_TBWC_POP_QUOTE_HISTORY_ITEMS ";                
                sSQL = sSQL + "" + qInfo.QuoteID + "," + qInfo.ProductID + ", " + qInfo.Quantity + "," + qInfo.PriceApplied + "";
                sSQL = sSQL + ",'" + DateTime.Now +"'," + qInfo.UserID ;
                return ObjHelperDB.ExecuteSQLQueryDB(sSQL);   
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
        /// This is used to Insert the New Quote in the Quote Table
        /// </summary>
        /// <param name="qInfo">QuoteInfo</param>
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
        ///     Quote oQuote = new Quote();
        ///     Quote.QuoteInfo oInfo = new  Quote.QuoteInfo ();
        ///     int initOrder;
        ///     ...
        ///     initOrder = oQuote.InitilizeQuote(qInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO ADD NEW QUOTE INTO QUOTE TABLE  ***/
        /********************************************************************************/
        public int InitilizeQuote(QuoteInfo qInfo)
        {
            try
            {
                //Order status 0 is Created.
                //string sSQL = " INSERT INTO TBWC_QUOTE(USER_ID,QUOTE_STATUS,CREATED_DATE,CREATED_USER,MODIFIED_DATE,MODIFIED_USER)";
                //sSQL = sSQL + " VALUES( " + qInfo.UserID + ",1,{fn now()}," + qInfo.UserID + ",{fn now()}," + qInfo.UserID + " )";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string sSQL = "Exec STP_TBWC_POP_QUOTE_Initilize ";
                sSQL = sSQL + "" + qInfo.UserID + ",1," + qInfo.UserID + "," + qInfo.UserID ;
                return ObjHelperDB.ExecuteSQLQueryDB(sSQL);   


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
        /// This is used to get details about the Quote based on Quote ID.
        /// </summary>
        /// <param name="QuoteID">int</param>
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
        ///protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     Quote.QuoteInfo oInfo = new Quote.QuoteInfo();
        ///     ...
        ///     oInfo = oQuote.GetQuote(QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE QUOTE DETAILS ***/
        /********************************************************************************/
        public QuoteInfo GetQuote(int QuoteID)
        {
            {
                //try
                //{
                string sSQL;
                DataSet dsOD = new DataSet();
                QuoteInfo rInfo = new QuoteInfo();
                try
                {

                    //sSQL = "SELECT * FROM TBWC_QUOTE WHERE QUOTE_ID = " + QuoteID;

                    //oHelper.SQLString = sSQL;
                    //dsOD = oHelper.GetDataSet("Quote");
                    dsOD = (DataSet)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString(), "GET_QUOTE_DETAILS", QuoteDB.ReturnType.RTDataSet);
                    if (dsOD != null)
                    {
                        dsOD.Tables[0].TableName = "Quote";
                        foreach (DataRow drOD in dsOD.Tables["Quote"].Rows)
                        {
                            rInfo.QuoteID = ObjHelperServices.CI(drOD["QUOTE_ID"]);
                            rInfo.QuoteFName = ObjHelperServices.CS(drOD["QUOTE_FNAME"]);
                            rInfo.QuoteLName = ObjHelperServices.CS(drOD["QUOTE_LNAME"]);
                            rInfo.QuoteAdd1 = ObjHelperServices.CS(drOD["QUOTE_ADDRESS1"]);
                            rInfo.QuoteAdd2 = ObjHelperServices.CS(drOD["QUOTE_ADDRESS2"]);
                            rInfo.QuoteAdd3 = ObjHelperServices.CS(drOD["QUOTE_ADDRESS3"]);
                            rInfo.QuoteCity = ObjHelperServices.CS(drOD["QUOTE_CITY"]);
                            rInfo.QuoteState = ObjHelperServices.CS(drOD["QUOTE_STATE"]);
                            rInfo.QuoteZip = ObjHelperServices.CS(drOD["QUOTE_ZIP"]);
                            rInfo.QuoteCountry = ObjHelperServices.CS(drOD["QUOTE_COUNTRY"]);
                            rInfo.QuoteEmail = ObjHelperServices.CS(drOD["QUOTE_EMAIL"]);
                            rInfo.QuotePhone = ObjHelperServices.CS(drOD["QUOTE_PHONE"]);
                            rInfo.QuoteComments = ObjHelperServices.CS(drOD["QUOTE_COMMENTS"]);
                            rInfo.UserID = ObjHelperServices.CI(drOD["USER_ID"].ToString());
                            rInfo.QuoteStatus = ObjHelperServices.CI(drOD["QUOTE_STATUS"]);
                            rInfo.ProdTotalPrice = ObjHelperServices.CDEC(drOD["PRODUCT_TOTAL_PRICE"]);
                            rInfo.TotalAmount = ObjHelperServices.CDEC(drOD["TOTAL_AMOUNT"]);
                            //rInfo.CreatedDate = oHelper.CS(drOD["CREATED_DATE"]);
                        }
                    }
                }
                catch (Exception e)
                {
                    objErrorHandler.ErrorMsg = e;
                    objErrorHandler.CreateLog();
                    //rInfo = - 1;
                }
                return rInfo;
            }
        }

        /*********************************** OLD CODE ***********************************/

        /// <summary>
        /// This is used to get Quote list Based on UserID and sStatus
        /// </summary>
        /// <param name="UserID">integer</param>
        /// <param name="sStatus">integer</param>
        /// <returns>DataTable</returns>
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
        ///     Quote oQuote = new Quote();
        ///     Quote.QuoteInfo oInfo = new  Quote.QuoteInfo ();
        ///     int UserID; 
        ///     string sStatus;
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oQuote.GetQuoteList(UserID,sStatus);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE QUOTE LIST DETAILS ***/
        /********************************************************************************/
        public DataTable GetQuoteList(int UserID, int sStatus)
        {
            string sStatusType = "";
            try
            {
                if (sStatus == 3)
                {
                    
                    sStatusType = "3,7";
                    //string sSQL = " SELECT QUOTE_ID,TOTAL_AMOUNT,";
                    //sSQL = sSQL + " MODIFIED_DATE AS QUOTEDATE";
                    //sSQL = sSQL + " FROM TBWC_QUOTE";
                    //sSQL = sSQL + " WHERE USER_ID=" + UserID;
                    //sSQL = sSQL + " AND QUOTE_STATUS IN(" + sStatusType + ") ORDER BY  MODIFIED_DATE DESC ";
                    //oHelper.SQLString = sSQL;
                }
                else if (sStatus == 1)
                {
                    sStatusType = "1";
                    //string sSQL = " SELECT QUOTE_ID,TOTAL_AMOUNT,";
                    //sSQL = sSQL + " MODIFIED_DATE AS QUOTEDATE";
                    //sSQL = sSQL + " FROM TBWC_QUOTE";
                    //sSQL = sSQL + " WHERE USER_ID=" + UserID;
                    //sSQL = sSQL + " AND QUOTE_STATUS IN(" + sStatusType + ") ORDER BY  MODIFIED_DATE DESC ";
                    //oHelper.SQLString = sSQL;
                }
                else if (sStatus == 2)
                {
                    sStatusType = "2";
                    //string sSQL = " SELECT QUOTE_ID,TOTAL_AMOUNT,";
                    //sSQL = sSQL + " MODIFIED_DATE AS QUOTEDATE";
                    //sSQL = sSQL + " FROM TBWC_QUOTE";
                    //sSQL = sSQL + " WHERE USER_ID=" + UserID;
                    //sSQL = sSQL + " AND QUOTE_STATUS IN(" + sStatusType + ") ORDER BY  MODIFIED_DATE DESC ";
                    //oHelper.SQLString = sSQL;
                }


                //return oHelper.GetDataTable();
                return (DataTable)ObjQuoteDB.GetGenericDataDB("", UserID.ToString(), sStatusType, "GET_QUOTE_LIST", QuoteDB.ReturnType.RTTable);      
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Quote DataSet.
        /// </summary>
        /// <param name="QuoteID">int</param>
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
        ///protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     Quote.QuoteInfo oInfo = new Quote.QuoteInfo();
        ///     ...
        ///     oInfo = oQuote.GetQuoteDetails(QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE QUOTE DETAILS  ***/
        /********************************************************************************/
        public DataSet GetQuoteDetails(int QuoteID)
        {
            DataSet objds = new DataSet();
            try
            {
                string sSQL;
                //New DB changes
                //sSQL = "SELECT (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = P.PRODUCT_ID) AS CATALOG_ITEM_NO,";
                //sSQL = sSQL + "QI.QTY,QI.PRICE_APPLIED FROM TB_PRODUCT P,";
                //sSQL = sSQL + "TBWC_QUOTE_ITEMS QI WHERE P.PRODUCT_ID=QI.PRODUCT_ID AND QI.QUOTE_ID=" + QuoteID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet("QuoteDetails");
                objds = (DataSet)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString(), "GET_QUOTE_DETAILS_VALUES", QuoteDB.ReturnType.RTDataSet);
                if (objds != null)
                    objds.Tables[0].TableName = "QuoteDetails";
                else
                    objds = null;



            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
            return objds;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to Update all the Quote Details
        /// </summary>
        /// <param name="qInfo">QuoteInfo</param>
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
        ///     Quote oQuote = new Quote();
        ///     Quote.QuoteInfo oInfo = new  Quote.QuoteInfo ();
        ///     int UpdateQuoteStatus;
        ///     ...
        ///     UpdateQuoteStatus = oOrder.UpdateQuote(qInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE QUOTE DETAILS  ***/
        /********************************************************************************/
        public int UpdateQuote(QuoteInfo qInfo)
        {
            //if (oInfo.OrderStatus == 1)
            //{
            //try
            //{
            string sSQL;
            //sSQL = "UPDATE TBWC_QUOTE SET QUOTE_FNAME='" + qInfo.QuoteFName + "',QUOTE_LNAME='" + qInfo.QuoteLName + "',";
            //sSQL = sSQL + "QUOTE_ADDRESS1='" + qInfo.QuoteAdd1 + "',QUOTE_ADDRESS2='" + qInfo.QuoteAdd2 + "',QUOTE_ADDRESS3='" + qInfo.QuoteAdd3 + "',";
            //sSQL = sSQL + "QUOTE_CITY='" + qInfo.QuoteCity + "',QUOTE_STATE='" + qInfo.QuoteState + "',QUOTE_ZIP='" + qInfo.QuoteZip + "',";
            //sSQL = sSQL + "QUOTE_COUNTRY='" + qInfo.QuoteCountry + "',QUOTE_PHONE='" + qInfo.QuotePhone + "',QUOTE_EMAIL='" + qInfo.QuoteEmail + "',";
            //sSQL = sSQL + "QUOTE_COMMENTS='" + qInfo.QuoteComments + "',USER_ID=" + qInfo.UserID + ",QUOTE_STATUS=" + qInfo.QuoteStatus + ",";
            //sSQL = sSQL + "PRODUCT_TOTAL_PRICE=" + qInfo.ProdTotalPrice + ",TOTAL_AMOUNT=" + qInfo.TotalAmount + ",";
            //sSQL = sSQL + "MODIFIED_USER=" + qInfo.UserID + " WHERE QUOTE_ID=" + qInfo.QuoteID;


           // oHelper.SQLString = sSQL;
           // return oHelper.ExecuteSQLQuery();

            try
            {
                sSQL = "Exec STP_TBWC_RENEW_QUOTE ";
                sSQL = sSQL +  "'" + qInfo.QuoteFName + "','" + qInfo.QuoteLName + "',";
                sSQL = sSQL + "'" + qInfo.QuoteAdd1 + "','" + qInfo.QuoteAdd2 + "','" + qInfo.QuoteAdd3 + "',";
                sSQL = sSQL + "'" + qInfo.QuoteCity + "','" + qInfo.QuoteState + "','" + qInfo.QuoteZip + "',";
                sSQL = sSQL + "'" + qInfo.QuoteCountry + "','" + qInfo.QuotePhone + "','" + qInfo.QuoteEmail + "',";
                sSQL = sSQL + "'" + qInfo.QuoteComments + "'," + qInfo.UserID + "," + qInfo.QuoteStatus + ",";
                sSQL = sSQL + "" + qInfo.ProdTotalPrice + "," + qInfo.TotalAmount + ",";
                sSQL = sSQL + "" + qInfo.UserID + ","+ qInfo.QuoteID;
                return ObjHelperDB.ExecuteSQLQueryDB(sSQL); 
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
            //}
            //catch (Exception e)
            //{
            //    oErrHand.ErrorMsg = e;
            //    //return null;
            //}
            //}
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to set the Price of the Quote
        /// </summary>
        /// <param name="qInfo">QuoteInfo</param>
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
        ///     Quote oQuote = new Quote();
        ///     Quote.QuoteInfo oInfo = new  Quote.QuoteInfo ();
        ///     int UpdateQPr;
        ///     ...
        ///     UpdateQPr = oQuote.UpdateQuotePrice(qInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE PRICE FOR QUOTE  ***/
        /********************************************************************************/
        public int UpdateQuotePrice(QuoteInfo qInfo)
        {
            int retVal;
            try
            {
                //string sSQL = " UPDATE TBWC_QUOTE SET PRODUCT_TOTAL_PRICE = " + qInfo.ProdTotalPrice + ",";
                //sSQL = sSQL + " TOTAL_AMOUNT = " + qInfo.TotalAmount;
                //sSQL = sSQL + " WHERE QUOTE_ID =" + qInfo.QuoteID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.ExecuteSQLQuery();
                string sSQL ="exec STP_TBWC_RENEW_QUOTE_UPDATE_PRICE ";
                sSQL =  sSQL+ "" + qInfo.ProdTotalPrice + ",";
                sSQL = sSQL + "" + qInfo.TotalAmount;
                sSQL = sSQL + "," + qInfo.QuoteID;
                return ObjHelperDB.ExecuteSQLQueryDB(sSQL);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retVal = -1;
            }

            return retVal;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to Set the Price of Removed Items
        /// </summary>
        /// <param name="RemovedItemsPrice">decimal</param>
        /// <param name="QuoteID">int</param>
        /// <param name="TaxAmount">decimal</param>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     int retVal;
        ///     decimal Tax;
        ///     decimal RemovedItemsPrice;
        ///     ...
        ///     retVal = oQuote.UpdateRemovedItemsPrice(RemovedItemsPrice, QuoteID, Tax);
        /// }
        /// </code>
        /// </example>
        /// 
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE PRODUCT TOTAL PRICE,TAX AMOUNT,TOTAL AMOUNT OF REMOVED ITEMS  ***/
        /********************************************************************************/
        public int UpdateRemovedItemsPrice(decimal RemovedItemsPrice, int QuoteID, decimal TaxAmount)
        {
            int retVal;
            try
            {
                decimal TotalAmount = RemovedItemsPrice ;
                //string sSQL = "UPDATE TBWC_QUOTE SET PRODUCT_TOTAL_PRICE = PRODUCT_TOTAL_PRICE - " + RemovedItemsPrice + ",TAX_AMOUNT =TAX_AMOUNT - " + TaxAmount + ",TOTAL_AMOUNT = TOTAL_AMOUNT - " + TotalAmount + " WHERE QUOTE_ID = " + QuoteID;

                //oHelper.SQLString = sSQL;
                //retVal = oHelper.ExecuteSQLQuery();
                string sSQL = "Exec STP_TBWC_RENEW_QUOTE_Removed_Price ";
                sSQL =sSQL  + "" +RemovedItemsPrice + "," + TaxAmount + "," + TotalAmount + "," + QuoteID;
                return ObjHelperDB.ExecuteSQLQueryDB(sSQL);


            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retVal = -1;
            }

            return retVal;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to Set the Quote Status
        /// </summary>
        /// <param name="QuoteID">int</param>
        /// <param name="QuoteStatus">int</param>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     int QteStatusID;
        ///     int UpdateQteStatus;
        ///     ...
        ///     UpdateQteStatus = oQuote.UpdateQuoteStatus(QuoteID, QuoteStatus)
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE THE QUOTE STATUS  ***/
        /********************************************************************************/
        public int UpdateQuoteStatus(int QuoteID, int QuoteStatus)
        {
            int retVal = 0;
            try
            {
                //string sSQL = "UPDATE TBWC_QUOTE SET QUOTE_STATUS = " + QuoteStatus + " WHERE QUOTE_ID = " + QuoteID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.ExecuteSQLQuery();
                string sSQL = " Exec STP_TBWC_RENEW_QUOTE_STATUS " + QuoteID + "," + QuoteStatus;
                retVal= ObjHelperDB.ExecuteSQLQueryDB(sSQL); 
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return retVal;
            }
            return retVal;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Quote ID based on User ID
        /// </summary>
        /// <param name="UserID">int</param>
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
        ///    Quote oQuote = new Quote();
        ///     int UserID;
        ///     int QteID;
        ///     ...
        ///     QteID = oQuote.GetQuoteID(UserID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE QUOTE ID FROM QOUTE TABLE ***/
        /********************************************************************************/
        public int GetQuoteID(int UserID)
        {
            try
            {
                string QuoteID;
                //string sSQL = "SELECT QUOTE_ID FROM TBWC_QUOTE WHERE USER_ID = " + UserID;
                //oHelper.SQLString = sSQL;
                //QuoteID = oHelper.GetValue("QUOTE_ID");
                QuoteID = (string)ObjQuoteDB.GetGenericDataDB(UserID.ToString(), "GET_QUOTE_ID", QuoteDB.ReturnType.RTString);
                if (QuoteID == "" & QuoteID==null)
                {
                    return 0;
                }
                else
                {
                    return ObjHelperServices.CI(QuoteID);
                }
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
        /// This is used to get the Quote ID based on User ID and Quote Status
        /// </summary>
        /// <param name="UserID">int</param>
        /// <param name="QuoteStatus">int</param>
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
        ///    Quote oQuote = new Quote();
        ///     int UserID;
        ///     int QuoteStatus;
        ///     int QteID;
        ///     ...
        ///     QteID = oQuote.GetQuoteID(UserID,QuoteStatus);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE QUOTE ID AND QUOTE ID STATUS DETAILS ***/
        /********************************************************************************/
        public int GetQuoteID(int UserID, int QuoteStatus)
        {
            try
            {
                string QuoteID;
                //string sSQL = "SELECT QUOTE_ID FROM TBWC_QUOTE WHERE USER_ID = " + UserID + " AND QUOTE_STATUS = " + QuoteStatus;
                //oHelper.SQLString = sSQL;
                //QuoteID = oHelper.GetValue("QUOTE_ID");
                QuoteID = (string)ObjQuoteDB.GetGenericDataDB("", UserID.ToString(), QuoteStatus.ToString(), "GET_QUOTE_ID_STATUS", QuoteDB.ReturnType.RTString);
                if (QuoteID == "" & QuoteID == null)
                {
                    return 0;
                }
                else
                {
                    return ObjHelperServices.CI(QuoteID);
                }
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
        /// This is used to get the Quote Date based on Quote ID
        /// </summary>
        /// <param name="QuoteID">int</param>     
        /// <returns>DateTime</returns>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;          
        ///     DataSet DateDS = new DataSet();
        ///     ...
        ///     DateDS = oQuote.GetQuoteDate(QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE QUOTE DATE ***/
        /********************************************************************************/
        public DateTime GetQuoteDate(int QuoteID)
        {
            DataSet ObjDs = new DataSet();
            try
            {
                DateTime QuoteDate;
                //string sSQL = "SELECT MODIFIED_DATE FROM TBWC_QUOTE WHERE QUOTE_ID = " + QuoteID;
                //oHelper.SQLString = sSQL;
                //QuoteDate =Convert.ToDateTime(oHelper.GetValue("MODIFIED_DATE"));
                //if (QuoteDate.ToString() == "")
                //{
                //    return Convert.ToDateTime(null);
                //}
                //else
                //{
                //    return QuoteDate;
                //}
                ObjDs = (DataSet)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString() , "GET_QUOTE_MODIFIED_DATE", QuoteDB.ReturnType.RTDataSet);
                if (ObjDs != null && ObjDs.Tables.Count > 0 && ObjDs.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToDateTime(ObjDs.Tables[0].Rows[0]["MODIFIED_DATE"]) ;
                }
                else
                {
                    return Convert.ToDateTime(null);
                }

                
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog(); 
                return Convert.ToDateTime(null);
            }

        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Price Value for the Quote
        /// </summary>
        /// <param name="QuoteID">int</param>
        /// <returns>DataSet</returns>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oQuote.GetOrderPriceValues(QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRICE VALUES FOR QUOTES ***/
        /********************************************************************************/
        public DataSet GetQuotePriceValues(int QuoteID)
        {
            try
            {
                //string sSQL = " SELECT PRODUCT_TOTAL_PRICE,TOTAL_AMOUNT FROM TBWC_QUOTE WHERE QUOTE_ID =" + QuoteID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString(), "GET_QUOTE_PRICE_VALUE", QuoteDB.ReturnType.RTDataSet);
                
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog(); 
                return null;
            }
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get Quote History Based on UserID and sStatus
        /// </summary>
        /// <param name="UserID">int</param> 
        /// <returns>DataTable</returns>
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
        ///     Quote oQuote = new Quote();
        ///     Quote.QuoteInfo oInfo = new  Quote.QuoteInfo();
        ///     int UserID; 
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oQuote.GetQuotesHistory(UserID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE QUOTE HISTORY ***/
        /********************************************************************************/
        public DataTable GetQuotesHistory(int UserID)
        {
            try
            {
                // string sStatusType = "";
                // sStatusType = "1,2,3,4,5";
               // string sSQL = " SELECT QUOTE_ID,";
                //sSQL = sSQL + " CREATED_DATE AS QUOTEDATE,QUOTE_HISTORY_STATUS,";
                //sSQL = sSQL + " TOTAL_AMOUNT FROM TBWC_QUOTE_HISTORY";
                //sSQL = sSQL + " WHERE USER_ID=" + UserID;
                //sSQL = sSQL + " ORDER BY QUOTEDATE DESC ";

                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataTable();
                return (DataTable)ObjQuoteDB.GetGenericDataDB(UserID.ToString(), "GET_QUOTE_HISTORY", QuoteDB.ReturnType.RTTable);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog(); 
                return null;
            }
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to Get the Quote Status
        /// </summary>
        /// <param name="QuoteID">int</param>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     string UpdateQteStatus;
        ///     ...
        ///     UpdateQteStatus; = oQuote.GetQuoteStatus(QuoteID)
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE QUOTE STATUS BASED ON QUOTE ID ***/
        /********************************************************************************/
        public string GetQuoteStatus(int QuoteID)
        {
            string sSQL;
            int StatusValue = 0;
            string sQuoteStatus = "";
            DataSet dsOS = new DataSet();

            try
            {
                //sSQL = "SELECT QUOTE_STATUS FROM TBWC_QUOTE WHERE QUOTE_ID=" + QuoteID;
                //oHelper.SQLString = sSQL;
                //dsOS = oHelper.GetDataSet("QuoteStatus");
                dsOS = (DataSet)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString(), "GET_QUOTE_STATUS", QuoteDB.ReturnType.RTDataSet);
                if (dsOS != null)
                {
                    dsOS.Tables[0].TableName = "QuoteStatus";  
                    foreach (DataRow oDR in dsOS.Tables["QuoteStatus"].Rows)
                    {
                        StatusValue = (int)oDR["QUOTE_STATUS"];
                    }
                }
                if (StatusValue > 0)
                {
                    sQuoteStatus = Enum.GetName(typeof(QuoteStatus), StatusValue);
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog(); 
                return null;
            }
            return sQuoteStatus;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to Get the Quote History Status
        /// </summary>
        /// <param name="QuoteID">int</param>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     string UpdateQteStatus;
        ///     ...
        ///     UpdateQteStatus; = oQuote.GetQuoteHistoryStatus(QuoteID)
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE DETAILS FOR QUOTE HISTORY STATUS ***/
        /********************************************************************************/
        public string GetQuoteHistoryStatus(int QuoteID)
        {
            string sSQL;
            int StatusValue = 0;
            string sQuoteStatus = "";
            DataSet dsOS = new DataSet();

            try
            {
                //sSQL = "SELECT QUOTE_HISTORY_STATUS FROM TBWC_QUOTE_HISTORY WHERE QUOTE_ID=" + QuoteID;
                //oHelper.SQLString = sSQL;
                //dsOS = oHelper.GetDataSet("QuoteStatus");
                dsOS = (DataSet)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString(), "GET_QUOTE_HISTORY_STATUS", QuoteDB.ReturnType.RTDataSet);
                if (dsOS != null)
                {
                    dsOS.Tables[0].TableName = "QuoteStatus";
                    foreach (DataRow oDR in dsOS.Tables["QuoteStatus"].Rows)
                    {
                        StatusValue = (int)oDR["QUOTE_HISTORY_STATUS"];
                    }
                }
                if (StatusValue > 0)
                {
                    sQuoteStatus = Enum.GetName(typeof(QuoteStatus), StatusValue);
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog(); 
                return null;
            }
            return sQuoteStatus;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Current Product Total Cost
        /// </summary>
        /// <param name="QuoteID">int</param>
        /// <returns>decimal</returns>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     decimal retVal;
        ///     ...
        ///     retVal = oQuote.GetCurrentProductTotalCost(QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE TOTAL COST FOR THE CURRENT PRODUCT ***/
        /********************************************************************************/
        public decimal GetCurrentProductTotalCost(int QuoteID)
        {
            decimal retVal = 0;
            DataSet dsOS =new DataSet ();
            try
            {
                //string sSQL = "SELECT TOTAL_AMOUNT FROM TBWC_QUOTE WHERE QUOTE_ID =" + QuoteID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.CDEC(oHelper.GetValue("TOTAL_AMOUNT"));
                //if (retVal == -1)
                ///{
                 ///   retVal = 0;
                //}

                dsOS = (DataSet)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString(), "GET_QUOTE_STATUS", QuoteDB.ReturnType.RTDataSet);
                if (dsOS != null && dsOS.Tables.Count > 0 && dsOS.Tables[0].Rows.Count>0   )
                {
                    retVal = ObjHelperServices.CDEC(dsOS.Tables[0].Rows[0]["TOTAL_AMOUNT"]);
                }
                if (retVal <= -1)
                {
                   retVal = 0;
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog(); 
                retVal = -1;
            }
            return retVal;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to Cancel the Quote
        /// </summary>
        /// <param name="QuoteID">int</param>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     int CancelQte;
        ///     ...
        ///     CancelQte = oQuote.CancelQuote(QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE CANCEL QUOTE STATUS  ***/
        /********************************************************************************/
        public int CancelQuote(int QuoteID)
        {
            try
            {
                //string sSQL = " UPDATE TBWC_QUOTE";
                //sSQL = sSQL + " SET QUOTE_STATUS=" + (int)QuoteStatus.CANCELED;
                //sSQL = sSQL + " WHERE QUOTE_ID=" + QuoteID;
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string sSQL = "exec STP_TBWC_RENEW_QUOTE_CALNCE ";
                sSQL = sSQL = QuoteID +"," +(int)QuoteStatus.CANCELED;
                return ObjHelperDB.ExecuteSQLQueryDB(sSQL);
                    
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
        /// 
        /// </summary>
        /// <param name="QuoteID">integer</param>
        /// <param name="UserID">integer</param>
        /// <param name="QuoteStatus">integer</param>
        /// <returns>integer</returns>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     int SelectQuoteID;
        ///     ...
        ///     SelectQuoteID = oQuote.SelectQuoteID(QuoteID,UserID,QteStatus);
        /// }
        /// </code>
        /// </example>
        //
        //public int SelectQuoteID(int QuoteID, int UserID, int QuoteStatus)
        //{
        //    string tempstr = "";
        //    try
        //    {
        //        int QID = 0;
        //        string sSQL = "SELECT QUOTE_ID FROM TBWC_QUOTE WHERE";
        //        sSQL = sSQL + " USER_ID=" + UserID + " AND QUOTE_STATUS =" + QuoteStatus;
        //        sSQL = sSQL + " AND QUOTE_STATUS<>" + QuoteID;
        //        oHelper.SQLString = sSQL;
        //        QID = oHelper.CI(oHelper.GetValue("QUOTE_ID"));
        //        tempstr=(string) ObjQuoteDB.GetGenericDataDB("",UserID.ToString(),QuoteStatus.ToString(),    
        //        if (QID == 0)
        //        {
        //            return 0;
        //        }
        //        else
        //        {
        //            return oHelper.CI(QID);
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        oErrHand.ErrorMsg = e;
        //        return -1;
        //    }
        //}
        #endregion
        /*********************************** OLD CODE ***********************************/

        #region "Quote Item Functions.."
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to insert the Items into the Quote Item Table
        /// </summary>
        /// <param name="oItem">QuoteItemInfo</param>
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
        ///     Quote oQuote = new Quote();
        ///     Quote.QuoteItemInfo oItem;
        ///     int addItem;
        ///     ...
        ///     addItem = oQuote.AddQuoteItem(oItem);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO ADD NEW ITEMS INTO QUOTE ITEM   ***/
        /********************************************************************************/
        public int AddQuoteItem(QuoteItemInfo oItem)
        {
            try
            {
                string sSQL;

                //sSQL = "INSERT INTO TBWC_QUOTE_ITEMS(QUOTE_ID,PRODUCT_ID,QTY,PRICE_APPLIED,CREATED_USER,MODIFIED_USER) ";
                //sSQL = sSQL + "VALUES(" + oItem.QuoteID + "," + oItem.ProductID + "," + oItem.Quantity + "," + oItem.PriceApplied + ",";//'" + oItem.PriceCalcMethod + "',";
                //sSQL = sSQL + oItem.UserID + "," + oItem.UserID + ")";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                sSQL = "Exec STP_TBWC_POP_QUOTE_ITEM ";
                sSQL = sSQL + "" + oItem.QuoteID + "," + oItem.ProductID + "," + oItem.Quantity + "," + oItem.PriceApplied + ",";//'" + oItem.PriceCalcMethod + "',";
                sSQL = sSQL + oItem.UserID + "," + oItem.UserID ;
                return ObjHelperDB.ExecuteSQLQueryDB(sSQL); 

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
        /// This is used to get the Quote Items using QuoteID
        /// </summary>
        /// <param name="QuoteID">int</param>
        /// <returns>DataSet</returns>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oQuote.GetQuoteItems(QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE QUOTE ITEM DETAILS ***/
        /********************************************************************************/
        public DataSet GetQuoteItems(int QuoteID)
        {
            try
            {
                //string sSQL = " SELECT (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = TP.PRODUCT_ID) AS CATALOG_ITEM_NO,TQI.QUOTE_ID,TQI.PRODUCT_ID,TQI.QTY,TQI.PRICE_APPLIED,";
                //sSQL = sSQL + " TI.QTY_AVAIL ,TI.MIN_ORD_QTY,";
                //sSQL = sSQL + " PRODUCT_STATUS";
                //sSQL = sSQL + " FROM TBWC_QUOTE_ITEMS TQI,TBWC_INVENTORY TI,TB_PRODUCT TP";
                //sSQL = sSQL + " WHERE TQI.QUOTE_ID =" + QuoteID;
                //sSQL = sSQL + " AND TQI.PRODUCT_ID = TI.PRODUCT_ID AND TQI.PRODUCT_ID = TP.PRODUCT_ID";

                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString(), "GET_QUOTE_ITEMS", QuoteDB.ReturnType.RTDataSet);  

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog(); 
                return null;
            }
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to count the Number of Items in Quote
        /// </summary>
        /// <param name="QuoteID">int</param>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     int retVal;
        ///     ...
        ///     retVal = oQuote.GetQuoteItemCount(QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE COUNT FOR QUOTE ITEMS ***/
        /********************************************************************************/
        public int GetQuoteItemCount(int QuoteID)
        {
            DataSet objds = new DataSet();
            
            try
            {
               // string sSQL = "SELECT COUNT(PRODUCT_ID)AS ITEMCOUNT FROM TBWC_QUOTE_ITEMS WHERE QUOTE_ID = " + QuoteID;
                //oHelper.SQLString = sSQL;
                //return oHelper.CI(oHelper.GetValue("ITEMCOUNT"));
                objds = (DataSet)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString() , "GET_QUOTE_ITEMS_COUNT", QuoteDB.ReturnType.RTDataSet);  
                if (objds!=null && objds.Tables.Count>0 & objds.Tables[0].Rows.Count>0)
                {
                    return ObjHelperServices.CI(objds.Tables[0].Rows[0]["ITEMCOUNT"]); 
                }
                else
                {
                    return 0;
                }
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
        /// This is used to get the Products Total Cost
        /// </summary>
        /// <param name="QuoteID">int</param>
        /// <returns>decimal</returns>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     decimal retVal;
        ///     ...
        ///     retVal = oQuote.GetProductTotalCost(QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PEODUCT TOTAL COST DETAILS ***/
        /********************************************************************************/
        public decimal GetProductTotalCost(int QuoteID)
        {
            decimal retVal = 0;
            DataSet objds = new DataSet(); 
            try
            {
                //string sSQL = "SELECT SUM(PRICE_APPLIED)AS TOTALAMOUNT FROM TBWC_QUOTE_ITEMS WHERE QUOTE_ID =" + QuoteID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.CDEC(oHelper.GetValue("TOTALAMOUNT"));
                //if (retVal < 0) retVal = 0.00M;
                objds = (DataSet)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString(), "GET_QUOTE_PRODUCT_TOTAL_COST", QuoteDB.ReturnType.RTDataSet);
                if (objds != null && objds.Tables.Count > 0 & objds.Tables[0].Rows.Count > 0)
                {
                    retVal= ObjHelperServices.CDEC(objds.Tables[0].Rows[0]["TOTALAMOUNT"]);
                }
                else
                {
                    retVal= 0;
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog(); 
                retVal = -1;
            }
            return retVal;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Quote Items Quantity
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="QuoteID">int</param>
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
        ///     Quote oQuote = new Quote();
        ///     int ProductID;
        ///     int QuoteID;
        ///     int retVal;
        ///     ...
        ///     retVal = oQuote.GetQuoteItemQty(ProductID, QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE QUOTE ITEMS QUANTITY BASED ON PRODUCT ID AND QUOTE ID ***/
        /********************************************************************************/
        public int GetQuoteItemQty(int ProductID, int QuoteID)
        {
            int retVal = 0;
            DataSet objds = new DataSet(); 
            try
            {
                //string sSQL = "SELECT QTY FROM TBWC_QUOTE_ITEMS WHERE PRODUCT_ID = " + ProductID + " AND QUOTE_ID =" + QuoteID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.CI(oHelper.GetValue("QTY"));
                objds = (DataSet)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString(), "GET_QUOTE_ITEM_QTY", QuoteDB.ReturnType.RTDataSet);
                if (objds != null && objds.Tables.Count > 0 & objds.Tables[0].Rows.Count > 0)
                {
                    retVal = ObjHelperServices.CI(objds.Tables[0].Rows[0]["QTY"]);
                }               
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog(); 
                retVal = -1;
            }
            return retVal;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to Update the Quote Items 
        /// </summary>
        /// <param name="OrItemInfo">QuoteItemInfo</param>
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
        ///     Quote oQuote = new Quote();
        ///     Quote.QuoteItemInfo oQteItemInfo;
        ///     int UpdateQteItem;
        ///     ...
        ///     UpdateQteItem = oQuote.UpdateQuoteItem(OQItemInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE QUOTE ITEM DATEILS  ***/
        /********************************************************************************/
        public int UpdateQuoteItem(QuoteItemInfo OQItemInfo)
        {
            try
            {

                //string sSQL = "UPDATE TBWC_QUOTE_ITEMS SET QTY=" + OQItemInfo.Quantity + ",PRICE_APPLIED=" + OQItemInfo.PriceApplied;
                //sSQL = sSQL + " WHERE PRODUCT_ID=" + OQItemInfo.ProductID + "AND QUOTE_ID =" + OQItemInfo.QuoteID;
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string sSQL= "Exec STP_TBWC_RENEW_QUOTE_ITEM " ;
                sSQL = sSQL + OQItemInfo.QuoteID + ","+ OQItemInfo.ProductID +"," + OQItemInfo.Quantity + "," + OQItemInfo.PriceApplied;
                return ObjHelperDB.ExecuteSQLQueryDB(sSQL); 
                
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
        /// This is used to remove the Items from the Quote Item Table 
        /// </summary>
        /// <param name="ProductID">string</param>
        /// <param name="QuoteID">int</param>
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
        ///     Quote oQuote = new Quote();
        ///     string ProductID;
        ///     int QuoteID;
        ///     int remItem;
        ///     ...
        ///     remItem = oQuote.RemoveQuoteItem(ProductID, QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO REMOVE QUOTE ITEM FROM QUOTE TABLE  ***/
        /********************************************************************************/
        public int RemoveQuoteItem(string ProductID, int QuoteID)
        {
            try
            {
                string sSQL = "";
                //if (ProductID == "AllProd")
                //{
                //    // sSQL = "DELETE FROM TBWC_QUOTE_ITEMS WHERE QUOTE_ID=" + QuoteID;
                //}
                //else
                //{
                //    sSQL = "DELETE FROM TBWC_QUOTE_ITEMS WHERE PRODUCT_ID IN(" + ProductID + ") AND QUOTE_ID=" + QuoteID;
                //}
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();

                sSQL = "exec STP_TBWC_CANCEL_QUOTE_ITEM ";
                sSQL = sSQL + QuoteID + ",'" + ProductID + "'";
                return ObjHelperDB.ExecuteSQLQueryDB(sSQL); 
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog(); 
                return -1;
            }
        }
        #endregion

        #region "Quote Invoice functions.."
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Quote Total Cost
        /// </summary>
        /// <param name="QuoteID">int</param>
        /// <returns>decimal</returns>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     decimal TotCost;
        ///     ...
        ///     TotCost = oQuote.GetQuoteTotalCost(QuoteID);
        /// }
        /// </code>
        /// </example>
        
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE TOTAL COST FROM QUOTE TABLE ***/
        /********************************************************************************/
        public decimal GetQuoteTotalCost(int QuoteID)
        {
            decimal TotCst = 0.00M;
            DataSet objds = new DataSet(); 
            try
            {
                //string sSQL = "SELECT ISNULL(TOTAL_AMOUNT,0.00)AS TOTAL_AMOUNT  FROM TBWC_QUOTE WHERE QUOTE_ID =" + QuoteID;
                //oHelper.SQLString = sSQL;
                //TotCst = oHelper.CDEC(oHelper.GetValue("TOTAL_AMOUNT"));
                objds = (DataSet)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString(), "GET_QUOTE_TOTAL_COST", QuoteDB.ReturnType.RTDataSet);
                if (objds != null && objds.Tables.Count > 0 & objds.Tables[0].Rows.Count > 0)
                {
                    TotCst = ObjHelperServices.CDEC(objds.Tables[0].Rows[0]["TOTAL_AMOUNT"]);
                }  
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return 0;
            }
            return TotCst;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Quote Version Number
        /// </summary>
        /// <param name="QuoteID">integer</param>
        /// <returns>integer</returns>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     int QteVersion;
        ///     ...
        ///     QteVersion = oQuote.GetQuoteVersion(QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE QUOTE VERSION NUMBER ***/
        /********************************************************************************/
        public int GetQuoteVersion(int QuoteID)
        {
            int VersionNo = 0;
            try
            {
                string sSQL;
                //sSQL = "SELECT VERSION FROM TBWC_QUOTE_HISTORY WHERE QUOTE_ID=" + QuoteID;
                //oHelper.SQLString = sSQL;
                //VersionNo = oHelper.ExecuteSQLQuery();
                string tempstr  = (string)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString(), "GET_QUOTE_VERSION", QuoteDB.ReturnType.RTString );

                if (tempstr != null && tempstr!="")
                {
                    VersionNo = Convert.ToInt32(tempstr);   
                } 

                // VersionNo = VersionNo + 1;
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                VersionNo = -1;
            }
            return VersionNo;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Quote's maximum  version number based on Quote ID
        /// </summary>
        /// <param name="QuoteID">integer</param>
        /// <returns>integer</returns>
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
        ///     Quote oQuote = new Quote();
        ///     int QuoteID;
        ///     int QteVersion;
        ///     ...
        ///     QteVersion = oQuote.GetMaxVersion(QuoteID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE MAXIMUM VERSION NUMBER OF QUOTE ***/
        /********************************************************************************/
        public int GetMaxVersion(int QuoteID)
        {

            int VerMax = 0;
            try
            {
                string sSQL;
                //sSQL = "SELECT MAX(VERSION) FROM TBWC_QUOTE_HISTORY WHERE QUOTE_ID=" + QuoteID + "";
                //oHelper.SQLString = sSQL;
                //VerMax = oHelper.ExecuteSQLQuery();
                string tempstr = (string)ObjQuoteDB.GetGenericDataDB(QuoteID.ToString(), "GET_QUOTE_MAX_VERSION", QuoteDB.ReturnType.RTString);

                if (tempstr != null && tempstr != "")
                {
                    VerMax = Convert.ToInt32(tempstr);
                } 
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                VerMax = -1;
            }
            return VerMax;
        }
        #endregion

    }
    /*********************************** J TECH CODE ***********************************/
}



