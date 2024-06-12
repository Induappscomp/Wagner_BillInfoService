using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    /// <summary>
    /// This is used to get the Category Details
    /// </summary>
    /// <remarks>
    /// Used to get Category ID,Name,List of sub Categories,Parent category,...
    /// </remarks>
    /// <example>
    /// Category oCat=new Category();
    /// </example>
    
    public class CategoryServices 
    {
        /*********************************** DECLARATION ***********************************/
        HelperDB objHelperDB = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        CategoryDB objCategoryDB = new CategoryDB();
        /*********************************** DECLARATION ***********************************/
        /// <summary>
        /// This is used to get the Category Details
        /// </summary>
        /*********************************** CONSTRUCTOR ***********************************/
        public CategoryServices()
        {

        }
        /*********************************** CONSTRUCTOR ***********************************/

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Catalog Category list details.
        /// </summary>
        /// <param name="catalogID">int</param>
        /// <returns>DataTable</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.IO;
        ///using System.Web;
        ///using System.Data;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Category oCat=new Category();
        ///     int catalogID;
        ///     DataTable dtCat = new DataTable();
        ///     ...
        ///     dtCat = oCat.GetCatalogCategoryList(catalogID);
        /// }
        ///  </code>
        /// </example>
        //
        //public DataTable GetCatalogCategoryList(int catalogID)
        //{
        //    DataTable dtCat = new DataTable();
        //    try
        //    {
        //        string sSQL = " SELECT TC.CATEGORY_ID AS CATEGORY_ID,";
        //        sSQL = sSQL + " TC.CATEGORY_NAME AS CATEGORY_NAME,";
        //        sSQL = sSQL + " TC.PARENT_CATEGORY AS PARENT_CATEGORY";
        //        sSQL = sSQL + " FROM TB_CATALOG_SECTIONS TCS, TB_CATEGORY TC";
        //        sSQL = sSQL + " WHERE TC.CATEGORY_ID = TCS.CATEGORY_ID";
        //        sSQL = sSQL + " AND CATALOG_ID = " + catalogID;
        //        oHelper.SQLString = sSQL;
        //        dtCat = oHelper.GetDataTable();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //    }
        //    return dtCat;
        //}
        /// <summary>
        /// This is used to get the Category List details
        /// </summary>
        /// <param name="catalogID">int</param>
        /// <param name="searchText">string</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.IO;
        ///using System.Web;
        ///using System.Data;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Category oCat=new Category();
        ///     int catalogID;
        ///     string searchText;
        ///     DataSet oDs = new DataSet();
        ///     ...
        ///     oDs = oCat.GetCategoryList(catalogID,searchText);
        /// }
        ///  </code>
        /// </example>
        //
        //public DataSet GetCategoryList(int catalogID, string searchText)
        //{
        //    try
        //    {
        //        string sSQL = " SELECT TC.CATEGORY_ID, TC.CATEGORY_NAME, TC.PARENT_CATEGORY, TCS.CATALOG_ID";
        //        sSQL = sSQL + " FROM TB_CATALOG_SECTIONS TCS, TB_CATEGORY TC";
        //        sSQL = sSQL + " WHERE TC.CATEGORY_ID = TCS.CATEGORY_ID AND CATALOG_ID = " + catalogID;
        //        sSQL = sSQL + " AND TC.CATEGORY_NAME LIKE N'%" + searchText + "%'";
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataSet("ct");
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        // oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>
        /// This is used to get the Category Name Details
        /// </summary>
        /// <param name="categoryID">string</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.IO;
        ///using System.Web;
        ///using System.Data;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Category oCat=new Category();
        ///     string catalogID;   
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oCat.GetCategoryName(categoryID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/


        /*********************************************************************************/
        /******* ORGANIZATION : J TECH *******/
        /******* PURPOSE      : TO RETRIVE CATEGORY DETAILS BASED ON CATEGORY ID *******/
        /*********************************************************************************/
        public DataSet GetCategoryName(string categoryID)
        {
            DataSet tmpds = new DataSet();
            try
            {
                //string sSQL = " SELECT * FROM TB_CATEGORY";
                //sSQL = sSQL + " WHERE CATEGORY_ID = N'" + categoryID + "'";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet("CategoryName");
                tmpds = (DataSet)objCategoryDB.GetGenericDataDB(categoryID, "GET_CATEGORYDETAILS", CategoryDB.ReturnType.RTDataSet);
                if (tmpds != null)
                    tmpds.Tables[0].TableName = "CategoryName"; 
                else
                    tmpds=null;
  
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
            return tmpds;
        }
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to check that the Category is Parent or not
        /// </summary>
        /// <param name="CatId">string</param>
        /// <returns>DateSet</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.IO;
        ///using System.Web;
        ///using System.Data;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Category oCat=new Category();
        ///     string CatId;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oCat.CheckParent(CatId);
        /// }
        /// </code>
        /// </example>
        //
        //public DataSet CheckParent(string CatId)
        //{
        //    try
        //    {
        //        string sSQL;
        //        sSQL = " SELECT PARENT_CATEGORY FROM TB_CATEGORY";
        //        sSQL = sSQL + " WHERE CATEGORY_ID = N'" + CatId + "'";
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
        /// This is used to get the Parent Categories
        /// </summary>
        /// <param name="catalogID">int</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.IO;
        ///using System.Web;
        ///using System.Data;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Category oCat=new Category();
        ///     int catalogID;
        ///     DataSet oDs = new DataSet();
        ///     ...
        ///     oDs = oCat.GetRootCategories(catalogID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /******* ORGANIZATION : J TECH *******/
        /******* PURPOSE      : TO RETRIVE THE ROOT CATEGORY DETAILS BASED ON CATALOGID  *******/
        /*********************************************************************************/
        public DataSet GetRootCategories(int catalogID)
        {
            try
            {
                //string sSQL = " SELECT TC.CATEGORY_ID,TC.CATEGORY_NAME";
                //sSQL = sSQL + " FROM TB_CATEGORY TC,TB_CATALOG_SECTIONS TCS";
                //sSQL = sSQL + " WHERE TC.CATEGORY_ID =TCS.CATEGORY_ID AND TC.PARENT_CATEGORY ='0' ";
                //sSQL = sSQL + " AND TC.CATEGORY_NAME != 'DEFAULT CATEGORY' AND TC.CATEGORY_NAME != 'General Category' ";
                //sSQL = sSQL + " AND TCS.CATALOG_ID = " + catalogID;
                //sSQL = sSQL + " ORDER BY CATEGORY_NAME";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objCategoryDB.GetGenericDataDB(catalogID.ToString(), "GET_ROOT_CATEGORYDETAILS", CategoryDB.ReturnType.RTDataSet);
                
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
        /// This is used to get the sub Category based on Parent Category.
        /// </summary>
        /// <param name="parentCategoryID">string</param>
        /// <param name="CatalogID">int</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.IO;
        ///using System.Web;
        ///using System.Data;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Category oCat=new Category();
        ///     string parentCategoryID;
        ///     DataSet oSubCategoriesDS = new DataSet();
        ///     ...
        ///     oSubCategoriesDS = oCat.GetSubCategories(parentCategoryID,CatalogID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /******* ORGANIZATION : J TECH *******/
        /******* PURPOSE      : TO RETRIVE THE SUB CATEGORY DETAILS FROM RELATED DATABASE TABLES *******/
        /*********************************************************************************/
        //public DataSet GetSubCategories(string parentCategoryID, int CatalogID)
        //{
        //    try
        //    {
        //        //string sSQL = " SELECT * FROM TB_CATEGORY TC,TB_CATALOG_SECTIONS TCS";
        //        //sSQL = sSQL + " WHERE TC.PARENT_CATEGORY =N'" + parentCategoryID + "'";
        //        //sSQL = sSQL + " AND ISNULL(TC.CUSTOM_NUM_FIELD3,0)<> 3 AND TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID =" + CatalogID;
        //        //sSQL = sSQL + " ORDER BY CATEGORY_NAME";
        //        //oHelper.SQLString = sSQL;
        //        //return oHelper.GetDataSet();
        //        return (DataSet)objCategoryDB.GetGenericDataDB(CatalogID.ToString(), parentCategoryID, "GET_SUB_CATEGORYS", CategoryDB.ReturnType.RTDataSet);                
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        return null;
        //    }
        //}


        /*********************************************************************************/
        /******* ORGANIZATION : J TECH *******/
        /******* PURPOSE      : TO RETRIVE THE PRODUCT DETAILS OF SUB CATEGORIES  *******/
        /*********************************************************************************/
        
        //public DataSet GetSubCategories_product(string parentCategoryID, int CatalogID)
        //{
        //    try
        //    {
        //        //string sSQL = " SELECT * FROM TB_CATEGORY TC,TB_CATALOG_SECTIONS TCS";
        //        //sSQL = sSQL + " WHERE TC.PARENT_CATEGORY =(SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY=N'" + parentCategoryID + "')";
        //        //sSQL = sSQL + " AND ISNULL(TC.CUSTOM_NUM_FIELD3,0)<> 3 AND TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID =" + CatalogID;
        //        //sSQL = sSQL + " AND TC.CUSTOM_NUM_FIELD3 IN(1,3) ORDER BY CATEGORY_NAME";
        //        //oHelper.SQLString = sSQL;
        //        //return oHelper.GetDataSet();
        //        return (DataSet)objCategoryDB.GetGenericDataDB(CatalogID.ToString(), parentCategoryID, "GET_SUB_CATEGORYS_PRODUCT", CategoryDB.ReturnType.RTDataSet);                
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        return null;
        //    }
        //}

        /*********************************** OLD CODE ***********************************/
        //
        //public DataSet GetSubCategoriesBrand(string parentCategoryID, int CatalogID)
        //{
        //    try
        //    {
        //        string sSQL = " SELECT * FROM TB_CATEGORY TC,TB_CATALOG_SECTIONS TCS";
        //        sSQL = sSQL + " WHERE TC.PARENT_CATEGORY =N'" + parentCategoryID + "'";
        //        sSQL = sSQL + " AND ISNULL(TC.CUSTOM_NUM_FIELD3,0)<> 3 AND TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID =" + CatalogID;
        //        sSQL = sSQL + " ORDER BY CATEGORY_NAME";
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


        //
        //public DataSet GetWESBrand(string parentCategoryID, int CatalogID)
        //{
        //    try
        //    {
        //        //string sSQL = " SELECT DISTINCT SUBCATID_L2 AS CATEGORY_ID,SUBCATNAME_L2 AS CATEGORY_NAME FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID=N'" + parentCategoryID + "' AND SUBCATNAME_L1='BRAND' AND SUBCATID_L2 IS NOT NULL ORDER BY SUBCATNAME_L2";                
        //        string sSQL = "SELECT  DISTINCT TOSUITE_BRAND FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 <> '' AND CATEGORY_ID='" + parentCategoryID + "' AND CATALOG_ID=" + CatalogID + " ORDER BY TOSUITE_BRAND";
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

        //
        //public DataSet GetWESBrandFamily(string parentCategoryID, int CatalogID, string category_id)
        //{
        //    try
        //    {
        //        string sSQL = "SELECT DISTINCT TOSUITE_BRAND FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID IN(" +
        //          "SELECT FAMILY_ID FROM TB_FAMILY WHERE FAMILY_ID IN " +
        //          " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //          " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + category_id + "'" +
        //          " UNION " +
        //          " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + category_id + "'" +
        //          " UNION " +
        //          " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //          " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + category_id + "'))) " +
        //          " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + CatalogID + ")) AND CATEGORY_ID='" + parentCategoryID + "' ORDER BY TOSUITE_BRAND";

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


        //
        //public int GetWESBrandFamilycount(string parentCategoryID, int CatalogID, string category_id)
        //{
        //    try
        //    {
        //        string sSQL = "SELECT COUNT(DISTINCT TOSUITE_BRAND) AS TOSUITE_COUNT FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID IN(" +
        //          "SELECT FAMILY_ID FROM TB_FAMILY WHERE FAMILY_ID IN " +
        //          " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //          " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + category_id + "'" +
        //          " UNION " +
        //          " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + category_id + "'" +
        //          " UNION " +
        //          " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //          " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + category_id + "'))) " +
        //          " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + CatalogID + ")) AND CATEGORY_ID='" + parentCategoryID + "'";

        //        oHelper.SQLString = sSQL;
        //        return Convert.ToInt32(oHelper.GetValue("TOSUITE_COUNT"));
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return 0;
        //    }
        //}



        //
        //public DataSet GetWESModel(string parentCategoryID, int CatalogID, string tosuite_brand)
        //{
        //    try
        //    {
        //        //string sSQL = " SELECT DISTINCT SUBCATID_L3 AS CATEGORY_ID,SUBCATNAME_L3 AS CATEGORY_NAME FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID=N'" + parentCategoryID + "' AND SUBCATNAME_L1='BRAND' AND SUBCATID_L2=N'" + subcatid_l2.ToString() + "'AND SUBCATID_L3 IS NOT NULL ORDER BY SUBCATNAME_L3";
        //        string sSQL = "SELECT DISTINCT TOSUITE_MODEL FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 <> '' AND CATEGORY_ID='" + parentCategoryID + "' AND CATALOG_ID=" + CatalogID + " AND TOSUITE_BRAND = '" + tosuite_brand + "' ORDER BY TOSUITE_MODEL";
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


        //
        //public DataSet GetWESModelFamily(string parentCategoryID, int CatalogID, string tosuite_brand, string category_id)
        //{
        //    try
        //    {
        //        string sSQL = "SELECT DISTINCT TOSUITE_MODEL FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE FAMILY_ID IN(" +
        //         "SELECT FAMILY_ID FROM TB_FAMILY WHERE FAMILY_ID IN " +
        //         " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //         " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + category_id + "'" +
        //         " UNION " +
        //         " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + category_id + "'" +
        //         " UNION " +
        //         " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //         " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + category_id + "'))) " +
        //         " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + CatalogID + ")) AND TOSUITE_BRAND = '" + tosuite_brand + "' AND CATEGORY_ID='" + parentCategoryID + "' ORDER BY TOSUITE_MODEL";
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

        //
        //public DataSet GetWESFamily(string parentCategoryID, int CatalogID, string tosuite_brand, string tosuite_model)
        //{
        //    try
        //    {
        //        //string sSQL = " SELECT DISTINCT SUBCATID_L3 AS CATEGORY_ID,SUBCATNAME_L3 AS CATEGORY_NAME FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID=N'" + parentCategoryID + "' AND SUBCATNAME_L1='BRAND' AND SUBCATID_L2=N'" + subcatid_l2.ToString() + "'AND SUBCATID_L3 IS NOT NULL ORDER BY SUBCATNAME_L3";
        //        //string sSQL = "SELECT DISTINCT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2 AS BYPRODUCT,SUBCATID_L1,SUBCATID_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT" +
        //        //              " WHERE SUBCATID_L1 <> '' AND CATEGORY_ID='" + parentCategoryID + "' AND CATALOG_ID="+ CatalogID + 
        //        //              " AND TOSUITE_BRAND = '" + tosuite_brand + "' AND TOSUITE_MODEL = '" + tosuite_model + "' ORDER BY BYPRODUCT";

        //        string sSQL = "SELECT DISTINCT tsbp.SUBCATNAME_L1 + ' - ' + tsbp.SUBCATNAME_L2 AS BYPRODUCT, tsbp.SUBCATID_L1, tsbp.SUBCATID_L2 " +
        //                        " FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT tsbp, TB_CATEGORY tc " +
        //                        " WHERE tsbp.SUBCATID_L1 <> '' AND tsbp.SUBCATID_L1 = tc.CATEGORY_ID AND ISNULL(TC.CUSTOM_NUM_FIELD3,0)<> 3 AND " +
        //                        " tsbp.CATEGORY_ID='" + parentCategoryID + "' AND tsbp.CATALOG_ID=" + CatalogID + " AND " +
        //                        " tsbp.TOSUITE_BRAND = '" + tosuite_brand + "' AND tsbp.TOSUITE_MODEL = '" + tosuite_model + "' ORDER BY BYPRODUCT";

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

        //
        //public DataSet GetSubCategoriesL4BrandWES(string parentCategoryID, int CatalogID, string subcatid_l2, string subcatid_l3)
        //{
        //    try
        //    {
        //        string sSQL = " SELECT DISTINCT SUBCATID_L4 AS CATEGORY_ID,SUBCATNAME_L4 AS CATEGORY_NAME FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID=N'" + parentCategoryID + "' AND SUBCATNAME_L1='BRAND' AND SUBCATID_L2=N'" + subcatid_l2.ToString() + "' AND SUBCATID_L3=N'" + subcatid_l3.ToString() + "' ORDER BY SUBCATNAME_L4";
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

        //
        //public DataSet GetWESproductL1(string parentCategoryID, int CatalogID)
        //{
        //    try
        //    {
        //        //string sSQL = " SELECT DISTINCT SUBCATID_L2 AS CATEGORY_ID,SUBCATNAME_L2 AS CATEGORY_NAME FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID=N'" + parentCategoryID + "' AND SUBCATNAME_L1='PRODUCT' ORDER BY SUBCATNAME_L2";
        //        string sSQL = "SELECT DISTINCT SUBCATNAME_L1,SUBCATID_L1 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 <> '' AND CATEGORY_ID=N'" + parentCategoryID + "' AND CATALOG_ID=" + CatalogID + " ORDER BY SUBCATNAME_L1";
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



        //
        //public DataSet GetWESproductL2(string parentCategoryID, int CatalogID, string subcatid_l1)
        //{
        //    try
        //    {
        //        //string sSQL = " SELECT DISTINCT SUBCATID_L2 AS CATEGORY_ID,SUBCATNAME_L2 AS CATEGORY_NAME FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID=N'" + parentCategoryID + "' AND SUBCATNAME_L1='PRODUCT' ORDER BY SUBCATNAME_L2";
        //        string sSQL = "SELECT DISTINCT SUBCATNAME_L2,SUBCATID_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 ='" + subcatid_l1 + "' AND SUBCATID_L2 <> '' AND CATEGORY_ID=N'" + parentCategoryID + "' AND CATALOG_ID=" + CatalogID + " ORDER BY SUBCATNAME_L2";
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


        //

        //public int GetWESProductL2count(string parentCategoryID, int CatalogID, string subcatid_l1)
        //{
        //    try
        //    {
        //        string sSQL = "SELECT COUNT(DISTINCT SUBCATID_L2) AS SUBCATID_L2_COUNT FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 ='" + subcatid_l1 + "' AND SUBCATID_L2 <> '' AND CATEGORY_ID=N'" + parentCategoryID + "' AND CATALOG_ID=" + CatalogID;
        //        oHelper.SQLString = sSQL;
        //        return Convert.ToInt32(oHelper.GetValue("SUBCATID_L2_COUNT"));
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return 0;
        //    }
        //}

        /******************** SELECT CATAGORY ATTRIBUTE*****************/
        /// <summary>
        /// This is used to get all the Category related Images,Description,Type ...
        /// </summary>
        /// <param name="CategoryId">string</param>
        /// <returns>DataSet</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.IO;
        ///using System.Web;
        ///using System.Data;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Category oCat=new Category();
        ///     string CategoryId;
        ///     DataSet oCategoriesDs = new DataSet();
        ///     ...
        ///     oCategoriesDs = Cat.GetCategories(CategoryId);
        ///  }
        /// </code>
        /// </example>
        //
        //public DataSet GetCategories(string CategoryId)
        //{

        //    try
        //    {
        //        string sSQL = "SELECT * FROM TB_CATEGORY";
        //        sSQL = sSQL + " WHERE CATEGORY_ID = N'" + CategoryId + "'";
        //        sSQL = sSQL + "ORDER BY CATEGORY_NAME";
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
        /// This is used to get the sub Category Details in the Catalog.
        /// </summary>
        /// /// <param name="CatalogID">int</param>
        /// <returns>DataTable</returns>
        /// <example>
        /// <code>
        ///using System;
        ///using System.IO;
        ///using System.Web;
        ///using System.Data;
        ///using TradingBell.Common;
        ///using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Category oCat=new Category();
        ///     DataTable SubCategories = new DataTable();
        ///     ... 
        ///     SubCategories = oCat.GetSubCategories(CatalogID);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /******* ORGANIZATION : J TECH *******/
        /******* PURPOSE      : TO RETRIVE THE SUB CATEGORY DETAILS FROM RELATED DATABASE TABLES *******/
        /*********************************************************************************/
        public DataTable GetSubCategories(int CatalogID)
        {
            try
            {
                //string sSQL = " SELECT * FROM TB_CATEGORY TC,TB_CATALOG_SECTIONS TCS";
                //sSQL = sSQL + " WHERE TC.PARENT_CATEGORY <>N'0'";
                //sSQL = sSQL + " AND TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID =" + CatalogID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataTable();
                return (DataTable)objCategoryDB.GetGenericDataDB(CatalogID.ToString(), "", "GET_SUB_CATEGORYS_ALL", CategoryDB.ReturnType.RTTable);    
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        public DataTable CheckFamilyExist(int CatalogID,string category_id)
        {
            try
            {
                //string sSQL = " SELECT * FROM TB_CATEGORY TC,TB_CATALOG_SECTIONS TCS";
                //sSQL = sSQL + " WHERE TC.PARENT_CATEGORY <>N'0'";
                //sSQL = sSQL + " AND TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID =" + CatalogID;
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataTable();
                return (DataTable)objCategoryDB.GetGenericDataDB(CatalogID.ToString(), category_id, "CHECK_FAMILY_EXIST", CategoryDB.ReturnType.RTTable);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************************************************************/
        /******* ORGANIZATION : J TECH *******/
        /******* PURPOSE      : TO RETRIVE THE TOTAL COUNT OF SUB CATEGORIES  *******/
        /*********************************************************************************/
        //public int GetSubCategoriesCount(string parentCategoryID, int CatalogID)
        //{
        //    int retval=0;
        //    string tempstr = "";
        //    try
        //    {
        //        //string sSQL = " SELECT count(*) as CATEGORY_COUNT FROM TB_CATEGORY TC,TB_CATALOG_SECTIONS TCS";
        //        //sSQL = sSQL + " WHERE TC.PARENT_CATEGORY =N'" + parentCategoryID + "'";
        //        //sSQL = sSQL + " AND TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID =" + CatalogID;
        //        //oHelper.SQLString = sSQL;
        //        //return Convert.ToInt32(oHelper.GetValue("CATEGORY_COUNT"));
        //        tempstr= (string)objCategoryDB.GetGenericDataDB(CatalogID.ToString(), parentCategoryID, "GET_SUB_CATEGORYS_COUNT", CategoryDB.ReturnType.RTString);
        //        if (tempstr != null && tempstr != "")
        //            retval = Convert.ToInt32(tempstr);

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        return 0;
        //    }
        //    return retval;
        //}

        /*********************************** OLD CODE ***********************************/
        //
        //public int GetWESModelcount(string parentCategoryID, int CatalogID, string tosuite_brand)
        //{
        //    try
        //    {
        //        //string sSQL = " SELECT COUNT(DISTINCT SUBCATID_L3) AS CATEGORY_COUNT FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID=N'" + parentCategoryID + "' AND SUBCATNAME_L1='BRAND' AND SUBCATID_L2=N'" + subCatid_l2 + "' AND SUBCATID_L3 IS NOT NULL";               
        //        string sSQL = "SELECT COUNT(DISTINCT TOSUITE_MODEL) AS TOSUITE_MODEL_COUNT FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 <> '' AND CATEGORY_ID='" + parentCategoryID + "' AND CATALOG_ID=" + CatalogID + " AND TOSUITE_BRAND = '" + tosuite_brand + "'";
        //        oHelper.SQLString = sSQL;
        //        return Convert.ToInt32(oHelper.GetValue("TOSUITE_MODEL_COUNT"));
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return 0;
        //    }
        //}


        //
        //public int GetWESFamilycount(string parentCategoryID, int CatalogID, string tosuite_brand, string tosuite_model)
        //{
        //    try
        //    {
        //        //string sSQL = " SELECT COUNT(DISTINCT SUBCATID_L4) AS CATEGORY_COUNT FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID=N'" + parentCategoryID + "' AND SUBCATNAME_L1='BRAND' AND SUBCATID_L2=N'" + subCatid_l2 + "' AND SUBCATID_L3=N'" + subCatid_l3 + "' AND SUBCATID_L4 IS NOT NULL";
        //        string sSQL = "SELECT COUNT(DISTINCT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2) AS BYPRODUCT_COUNT FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 <> '' AND SUBCATID_L2 <> '' AND CATEGORY_ID='" + parentCategoryID + "' AND CATALOG_ID=" + CatalogID + " AND TOSUITE_BRAND = '" + tosuite_brand + "' AND TOSUITE_MODEL = '" + tosuite_model + "'";
        //        oHelper.SQLString = sSQL;
        //        return Convert.ToInt32(oHelper.GetValue("BYPRODUCT_COUNT"));
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return 0;
        //    }
        //}

        //
        //public string GetParent(string CatId)
        //{
        //    try
        //    {
        //        string sSQL;
        //        sSQL = " SELECT PARENT_CATEGORY FROM TB_CATEGORY";
        //        sSQL = sSQL + " WHERE CATEGORY_ID = N'" + CatId + "'";
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetValue("PARENT_CATEGORY");
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return "";
        //    }
        //}
        /*********************************** OLD CODE ***********************************/
        /*********************************************************************************/
        /******* ORGANIZATION : J TECH *******/
        /******* PURPOSE      : TO RETRIVE PDF CATALOG FILE DETAILS *******/
        /*********************************************************************************/
        public DataSet GetCatalogPDFDownload(int CatalogID)
        {
            try
            {
                //string sSQL = @"SELECT REPLACE(LTRIM(RTRIM(TC.IMAGE_FILE2)),'\','') AS IMAGE_FILE2, TC.IMAGE_NAME2, TC.IMAGE_TYPE2, TC.MODIFIED_DATE ";
                //sSQL = sSQL + "FROM TB_CATEGORY TC, TB_CATALOG_SECTIONS TCS ";
                //sSQL = sSQL + "WHERE TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID = " +  CatalogID + " AND  ";
                //sSQL = sSQL + "LTRIM(RTRIM(TC.IMAGE_FILE2)) <> '' AND LTRIM(RTRIM(TC.IMAGE_NAME2)) <> '' AND ISNULL(TC.CUSTOM_NUM_FIELD3,0)<> 3";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();

                return (DataSet)objCategoryDB.GetGenericDataDB(CatalogID.ToString(), "", "GET_CATEGORY_PDF_DOWNLOAD", CategoryDB.ReturnType.RTDataSet);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************************************************************/
        /******* ORGANIZATION : J TECH *******/
        /******* PURPOSE      : TO RETRIVE THE DETAILS FOR PDF CATALOG FILE BASED ON CATEGORY ID*******/
        /*********************************************************************************/
        public DataSet GetCatalogPDFDownload1(string categoryid)
        {
            try
            {
                //string sSQL = @"SELECT REPLACE(LTRIM(RTRIM(TC.IMAGE_FILE2)),'\','/') AS IMAGE_FILE2, TC.IMAGE_NAME2, TC.IMAGE_TYPE2, TC.MODIFIED_DATE ";
                //sSQL = sSQL + "FROM TB_CATEGORY TC, TB_CATALOG_SECTIONS TCS ";
                //sSQL = sSQL + "WHERE TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID =  2  AND TCS.CATEGORY_ID='" + categoryid + "' AND ";
                //sSQL = sSQL + "LTRIM(RTRIM(TC.IMAGE_FILE2)) <> '' AND LTRIM(RTRIM(TC.IMAGE_NAME2)) <> '' AND ISNULL(TC.CUSTOM_NUM_FIELD3,0)<> 3";                
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objCategoryDB.GetGenericDataDB(categoryid.ToString(), "GET_CATEGORY_PDF_DOWNLOAD1", CategoryDB.ReturnType.RTDataSet);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************************************************************/
        /******* ORGANIZATION : J TECH *******/
        /******* PURPOSE      : TO RETRIVE THE COUNT OF PDF FILES  *******/
        /*********************************************************************************/
        public DataSet GetCatalogPDFCount(int CatalogID)
        {
            try
            {
                //string sSQL = @"SELECT COUNT(*) AS CountFiles ";
                //sSQL = sSQL + "FROM TB_CATEGORY TC, TB_CATALOG_SECTIONS TCS ";
                //sSQL = sSQL + "WHERE TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID = " + CatalogID + " AND  ";
                //sSQL = sSQL + "LTRIM(RTRIM(TC.IMAGE_FILE2)) <> '' AND LTRIM(RTRIM(TC.IMAGE_NAME2)) <> '' AND ISNULL(TC.CUSTOM_NUM_FIELD3,0)<> 3";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objCategoryDB.GetGenericDataDB(CatalogID.ToString(), "", "GET_CATALOG_PDF_COUNT", CategoryDB.ReturnType.RTDataSet);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }
    }
    /*********************************** J TECH CODE ***********************************/
}
