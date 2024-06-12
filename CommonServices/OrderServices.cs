using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Web.Services;
using System.Web;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Collections.Generic;
using System.Web.Security;


namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    /// <summary>
    /// This is used to Get and Return all the Order related Methods and Functions
    /// </summary>
    /// <remarks>
    /// Used to get Order Details like Data Retrieve Functions,Shipping Functions,Inventory Data Retrieve Funtions,Invoice Functions..
    /// </remarks>
    /// <example>
    /// Order oOrder = new Order();
    /// </example>

    public class OrderServices
    {
        /*********************************** DECLARATION ***********************************/      
        OrderDB objOrderDB = new OrderDB();
        HelperDB objHelperDB = new HelperDB();
        HelperServices objHelperService = new HelperServices();
        //CountryServices objCountryServices = new CountryServices();

        ErrorHandler objErrorHandler = new ErrorHandler();
        UserServices objUserServices = new UserServices();
        public string FIXED_TAX = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX"].ToString();
        public string FIXED_TAX_PERCENTAGE = System.Configuration.ConfigurationManager.AppSettings["FIXED_TAX_PERCENTAGE"].ToString();
        //ConnectionDB objConnectionDB = new ConnectionDB();
        /*********************************** DECLARATION ***********************************/      
        /// <summary>
        /// Create Structure for Order Related Information(includes shipping,Billing)
        /// </summary>
        /// <example>
        /// Order.OrderInfo oOI = new Order.OrderInfo();
        /// oOI.ShipLName
        /// </example>
        public struct OrderInfo
        {
            /// <summary>
            /// Order ID
            /// </summary>
            public int OrderID;
            /// <summary>
            /// Users's ID
            /// </summary>
            public int UserID;
            /// <summary>
            /// Shipping Prefix
            /// </summary>
            public string ShipPrefix;
            /// <summary>
            /// Shipping First Name
            /// </summary>
            public string ShipFName;
            /// <summary>
            /// Shipping Last Name
            /// </summary>
            public string ShipLName;
            /// <summary>
            /// Shipping Middle Name
            /// </summary>
            public string ShipMName;
            /// <summary>
            /// Shipping Suffix
            /// </summary>
            public string ShipSuffix;
            /// <summary>
            /// Shipping Address 1
            /// </summary>
            public string ShipAdd1;
            /// <summary>
            /// Shipping Address 2
            /// </summary>
            public string ShipAdd2;
            /// <summary>
            /// Shipping Address 3
            /// </summary>
            public string ShipAdd3;
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
            /// Shipping Phone Number
            /// </summary>
            public string ShipPhone;
            /// <summary>
            /// Shipping Notes
            /// </summary>
            public string ShipNotes;
            /// <summary>
            /// Billing First Name
            /// </summary>
            public string BillFName;
            /// <summary>
            /// Billing Last Name
            /// </summary>
            public string BillLName;
            /// <summary>
            /// Billing Middle Name
            /// </summary>
            public string BillMName;
            /// <summary>
            /// Billing Address 1
            /// </summary>
            public string BillAdd1;
            /// <summary>
            /// Billing Address 2
            /// </summary>
            public string BillAdd2;
            /// <summary>
            /// Billing Address 3
            /// </summary>
            public string BillAdd3;
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
            /// Billing Phone Number
            /// </summary>
            public string BillPhone;


            public string BillcompanyName;

            /// <summary>
            /// Order Status
            /// </summary>
            public int OrderStatus;
            /// <summary>
            /// Product Total Price
            /// </summary>
            public decimal ProdTotalPrice;
            /// <summary>
            /// Order's Tax Amount
            /// </summary>
            public decimal TaxAmount;
            /// <summary>
            /// Order's Shipping Cost
            /// </summary>
            public decimal ShipCost;
            /// <summary>
            /// Order's Total Amount
            /// </summary>
            public decimal TotalAmount;
            /// <summary>
            /// Order's Shipping Method
            /// </summary>
            public string ShipMethod;
            /// <summary>
            /// Order's Shipping Status
            /// </summary>
            public bool IsShipped;
            /// <summary>
            /// Order's Tracking Number
            /// </summary>
            public string TrackingNo;
            /// <summary>
            /// Order's Est Delivery
            /// </summary>
            public string EstDelivery;
            /// <summary>
            /// Order's Shipping Company
            /// </summary>
            public string ShipCompany;
            /// <summary>
            /// Order's Shipping Confirmation
            /// </summary>
            public string ShipConf;
            /// <summary>
            /// Email Sent Status
            /// </summary>
            public bool isEmailSent;
            /// <summary>
            /// Invoice Sent Status
            /// </summary>
            public bool isInvoiceSent;
            /// <summary>
            /// Client IP Address
            /// </summary>
            public string ClientIPAddress;
            /// <summary>
            /// Invoice No
            /// </summary>
            public string InvoiceNo;
            /// <summary>
            /// Drop Ship
            /// </summary>
            public int DropShip;
            /// <summary>
            /// Created Date
            /// </summary>
            public DateTime CreatedDate;
            /// <summary>
            /// Modified Date
            /// </summary>
            public DateTime ModifiedDate;
            /// <summary>
            /// Modified User
            /// </summary>
            public int ModifiedUser;
            /// <summary>
            /// Created User
            /// </summary>
            public int CreatedUser;
            /// <summary>
            /// SHIP COMPANY NAME
            /// </summary>
            public string ShipCompName;
            /// <summary>
            /// DELIVERY INSTRUCTIONS
            /// </summary>
            public string DeliveryInstr;
            /// <summary>
            /// INVOICE DATE
            /// </summary>
            public DateTime InvoiceDate;

            public string Receiver_Contact;

            public string ShipTrackUrl;
            public int Websiteid;
            public string Bill_Name;
            public string SHIP_CODE;
            public string SHIP_NUMBER;
        }
        /// <summary>
        /// Create Structure for Order Item Information
        /// </summary>
        /// <example>
        /// Order.OrderItemInfo oItemInFo = new Order.OrderItemInfo();
        /// oItemInFo.OrderID
        /// </example>
        public struct OrderItemInfo
        {
            /// <summary>
            /// User's ID
            /// </summary>
            public int UserID;
            /// <summary>
            /// Order ID
            /// </summary>
            public int OrderID;
            /// <summary>
            /// Product ID
            /// </summary>
            public int ProductID;
            /// <summary>
            /// Product's Quantity
            /// </summary>
            public decimal Quantity;
            /// <summary>
            /// Price Applied for each Product
            /// </summary>
            public decimal PriceApplied;
            /// <summary>
            /// Price Calculation Method(Percentage or Amount)
            /// </summary>
            public string PriceCalcMethod;
            /// <summary>
            /// Price Calculation Method(Percentage or Amount)
            /// </summary>
            public decimal PriceIncApplied;

            /// <summary>
            /// Product's Tax_Amount
            /// </summary>
            public decimal Tax_Amount;
            /// <summary>
            /// Product's Shipping cost
            /// </summary>
            public decimal Ship_Cost;
            /// <summary>
            /// Product's Order_item id
            /// </summary>
            public double ORDER_ITEM_ID;

            public string PRD_SESSION_ID;

           

        }
        public struct OrderTemplateInfo
        {
            public int UserID;
     
            public int ProductID;
            
            public decimal Quantity;
        }

        public struct Order_Calrification_ItemInfo
        {
            /// <summary>
            /// User's ID
            /// </summary>
            public int UserID;
            /// <summary>
            /// Order ID
            /// </summary>
            public int OrderID;
            /// <summary>
            /// Product desc
            /// </summary>
            public string ProductDesc;
            /// <summary>
            /// Product's Quantity
            /// </summary>
            public decimal Quantity;

            public double Clarification_ID;

            public string Clarification_Type;
            public string PROD_SESSION_ID;
        }

        /// <summary>
        /// Create Enum Values for Order Status Details
        /// </summary>
        /// <example>
        /// Order.OrderStatus.OPEN;
        /// </example>
        public enum OrderStatus
        {
            /// <summary>
            /// Open Order Status
            /// </summary>
            OPEN = 1,
            /// <summary>
            /// Paymeny Order Status
            /// </summary>
            PAYMENT = 2,
            /// <summary>
            /// Transit Order Status
            /// </summary>
            SHIPPED = 3,
            /// <summary>
            /// Completed Order Status
            /// </summary>
            COMPLETED = 4,
            /// <summary>
            /// Cancelled Order Status
            /// </summary>
            CANCELED = 5,
            /// <summary>
            /// Order Placed Order Status
            /// </summary>
            ORDERPLACED = 6,
            /// <summary>
            /// Manual Process Order Status(Manual Verify Users)
            /// </summary>
            MANUALPROCESS = 7,
            /// <summary>
            /// Place the Quote  
            /// </summary>
            QUOTEPLACED = 8,
            /// <summary>
            /// For CAU ORDERS - Order Completed Status
            /// </summary>
            CAU_READYTOVERIFY = 9,
            /// <summary>
            /// For CAU ORDERS - Order Manual check status
            /// </summary>
            CAU_MANUALPROCESS = 10,
            /// <summary>
            /// For CAU PENDING - Order status
            /// </summary>
            CAU_PENDING = 11,
            /// <summary>
            /// For Deleted- Order status
            /// </summary>
            DELETED = 12,

            Order_Received=13,
           Processing_Order=14,
           Payment_Required=15  ,
           Payment_Successful=16,
           Intl_Waiting_Verification=17,
           Proforma_Payment_Required=18,
           Proforma_Payment_Success=19,
           Intl_Order_Processing=20,
           Proforma_Payment_Received=21 ,
           Online_Payment = 22,
           Order_Ready_For_Pick_Up = 34
        }
        /// <summary>
        /// Create Structure for Inventory Products
        /// </summary>
        /// <example>
        /// Order.Inventory Inv = new Order.Inventory();
        /// Inv.MinOrdQty
        /// </example>
        public struct Inventory
        {
            /// <summary>
            /// User ID
            /// </summary>
            public int UserID;
            /// <summary>
            /// Product ID
            /// </summary>
            public int ProductID;
            /// <summary>
            /// Quantity Available
            /// </summary>
            public decimal QtyAvail;
            /// <summary>
            /// Minimum Order Quantity
            /// </summary>
            public decimal MinOrdQty;
            /// <summary>
            /// Lead Time
            /// </summary>
            public string LeadTime;
            /// <summary>
            /// Sale Status
            /// </summary>
            public bool isOnSale;
            /// <summary>
            /// Discount Price
            /// </summary>
            public decimal Discount;
            /// <summary>
            /// List Price Valid Date
            /// </summary>
            public string ListPriceValidTill;
            /// <summary>
            /// Sales Price Valid Date
            /// </summary>
            public string SalePriceValidTill;
            /// <summary>
            /// Product's Status
            /// </summary>
            public int ProductStatus;
        }
        /// <summary>
        /// Create Enum Values for Product Status Details
        /// </summary>
        /// <example>
        /// oInv.ProductStatus.AVALIABLE;
        /// </example>
        public enum ProductStatus
        {
            /// <summary>
            /// Available Products
            /// </summary>
            AVALIABLE = 1,
            /// <summary>
            /// Not Available Products
            /// </summary>
            NA = 2,
            /// <summary>
            /// Discontinued Products
            /// </summary>
            DISCOUNTINUED = 3
        }
        /// <summary>
        /// Create Enum Values for Shipping Company Details
        /// </summary>
        /// <example>
        /// oInv.ShippingCompany.FEDEX;
        /// </example>
        public enum ShippingCompany
        {
            /// <summary>
            /// FEDEX Shipping Company
            /// </summary>
            ///
            FEDEX = 1,
            /// <summary>
            /// DHL Shipping Company
            /// </summary>
            DHL = 2,
            /// <summary>
            /// UPS Shipping Company
            /// </summary>
            UPS = 3,
            /// <summary>
            /// USPS Shipping Company
            /// </summary>
            USPS = 4
        }
        /// <summary>
        /// Default Constructor
        /// </summary>

        /*********************************** CONSTRUCTOR ***********************************/  
        public OrderServices()
        {

        }
        /*********************************** CONSTRUCTOR ***********************************/
        /*********************************** OLD CODE ***********************************/
        #region "Order Data Retrieve Functions"
        /// <summary>
        /// This is used to create the New Order in the Order Table
        /// </summary>
        /// <param name="oInfo">OrderInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderInfo oInfo = new Order.OrderInfo();
        ///     int order;
        ///     ...
        ///     order = oOrder.CreateOrder(oInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CRETAE A NEW ORDER IN ORDER TABLE ***/
        /********************************************************************************/
        public int CreateOrder(OrderInfo oInfo)
        {
            try
            {
                string sSQL;
                //sSQL = "INSERT INTO TBWC_ORDER(USER_ID,SHIP_PREFIX,SHIP_FNAME,SHIP_LNAME,SHIP_MNAME,SHIP_SUFFIX,SHIP_ADDRESS_LINE1,SHIP_ADDRESS_LINE2,SHIP_ADDRESS_LINE3,";
                //sSQL = sSQL + "SHIP_CITY,SHIP_STATE,SHIP_ZIP,SHIP_COUNTRY,SHIP_PHONE,SHIP_NOTES,ORDER_STATUS,PRODUCT_TOTAL_PRICE,TAX_AMOUNT,SHIP_COST,TOTAL_AMOUNT,";
                //sSQL = sSQL + "CREATED_DATE,CREATED_USER,MODIFIED_DATE,MODIFIED_USER,SHIP_METHOD,IS_SHIPPED,TRACKING_NO,EST_DELIVERY,SHIP_COMPANY,SHIP_CONF,ORDER_EMAIL_SENT,ORDER_INVOICE_SENT) ";
                //sSQL = sSQL + "VALUES(" + oInfo.UserID + ",'" + oInfo.ShipPrefix + "','" + oInfo.ShipFName + "','" + oInfo.ShipLName + "','" + oInfo.ShipMName + "','" + oInfo.ShipSuffix + "','" + oInfo.ShipAdd1 + "','" + oInfo.ShipAdd2 + "','" + oInfo.ShipAdd3 + "','";
                //sSQL = sSQL + oInfo.ShipCity + "','" + oInfo.ShipState + "','" + oInfo.ShipZip + "','" + oInfo.ShipCountry + "','" + oInfo.ShipPhone + "','" + oInfo.ShipNotes + "'," + oInfo.OrderStatus + "," + oInfo.ProdTotalPrice + ",";
                //sSQL = sSQL + oInfo.TaxAmount + "," + oInfo.ShipCost + "," + oInfo.TotalAmount + ",{fn now()},'" + oInfo.UserID + "',{fn now()},'" + oInfo.UserID + "','" + oInfo.ShipMethod + "',";
                //sSQL = sSQL + oInfo.IsShipped + ",'" + oInfo.TrackingNo + "','" + oInfo.EstDelivery + "','" + oInfo.ShipCompany + "','" + oInfo.ShipConf + "'," + oInfo.isEmailSent + "," + oInfo.isInvoiceSent + ")";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();

                sSQL = "exec STP_TBWC_POP_SAVE_ORDER ";
                sSQL = sSQL + oInfo.UserID + ",'" + oInfo.ShipPrefix + "','" + oInfo.ShipFName + "','" + oInfo.ShipLName + "','" + oInfo.ShipMName + "','" + oInfo.ShipSuffix + "','" + oInfo.ShipAdd1 + "','" + oInfo.ShipAdd2 + "','" + oInfo.ShipAdd3 + "','";
                sSQL = sSQL + oInfo.ShipCity + "','" + oInfo.ShipState + "','" + oInfo.ShipZip + "','" + oInfo.ShipCountry + "','" + oInfo.ShipPhone + "','" + oInfo.ShipNotes + "'," + oInfo.OrderStatus + "," + oInfo.ProdTotalPrice + ",";
                sSQL = sSQL + oInfo.TaxAmount + "," + oInfo.ShipCost + "," + oInfo.TotalAmount + ",'" + DateTime.Now + "','" + oInfo.UserID + "','" + DateTime.Now + "','" + oInfo.UserID + "','" + oInfo.ShipMethod + "',";
                sSQL = sSQL + oInfo.IsShipped + ",'" + oInfo.TrackingNo + "','" + oInfo.EstDelivery + "','" + oInfo.ShipCompany + "','" + oInfo.ShipConf + "'," + oInfo.isEmailSent + "," + oInfo.isInvoiceSent;
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
        /// This is used to Insert the New Order in the Order Table
        /// </summary>
        /// <param name="oInfo">oInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderInfo oInfo = new Order.OrderInfo();
        ///     int initOrder;
        ///     ...
        ///     initOrder = oOrder.InitilizeOrder(oInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

      
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO STORE THR ORDER DETAILS INTO THE ORDER TABLE  ***/
        /********************************************************************************/
        public int InitilizeOrder(OrderInfo oInfo)
        {
            try
            {
                //string sSQL = " INSERT INTO TBWC_ORDER(USER_ID,ORDER_STATUS,IS_SHIPPED,ORDER_EMAIL_SENT,ORDER_INVOICE_SENT,CREATED_USER,MODIFIED_USER)";
                //sSQL = sSQL + " VALUES( " + oInfo.UserID + ",1,0,0,0," + oInfo.UserID + "," + oInfo.UserID + " )";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

                string sSQL;
                sSQL = "exec STP_TBWC_POP_SAVE_ORDER_INITILIZE ";
                sSQL = sSQL + oInfo.UserID + ",1,0,0,0," + oInfo.UserID + "," + oInfo.UserID + "," + websiteid;
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }

        }

        public int InitilizeOrder_ipaddress( int order_id ,string clt_ip_address)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

                string sSQL;
                sSQL = "exec STP_TBWC_RENEW_ORDER_INITILIZE_IPADDRESS ";
                sSQL = sSQL + order_id.ToString() + "," + websiteid + ",'" + clt_ip_address.ToString() + "'";
               return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }

        }

        public int OrderUpdate_UserId(int order_id, int  user_id)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

                string sSQL;
                sSQL = "exec STP_TBWC_RENEW_ORDER_USERID ";
                sSQL = sSQL + order_id.ToString() + "," + websiteid + "," + user_id + "";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
            

        }
        public int OrderItemsUpdate_UserId(int order_id, int user_id,string prd_session_id,int neworder_id,string prod_session_id)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

                string sSQL;
                sSQL = "exec STP_TBWC_RENEW_ORDER_DETAILS_USERID ";
                sSQL = sSQL + order_id.ToString() + "," + user_id + ",'" + prd_session_id + "'," + neworder_id + ",'" + prod_session_id + "'";
              //  objErrorHandler.CreateLog(sSQL);
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
             
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }


        }

        public int OrderItemsUpdate_UserId_ExpressCheckout(int order_id, int user_id, string prd_session_id)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

                string sSQL;
                sSQL = "exec STP_TBWC_RENEW_ORDER_DETAILS_USERID_EXPRESS_CHECKOUT ";
                sSQL = sSQL + order_id.ToString() + "," + user_id + ",'" + prd_session_id + "'";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);

            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        public int OrderItemsUpdate_flg(int order_id, int user_id, string prd_session_id, int sessinflg)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

                string sSQL;
                sSQL = "exec STP_TBWC_RENEW_ORDER_DETAILS_FLGUPDATE ";
                sSQL = sSQL + order_id.ToString() + "," + user_id + ",'" + prd_session_id + "'," + sessinflg + "";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }


        }


        public DataTable GetSubstituteProduct(string ProductCode)
        {
            DataTable retvalue = null;
            try
            {
                string sSQL = string.Format("EXEC STP_TBWC_PICK_SUBSTITUTE_PRODUCT '" + ProductCode + "'");
                retvalue = objHelperDB.GetDataTableDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }

        public DataTable CheckMailLog(int OrderID)
        {
            DataTable retvalue = null;
            try
            {
                string sSQL = string.Format("EXEC Get_Order_Mail_Send_Details '" + OrderID + "'");
                retvalue = objHelperDB.GetDataTableDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }
        public DataTable CheckMailLog_Email(int OrderID,string Email)
        {
            DataTable retvalue = null;
            try
            {
                string sSQL = string.Format("EXEC Get_Order_Mail_Send_Details_WITHEmail '" + OrderID + "','"+ Email +"'");
                retvalue = objHelperDB.GetDataTableDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }
        public DataTable GetSubstituteProduct_wagnerproductcode(string product_code)
        {
            DataTable retvalue = null;
            try
            {
                string sSQL = string.Format("EXEC STP_TBWC_PICK_SUBSTITUTE_PRODUCT_WAGNER '" + product_code + "'");
                retvalue = objHelperDB.GetDataTableDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }

        public int OrderDelete_tempordid(int order_id, int user_id)
        {
            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

                string sSQL;
                sSQL = "exec STP_TBWC_CANCEL_ORDER_TEMPID ";
                sSQL = sSQL + order_id.ToString() + "," + user_id + "," + websiteid + "";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }


        }

        public int OrderId_checkcurruserid(int user_id)
        {
          
            try
            {
               
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

                string sSQL;
                sSQL = "exec STP_TBWC_PICK_ORDERID_CURRENTID ";
                sSQL = sSQL  + user_id + "," + websiteid + "";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }

            
            
        }

        public int ChkDelete_PreviousDupOrderItem(int user_id)
        {

            try
            {

                string sSQL;
                sSQL = "exec STP_TBWC_CANCEL_ORDER_ITEM_PRERECORDS ";
                sSQL = sSQL + user_id + "";
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
        /// This is used to get the Order
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <returns>OrderInfo</returns>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     Order.OrderInfo oInfo = new Order.OrderInfo();
        ///     ...
        ///     oInfo = oOrder.GetOrder(OrderID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public OrderInfo GetOrder(int OrderID)
        //{
        //    string sSQL;
        //    DataSet dsOD = new DataSet();
        //    OrderInfo rInfo = new OrderInfo();
        //    try
        //    {
        //        //sSQL = "SELECT * FROM TBWC_ORDER WHERE ORDER_ID = " + OrderID;
        //        //oHelper.SQLString = sSQL;
        //        //dsOD = oHelper.GetDataSet("Order");
        //        dsOD = (DataSet)objOrderDB.GetGenericDataDB(OrderID.ToString(), "GET_ORDER_DETAILS", OrderDB.ReturnType.RTDataSet);
        //        if (dsOD != null)
        //        {
        //            dsOD.Tables[0].TableName = "Order";
        //            foreach (DataRow drOD in dsOD.Tables["Order"].Rows)
        //            {
        //                rInfo.OrderID = objHelperService.CI(drOD["ORDER_ID"]);
        //                rInfo.ShipPrefix = objHelperService.CS(drOD["SHIP_PREFIX"]);
        //                rInfo.ShipFName = objHelperService.CS(drOD["SHIP_FNAME"]);
        //                rInfo.ShipLName = objHelperService.CS(drOD["SHIP_LNAME"]);
        //                rInfo.ShipMName = objHelperService.CS(drOD["SHIP_MNAME"]);
        //                rInfo.ShipSuffix = objHelperService.CS(drOD["SHIP_SUFFIX"]);
        //                rInfo.ShipAdd1 = objHelperService.CS(drOD["SHIP_ADDRESS_LINE1"]);
        //                rInfo.ShipAdd2 = objHelperService.CS(drOD["SHIP_ADDRESS_LINE2"]);
        //                rInfo.ShipAdd3 = objHelperService.CS(drOD["SHIP_ADDRESS_LINE3"]);
        //                rInfo.ShipCity = objHelperService.CS(drOD["SHIP_CITY"]);
        //                rInfo.ShipState = objHelperService.CS(drOD["SHIP_STATE"]);
        //                rInfo.ShipZip = objHelperService.CS(drOD["SHIP_ZIP"]);
        //                rInfo.ShipCountry = objHelperService.CS(drOD["SHIP_COUNTRY"]);
        //                rInfo.ShipPhone = objHelperService.CS(drOD["SHIP_PHONE"]);
        //                rInfo.ShipNotes = objHelperService.CS(drOD["SHIP_NOTES"]);

        //                rInfo.BillFName = objHelperService.CS(drOD["BILL_FNAME"]);
        //                rInfo.BillLName = objHelperService.CS(drOD["BILL_LNAME"]);
        //                rInfo.BillMName = objHelperService.CS(drOD["BILL_MNAME"]);
        //                rInfo.BillAdd1 = objHelperService.CS(drOD["BILL_ADDRESS_LINE1"]);
        //                rInfo.BillAdd2 = objHelperService.CS(drOD["BILL_ADDRESS_LINE2"]);
        //                rInfo.BillAdd3 = objHelperService.CS(drOD["BILL_ADDRESS_LINE3"]);
        //                rInfo.BillCity = objHelperService.CS(drOD["BILL_CITY"]);
        //                rInfo.BillState = objHelperService.CS(drOD["BILL_STATE"]);
        //                rInfo.BillZip = objHelperService.CS(drOD["BILL_ZIP"]);
        //                rInfo.BillCountry = objHelperService.CS(drOD["BILL_COUNTRY"]);
        //                rInfo.BillPhone = objHelperService.CS(drOD["BILL_PHONE"]);

        //                rInfo.OrderStatus = objHelperService.CI(drOD["ORDER_STATUS"]);
        //                rInfo.TaxAmount = objHelperService.CDEC(drOD["TAX_AMOUNT"]);
        //                rInfo.ShipCost = objHelperService.CDEC(drOD["SHIP_COST"]);
        //                rInfo.TotalAmount = objHelperService.CDEC(drOD["TOTAL_AMOUNT"]);
        //                rInfo.ProdTotalPrice = objHelperService.CDEC(drOD["PRODUCT_TOTAL_PRICE"]);
        //                rInfo.ShipMethod = objHelperService.CS(drOD["SHIP_METHOD"]);
        //                rInfo.IsShipped = (bool)drOD["IS_SHIPPED"];
        //                rInfo.TrackingNo = objHelperService.CS(drOD["TRACKING_NO"]);
        //                rInfo.EstDelivery = objHelperService.CS(drOD["EST_DELIVERY"]);
        //                rInfo.ShipCompany = objHelperService.CS(drOD["SHIP_COMPANY"]);
        //                rInfo.InvoiceNo = objHelperService.CS(drOD["INVOICENO"]);
        //                rInfo.ShipConf = objHelperService.CS(drOD["SHIP_CONF"]);
        //                rInfo.isEmailSent = (bool)drOD["ORDER_EMAIL_SENT"];
        //                rInfo.isInvoiceSent = (bool)drOD["ORDER_INVOICE_SENT"];
        //                rInfo.CreatedDate = (DateTime)drOD["CREATED_DATE"];
        //                rInfo.CreatedUser = objHelperService.CI(drOD["CREATED_USER"]);
        //                rInfo.ModifiedDate = (DateTime)drOD["MODIFIED_DATE"];
        //                rInfo.ModifiedUser = objHelperService.CI(drOD["MODIFIED_USER"]);
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //    }
        //    return rInfo;
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE ORDER DETAILS FROM RELATED DATABASE TABLES ***/
        /********************************************************************************/
        public OrderInfo GetOrder(int OrderID)
        {
            string sSQL;
            DataSet dsOD = new DataSet();
            OrderInfo rInfo = new OrderInfo();
            try
            {
                //sSQL = "SELECT * FROM TBWC_ORDER WHERE ORDER_ID = " + OrderID;
                //oHelper.SQLString = sSQL;
                //dsOD = oHelper.GetDataSet("Order");
                dsOD = (DataSet)objOrderDB.GetGenericDataDB(OrderID.ToString(), "GET_ORDER_DETAILS", OrderDB.ReturnType.RTDataSet);
                if (dsOD != null)
                {
                    dsOD.Tables[0].TableName = "Order";
                    foreach (DataRow drOD in dsOD.Tables["Order"].Rows)
                    {
                        rInfo.OrderID = objHelperService.CI(drOD["ORDER_ID"]);
                        rInfo.ShipPrefix = objHelperService.CS(drOD["SHIP_PREFIX"]);
                        rInfo.ShipFName = objHelperService.CS(drOD["SHIP_FNAME"]);
                        rInfo.ShipLName = objHelperService.CS(drOD["SHIP_LNAME"]);
                        rInfo.ShipMName = objHelperService.CS(drOD["SHIP_MNAME"]);
                        rInfo.ShipSuffix = objHelperService.CS(drOD["SHIP_SUFFIX"]);
                        rInfo.ShipAdd1 = objHelperService.CS(drOD["SHIP_ADDRESS_LINE1"]);
                        rInfo.ShipAdd2 = objHelperService.CS(drOD["SHIP_ADDRESS_LINE2"]);
                        rInfo.ShipAdd3 = objHelperService.CS(drOD["SHIP_ADDRESS_LINE3"]);
                        rInfo.ShipCity = objHelperService.CS(drOD["SHIP_CITY"]);
                        rInfo.ShipState = objHelperService.CS(drOD["SHIP_STATE"]);
                        rInfo.ShipZip = objHelperService.CS(drOD["SHIP_ZIP"]);
                        rInfo.ShipCountry = objHelperService.CS(drOD["SHIP_COUNTRY"]);
                        rInfo.ShipPhone = objHelperService.CS(drOD["SHIP_PHONE"]);
                        rInfo.ShipNotes = objHelperService.CS(drOD["SHIP_NOTES"]);

                        rInfo.BillFName = objHelperService.CS(drOD["BILL_FNAME"]);
                        rInfo.BillLName = objHelperService.CS(drOD["BILL_LNAME"]);
                        rInfo.BillMName = objHelperService.CS(drOD["BILL_MNAME"]);
                        rInfo.BillAdd1 = objHelperService.CS(drOD["BILL_ADDRESS_LINE1"]);
                        rInfo.BillAdd2 = objHelperService.CS(drOD["BILL_ADDRESS_LINE2"]);
                        rInfo.BillAdd3 = objHelperService.CS(drOD["BILL_ADDRESS_LINE3"]);
                        rInfo.BillCity = objHelperService.CS(drOD["BILL_CITY"]);
                        rInfo.BillState = objHelperService.CS(drOD["BILL_STATE"]);
                        rInfo.BillZip = objHelperService.CS(drOD["BILL_ZIP"]);
                        rInfo.BillCountry = objHelperService.CS(drOD["BILL_COUNTRY"]);
                        rInfo.BillPhone = objHelperService.CS(drOD["BILL_PHONE"]);
                        rInfo.BillcompanyName = objHelperService.CS(drOD["BILL_COMPNAME"]);

                        rInfo.OrderStatus = objHelperService.CI(drOD["ORDER_STATUS"]);
                        rInfo.TaxAmount = objHelperService.CDEC(drOD["TAX_AMOUNT"]);
                        rInfo.ShipCost = objHelperService.CDEC(drOD["SHIP_COST"]);
                        rInfo.TotalAmount = objHelperService.CDEC(drOD["TOTAL_AMOUNT"]);
                        rInfo.ProdTotalPrice = objHelperService.CDEC(drOD["PRODUCT_TOTAL_PRICE"]);
                        rInfo.ShipMethod = objHelperService.CS(drOD["SHIP_METHOD"]);
                        rInfo.IsShipped = (bool)drOD["IS_SHIPPED"];

                        rInfo.TrackingNo = objHelperService.CS(drOD["TRACKING_NO"]);
                        rInfo.EstDelivery = objHelperService.CS(drOD["EST_DELIVERY"]);
                        rInfo.ShipCompany = objHelperService.CS(drOD["SHIP_COMPANY"]);
                        rInfo.InvoiceNo = objHelperService.CS(drOD["INVOICENO"]);
                        rInfo.ShipConf = objHelperService.CS(drOD["SHIP_CONF"]);
                        rInfo.isEmailSent = (bool)drOD["ORDER_EMAIL_SENT"];
                        rInfo.isInvoiceSent = (bool)drOD["ORDER_INVOICE_SENT"];
                        rInfo.CreatedDate = (DateTime)drOD["CREATED_DATE"];
                        rInfo.CreatedUser = objHelperService.CI(drOD["CREATED_USER"]);
                        rInfo.ModifiedDate = (DateTime)drOD["MODIFIED_DATE"];
                        rInfo.ModifiedUser = objHelperService.CI(drOD["MODIFIED_USER"]);
                        rInfo.ShipCompName = objHelperService.CS(drOD["SHIP_COMPNAME"]);

                        rInfo.DeliveryInstr = objHelperService.CS(drOD["DELIVERYINST"]);
                        rInfo.Receiver_Contact = objHelperService.CS(drOD["RECEIVER_CONTACT"]);
                        rInfo.ShipTrackUrl = objHelperService.CS(drOD["SHIPTRACKURL"]);
                        rInfo.UserID = objHelperService.CI(drOD["USER_ID"]);
                        rInfo.Websiteid = objHelperService.CI(drOD["WEBSITE_ID"]);
                        rInfo.Bill_Name = objHelperService.CS(drOD["BILL_NAME"]);
                        try
                        {
                            if (!string.IsNullOrEmpty(drOD["INVOICEDATE"].ToString()))
                            {
                                rInfo.InvoiceDate = (DateTime)drOD["INVOICEDATE"];
                            }
                        }
                        catch { }
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            return rInfo;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to Update all the Order Details
        /// </summary>
        /// <param name="oInfo">OrderInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderInfo oInfo = new Order.OrderInfo();
        ///     int UpdateOrderStatus;
        ///     ...
        ///     UpdateOrderStatus = oOrder.UpdateOrder(oInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE THE ORDER DETAILS  ***/
        /********************************************************************************/
        public int UpdateOrder(OrderInfo oInfo)
        {

            try
            {
                string sSQL;
                //sSQL = "UPDATE TBWC_ORDER SET SHIP_PREFIX='" + oInfo.ShipPrefix + "',SHIP_FNAME='" + oInfo.ShipFName + "',";
                //sSQL = sSQL + "SHIP_LNAME='" + oInfo.ShipLName + "',SHIP_MNAME='" + oInfo.ShipMName + "',SHIP_SUFFIX='" + oInfo.ShipSuffix + "',";
                //sSQL = sSQL + "SHIP_ADDRESS_LINE1='" + oInfo.ShipAdd1 + "',SHIP_ADDRESS_LINE2='" + oInfo.ShipAdd2 + "',SHIP_ADDRESS_LINE3='" + oInfo.ShipAdd3 + "',";
                //sSQL = sSQL + "SHIP_CITY='" + oInfo.ShipCity + "',SHIP_STATE='" + oInfo.ShipState + "',SHIP_ZIP='" + oInfo.ShipZip + "',";
                //sSQL = sSQL + "SHIP_COUNTRY='" + oInfo.ShipCountry + "',SHIP_PHONE='" + oInfo.ShipPhone + "',SHIP_NOTES='" + oInfo.ShipNotes + "',";

                //sSQL = sSQL + "BILL_FNAME='" + oInfo.BillFName + "',BILL_LNAME='" + oInfo.BillLName + "',BILL_MNAME='" + oInfo.BillMName + "',";
                //sSQL = sSQL + "BILL_ADDRESS_LINE1='" + oInfo.BillAdd1 + "',BILL_ADDRESS_LINE2='" + oInfo.BillAdd2 + "',BILL_ADDRESS_LINE3='" + oInfo.BillAdd3 + "',";
                //sSQL = sSQL + "BILL_CITY='" + oInfo.BillCity + "',BILL_STATE='" + oInfo.BillState + "',BILL_ZIP='" + oInfo.BillZip + "',";
                //sSQL = sSQL + "BILL_COUNTRY='" + oInfo.BillCountry + "',BILL_PHONE='" + oInfo.BillPhone + "',";

                //sSQL = sSQL + "ORDER_STATUS=" + oInfo.OrderStatus + ",PRODUCT_TOTAL_PRICE=" + oInfo.ProdTotalPrice + ",TAX_AMOUNT=" + oInfo.TaxAmount + ",";
                //sSQL = sSQL + "SHIP_COST=" + oInfo.ShipCost + ",TOTAL_AMOUNT=" + oInfo.TotalAmount + ",";
                //sSQL = sSQL + "SHIP_METHOD='" + oInfo.ShipMethod + "',IS_SHIPPED='" + oInfo.IsShipped + "',TRACKING_NO='" + oInfo.TrackingNo + "',";
                //sSQL = sSQL + "EST_DELIVERY='" + oInfo.EstDelivery + "',SHIP_COMPANY='" + oInfo.ShipCompany + "',SHIP_CONF='" + oInfo.ShipConf + "',";
                //sSQL = sSQL + "ORDER_EMAIL_SENT='" + oInfo.isEmailSent + "',ORDER_INVOICE_SENT='" + oInfo.isInvoiceSent + "',DROPSHIP=" + oInfo.DropShip + ",";
                //sSQL = sSQL + "SHIP_COMPNAME='" + oInfo.ShipCompName + "',DELIVERYINST='" + oInfo.DeliveryInstr + "',";
                //sSQL = sSQL + "MODIFIED_USER=" + oInfo.UserID + ",CUSTOM_TEXT_FIELD1='" + oInfo.ClientIPAddress + "'  WHERE ORDER_ID=" + oInfo.OrderID;
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string isppp="";
                //string shipcode = "";
                sSQL = "exec STP_TBWC_RENEW_UPDATE_ORDER ";
                sSQL = sSQL + "'" + oInfo.ShipPrefix + "','" + oInfo.ShipFName + "',";
                sSQL = sSQL + "'" + oInfo.ShipLName + "','" + oInfo.ShipMName + "','" + oInfo.ShipSuffix + "',";
                sSQL = sSQL + "'" + oInfo.ShipAdd1 + "','" + oInfo.ShipAdd2 + "','" + oInfo.ShipAdd3 + "',";
                sSQL = sSQL + "'" + oInfo.ShipCity + "','" + oInfo.ShipState + "','" + oInfo.ShipZip + "',";
                sSQL = sSQL + "'" + oInfo.ShipCountry + "','" + oInfo.ShipPhone + "','" + oInfo.ShipNotes + "',";

                sSQL = sSQL + "'" + oInfo.BillFName + "','" + oInfo.BillLName + "','" + oInfo.BillMName + "',";
                sSQL = sSQL + "'" + oInfo.BillAdd1 + "','" + oInfo.BillAdd2 + "','" + oInfo.BillAdd3 + "',";
                sSQL = sSQL + "'" + oInfo.BillCity + "','" + oInfo.BillState + "','" + oInfo.BillZip + "',";
                sSQL = sSQL + "'" + oInfo.BillCountry + "','" + oInfo.BillPhone + "',";

                sSQL = sSQL + "" + oInfo.OrderStatus + "," + oInfo.ProdTotalPrice + "," + oInfo.TaxAmount + ",";
                sSQL = sSQL + "" + oInfo.ShipCost + "," + oInfo.TotalAmount + ",";
                sSQL = sSQL + "'" + oInfo.ShipMethod + "','" + oInfo.IsShipped + "','" + oInfo.TrackingNo + "',";
                sSQL = sSQL + "'" + oInfo.EstDelivery + "','" + oInfo.ShipCompany + "','" + oInfo.ShipConf + "',";
                sSQL = sSQL + "'" + oInfo.isEmailSent + "','" + oInfo.isInvoiceSent + "'," + oInfo.DropShip + ",";
                sSQL = sSQL + "'" + oInfo.ShipCompName + "','" + oInfo.DeliveryInstr + "',";
                sSQL = sSQL + "" + oInfo.UserID + ",'" + oInfo.ClientIPAddress + "',";
                sSQL = sSQL +"" + oInfo.OrderID +",'"+ isppp +"','"+ oInfo.SHIP_CODE +"','"+oInfo.Receiver_Contact+"',";
                sSQL = sSQL + "'" + oInfo.BillcompanyName + "', '" + oInfo.Bill_Name + "'";
                 //objErrorHandler.CreatePayLog_Final(sSQL);

                //sSQL = sSQL + " Exec STP_TBWC_UPDATE_SHIP_NUMBER_ORDER ";
                //sSQL = sSQL + "'" + oInfo.SHIP_NUMBER + "'," + oInfo.OrderID + "";

                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {
                //oErrHand.ErrorMsg = e;
                ////oErrHand.CreateLog();
                //return -1;
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }

        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to set the Price of the Order
        /// </summary>
        /// <param name="oOrdInfo">OrderInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderInfo oOrdInfo = new Order.OrderInfo();
        ///     int UpdateOrdPr;
        ///     ...
        ///     UpdateOrdPr = oOrder.UpdateOrderPrice(oOrdInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public int UpdateOrderPrice(OrderInfo oOrdInfo, bool ItemShipping)
        //{
        //    int retVal;
        //    try
        //    {
        //        //string sSQL = " UPDATE TBWC_ORDER SET PRODUCT_TOTAL_PRICE = " + oOrdInfo.ProdTotalPrice  + ",";
        //        //sSQL = sSQL + " TAX_AMOUNT =" + oOrdInfo.TaxAmount + ",TOTAL_AMOUNT = " + oOrdInfo.TotalAmount;
        //        //if (ItemShipping)
        //        //{
        //        //sSQL = sSQL + ",SHIP_COST = " + oOrdInfo.ShipCost;
        //        //}
        //        //sSQL = sSQL + " WHERE ORDER_ID =" + oOrdInfo.OrderID;
        //        //oHelper.SQLString = sSQL;
        //        //retVal = oHelper.ExecuteSQLQuery();
        //        string sSQL = " exec STP_TBWC_RENEW_ORDER_PRICE ";
        //        sSQL = sSQL + "" + oOrdInfo.ProdTotalPrice + ",";
        //        sSQL = sSQL + "" + oOrdInfo.TaxAmount + "," + oOrdInfo.TotalAmount;
        //        sSQL = sSQL + "," + oOrdInfo.ShipCost;
        //        sSQL = sSQL + "," + oOrdInfo.OrderID + ",'" + ((ItemShipping == true) ? "1" : "0") + "'";
        //        retVal = objHelperDB.ExecuteSQLQueryDB(sSQL);


        //    }
        //    catch (Exception e)
        //    {

        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        retVal = -1;
        //    }

        //    return retVal;
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO SET PRODUCT TOTAL PRICE,TAX AMOUNT,TOTAL AMOUNT FOR ORDER DETAILS  ***/
        /********************************************************************************/
        public int UpdateOrderPrice(OrderInfo oOrdInfo, bool ItemShipping)
        {
            int retVal;
            try
            {
                //string sSQL = " UPDATE TBWC_ORDER SET PRODUCT_TOTAL_PRICE = " + oOrdInfo.ProdTotalPrice  + ",";
                //sSQL = sSQL + " TAX_AMOUNT =" + oOrdInfo.TaxAmount + ",TOTAL_AMOUNT = " + oOrdInfo.TotalAmount;
                //if (ItemShipping)
                //{
                //sSQL = sSQL + ",SHIP_COST = " + oOrdInfo.ShipCost;
                //}
                //sSQL = sSQL + " WHERE ORDER_ID =" + oOrdInfo.OrderID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.ExecuteSQLQuery();
                retVal = -1;
                DataSet tmpds = GetOrderItemDetailSum(oOrdInfo.OrderID);
                if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                {
                    oOrdInfo.ProdTotalPrice = objHelperService.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                    oOrdInfo.TaxAmount = objHelperService.CDEC(tmpds.Tables[0].Rows[0]["TAX_AMOUNT"].ToString());
                    if (HttpContext.Current.Session["tabl3" + oOrdInfo.OrderID] != null || oOrdInfo.ShipMethod=="Standard Shipping")
                    {
                        //objErrorHandler.CreatePayLog_Final("tab3 session" + "tabl3" + HttpContext.Current.Session["tabl3" + oOrdInfo.OrderID].ToString());
                        if (HttpContext.Current.Session["tabl3" + oOrdInfo.OrderID].ToString() != "")
                        {
                            oOrdInfo.ShipCost = Convert.ToDecimal(HttpContext.Current.Session["tabl3" + oOrdInfo.OrderID].ToString());
                        }
                          oOrdInfo.TaxAmount = CalculateTaxAmount_Express(oOrdInfo.ProdTotalPrice + oOrdInfo.ShipCost, oOrdInfo.OrderID.ToString());

                        ItemShipping = true;
                    }
                    else
                    {
                        
                         oOrdInfo.ShipCost = objHelperService.CDEC(tmpds.Tables[0].Rows[0]["SHIP_COST"].ToString());
                    }
                    oOrdInfo.TotalAmount = oOrdInfo.ProdTotalPrice + oOrdInfo.TaxAmount + oOrdInfo.ShipCost;
                    objErrorHandler.CreatePayLog_Final(oOrdInfo.OrderID + "-ProdTotalPrice-" + oOrdInfo.ProdTotalPrice + "-TaxAmount-" + oOrdInfo.TaxAmount + "-ShipCost-" + oOrdInfo.ShipCost + "UpdateOrderPrice");
                    string sSQL = " exec STP_TBWC_RENEW_ORDER_PRICE ";
                    sSQL = sSQL + "" + oOrdInfo.ProdTotalPrice + ",";
                    sSQL = sSQL + "" + oOrdInfo.TaxAmount + "," + oOrdInfo.TotalAmount;
                    sSQL = sSQL + "," + oOrdInfo.ShipCost;
                    sSQL = sSQL + "," + oOrdInfo.OrderID + ",'" + ((ItemShipping == true) ? "1" : "0") + "'";
                    retVal = objHelperDB.ExecuteSQLQueryDB(sSQL);
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


        public int UpdateOrderPrice_ExpressCheckout(OrderInfo oOrdInfo, bool ItemShipping, string SessionID)
        {
            int retVal;
            try
            {
                retVal = -1;
                DataSet tmpds = GetOrderItemDetailSum_ExpressCheckout(oOrdInfo.OrderID, SessionID);
                if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                {
                    oOrdInfo.ProdTotalPrice = objHelperService.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                    oOrdInfo.TaxAmount = objHelperService.CDEC(tmpds.Tables[0].Rows[0]["TAX_AMOUNT"].ToString());
                 if (  HttpContext.Current.Session ["tabl3" + oOrdInfo.OrderID] !=null)
                    {
                       // objErrorHandler.CreatePayLog_Final("tab3 session" + "tabl3" + HttpContext.Current.Session["tabl3" + oOrdInfo.OrderID].ToString());
                    if (IsNativeCountry_Express( oOrdInfo.OrderID)== 0)
                    {
                     oOrdInfo.ShipCost =Convert.ToDecimal( HttpContext.Current.Session["tabl3" + oOrdInfo.OrderID].ToString());
                     oOrdInfo.TaxAmount = CalculateTaxAmount_Express(oOrdInfo.ProdTotalPrice + oOrdInfo.ShipCost, oOrdInfo.OrderID.ToString());
                    }
                       
                   
                     ItemShipping = true;
                 }
                 else
                 {
                  oOrdInfo.ShipCost =objHelperService.CDEC(tmpds.Tables[0].Rows[0]["SHIP_COST"].ToString());
                 }
                    oOrdInfo.TotalAmount = oOrdInfo.ProdTotalPrice + oOrdInfo.TaxAmount + oOrdInfo.ShipCost;
                    objErrorHandler.CreatePayLog_Final(oOrdInfo.OrderID + "-ProdTotalPrice-" + oOrdInfo.ProdTotalPrice + "-TaxAmount-" + oOrdInfo.TaxAmount + "-ShipCost-" + oOrdInfo.ShipCost + "UpdateOrderPrice_ExpressCheckout");
                    string sSQL = " exec STP_TBWC_RENEW_ORDER_PRICE ";
                    sSQL = sSQL + "" + oOrdInfo.ProdTotalPrice + ",";
                    sSQL = sSQL + "" + oOrdInfo.TaxAmount + "," + oOrdInfo.TotalAmount;
                    sSQL = sSQL + "," + oOrdInfo.ShipCost;
                    sSQL = sSQL + "," + oOrdInfo.OrderID + ",'" + ((ItemShipping == true) ? "1" : "0") + "'";
                    retVal = objHelperDB.ExecuteSQLQueryDB(sSQL);
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

        public int DeletePaymentRecordUnpaid(OrderInfo oOrdInfo)
        {
             int retVal;
             try
             {
                retVal = -1;
                //DataSet tmpds = GetOrderItemDetailSum(oOrdInfo.OrderID);
                //if (tmpds != null && tmpds.Tables.Count > 0 && tmpds.Tables[0].Rows.Count > 0)
                //{
                //    oOrdInfo.ProdTotalPrice = objHelperService.CDEC(tmpds.Tables[0].Rows[0]["PRODUCT_TOTAL_PRICE"].ToString());
                //    oOrdInfo.TaxAmount = objHelperService.CDEC(tmpds.Tables[0].Rows[0]["TAX_AMOUNT"].ToString());
                //    oOrdInfo.ShipCost = objHelperService.CDEC(tmpds.Tables[0].Rows[0]["SHIP_COST"].ToString());
                //    oOrdInfo.TotalAmount = oOrdInfo.ProdTotalPrice + oOrdInfo.TaxAmount + oOrdInfo.ShipCost;


                //    string sSQL = " exec STP_TBWC_RENEW_PAYMENT_ITEM_UPDATE ";
                //    sSQL = sSQL + "" + oOrdInfo.TotalAmount;
                //    sSQL = sSQL + "," + oOrdInfo.OrderID;
                //    retVal = objHelperDB.ExecuteSQLQueryDB(sSQL);
                //}

                string sSQL = " exec STP_TBWC_DELETE_PAYMENT_RECORD_UNPAID ";
                sSQL = sSQL + "" + oOrdInfo.OrderID;

             //   objErrorHandler.CreateLog(sSQL);
             retVal = objHelperDB.ExecuteSQLQueryDB(sSQL);
                
             }
             catch (Exception e)
             {

                 objErrorHandler.ErrorMsg = e;
                 objErrorHandler.CreateLog();
                 retVal = -1;
             }

             return retVal;
        }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK USER CAN DROP SHIPMENT OR NOT  ***/
        /********************************************************************************/
        public bool IsUserCanDropShip(int UserID)
        {
            bool isDropShip = false;
            string tempstr = "";
            try
            {
                //string sSQL = string.Format("SELECT ORDDRPSHP FROM TBWC_COMPANY_BUYERS WHERE USER_ID={0}", UserID);
                //oHelper.SQLString = sSQL;
                tempstr = (string)objOrderDB.GetGenericDataDB(UserID.ToString(), "Is_User_CanDropShip", OrderDB.ReturnType.RTString);

                // if (oHelper.GetValue("ORDDRPSHP").ToString() == "1")
                if (tempstr == "1")
                {
                    isDropShip = true;
                }
                else
                {
                    isDropShip = false;
                }
            }
            catch (Exception e)
            {
                //   oErrHand.ErrorMsg = e;
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return isDropShip;
            }
            return isDropShip;
        }

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public int UpdateCustomFields(OrderInfo oOrdInfo)
        //{
        //    int retval = -1;
        //    DataSet objds = new DataSet();
        //    string sShipCompany = "";
        //    int CompanyID = 0;
        //    string sBillCompany = "";
        //    int billto = 0;
        //    string sSQL = "";
        //    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();

        //    //string sSQL = string.Format("SELECT COMPANY_ID,COMPANY_NAME FROM TBWC_COMPANY WHERE COMPANY_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = {0})", oOrdInfo.UserID.ToString());
        //    //oHelper.SQLString = sSQL;
        //    //     string sShipCompany = string.IsNullOrEmpty(oOrdInfo.ShipCompName) ? oHelper.GetValue("COMPANY_NAME") : oOrdInfo.ShipCompName;

        //    //     int CompanyID = oHelper.CI(oHelper.GetValue("COMPANY_ID"));


        //    //sSQL = string.Format("SELECT CUST_NAME FROM WES_CUSTOMER WHERE WES_CUSTOMER_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = {0})", oOrdInfo.UserID.ToString());
        //    //oHelper.SQLString = sSQL;
        //    //string sBillCompany = oHelper.GetValue("CUST_NAME");

        //    //sSQL = string.Format("SELECT BILLTO_ID FROM WES_CUSTOMER WHERE WES_CUSTOMER_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = {0})", oOrdInfo.UserID.ToString());
        //    //oHelper.SQLString = sSQL;
        //    //int billto = oHelper.CI(oHelper.GetValue("BILLTO_ID"));

        //    //sSQL = string.Format("UPDATE TBWC_ORDER SET WEBSITE_ID = {0}, SHIP_COMPNAME='{1}', BILL_COMPNAME='{2}', BILLTO_ID = {3}, COMPANY_ID={5} WHERE ORDER_ID = {4}", websiteid, sShipCompany, sBillCompany, billto, oOrdInfo.OrderID, CompanyID);
        //    //oHelper.SQLString = sSQL;
        //    //retval = oHelper.ExecuteSQLQuery();
        //    //return retval;

        //    objds = (DataSet)objOrderDB.GetGenericDataDB(oOrdInfo.UserID.ToString(), "GET_ORDER_CUSTOM_FIELDS", OrderDB.ReturnType.RTDataSet);
        //    if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
        //    {
        //        sShipCompany = string.IsNullOrEmpty(oOrdInfo.ShipCompName) ? objds.Tables[0].Rows[0]["COMPANY_NAME"].ToString() : oOrdInfo.ShipCompName;

        //        CompanyID = objHelperService.CI(objds.Tables[0].Rows[0]["COMPANY_ID"].ToString());
        //    }
        //    //sSQL = string.Format("SELECT CUST_NAME FROM WES_CUSTOMER WHERE WES_CUSTOMER_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = {0})", oOrdInfo.UserID.ToString());
        //    //oHelper.SQLString = sSQL;
        //    //string sBillCompany = oHelper.GetValue("CUST_NAME");
        //    objds = (DataSet)objOrderDB.GetGenericDataDB(oOrdInfo.UserID.ToString(), "GET_ORDER_CUSTOM_FIELDS_1", OrderDB.ReturnType.RTDataSet);
        //    if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
        //    {
        //        sBillCompany = objds.Tables[0].Rows[0]["CUST_NAME"].ToString();
        //    }

        //    //sSQL = string.Format("SELECT BILLTO_ID FROM WES_CUSTOMER WHERE WES_CUSTOMER_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = {0})", oOrdInfo.UserID.ToString());
        //    //oHelper.SQLString = sSQL;
        //    //int billto = oHelper.CI(oHelper.GetValue("BILLTO_ID"));
        //    objds = (DataSet)objOrderDB.GetGenericDataDB(oOrdInfo.UserID.ToString(), "GET_ORDER_CUSTOM_FIELDS_2", OrderDB.ReturnType.RTDataSet);
        //    if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
        //    {
        //        billto = objHelperService.CI(objds.Tables[0].Rows[0]["BILLTO_ID"].ToString());
        //    }


        //    //sSQL = string.Format("UPDATE TBWC_ORDER SET WEBSITE_ID = {0}, SHIP_COMPNAME='{1}', BILL_COMPNAME='{2}', BILLTO_ID = {3}, COMPANY_ID={5} WHERE ORDER_ID = {4}", websiteid, sShipCompany, sBillCompany, billto, oOrdInfo.OrderID, CompanyID);
        //    //oHelper.SQLString = sSQL;
        //    //retval = oHelper.ExecuteSQLQuery();
        //    sSQL = "exec STP_TBWC_RENEW_ORDER_CUSTOM_FIELDS ";
        //    sSQL = sSQL + "" + websiteid + ",'" + sShipCompany + "','" + sBillCompany + "'," + billto + "," + oOrdInfo.OrderID + "," + CompanyID;
        //    retval = objHelperDB.ExecuteSQLQueryDB(sSQL);
        //    return retval;
        //}

        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE CUSTOM FIELDS IN ORDER TABLE  ***/
        /********************************************************************************/
        public int UpdateCustomFields(OrderInfo oOrdInfo)
        {
            int retval = -1;
            DataSet objds = new DataSet();
            string sShipCompany = "";
            int CompanyID = 0;
            string sBillCompany = "";
            int billto = 0;
            string sSQL = "";
            string delivery_inst = "";
            string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();

            //string sSQL = string.Format("SELECT COMPANY_ID,COMPANY_NAME FROM TBWC_COMPANY WHERE COMPANY_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = {0})", oOrdInfo.UserID.ToString());
            //oHelper.SQLString = sSQL;
            //     string sShipCompany = string.IsNullOrEmpty(oOrdInfo.ShipCompName) ? oHelper.GetValue("COMPANY_NAME") : oOrdInfo.ShipCompName;

            //     int CompanyID = oHelper.CI(oHelper.GetValue("COMPANY_ID"));


            //sSQL = string.Format("SELECT CUST_NAME FROM WES_CUSTOMER WHERE WES_CUSTOMER_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = {0})", oOrdInfo.UserID.ToString());
            //oHelper.SQLString = sSQL;
            //string sBillCompany = oHelper.GetValue("CUST_NAME");

            //sSQL = string.Format("SELECT BILLTO_ID FROM WES_CUSTOMER WHERE WES_CUSTOMER_ID = (SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID = {0})", oOrdInfo.UserID.ToString());
            //oHelper.SQLString = sSQL;
            //int billto = oHelper.CI(oHelper.GetValue("BILLTO_ID"));

            //sSQL = string.Format("UPDATE TBWC_ORDER SET WEBSITE_ID = {0}, SHIP_COMPNAME='{1}', BILL_COMPNAME='{2}', BILLTO_ID = {3}, COMPANY_ID={5} WHERE ORDER_ID = {4}", websiteid, sShipCompany, sBillCompany, billto, oOrdInfo.OrderID, CompanyID);
            //oHelper.SQLString = sSQL;
            //retval = oHelper.ExecuteSQLQuery();
            //return retval;
            
            /*objds = (DataSet)objOrderDB.GetGenericDataDB(oOrdInfo.UserID.ToString(), "GET_ORDER_CUSTOM_FIELDS", OrderDB.ReturnType.RTDataSet);
            if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
            {
                sShipCompany = string.IsNullOrEmpty(oOrdInfo.ShipCompName) ? objds.Tables[0].Rows[0]["COMPANY_NAME"].ToString() : oOrdInfo.ShipCompName;

                CompanyID = objHelperService.CI(objds.Tables[0].Rows[0]["COMPANY_ID"].ToString());
            }
            
            objds = (DataSet)objOrderDB.GetGenericDataDB(oOrdInfo.UserID.ToString(), "GET_ORDER_CUSTOM_FIELDS_1", OrderDB.ReturnType.RTDataSet);
            if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
            {
                sBillCompany = objds.Tables[0].Rows[0]["CUST_NAME"].ToString();
            }

           */
            objds = (DataSet)objOrderDB.GetGenericDataDB(oOrdInfo.UserID.ToString(), "GET_ORDER_CUSTOM_FIELDS_2", OrderDB.ReturnType.RTDataSet);
            if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
            {
                billto = objHelperService.CI(objds.Tables[0].Rows[0]["BILLTO_ID"].ToString());
            }
            sShipCompany=objUserServices.GetCompanyName(oOrdInfo.UserID) ;
            CompanyID = objUserServices.GetCompanyID(oOrdInfo.UserID);
            sBillCompany=objUserServices.GetBillToCompanyName(oOrdInfo.UserID); 
            


            //sSQL = string.Format("UPDATE TBWC_ORDER SET WEBSITE_ID = {0}, SHIP_COMPNAME='{1}', BILL_COMPNAME='{2}', BILLTO_ID = {3}, COMPANY_ID={5} WHERE ORDER_ID = {4}", websiteid, sShipCompany, sBillCompany, billto, oOrdInfo.OrderID, CompanyID);
            //oHelper.SQLString = sSQL;
            //retval = oHelper.ExecuteSQLQuery();
            sSQL = "exec STP_TBWC_RENEW_ORDER_CUSTOM_FIELDS ";
            sSQL = sSQL + "" + websiteid + ",'" + sShipCompany + "','" + sBillCompany + "'," + billto + "," + oOrdInfo.OrderID + "," + CompanyID;
            retval = objHelperDB.ExecuteSQLQueryDB(sSQL);
            return retval;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to Set the Price of Removed Items
        /// </summary>
        /// <param name="RemovedItemsPrice">decimal</param>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderId;
        ///     int retVal;
        ///     decimal Tax;
        ///     decimal RemovedItemsPrice;
        ///     ...
        ///     retVal = oOrder.UpdateRemovedItemsPrice(RemovedItemsPrice, OrderId, Tax);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public int UpdateRemovedItemsPrice(decimal RemovedItemsPrice, int OrderID, decimal TaxAmount, decimal ShipCost)
        //{
        //    int retVal;
        //    try
        //    {
        //        decimal TotalAmount = RemovedItemsPrice + TaxAmount + ShipCost;
        //        //string sSQL = "UPDATE TBWC_ORDER SET PRODUCT_TOTAL_PRICE = PRODUCT_TOTAL_PRICE - " + RemovedItemsPrice + ",SHIP_COST = SHIP_COST - " + ShipCost + ",TAX_AMOUNT =TAX_AMOUNT - " + TaxAmount + ",TOTAL_AMOUNT = TOTAL_AMOUNT -" + TotalAmount + " WHERE ORDER_ID = " + OrderID;
        //        //oHelper.SQLString = sSQL;
        //        //retVal = oHelper.ExecuteSQLQuery();
        //        string sSQL = "exec STP_TBWC_RENEW_ORDER_Removed_Price ";
        //        sSQL = sSQL + "" + RemovedItemsPrice + "," + ShipCost + "," + TaxAmount + "," + TotalAmount + "," + OrderID;
        //        return objHelperDB.ExecuteSQLQueryDB(sSQL);

        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        retVal = -1;
        //    }
        //    return retVal;
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE REMOVED ITEMS PRICE IN ORDER TABLE  ***/
        /********************************************************************************/       
        public int UpdateRemovedItemsPrice(decimal RemovedItemsPrice, int OrderID, decimal TaxAmount, decimal ShipCost)
        {
            int retVal;
            try
            {
                if (RemovedItemsPrice < 0)
                    RemovedItemsPrice = 0;

                if (TaxAmount < 0)
                    TaxAmount = 0;

                if (ShipCost < 0)
                    ShipCost = 0;

                decimal TotalAmount = RemovedItemsPrice + TaxAmount + ShipCost;
                //string sSQL = "UPDATE TBWC_ORDER SET PRODUCT_TOTAL_PRICE = PRODUCT_TOTAL_PRICE - " + RemovedItemsPrice + ",SHIP_COST = SHIP_COST - " + ShipCost + ",TAX_AMOUNT =TAX_AMOUNT - " + TaxAmount + ",TOTAL_AMOUNT = TOTAL_AMOUNT -" + TotalAmount + " WHERE ORDER_ID = " + OrderID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.ExecuteSQLQuery();
                string sSQL = "exec STP_TBWC_RENEW_ORDER_Removed_Price ";
                sSQL = sSQL + "" + RemovedItemsPrice + "," + ShipCost + "," + TaxAmount + "," + TotalAmount + "," + OrderID;
                return objHelperDB.ExecuteSQLQueryDB(sSQL);

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
        /// This is used to Set the Order Status
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <param name="OrderStatus">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     int OrdStatusID;
        ///     int UpdateOrdStatus;
        ///     ...
        ///     UpdateOrdStatus = oOrder.UpdateOrderStatus(OrderID, OrdStatusID)
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE ORDER STATUS BASED ON ORDER ID ***/
        /********************************************************************************/
        public int UpdateOrderStatus(int OrderID, int OrderStatus)
        {
            int retVal = 0;
            try
            {
                //string sSQL = "UPDATE TBWC_ORDER SET ORDER_STATUS = " + OrderStatus + " WHERE ORDER_ID = " + OrderID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.ExecuteSQLQuery();
                string sSQL = "Exec  STP_TBWC_RENEW_UPDATE_ORDER_STATUS ";
                sSQL = sSQL + "" + OrderID + "," + OrderStatus + "";
                retVal = objHelperDB.ExecuteSQLQueryDB(sSQL);

            }
            catch (Exception e)
            {
                // oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return retVal;
            }
            return retVal;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE QUOTE ID BASD ON ORDER ID ***/
        /********************************************************************************/
        public int UpdateQuoteID(int OrderID, int QuoteID)
        {
            int retVal = 0;
            try
            {
                //string sSQL = "UPDATE TBWC_ORDER SET QUOTE_ID = " + QuoteID + " WHERE ORDER_ID = " + OrderID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.ExecuteSQLQuery();
                string sSQL = "Exec STP_TBWC_RENEW_UPDATE_QUOTEID ";
                sSQL = sSQL + "" + OrderID + "," + QuoteID + " ";
                retVal = objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {
                //oErrHand.ErrorMsg = e;
                //return retVal;
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return retVal;
            }
            return retVal;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ORDER ID FOR QUOTE  ***/
        /********************************************************************************/
        public int GetOrderIDForQuote(int QuoteID)
        {
            int retVal = 0;
            try
            {
                string OrderId;
                //string sSQL = "SELECT ORDER_ID FROM TBWC_ORDER WHERE QUOTE_ID= " + QuoteID;
                //oHelper.SQLString = sSQL;
                //OrderId = oHelper.GetValue("ORDER_ID");
                OrderId = (string)objOrderDB.GetGenericDataDB(QuoteID.ToString(), "Get_OrderID_For_Quote", OrderDB.ReturnType.RTString);

                if (OrderId == "" && OrderId == null)
                {
                    retVal = 0;
                }
                else
                {
                    // return oHelper.CI(OrderId);
                    retVal = objHelperService.CI(OrderId);
                }
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
        /// This is used to get the Order ID based on User ID
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
        ///     Order oOrder = new Order();
        ///     int UserID;
        ///     int OrdID;
        ///     ...
        ///     OrdID = oOrder.GetOrderID(UserID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE ORDER ID  ***/
        /********************************************************************************/
        public int GetOrderID(int UserID)
        {
            try
            {
                string OrderID;
                //string sSQL = "SELECT ORDER_ID FROM TBWC_ORDER WHERE USER_ID = " + UserID;
                //oHelper.SQLString = sSQL;
                //OrderID = oHelper.GetValue("ORDER_ID");
                OrderID = (string)objOrderDB.GetGenericDataDB(UserID.ToString(), "Get_OrderID_For_Quote_NEW", OrderDB.ReturnType.RTString);

                if (OrderID == "" && OrderID == null)
                {
                    return 0;
                }
                else
                {
                    //return oHelper.CI(OrderID);
                    return objHelperService.CI(OrderID);
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
        /// This is used to get the Order ID based on User ID and Order Status
        /// </summary>
        /// <param name="UserID">int</param>
        /// <param name="OrderStatus">int</param>
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
        ///     Order oOrder = new Order();
        ///     int UserID;
        ///     int OrderStatus;
        ///     int Id;
        ///     ...
        ///     Id = oOrder.GetOrderID(UserID, OrderStatus);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE ORDER ID FOR PENDING ORDER ITEMS ***/
        /********************************************************************************/
        public int GetPendingOrderItems(int UserID, int OrderStatus, int OrderID)
        {
            try
            {
                string Order_ID;
                //string sSQL = "SELECT ORDER_ID FROM TBWC_ORDER WHERE USER_ID = " + UserID + " AND ORDER_STATUS = " + OrderStatus + " AND ORDER_ID = " + OrderID;
                //oHelper.SQLString = sSQL;
                //Order_ID = oHelper.GetValue("ORDER_ID");
                Order_ID = (string)objOrderDB.GetGenericDataDB(UserID.ToString(), OrderStatus.ToString(), OrderID.ToString(), "Get_PendingOrder_Items", OrderDB.ReturnType.RTString);
                if (Order_ID == "" && Order_ID == null)
                {
                    return 0;
                }
                else
                {
                    //  return oHelper.CI(Order_ID);
                    return objHelperService.CI(Order_ID);
                }
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
        /*** PURPOSE      : TO RETRIVE THE ORDER ID  ***/
        /********************************************************************************/
        public int GetOrderID(int UserID, int OrderStatus)
        {
            try
            {
                string OrderID;
                //string sSQL = "SELECT ORDER_ID FROM TBWC_ORDER WHERE USER_ID = " + UserID + " AND ORDER_STATUS = " + OrderStatus;
                //oHelper.SQLString = sSQL;
                //OrderID = oHelper.GetValue("ORDER_ID");
                OrderID = (string)objOrderDB.GetGenericDataDB("", UserID.ToString(), OrderStatus.ToString(), "Get_OrderID", OrderDB.ReturnType.RTString);

                if (OrderID == "" && OrderID == null)
                {
                    return 0;
                }
                else
                {
                    //return oHelper.CI(OrderID);
                    return objHelperService.CI(OrderID);
                }
            }
            catch (Exception e)
            {
                // oErrHand.ErrorMsg = e;
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                //oErrHand.CreateLog();
                return -1;
            }

        }

        public int GetOrderItemNumber(int Ordid, int Pid, int UserID)
        {
            try
            {
                string ItemNumber;
                ItemNumber = (string)objOrderDB.GetGenericDataDB("", Ordid.ToString(), Pid.ToString(), UserID.ToString(), "GET_ORDER_ITEM_NUMBER", OrderDB.ReturnType.RTString);

                if (ItemNumber == "" && ItemNumber == null)
                {
                    return 0;
                }
                else
                {
                    //return oHelper.CI(OrderID);
                    return objHelperService.CI(ItemNumber);
                }
            }
            catch (Exception e)
            {
                // oErrHand.ErrorMsg = e;
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                //oErrHand.CreateLog();
                return -1;
            }
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Price Value for the Order
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oOrder.GetOrderPriceValues(OrderID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE ORDER PRICE DETAILS  ***/
        /********************************************************************************/
        public DataSet GetOrderPriceValues(int OrderID)
        {
            try
            {
                //string sSQL = " SELECT PRODUCT_TOTAL_PRICE,TAX_AMOUNT,SHIP_COST,TOTAL_AMOUNT FROM TBWC_ORDER WHERE ORDER_ID =" + OrderID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objOrderDB.GetGenericDataDB(OrderID.ToString(), "Get_OrderPrice_Values", OrderDB.ReturnType.RTDataSet);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE SUM DETAILS OF ORDERED ITEMS  ***/
        /********************************************************************************/
        public DataSet GetOrderItemDetailSum(int OrderID)
        {
            try
            {

                return (DataSet)objOrderDB.GetGenericDataDB(OrderID.ToString(), "GET_ORDER_ITEM_DETAIL_SUM", OrderDB.ReturnType.RTDataSet);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }


        public DataSet GetOrderItemDetailSum_ExpressCheckout(int OrderID,string SessionID)
        {
            try
            {
              
                return (DataSet)objOrderDB.GetGenericDataDB("",OrderID.ToString(),SessionID, "GET_ORDER_ITEM_DETAIL_SUM_EXPRESS_CHECKOUT", OrderDB.ReturnType.RTDataSet);
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
        /// This is used to get all the Orders
        /// </summary>
        /// <param name="UserID">int</param>
        /// <param name="sStatus">string</param>
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
        ///     Order oOrder = new Order();
        ///     int UserID;
        ///     string sStatus;
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oOrder.GetOrders(UserID, sStatus);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ALL THE ORDER DETAILS  ***/
        /********************************************************************************/
        public DataTable GetOrders(int UserID, string sStatus)
        {
            try
            {
                string sStatusType = "";
                string company_id = HttpContext.Current.Session["COMPANY_ID"].ToString();
                if (sStatus.Equals("HISTORY"))
                {
                    sStatusType = "2,3,4,5,6,7";
                }
                if (sStatus.Equals("STATUS"))
                {
                    sStatusType = "2,3,6,7";
                }
                if (sStatus.Equals("COMPANYORDS"))
                {
                    sStatusType = "2,3,4,5,6,7";
                }
                //string sSQL = " SELECT TBP.PO_RELEASE AS ORDER_ID,TBO.ORDER_ID AS REFID,";
                //sSQL = sSQL + " TBO.MODIFIED_DATE AS ORDERDATE,TBO.ORDER_STATUS,";
                //sSQL = sSQL + " TBO.TOTAL_AMOUNT,TBO.SHIP_COMPANY,TBO.TRACKING_NO  FROM TBWC_ORDER TBO,TBWC_PAYMENT TBP";
                //sSQL = sSQL + " WHERE TBO.ORDER_ID=TBP.ORDER_ID AND " + (sStatus != "COMPANYORDS" ? "TBO.[USER_ID]=" + UserID : "TBO.COMPANY_ID = " + Session["COMPANY_ID"]);
                //sSQL = sSQL + " AND TBO.ORDER_STATUS IN(" + sStatusType + ") ORDER BY ORDERDATE DESC";

                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataTable();
                return (DataTable)objOrderDB.GetGenericDataDB("", UserID.ToString(), company_id, sStatusType, sStatus, "Get_Order_PAYMENT", OrderDB.ReturnType.RTTable);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PENDING ORDER DETAILS  ***/
        /********************************************************************************/
        public DataTable PendingOrders()
        {
            DataTable retvalue = null;
            string company_id = HttpContext.Current.Session["COMPANY_ID"].ToString();
            try
            {
                ////string sSQL = string.Format("select order_id [Reference], tcb.Contact, tbo.MODIFIED_DATE [Date], (select COUNT(*) from TBWC_ORDER_ITEM where ORDER_ID = tbo.ORDER_ID) Items, tbo.TOTAL_AMOUNT [Total Amount] from TBWC_ORDER tbo, TBWC_COMPANY_BUYERS tcb where tcb.user_id = tbo.MODIFIED_USER and ORDER_STATUS = 1 and tbo.company_id={0}", Convert.ToInt32(Session["COMPANY_ID"]));
                //string sSQL = string.Format("SELECT ORDER_ID [ORDER], TCB.CONTACT, TBO.CREATED_DATE,(SELECT PO_RELEASE FROM TBWC_PAYMENT WHERE ORDER_ID = TBO.ORDER_ID) as [Cust.Order No], TBO.TOTAL_AMOUNT [TOTAL AMOUNT] FROM TBWC_ORDER TBO, TBWC_COMPANY_BUYERS TCB WHERE TCB.USER_ID = TBO.MODIFIED_USER AND ORDER_STATUS = 11 AND TBO.COMPANY_ID={0} Order by TBO.CREATED_DATE DESC", Convert.ToInt32(Session["COMPANY_ID"]));
                //oHelper.SQLString = sSQL;
                //retvalue = oHelper.GetDataTable();
                retvalue = (DataTable)objOrderDB.GetGenericDataDB(company_id, "PendingOrders", OrderDB.ReturnType.RTTable);
            }
            catch (Exception ex)
            {
                //oErrHand.ErrorMsg = ex;
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PRODUCT DETAILS OF PENDING ORDERS  ***/
        /********************************************************************************/
        public DataTable GetPendingOrderProducts()
        {
            DataTable retvalue = null;
            int order_id = Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"].ToString());

            try
            {
                //string sSQL = string.Format("select * from tbwc_order_item where order_id in (select order_id from tbwc_order where order_id = {0})", Convert.ToInt32(Session["ORDER_ID"]));
                //oHelper.SQLString = sSQL;
                //retvalue = oHelper.GetDataTable();
                retvalue = (DataTable)objOrderDB.GetGenericDataDB(order_id.ToString(), "Get_PendingOrder_Products", OrderDB.ReturnType.RTTable);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public DataTable GetOrderHistory()
        //{
        //    DataTable retvalue = null;

        //    try
        //    {
        //        //string sSQL = string.Format("SELECT	TBCB.CONTACT [User], TBO.CREATED_DATE [Order Date],  TBO.ORDER_ID OrderID,(SELECT PO_RELEASE FROM TBWC_PAYMENT WHERE ORDER_ID = TBO.ORDER_ID) as [Cust.Order No], TBO.InvoiceNo as [Invoice No], CASE TBO.ORDER_STATUS  WHEN 1 THEN 'Open' WHEN 2 THEN 'Payment' WHEN 3 THEN 'Shipped' WHEN 4 THEN 'Completed' WHEN 5 THEN 'Canceled' WHEN 6 THEN 'Order Placed' WHEN 7 THEN 'Manual Process' WHEN 8 THEN 'Quote Placed' WHEN 9 THEN 'Ready to Verify' WHEN 10 THEN 'Custom Manual Process' WHEN 11 THEN 'Pending'END [Order Status], TBO.TRACKING_NO [Shipping Track & Trace], 'View Order' as [Submitted Order] FROM TBWC_ORDER AS TBO INNER JOIN TBWC_COMPANY_BUYERS AS TBCB ON TBO.COMPANY_ID = TBCB.COMPANY_ID");
        //        //string sSQL = string.Format("SELECT	TBCB.CONTACT [User], TBO.CREATED_DATE [Order Date],  TBO.ORDER_ID OrderID,(SELECT PO_RELEASE FROM TBWC_PAYMENT WHERE ORDER_ID = TBO.ORDER_ID) as [Cust.Order No],TBO.InvoiceNo as [Invoice No], CASE TBO.ORDER_STATUS  WHEN 1 THEN 'Open' WHEN 2 THEN 'Payment' WHEN 3 THEN 'Shipped'WHEN 4 THEN 'Completed' WHEN 5 THEN 'Canceled' WHEN 6 THEN 'Order Placed' WHEN 7 THEN 'Manual Process'WHEN 8 THEN 'Quote Placed' WHEN 9 THEN 'Ready to Verify' WHEN 10 THEN 'Custom Manual Process' WHEN 11 THEN 'Pending'END [Order Status], TBO.TRACKING_NO [Shipping Track & Trace], 'View Order' as [Submitted Order]FROM TBWC_ORDER AS TBO INNER JOIN TBWC_COMPANY_BUYERS AS TBCB ON TBO.USER_ID = TBCB.USER_ID ORDER BY TBO.CREATED_DATE DESC");
        //        //string sSQL = string.Format("SELECT	TBCB.CONTACT [User], TBO.CREATED_DATE [Order Date],TBO.MODIFIED_DATE [Modified Date], TBO.ORDER_ID OrderID,TBP.PO_RELEASE  as [Cust.Order No], TBO.InvoiceNo as [Invoice No], CASE TBO.ORDER_STATUS  WHEN 1 THEN 'Open' WHEN 2 THEN 'Payment' WHEN 3 THEN 'Shipped'WHEN 4 THEN 'Completed' WHEN 5 THEN 'Canceled' WHEN 6 THEN 'Order Placed' WHEN 7 THEN 'Manual Process'WHEN 8 THEN 'Quote Placed' WHEN 9 THEN 'Ready to Verify' WHEN 10 THEN 'Custom Manual Process' WHEN 11 THEN 'Pending'END [Order Status],TBO.TRACKING_NO [Shipping Track & Trace], 'View Order' as [Submitted Order]FROM TBWC_ORDER AS TBO INNER JOIN TBWC_COMPANY_BUYERS AS TBCB ON TBO.USER_ID = TBCB.USER_ID INNER JOIN TBWC_PAYMENT AS TBP ON TBP.ORDER_ID = TBO.ORDER_ID ORDER BY TBO.CREATED_DATE DESC");

        //        //string sSQL = string.Format("EXEC GetOrderHistory");
        //        //oHelper.SQLString = sSQL;
        //        //retvalue = oHelper.GetDataTable();
        //        string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

        //        string sSQL = " Exec STP_TBWC_PICK_GetOrderHistory " + websiteid;

        //        return objHelperDB.GetDataTableDB(sSQL);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return retvalue;
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ORDER HISTORY  ***/
        /********************************************************************************/
        public DataTable GetOrderHistory()
        {
            DataTable retvalue = null;

            try
            {
                //string sSQL = string.Format("SELECT	TBCB.CONTACT [User], TBO.CREATED_DATE [Order Date],  TBO.ORDER_ID OrderID,(SELECT PO_RELEASE FROM TBWC_PAYMENT WHERE ORDER_ID = TBO.ORDER_ID) as [Cust.Order No], TBO.InvoiceNo as [Invoice No], CASE TBO.ORDER_STATUS  WHEN 1 THEN 'Open' WHEN 2 THEN 'Payment' WHEN 3 THEN 'Shipped' WHEN 4 THEN 'Completed' WHEN 5 THEN 'Canceled' WHEN 6 THEN 'Order Placed' WHEN 7 THEN 'Manual Process' WHEN 8 THEN 'Quote Placed' WHEN 9 THEN 'Ready to Verify' WHEN 10 THEN 'Custom Manual Process' WHEN 11 THEN 'Pending'END [Order Status], TBO.TRACKING_NO [Shipping Track & Trace], 'View Order' as [Submitted Order] FROM TBWC_ORDER AS TBO INNER JOIN TBWC_COMPANY_BUYERS AS TBCB ON TBO.COMPANY_ID = TBCB.COMPANY_ID");
                //string sSQL = string.Format("SELECT	TBCB.CONTACT [User], TBO.CREATED_DATE [Order Date],  TBO.ORDER_ID OrderID,(SELECT PO_RELEASE FROM TBWC_PAYMENT WHERE ORDER_ID = TBO.ORDER_ID) as [Cust.Order No],TBO.InvoiceNo as [Invoice No], CASE TBO.ORDER_STATUS  WHEN 1 THEN 'Open' WHEN 2 THEN 'Payment' WHEN 3 THEN 'Shipped'WHEN 4 THEN 'Completed' WHEN 5 THEN 'Canceled' WHEN 6 THEN 'Order Placed' WHEN 7 THEN 'Manual Process'WHEN 8 THEN 'Quote Placed' WHEN 9 THEN 'Ready to Verify' WHEN 10 THEN 'Custom Manual Process' WHEN 11 THEN 'Pending'END [Order Status], TBO.TRACKING_NO [Shipping Track & Trace], 'View Order' as [Submitted Order]FROM TBWC_ORDER AS TBO INNER JOIN TBWC_COMPANY_BUYERS AS TBCB ON TBO.USER_ID = TBCB.USER_ID ORDER BY TBO.CREATED_DATE DESC");
                //string sSQL = string.Format("SELECT	TBCB.CONTACT [User], TBO.CREATED_DATE [Order Date],TBO.MODIFIED_DATE [Modified Date], TBO.ORDER_ID OrderID,TBP.PO_RELEASE  as [Cust.Order No], TBO.InvoiceNo as [Invoice No], CASE TBO.ORDER_STATUS  WHEN 1 THEN 'Open' WHEN 2 THEN 'Payment' WHEN 3 THEN 'Shipped'WHEN 4 THEN 'Completed' WHEN 5 THEN 'Canceled' WHEN 6 THEN 'Order Placed' WHEN 7 THEN 'Manual Process'WHEN 8 THEN 'Quote Placed' WHEN 9 THEN 'Ready to Verify' WHEN 10 THEN 'Custom Manual Process' WHEN 11 THEN 'Pending'END [Order Status],TBO.TRACKING_NO [Shipping Track & Trace], 'View Order' as [Submitted Order]FROM TBWC_ORDER AS TBO INNER JOIN TBWC_COMPANY_BUYERS AS TBCB ON TBO.USER_ID = TBCB.USER_ID INNER JOIN TBWC_PAYMENT AS TBP ON TBP.ORDER_ID = TBO.ORDER_ID ORDER BY TBO.CREATED_DATE DESC");

                //string sSQL = string.Format("EXEC GetOrderHistory");
                //oHelper.SQLString = sSQL;
                //retvalue = oHelper.GetDataTable();
                int User_id=0;
                if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null   && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString() == "Retailer")
                {
                    if( Convert.ToInt32 ( HttpContext.Current.Session["USER_ROLE"])==2)
                        User_id = Convert.ToInt32(HttpContext.Current.Session["USER_ID"]);
                }

                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

                string sSQL = " Exec STP_TBWC_PICK_GetOrderHistory " + websiteid + "," + User_id.ToString() ;

                return objHelperDB.GetDataTableDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public DataTable GetFilteredOrderHistory(string pInvoice, string pFromDate, string pToDate, string pUsers, int Companyid)
        //{
        //    DataTable retvalue = null;

        //    try
        //    {
        //        //string sSQL = string.Format("EXEC GetOrderHistory_Search '" + pInvoice + "','" + pUsers + "','" + pFromDate + "','" + pToDate + "'," + Companyid + " ");
        //        //oHelper.SQLString = sSQL;
        //        //retvalue = oHelper.GetDataTable();
        //        string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

        //        string sSQL = string.Format("EXEC STP_TBWC_PICK_GetOrderHistory_Search '" + pInvoice + "','" + pUsers + "','" + pFromDate + "','" + pToDate + "'," + Companyid + "," + websiteid + "");
        //        retvalue = objHelperDB.GetDataTableDB(sSQL);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return retvalue;
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE FILTERD ORDER HISTORY  ***/
        /********************************************************************************/
        public DataTable GetFilteredOrderHistory(string pInvoice, string pFromDate, string pToDate, string pUsers, int Companyid)
        {
            DataTable retvalue = null;

            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                int User_id = 0;
                if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString() == "Retailer")
                {
                    if (Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"]) == 2)
                    User_id = Convert.ToInt32(HttpContext.Current.Session["USER_ID"]);
                }
                //string sSQL = string.Format("EXEC GetOrderHistory_Search '" + pInvoice + "','" + pUsers + "','" + pFromDate + "','" + pToDate + "'," + Companyid + " ");
                //oHelper.SQLString = sSQL;
                //retvalue = oHelper.GetDataTable();
                string sSQL = string.Format("EXEC STP_TBWC_PICK_GetOrderHistory_Search '" + pInvoice + "','" + pUsers + "','" + pFromDate + "','" + pToDate + "'," + Companyid + "," + websiteid + "," + User_id.ToString() );
                objErrorHandler.CreateLog(sSQL);
                retvalue = objHelperDB.GetDataTableDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE FILTERD Payment HISTORY  ***/
        /********************************************************************************/
        public DataTable GetPaymentHistory(string pInvoice, string pFromDate, string pToDate, string pUsers, int Companyid)
        {
            DataTable retvalue = null;

            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                int User_id = 0;
                if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString() == "Retailer")
                {
                    if (Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"]) == 2)
                        User_id = Convert.ToInt32(HttpContext.Current.Session["USER_ID"]);
                }
                //string sSQL = string.Format("EXEC GetOrderHistory_Search '" + pInvoice + "','" + pUsers + "','" + pFromDate + "','" + pToDate + "'," + Companyid + " ");
                //oHelper.SQLString = sSQL;
                //retvalue = oHelper.GetDataTable();
                string sSQL = string.Format("EXEC STP_TBWC_PICK_GetPaymentReceivedDetails '" + pInvoice + "','" + pUsers + "','" + pFromDate + "','" + pToDate + "'," + Companyid + "," + websiteid + "," + User_id.ToString() );
                retvalue = objHelperDB.GetDataTableDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE FILTERD Payment HISTORY  ***/
        /********************************************************************************/
        public DataTable GetFailedHistory(string pInvoice, string pFromDate, string pToDate, string pUsers, int Companyid)
        {
            DataTable retvalue = null;

            try
            {
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                int User_id = 0;
                if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString() == "Retailer")
                {
                    if (Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"]) == 2)
                        User_id = Convert.ToInt32(HttpContext.Current.Session["USER_ID"]);
                }
                //string sSQL = string.Format("EXEC GetOrderHistory_Search '" + pInvoice + "','" + pUsers + "','" + pFromDate + "','" + pToDate + "'," + Companyid + " ");
                //oHelper.SQLString = sSQL;
                //retvalue = oHelper.GetDataTable();
                string sSQL = string.Format("EXEC STP_TBWC_PICK_GetPaymentFailedDetails '" + pInvoice + "','" + pUsers + "','" + pFromDate + "','" + pToDate + "'," + Companyid + "," + websiteid + "," + User_id.ToString());
                retvalue = objHelperDB.GetDataTableDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DETAILS FOR INVOICE SIGNAL NOTIFICATION  ***/
        /********************************************************************************/
        public int SentSignalInvoiceNotification(string InvoiceNo)
        {
            int retVal = 0;
            DataSet dsOD = new DataSet();
            try
            {
                //string sSQL = string.Format("EXEC STP_REQINVOICE '{0}'", InvoiceNo);
                //oHelper.SQLString = sSQL;
                //dsOD = oHelper.GetDataSet("inv");
                string sSQL = "Exec STP_TBWC_PICKREQINVOICE '" + InvoiceNo + "'";
                dsOD = objHelperDB.GetDataSetDB(sSQL);
                if (dsOD != null)
                {
                    foreach (DataRow drOD in dsOD.Tables[0].Rows)
                    {
                        retVal = objHelperService.CI(drOD["output"]);
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return retVal;
            }
            return retVal;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DETAILS FOR ORDER SIGNAL NOTIFICATION  ***/
        /********************************************************************************/
        public int SentSignalOrderNotification(string InvoiceNo)
        {
            int retVal = 0;
            DataSet dsOD = new DataSet();
            try
            {
                //string sSQL = string.Format("EXEC STP_REQORDER '{0}'", InvoiceNo);
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.ExecuteSQLQuery();
                string sSQL = "Exec STP_TBWC_PICK_REQORDER '" + InvoiceNo + "'";
                dsOD = objHelperDB.GetDataSetDB(sSQL);
                if (dsOD != null)
                {
                    foreach (DataRow drOD in dsOD.Tables[0].Rows)
                    {
                        retVal = objHelperService.CI(drOD[0]);
                    }
                }

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return retVal;
            }
            return retVal;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : SIGNAL NOTIFICATION  ***/
        /********************************************************************************/
        public int SentSignal(string payment_id, string order_id,string Signal_number)
        {
            int retVal = 0;
            DataSet dsOD = new DataSet();
            try
            {
               bool checkinsert=true;
                if (Signal_number=="150")
                {
                DataTable dt= Get_Order_Send_signal(order_id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    checkinsert = false;
                    retVal = 1;
                    objErrorHandler.CreatePayLog_Final("Already Inserted signal for orderid" + order_id);
                }
                }
                if (checkinsert == true)
                {

                    string sSQL = "Exec STP_TBWC_SEND_SIGNAL '" + payment_id + "','" + order_id + "','" + Signal_number + "'";
                    objErrorHandler.CreatePayLog(sSQL);
                    dsOD = objHelperDB.GetDataSetDB(sSQL);
                    if (dsOD != null && dsOD.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drOD in dsOD.Tables[0].Rows)
                        {
                            retVal = objHelperService.CI(drOD[0]);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return retVal;
            }
            return retVal;
        }
        public DataTable Get_Order_Send_signal(string OrderID)
        {
            DataTable retvalue = null;
            try
            {
                string sSQL = string.Format("EXEC Get_Order_Send_signal '" + OrderID + "'");
                retvalue = objHelperDB.GetDataTableDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Order Details
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oOrder.GetOrderDetails(OrderID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ORDER DETAILS   ***/
        /********************************************************************************/
        public DataSet GetOrderDetails(int OrderID)
        {
            try
            {
                //string sSQL;
                //sSQL = "SELECT (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = P.PRODUCT_ID) AS CATALOG_ITEM_NO,";
                //sSQL = sSQL + "OI.QTY,OI.PRICE_EXT_APPLIED,OI.PRICE_INC_APPLIED,OI.PRICE_CALC_MTHD,(SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =41 AND PRODUCT_ID = P.PRODUCT_ID) AS DESCRIPTION FROM TB_PRODUCT P,";
                //sSQL = sSQL + "TBWC_ORDER_ITEM OI WHERE P.PRODUCT_ID=OI.PRODUCT_ID AND OI.ORDER_ID=" + OrderID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet("OrderDetails");
                return (DataSet)objOrderDB.GetGenericDataDB(OrderID.ToString(), "Get_Order_Details_Attribute", OrderDB.ReturnType.RTDataSet);
            }
            catch (Exception e)
            {
                // oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Order Status Details
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     string Orderstatus;
        ///     ...
        ///     Orderstatus = oOrder.GetOrderStatus(OrderID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ORDER STATUS  ***/
        /********************************************************************************/
        public string GetOrderStatus(int OrderID)
        {
            string sSQL;
            int StatusValue = 0;
            string sOrderStatus = "";
            DataSet dsOS = new DataSet();

            try
            {
                //sSQL = "SELECT ORDER_STATUS FROM TBWC_ORDER WHERE ORDER_ID=" + OrderID;
                //oHelper.SQLString = sSQL;
                //dsOS = oHelper.GetDataSet("OrderStatus");
                dsOS = (DataSet)objOrderDB.GetGenericDataDB(OrderID.ToString(), "GetOrderStatus", OrderDB.ReturnType.RTDataSet);
                if (dsOS != null)
                {
                    dsOS.Tables[0].TableName = "OrderStatus";
                    foreach (DataRow oDR in dsOS.Tables["OrderStatus"].Rows)
                    {
                        StatusValue = (int)oDR["ORDER_STATUS"];
                    }
                }
                if (StatusValue > 0)
                {
                    sOrderStatus = Enum.GetName(typeof(OrderStatus), StatusValue);
                }
                if (sOrderStatus == null && StatusValue == 99)
                {
                    sOrderStatus = "99";
                }

            }
            catch (Exception e)
            {
                //oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
            return sOrderStatus;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Current Product Total Cost
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     decimal retVal;
        ///     ...
        ///     retVal = oOrder.GetCurrentProductTotalCost(OrderID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/
        public DataTable Get_Check_Request_Response(string mertransid)
        {
            DataTable retvalue = null;
            try
            {
                string sSQL = string.Format("EXEC Get_Check_Request_Response '" + mertransid + "'");
                retvalue = objHelperDB.GetDataTableDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }
        
        public int Get_Error_Mail_Log_OrderStatus(int OrderID)
        {
            int StatusValue = 0;
            DataSet dsOS = new DataSet();

            try
            {
                dsOS = (DataSet)objOrderDB.GetGenericDataDB(OrderID.ToString(), "GetOrderStatus", OrderDB.ReturnType.RTDataSet);
                if (dsOS != null)
                {
                    dsOS.Tables[0].TableName = "OrderStatus";
                    foreach (DataRow oDR in dsOS.Tables["OrderStatus"].Rows)
                    {
                        StatusValue = (int)oDR["ORDER_STATUS"];
                    }
                }
                //if (StatusValue > 0)
                //{
                //    sOrderStatus = Enum.GetName(typeof(OrderStatus), StatusValue);
                //}
            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
            return StatusValue;
        }
        public int updateorderstatus_99_1(int OrderID)
        {

            int retVal = 0;
            try
            {
                //string sSQL = "UPDATE TBWC_ORDER SET ORDER_STATUS = " + OrderStatus + " WHERE ORDER_ID = " + OrderID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.ExecuteSQLQuery();
                string sSQL = "Exec  STP_TBWC_RENEW_UPDATE_ORDER_STATUS_99 ";
                sSQL = sSQL + "" + OrderID + "";
                retVal = objHelperDB.ExecuteSQLQueryDB(sSQL);
                objErrorHandler.CreateLog(sSQL);

            }
            catch (Exception e)
            {
                // oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return retVal;
            }
            return retVal;

        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE TOTAL COST FOR CURRENT PRODUCT  ***/
        /********************************************************************************/
        public decimal GetCurrentProductTotalCost(int OrderID)
        {
            decimal retVal = 0;
            string tempstr = "";
            try
            {
                //string sSQL = "SELECT PRODUCT_TOTAL_PRICE FROM TBWC_ORDER WHERE ORDER_ID =" + OrderID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.CDEC(oHelper.GetValue("PRODUCT_TOTAL_PRICE"));
                tempstr = (string)objOrderDB.GetGenericDataDB(OrderID.ToString(), "Get_Current_Product_TotalCost", OrderDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    retVal = objHelperService.CDEC(tempstr);

                if (retVal == -1)
                {
                    retVal = 0;
                }
            }
            catch (Exception e)
            {
                // oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retVal = -1;
            }
            return retVal;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to cancel the order
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     int CancelOrd;
        ///     ...
        ///     CancelOrd = oOrder.CancelOrder(OrderID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CANCEL THE ORDER  ***/
        /********************************************************************************/
        public int CancelOrder(int OrderID)
        {
            try
            {
                //string sSQL = " UPDATE TBWC_ORDER";
                //sSQL = sSQL + " SET ORDER_STATUS=" + (int)OrderStatus.CANCELED;
                //sSQL = sSQL + " WHERE ORDER_ID=" + OrderID;
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string sSQL = "Exec STP_TBWC_RENEW_CANCEL_ORDER ";
                sSQL = sSQL + "" + OrderID + "";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {
                //oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        public int UpdateOrderItemQtyPopUp(int Order_Id, int Qty, int Pid)
        {
            try
            {
                string sSQL = "Exec STP_TBWC_RENEW_ORDER_ITEM_UPDATE ";
                sSQL = sSQL + "" + Order_Id + "," + Qty + "," + Pid;
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {
                //oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        #region "Order Item Functions.."

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to insert the items into the Order Table
        /// </summary>
        /// <param name="oItem">OrderItemInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.oItemInFo oItem;
        ///     int addItem;
        ///     ...
        ///     addItem = oOrder.AddOrderItem(oItem);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO ADD THE ITEMS INTO THE ORDER TABLE  ***/
        /********************************************************************************/
        public int AddOrderItem(OrderItemInfo oItem)
        {
            try
            {
                //string sSQL;
                //sSQL = "INSERT INTO TBWC_ORDER_ITEM(ORDER_ID,PRODUCT_ID,QTY,PRICE_EXT_APPLIED,PRICE_CALC_MTHD,CREATED_USER,MODIFIED_USER) ";
                //sSQL = sSQL + "VALUES(" + oItem.OrderID + "," + oItem.ProductID + "," + oItem.Quantity + "," + oItem.PriceApplied + ",'" + oItem.PriceCalcMethod + "',";
                //sSQL = sSQL + oItem.UserID + "," + oItem.UserID + ")";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
               
                if (oItem.UserID > 0)
                {
                    string sSQL = "Exec STP_TBWC_POP_ADD_ORDER_ITEM_WAGNER ";
                    sSQL = sSQL + "" + oItem.OrderID + "," + oItem.ProductID + "," + oItem.Quantity + "," + oItem.PriceApplied + ",'" + oItem.PriceCalcMethod + "',";
                    sSQL = sSQL + oItem.Tax_Amount + "," + oItem.Ship_Cost + ",";
                    sSQL = sSQL + oItem.UserID + "," + oItem.UserID + " ,'" + oItem.PRD_SESSION_ID + "'";
                    return objHelperDB.ExecuteSQLQueryDB(sSQL);
                }
                else
                    return -1;

            }
            catch (Exception e)
            {
                //oErrHand.ErrorMsg = e;
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO ADD THE ITEMS INTO THE ORDER TABLE TEMPLATE  ***/
        /********************************************************************************/
        public int AddOrderItemTemplate(OrderTemplateInfo oItem)
        {
            try
            {
                int websiteid = Convert.ToInt32(   ConfigurationManager.AppSettings["WEBSITEID"].ToString());
                if (oItem.UserID > 0)
                {
                    string sSQL = "Exec STP_TBWC_POP_ADD_ORDER_TEMPLATE ";
                    sSQL = sSQL + "" + oItem.UserID  + "," + oItem.ProductID + "," + oItem.Quantity +","+ websiteid ;
                    return objHelperDB.ExecuteSQLQueryDB(sSQL);
                }
                else
                    return -1;

            }
            catch (Exception e)
            {
                //oErrHand.ErrorMsg = e;
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO ADD THE ORDER CLRIFICATION ITEM DETAILS   ***/
        /********************************************************************************/
        public int AddOrder_ClarificationItem(Order_Calrification_ItemInfo oItem)
        {
            try
            {
               
                string websiteid = ConfigurationManager.AppSettings["WEBSITEID"].ToString();

                if (oItem.UserID > 0)
                {
                    string sSQL = "Exec STP_TBWC_POP_ORDER_CLARIFICATION_WAG ";
                    sSQL = sSQL + "" + oItem.OrderID + ",'" + oItem.ProductDesc + "'," + oItem.Quantity + "," + oItem.UserID + ",'" + oItem.Clarification_Type + "','" + oItem.PROD_SESSION_ID + "'," + websiteid + "";
                    return objHelperDB.ExecuteSQLQueryDB(sSQL);
                }
                else
                    return -1;

            }
            catch (Exception e)
            {
                //oErrHand.ErrorMsg = e;
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the order Items using order ID
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <returns>DatSet</returns>
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
        ///     Order oOrder = new Order();
        ///     int OrderId;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oOrder.GetOrderItems(OrderId);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ORDER ITEM DETAILS  ***/
        /********************************************************************************/
        public DataSet GetOrderItems(int OrderID)
        {
            try
            {
                //string sSQL = " SELECT (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = TP.PRODUCT_ID) AS CATALOG_ITEM_NO,TOI.ORDER_ID,PF.FAMILY_ID,TOI.PRODUCT_ID,TOI.QTY,TOI.PRICE_APPLIED,TOI.PRICE_CALC_MTHD,";
                //sSQL = sSQL + " TI.QTY_AVAIL ,TI.MIN_ORD_QTY,"; //TI.LEAD_TIME,";//TI.IS_ON_SALE,TI.DISCOUNT_PERCENT,TI.LIST_PRICE_VALID_TILL,";
                //sSQL = sSQL + " PRODUCT_STATUS";
                //sSQL = sSQL + " FROM TBWC_ORDER_ITEM TOI,TBWC_INVENTORY TI,TB_PRODUCT TP,TB_PROD_FAMILY PF";
                //sSQL = sSQL + " WHERE TOI.ORDER_ ID =" + OrderID;
                //sSQL = sSQL + " AND TOI.PRODUCT_ID = TI.PRODUCT_ID AND TOI.PRODUCT_ID = TP.PRODUCT_ID AND TOI.PRODUCT_ID = PF.PRODUCT_ID AND TP.PRODUCT_ID=PF.PRODUCT_ID";

                //string sSQL = " SELECT (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = TP.PRODUCT_ID) AS CATALOG_ITEM_NO,TOI.ORDER_ID,TOI.PRODUCT_ID,TOI.QTY,TOI.PRICE_INC_APPLIED,TOI.PRICE_EXT_APPLIED,TOI.PRICE_CALC_MTHD,";
                //sSQL = sSQL + " TI.QTY_AVAIL ,TI.MIN_ORD_QTY,"; 
                //sSQL = sSQL + " PRODUCT_STATUS,(SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =(SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME='PROD_DSC') AND PRODUCT_ID = TP.PRODUCT_ID) AS DESCRIPTION";
                //sSQL = sSQL + " FROM TBWC_ORDER_ITEM TOI,TBWC_INVENTORY TI,TB_PRODUCT TP";
                //sSQL = sSQL + " WHERE TOI.ORDER_ID =" + OrderID;
                //sSQL = sSQL + " AND TOI.PRODUCT_ID = TI.PRODUCT_ID AND TOI.PRODUCT_ID = TP.PRODUCT_ID ORDER BY TOI.CREATED_DATE DESC";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objOrderDB.GetGenericDataDB(OrderID.ToString(), "Get_ORDER_ITEMS", OrderDB.ReturnType.RTDataSet);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        public DataTable GetPaymentDetails(string OrderID, string payment_id, string txn_id)
        {
            try
            {

                return (DataTable)objOrderDB.GetGenericDataDB("", payment_id, OrderID.ToString(), txn_id, "GET_Payment_REQUEST_RESPONSE", OrderDB.ReturnType.RTTable);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }


        public DataSet GetOrderItemsAmtDetails(int OrderID)
        {
            try
            {

                return (DataSet)objOrderDB.GetGenericDataDB(OrderID.ToString(), "Get_ORDER_ITEMS_AMT_DETAILS", OrderDB.ReturnType.RTDataSet);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE DISTINCT ORDER ITEM DETAILS ***/
        /********************************************************************************/
        public DataSet GetOrderItemsWithoutDuplicate(int OrderID, string excProductSids, string prd_session_id)
        {
            try
            {

                //return (DataSet)objOrderDB.GetGenericDataDB("", OrderID.ToString(), excProductSids,prd_session_id, "Get_ORDER_ITEMS_WITHOUT_DUPLICATE", OrderDB.ReturnType.RTDataSet);
              //  objErrorHandler.CreateLog("orderid:" + OrderID.ToString() + "excProductSids:" + excProductSids + "prd_session_id" + prd_session_id);
                return (DataSet)objOrderDB.GetGenericDataDB("", OrderID.ToString(), excProductSids, prd_session_id, "Get_ORDER_ITEMS_WITHOUT_DUPLICATE_WAGNER_EXP", OrderDB.ReturnType.RTDataSet);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        //public DataSet GetOrderItemsWithoutDuplicate_Dumuser(int OrderID, string excProductSids,string prd_session_id)
        //{
        //    try
        //    {

        //        return (DataSet)objOrderDB.GetGenericDataDB("", OrderID.ToString(), excProductSids, prd_session_id, "Get_ORDER_ITEMS_WITHOUT_DUPLICATE_DUMUSER", OrderDB.ReturnType.RTDataSet);

        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        return null;
        //    }
        //}
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ALL ORDER ITEM DETAILS ***/
        /********************************************************************************/
        public DataSet GetOrderItemsWithDuplicate(int OrderID, string excProductSids, string prd_session_id)
        {
            try
            {

                return (DataSet)objOrderDB.GetGenericDataDB("", OrderID.ToString(), excProductSids,prd_session_id, "Get_ORDER_ITEMS_WITH_DUPLICATE", OrderDB.ReturnType.RTDataSet);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }


        //public DataSet GetOrderItemsWithDuplicate_Dumuser(int OrderID, string excProductSids,string prd_session_id)
        //{
        //    try
        //    {

        //        return (DataSet)objOrderDB.GetGenericDataDB("", OrderID.ToString(), excProductSids,prd_session_id, "Get_ORDER_ITEMS_WITH_DUPLICATE_DUMUSER", OrderDB.ReturnType.RTDataSet);

        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        return null;
        //    }
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DETAILS OF ORDER CLARIFICATION ITEMS  ***/
        /********************************************************************************/
        public DataTable GetOrder_Clarification_Items(int OrderID, string Type, string session_id)
        {
            try
            {

                return (DataTable)objOrderDB.GetGenericDataDB("", OrderID.ToString(), Type,session_id, "Get_ORDER_CLARIFICATION_ITEMS", OrderDB.ReturnType.RTTable);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        public DataTable GetOrder_Clarification_Items_Checkout(int OrderID, string Type, string session_id)
        {
            try
            {

                return (DataTable)objOrderDB.GetGenericDataDB("", OrderID.ToString(), Type, session_id, "Get_ORDER_CLARIFICATION_ITEMS_CHECKOUT", OrderDB.ReturnType.RTTable);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        //public DataTable GetOrder_Clarification_Items_wag(int OrderID, string Type, string session_id)
        //{
        //    try
        //    {

        //        return (DataTable)objOrderDB.GetGenericDataDB("", OrderID.ToString(), Type, session_id, "Get_ORDER_CLARIFICATION_ITEMS_WAG", OrderDB.ReturnType.RTTable);

        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        return null;
        //    }
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ORDER ITEMS HAVING DUPLICATE PRODUCT ID ***/
        /********************************************************************************/
        public DataSet GetOrderItemsWithDuplicate_Prod_id(int OrderID, string excProductSids,string prd_session_id)
        {
            try
            {

                return (DataSet)objOrderDB.GetGenericDataDB("", OrderID.ToString(), excProductSids, prd_session_id,"Get_ORDER_ITEMS_WITH_DUPLICATE_PROD_ID", OrderDB.ReturnType.RTDataSet);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }


        //public DataSet GetOrderItemsWithDuplicate_Prod_id_Dumuser(int OrderID, string excProductSids,string prd_session_id)
        //{
        //    try
        //    {

        //        return (DataSet)objOrderDB.GetGenericDataDB("", OrderID.ToString(), excProductSids,prd_session_id, "Get_ORDER_ITEMS_WITH_DUPLICATE_PROD_ID_DUMUSER", OrderDB.ReturnType.RTDataSet);

        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        return null;
        //    }
        //}

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to count the Number of Items in Order
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     int retVal;
        ///     ...
        ///     retVal = oOrder.GetOrderItemCount(OrderID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ITMS COUNT IN ORDER   ***/
        /********************************************************************************/
        public int GetOrderItemCount(int OrderID)
        {
            string tempstr = "";


            try
            {

                //string sSQL = "SELECT COUNT(PRODUCT_ID) AS ITEMCOUNT FROM TBWC_ORDER_ITEM WHERE ORDER_ID = " + OrderID;
                //oHelper.SQLString = sSQL;
                //return oHelper.CI(oHelper.GetValue("ITEMCOUNT"));
                tempstr = (string)objOrderDB.GetGenericDataDB(OrderID.ToString(), "Get_OrderItem_Count", OrderDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    return objHelperService.CI(tempstr);
                else
                    return 0;
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        public int GetOrderItemCount_BL(int OrderID,string session_id)
        {
            string tempstr = "";


            try
            {

                //string sSQL = "SELECT COUNT(PRODUCT_ID) AS ITEMCOUNT FROM TBWC_ORDER_ITEM WHERE ORDER_ID = " + OrderID;
                //oHelper.SQLString = sSQL;
                //return oHelper.CI(oHelper.GetValue("ITEMCOUNT"));
                tempstr = (string)objOrderDB.GetGenericDataDB("", OrderID.ToString(), session_id.ToString(), "Get_OrderItem_Count_BL", OrderDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    return objHelperService.CI(tempstr);
                else
                    return 0;
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
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     decimal retVal;
        ///     ...
        ///     retVal = oOrder.GetProductTotalCost(OrderID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT TOTAL COST  ***/
        /********************************************************************************/
        public decimal GetProductTotalCost(int OrderID)
        {
            decimal retVal = 0;
            string tempstr = "";
            try
            {
                //string sSQL = "SELECT SUM(PRICE_EXT_APPLIED)AS TOTALAMOUNT FROM TBWC_ORDER_ITEM WHERE ORDER_ID =" + OrderID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.CDEC(oHelper.GetValue("TOTALAMOUNT"));
                tempstr = (string)objOrderDB.GetGenericDataDB(OrderID.ToString(), "Get_Product_TotalCost", OrderDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    retVal = objHelperService.CDEC(tempstr);

                if (retVal < 0) retVal = 0.00M;
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
        /// This is used to get the Order Items Quantity
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int ProductID;
        ///     int OrderID;
        ///     int retVal;
        ///     ...
        ///     retVal = oOrder.GetOrderItemQty(ProductID, OrderID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO GET ORDER ITEM QUANTITY  ***/
        /********************************************************************************/
        public int GetOrderItemQty(int ProductID, int OrderID, double order_item_id)
        {
            int retVal = 0;
            string tempstr = "";
            try
            {
                //string sSQL = "SELECT QTY FROM TBWC_ORDER_ITEM WHERE PRODUCT_ID = " + ProductID + " AND ORDER_ID =" + OrderID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.CI(oHelper.GetValue("QTY"));
                tempstr = (string)objOrderDB.GetGenericDataDB("", ProductID.ToString(), OrderID.ToString(), order_item_id.ToString(), "Get_Order_ItemQty", OrderDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    retVal = objHelperService.CI(tempstr);

            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retVal = -1;
            }
            return retVal;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO GET ORDER TEMPLATE ITEM   ***/
        /********************************************************************************/
        public DataSet  GetOrderTemplateItem(int userid )
        {
            DataSet tbl = null;            
            try
            {

                tbl = (DataSet)objOrderDB.GetGenericDataDB(userid.ToString(), "GET_ORDER_TEMPLATE_ITEM", OrderDB.ReturnType.RTDataSet );
                

            }
            catch (Exception e)
            {

                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
            return tbl;
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrItemInfo"></param>
        /// <returns></returns>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO GET APPROVED ORDER ITEM DETAILS  ***/
        /********************************************************************************/
        public DataSet GetApproveOrderItems(int OrderID)
        {
            try
            {
                //string sSQL = "SELECT TBO.ORDER_ID, TBO.SHIP_METHOD, TBO.SHIP_NOTES AS COMMENTS, TBP.PO_RELEASE FROM TBWC_ORDER TBO, ";
                //sSQL = sSQL + "TBWC_PAYMENT TBP ";
                //sSQL = sSQL + "WHERE TBO.ORDER_ID = TBP.ORDER_ID AND ";
                //sSQL = sSQL + "TBO.ORDER_ID = " + OrderID; ;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objOrderDB.GetGenericDataDB(OrderID.ToString(), "Get_ApproveOrder_Items", OrderDB.ReturnType.RTDataSet);

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
        /// This is used to Update the Order Items 
        /// </summary>
        /// <param name="OrItemInfo">OrderItemInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderItemInfo oOrdItemInfo;
        ///     int UpdateOrdItem;
        ///     ...
        ///     UpdateOrdItem = oOrder.UpdateOrderItem(oOrdItemInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public int UpdateOrderItem(OrderItemInfo OrItemInfo)
        //{
        //    try
        //    {

        //        //string sSQL = "UPDATE TBWC_ORDER_ITEM SET QTY=(" + OrItemInfo.Quantity + ") ,PRICE_EXT_APPLIED=" + OrItemInfo.PriceApplied;
        //        //sSQL = sSQL + " WHERE PRODUCT_ID=" + OrItemInfo.ProductID + " AND ORDER_ID =" + OrItemInfo.OrderID;
        //        //oHelper.SQLString = sSQL;
        //        //return oHelper.ExecuteSQLQuery();
        //        string sSQL = "Exec STP_TBWC_RENEW_ORDER_ITEM ";
        //        sSQL = sSQL + "" + OrItemInfo.Quantity + "," + OrItemInfo.PriceApplied + "," + OrItemInfo.ProductID + "," + OrItemInfo.OrderID + "";
        //        return objHelperDB.ExecuteSQLQueryDB(sSQL);
        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        return -1;
        //    }
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE ORDER ITEM DETAILS  ***/
        /********************************************************************************/
        public int UpdateOrderItem(OrderItemInfo OrItemInfo)
        {
            try
            {

                //string sSQL = "UPDATE TBWC_ORDER_ITEM SET QTY=(" + OrItemInfo.Quantity + ") ,PRICE_EXT_APPLIED=" + OrItemInfo.PriceApplied;
                //sSQL = sSQL + " WHERE PRODUCT_ID=" + OrItemInfo.ProductID + " AND ORDER_ID =" + OrItemInfo.OrderID;
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string sSQL = "Exec STP_TBWC_RENEW_ORDER_ITEM ";
                sSQL = sSQL + "" + OrItemInfo.Quantity + "," + OrItemInfo.PriceApplied + "," + OrItemInfo.ProductID + "," + OrItemInfo.OrderID + "";
                sSQL = sSQL + "," + OrItemInfo.Tax_Amount + "," + OrItemInfo.Ship_Cost + "," + OrItemInfo.ORDER_ITEM_ID;
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
        /// This is used to remove the Items from the "order item" table 
        /// </summary>
        /// <param name="ProductID">string</param>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     string ProductID;
        ///     int OrderId;
        ///     int remItem;
        ///     ...
        ///     remItem = oOrder.RemoveItem(ProductID, OrderId);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public int RemoveItem(string ProductID, int OrderID, int UserID)
        //{
        //    try
        //    {
        //        string sSQL;
        //        //if (ProductID == "AllProd")
        //        //{
        //        //    sSQL = "DELETE FROM TBWC_ORDER_ITEM WHERE ORDER_ID=" + OrderID + " AND MODIFIED_USER=" + UserID;
        //        //}
        //        //else
        //        //{
        //        //    sSQL = "DELETE FROM TBWC_ORDER_ITEM WHERE PRODUCT_ID IN(" + ProductID + ") AND ORDER_ID=" + OrderID + " AND MODIFIED_USER=" + UserID;
        //        //}
        //        //oHelper.SQLString = sSQL;
        //        //return oHelper.ExecuteSQLQuery();
        //        sSQL = "Exec STP_TBWC_CANCEL_ORDER_ITEM '" + ProductID + "'," + OrderID + "," + UserID;
        //        return objHelperDB.ExecuteSQLQueryDB(sSQL);
        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        return -1;
        //    }
        //}

        /*********************************** OLD CODE TRADING BELL ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO REMOVE PRODUCT ITEMS BASED ON ORDER ID   ***/
        /********************************************************************************/
        public int RemoveItem(string ProductID, int OrderID, int UserID, string Order_Item_id)
        {
            try
            {
                string sSQL;
                //if (ProductID == "AllProd")
                //{
                //    sSQL = "DELETE FROM TBWC_ORDER_ITEM WHERE ORDER_ID=" + OrderID + " AND MODIFIED_USER=" + UserID;
                //}
                //else
                //{
                //    sSQL = "DELETE FROM TBWC_ORDER_ITEM WHERE PRODUCT_ID IN(" + ProductID + ") AND ORDER_ID=" + OrderID + " AND MODIFIED_USER=" + UserID;
                //}
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string orderstatus = GetOrderStatus(OrderID);
               // objErrorHandler.CreateLog(orderstatus + " before removeitem" + OrderID);
                if ((UserID > 0) && (orderstatus.ToUpper() == "OPEN"))
                {
                  //  objErrorHandler.CreateLog("Inside Remove Item ProductID -" + ProductID +"UserId -" +UserID);
                    sSQL = "Exec STP_TBWC_CANCEL_ORDER_ITEM '" + ProductID + "'," + OrderID + "," + UserID + ",'" + Order_Item_id + "'";
                    return objHelperDB.ExecuteSQLQueryDB(sSQL);
                }
                else
                {
                    return -1;
                }

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
        /*** PURPOSE      : TO REMOVE ORDER CLARIFICATION  ***/
        /********************************************************************************/
        public int Remove_Clarification_item(double Order_Clarification_id)
        {
            try
            {
                string sSQL = "Exec STP_TBWC_CALNCEL_ORDER_CLARIFICATION ";
                sSQL = sSQL + "" + Order_Clarification_id + "";
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {
                //oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        #endregion

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO REMOVE ORDER FROM ORDER TABLE  ***/
        /********************************************************************************/
        public int RemoveOrder(int OrderID, int UserID)
        {
            try
            {

                string sSQL = string.Empty;
                //sSQL = "DELETE FROM TBWC_ORDER WHERE ORDER_ID=" + OrderID + " AND USER_ID = " + UserID;
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string orderstatus = GetOrderStatus(OrderID);
                //objErrorHandler.CreateLog(orderstatus + "inside removeitem" + OrderID);
                if  (orderstatus.ToUpper() == "OPEN")
                {
                    sSQL = "Exec STP_TBWC_CANCEL_ORDER " + OrderID + "," + UserID;
                    return objHelperDB.ExecuteSQLQueryDB(sSQL);
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        public decimal CalculateTaxAmount_Express(decimal ProdTotalPrice, string Orderid)
        {
            try
            {
                //
                string BillState;
                string BillCountry;
                ProdTotalPrice = Math.Round(ProdTotalPrice, 2, MidpointRounding.AwayFromZero);
                /*if (objUserServices.GetTaxExempt(objHelperServices.CI(Session["USER_ID"])) == false)
                {
                    decimal RetTax = 0.00M;
                    if (FIXED_TAX.ToUpper() == "TRUE")
                    {

                        decimal tax = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString()); 
                        RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
                    }
                    else
                    {
                        BillState = objUserServices.GetUserBillStateCode(objHelperServices.CI(Session["USER_ID"]));
                        BillCountry = objUserServices.GetUserBillCountryCode(objHelperServices.CI(Session["USER_ID"]));
                        decimal tax = objHelperServices.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));
                    
                        RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
                    }
                    return RetTax;
                }*/

                //if (objUserServices.GetTaxExempt(objHelperService.CI(Userid), objHelperService.CI(Orderid)) == false)
                if (IsNativeCountry_Express(objHelperService.CI(Orderid)) > 0)
                {
                    decimal RetTax = 0.00M;
                    if (FIXED_TAX.ToUpper() == "TRUE")
                    {

                        decimal tax = objHelperService.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                        RetTax = objHelperService.CDEC(ProdTotalPrice * (tax / 100));
                    }
                    else
                    {
                        //BillState = objUserServices.GetUserBillStateCode(objHelperService.CI(Userid));
                        //BillCountry = objUserServices.GetUserBillCountryCode(objHelperService.CI(Userid));
                        //decimal tax = objHelperService.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));

                        RetTax = 0;// objHelperService.CDEC(ProdTotalPrice * (tax / 100));
                    }
                    return RetTax;
                }
                return 0;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        public decimal CalculateTaxAmount(decimal ProdTotalPrice, string Orderid, string productid)
        {
            try
            {

                ProdTotalPrice = Math.Round(ProdTotalPrice, 2, MidpointRounding.AwayFromZero);

                if (IsNativeCountry(objHelperService.CI(Orderid)) > 0)
                {

                    decimal RetTax = 0.00M;
                    string tempstr = (string)objOrderDB.GetGenericDataDB(productid.ToString(), "GET_PROD_GST", OrderDB.ReturnType.RTString);
                    if (tempstr.ToUpper() == "TRUE")
                    {
                        if (FIXED_TAX.ToUpper() == "TRUE")
                        {

                            decimal tax = objHelperService.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                            RetTax = objHelperService.CDEC(ProdTotalPrice * (tax / 100));
                            RetTax = objHelperService.CDEC(objHelperService.FixDecPlace(RetTax));
                        }
                        else
                        {

                            RetTax = 0;
                        }
                    }
                    else
                    {
                        RetTax = 0;
                    }
                    return RetTax;
                }
                return 0;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        public decimal CalculateTaxAmount(decimal ProdTotalPrice,string Orderid)
        {
            try
            {
                //
                string BillState;
                string BillCountry;
                ProdTotalPrice = Math.Round(ProdTotalPrice, 2, MidpointRounding.AwayFromZero);
                /*if (objUserServices.GetTaxExempt(objHelperServices.CI(Session["USER_ID"])) == false)
                {
                    decimal RetTax = 0.00M;
                    if (FIXED_TAX.ToUpper() == "TRUE")
                    {

                        decimal tax = objHelperServices.CDEC(FIXED_TAX_PERCENTAGE.ToString()); 
                        RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
                    }
                    else
                    {
                        BillState = objUserServices.GetUserBillStateCode(objHelperServices.CI(Session["USER_ID"]));
                        BillCountry = objUserServices.GetUserBillCountryCode(objHelperServices.CI(Session["USER_ID"]));
                        decimal tax = objHelperServices.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));
                    
                        RetTax = objHelperServices.CDEC(objHelperServices.FixDecPlace(ProdTotalPrice * (tax / 100)));
                    }
                    return RetTax;
                }*/

                //if (objUserServices.GetTaxExempt(objHelperService.CI(Userid), objHelperService.CI(Orderid)) == false)
                if (IsNativeCountry(objHelperService.CI(Orderid)) > 0) 
                {
                    decimal RetTax = 0.00M;
                    if (FIXED_TAX.ToUpper() == "TRUE")
                    {

                        decimal tax = objHelperService.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                        RetTax = objHelperService.CDEC(ProdTotalPrice * (tax / 100));
                    }
                    else
                    {
                        //BillState = objUserServices.GetUserBillStateCode(objHelperService.CI(Userid));
                        //BillCountry = objUserServices.GetUserBillCountryCode(objHelperService.CI(Userid));
                        //decimal tax = objHelperService.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));

                        RetTax = 0;// objHelperService.CDEC(ProdTotalPrice * (tax / 100));
                    }
                    return RetTax;
                }
                return 0;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
        }

        public decimal CalculateTaxAmount_DumUser(decimal ProdTotalPrice, string Orderid)
        {
            decimal RetTax = 0.00M;
            try
            {
                if (FIXED_TAX.ToUpper() == "TRUE")
                {

                    decimal tax = objHelperService.CDEC(FIXED_TAX_PERCENTAGE.ToString());
                    RetTax = objHelperService.CDEC(ProdTotalPrice * (tax / 100));
                }
                else
                {
                    //BillState = objUserServices.GetUserBillStateCode(objHelperService.CI(Userid));
                    //BillCountry = objUserServices.GetUserBillCountryCode(objHelperService.CI(Userid));
                    //decimal tax = objHelperService.CDEC(objCountryServices.GetStateTax(BillCountry, BillState));

                    RetTax = 0;// objHelperService.CDEC(ProdTotalPrice * (tax / 100));
                }
                return RetTax;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
            return RetTax;
        }


        public int IsNativeCountry_Express(int order_id)
        {
            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            OrderInfo oOrderInfo = new OrderInfo();
            UserServices ObjUserServices = new UserServices();
            try
            {
                oOrderInfo = GetOrder(order_id);
                ouserinfo = ObjUserServices.GetUserInfo(oOrderInfo.UserID);

                //if (ouserinfo.ShipCountry != null && ouserinfo.ShipCountry != "")
                //{
                //    if (ouserinfo.ShipCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(oOrderInfo.ShipCountry.ToLower()).ToLower() == "au") // is other then au
                //        return 1;

                //}
                 if ( ouserinfo.ShipCountry != null && ouserinfo.ShipCountry != "")
                {
                    if ( ouserinfo.ShipCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(ouserinfo.ShipCountry.ToLower()).ToLower() == "au") // is other then au
                        return 1;
                }
                else
                    return 0;

                return 0;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return 0;
            }
        }

        public int IsNativeCountry_Express_userid(int user_id)
        {
            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            OrderInfo oOrderInfo = new OrderInfo();
            UserServices ObjUserServices = new UserServices();
            try
            {
               // oOrderInfo = GetOrder(order_id);
                ouserinfo = ObjUserServices.GetUserInfo(user_id);

                //if (ouserinfo.ShipCountry != null && ouserinfo.ShipCountry != "")
                //{
                //    if (ouserinfo.ShipCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(oOrderInfo.ShipCountry.ToLower()).ToLower() == "au") // is other then au
                //        return 1;

                //}
                if (ouserinfo.ShipCountry != null && ouserinfo.ShipCountry != "")
                {
                    if (ouserinfo.ShipCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(ouserinfo.ShipCountry.ToLower()).ToLower() == "au") // is other then au
                        return 1;
                }
                else
                    return 0;

                return 0;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return 0;
            }
        }
        //Added By Indu:To checkisnativecountry
        public int IsNativeCountry_Express(string BillCountry, string ShipCountry)
        {
            //UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            // OrderInfo oOrderInfo = new OrderInfo();
            //UserServices ObjUserServices = new UserServices();
            try
            {
                //  oOrderInfo = GetOrder(order_id);
                // ouserinfo = ObjUserServices.GetUserInfo(oOrderInfo.UserID);

                if (ShipCountry != null && ShipCountry != "")
                {
                    if ( ShipCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(ShipCountry.ToLower()).ToLower() == "au") // is other then au
                        return 1;

                }
                //else if (ouserinfo.BillCountry != null && ouserinfo.BillCountry != "" && ouserinfo.ShipCountry != null && ouserinfo.ShipCountry != "")
                //{
                //    if (ouserinfo.BillCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(ouserinfo.BillCountry.ToLower()).ToLower() == "au" && ouserinfo.ShipCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(ouserinfo.ShipCountry.ToLower()).ToLower() == "au") // is other then au
                //        return 1;
                //}
                else
                    return 0;

                return 0;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return 0;
            }
        }

        





        public int IsNativeCountry(int order_id)
        {
            UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
            OrderInfo oOrderInfo=new OrderInfo();
            UserServices ObjUserServices=new UserServices();
            try
            {
                oOrderInfo = GetOrder(order_id);
                ouserinfo = ObjUserServices.GetUserInfo(oOrderInfo.UserID);

                if (oOrderInfo.BillCountry != null && oOrderInfo.BillCountry != "" && oOrderInfo.ShipCountry != null && oOrderInfo.ShipCountry != "")
                {
                    if (oOrderInfo.BillCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(oOrderInfo.BillCountry.ToLower()).ToLower() == "au" && oOrderInfo.ShipCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(oOrderInfo.ShipCountry.ToLower()).ToLower() == "au") // is other then au
                        return 1;

                }
                else if (ouserinfo.BillCountry != null && ouserinfo.BillCountry != "" && ouserinfo.ShipCountry != null && ouserinfo.ShipCountry != "")
                {
                    if (ouserinfo.BillCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(ouserinfo.BillCountry.ToLower()).ToLower() == "au" && ouserinfo.ShipCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(ouserinfo.ShipCountry.ToLower()).ToLower() == "au") // is other then au
                        return 1;
                }
                else 
                    return 0;

                return 0;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return 0;
            }
        }


        //Added By Indu:To checkisnativecountry
        public int IsNativeCountry(string BillCountry, string ShipCountry)
        {
            //UserServices.UserInfo ouserinfo = new UserServices.UserInfo();
           // OrderInfo oOrderInfo = new OrderInfo();
            //UserServices ObjUserServices = new UserServices();
            try
            {
              //  oOrderInfo = GetOrder(order_id);
               // ouserinfo = ObjUserServices.GetUserInfo(oOrderInfo.UserID);

                if (BillCountry != null && BillCountry != "" && ShipCountry != null && ShipCountry != "")
                {
                    if (BillCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(BillCountry.ToLower()).ToLower() == "au" && ShipCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(ShipCountry.ToLower()).ToLower() == "au") // is other then au
                        return 1;

                }
                //else if (ouserinfo.BillCountry != null && ouserinfo.BillCountry != "" && ouserinfo.ShipCountry != null && ouserinfo.ShipCountry != "")
                //{
                //    if (ouserinfo.BillCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(ouserinfo.BillCountry.ToLower()).ToLower() == "au" && ouserinfo.ShipCountry.ToLower().Trim() == "australia" && objUserServices.GetUserCountryCode(ouserinfo.ShipCountry.ToLower()).ToLower() == "au") // is other then au
                //        return 1;
                //}
                else
                    return 0;

                return 0;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return 0;
            }
        }



        public int Update_SHIP_NUMBER(string SHIP_NUMBER, int Order_id)
        {
            int retVal = 0;
            if (SHIP_NUMBER != "" || SHIP_NUMBER != string.Empty)
            {
                SHIP_NUMBER = SHIP_NUMBER.Trim();
            }
            try
            {
                int websiteid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString());
                string sSQL = "Exec STP_TBWC_UPDATE_SHIP_NUMBER_ORDER ";
                sSQL = sSQL + "'" + SHIP_NUMBER + "'," + Order_id + "";
                retVal = objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return retVal;
            }
            return retVal;
        }
        public int Update_MOBILE_NUMBER(string SHIP_NUMBER, int uSER_ID, int Order_id, bool updateorder)
        {
            int retVal = 0;
            if (SHIP_NUMBER != "" || SHIP_NUMBER != string.Empty)
            {
                SHIP_NUMBER = SHIP_NUMBER.Trim();
            }
            try
            {
                int websiteid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString());
                string sSQL = "Exec STP_TBWC_UPDATE_MOBILE_NUMBER_USER ";
                sSQL = sSQL + "'" + SHIP_NUMBER + "'," + uSER_ID + "";
                if (updateorder == true)
                {
                    sSQL = sSQL + " Exec STP_TBWC_UPDATE_SHIP_NUMBER_ORDER ";
                    sSQL = sSQL + "'" + SHIP_NUMBER + "'," + Order_id + "";
                }
                //objErrorHandler.CreateLog(sSQL);
                retVal = objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return retVal;
            }
            return retVal;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO REMOVE TEMPLATE FROM ORDER TEMPLATE  ***/
        /********************************************************************************/
        public int RemoveOrderTemplate( int UserID)
        {
            try
            {
                string sSQL = string.Empty;          
                sSQL = "Exec STP_TBWC_CANCEL_ORDER_TEMPLATE " + UserID;
                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        #endregion
        #region "Shipping Data Retrieve Functions"
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to set the shipping details to order table 
        /// </summary>
        /// <param name="oOrdShipInfo">OrderInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderInfo oOrdShipInfo = new Order.OrderInfo();
        ///     int UpdateStatus;
        ///     ...
        ///     UpdateStatus = oOrder.UpdateShippDetailsToOrder(oOrdShipInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO SET SHIPPING DETAILS FOR ORDER TABLE  ***/
        /********************************************************************************/
        public int UpdateShippDetailsToOrder(OrderInfo oOrdShipInfo)
        {
            try
            {

                //string sSQL = "UPDATE TBWC_ORDER SET SHIP_PREFIX='" + oOrdShipInfo.ShipPrefix + "',SHIP_FNAME='" + oOrdShipInfo.ShipFName + "',";
                //sSQL = sSQL + "SHIP_LNAME='" + oOrdShipInfo.ShipLName + "',SHIP_MNAME='" + oOrdShipInfo.ShipMName + "',SHIP_SUFFIX='" + oOrdShipInfo.ShipSuffix + "',";
                //sSQL = sSQL + "SHIP_ADDRESS_LINE1='" + oOrdShipInfo.ShipAdd1 + "',SHIP_ADDRESS_LINE2='" + oOrdShipInfo.ShipAdd2 + "',SHIP_ADDRESS_LINE3='" + oOrdShipInfo.ShipAdd3 + "',";
                //sSQL = sSQL + "SHIP_CITY='" + oOrdShipInfo.ShipCity + "',SHIP_STATE='" + oOrdShipInfo.ShipState + "',SHIP_ZIP='" + oOrdShipInfo.ShipZip + "',";
                //sSQL = sSQL + "SHIP_COUNTRY='" + oOrdShipInfo.ShipCountry + "',SHIP_PHONE='" + oOrdShipInfo.ShipPhone + "',SHIP_NOTES='" + oOrdShipInfo.ShipNotes + "',SHIP_COST =" + oOrdShipInfo.ShipCost + ",";
                //sSQL = sSQL + "TOTAL_AMOUNT =" + oOrdShipInfo.TotalAmount + ",TAX_AMOUNT =" + oOrdShipInfo.TaxAmount + ",SHIP_METHOD='" + oOrdShipInfo.ShipMethod + "',IS_SHIPPED=" + oHelper.CB(oOrdShipInfo.IsShipped) + ",TRACKING_NO='" + oOrdShipInfo.TrackingNo + "',";
                //sSQL = sSQL + "EST_DELIVERY='" + oOrdShipInfo.EstDelivery + "',SHIP_COMPANY='" + oOrdShipInfo.ShipCompany + "',SHIP_CONF='" + oOrdShipInfo.ShipConf + "',";
                //sSQL = sSQL + "MODIFIED_USER=" + oOrdShipInfo.UserID + ",";
                //sSQL = sSQL + "ORDER_EMAIL_SENT=" + oHelper.CB(oOrdShipInfo.isEmailSent) + ",ORDER_INVOICE_SENT=" + oHelper.CB(oOrdShipInfo.isInvoiceSent) + " WHERE ORDER_ID=" + oOrdShipInfo.OrderID;

                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();


                string sSQL;
                sSQL = "exec STP_TBWC_RENEW_UPDATESHIPPDETAILSTOORDER ";
                sSQL = "'" + oOrdShipInfo.ShipPrefix + "','" + oOrdShipInfo.ShipFName + "',";
                sSQL = sSQL + "'" + oOrdShipInfo.ShipLName + "','" + oOrdShipInfo.ShipMName + "','" + oOrdShipInfo.ShipSuffix + "',";
                sSQL = sSQL + "'" + oOrdShipInfo.ShipAdd1 + "','" + oOrdShipInfo.ShipAdd2 + "','" + oOrdShipInfo.ShipAdd3 + "',";
                sSQL = sSQL + "'" + oOrdShipInfo.ShipCity + "','" + oOrdShipInfo.ShipState + "','" + oOrdShipInfo.ShipZip + "',";
                sSQL = sSQL + "'" + oOrdShipInfo.ShipCountry + "','" + oOrdShipInfo.ShipPhone + "','" + oOrdShipInfo.ShipNotes + "'," + oOrdShipInfo.ShipCost + ",";
                sSQL = sSQL + "" + oOrdShipInfo.TotalAmount + "," + oOrdShipInfo.TaxAmount + ",'" + oOrdShipInfo.ShipMethod + "'," + objHelperService.CB(oOrdShipInfo.IsShipped) + ",'" + oOrdShipInfo.TrackingNo + "',";
                sSQL = sSQL + "'" + oOrdShipInfo.EstDelivery + "','" + oOrdShipInfo.ShipCompany + "','" + oOrdShipInfo.ShipConf + "',";
                sSQL = sSQL + "" + oOrdShipInfo.UserID + ",";
                sSQL = sSQL + "" + objHelperService.CB(oOrdShipInfo.isEmailSent) + "," + objHelperService.CB(oOrdShipInfo.isInvoiceSent) + ", " + oOrdShipInfo.OrderID;
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
        /// This is used to set the billing details in the order table
        /// </summary>
        /// <param name="oOrdBillInfo">OrderInfo</param>
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
        ///     Order oOrder = new Order();
        ///     Order.OrderInfo oOrdBillInfo = new Order.OrderInfo();
        ///     int retUpdateStatus;
        ///     ...
        ///     retUpdateStatus = oOrder.UpdateBillDetailsToOrder(oOrdBillInfo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE BILLING DETAILS TO ORDER TABLE ***/
        /********************************************************************************/
        public int UpdateBillDetailsToOrder(OrderInfo oOrdBillInfo)
        {
            try
            {
                //string sSQL = "UPDATE TBWC_ORDER SET ";
                //sSQL = sSQL + "BILL_ADDRESS_LINE1='" + oOrdBillInfo.BillAdd1 + "',BILL_ADDRESS_LINE2='" + oOrdBillInfo.BillAdd2 + "',BILL_ADDRESS_LINE3='" + oOrdBillInfo.BillAdd3 + "',";
                //sSQL = sSQL + "BILL_CITY='" + oOrdBillInfo.BillCity + "',BILL_STATE='" + oOrdBillInfo.BillState + "',BILL_ZIP='" + oOrdBillInfo.BillZip + "',";
                //sSQL = sSQL + "BILL_COUNTRY='" + oOrdBillInfo.BillCountry + "',BILL_PHONE='" + oOrdBillInfo.BillPhone + "'WHERE ORDER_ID=" + oOrdBillInfo.OrderID;
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();
                string sSQL;
                sSQL = "exec STP_TBWC_RENEW_UPDATE_BILLDETAILS_TOORDER ";
                sSQL = sSQL + "'" + oOrdBillInfo.BillAdd1 + "','" + oOrdBillInfo.BillAdd2 + "','" + oOrdBillInfo.BillAdd3 + "',";
                sSQL = sSQL + "'" + oOrdBillInfo.BillCity + "','" + oOrdBillInfo.BillState + "','" + oOrdBillInfo.BillZip + "',";
                sSQL = sSQL + "'" + oOrdBillInfo.BillCountry + "','" + oOrdBillInfo.BillPhone + "'," + oOrdBillInfo.OrderID;
                return objHelperDB.ExecuteSQLQueryDB(sSQL);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
        }
        #endregion
        #region "Inventry Data Retrieve Functions..."
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to add the Products to the Inventory Table
        /// </summary>
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
        ///     Order oOrder = new Order();
        ///     Order.Inventory Inv = new Order.Inventory();
        ///     int addProd;
        ///     ...
        ///     addProd = oOrder.AddProductToInventory(Inv);
        /// }
        /// </code>
        /// </example> 


        //comment by : palani
        //Comment on : 05.09.2012

        //public int AddProductToInventory(Inventory oInv)
        //{
        //    try
        //    {
        //        string sSQL;
        //        sSQL = "INSERT INTO TBWC_INVENTORY(PRODUCT_ID,QTY_AVAIL,MINORDQTY,LEAD_TIME,IS_ON_SALE,DISCOUNT_PERCENT,LIST_PRICE_VALID_TILL,";
        //        sSQL = sSQL + "SALE_PRICE_VALID_TILL,PRODUCT_STATUS,CREATED_DATA,CREATED_USER,MODIFIED_DATE,MODIFIED_USER) ";
        //        sSQL = sSQL + "VALUES(" + oInv.ProductID + "," + oInv.QtyAvail + "," + oInv.MinOrdQty + ",'" + oInv.MinOrdQty + "'," + oInv.isOnSale + ",";
        //        sSQL = sSQL + oInv.isOnSale + ",'" + oInv.ListPriceValidTill + "','" + oInv.SalePriceValidTill + "'," + oInv.ProductStatus;
        //        sSQL = sSQL + "',{fn now()},'" + oInv.UserID + "','{fn now()},'" + oInv.UserID + "')";
        //        oHelper.SQLString = sSQL;
        //        return oHelper.ExecuteSQLQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        //oErrHand.ErrorMsg = e;
        //        ////oErrHand.CreateLog();
        //        //return -1;
        //        objErrorHandler.ErrorMsg = e;
        //        //oErrHand.CreateLog();
        //        return -1;
        //    }
        //}
        /// <summary>
        /// This is used to set the products to Inventory list
        /// </summary>
        /// <param name="oInv">Inventory</param>
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
        ///     Order oOrder = new Order();
        ///     Order.Inventory Inv = new Order.Inventory();
        ///     int UpdateStatus;
        ///     ...
        ///     UpdateStatus = oOrder.UpdateProductDetailsToInventry(Inv);
        /// }
        /// </code>
        /// </example>

        //Comment by : palani
        //Comment on : 05.09.2012

        //public int UpdateProductDetailsToInventry(Inventory oInv)
        //{
        //    try
        //    {
        //        string sSQL;
        //        sSQL = "UPDATE TBWC_INVENTORY SET QTY_AVAIL =" + oInv.QtyAvail + ",MINIORDQTY =" + oInv.MinOrdQty + ",LEAD_TIME = '" + oInv.LeadTime + "'";
        //        sSQL = sSQL + ",IS_ON_SALE =" + oInv.isOnSale + ",DISCOUNT_PERCENT =" + oInv.Discount + ",LIST_PROCE_VALID_TILL ='" + oInv.ListPriceValidTill + "'";
        //        sSQL = sSQL + ",SALE_PRICE_VALID_TILL = '" + oInv.SalePriceValidTill + "',PRODUCT_STAUS =" + oInv.ProductStatus + ", MODIFIED_DATE = {fn now()},'MODIFIED_USER ='" + oInv.UserID + "'";
        //        sSQL = sSQL + "WHERE PRODUCT_ID = " + oInv.ProductID;
        //        oHelper.SQLString = sSQL;
        //        return oHelper.ExecuteSQLQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        oErrHand.ErrorMsg = e;
        //        //oErrHand.CreateLog(); 
        //        return -1;
        //    }
        //}
        /// <summary>
        /// This is used to set the Available Quantity
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="Qty">int</param>
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
        ///     Order oOrder = new Order();
        ///     int ProductID;
        ///     int Qty;
        ///     int UpdateQty;
        ///     ...
        ///     UpdateQty = oOrder.UpdateQuantity(ProductID, Qty);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO UPDATE THE QUANTITY AVAILABILITY  ***/
        /********************************************************************************/
        public int UpdateQuantity(int ProductID, int Qty)
        {
            int retVal = 0;
            try
            {
                string sSQL = "";
                //if (Qty > 0)
                //    sSQL = " UPDATE TBWC_INVENTORY SET QTY_AVAIL=" + Qty + ",PRODUCT_STATUS='AVAILABLE'";
                //else
                //    sSQL = " UPDATE TBWC_INVENTORY SET QTY_AVAIL=" + Qty;

                //sSQL = sSQL + " WHERE PRODUCT_ID = " + ProductID;
                //oHelper.SQLString = sSQL;
                retVal = 1; //oHelper.ExecuteSQLQuery(); /// TBWC_INVENTORY DOESNOT APPLY TO WES CUSTOMER
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
        /// This is used to set the Tax Amount
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     decimal TaxAmount;
        ///     int retVal;
        ///      ...
        ///     retVal = oOrder.UpdateTaxAmount(OrderID,TaxAmount);
        /// }
        /// </code>
        /// </example>

        //comment by : palani
        //Comment on : 05.09.2012

        //public int UpdateTaxAmount(int OrderID, decimal TaxAmount)
        //{
        //    int retVal = 0;
        //    try
        //    {
        //        string sSQL = "UPDATE TBWC_ORDER SET TAX_AMOUNT =" + TaxAmount + ",TOTAL_AMOUNT = PRODUCT_TOTAL_PRICE + SHIP_COST + " + TaxAmount + " WHERE ORDER_ID =" + OrderID;
        //        oHelper.SQLString = sSQL;
        //        return oHelper.ExecuteSQLQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        oErrHand.ErrorMsg = e;
        //        //oErrHand.CreateLog();
        //        retVal = -1;
        //        return retVal;
        //    }

        //}
        /// <summary>
        /// This is used to check the product having shipping charge or not
        /// </summary>
        /// <param name="ProductID">int</param>
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
        ///     Order oOrder = new Order();
        ///     bool IsShipping = false;
        ///     ...
        ///     IsShipping = oOrder.GetProductIsShipping(oHelper.CI(rItem["Product_id"].ToString()));
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK THE PRODUCT IS SHPPED OR NOT  ***/
        /********************************************************************************/
        public bool GetProductIsShipping(int ProductID)
        {
            bool retVal = false;
            string tempstr = "";
            bool boolstr = false;
            try
            {
                //string sSQL = "SELECT IS_SHIPPING FROM TBWC_INVENTORY WHERE PRODUCT_ID= " + ProductID;
                //oHelper.SQLString = sSQL;
                tempstr = (string)objOrderDB.GetGenericDataDB(ProductID.ToString(), "Get_Product_IsShipping", OrderDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    boolstr = Convert.ToBoolean(tempstr);

                //if (oHelper.GetValue("IS_SHIPPING") == "True")
                if (boolstr == true)
                {
                    retVal = true;
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retVal = false;
            }
            return retVal;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to set the shipping cost
        /// </summary>
        /// <param name="OrderID">int</param>
        /// <param name="ShipCost">decimal</param>
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
        ///     Order oOrder = new Order(); 
        ///     int OrdId;
        ///     decimal Shipcost;
        ///     int retVal;
        ///     ...
        ///     retVal = oOrder.UpdateShippingCost(OrdId,Shipcost);
        /// }
        /// </code>
        /// </example>

        //comment by : palani
        //Comment on : 05.09.2012

        //public int UpdateShippingCost(int OrderID, decimal ShipCost)
        //{
        //    int retVal = 0;
        //    try
        //    {
        //        string sSQL = "UPDATE TBWC_ORDER SET SHIP_COST =" + ShipCost + ",TOTAL_AMOUNT = PRODUCT_TOTAL_PRICE + TAX_AMOUNT + " + ShipCost + " WHERE ORDER_ID =" + OrderID;
        //        oHelper.SQLString = sSQL;
        //        retVal = oHelper.ExecuteSQLQuery();
        //    }
        //    catch (Exception e)
        //    {
        //        oErrHand.ErrorMsg = e;
        //        //oErrHand.CreateLog();
        //        retVal = -1;
        //    }
        //    return retVal;
        //}

        /// <summary>
        /// This is used to get the products available quantity
        /// </summary>
        /// <param name="ProductID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int ProductID;
        ///     int qtyAvail;
        ///     ...
        ///     qtyAvail = oOrder.GetProductAvilableQty(ProductID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT AVAILABLE QUANTITY  ***/
        /********************************************************************************/
        public int GetProductAvilableQty(int ProductID)
        {
            string tempstr = "";
            int reval = 0;
            try
            {
                //string sSQL;
                //sSQL = "SELECT QTY_AVAIL FROM TBWC_INVENTORY WHERE PRODUCT_ID = " + ProductID;
                //oHelper.SQLString = sSQL;
                //return oHelper.CI(oHelper.GetValue("QTY_AVAIL"));
                tempstr = (string)objOrderDB.GetGenericDataDB(ProductID.ToString(), "Get_Product_AvilableQty", OrderDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    reval = objHelperService.CI(tempstr);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
            return reval;
        }
        /*********************************** OLD CODE ***********************************/

        /// <summary>
        /// This is used to get products minimum order quantity
        /// </summary>
        /// <param name="ProductID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int ProductID;
        ///     int retVal;
        ///     ...
        ///     retVal = oOrder.GetProductMinimumOrderQty(ProductID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO GET PRODUCT MINIMUM ORDER QUQNTITY  ***/
        /********************************************************************************/
        public int GetProductMinimumOrderQty(int ProductID)
        {
            string sSQL;
            string tempstr = "";
            int reval = 0;
            try
            {
                //sSQL = "SELECT MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_ID = " + ProductID;
                //oHelper.SQLString = sSQL;
                //return oHelper.CI(oHelper.GetValue("MIN_ORD_QTY"));
                tempstr = (string)objOrderDB.GetGenericDataDB(ProductID.ToString(), "Get_Product_Minimum_OrderQty", OrderDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    reval = objHelperService.CI(tempstr);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return -1;
            }
            return reval;
        }

        /// <summary>
        /// This is used to get ItemDetailsFrom Inventory based on ProductID
        /// </summary>
        /// <param name="ProductID">int</param>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ITEM DETAILS FROM INVENTORY  ***/
        /********************************************************************************/
        public DataSet GetItemDetailsFromInventory(int ProductID)
        {
            DataSet oDs = new DataSet();
            try
            {
                //string sSQL = "SELECT * FROM TBWC_INVENTORY WHERE PRODUCT_ID = " + ProductID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objOrderDB.GetGenericDataDB(ProductID.ToString(), "Get_ItemDetails_From_Inventory", OrderDB.ReturnType.RTDataSet);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK DROP SHIPMENT KEY EXIST OR NOT ***/
        /********************************************************************************/
        public bool GetDropShipmentKeyExist(string value, string Type)
        {
            string tempstr = "";
            bool reval = false;
            try
            {
                if (Type == "PostCode")
                {
                    tempstr = (string)objOrderDB.GetGenericDataDB("", value.ToString(), Type, "GET_DROP_SHIPMENT_CHECKING", OrderDB.ReturnType.RTString);
                }
                else
                {
                    tempstr = (string)objOrderDB.GetGenericDataDB("", value.ToString(), Type, "GET_DROP_SHIPMENT_CHECKING", OrderDB.ReturnType.RTString);
                }
                if (tempstr != null && tempstr != "")
                    reval = true;

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return true;
            }
            return reval;
        }
        #endregion
        #region "Invoice functions.."

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the orders total cost
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     decimal TotCost;
        ///     ...
        ///     TotCost = oOrder.GetOrderTotalCost(OrderID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ORDER TOTAL COST  ***/
        /********************************************************************************/
        public decimal GetOrderTotalCost(int OrderID)
        {
            decimal TotCst = 0.00M;
            string tempstr = "";
            try
            {
                //string sSQL = "SELECT ISNULL(TOTAL_AMOUNT,0.00)AS TOTAL_AMOUNT  FROM TBWC_ORDER WHERE ORDER_ID =" + OrderID;
                //oHelper.SQLString = sSQL;
                //TotCst = oHelper.CDEC(oHelper.GetValue("TOTAL_AMOUNT"));
                tempstr = (string)objOrderDB.GetGenericDataDB(OrderID.ToString(), "Get_Order_TotalCost", OrderDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    TotCst = objHelperService.CDEC(tempstr);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return TotCst;

        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ORDER - Make Payment mail=0  ***/
        /********************************************************************************/
        public DataTable  GetOrder_Mate_Payment_mail_detail()
        {

            
            try
            {

                return (DataTable)objOrderDB.GetGenericDataDB("", "GET_MAKE_PAYMENT_ORDERS", OrderDB.ReturnType.RTTable);
              
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return null;

        }


        public DataTable Get_Order_Open_Status_Details()
        {
            try
            {

                return (DataTable)objOrderDB.GetGenericDataDB("", "GET_OPEN_STATUS_ORDERS", OrderDB.ReturnType.RTTable);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return null;
        }

        public DataTable Get_Order_Open_Status_Details_ABO()
        {
            try
            {

                return (DataTable)objOrderDB.GetGenericDataDB("", "GET_OPEN_STATUS_ORDERS_WAG_ABO", OrderDB.ReturnType.RTTable);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return null;
        }
        public DataTable Get_Error_Mail_List_Resend()
        {
            try
            {

                return (DataTable)objOrderDB.GetGenericDataDB("", "GET_ERROR_MAIL_LIST", OrderDB.ReturnType.RTTable);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return null;
        }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ORDER - Make Payment mail=0  ***/
        /********************************************************************************/
        public int UpdateMake_Payment_mail_status(int order_id,int status)
        {


            try
            {

                string sSQL;
                sSQL = "exec STP_TBWC_RENEW_ORDER_MAKE_PAYMENT_MAIL ";
                sSQL = sSQL + order_id.ToString() + "," + status ;
                return objHelperDB.ExecuteSQLQueryDB(sSQL);


            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return -1;

        }

        public int Update_Mail_Error_Log(int order_id, int isresend)
        {
            try
            {

                string sSQL;
                sSQL = "exec STP_TBWC_RENEW_MAIL_ERROR_LOG ";
                sSQL = sSQL + order_id.ToString() + "," + isresend;
                return objHelperDB.ExecuteSQLQueryDB(sSQL);


            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return -1;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the shipping cost based on the order id
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///     Order oOrder = new Order();
        ///     int OrderID;
        ///     decimal ShippCost;
        ///     ...
        ///     ShippCost = oOrder.GetShippingCost(OrderID);
        /// }
        /// </code>

        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE SHIPPING TOTAL COST  ***/
        /********************************************************************************/
        public decimal GetShippingCost(int OrderID)
        {
            decimal ShippCst = 0.00M;
            string tempstr = "";
            try
            {
                //string sSQL = "SELECT ISNULL(SHIP_COST,0.00)AS SHIP_COST FROM TBWC_ORDER WHERE ORDER_ID =" + OrderID;
                //oHelper.SQLString = sSQL;
                //ShippCst = oHelper.CDEC(oHelper.GetValue("SHIP_COST"));
                tempstr = (string)objOrderDB.GetGenericDataDB(OrderID.ToString(), "Get_Shipping_Cost", OrderDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    ShippCst = objHelperService.CDEC(tempstr);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            return ShippCst;

        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the tax amount based on order id
        /// </summary>
        /// <param name="OrderID">int</param>
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
        ///  Order oOrder = new Order();
        ///  int OrderID;
        ///  decimal Tax;
        ///  ...
        ///  Tax = oOrder.GetTaxAmount(OrderID);
        /// }
        /// </code>
        /// </example>
         /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE TAX AMOUNT DETAILS ***/
        /********************************************************************************/
        public decimal GetTaxAmount(int OrderID)
        {
            decimal Tax = 0.00M;
            string tempstr = "";
            try
            {
                //string sSQL = "SELECT ISNULL(TAX_AMOUNT,0.00) AS TAX_AMOUNT FROM TBWC_ORDER WHERE ORDER_ID =" + OrderID;
                //oHelper.SQLString = sSQL;
                //Tax = oHelper.CDEC(oHelper.GetValue("TAX_AMOUNT"));
                tempstr = (string)objOrderDB.GetGenericDataDB(OrderID.ToString(), "Get_Tax_Amount", OrderDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    Tax = objHelperService.CDEC(tempstr);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return Tax;
        }
        #endregion

        public void UpdatePaymentOrderStatus_Express(int OrderID, int PaymentID, bool isipn)
        {
            if (IsNativeCountry_Express(OrderID) == 0) // is other then au
            {
                if (isipn == false)
                {
                    objErrorHandler.CreatePayLog(" UpdatePaymentOrderStatus return0 Orderid=" + OrderID);
                    UpdateOrderStatus(OrderID, (int)OrderServices.OrderStatus.Proforma_Payment_Success);
                    int Rtn = SentSignal(PaymentID.ToString(), OrderID.ToString(), "190");
                }
            }
            else
            {
                if (isipn == false)
                {
                    objErrorHandler.CreatePayLog("UpdatePaymentOrderStatus return1 Orderid=" + OrderID);
                    UpdateOrderStatus(OrderID, (int)OrderServices.OrderStatus.Payment_Successful);
                    int Rtn = SentSignal(PaymentID.ToString(), OrderID.ToString(), "150");
                }
            }

        }
 

        public void UpdatePaymentOrderStatus(int OrderID, int PaymentID, bool isipn)
        {
            if (IsNativeCountry(OrderID) == 0) // is other then au
            {
                if (isipn == false)
                {
                    objErrorHandler.CreatePayLog(" UpdatePaymentOrderStatus return0 Orderid=" + OrderID);
                    UpdateOrderStatus(OrderID, (int)OrderServices.OrderStatus.Proforma_Payment_Success);
                    int Rtn = SentSignal(PaymentID.ToString(), OrderID.ToString(), "190");
                }
            }
            else
            {
                if (isipn == false)
                {
                    objErrorHandler.CreatePayLog("UpdatePaymentOrderStatus return1 Orderid=" + OrderID);
                    UpdateOrderStatus(OrderID, (int)OrderServices.OrderStatus.Payment_Successful);
                    int Rtn = SentSignal(PaymentID.ToString(), OrderID.ToString(), "150");
                }
            }

        }

        public DataTable GetOrder_PICKUP_ORDER()
        {


            try
            {

                return (DataTable)objOrderDB.GetGenericDataDB("", "GET_PICKUP_ORDERS", OrderDB.ReturnType.RTTable);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return null;

        }

        public DataTable GET_MAKE_PAYMENT_ORDERS_RESEND()
        {


            try
            {

                return (DataTable)objOrderDB.GetGenericDataDB("", "GET_MAKE_PAYMENT_ORDERS_RESEND", OrderDB.ReturnType.RTTable);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return null;

        }
        public DataTable Awaiting_Make_Payment_mail_detail()
        {


            try
            {

                return (DataTable)objOrderDB.GetGenericDataDB("", "GET_MAKE_PAYMENT_ORDERS_5", OrderDB.ReturnType.RTTable);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return null;

        }
        public int UpdatePAYMENTSELECTION(int OrderID, string PAYMENTSELECTION)
        {
            int retVal = 0;
            try
            {
                //string sSQL = "UPDATE TBWC_ORDER SET ORDER_STATUS = " + OrderStatus + " WHERE ORDER_ID = " + OrderID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.ExecuteSQLQuery();
                string sSQL = "Exec STP_TBWC_RENEW_UPDATE_PAYMENTSELECTION ";
                sSQL = sSQL + "" + OrderID + "," + PAYMENTSELECTION + "";
                retVal = objHelperDB.ExecuteSQLQueryDB(sSQL);
                //  objErrorHandler.CreateLog(sSQL + "----" + retVal.ToString());

            }
            catch (Exception e)
            {
                // oErrHand.ErrorMsg = e;
                //oErrHand.CreateLog();
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return retVal;
            }
            return retVal;
        }

        public int UpdateDeleteOrderStatus(int OrderID, int UserId)
        {
            int retVal = 0;
            try
            {
                string sSQL = "Exec STP_TBWC_CANCEL_DeleteOrder ";
                sSQL = sSQL + "" + OrderID + "," + UserId + "";
                retVal = objHelperDB.ExecuteSQLQueryDB(sSQL);

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return retVal;
            }
            return retVal;
        }

        public DataSet getBulkBuyPrice(int ProductID, int UserId)
        {
            DataSet oDs = new DataSet();
            try
            {
                string sSQL = "exec GetPriceTableWagner '" + ProductID.ToString() + "','" + UserId + "'";
                return objHelperDB.GetDataSetDB(sSQL);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                return null;
            }
        }

    }

   



    /*********************************** J TECH CODE ***********************************/
}