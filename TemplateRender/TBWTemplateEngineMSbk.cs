using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using StringTemplate = Antlr3.ST.StringTemplate;  
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
//using TradingBell.Common;
//using TradingBell.WebServices;
using System.Xml;
using TradingBell.WebCat.Helpers;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.CommonServices;
using TradingBell.WebCat.EasyAsk ;
using System.Configuration;
using System.Collections;
namespace TradingBell.WebCat.TemplateRender
{
    /*********************************** J TECH CODE ***********************************/
    public class TBWTemplateEngineMS
    {
        /*********************************** DECLARATION ***********************************/
        EasyAsk_WAGNER EasyAsk = new EasyAsk_WAGNER();
        ErrorHandler objErrorHandler = new ErrorHandler();
        OrderServices objOrderServices = new OrderServices();
        HelperServices objHelperServices = new HelperServices();
        FamilyServices objFamilyServices = new FamilyServices();
        Dictionary<string, TBWDataList[]> _dict_inner_html = new Dictionary<string, TBWDataList[]>();

        private StringTemplateGroup _stg_container = null;
        private StringTemplateGroup _stg_records = null;
        private StringTemplate _stmpl_container = null;
        private StringTemplate _stmpl_records = null;

        private string _SkinRootPath = null;
        private string _SkinRoot_container = null;
        private string _SkinRoot_records = null;

        private string _RenderedHTML = null; //for this use the big string variable
        private string _DBConnectionString = null;
        private string _Package = null;

        //Get the following from TBWC_PACKAGE table 
        private string _skin_container = null;
        private int _grid_cols = 1;
        private int _grid_rows = 1;
        private string _skin_sql_container = null;
        private string _skin_sql_type_container = null;
        private string _skin_sql_param_container = null;
        private int _package_order = 0;
        private string _skin_body_attribute = null;

        private string _skin_records = null;
        private string _skin_sql_records = null;
        private string _skin_sql_type_records = null;
        private string _skin_sql_param_records = null;
        private DataSet _GDataSet = null;
        public string paraValue = "";
        public string paraPID = "";
        public string paraFID = "";
        public string paraCID = "";
        private string _cartitem = "";
        private string _CATALOG_ID = "";
        private HelperServices objHelperService = new HelperServices();
        private Security objSecurity = new Security();
        private HelperDB objHelperDB = new HelperDB();
        private ConnectionDB objConnectionDB = new ConnectionDB();
        string _fid = "";
        string downloadST="";
        bool isdownload=false;
        bool chkdwld = false;
        public string strsupplierName = "";
        public string strsupplierDesc = "";
        public string strsupplierId = "";
        public string WesNewsCategoryId = System.Configuration.ConfigurationManager.AppSettings["WESNEWS_CATEGORY_ID"].ToString();
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();
        string strProdImages = HttpContext.Current.Server.MapPath("ProdImages");
        const string EnDekey = "WAGNER@PayPalPAY@dm1n@123";
        //END TBWC_PACKAGE table info

        public string cartitem
        {
            get
            {
                return _cartitem;
            }
            set
            {
                _cartitem = value;
            }
        }

        public string CATALOG_ID
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

        public string DBConnectionString
        {
            get
            {
                return _DBConnectionString;
            }
            set
            {
                _DBConnectionString = value;
            }
        }

        public string Package
        {
            get
            {
                return _Package;
            }
            set
            {
                _Package = value;
            }
        }

        public string SkinRootPath
        {
            get
            {
                return _SkinRootPath;
            }
            set
            {
                _SkinRootPath = value;
            }
        }

        public string RenderedHTML
        {
            get
            {
                return objHelperService.StripWhitespace(_RenderedHTML);
            }
        }

        public DataSet GDataSet
        {
            get
            {
                return _GDataSet;
            }
            set
            {
                _GDataSet = value;
            }
        }

        
        /*********************************** DECLARATION ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PACKAGE,SKIN NAME AND DATABASE CONNECTION STRING  DETAILS ***/
        /********************************************************************************/
        public TBWTemplateEngineMS(string Package, string SkinRootPath, string DBConnectionString)
        {
            _Package = Package;
            _SkinRootPath = SkinRootPath;
            //_DBConnectionString = DBConnectionString.Substring(DBConnectionString.IndexOf(';') + 1);
            //DataSet ds = new DataSet();
            //SqlDataAdapter da = new SqlDataAdapter("select convert(int, option_value) as OPTION_VALUE from tbwc_store_options where option_name = 'DEFAULT CATALOG'", _DBConnectionString);
            //da.Fill(ds, "generictable");
            //_CATALOG_ID = ds.Tables[0].Rows[0].ItemArray[0].ToString();
            _CATALOG_ID = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"].ToString();

        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK HTML TEMPLATE PAGE IS GENERATED OR NOT  ***/
        /********************************************************************************/
        public bool RenderHTML(string rType)
        {
            bool _status = false;
          //  string _sqlpkginfo;
            
            //_sqlpkginfo = " SELECT * FROM TBW_PACKAGE ";
            //_sqlpkginfo = _sqlpkginfo + " WHERE IS_ROOT = 0 AND PACKAGE_NAME = '" + _Package + "' ORDER BY PROCESS_ORDER ASC ";

            DataSet dspkg = new DataSet();
            try
            {
                //dspkg = GetDataSet(_sqlpkginfo);
                dspkg = (DataSet)objHelperDB.GetGenericDataDB(_Package, "GET_PACKAGE", HelperDB.ReturnType.RTDataSet);
              
                if (dspkg != null)
                {
                    if (dspkg.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dspkg.Tables[0].Rows)
                        {
                        
                            _skin_container = dr["SKIN_NAME"].ToString();
                            _grid_cols = Convert.ToInt32(dr["GRID_COLS"]);
                            _grid_rows = Convert.ToInt32(dr["GRID_ROWS"]);
                            _skin_sql_container = dr["SKIN_SQL"].ToString();
                            _skin_sql_type_container = dr["SKIN_SQL_TYPE"].ToString();
                            _skin_sql_param_container = dr["SKIN_SQL_PARAM"].ToString();
                            _package_order = Convert.ToInt32(dr["PROCESS_ORDER"]);
                            _skin_body_attribute = dr["SKIN_BODY_ATTRIBUTE"].ToString();
                            _skin_records = dr["LIST_SKIN_NAME"].ToString();
                            _skin_sql_records = dr["LIST_SKIN_SQL"].ToString();
                            _skin_sql_type_records = dr["LIST_SKIN_SQL_TYPE"].ToString();
                            _skin_sql_param_records = dr["LIST_SKIN_SQL_PARAM"].ToString();

                            if (rType == "Column")
                                BuildRecordsTemplateColumn();
                            else
                                BuildRecordsTemplateRow();
                        }
                    }
                }

                if (BuildMainContainer())
                {
                    _RenderedHTML = _stmpl_container.ToString().Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

                      if (_Package == "NEWPRODUCT" || _Package == "CATEGORYLISTIMG" || _Package == "CSFAMILYPAGE" || _Package == "CSFAMILYPAGEWITHSUBFAMILY" || _Package == "PRODUCT")
                    {
                        if (_stmpl_container.ToString().Contains("data-original=\"/prodimages\""))
                        {
                            if (_Package == "CSFAMILYPAGE")
                                _RenderedHTML = _stmpl_container.ToString().Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages/images/noimage.gif\"");
                            else
                                _RenderedHTML = _stmpl_container.ToString().Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages/images/noimage.gif\"");
                            _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                        }
                        if (_stmpl_container.ToString().Contains("data-original=\"\""))
                        {
                            _RenderedHTML = _stmpl_container.ToString().Replace("data-original=\"\"", "data-original=\"/prodimages/images/noimage.gif\"");
                            _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                        }
                    }
                    else
                    {
                        if (_stmpl_container.ToString().Contains("src=\"/prodimages\""))
                        {
                            if (_Package == "CSFAMILYPAGE")
                                _RenderedHTML = _stmpl_container.ToString().Replace("src=\"/prodimages\"", "src=\"/prodimages/images/noimage.gif\" style=\"display:none;\"");
                            else
                                _RenderedHTML = _stmpl_container.ToString().Replace("src=\"/prodimages\"", "src=\"/prodimages/images/noimage.gif\"");
                            _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                        }
                        if (_stmpl_container.ToString().Contains("src=\"\""))
                        {
                            _RenderedHTML = _stmpl_container.ToString().Replace("src=\"\"", "src=\"/prodimages/images/noimage.gif\"");
                            _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                        }

                    }

                    /*DataSet DSnaprod = new DataSet();
                    DSnaprod = GetDataSet("SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_STATUS <> 'AVAILABLE'");
                    foreach (DataRow DDR in DSnaprod.Tables[0].Rows)
                    {
                        if (_RenderedHTML.Contains("<img src=\"images/but_buy1.gif\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_"))
                            _RenderedHTML = _RenderedHTML.Replace("<img src=\"images/but_buy1.gif\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_", " <img src=\"images/but_buy1.gif\" style=\"display:none\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_");

                        if (_RenderedHTML.Contains("<img src=\"images/but_buyitem1.gif\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_"))
                            _RenderedHTML = _RenderedHTML.Replace("<img src=\"images/but_buyitem1.gif\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_", " <img src=\"images/but_buyitem1.gif\" style=\"display:none\" name=\"Image" + DDR["PRODUCT_ID"].ToString() + "_");

                        if (_RenderedHTML.Contains("<strong>QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;\""))
                            _RenderedHTML = _RenderedHTML.Replace("<strong>QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;\"", "<strong style=\" display:none\">QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "\" style=\" display:none\"");

                        if (_RenderedHTML.Contains("<strong>QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "_" + DDR["QTY_AVAIL"].ToString() + "_" + DDR["MIN_ORD_QTY"].ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;\" size=\"5\" type=\"text\" id=\"txt" + DDR["PRODUCT_ID"].ToString() + "_" + DDR["QTY_AVAIL"].ToString() + "_" + DDR["MIN_ORD_QTY"].ToString() + "\"/>"))
                            _RenderedHTML = _RenderedHTML.Replace("<strong>QTY</strong><input name=\"txt" + DDR["PRODUCT_ID"].ToString() + "_" + DDR["QTY_AVAIL"].ToString() + "_" + DDR["MIN_ORD_QTY"].ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;\" size=\"5\" type=\"text\" id=\"txt" + DDR["PRODUCT_ID"].ToString() + "_" + DDR["QTY_AVAIL"].ToString() + "_" + DDR["MIN_ORD_QTY"].ToString() + "\"/>", "&nbsp;");

                    }*/
                }
                _status = true;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                _status = false;
            }
           
