using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using System.Configuration;

namespace TradingBell.WebCat.CommonServices
{

    /*********************************** J TECH CODE ***********************************/
   public  class PowerSearchServices
    {
        /*********************************** DECLARATION ***********************************/
        string _USER_SESSION_ID;
        int _CATALOG_ID = 1;
        string _CATEGORY_ID = "";
        string _SEARCH_STR = "";
        string _FILTER_STR = "";
        int _INVENTORY_CHECK = 0;
        bool _USE_PARAMETRIC_FILTER = false;
        bool _IS_START_OVER = false;
        int _FAMILY_ID = 0;
        string _SORT_BY = "";
        bool _DO_PAGING = false;
        int _PAGE_NO = 1;
        int _RECORDS_PER_PAGE = 10;
        bool _SORT_DIR_ASC = true;
        SqlConnection _SQLConn;
        //Helper oHelper = new Helper();
        Security objSecurity = new Security();
        HelperServices objHelperService = new HelperServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        


        public string USER_SESSION_ID
        {
            get
            {
                return _USER_SESSION_ID;
            }
            set
            {
                _USER_SESSION_ID = value;
            }
        }
        public int CATALOG_ID
        {
            get
            {
                return _CATALOG_ID;
            }
            set
            {
                _CATALOG_ID = value;
            }
        }
        public string CATEGORY_ID
        {
            get
            {
                return _CATEGORY_ID;
            }
            set
            {
                _CATEGORY_ID = value;
            }
        }
        public string SEARCH_STR
        {
            get
            {
                return _SEARCH_STR;
            }
            set
            {
                _SEARCH_STR = value;
            }
        }
        public string FILTER_STR
        {
            get
            {
                return _FILTER_STR;
            }
            set
            {
                _FILTER_STR = value;
            }
        }
        public int INVENTORY_CHECK
        {
            get
            {
                return _INVENTORY_CHECK;
            }
            set
            {
                _INVENTORY_CHECK = value;
            }
        }
        public bool USE_PARAMETRIC_FILTER
        {
            get
            {
                return _USE_PARAMETRIC_FILTER;
            }
            set
            {
                _USE_PARAMETRIC_FILTER = value;
            }
        }
        public bool IS_START_OVER
        {
            get
            {
                return _IS_START_OVER;
            }
            set
            {
                _IS_START_OVER = value;
            }
        }
        public string SORT_BY
        {
            get
            {
                return _SORT_BY;
            }
            set
            {
                _SORT_BY = value;
            }
        }
        public bool DO_PAGING
        {
            get
            {
                return _DO_PAGING;
            }
            set
            {
                _DO_PAGING = value;
            }
        }
        public int PAGE_NO
        {
            get
            {
                return _PAGE_NO;
            }
            set
            {
                _PAGE_NO = value;
            }
        }
        public int RECORDS_PER_PAGE
        {
            get
            {
                return _RECORDS_PER_PAGE;
            }
            set
            {
                _RECORDS_PER_PAGE = value;
            }
        }
        public bool SORT_DIR_ASC
        {
            get
            {
                return _SORT_DIR_ASC;
            }
            set
            {
                _SORT_DIR_ASC = value;
            }
        }
        /*********************************** DECLARATION ***********************************/


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RUN POWER SEARCH   ***/
        /********************************************************************************/
        // public int ExecutePowerSearch()
        //{
        //    try
        //    {
        //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
        //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
        //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
        //        objpowersearch.CATALOG_ID = _CATALOG_ID;
        //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;
        //        objpowersearch.SEARCH_STR = _SEARCH_STR;
        //        objpowersearch.FILTER_STR = "";
        //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
        //        objpowersearch.USE_PARAMETRIC_FILTER = false;
        //        objpowersearch.IS_START_OVER = _IS_START_OVER;
        //        objpowersearch.ExecutePowerSearchDB();
        //    }
        //    catch (Exception objException)
        //    {
        //        objErrorHandler.ErrorMsg = objException;
        //        objErrorHandler.CreateLog();
        //    }
        //    finally
        //    {
        //    }
        //    return 1;
        //}

         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO RETRIVE THE PRODCUT DETAILS FROM POWER SEARCH DATABASE  ***/
         /********************************************************************************/
         //public DataSet GetProducts()
         //{
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
         //        objpowersearch.SEARCH_STR = _SEARCH_STR;
         //        objpowersearch.FILTER_STR = "";
         //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
         //        objpowersearch.USE_PARAMETRIC_FILTER = false;
         //        objpowersearch.IS_START_OVER = _IS_START_OVER;
         //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
         //        objpowersearch.CATALOG_ID = _CATALOG_ID;

