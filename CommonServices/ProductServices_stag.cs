using System;
using System.Data;
using System.Data.OleDb;
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
    /// This is used to get all the Product Details 
    /// </summary>
    ///<remarks>
    /// Used to get the Product List,Produt fl List,Attributes of the Product ,Specifications and Descriptions of the Products..
    ///</remarks>
    ///<example>
    /// Product oProd = new Product();
    ///</example>




    public class ProductServices
    {
        /*********************************** DECLARATION ***********************************/
        HelperDB objHelperDB = new HelperDB();
        HelperServices objHelperServices = new HelperServices();
        ProductDB objProductDB = new ProductDB();
        Security objSecurity = new Security();
    
        ErrorHandler objErrorHandler = new ErrorHandler();
        string EA_root_Cat_path1 = System.Configuration.ConfigurationManager.AppSettings["EA_ROOT_CATEGORY_PATH"].ToString();
        string EA_New_Product_init_cat_path1 = System.Configuration.ConfigurationManager.AppSettings["EA_NEW_PRODUCT_INIT_CATEGORY_PATH"].ToString();
        string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        //ConnectionDB objConnectionDB = new ConnectionDB();
        string sCategoryNames = "";
        string retVal = "";
        /*********************************** DECLARATION ***********************************/
        /// <summary>   
        /// Default constructor.
        /// </summary>
        /*********************************** CONSTRUCTOR ***********************************/
        public ProductServices()
        {

        }
        /*********************************** CONSTRUCTOR ***********************************/
        # region "Functions"
        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get all the Product Details using Product ID 
        /// </summary>
        /// <param name="productID">int</param>
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
        ///     Product oProd = new Product();
        ///     int productID;
        ///     DataSet oDS = new DataSet():
        ///     ...
        ///     oDS = oProd.GetProduct(productID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        public DataTable GetPopProduct(int product_id, int Family_id)
        {
            try
            {

                return objHelperDB.GetDataTableDB("exec STP_TBWC_PICK_POP_PRODUCT " + product_id.ToString() + "," + Family_id.ToString());


            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        public string GetProductCode(int ProductID)
        {
            string Retval = "";
            try
            {
                DataTable objrbl = (DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_CODE", HelperDB.ReturnType.RTTable);
                // DataTable objrbl = (DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_CODE_WAG", HelperDB.ReturnType.RTTable);

                if (objrbl != null && objrbl.Rows.Count > 0)
                {
                    Retval = objrbl.Rows[0]["STRING_VALUE"].ToString();
                }
                else
                    Retval = "";
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            return Retval;
        }
      
        public int GetStockStatusDesc(string StockStatusDesc, int StockStatus, int StockFlag, int user_id)
        {
            UserServices objUserServices = new UserServices();
            //string CustomerType = objUserServices.GetCustomerType(user_id);

            int Retval = 0;

            //if ((StockStatusDesc.ToUpper().Contains("OUT OF STOCK ITEM WILL BE BACK ORDERED") || StockStatusDesc.ToUpper().Contains("SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED"))) // && CustomerType.ToLower() == "dealer"
            //    Retval = 1;
            //else
            //    if (StockStatus==1)
            //        Retval = 1;
            //    else if (StockFlag == 0)
            //        Retval = 1;
            //    else
            //        Retval = 0;

            if (StockFlag == 0)
                Retval = 1;
            else
                Retval = 0;

            return Retval;


        }
       
        public int GetStockStatus(int ProductID, int user_id)
        {
            int Retval = 0;
            try
            {
                UserServices objUserServices = new UserServices();
                string CustomerType = objUserServices.GetCustomerType(user_id);

                DataTable objrbl = (DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_INVENTORY_BULKORDER", HelperDB.ReturnType.RTTable);
                if (objrbl != null && objrbl.Rows.Count > 0)
                {
                    string strstockstatus = "";
                    string strstockstatus_flag = "";
                    string strsubs_pro = "";
                    foreach (DataRow dr in objrbl.Rows)
                    {
                        //strstockstatus = dr["STOCK_STATUS"].ToString().Replace("_", " ");                      
                        //if ((strstockstatus.ToUpper().Contains("OUT OF STOCK ITEM WILL BE BACK ORDERED") || strstockstatus.ToUpper().Contains("SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED"))) // && CustomerType.ToLower() == "dealer"
                        //    Retval = 1;
                        //else
                        //{
                        //    strstockstatus = dr["PROD_STOCK_STATUS"].ToString();
                        //    strstockstatus_flag = dr["PROD_STOCK_FLAG"].ToString();
                        //    if (strstockstatus.ToLower() == "true" || strstockstatus.ToLower() == "1")
                        //        Retval = 1;
                        //    else if(strstockstatus_flag.ToLower()=="0")
                        //        Retval = 1;
                        //}

                        //Modified By indu..To get updated stock status
                   /*     strstockstatus_flag = dr["PROD_STOCK_FLAG"].ToString();
                        strsubs_pro = dr["PROD_SUBSTITUTE"].ToString();

                        if (strstockstatus_flag.ToLower() == "0")
                            Retval = 1;
                        else if (strsubs_pro.ToString().Trim().Length > 0)
                            Retval = 0;
                        else
                            Retval = 1; */
                        string PROD_SUBSTITUTE = dr["PROD_SUBSTITUTE"].ToString();
                        if (strstockstatus_flag == "0")
                        {
                            Retval = 1;
                        } 
                        //Modified by:indu for client new requirement to allow out of stock item  to be added into add to cart.

                        else if (strstockstatus_flag == "-2" || strstockstatus.ToUpper().Contains("OUT OF STOCK ITEM WILL BE BACK ORDERED"))
                        {
                            objErrorHandler.CreateLog("strstockstatus_flag:" + strstockstatus_flag + " " + "PROD_STOCK_STATUS:" + dr["PROD_STOCK_STATUS"].ToString() + " " + ProductID.ToString() + strstockstatus);

                            if (PROD_SUBSTITUTE.ToString().Trim().Length > 0)
                            {
                                Retval = 0;
                            }
                            //
                            else if (strstockstatus_flag == "-2" && (dr["PROD_STOCK_STATUS"].ToString().ToUpper() == "FALSE" || dr["PROD_STOCK_STATUS"].ToString().ToUpper() == "0") && (strstockstatus.ToUpper().Contains("DISCONTINUED NO LONGER AVAILABLE") || strstockstatus.ToUpper().Contains("TEMPORARY_UNAVAILABLE NO ETA") || strstockstatus.ToUpper().Contains("TEMPORARY UNAVAILABLE NO ETA")))
                            {


                                Retval = 0;


                            }
                            else
                            {
                                Retval = 1;
                            }

                        }
                      

                    }

                }
                else
                    Retval = 0;
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            return Retval;
        }
        public string GetProductID_code(string ProductCode)
        {
            string Retval = "";
            try
            {
                DataTable objrbl = (DataTable)objHelperDB.GetGenericDataDB(ProductCode.ToString(), "GET_SINGLE_PRODUCT_ID", HelperDB.ReturnType.RTTable);
                // DataTable objrbl = (DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_CODE_WAG", HelperDB.ReturnType.RTTable);

                if (objrbl != null && objrbl.Rows.Count > 0)
                {
                    Retval = objrbl.Rows[0]["PRODUCT_ID"].ToString();
                }
                else
                    Retval = "";
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            return Retval;
        }
        
        public DataTable GetSubstituteProduct(string ProductCode)
        {
            DataTable retvalue = null;
            try
            {
                string sSQL = string.Format("EXEC STP_TBWC_PICK_SUBSTITUTE_PRODUCT '" + ProductCode + "'");
               // objErrorHandler.CreateLog(sSQL);
                retvalue = objHelperDB.GetDataTableDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retvalue;
        }
        public DataTable GetSubstituteProductDetails(string _replaceItem, int userid,string pagename)
        {
            DataTable substituteproduct = new DataTable();
            DataTable rtntable = new DataTable();
            bool samecodesubproduct = false;
            bool samecodenotFound = false;
            string wag_product_code = "";

            int checksetsub = -1;
            string product_status = "";
            int StrProductStatusSub = -1;
            string StrStockStatus = "";
            int ProdCodeId = 0;
            string Ea_Path = "";

            string pfid = "";
            string _catid = "";
            string SubstuyutePid = "";
            string neweapath = "";
            DataColumn dc = new DataColumn("CatId", typeof(string));
            dc.DefaultValue = "";
            rtntable.Columns.Add(dc);
            dc = new DataColumn("Pfid", typeof(string));
            dc.DefaultValue = "";
            rtntable.Columns.Add(dc);

            dc = new DataColumn("Ea_Path", typeof(string));
            dc.DefaultValue = "";
            rtntable.Columns.Add(dc);

            dc = new DataColumn("samecodesubproduct", typeof(bool));
            dc.DefaultValue = false;
            rtntable.Columns.Add(dc);

            dc = new DataColumn("samecodenotFound", typeof(bool));
            dc.DefaultValue = false;
            rtntable.Columns.Add(dc);
            dc = new DataColumn("wag_product_code", typeof(string));
            dc.DefaultValue = "";
            rtntable.Columns.Add(dc);
            dc = new DataColumn("SubstuyutePid", typeof(string));
            dc.DefaultValue = "";
            rtntable.Columns.Add(dc);


            try
            {

                substituteproduct = GetSubstituteProduct(_replaceItem);


                if (substituteproduct != null && substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString() != "Not Found" && substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString() != "")
                {
                    DataSet tmpds1 = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, substituteproduct.Rows[0]["SUBSTITUTEPID"].ToString(), "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID", HelperDB.ReturnType.RTDataSet);
                    SubstuyutePid = substituteproduct.Rows[0]["SUBSTITUTEPID"].ToString();
                    if (tmpds1 != null && tmpds1.Tables.Count > 0)
                    {
                        _catid = tmpds1.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                        if (objHelperServices.CI(tmpds1.Tables[0].Rows[0]["PARENT_FAMILY_ID"]) > 0)
                        {
                            pfid = tmpds1.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();
                            string[] catpath = null;
                          
                                catpath = tmpds1.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                           
                          

                                Ea_Path = EA_New_Product_init_cat_path1 + "////" +pfid+"////"+SubstuyutePid + "=" + substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString()+"////"+ (catpath.Length >=1?catpath[0]:" ")  +"////"+(catpath.Length >=2?catpath[1]:" ") + "////" +tmpds1.Tables[0].Rows[0]["FAMILY_NAME"].ToString() ;
                           // Ea_Path += SubstuyutePid + "=" + substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString();
                           
                            Ea_Path = objHelperServices.SimpleURL_Str( Ea_Path, "pd.aspx",true) + "/"+pagename+"/";


                            //Ea_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                        }
                        else
                        {
                            pfid = tmpds1.Tables[0].Rows[0]["FAMILY_ID"].ToString();
                            string[] catpath = tmpds1.Tables[0].Rows[0]["CATEGORY_PATH"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);

                            Ea_Path = EA_New_Product_init_cat_path1 + "////" +pfid+"////"+SubstuyutePid + "=" + substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString()+"////"+
                               (catpath.Length >= 1 ? catpath[0] : " ") + "////" + (catpath.Length >= 2 ? catpath[1] : " ") + "////" + tmpds1.Tables[0].Rows[0]["FAMILY_NAME"].ToString();
                           
                           
                            
                            Ea_Path = objHelperServices.SimpleURL_Str( Ea_Path, "pd.aspx",true) + "/" + pagename + "/";
                            //Ea_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(Ea_Path));
                        }
                       
                         
                        
                    }
                    if (_replaceItem == substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString() || substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString().Trim() == "")
                        samecodesubproduct = true;
                    else
                    {
                        wag_product_code = substituteproduct.Rows[0]["SUBSTITUTEPCODE"].ToString();
                        DataTable protblsub = (DataTable)objHelperDB.GetGenericPageDataDB("'" + wag_product_code + "'", "GET_BULKORDER_INVENTORY_COUNT_ALL", HelperDB.ReturnType.RTTable);

                        if (protblsub != null && protblsub.Rows.Count > 0)
                        {

                            foreach (DataRow drow in protblsub.Rows)
                            {
                                if (drow["product_status"].ToString().ToUpper() == "AVAILABLE")
                                {
                                    checksetsub = 1;
                                    ProdCodeId = Convert.ToInt32(drow["PRODUCT_ID"].ToString());
                                    StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_", " ");
                                    StrProductStatusSub = GetStockStatusDesc(StrStockStatus, Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString())), Convert.ToInt32((drow["PROD_STOCK_FLAG"].ToString())), userid);

                                    //if (GetStockStatusDesc(StrStockStatus,Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString())),Convert.ToInt32((drow["PROD_STOCK_FLAG"].ToString())),  userid) == 1)
                                    //    StrProductStatusSub = 1;
                                    //else
                                    //    StrProductStatusSub = Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString()));
                                    product_status = drow["product_status"].ToString();
                                    break;
                                }
                            }
                            if (product_status == "")
                            {
                                foreach (DataRow drow in protblsub.Rows)
                                {
                                    if (drow["product_status"].ToString().ToUpper() != "AVAILABLE")
                                    {
                                        checksetsub = 2;
                                        ProdCodeId = Convert.ToInt32(drow["PRODUCT_ID"].ToString());
                                        StrStockStatus = drow["STOCK_STATUS"].ToString().Replace("_", " ");
                                        StrProductStatusSub = GetStockStatusDesc(StrStockStatus, Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString())), Convert.ToInt32((drow["PROD_STOCK_FLAG"].ToString())), userid);

                                        //if (GetStockStatusDesc(StrStockStatus, userid) == 1)
                                        //    StrProductStatusSub = 1;
                                        //else
                                        //    StrProductStatusSub = Convert.ToInt32((drow["PROD_STOCK_STATUS"].ToString()));

                                        product_status = drow["product_status"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        if (checksetsub == 1 && StrProductStatusSub == 1)
                        {
                            samecodesubproduct = false;
                        }
                        else
                            samecodesubproduct = true;

                    }
                }
                else
                {
                    wag_product_code = _replaceItem;
                    DataTable protblsub = (DataTable)objHelperDB.GetGenericPageDataDB("'" + wag_product_code + "'", "GET_BULKORDER_INVENTORY_COUNT_ALL", HelperDB.ReturnType.RTTable);

                    if (protblsub != null && protblsub.Rows.Count > 0)
                    {
                        samecodesubproduct = true;
                    }
                    else
                        samecodenotFound = true;

                }

           
                DataRow dr = rtntable.NewRow();
                dr["CatId"] = _catid;
                dr["Pfid"] = pfid;
                dr["Ea_Path"] = Ea_Path;
                dr["samecodesubproduct"] = samecodesubproduct;
                dr["samecodenotFound"] = samecodenotFound;
                dr["wag_product_code"] = wag_product_code;
                dr["SubstuyutePid"] = SubstuyutePid;
                rtntable.Rows.Add(dr);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
            return rtntable;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  RETRIVE ALL PRODUCT DETAILS***/
        /********************************************************************************/
        public DataSet GetProduct(int productID)
        {
            try
            {
                //string sSQL = " SELECT TP.PRODUCT_ID, TP.CATEGORY_ID, ";
                //sSQL = sSQL + " (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = TP.PRODUCT_ID) AS CATALOG_ITEM_NO,";
                //sSQL = sSQL + " (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =2 AND PRODUCT_ID = TP.PRODUCT_ID) AS MFG_PART_NO,";
                //sSQL = sSQL + " (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =4 AND PRODUCT_ID = TP.PRODUCT_ID) AS SUPPLIER_NAME,";
                //sSQL = sSQL + " (SELECT NUMERIC_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =3 AND PRODUCT_ID = TP.PRODUCT_ID) AS BASE_PRICE,  ";
                //sSQL = sSQL + " TPF.FAMILY_ID FROM TB_PRODUCT TP, TB_PROD_FAMILY TPF ";
                //sSQL = sSQL + " WHERE  TP.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE' AND  TP.PRODUCT_ID = " + productID;
                //sSQL = sSQL + " ORDER BY SORT_ORDER ";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objProductDB.GetGenericDataDB(productID.ToString() , "GET_PRODUCT", ProductDB.ReturnType.RTDataSet); 
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get all the Product Details 
        /// </summary>
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
        ///     Product oProd = new Product();
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oProd.GetProductList();
        /// }
        /// </code>
        /// </example>

        //
        //public DataTable GetProductList()
        //{
        //    try
        //    {
        //        DataTable dtProd = new DataTable();
        //        string sSQL = " SELECT TP.PRODUCT_ID AS PRODUCT_ID,";
        //        sSQL = sSQL + " (SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID =1 AND PRODUCT_ID = TP.PRODUCT_ID) AS CATALOG_ITEM_NO,";
        //        sSQL = sSQL + " TPF.FAMILY_ID AS FAMILY_ID ";
        //        sSQL = sSQL + " FROM TB_PRODUCT TP, TB_PROD_FAMILY TPF";
        //        sSQL = sSQL + " WHERE TP.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE'";
        //        sSQL = sSQL + " AND TPF.FAMILY_ID > 0";
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>   
        /// This is used to get all the Product List using core Attributes
        /// </summary>
        /// <param name="ProdID">int</param>
        /// <param name="CoreAttribute1">string</param>
        /// <param name="CoreAttribute2">string</param>
        /// <param name="CoreAttribute4">string</param>
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
        ///     Product oProd = new Product();
        ///     string CoreAttribute_1;
        ///     string CoreAttribute_2;
        ///     string CoreAttribute_4;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductList(0, CoreAttribute_1, CoreAttribute_2, CoreAttribute_4);
        /// }
        /// </code>
        /// </example>

        //
        //public DataSet GetProductList(int ProdID, string CoreAttribute1, string CoreAttribute2, string CoreAttribute4)
        //{
        //    try
        //    {
        //        DataTable dtProd = new DataTable();
        //        string sSQL = "";

        //        if (ProdID == 0)
        //        {
        //            sSQL = " SELECT PRODUCT_ID AS PRODUCT_ID, CATEGORY_ID,";
        //            sSQL = sSQL + " CATALOG_ITEM_NO AS '" + CoreAttribute1 + "', MFG_PART_NO AS '" + CoreAttribute2 + "', SUPPLIER_NAME AS '" + CoreAttribute4 + "'";
        //            sSQL = sSQL + " FROM TB_PRODUCT ";
        //        }
        //        else
        //        {
        //            sSQL = " SELECT PRODUCT_ID AS PRODUCT_ID, CATEGORY_ID,";
        //            sSQL = sSQL + " CATALOG_ITEM_NO AS '" + CoreAttribute1 + "', MFG_PART_NO AS '" + CoreAttribute2 + "', SUPPLIER_NAME AS '" + CoreAttribute4 + "'";
        //            sSQL = sSQL + " FROM TB_PRODUCT WHERE PRODUCT_ID IN(" + ProdID + ")";
        //        }
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>   
        /// This is used to get all the product list
        /// </summary>
        /// <param name="SupplierName">string</param>
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
        ///     Product oProd = new Product();
        ///     string SupplierName;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductList(SupplierName);
        /// }
        /// </code>
        /// </example>

        //
        //public DataSet GetProductList(string SupplierName)
        //{
        //    try
        //    {
        //        DataSet oDS = GetCoreAttributesName();
        //        string CoreAttr1 = oDS.Tables[0].Rows[0].ItemArray[0].ToString();

        //        string sSQL = " SELECT PRODUCT_ID,SUPPLIER_NAME AS 'SUPPLIER NAME',CATALOG_ITEM_NO AS '" + CoreAttr1 + "' FROM TB_PRODUCT WHERE SUPPLIER_NAME = '" + SupplierName + "'";
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataSet();
        //    }
        //    catch (Exception e)
        //    {
        //        oErrHand.ErrorMsg = e;
        //        //oErrHand.CreateLog()
        //        return null;
        //    }
        //}
        /// <summary>   
        /// This is used to get the family product list
        /// If restricted product is Enabled that product is not able to Purchase.
        /// 
        /// </summary>
        /// <param name="ProductFamilyID">int</param>
        /// <param name="EnabledRestrictedProducts">bool</param>
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
        ///     Product oProd = new Product();
        ///     int ProductFamilyID;
        ///     bool EnabledRestrictedProducts;
        ///     int CatalogID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetFamilyProductList(ProductFamilyID, EnabledRestrictedProducts,CatalogID);
        /// }
        /// </code>
        /// </example>

        //
        //public DataSet GetRestrictedFamilyProductList(int FamilyID, bool EnabledRestrictedProducts,int CatalogID)
        //{
        //    try
        //    {
        //        string sSQL = "";
        //        if (EnabledRestrictedProducts)
        //        {
        //            sSQL = " SELECT TP.PRODUCT_ID AS PRODUCT_ID, TP.CATEGORY_ID,TPF.FAMILY_ID,TBI.MIN_ORD_QTY AS MIN_ORD_QTY,";
        //            sSQL = sSQL + " TBI.QTY_AVAIL AS QTY_AVAIL,TBI.IS_SHIPPING AS IS_SHIPPING, TPS.STRING_VALUE AS RESTRICTED";
        //            sSQL = sSQL + " FROM TB_PRODUCT TP,TB_PROD_FAMILY TPF, TBWC_INVENTORY TBI, TB_PROD_SPECS TPS,TB_CATALOG_PRODUCT TCP ";
        //            sSQL = sSQL + " WHERE  TP.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE' AND TPS.PRODUCT_ID=TP.PRODUCT_ID";
        //            sSQL = sSQL + " AND TBI.PRODUCT_ID=TPF.PRODUCT_ID AND TBI.PRODUCT_STATUS='AVAILABLE'AND TPF.FAMILY_ID IN(" + FamilyID + " )";
        //            sSQL = sSQL + " AND TPS.ATTRIBUTE_ID = (SELECT ATTRIBUTE_ID AS RESTRICTED FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME ='~Restricted')";
        //            sSQL = sSQL + " AND TP.PRODUCT_ID = TCP.PRODUCT_ID AND TCP.CATALOG_ID =" + CatalogID;
        //            sSQL = sSQL + " ORDER BY SORT_ORDER";
        //        }
        //        else
        //        {
        //            sSQL = " SELECT TP.PRODUCT_ID AS PRODUCT_ID, TP.CATEGORY_ID,TPF.FAMILY_ID,TBI.MIN_ORD_QTY AS MIN_ORD_QTY,TBI.QTY_AVAIL AS QTY_AVAIL,TBI.IS_SHIPPING AS IS_SHIPPING ";
        //            sSQL = sSQL + " FROM TB_PRODUCT TP, TB_PROD_FAMILY TPF, TBWC_INVENTORY TBI,TB_CATALOG_PRODUCT TCP WHERE  TP.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE'";
        //            sSQL = sSQL + " AND TBI.PRODUCT_ID=TPF.PRODUCT_ID AND TBI.PRODUCT_STATUS='AVAILABLE' AND TPF.FAMILY_ID IN(" + FamilyID + " )"; 
        //            sSQL = sSQL + " AND TP.PRODUCT_ID = TCP.PRODUCT_ID AND TCP.CATALOG_ID ="+ CatalogID;
        //            sSQL =sSQL + " ORDER BY SORT_ORDER";
        //        }
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataSet();

        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}

        /// <summary>   
        /// This is used to get the Restricted Products from the Products List
        /// </summary>
        /// <param name="ProductID">int</param> 
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
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     string retVal;
        ///     ...
        ///     retVal = oProd.GetRestrictedProduct(ProductID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DETAILS OF RESTRICTED PRODUCTS ***/
        /********************************************************************************/
        public string GetRestrictedProduct(int ProductID)
        {
            string retVal = "";
            try
            {
                //string sSQL = "SELECT TPS.STRING_VALUE AS RESTRICTED FROM TB_PRODUCT TP,TB_PROD_FAMILY TPF, TB_PROD_SPECS TPS ";
                //sSQL = sSQL + "WHERE  TP.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE' ";
                //sSQL = sSQL + "AND TPS.PRODUCT_ID=TP.PRODUCT_ID ";
                //sSQL = sSQL + "AND TPS.PRODUCT_ID IN(" + ProductID + " )";
                //sSQL = sSQL + "AND TPS.ATTRIBUTE_ID = (SELECT ATTRIBUTE_ID AS RESTRICTED FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME ='~Restricted') ";
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.GetValue("RESTRICTED");
                //return retVal;
                return (string)objProductDB.GetGenericDataDB(ProductID.ToString(), "GET_RESTRICTED_PRODUCT", ProductDB.ReturnType.RTString); 


            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return "";
            }
            //return retVal;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get all the product attribute values using family and product id
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="FamilyID">int</param>
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
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     int FamilyID;
        ///     int CatalogID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductsAttributesValues(ProductID,FamilyID,CatalogID);
        /// }
        /// </code>
        /// </example>

        //
        //public DataSet GetProductsAttributesValues(int ProductID, int FamilyID,int CatalogID)
        //{
        //    try
        //    {
        //        string sSQL = " SELECT TPS.PRODUCT_ID,TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME,TPS.STRING_VALUE AS ATTRIBUTE_VALUE  FROM TB_PROD_SPECS TPS,TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA ";
        //        sSQL = sSQL + " WHERE TPS.PRODUCT_ID =" + ProductID + " AND TA.ATTRIBUTE_ID = TPS.ATTRIBUTE_ID AND TA.ATTRIBUTE_ID IN(1,2,4)AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
        //        sSQL = sSQL + " UNION";
        //        sSQL = sSQL + " SELECT TPS.PRODUCT_ID,TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME,TPS.STRING_VALUE AS ATTRIBUTE_VALUE FROM TB_PROD_SPECS TPS,TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA ";
        //        sSQL = sSQL + " WHERE TPS.PRODUCT_ID = " + ProductID + " AND TA.ATTRIBUTE_ID = TPS.ATTRIBUTE_ID AND TA.ATTRIBUTE_ID <>3";
        //        sSQL = sSQL + " AND TA.ATTRIBUTE_TYPE =1 AND TA.PUBLISH2WEB=1 AND TA.ATTRIBUTE_ID IN(SELECT ATTRIBUTE_ID FROM TB_PROD_FAMILY_ATTR_LIST WHERE FAMILY_ID=" + FamilyID + ")";
        //        sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID; 
        //        sSQL = sSQL + " ORDER BY TPS.PRODUCT_ID,TA.ATTRIBUTE_ID";
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataSet();

        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>   
        /// This is used to get all the product attribute values
        /// </summary>
        /// <param name="ProductID">int</param>
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
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     int CatalogID;
        ///     DataSet oDS =new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductsAllAttributesValues(ProductID,CatalogID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ATTRIBUTE VALUES FOR ALL PRODUCTS ***/
        /********************************************************************************/
        public DataSet GetProductsAllAttributesValues(int ProductID,int CatalogID)
        {
            try
            {
                //string sSQL = " SELECT TPS.PRODUCT_ID,TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME,TPS.STRING_VALUE AS ATTRIBUTE_VALUE  FROM TB_PROD_SPECS TPS,TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA";
                //sSQL = sSQL + " WHERE TPS.PRODUCT_ID =" + ProductID + " AND TA.ATTRIBUTE_ID = TPS.ATTRIBUTE_ID AND TA.ATTRIBUTE_ID IN(1,2,4)";
                //sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
                //sSQL = sSQL + " UNION";
                //sSQL = sSQL + " SELECT TPS.PRODUCT_ID,TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME,TPS.STRING_VALUE AS ATTRIBUTE_VALUE   FROM TB_PROD_SPECS TPS,TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA";
                //sSQL = sSQL + " WHERE TPS.PRODUCT_ID = " + ProductID + " AND TA.ATTRIBUTE_ID = TPS.ATTRIBUTE_ID AND TA.ATTRIBUTE_ID <>3";
                //sSQL = sSQL + " AND TA.ATTRIBUTE_TYPE =1 AND TA.PUBLISH2WEB=1 AND TPS.STRING_VALUE <>''";
                //sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID ="+ CatalogID;
                //sSQL = sSQL + " ORDER BY TPS.PRODUCT_ID,TA.ATTRIBUTE_ID";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objProductDB.GetGenericDataDB(CatalogID.ToString(), ProductID.ToString(), "GET_PRODUCT_ATTRIBUTE_VALUES", ProductDB.ReturnType.RTDataSet);   

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }
        /*********************************** OLD CODE ***********************************/
                /// <summary>   
        /// This is used to get all the Products Specification details using attribute ID 
        /// </summary>
        /// <param name="productID">int</param>
        /// <param name="AttributesID">string</param>
        /// <returns>Dataset</returns>
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
        ///     Product oProd = new Product();
        ///     int productID;
        ///     string AttributesID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductSpecsValues(productID, AttributesID);
        /// }
        /// </code>
        /// </example>

        //
        //public DataSet GetProductSpecsValues(int productID, String AttributesID)
        //{

        //    try
        //    {
        //        string sSQL = "SELECT ATTRIBUTE_ID,STRING_VALUE AS ATTRIBUTE_VALUE FROM TB_PROD_SPECS";
        //        sSQL = sSQL + " WHERE ATTRIBUTE_ID IN( " + AttributesID + ")";
        //        sSQL = sSQL + " AND PRODUCT_ID = " + productID;
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>   
        /// This is used to get all the Products Specification Details
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
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     bool isValues;
        ///     int CatalogID
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductSpecsValues(ProductID,isValues,CatalogID);
        /// }
        /// </code>
        /// </example>

        //
        //public DataSet GetProductSpecsValues(int ProductID, bool isValues,int CatalogID)
        //{
        //    try
        //    {
        //        string sSQL = "";
        //        if (isValues)
        //        {
        //            sSQL = " SELECT TPS.STRING_VALUE AS ATTRIBUTE_VALUE, TA.ATTRIBUTE_NAME, TPS.ATTRIBUTE_ID ";
        //            sSQL = sSQL + " FROM TB_PROD_SPECS TPS, TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA ";
        //            sSQL = sSQL + " WHERE TPS.ATTRIBUTE_ID = TA.ATTRIBUTE_ID AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
        //            sSQL = sSQL + " AND TA.PUBLISH2WEB = 1 AND TA.ATTRIBUTE_ID IN (1,2,4) AND PRODUCT_ID = " + ProductID;

        //        }
        //        else
        //        {
        //            sSQL = "SELECT  TPS.STRING_VALUE AS ATTRIBUTE_VALUE, TA.ATTRIBUTE_NAME, TPS.ATTRIBUTE_ID  ";
        //            sSQL = sSQL + " FROM TB_PROD_SPECS TPS, TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE TPS.ATTRIBUTE_ID = TA.ATTRIBUTE_ID ";
        //            sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID ="+ CatalogID + " AND TA.PUBLISH2WEB = 1 AND PRODUCT_ID = " + ProductID;
        //        }
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE MAXIMUM AVAILABLE QUANTITY OF PRODUCT ***/
        /********************************************************************************/
        public int GetMaxAvailQty(int ProductID)
        {
            int retVal = 0;
            DataSet objds = new DataSet();
            
            try
            {
                //string sSQL = "SELECT QTY_AVAIL FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + ProductID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.CI(oHelper.GetValue("QTY_AVAIL"));
                objds=(DataSet)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_PRODUCT_INVENTORY", HelperDB.ReturnType.RTDataSet);  
                if( objds!=null && objds.Tables.Count>0 && objds.Tables[0].Rows.Count>0)
                    retVal = objHelperServices.CI(objds.Tables[0].Rows[0]["QTY_AVAIL"].ToString());
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
        /*** PURPOSE      : TO RETRIVE THE MINIMUM AVAILABLE QUANTITY OF PRODUCT ***/
        /********************************************************************************/
        public int GetMinOrdQty(int ProductID)
        {
            int retVal = 0;
            DataSet objds = new DataSet();
            try
            {
                objds = (DataSet)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_PRODUCT_INVENTORY", HelperDB.ReturnType.RTDataSet);
                if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
                    retVal = objHelperServices.CI(objds.Tables[0].Rows[0]["MIN_ORD_QTY"].ToString());

               // string sSQL = "SELECT MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + ProductID;
               // oHelper.SQLString = sSQL;
               // retVal = oHelper.CI(oHelper.GetValue("MIN_ORD_QTY"));
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
        /// This is used to get the Product Image
        /// </summary>
        /// <param name="ProductID">int</param>
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
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     int CatalogID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductImages(ProductID,CatalogID);
        /// }
        /// </code>
        /// </example>

        //
        //public DataSet GetProductImages(int ProductID,int CatalogID)
        //{
        //    try
        //    {
        //        string sSQL = " SELECT TPS.OBJECT_TYPE, TPS.STRING_VALUE,TPS.OBJECT_NAME,TA.ATTRIBUTE_NAME,";
        //        sSQL = sSQL + " TPS.ATTRIBUTE_ID FROM TB_PROD_SPECS TPS, TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA";
        //        sSQL = sSQL + " WHERE TPS.ATTRIBUTE_ID = TA.ATTRIBUTE_ID AND STRING_VALUE <>''";
        //        sSQL = sSQL + " AND TPS.OBJECT_TYPE IS NOT NULL AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
        //        sSQL = sSQL + " AND TA.PUBLISH2WEB = 1 AND PRODUCT_ID = " + ProductID;
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>   
        /// This is used to get the Product Base Price 
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <returns>Double</returns>
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
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     Double price;
        ///     ...
        ///     price = oProd.GetProductBasePrice(ProductID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DETAILS FOR PRODUCTS BASIC PRICE ***/
        /********************************************************************************/
        public Double GetProductBasePrice(int ProductID)
        {
            double price = 0.0;
            DataSet objds = new DataSet();
            try
            {
                //string sSQL = " SELECT ISNULL(NUMERIC_VALUE,0) AS BASE_PRICE FROM TB_PROD_SPECS";
                //sSQL = sSQL + " WHERE PRODUCT_ID = " + ProductID + " AND ATTRIBUTE_ID =4";
                //OleDbCommand oCmd = new OleDbCommand(sSQL, oCon.GetConnection());
                //price = oHelper.CD(oCmd.ExecuteScalar());

                objds = (DataSet) objProductDB.GetGenericDataDB(ProductID.ToString(),"GET_PRODUCT_BASIC_PRICE", ProductDB.ReturnType.RTDataSet);

                if (objds != null && objds.Tables.Count > 0 && objds.Tables[0].Rows.Count > 0)
                    price = Convert.ToDouble(objds.Tables[0].Rows[0]["BASE_PRICE"].ToString());

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                //oErrHand.CreateLog();
                price = -1;
            }
            return price;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get the Products Base Price 
        /// </summary>
        /// <param name="ProductID">int</param>
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
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductBasePriceValues(ProductID);
        /// }   
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE VALUES FOR PRODUCT BASE PRICE ***/
        /********************************************************************************/
        public DataSet GetProductBasePriceValues(int ProductID)
        {
            try
            {
                //string sSQL = " SELECT ATTRIBUTE_ID,NUMERIC_VALUE AS ATTRIBUTE_VALUE FROM TB_PROD_SPECS";
                //sSQL = sSQL + " WHERE PRODUCT_ID =" + ProductID + " AND ATTRIBUTE_ID = 3 ";

                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objProductDB.GetGenericDataDB(ProductID.ToString(), "GET_PRODUCT_BASIC_PRICE_VALUE", ProductDB.ReturnType.RTDataSet);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                //oErrHand.CreateLog();
                return null;
            }
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get the Products Price Values based on Product ID and Attribute ID(string).
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="AttributesID">string</param>
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
        ///     Product oProd = new Product();
        ///     String AttributesID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductPriceValues(oHelper.CI(rProd["PRODUCT_ID"]), AttributesID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ALL ATTRIBUTE VALUES FOR PRODUCT PRICE DETAILS ***/
        /********************************************************************************/
        public DataSet GetProductPriceValues(int ProductID, String AttributesID)
        {
            try
            {
                //string sSQL = "SELECT ATTRIBUTE_ID,NUMERIC_VALUE AS ATTRIBUTE_VALUE FROM TB_PROD_SPECS";
                //sSQL = sSQL + " WHERE ATTRIBUTE_ID IN( " + AttributesID + ")";
                //sSQL = sSQL + " AND PRODUCT_ID = " + ProductID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objProductDB.GetGenericDataDB("",AttributesID,ProductID.ToString(), "GET_PRODUCT_PRICE_ATTRIBUTE", ProductDB.ReturnType.RTDataSet);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                //oErrHand.CreateLog();
                return null;
            }
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get the Products Price Values based on Product ID and Attribute ID
        /// </summary>
        /// <param name="ProductID">int</param>
        /// <param name="AttributeID">int</param>
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
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     int BGPriceID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProductPriceValue(ProductID, BGPriceID);
        ///     /// Where BGPriceID is the Buyer Group Price Attribute ID
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ATTRIBUTE VALUE FOR PRODUCT PRICE  ***/
        /********************************************************************************/
        public DataSet GetProductPriceValue(int ProductID, int AttributeID)
        {

            try
            {
                //string sSQL = " SELECT TA.ATTRIBUTE_NAME,TPS.NUMERIC_VALUE AS ATTRIBUTE_VALUE FROM TB_PROD_SPECS TPS,TB_ATTRIBUTE TA";
                //sSQL = sSQL + " WHERE TPS.ATTRIBUTE_ID = TA.ATTRIBUTE_ID AND TPS.ATTRIBUTE_ID =" + AttributeID + " AND TPS.PRODUCT_ID =" + ProductID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objProductDB.GetGenericDataDB("", AttributeID.ToString(), ProductID.ToString(), "GET_PRODUCT_PRICE_ATTRIBUTE_SINGLE", ProductDB.ReturnType.RTDataSet);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                //oErrHand.CreateLog();
                return null;
            }
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get the price value of the Buyer Group for Every User
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
        ///     Product oProd = new Product();
        ///     int UserID;
        ///     int retPriceID;
        ///     ...
        ///     retPriceID = oProd.GetBuyerGroupPrice(UserID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRICE DETAILS FOR BUYER GROUP ***/
        /********************************************************************************/
        public int GetBuyerGroupPrice(int UserID)
        {
            int retPriceID = 0;
            string tempstr = "0";
            try
            {
                string sSQL = "";
                if (UserID > 0)
                {
                   // sSQL = " SELECT TBG.PRICE_ATTRIBUTE_ID FROM TBWC_COMPANY TC,TBWC_COMPANY_BUYERS TCB,TBWC_BUYER_GROUP TBG";
                   // sSQL = sSQL + " WHERE TC.COMPANY_ID = TCB.COMPANY_ID AND TBG.BUYER_GROUP = TC.BUYER_GROUP";
                   // sSQL = sSQL + " AND TCB.USER_ID = " + UserID;
                    tempstr = (string)objProductDB.GetGenericDataDB("", UserID.ToString(), "1", "GET_PRODUCT_PRICE_ATTRIBUTE_ID", ProductDB.ReturnType.RTString);    
                }
                else if (UserID == 0)
                {
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
        /// This is used to get the Product ID
        /// </summary>
        /// <param name="CatalogItemNo">string</param>
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
        ///     Product oProd = new Product();
        ///     string CatalogItemNo;
        ///     int ProductID;
        ///     ...
        ///     ProductID = oProd.GetProductID(CatalogItemNo);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PRODUCT ID ***/
        /********************************************************************************/
        public int GetProductID(string CatalogItemNo)
        {
            int ProductID = 0;
            string tempstr = "0";
            try
            {
                //string sSQL = " SELECT PRODUCT_ID FROM TB_PRODUCT";
                //sSQL = sSQL + " WHERE CATALOG_ITEM_NO = N'" + CatalogItemNo + "'";
                //OleDbCommand oCmd = new OleDbCommand(sSQL, oCon.GetConnection());
                //ProductID = oHelper.CI(oCmd.ExecuteScalar());
                tempstr = (string)objProductDB.GetGenericDataDB(CatalogItemNo, "GET_PRODUCT_ProductID", ProductDB.ReturnType.RTString);    
                if( tempstr!=null && tempstr!="")
                    ProductID=Convert.ToInt32( tempstr); 


            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return ProductID;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get the Document Details based on Product ID
        /// <param name="productID">int</param>
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
        ///     Product oProd = new Product();
        ///     int productID;
        ///     int CatalogID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetDocuments(productID,CatalogID);
        /// }
        /// </code>
        /// </example>

        //
        //public DataSet GetDocuments(int productID,int CatalogID)
        //{
        //    try
        //    {
        //        string sSQL = " SELECT TPS.ATTRIBUTE_ID AS ATTRIBUTEID,TA.ATTRIBUTE_NAME AS ATTRIBUTENAME,TPS.OBJECT_NAME AS IMAGENAME,STRING_VALUE AS IMAGEFILE,TPS.OBJECT_TYPE AS IMAGETYPE ";
        //        sSQL = sSQL + " FROM TB_PROD_SPECS TPS, TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA";
        //        sSQL = sSQL + " WHERE TA.ATTRIBUTE_ID = TPS.ATTRIBUTE_ID";
        //        sSQL = sSQL + " AND PRODUCT_ID = " + productID + " AND TPS.STRING_VALUE <>''AND TPS.OBJECT_TYPE ='pdf' ";
        //        sSQL = sSQL + " AND TA.ATTRIBUTE_ID IN(SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_TYPE =3 AND PUBLISH2WEB =1 )";
        //        sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
        //        sSQL = sSQL + " ORDER BY TA.ATTRIBUTE_NAME";
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>   
        /// This is used to get the Product Count based on family ID 
        /// </summary>
        /// <param name="familyID">int</param>
        /// <param name="CatalogID">int</param>
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
        ///     Product oProd = new Product();
        ///     int familyID;
        ///     int CatalogID;
        ///     int productCount;
        ///     ...
        ///     productCount = oProd.GetProductCount(familyID,CatalogID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT COUNT  ***/
        /********************************************************************************/
        public int GetProductCount(int familyID,int CatalogID)
        {
            int productCount = 0;
            string tempstr = "0";
            try
            {
                //string sSQL = " SELECT COUNT(TP.PRODUCT_ID) AS PRODUCTCOUNT";
                //sSQL = sSQL + " FROM TB_PRODUCT TP, TB_PROD_FAMILY TPF,TBWC_INVENTORY TI,TB_CATALOG_PRODUCT TCP";
                //sSQL = sSQL + " WHERE  TP.PRODUCT_ID = TPF.PRODUCT_ID AND TI.PRODUCT_ID = TP.PRODUCT_ID";
                //sSQL = sSQL + " AND  TPF.FAMILY_ID = " + familyID;
                //sSQL = sSQL + " AND TP.PRODUCT_ID = TCP.PRODUCT_ID AND TCP.CATALOG_ID =" + CatalogID;
                //OleDbCommand oCmd = new OleDbCommand(sSQL, oCon.GetConnection());
                //oHelper.SQLString = sSQL;
                //productCount =oHelper.CI(oHelper.GetValue("PRODUCTCOUNT").ToString());
                //productCount = oHelper.CI(oCmd.ExecuteScalar());

                tempstr = (string)objProductDB.GetGenericDataDB(CatalogID.ToString(), familyID.ToString(), "GET_PRODUCT_PRODUCT_COUNT", ProductDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    productCount = Convert.ToInt32(tempstr); 
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return productCount;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get all the Product Details
        /// </summary>
        /// <param name="CateID">string</param>
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
        ///     Product oProd = new Product();
        ///     string CateID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetProducts(CateID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE DETAILS FOR ALL PRODUCTS  ***/
        /********************************************************************************/
        public DataSet GetProducts(string CateID)
        {
            try
            {
                //string sSQL = "SELECT product_id FROM tb_prod_family WHERE family_id in( ";
                //sSQL = sSQL + "SELECT DISTINCT(family_id) FROM tb_family WHERE category_id ='" + CateID + "')";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objProductDB.GetGenericDataDB(CateID.ToString(), "GET_PRODUCT_CATEGORY_PRODUCT", ProductDB.ReturnType.RTDataSet);
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
        /*** PURPOSE      : TO RETRIVE PRODUCT DETAILS BASED ON FAMILY ID AND CATALOG ID ***/
        /********************************************************************************/
        public DataSet GetProducts(int familyId,int catalogID)
        {
            try
            {
                //string sSQL = "SELECT TPF.PRODUCT_ID FROM TB_PROD_FAMILY TPF,TB_CATALOG_PRODUCT TCP WHERE TPF.FAMILY_ID=" + familyId +" AND TPF.PRODUCT_ID IN(SELECT PRODUCT_ID FROM TBWC_INVENTORY WHERE PRODUCT_STATUS='AVAILABLE') AND TCP.PRODUCT_ID=TPF.PRODUCT_ID AND TCP.CATALOG_ID="+catalogID;                
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objProductDB.GetGenericDataDB(catalogID.ToString(), familyId.ToString()  , "GET_FAMILY_PRODUCTS", ProductDB.ReturnType.RTDataSet);
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
        /*** PURPOSE      : TO RETRIVE PRODUCT SPECIFICTION DETAILS  ***/
        /********************************************************************************/
        public DataSet GetProductsSpecs(string product_id)
        {
            
            try
            {
                //string cmdd = "SELECT STRING_VALUE,PRODUCT_ID,ATTRIBUTE_ID,NUMERIC_VALUE,OBJECT_TYPE,[OBJECT_NAME],(SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = PS.ATTRIBUTE_ID) AS ATTRIBUTE_TYPE FROM TB_PROD_SPECS PS  WHERE PRODUCT_ID=" + product_id ;
                //oHelper.SQLString = cmdd;
                //return oHelper.GetDataSet();
                return (DataSet)objProductDB.GetGenericDataDB(product_id.ToString(), "GET_PRODUCT_SPEC", ProductDB.ReturnType.RTDataSet);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
            
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get the Category Name using Product ID 
        /// </summary>
        /// <param name="isHirearchy">int</param> 
        /// <param name="ProductID">bool</param>
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
        ///     Product oProd = new Product();
        ///     int ProductID;
        ///     bool isHirearchy;
        ///     string retVal;
        ///     ...
        ///     retVal = oProd.GetCategoryName(ProductID,isHirearchy);
        /// }
        /// </code>
        ///</example>  
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT CATEGORY NAME ***/
        /********************************************************************************/
        public string GetCategoryName(int ProductID, bool isHirearchy)
        {
            string retVal="";
            DataSet objDs=new  DataSet();
            try
            {
                //string sSQL = "SELECT CATEGORY_ID FROM TB_PRODUCT WHERE PRODUCT_ID = " + ProductID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.GetValue("CATEGORY_ID");
                objDs = (DataSet)objProductDB.GetGenericDataDB(ProductID.ToString(), "GET_PRODUCT_CATEGORY_ID", ProductDB.ReturnType.RTDataSet);
                if (objDs != null)
                    retVal = objDs.Tables[0].Rows[0]["CATEGORY_ID"].ToString();

                if (isHirearchy)
                {
                    retVal = GetParentCategory(retVal);
                }
                else
                {
                    //oHelper.SQLString = " SELECT CATEGORY_NAME FROM TB_CATEGORY WHERE CATEGORY_ID ='" + retVal + "'";
                    //retVal = oHelper.GetValue("CATEGORY_NAME");
                    objDs = (DataSet)objProductDB.GetGenericDataDB(retVal.ToString(), "GET_PRODUCT_CATEGORY_NAME", ProductDB.ReturnType.RTDataSet);
                    if (objDs != null)
                        retVal = objDs.Tables[0].Rows[0]["CATEGORY_NAME"].ToString();
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retVal = null;
            }

            return retVal;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get the Parent Category Name
        /// </summary>
        /// <param name="CategoryID">string</param> 
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
        ///     Product oProd = new Product();
        ///     string CategoryID;
        ///     string _ParentCategory;
        ///     ...
        ///     _ParentCategory = oProd.GetParentCategory(CategoryID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PARENT CATEGORY NAME ***/
        /********************************************************************************/
        public string GetParentCategory(string CategoryID)
        {
            DataSet objDs = new DataSet();
            try
            {
                string _ParentCategory;
                //string sSQL = "SELECT CATEGORY_NAME, PARENT_CATEGORY FROM TB_CATEGORY WHERE CATEGORY_ID = '" + CategoryID + "' ";
                //oHelper.SQLString = sSQL;
                //_ParentCategory = oHelper.GetValue("PARENT_CATEGORY");
                //_ParentCategory = objDs.Tables[0].Rows[0]["PARENT_CATEGORY"].ToString();
                //if (_ParentCategory != "0")
                //{
                //    retVal = ">>" + oHelper.GetValue("CATEGORY_NAME") + retVal;
                //    sCategoryNames = retVal;
                //    GetParentCategory(_ParentCategory);
                //}
                //retVal = oHelper.GetValue("CATEGORY_NAME") + sCategoryNames;
                objDs = (DataSet)objProductDB.GetGenericDataDB(CategoryID.ToString(), "GET_PRODUCT_PARENT_CATEGORY_NAME", ProductDB.ReturnType.RTDataSet);
                if (objDs != null)
                {
                    _ParentCategory = objDs.Tables[0].Rows[0]["PARENT_CATEGORY"].ToString();
                    if (_ParentCategory != "0")
                    {
                        retVal = ">>" + objDs.Tables[0].Rows[0]["CATEGORY_NAME"].ToString() + retVal;
                        sCategoryNames = retVal;
                        GetParentCategory(_ParentCategory);
                    }

                    retVal = objDs.Tables[0].Rows[0]["CATEGORY_NAME"].ToString() + sCategoryNames;
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retVal = null;
            }
            return retVal;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get the attribute names
        /// </summary>
        /// <param name="CatalogID">int</param>
        ///<returns>DataTable</returns> 
        ///<example>
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
        ///     int CatalogID;
        ///     Product oProd = new Product();
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oProd.GetAttributesName(CatalogID);
        /// }
        /// </code>
        ///</example> 

        
        //public DataTable GetAttributesName(int CatalogID)
        //{
        //    try
        //    {
        //        string sSQL = " SELECT TA.ATTRIBUTE_ID AS ATTRIBUTEID,TA.ATTRIBUTE_NAME AS ATTRIBUTENAME,TA.ATTRIBUTE_TYPE AS ATTRIBUTETYPE ";
        //        sSQL = sSQL + " FROM TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE TA.ATTRIBUTE_ID IN(1,2,4)";
        //        sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
        //        sSQL = sSQL + " UNION";
        //        sSQL = sSQL + " SELECT TA.ATTRIBUTE_ID AS ATTRIBUTEID,TA.ATTRIBUTE_NAME AS ATTRIBUTENAME,TA.ATTRIBUTE_TYPE AS ATTRIBUTETYPE FROM TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE TA.ATTRIBUTE_TYPE IN(1,2)";
        //        sSQL = sSQL + " AND TA.ATTRIBUTE_ID <> 3 AND TA.PUBLISH2WEB =1";
        //        sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID ="+ CatalogID ;
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>   
        /// This is used to get the attribute name using fl ID
        /// </summary>
        /// <param name="FamilyID">int</param> 
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
        ///     Product oProd = new Product();
        ///     int FamilyID;
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oProd.GetAttributesName(FamilyID);
        /// }
        /// </code>
        /// </example> 

        //
        //public DataTable GetAttributesName(int FamilyID,int CatalogID)
        //{
        //    try
        //    {
        //        string sSQL = " SELECT TA.ATTRIBUTE_ID AS ATTRIBUTEID,TA.ATTRIBUTE_NAME AS ATTRIBUTENAME,TA.ATTRIBUTE_TYPE AS ATTRIBUTETYPE ";
        //        sSQL = sSQL + " FROM TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE TA.ATTRIBUTE_ID IN(1,2,4)";
        //        sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
        //        sSQL = sSQL + " UNION";
        //        sSQL = sSQL + " SELECT TA.ATTRIBUTE_ID AS ATTRIBUTEID,TA.ATTRIBUTE_NAME AS ATTRIBUTENAME,TA.ATTRIBUTE_TYPE AS ATTRIBUTETYPE FROM TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE TA.ATTRIBUTE_TYPE IN(1,2)";
        //        sSQL = sSQL + " AND TA.ATTRIBUTE_ID <> 3 AND TA.PUBLISH2WEB =1";
        //        sSQL = sSQL + " AND TA.ATTRIBUTE_ID IN (SELECT DISTINCT ATTRIBUTE_ID FROM TB_PROD_FAMILY_ATTR_LIST WHERE FAMILY_ID =" + FamilyID + ")";
        //        sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;

        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>   
        /// This is used to get the Price Attribute Name based on the Price ID
        /// </summary>
        /// <param name="BGPriceID">int</param> 
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
        ///     Product oProd = new Product();
        ///     int BGPriceID;
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oProd.GetBGAttributesName(BGPriceID);
        /// }
        /// </code>
        /// </example> 

        //
        //public DataTable GetBGAttributesName(int BGPriceID)
        //{
        //    try
        //    {
        //        string sSQL = " SELECT ATTRIBUTE_ID AS ATTRIBUTEID,ATTRIBUTE_NAME AS ATTRIBUTENAME,ATTRIBUTE_TYPE AS ATTRIBUTETYPE";
        //        sSQL = sSQL + " FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + BGPriceID;
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>   
        /// This is used to get core attributes(itemno,suppliername,mfg,price)names
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
        ///     Product oProd = new Product();
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetCoreAttributesName();
        /// }
        /// </code>
        /// </example> 

        //
        //public DataSet GetCoreAttributesName()
        //{
        //    try
        //    {
        //        oHelper.SQLString = "SELECT ATTRIBUTE_NAME FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID IN(1,2,4)AND PUBLISH2WEB=1";
        //        return oHelper.GetDataSet();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>   
        /// This is used to get all the Supplier Name Index
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
        ///     Product oProd = new Product();
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oProd.GetSupplierNameIndex();
        /// }
        /// </code>
        /// </example> 
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE DISTINCT SUPPLIER NAME INDEX ***/
        /********************************************************************************/
        public DataSet GetSupplierNameIndex()
        {
            DataSet dsRet = new DataSet();
            try
            {
                //string sSQL = " SELECT DISTINCT(SUBSTRING(STRING_VALUE,1,1))AS SUPPLIERNAMEINDEX  FROM TB_PROD_SPECS WHERE STRING_VALUE <>''AND ATTRIBUTE_ID=4";
                //sSQL = sSQL + " ORDER BY SUBSTRING(STRING_VALUE,1,1)";
                //oHelper.SQLString = sSQL;
                //dsRet = oHelper.GetDataSet();
                return (DataSet)objProductDB.GetGenericDataDB("", "GET_PRODUCT_SUPPLIER_NAME_INDEX", ProductDB.ReturnType.RTDataSet);   
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                dsRet = null;
            }
            return dsRet;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>   
        /// This is used to get all the Suppliers List
        /// </summary>
        /// <param name="NameIndex">string</param> 
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
        ///     Product oProd = new Product();
        ///     string NameIndex;
        ///     DataSet oDs= new DataSet();
        ///     ...
        ///     oDs = oProd.GetSupplierrList(NameIndex);
        /// }
        /// </code>
        /// </example> 
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE LIST OF SUPPLIERS ***/
        /********************************************************************************/

        public DataSet GetSupplierrList(string NameIndex)
        {
            DataSet dsRet = new DataSet();
            try
            {
                //string sSQL = " SELECT DISTINCT STRING_VALUE  FROM TB_PROD_SPECS WHERE STRING_VALUE LIKE '" + NameIndex + "%'";
                //sSQL = sSQL + " ORDER BY STRING_VALUE";
                //oHelper.SQLString = sSQL;
                //dsRet = oHelper.GetDataSet();
                return (DataSet)objProductDB.GetGenericDataDB(NameIndex, "GET_PRODUCT_SUPPLIER_LIST", ProductDB.ReturnType.RTDataSet);   
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                dsRet = null;
            }
            return dsRet;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// Get the Restricted product list using ZipCode
        /// </summary>
        /// <param name="ZipCode">string</param>
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
        ///     Product oProd = new Product();
        ///     string ZipCode;
        ///     DataSet oDs= new DataSet();
        ///     ...
        ///     oDs = oProd.GetRestProdDS(ZipCode);
        /// }
        /// </code>
        /// </example>
        
        //
        //public DataSet GetRestrictedProdDS(string ZipCode)
        //{
        //    DataSet oDS = new DataSet();
        //    try
        //    {
        //        oHelper.SQLString = "SELECT * FROM TBWC_PROD_RESTRICT WHERE ZIPCODE='" + ZipCode + "'";
        //        oDS = oHelper.GetDataSet();
        //        if (oDS.Tables[0].Rows.Count == 0)
        //        {
        //            oDS = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        oErrHand.CreateLog();
        //        oDS = null;
        //    }
        //    return oDS;
        //}

        /// <summary>
        /// To get productIDs
        /// </summary>
        /// <param name="ItemCode">string</param>
        /// <returns>Dataset</returns>
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
        ///     Product oProd = new Product();
        ///     string[] ItemCode;
        ///     DataSet oDs= new DataSet();
        ///     ...
        ///     oDs = oProd.GetProdIDDS(ItemCode);
        ///     ...
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT ID DETAILS ***/
        /********************************************************************************/
        public DataSet GetProdIDDS(string[] ItemCode)
        {
            DataSet oProdListDS = new DataSet();
            string ItemList = "";
            for (int l = 0; l < ItemCode.Length; l++)
            {
                if (ItemCode.Length - 1 == l)
                {
                    ItemList = ItemList + "'" + ItemCode[l].ToString() + "'";
                }
                else
                {
                    ItemList = ItemList + "'" + ItemCode[l].ToString() + "',";
                }
            }
            //oHelper.SQLString = "SELECT DISTINCT (SELECT TOP 1 PRODUCT_ID FROM TB_PROD_SPECS WHERE ATTRIBUTE_ID=1 AND STRING_VALUE=PS.STRING_VALUE) PRODUCT_ID,STRING_VALUE FROM TB_PROD_SPECS PS WHERE STRING_VALUE IN (" + ItemList + ") AND ATTRIBUTE_ID=1";
            //oProdListDS = oHelper.GetDataSet();
            //return oProdListDS;
            return (DataSet)objProductDB.GetGenericDataDB(ItemList, "GET_PRODUCT_IDS_WITH_STRING_VALUE", ProductDB.ReturnType.RTDataSet);   

        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO GET PRODUCT AVAILABILITY DETAILS ***/
        /********************************************************************************/
        public int GetProductAvailability(int ProdID)
        {
            int retValue = 0;
            string Avlbty = "";
            try
            {
                //oHelper.SQLString = "SELECT PRODUCT_STATUS FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + ProdID;
                //Avlbty = oHelper.GetValue("PRODUCT_STATUS");
                Avlbty = (string)objProductDB.GetGenericDataDB(ProdID.ToString(), "GET_PRODUCT_AVALILABILITY", ProductDB.ReturnType.RTString);   
                if (Avlbty == "AVAILABLE")
                    retValue = 1;
                else if (Avlbty == "N/A" || Avlbty == "DISCONTINUED")
                {
                    retValue = 0;
                }
            }
            catch(Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                retValue = 0;
                return retValue;
            }
            return retValue;
        }


        public string GetProductSortKeyCode(string Product_Code)
        {
            string strSortKey = "";
            DataTable dsSortKey = new DataTable();
            try
            {
                dsSortKey = (DataTable)objProductDB.GetGenericDataDB(Product_Code, "GET_PRODUCT_SORTKEY_CODE_METATAG", ProductDB.ReturnType.RTTable);

                if (dsSortKey != null)
                {
                    if (dsSortKey.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dsSortKey.Rows.Count - 1; i++)
                        {
                            strSortKey = strSortKey + "," + dsSortKey.Rows[i]["SORTKEY"].ToString().Trim();
                        }
                    }
                }



            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            return strSortKey;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE FAMILY ID DETAILS ***/
        /********************************************************************************/
        public int GetFamilyID(int ProductID)
        {
            int retVal = 0;
            try
            {
                //string sSQL = "SELECT DISTINCT FAMILY_ID FROM TB_PROD_FAMILY WHERE PRODUCT_ID =" + ProductID;
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.CI(oHelper.GetValue("FAMILY_ID").ToString());
                string tempstr = (string)objProductDB.GetGenericDataDB(ProductID.ToString(), "GET_PRODUCT_FAMILY_ID", ProductDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    retVal = objHelperServices.CI(tempstr);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return retVal;
        }

        #endregion
    }
    /*********************************** J TECH CODE ***********************************/
}

