using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.Common;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    /// <summary>
    /// This is used to get the Promotional Product Details
    /// </summary>
    /// <remarks>
    /// Used to get Product Promotional List (discount Products),DisCount amount..
    /// </remarks>
    /// <example>
    ///  ProductPromotion oPP = new ProductPromotion();
    /// </example>
    public class ProductPromotionServices
    {
        /*********************************** DECLARATION ***********************************/
        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        ProductDB objProductDB = new ProductDB();
        HelperServices objHelperServices = new HelperServices();
        /*********************************** DECLARATION ***********************************/
        /// <summary>
        /// Default Constructor
        /// </summary>
        /*********************************** CONSTRUCTOR ***********************************/
        public ProductPromotionServices()
        {
            //InitializeComponent(); 
        }
        /*********************************** CONSTRUCTOR ***********************************/
        #region "Functions"
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Products List
        /// </summary>
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
        ///     ProductPromotion oPP = new ProductPromotion();
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oPP.GetProductsList();
        /// }
        /// </code>
        /// </example>
        //
        //public DataSet GetProductsList()
        //{
        //    try
        //    {
        //        //string sSQL = "SELECT PP.PRODUCT_ID,";
        //        //sSQL = sSQL + "(SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = TP.PRODUCT_ID) AS CATALOG_ITEM_NO,";
        //        //sSQL = sSQL + "PP.DISCOUNT_PRICE FROM TBWC_PROD_PROMOTION PP, TBWC_INVENTORY TI,TB_PRODUCT TP";
        //        //sSQL = sSQL + " WHERE PP.PRODUCT_ID=TI.PRODUCT_ID";
        //        //sSQL = sSQL + " AND TP.PRODUCT_ID = PP.PRODUCT_ID";
        //        //sSQL = sSQL + " AND TI.PRODUCT_STATUS='AVAILABLE'";


        //        string sSQL = "SELECT PF.FAMILY_ID,PP.PRODUCT_ID, " 
        //                      + "(SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = TP.PRODUCT_ID) AS CATALOG_ITEM_NO," 
        //                      + "PP.DISCOUNT_PRICE,TI.QTY_AVAIL,TI.MIN_ORD_QTY FROM TBWC_PROD_PROMOTION PP, TBWC_INVENTORY TI,TB_PRODUCT TP,TB_PROD_FAMILY PF " 
        //                      + "WHERE PP.PRODUCT_ID=TI.PRODUCT_ID AND TP.PRODUCT_ID = PP.PRODUCT_ID AND TI.PRODUCT_STATUS='AVAILABLE' AND PF.PRODUCT_ID= PP.PRODUCT_ID ";

        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErr.ErrorMsg = ex;
        //        // oErr.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>
        /// This is used to check that the product is available in the promotional list
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
        ///  ProductPromotion oPP = new ProductPromotion();
        ///  int ProductID;
        ///  bool retVal;
        ///  ...
        ///  retVal = oPP.CheckPromotion(ProductID);
        /// }
        /// </code>
        /// </example> 
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK THE PRODCUT IS IN PROMOTION LIST OR NOT  ***/
        /********************************************************************************/
        public bool CheckPromotion(int ProductID)
        {
            bool retVal = false;
            string tempstr="";
            try
            {
                //string sSQL = "SELECT COUNT(PRODUCT_ID)AS PRODUCTCOUNT FROM TBWC_PROD_PROMOTION WHERE PRODUCT_ID = " + ProductID;
                //oHelper.SQLString = sSQL;
                //if (oHelper.CI(oHelper.GetValue("PRODUCTCOUNT")) > 0)
                //{
                //    retVal = true;
               // }
                tempstr = (string)objProductDB.GetGenericDataDB(ProductID.ToString(), "GET_PRODUCT_CHECK_PROMOTION", ProductDB.ReturnType.RTString); 
                if (tempstr!=null && tempstr!="")
                    if (System.Convert.ToInt32(tempstr)>0)
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
        /// This is used to get the product promotional discount values
        /// </summary>
        /// <param name="ProductID">int</param> 
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
        ///  ProductPromotion oPP = new ProductPromotion();
        ///  int ProductID;
        ///  decimal retVal;
        ///  ...
        ///  retVal = oPP.GetProductPromotionDiscValue(ProductID));
        /// }
        /// </code>
        /// </example> 
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PRODUCT PROMOTIONAL DISCOUNT VALUES ***/
        /********************************************************************************/
        public decimal GetProductPromotionDiscValue(int ProductID)
        {
            decimal retVal = 0.00M;
            string tempstr = "";
            try
            {
                //string sSQL = "SELECT DISCOUNT_PRICE FROM TBWC_PROD_PROMOTION WHERE PRODUCT_ID = " + ProductID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.CDEC(oHelper.GetValue("DISCOUNT_PRICE"));
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
        /// <summary>
        /// This is used to get the promotional products specifications
        /// </summary>
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
        ///  ProductPromotion oPP = new ProductPromotion();
        ///  DataSet oDS=new DataSet();
        ///  ...
        ///  oDS = oPP.GetPromotionalProducts();
        /// }
        /// </code>
        /// </example> 
        //
        //public DataSet GetPromotionalProducts()
        //{
        //    int rowcount = 0;
        //    int CatalogID = Convert.ToInt32(oHelper.GetOptionValues("DEFAULT CATALOG"));
        //    string currencyFormat = oHelper.GetOptionValues("CURRENCYFORMAT");
        //    string productID = null;
        //    string FlagPID = null;
        //    DataSet dsSpec;
        //    DataSet dsProd;
        //    DataSet dsFinal = new DataSet();
        //    DataColumn dc;
        //    dsFinal.Tables.Add();
        //    DataRow drF = dsFinal.Tables[0].NewRow();
        //    dsProd = GetProductsList();
        //    //Assigning primary key for find
        //    DataColumn[] dcc = new DataColumn[1];
        //    dcc[0] = dsProd.Tables[0].Columns[1];
        //    dsProd.Tables[0].PrimaryKey = dcc;
        //    foreach (DataRow drP in dsProd.Tables[0].Rows)
        //    {
        //        productID = productID + drP["Product_id"].ToString() + ",";
        //    }
        //    dsSpec = GetProductSpecsValuesAll(productID.Substring(0, productID.Length - 1), false, CatalogID);
        //    FlagPID = dsSpec.Tables[0].Rows[0]["PRODUCT_ID"].ToString();
        //    dc = new DataColumn("PRODUCT_ID", typeof(string));
        //    dsFinal.Tables[0].Columns.Add(dc);
        //    dc = new DataColumn("DISCOUNT_PRICE");
        //    dsFinal.Tables[0].Columns.Add(dc);
        //    foreach (DataRow drS in dsSpec.Tables[0].Rows)
        //    {
        //        //Build rows
        //        if (FlagPID != drS["PRODUCT_ID"].ToString())
        //        {
        //            drF["PRODUCT_ID"] = FlagPID.ToString();
        //            drF["DISCOUNT_PRICE"] = dsProd.Tables[0].Rows.Find(FlagPID.ToString()).ItemArray[3].ToString();
        //            dsFinal.Tables[0].Rows.Add(drF);
        //            drF = dsFinal.Tables[0].NewRow();
        //            FlagPID = drS["PRODUCT_ID"].ToString();
        //        }
        //        //Build Columns and its data
        //        if (dsFinal.Tables[0].Columns.Contains(drS["ATTRIBUTE_NAME"].ToString().ToUpper()) == false)
        //        {
        //            dc = new DataColumn(drS["ATTRIBUTE_NAME"].ToString().ToUpper(), typeof(string));
        //            dsFinal.Tables[0].Columns.Add(dc);
        //            if (drS["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX") == true)
        //            {
        //                drF[drS["ATTRIBUTE_NAME"].ToString().ToUpper()] = drS["STRING_VALUE"].ToString();
        //            }
        //            else if (drS["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("NUM") == true)
        //            {
        //                //To check Price Attributes and to calculate Discount price
        //                if (drS["ATTRIBUTE_TYPE"].ToString() == "4")
        //                {
        //                    drF[drS["ATTRIBUTE_NAME"].ToString().ToUpper()] = currencyFormat + Math.Round(Convert.ToDecimal(drS["NUMERIC_VALUE"].ToString()), 2);
        //                    if (dsFinal.Tables[0].Columns.Contains(drS["ATTRIBUTE_NAME"].ToString().ToUpper() + "_DISCOUNT") == false)
        //                    {
        //                        dc = new DataColumn(drS["ATTRIBUTE_NAME"].ToString().ToUpper() + "_DISCOUNT", typeof(string));
        //                        dsFinal.Tables[0].Columns.Add(dc);
        //                        drF[drS["ATTRIBUTE_NAME"].ToString().ToUpper() + "_DISCOUNT"] = currencyFormat + (Math.Round(Convert.ToDecimal(drS["NUMERIC_VALUE"].ToString()), 2) - Math.Round(Convert.ToDecimal(dsProd.Tables[0].Rows.Find(FlagPID.ToString()).ItemArray[3].ToString()), 2));
        //                    }
        //                    else
        //                    {
        //                        drF[drS["ATTRIBUTE_NAME"].ToString().ToUpper() + "_DISCOUNT"] = currencyFormat + (Math.Round(Convert.ToDecimal(drS["NUMERIC_VALUE"].ToString()), 2) - Math.Round(Convert.ToDecimal(dsProd.Tables[0].Rows.Find(FlagPID.ToString()).ItemArray[3].ToString()), 2));
        //                    }
        //                }
        //                else
        //                {
        //                    drF[drS["ATTRIBUTE_NAME"].ToString().ToUpper()] = drS["NUMERIC_VALUE"].ToString();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (drS["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX") == true)
        //            {
        //                drF[drS["ATTRIBUTE_NAME"].ToString().ToUpper()] = drS["STRING_VALUE"].ToString();
        //            }
        //            else if (drS["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("NUM") == true)
        //            {
        //                //To check Price Attributes and to calculate Discount price
        //                if (drS["ATTRIBUTE_TYPE"].ToString() == "4")
        //                {
        //                    drF[drS["ATTRIBUTE_NAME"].ToString().ToUpper()] = currencyFormat + Math.Round(Convert.ToDecimal(drS["NUMERIC_VALUE"].ToString()), 2);
        //                    if (dsFinal.Tables[0].Columns.Contains(drS["ATTRIBUTE_NAME"].ToString().ToUpper() + "_DISCOUNT") == false)
        //                    {
        //                        dc = new DataColumn(drS["ATTRIBUTE_NAME"].ToString().ToUpper() + "_DISCOUNT", typeof(string));
        //                        dsFinal.Tables[0].Columns.Add(dc);
        //                        drF[drS["ATTRIBUTE_NAME"].ToString().ToUpper() + "_DISCOUNT"] = currencyFormat + (Math.Round(Convert.ToDecimal(drS["NUMERIC_VALUE"].ToString()), 2) - Math.Round(Convert.ToDecimal(dsProd.Tables[0].Rows.Find(FlagPID.ToString()).ItemArray[3].ToString()), 2));
        //                    }
        //                    else
        //                    {
        //                        drF[drS["ATTRIBUTE_NAME"].ToString().ToUpper() + "_DISCOUNT"] = currencyFormat + (Math.Round(Convert.ToDecimal(drS["NUMERIC_VALUE"].ToString()), 2) - Math.Round(Convert.ToDecimal(dsProd.Tables[0].Rows.Find(FlagPID.ToString()).ItemArray[3].ToString()), 2));
        //                    }
        //                }
        //                else
        //                {
        //                    drF[drS["ATTRIBUTE_NAME"].ToString().ToUpper()] = drS["NUMERIC_VALUE"].ToString();
        //                }
        //            }
        //        }
        //        rowcount++;
        //        if (rowcount == dsSpec.Tables[0].Rows.Count)
        //        {
        //            drF["PRODUCT_ID"] = drS["PRODUCT_ID"].ToString();
        //            drF["DISCOUNT_PRICE"] = dsProd.Tables[0].Rows.Find(drS["PRODUCT_ID"].ToString()).ItemArray[3].ToString();
        //            dsFinal.Tables[0].Rows.Add(drF);
        //        }

        //    }
        //    return (dsFinal);
        //}
        /// This is used to get the specifications for set of Products
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="isValues">bool</param>
        /// <param name="CatalogID">int</param>
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
        ///  ProductPromotion oPP = new ProductPromotion();
        ///  int ProductID;
        ///  bool Status;
        ///  int CatalogID;
        ///  DataSet oDS=new DataSet();
        ///  ...
        ///  oDS = oPP.GetProductSpecsValuesAll(ProductID,Status,CatalogID);
        /// }
        /// </code>
        /// </example> 
        //
        //public DataSet GetProductSpecsValuesAll(string ProductID, bool isValues, int CatalogID)
        //{
        //    string sSQL = "";
        //    if (isValues)
        //    {
        //        sSQL = " SELECT TPS.STRING_VALUE,TPS.NUMERIC_VALUE,TA.ATTRIBUTE_NAME, TPS.ATTRIBUTE_ID,TA.ATTRIBUTE_TYPE,TA.ATTRIBUTE_DATATYPE,TPS.PRODUCT_ID ";
        //        sSQL = sSQL + " FROM TB_PROD_SPECS TPS, TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA ";
        //        sSQL = sSQL + " WHERE TPS.ATTRIBUTE_ID = TA.ATTRIBUTE_ID AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
        //        sSQL = sSQL + " AND TA.PUBLISH2WEB = 1 AND TA.ATTRIBUTE_ID IN (1,2,5) AND PRODUCT_ID IN(" + ProductID + ") ORDER BY TPS.PRODUCT_ID";

        //    }
        //    else
        //    {
        //        sSQL = "SELECT  TPS.STRING_VALUE ,TPS.NUMERIC_VALUE, TA.ATTRIBUTE_NAME, TPS.ATTRIBUTE_ID,TA.ATTRIBUTE_TYPE,TA.ATTRIBUTE_DATATYPE, TPS.PRODUCT_ID ";
        //        sSQL = sSQL + " FROM TB_PROD_SPECS TPS, TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE TPS.ATTRIBUTE_ID = TA.ATTRIBUTE_ID ";
        //        sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID + " AND TA.PUBLISH2WEB = 1 AND PRODUCT_ID IN(" + ProductID + ") ORDER BY TPS.PRODUCT_ID";
        //    }

        //    oHelper.SQLString = sSQL;
        //    return oHelper.GetDataSet();
        //}
        #endregion
        /*********************************** OLD CODE ***********************************/

    }

    /*********************************** J TECH CODE ***********************************/
}