         //        objpowersearch.SORT_BY = _SORT_BY;
         //        objpowersearch.DO_PAGING = _DO_PAGING;
         //        objpowersearch.PAGE_NO = _PAGE_NO;
         //        objpowersearch.RECORDS_PER_PAGE = _RECORDS_PER_PAGE;
         //        objpowersearch.SORT_DIR_ASC = _SORT_DIR_ASC;

         //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;

         //        ds=objpowersearch.GetProductsDB();
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //    }
         //    finally
         //    {
         //    }
         //    return ds;
         //}

         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO RETRIVE WES RELATED PRODUCTS ***/
         /********************************************************************************/
         //public DataSet getwesproducts()
         //{
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
         //        objpowersearch.SEARCH_STR = _SEARCH_STR;
         //        objpowersearch.FILTER_STR = "";
         //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
         //        objpowersearch.USE_PARAMETRIC_FILTER = false;
         //        objpowersearch.IS_START_OVER = _IS_START_OVER;
         //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
         //        objpowersearch.CATALOG_ID = _CATALOG_ID;

         //        objpowersearch.SORT_BY = _SORT_BY;
         //        objpowersearch.DO_PAGING = _DO_PAGING;
         //        objpowersearch.PAGE_NO = _PAGE_NO;
         //        objpowersearch.RECORDS_PER_PAGE = _RECORDS_PER_PAGE;
         //        objpowersearch.SORT_DIR_ASC = _SORT_DIR_ASC;

         //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;

         //        ds = objpowersearch.getwesproductsDB();
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //    }
         //    finally
         //    {
         //    }
         //    return ds;
         //}

         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO RETRIVE ALL PRODUCT CATEGORIES ***/
         /********************************************************************************/
         //public DataSet GetCategories()
         //{
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
         //        objpowersearch.SEARCH_STR = _SEARCH_STR;
         //        objpowersearch.FILTER_STR = "";
         //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
         //        objpowersearch.USE_PARAMETRIC_FILTER = false;
         //        objpowersearch.IS_START_OVER = _IS_START_OVER;
         //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
         //        objpowersearch.CATALOG_ID = _CATALOG_ID;

         //        objpowersearch.SORT_BY = _SORT_BY;
         //        objpowersearch.DO_PAGING = _DO_PAGING;
         //        objpowersearch.PAGE_NO = _PAGE_NO;
         //        objpowersearch.RECORDS_PER_PAGE = _RECORDS_PER_PAGE;
         //        objpowersearch.SORT_DIR_ASC = _SORT_DIR_ASC;

         //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;

         //        ds = objpowersearch.GetCategoriesDB();
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //    }
         //    finally
         //    {
         //    }
         //    return ds;
         //}
         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO RETRIVE ALL PRODUCT CATEGORIES ***/
         /********************************************************************************/
         //public DataSet GetCategoriespl()
         //{
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
         //        objpowersearch.SEARCH_STR = _SEARCH_STR;
         //        objpowersearch.FILTER_STR = "";
         //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
         //        objpowersearch.USE_PARAMETRIC_FILTER = false;
         //        objpowersearch.IS_START_OVER = _IS_START_OVER;
         //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
         //        objpowersearch.CATALOG_ID = _CATALOG_ID;

         //        objpowersearch.SORT_BY = _SORT_BY;
         //        objpowersearch.DO_PAGING = _DO_PAGING;
         //        objpowersearch.PAGE_NO = _PAGE_NO;
         //        objpowersearch.RECORDS_PER_PAGE = _RECORDS_PER_PAGE;
         //        objpowersearch.SORT_DIR_ASC = _SORT_DIR_ASC;

         //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;

         //        ds = objpowersearch.GetCategoriesplDB();
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //    }
         //    finally
         //    {
         //    }
         //    return ds;
         //}
         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO RETRIVE THE PRODUCT FAMILY DETAILS  ***/
         /********************************************************************************/
         //public DataSet GetFamilies()
         //{
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
         //        objpowersearch.SEARCH_STR = _SEARCH_STR;
         //        objpowersearch.FILTER_STR = "";
         //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
         //        objpowersearch.USE_PARAMETRIC_FILTER = false;
         //        objpowersearch.IS_START_OVER = _IS_START_OVER;
         //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
         //        objpowersearch.CATALOG_ID = _CATALOG_ID;

         //        objpowersearch.SORT_BY = _SORT_BY;
         //        objpowersearch.DO_PAGING = _DO_PAGING;
         //        objpowersearch.PAGE_NO = _PAGE_NO;
         //        objpowersearch.RECORDS_PER_PAGE = _RECORDS_PER_PAGE;
         //        objpowersearch.SORT_DIR_ASC = _SORT_DIR_ASC;

         //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;

