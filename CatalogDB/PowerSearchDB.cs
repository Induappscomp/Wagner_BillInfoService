using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TradingBell.WebCat;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Text;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebCat.CatalogDB
{
    #region OLD CODE TRADING BELL
    //string _USER_SESSION_ID;
    //    int _CATALOG_ID = 1;
    //    string _CATEGORY_ID = "";
    //    string _SEARCH_STR = "";
    //    string _FILTER_STR = "";
    //    int _INVENTORY_CHECK = 0;
    //    bool _USE_PARAMETRIC_FILTER = false;
    //    bool _IS_START_OVER = false;
    //    int _FAMILY_ID = 0;
    //    string _SORT_BY = "";
    //    bool _DO_PAGING = false;
    //    int _PAGE_NO = 1;
    //    int _RECORDS_PER_PAGE = 10;
    //    bool _SORT_DIR_ASC = true;
    //    SqlConnection _SQLConn;
    //    //Helper oHelper = new Helper();
    //    ErrorHandler objErrorHandler = new ErrorHandler();



    //    public string USER_SESSION_ID
    //    {
    //        get
    //        {
    //            return _USER_SESSION_ID;
    //        }
    //        set
    //        {
    //            _USER_SESSION_ID = value;
    //        }
    //    }
    //    public int CATALOG_ID
    //    {
    //        get
    //        {
    //            return _CATALOG_ID;
    //        }
    //        set
    //        {
    //            _CATALOG_ID = value;
    //        }
    //    }
    //    public string CATEGORY_ID
    //    {
    //        get
    //        {
    //            return _CATEGORY_ID;
    //        }
    //        set
    //        {
    //            _CATEGORY_ID = value;
    //        }
    //    }
    //    public string SEARCH_STR
    //    {
    //        get
    //        {
    //            return _SEARCH_STR;
    //        }
    //        set
    //        {
    //            _SEARCH_STR = value;
    //        }
    //    }
    //    public string FILTER_STR
    //    {
    //        get
    //        {
    //            return _FILTER_STR;
    //        }
    //        set
    //        {
    //            _FILTER_STR = value;
    //        }
    //    }
    //    public int INVENTORY_CHECK
    //    {
    //        get
    //        {
    //            return _INVENTORY_CHECK;
    //        }
    //        set
    //        {
    //            _INVENTORY_CHECK = value;
    //        }
    //    }
    //    public bool USE_PARAMETRIC_FILTER
    //    {
    //        get
    //        {
    //            return _USE_PARAMETRIC_FILTER;
    //        }
    //        set
    //        {
    //            _USE_PARAMETRIC_FILTER = value;
    //        }
    //    }
    //    public bool IS_START_OVER
    //    {
    //        get
    //        {
    //            return _IS_START_OVER;
    //        }
    //        set
    //        {
    //            _IS_START_OVER = value;
    //        }
    //    }
    //    public string SORT_BY
    //    {
    //        get
    //        {
    //            return _SORT_BY;
    //        }
    //        set
    //        {
    //            _SORT_BY = value;
    //        }
    //    }
    //    public bool DO_PAGING
    //    {
    //        get
    //        {
    //            return _DO_PAGING;
    //        }
    //        set
    //        {
    //            _DO_PAGING = value;
    //        }
    //    }
    //    public int PAGE_NO
    //    {
    //        get
    //        {
    //            return _PAGE_NO;
    //        }
    //        set
    //        {
    //            _PAGE_NO = value;
    //        }
    //    }
    //    public int RECORDS_PER_PAGE
    //    {
    //        get
    //        {
    //            return _RECORDS_PER_PAGE;
    //        }
    //        set
    //        {
    //            _RECORDS_PER_PAGE = value;
    //        }
    //    }
    //    public bool SORT_DIR_ASC
    //    {
    //        get
    //        {
    //            return _SORT_DIR_ASC;
    //        }
    //        set
    //        {
    //            _SORT_DIR_ASC = value;
    //        }
    //    }
    //    public PowerSearchDB(SqlConnection SQLConn)
    //    {
    //        _SQLConn = SQLConn;
    //    }

        //public int ExecutePowerSearchDB()
        //{
        //    int ict = 0;
        //    SqlDataReader psrdr = null;
        //    try
        //    {

        //        SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH", _SQLConn);
        //        pscmd.CommandType = CommandType.StoredProcedure;
        //        pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATALOG_ID", _CATALOG_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", _CATEGORY_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@SEARCH_STR", _SEARCH_STR.Replace("→", "<ars>g</ars>")));
        //        pscmd.Parameters.Add(new SqlParameter("@FILTER_STR", _FILTER_STR));
        //        pscmd.Parameters.Add(new SqlParameter("@INVENTORY_CHECK", _INVENTORY_CHECK));
        //        pscmd.Parameters.Add(new SqlParameter("@USE_PARAMETRIC_FILTER", _USE_PARAMETRIC_FILTER));
        //        pscmd.Parameters.Add(new SqlParameter("@IS_START_OVER", _IS_START_OVER));
        //        psrdr = pscmd.ExecuteReader();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return 1;
        //}

        //public DataSet GetProductsDB()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_GET_PRODUCTS", _SQLConn);
        //        pscmd.CommandType = CommandType.StoredProcedure;
        //        pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATALOG_ID", _CATALOG_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@INVENTORY_CHECK", _INVENTORY_CHECK));
        //        pscmd.Parameters.Add(new SqlParameter("@SORT_BY", _SORT_BY));
        //        pscmd.Parameters.Add(new SqlParameter("@DO_PAGING", _DO_PAGING));
        //        pscmd.Parameters.Add(new SqlParameter("@PAGE_NO", _PAGE_NO));
        //        pscmd.Parameters.Add(new SqlParameter("@RECORDS_PER_PAGE", _RECORDS_PER_PAGE));
        //        pscmd.Parameters.Add(new SqlParameter("@SORT_DIR_ASC", _SORT_DIR_ASC));
        //        pscmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", _CATEGORY_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@BG_CATALOG", _CATALOG_ID));
        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return ds;
        //}

        //public DataSet getwesproductsDB()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_GET_WES_PRODUCTS", _SQLConn);
        //        pscmd.CommandType = CommandType.StoredProcedure;
        //        pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATALOG_ID", _CATALOG_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@INVENTORY_CHECK", _INVENTORY_CHECK));
        //        pscmd.Parameters.Add(new SqlParameter("@SORT_BY", _SORT_BY));
        //        pscmd.Parameters.Add(new SqlParameter("@DO_PAGING", _DO_PAGING));
        //        pscmd.Parameters.Add(new SqlParameter("@PAGE_NO", _PAGE_NO));
        //        pscmd.Parameters.Add(new SqlParameter("@RECORDS_PER_PAGE", _RECORDS_PER_PAGE));
        //        pscmd.Parameters.Add(new SqlParameter("@SORT_DIR_ASC", _SORT_DIR_ASC));
        //        pscmd.Parameters.Add(new SqlParameter("@BG_CATALOG", _CATALOG_ID));
        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return ds;
        //}


        //public DataSet GetCategoriesDB()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {

        //        SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_GET_CATEGORY", _SQLConn);
        //        pscmd.CommandType = CommandType.StoredProcedure;
        //        pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATALOG_ID", _CATALOG_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@INVENTORY_CHECK", _INVENTORY_CHECK));
        //        pscmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", _CATEGORY_ID));
        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //        string cmdd = " SELECT * FROM TBWC_SEARCH_PROD_LIST WHERE USER_SESSION_ID = '" + _USER_SESSION_ID + "'";
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        DataSet DDS = new DataSet();
        //        SqlDataAdapter dDDa = new SqlDataAdapter(pscmd);
        //        dDDa.Fill(DDS);
        //        DDS = ProductFilterFlatTable(DDS);

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return ds;
        //}

        //public DataSet GetCategoriesplDB()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        DataSet dsC = new DataSet();
        //        string sql = "SELECT * FROM T2";
        //        SqlCommand dscC = new SqlCommand(sql, _SQLConn);
        //        try
        //        {
        //            SqlDataAdapter dsdC = new SqlDataAdapter(dscC);
        //            dsdC.Fill(dsC);
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        string cmdd = "";
        //        SqlCommand pscmd;
        //        int valr;
        //        if (dsC != null && dsC.Tables.Count > 0)
        //        {
        //            cmdd = " DROP TABLE T2 ";
        //            pscmd = new SqlCommand(cmdd, _SQLConn);
        //            pscmd.CommandType = CommandType.Text;
        //            valr = pscmd.ExecuteNonQuery();
        //        }
        //        cmdd = " SELECT CATEGORY_ID, NULL COUNT INTO T2 FROM TB_cATEGORY WHERE 1=2 ";
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        valr = pscmd.ExecuteNonQuery();
        //        cmdd = " INSERT INTO T2 " +
        //                      " SELECT C.CATEGORY_ID, COUNT(PF.PRODUCT_ID) COUNT  FROM TB_CATEGORY C, TB_FAMILY F, " +
        //                      " TB_PROD_FAMILY PF, TB_CATALOG_FAMILY CF, TBWC_INVENTORY WC WHERE " +
        //                      " C.PARENT_CATEGORY='" + _CATEGORY_ID + "' AND F.CATEGORY_ID=C.CATEGORY_ID AND " +
        //                      " F.FAMILY_ID=PF.FAMILY_ID AND PF.FAMILY_ID=CF.FAMILY_ID AND CATALOG_ID=" + _CATALOG_ID + " " +
        //                      " AND PF.PRODUCT_ID=WC.PRODUCT_ID AND QTY_AVAIL>=" + _INVENTORY_CHECK + " GROUP BY C.CATEGORY_ID " +
        //                      " UNION " +
        //                      " SELECT C1.CATEGORY_ID, COUNT(PF.PRODUCT_ID) COUNT FROM TB_CATEGORY C1, TB_FAMILY F, " +
        //                      " TB_PROD_FAMILY PF, TB_CATEGORY C2, TB_CATALOG_FAMILY CF, TBWC_INVENTORY WC WHERE " +
        //                      " C1.PARENT_CATEGORY='" + _CATEGORY_ID + "' AND C1.CATEGORY_ID=C2.PARENT_CATEGORY AND " +
        //                      " F.CATEGORY_ID=C2.CATEGORY_ID AND  F.FAMILY_ID=PF.FAMILY_ID AND " +
        //                      " PF.FAMILY_ID=CF.FAMILY_ID AND CF.CATALOG_ID=" + _CATALOG_ID + " AND PF.PRODUCT_ID=WC.PRODUCT_ID " +
        //                      " AND QTY_AVAIL>=" + _INVENTORY_CHECK + " GROUP BY C1.CATEGORY_ID ";
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        valr = pscmd.ExecuteNonQuery();
        //        cmdd = " SELECT DISTINCT C.*, (SELECT CONVERT(NVARCHAR, SUM(COUNT)) AS " +
        //                    " CATEGORY_NAME_WITH_COUNT FROM T2 WHERE CATEGORY_ID=A.CATEGORY_ID) CATEGORY_NAME_WITH_COUNT FROM T2 A, " +
        //                    " TB_CATEGORY C WHERE A.CATEGORY_ID=C.CATEGORY_ID AND ISNULL(C.CUSTOM_NUM_FIELD3,0)<> 3 ";
        //        if (HttpContext.Current.Session["PARAFILTER"] == "Value")
        //        {
        //            //ps.FILTER_STR 4^American Scholar|339^Blue                    
        //            cmdd = cmdd + " and C.CATEGORY_ID in (select category_id from tb_family where family_id in" +
        //                      " (select family_id from tb_prod_family where product_id in" +
        //                      " (select product_id from TBWC_SEARCH_PROD_LIST where user_session_id = '" + _USER_SESSION_ID + "'))) ";


        //        }

                //"SELECT DISTINCT C.CATEGORY_ID, C.CATEGORY_NAME, C.PARENT_CATEGORY, C.SHORT_DESC, C.IMAGE_FILE, C.IMAGE_TYPE, C.IMAGE_NAME, C.CUSTOM_TEXT_FIELD1, CONVERT(NVARCHAR, COUNT(*)) AS CATEGORY_NAME_WITH_COUNT " +
                //                              " FROM	TB_CATEGORY C WITH (NOLOCK), TBWC_INVENTORY I WITH (NOLOCK), TB_CATALOG_FAMILY CF WITH (NOLOCK), TB_PROD_FAMILY PF WITH (NOLOCK) " +
                //                              " WHERE	C.CATEGORY_ID = CF.CATEGORY_ID AND " +
                //                              " CF.FAMILY_ID = PF.FAMILY_ID AND " +
                //                              " PF.PRODUCT_ID = I.PRODUCT_ID AND " +
                //                              " I.QTY_AVAIL >= " + _INVENTORY_CHECK + " AND " +
                //                              " CF.CATALOG_ID = " + _CATALOG_ID + " AND	" +
                //                              "	C.parent_CATEGORY ='" + _CATEGORY_ID + "'" +
                //                              " GROUP BY C.CATEGORY_ID, C.CATEGORY_NAME , C.PARENT_CATEGORY, C.SHORT_DESC, C.IMAGE_FILE, C.IMAGE_TYPE, C.IMAGE_NAME, C.CUSTOM_TEXT_FIELD1, C.CATEGORY_NAME " +
                //                              " ORDER BY C.CATEGORY_NAME ";
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //        cmdd = " DELETE FROM TBWC_SEARCH_PROD_LIST WHERE USER_SESSION_ID='" + _USER_SESSION_ID + "'";
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        valr = pscmd.ExecuteNonQuery();
        //        cmdd = " INSERT INTO TBWC_SEARCH_PROD_LIST SELECT FAMILY_ID AS PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM TB_FAMILY WHERE FAMILY_ID IN " +
        //               " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //               " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + _CATEGORY_ID + "'" +
        //               " UNION " +
        //               " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'" +
        //               " UNION " +
        //               " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //               " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'))) " +
        //               " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + _CATALOG_ID + ")";
        //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true ||
        //           HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx") == true)
        //        {
        //            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true)
        //            {
        //                if (HttpContext.Current.Request.QueryString["sl1"] != null && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl1"].ToString() != "" && HttpContext.Current.Request.QueryString["sl2"].ToString() != "" && HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request.QueryString["tsb"].ToString() != "" && HttpContext.Current.Request.QueryString["tsm"] != null && HttpContext.Current.Request.QueryString["tsm"].ToString() != "")
        //                {
        //                    //cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID='" + _CATEGORY_ID + "' AND SUBCATNAME_L1='BRAND' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND SUBCATID_L3='" + HttpContext.Current.Request.QueryString["sl3"].ToString() + "' AND FAMILY_ID=" + HttpContext.Current.Request.QueryString["pfid"].ToString() + " AND SUBFAMILY_ID=" + HttpContext.Current.Request.QueryString["fid"].ToString() + " AND PRODUCT_ID IS NOT NULL";
        //                    if (HttpContext.Current.Request.QueryString["sl2"].ToString() != "0")
        //                        cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 = '" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2 = '" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND CATEGORY_ID=N'" + _CATEGORY_ID + "' AND CATALOG_ID=" + _CATALOG_ID + " AND TOSUITE_BRAND = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "' AND TOSUITE_MODEL = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "'";
        //                    else
        //                        cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 = '" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2 = '' AND CATEGORY_ID=N'" + _CATEGORY_ID + "' AND CATALOG_ID=" + _CATALOG_ID + " AND TOSUITE_BRAND = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "' AND TOSUITE_MODEL = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "'";
        //                }
        //                else if (HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request.QueryString["tsb"].ToString() != "" && HttpContext.Current.Request.QueryString["tsm"] != null && HttpContext.Current.Request.QueryString["tsm"].ToString() != "")
        //                {
        //                    cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID=N'" + _CATEGORY_ID + "' AND CATALOG_ID=" + _CATALOG_ID + " AND TOSUITE_BRAND = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "' AND TOSUITE_MODEL = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "'";
        //                }
        //                else
        //                {
        //                    cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID=N'" + _CATEGORY_ID + "' AND CATALOG_ID=" + _CATALOG_ID + " AND TOSUITE_BRAND = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "'";
        //                }
        //                //else
        //                //{
        //                //    cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID='" + _CATEGORY_ID + "' AND SUBCATNAME_L1='BRAND' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND SUBCATID_L3='" + HttpContext.Current.Request.QueryString["sl3"].ToString() + "' AND PRODUCT_ID IS NOT NULL";
        //                //}
        //            }
        //            else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx") == true && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl2"].ToString() != "")
        //            {
        //                if (HttpContext.Current.Request.QueryString["fid"] != null && HttpContext.Current.Request.QueryString["fid"].ToString() != "")
        //                {
        //                    cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID='" + _CATEGORY_ID + "' AND SUBCATNAME_L1='PRODUCT' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND FAMILY_ID=" + HttpContext.Current.Request.QueryString["fid"].ToString() + " AND PRODUCT_ID IS NOT NULL";
        //                }
        //                else
        //                {
        //                    cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID='" + _CATEGORY_ID + "' AND SUBCATNAME_L1='PRODUCT' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND PRODUCT_ID IS NOT NULL";
        //                }
        //            }
        //            else
        //            {

        //                if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == false &&
        //                    HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx") == false)
        //                {
        //                    cmdd = " INSERT INTO TBWC_SEARCH_PROD_LIST SELECT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM TB_PROD_FAMILY WHERE FAMILY_ID IN " +
        //                          " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //                          " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + _CATEGORY_ID + "'" +
        //                          " UNION " +
        //                          " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'" +
        //                          " UNION " +
        //                          " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //                          " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'))) " +
        //                          " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + _CATALOG_ID + ")";
        //                    if (HttpContext.Current.Request.QueryString["fid"] != null && HttpContext.Current.Request.QueryString["fid"].ToString() != "")
        //                    {
        //                        cmdd = " INSERT INTO TBWC_SEARCH_PROD_LIST SELECT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM TB_PROD_FAMILY WHERE FAMILY_ID IN " +
        //                         " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //                         " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + _CATEGORY_ID + "'" +
        //                         " UNION " +
        //                         " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'" +
        //                         " UNION " +
        //                         " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //                         " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'))) " +
        //                         " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + _CATALOG_ID + ") AND FAMILY_ID=" + HttpContext.Current.Request.QueryString["fid"].ToString();
        //                    }
        //                }
        //            }
        //        }
        //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "" && HttpContext.Current.Request["sl2"] != null && HttpContext.Current.Request["sl2"].ToString() != "")
        //        {
        //            cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT FAMILY_ID AS PRODUCT_ID,'" + _USER_SESSION_ID + "' FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID=N'" + _CATEGORY_ID + "' AND CATALOG_ID=" + CATALOG_ID + " AND  SUBCATID_L1 = '" + HttpContext.Current.Request["sl1"].ToString() + "' AND SUBCATID_L2 = '" + HttpContext.Current.Request["sl2"].ToString() + "'";
        //        }
        //        else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "")
        //        {
        //            cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT FAMILY_ID AS PRODUCT_ID,'" + _USER_SESSION_ID + "' FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE category_id=N'" + _CATEGORY_ID + "' AND CATALOG_ID=" + CATALOG_ID + " AND  SUBCATID_L1 = '" + HttpContext.Current.Request["sl1"].ToString() + "'";
        //        }
        //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true && HttpContext.Current.Request["sb"] != null && HttpContext.Current.Request["sb"].ToString() != "" && HttpContext.Current.Request["sm"] != null && HttpContext.Current.Request["sm"].ToString() != "")
        //        {
        //            cmdd = " INSERT INTO TBWC_SEARCH_PROD_LIST SELECT FAMILY_ID AS PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM TB_FAMILY WHERE FAMILY_ID IN " +
        //               " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //               " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + _CATEGORY_ID + "'" +
        //               " UNION " +
        //               " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'" +
        //               " UNION " +
        //               " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //               " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'))) " +
        //               " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + _CATALOG_ID + ") AND FAMILY_ID IN(SELECT DISTINCT FAMILY_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + HttpContext.Current.Request["pcr"].ToString() + "' AND TOSUITE_BRAND='" + HttpUtility.UrlDecode(HttpContext.Current.Request["sb"].ToString()) + "' AND TOSUITE_MODEL='" + HttpUtility.UrlDecode(HttpContext.Current.Request["sm"].ToString()) + "')";
        //        }
        //        else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true && HttpContext.Current.Request["sb"] != null && HttpContext.Current.Request["sb"].ToString() != "")
        //        {
        //            cmdd = " INSERT INTO TBWC_SEARCH_PROD_LIST SELECT FAMILY_ID AS PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM TB_FAMILY WHERE FAMILY_ID IN " +
        //               " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //               " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + _CATEGORY_ID + "'" +
        //               " UNION " +
        //               " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'" +
        //               " UNION " +
        //               " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //               " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'))) " +
        //               " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + _CATALOG_ID + ") AND FAMILY_ID IN(SELECT DISTINCT FAMILY_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + HttpContext.Current.Request["pcr"].ToString() + "' AND TOSUITE_BRAND='" + HttpUtility.UrlDecode(HttpContext.Current.Request["sb"].ToString()) + "')";
        //        }
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        valr = pscmd.ExecuteNonQuery();
        //        cmdd = " SELECT * FROM TBWC_SEARCH_PROD_LIST WHERE USER_SESSION_ID = '" + _USER_SESSION_ID + "'";
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        DataSet DDS = new DataSet();
        //        SqlDataAdapter dDDa = new SqlDataAdapter(pscmd);
        //        dDDa.Fill(DDS);
        //        DDS = ProductFilterFlatTable(DDS);

        //        if (HttpContext.Current.Session["PARAFILTER"] == "Value")
        //        {
        //            ApplyParametricFiltersDB();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return ds;
        //}


        //public DataSet GetFamiliesDB()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_GET_FAMILY", _SQLConn);
        //        pscmd.CommandType = CommandType.StoredProcedure;
        //        pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATALOG_ID", _CATALOG_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@INVENTORY_CHECK", _INVENTORY_CHECK));
        //        pscmd.Parameters.Add(new SqlParameter("@SORT_BY", _SORT_BY));
        //        pscmd.Parameters.Add(new SqlParameter("@DO_PAGING", _DO_PAGING));
        //        pscmd.Parameters.Add(new SqlParameter("@PAGE_NO", _PAGE_NO));
        //        pscmd.Parameters.Add(new SqlParameter("@RECORDS_PER_PAGE", _RECORDS_PER_PAGE));
        //        pscmd.Parameters.Add(new SqlParameter("@SORT_DIR_ASC", _SORT_DIR_ASC));
        //        pscmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", _CATEGORY_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@BG_CATALOG", _CATALOG_ID));
        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return ds;
        //}

    //    public string GetCategorynameDB()
    //    {//
    //        //DataSet ds = new DataSet();
    //        //try
    //        //{
    //        //    string cmdd = "SELECT CATEGORY_NAME " +
    //        //                                      " FROM	TB_CATEGORY " +
    //        //                                      " WHERE	CATEGORY_ID ='" + _CATEGORY_ID + "'";
    //        //    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
    //        //    pscmd.CommandType = CommandType.Text;
    //        //    SqlDataAdapter da = new SqlDataAdapter(pscmd);
    //        //    da.Fill(ds);
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    objErrorHandler.ErrorMsg = ex;
    //        //    objErrorHandler.CreateLog();
    //        //}
    //        //if (ds.Tables[0].Rows.Count > 0)
    //        //    return ds.Tables[0].Rows[0].ItemArray[0].ToString();
    //        //return "";
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            SqlCommand pscmd = new SqlCommand("STP_TBWC_PICK_WES_POWERSEARCH_GET_CATEGORYNAME", _SQLConn) { CommandType = CommandType.StoredProcedure };
    //            pscmd.Parameters.AddWithValue("@CATEGORY_ID", _CATEGORY_ID);
    //            SqlDataAdapter da = new SqlDataAdapter(pscmd);
    //            da.Fill(ds);
    //        }
    //        catch (Exception objException)
    //        {
    //            objErrorHandler.ErrorMsg = objException;
    //            objErrorHandler.CreateLog();
    //        }
    //        finally
    //        {
    //        }
    //        if (ds.Tables[0].Rows.Count > 0)
    //            return ds.Tables[0].Rows[0][0].ToString();
    //        return "";
    //    }

    //    public DataSet GetNotAvailableProdDB()
    //    {
    //        //DataSet ds = new DataSet();
    //        //try
    //        //{
    //        //    string cmdd = "select product_id from tbwc_inventory where product_status <> 'AVAILABLE'";
    //        //    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
    //        //    pscmd.CommandType = CommandType.Text;
    //        //    SqlDataAdapter da = new SqlDataAdapter(pscmd);
    //        //    da.Fill(ds);
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    objErrorHandler.ErrorMsg = ex;
    //        //    objErrorHandler.CreateLog();
    //        //}

    //        //return ds;
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("STP_TBWC_PICK_WES_GET_NOTAVAILABLE_PRODUCTS", _SQLConn) { CommandType = CommandType.StoredProcedure });
    //            da.Fill(ds);
    //        }
    //        catch (Exception objException)
    //        {
    //            objErrorHandler.ErrorMsg = objException;
    //            objErrorHandler.CreateLog();
    //        }
          

    //        return ds;
    //    }

    //    public string GetBestSellerDB()
    //    {
    //        //DataSet ds = new DataSet();
    //        //try
    //        //{
    //        //    string cmdd = "SELECT ATTRIBUTE_ID " +
    //        //                                      " FROM	TB_ATTRIBUTE " +
    //        //                                      " WHERE	ATTRIBUTE_NAME ='Best Seller'";
    //        //    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
    //        //    pscmd.CommandType = CommandType.Text;
    //        //    SqlDataAdapter da = new SqlDataAdapter(pscmd);
    //        //    da.Fill(ds);
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    objErrorHandler.ErrorMsg = ex;
    //        //    objErrorHandler.CreateLog();
    //        //}
    //        //if (ds.Tables[0].Rows.Count > 0)
    //        //    return ds.Tables[0].Rows[0].ItemArray[0].ToString();
    //        //return "";
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //             SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("STP_TBWC_PICK_WES_POWERSEARCH_GET_BESTATTRIBUTE", _SQLConn) { CommandType = CommandType.StoredProcedure });
    //            da.Fill(ds);
    //        }
    //        catch (Exception objException)
    //        {
    //            objErrorHandler.ErrorMsg = objException;
    //            objErrorHandler.CreateLog();
    //        }
    //        finally
    //        {
    //        }
    //        if (ds.Tables[0].Rows.Count > 0)
    //            return ds.Tables[0].Rows[0].ItemArray[0].ToString();
    //        return "";
    //    }

    //    public DataSet GetFeaturedProductDB()
    //    {
    //        //DataSet ds = new DataSet();
    //        //try
    //        //{
    //        //    string cmdd = "SELECT STRING_VALUE,PRODUCT_ID,ATTRIBUTE_ID,NUMERIC_VALUE,OBJECT_TYPE,[OBJECT_NAME],(SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = PS.ATTRIBUTE_ID) AS ATTRIBUTE_TYPE FROM TB_PROD_SPECS PS  WHERE PRODUCT_ID IN (SELECT PRODUCT_ID " +
    //        //                                      " FROM	TBWC_FEATURED_PRODUCT" +
    //        //                                      " WHERE	CATEGORY_ID ='" + _CATEGORY_ID + "')";
    //        //    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
    //        //    pscmd.CommandType = CommandType.Text;
    //        //    SqlDataAdapter da = new SqlDataAdapter(pscmd);
    //        //    da.Fill(ds);
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    objErrorHandler.ErrorMsg = ex;
    //        //    objErrorHandler.CreateLog();
    //        //}
    //        //return ds;
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            SqlCommand pscmd = new SqlCommand("STP_WES_GET_FEATURED_PRODUCTS", _SQLConn) { CommandType = CommandType.StoredProcedure };
    //            pscmd.Parameters.AddWithValue("@CATEGORY_ID", _CATEGORY_ID);
    //            SqlDataAdapter da = new SqlDataAdapter(pscmd);
    //            da.Fill(ds);
    //        }
    //        catch (Exception objException)
    //        {
    //            objErrorHandler.ErrorMsg = objException;
    //            objErrorHandler.CreateLog();
    //        }
           
           
    //        return ds;
    //    }

    //    public DataSet GetNarrowFieldsDB()
    //    {
    //        //DataSet ds = new DataSet();
    //        //try
    //        //{
    //        //    string cmdd = "select attribute_id from tbwc_search_narrow_fields order by sort_order";
    //        //    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
    //        //    pscmd.CommandType = CommandType.Text;
    //        //    SqlDataAdapter da = new SqlDataAdapter(pscmd);
    //        //    da.Fill(ds);
    //        //}
    //        //catch (Exception ex)
    //        //{
    //        //    objErrorHandler.ErrorMsg = ex;
    //        //    objErrorHandler.CreateLog();
    //        //}
    //        //return ds;
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("STP_WES_GET_NARROW_FIELDS", _SQLConn) { CommandType = CommandType.StoredProcedure });
    //            da.Fill(ds);
    //        }
    //        catch (Exception objException)
    //        {
    //            objErrorHandler.ErrorMsg = objException;
    //            objErrorHandler.CreateLog();
    //        }
        
    //        return ds;
    //    }

    //    public void ApplyParametricFiltersDB()
    //    {
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            if (System.Web.HttpContext.Current.Request.Url.ToString().ToUpper().Contains("PL"))
    //            {
    //                if (System.Web.HttpContext.Current.Session["filter"] == null || System.Web.HttpContext.Current.Session["filter"] == "")
    //                    System.Web.HttpContext.Current.Session["filter"] = _FILTER_STR;

    //                else
    //                {
    //                    System.Web.HttpContext.Current.Session["filter"] = System.Web.HttpContext.Current.Session["filter"] + "|" + _FILTER_STR;
    //                    _FILTER_STR = System.Web.HttpContext.Current.Session["filter"].ToString();
    //                }
    //            }
    //            else
    //            {
    //                System.Web.HttpContext.Current.Session["filter"] = null;
    //            }
    //            SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_APPLY_PARAMETRIC", _SQLConn);
    //            pscmd.CommandType = CommandType.StoredProcedure;
    //            pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
    //            pscmd.Parameters.Add(new SqlParameter("@FILTER", _FILTER_STR));
    //            pscmd.ExecuteNonQuery();
    //        }
    //        catch (Exception ex)
    //        {
    //            objErrorHandler.ErrorMsg = ex;
    //            objErrorHandler.CreateLog();
    //        }

    //    }

    //    public DataSet GetParametricFiltersDB()
    //    {
    //        DataSet ds = new DataSet();
    //        try
    //        {
    //            SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_GET_PARAMETRIC_FILTERS", _SQLConn);
    //            pscmd.CommandType = CommandType.StoredProcedure;
    //            pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
    //            pscmd.Parameters.Add(new SqlParameter("@CATALOG_ID", _CATALOG_ID));
    //            pscmd.Parameters.Add(new SqlParameter("@INVENTORY_CHECK", _INVENTORY_CHECK));
    //            pscmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", _CATEGORY_ID));

    //            SqlDataAdapter da = new SqlDataAdapter(pscmd);
    //            da.Fill(ds);
    //        }
    //        catch (Exception ex)
    //        {
    //            objErrorHandler.ErrorMsg = ex;
    //            objErrorHandler.CreateLog();
    //        }
    //        return ds;
    //    }
    //    public int ClearSearchResultsDB()
    //    {
    //        int result = -1;
    //        try
    //        {
    //            SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_CLEAR", _SQLConn);
    //            pscmd.CommandType = CommandType.StoredProcedure;
    //            pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
    //            result = Convert.ToInt32(pscmd.ExecuteNonQuery());
    //        }
    //        catch (Exception ex)
    //        {
    //            objErrorHandler.ErrorMsg = ex;
    //            objErrorHandler.CreateLog();
    //            return -1;
    //        }
    //        return result;
    //    }
    //    public int ClearFilterproductDB(string sqlquery)
    //    {
    //        int result = -1;
    //        try
    //        {
    //            SqlCommand pscmd = new SqlCommand(sqlquery, _SQLConn);
    //            result = Convert.ToInt32(pscmd.ExecuteNonQuery());
    //        }
    //        catch (Exception ex)
    //        {
    //            objErrorHandler.ErrorMsg = ex;
    //            objErrorHandler.CreateLog();
    //            return -1;
    //        }
    //        return result;
    //    }
    //    private DataSet ProductFilterFlatTable(DataSet flatDataset)
    //    {
    //        {
    //            StringBuilder SQLstring = new StringBuilder();
    //            DataSet oDsProductFilter = new DataSet();
    //            string SQLString = " SELECT PRODUCT_FILTERS FROM TB_CATALOG WHERE  CATALOG_ID = " + CATALOG_ID + " ";
    //            SqlDataAdapter da = new SqlDataAdapter(SQLString, _SQLConn);
    //            da.Fill(oDsProductFilter);
    //            string sProductFilter = string.Empty;
    //            if (oDsProductFilter.Tables[0].Rows.Count > 0 && oDsProductFilter.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
    //            {
    //                sProductFilter = oDsProductFilter.Tables[0].Rows[0].ItemArray[0].ToString();
    //                XmlDocument xmlDOc = new XmlDocument();
    //                xmlDOc.LoadXml(sProductFilter);
    //                XmlNode rNode = xmlDOc.DocumentElement;

    //                if (rNode.ChildNodes.Count > 0)
    //                {
    //                    for (int i = 0; i < rNode.ChildNodes.Count; i++)
    //                    {
    //                        XmlNode TableDataSetNode = rNode.ChildNodes[i];

    //                        if (TableDataSetNode.HasChildNodes)
    //                        {
    //                            if (TableDataSetNode.ChildNodes[2].InnerText == " ")
    //                            {
    //                                TableDataSetNode.ChildNodes[2].InnerText = "=";
    //                            }
    //                            if (TableDataSetNode.ChildNodes[0].InnerText == " ")
    //                            {
    //                                TableDataSetNode.ChildNodes[0].InnerText = "0";
    //                            }
    //                            string stringval = TableDataSetNode.ChildNodes[3].InnerText.Replace("'", "''");
    //                            DataSet attribuetypeDS = new DataSet();
    //                            string sSQLString = " SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE  ATTRIBUTE_ID = " + Convert.ToInt32(TableDataSetNode.ChildNodes[0].InnerText) + " ";
    //                            SqlDataAdapter das = new SqlDataAdapter(sSQLString, _SQLConn);
    //                            das.Fill(attribuetypeDS);
    //                            if (attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("TEX") == true || attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("DATE") == true)
    //                            {

    //                                if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
    //                                {
    //                                    SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
    //                                }
    //                                else
    //                                {
    //                                    SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
    //                                }
    //                            }
    //                            else if (attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("DECI") == true || attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("NUM") == true)
    //                            {
    //                                if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
    //                                {
    //                                    SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE  (NUMERIC_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
    //                                }
    //                                else
    //                                {
    //                                    SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (NUMERIC_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
    //                                }
    //                            }


    //                        }
    //                        if (TableDataSetNode.ChildNodes[4].InnerText == "NONE")
    //                        {
    //                        }
    //                        if (TableDataSetNode.ChildNodes[4].InnerText == "AND")
    //                        {
    //                            SQLstring.Append(" INTERSECT \n");
    //                        }
    //                        if (TableDataSetNode.ChildNodes[4].InnerText == "OR")
    //                        {
    //                            SQLstring.Append(" UNION \n");
    //                        }

    //                    }

    //                }

    //            }
    //            string productFiltersql = SQLstring.ToString();
    //            // Boolean variableFilter = false;
    //            if (productFiltersql.Length > 0)
    //            {
    //                string s = "SELECT PRODUCT_ID FROM [PRODUCT FAMILY](" + CATALOG_ID + ") WHERE CATALOG_ID=" + CATALOG_ID + " AND PRODUCT_ID IN\n" +
    //                      "(\n";// +
    //                //"SELECT DISTINCT PRODUCT_ID\n" +
    //                //"FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") \n" +
    //                //"WHERE\n";
    //                productFiltersql = s + productFiltersql + "\n)";
    //                SqlDataAdapter dad = new SqlDataAdapter(productFiltersql, _SQLConn);
    //                dad.Fill(oDsProductFilter);

    //                bool available = false;

    //                for (int rowCount = 0; rowCount < flatDataset.Tables[0].Rows.Count; rowCount++)
    //                {//foreach (DataRow odr in flatDataset.Tables[0].Rows)
    //                    DataRow odr = flatDataset.Tables[0].Rows[rowCount];
    //                    available = false;
    //                    foreach (DataRow dr in oDsProductFilter.Tables[0].Rows)
    //                    {
    //                        if (dr["PRODUCT_ID"].ToString() == odr["PRODUCT_ID"].ToString())
    //                        {
    //                            available = true;
    //                        }

    //                    }
    //                    if (available == false)
    //                    {
    //                        string cmdd = " DELETE FROM TBWC_SEARCH_PROD_LIST WHERE PRODUCT_ID = " + odr["PRODUCT_ID"].ToString() + " AND USER_SESSION_ID='" + _USER_SESSION_ID + "'";
    //                        SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
    //                        pscmd.CommandType = CommandType.Text;
    //                        int valr = pscmd.ExecuteNonQuery();
    //                        odr.Delete();
    //                        flatDataset.AcceptChanges();
    //                        rowCount--;
    //                    }

    //                }

    //            }
    //        }
    //        return flatDataset;
    //    }
    //    private DataSet FamilyFilterFlatTable(DataSet flatDataset)
    //    {

    //        {
    //            StringBuilder SQLstring = new StringBuilder();
    //            DataSet oDsFamilyFilter = new DataSet();
    //            string SQLString = " SELECT FAMILY_FILTERS FROM TB_CATALOG WHERE  CATALOG_ID = " + CATALOG_ID + " ";
    //            SqlDataAdapter da = new SqlDataAdapter(SQLString, _SQLConn);
    //            da.Fill(oDsFamilyFilter);
    //            string sFamilyFilter = string.Empty;
    //            if (oDsFamilyFilter.Tables[0].Rows.Count > 0 && oDsFamilyFilter.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
    //            {
    //                sFamilyFilter = oDsFamilyFilter.Tables[0].Rows[0].ItemArray[0].ToString();
    //                XmlDocument xmlDOc = new XmlDocument();
    //                xmlDOc.LoadXml(sFamilyFilter);
    //                XmlNode rNode = xmlDOc.DocumentElement;

    //                if (rNode.ChildNodes.Count > 0)
    //                {
    //                    for (int i = 0; i < rNode.ChildNodes.Count; i++)
    //                    {
    //                        XmlNode TableDataSetNode = rNode.ChildNodes[i];

    //                        if (TableDataSetNode.HasChildNodes)
    //                        {
    //                            if (TableDataSetNode.ChildNodes[2].InnerText == " ")
    //                            {
    //                                TableDataSetNode.ChildNodes[2].InnerText = "=";
    //                            }
    //                            if (TableDataSetNode.ChildNodes[0].InnerText == " ")
    //                            {
    //                                TableDataSetNode.ChildNodes[0].InnerText = "0";
    //                            }
    //                            string stringval = TableDataSetNode.ChildNodes[3].InnerText.Replace("'", "''");
    //                            if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
    //                            {
    //                                SQLstring.Append("SELECT DISTINCT FAMILY_ID FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
    //                            }
    //                            else
    //                            {
    //                                SQLstring.Append("SELECT DISTINCT FAMILY_ID FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ") WHERE  (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
    //                            }


    //                        }
    //                        if (TableDataSetNode.ChildNodes[4].InnerText == "NONE")
    //                        {
    //                        }
    //                        if (TableDataSetNode.ChildNodes[4].InnerText == "AND")
    //                        {
    //                            SQLstring.Append(" INTERSECT \n");
    //                        }
    //                        if (TableDataSetNode.ChildNodes[4].InnerText == "OR")
    //                        {
    //                            SQLstring.Append(" UNION \n");
    //                        }

    //                    }

    //                }

    //            }
    //            string familyFiltersql = SQLstring.ToString();

    //            if (familyFiltersql.Length > 0)
    //            {
    //                string s = "SELECT FAMILY_ID FROM FAMILY(" + CATALOG_ID + ") WHERE CATALOG_ID=" + CATALOG_ID + " AND FAMILY_ID IN\n" +
    //                      "(\n";// +
    //                //"SELECT DISTINCT FAMILY_ID\n" +
    //                //"FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ")\n" +
    //                //"WHERE\n";
    //                familyFiltersql = s + familyFiltersql + "\n)";

    //                SqlDataAdapter dda = new SqlDataAdapter(familyFiltersql, _SQLConn);
    //                dda.Fill(oDsFamilyFilter);

    //                bool available = false;
    //                DataSet AvailableDs = flatDataset;
    //                for (int rowCount = 0; rowCount < flatDataset.Tables[0].Rows.Count; rowCount++)
    //                {//foreach (DataRow odr in flatDataset.Tables[0].Rows)
    //                    DataRow odr = flatDataset.Tables[0].Rows[rowCount];
    //                    available = false;
    //                    foreach (DataRow dr in oDsFamilyFilter.Tables[0].Rows)
    //                    {
    //                        if (dr["FAMILY_ID"].ToString() == odr["FAMILY_ID"].ToString() || dr["FAMILY_ID"].ToString() == odr["SUBFAMILY_ID"].ToString())
    //                        {
    //                            available = true;
    //                        }

    //                    }
    //                    if (available == false)
    //                    {
    //                        //string cmdd = " DELETE FROM TBWC_SEARCH_PROD_LIST WHERE FAMILY_ID = " + odr["FAMILY_ID"].ToString() + " OR FAMILY_ID = " + odr["SUBFAMILY_ID"].ToString() + " AND  USER_SESSION_ID='" + _USER_SESSION_ID + "'";
    //                        //SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
    //                        //pscmd.CommandType = CommandType.Text;
    //                        //int valr = pscmd.ExecuteNonQuery();
    //                        odr.Delete();
    //                        flatDataset.AcceptChanges();
    //                        rowCount--;
    //                    }

    //                }


    //            }

    //        }
    //        //ProductFilterFlatTable(flatDataset);
    //        return flatDataset;
    //    }
    //}

    #endregion
    /*********************************** J TECH CODE ***********************************/
    public  class PowerSearchDB
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
        
        /*********************************** CONSTRUCTOR WITH SQL CONNECTION ***********************************/
        public PowerSearchDB(SqlConnection SQLConn)
        {
            _SQLConn = SQLConn;
        }
        /*********************************** CONSTRUCTOR ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT DETAILS FROM POWER SEARCH DATABASE USING PARAMETERS ***/
        /********************************************************************************/

        //public int ExecutePowerSearchDB()
        //{
        //    //int ict = 0;
        //    SqlDataReader psrdr = null;
        //    try
        //    {

        //        SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH", _SQLConn);
        //        pscmd.CommandType = CommandType.StoredProcedure;
        //        pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATALOG_ID", _CATALOG_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", _CATEGORY_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@SEARCH_STR", _SEARCH_STR.Replace("→", "<ars>g</ars>")));
        //        pscmd.Parameters.Add(new SqlParameter("@FILTER_STR", _FILTER_STR));
        //        pscmd.Parameters.Add(new SqlParameter("@INVENTORY_CHECK", _INVENTORY_CHECK));
        //        pscmd.Parameters.Add(new SqlParameter("@USE_PARAMETRIC_FILTER", _USE_PARAMETRIC_FILTER));
        //        pscmd.Parameters.Add(new SqlParameter("@IS_START_OVER", _IS_START_OVER));
        //        psrdr = pscmd.ExecuteReader();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return 1;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT DETAILS FROM RELATED DATABASE ***/
        /********************************************************************************/

        //public DataSet GetProductsDB()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_GET_PRODUCTS", _SQLConn);
        //        pscmd.CommandType = CommandType.StoredProcedure;
        //        pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATALOG_ID", _CATALOG_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@INVENTORY_CHECK", _INVENTORY_CHECK));
        //        pscmd.Parameters.Add(new SqlParameter("@SORT_BY", _SORT_BY));
        //        pscmd.Parameters.Add(new SqlParameter("@DO_PAGING", _DO_PAGING));
        //        pscmd.Parameters.Add(new SqlParameter("@PAGE_NO", _PAGE_NO));
        //        pscmd.Parameters.Add(new SqlParameter("@RECORDS_PER_PAGE", _RECORDS_PER_PAGE));
        //        pscmd.Parameters.Add(new SqlParameter("@SORT_DIR_ASC", _SORT_DIR_ASC));
        //        pscmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", _CATEGORY_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@BG_CATALOG", _CATALOG_ID));
        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return ds;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE WES PRODUCTS BASED ON PARAMETERS ***/
        /********************************************************************************/

        //public DataSet getwesproductsDB()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_GET_WES_PRODUCTS", _SQLConn);
        //        pscmd.CommandType = CommandType.StoredProcedure;
        //        pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATALOG_ID", _CATALOG_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@INVENTORY_CHECK", _INVENTORY_CHECK));
        //        pscmd.Parameters.Add(new SqlParameter("@SORT_BY", _SORT_BY));
        //        pscmd.Parameters.Add(new SqlParameter("@DO_PAGING", _DO_PAGING));
        //        pscmd.Parameters.Add(new SqlParameter("@PAGE_NO", _PAGE_NO));
        //        pscmd.Parameters.Add(new SqlParameter("@RECORDS_PER_PAGE", _RECORDS_PER_PAGE));
        //        pscmd.Parameters.Add(new SqlParameter("@SORT_DIR_ASC", _SORT_DIR_ASC));
        //        pscmd.Parameters.Add(new SqlParameter("@BG_CATALOG", _CATALOG_ID));
        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return ds;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE CATEGORY DETAILS BASED ON PARAMETERS ***/
        /********************************************************************************/

        //public DataSet GetCategoriesDB()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {

        //        SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_GET_CATEGORY", _SQLConn);
        //        pscmd.CommandType = CommandType.StoredProcedure;
        //        pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATALOG_ID", _CATALOG_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@INVENTORY_CHECK", _INVENTORY_CHECK));
        //        pscmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", _CATEGORY_ID));
        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //        string cmdd = " SELECT * FROM TBWC_SEARCH_PROD_LIST WHERE USER_SESSION_ID = '" + _USER_SESSION_ID + "'";
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        DataSet DDS = new DataSet();
        //        SqlDataAdapter dDDa = new SqlDataAdapter(pscmd);
        //        dDDa.Fill(DDS);
        //        DDS = ProductFilterFlatTable(DDS);

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return ds;
        //}

        //public DataSet GetCategoriesplDB()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        DataSet dsC = new DataSet();
        //        string sql = "SELECT * FROM T2";
        //        SqlCommand dscC = new SqlCommand(sql, _SQLConn);
        //        try
        //        {
        //            SqlDataAdapter dsdC = new SqlDataAdapter(dscC);
        //            dsdC.Fill(dsC);
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        string cmdd = "";
        //        SqlCommand pscmd;
        //        int valr;
        //        if (dsC != null && dsC.Tables.Count > 0)
        //        {
        //            cmdd = " DROP TABLE T2 ";
        //            pscmd = new SqlCommand(cmdd, _SQLConn);
        //            pscmd.CommandType = CommandType.Text;
        //            valr = pscmd.ExecuteNonQuery();
        //        }
        //        cmdd = " SELECT CATEGORY_ID, NULL COUNT INTO T2 FROM TB_cATEGORY WHERE 1=2 ";
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        valr = pscmd.ExecuteNonQuery();
        //        cmdd = " INSERT INTO T2 " +
        //                      " SELECT C.CATEGORY_ID, COUNT(PF.PRODUCT_ID) COUNT  FROM TB_CATEGORY C, TB_FAMILY F, " +
        //                      " TB_PROD_FAMILY PF, TB_CATALOG_FAMILY CF, TBWC_INVENTORY WC WHERE " +
        //                      " C.PARENT_CATEGORY='" + _CATEGORY_ID + "' AND F.CATEGORY_ID=C.CATEGORY_ID AND " +
        //                      " F.FAMILY_ID=PF.FAMILY_ID AND PF.FAMILY_ID=CF.FAMILY_ID AND CATALOG_ID=" + _CATALOG_ID + " " +
        //                      " AND PF.PRODUCT_ID=WC.PRODUCT_ID AND QTY_AVAIL>=" + _INVENTORY_CHECK + " GROUP BY C.CATEGORY_ID " +
        //                      " UNION " +
        //                      " SELECT C1.CATEGORY_ID, COUNT(PF.PRODUCT_ID) COUNT FROM TB_CATEGORY C1, TB_FAMILY F, " +
        //                      " TB_PROD_FAMILY PF, TB_CATEGORY C2, TB_CATALOG_FAMILY CF, TBWC_INVENTORY WC WHERE " +
        //                      " C1.PARENT_CATEGORY='" + _CATEGORY_ID + "' AND C1.CATEGORY_ID=C2.PARENT_CATEGORY AND " +
        //                      " F.CATEGORY_ID=C2.CATEGORY_ID AND  F.FAMILY_ID=PF.FAMILY_ID AND " +
        //                      " PF.FAMILY_ID=CF.FAMILY_ID AND CF.CATALOG_ID=" + _CATALOG_ID + " AND PF.PRODUCT_ID=WC.PRODUCT_ID " +
        //                      " AND QTY_AVAIL>=" + _INVENTORY_CHECK + " GROUP BY C1.CATEGORY_ID ";
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        valr = pscmd.ExecuteNonQuery();
        //        cmdd = " SELECT DISTINCT C.*, (SELECT CONVERT(NVARCHAR, SUM(COUNT)) AS " +
        //                    " CATEGORY_NAME_WITH_COUNT FROM T2 WHERE CATEGORY_ID=A.CATEGORY_ID) CATEGORY_NAME_WITH_COUNT FROM T2 A, " +
        //                    " TB_CATEGORY C WHERE A.CATEGORY_ID=C.CATEGORY_ID AND ISNULL(C.CUSTOM_NUM_FIELD3,0)<> 3 ";
        //        if (HttpContext.Current.Session["PARAFILTER"] == "Value")
        //        {
        //            //ps.FILTER_STR 4^American Scholar|339^Blue                    
        //            cmdd = cmdd + " and C.CATEGORY_ID in (select category_id from tb_family where family_id in" +
        //                      " (select family_id from tb_prod_family where product_id in" +
        //                      " (select product_id from TBWC_SEARCH_PROD_LIST where user_session_id = '" + _USER_SESSION_ID + "'))) ";


        //        }

        //        //"SELECT DISTINCT C.CATEGORY_ID, C.CATEGORY_NAME, C.PARENT_CATEGORY, C.SHORT_DESC, C.IMAGE_FILE, C.IMAGE_TYPE, C.IMAGE_NAME, C.CUSTOM_TEXT_FIELD1, CONVERT(NVARCHAR, COUNT(*)) AS CATEGORY_NAME_WITH_COUNT " +
        //        //                              " FROM	TB_CATEGORY C WITH (NOLOCK), TBWC_INVENTORY I WITH (NOLOCK), TB_CATALOG_FAMILY CF WITH (NOLOCK), TB_PROD_FAMILY PF WITH (NOLOCK) " +
        //        //                              " WHERE	C.CATEGORY_ID = CF.CATEGORY_ID AND " +
        //        //                              " CF.FAMILY_ID = PF.FAMILY_ID AND " +
        //        //                              " PF.PRODUCT_ID = I.PRODUCT_ID AND " +
        //        //                              " I.QTY_AVAIL >= " + _INVENTORY_CHECK + " AND " +
        //        //                              " CF.CATALOG_ID = " + _CATALOG_ID + " AND	" +
        //        //                              "	C.parent_CATEGORY ='" + _CATEGORY_ID + "'" +
        //        //                              " GROUP BY C.CATEGORY_ID, C.CATEGORY_NAME , C.PARENT_CATEGORY, C.SHORT_DESC, C.IMAGE_FILE, C.IMAGE_TYPE, C.IMAGE_NAME, C.CUSTOM_TEXT_FIELD1, C.CATEGORY_NAME " +
        //        //                              " ORDER BY C.CATEGORY_NAME ";
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //        cmdd = " DELETE FROM TBWC_SEARCH_PROD_LIST WHERE USER_SESSION_ID='" + _USER_SESSION_ID + "'";
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        valr = pscmd.ExecuteNonQuery();
        //        cmdd = " INSERT INTO TBWC_SEARCH_PROD_LIST SELECT FAMILY_ID AS PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM TB_FAMILY WHERE FAMILY_ID IN " +
        //               " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //               " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + _CATEGORY_ID + "'" +
        //               " UNION " +
        //               " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'" +
        //               " UNION " +
        //               " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //               " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'))) " +
        //               " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + _CATALOG_ID + ")";
        //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true ||
        //           HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx") == true)
        //        {
        //            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true)
        //            {
        //                if (HttpContext.Current.Request.QueryString["sl1"] != null && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl1"].ToString() != "" && HttpContext.Current.Request.QueryString["sl2"].ToString() != "" && HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request.QueryString["tsb"].ToString() != "" && HttpContext.Current.Request.QueryString["tsm"] != null && HttpContext.Current.Request.QueryString["tsm"].ToString() != "")
        //                {
        //                    //cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID='" + _CATEGORY_ID + "' AND SUBCATNAME_L1='BRAND' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND SUBCATID_L3='" + HttpContext.Current.Request.QueryString["sl3"].ToString() + "' AND FAMILY_ID=" + HttpContext.Current.Request.QueryString["pfid"].ToString() + " AND SUBFAMILY_ID=" + HttpContext.Current.Request.QueryString["fid"].ToString() + " AND PRODUCT_ID IS NOT NULL";
        //                    if (HttpContext.Current.Request.QueryString["sl2"].ToString() != "0")
        //                        cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 = '" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2 = '" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND CATEGORY_ID=N'" + _CATEGORY_ID + "' AND CATALOG_ID=" + _CATALOG_ID + " AND TOSUITE_BRAND = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "' AND TOSUITE_MODEL = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "'";
        //                    else
        //                        cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE SUBCATID_L1 = '" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2 = '' AND CATEGORY_ID=N'" + _CATEGORY_ID + "' AND CATALOG_ID=" + _CATALOG_ID + " AND TOSUITE_BRAND = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "' AND TOSUITE_MODEL = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "'";
        //                }
        //                else if (HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request.QueryString["tsb"].ToString() != "" && HttpContext.Current.Request.QueryString["tsm"] != null && HttpContext.Current.Request.QueryString["tsm"].ToString() != "")
        //                {
        //                    cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID=N'" + _CATEGORY_ID + "' AND CATALOG_ID=" + _CATALOG_ID + " AND TOSUITE_BRAND = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "' AND TOSUITE_MODEL = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "'";
        //                }
        //                else
        //                {
        //                    cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID=N'" + _CATEGORY_ID + "' AND CATALOG_ID=" + _CATALOG_ID + " AND TOSUITE_BRAND = '" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "'";
        //                }
        //                //else
        //                //{
        //                //    cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID='" + _CATEGORY_ID + "' AND SUBCATNAME_L1='BRAND' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND SUBCATID_L3='" + HttpContext.Current.Request.QueryString["sl3"].ToString() + "' AND PRODUCT_ID IS NOT NULL";
        //                //}
        //            }
        //            else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx") == true && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl2"].ToString() != "")
        //            {
        //                if (HttpContext.Current.Request.QueryString["fid"] != null && HttpContext.Current.Request.QueryString["fid"].ToString() != "")
        //                {
        //                    cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID='" + _CATEGORY_ID + "' AND SUBCATNAME_L1='PRODUCT' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND FAMILY_ID=" + HttpContext.Current.Request.QueryString["fid"].ToString() + " AND PRODUCT_ID IS NOT NULL";
        //                }
        //                else
        //                {
        //                    cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM WESTB_TOSUITE_DATA WHERE CATEGORY_ID='" + _CATEGORY_ID + "' AND SUBCATNAME_L1='PRODUCT' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND PRODUCT_ID IS NOT NULL";
        //                }
        //            }
        //            else
        //            {

        //                if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == false &&
        //                    HttpContext.Current.Request.Url.ToString().ToLower().Contains("byproduct.aspx") == false)
        //                {
        //                    cmdd = " INSERT INTO TBWC_SEARCH_PROD_LIST SELECT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM TB_PROD_FAMILY WHERE FAMILY_ID IN " +
        //                          " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //                          " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + _CATEGORY_ID + "'" +
        //                          " UNION " +
        //                          " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'" +
        //                          " UNION " +
        //                          " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //                          " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'))) " +
        //                          " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + _CATALOG_ID + ")";
        //                    if (HttpContext.Current.Request.QueryString["fid"] != null && HttpContext.Current.Request.QueryString["fid"].ToString() != "")
        //                    {
        //                        cmdd = " INSERT INTO TBWC_SEARCH_PROD_LIST SELECT PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM TB_PROD_FAMILY WHERE FAMILY_ID IN " +
        //                         " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //                         " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + _CATEGORY_ID + "'" +
        //                         " UNION " +
        //                         " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'" +
        //                         " UNION " +
        //                         " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //                         " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'))) " +
        //                         " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + _CATALOG_ID + ") AND FAMILY_ID=" + HttpContext.Current.Request.QueryString["fid"].ToString();
        //                    }
        //                }
        //            }
        //        }
        //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "" && HttpContext.Current.Request["sl2"] != null && HttpContext.Current.Request["sl2"].ToString() != "")
        //        {
        //            cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT FAMILY_ID AS PRODUCT_ID,'" + _USER_SESSION_ID + "' FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID=N'" + _CATEGORY_ID + "' AND CATALOG_ID=" + CATALOG_ID + " AND  SUBCATID_L1 = '" + HttpContext.Current.Request["sl1"].ToString() + "' AND SUBCATID_L2 = '" + HttpContext.Current.Request["sl2"].ToString() + "'";
        //        }
        //        else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "")
        //        {
        //            cmdd = "INSERT INTO TBWC_SEARCH_PROD_LIST SELECT DISTINCT FAMILY_ID AS PRODUCT_ID,'" + _USER_SESSION_ID + "' FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE category_id=N'" + _CATEGORY_ID + "' AND CATALOG_ID=" + CATALOG_ID + " AND  SUBCATID_L1 = '" + HttpContext.Current.Request["sl1"].ToString() + "'";
        //        }
        //        if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true && HttpContext.Current.Request["sb"] != null && HttpContext.Current.Request["sb"].ToString() != "" && HttpContext.Current.Request["sm"] != null && HttpContext.Current.Request["sm"].ToString() != "")
        //        {
        //            cmdd = " INSERT INTO TBWC_SEARCH_PROD_LIST SELECT FAMILY_ID AS PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM TB_FAMILY WHERE FAMILY_ID IN " +
        //               " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //               " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + _CATEGORY_ID + "'" +
        //               " UNION " +
        //               " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'" +
        //               " UNION " +
        //               " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //               " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'))) " +
        //               " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + _CATALOG_ID + ") AND FAMILY_ID IN(SELECT DISTINCT FAMILY_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + HttpContext.Current.Request["pcr"].ToString() + "' AND TOSUITE_BRAND='" + HttpUtility.UrlDecode(HttpContext.Current.Request["sb"].ToString()) + "' AND TOSUITE_MODEL='" + HttpUtility.UrlDecode(HttpContext.Current.Request["sm"].ToString()) + "')";
        //        }
        //        else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx") == true && HttpContext.Current.Request["sb"] != null && HttpContext.Current.Request["sb"].ToString() != "")
        //        {
        //            cmdd = " INSERT INTO TBWC_SEARCH_PROD_LIST SELECT FAMILY_ID AS PRODUCT_ID,'" + _USER_SESSION_ID + "'  AS USER_SESSION_ID  FROM TB_FAMILY WHERE FAMILY_ID IN " +
        //               " (SELECT FAMILY_ID FROM TB_FAMILY WHERE CATEGORY_ID IN " +
        //               " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID='" + _CATEGORY_ID + "'" +
        //               " UNION " +
        //               " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'" +
        //               " UNION " +
        //               " SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY IN " +
        //               " (SELECT CATEGORY_ID FROM TB_CATEGORY WHERE PARENT_CATEGORY='" + _CATEGORY_ID + "'))) " +
        //               " AND FAMILY_ID IN (SELECT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATALOG_ID=" + _CATALOG_ID + ") AND FAMILY_ID IN(SELECT DISTINCT FAMILY_ID FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + HttpContext.Current.Request["pcr"].ToString() + "' AND TOSUITE_BRAND='" + HttpUtility.UrlDecode(HttpContext.Current.Request["sb"].ToString()) + "')";
        //        }
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        valr = pscmd.ExecuteNonQuery();
        //        cmdd = " SELECT * FROM TBWC_SEARCH_PROD_LIST WHERE USER_SESSION_ID = '" + _USER_SESSION_ID + "'";
        //        pscmd = new SqlCommand(cmdd, _SQLConn);
        //        pscmd.CommandType = CommandType.Text;
        //        DataSet DDS = new DataSet();
        //        SqlDataAdapter dDDa = new SqlDataAdapter(pscmd);
        //        dDDa.Fill(DDS);
        //        DDS = ProductFilterFlatTable(DDS);

        //        if (HttpContext.Current.Session["PARAFILTER"] == "Value")
        //        {
        //            ApplyParametricFiltersDB();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return ds;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE FAMILY RELATED DETAILS ***/
        /********************************************************************************/

        //public DataSet GetFamiliesDB()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_GET_FAMILY", _SQLConn);
        //        pscmd.CommandType = CommandType.StoredProcedure;
        //        pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATALOG_ID", _CATALOG_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@INVENTORY_CHECK", _INVENTORY_CHECK));
        //        pscmd.Parameters.Add(new SqlParameter("@SORT_BY", _SORT_BY));
        //        pscmd.Parameters.Add(new SqlParameter("@DO_PAGING", _DO_PAGING));
        //        pscmd.Parameters.Add(new SqlParameter("@PAGE_NO", _PAGE_NO));
        //        pscmd.Parameters.Add(new SqlParameter("@RECORDS_PER_PAGE", _RECORDS_PER_PAGE));
        //        pscmd.Parameters.Add(new SqlParameter("@SORT_DIR_ASC", _SORT_DIR_ASC));
        //        pscmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", _CATEGORY_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@BG_CATALOG", _CATALOG_ID));
        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return ds;
        //}


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE CATEGORY NAME FROM RELETED DATABASE ***/
        /********************************************************************************/

        //public string GetCategorynameDB()
        //{//
        //    //DataSet ds = new DataSet();
        //    //try
        //    //{
        //    //    string cmdd = "SELECT CATEGORY_NAME " +
        //    //                                      " FROM	TB_CATEGORY " +
        //    //                                      " WHERE	CATEGORY_ID ='" + _CATEGORY_ID + "'";
        //    //    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
        //    //    pscmd.CommandType = CommandType.Text;
        //    //    SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //    //    da.Fill(ds);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    objErrorHandler.ErrorMsg = ex;
        //    //    objErrorHandler.CreateLog();
        //    //}
        //    //if (ds.Tables[0].Rows.Count > 0)
        //    //    return ds.Tables[0].Rows[0].ItemArray[0].ToString();
        //    //return "";
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand pscmd = new SqlCommand("STP_TBWC_PICK_WES_POWERSEARCH_GET_CATEGORYNAME", _SQLConn) { CommandType = CommandType.StoredProcedure };
        //        pscmd.Parameters.AddWithValue("@CATEGORY_ID", _CATEGORY_ID);
        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //    }
        //    catch (Exception objException)
        //    {
        //        objErrorHandler.ErrorMsg = objException;
        //        objErrorHandler.CreateLog();
        //    }
        //    finally
        //    {
        //    }
        //    if (ds.Tables[0].Rows.Count > 0)
        //        return ds.Tables[0].Rows[0][0].ToString();
        //    return "";
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE UNAVAILABLE PRODUCTS DETAILS ***/
        /********************************************************************************/

        //public DataSet GetNotAvailableProdDB()
        //{
        //    //DataSet ds = new DataSet();
        //    //try
        //    //{
        //    //    string cmdd = "select product_id from tbwc_inventory where product_status <> 'AVAILABLE'";
        //    //    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
        //    //    pscmd.CommandType = CommandType.Text;
        //    //    SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //    //    da.Fill(ds);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    objErrorHandler.ErrorMsg = ex;
        //    //    objErrorHandler.CreateLog();
        //    //}

        //    //return ds;
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("STP_TBWC_PICK_WES_GET_NOTAVAILABLE_PRODUCTS", _SQLConn) { CommandType = CommandType.StoredProcedure });
        //        da.Fill(ds);
        //    }
        //    catch (Exception objException)
        //    {
        //        objErrorHandler.ErrorMsg = objException;
        //        objErrorHandler.CreateLog();
        //    }
          

        //    return ds;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE BEST SELLER ATTRIBUTE DETAILS ***/
        /********************************************************************************/

        //public string GetBestSellerDB()
        //{
        //    //DataSet ds = new DataSet();
        //    //try
        //    //{
        //    //    string cmdd = "SELECT ATTRIBUTE_ID " +
        //    //                                      " FROM	TB_ATTRIBUTE " +
        //    //                                      " WHERE	ATTRIBUTE_NAME ='Best Seller'";
        //    //    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
        //    //    pscmd.CommandType = CommandType.Text;
        //    //    SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //    //    da.Fill(ds);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    objErrorHandler.ErrorMsg = ex;
        //    //    objErrorHandler.CreateLog();
        //    //}
        //    //if (ds.Tables[0].Rows.Count > 0)
        //    //    return ds.Tables[0].Rows[0].ItemArray[0].ToString();
        //    //return "";
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //         SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("STP_TBWC_PICK_WES_POWERSEARCH_GET_BESTATTRIBUTE", _SQLConn) { CommandType = CommandType.StoredProcedure });
        //        da.Fill(ds);
        //    }
        //    catch (Exception objException)
        //    {
        //        objErrorHandler.ErrorMsg = objException;
        //        objErrorHandler.CreateLog();
        //    }
        //    finally
        //    {
        //    }
        //    if (ds.Tables[0].Rows.Count > 0)
        //        return ds.Tables[0].Rows[0].ItemArray[0].ToString();
        //    return "";
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE FEATURED PRODUCTS DETAILS ***/
        /********************************************************************************/

        //public DataSet GetFeaturedProductDB()
        //{
        //    //DataSet ds = new DataSet();
        //    //try
        //    //{
        //    //    string cmdd = "SELECT STRING_VALUE,PRODUCT_ID,ATTRIBUTE_ID,NUMERIC_VALUE,OBJECT_TYPE,[OBJECT_NAME],(SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = PS.ATTRIBUTE_ID) AS ATTRIBUTE_TYPE FROM TB_PROD_SPECS PS  WHERE PRODUCT_ID IN (SELECT PRODUCT_ID " +
        //    //                                      " FROM	TBWC_FEATURED_PRODUCT" +
        //    //                                      " WHERE	CATEGORY_ID ='" + _CATEGORY_ID + "')";
        //    //    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
        //    //    pscmd.CommandType = CommandType.Text;
        //    //    SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //    //    da.Fill(ds);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    objErrorHandler.ErrorMsg = ex;
        //    //    objErrorHandler.CreateLog();
        //    //}
        //    //return ds;
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand pscmd = new SqlCommand("STP_WES_GET_FEATURED_PRODUCTS", _SQLConn) { CommandType = CommandType.StoredProcedure };
        //        pscmd.Parameters.AddWithValue("@CATEGORY_ID", _CATEGORY_ID);
        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //    }
        //    catch (Exception objException)
        //    {
        //        objErrorHandler.ErrorMsg = objException;
        //        objErrorHandler.CreateLog();
        //    }
           
           
        //    return ds;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE POWER SEARCH NARROW FIELDS DETAILS ***/
        /********************************************************************************/

        //public DataSet GetNarrowFieldsDB()
        //{
        //    //DataSet ds = new DataSet();
        //    //try
        //    //{
        //    //    string cmdd = "select attribute_id from tbwc_search_narrow_fields order by sort_order";
        //    //    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
        //    //    pscmd.CommandType = CommandType.Text;
        //    //    SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //    //    da.Fill(ds);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    objErrorHandler.ErrorMsg = ex;
        //    //    objErrorHandler.CreateLog();
        //    //}
        //    //return ds;
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("STP_WES_GET_NARROW_FIELDS", _SQLConn) { CommandType = CommandType.StoredProcedure });
        //        da.Fill(ds);
        //    }
        //    catch (Exception objException)
        //    {
        //        objErrorHandler.ErrorMsg = objException;
        //        objErrorHandler.CreateLog();
        //    }
        
        //    return ds;
        //}

        //public void ApplyParametricFiltersDB()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        if (System.Web.HttpContext.Current.Request.Url.ToString().ToUpper().Contains("PL"))
        //        {
        //            if (System.Web.HttpContext.Current.Session["filter"] == null || System.Web.HttpContext.Current.Session["filter"] == "")
        //                System.Web.HttpContext.Current.Session["filter"] = _FILTER_STR;

        //            else
        //            {
        //                System.Web.HttpContext.Current.Session["filter"] = System.Web.HttpContext.Current.Session["filter"] + "|" + _FILTER_STR;
        //                _FILTER_STR = System.Web.HttpContext.Current.Session["filter"].ToString();
        //            }
        //        }
        //        else
        //        {
        //            System.Web.HttpContext.Current.Session["filter"] = null;
        //        }
        //        SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_APPLY_PARAMETRIC", _SQLConn);
        //        pscmd.CommandType = CommandType.StoredProcedure;
        //        pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@FILTER", _FILTER_STR));
        //        pscmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }

        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PARAMETRIC FILTERS DETAILS FROM RELATED DATABASE ***/
        /********************************************************************************/

        //public DataSet GetParametricFiltersDB()
        //{
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_GET_PARAMETRIC_FILTERS", _SQLConn);
        //        pscmd.CommandType = CommandType.StoredProcedure;
        //        pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@CATALOG_ID", _CATALOG_ID));
        //        pscmd.Parameters.Add(new SqlParameter("@INVENTORY_CHECK", _INVENTORY_CHECK));
        //        pscmd.Parameters.Add(new SqlParameter("@CATEGORY_ID", _CATEGORY_ID));

        //        SqlDataAdapter da = new SqlDataAdapter(pscmd);
        //        da.Fill(ds);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return ds;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CLEAR THE SEARCH RESUTS  ***/
        /********************************************************************************/

        public int ClearSearchResultsDB()
        {
            int result = -1;
            try
            {
                SqlCommand pscmd = new SqlCommand("STP_WES_POWER_SEARCH_CLEAR", _SQLConn);
                pscmd.CommandType = CommandType.StoredProcedure;
                pscmd.Parameters.Add(new SqlParameter("@USER_SESSION_ID", _USER_SESSION_ID));
                result = Convert.ToInt32(pscmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }
            return result;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CLEAR FILTERD PRODUCTS FROM RELATED DATABASE  ***/
        /********************************************************************************/

        //public int ClearFilterproductDB(string sqlquery)
        //{
        //    int result = -1;
        //    try
        //    {
        //        SqlCommand pscmd = new SqlCommand(sqlquery, _SQLConn);
        //        result = Convert.ToInt32(pscmd.ExecuteNonQuery());
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        return -1;
        //    }
        //    return result;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT FILTER DETAILS FROM CATALOG TABLE ***/
        /********************************************************************************/
        //private DataSet ProductFilterFlatTable(DataSet flatDataset)
        //{
        //    {
        //        StringBuilder SQLstring = new StringBuilder();
        //        DataSet oDsProductFilter = new DataSet();
        //        string SQLString = " SELECT PRODUCT_FILTERS FROM TB_CATALOG WHERE  CATALOG_ID = " + CATALOG_ID + " ";
        //        SqlDataAdapter da = new SqlDataAdapter(SQLString, _SQLConn);
        //        da.Fill(oDsProductFilter);
        //        string sProductFilter = string.Empty;
        //        if (oDsProductFilter.Tables[0].Rows.Count > 0 && oDsProductFilter.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
        //        {
        //            sProductFilter = oDsProductFilter.Tables[0].Rows[0].ItemArray[0].ToString();
        //            XmlDocument xmlDOc = new XmlDocument();
        //            xmlDOc.LoadXml(sProductFilter);
        //            XmlNode rNode = xmlDOc.DocumentElement;

        //            if (rNode.ChildNodes.Count > 0)
        //            {
        //                for (int i = 0; i < rNode.ChildNodes.Count; i++)
        //                {
        //                    XmlNode TableDataSetNode = rNode.ChildNodes[i];

        //                    if (TableDataSetNode.HasChildNodes)
        //                    {
        //                        if (TableDataSetNode.ChildNodes[2].InnerText == " ")
        //                        {
        //                            TableDataSetNode.ChildNodes[2].InnerText = "=";
        //                        }
        //                        if (TableDataSetNode.ChildNodes[0].InnerText == " ")
        //                        {
        //                            TableDataSetNode.ChildNodes[0].InnerText = "0";
        //                        }
        //                        string stringval = TableDataSetNode.ChildNodes[3].InnerText.Replace("'", "''");
        //                        DataSet attribuetypeDS = new DataSet();
        //                        string sSQLString = " SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE  ATTRIBUTE_ID = " + Convert.ToInt32(TableDataSetNode.ChildNodes[0].InnerText) + " ";
        //                        SqlDataAdapter das = new SqlDataAdapter(sSQLString, _SQLConn);
        //                        das.Fill(attribuetypeDS);
        //                        if (attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("TEX") == true || attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("DATE") == true)
        //                        {

        //                            if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
        //                            {
        //                                SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
        //                            }
        //                            else
        //                            {
        //                                SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
        //                            }
        //                        }
        //                        else if (attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("DECI") == true || attribuetypeDS.Tables[0].Rows[0].ItemArray[0].ToString().ToUpper().Contains("NUM") == true)
        //                        {
        //                            if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
        //                            {
        //                                SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE  (NUMERIC_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
        //                            }
        //                            else
        //                            {
        //                                SQLstring.Append("SELECT DISTINCT PRODUCT_ID FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") WHERE (NUMERIC_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
        //                            }
        //                        }


        //                    }
        //                    if (TableDataSetNode.ChildNodes[4].InnerText == "NONE")
        //                    {
        //                    }
        //                    if (TableDataSetNode.ChildNodes[4].InnerText == "AND")
        //                    {
        //                        SQLstring.Append(" INTERSECT \n");
        //                    }
        //                    if (TableDataSetNode.ChildNodes[4].InnerText == "OR")
        //                    {
        //                        SQLstring.Append(" UNION \n");
        //                    }

        //                }

        //            }

        //        }
        //        string productFiltersql = SQLstring.ToString();
        //        // Boolean variableFilter = false;
        //        if (productFiltersql.Length > 0)
        //        {
        //            string s = "SELECT PRODUCT_ID FROM [PRODUCT FAMILY](" + CATALOG_ID + ") WHERE CATALOG_ID=" + CATALOG_ID + " AND PRODUCT_ID IN\n" +
        //                  "(\n";// +
        //            //"SELECT DISTINCT PRODUCT_ID\n" +
        //            //"FROM [PRODUCT SPECIFICATION](" + CATALOG_ID + ") \n" +
        //            //"WHERE\n";
        //            productFiltersql = s + productFiltersql + "\n)";
        //            SqlDataAdapter dad = new SqlDataAdapter(productFiltersql, _SQLConn);
        //            dad.Fill(oDsProductFilter);

        //            bool available = false;

        //            for (int rowCount = 0; rowCount < flatDataset.Tables[0].Rows.Count; rowCount++)
        //            {//foreach (DataRow odr in flatDataset.Tables[0].Rows)
        //                DataRow odr = flatDataset.Tables[0].Rows[rowCount];
        //                available = false;
        //                foreach (DataRow dr in oDsProductFilter.Tables[0].Rows)
        //                {
        //                    if (dr["PRODUCT_ID"].ToString() == odr["PRODUCT_ID"].ToString())
        //                    {
        //                        available = true;
        //                    }

        //                }
        //                if (available == false)
        //                {
        //                    string cmdd = " DELETE FROM TBWC_SEARCH_PROD_LIST WHERE PRODUCT_ID = " + odr["PRODUCT_ID"].ToString() + " AND USER_SESSION_ID='" + _USER_SESSION_ID + "'";
        //                    SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
        //                    pscmd.CommandType = CommandType.Text;
        //                    int valr = pscmd.ExecuteNonQuery();
        //                    odr.Delete();
        //                    flatDataset.AcceptChanges();
        //                    rowCount--;
        //                }

        //            }

        //        }
        //    }
        //    return flatDataset;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE FAMILY FILTER DETAILS FROM CATALOG TABLE ***/
        /********************************************************************************/

        //private DataSet FamilyFilterFlatTable(DataSet flatDataset)
        //{

        //    {
        //        StringBuilder SQLstring = new StringBuilder();
        //        DataSet oDsFamilyFilter = new DataSet();
        //        string SQLString = " SELECT FAMILY_FILTERS FROM TB_CATALOG WHERE  CATALOG_ID = " + CATALOG_ID + " ";
        //        SqlDataAdapter da = new SqlDataAdapter(SQLString, _SQLConn);
        //        da.Fill(oDsFamilyFilter);
        //        string sFamilyFilter = string.Empty;
        //        if (oDsFamilyFilter.Tables[0].Rows.Count > 0 && oDsFamilyFilter.Tables[0].Rows[0].ItemArray[0].ToString() != string.Empty)
        //        {
        //            sFamilyFilter = oDsFamilyFilter.Tables[0].Rows[0].ItemArray[0].ToString();
        //            XmlDocument xmlDOc = new XmlDocument();
        //            xmlDOc.LoadXml(sFamilyFilter);
        //            XmlNode rNode = xmlDOc.DocumentElement;

        //            if (rNode.ChildNodes.Count > 0)
        //            {
        //                for (int i = 0; i < rNode.ChildNodes.Count; i++)
        //                {
        //                    XmlNode TableDataSetNode = rNode.ChildNodes[i];

        //                    if (TableDataSetNode.HasChildNodes)
        //                    {
        //                        if (TableDataSetNode.ChildNodes[2].InnerText == " ")
        //                        {
        //                            TableDataSetNode.ChildNodes[2].InnerText = "=";
        //                        }
        //                        if (TableDataSetNode.ChildNodes[0].InnerText == " ")
        //                        {
        //                            TableDataSetNode.ChildNodes[0].InnerText = "0";
        //                        }
        //                        string stringval = TableDataSetNode.ChildNodes[3].InnerText.Replace("'", "''");
        //                        if (TableDataSetNode.ChildNodes[4].InnerText != "NONE")
        //                        {
        //                            SQLstring.Append("SELECT DISTINCT FAMILY_ID FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ") WHERE (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ") " + "\n");
        //                        }
        //                        else
        //                        {
        //                            SQLstring.Append("SELECT DISTINCT FAMILY_ID FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ") WHERE  (STRING_VALUE " + TableDataSetNode.ChildNodes[2].InnerText + " '" + stringval + "' AND ATTRIBUTE_ID = " + TableDataSetNode.ChildNodes[0].InnerText + ")" + "\n");
        //                        }


        //                    }
        //                    if (TableDataSetNode.ChildNodes[4].InnerText == "NONE")
        //                    {
        //                    }
        //                    if (TableDataSetNode.ChildNodes[4].InnerText == "AND")
        //                    {
        //                        SQLstring.Append(" INTERSECT \n");
        //                    }
        //                    if (TableDataSetNode.ChildNodes[4].InnerText == "OR")
        //                    {
        //                        SQLstring.Append(" UNION \n");
        //                    }

        //                }

        //            }

        //        }
        //        string familyFiltersql = SQLstring.ToString();

        //        if (familyFiltersql.Length > 0)
        //        {
        //            string s = "SELECT FAMILY_ID FROM FAMILY(" + CATALOG_ID + ") WHERE CATALOG_ID=" + CATALOG_ID + " AND FAMILY_ID IN\n" +
        //                  "(\n";// +
        //            //"SELECT DISTINCT FAMILY_ID\n" +
        //            //"FROM [FAMILY DESCRIPTION](" + CATALOG_ID + ")\n" +
        //            //"WHERE\n";
        //            familyFiltersql = s + familyFiltersql + "\n)";

        //            SqlDataAdapter dda = new SqlDataAdapter(familyFiltersql, _SQLConn);
        //            dda.Fill(oDsFamilyFilter);

        //            bool available = false;
        //            DataSet AvailableDs = flatDataset;
        //            for (int rowCount = 0; rowCount < flatDataset.Tables[0].Rows.Count; rowCount++)
        //            {//foreach (DataRow odr in flatDataset.Tables[0].Rows)
        //                DataRow odr = flatDataset.Tables[0].Rows[rowCount];
        //                available = false;
        //                foreach (DataRow dr in oDsFamilyFilter.Tables[0].Rows)
        //                {
        //                    if (dr["FAMILY_ID"].ToString() == odr["FAMILY_ID"].ToString() || dr["FAMILY_ID"].ToString() == odr["SUBFAMILY_ID"].ToString())
        //                    {
        //                        available = true;
        //                    }

        //                }
        //                if (available == false)
        //                {
        //                    //string cmdd = " DELETE FROM TBWC_SEARCH_PROD_LIST WHERE FAMILY_ID = " + odr["FAMILY_ID"].ToString() + " OR FAMILY_ID = " + odr["SUBFAMILY_ID"].ToString() + " AND  USER_SESSION_ID='" + _USER_SESSION_ID + "'";
        //                    //SqlCommand pscmd = new SqlCommand(cmdd, _SQLConn);
        //                    //pscmd.CommandType = CommandType.Text;
        //                    //int valr = pscmd.ExecuteNonQuery();
        //                    odr.Delete();
        //                    flatDataset.AcceptChanges();
        //                    rowCount--;
        //                }

        //            }


        //        }

        //    }
        //    //ProductFilterFlatTable(flatDataset);
        //    return flatDataset;
        //}
    }
    /*********************************** J TECH CODE ***********************************/
}