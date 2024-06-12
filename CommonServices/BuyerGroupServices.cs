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
    /// This is Used to Manage the Dealer's Details
    /// </summary>
    /// <remarks>
    /// Used to get the Buyer group details like Discount Percentage,Discount Amount,Dealers...
    /// </remarks>
    /// <example>
    ///  BuyerGroup oBG = new BuyerGroup();
    /// </example>
    
    public class BuyerGroupServices 
    {
        /*********************************** DECLARATION ***********************************/
        HelperDB objHelperDb = new HelperDB();
        ProductDB objProductDB = new ProductDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        /// <summary>
        /// Create Enum Values for Default Buyer Group
        /// </summary>

        public enum DefaultBG
        {
            /// <summary>
            /// Default Buyer Group
            /// </summary>
            DEFAULTBG = 1,
            /// <summary>
            /// Buyer Group default method is Percentage
            /// </summary>
            PERCENTAGEMETHOD = 2,
            /// <summary>
            /// Buyer Group default method is Amount
            /// </summary>
            AMOUNTMETHOD = 3
        }
        /*********************************** DECLARATION ***********************************/
        /// <summary>
        /// This is Used to Manage the Dealer's Details
        /// </summary>
        /// 
        /*********************************** CONSTRUCTOR ***********************************/
        public BuyerGroupServices()
        {
        }
        /*********************************** CONSTRUCTOR ***********************************/
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is Used to get the Discount Price Details.
        /// </summary>
        /// <param name="BuyerGroup">Stirng</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.Web;
        ///using System.Data;
        ///using System.IO;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// 
        ///protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     BuyerGroup oBG =new BuyerGroup();
        ///     string BuyerGroup;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oBG.GetBuyerGroupBasedDiscountDetails(BuyerGroup);
        /// }
        ///  </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE TO RETRIVE THE DISCOUNT DETAILS FOR BUYER GROUP ***/
        /********************************************************************************/

        public DataSet GetBuyerGroupBasedDiscountDetails(string BuyerGroup)
        {
            try
            {
                //string sSQL = " SELECT DISCOUNT =";
                //sSQL = sSQL + " CASE";
                //sSQL = sSQL + " WHEN USE_PCT = 1 THEN DISC_PCT";
                //sSQL = sSQL + " ELSE DISC_AMT";
                //sSQL = sSQL + " END, VALID_DATE =";
                //sSQL = sSQL + " CASE";
                //sSQL = sSQL + " WHEN USE_PCT = 1 THEN DISC_PCT_TILL_DT";
                //sSQL = sSQL + " ELSE DISC_AMT_VALID_TILL_DT";
                //sSQL = sSQL + " END,DISC_METHOD =";
                //sSQL = sSQL + " CASE";
                //sSQL = sSQL + " WHEN USE_PCT = 1 THEN '" + DefaultBG.PERCENTAGEMETHOD.ToString() + "'";
                //sSQL = sSQL + " ELSE '" + DefaultBG.AMOUNTMETHOD.ToString() + "'";
                //sSQL = sSQL + " END FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP = '" + BuyerGroup + "'";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objHelperDb.GetGenericDataDB("", BuyerGroup, DefaultBG.PERCENTAGEMETHOD.ToString(), DefaultBG.AMOUNTMETHOD.ToString(), "GET_BUYER_GROUP_BASED_DISCOUNT", HelperDB.ReturnType.RTDataSet);

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
        /// This is Used to get the buyer group
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>String</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.Web;
        ///using System.Data;
        ///using System.IO;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     BuyerGroup oBG =new BuyerGroup();
        ///     int UserID;
        ///     string BGroup;
        ///     ...
        ///     BGroup = oBG.GetBuyerGroup(UserID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE BUYER GROUP DETAILS FROM RELATED DATABASE TABLES ***/
        /********************************************************************************/
        public string GetBuyerGroup(int UserID)
        {
            string retVal;
            try
            {
                if (UserID > 0)
                {
                    //string sSQL = " SELECT TBG.BUYER_GROUP FROM TBWC_BUYER_GROUP TBG,TBWC_COMPANY TC,TBWC_COMPANY_BUYERS TCB";
                    //sSQL = sSQL + " WHERE TBG.BUYER_GROUP = TC.BUYER_GROUP";
                    //sSQL = sSQL + " AND TC.COMPANY_ID = TCB.COMPANY_ID AND USER_ID =" + UserID;
                    //oHelper.SQLString = sSQL;
                    //retVal = oHelper.GetValue("BUYER_GROUP");
                    retVal = (string)objHelperDb.GetGenericDataDB(UserID.ToString(), "GET_BUYER_GROUP", HelperDB.ReturnType.RTString);
                }
                else
                {
                    retVal = DefaultBG.DEFAULTBG.ToString();
                }

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retVal = "";
            }
            return retVal;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is Used to get the buyer group price ID
        /// </summary>
        /// <param name="UserID">int</param>
        /// <returns>int</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.Web;
        ///using System.Data;
        ///using System.IO;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     BuyerGroup oBG =new BuyerGroup();
        ///     int UserID;
        ///     int BGPriceId;
        ///     ...
        ///     BGPriceId = oBG.GetBuyerGroupPriceID(UserID);
        /// }
        ///  </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/
        /********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE  BUYER GROUP PRICE ID FROM RELATED DATABASE TABLES ***/
        /********************************************************************************/
      
        public int GetBuyerGroupPriceID(int UserID)
        {
            int retPriceID = 0;
            string tempstr="";
            try
            {
                string sSQL = "";
                if (UserID > 0)
                {
                  //  sSQL = " SELECT TBG.PRICE_ATTRIBUTE_ID FROM TBWC_COMPANY TC,TBWC_COMPANY_BUYERS TCB,TBWC_BUYER_GROUP TBG";
                   // sSQL = sSQL + " WHERE TC.COMPANY_ID = TCB.COMPANY_ID AND TBG.BUYER_GROUP = TC.BUYER_GROUP";
                   // sSQL = sSQL + " AND TCB.USER_ID = " + UserID;
                    tempstr = (string)objProductDB.GetGenericDataDB("", UserID.ToString(), "1", "GET_PRODUCT_PRICE_ATTRIBUTE_ID", ProductDB.ReturnType.RTString);    

                }
                else if (UserID == 0)
                {
                    //Get default buyer group price(base_price).
                    //sSQL = "SELECT PRICE_ATTRIBUTE_ID FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP = 'DEFAULTBG'";
                    tempstr = (string)objProductDB.GetGenericDataDB("", UserID.ToString(), "0", "GET_PRODUCT_PRICE_ATTRIBUTE_ID", ProductDB.ReturnType.RTString);  
                }
                //oHelper.SQLString = sSQL;
                //retPriceID = oHelper.CI(oHelper.GetValue("PRICE_ATTRIBUTE_ID"));
                if (tempstr != null && tempstr != "")
                    retPriceID = Convert.ToInt32(tempstr);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retPriceID = -1;
            }
            return retPriceID;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is Used to calculate the Discount Price
        /// </summary>
        /// <param name="CurrentPrice">decimal</param>
        /// <param name="DiscountValue">decimal</param>
        /// <param name="DiscoundMethod">String</param>
        /// <returns>decimal</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.Web;
        ///using System.Data;
        ///using System.IO;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     BuyerGroup oBG =new BuyerGroup();
        ///     decimal CurrentPrice;
        ///     decimal DiscountValue;
        ///     string DiscoundMethod;
        ///     decimal BGDiscountPrice;
        ///     ...
        ///     BGDiscountPrice = oBG.CalculateBGDiscountPrice(CurrentPrice, DiscountValue, DiscoundMethod);
        /// }
        ///  </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DISCOUNT PRICE FOR BUYER GROUP BASED ON CURRENT PRICE,DISCOUNT VALUE,DISCOUNT METHOD ***/
        /********************************************************************************/
        public decimal CalculateBGDiscountPrice(decimal CurrentPrice, decimal DiscountValue, string DiscoundMethod)
        {
            decimal retPrice = 0;
            try
            {
                if (DiscoundMethod == DefaultBG.PERCENTAGEMETHOD.ToString())
                {
                    decimal DiscountPrice = (CurrentPrice * DiscountValue) / 100;
                    retPrice = CurrentPrice - DiscountPrice;
                }
                else
                {
                    retPrice = CurrentPrice - DiscountValue;

                }
                if (retPrice < 0)
                {
                    retPrice = 0;
                }
                else
                {
                    retPrice = objHelperServices.CDEC(retPrice.ToString("N2"));
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retPrice = 0;
            }

            return retPrice;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is Used to check that the products are already exist int the Promotional Product List
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <returns>bool</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.Web;
        ///using System.Data;
        ///using System.IO;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     BuyerGroup oBG =new BuyerGroup(); 
        ///     int ProductID;
        ///     bool chkPromotion;
        ///     ...
        ///     chkPromotion = oBG.CheckPromotion(ProductID);
        ///  </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER THE PRODUCT ID IS PROMOTION OR NOT ***/
        /********************************************************************************/
        public bool CheckPromotion(int ProductID)
        {
            bool retVal = false;
            string tempstr = "";
            try
            {
                //string sSQL = "SELECT COUNT(PRODUCT_ID) FROM TBWC_PROD_PROMOTION WHERE PRODUCT_ID = " + ProductID;
                //oHelper.SQLString = sSQL;
                //if (oHelper.CI(oHelper.GetValue("PRODUCT_ID")) > 0)
                //{
                   //retVal = true;
                //}
                tempstr = (string)objProductDB.GetGenericDataDB(ProductID.ToString(), "GET_PRODUCT_CHECK_PROMOTION", ProductDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    if (Convert.ToInt32(tempstr) > 0)
                        retVal = true;

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retVal = false;
            }
            return retVal;

        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is Used to Get the Promotional Product Price
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <returns>decimal</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.Web;
        ///using System.Data;
        ///using System.IO;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///    BuyerGroup oBG =new BuyerGroup();
        ///    int ProductID;
        ///    decimal ProdpromotPrice;
        ///    ...
        ///    ProdpromotPrice = oBG.GetProductPromotionPrice(ProductID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DISCOUNT PRICE FOR A PARTICULAR PRODUCT ***/
        /********************************************************************************/
        public decimal GetProductPromotionPrice(int ProductID)
        {
            decimal retVal = 0.00M;
            string tempstr="";
            try
            {
                //string sSQL = "SELECT DISCOUNT_PRICE FROM TBWC_PROD_PROMOTION WHERE PRODUCT_ID = " + ProductID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.CDEC(oHelper.GetValue("PRODUCT_ID"));
                tempstr = (string)objProductDB.GetGenericDataDB(ProductID.ToString(), "GET_PRODUCT_PROMOTION_PRICE", ProductDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    retVal = objHelperServices.CDEC(tempstr); 

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retVal = 0.00M;
            }
            return retVal;

        }

        /*********************************** OLD CODE ***********************************/
        //public int GetBuyerGroupCatalog(int UserID)
        //{
        //    int retVal = 1;
        //    try
        //    {
        //        string sSQL = " SELECT TBG.BUYER_GROUP,TBG.CATALOG_ID FROM TBWC_BUYER_GROUP TBG,TBWC_COMPANY TC,TBWC_COMPANY_BUYERS TCB";
        //        sSQL = sSQL + " WHERE TBG.BUYER_GROUP = TC.BUYER_GROUP";
        //        sSQL = sSQL + " AND TC.COMPANY_ID = TCB.COMPANY_ID AND USER_ID =" + UserID;
        //        oHelper.SQLString = sSQL;
        //        DataSet oDS = new DataSet();
        //        oDS = oHelper.GetDataSet();
        //        retVal = oHelper.CI(oDS.Tables[0].Rows[0]["CATALOG_ID"].ToString());
        //    }
        //    catch (Exception e)
        //    {
        //        oErr.ErrorMsg = e;
        //        oErr.CreateLog();
        //    }
        //    return retVal;
        //}
        /*********************************** OLD CODE ***********************************/

        /********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER THE PRODUCT CATALOG ID AND BUYER GROUP CATALOD ID ARE THE SAME OR NOT ***/
        /********************************************************************************/
        public bool IsBGCatalogProduct(int ProductCatalogID,string BuyerGroupName)
        {
            bool retVal = false;
            string tempstr = "";
            try
            {
                //string sSQL = "SELECT CATALOG_ID FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP='" + BuyerGroupName + "'";
                //oHelper.SQLString = sSQL;
                //int BGCatalogID = oHelper.CI(oHelper.GetValue("CATALOG_ID").ToString());
                int BGCatalogID = 0;
                tempstr = (string)objHelperDb.GetGenericDataDB(BuyerGroupName, "GET_BUYER_GROUP_CATALOG_ID", HelperDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    BGCatalogID = objHelperServices.CI(tempstr);

                if (ProductCatalogID == BGCatalogID)
                {
                    retVal = true;
                }
                else
                {
                    retVal = false;
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retVal = false;
                return retVal;
            }
            return retVal;
        }

    }
    /*********************************** J TECH CODE ***********************************/
}