         //        ds = objpowersearch.GetFamiliesDB();
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //    }
         //    finally
         //    {
         //    }
         //    return ds;
         //}


         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO RETRIVE EACH CATEGORY NAME  ***/
         /********************************************************************************/
         //public string GetCategoryname()
         //{
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);

         //        objpowersearch.SEARCH_STR = _SEARCH_STR;
         //        objpowersearch.FILTER_STR = "";
         //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
         //        objpowersearch.USE_PARAMETRIC_FILTER = false;
         //        objpowersearch.IS_START_OVER = _IS_START_OVER;
         //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
         //        objpowersearch.CATALOG_ID = _CATALOG_ID;

         //        objpowersearch.SORT_BY = _SORT_BY;
         //        objpowersearch.DO_PAGING = _DO_PAGING;
         //        objpowersearch.PAGE_NO = _PAGE_NO;
         //        objpowersearch.RECORDS_PER_PAGE = _RECORDS_PER_PAGE;
         //        objpowersearch.SORT_DIR_ASC = _SORT_DIR_ASC;

         //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;
         //        return objpowersearch.GetCategorynameDB();
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //        return "";
         //    }
         //    finally
         //    {
         //    }

         //}

         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO RETRIVE THE DETAILS FOR UNAVAILABLE PRODUCTS  ***/
         /********************************************************************************/
         //public DataSet GetNotAvailableProd()
         //{
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
         //        objpowersearch.SEARCH_STR = _SEARCH_STR;
         //        objpowersearch.FILTER_STR = "";
         //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
         //        objpowersearch.USE_PARAMETRIC_FILTER = false;
         //        objpowersearch.IS_START_OVER = _IS_START_OVER;
         //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
         //        objpowersearch.CATALOG_ID = _CATALOG_ID;

         //        objpowersearch.SORT_BY = _SORT_BY;
         //        objpowersearch.DO_PAGING = _DO_PAGING;
         //        objpowersearch.PAGE_NO = _PAGE_NO;
         //        objpowersearch.RECORDS_PER_PAGE = _RECORDS_PER_PAGE;
         //        objpowersearch.SORT_DIR_ASC = _SORT_DIR_ASC;

         //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;