            return _status;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER THE MAIN CONTENT IS GET BUILD OR NOT  ***/
        /********************************************************************************/
        private bool BuildMainContainer()
        {
            bool _status = false;
            string strFile = HttpContext.Current.Server.MapPath("ProdImages");
            string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
            string strPDFFiles2 = HttpContext.Current.Server.MapPath("News update");
            string _sqlpkginfo;
            string package_name = _Package.ToString();
            //_sqlpkginfo = " SELECT TOP 1 * FROM TBW_PACKAGE ";
            //_sqlpkginfo = _sqlpkginfo + " WHERE IS_ROOT = 1 AND PACKAGE_NAME = '" + _Package + "' ORDER BY PROCESS_ORDER ASC ";

            DataSet dspkg = new DataSet();
            try
            {
                //dspkg = GetDataSet(_sqlpkginfo);
                dspkg = (DataSet)objHelperDB.GetGenericDataDB(_Package, "GET_MAIN_PACKAGE", HelperDB.ReturnType.RTDataSet);
                if (dspkg != null)
                {
                    if (dspkg.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dspkg.Tables[0].Rows)
                        {
                            _skin_container = dr["SKIN_NAME"].ToString();
                            _grid_cols = Convert.ToInt32(dr["GRID_COLS"]);
                            _grid_rows = Convert.ToInt32(dr["GRID_ROWS"]);
                            _skin_sql_container = dr["SKIN_SQL"].ToString();
                            _skin_sql_type_container = dr["SKIN_SQL_TYPE"].ToString();
                            _skin_sql_param_container = dr["SKIN_SQL_PARAM"].ToString();
                            _package_order = Convert.ToInt32(dr["PROCESS_ORDER"]);
                            _skin_body_attribute = dr["SKIN_BODY_ATTRIBUTE"].ToString();
                        }
                    }
                }

                if (_skin_container == "" || _SkinRootPath == "" || _skin_container == null || _SkinRootPath == null)
                {
                    return false;
                }
                //Build the outer body of the HTML - for main container
                _stg_container = new StringTemplateGroup(_skin_container, _SkinRootPath);
                DataSet dscontainer = null;
                if (_GDataSet != null && _GDataSet.Tables.Count > 1)
                {
                    dscontainer = _GDataSet;
                }
                else if (package_name.Equals("CSFAMILYPAGE"))
                {
                    dscontainer = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                }
                else if (package_name.Equals("CSFAMILYPAGEWITHSUBFAMILY"))
                {
                    dscontainer = null;
                    DataSet tempDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                    if (tempDs != null && tempDs.Tables["SubFamily"] != null)
                    {
                        DataRow[] dr = tempDs.Tables["SubFamily"].Select("FAMILY_ID='" + paraValue + "'");
                        if (dr.Length > 0)
                        {
                            dscontainer = new DataSet();
                            dscontainer.Tables.Add(dr.CopyToDataTable().Copy());
                        }

                    }
                }
                else
                {
                    dscontainer = GetDataSet(_skin_sql_container, _skin_sql_type_container, _skin_sql_param_container);
                }
                if (dscontainer != null)
                {
                    if (dscontainer.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dscontainer.Tables[0].Rows)
                        {
                            if (package_name.Equals("CSFAMILYPAGE"))
                            {
                                if (dr["STATUS"].ToString().ToUpper() == "TRUE")
                                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container + "1");
                                else if (dr["STATUS"].ToString().ToUpper() == "FALSE")
                                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                                else
                                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container + "2");

                            }
                            else
                              
                                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                            if (HttpContext.Current.Session["USER_ID"]== null || HttpContext.Current.Session["USER_ID"] == "")
                            {
                            }
                            else
                            {

                                if (package_name.Equals("BOTTOM"))
                                {
                                    if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                                        _stmpl_container.SetAttribute("TBT_RET_PRO", false);
                                    else
                                        _stmpl_container.SetAttribute("TBT_RET_PRO", true);
                                }
                                if (package_name.Equals("BOTTOM") && !HttpContext.Current.Session["USER_ID"].Equals(""))
                                {
                                    if(HttpContext.Current.Session["USER_ID"].ToString().Equals(System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                                        _stmpl_container.SetAttribute("TBT_DUMUSER_CHECK", false);
                                    else
                                        _stmpl_container.SetAttribute("TBT_DUMUSER_CHECK", true);
                                }
                            }

                       

                            if (package_name.Equals("CSFAMILYPAGE"))
                            {
                                
                                DataSet tempdscat = (DataSet)HttpContext.Current.Session["LHSAttributes"];
                                if (tempdscat != null && tempdscat.Tables.Count > 0)
                                {
                                    if (tempdscat.Tables.Count == 1 && tempdscat.Tables[0].Rows.Count > 0)
                                        _stmpl_container.SetAttribute("TBT_LHS_TIP", true);
                                    else if (tempdscat.Tables.Count > 1 && tempdscat.Tables[1].Rows.Count > 0)
                                        _stmpl_container.SetAttribute("TBT_LHS_TIP", true);
                                    else
                                        _stmpl_container.SetAttribute("TBT_LHS_TIP", false);
                                }
                                else
                                    _stmpl_container.SetAttribute("TBT_LHS_TIP",false);


                                if (HttpContext.Current.Request.QueryString["fid"] != null)
                                {
                                    _fid = HttpContext.Current.Request.QueryString["fid"].ToString();

                                    DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, _fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                                    if (tmpds != null && tmpds.Tables.Count > 0)
                                    {
                                        _stmpl_container.SetAttribute("TBT_FAMILY_ID", _fid);
                                      //  objHelperService.Cons_NewURl(_stmpl_container, _stmpl_container.GetAttribute("TBT_FAMILY_ID").ToString() + "////" + _fid, "fl.aspx", true, "", false, false);
                                        string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
                                        _stmpl_container.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                                        _stmpl_container.SetAttribute("TBT_ORGEA_PATH", eapath);
                                        string bceapath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();   
                                        objHelperService.Cons_NewURl(_stmpl_container, _stmpl_container.GetAttribute("TBT_ORGEA_PATH").ToString() + "////" + _fid+"="+"Family", "fl.aspx", true, "", false, false);
                                       // objHelperService.Cons_NewURl(_stmpl_container, bceapath + "////" + _fid, "fl.aspx", true, "", false, false);
                                        
                                    }

                                }
                               
                            }

                            _stmpl_container.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());


                            if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].Equals("") && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                            {
                                _stmpl_container.SetAttribute("TBT_LOGIN_NAME", GetLoginName());
                            }

                             if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                            {
                                if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString())==4)
                                {
                                    string ReMailLink = "<a Href=mConfirmMessage.aspx?Result=REMAILACTIVATION class=\"linkemailre\">Re-Email Activation Link Now</a>";

                                  _stmpl_container.SetAttribute("TBT_LOGIN_NAME", " Your Account Has Not Been Activated! " + ReMailLink);                                              
                                }
                                else
                                _stmpl_container.SetAttribute("TBT_COMPANY_NAME", "");
                            }
                            else
                            {
                                if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                                {
                                    _stmpl_container.SetAttribute("TBT_COMPANY_NAME", GetCompanyName() + ",");
                                }
                            }

                             if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] !="" && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                                           HttpContext.Current.Session["LOGIN_NAME"] = GetLoginName();

                            foreach (DataColumn dc in dr.Table.Columns)
                            {
                                _stmpl_container.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc.ColumnName.ToString()].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>"));
                            }
                            // code by palani
                       
                        }

                        
                        if (dscontainer.Tables[0].Columns.Contains("attribute_name"))
                        {
                            string descall = "";
                            string descalltrim = "";
                            string desc1 = "";
                            string descallstring = "";
                            string Att_name = "";
                            foreach (DataRow dr in dscontainer.Tables[0].Rows)
                            {
                                desc1 = "";
                                if (dr["ATTRIBUTE_TYPE"].ToString().Equals("3") || dr["ATTRIBUTE_TYPE"].ToString().Equals("9"))
                                {
                                    FileInfo Fil;
                                    if (package_name.Equals("CSFAMILYPAGE"))
                                    {
                                        Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("_TH", "_Images_200"));
                                    }
                                    else
                                    {
                                        Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString());
                                    }
                                    if (Fil.Exists)
                                    {
                                        if (package_name.Equals("CSFAMILYPAGE"))
                                        {
                                            //_stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("_TH", "_Images_200"));
                                        }
                                        else
                                            _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                                    }
                                    else
                                    {
                                        _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), "");
                                    }

                                }
                                else
                                {
                                   // 
                                    Att_name = dr["ATTRIBUTE_NAME"].ToString().ToUpper();

                                    if (package_name.Equals("CSFAMILYPAGE"))
                                    {
                                        if (Att_name.Equals("DESCRIPTIONS") || Att_name.Equals("FEATURES") || Att_name.Equals("SPECIFICATION") || Att_name.Equals("SPECIFICATIONS") || Att_name.Equals("APPLICATIONS") || Att_name.Equals("NOTES"))
                                        {
                                            desc1 = dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                            desc1 = desc1.ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                                        }
                                        else
                                            _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));
                                    }
                                    else
                                    {
                                        _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));

                                    }
                                    //if (Att_name == "short description" || Att_name == "short description1" || Att_name == "note" || Att_name == "Notes" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "DESCRIPTIONS")
                                    //{

                                    //    desc1 = dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                    //    _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));
                                    //   // _stmpl_container.SetAttribute("TBT_MORE", desc1);
                                        
                                        
                                    //}
                                    //else if (dr["ATTRIBUTE_NAME"].ToString() == "Descriptions1" || dr["ATTRIBUTE_NAME"].ToString() == "DescriptionTemp")
                                    //{
                                    //    desc1 = dr["STRING_VALUE"].ToString().Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                                    //}
                                    //else
                                    //{
                                    //    _stmpl_container.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;"));
                                    //}
                                }
                               
                               // if (dr["ATTRIBUTE_NAME"].ToString() == "Short Description" || dr["ATTRIBUTE_NAME"].ToString() == "Short Description1" || dr["ATTRIBUTE_NAME"].ToString() == "Note" || dr["ATTRIBUTE_NAME"].ToString() == "Notes" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "DESCRIPTIONS" || dr["ATTRIBUTE_NAME"].ToString() == "Descriptions1" || dr["ATTRIBUTE_NAME"].ToString() == "DescriptionTemp")
                                if (package_name.Equals("CSFAMILYPAGE"))
                                {
                                 
                                        if (!desc1.Equals(""))
                                            descall = descall + desc1 + "<br/><br/>";                                  
                                }
                             
                            }
                            if (package_name.Equals("CSFAMILYPAGE"))
                            {
                                if (!desc1.Equals(""))
                            {
                                descall = descall.Trim();
                                descall = descall.Substring(0, descall.Length - 5);
                            }
                            
                          
                            if (descall.Length > 1080)
                            {
                              //  _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
                                descallstring = descall.Substring(0, 1080).ToString();
                                _stmpl_container.SetAttribute("TBT_MORE", descallstring);
                                _stmpl_container.SetAttribute("TBT_MENU_ID", "2");
                               descall = descall.Substring(0,descall.Length).ToString();
                               // descall = descall.Substring(300).ToString();

                                _stmpl_container.SetAttribute("TBT_DESCALL", descall);

                            }
                            else
                            {
                                _stmpl_container.SetAttribute("TBT_DESCALL", descall);
                                _stmpl_container.SetAttribute("TBT_MENU_ID", "2");
                               // _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);
                                _stmpl_container.SetAttribute("TBT_MORE", descall);
                            }
                            if (descall.Length > 1080)
                                _stmpl_container.SetAttribute("TBT_MORE_SHOW", true);
                            else
                                _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);

                            
                            }
                            
                        }

                    }
                }
                if (package_name.Equals("BROWSEBYCATEGORY") || package_name.Equals("BROWSEBYBRAND") || package_name.Equals("BROWSEBYPRODUCT"))
                {
                    /*string bbvalue = "", catName = ""; int recvalue = 0; string cidvalue = "";
                    DataSet DSDR = null;
                    catName = paraValue;
                    DSDR = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catName + "'");
                    if (DSDR != null && DSDR.Tables[0].Rows.Count>=1   )
                    {
                        while (DSDR.Tables[0].Rows[0].ItemArray[1].ToString() != "0")
                        {
                            DSDR = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + DSDR.Tables[0].Rows[0].ItemArray[1].ToString() + "'");
                        }
                        bbvalue = "<h3 class=\"headerbar\"> " + DSDR.Tables[0].Rows[0]["CATEGORY_NAME"].ToString() + "</h3>";
                        cidvalue = DSDR.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                        #region comments
                        ////do
                        //{
                        //    DSDR = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraValue + "'");
                        //    foreach (DataRow DR in DSDR.Tables[0].Rows)
                        //    {
                        //        if (DR["PARENT_CATEGORY"].ToString() == "0")
                        //            if (recvalue > 0)
                        //            {
                        //                bbvalue = "<h3 class=\"headerbar\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //                catName = DR["CATEGORY_NAME"].ToString();
                        //            }
                        //            else
                        //            {
                        //                bbvalue = "<h3 class=\"headerbar\">&raquo; " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //                catName = DR["CATEGORY_NAME"].ToString();
                        //            }
                        //        else
                        //        {
                        //            bbvalue = "<h3 class=\"headerbar\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //            catName = DR["CATEGORY_NAME"].ToString();
                        //        }
                        //        //if (DR["PARENT_CATEGORY"].ToString() == "0")
                        //        //    if(recvalue >0)
                        //        //        bbvalue = "<h3 class=\"headerbar\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //        //    else
                        //        //        bbvalue= "<h3 class=\"headerbar\">&raquo; " + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //        //else
                        //        //    bbvalue = "<h3 class=\"headerbar2\">" + DR["CATEGORY_NAME"].ToString() + "</h3>" + bbvalue;
                        //    }
                        //    paraValue = DSDR.Tables[0].Rows[0].ItemArray[1].ToString(); recvalue++;
                        //    DSDR = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraValue + "'");
                        //    foreach (DataRow DR in DSDR.Tables[0].Rows)
                        //    {
                        //        string Convalue = "pl";
                        //        if (_Package == "BROWSEBYBRAND")
                        //            Convalue = "bb";
                        //        if (_Package == "BROWSEBYPRODUCT")
                        //            Convalue = "byproduct";

                        //        if (catName.ToLower() == "brand" || catName.ToLower() == "product")
                        //        {
                        //            bbvalue = "<h3 class=\"headerbar\">&raquo; " + catName + "</h3>";

                        //         //   bbvalue = "<h3 class=\"headerbar\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + catName + "</h3>";
                        //        }
                        //        else
                        //            bbvalue = "<h3 class=\"headerbar\"><A HREF=\"" + Convalue + ".aspx?&ld=0&&cid=" + DR["CATEGORY_ID"].ToString() + "\"><img src=\"images/ico_menu_back.gif\" width=\"15\" height=\"15\" align=\"absmiddle\"> " + catName + "</a></h3>";
                        //        //bbvalue = bbvalue.Replace(catName, "<A HREF=\"pl.aspx?&ld=0&&cid=" + DR["CATEGORY_ID"].ToString() + "\">" + catName + "</a>");                            
                        //    }
                        //}//while(DSDR.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
                        #endregion
                        _stmpl_container.SetAttribute("TBT_SELECTED_CATEGORY_NAME", bbvalue);
                        _stmpl_container.SetAttribute("TBT_SELECTED_CATEGORY_ID", cidvalue);
                    }
                    else
                    {
                        _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                        _stmpl_container.SetAttribute("TBT_SELECTED_CATEGORY_NAME", "");
                        _stmpl_container.SetAttribute("TBT_SELECTED_CATEGORY_ID", "");
                    }*/
                }
                _stmpl_container.SetAttribute("TBW_CART_ITEM", cartitem);
                //Lets get the block inner elements from the dictionary and finish building the main container

                if (package_name.Equals("CATEGORYLISTIMG"))
                {
                    DataSet DSDR = new DataSet();
                    //DSDR = GetDataSet("SELECT CATEGORY_ID,CATEGORY_NAME,IMAGE_FILE,SHORT_DESC,IMAGE_FILE2 FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraValue.ToString() + "'");
                    DataRow[] row = EasyAsk.GetCategoryAndBrand("MainCategory").Tables[0].Select("CATEGORY_ID='" + paraValue.ToString() + "'");
                    if (row.Length > 0)
                    {
                        DSDR.Tables.Add(row.CopyToDataTable());
                    }


                    if (DSDR != null && DSDR.Tables.Count>0)
                    {
                    

                            
                        foreach (DataRow _dsrow in DSDR.Tables[0].Rows)
                        {
                            _stmpl_container.SetAttribute("TBT_CATEGORY_NAME", _dsrow["CATEGORY_NAME"].ToString());
                            _stmpl_container.SetAttribute("TBT_CATEGORY_ID", _dsrow["CATEGORY_ID"].ToString());
                            _stmpl_container.SetAttribute("TBT_SHORT_DESC", _dsrow["SHORT_DESC"].ToString());
                            FileInfo Fil = new FileInfo(strFile + _dsrow["IMAGE_FILE"].ToString());
                            if (Fil.Exists)
                            {
                                _stmpl_container.SetAttribute("TBT_IMAGE_FILE1", _dsrow["IMAGE_FILE"].ToString().Replace("\\", "/"));
                            }
                            else
                            {
                                _stmpl_container.SetAttribute("TBT_IMAGE_FILE1", "");
                            }

                            /* For Category PDF file attachment (checking)  */
                            FileInfo Fil2 = new FileInfo(strPDFFiles1 + _dsrow["IMAGE_FILE2"].ToString());
                            FileInfo Fil3 = new FileInfo(strPDFFiles2 + _dsrow["IMAGE_FILE2"].ToString());
                            if (Fil2.Exists || Fil3.Exists)
                            {
                                _stmpl_container.SetAttribute("TBT_PDF_STATUS", true);
                            }
                            else
                            {
                                _stmpl_container.SetAttribute("TBT_PDF_STATUS", false);
                            }

                        }
                    }

                }

                string _Tbt_Order_Id = "";
                string _Tbt_Ship_URL = "";
                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                //Modified by:indu
                if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                {
                    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
                }
                else
                {
                    _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID).ToString();
                    //    HttpContext.Current.Session["ORDER_ID"] = _Tbt_Order_Id;
                }
               
                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                {
                    _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                }
                else if (_Tbt_Order_Id!="" && Convert.ToInt32(_Tbt_Order_Id) > 0)
                {
                    _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                }
                else
                {
                    _Tbt_Ship_URL = "shipping.aspx";
                }

                _stmpl_container.SetAttribute("TBT_ORDER_ID", _Tbt_Order_Id);
                _stmpl_container.SetAttribute("TBT_SHIP_URL", _Tbt_Ship_URL);

                if (HttpContext.Current.Session["USER_ID"] == null || HttpContext.Current.Session["USER_ID"].Equals("" ))
                    _stmpl_container.SetAttribute("TBT_LOGIN", false);
                else
                    _stmpl_container.SetAttribute("TBT_LOGIN", true);

                #region "breadcrumb"
                string breadcrumb = "";
                /*if (HttpContext.Current.Request.Url.ToString().Contains("pd.aspx") == true && HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request["tsb"].ToString() != "" && HttpContext.Current.Request["tsm"] != null && HttpContext.Current.Request["tsm"].ToString() != "" && HttpContext.Current.Request["sl1"] != null && HttpContext.Current.Request["sl1"].ToString() != "" && HttpContext.Current.Request["sl2"] != null && HttpContext.Current.Request["sl2"].ToString() != "")
                {
                    DataSet DSBC = null;
                    if (paraCID != "")
                    {
                        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraCID + "'");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + paraCID + "&byp=2&bypcat=1\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                            breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + paraCID + "&byp=2\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        }
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bb.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&byp=2&bypcat=1\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "</a>";
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bb.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&byp=2\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "</a>";
                        string sql = "";
                        if (HttpContext.Current.Request["sl2"].ToString() != "0")
                        {
                            sql = "SELECT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE TOSUITE_BRAND='" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "' AND TOSUITE_MODEL='" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "' AND CATEGORY_ID='" + paraCID + "' AND SUBCATID_L1='" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "' AND PRODUCT_ID=" + paraPID.ToString();
                        }
                        else
                        {
                            sql = "SELECT SUBCATNAME_L1 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE TOSUITE_BRAND='" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "' AND TOSUITE_MODEL='" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "' AND CATEGORY_ID='" + paraCID + "' AND SUBCATID_L1='" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='' AND PRODUCT_ID=" + paraPID.ToString();
                        }
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bb.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2\">" + GetDataSet(sql).Tables[0].Rows[0][0].ToString() + "</a>";
                    }
                    if (paraPID != "")
                    {
                        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                        }
                    }
                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("pd.aspx") == true && HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request["tsb"].ToString() != "" && HttpContext.Current.Request["tsm"] != null && HttpContext.Current.Request["tsm"].ToString() != "")
                {
                    DataSet DSBC = null;
                    if (paraCID != "")
                    {
                        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraCID + "'");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + paraCID + "&byp=2&bypcat=1\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                            breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + paraCID + "&byp=2\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        }
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bb.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&byp=2&bypcat=1\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "</a>";
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bb.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&byp=2\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "</a>";
                    }
                    if (paraPID != "")
                    {
                        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&tsm=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsm"].ToString()) + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                        }
                    }

                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("pd.aspx") == true && HttpContext.Current.Request.QueryString["tsb"] != null && HttpContext.Current.Request["tsb"].ToString() != "")
                {
                    DataSet DSBC = null;
                    if (paraCID != "")
                    {
                        DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + paraCID + "'");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + paraCID + "&byp=2&bypcat=1\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                            breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + paraCID + "&byp=2\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        }
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"bb.aspx?&ld=0&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&byp=2\">" + HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "</a>";
                    }
                    if (paraPID != "")
                    {
                        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + " and attribute_id=1");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + paraCID + "&tsb=" + HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()) + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                        }
                    }
                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("fl.aspx") == true && _Package == "CSFAMILYPAGE" && HttpContext.Current.Request.QueryString["sl1"] != null && HttpContext.Current.Request.QueryString["sl1"].ToString() != "" && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl2"].ToString() != "")
                {
                    DataSet DSBC = null;
                    DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + HttpContext.Current.Request.QueryString["cid"].ToString() + "'");
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() +"&byp=2&bypcat=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=2\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                    }
                    string sql = "SELECT DISTINCT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + HttpContext.Current.Request.QueryString["cid"].ToString() + "' AND SUBCATID_L1='" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "'";
                    breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"pl.aspx?&ld=0&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + GetDataSet(sql).Tables[0].Rows[0][0].ToString() + "</a>";

                    DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"fl.aspx?&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&by=2&qf=1\" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                    }

                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("pd.aspx") == true && _Package == "PRODUCT" && HttpContext.Current.Request.QueryString["sl1"] != null && HttpContext.Current.Request.QueryString["sl1"].ToString() != "" && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl2"].ToString() != "")
                {
                    DataSet DSBC = null;
                    DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + HttpContext.Current.Request.QueryString["cid"].ToString() + "'");
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + DR["CATEGORY_ID"].ToString() + "&byp=2\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                    }
                    string sql = "SELECT DISTINCT SUBCATNAME_L1 + ' - ' + SUBCATNAME_L2 FROM VIEW_WESTB_TOSUITE_BY_BRAND_PRODUCT WHERE CATEGORY_ID='" + HttpContext.Current.Request.QueryString["cid"].ToString() + "' AND SUBCATID_L1='" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "' AND SUBCATID_L2='" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "'";
                    breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"pl.aspx?&ld=0&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + GetDataSet(sql).Tables[0].Rows[0][0].ToString() + "</a>";
                    if (HttpContext.Current.Request.QueryString["tf"] != null && HttpContext.Current.Request.QueryString["tf"].ToString() != "")
                    {
                        DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font><a href=\"fl.aspx?&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&by=2&qf=1\" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        }
                    }
                    DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + " and attribute_id=1");
                    foreach (DataRow DR in DSBC.Tables[0].Rows)
                    {
                        breadcrumb = breadcrumb + "<font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font> <a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + paraCID + "&sl1=" + HttpContext.Current.Request.QueryString["sl1"].ToString() + "&sl2=" + HttpContext.Current.Request.QueryString["sl2"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                    }
                }
                else if (HttpContext.Current.Request.Url.ToString().Contains("fl.aspx") == true && HttpContext.Current.Request.QueryString["sl1"] != null && HttpContext.Current.Request.QueryString["sl1"].ToString() != "" && HttpContext.Current.Request.QueryString["sl2"] != null && HttpContext.Current.Request.QueryString["sl2"].ToString() != "")
                {

                }
                else
                {
                    if (paraPID != "")
                    {
                        DataSet DSBC = null;
                        DSBC = GetDataSet("SELECT String_value FROM TB_prod_specs WHERE product_ID = " + paraPID + "and attribute_id=1");
                        foreach (DataRow DR in DSBC.Tables[0].Rows)
                        {
                            if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                            {
                                if (HttpContext.Current.Request.QueryString["byp"] != null && HttpContext.Current.Request.QueryString["byp"].ToString() != "")
                                {
                                    if (HttpContext.Current.Request.QueryString["cid"] != null && HttpContext.Current.Request.QueryString["cid"].ToString() != "")
                                    {
                                        breadcrumb = "<a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                                    }
                                    else
                                        breadcrumb = "<a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                                }
                                else
                                {
                                    breadcrumb = "<a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                                }
                            }
                            else
                            {
                                breadcrumb = "<a href=\"pd.aspx?&pid=" + paraPID + "&fid=" + paraFID + " \" style=\"color:Black;\">" + DR[0].ToString() + "</a>";
                            }
                        }
                        if (paraFID != "")
                        {
                            string catIDtemp = "";
                            DSBC = GetDataSet("SELECT family_name,category_id FROM TB_family WHERE family_ID = " + paraFID);
                            foreach (DataRow DR in DSBC.Tables[0].Rows)
                            {
                                if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                                {
                                    if (HttpContext.Current.Request.QueryString["byp"] != null && HttpContext.Current.Request.QueryString["byp"].ToString() != "")
                                    {
                                        if (HttpContext.Current.Request.QueryString["cid"] != null && HttpContext.Current.Request.QueryString["cid"].ToString() != "")
                                            breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        else
                                            breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                    }
                                    else
                                    {
                                        breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + " \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                    }
                                }
                                else
                                {
                                    breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + " \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                }
                                catIDtemp = DR[1].ToString();
                            }
                            do
                            {
                                DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                                foreach (DataRow DR in DSBC.Tables[0].Rows)
                                {
                                    if (DR["PARENT_CATEGORY"].ToString() != "0")
                                    {
                                        if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                                        {
                                            breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        }
                                        else
                                        {
                                            breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        }
                                    }
                                    else
                                    {
                                        if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                                        {
                                            breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2&qf=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        }
                                        else
                                        {
                                            breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                        }
                                    }
                                    catIDtemp = DR["PARENT_CATEGORY"].ToString();
                                }
                            } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
                        }
                    }
                    else if (paraFID != "")
                    {
                        //DataSet DSBC = null;
                        //string catIDtemp = "";
                        //DSBC = GetDataSet("SELECT family_NAME,category_id FROM TB_family WHERE family_ID = " + paraFID);
                        //foreach (DataRow DR in DSBC.Tables[0].Rows)
                        //{
                        //    if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                        //    {
                        //        if (HttpContext.Current.Request.QueryString["byp"] != null && HttpContext.Current.Request.QueryString["byp"].ToString() != "")
                        //        {
                        //            if (HttpContext.Current.Request.QueryString["cid"] != null && HttpContext.Current.Request.QueryString["cid"].ToString() != "")
                        //                breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + "&cid=" + HttpContext.Current.Request.QueryString["cid"].ToString() + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        //            else
                        //                breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=" + HttpContext.Current.Request.QueryString["byp"].ToString() + "&qf=1 \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        //        }
                        //        else
                        //        {
                        //            breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + " \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        //        }
                        //    }
                        //    else
                        //    {
                        //        breadcrumb = "<a href=\"fl.aspx?&fid=" + paraFID + " \" style=\"color:Black;\">" + DR[0].ToString().Replace("<ars>g</ars>", "&rarr;") + "</a>";
                        //    }
                        //    catIDtemp = DR[1].ToString();
                        //}
                        //do
                        //{
                        //    DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                        //    foreach (DataRow DR in DSBC.Tables[0].Rows)
                        //    {
                        //        if (DR["PARENT_CATEGORY"].ToString() != "0")
                        //        {
                        //            if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                        //            {
                        //                breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2&qf=1 \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //            }
                        //            else
                        //            {
                        //                breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (HttpContext.Current.Request.QueryString["pcr"] != null && HttpContext.Current.Request.QueryString["pcr"].ToString() != "")
                        //            {
                        //                //breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2&bypcat=1\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //                breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + "&pcr=" + HttpContext.Current.Request.QueryString["pcr"].ToString() + "&byp=2\" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //            }
                        //            else
                        //            {
                        //                breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                        //            }
                        //        }
                        //        catIDtemp = DR["PARENT_CATEGORY"].ToString();
                        //    }
                        //} while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
                    }
                    else if (paraCID != "")
                    {
                        DataSet DSBC = null;
                        string catIDtemp = paraCID;
                        do
                        {
                            DSBC = GetDataSet("SELECT CATEGORY_NAME,PARENT_CATEGORY,CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_ID = '" + catIDtemp + "'");
                            foreach (DataRow DR in DSBC.Tables[0].Rows)
                            {
                                if (DR["PARENT_CATEGORY"].ToString() != "0")
                                {
                                    if (breadcrumb == "")
                                        breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                                    else
                                        breadcrumb = "<a href=\"pl.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;
                                }
                                else
                                {
                                    if (breadcrumb == "")
                                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a>";
                                    else
                                        breadcrumb = "<a href=\"ct.aspx?&ld=0&cid=" + catIDtemp + " \" style=\"color:Black;\">" + DR["CATEGORY_NAME"].ToString() + "</a> <font style=\"font-family: Arial, Helvetica, sans-serif; font-weight:bolder; font-size: small; font-style: normal\"> / </font>" + breadcrumb;

                                }
                                catIDtemp = DR["PARENT_CATEGORY"].ToString();
                            }
                        } while (DSBC.Tables[0].Rows[0].ItemArray[1].ToString() != "0");
                    }
                }*/
                #endregion
                _stmpl_container.SetAttribute("TBT_BREAD_CRUMBS", breadcrumb.Replace("<ars>g</ars>", "&rarr;"));

                foreach (KeyValuePair<string, TBWDataList[]> kvp in _dict_inner_html)
                {
                    _stmpl_container.SetAttribute(kvp.Key, kvp.Value);
                }

                _status = true;
                if (package_name.Equals("CSFAMILYPAGE"))
                {
                   // GetFamilyMultipleImages(Convert.ToInt32(paraFID), _stmpl_container);
                }
                
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                _status = false;
            }
            return _status;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO BUILD TEMPLATE RECORDS IN COLUMN WISE  ***/
        /********************************************************************************/
        private void BuildRecordsTemplateColumn()
        {
            TBWDataList[] lstrecords = new TBWDataList[0];
            string strFile = HttpContext.Current.Server.MapPath("ProdImages");
            //Build the cell inner body of the HTML
            _stg_records = new StringTemplateGroup(_skin_records, _SkinRootPath);
            DataSet dsrecords = null;
            string package_name = _Package.ToString();
            if (_GDataSet != null && _skin_sql_records.Length == 0)
            {
                dsrecords = _GDataSet;
            }
            else if (package_name.Equals("PRODUCT"))
            {
                dsrecords = (DataSet)HttpContext.Current.Session["FamilyProduct"];

            }
            else if (package_name.Equals("NEWPRODUCTLOGNAV"))
            {
                dsrecords = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_LOG_NAV");  
            }
            else if (package_name.Equals("NEWPRODUCTHIGHLIGHTSCATLIST"))
            {
                dsrecords = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRO_HIGHLIGHTS_CAT_LIST");
            }
            else if (package_name.Equals("NEWPRODUCT"))
            {
                 string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                 dsrecords = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT 10" +","+ websiteid);
            }
            else if (package_name.Equals("NEWPRODUCTNAV"))
            {
                dsrecords = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_NAV");
            }
            else
            {
                dsrecords = GetDataSet(_skin_sql_records, _skin_sql_type_records, _skin_sql_param_records);
            }
            _stg_container = new StringTemplateGroup(_skin_container, _SkinRootPath);
            if (dsrecords != null)
            {
                 
                //HttpCookie RVPCookie =  HttpContext.Current.Request.Cookies["RVPCookie"];
                //if (RVPCookie == null)
                //    RVPCookie = new HttpCookie("RVPCookie"); 
                if (dsrecords.Tables[0].Rows.Count > 0)
                {
                    DataRow[] cellrow = dsrecords.Tables[0].Select("ATTRIBUTE_ID = 1");
                        DataRow[] cellrow_np = dsrecords.Tables[0].Select("ATTRIBUTE_ID = 450");
                   
                    lstrecords = new TBWDataList[cellrow.Length];
                    int ictrecords = 0, ictcol = 1; string strValue = "";
                
                    foreach (DataRow cdr in cellrow)
                    {
                        _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + _skin_records);

                        _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());

                        if (!package_name.Equals("PRODUCT"))
                             _stmpl_records.SetAttribute("TBT_YOURCOST", GetMyPrice(System.Convert.ToInt32(cdr["PRODUCT_ID"])));
                       //modify
                        if (package_name.Equals( "PRODUCT"))
                        {
                            GetMultipleImages(System.Convert.ToInt32(cdr["PRODUCT_ID"]), System.Convert.ToInt32(paraFID), System.Convert.ToInt32(cdr["Family_ID"]), _stmpl_records);
                            if (HttpContext.Current.Request.QueryString["fid"] != null)
                            {
                                _fid = HttpContext.Current.Request.QueryString["fid"].ToString();

                                DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, _fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                                if (tmpds != null && tmpds.Tables.Count > 0)
                                {
                                    _stmpl_records.SetAttribute("TBT_CATEGORY_ID", tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString());
                                    string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
                                    _stmpl_records.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                                    _stmpl_records.SetAttribute("ORG_TBT_EA_PATH", eapath);
                                }

                            }

                        }


                        if (package_name.Equals("NEWPRODUCTLOGNAV") || package_name.Equals("NEWPRODUCTNAV") || package_name.Equals("NEWPRODUCTHIGHLIGHTSCATLIST"))
                        {

                            //Added by Indu
                           _stmpl_records.SetAttribute("TBT_FAMILY_ID", cdr["family_id"].ToString());
                           //
                            string espath="AllProducts////WESAUSTRALASIA////" + cdr["CATEGORY_PATH"].ToString();
                            _stmpl_records.SetAttribute("FAMILY_EA_PATH" , HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));
                      //
                            objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + "////" + cdr["CATEGORY_PATH"].ToString() + "////" + cdr["FAMILY_ID"].ToString()+"="+cdr["FAMILY_name"].ToString() ,"fl.aspx", true, "",false,true);
                           //
                            espath = "AllProducts////WESAUSTRALASIA////" + cdr["CATEGORY_PATH"].ToString() + "////UserSearch1=fl Id=" + cdr["FAMILY_ID"].ToString();
                            _stmpl_records.SetAttribute("PRODUCT_EA_PATH",  HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));
                           //
                            string pcode = string.Empty;
                            if (cdr["attribute_name"].ToString().ToUpper() == "CODE")
                            {

                                pcode = cdr["string_value"].ToString();
                            }
                            //if (cdr["attribute_name"].ToString().ToUpper().Equals("CODE"))
                            //{
                            //    foreach (DataRow cdr_np in cellrow_np)
                            //    {
                            //        if (cdr["PRODUCT_ID"].ToString().Equals(cdr_np["PRODUCT_ID"].ToString()) && cdr_np["attribute_name"].ToString().ToUpper().Equals("PROD_CODE"))
                            //        {
                            //            pcode = cdr_np["string_value"].ToString();
                            //            break;
                            //        }
                            //    }
                            //}
                          objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + "////" + cdr["CATEGORY_PATH"].ToString() + "////" +cdr["FAMILY_ID"].ToString()+"="+ cdr["FAMILY_name"].ToString() + "////" + cdr["product_ID"].ToString()+"="+pcode, "pd.aspx", true, "");
                        //
                        }
                        if (package_name.Equals("NEWPRODUCT"))
                        {
                            string pcode = string.Empty;
                            if (cdr["attribute_name"].ToString().ToUpper() == "CODE")
                            {
                                pcode = cdr["string_value"].ToString();
                            }
                            //if (cdr["attribute_name"].ToString().ToUpper().Equals("CODE"))
                            //{
                            //    foreach (DataRow cdr_np in cellrow_np)
                            //    {
                            //        if (cdr["PRODUCT_ID"].ToString().Equals(cdr_np["PRODUCT_ID"].ToString()) && cdr_np["attribute_name"].ToString().ToUpper().Equals("PROD_CODE"))
                            //        {
                            //            pcode = cdr_np["string_value"].ToString();
                            //            break;
                            //        }
                            //    }
                            //}
                            string espath = "AllProducts////WESAUSTRALASIA////" + cdr["CATEGORY_PATH"].ToString() + "////UserSearch1=Family Id=" + cdr["FAMILY_ID"].ToString();
                          //  objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + "////" + cdr["CATEGORY_PATH"].ToString() + "////" + cdr["FAMILY_name"].ToString(), "fl.aspx", true, "");
                            _stmpl_records.SetAttribute("PRODUCT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(espath)));
                           
                        }
                        
                        foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                        {

                            //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());                                                       
                            _stmpl_records.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), cdr[dc.ColumnName.ToString()].ToString());

                            if ( dc.ColumnName.ToString().ToUpper() == "FAMILY_NAME" && package_name.Equals("PRODUCT"))
                            {
                                HttpContext.Current.Session["FNAME_PRODUCT"] = cdr[dc.ColumnName.ToString()].ToString();
                            }

                            //if (package_name.Equals("PRODUCT"))
                            //{
                            //    if (dc.ColumnName.ToUpper().ToString() == "FAMILY_NAME")
                            //    {

                            //        string fname = cdr[dc.ColumnName.ToString()].ToString();
                            //        string option_name = "family_name";
                            //        SetCookie(option_name, fname,"","","","","");
                                
                                   
                            //    }
                            //}


                            //if ((_Package.ToUpper() == "NEWPRODUCT") && (dc.ColumnName.ToString().ToLower()=="family_name"))

                            //{
                            //    string str

                            //    if (cdr[dc.ColumnName.ToString()].ToString().Length > 55)
                            //    { 
                                
                            //    }
                            //    _stmpl_records.SetAttribute("TBT_" + dc.ColumnName.ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), cdr[dc.ColumnName.ToString()].ToString().Trim());

                            //}
                                //if (OLDPRODUCTCODE == string.Empty)
                                //{
                                // OLDPRODUCTCODE=cdr["Product_ID"].ToString();
                                //}
                                //else if(OLDPRODUCTCODE!=cdr["Product_ID"].ToString())
                                //{
                                // NEWP = true;
                                //    if( NEWPCHECK == false)
                                //    {
                                //    OLDPRODUCTCODE=cdr["Product_ID"].ToString();
                                   
                                //    }
                                //}


                             
                          

                            if (package_name.Equals("COMPARE") && dc.ColumnName.ToString().ToUpper().Equals("PRODUCT_ID"))
                            {
                                //DataSet Dsfamilyname = GetDataSet("SELECT TOP(1) FAMILY_ID FROM TB_PROD_FAMILY WHERE PRODUCT_ID =" + cdr["Product_ID"].ToString() + " AND FAMILY_ID IN(SELECT DISTINCT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATEGORY_ID IN (SELECT CATEGORY_ID FROM CATEGORY_FUNCTION(" + CATALOG_ID + ", '" + cdr["CATEGORY_ID"].ToString() + "')))");
                                DataSet Dsfamilyname = (DataSet)objHelperDB.GetGenericDataDB(_CATALOG_ID, cdr["Product_ID"].ToString(), cdr["CATEGORY_ID"].ToString(), "GET_FAMILY_ID_COMPARE_PACKAGE", HelperDB.ReturnType.RTDataSet);
                                if (Dsfamilyname != null && Dsfamilyname.Tables[0].Rows.Count > 0)
                                {
                                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", Dsfamilyname.Tables[0].Rows[0][0].ToString());
                                }
                            }
                            else if ((package_name.Equals("NEWPRODUCT") || package_name.Equals("PROMOTIONS") || package_name.Equals("MOREPRODUCTS")) && dc.ColumnName.ToString().ToUpper().Equals("PRODUCT_ID"))
                            {
                                //DataSet Dsfamilyname = GetDataSet("select f.family_id,f.family_name,fs.string_value,a.attribute_id,a.attribute_name from tb_family_specs fs,tb_Family f,tb_catalog_family cf,tb_attribute a where f.family_id =fs.family_id and f.family_id=cf.family_id and fs.attribute_id=a.attribute_id and f.family_id in(SELECT TOP(1) FAMILY_ID FROM TB_PROD_FAMILY WHERE PRODUCT_ID =" + cdr["Product_ID"].ToString() + " ) and cf.catalog_id=" + CATALOG_ID);
                                DataSet Dsfamilyname = (DataSet)objHelperDB.GetGenericDataDB(_CATALOG_ID, cdr["Product_ID"].ToString(), "GET_FAMILY_ATTRIBUTE_NEWPRODUCT_PACKAGE", HelperDB.ReturnType.RTDataSet);
                                string pcode = string.Empty;
                                if (cdr["attribute_name"].ToString().ToUpper() == "CODE")
                                {
                                    pcode = cdr["string_value"].ToString();
                                }
                                //modified by indu july18 since dsfamilyname is null
                                if (package_name.Equals("NEWPRODUCT"))
                                {
                                    objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + "////" + cdr["CATEGORY_PATH"].ToString() + "////" + cdr["Family_id"].ToString() + "=" + cdr["Family_name"].ToString() + "////" + cdr["Product_ID"].ToString() + "=" + pcode, "pd.aspx", true, "");
                                    //objErrorHandler.CreateLog("AllProducts////WESAUSTRALASIA////" + "////" + cdr["CATEGORY_PATH"].ToString() + "////" + cdr["Family_id"].ToString() + "=" + cdr["Family_name"].ToString() + "////" + cdr["Product_ID"].ToString() + "=" + pcode);
                                }
                               // objErrorHandler.CreateLog( cdr["CATEGORY_PATH"].ToString() + "////" + cdr["Family_id"].ToString() + "=" + cdr["Family_name"].ToString() + "////" + cdr["Product_ID"].ToString() + "=" + pcode);
                                //if (cdr["attribute_name"].ToString().ToUpper().Equals("CODE"))
                                //{
                                //    foreach (DataRow cdr_np in cellrow_np)
                                //    {
                                //        if (cdr["PRODUCT_ID"].ToString().Equals(cdr_np["PRODUCT_ID"].ToString()) && cdr_np["attribute_name"].ToString().ToUpper().Equals("PROD_CODE"))
                                //        {
                                //            pcode = cdr_np["string_value"].ToString();
                                //            break;
                                //        }
                                //    }
                                //}
                                if (Dsfamilyname != null && Dsfamilyname.Tables[0].Rows.Count > 0)
                                {
                                     //_stmpl_records.SetAttribute("TBT_FAMILY_NAME", Dsfamilyname.Tables[0].Rows[0]["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;"));
                                    if (!package_name.Equals("NEWPRODUCT"))
                                     _stmpl_records.SetAttribute("TBT_FAMILY_ID", Dsfamilyname.Tables[0].Rows[0]["FAMILY_ID"].ToString());
                                    //added by :indu

                                     if (package_name.Equals("NEWPRODUCT"))
                                     {
                                         //objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + "////" + cdr["CATEGORY_PATH"].ToString() + "////" + cdr["Family_id"].ToString() + "=" + cdr["Family_name"].ToString() + "////" + cdr["Product_ID"].ToString() + "=" + pcode, "pd.aspx", true, "");
                                         //objErrorHandler.CreateLog("AllProducts////WESAUSTRALASIA////" + "////" + cdr["CATEGORY_PATH"].ToString() + "////" + cdr["Family_id"].ToString() + "=" + cdr["Family_name"].ToString() + "////" + cdr["Product_ID"].ToString() + "=" + pcode);
                                     }
                                     else
                                     {
                                         //if (_Package.ToString() == "PROMOTIONS")
                                         //{
                                         //    _stmpl_records.SetAttribute("TBT_PRO_EAPATH1", HttpUtility.UrlEncode(objSecurity.StringEnCrypt("AllProducts////WESAUSTRALASIA////" + pcode))); 
                                         //}
                                         objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + _stmpl_records.GetAttribute("TBT_FAMILY_ID") + "=" + _stmpl_records.GetAttribute("TBT_FAMILY_NAME") + "////" + cdr["Product_ID"].ToString() + "=" + pcode, "pd.aspx", true, "");
                                     
                                     }
                                     if (package_name.Equals("NEWPRODUCT"))
                                     {
                                         foreach (DataRow Drow in Dsfamilyname.Tables[0].Rows)
                                         {
                                             string desc = "";
                                             string string_value=Drow["STRING_VALUE"].ToString();
                                             //if (string_value.Length > 100)
                                             //{
                                             //    desc = string_value.Substring(0, 100) + "...";
                                             if (string_value.Length > 75)
                                             {
                                                 desc = string_value.Substring(0, 75) + "...";   
                                         }
                                             else
                                             {
                                                 desc = string_value;
                                             }
                                             _stmpl_records.SetAttribute("TBT_" + Drow["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), desc.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("\n", "<br/>").Replace("\r", "&nbsp;"));



                                         }
                                     }
                                     else
                                     {
                                        
                                         foreach (DataRow Drow in Dsfamilyname.Tables[0].Rows)
                                         {
                                   
                                             string desc = "";
                                             string string_value = Drow["STRING_VALUE"].ToString();
                                             //if (string_value.Length > 80)
                                             if (string_value.Length > 75)
                                             {
                                                 desc = string_value.Substring(0, 75) + "...";
                                             }
                                             else
                                             {
                                                 desc = string_value;
                                             }
                                             _stmpl_records.SetAttribute("TBT_" + Drow["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), desc.Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("\n", "<br/>").Replace("\r", "&nbsp;"));



                                         }
                                     }

                                }
                            }
                           
                        }
                        //_stmpl_records.SetAttribute("TBT_FAMILY_ID", paraFID);
                        string descall = "";
                        string desc1 = "";
                        string descallstring = "";
                        string attName="";
                       // string pcode_hm = "";
                        string strurlnew = "";
                        int setatttr = 0;
                        DataRow[] PRODRows = dsrecords.Tables[0].Select("PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString());
                        //MODIFIED BY INDU FOR TUNING 05AUG2014
                        //foreach (DataRow dr in  dsrecords.Tables[0].Select("PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString()) )
                        //{
                        foreach (DataRow dr in PRODRows)
                        {
                            //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());  
                            desc1 = "";
                            string attr_datatype = dr["ATTRIBUTE_DATATYPE"].ToString();
                            string attr_type = dr["ATTRIBUTE_TYPE"].ToString();
                            string attr_name = dr["ATTRIBUTE_NAME"].ToString();
                            //Added by Indu-When there is no product image then refer family image
                            try
                            {
                                if (package_name.Equals("NEWPRODUCTLOGNAV") && setatttr == 0)
                                {

                                    string Product_ID = dr["Product_ID"].ToString();
                                    string Attrname = "TWEB IMAGE1";
                                    DataRow[] dr1 = dsrecords.Tables[0].Select("PRODUCT_ID='" + Product_ID + "' AND ATTRIBUTE_NAME='" + Attrname + "'");
                                    string sSQL = string.Empty;

                                    if (dr1.Length <= 0)
                                    {
                                        string family_id = dr["Family_ID"].ToString();
                                        sSQL = "Exec Get_FamilyImage " + family_id + "," + Product_ID;
                                        HelperDB objHelperDB = new HelperDB();
                                        DataSet dsfamily = objHelperDB.GetDataSetDB(sSQL);

                                        setatttr = 1;
                                        if (dsfamily.Tables[0] != null)
                                        {
                                            FileInfo Fil1;
                                            Fil1 = new FileInfo(strFile + dsfamily.Tables[0].Rows[0]["string_value"].ToString().Replace("\\", "/"));
                                            if (Fil1.Exists)
                                            {
                                                _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", dsfamily.Tables[0].Rows[0]["string_value"].ToString().Replace("\\", "/"));
                                            }
                                            else
                                            {
                                                _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", "/images/noimage.gif");
                                            }
                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", "/images/noimage.gif");
                                        }

                                    }
                                }
                            }
                            catch
                            { }
                            //end
                            if (attr_datatype.ToUpper().StartsWith("TEX"))
                            {
                                if (attr_type.Equals("3") || attr_type.Equals("9"))
                                {
                                    FileInfo Fil;
                                    if (package_name.Equals("PRODUCT"))
                                    {
                                        Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString().Replace("_TH", "_Images_200"));
                                    }
                                    else
                                    {
                                        Fil = new FileInfo(strFile + dr["STRING_VALUE"].ToString());
                                    }
                                    if (Fil.Exists)
                                    {
                                        if (package_name.Equals("PRODUCT"))
                                        {
                                            //_stmpl_records.SetAttribute("TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("_TH", "_Images_200"));
                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("TBT_" + attr_name.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                                            //set cookie value
                                            if (package_name.Equals("PRODUCT"))
                                            {
                                             
                                                string img1 = dr["STRING_VALUE"].ToString().Replace("\\", "/");

                                               // SetCookie("img1", "", "", img1,"","","");
                                         


                                            }
                                        }
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + attr_name.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), "");

                                        if (package_name.Equals("PRODUCT"))
                                        {
                                            //HttpContext.Current.Response.Cookies["RVP"]["img1"] = dr["STRING_VALUE"].ToString().Replace("\\", "/");
                                           // HttpContext.Current.Response.Cookies["RVP"].Expires = DateTime.Now.AddDays(1);
                                            //if (RVPCookie != null)
                                            //{
                                            //    RVPCookie["img1"] = dr["STRING_VALUE"].ToString().Replace("\\", "/");

                                            //    RVPCookie.Expires = DateTime.Now.AddDays(1);
                                            //    HttpContext.Current.Response.AppendCookie(RVPCookie);
                                            //}

                                            string img1 = dr["STRING_VALUE"].ToString().Replace("\\", "/");
                                           // SetCookie("img1", "", "", img1,"","","");

                                        }
                                    }
                                }
                                //else if (dr["ATTRIBUTE_NAME"].ToString().ToLower() == " SHORT_DESCRIPTION" || dr["ATTRIBUTE_NAME"].ToString() == "Descriptions" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "FEATURES" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "NOTES")
                                //{
                                    //desc1 = dr["STRING_VALUE"].ToString().Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                                //}
                                else
                                {
                                    attName = dr["ATTRIBUTE_NAME"].ToString().ToUpper();

                                    if (package_name.Equals("PRODUCT"))
                                    {
                                        if (attName == "CODE")
                                        {
                                           // HttpContext.Current.Response.Cookies["RVP"]["code1"] = dr["STRING_VALUE"].ToString();
                                            //HttpContext.Current.Response.Cookies["RVP"].Expires = DateTime.Now.AddDays(1);
                                            //if (RVPCookie != null)
                                            //{
                                            //    RVPCookie["code1"] = dr["STRING_VALUE"].ToString();
                                            //}

                                            string code = dr["STRING_VALUE"].ToString();

                                            //SetCookie("code", "", code,"","","","");

                                            //string prduct_url = objHelperService.Cons_NewURl(_stmpl_records, _stmpl_records.GetAttribute("ORG_TBT_EA_PATH") + "////" + cdr["Family_id"].ToString() + "=" + cdr["Family_name"].ToString() + "////" + cdr["Product_ID"].ToString() + "=" + code, "pd.aspx", true, "");
                                           // SetCookie("product_url", "", "", "", "", "", prduct_url);
                                           // string product_url_org = objHelperService.cons_NEWURL_ORG(_stmpl_records, _stmpl_records.GetAttribute("ORG_TBT_EA_PATH") + "////" + cdr["Family_id"].ToString() + "=" + cdr["Family_name"].ToString() + "////" + cdr["Product_ID"].ToString() + "=" + code, "pd.aspx", true, "");
                                           
                                        }
                                    }

                                 
                                    if (package_name.Equals("PRODUCT"))
                                    {
                                        if (attName.Equals("SHORT DESCRIPTION"))
                                        {
                                            _stmpl_records.SetAttribute("TBT_SHORTDESCRIPTION", dr["STRING_VALUE"].ToString().Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;"));
                                        }
                                        if (attName.Equals("SHORT_DESCRIPTION") || attName.Equals("DESCRIPTIONS") || attName.Equals("FEATURES") || attName.Equals("SPECIFICATION") || attName.Equals("SPECIFICATIONS") || attName.Equals("APPLICATIONS") || attName.Equals("NOTES"))
                                        {

                                            desc1 = dr["STRING_VALUE"].ToString().Replace("*", "").Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                                        }
                                            else if( attName.Equals("DESCRIPTION"))
                                        {
                                            if (dr["STRING_VALUE"].ToString().Length > 0)
                                            {
                                                _stmpl_records.SetAttribute("TBT_" + attr_name.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString());
                                                _stmpl_records.SetAttribute("PROD_DESC_ALT", dr["STRING_VALUE"].ToString());
                                                _stmpl_records.SetAttribute("PROD_DESC_TITLE", dr["STRING_VALUE"].ToString().Replace('"', ' '));
                                            }
                                            else
                                            {
                                                _stmpl_records.SetAttribute("PROD_DESC_ALT", dr["FAMILY_NAME"].ToString());
                                                _stmpl_records.SetAttribute("PROD_DESC_TITLE", dr["FAMILY_NAME"].ToString().Replace('"', ' '));
                                            }
                                         }
                                        else
                                            _stmpl_records.SetAttribute("TBT_" + attr_name.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString());
                                    }
                                    else
                                        _stmpl_records.SetAttribute("TBT_" + attr_name.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr["STRING_VALUE"].ToString());
                                }
                                //if (dr["ATTRIBUTE_NAME"].ToString().ToLower() == " SHORT_DESCRIPTION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "DESCRIPTIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "FEATURES" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "NOTES")
                                //{
                                //    descall = descall + desc1;
                                //}
                            }
                            //if (dr["ATTRIBUTE_NAME"].ToString().ToLower() == " SHORT_DESCRIPTION" || dr["ATTRIBUTE_NAME"].ToString() == "Descriptions" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "FEATURES" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "NOTES")
                            if (package_name.Equals("PRODUCT"))
                            {
                                if (!desc1.Equals(""))
                                    descall = descall + desc1 + "<br/><br/>";
                            }

                            else if (attr_datatype.ToUpper().StartsWith("NUM"))
                            {
                                if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                                    _stmpl_records.SetAttribute("TBT_" + attr_name.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), objHelperService.FixDecPlace(Convert.ToDecimal(dr["NUMERIC_VALUE"])).ToString());
                                else
                                    _stmpl_records.SetAttribute("TBT_" + attr_name.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), "");
                            }

                            //if (cdr["attribute_name"].ToString().ToUpper() == "PROD_CODE")
                            //{
                            //   // pcode_hm = cdr["string_value"].ToString();
                            //}
                                             

                        }
                        //if (_Package.ToString() == "NEWPRODUCT")
                        //{
                        //    objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + "////" + cdr["CATEGORY_PATH"].ToString() + "////" + _stmpl_records.GetAttribute("TBT_FAMILY_NAME") + "////" + pcode_hm, "pd.aspx", true, "");
                        //}
                        //else
                        //{
                        //    objHelperService.Cons_NewURl(_stmpl_records, "AllProducts////WESAUSTRALASIA////" + _stmpl_records.GetAttribute("TBT_FAMILY_NAME") + "////" + pcode_hm, "pd.aspx", true, "");
                        //}
                        // if (dr["ATTRIBUTE_NAME"].ToString().ToLower() == " SHORT_DESCRIPTION" || dr["ATTRIBUTE_NAME"].ToString() == "Descriptions" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "FEATURES" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATION" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "SPECIFICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "APPLICATIONS" || dr["ATTRIBUTE_NAME"].ToString().ToLower() == "NOTES")
                        if (package_name.Equals("PRODUCT"))
                        {
                            descall = descall.Replace("<br/>", " ").Replace("   "," ").Replace("  ", " ");
                            if(descall == "")
                                 _stmpl_records.SetAttribute("TBT_PROD_DESC_SHOW", true);
                            else
                                _stmpl_records.SetAttribute("TBT_PROD_DESC_SHOW", false);


                            if (descall.Length > 400)
                                _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
                            else
                                _stmpl_records.SetAttribute("TBT_MORE_SHOW", false);

                            if (descall.Length > 400)
                            {
                                //  _stmpl_records.SetAttribute("TBT_MORE_SHOW", true);
                                descallstring = descall.Substring(0, 400).ToString();
                                _stmpl_records.SetAttribute("TBT_MORE", descallstring);
                                _stmpl_records.SetAttribute("TBT_MENU_ID", "2");
                                descall = descall.Substring(0, descall.Length).ToString();
                                // descall = descall.Substring(300).ToString();

                                _stmpl_records.SetAttribute("TBT_DESCALL", descall);

                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_DESCALL", descall);
                                _stmpl_records.SetAttribute("TBT_MENU_ID", "2");
                                // _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);
                                _stmpl_records.SetAttribute("TBT_MORE", descall);
                            }

                            if(descall.Length > 0)
                                _stmpl_records.SetAttribute("TBT_DIS_BLK", "block");
                            else
                                _stmpl_records.SetAttribute("TBT_DIS_BLK", "none");
                        }
                        if (package_name.Equals("PRODUCT"))
                        {
                            //bool descflag = false;
                           // int familyrows = 0;
                            DataSet dsfamily = new DataSet();
                            //dsfamily = GetDataSet("select f.family_id,f.family_name,fs.string_value,a.attribute_id,a.attribute_name from tb_family f,tb_family_specs fs,tb_attribute a where f.family_id=fs.family_id and f.family_id =" + paraFID + " and fs.attribute_id=a.attribute_id and a.attribute_type=7 and a.attribute_id in(90,91,377,379,4)");
                            //if (dsfamily != null && dsfamily.Tables[0] != null && dsfamily.Tables[0].Rows.Count > 0)
                            //{
                            //    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dsfamily.Tables[0].Rows[0]["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;"));
                            //    foreach (DataRow _rows in dsfamily.Tables[0].Rows)
                            //    {
                            //        if (_rows["STRING_VALUE"].ToString().Trim() == "")
                            //        {
                            //            familyrows++;
                            //        }
                            //        _stmpl_records.SetAttribute("TBT_" + _rows["ATTRIBUTE_NAME"].ToString().ToUpper().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_"), _rows["STRING_VALUE"].ToString().Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("\r", "&nbsp;").Replace("\n", "<br/>"));
                            //    }
                            //    if (familyrows == dsfamily.Tables[0].Rows.Count)
                            //    {
                            //        _stmpl_records.SetAttribute("TBT_DESHEADER", "none");
                            //    }
                            //}
                            //else
                            //{
                            //    dsfamily = GetDataSet("select family_id,family_name from tb_family where family_id=" + paraFID);
                            //    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dsfamily.Tables[0].Rows[0]["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;"));
                            //    _stmpl_records.SetAttribute("TBT_DESHEADER", "none");
                            //}                                     

                            // _stmpl_records.SetAttribute("TBT_FAMILY_ID", cdr["FAMILY_ID"]);
                            //_stmpl_records.SetAttribute("TBT_FAMILY_NAME", cdr["FAMILY_NAME"].ToString().Replace("<ars>g</ars>", "&rarr;"));
                            //_stmpl_records.SetAttribute("TBT_DESHEADER", "none");
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_FAMILY_ID", paraFID);
                        }
                        if (package_name.Equals("PRODUCT") || package_name.Equals("COMPARE") || package_name.Equals("NEWPRODUCT") || package_name.Equals("PROMOTIONS") || package_name.Equals("NEWPRODUCTNAV") || package_name.Equals("MOREPRODUCTS") || package_name.Equals("NEWPRODUCTHIGHLIGHTSCATLIST"))
                        {

                            if (package_name.Equals("PRODUCT"))
                            {
                                if (cdr["QTY_AVAIL"] != null && cdr["MIN_ORD_QTY"] != null)
                                {
                                    if (Convert.ToInt32(cdr["QTY_AVAIL"].ToString()) > 0)
                                    {
                                        // _stmpl_records.SetAttribute("TBT_QTY_AVAIL", cdr["QTY_AVAIL"].ToString());
                                        // _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", cdr["MIN_ORD_QTY"].ToString());
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                    }
                                }
                                else
                                {
                                    _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                }
                             
                                if (cdr["FAMILY_PROD_COUNT"] != null && cdr["PROD_COUNT"] != null)
                                {
                                    if (!cdr["FAMILY_PROD_COUNT"].ToString().Equals(cdr["PROD_COUNT"].ToString()))
                                    {
                                        DataSet _parentFamilyds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, cdr["FAMILY_ID"].ToString(), "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
                                        string _parentFamily_Id = "0";
                                        if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                                            _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

                                        if (_parentFamily_Id.Equals("0"))
                                            _parentFamily_Id = cdr["FAMILY_ID"].ToString();

                                        _stmpl_records.SetAttribute("TBT_PARENT_FAMILY_ID", _parentFamily_Id);

                                        _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "block");
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");
                                    }
                                    //objHelperService.Cons_NewURl(_stmpl_records, _stmpl_records.GetAttribute("ORG_TBT_EA_PATH") + "////" + cdr["FAMILY_name"].ToString(), "fl.aspx", true, "");
                                }
                                else
                                    _stmpl_records.SetAttribute("TBT_DISPLAY_TIP", "none");


                                if (package_name.Equals("PRODUCT"))
                                {

                                    strurlnew = objHelperService.Cons_NewURl(_stmpl_records, _stmpl_records.GetAttribute("ORG_TBT_EA_PATH") + "////" +  _stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID") + "=" + cdr["FAMILY_name"].ToString(), "fl.aspx", true, "");
                                    _stmpl_records.SetAttribute("TBT_REWRITEURL_NEW", strurlnew);

                                   // string forgurl = objHelperService.cons_NEWURL_ORG(_stmpl_records, _stmpl_records.GetAttribute("ORG_TBT_EA_PATH") + "////" + _stmpl_records.GetAttribute("TBT_PARENT_FAMILY_ID") + "=" + cdr["FAMILY_name"].ToString(), "fl.aspx", true, "");
                                   // SetCookie("family_url", "", "", "", strurlnew,"","");
                                    //SetCookie("family_url_org", "", "", "", "", forgurl,"");
                                  

                                    
                                } 

                            }

                            else
                            {
                                DataSet dsProduct = new DataSet();
                                //dsProduct = GetDataSet("SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + cdr["PRODUCT_ID"].ToString());
                                dsProduct = (DataSet)objHelperDB.GetGenericDataDB(cdr["PRODUCT_ID"].ToString(), "GET_SINGLE_PRODUCT_INVENTORY", HelperDB.ReturnType.RTDataSet);
                                if (dsProduct != null && dsProduct.Tables[0] != null && dsProduct.Tables[0].Rows.Count > 0)
                                {
                                    if (Convert.ToInt32(dsProduct.Tables[0].Rows[0]["QTY_AVAIL"]) > 0)
                                    {
                                        _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dsProduct.Tables[0].Rows[0]["QTY_AVAIL"].ToString());
                                        _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dsProduct.Tables[0].Rows[0]["MIN_ORD_QTY"].ToString());
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                    }
                                }
                                else
                                {
                                    if (package_name.Equals("PRODUCT"))
                                    {
                                        _stmpl_records.SetAttribute("TB_DISPLAY", "none");
                                    }
                                }
                            }
                            string prodValues = "";
                            /*foreach (DataRow dr in dsrecords.Tables[dsrecords.Tables.Count - 1].Select("ATTRIBUTE_TYPE <> 3 AND ATTRIBUTE_ID NOT IN (1,5,450,449,481,482,483,484,485,486,487,488,489,490,491,492,493,494,495,496,497,498,499,500,501,502,503) AND PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString(), "ATTRIBUTE_NAME"))
                            {
                                
                                //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());                            
                                if( dr["attribute_name"].ToString().ToUpper()!="SUIT" &&  dr["attribute_name"].ToString().ToUpper()!="BRAND")
                                if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                                {
                                    if(dr["STRING_VALUE"]!=null && dr["STRING_VALUE"].ToString() !="" && dr["ATTRIBUTE_NAME"].ToString().ToUpper() != "NEW PRODUCTS")
                                        prodValues = prodValues + "<TR><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + dr["STRING_VALUE"] + "</TD></TR>"; 
                                }
                                else if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("NUM"))
                                {
                                    if (dr["NUMERIC_VALUE"].ToString().Length > 0)
                                        if (dr["ATTRIBUTE_TYPE"].ToString() == "4")
                                        {
                                            prodValues = prodValues + "<TR><TD bgcolor=\"white\" style=\"border-color:Black;\" align=\"center\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + " " + objHelperService.GetOptionValues("CURRENCYFORMAT").ToString() + " " + Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString() + "</TD></TR>";
                                        }
                                        else
                                        {
                                            prodValues = prodValues + "<TR><TD bgcolor=\"white\" style=\"border-color:Black;\" align=\"center\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + Math.Round(Convert.ToDecimal(dr["NUMERIC_VALUE"]), 2).ToString() + "</TD></TR>";
                                        }
                                }

                            }*/
                            DataRow[] ATTRows = dsrecords.Tables[0].Select("ATTRIBUTE_TYPE <> 3 AND ATTRIBUTE_ID <> 1 and  PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString(), "ATTRIBUTE_NAME");
                            //MODIFIED BY INDU FOR TUNING 05AUG2014
                            //foreach (DataRow dr in dsrecords.Tables[0].Select("ATTRIBUTE_TYPE <> 3 AND ATTRIBUTE_ID <> 1 and  PRODUCT_ID = " + cdr["PRODUCT_ID"].ToString(), "ATTRIBUTE_NAME"))
                            //{
                            foreach (DataRow dr in ATTRows)
                            {
                                if (!dr["attribute_name"].ToString().ToUpper().Equals("SUIT") && !dr["attribute_name"].ToString().ToUpper().Equals("BRAND"))
                                    if (dr["ATTRIBUTE_DATATYPE"].ToString().ToUpper().StartsWith("TEX"))
                                    {
                                        if (dr["STRING_VALUE"] != null && !dr["STRING_VALUE"].ToString().Equals("") && !dr["ATTRIBUTE_NAME"].ToString().ToUpper().Equals("NEW PRODUCTS"))
                                           prodValues = prodValues + "<TR><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\"><strong>" + dr["attribute_name"] + ": </strong></TD><TD bgcolor=\"white\" align=\"center\" style=\"border-color:Black;\" width=\"250px\">" + dr["STRING_VALUE"] + "</TD></TR>";
                                           
                                    }
                            }
                            string _sPriceTable = "";
                            string _StockStatus = "";
                            string _Eta = "";
                            if (package_name.Equals("PRODUCT"))
                            {

                                _sPriceTable = GetProductPriceTable(Convert.ToInt32(cdr["PRODUCT_ID"]));
                                _StockStatus = cdr["STOCK_STATUS_DESC"].ToString().Replace("_", " ");

                                if (cdr["ETA"].ToString() != "")
                                {
                                   // _Eta = string.Format("<tr><td class=\"tdfirst bright\">ETA</td><td>" + cdr["ETA"].ToString() + "</td></tr>");
                                    // Eta new UI integrated 
                                    if( cdr["ETA"].ToString().Contains("PLEASE CALL"))
                                        _Eta = string.Format("<p class=\"pull-left width_size\">ETA</p><span class=\"mar_left_30 mar_right_30\"> : </span><span class=\"font_15\" style=\"text-transform: capitalize;\">" + cdr["ETA"].ToString().ToLower() + "</span>");
                                    else
                                    _Eta = string.Format("<p class=\"pull-left width_size\">ETA</p><span class=\"mar_left_30 mar_right_30\"> : </span><span class=\"font_15\">" + " Available for shipping on " + cdr["ETA"].ToString() + "</span>");
                                }
                            }
                            else
                            {
                                _sPriceTable = GetProductPriceTable(Convert.ToInt32(cdr["PRODUCT_ID"]));
                                _StockStatus = GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"]));
                            }
                            string _Tbt_Stock_Status = "";
                            string _Tbt_Stock_Status_1 = "";
                            bool _Tbt_Stock_Status_2 = false;
                            string _Tbt_Stock_Status_3 = "";
                            string _Colorcode1 = "";
                            string _Colorcode;
                            string _StockStatusTrim = _StockStatus.Trim();
                            bool blnstkstatus = false;



                            //switch (_StockStatusTrim)
                            //{
                            //    case "IN STOCK":
                            //        _Tbt_Stock_Status = "INSTOCK";
                            //        _Tbt_Stock_Status_2 = true;
                            //        _Colorcode = "#43A246";
                            //        break;
                            //    case "SPECIAL ORDER":
                            //        _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILBAILTY TO BE CONFIRMED";
                            //        _Tbt_Stock_Status_2 = true;
                            //        _Colorcode = "#43A246";
                            //        break;
                            //    case "DISCONTINUED":
                            //        _Tbt_Stock_Status = "DISCONTINUED";
                            //        _Tbt_Stock_Status_2 = false;
                            //        _Colorcode = "#ED1C24";
                            //        break;
                            //    case "TEMPORARY UNAVAILBLE":
                            //        _Tbt_Stock_Status = "TEMPORARY UNAVAILABLE NO ETA";
                            //        _Tbt_Stock_Status_2 = false;
                            //        _Colorcode = "#F9A023";
                            //        break;
                            //    case "OUT OF STOCK":
                            //        _Tbt_Stock_Status = "OUT OF STOCK";
                            //        _Tbt_Stock_Status_2 = false;
                            //        _Colorcode = "#F9A023";
                            //        _Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                            //        _Colorcode1 = "#43A246";
                            //        break;
                            //    default:
                            //        _Tbt_Stock_Status = _StockStatus;
                            //        _Tbt_Stock_Status_2 = true;
                            //        _Colorcode = "Black";
                            //        break;
                            //}

                            switch (_StockStatusTrim)
                            {
                                case "IN STOCK":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status = "INSTOCK";
                                    _Tbt_Stock_Status_2 = true;
                                    break;
                                case "SPECIAL ORDER":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                                    break;
                                case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                                    break;
                                case "SPECIAL ORDER PRICE &":
                                    _Colorcode = "#43A246";
                                    _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                                    break;
                                case "DISCONTINUED":
                                    _Colorcode = "#ED1C24";
                                    _Tbt_Stock_Status_2 = false;
                                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                                    break;
                                case "DISCONTINUED NO LONGER AVAILABLE":
                                    _Colorcode = "#ED1C24";
                                    _Tbt_Stock_Status_2 = false;
                                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                                    break;
                                case "DISCONTINUED NO LONGER":
                                    _Colorcode = "#ED1C24";
                                    _Tbt_Stock_Status_2 = false;
                                    _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                                    break;
                                case "TEMPORARY UNAVAILABLE":
                                    _Colorcode = "#F9A023";
                                    _Tbt_Stock_Status_2 = true;
                                    //_Tbt_Stock_Status = "<span style=\"color:" + _Colorcode + "\">TEMPORARY UNAVAILABLE NO ETA</span>";
                                    _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                                    break;
                                case "TEMPORARY UNAVAILABLE NO ETA":
                                    _Colorcode = "#F9A023";
                                    _Tbt_Stock_Status_2 = true;
                                    _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                                    break;
                                case "OUT OF STOCK":
                                    //_Tbt_Stock_Status_3 = "OUT OF STOCK";
                                    _Tbt_Stock_Status_2 = true;
                                    _Colorcode = "#F9A023";
                                    //_Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                                    _Tbt_Stock_Status_1 = "Contact us for ETA";
                                    _Tbt_Stock_Status_3 = "";
                                    _Colorcode1 = "#43A246";
                                    break;
                                case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                                    //_Tbt_Stock_Status_3 = "OUT OF STOCK";
                                    _Tbt_Stock_Status_2 = true;
                                    _Colorcode = "#F9A023";
                                    //_Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                                    _Tbt_Stock_Status_1 = "Contact us for ETA";
                                    _Tbt_Stock_Status_3 = "";
                                    _Colorcode1 = "#43A246";
                                    blnstkstatus = true;
                                    break;
                                case "OUT OF STOCK ITEM WILL":
                                    //_Tbt_Stock_Status_3 = "OUT OF STOCK";
                                    
                                    _Tbt_Stock_Status_2 = true;
                                    _Colorcode = "#F9A023";
                                    //_Tbt_Stock_Status_1 = "ITEM WILL BE BACK ORDERED";
                                    _Tbt_Stock_Status_1 = "Contact us for ETA";
                                    _Tbt_Stock_Status_3 = "";
                                    _Colorcode1 = "#43A246";
                                    break;
                                default:
                                    _Colorcode = "Black";
                                    _Tbt_Stock_Status = _StockStatusTrim;
                                    break;
                            }



                            if( blnstkstatus == true)
                                _stmpl_records.SetAttribute("TBT_STKSTATUS", false);
                            else
                                _stmpl_records.SetAttribute("TBT_STKSTATUS", true);

                            _stmpl_records.SetAttribute("TBT_COLOR_CODE", _Colorcode);

                            if (!_Tbt_Stock_Status.Equals(""))
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_STATUS_3", _Tbt_Stock_Status_3);
                            }
                            _stmpl_records.SetAttribute("TBT_COLOR_CODE_1", _Colorcode1);
                            _stmpl_records.SetAttribute("TBT_STOCK_STATUS_1", _Tbt_Stock_Status_1);
                            _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                            _stmpl_records.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);



                            if (_Tbt_Stock_Status.ToUpper() == "INSTOCK" || _Tbt_Stock_Status_1.ToUpper() == "INSTOCK")
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                            }
                            else if (_Tbt_Stock_Status.ToUpper() == "ITEM WILL BE BACK ORDERED" || _Tbt_Stock_Status_1.ToUpper() == "ITEM WILL BE BACK ORDERED")
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/OutOfStock");
                            }
                            else if (_Tbt_Stock_Status.Contains("SPECIAL") || _Tbt_Stock_Status_1.Contains("SPECIAL"))
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/LimitedAvailability");
                            }
                            else if (_Tbt_Stock_Status_2 == false)
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/Discontinued");
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                            }


                           // _stmpl_records.SetAttribute("TBT_COST", cdr["COST"]);
                           
                            //_stmpl_records.SetAttribute("TBT_STOCK_STATUS", GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"])));
                            _stmpl_records.SetAttribute("TBT_PRICE_TABLE", _sPriceTable);
                            _stmpl_records.SetAttribute("TBT_PDISPLAY_MODE", _sPriceTable == "" ? "none" : "");
                            if (!prodValues.Equals(""))
                            {
                                if (package_name.Equals("PRODUCT"))
                                {
                                    GetProductDetails(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records);
                                    //GetProductDesc(Convert.ToInt32(cdr["PRODUCT_ID"]), Convert.ToInt32(paraFID), _stmpl_records);
                                }
                                _stmpl_records.SetAttribute("TBT_ALL_PRODUCTVALUES", prodValues);
                                _stmpl_records.SetAttribute("TBT_DISPLAY_MODE", "");
                            }
                            else
                            {
                                _stmpl_records.SetAttribute("TBT_DISPLAY_MODE", "none");
                            }
                        }
                        if (package_name.Equals("PRODUCT")) // for download tab
                        {
                            ST_Product_Download(cdr["PRODUCT_ID"].ToString());

                            _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", downloadST);
                            _stmpl_records.SetAttribute("TBT_DOWNLOAD", true);
                            downloadST = "";


                            //if (isdownload == true)
                            //{
                            //    chkdwld = true;
                            //    string dwldmrge = ST_Downloads_Update();
                            //    dwldmrge = downloadST + dwldmrge;
                            //    _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", dwldmrge);
                            //    _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
                            //    downloadST = "";
                            //}
                            //else
                            //{
                            //    isdownload = true;
                            //    chkdwld = false;
                            //    _stmpl_records.SetAttribute("TBT_DOWNLOAD_DATA", ST_Downloads_Update());
                            //    _stmpl_records.SetAttribute("TBT_DOWNLOAD", isdownload);
                            //}
                            if (HttpContext.Current.Session["FNAME_PRODUCT"] != null)
                                _stmpl_records.SetAttribute("FNAME_BBPP", HttpContext.Current.Session["FNAME_PRODUCT"].ToString());
                            if (HttpContext.Current.Session["AQ_PD_CAPTCH_IMAGE"] != null)
                            {
                                _stmpl_records.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_PD_CAPTCH_IMAGE"].ToString());
                                _stmpl_records.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_PD_CAPTCH_VALUE"].ToString());
                            }

                        }
                       
                        if (_Package.ToString().Contains("FAMILYPAGE"))
                        {
                            DataSet dsProduct = new DataSet();
                            //dsProduct = GetDataSet("SELECT PRODUCT_ID,QTY_AVAIL,MIN_ORD_QTY FROM TBWC_INVENTORY WHERE PRODUCT_ID=" + cdr["PRODUCT_ID"].ToString());
                            dsProduct = (DataSet)objHelperDB.GetGenericDataDB(cdr["PRODUCT_ID"].ToString(), "GET_SINGLE_PRODUCT_INVENTORY", HelperDB.ReturnType.RTDataSet);
                            _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dsProduct.Tables[0].Rows[0]["QTY_AVAIL"].ToString());
                            _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dsProduct.Tables[0].Rows[0]["MIN_ORD_QTY"].ToString());
                        }
                        strValue = strValue + _stmpl_records.ToString();
                        if (ictcol.Equals(_grid_cols))
                        {
                            _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                            _stmpl_container.SetAttribute("TBWDataList", strValue);
                            if (ictrecords % 2 == 0)
                                _stmpl_container.SetAttribute("BGCOLOR", "TableEvenRow");
                            else
                                _stmpl_container.SetAttribute("BGCOLOR", "TableOddRow");
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                            ictrecords++; ictcol = 1; strValue = "";
                        }
                        else
                        {
                            ictcol++;
                        }
                    }
                    if (!strValue.Equals(""))
                    {
                        _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + _skin_container);
                        _stmpl_container.SetAttribute("TBWDataList", strValue);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        ictrecords++; ictcol = 1; strValue = "";
                    }
                }
            }

            _dict_inner_html[_skin_body_attribute] = lstrecords;
        }

        public string ST_Downloads_Update( Boolean isdata)
        {
            StringTemplateGroup _stg_container = null;
            StringTemplate _stmpl_container = null;
            try
            {

                _stg_container = new StringTemplateGroup("main", _SkinRootPath);

               // if (chkdwld == false)
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "DowloadUpdate");
               // else
                //    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "WithDowloadUpdate");

                if (HttpContext.Current.Session["AQ_PD_CAPTCH_IMAGE"] != null)
                {
                    _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_PD_CAPTCH_IMAGE"].ToString());
                    _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_PD_CAPTCH_VALUE"].ToString());
                }
                if(HttpContext.Current.Session["FNAME_PRODUCT"] != null)
                    _stmpl_container.SetAttribute("FNAME_DU", HttpContext.Current.Session["FNAME_PRODUCT"].ToString());
                if (isdata==true)
                    _stmpl_container.SetAttribute("TBT_DOWNLOAD", false);
                else
                    _stmpl_container.SetAttribute("TBT_DOWNLOAD", true);

                return _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();

            }
            return "";
        }
        //private void SetCookie( string option_name, string fname,string code,string img1,string family_url,string family_url_org,string product_url)
        //{


        //    HttpCookie RVPCookie = HttpContext.Current.Request.Cookies["RVPCookie"];
        //    if (RVPCookie == null)
        //        RVPCookie = new HttpCookie("RVPCookie");

        //    try
        //    {
        //        if (option_name == "family_name")
        //        {
        //            if (RVPCookie != null)
        //            {

        //                if (RVPCookie["family_name1"] != null && RVPCookie["family_name2"] != null && RVPCookie["family_name3"] != null && RVPCookie["family_name1"] != fname && RVPCookie["family_name2"] != fname && RVPCookie["family_name3"] != fname)
        //                {
        //                    if (RVPCookie["family_name_lastvisit1"] != null && RVPCookie["family_name_lastvisit2"] != null && RVPCookie["family_name_lastvisit3"] != null)
        //                    {


        //                        List<string> nameList = new List<string>();
        //                        nameList.Add(RVPCookie["family_name_lastvisit1"]);
        //                        nameList.Add(RVPCookie["family_name_lastvisit2"]);
        //                        nameList.Add(RVPCookie["family_name_lastvisit3"]);

        //                        nameList.Sort();

        //                        if (nameList[0].Contains("_family_name1") == true)
        //                        {
        //                            RVPCookie["family_name1"] = fname;
        //                            RVPCookie["family_name_lastvisit1"] = DateTime.Now.ToString() + "_family_name1";
        //                        }
        //                        else if (nameList[0].Contains("_family_name2") == true)
        //                        {
        //                            RVPCookie["family_name2"] = fname;
        //                            RVPCookie["family_name_lastvisit2"] = DateTime.Now.ToString() + "_family_name2";
        //                        }
        //                        else if (nameList[0].Contains("_family_name3") == true)
        //                        {
        //                            RVPCookie["family_name3"] = fname;
        //                            RVPCookie["family_name_lastvisit3"] = DateTime.Now.ToString() + "_family_name3";
        //                        }

        //                    }
        //                }
        //            }
        //            if (RVPCookie != null)
        //            {
        //                if (RVPCookie["family_name1"] == null && RVPCookie["family_name1"] != fname)
        //                {
        //                    RVPCookie["family_name1"] = fname;
        //                    RVPCookie["family_name_lastvisit1"] = DateTime.Now.ToString() + "_family_name1";
        //                }
        //                else if (RVPCookie["family_name1"] != null && RVPCookie["family_name2"] == null && RVPCookie["family_name3"] == null && RVPCookie["family_name1"] != fname && RVPCookie["family_name2"] != fname)
        //                {
        //                    RVPCookie["family_name2"] = fname;
        //                    RVPCookie["family_name_lastvisit2"] = DateTime.Now.ToString() + "_family_name2";
        //                }
        //                else
        //                {
        //                    if (RVPCookie["family_name3"] == null && RVPCookie["family_name3"] != fname && RVPCookie["family_name1"] != null && RVPCookie["family_name2"] != null && RVPCookie["family_name1"] != fname && RVPCookie["family_name2"] != fname)
        //                    {
        //                        RVPCookie["family_name3"] = fname;
        //                        RVPCookie["family_name_lastvisit3"] = DateTime.Now.ToString() + "_family_name3";
        //                    }

        //                }
        //            }
        //            //else
        //            //{
        //            //    //HttpCookie RVPCookie = new HttpCookie("RVPCookie");
        //            //    RVPCookie["family_name1"] = fname;
        //            //}
        //            RVPCookie.Expires = DateTime.Now.AddDays(1);
        //            HttpContext.Current.Response.AppendCookie(RVPCookie);
        //        }

        //        if (option_name == "code")
        //        {
        //            if (RVPCookie != null)
        //            {

        //                if (RVPCookie["code1"] != null && RVPCookie["code2"] != null && RVPCookie["code3"] != null && RVPCookie["code1"] != code && RVPCookie["code2"] != code && RVPCookie["code3"] != code)
        //                {
        //                    if (RVPCookie["code_lastvisit1"] != null && RVPCookie["code_lastvisit2"] != null && RVPCookie["code_lastvisit3"] != null)
        //                    {


        //                        List<string> nameList = new List<string>();
        //                        nameList.Add(RVPCookie["code_lastvisit1"]);
        //                        nameList.Add(RVPCookie["code_lastvisit2"]);
        //                        nameList.Add(RVPCookie["code_lastvisit3"]);

        //                        nameList.Sort();

        //                        if (nameList[0].Contains("_code1") == true)
        //                        {
        //                            RVPCookie["code1"] = code;
        //                            RVPCookie["code_lastvisit1"] = DateTime.Now.ToString() + "_code1";
        //                        }
        //                        else if (nameList[0].Contains("_code2") == true)
        //                        {
        //                            RVPCookie["code2"] = code;
        //                            RVPCookie["code_lastvisit2"] = DateTime.Now.ToString() + "_code2";
        //                        }
        //                        else if (nameList[0].Contains("_code3") == true)
        //                        {
        //                            RVPCookie["code3"] = code;
        //                            RVPCookie["code_lastvisit3"] = DateTime.Now.ToString() + "_code3";
        //                        }

        //                    }
        //                }
        //            }

        //            if (RVPCookie != null)
        //            {
        //                if (RVPCookie["code1"] == null && RVPCookie["code1"] != code)
        //                {
        //                    RVPCookie["code1"] = code;
        //                    RVPCookie["code_lastvisit1"] = DateTime.Now.ToString() + "_code1";
        //                }
        //                else if (RVPCookie["code1"] != null && RVPCookie["code2"] == null && RVPCookie["code3"] == null && RVPCookie["code1"] != code && RVPCookie["code2"] != code)
        //                {
        //                    RVPCookie["code2"] = code;
        //                    RVPCookie["code_lastvisit2"] = DateTime.Now.ToString() + "_code2";
        //                }
        //                else
        //                {
        //                    if (RVPCookie["code3"] == null && RVPCookie["code3"] != code && RVPCookie["code1"] != null && RVPCookie["code2"] != null && RVPCookie["code1"] != code && RVPCookie["code2"] != code)
        //                    {
        //                        RVPCookie["code3"] = code;
        //                        RVPCookie["code_lastvisit3"] = DateTime.Now.ToString() + "_code3";
        //                    }

        //                }
        //            }

        //            RVPCookie.Expires = DateTime.Now.AddDays(1);
        //            HttpContext.Current.Response.AppendCookie(RVPCookie);
        //        }
        //        if (option_name == "img1")
        //        {
        //            if (RVPCookie != null)
        //            {

        //                if (RVPCookie["img1"] != null && RVPCookie["img2"] != null && RVPCookie["img3"] != null && RVPCookie["img1"] != img1 && RVPCookie["img2"] != img1 && RVPCookie["img3"] != img1)
        //                {
        //                    if (RVPCookie["img_lastvisit1"] != null && RVPCookie["img_lastvisit2"] != null && RVPCookie["img_lastvisit3"] != null)
        //                    {


        //                        List<string> nameList = new List<string>();
        //                        nameList.Add(RVPCookie["img_lastvisit1"]);
        //                        nameList.Add(RVPCookie["img_lastvisit2"]);
        //                        nameList.Add(RVPCookie["img_lastvisit3"]);

        //                        nameList.Sort();

        //                        if (nameList[0].Contains("_img1") == true)
        //                        {
        //                            RVPCookie["img1"] = img1;
        //                            RVPCookie["img_lastvisit1"] = DateTime.Now.ToString() + "_img1";
        //                        }
        //                        else if (nameList[0].Contains("_img2") == true)
        //                        {
        //                            RVPCookie["img2"] = img1;
        //                            RVPCookie["img_lastvisit2"] = DateTime.Now.ToString() + "_img2";
        //                        }
        //                        else if (nameList[0].Contains("_img3") == true)
        //                        {
        //                            RVPCookie["img3"] = img1;
        //                            RVPCookie["img_lastvisit3"] = DateTime.Now.ToString() + "_img3";
        //                        }

        //                    }
        //                }
        //            }

        //            if (RVPCookie != null)
        //            {
        //                if (RVPCookie["img1"] == null && RVPCookie["img1"] != img1)
        //                {
        //                    RVPCookie["img1"] = img1;
        //                    RVPCookie["img_lastvisit1"] = DateTime.Now.ToString() + "_img1";
        //                }
        //                else if (RVPCookie["img1"] != null && RVPCookie["img2"] == null && RVPCookie["img3"] == null && RVPCookie["img1"] != img1 && RVPCookie["img2"] != img1)
        //                {
        //                    RVPCookie["img2"] = img1;
        //                    RVPCookie["img_lastvisit2"] = DateTime.Now.ToString() + "_img2";
        //                }
        //                else
        //                {
        //                    if (RVPCookie["img3"] == null && RVPCookie["img3"] != img1 && RVPCookie["img1"] != null && RVPCookie["img2"] != null && RVPCookie["img1"] != img1 && RVPCookie["img2"] != img1)
        //                    {
        //                        RVPCookie["img3"] = img1;
        //                        RVPCookie["img_lastvisit3"] = DateTime.Now.ToString() + "_img3";
        //                    }

        //                }
        //            }

        //            RVPCookie.Expires = DateTime.Now.AddDays(1);
        //            HttpContext.Current.Response.AppendCookie(RVPCookie);
        //        }
        //        if (option_name == "family_url")
        //        {
        //            if (RVPCookie != null)
        //            {
        //                family_url = HttpUtility.UrlEncode(family_url.Trim());
        //                family_url = family_url.Replace("%2f", "/").Replace("%3f", "?").Replace("%3d", "=").Replace("%26", "&");
        //                if (RVPCookie["family_url1"] != null && RVPCookie["family_url2"] != null && RVPCookie["family_url3"] != null && RVPCookie["family_url1"] != family_url && RVPCookie["family_url2"] != family_url && RVPCookie["family_url3"] != family_url)
        //                {
        //                    if (RVPCookie["family_url_lastvisit1"] != null && RVPCookie["family_url_lastvisit2"] != null && RVPCookie["family_url_lastvisit3"] != null)
        //                    {


        //                        List<string> nameList = new List<string>();
        //                        nameList.Add(RVPCookie["family_url_lastvisit1"]);
        //                        nameList.Add(RVPCookie["family_url_lastvisit2"]);
        //                        nameList.Add(RVPCookie["family_url_lastvisit3"]);

        //                        nameList.Sort();

        //                        if (nameList[0].Contains("_family_url1") == true)
        //                        {
        //                            RVPCookie["family_url1"] = family_url;
        //                            RVPCookie["family_url_lastvisit1"] = DateTime.Now.ToString() + "_family_url1";
        //                        }
        //                        else if (nameList[0].Contains("_family_url2") == true)
        //                        {
        //                            RVPCookie["family_url2"] = family_url;
        //                            RVPCookie["family_url_lastvisit2"] = DateTime.Now.ToString() + "_family_url2";
        //                        }
        //                        else if (nameList[0].Contains("_family_url3") == true)
        //                        {
        //                            RVPCookie["family_url3"] = family_url;
        //                            RVPCookie["family_url_lastvisit3"] = DateTime.Now.ToString() + "_family_url3";
        //                        }

        //                    }
        //                }
        //            }

        //            if (RVPCookie != null)
        //            {
        //                if (RVPCookie["family_url1"] == null && RVPCookie["family_url1"] != family_url)
        //                {
        //                    RVPCookie["family_url1"] = family_url;
        //                    RVPCookie["family_url_lastvisit1"] = DateTime.Now.ToString() + "_family_url1";
        //                }
        //                else if (RVPCookie["family_url1"] != null && RVPCookie["family_url2"] == null && RVPCookie["family_url3"] == null && RVPCookie["family_url1"] != family_url && RVPCookie["family_url2"] != family_url)
        //                {
        //                    RVPCookie["family_url2"] = family_url;
        //                    RVPCookie["family_url_lastvisit2"] = DateTime.Now.ToString() + "_family_url2";
        //                }
        //                else
        //                {
        //                    if (RVPCookie["family_url3"] == null && RVPCookie["family_url3"] != family_url && RVPCookie["family_url1"] != null && RVPCookie["family_url2"] != null && RVPCookie["family_url1"] != family_url && RVPCookie["family_url2"] != family_url)
        //                    {
        //                        RVPCookie["family_url3"] = family_url;
        //                        RVPCookie["family_url_lastvisit3"] = DateTime.Now.ToString() + "_family_url3";
        //                    }

        //                }
        //            }

        //            RVPCookie.Expires = DateTime.Now.AddDays(1);
        //            HttpContext.Current.Response.AppendCookie(RVPCookie);
        //        }

        //        if (option_name == "family_url_org")
        //        {
        //            if (RVPCookie != null)
        //            {
        //                family_url_org = HttpUtility.UrlEncode(family_url_org.Trim());
        //                family_url_org = family_url_org.Replace("%2f", "/").Replace("%3f", "?").Replace("%3d", "=").Replace("%26", "&");
        //                if (RVPCookie["family_url_org1"] != null && RVPCookie["family_url_org2"] != null && RVPCookie["family_url_org3"] != null && RVPCookie["family_url_org1"] != family_url_org && RVPCookie["family_url_org2"] != family_url_org && RVPCookie["family_url_org3"] != family_url_org)
        //                {
        //                    if (RVPCookie["family_url_org_lastvisit1"] != null && RVPCookie["family_url_org_lastvisit2"] != null && RVPCookie["family_url_org_lastvisit3"] != null)
        //                    {


        //                        List<string> nameList = new List<string>();
        //                        nameList.Add(RVPCookie["family_url_org_lastvisit1"]);
        //                        nameList.Add(RVPCookie["family_url_org_lastvisit2"]);
        //                        nameList.Add(RVPCookie["family_url_org_lastvisit3"]);

        //                        nameList.Sort();

        //                        if (nameList[0].Contains("_family_url_org1") == true)
        //                        {
        //                            RVPCookie["family_url_org1"] = family_url_org;
        //                            RVPCookie["family_url_org_lastvisit1"] = DateTime.Now.ToString() + "_family_url_org1";
        //                        }
        //                        else if (nameList[0].Contains("_family_url_org2") == true)
        //                        {
        //                            RVPCookie["family_url_org2"] = family_url_org;
        //                            RVPCookie["family_url_org_lastvisit2"] = DateTime.Now.ToString() + "_family_url_org2";
        //                        }
        //                        else if (nameList[0].Contains("_family_url_org3") == true)
        //                        {
        //                            RVPCookie["family_url_org3"] = family_url_org;
        //                            RVPCookie["family_url_org_lastvisit3"] = DateTime.Now.ToString() + "_family_url_org3";
        //                        }

        //                    }
        //                }
        //            }

        //            if (RVPCookie != null)
        //            {
        //                if (RVPCookie["family_url_org1"] == null && RVPCookie["family_url_org1"] != family_url_org)
        //                {
        //                    RVPCookie["family_url_org1"] = family_url_org;
        //                    RVPCookie["family_url_org_lastvisit1"] = DateTime.Now.ToString() + "_family_url_org1";
        //                }
        //                else if (RVPCookie["family_url_org1"] != null && RVPCookie["family_url_org2"] == null && RVPCookie["family_url_org3"] == null && RVPCookie["family_url_org1"] != family_url_org && RVPCookie["family_url_org2"] != family_url_org)
        //                {
        //                    RVPCookie["family_url_org2"] = family_url_org;
        //                    RVPCookie["family_url_org_lastvisit2"] = DateTime.Now.ToString() + "_family_url_org2";
        //                }
        //                else
        //                {
        //                    if (RVPCookie["family_url_org3"] == null && RVPCookie["family_url_org3"] != family_url_org && RVPCookie["family_url_org1"] != null && RVPCookie["family_url_org2"] != null && RVPCookie["family_url_org1"] != family_url_org && RVPCookie["family_url_org2"] != family_url_org)
        //                    {
        //                        RVPCookie["family_url_org3"] = family_url_org;
        //                        RVPCookie["family_url_org_lastvisit3"] = DateTime.Now.ToString() + "_family_url_org3";
        //                    }

        //                }
        //            }

        //            RVPCookie.Expires = DateTime.Now.AddDays(1);
        //            HttpContext.Current.Response.AppendCookie(RVPCookie);
        //        }
        //        if (option_name == "product_url")
        //        {
        //            if (RVPCookie != null)
        //            {
        //                product_url = HttpUtility.UrlEncode(product_url.Trim());
        //                product_url = product_url.Replace("%2f", "/").Replace("%3f", "?").Replace("%3d", "=").Replace("%26", "&");
        //                if (RVPCookie["product_url1"] != null && RVPCookie["product_url2"] != null && RVPCookie["product_url3"] != null && RVPCookie["product_url1"] != product_url && RVPCookie["product_url2"] != product_url && RVPCookie["product_url3"] != product_url)
        //                {
        //                    if (RVPCookie["product_url_lastvisit1"] != null && RVPCookie["product_url_lastvisit2"] != null && RVPCookie["product_url_lastvisit3"] != null)
        //                    {


        //                        List<string> nameList = new List<string>();
        //                        nameList.Add(RVPCookie["product_url_lastvisit1"]);
        //                        nameList.Add(RVPCookie["product_url_lastvisit2"]);
        //                        nameList.Add(RVPCookie["product_url_lastvisit3"]);

        //                        nameList.Sort();

        //                        if (nameList[0].Contains("_product_url1") == true)
        //                        {
        //                            RVPCookie["product_url1"] = product_url;
        //                            RVPCookie["product_url_lastvisit1"] = DateTime.Now.ToString() + "_product_url1";
        //                        }
        //                        else if (nameList[0].Contains("_product_url2") == true)
        //                        {
        //                            RVPCookie["product_url2"] = product_url;
        //                            RVPCookie["product_url_lastvisit2"] = DateTime.Now.ToString() + "_product_url2";
        //                        }
        //                        else if (nameList[0].Contains("_product_url3") == true)
        //                        {
        //                            RVPCookie["product_url3"] = product_url;
        //                            RVPCookie["product_url_lastvisit3"] = DateTime.Now.ToString() + "_product_url3";
        //                        }

        //                    }
        //                }
        //            }

        //            if (RVPCookie != null)
        //            {
        //                if (RVPCookie["product_url_org1"] == null && RVPCookie["product_url1"] != product_url)
        //                {
        //                    RVPCookie["product_url1"] = product_url;
        //                    RVPCookie["product_url_lastvisit1"] = DateTime.Now.ToString() + "_product_url1";
        //                }
        //                else if (RVPCookie["product_url1"] != null && RVPCookie["product_url2"] == null && RVPCookie["product_url3"] == null && RVPCookie["product_url1"] != product_url && RVPCookie["product_url2"] != product_url)
        //                {
        //                    RVPCookie["product_url2"] = product_url;
        //                    RVPCookie["product_url_lastvisit2"] = DateTime.Now.ToString() + "_product_url2";
        //                }
        //                else
        //                {
        //                    if (RVPCookie["product_url3"] == null && RVPCookie["product_url3"] != product_url && RVPCookie["product_url1"] != null && RVPCookie["product_url2"] != null && RVPCookie["product_url1"] != product_url && RVPCookie["product_url2"] != product_url)
        //                    {
        //                        RVPCookie["product_url3"] = product_url;
        //                        RVPCookie["product_url_lastvisit3"] = DateTime.Now.ToString() + "_product_url_org3";
        //                    }

        //                }
        //            }

        //            RVPCookie.Expires = DateTime.Now.AddDays(1);
        //            HttpContext.Current.Response.AppendCookie(RVPCookie);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //}



        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USERS PRICE IN ROUND VALUE ***/
        /********************************************************************************/
        private decimal GetMyPrice(int ProductID)
        {
            decimal retval = 0.00M;
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();

                if (userid == string.Empty)
                    userid =  ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

                //if (!string.IsNullOrEmpty(userid))
                //{
                //    string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                //    objHelperService.SQLString = sSQL;
                //    int pricecode = objHelperService.CI(objHelperService.GetValue("price_code"));

                //    if (!string.IsNullOrEmpty(userid))
                //    {
                //        string strquery = "";
                //        if (pricecode == 1)
                //        {
                //            strquery = string.Format("exec GetWESIncProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
                //        }
                //        else
                //        {
                //            strquery = string.Format("exec GetWESProductPrice {0},{1},{2}", ProductID, 1, HttpContext.Current.Session["USER_ID"]);
                //        }

                //        DataSet DSprice = new DataSet();
                //        objHelperService.SQLString = strquery;
                //        retval = Math.Round(Convert.ToDecimal(objHelperService.GetValue("Numeric_Value")), 2);
                //    }
                //}
                retval = objHelperDB.GetProductPrice(ProductID, 1, userid);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            return retval;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE STOCK STATUS USING PRODUCT ID ***/
        /********************************************************************************/
        private string GetStockStatus(int ProductID)
        {
            string Retval = "NO STATUS AVAILABLE";
            try
            {
                //string sSQL = string.Format("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = {0}", ProductID);
                //objHelperService.SQLString = sSQL;
                //Retval = objHelperService.GetValue("PROD_STK_STATUS_DSC").ToString().Replace("_", " ");
		        DataTable objrbl =(DataTable)objHelperDB.GetGenericDataDB(ProductID.ToString(), "GET_SINGLE_PRODUCT_INVENTORY", HelperDB.ReturnType.RTTable);
                if (objrbl != null )
                {
                    Retval = objrbl.Rows[0]["STOCK_STATUS"].ToString().Replace("_", " ");                    
                }
                else
                    Retval = "NO STATUS AVAILABLE";
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
            return Retval;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT PRICE TABLE DETAILS USING PRODUCT ID ***/
        /********************************************************************************/
        private string GetProductPriceTable(int ProductID)
        {

            string _sPriceTable = "";
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();

                if (userid == string.Empty)
                    userid =  ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
              


                if (!string.IsNullOrEmpty(userid))
                {
                    //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                    //objHelperService.SQLString = sSQL;
                    //int pricecode = objHelperService.CI(objHelperService.GetValue("price_code"));
                    int pricecode = objHelperDB.GetPriceCode(userid);
                    DataSet dsPriceTable = new DataSet();
                    //SqlDataAdapter oDa = new SqlDataAdapter();
                    //oDa.SelectCommand = new SqlCommand();
                    //oDa.SelectCommand.CommandText = "GetPriceTable";
                    //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                    //oDa.SelectCommand.Connection = new SqlConnection(  );
                    //oDa.SelectCommand.Parameters.Clear();
                    //oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
                    //oDa.SelectCommand.Parameters.AddWithValue("@UserID", userid);
                    //oDa.Fill(dsPriceTable, "Price");
                    dsPriceTable = objHelperDB.GetProductPriceTable(ProductID, Convert.ToInt32(userid.ToString()));
                    _sPriceTable = "";

                    int TotalCount = 0;
                    int RowCount = 0;
                    string[] P1 = null;
                    string[] P2 = null;
                    if (pricecode == 3)
                    {

                        DataRow[] dr = dsPriceTable.Tables["Price"].Select();
                        //foreach (DataRow oDr in dsPriceTable.Tables["Price"].Rows)
                        //{
                        foreach (DataRow oDr in dr)
                {
                    P1 = oDr["Price1"].ToString().Split('.');
                    P2 = oDr["Price2"].ToString().Split('.');
                           

                    //_sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.00}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" + oDr["Price1"] + "</span>", "<span itemprop=\"price\">" + oDr["Price2"] + "</span>");
                    if (P1[1].Length >= 4 && P2[1].Length >= 4)
                    {
                        if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                            _sPriceTable += string.Format("<tr><td align=\"center\" class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.0000}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.0000}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" + oDr["Price1"] + "</span>", "<span itemprop=\"price\">" + oDr["Price2"] + "</span>");
                        else
                            _sPriceTable += string.Format("<tr><td align=\"center\" class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.00}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" + oDr["Price1"] + "</span>", "<span itemprop=\"price\">" + oDr["Price2"] + "</span>");
                    }
                    else
                        _sPriceTable += string.Format("<tr><td align=\"center\" class=\"bg_grey3\">{0}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.00}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" + oDr["Price1"] + "</span>", "<span itemprop=\"price\">" + oDr["Price2"] + "</span>");

                            
                        }
                    }
                    else
                    {
                        bool bLastRow = false;

                        TotalCount = dsPriceTable.Tables["Price"].Rows.Count;
                        RowCount = 0;
                        DataRow[] dr = dsPriceTable.Tables["Price"].Select();

                        //foreach (DataRow oDr in dsPriceTable.Tables["Price"].Rows)
                        //{
                        foreach (DataRow oDr in dr)
                  {
                            RowCount = RowCount + 1;
                            if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))   // check whether it is Last Row
                            {
                                bLastRow = true;
                            }

                           // string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                            string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                            P1 = oDr["Price1"].ToString().Split('.');
                            P2 = oDr["Price2"].ToString().Split('.');
                            
                           // _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${1:0.00}</td><td class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>${2:0.00}</td></tr>", oDr["QTY"], "<span itemprop=\"price\">" + oDr["Price1"] + "</span>", "<span itemprop=\"price\">" + oDr["Price2"] + "</span>", _color);
                            if (P1[1].Length >= 4 && P2[1].Length >= 4)
                            {
                                if ((P1.Length > 0 && Convert.ToInt32(P1[1].Substring(2, 2)) > 1) || P2.Length > 0 && Convert.ToInt32(P2[1].Substring(2, 2)) > 1)
                                    _sPriceTable += string.Format("<tr><td align=\"center\" class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>$<span itemprop=\"price\">{1:0.0000}</span></td><td class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>$<span itemprop=\"price\">{2:0.0000}</span></td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                                else
                                    _sPriceTable += string.Format("<tr><td align=\"center\" class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>$<span itemprop=\"price\">{1:0.00}</span></td><td class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>$ <span itemprop=\"price\">{2:0.00}</span></td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                            }
                            else
                                _sPriceTable += string.Format("<tr><td align=\"center\" class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\"><meta itemprop=\"priceCurrency\" content=\"AUD\"/>$<span itemprop=\"price\">{1:0.00}</span></td><td class=\"{3}\" align=\"center\">$<span itemprop=\"price\">{2:0.00}</span></td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                          
                            
                           
                        }
                    }

                }
            }


            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                _sPriceTable="-1";
            }          
            return _sPriceTable;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT DETAILS BASED ON FAMILY ID AND PRODUCT ID ***/
        /********************************************************************************/

        private void GetProductDetails(int ProductID, int FamilyID, StringTemplate st)
        {
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            //SqlDataAdapter oDa = new SqlDataAdapter("GetProductDetails", oCon);
            //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            //oDa.SelectCommand.Parameters.Clear();
            //oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
            //oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
            DataSet oDs = new DataSet();
            //oDa.Fill(oDs, "PrdDetails");
            DataSet tmp = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            try
            {
                if (tmp != null)
                {
                    DataRow[] Dr = tmp.Tables[0].Select("ATTRIBUTE_TYPE=1");
                    if (Dr.Length > 0)
                    {
                        oDs.Tables.Add(Dr.CopyToDataTable());
                        oDs.Tables[0].TableName = "PrdDetails";
                    }
                }

                if (oDs != null & oDs.Tables.Count > 0)
                {
                    // DataRow[] Familyspec2 = oDs.Tables[0].Select("ATTRIBUTE_ID in (13,4,240,241,2,51,18)");
                    DataRow[] dro = oDs.Tables["PrdDetails"].Select();
                    foreach (DataRow oDr in dro)
                    {
                    //foreach (DataRow oDr in oDs.Tables["PrdDetails"].Rows)
                    //{
                        if (oDr["ATTRIBUTE_NAME"].ToString() != "Long Description" && oDr["ATTRIBUTE_NAME"].ToString() != "PROD_CODE")
                        {
                            if (!string.IsNullOrEmpty(oDr["STRING_VALUE"].ToString()))
                            {
                                ProductDetails oPrdDet = new ProductDetails();
                                oPrdDet.AttributeID = Convert.ToInt32(oDr["ATTRIBUTE_ID"]);
                                if (oDr["ATTRIBUTE_NAME"].ToString().ToUpper() == "CODE")
                                    oPrdDet.AttributeName = "Order Code";
                                else
                                    oPrdDet.AttributeName = oDr["ATTRIBUTE_NAME"].ToString();
                                oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString();
                                st.SetAttribute("TBT_PRODDETAILS", oPrdDet);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();

            }
            
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DESCRIPTION DETAILS OF PRODUCTS ***/
        /********************************************************************************/
        private void GetProductDesc(int ProductID, int FamilyID, StringTemplate st)
        {
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            //SqlDataAdapter oDa = new SqlDataAdapter("GetFamilyDetails", oCon);
            //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            //oDa.SelectCommand.Parameters.Clear();
            //oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
            DataSet oDs = new DataSet();

            DataSet tmp = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            try
            {
                if (tmp != null)
                {
                    DataRow[] Dr = tmp.Tables[0].Select("ATTRIBUTE_TYPE=7");
                    if (Dr.Length > 0)
                    {
                        oDs.Tables.Add(Dr.CopyToDataTable());
                        oDs.Tables[0].TableName = "PrdDetails";
                    }
                }
                if (oDs != null & oDs.Tables.Count > 0)
                {
                    // DataRow[] Familyspec2 = oDs.Tables[0].Select("ATTRIBUTE_ID in (13,4,240,241,2,51,18)");
                    DataRow[] drproddet = oDs.Tables["PrdDetails"].Select();
                    foreach (DataRow oDr in drproddet)
                    {

                    //     foreach (DataRow oDr in oDs.Tables["PrdDetails"].Rows)
                    //{
                        ProductDetails oPrdDet = new ProductDetails();
                        oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString();
                        //string attr = "TBT_" + oPrdDet.AttributeName;
                        st.SetAttribute("TBT_PRODDESC", oPrdDet);
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg=e;
                objErrorHandler.CreateLog();
            }
            
        
       
            //oDa.Fill(oDs, "PrdDetails");
            //if (oDs != null && oDs.Tables["PrdDetails"].Rows.Count > 0 && oDs.Tables.Count > 0)
            //{
            //    StringBuilder sBuilder = new StringBuilder();
            //    DataRow[] Familyspec2 = oDs.Tables[0].Select("ATTRIBUTE_ID in (13,4,240,241,2,51,18)");
            //    int[] AttributeIdList = new int[7];
            //    AttributeIdList[0] = 13;
            //    AttributeIdList[1] = 4;
            //    AttributeIdList[2] = 240;
            //    AttributeIdList[3] = 241;
            //    AttributeIdList[4] = 2;
            //    AttributeIdList[5] = 51;
            //    AttributeIdList[6] = 18;

            //    if (Familyspec2.Length > 0)
            //    {
            //        for (int i = 0; i < AttributeIdList.Length; i++)
            //        {
            //            foreach (DataRow oDr in oDs.Tables["PrdDetails"].Rows)
            //            {
            //                if (oDr["STRING_VALUE"].ToString().Length > 0)
            //                {

            //                    string t = oDr["ATTRIBUTE_ID"].ToString();
            //                    if (oDr["ATTRIBUTE_ID"].ToString() == AttributeIdList[i].ToString())
            //                    {
            //                        ProductDetails oPrdDet = new ProductDetails();
            //                        oPrdDet.AttributeID = Convert.ToInt32(oDr["ATTRIBUTE_ID"]);
            //                        oPrdDet.AttributeName = oDr["ATTRIBUTE_NAME"].ToString();

            //                        oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString().Replace("\r", "&nbsp;").Replace("\n", "<br/>");
            //                        //string attr = "TBT_" + oPrdDet.AttributeName;
            //                        st.SetAttribute("TBT_PRODDESC", oPrdDet);
            //                    }
            //                }
            //            }
            //        }
            //    }

            //    //foreach (DataRow oDr in oDs.Tables["PrdDetails"].Rows)
            //    //{
            //    //    ProductDetails oPrdDet = new ProductDetails();
            //    //    oPrdDet.AttributeID = Convert.ToInt32(oDr["ATTRIBUTE_ID"]);
            //    //    oPrdDet.AttributeName = oDr["ATTRIBUTE_NAME"].ToString();

            //    //    oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString();
            //    //    //string attr = "TBT_" + oPrdDet.AttributeName;
            //    //    st.SetAttribute("TBT_PRODDESC", oPrdDet);
            //    //}
            //}
            //else
            //{
            //    string sql_query = "select string_value from TB_PROD_SPECS where ATTRIBUTE_ID=(select ATTRIBUTE_ID from TB_ATTRIBUTE where ATTRIBUTE_NAME='Description') and product_id=" + ProductID;
            //    SqlDataAdapter oDa1 = new SqlDataAdapter(sql_query, oCon);

            //    DataSet oDs1 = new DataSet();
            //    oDa1.Fill(oDs1, "Productdetails");
            //    if (oDs1 != null)
            //    {
            //        foreach (DataRow oDr in oDs1.Tables["Productdetails"].Rows)
            //        {
            //            ProductDetails oPrdDet = new ProductDetails();
            //            oPrdDet.SpecValue = oDr["STRING_VALUE"].ToString();
            //            //string attr = "TBT_" + oPrdDet.AttributeName;
            //            st.SetAttribute("TBT_PRODDESC", oPrdDet);
            //        }
            //    }
            //}
               
            }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE PRODUCT IMAGES IN DIFFERENT FORMATS BASED ON PARAMETERS ***/
        /********************************************************************************/

        private void GetMultipleImages(int ProductID, int FamilyID,int SubFamilyId, StringTemplate st)
        {
            ConnectionDB objConnectionDB = new ConnectionDB();
            string strfile = HttpContext.Current.Server.MapPath("ProdImages");
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            SqlDataAdapter oDa = new SqlDataAdapter("GetProductImages", objConnectionDB.GetConnection());
            oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            oDa.SelectCommand.Parameters.Clear();
            oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
            
            oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
            oDa.SelectCommand.Parameters.AddWithValue("@SubFamilyId", SubFamilyId);
            DataSet oDs = new DataSet();
            oDa.Fill(oDs, "Images");
            bool firstImg = true;
            bool IsEmptyImg = true;
            int intSno = 0;
            objConnectionDB.CloseConnection();
            try
            { 
            if (oDs != null)
            {
                DataRow[] drimages = oDs.Tables["Images"].Select() ;
                foreach (DataRow oDr in drimages)
                {
                //    foreach (DataRow oDr in oDs.Tables["Images"].Rows)
                //{
                    ProductImage oPrd = new ProductImage();
                    if (File.Exists(string.Format(@"{0}/{1}", strfile, oDr["STRING_VALUE"].ToString().Replace("\\", "/"))))
                    {
                        oPrd.LargeImage = oDr["STRING_VALUE"].ToString().Replace("\\", "/");                        

                        //oPrd.Thumpnail = oPrd.LargeImage.ToLower().Replace("_images", "_th50");
                        //oPrd.SmallImage = oPrd.LargeImage.ToLower().Replace("_images", "_th");
                        //oPrd.MediumImage = oPrd.LargeImage.ToLower().Replace("_images", "_images_200");
                        intSno = intSno + 1;
                        if (oPrd.LargeImage.ToLower().Contains("_th") == true)
                        {
                            
                            string tmpimg = oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                            oPrd.LargeImage = objHelperService.SetImageFolderPath(tmpimg, "_th", "_images");
                            oPrd.Thumpnail = objHelperService.SetImageFolderPath(tmpimg, "_th", "_th50");
                            oPrd.SmallImage = objHelperService.SetImageFolderPath(tmpimg, "_th", "_th");
                            oPrd.MediumImage = objHelperService.SetImageFolderPath(tmpimg, "_th", "_images_200");
                            oPrd.Sno = intSno.ToString();
                        }                        
                        else
                        {
                            oPrd.Thumpnail = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_images", "_th50");
                            oPrd.SmallImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_images", "_th");
                            oPrd.MediumImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_images", "_images_200");
                            oPrd.Sno = intSno.ToString();
                        }
                        if (intSno == 1)
                        {
                            oPrd.Active  = "active";
                        }
                        IsEmptyImg = false;
                        st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                        if (firstImg)
                        {
                            st.SetAttribute("TBT_TWEB_IMAGE12", oPrd.MediumImage);
                            st.SetAttribute("TBT_TWEB_IMAGE12_LARGE", oPrd.LargeImage);
                            firstImg = false;
                        }
                    }
                }
                if (IsEmptyImg == true)
                {
                    ProductImage oPrd = new ProductImage();
                    intSno =1;

                        oPrd.LargeImage = "/images/noimage.gif";
                        oPrd.Thumpnail = "/images/noimage.gif";
                        oPrd.SmallImage = "/images/noimage.gif";
                        oPrd.MediumImage = "/images/noimage.gif";
                        oPrd.Sno = intSno.ToString();                    
                        oPrd.Active = "active";                    
                        st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                        st.SetAttribute("TBT_TWEB_IMAGE12", oPrd.MediumImage);
                        st.SetAttribute("TBT_TWEB_IMAGE12_LARGE", oPrd.LargeImage);
                       
                }
                if (IsEmptyImg == false)
                    st.SetAttribute("TBT_NOIMAGE", true);
                else
                    st.SetAttribute("TBT_NOIMAGE", false);

            }
            }
            
                catch (Exception e)
                {
                    objErrorHandler.ErrorMsg=e;
                    objErrorHandler.CreateLog();
                }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE FAMILY PAGE PRODUCT IMAGES IN DIFFERENT FORMATS ***/
        /********************************************************************************/
        private void GetFamilyMultipleImages(int FamilyID, StringTemplate st, DataSet oDs)
        {
            string strfile = HttpContext.Current.Server.MapPath("ProdImages");
            //SqlConnection oCon = new SqlConnection(_DBConnectionString);
            //SqlDataAdapter oDa = new SqlDataAdapter("GetFamilyImages", oCon);
            //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
            //oDa.SelectCommand.Parameters.Clear();
            //oDa.SelectCommand.Parameters.AddWithValue("@FamilyID", FamilyID);
           // DataSet oDs = new DataSet();
            //oDa.Fill(oDs, "Images");            
            DataTable dt = new DataTable();
            DataRow[] dr;
            int intSno = 0;
           // oDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
            bool firstImg = true;
            bool IsEmptyImg =true;
            try
            {
                if (oDs != null)
                {
                    dr = oDs.Tables[0].Select("ATTRIBUTE_TYPE=9 And ATTRIBUTE_ID Not in (746, 747)", "ATTRIBUTE_ID");
                    if (dr.Length > 0)
                    {
                       // dt = dr.CopyToDataTable();
                        foreach (DataRow oDr in dr)
                        {
                            ProductImage oPrd = new ProductImage();
                            if (File.Exists(string.Format(@"{0}/{1}", strfile, oDr["STRING_VALUE"].ToString().Replace("\\", "/"))))
                            {
                                intSno = intSno + 1;
                                oPrd.LargeImage = oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                                oPrd.Thumpnail = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_th50");
                                oPrd.SmallImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_th");
                                oPrd.MediumImage = objHelperService.SetImageFolderPath(oPrd.LargeImage.ToLower(), "_Images", "_images_200");

                                //oPrd.Thumpnail = oPrd.LargeImage.ToLower().Replace("_images", "_th50");
                                //oPrd.SmallImage = oPrd.LargeImage.ToLower().Replace("_images", "_th");
                                //oPrd.MediumImage = oPrd.LargeImage.ToLower().Replace("_images", "_images_200");
                                if (intSno == 1)
                                {
                                    oPrd.Active = "active";
                                }
                                IsEmptyImg =false;
                                st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                                if (firstImg)
                                {
                                    st.SetAttribute("TBT_TFWEB_IMAGE1", oPrd.MediumImage);
                                    st.SetAttribute("TBT_TFWEB_LIMAGE", oPrd.LargeImage);
                                    st.SetAttribute("ISImageexist", true);  
                                    firstImg = false;
                                }
                            }
                        }

                    }
                    else
                    {
                        string tempstr = "";
                        string tempstr1 = "";
                        string[] tempstrs = null;
                        dr = oDs.Tables[0].Select("ATTRIBUTE_TYPE=9 And ATTRIBUTE_ID in (746) And STRING_VALUE<>'noimage.gif'", "ATTRIBUTE_ID");
                        if (dr.Length > 0)
                        {
                           // dt = dr.CopyToDataTable();
                            foreach (DataRow oDr in dr)
                            {

                                ProductImage oPrd = new ProductImage();
                                if (File.Exists(string.Format(@"{0}/{1}", strfile, oDr["STRING_VALUE"].ToString().Replace("\\", "/"))))
                                {

                                    intSno = intSno + 1;
                                    string img = oDr["STRING_VALUE"].ToString().Replace("\\", "/");
                                    oPrd.LargeImage = objHelperService.SetImageFolderPath(img, "_th", "_images");

                                    oPrd.Thumpnail = objHelperService.SetImageFolderPath(img, "_th", "_th50");
                                    oPrd.SmallImage = objHelperService.SetImageFolderPath(img, "_th", "_th");
                                    oPrd.MediumImage = objHelperService.SetImageFolderPath(img, "_Th", "_images_200");

                                    //oPrd.Thumpnail = oPrd.LargeImage.ToLower().Replace("_images", "_th50");
                                    //oPrd.SmallImage = oPrd.LargeImage.ToLower().Replace("_images", "_th");
                                    //oPrd.MediumImage = oPrd.LargeImage.ToLower().Replace("_images", "_images_200");
                                    if (intSno == 1)
                                    {
                                        oPrd.Active = "active";
                                    }
                                    IsEmptyImg =false;
                                    st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                                    if (firstImg)
                                    {
                                        st.SetAttribute("TBT_TFWEB_IMAGE1", oPrd.MediumImage);
                                        st.SetAttribute("TBT_TFWEB_LIMAGE", oPrd.LargeImage);
                                        st.SetAttribute("ISImageexist", true);  
                                        firstImg = false;
                                    }
                                }

                            }

                        }
                    }

                }

                  if (IsEmptyImg == true)
                {
                    ProductImage oPrd = new ProductImage();
                    intSno =1;

                        oPrd.LargeImage = "/images/noimage.gif";
                        oPrd.Thumpnail = "/images/noimage.gif";
                        oPrd.SmallImage = "/images/noimage.gif";
                        oPrd.MediumImage = "/images/noimage.gif";
                        oPrd.Sno = intSno.ToString();                    
                        oPrd.Active = "active";                    
                        st.SetAttribute("TBT_MULTIIMAGES", oPrd);
                        st.SetAttribute("TBT_TFWEB_IMAGE1", "/images/noimage.gif");
                        st.SetAttribute("TBT_TFWEB_LIMAGE", "/images/noimage.gif");
                       // st.SetAttribute("ISImageexist", false);  
                }

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
            }
          
            }



        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK WEATHER AN URL ALREADY EXIST OR NOT  ***/
        /********************************************************************************/
        private static bool UrlExists(string url)
        {
            bool retval = false;
            try
            {
                new System.Net.WebClient().DownloadData(url);
                retval = true;
            }
            catch (System.Net.WebException)
            {


            }
            return retval;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO BUILD ROW WISE TEMPLATE RECORDS   ***/
        /********************************************************************************/
        private void BuildRecordsTemplateRow()
        {
            TBWDataList[] lstrecords = new TBWDataList[0];
            string package_name = _Package.ToString();
            //Build the cell inner body of the HTML
            _stg_records = new StringTemplateGroup(_skin_records, _SkinRootPath);
            DataSet dsrecords = null;
            string strFile = HttpContext.Current.Server.MapPath("ProdImages");
            if (_GDataSet != null && _skin_sql_records.Length == 0)
            {
                dsrecords = _GDataSet;
            }
            else if (package_name.Equals("TOP") || package_name.Equals("TOPLOG"))
            {         
                DataSet tmpds=EasyAsk.GetCategoryAndBrand("MainCategory") ;
                if(tmpds!=null)
                { // remove WES NEWS MENU 
                 DataRow[] dr= tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'","CATEGORY_NAME" );
                    //TO ADD WESNEWS
                 //DataRow[] dr=tmpds.Tables[0].Select("CATEGORY_ID<>''", "CATEGORY_NAME");
                    if (dr.Length>0)
                    {
                      dsrecords  =new DataSet();
                      dsrecords.Tables.Add(dr.CopyToDataTable().Copy());   
                    }
                }
                else
                    dsrecords=null;                 
                  
            }

            else if (package_name.Equals("BOTTOM"))
            {
                DataSet tmpds = EasyAsk.GetCategoryAndBrand("MainCategory");
                if (tmpds != null)
                { // remove WES NEWS MENU 
               //DataRow[] dr = tmpds.Tables[0].Select("CATEGORY_ID<>'" + WesNewsCategoryId + "'", "CATEGORY_NAME");
                    //TO  ADD WESNEWS IN MENU
                    DataRow[] dr = tmpds.Tables[0].Select("CATEGORY_ID<>''", "CATEGORY_NAME");
                  
                    if (dr.Length > 0)
                    {
                        dsrecords = new DataSet();
                        dsrecords.Tables.Add(dr.CopyToDataTable().Copy());
                    }
                }
                else
                    dsrecords = null;

            }

            else if (package_name.Equals("BROWSEBYCATEGORY") || package_name.Equals("BROWSEBYBRAND") || package_name.Equals("BROWSEBYPRODUCT")) // unwanted db calls
            {
                dsrecords = null;
            }
            else
            {
                dsrecords = GetDataSet(_skin_sql_records, _skin_sql_type_records, _skin_sql_param_records);
            }
            _stg_container = new StringTemplateGroup(_skin_container, _SkinRootPath);

            if (dsrecords != null)
            {//dsrecords.Tables.Count - 1
                if (dsrecords.Tables[0].Rows.Count > 0)
                {

                    lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count];
                    int ictrecords = 0, ictcol = 1; string strValue = "";
                    DataRow[] drecords = dsrecords.Tables[0].Select() ;
                    
                       
                    foreach (DataRow dr in dsrecords.Tables[0].Rows)
                    {
                        if (package_name.Equals("CATEGORYLISTIMG"))
                        {
                            if (HttpContext.Current.Request.QueryString["tsb"] == null && HttpContext.Current.Request.QueryString["cid"] != null && !HttpContext.Current.Request.QueryString["cid"].Equals(""))
                                _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + _skin_records.ToString() + "1");
                            else
                                _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + _skin_records);
                        }
                        else
                            
                            _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + _skin_records);
                       

                        _stmpl_records.SetAttribute("TBT_ECOMENABLED", IsEcomenabled());
                        if (HttpContext.Current.Request.QueryString["cid"] != null)
                        {
                            _stmpl_records.SetAttribute("TBT_CAT_ID", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["cid"].ToString()));                            
                        }
                        if (HttpContext.Current.Request.QueryString["tsb"] != null)
                        {
                            _stmpl_records.SetAttribute("TBT_TOSUITE_BRAND", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["tsb"].ToString()));
                            _stmpl_records.SetAttribute("TBT_TOSUITE_BRAND1", HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["tsb"].ToString()));
                        }
                        if (package_name.Equals("CATEGORYLISTIMG"))
                        {
                            if (HttpContext.Current.Session["EA"] != null)
                            {
                                _stmpl_records.SetAttribute("EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(HttpContext.Current.Session["EA"].ToString())));
                            }
                        }
                        if (HttpContext.Current.Request.QueryString["sl1"] != null)
                            _stmpl_records.SetAttribute("TBT_TOSUITE_SL1", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["sl1"].ToString()));
                        if (HttpContext.Current.Request.QueryString["sl2"] != null)
                            _stmpl_records.SetAttribute("TBT_TOSUITE_SL2", HttpUtility.UrlEncode(HttpContext.Current.Request.QueryString["sl2"].ToString()));
                       
                        foreach (DataColumn dc in dsrecords.Tables[0].Columns)
                       
                        {
                            //_stmpl_records.SetAttribute(dc.ColumnName, dr[dc.ColumnName].ToString());                                                       
                           
                            string dc_columnname = dc.ColumnName.ToString();

                            if ((package_name.Equals("TOP") || package_name.Equals("TOPLOG")) && dc_columnname.ToUpper().Equals("CATEGORY_NAME") && dr[dc_columnname].ToString().Length > 8)
                            {
                                string sttr = dr[dc_columnname].ToString();
                                int indx = sttr.IndexOf(" ", 7);
                                if (indx >= 7)
                                    sttr = sttr.Substring(0, indx) + "<br/>" + sttr.Substring(indx + 1);
                                else if (sttr.Equals("VCR COMPONENTS"))
                                    sttr = "VCR<br/>COMPONENTS";
                                _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), sttr);
                            }
                            else
                            {
                                if (dc_columnname.ToUpper().Equals("IMAGE_FILE"))
                                {
                                    FileInfo Fil = new FileInfo(strFile + dr[dc_columnname].ToString());
                                    if (Fil.Exists)
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc_columnname].ToString());
                                    }
                                    else
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), "");
                                    }
                                }
                                else
                                {
                                    if (dc_columnname.ToUpper().Equals("CUSTOM_NUM_FIELD3") && dr[dc_columnname].ToString().Equals("2"))
                                    {
                                        _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc_columnname].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));// + "&bypcat=1");
                                    }
                                    else if (dc_columnname.ToUpper().ToString() == "EA_PATH" && (package_name.Equals("TOP") || package_name.Equals("TOPLOG")))
                                    {
                                        _stmpl_records.SetAttribute("TBT_EAPATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(dr["EA_PATH"].ToString())));
                                    }
                                    else
                                    {
                                        if (dc_columnname.ToUpper().Equals("TOSUITE_MODEL") || dc_columnname.ToUpper().Equals("TOSUITE_BRAND"))
                                        {
                                            _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), HttpUtility.UrlEncode(dr[dc_columnname].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")));
                                            _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper() + "1", dr[dc_columnname].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));

                                            if (package_name.Equals("CATEGORYLISTIMG"))
                                            {
                                                string[] parentcategory = HttpContext.Current.Session["EA"].ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                                                string parentcat = parentcategory[2];
                                                if (parentcat == "")
                                                {
                                                    if (HttpContext.Current.Request.QueryString["cid"] != null)
                                                    {
                                                        DataTable dt1 = (DataTable)HttpContext.Current.Session["dtcid"];

                                                        DataRow[] foundRows;
                                                        string cid = HttpContext.Current.Request.QueryString["cid"].ToString();
                                                        foundRows = dt1.Select("cid='" + cid + "' ");
                                                        if (foundRows.Length > 0)
                                                        {
                                                            parentcat = foundRows[0][1].ToString();

                                                        }
                                                    }


                                                }
                                              string model= _stmpl_records.GetAttribute("TBT_TOSUITE_MODEL").ToString();  
                                                objHelperService.Cons_NewURl(_stmpl_records, "//// ////" + parentcat + "////" + HttpContext.Current.Request.QueryString["tsb"] + "////" +model , "bb.aspx", true, "");

                                            }
                                        }
                                        else
                                        {
                                            _stmpl_records.SetAttribute("TBT_" + dc_columnname.Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper(), dr[dc_columnname].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                                        }
                                    }
                                   
                                }
                            }

                        }
                        if (package_name.Equals("CSFAMILYPAGE"))
                        {
                            _stmpl_records.SetAttribute("TBT_FAMILYIMAGE", "My fl");
                        }
                        //Mofified by:Indu.For URLRewrite for PRODUCT CATEGORIES
                        if (package_name.Equals("TOP") || package_name.Equals("TOPLOG") || package_name.Equals("BOTTOM"))
                        {
                            if (HttpContext.Current.Request.Url.Segments[1].ToString().ToUpper().Equals("/Home.aspx") || HttpContext.Current.Session["URLINIHOME"] == null || HttpContext.Current.Request.Url.Segments[1].ToString().ToUpper().Equals("/Login.aspx"))
                            {
                                //objHelperService.Cons_NewURl(_stmpl_records, dr["EA_PATH"].ToString(), "ct.aspx", true, "CATEGORY");
                                objHelperService.Cons_NewURl(_stmpl_records,"//// ////"+  dr["Category_name"].ToString(), "ct.aspx", true, "CATEGORY");
                           
                            }
                            else
                            {
                                objHelperService.Cons_NewURl(_stmpl_records, "//// ////" + dr["Category_name"].ToString(), "ct.aspx", false, "CATEGORY");
                                //objHelperService.Cons_NewURl(_stmpl_records, dr["EA_PATH"].ToString(), "ct.aspx", false, "CATEGORY");
                            }
                            if (dr["CATEGORY_ID"].ToString() != WesNewsCategoryId)
                        {
                            objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["Category_name"].ToString().ToLower());
                            
                            //objHelperService.Cons_NewURl(_stmpl_records, dr["EA_PATH"].ToString(), "ct.aspx", true, "CATEGORY");
                        }

                            else
                            {
                                objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), "New Products");
                            }
                        }

                        if (package_name.Equals("TOP") || package_name.Equals("TOPLOG"))
                        {
                        if (dr["CATEGORY_ID"].ToString() != WesNewsCategoryId)
                        {
                            strValue = strValue + _stmpl_records.ToString();

                        }

                        }
                        else
                        
                        {
                        strValue = strValue + _stmpl_records.ToString();

                        }
                        if (ictrecords == 10 && package_name.Equals("TOP"))
                        {
                            // strValue += "</ul></div></td><tr><td><div id=\"navcontainer\">";
                        }
                        if (ictcol.Equals(_grid_cols))
                        {
                            _stmpl_container = _stg_container.GetInstanceOf(package_name + "\\" + _skin_container);
                            _stmpl_container.SetAttribute("TBWDataList", strValue);
                            lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                            ictrecords++; ictcol = 1; strValue = "";
                        }
                        else
                        {
                            ictcol++;
                        }
                    }
                    if (!strValue.Equals(""))
                    {
                        _stmpl_container = _stg_container.GetInstanceOf(package_name + "\\" + _skin_container);
                        _stmpl_container.SetAttribute("TBWDataList", strValue);
                        lstrecords[ictrecords] = new TBWDataList(_stmpl_container.ToString());
                        ictrecords++; ictcol = 1; strValue = "";
                    }
                }
            }
            _dict_inner_html[_skin_body_attribute] = lstrecords;
        }

        //private DataSet GetDataSet(string SQLQuery)
        //{
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter da = new SqlDataAdapter(SQLQuery, _DBConnectionString);
        //    da.Fill(ds, "generictable");
        //    return ds;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE DATASET DETAILS BASED ON PARAMETERS  ***/
        /********************************************************************************/
        private DataSet GetDataSet(string SQLQuery, string SQLType, string SQLParam)
        {
            DataSet ds = new DataSet();
            try
            {
                if (paraValue != "" && SQLParam != "")
                    SQLQuery = SQLQuery.Replace(SQLParam, paraValue);
                if (_Package.Equals("BROWSEBYCATEGORY") && HttpContext.Current.Session["PARAFILTER"].Equals("Value"))
                {
                    if (SQLQuery.Contains("order by tc.category_id"))
                        SQLQuery = SQLQuery.Replace("order by tc.category_id", " and tC.CATEGORY_ID in (select distinct category_id from tb_family where family_id in (select family_id from tb_prod_family where product_id in  (select product_id from TBWC_SEARCH_PROD_LIST where user_session_id = '" + HttpContext.Current.Session.SessionID + "')))" + " order by tc.category_id");
                }
                ConnectionDB objConnectionDB = new ConnectionDB();
                SqlDataAdapter da = new SqlDataAdapter(SQLQuery, objConnectionDB.GetConnection());
                da.Fill(ds, "generictable");
                objConnectionDB.CloseConnection();
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg=e;
                objErrorHandler.CreateLog();
            }          
            return ds;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK ECOM  IS ENABLED OR NOT  ***/
        /********************************************************************************/
        private bool IsEcomenabled()
        {

            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            if (userid.Equals(string.Empty))
                userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
            return objHelperService.GetIsEcomEnabled(userid);
        }

        public void ST_Product_Download(string _pid)
        {
            string rtnstr = "";
            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_records1 = null;
            StringTemplate _stmpl_recordsrows = null;
            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];

            StringTemplateGroup _stg_container1 = null;
            StringTemplateGroup _stg_records1 = null;
            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];

            

            DataTable dt = new DataTable();
            DataRow[] dr = null;

            int ictrecords = 0;
            downloadST = "";
            isdownload = false;
            if (_pid != "")
            {
                 _stg_container = new StringTemplateGroup("main", SkinRootPath);
                        lstrecords = new TBWDataList[1];
                        _stmpl_container = _stg_container.GetInstanceOf(_Package  + "\\" + "DownloadMain");


                DataSet TempEADs = objFamilyServices.GetFamilyPageProduct(_pid, "PRODUCT_ATTACHMENT");
                if (TempEADs != null && TempEADs.Tables.Count > 0 && TempEADs.Tables[0].Rows.Count > 0)
                {
                    //TempEADs.Tables[0].Columns.Add("FAMILY_NAME");







                    rtnstr = ST_Productpage_Download(TempEADs.Tables[0]);
                    if (rtnstr != "")
                    {
                        lstrecords[ictrecords] = new TBWDataList(rtnstr.ToString());
                        ictrecords = ictrecords + 1;
                    }



                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);

                    if (ictrecords > 0)
                        _stmpl_container.SetAttribute("TBT_DOWNLOAD_MAIL", ST_Downloads_Update(true));
                    else
                        _stmpl_container.SetAttribute("TBT_DOWNLOAD_MAIL", ST_Downloads_Update(false));
                    //if (ictrecords > 0)
                    //{

                    //    downloadST = _stmpl_container.ToString();
                    //    isdownload=true;

                    //}

                }
                else
                {
                    _stmpl_container.SetAttribute("TBT_DOWNLOAD_MAIL", lstrecords);
                    _stmpl_container.SetAttribute("TBT_DOWNLOAD_MAIL", ST_Downloads_Update(false));
                }

            }
            
            
                downloadST= _stmpl_container.ToString();

        }

        public string ST_Productpage_Download(DataTable Adt)
        {
            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;

            TBWDataList[] lstrecords = new TBWDataList[0];
            TBWDataList[] lstrows = new TBWDataList[0];


            TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            TBWDataList1[] lstrows1 = new TBWDataList1[0];


            string strPDFFiles1 = HttpContext.Current.Server.MapPath("attachments");
            string strImgFiles1 = HttpContext.Current.Server.MapPath("ProdImages");
            long FileInKB;
            string[] file = null;
            string strfile = "";
            if (Adt != null && Adt.Rows.Count > 0)
            {




                DataSet dscat = new DataSet();


                try
                {
                    _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
                    _stg_container = new StringTemplateGroup("row", _SkinRootPath);


                    lstrecords = new TBWDataList[Adt.Rows.Count + 1];



                    int ictrecords = 0;

                    foreach (DataRow dr in Adt.Rows)//For Records
                    {
                        strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");


                        FileInfo Fil;

                       

                        if (dr["PRODUCT_ATT_FILE"].ToString().ToLower().Contains(".jpg") == true)
                            Fil = new FileInfo(strImgFiles1 + dr["PRODUCT_ATT_FILE"].ToString());
                        else
                            Fil = new FileInfo(strPDFFiles1 + dr["PRODUCT_ATT_FILE"].ToString());


                        if (Fil.Exists)
                        {
                            _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "DownloadCell");

                            strfile = dr["PRODUCT_ATT_FILE"].ToString().Replace(@"\\", "/");
                            strfile = strfile.Replace(@"\", "/");

                            file = strfile.Split(new string[] { @"/" }, StringSplitOptions.None);
                            if (file.Length > 0)
                            {
                                _stmpl_records.SetAttribute("TBT_ATTACH_FILE_NAME", file[file.Length - 1].ToString());
                                if (file[file.Length - 1].ToString().ToLower().Contains(".jpg") == true)
                                    _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "prodimages");
                                else
                                    _stmpl_records.SetAttribute("TBT_ATTACH_INIT_PATH", "attachments");
                            }

                            //  FileInBytes = Fil.Length;
                            FileInKB = Fil.Length / 1024;

                            _stmpl_records.SetAttribute("TBT_PRODUCT_ATT_DESC", dr["PRODUCT_ATT_DESC"].ToString());

                            _stmpl_records.SetAttribute("TBT_ATTACH_FILE_PATH", dr["PRODUCT_ATT_FILE"].ToString());
                            _stmpl_records.SetAttribute("TBT_ATTACH_SIZE", FileInKB.ToString() + " KB");


                            lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                            ictrecords++;
                        }
                    }

                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "DownloadRow");
                   

                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                    if (ictrecords > 0)
                        return _stmpl_container.ToString();

                }
                catch (Exception ex)
                {
                    objErrorHandler.ErrorMsg = ex;
                    objErrorHandler.CreateLog();
                    return "";
                }

                return "";
            }
            return "";
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE USER ECOM LOGIN NAME ***/
        /********************************************************************************/
        private string GetLoginName()
        {
            string retvalue = string.Empty;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            DataTable objDt = new DataTable();
            try
            {
            if (!string.IsNullOrEmpty(userid))
            {
                string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                //string sSQL = "SELECT CONTACT FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
                //objHelperService.SQLString = sSQL;
                //string iLoginName = objHelperService.GetValue("CONTACT");
                string iLoginName = "";
                if (HttpContext.Current.Session["ECOM_LOGIN_COMP"] == null)
                {
                    objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                    if (objDt != null && objDt.Rows.Count > 0)
                    {
                        iLoginName = objDt.Rows[0]["CONTACT"].ToString();
                        HttpContext.Current.Session["ECOM_LOGIN_COMP"] = objDt;
                    }

                }
                else
                {
                   // objDt = (DataTable)HttpContext.Current.Session["ECOM_LOGIN_COMP"];
                    objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                    if (objDt != null && objDt.Rows.Count > 0)
                        iLoginName = objDt.Rows[0]["CONTACT"].ToString();
                }
                retvalue = iLoginName;
            }
            }
                catch (Exception e)
            {
                    objErrorHandler.ErrorMsg=e;
                    objErrorHandler.CreateLog();
                    retvalue="-1";
                }
            finally
            {
                objDt.Dispose();
                objDt=null;
            }
            return retvalue;
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE ECOM COMPANY NAME ***/
        /********************************************************************************/
        private string GetCompanyName()
        {
            string retvalue = string.Empty;
            DataTable objDt = new DataTable();
            try
            {
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (!string.IsNullOrEmpty(userid))
                {
                    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
                    //string sSQL = "SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
                    //objHelperService.SQLString = sSQL;
                    //int iCompanyID = objHelperService.CI(objHelperService.GetValue("COMPANY_ID"));
                    //string sSQL1 = "SELECT COMPANY_NAME FROM TBWC_COMPANY WHERE WEBSITE_ID = " + websiteid + " and COMPANY_ID = " + iCompanyID;
                    //objHelperService.SQLString = sSQL1;
                    //retvalue = objHelperService.GetValue("COMPANY_NAME").ToString().Trim();
                    if (HttpContext.Current.Session["ECOM_LOGIN_COMP"] == null)
                    {
                        objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                        if (objDt != null && objDt.Rows.Count > 0)
                        {
                            HttpContext.Current.Session["ECOM_LOGIN_COMP"] = objDt;
                            retvalue = objDt.Rows[0]["COMPANY_NAME"].ToString();

                        }
                    }
                    else
                    {
                      //  objDt = (DataTable)HttpContext.Current.Session["ECOM_LOGIN_COMP"];
                        objDt = (DataTable)objHelperDB.GetGenericDataDB(_CATALOG_ID, websiteid, userid, "GET_ECOM_ENABLED", HelperDB.ReturnType.RTTable);
                        if (objDt != null && objDt.Rows.Count > 0)
                        {
                            retvalue = objDt.Rows[0]["COMPANY_NAME"].ToString();

                        }
                    }
                }
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg=e;
                objErrorHandler.CreateLog();
                retvalue="-1";
            }
            finally
            {
                objDt.Dispose();
                objDt=null;
            }
                return retvalue;
            }




        public string ST_BOTTOM_SUPPLIER(string templatepath)
        {
            string sHTML = "";

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();
            


               // _stg_records = new StringTemplateGroup("row", _SkinRootPath);
               // _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records = new StringTemplateGroup("row", _SkinRootPath + "\\" + templatepath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath + "\\" + templatepath);


                if (_Package == "CONTACTUSPAGE_SUPPLIER1")
                {
                    _stmpl_records = _stg_records.GetInstanceOf("Contactuspage_Supplier1" + "\\" + "cell");
                    _stmpl_container = _stg_container.GetInstanceOf("Contactuspage_Supplier1" + "\\" + "Main");
                }
                else if (_Package == "ABOUTUS_SUPPLIER1")
                {
                    _stmpl_records = _stg_records.GetInstanceOf("Aboutus_Supplier1" + "\\" + "cell");
                    _stmpl_container = _stg_container.GetInstanceOf("Aboutus_Supplier1" + "\\" + "Main");
                }
                else if (_Package == "LEFTNAV_SUPPLIER1")
                {
                     _stmpl_records = _stg_records.GetInstanceOf("LeftNav_Supplier1" + "\\" + "cell");
                     _stmpl_container = _stg_container.GetInstanceOf("LeftNav_Supplier1" + "\\" + "Main");
                     _stmpl_container.SetAttribute("SUPPIER_NAME", templatepath);

                     DataSet dscatname = new DataSet();
                     dscatname = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_MICROSITE_CATEGORY_NAME '" + templatepath + "'," +"''");
                     string href = "";
                     string href1 = "";
                     string url = "";
                    
                   
                    string catname = "";

                    string hreffinal = "";
                    string tbt_category = "";
                    string cat_id = "";

                    if (dscatname != null && dscatname.Tables.Count > 0 && dscatname.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i <= dscatname.Tables[0].Rows.Count - 1; i++)
                        {
                            //catname ="";
                            //hreffinal = "";
                            //url = "";
                            //href = "";
                            //href1 = "";
                            //catname = dscatname.Tables[0].Rows[i]["CATEGORY_NAME"].ToString();
                            //cat_id = dscatname.Tables[0].Rows[i]["CATEGORY_ID"].ToString();
                            //url = "/SupplierList/Supplier1/Cl_Supplier1.aspx?supplier_name=";
                            ////href = "<li>" + "<a href=" + url + templatepath.Trim() + "&CATEGORY_ID=" + cat_id+ ">";
                            //href = "<li>" + "<a href=" + url + HttpContext.Current.Request.QueryString["supplier_name"].ToString() + "&" + "category_id=" + cat_id + ">";
                            //href1 = "</a></li>";
                            //hreffinal = href + catname + href1;
                            //tbt_category = tbt_category + hreffinal;


                            catname = "";
                            hreffinal = "";
                            url = "";
                            href = "";
                            href1 = "";
                            catname = dscatname.Tables[0].Rows[i]["CATEGORY_NAME"].ToString();
                            cat_id = dscatname.Tables[0].Rows[i]["CATEGORY_ID"].ToString();
                            url = "/SupplierList/Supplier1/Cl_Supplier1.aspx?category_id=" + cat_id;
                            //href = "<li>" + "<a href=" + url + templatepath.Trim() + "&CATEGORY_ID=" + cat_id+ ">";
                            href = "<li>" + "<a href=" +"\"" + url +"&supplier_name="+ HttpContext.Current.Request.QueryString["supplier_name"].ToString()+"\"" +">";
                            href1 = "</a></li>";
                            hreffinal = href + catname + href1;
                            tbt_category = tbt_category + hreffinal;
                        }
                    }
                    _stmpl_container.SetAttribute("TBT_CATLIST", tbt_category.Trim());

                   // _stmpl_container.SetAttribute("TBT_CATLIST1", HttpContext.Current.Request.QueryString["supplier_name"].ToString()); 
                }
                else if (_Package == "CATEGORYLIST_SUPPLIER1")
                {
                    _stmpl_records = _stg_records.GetInstanceOf("Categorylist" + "\\" + "cell");
                    _stmpl_container = _stg_container.GetInstanceOf("Categorylist" + "\\" + "Main");
                    _stmpl_container.SetAttribute("SUPPIER_NAME", templatepath);

                    string cat_id ="";
                    if (HttpContext.Current.Request.QueryString["CATEGORY_ID"] != null)
                    {
                        cat_id = HttpContext.Current.Request.QueryString["CATEGORY_ID"].ToString();
                    }
                    DataSet dscatnamemicrosite = new DataSet();
                    dscatnamemicrosite = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_MICROSITE_CATEGORY_NAME '" + templatepath + "'"+",'"+ cat_id +"'");
                    if (dscatnamemicrosite != null && dscatnamemicrosite.Tables[0].Rows.Count > 0)
                    {
                        _stmpl_container.SetAttribute("MS_CATEGORY_NAME", dscatnamemicrosite.Tables[0].Rows[0]["CATEGORY_NAME"].ToString());
                        _stmpl_container.SetAttribute("MS_CATEGORY_DESC", dscatnamemicrosite.Tables[0].Rows[0]["CATEGORY_DESC1"].ToString()+dscatnamemicrosite.Tables[0].Rows[0]["CATEGORY_DESC"].ToString());
                    }
                }
                else
                {

                    _stmpl_records = _stg_records.GetInstanceOf("Bottom_Supplier1" + "\\" + "cell");
                    _stmpl_container = _stg_container.GetInstanceOf("Bottom_Supplier1" + "\\" + "Main");
                    _stmpl_container.SetAttribute("SUPPIER_NAME", templatepath.Trim());
                }

                string pac = _Package;

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }

        public string ST_TOPLOG_SUPPLIER( string templatepath)
        {
            string sHTML = "";

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                DataSet dsrecords = new DataSet();



                _stg_records = new StringTemplateGroup("row", _SkinRootPath + "\\" + templatepath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath + "\\" + templatepath);

                _stmpl_records = _stg_records.GetInstanceOf("Toplog_Supplier1" + "\\" + "cell");
                _stmpl_container = _stg_container.GetInstanceOf("Toplog_Supplier1" + "\\" + "Main");
             


                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
        }

        public string ST_Top_Load()
        {
            string sHTML = "";

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
              
                TBWDataList[] lstrecords = new TBWDataList[0];
       
                DataSet dsrecords = new DataSet();
              
                DataRow[] drs = null;

                dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");
             
               //  if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
                //     return "";


                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






               _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                //lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];



                //int ictrecords = 0;

                //foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                //{
                //    if (dr["Category_id"].ToString().ToUpper() != WesNewsCategoryId && dr["CATEGORY_NAME"].ToString().ToUpper() != "NOKIA")
                //    {
                //        if (_Package == "TOP")
                //            _stmpl_records = _stg_records.GetInstanceOf("Top" + "\\" + "cell");
                //        else
                //            _stmpl_records = _stg_records.GetInstanceOf("TopLog" + "\\" + "cell");

                //        _stmpl_records.SetAttribute("TBT_REWRITEURL", dr["URL_RW_PATH"].ToString());
                //        _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME_TOP"].ToString());

                //        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                //        ictrecords++;
                //    }
                //    objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
                //}

                if (_Package == "TOP")
                    _stmpl_container = _stg_container.GetInstanceOf("Top" + "\\" + "Main");
                else
                    _stmpl_container = _stg_container.GetInstanceOf("TopLog" + "\\" + "Main");

                _stmpl_container.SetAttribute("TBW_CART_ITEM", cartitem);

               

                    //if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].Equals("") && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                    //{
                    //    _stmpl_container.SetAttribute("TBT_LOGIN_NAME", GetLoginName());
                    //}

                    //if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                    //{
                    //    if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString()) == 4)
                    //    {
                    //        string ReMailLink = "<a Href=ConfirmMessage.aspx?Result=REMAILACTIVATION class=\"linkemailre\">Re-Email Activation Link Now</a>";

                    //        _stmpl_container.SetAttribute("TBT_LOGIN_NAME", " Your Account Has Not Been Activated! " + ReMailLink);
                    //    }
                    //    else
                    //        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", "");
                    //}
                    //else
                    //{
                    //    if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                    //    {
                    //        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", GetCompanyName() + ",");
                    //    }
                    //}

                    if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "" && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                        HttpContext.Current.Session["LOGIN_NAME"] = GetLoginName();

                    
                //}


                    string _Tbt_Order_Id = "";
                    string _Tbt_Ship_URL = "";
                    int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                    if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                    {
                        _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
                    }
                    else
                    {
                        _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID).ToString();
                        //    HttpContext.Current.Session["ORDER_ID"] = _Tbt_Order_Id;
                    }

                    if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                    {
                        _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                    }
                    else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
                    {
                        _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                    }
                    else
                    {
                        _Tbt_Ship_URL = "shipping.aspx";
                    }
                    _stmpl_container.SetAttribute("TBT_ORDER_ID", _Tbt_Order_Id);
                    _stmpl_container.SetAttribute("TBT_SHIP_URL", _Tbt_Ship_URL);

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
           // return ProdimageRreplaceImages(sHTML);
        }

        public string ST_Top_attribute_Filter()
        {
            string sHTML = "";

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_records1 = null;
                StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrows = new TBWDataList[0];

                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                TBWDataList1[] lstrows1 = new TBWDataList1[0];


                DataSet dscat = new DataSet();
                string _BCEAPath = "";
                DataRow[] drs = null;
                string selectedname ="";
                int ictrows = 0;
                bool isrecordexists = false;
                dscat = (DataSet)HttpContext.Current.Session["TOPAttributes"];

                _BCEAPath = GetEAPath();

                string strEA_PATH = "";
                string Filter_EA_Path = "";
                 string TBW_ATTRIBUTE_TYPE ="";
                                         string TBW_ATTRIBUTE_VALUE="";

                                         string pagetype = "mpl";
                _stg_records = new StringTemplateGroup("searchrsltcategorytoprecords", _SkinRootPath);
                _stg_container = new StringTemplateGroup("searchrsltcategoryleftmain", _SkinRootPath);



                if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mpl.aspx") == true)
                    pagetype="mpl";
                else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mps.aspx") == true)
                    pagetype="mps";
                else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mfl.aspx") == true)
                    pagetype = "mfl";
                else
                    pagetype = "mpl";


                if (dscat != null)
                {

                    if (dscat.Tables.Count > 0)
                        lstrows = new TBWDataList[dscat.Tables.Count + 1];

                    for (int i = 0; i < dscat.Tables.Count; i++)
                    {
                        Boolean tmpallow = true;

                        
                        if (tmpallow == true)
                        {
                            if (dscat.Tables[i].Rows.Count > 0)
                            {
                                lstrecords = new TBWDataList[dscat.Tables[i].Rows.Count + 1];
                                lstrecords1 = new TBWDataList1[dscat.Tables[i].Rows.Count + 1];
                                int ictrecords = 0;
                                selectedname = "";
                                int j = 0;
                                foreach (DataRow dr in dscat.Tables[i].Rows)//For Records
                                {

                                    isrecordexists = true;
                                    strEA_PATH="";

                                    if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("microsite.aspx") == true)
                                    {
                                        _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryTop" + "\\" + "cell");
                                    }

                                    else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mbb.aspx") == true)
                                    {
                                        _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryTop" + "\\" + "cell1");
                                    }
                                    else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mps.aspx") == true)
                                    {
                                        _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryTop" + "\\" + "cell2");
                                    }
                                    else if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mfl.aspx") == true)
                                    {
                                        _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryTop" + "\\" + "cell3");
                                    }
                                    else
                                    {
                                        _stmpl_records = _stg_records.GetInstanceOf("searchrsltcategoryTop" + "\\" + "cell");
                                    }

                        

                                    

                                    string brandname = string.Empty;
                                    if (dscat.Tables[i].TableName.Contains("Category"))
                                    {
                                     //   _stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(_parentCatID.ToString()));
                                        _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                        //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));

                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr["Category_Name"].ToString());
                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr["Category_Name"].ToString()));
                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                       
                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));


                                        if (dr["select"].ToString() == "1")
                                            selectedname = dr[1].ToString();
                                      
                                          TBW_ATTRIBUTE_TYPE =dscat.Tables[i].TableName.ToString().ToUpper();  // _stmpl_records.GetAttribute("TBW_ATTRIBUTE_TYPE").ToString().ToUpper();
                                          TBW_ATTRIBUTE_VALUE = dr["Category_Name"].ToString();
                                    }
                                    else
                                    {
                                        //_stmpl_records.SetAttribute("TBW_PARENT_CATEGORY_ID", HttpUtility.UrlEncode(ParentCatID.ToString()));
                                        //_stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(dr["CATEGORY_ID"].ToString()));
                                       // _stmpl_records.SetAttribute("TBW_CATEGORY_ID", HttpUtility.UrlEncode(_catCid.ToString()));
                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_NAME", dr[0].ToString());
                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_VALUE", HttpUtility.UrlEncode(dr[0].ToString()));
                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_BRAND", HttpUtility.UrlEncode(dr["brandvalue"].ToString()));
                                     

                                        _stmpl_records.SetAttribute("TBW_ATTRIBUTE_SEARCH", HttpUtility.UrlEncode(dr["SearchString"].ToString()));

                                        if (dr["select"].ToString() == "1")
                                            selectedname = dr[0].ToString();

                                          TBW_ATTRIBUTE_TYPE =dscat.Tables[i].TableName.ToString().ToUpper();  // _stmpl_records.GetAttribute("TBW_ATTRIBUTE_TYPE").ToString().ToUpper();
                                          TBW_ATTRIBUTE_VALUE = dr[0].ToString();
                                    }


                                    //_stmpl_records.SetAttribute("TBW_CUSTOM_NUM_FIELD3", _byp);
                                    //_stmpl_records.SetAttribute("TBW_BRAND", HttpUtility.UrlEncode(_tsb));
                                    //_stmpl_records.SetAttribute("TBW_MODEL", HttpUtility.UrlEncode(_tsm));
                                    //_stmpl_records.SetAttribute("TBW_FAMILY_ID", _fid);

                                    _stmpl_records.SetAttribute("TBW_ATTRIBUTE_TYPE", dscat.Tables[i].TableName.ToString());




                                    if (dr["eapath"].ToString() == "")
                                    {
                                        Filter_EA_Path = _BCEAPath;
                                        dr["eapath"] = _BCEAPath;
                                    }
                                    else
                                        Filter_EA_Path = dr["eapath"].ToString();

                                    if ((TBW_ATTRIBUTE_TYPE.ToUpper() == "CATEGORY") && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mbb.aspx")))
                                    {
                                       strEA_PATH= objHelperServices.Cons_NewURlASPX("", Filter_EA_Path + "////" + TBW_ATTRIBUTE_TYPE + "=" + TBW_ATTRIBUTE_VALUE, "mbb.aspx", true, TBW_ATTRIBUTE_TYPE);
                                    }

                                    else if (TBW_ATTRIBUTE_TYPE.ToUpper() != "CATEGORY")
                                    {
                                        // objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" + brandname + TBW_ATTRIBUTE_VALUE, "pl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                        if (_BCEAPath.ToLower().Contains("brand") == true)
                                        {
                                            //objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + TBW_ATTRIBUTE_TYPE + "=" + TBW_ATTRIBUTE_VALUE, "mpl.aspx", true, TBW_ATTRIBUTE_TYPE);
                                           strEA_PATH= objHelperServices.Cons_NewURlASPX("", Filter_EA_Path + "////" + TBW_ATTRIBUTE_TYPE + "=" + TBW_ATTRIBUTE_VALUE, pagetype+".aspx", true, TBW_ATTRIBUTE_TYPE);
                                        }
                                        else
                                        {
                                            strEA_PATH = objHelperServices.Cons_NewURlASPX("", Filter_EA_Path + "////" + TBW_ATTRIBUTE_TYPE + "=" + brandname + TBW_ATTRIBUTE_VALUE, pagetype+".aspx", true, TBW_ATTRIBUTE_TYPE);
                                        }
                                    }
                                    else
                                    {
                                        strEA_PATH=objHelperServices.Cons_NewURlASPX("", Filter_EA_Path + "////" + TBW_ATTRIBUTE_VALUE, pagetype+".aspx", true, TBW_ATTRIBUTE_TYPE);
                                    }

                                    _stmpl_records.SetAttribute("TBT_REWRITEURL", strEA_PATH);
                                    //  objHelperServices.Cons_NewURl(_stmpl_records, _BCEAPath + "////" + _stmpl_records.GetAttribute("TBW_ATTRIBUTE_NAME"), "pl.aspx", true, "");

                                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());

                                 
                                    ictrecords++;
                                }

                                j++;
                                _stmpl_recordsrows = _stg_container.GetInstanceOf("searchrsltcategoryTop" + "\\" + "row1");
                              

                                _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", dscat.Tables[i].TableName.ToString());
                                _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_ID", dscat.Tables[i].TableName.ToString().Replace("/", "").Replace(" ", ""));
                                _stmpl_recordsrows.SetAttribute("TBT_MENU_ID", (i + 1).ToString());
                                if( selectedname=="")
                                    _stmpl_recordsrows.SetAttribute("SELECT_ATT_NAME", dscat.Tables[i].TableName.ToString());
                                else
                                    _stmpl_recordsrows.SetAttribute("SELECT_ATT_NAME", selectedname);
                                
                                _stmpl_recordsrows.SetAttribute("TBWDataList", lstrecords);
                                //_stmpl_recordsrows.SetAttribute("TBWDataList1", lstrecords1);
                              //  sHTML = sHTML + _stmpl_recordsrows.ToString();

                                lstrows[ictrows] = new TBWDataList(_stmpl_recordsrows.ToString());
                                ictrows++;
                            }
                        }
                    }

                    if (isrecordexists == false)
                    {
                        sHTML= "";
                    }
                    else
                    {
                         if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("mfl.aspx") == true)                    
                            _stmpl_container = _stg_container.GetInstanceOf("searchrsltcategoryTop" + "\\" + "mainfl");
                         else
                             _stmpl_container = _stg_container.GetInstanceOf("searchrsltcategoryTop" + "\\" + "main");

                        _stmpl_container.SetAttribute("TBWDataList", lstrows);
                        //if (dscat.Tables.Count > 16)
                        //    _stmpl_container.SetAttribute("FILTER_HEIGHT_CLASS", "style=height:auto;");                        
                        //else if(dscat.Tables.Count>8)
                        //    _stmpl_container.SetAttribute("FILTER_HEIGHT_CLASS", "style=height:auto;");                        
                        //else
                        //    _stmpl_container.SetAttribute("FILTER_HEIGHT_CLASS", "style=height:auto");

                        sHTML = _stmpl_container.ToString();
                    }
                    HttpContext.Current.Session["TOPAttributes"] = dscat;
                }

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
            // return ProdimageRreplaceImages(sHTML);
        }
        public string ST_MainMenu()
        {
            string sHTML = "";

            try
            {

                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_container1 = null;
                StringTemplate _stmpl_records1 = null;

                TBWDataList[] lstrecords = new TBWDataList[0];
                // TBWDataList[] lstrows = new TBWDataList[0];

                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                // TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                DataSet dsrecords = new DataSet();
                DataSet dsMainCat = new DataSet();
                DataSet dsSubCat = new DataSet();
                DataRow[] drs = null;


                dsMainCat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
               // dsSubCat = EasyAsk.GetCategoryAndBrand("SubCategory");


                if (dsMainCat == null || dsMainCat.Tables.Count <= 0 || dsMainCat.Tables[0].Rows.Count <= 0)
                    return "";

                if (dsMainCat.Tables["Category"]==null)
                    return "";
                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("cell", _SkinRootPath);


                lstrecords = new TBWDataList[dsMainCat.Tables["Category"].Rows.Count + 1];



                int ictrecords = 0;
                int UlchangeCount = 0;
                
                string Subcatlist = "";
                string _BCEAPath = GetEAPath();
                foreach (DataRow dr in dsMainCat.Tables[0].Rows)//For Records
                {
                    Subcatlist = "";
                    ictrows = ictrows + 1;
                    _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "Row");
                    _stmpl_records.SetAttribute("TBT_REWRITEURL",  objHelperServices.Cons_NewURlASPX("", _BCEAPath + "////" + dr["CATEGORY_NAME"].ToString(), "mpl.aspx", true, "Category"));
                    _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());
                    _stmpl_records.SetAttribute("MENU_NO", ictrows);

                   

                    //if (dsSubCat != null && dsSubCat.Tables.Count > 0)
                    //{
                    //    DataRow[] subrows = dsSubCat.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + dr["CATEGORY_ID"].ToString() + "'");

                    //    if (subrows.Length > 0)
                    //    {
                    //        foreach (DataRow subdr in subrows)//For Records
                    //        {
                    //            _stmpl_records1 = _stg_records1.GetInstanceOf(_Package + "\\" + "cell");
                    //            _stmpl_records1.SetAttribute("TBT_REWRITEURL", subdr["URL_RW_PATH"].ToString());
                    //            _stmpl_records1.SetAttribute("TBT_CATEGORY_NAME", subdr["CATEGORY_NAME"].ToString());
                    //            Subcatlist = Subcatlist + _stmpl_records1.ToString();
                    //        }
                    //        _stmpl_records.SetAttribute("TBT_SUBCATEGORY_LIST", Subcatlist);

                    //        _stmpl_records.SetAttribute("TBT_SUB_MENU", true);
                    //    }
                    //    else
                    //    {
                    //        _stmpl_records.SetAttribute("TBT_SUB_MENU", false);
                    //    }
                    //}
                   // _stmpl_records.SetAttribute("TBT_SUBCATEGORY_LIST", false);
                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;

                    // objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
                }

                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");

   


                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
            // return ProdimageRreplaceImages(sHTML);
        }


        public string ST_TopMenu()
        {
            string sHTML = "";

            try
            {

                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_container1 = null;
                StringTemplate _stmpl_records1 = null;

                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrecords2 = new TBWDataList[0];
                TBWDataList[] lstrecords3 = new TBWDataList[0];
                // TBWDataList[] lstrows = new TBWDataList[0];

                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                // TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                DataSet dsrecords = new DataSet();
                DataTable dtMainCat = new DataTable();
                DataTable dtSubCat = new DataTable();
                DataRow[] drs = null;


                
                DataTable ttbl = GetSupplierDetail();
                if (ttbl != null && ttbl.Rows.Count > 0)
                {
                    strsupplierName = ttbl.Rows[0]["CATEGORY_NAME"].ToString();
                    strsupplierDesc = ttbl.Rows[0]["SHORT_DESC"].ToString();
                    strsupplierId = ttbl.Rows[0]["CATEGORY_ID"].ToString();
                    
                }
               // EasyAsk.GetMainMenuClickDetailJson(strsupplierId, "", true );
                DataSet Mainmnuds = (DataSet)HttpContext.Current.Session["MainMenuClick"];

                if (Mainmnuds == null || Mainmnuds.Tables.Count <= 0 || Mainmnuds.Tables["MainCategory"].Rows.Count <= 0)
                    return "";

                dtMainCat = Mainmnuds.Tables["MainCategory"];
                dtSubCat = Mainmnuds.Tables["SubCategory"];


                if (dtMainCat == null ||  dtMainCat.Rows.Count <= 0)
                    return "";


                //if (dsMainCat.Tables["Category"] == null)
                //    return "";
                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("cell", _SkinRootPath);


                lstrecords = new TBWDataList[dtMainCat.Rows.Count + 1];
                lstrecords2 = new TBWDataList[dtMainCat.Rows.Count + 1];
                lstrecords3 = new TBWDataList[dtMainCat.Rows.Count + 1];



                int ictrecords = 0;
                int UlchangeCount = 0;

                string Subcatlist = "";
                string Subcatlist2 = "";
                string Subcatlist3 = "";
                string _BCEAPath = GetEAPath();
                foreach (DataRow dr in dtMainCat.Rows)//For Records
                {
                    Subcatlist = "";
                    Subcatlist2 = "";
                    Subcatlist3 = "";
                    UlchangeCount = 0;
                    ictrows = ictrows + 1;
                    _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "Row");
                    _stmpl_records.SetAttribute("TBT_REWRITEURL", objHelperServices.Cons_NewURlASPX("", dr["EA_PATH"]  + "////" + dr["CATEGORY_NAME"].ToString(), "mpl.aspx", true, "Category"));
                    _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());
                    _stmpl_records.SetAttribute("MENU_NO", ictrows);
                    if (ictrows==1)
                        _stmpl_records.SetAttribute("ISFIRST_MENU", "block");
                    else
                         _stmpl_records.SetAttribute("ISFIRST_MENU", "none");

                    

                    if (dtSubCat != null && dtSubCat.Rows.Count > 0)
                    {
                        DataRow[] subrows = dtSubCat.Select("PARENT_CATEGORY_ID='" + dr["CATEGORY_ID"].ToString() + "'");

                        if (subrows.Length > 0)
                        {
                            foreach (DataRow subdr in subrows)//For Records
                            {
                             
                                _stmpl_records1 = _stg_records1.GetInstanceOf(_Package + "\\" + "cell");
                                //_stmpl_records1.SetAttribute("TBT_REWRITEURL", subdr["URL_RW_PATH"].ToString());
                                _stmpl_records1.SetAttribute("TBT_REWRITEURL", objHelperServices.Cons_NewURlASPX("", dr["EA_PATH"] + "////" + dr["CATEGORY_NAME"].ToString() + "////" + subdr["CATEGORY_NAME"].ToString(), "mpl.aspx", true, "Category"));

                                _stmpl_records1.SetAttribute("TBT_CATEGORY_NAME", subdr["CATEGORY_NAME"].ToString());
                                if (UlchangeCount<15)
                                    Subcatlist = Subcatlist + _stmpl_records1.ToString();
                                else if (UlchangeCount > 15 && UlchangeCount < 30)
                                    Subcatlist2 = Subcatlist2 + _stmpl_records1.ToString();
                                else if (UlchangeCount > 15 && UlchangeCount < 30)
                                    Subcatlist3 = Subcatlist3 + _stmpl_records1.ToString();

                                UlchangeCount = UlchangeCount + 1;
                            }
                            _stmpl_records.SetAttribute("TBT_SUBCATEGORY_LIST", Subcatlist);
                            _stmpl_records.SetAttribute("TBT_SUBCATEGORY_LIST2", Subcatlist2);
                            _stmpl_records.SetAttribute("TBT_SUBCATEGORY_LIST3", Subcatlist3);

                            _stmpl_records.SetAttribute("TBT_SUB_MENU", true);
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_SUB_MENU", false);
                        }
                    }
                    //_stmpl_records.SetAttribute("TBT_SUBCATEGORY_LIST", false);
                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;

                    // objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
                }

                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");




                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
            // return ProdimageRreplaceImages(sHTML);
        }
        public string ST_TopMenuMobile()
        {
            string sHTML = "";

            try
            {

                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_container1 = null;
                StringTemplate _stmpl_records1 = null;

                TBWDataList[] lstrecords = new TBWDataList[0];
                TBWDataList[] lstrecords2 = new TBWDataList[0];
                TBWDataList[] lstrecords3 = new TBWDataList[0];
                // TBWDataList[] lstrows = new TBWDataList[0];

                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                // TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                DataSet dsrecords = new DataSet();
                DataTable dtMainCat = new DataTable();
                DataTable dtSubCat = new DataTable();
                DataRow[] drs = null;



                DataTable ttbl = GetSupplierDetail();
                if (ttbl != null && ttbl.Rows.Count > 0)
                {
                    strsupplierName = ttbl.Rows[0]["CATEGORY_NAME"].ToString();
                    strsupplierDesc = ttbl.Rows[0]["SHORT_DESC"].ToString();
                    strsupplierId = ttbl.Rows[0]["CATEGORY_ID"].ToString();

                }
                // EasyAsk.GetMainMenuClickDetailJson(strsupplierId, "", true );
                DataSet Mainmnuds = (DataSet)HttpContext.Current.Session["MainMenuClick"];

                if (Mainmnuds == null || Mainmnuds.Tables.Count <= 0 || Mainmnuds.Tables["MainCategory"].Rows.Count <= 0)
                    return "";

                dtMainCat = Mainmnuds.Tables["MainCategory"];
                dtSubCat = Mainmnuds.Tables["SubCategory"];


                if (dtMainCat == null || dtMainCat.Rows.Count <= 0)
                    return "";


                //if (dsMainCat.Tables["Category"] == null)
                //    return "";
                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("cell", _SkinRootPath);


                lstrecords = new TBWDataList[dtMainCat.Rows.Count + 1];
                lstrecords2 = new TBWDataList[dtMainCat.Rows.Count + 1];
                lstrecords3 = new TBWDataList[dtMainCat.Rows.Count + 1];



                int ictrecords = 0;
                int UlchangeCount = 0;

                string Subcatlist = "";
                string Subcatlist2 = "";
                string Subcatlist3 = "";
                string _BCEAPath = GetEAPath();
                foreach (DataRow dr in dtMainCat.Rows)//For Records
                {
                    Subcatlist = "";
                    Subcatlist2 = "";
                    Subcatlist3 = "";
                    UlchangeCount = 0;
                    ictrows = ictrows + 1;
                    _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "Row");
                    _stmpl_records.SetAttribute("TBT_REWRITEURL", objHelperServices.Cons_NewURlASPX("", dr["EA_PATH"] + "////" + dr["CATEGORY_NAME"].ToString(), "mpl.aspx", true, "Category"));
                    _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());
                    _stmpl_records.SetAttribute("MENU_NO", ictrows);
                    //if (ictrows == 1)
                    //    _stmpl_records.SetAttribute("ISFIRST_MENU", "block");
                    //else
                    //    _stmpl_records.SetAttribute("ISFIRST_MENU", "none");



                    if (dtSubCat != null && dtSubCat.Rows.Count > 0)
                    {
                        DataRow[] subrows = dtSubCat.Select("PARENT_CATEGORY_ID='" + dr["CATEGORY_ID"].ToString() + "'");

                        if (subrows.Length > 0)
                        {
                            foreach (DataRow subdr in subrows)//For Records
                            {

                                _stmpl_records1 = _stg_records1.GetInstanceOf(_Package + "\\" + "cell");
                                //_stmpl_records1.SetAttribute("TBT_REWRITEURL", subdr["URL_RW_PATH"].ToString());
                                _stmpl_records1.SetAttribute("TBT_REWRITEURL", objHelperServices.Cons_NewURlASPX("", dr["EA_PATH"] + "////" + dr["CATEGORY_NAME"].ToString() + "////" + subdr["CATEGORY_NAME"].ToString(), "mpl.aspx", true, "Category"));

                                _stmpl_records1.SetAttribute("TBT_CATEGORY_NAME", subdr["CATEGORY_NAME"].ToString());
                                Subcatlist = Subcatlist + _stmpl_records1.ToString();
                               // if (UlchangeCount < 15)
                                //   
                                //else if (UlchangeCount > 15 && UlchangeCount < 30)
                                    Subcatlist2 = Subcatlist2 + _stmpl_records1.ToString();

                                //else if (UlchangeCount > 15 && UlchangeCount < 30)
                                 //   Subcatlist3 = Subcatlist3 + _stmpl_records1.ToString();

                                //UlchangeCount = UlchangeCount + 1;
                            }
                            _stmpl_records.SetAttribute("TBT_SUBCATEGORY_LIST", Subcatlist);
                            //_stmpl_records.SetAttribute("TBT_SUBCATEGORY_LIST2", Subcatlist2);
                            //_stmpl_records.SetAttribute("TBT_SUBCATEGORY_LIST3", Subcatlist3);

                            _stmpl_records.SetAttribute("TBT_SUB_MENU", true);
                        }
                        else
                        {
                            _stmpl_records.SetAttribute("TBT_SUB_MENU", false);
                        }
                    }
                    //_stmpl_records.SetAttribute("TBT_SUBCATEGORY_LIST", false);
                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;

                    // objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
                }

                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");




                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
            // return ProdimageRreplaceImages(sHTML);
        }
        public string ST_Top_Cart_item()
        {
            string sHTML = "";

            try
            {

                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null; 
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_container1 = null;
                StringTemplate _stmpl_records1 = null;

                TBWDataList[] lstrecords = new TBWDataList[0];
                // TBWDataList[] lstrows = new TBWDataList[0];

                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                // TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                DataSet dsrecords = new DataSet();
                DataSet dsCart = new DataSet();
                DataSet dsSubCat = new DataSet();
                DataRow[] drs = null;

                String sessionId_dum;
                // string sessionuserid_dum;
                sessionId_dum = HttpContext.Current.Session.SessionID;

                string userid = HttpContext.Current.Session["USER_ID"].ToString();

                if (userid == string.Empty)
                {
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
                }   
                

                string _Tbt_Order_Id = "";
                string _Tbt_Ship_URL = "";
                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                {
                    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
                }
                else
                {
                    _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(userid), OpenOrdStatusID).ToString();

                }
                string OrderStatus = objOrderServices.GetOrderStatus(objHelperServices.CI(_Tbt_Order_Id));
                if (objHelperServices.CI(_Tbt_Order_Id) > 0 && OrderStatus == OrderServices.OrderStatus.OPEN.ToString())    // || OrderStatus=="CAU_PENDING")
                {

                    dsCart = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_CART_ITEM " + _Tbt_Order_Id + ",'" + sessionId_dum + "','" + strsupplierId + "'" );
                }
                else
                {
                    dsCart = null;
                }
                GetSupplierDetail();

                // dsSubCat = EasyAsk.GetCategoryAndBrand("SubCategory");
                



               // if (dsCart == null || dsCart.Tables.Count <= 0 || dsCart.Tables[0].Rows.Count <= 0)
                //    return "";

                
                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("count", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("cell", _SkinRootPath);


               



                int ictrecords = 0;
                int UlchangeCount = 0;

                string Itemslist = "";
                string newurl = "";
                //string _BCEAPath = GetEAPath();
              

                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                {
                   // _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                    //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
               _Tbt_Ship_URL = "/morderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + _Tbt_Order_Id  ;
                
                }
                else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
                {
                    _Tbt_Ship_URL = "/morderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + _Tbt_Order_Id;
                    //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                }
                //else
                //{
                //    _Tbt_Ship_URL = "shipping.aspx";
                //}

                if (dsCart == null || dsCart.Tables.Count <= 0 || dsCart.Tables[0].Rows.Count <= 0)
                {
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");
                
                    _stmpl_container.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);


                    _stmpl_container.SetAttribute("TBWDataList", "");


                    _stmpl_records1  = _stg_records1.GetInstanceOf(_Package + "\\" + "cartCount");

                    _stmpl_records1.SetAttribute("CART_AMOUNT", "0.00" );
                    //_stmpl_records1.SetAttribute("CART_COUNT", "0.00");
                    _stmpl_records1.SetAttribute("CART_COUNT", "0");
                    _stmpl_records1.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);

                    sHTML = _stmpl_records1.ToString() + "~" + "" + "~" + "0" + "~"  +_Tbt_Order_Id + "~" + EncryptSP(_Tbt_Order_Id.ToString()) ;
                    sHTML = sHTML + "~" + ST_Top_Cart_itemMobile(dsCart, _Tbt_Order_Id);
                }
                else
                {
                    lstrecords = new TBWDataList[dsCart.Tables[0].Rows.Count + 1];
                    foreach (DataRow dr in dsCart.Tables[0].Rows)//For Records
                    {
                       
                        ictrows = ictrows + 1;
                        _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "cell");

                        string category = dr["Category_Path"].ToString();

                        category = strsupplierName;

                        DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, dr["product_id"].ToString(), strsupplierId, "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID", HelperDB.ReturnType.RTDataSet);
                         if (tmpds != null && tmpds.Tables.Count > 0)
                         {
                             newurl = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////" + tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString() + "=" + tmpds.Tables[0].Rows[0]["FAMILY_name"].ToString() + "////" + tmpds.Tables[0].Rows[0]["product_ID"].ToString() + "=" + dr["Code"].ToString(), "mpd.aspx", true, "");

                             _stmpl_records.SetAttribute("TBT_REWRITEURL", "/" + newurl  + "/mpd/");
                         }
                      
                        // _stmpl_records.SetAttribute("TBT_REWRITEURL", objHelperServices.Cons_NewURlASPX("", _BCEAPath + "////" + dr["CATEGORY_NAME"].ToString(), "mpl.aspx", true, "Category"));
                        _stmpl_records.SetAttribute("FAMILY_NAME", dr["Family_name"].ToString());
                        _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", dr["Family_name"].ToString().Replace('"', ' '));
                        string imgpath = GetImagePath(dr["TWEB Image1"].ToString());
                        _stmpl_records.SetAttribute("TWebImage1", imgpath);
                            
                        
                        _stmpl_records.SetAttribute("COST", dr["Cost"].ToString());
                        _stmpl_records.SetAttribute("CODE", dr["Code"].ToString());





                        Itemslist = Itemslist + _stmpl_records.ToString();

                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;

                        // objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
                    }
 
                    _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");


                 _stmpl_container.SetAttribute("CART_AMOUNT", dsCart.Tables[0].Rows[0]["ORDER_AMOUNT"].ToString());
                    //modified by:indu
                    //_stmpl_container.SetAttribute("CART_AMOUNT", dsCart.Tables[0].Rows[0]["cost"].ToString());
                    _stmpl_container.SetAttribute("CART_COUNT", dsCart.Tables[0].Rows[0]["ITEM_COUNT"].ToString());
                    _stmpl_container.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);


                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);


                    _stmpl_records1 = _stg_records1.GetInstanceOf(_Package + "\\" + "cartCount");

                    _stmpl_records1.SetAttribute("CART_AMOUNT", dsCart.Tables[0].Rows[0]["ORDER_AMOUNT"].ToString());
                    _stmpl_records1.SetAttribute("CART_COUNT", dsCart.Tables[0].Rows[0]["ITEM_COUNT"].ToString());

                    _stmpl_records1.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);

                    sHTML = _stmpl_records1.ToString() + "~" + _stmpl_container.ToString() + "~" + dsCart.Tables[0].Rows[0]["ITEM_COUNT"].ToString() +"~" + _Tbt_Order_Id + "~" + EncryptSP(_Tbt_Order_Id.ToString());
                    sHTML = sHTML + "~" + ST_Top_Cart_itemMobile(dsCart, _Tbt_Order_Id);
                   
                }
            }
            catch (Exception ex)
            { 
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
            // return ProdimageRreplaceImages(sHTML);
        }
        public string ST_Top_Cart_itemMobile(DataSet dsCart, string _Tbt_Order_Id)
        {
            string sHTML = "";

            try
            {

                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_container1 = null;
                StringTemplate _stmpl_records1 = null;

                TBWDataList[] lstrecords = new TBWDataList[0];
                // TBWDataList[] lstrows = new TBWDataList[0];

                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                // TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                DataSet dsrecords = new DataSet();
    
                DataSet dsSubCat = new DataSet();
                DataRow[] drs = null;

                String sessionId_dum;
                // string sessionuserid_dum;
                //sessionId_dum = HttpContext.Current.Session.SessionID;

                //string userid = HttpContext.Current.Session["USER_ID"].ToString();

                //if (userid == string.Empty)
                //{
                //    userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
                //}


                //string _Tbt_Order_Id = "";
                string _Tbt_Ship_URL = "";
                string _Tbt_chkout_URL = "";
                //int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                //if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                //{
                //    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
                //}
                //else
                //{
                //    _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(userid), OpenOrdStatusID).ToString();

                //}
                //string OrderStatus = objOrderServices.GetOrderStatus(objHelperServices.CI(_Tbt_Order_Id));
                //if (objHelperServices.CI(_Tbt_Order_Id) > 0 && OrderStatus == OrderServices.OrderStatus.OPEN.ToString())    // || OrderStatus=="CAU_PENDING")
                //{

                //    dsCart = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_CART_ITEM " + _Tbt_Order_Id + ",'" + sessionId_dum + "'");
                //}
                //else
                //{
                //    dsCart = null;
                //}
                //GetSupplierDetail();

                // dsSubCat = EasyAsk.GetCategoryAndBrand("SubCategory");




                // if (dsCart == null || dsCart.Tables.Count <= 0 || dsCart.Tables[0].Rows.Count <= 0)
                //    return "";


                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("count", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("cell", _SkinRootPath);






                int ictrecords = 0;
                int UlchangeCount = 0;

                string Itemslist = "";
                string newurl = "";
                //string _BCEAPath = GetEAPath();
                _Tbt_Ship_URL = "/morderDetails.aspx?bulkorder=1&amp;Pid=0&amp;ORDER_ID=" + _Tbt_Order_Id;
                _Tbt_chkout_URL = "/mcheckout.aspx?" + EncryptSP(_Tbt_Order_Id.ToString());

                //if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                //{
                //    // _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                //    //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                   

                //}
                //else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
                //{
                //    _Tbt_Ship_URL = "/morderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + _Tbt_Order_Id;
                //    //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                //}
                //else
                //{
                //    _Tbt_Ship_URL = "shipping.aspx";
                //}

                if (dsCart == null || dsCart.Tables.Count <= 0 || dsCart.Tables[0].Rows.Count <= 0)
                {
                    _stmpl_container = _stg_container.GetInstanceOf("CartItemsMobile" + "\\" + "Main");

                    _stmpl_container.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);

                    _stmpl_container.SetAttribute("CHECKOUT", _Tbt_chkout_URL);
                    _stmpl_container.SetAttribute("TBT_IS_CART", false);

                    
                    _stmpl_container.SetAttribute("TBWDataList", "");




                    sHTML = _stmpl_container.ToString();

                }
                else
                {
                    lstrecords = new TBWDataList[dsCart.Tables[0].Rows.Count + 1];
                    foreach (DataRow dr in dsCart.Tables[0].Rows)//For Records
                    {

                        ictrows = ictrows + 1;
                        _stmpl_records = _stg_records.GetInstanceOf("CartItemsMobile" + "\\" + "cell");

                        string category = dr["Category_Path"].ToString();

                        category = strsupplierName;
                           DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, dr["product_id"].ToString(), strsupplierId, "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID", HelperDB.ReturnType.RTDataSet);
                         if (tmpds != null && tmpds.Tables.Count > 0)
                         {
                             newurl = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////" + tmpds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString() + "=" + tmpds.Tables[0].Rows[0]["FAMILY_name"].ToString() + "////" + tmpds.Tables[0].Rows[0]["product_ID"].ToString() + "=" + dr["Code"].ToString(), "mpd.aspx", true, "");

                             _stmpl_records.SetAttribute("TBT_REWRITEURL", "/" + newurl  + "/mpd/");
                         }
                        //if (dr["Category_Path"].ToString() == "" || dr["Category_Path"].ToString().ToUpper().Contains(category.ToUpper()))
                        //{

                        //    newurl = category + "////" +
                        //             dr["FAMILY_ID"].ToString() +
                        //             "=" + dr["FAMILY_NAME"].ToString() +
                        //             "////" +
                        //              dr["PRODUCT_ID"].ToString() + "=" + dr["CODE"].ToString();

                        //    _stmpl_records.SetAttribute("TBT_REWRITEURL", "/" + objHelperServices.Cons_NewURlASPX("", newurl, "mpd.aspx", true, "") + "/mpd/");

                        //}
                        //else
                        //{
                        //    newurl = dr["Category_Path"].ToString() + "////" +
                        //                dr["FAMILY_ID"].ToString() +
                        //                "=" + dr["FAMILY_NAME"].ToString() +
                        //                "////" +
                        //                 dr["PRODUCT_ID"].ToString() + "=" + dr["CODE"].ToString();
                        //    _stmpl_records.SetAttribute("TBT_REWRITEURL", "/" + objHelperServices.Cons_NewURlASPX("", newurl, "pd.aspx", true, "") + "/pd/");
                        //}

                        // _stmpl_records.SetAttribute("TBT_REWRITEURL", objHelperServices.Cons_NewURlASPX("", _BCEAPath + "////" + dr["CATEGORY_NAME"].ToString(), "mpl.aspx", true, "Category"));
                        _stmpl_records.SetAttribute("FAMILY_NAME", dr["Family_name"].ToString());
                        _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", dr["Family_name"].ToString().Replace('"', ' '));
                        string imgpath = GetImagePath(dr["TWEB Image1"].ToString());
                        _stmpl_records.SetAttribute("TWebImage1", imgpath);


                        _stmpl_records.SetAttribute("COST", dr["Cost"].ToString());
                        _stmpl_records.SetAttribute("CODE", dr["Code"].ToString());





                        Itemslist = Itemslist + _stmpl_records.ToString();

                        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                        ictrecords++;

                        // objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
                    }

                    _stmpl_container = _stg_container.GetInstanceOf("CartItemsMobile" + "\\" + "Main");

                    _stmpl_container.SetAttribute("TBT_IS_CART", true);
                    _stmpl_container.SetAttribute("CART_AMOUNT", dsCart.Tables[0].Rows[0]["ORDER_AMOUNT"].ToString());
                    //modified by:indu
                    //_stmpl_container.SetAttribute("CART_AMOUNT", dsCart.Tables[0].Rows[0]["cost"].ToString());
                    _stmpl_container.SetAttribute("CART_COUNT", dsCart.Tables[0].Rows[0]["ITEM_COUNT"].ToString());
                    _stmpl_container.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);
                    _stmpl_container.SetAttribute("CHECKOUT", _Tbt_chkout_URL);

                    _stmpl_container.SetAttribute("TBWDataList", lstrecords);




                    sHTML = _stmpl_container.ToString();


                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
            // return ProdimageRreplaceImages(sHTML);
        }
        protected string EncryptSP(string ordid)
        {
            string enc = "";
            enc = objSecurity.StringEnCrypt(ordid, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            enc = objSecurity.StringEnCrypt(enc, EnDekey);
            return HttpUtility.UrlEncode(enc);
        }
        //public string ST_Top_Cart_item_microsite(string formname)
        //{
        //    string sHTML = "";

        //    try
        //    {

        //        StringTemplateGroup _stg_container = null;
        //        StringTemplateGroup _stg_records = null;
        //        StringTemplate _stmpl_container = null;
        //        StringTemplate _stmpl_records = null;
        //        StringTemplate _stmpl_container1 = null;
        //        StringTemplate _stmpl_records1 = null;

        //        TBWDataList[] lstrecords = new TBWDataList[0];
        //        // TBWDataList[] lstrows = new TBWDataList[0];

        //        StringTemplateGroup _stg_container1 = null;
        //        StringTemplateGroup _stg_records1 = null;
        //        // TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        //        // TBWDataList1[] lstrows1 = new TBWDataList1[0];

        //        DataSet dsrecords = new DataSet();
        //        DataSet dsCart = new DataSet();
        //        DataSet dsSubCat = new DataSet();
        //        DataRow[] drs = null;

        //        String sessionId_dum;
        //        // string sessionuserid_dum;
        //        sessionId_dum = HttpContext.Current.Session.SessionID;

        //        string userid = HttpContext.Current.Session["USER_ID"].ToString();

        //        if (userid == string.Empty)
        //        {
        //            userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
        //        }


        //        string _Tbt_Order_Id = "";
        //        string _Tbt_Ship_URL = "";
        //        int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
        //        if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
        //        {
        //            _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
        //        }
        //        else
        //        {
        //            _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(userid), OpenOrdStatusID).ToString();

        //        }


        //        // dsSubCat = EasyAsk.GetCategoryAndBrand("SubCategory");
        //        dsCart = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_CART_ITEM " + _Tbt_Order_Id + ",'" + sessionId_dum + "'");



        //        // if (dsCart == null || dsCart.Tables.Count <= 0 || dsCart.Tables[0].Rows.Count <= 0)
        //        //    return "";


        //        // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
        //        int ictrows = 0;






        //        _stg_records = new StringTemplateGroup("count", _SkinRootPath);
        //        _stg_container = new StringTemplateGroup("main", _SkinRootPath);
        //        _stg_records1 = new StringTemplateGroup("cell", _SkinRootPath);






        //        int ictrecords = 0;
        //        int UlchangeCount = 0;

        //        string Itemslist = "";
        //        string newurl = "";
        //        //string _BCEAPath = GetEAPath();


        //        if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
        //        {
        //            // _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
        //            //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
        //            _Tbt_Ship_URL = "/orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + _Tbt_Order_Id;

        //        }
        //        else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
        //        {
        //            _Tbt_Ship_URL = "/orderDetails.aspx?&bulkorder=1&Pid=0&ORDER_ID=" + _Tbt_Order_Id;
        //            //_Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
        //        }
        //        //else
        //        //{
        //        //    _Tbt_Ship_URL = "shipping.aspx";
        //        //}

        //        if (dsCart == null || dsCart.Tables.Count <= 0 || dsCart.Tables[0].Rows.Count <= 0)
        //        {
        //            _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");

        //            _stmpl_container.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);


        //            _stmpl_container.SetAttribute("TBWDataList", "");


        //            _stmpl_records1 = _stg_records1.GetInstanceOf(_Package + "\\" + "cartCount");

        //            _stmpl_records1.SetAttribute("CART_AMOUNT", "0");
        //            _stmpl_records1.SetAttribute("CART_COUNT", "0.00");

        //            _stmpl_records1.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);

        //            sHTML = _stmpl_records1.ToString() + "~" + _stmpl_container.ToString();

        //        }
        //        else
        //        {
        //            lstrecords = new TBWDataList[dsCart.Tables[0].Rows.Count + 1];
        //            foreach (DataRow dr in dsCart.Tables[0].Rows)//For Records
        //            {

        //                ictrows = ictrows + 1;
        //                _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "cell");

        //                newurl = dr["Category_Path"].ToString() + "////" +
        //                               dr["FAMILY_ID"].ToString() +
        //                               "=" + dr["FAMILY_NAME"].ToString() +
        //                               "////" +
        //                                dr["PRODUCT_ID"].ToString() + "=" + dr["CODE"].ToString();

        //                _stmpl_records.SetAttribute("TBT_REWRITEURL", "/" + objHelperServices.Cons_NewURlASPX("", newurl, formname, true, "") + "/" + formname);

        //                // _stmpl_records.SetAttribute("TBT_REWRITEURL", objHelperServices.Cons_NewURlASPX("", _BCEAPath + "////" + dr["CATEGORY_NAME"].ToString(), "mpl.aspx", true, "Category"));
        //                _stmpl_records.SetAttribute("FAMILY_NAME", dr["Family_name"].ToString());
        //                _stmpl_records.SetAttribute("TWebImage1", GetImagePath(dr["TWEB Image1"].ToString()));
        //                _stmpl_records.SetAttribute("COST", dr["Cost"].ToString());
        //                _stmpl_records.SetAttribute("CODE", dr["Code"].ToString());





        //                Itemslist = Itemslist + _stmpl_records.ToString();

        //                lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
        //                ictrecords++;

        //                // objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
        //            }

        //            _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");


        //            _stmpl_container.SetAttribute("CART_AMOUNT", dsCart.Tables[0].Rows[0]["ORDER_AMOUNT"].ToString());
        //            _stmpl_container.SetAttribute("CART_COUNT", dsCart.Tables[0].Rows[0]["ITEM_COUNT"].ToString());
        //            _stmpl_container.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);


        //            _stmpl_container.SetAttribute("TBWDataList", lstrecords);


        //            _stmpl_records1 = _stg_records1.GetInstanceOf(_Package + "\\" + "cartCount");

        //            _stmpl_records1.SetAttribute("CART_AMOUNT", dsCart.Tables[0].Rows[0]["ORDER_AMOUNT"].ToString());
        //            _stmpl_records1.SetAttribute("CART_COUNT", dsCart.Tables[0].Rows[0]["ITEM_COUNT"].ToString());

        //            _stmpl_records1.SetAttribute("VIEW_ORDER", _Tbt_Ship_URL);

        //            sHTML = _stmpl_records1.ToString() + "~" + _stmpl_container.ToString();


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        sHTML = "";
        //    }
        //    return sHTML;
        //    // return ProdimageRreplaceImages(sHTML);
        //}
        public string ST_Bootom_Cat_menu()
        {
            string sHTML = "";

            try
            {

                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_container1 = null;
                StringTemplate _stmpl_records1 = null;

                TBWDataList[] lstrecords = new TBWDataList[0];
                // TBWDataList[] lstrows = new TBWDataList[0];

                StringTemplateGroup _stg_container1 = null;
                StringTemplateGroup _stg_records1 = null;
                // TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                DataSet dsrecords = new DataSet();
                DataSet dsMainCat = new DataSet();
                DataSet dsSubCat = new DataSet();
                DataRow[] drs = null;


                dsMainCat = (DataSet)HttpContext.Current.Session["Category_Attributes"];
                // dsSubCat = EasyAsk.GetCategoryAndBrand("SubCategory");


                if (dsMainCat == null || dsMainCat.Tables.Count <= 0 || dsMainCat.Tables[0].Rows.Count <= 0)
                    return "";

                if (dsMainCat.Tables["Category"] == null)
                    return "";
                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("cell", _SkinRootPath);


                lstrecords = new TBWDataList[dsMainCat.Tables["Category"].Rows.Count + 1];



                int ictrecords = 0;
                int UlchangeCount = 0;

                string Subcatlist = "";
                //string _BCEAPath = GetEAPath();

                string suppliername = string.Empty ;
                if (strsupplierName == "")
                {
                    GetSupplierDetail();
                     
                }
                suppliername = strsupplierName;
                foreach (DataRow dr in dsMainCat.Tables["Category"].Rows)//For Records
                {
                    Subcatlist = "";
                    ictrows = ictrows + 1;
                    _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "Row");
                    _stmpl_records.SetAttribute("TBT_REWRITEURL", objHelperServices.Cons_NewURlASPX("", suppliername + "////" + dr["CATEGORY_NAME"].ToString(), "mpl.aspx", true, "Category"));
                    _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());
                    _stmpl_records.SetAttribute("MENU_NO", ictrows);



                    //if (dsSubCat != null && dsSubCat.Tables.Count > 0)
                    //{
                    //    DataRow[] subrows = dsSubCat.Tables[0].Select("TBT_PARENT_CATEGORY_ID='" + dr["CATEGORY_ID"].ToString() + "'");

                    //    if (subrows.Length > 0)
                    //    {
                    //        foreach (DataRow subdr in subrows)//For Records
                    //        {
                    //            _stmpl_records1 = _stg_records1.GetInstanceOf(_Package + "\\" + "cell");
                    //            _stmpl_records1.SetAttribute("TBT_REWRITEURL", subdr["URL_RW_PATH"].ToString());
                    //            _stmpl_records1.SetAttribute("TBT_CATEGORY_NAME", subdr["CATEGORY_NAME"].ToString());
                    //            Subcatlist = Subcatlist + _stmpl_records1.ToString();
                    //        }
                    //        _stmpl_records.SetAttribute("TBT_SUBCATEGORY_LIST", Subcatlist);

                    //        _stmpl_records.SetAttribute("TBT_SUB_MENU", true);
                    //    }
                    //    else
                    //    {
                    //        _stmpl_records.SetAttribute("TBT_SUB_MENU", false);
                    //    }
                    //}
                    // _stmpl_records.SetAttribute("TBT_SUBCATEGORY_LIST", false);
                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;

                    // objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
                }

                _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");




                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
            // return ProdimageRreplaceImages(sHTML);
        }

        
        public DataTable  GetSupplierDetail()
        {
            DataSet dsbc = null;
            DataTable tmptbl = null;
            DataTable rtntbl = null;
            
                        strsupplierName ="";
                        strsupplierDesc ="";
                        strsupplierId = ""; 
            try
            {
            
                if (HttpContext.Current.Session["BreadCrumbDS"] != null)
                {
                    dsbc = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];
                    if (dsbc.Tables[0].Rows[0]["itemType"].ToString().ToLower() == "category")
                    {
                        if (HttpContext.Current.Session["MainCategory"] != null)
                        {
                            tmptbl = ((DataSet)HttpContext.Current.Session["MainCategory"]).Tables[0];

                            rtntbl = tmptbl.Select("CATEGORY_Name='" + dsbc.Tables[0].Rows[0]["ItemValue"].ToString() + "'").CopyToDataTable();

                            strsupplierName = rtntbl.Rows[0]["CATEGORY_NAME"].ToString();
                            strsupplierDesc = rtntbl.Rows[0]["SHORT_DESC"].ToString();
                            strsupplierId = rtntbl.Rows[0]["CATEGORY_ID"].ToString();


                        }
                    }

                }
              
                return rtntbl;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private string GetEAPath()
        {
            DataSet dsbc = null;
            if (HttpContext.Current.Session["BreadCrumbDS"] != null)
            {
                dsbc = (DataSet)HttpContext.Current.Session["BreadCrumbDS"];

            }
            
            string _BCEAPath = "//// //// ////";
            for (int i = 0; i <= dsbc.Tables[0].Rows.Count - 1; i++)
            {
                string TBW_ITEM_NAME = string.Empty;
                DataRow row = dsbc.Tables[0].Rows[i];
                //if (row["ItemType"].ToString().ToLower() == "family")
                //    TBW_ITEM_NAME = row["FamilyName"].ToString();
                //else if (row["ItemType"].ToString().ToLower() == "product")
                //    TBW_ITEM_NAME = row["ProductCode"].ToString();
                //else
                //    TBW_ITEM_NAME = row["ItemValue"].ToString();

                string _istsb = string.Empty;

                if (row["ItemType"].ToString().ToLower() == "brand")
                {

                    _istsb = row["ItemValue"].ToString();
                }

                if (row["ItemType"].ToString().ToLower() == "family")
                {
                    TBW_ITEM_NAME = row["ItemValue"].ToString() + "=" + row["FamilyName"].ToString();
                }
                //else if (row["ItemType"].ToString().ToLower() == "category" && i == 0)
                //{
                //    TBW_ITEM_NAME = hfcid.Value + "=" + row["ItemValue"].ToString();

                //}
                else if (row["ItemType"].ToString().ToLower() == "product")
                {
                    if (row["ProductCode"].ToString() != "")
                    {
                        TBW_ITEM_NAME = row["ItemValue"].ToString() + "=" + row["ProductCode"].ToString();
                    }
                    else
                    {
                        TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                    }
                }
                //(row["ItemType"].ToString().ToLower() != "category") && 
                else if ((row["ItemType"].ToString().ToLower() == "category" && i != 0) && (row["ItemType"].ToString().ToLower() != "model") && (row["ItemType"].ToString().ToLower() != "brand") && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("bb.aspx") == true))
                {
                    TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                }

                //else if ((row["ItemType"].ToString().ToLower() == "brand") || (row["ItemType"].ToString().ToLower() == "model") && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx") == true)||(HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx") == true)))
                //{
                //    TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                //}
                else if ((row["ItemType"].ToString().ToLower() != "category") && (row["ItemType"].ToString().ToLower() != "model") && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx")) && ((row["ItemType"].ToString().ToLower() != "user search") && row["ItemType"].ToString().ToLower().Contains("usersearch") == false)))
                {
                    TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                }

                else if ((row["ItemType"].ToString().ToLower() != "brand") && (row["ItemType"].ToString().ToLower() != "category") && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ct.aspx") == true || HttpContext.Current.Request.Url.ToString().ToLower().Contains("microsite.aspx") == true))
                {
                    TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                }
                else if ((row["ItemType"].ToString().ToLower() == "model") && ((HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("pd.aspx")) || (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx"))))
                {
                    if (_istsb != "")
                    {
                        TBW_ITEM_NAME = row["Actualvalue"].ToString().ToLower().Replace("attribselect=", "").Replace("model = ", "model=").Replace("model = ", "model=").Replace("'", "").Replace(_istsb + ":", "");
                    }
                    else
                    {
                        TBW_ITEM_NAME = row["Actualvalue"].ToString().ToLower().Replace("attribselect=", "").Replace("MODEL = ", "MODEL=").Replace("model = ", "model=").Replace("'", "");
                    }
                }
                else if ((row["ItemType"].ToString().ToLower() != "category") && (row["ItemType"].ToString().ToLower() != "user search") && (row["ItemType"].ToString().ToLower().Contains("usersearch") == false) && (row["ItemType"].ToString().ToLower() != "model") && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("ps.aspx") || HttpContext.Current.Request.Url.ToString().ToLower().Contains("pl.aspx")))
                {
                    TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();
                }
                else if ((row["ItemType"].ToString().ToLower() == "user search") && (row["url"].ToString().ToLower().Contains("ps.aspx")) && (HttpContext.Current.Request.Url.ToString().ToLower().Contains("fl.aspx")))
                {
                    TBW_ITEM_NAME = "UserSearch" + "=" + row["ItemValue"].ToString();
                }
                else if (((row["ItemType"].ToString().ToLower() != "brand")) && ((row["ItemType"].ToString().ToLower() != "model")) && (row["Url"].ToString().Contains("bb.aspx") == true))
                {

                    TBW_ITEM_NAME = row["ItemType"].ToString() + "=" + row["ItemValue"].ToString();

                }
                else
                {
                    TBW_ITEM_NAME = row["ItemValue"].ToString();
                }
                TBW_ITEM_NAME = HttpUtility.UrlEncode(TBW_ITEM_NAME);
                //.Replace("+", "||").Replace("&", "^^").Replace(":", "~`");;

                if (_BCEAPath == "//// //// ////")
                {
                    _BCEAPath = _BCEAPath + TBW_ITEM_NAME;
                }
                else
                {

                    _BCEAPath = _BCEAPath + "////" + TBW_ITEM_NAME;


                }
            }
           // HttpContext.Current.Session["breadcrumEAPATH"] = _BCEAPath;
            return _BCEAPath;
        }

        public string ST_Header_Load()
        {
            string sHTML = "";

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;

                TBWDataList[] lstrecords = new TBWDataList[0];

                DataSet dsrecords = new DataSet();

                DataRow[] drs = null;

              


                //   _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                //lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];



                //int ictrecords = 0;

                //foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                //{
                //    if (dr["Category_id"].ToString().ToUpper() != WesNewsCategoryId && dr["CATEGORY_NAME"].ToString().ToUpper() != "NOKIA")
                //    {
                //        if (_Package == "TOP")
                //            _stmpl_records = _stg_records.GetInstanceOf("Top" + "\\" + "cell");
                //        else
                //            _stmpl_records = _stg_records.GetInstanceOf("TopLog" + "\\" + "cell");

                //        _stmpl_records.SetAttribute("TBT_REWRITEURL", dr["URL_RW_PATH"].ToString());
                //        _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME_TOP"].ToString());

                //        lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                //        ictrecords++;
                //    }
                //    objHelperService.Insert_CIDTopDT(dr["Category_id"].ToString(), dr["CATEGORY_NAME"].ToString());
                //}

                if (_Package == "TOP")
                    _stmpl_container = _stg_container.GetInstanceOf("Header" + "\\" + "Main");
                else
                    _stmpl_container = _stg_container.GetInstanceOf("HeaderLog" + "\\" + "Main");

                _stmpl_container.SetAttribute("TBW_CART_ITEM", cartitem);



                //if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].Equals("") && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                //{
                //    _stmpl_container.SetAttribute("TBT_LOGIN_NAME", GetLoginName());
                //}

                //if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                //{
                //    if (HttpContext.Current.Session["USER_ROLE"] != null && Convert.ToInt32(HttpContext.Current.Session["USER_ROLE"].ToString()) == 4)
                //    {
                //        string ReMailLink = "<a Href=ConfirmMessage.aspx?Result=REMAILACTIVATION class=\"linkemailre\">Re-Email Activation Link Now</a>";

                //        _stmpl_container.SetAttribute("TBT_LOGIN_NAME", " Your Account Has Not Been Activated! " + ReMailLink);
                //    }
                //    else
                //        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", "");
                //}
                //else
                //{
                //    if (HttpContext.Current.Session["USER_ID"] != null && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                //    {
                //        _stmpl_container.SetAttribute("TBT_COMPANY_NAME", GetCompanyName() + ",");
                //    }
                //}

                if (HttpContext.Current.Session["USER_ID"] != null && HttpContext.Current.Session["USER_ID"] != "" && !HttpContext.Current.Session["USER_ID"].ToString().Equals(ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                    HttpContext.Current.Session["LOGIN_NAME"] = GetLoginName();


                //}


                string _Tbt_Order_Id = "";
                string _Tbt_Ship_URL = "";
                int OpenOrdStatusID = (int)OrderServices.OrderStatus.OPEN;
                if (HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0)
                {
                    _Tbt_Order_Id = HttpContext.Current.Session["ORDER_ID"].ToString();
                }
                else
                {
                    _Tbt_Order_Id = objOrderServices.GetOrderID(objHelperServices.CI(HttpContext.Current.Session["USER_ID"]), OpenOrdStatusID).ToString();
                    //    HttpContext.Current.Session["ORDER_ID"] = _Tbt_Order_Id;
                }

                if ((HttpContext.Current.Session["ORDER_ID"] != null && Convert.ToInt32(HttpContext.Current.Session["ORDER_ID"]) > 0) || (HttpContext.Current.Request.QueryString["ViewOrder"] != null && HttpContext.Current.Request.QueryString["ViewOrder"].Equals("View")) || (HttpContext.Current.Request.QueryString["ApproveOrder"] != null && HttpContext.Current.Request.QueryString["ApproveOrder"].Equals("Approve")))
                {
                    _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                }
                else if (_Tbt_Order_Id != "" && Convert.ToInt32(_Tbt_Order_Id) > 0)
                {
                    _Tbt_Ship_URL = "/shipping.aspx?OrderID=" + _Tbt_Order_Id + "&ApproveOrder=Approve";
                }
                else
                {
                    _Tbt_Ship_URL = "shipping.aspx";
                }
                _stmpl_container.SetAttribute("TBT_ORDER_ID", _Tbt_Order_Id);
                _stmpl_container.SetAttribute("TBT_SHIP_URL", _Tbt_Ship_URL);

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return sHTML;
            // return ProdimageRreplaceImages(sHTML);
        }
 
         public string ST_NewProduct_Load()
        {
            string sHTML = "";

            try
            {
                StringTemplateGroup _stg_container = null;
                StringTemplateGroup _stg_records = null;
                StringTemplate _stmpl_container = null;
                StringTemplate _stmpl_records = null;
                StringTemplate _stmpl_recordstemp = new StringTemplate();
                //  StringTemplate _stmpl_records1 = null;
                // StringTemplate _stmpl_recordsrows = null;
                TBWDataList[] lstrecords = new TBWDataList[0];
                // TBWDataList[] lstrows = new TBWDataList[0];

                // StringTemplateGroup _stg_container1 = null;
                // StringTemplateGroup _stg_records1 = null;
                //   TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                //   DataSet dscat = new DataSet();
                DataSet dsrecords = new DataSet();
                // DataTable dt = null;
                DataRow[] drs = null;

                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                
                if (userid == string.Empty)
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
                GetSupplierDetail();
                
               
                DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WAG 6,0," + userid + ",'" + strsupplierId + "'");
               // objErrorHandler.CreateLog("exec STP_TBWC_PICK_NEW_PRODUCT_WAG 6,0," + userid + ",'" + strsupplierId + "'");
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    //tmpds.Tables[0].Columns.Add("URL_RW_PATH", typeof(string));
                   // foreach (DataRow dr in tmpds.Tables[0].Rows)
                    //{
                        //DataSet tmpds1 = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, dr["product_id"].ToString(), strsupplierId, "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID", HelperDB.ReturnType.RTDataSet);
                        //if (tmpds1 != null && tmpds.Tables.Count > 0)
                        //{
                        //    dr["URL_RW_PATH"] = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + tmpds1.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////" + tmpds1.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString() + "=" + tmpds1.Tables[0].Rows[0]["FAMILY_name"].ToString() + "////" + tmpds1.Tables[0].Rows[0]["product_ID"].ToString() + "=" + dr["Code"].ToString(), "mpd.aspx", true, "");

                        //    //_stmpl_records.SetAttribute("TBT_REWRITEURL", "/" + newurl + "/mpd/");
                        //  //  dr["URL_RW_PATH"] = "/" + newurl + "/mpd/";
                        //}

                        //dr["URL_RW_PATH"] = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"].ToString() + "////" + dr["FAMILY_ID"].ToString() + "=" + dr["FAMILY_name"].ToString() + "////" + dr["product_ID"].ToString() + "=" + dr["Code"].ToString(), "mpd.aspx", true, "");

                   // }
                      
                }
                else
                    return "";

                // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                int ictrows = 0;






                _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                lstrecords = new TBWDataList[tmpds.Tables[0].Rows.Count + 1];

                bool isenable = IsEcomenabled();

                int ictrecords = 0;

                foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
                {
                    _stmpl_records = _stg_records.GetInstanceOf("NewProduct" + "\\" + "cellnew");


                    DataSet tmpds1 = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, dr["product_id"].ToString(), strsupplierId, "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID", HelperDB.ReturnType.RTDataSet);
                    string URL_RW_PATH = string.Empty;
                    
                    if (tmpds1 != null && tmpds.Tables.Count > 0)
                    {
                      URL_RW_PATH = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + tmpds1.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////" + tmpds1.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString() + "=" + tmpds1.Tables[0].Rows[0]["FAMILY_name"].ToString() + "////" + tmpds1.Tables[0].Rows[0]["product_ID"].ToString() + "=" + dr["Code"].ToString(), "mpd.aspx", true, "");

                        //_stmpl_records.SetAttribute("TBT_REWRITEURL", "/" + newurl + "/mpd/");
                        //  dr["URL_RW_PATH"] = "/" + newurl + "/mpd/";
                    }
                    _stmpl_records.SetAttribute("TBT_REWRITEURL", URL_RW_PATH);
                    string imgpath = GetImagePath(dr["TWEB Image1"].ToString());

                    if (imgpath.ToLower().Contains("noimages"))
                    {
                        _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", imgpath);
                        
                    }
                    else
                        _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", objHelperService.SetImageFolderPath(imgpath, "_th", "_Images_200"));
                        

                   // _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString()));
                    _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"].ToString());
                    _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", dr["FAMILY_NAME"].ToString().Replace('"',' ') );
                    //_stmpl_records.SetAttribute("TBT_DESCRIPTIONS", dr["DESCRIPTION"].ToString());
                    string mstrmstr = "";
                    mstrmstr = dr["DESCRIPTION"].ToString();
                    if (mstrmstr.Length > 120)
                    {
                        mstrmstr = mstrmstr.Substring(0, 120).ToString();
                        mstrmstr = mstrmstr.Substring(0, mstrmstr.LastIndexOf(" "));
                    }
                    _stmpl_records.SetAttribute("TBT_DESCRIPTIONS", mstrmstr);
                    _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"].ToString());
                    _stmpl_records.SetAttribute("TBT_YOURCOST", dr["COSt"].ToString());
                    _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);
                    _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2",true);
                    _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"].ToString());
                    _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"].ToString());
                    _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"].ToString());
                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"].ToString());
                    

                    lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                    ictrecords++;
                }

               
                    _stmpl_container = _stg_container.GetInstanceOf("NewProduct" + "\\" + "Mainnew");
               
                

                _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                sHTML = _stmpl_container.ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                sHTML = "";
            }
            return ProdimageRreplaceImages(sHTML);
        }
         public string ST_aboutus_Load()
         {
             string sHTML = "";

             try
             {
                 StringTemplateGroup _stg_container = null;
                 StringTemplateGroup _stg_records = null;
                 StringTemplate _stmpl_container = null;
                 StringTemplate _stmpl_records = null;
                 //  TBWDataList[] lstrecords = new TBWDataList[0];
                 //  DataSet dsrecords = new DataSet();




                 _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                 _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                 _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "cell");
                 _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");



                 // _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                 sHTML = _stmpl_container.ToString();
             }
             catch (Exception ex)
             {
                 objErrorHandler.ErrorMsg = ex;
                 objErrorHandler.CreateLog();
                 sHTML = "";
             }
             return ProdimageRreplaceImages(sHTML);
         }
         public string ST_NewProductLogNav_Load()
         {
             string sHTML = "";

             try
             {
                 StringTemplateGroup _stg_container = null;
                 StringTemplateGroup _stg_records = null;
                 StringTemplate _stmpl_container = null;
                 StringTemplate _stmpl_records = null;
                 StringTemplate _stmpl_recordstemp = new StringTemplate();
                 //  StringTemplate _stmpl_records1 = null;
                 // StringTemplate _stmpl_recordsrows = null;
                 TBWDataList[] lstrecords = new TBWDataList[0];
                 // TBWDataList[] lstrows = new TBWDataList[0];

                 // StringTemplateGroup _stg_container1 = null;
                 // StringTemplateGroup _stg_records1 = null;
                 //   TBWDataList1[] lstrecords1 = new TBWDataList1[0];
                 // TBWDataList1[] lstrows1 = new TBWDataList1[0];

                 //   DataSet dscat = new DataSet();
                 DataSet dsrecords = new DataSet();
                 // DataTable dt = null;
                 DataRow[] drs = null;

                 string userid = HttpContext.Current.Session["USER_ID"].ToString();

                 if (userid == string.Empty)
                     userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
                 GetSupplierDetail();

                 DataSet tmpds = objHelperDB.GetDataSetDB("exec STP_TBWC_PICK_NEW_PRODUCT_WAG 3,0," + userid + ",'" + strsupplierId +"'");

                 if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                 {
                     tmpds.Tables[0].Columns.Add("URL_RW_PATH", typeof(string));
                     tmpds.Tables[0].Columns.Add("URL_RW_PATH_FAMILY", typeof(string));
                     foreach (DataRow dr in tmpds.Tables[0].Rows)
                     {

                         DataSet tmpds1 = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, dr["product_id"].ToString(), strsupplierId, "GET_PARENT_CATEGORY_ID_PATH_INPUT_PID", HelperDB.ReturnType.RTDataSet);
                         if (tmpds1 != null && tmpds.Tables.Count > 0)
                         {
                             string newurl = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + tmpds1.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////" + tmpds1.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString() + "=" + tmpds1.Tables[0].Rows[0]["FAMILY_name"].ToString() + "////" + tmpds1.Tables[0].Rows[0]["product_ID"].ToString() + "=" + dr["Code"].ToString(), "mpd.aspx", true, "");

                          //   _stmpl_records.SetAttribute("TBT_REWRITEURL", "/" + newurl + "/mpd/");
                             dr["URL_RW_PATH"] =  newurl ;
                         }

                         //dr["URL_RW_PATH"] = objHelperServices.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"].ToString() + "////" + dr["FAMILY_ID"].ToString() + "=" + dr["FAMILY_name"].ToString() + "////" + dr["product_ID"].ToString() + "=" + dr["Code"].ToString(), "mpd.aspx", true, "");                          
                         //dr["URL_RW_PATH_FAMILY"] = objHelperService.Cons_NewURlASPX("", "AllProducts////WESAUSTRALASIA////" + "////" + dr["CATEGORY_PATH"].ToString() + "////" + dr["FAMILY_ID"].ToString() + "=" + dr["FAMILY_name"].ToString(), "fl.aspx", true, "", false, true);

                     }

                 }
                 else
                     return "";

                 
                 // stemplatepath = HttpContext.Current.Server.MapPath(objHelperServices.GetOptionValues("SHOPPTING TEMPLATE PATH").ToString());
                 int ictrows = 0;






                 _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                 _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                 lstrecords = new TBWDataList[tmpds.Tables[0].Rows.Count + 1];

                 bool isenable = IsEcomenabled();

                 int ictrecords = 0;

                 foreach (DataRow dr in tmpds.Tables[0].Rows)//For Records
                 {
                     _stmpl_records = _stg_records.GetInstanceOf(_Package  + "\\" + "cellnew");

                    _stmpl_records.SetAttribute("TBT_REWRITEURL", dr["URL_RW_PATH"].ToString());
                    _stmpl_records.SetAttribute("TBT_REWRITEURL_FAMILY", dr["URL_RW_PATH_FAMILY"].ToString());

                    string imgpath = GetImagePath(dr["TWEB Image1"].ToString());

                    if (imgpath.ToLower().Contains("noimages"))
                    {
                        _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", imgpath);

                    }
                    else
                        _stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", objHelperService.SetImageFolderPath(imgpath, "_th", "_Images_200"));
                        

                    //_stmpl_records.SetAttribute("TBT_TWEB_IMAGE1", GetImagePath(dr["TWEB Image1"].ToString()));

                     _stmpl_records.SetAttribute("TBT_FAMILY_NAME", dr["FAMILY_NAME"].ToString());
                     _stmpl_records.SetAttribute("TBT_FAMILY_TITLE", dr["FAMILY_NAME"].ToString().Replace('"', ' '));
                     //_stmpl_records.SetAttribute("TBT_DESCRIPTION1", dr["DESCRIPTION"].ToString());
                     _stmpl_records.SetAttribute("TBT_CODE", dr["CODE"].ToString());
                     _stmpl_records.SetAttribute("TBT_YOURCOST", dr["COSt"].ToString());
                     _stmpl_records.SetAttribute("TBT_ECOMENABLED", isenable);
                    _stmpl_records.SetAttribute("TBT_STOCK_STATUS_2", true);
                     _stmpl_records.SetAttribute("TBT_PRODUCT_ID", dr["PRODUCT_ID"].ToString());
                     _stmpl_records.SetAttribute("TBT_QTY_AVAIL", dr["QTY_AVAIL"].ToString());
                     _stmpl_records.SetAttribute("TBT_FAMILY_ID", dr["FAMILY_ID"].ToString());
                    _stmpl_records.SetAttribute("TBT_MIN_ORD_QTY", dr["MIN_ORD_QTY"].ToString());


                     lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                     ictrecords++;
                 }


                 _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Mainnew");



                 _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                 sHTML = _stmpl_container.ToString();
             }
             catch (Exception ex)
             {
                 objErrorHandler.ErrorMsg = ex;
                 objErrorHandler.CreateLog();
                 sHTML = "";
             }
             return ProdimageRreplaceImages(sHTML);
         }

         public string ST_mcontactus_Load()
         {
             string sHTML = "";

             try
             {
                 StringTemplateGroup _stg_container = null;
                 StringTemplateGroup _stg_records = null;
                 StringTemplate _stmpl_container = null;
                 StringTemplate _stmpl_records = null;
                 //  TBWDataList[] lstrecords = new TBWDataList[0];
                 //  DataSet dsrecords = new DataSet();




                 _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                 _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                 _stmpl_records = _stg_records.GetInstanceOf(_Package + "\\" + "cell");
                 _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");

                 if (HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"] != null)
                 {
                     _stmpl_container.SetAttribute("CAPTCHA_IMAGE", HttpContext.Current.Session["AQ_FL_CAPTCH_IMAGE"].ToString());
                     _stmpl_container.SetAttribute("CC_CODE", HttpContext.Current.Session["AQ_FL_CAPTCH_VALUE"].ToString());
                 }

                 // _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                 sHTML = _stmpl_container.ToString();
             }
             catch (Exception ex)
             {
                 objErrorHandler.ErrorMsg = ex;
                 objErrorHandler.CreateLog();
                 sHTML = "";
             }
             return ProdimageRreplaceImages(sHTML);
         }

       
         public string ST_Bottom_Load()
         {
             string sHTML = "";

             try
             {
                 DataSet dsbotmsname = new DataSet();
                 StringTemplateGroup _stg_container = null;
                 StringTemplateGroup _stg_records = null;
                 StringTemplate _stmpl_container = null;
                 StringTemplate _stmpl_records = null;
                
                 TBWDataList[] lstrecords = new TBWDataList[0];
                
                 DataSet dsrecords = new DataSet();
                 // DataTable dt = null;
                 DataRow[] drs = null;

                  dsrecords = EasyAsk.GetCategoryAndBrand("MainCategory");
               
                 int ictrows = 0;






                 _stg_records = new StringTemplateGroup("row", _SkinRootPath);
                 _stg_container = new StringTemplateGroup("main", _SkinRootPath);


                 lstrecords = new TBWDataList[dsrecords.Tables[0].Rows.Count + 1];



                 int ictrecords = 0;

                 foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                 {
                     
                     _stmpl_records = _stg_records.GetInstanceOf("Bottom" + "\\" + "cell");                     
                     _stmpl_records.SetAttribute("TBT_REWRITEURL", dr["URL_RW_PATH"].ToString());
                     _stmpl_records.SetAttribute("TBT_CATEGORY_NAME", dr["CATEGORY_NAME"].ToString());

                     lstrecords[ictrecords] = new TBWDataList(_stmpl_records.ToString());
                     ictrecords++;
                 }

                 _stmpl_container = _stg_container.GetInstanceOf("Bottom" + "\\" + "Main");

                 //if (HttpContext.Current.Session["CUSTOMER_TYPE"] != null && HttpContext.Current.Session["CUSTOMER_TYPE"].ToString().Equals("Retailer"))
                 //    _stmpl_container.SetAttribute("TBT_RET_PRO", false);
                 //else
                 //    _stmpl_container.SetAttribute("TBT_RET_PRO", true);

                 //if (!HttpContext.Current.Session["USER_ID"].Equals(""))
                 //{
                 //    if (HttpContext.Current.Session["USER_ID"].ToString().Equals(System.Configuration.ConfigurationManager.AppSettings["DUM_USER_ID"].ToString()))
                 //        _stmpl_container.SetAttribute("TBT_DUMUSER_CHECK", false);
                 //    else
                 //        _stmpl_container.SetAttribute("TBT_DUMUSER_CHECK", true);
                 //}

                

                 _stmpl_container.SetAttribute("TBWDataList", lstrecords);
                 sHTML = _stmpl_container.ToString();
             }
             catch (Exception ex)
             {
                 objErrorHandler.ErrorMsg = ex;
                 objErrorHandler.CreateLog();
                 sHTML = "";
             }
             return ProdimageRreplaceImages(sHTML);
         }

         public string ST_SubFamily_Load(DataSet tempDs)
         {
             string sHTML = "";

             try
             {
                 StringTemplateGroup _stg_container = null;
                 StringTemplateGroup _stg_records = null;
                 StringTemplate _stmpl_container = null;
                 StringTemplate _stmpl_records = null;
                
                 TBWDataList[] lstrecords = new TBWDataList[0];
                
                 DataSet dsrecords = new DataSet();
                
                 DataRow[] drs = null;




                 //DataSet tempDs = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                 if (tempDs != null && tempDs.Tables["FamilyPro"] != null)
                 {
                      drs = tempDs.Tables["FamilyPro"].Select("FAMILY_ID='" + paraValue + "'");
                     //if (dr.Length > 0)
                     //{
                     //    dsrecords = new DataSet();
                     //    dsrecords.Tables.Add(dr.CopyToDataTable().Copy());
                     //}

                 }
                 if (drs == null || drs.Length<=0 )
                     return "";

                 int ictrows = 0;





                 string attname="";

                 _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                 _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "Main");

                 _stmpl_container.SetAttribute("TBT_FAMILY_NAME", drs[0]["FAMILY_NAME"].ToString());
                 _stmpl_container.SetAttribute("TBT_FAMILY_TITLE", drs[0]["FAMILY_NAME"].ToString().Replace('"',' ') );
                 _stmpl_container.SetAttribute("TBT_SHORT_DESCRIPTION", drs[0]["FAMILY_SHORT_DESC"].ToString());
                 _stmpl_container.SetAttribute("TBT_DESCRIPTIONS", drs[0]["FAMILY_DESC"].ToString());

                 FileInfo Fil = new FileInfo(strProdImages + drs[0]["FAMILY_TH_IMAGE"].ToString().Replace("\\", "/"));
                 if (Fil.Exists)
                 {
                     _stmpl_container.SetAttribute("TBT_TFWEB_IMAGE1", drs[0]["FAMILY_TH_IMAGE"].ToString().Replace("\\", "/"));
                     _stmpl_container.SetAttribute("TBT_FWEB_IMAGE1",  "javascript:Zoom('/prodimages" + objHelperService.SetImageFolderPath(drs[0]["FAMILY_TH_IMAGE"].ToString().Replace("\\", "/"), "_th", "_Images_200")+"')" );
                    
                 }
                 else
                 {
                     _stmpl_container.SetAttribute("TBT_TFWEB_IMAGE1", "/images/noimage.gif");
                     _stmpl_container.SetAttribute("TBT_FWEB_IMAGE1", "");

                 }


                 //foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                 //{

                 //    attname = "TBT_" + dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();
                     
                 //     if (dr["ATTRIBUTE_TYPE"].ToString().Equals("3") || dr["ATTRIBUTE_TYPE"].ToString().Equals("9"))
                 //     {
                 //         FileInfo Fil = new FileInfo(strProdImages + dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                          
                 //         if (Fil.Exists)
                 //         {
                 //           _stmpl_container.SetAttribute(attname , dr["STRING_VALUE"].ToString().Replace("\\", "/"));
                 //         }
                 //         else
                 //             _stmpl_container.SetAttribute(attname, "/images/noimage.gif");

                 //     }
                 //     else
                 //       _stmpl_container.SetAttribute(attname, dr["STRING_VALUE"].ToString());
                     

                   
                 //}

                 sHTML = _stmpl_container.ToString();
             }
             catch (Exception ex)
             {
                 objErrorHandler.ErrorMsg = ex;
                 objErrorHandler.CreateLog();
                 sHTML = "";
             }
             return ProdimageRreplaceImages(sHTML);
         }

         public string ST_Family_Load(DataSet dsrecords)
         {
             string sHTML = "";

             try
             {
                 StringTemplateGroup _stg_container = null;
                 StringTemplateGroup _stg_records = null;
                 StringTemplate _stmpl_container = null;
                 StringTemplate _stmpl_records = null;

                 TBWDataList[] lstrecords = new TBWDataList[0];

                //DataSet dsrecords = new DataSet();

                 DataRow[] drs = null;




                 // dsrecords = (DataSet)HttpContext.Current.Session["FamilyProduct"];
                
                 if (dsrecords == null || dsrecords.Tables.Count <= 0 || dsrecords.Tables[0].Rows.Count <= 0)
                     return "";

                 int ictrows = 0;





                 string attname = "";

                 _stg_container = new StringTemplateGroup("main", _SkinRootPath);

                 if (dsrecords.Tables[0].Rows[0]["STATUS"].ToString().ToUpper() == "TRUE")
                     _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "main" + "1");
                 else if (dsrecords.Tables[0].Rows[0]["STATUS"].ToString().ToUpper() == "FALSE")
                     _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "main");
                 else
                     _stmpl_container = _stg_container.GetInstanceOf(_Package + "\\" + "main" + "2");


                 _stmpl_container.SetAttribute("TBT_FAMILY_NAME", dsrecords.Tables[0].Rows[0]["FAMILY_NAME"].ToString());
                 _stmpl_container.SetAttribute("TBT_FAMILY_TITLE", dsrecords.Tables[0].Rows[0]["FAMILY_NAME"].ToString().Replace('"',' ') );
                 HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"] = "";
                 HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"] = dsrecords.Tables[0].Rows[0]["FAMILY_NAME"].ToString();
                 _stmpl_container.SetAttribute("TBT_PROD_COUNT", dsrecords.Tables[0].Rows[0]["PROD_COUNT"].ToString());
                 _stmpl_container.SetAttribute("TBT_FAMILY_PROD_COUNT", dsrecords.Tables[0].Rows[0]["FAMILY_PROD_COUNT"].ToString());
                 string desc1 = "";
                 string descall = "";
                 string descallstring = "";
                 foreach (DataRow dr in dsrecords.Tables[0].Rows)//For Records
                 {

                     attname =  dr["ATTRIBUTE_NAME"].ToString().Replace(" ", "_").Replace(".", "_").Replace("#", "_").Replace("Ø", "_").Replace("*", "_").Replace("Ω", "_").Replace("’", "_").Replace(")", "_").Replace("(", "_").Replace("½", "_").Replace("&", "_").Replace("/", "_").Replace("-", "_").Replace("+", "_").ToUpper();


                     desc1="";
                     if (attname.Equals("DESCRIPTIONS") || attname.Equals("FEATURES") || attname.Equals("SPECIFICATION") || attname.Equals("SPECIFICATIONS") || attname.Equals("APPLICATIONS") || attname.Equals("NOTES"))
                     {
                         desc1 = dr["STRING_VALUE"].ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;").Replace("<ars>g</ars>", "<font face=\"Wingdings 3\">g</font>").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>").Replace("<sym>r</sym>", "<font face=\"WP MathA\">r</font>").Replace("<sym>s</sym>", "<font face=\"WP MathA\">s</font>");
                         desc1 = desc1.ToString().Replace("\n", "<br/>").Replace("\r", "&nbsp;");
                     }
                     else if (attname.Contains("SHORT DESCRIPTION"))
                     {
                         _stmpl_container.SetAttribute("TBT_"+attname, dr["STRING_VALUE"].ToString());
                     }
                     
                     if (!desc1.Equals(""))
                         descall = descall + desc1 + "<br/><br/>";
                    // _stmpl_container.SetAttribute(attname, dr["STRING_VALUE"].ToString());



                 }

                 if (!desc1.Equals(""))
                 {
                     descall = descall.Trim();
                     descall = descall.Substring(0, descall.Length - 5);
                 }
                 if (descall.Length > 1080)
                 {                    
                     descallstring = descall.Substring(0, 1080).ToString();
                     _stmpl_container.SetAttribute("TBT_MORE", descallstring);
                     _stmpl_container.SetAttribute("TBT_MENU_ID", "2");
                     descall = descall.Substring(0, descall.Length).ToString();         
                     _stmpl_container.SetAttribute("TBT_DESCALL", descall);

                 }
                 else
                 {
                     _stmpl_container.SetAttribute("TBT_DESCALL", descall);
                     _stmpl_container.SetAttribute("TBT_MENU_ID", "2");
                    
                     _stmpl_container.SetAttribute("TBT_MORE", descall);
                 }
                 if (descall.Length > 1080)
                     _stmpl_container.SetAttribute("TBT_MORE_SHOW", true);
                 else
                     _stmpl_container.SetAttribute("TBT_MORE_SHOW", false);



                 if (HttpContext.Current.Request.QueryString["fid"] != null)
                 {
                     _fid = HttpContext.Current.Request.QueryString["fid"].ToString();

                     DataSet tmpds = (DataSet)objHelperDB.GetGenericDataDB(WesCatalogId, _fid, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                     if (tmpds != null && tmpds.Tables.Count > 0)
                     {
                         _stmpl_container.SetAttribute("TBT_FAMILY_ID", _fid);
                         
                         string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
                         _stmpl_container.SetAttribute("TBT_EA_PATH", HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath)));
                         _stmpl_container.SetAttribute("TBT_ORGEA_PATH", eapath);
                         string bceapath = HttpContext.Current.Session["breadcrumEAPATH"].ToString();
                         objHelperService.Cons_NewURl(_stmpl_container, _stmpl_container.GetAttribute("TBT_ORGEA_PATH").ToString() + "////" + _fid + "=" + "Family", "fl.aspx", true, "", false, false);
                         

                     }

                 }


                 GetFamilyMultipleImages(Convert.ToInt32(paraFID), _stmpl_container, dsrecords);

                 sHTML = _stmpl_container.ToString();
             }
             catch (Exception ex)
             {
                 objErrorHandler.ErrorMsg = ex;
                 objErrorHandler.CreateLog();
                 sHTML = "";
             }
             return ProdimageRreplaceImages(sHTML);
         }

         public string ProdimageRreplaceImages(string shtml)
         {
             _RenderedHTML = shtml.ToString().Replace("</hcl>", "</B>").Replace("<hcl>", "<B>").Replace("<ars>g</ars>", "&rarr;").Replace("<sps>l</sps>", "<font face=\"Symbol\">l</font>");

             if (_Package == "NEWPRODUCT" || _Package == "CATEGORYLISTIMG" || _Package == "CSFAMILYPAGE" || _Package == "CSFAMILYPAGEWITHSUBFAMILY" || _Package == "PRODUCT")
             {
                 if (shtml.ToString().Contains("data-original=\"/prodimages\""))
                 {
                     if (_Package == "CSFAMILYPAGE")
                         _RenderedHTML = shtml.ToString().Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages/images/noimage.gif\"");
                     else
                         _RenderedHTML = shtml.ToString().Replace("data-original=\"/prodimages\"", "data-original=\"/prodimages/images/noimage.gif\"");
                     _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                 }
                 if (shtml.ToString().Contains("data-original=\"\""))
                 {
                     _RenderedHTML = shtml.ToString().Replace("data-original=\"\"", "data-original=\"/prodimages/images/noimage.gif\"");
                     _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                 }
             }
             else
             {
                 if (shtml.ToString().Contains("src=\"/prodimages\""))
                 {
                     if (_Package == "CSFAMILYPAGE")
                         _RenderedHTML = shtml.ToString().Replace("src=\"/prodimages\"", "src=\"/prodimages/images/noimage.gif\" style=\"display:none;\"");
                     else
                         _RenderedHTML = shtml.ToString().Replace("src=\"/prodimages\"", "src=\"/prodimages/images/noimage.gif\"");
                     _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('prodimages')\"", "href=\"#\"");
                 }
                 if (shtml.ToString().Contains("src=\"\""))
                 {
                     _RenderedHTML = shtml.ToString().Replace("src=\"\"", "src=\"/prodimages/images/noimage.gif\"");
                     _RenderedHTML = _RenderedHTML.Replace("href=\"javascript:Zoom('')\"", "href=\"#\"");
                 }

             }
             return _RenderedHTML;
         }

         protected string GetImagePath(object Path)
         {
             string retpath;
             System.IO.FileInfo Fil = new System.IO.FileInfo(strProdImages + Path.ToString());
             if (Fil.Exists)
             {
                 retpath = Path.ToString();
                 //retpath = objHelperServices.SetImageFolderPath(Path.ToString().Replace("\\", "/"), "_th", "_Images_200");
             }
             else
                 retpath = "/images/noimage.gif";

             return retpath;
         }
         }



        //public class ProductImage
        //{
        //    public string LargeImage { get; set; }
        //    public string Thumpnail { get; set; }
        //    public string MediumImage { get; set; }
        //    public string SmallImage { get; set; }
        //}

        //public class ProductDetails
        //{
        //    public string AttributeName { get; set; }
        //    public string SpecValue { get; set; }
        //    public int AttributeID { get; set; }
        //    public int SortOrder { get; set; }
        //}

        //public class ProductDesc
        //{
        //    public string AttributeName { get; set; }
        //    public string SpecValue { get; set; }
        //    public int AttributeID { get; set; }
        //}
        /*********************************** J TECH CODE ***********************************/
}