         //        ds = objpowersearch.GetNotAvailableProdDB();
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //    }
         //    finally
         //    {
         //    }
         //    return ds;
         //}

         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO RETRIVE BEST SELLER INFORMATIONS ***/
         /********************************************************************************/
         //public string GetBestSeller()
         //{
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);

         //        objpowersearch.SEARCH_STR = _SEARCH_STR;
         //        objpowersearch.FILTER_STR = "";
         //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
         //        objpowersearch.USE_PARAMETRIC_FILTER = false;
         //        objpowersearch.IS_START_OVER = _IS_START_OVER;
         //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
         //        objpowersearch.CATALOG_ID = _CATALOG_ID;

         //        objpowersearch.SORT_BY = _SORT_BY;
         //        objpowersearch.DO_PAGING = _DO_PAGING;
         //        objpowersearch.PAGE_NO = _PAGE_NO;
         //        objpowersearch.RECORDS_PER_PAGE = _RECORDS_PER_PAGE;
         //        objpowersearch.SORT_DIR_ASC = _SORT_DIR_ASC;

         //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;
         //        return objpowersearch.GetBestSellerDB();
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //        return "";
         //    }
         //    finally
         //    {
         //    }

         //}

         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO RETRIVE DETAILS FOR FEATURED PRODUCTS  ***/
         /********************************************************************************/
         //public DataSet GetFeaturedProduct()
         //{
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
         //        objpowersearch.SEARCH_STR = _SEARCH_STR;
         //        objpowersearch.FILTER_STR = "";
         //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
         //        objpowersearch.USE_PARAMETRIC_FILTER = false;
         //        objpowersearch.IS_START_OVER = _IS_START_OVER;
         //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
         //        objpowersearch.CATALOG_ID = _CATALOG_ID;

         //        objpowersearch.SORT_BY = _SORT_BY;
         //        objpowersearch.DO_PAGING = _DO_PAGING;
         //        objpowersearch.PAGE_NO = _PAGE_NO;
         //        objpowersearch.RECORDS_PER_PAGE = _RECORDS_PER_PAGE;
         //        objpowersearch.SORT_DIR_ASC = _SORT_DIR_ASC;

         //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;

         //        ds = objpowersearch.GetFeaturedProductDB();
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //    }
         //    finally
         //    {
         //    }
         //    return ds;
         //}

         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO RETRIVE NARROW FILEDS DETAILS  ***/
         /********************************************************************************/
         //public DataSet GetNarrowFields()
         //{
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
         //        objpowersearch.SEARCH_STR = _SEARCH_STR;
         //        objpowersearch.FILTER_STR = "";
         //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
         //        objpowersearch.USE_PARAMETRIC_FILTER = false;
         //        objpowersearch.IS_START_OVER = _IS_START_OVER;
         //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
         //        objpowersearch.CATALOG_ID = _CATALOG_ID;

         //        objpowersearch.SORT_BY = _SORT_BY;
         //        objpowersearch.DO_PAGING = _DO_PAGING;
         //        objpowersearch.PAGE_NO = _PAGE_NO;
         //        objpowersearch.RECORDS_PER_PAGE = _RECORDS_PER_PAGE;
         //        objpowersearch.SORT_DIR_ASC = _SORT_DIR_ASC;

         //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;

         //        ds =objpowersearch.GetNarrowFieldsDB();
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //    }
         //    finally
         //    {
         //    }
         //    return ds;
         //}
         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO APPLY PARAMETRIC FILTERS   ***/
         /********************************************************************************/
         //public void ApplyParametricFilters()
         //{
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
         //        objpowersearch.SEARCH_STR = _SEARCH_STR;
         //        objpowersearch.FILTER_STR = "";
         //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
         //        objpowersearch.USE_PARAMETRIC_FILTER = false;
         //        objpowersearch.IS_START_OVER = _IS_START_OVER;
         //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
         //        objpowersearch.CATALOG_ID = _CATALOG_ID;

         //        objpowersearch.SORT_BY = _SORT_BY;
         //        objpowersearch.DO_PAGING = _DO_PAGING;
         //        objpowersearch.PAGE_NO = _PAGE_NO;
         //        objpowersearch.RECORDS_PER_PAGE = _RECORDS_PER_PAGE;
         //        objpowersearch.SORT_DIR_ASC = _SORT_DIR_ASC;

         //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;

         //         objpowersearch.ApplyParametricFiltersDB();
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //    }
         //    finally
         //    {
         //    }

         //}

         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO RETRIVE THE DETAILS OF PARAMETRIC FILTERS  ***/
         /********************************************************************************/
         //public DataSet GetParametricFilters()
         //{
         //    DataSet ds = new DataSet();
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
         //        objpowersearch.SEARCH_STR = _SEARCH_STR;
         //        objpowersearch.FILTER_STR = "";
         //        objpowersearch.INVENTORY_CHECK = _INVENTORY_CHECK;
         //        objpowersearch.USE_PARAMETRIC_FILTER = false;
         //        objpowersearch.IS_START_OVER = _IS_START_OVER;
         //        objpowersearch.USER_SESSION_ID = _USER_SESSION_ID;
         //        objpowersearch.CATALOG_ID = _CATALOG_ID;

         //        objpowersearch.SORT_BY = _SORT_BY;
         //        objpowersearch.DO_PAGING = _DO_PAGING;
         //        objpowersearch.PAGE_NO = _PAGE_NO;
         //        objpowersearch.RECORDS_PER_PAGE = _RECORDS_PER_PAGE;
         //        objpowersearch.SORT_DIR_ASC = _SORT_DIR_ASC;

         //        objpowersearch.CATEGORY_ID = _CATEGORY_ID;

         //        ds=objpowersearch.GetParametricFiltersDB();
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //    }
         //    finally
         //    {
         //    }
         //    return ds;
         //}

         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO CLEAR THE SEARCH RESULT  ***/
         /********************************************************************************/
         public int ClearSearchResults()
         {
             int result = -1;
             try
             {
                 SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
                 PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
                 objpowersearch.ClearSearchResultsDB();
             }
             catch (Exception objException)
             {
                 objErrorHandler.ErrorMsg = objException;
                 objErrorHandler.CreateLog();
                 return -1;
             }
             finally
             {
             }

             return result;
         }

         /*********************************************************************************/
         /*** ORGANIZATION : J TECH ***/
         /*** PURPOSE      : TO CLEAR FILTER PRODUCT  ***/
         /********************************************************************************/
         //public int ClearFilterproduct(string sqlquery)
         //{
         //    int result = -1;
         //    try
         //    {
         //        SqlConnection objConnection = new SqlConnection(objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["TBWebCatShoppingCartConnString"].ToString()));
         //        PowerSearchDB objpowersearch = new PowerSearchDB(objConnection);
         //        result=objpowersearch.ClearFilterproductDB(sqlquery);
         //    }
         //    catch (Exception objException)
         //    {
         //        objErrorHandler.ErrorMsg = objException;
         //        objErrorHandler.CreateLog();
         //        return -1;
         //    }
         //    finally
         //    {
         //    }

         //    return result;
         //}
         

    }
   /*********************************** J TECH CODE ***********************************/
}
