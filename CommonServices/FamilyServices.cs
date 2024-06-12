using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.IO;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
using StringTemplate = Antlr3.ST.StringTemplate;
using StringTemplateGroup = Antlr3.ST.StringTemplateGroup;
using System.Globalization;
//using System.Diagnostics;  
namespace TradingBell.WebCat.CommonServices
{

    public class ProductSort
    {
        public string fid { get; set; }
        public string url { get; set; }
        public IList<DataClass> sortoptions { get; set; }
        public string screen { get; set; }

    }

    public class DataClass
    {
        public string option { get; set; }
        public string value { get; set; }
    }

    /*********************************** J TECH CODE ***********************************/
    public class FamilyServices
    {
        /*********************************** DECLARATION ***********************************/
        //Stopwatch stopwatch = new Stopwatch();
        ConnectionDB objConnectionDB = new ConnectionDB();
        HelperDB objHelperDb = new HelperDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        Security objSecurity = new Security();
        UserServices objUserServices=new UserServices();
        DataSet DsPreview = new DataSet();
        ProductServices objProductServices = new ProductServices(); 
        HelperServices objHelperServices = new HelperServices();
        public string WesCatalogId = System.Configuration.ConfigurationManager.AppSettings["WES_CATALOG_ID"];
        public string WAG_Root_Path = System.Configuration.ConfigurationManager.AppSettings["EA_ROOT_CATEGORY_PATH"];
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        // Order oOrder = new Order();
        //BuyerGroup oBG = new BuyerGroup();
        //Product oPro = new Product();
        string Restricted = "NO";
        int ProdID;
        //private int _familyID;
        private int _UserID;
        private int _catalogID;
        private bool _displayHeader;
        int pricecode = 0;
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
        /*********************************** OLD CODE ***********************************/
        //public int FamilyID
        //{
        //    get
        //    {
        //        return _familyID;
        //    }
        //    set
        //    {
        //        _familyID = value;
        //    }
        //}
        //public int UserID
        //{
        //    get
        //    {
        //        return _UserID;
        //    }
        //    set
        //    {
        //        _UserID = value;
        //    }
        //}
        //public int CatalogID
        //{
        //    get
        //    {
        //        return _catalogID;
        //    }
        //    set
        //    {
        //        _catalogID = value;

        //    }
        //}
        /*********************************** OLD CODE ***********************************/


        public bool DisplayHeaders
        {
            get
            {
                return _displayHeader;
            }
            set
            {
                _displayHeader = value;
            }
        }
        string Prefix = string.Empty; string Suffix = string.Empty; string EmptyCondition = string.Empty; string ReplaceText = string.Empty; string Headeroptions = string.Empty;

        /*********************************** DECLARATION ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PRICE CODE  ***/
        /********************************************************************************/
        public int GetPriceCode()
        {
            //int pc = -1;
            //DataTable Sqltb = new DataTable();
            //string userid = HttpContext.Current.Session["USER_ID"].ToString();
            //if (!string.IsNullOrEmpty(userid))
            //{
            //    string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
            //    Sqltb = oHelper.GetDataTable(sSQL);
            //    if (Sqltb != null && Sqltb.Rows.Count > 0)
            //        pc = Convert.ToInt32(Sqltb.Rows[0]["price_code"]);
            //}
            
            //return pc;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();

            if (userid == string.Empty)
                userid =  ConfigurationManager.AppSettings["DUM_USER_ID"];

            return objHelperDb.GetPriceCode(userid);
        }
        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public string GenerateHorizontalHTML(string _familyID, DataSet Ds)
        //{
        //    //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();
        //    DataSet dsBgDisc = new DataSet();
        //    decimal untPrice = 0;
        //    string AttrID = string.Empty;
        //    string HypColumn = "";
        //    int Min_ord_qty = 0;
        //    int Qty_avail;
        //    int flagtemp = 0;
        //    string _StockStatus = "NO STATUS AVAILABLE";
        //    string _AvilableQty = "0";

        //    string _Category_id = "";
        //    string _EA_Path = "";

        //    DataRow[] tempPriceDr;

        //    DataTable tempPriceDt;
        //    //int ProdID;
        //    int AttrType;
        //    string userid = HttpContext.Current.Session["USER_ID"].ToString();

        //    string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
        //    string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
        //    string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
        //    string _parentFamily_Id = "0";
        //    if (EComState == "YES")
        //        if (!objHelperServices.GetIsEcomEnabled(userid))
        //            EComState = "NO";
        //    StringBuilder strBldr = new StringBuilder();
        //    StringBuilder strBldrcost = new StringBuilder();

        //    if (HttpContext.Current.Request.QueryString["path"] != null)
        //        _EA_Path = HttpUtility.UrlDecode(objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString()));

        //    if (HttpContext.Current.Request.QueryString["cid"] != null)
        //        _Category_id = HttpContext.Current.Request.QueryString["cid"];

        //    DsPreview = Ds;
        //    if (DsPreview.Tables[_familyID] == null)
        //        return "";


        //    DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
        //    if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
        //        _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

        //    if (_parentFamily_Id == "0")
        //        _parentFamily_Id = _familyID;

        //    strBldr.Append("<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\"><tr> <td align=\"right\" ><TABLE width=\"99%\" border=0 cellspacing=1 Class=\"FamilyPageTable\" cellpadding=3>");
        //    //strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>");


        //    //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
        //    //oHelper.SQLString = sSQL;
        //    //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
        //    pricecode = GetPriceCode();
        //    DisplayHeaders = true;
        //    if (DisplayHeaders == true)
        //    {
        //        strBldrcost = new StringBuilder();
        //        strBldr.Append("<TR>");
        //        for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //        {
        //            //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
        //            //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());

        //            DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
        //            if (tempdr.Length > 0)
        //            {
        //                AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());
        //                if (AttrType != 3)
        //                {
        //                    if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() == "COST")
        //                    {
        //                        strBldrcost.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;  \" >");

        //                        if (pricecode == 1)
        //                        {
        //                            strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
        //                        }
        //                        else
        //                        {
        //                            strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
        //                        }

        //                        strBldrcost.Append("</TD>");
        //                    }
        //                    else
        //                    {
        //                        strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;  \" >");

        //                        if (pricecode == 1)
        //                        {
        //                            strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
        //                        }
        //                        else
        //                        {
        //                            strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
        //                        }

        //                        strBldr.Append("</TD>");
        //                    }
        //                }
        //                else
        //                {
        //                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;   \" >");
        //                    strBldr.Append("</TD>");
        //                }
        //            }
        //        }
        //        if (DsPreview.Tables[_familyID].Rows.Count > 0)
        //            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;   \" >More Info</TD>");

        //        if (strBldrcost.ToString() != "")
        //        {
        //            strBldr.Append(strBldrcost);
        //        }
        //        if (EComState.ToUpper() == "YES" && DsPreview.Tables[_familyID].Rows.Count > 0)
        //        {
        //            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width:120px;    \" >Cart</TD>");
        //        }
        //        strBldr.Append("</TR>");
        //    }
        //    string ValueFortag = string.Empty;
        //    bool rowcolor = false;

        //    if (_EA_Path == "" && _Category_id == "")
        //    {
        //        DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
        //        if (tmpds != null && tmpds.Tables.Count > 0)
        //        {
        //            _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
        //            string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch=Family Id=" + _familyID.ToString();
        //            _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
        //        }
        //    }



        //    for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
        //    {
        //        strBldr.Append("<TR>");
        //        if (rowcolor == false && i != 0)
        //        {
        //            rowcolor = true;
        //        }
        //        else if (rowcolor == true)
        //        {
        //            rowcolor = false;
        //        }
        //        tempPriceDr = DsPreview.Tables["ProductPrice"].Select("Product_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
        //        if (tempPriceDr.Length > 0)
        //            tempPriceDt = tempPriceDr.CopyToDataTable();
        //        else
        //            tempPriceDt = null;
        //        strBldrcost = new StringBuilder();
        //        for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //        {

        //            string alignVal = "LEFT";

        //            DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
        //            if (tempdr.Length > 0)
        //            {
        //                ExtractCurrenyFormat(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATARULE"].ToString());
        //                AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());


        //                //AttrID = DsPreview.Tables[1].Rows[0][DsPreview.Tables[_familyID].Columns[j].ToString()].ToString();
        //                //ExtractCurrenyFormat(Convert.ToInt32(AttrID));
        //                //oHelper.SQLString = "SELECT ATTRIBUTE_DATATYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID = " + AttrID;
        //                //DataSet DSS = oHelper.GetDataSet();
        //                //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
        //                //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());


        //                //if (AttrType == 4 || DSS.Tables[0].Rows[0].ItemArray[0].ToString().Substring(0, 3).ToUpper() == "NUM")
        //                if (AttrType == 4 || tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATATYPE"].ToString().Substring(0, 3).ToUpper() == "NUM")
        //                {
        //                    alignVal = "RIGHT";
        //                }
        //                if (AttrType == 3)
        //                {
        //                    ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString();
        //                    if (ValueFortag != "" && ValueFortag != null)
        //                    {
        //                        FileInfo Fil;
        //                        string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
        //                        Fil = new FileInfo(strFile + ValueFortag);
        //                        if (Fil.Exists)
        //                        {
        //                            ValueFortag = "prodimages" + ValueFortag;
        //                        }
        //                        else
        //                        {
        //                            ValueFortag = "Images/NoImage.gif";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ValueFortag = "Images/NoImage.gif";
        //                    }

        //                    if (rowcolor == false)
        //                    {
        //                        strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200px;   \"><img src=\"" + ValueFortag + "\"style=\"max-height:50px;max-width:50px\" /></td>");
        //                    }
        //                    else if (rowcolor == true)
        //                    {
        //                        strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200px;   \"><img src=\"" + ValueFortag + "\"style=\"max-height:50px;max-width:50px\" /></td>");

        //                    }
        //                }
        //                // strBldr.Append("<TD ALIGN=\"" + alignVal + getCellString(DsPreview.Tables[_familyID].Rows[i][j].ToString()));
        //                else  //if (chkAttrType[j] == 4)
        //                {
        //                    if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
        //                    {
        //                        if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[_familyID].Rows[i][j].ToString() == string.Empty))
        //                        {
        //                            ValueFortag = ReplaceText;
        //                        }
        //                        else if ((DsPreview.Tables[_familyID].Rows[i][j].ToString()) == (EmptyCondition))
        //                        {
        //                            ValueFortag = ReplaceText;
        //                        }
        //                        else
        //                        {
        //                            if (Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                            {
        //                                if (AttrType == 4)
        //                                {
        //                                    //int _prodid = System.Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
        //                                    //ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(GetMyPrice(_prodid))).ToString() + " " + Suffix;                                            

        //                                    if (tempPriceDt != null)
        //                                        ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(tempPriceDt.Rows[0]["Price"].ToString())).ToString() + " " + Suffix;
        //                                    else
        //                                        ValueFortag = Prefix + " " + "" + " " + Suffix;


        //                                }
        //                                else
        //                                {
        //                                    ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (DsPreview.Tables[_familyID].Rows[i][j].ToString().Length > 0)
        //                                {
        //                                    ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                                }
        //                                else
        //                                {
        //                                    ValueFortag = string.Empty;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                        {
        //                            ValueFortag = Convert.ToDouble(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
        //                        }
        //                        else
        //                        {
        //                            ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                        }
        //                    }
        //                    //if (DsPreview.Tables[_familyID].Columns[j].Caption.ToLower() == NavColumn.ToLower().ToString())
        //                    //{
        //                    //    ProdID = oHelper.CI(DsPreview.Tables[_familyID].Rows[i][0].ToString());
        //                    //    HypColumn = HypCURL.Replace("{PRODUCT_ID}", ProdID.ToString());
        //                    //    Min_ord_qty = oHelper.CI(oOrder.GetProductMinimumOrderQty(ProdID));
        //                    //    HypColumn = HypColumn.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
        //                    //    Qty_avail = oHelper.CI(oOrder.GetProductAvilableQty(ProdID));
        //                    //    HypColumn = HypColumn.Replace("{QTY_AVAIL}", Qty_avail.ToString());
        //                    //    HypColumn = HypColumn.Replace("{FAMILY_ID}", this.FamilyID.ToString());

        //                    //    ValueFortag = "<A HREF=\"" + HypColumn + "\" > " + ValueFortag + "</A>";
        //                    //}
        //                    if (AttrType == 4)
        //                    {
        //                        _StockStatus = "NO STATUS AVAILABLE";
        //                        _AvilableQty = "0";
        //                        string _ProCode = "";
        //                        if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
        //                            _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();
        //                        if (tempPriceDt != null)
        //                        {
        //                            _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
        //                            _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();

        //                        }
        //                        string _Buyer_Group = GetBuyerGroup(Convert.ToInt32(userid));
        //                        if (Convert.ToInt32(userid) > 0)
        //                        {

        //                            dsBgDisc = GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
        //                        }
        //                        else
        //                        {
        //                            dsBgDisc = GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
        //                        }

        //                        if (dsBgDisc != null)
        //                        {
        //                            if (dsBgDisc.Tables[0].Rows.Count > 0)
        //                            {
        //                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
        //                                DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
        //                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
        //                                untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
        //                                bool IsBGCatProd = IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
        //                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
        //                                {
        //                                    ValueFortag = CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

        //                                }
        //                            }
        //                        }
        //                        ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
        //                        //ValueFortag = oHelper.GetOptionValues("CURRENCYFORMAT").ToString() + ValueFortag;
        //                    }
        //                    if (rowcolor == false)
        //                    {
        //                        if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
        //                            strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \" >" + ValueFortag + "</TD>");
        //                        else
        //                            strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \" >" + ValueFortag + "</TD>");


        //                    }
        //                    else if (rowcolor == true)
        //                    {
        //                        if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
        //                            strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \" >" + ValueFortag + "</TD>");
        //                        else
        //                            strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \" >" + ValueFortag + "</TD>");
        //                    }
        //                }
        //                //else
        //                //{
        //                //    strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: white  \" >" + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + "</TD>");
        //                //}

        //                //Add the Shipping and Cart Images
        //                if (j == DsPreview.Tables[_familyID].Columns.Count - 1)
        //                {

        //                    ProdID = objHelperServices.CI(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
        //                    //Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);                                                        
        //                    //int IsAvailable = oPro.GetProductAvailability(ProdID);
        //                    string ShipImgPath = "";
        //                    int IsAvailable = 0;
        //                    _StockStatus = "NO STATUS AVAILABLE";
        //                    _AvilableQty = "0";
        //                    Boolean IsShipping = false;
        //                    if (tempPriceDt != null)
        //                    {
        //                        IsShipping = ((tempPriceDt.Rows[0]["IS_SHIPPING"].ToString() == "0") ? false : true);
        //                        if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "AVAILABLE")
        //                            IsAvailable = 1;
        //                        else if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "N/A" || tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "DISCONTINUED")
        //                            IsAvailable = 0;
        //                        _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
        //                        _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
        //                    }

        //                    if (IsShipping == true)
        //                    {
        //                        ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("SHIPPING IMAGE").ToString();
        //                        string ShipUrl = objHelperServices.GetOptionValues("SHIP URL").ToString();
        //                        ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
        //                    }
        //                    else if (IsShipping == false)
        //                    {
        //                        ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("NO SHIPPING IMAGE").ToString();
        //                        ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
        //                    }
        //                    string tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch=Family Id=" + _parentFamily_Id.ToString()));
        //                    ShipImgPath = "<a href=\"pd.aspx?pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath + "\" class=\"tx_3\">" +
        //                                      "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";


        //                    if (rowcolor == false)
        //                    {
        //                        strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \">" + ShipImgPath + "</TD>");
        //                    }
        //                    else if (rowcolor == true)
        //                    {
        //                        strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">" + ShipImgPath + "</TD>");
        //                    }
        //                    if (strBldrcost.ToString() != "")
        //                    {
        //                        strBldr.Append(strBldrcost);
        //                    }
        //                    if (EComState.ToUpper() == "YES")
        //                    {
        //                        //Add the Cart Image
        //                        string CartImgPath = "";
        //                        //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
        //                        if (Restricted.ToUpper() == "YES")
        //                        {
        //                            CartImgPath = objHelperServices.GetOptionValues("RESTRICTED PRODUCT TEXT");
        //                            string CartUrl = objHelperServices.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
        //                            CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
        //                        }
        //                        else
        //                        {
        //                            if (IsAvailable == 1)
        //                            {
        //                                CartImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("CARTIMGPATH").ToString();

        //                                //Min_ord_qty = oOrder.GetProductMinimumOrderQty(ProdID);
        //                                //string _StockStatus = GetStockStatus(Convert.ToInt32(ProdID.ToString()));
        //                                _StockStatus = "NO STATUS AVAILABLE";
        //                                _AvilableQty = "0";
        //                                if (tempPriceDt != null)
        //                                {
        //                                    Min_ord_qty = Convert.ToInt32(tempPriceDt.Rows[0]["MIN_ORD_QTY"].ToString());
        //                                    _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
        //                                    _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
        //                                }
        //                                string CartUrl = objHelperServices.GetOptionValues("CARTURL").ToString();

        //                                CartUrl = CartUrl.Replace("{PRODUCT_ID}", ProdID.ToString());
        //                                CartUrl = CartUrl.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
        //                                CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + CartImgPath + "\" style=\"border-width:0\"></A>";




        //                                string _StockStatusTrim = _StockStatus.Trim();
        //                                bool _Tbt_Stock_Status_2 = false;

        //                                switch (_StockStatusTrim)
        //                                {
        //                                    case "IN STOCK":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "SPECIAL ORDER":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "SPECIAL ORDER PRICE &":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "DISCONTINUED":
        //                                        _Tbt_Stock_Status_2 = false;
        //                                        break;
        //                                    case "DISCONTINUED NO LONGER AVAILABLE":
        //                                        _Tbt_Stock_Status_2 = false;
        //                                        break;
        //                                    case "DISCONTINUED NO LONGER":
        //                                        _Tbt_Stock_Status_2 = false;
        //                                        break;
        //                                    case "TEMPORARY UNAVAILABLE":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "TEMPORARY UNAVAILABLE NO ETA":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "OUT OF STOCK":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "OUT OF STOCK ITEM WILL":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    default:
        //                                        _Tbt_Stock_Status_2 = false;
        //                                        break;
        //                                }

        //                                if (_Tbt_Stock_Status_2 == true)
        //                                {

        //                                    CartImgPath = "<table width=\"100%\" border=\"0\" cellPadding=\"0\" cellSpacing=\"0\"><tr><td>" +
        //                                               "<input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" type=\"text\" size=\"1\" id=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;width: 18px;\"   /> " +
        //                                             "</td><td width=\"2\">&nbsp;</td><td>" +
        //                                             "  <a style=\"cursor:pointer;\" valign=\"middle\"  onMouseOut=\"javascript:MM_swapImgRestore();ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOver=\"javascript:MM_swapImage('Image" + ProdID.ToString() + "_fp','','images/but_buy2.gif',1);ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\">" +
        //                                        //"<div onmouseout=\"MM_swapImgRestore()\" onmouseover=\"MM_swapImage('Image"+ ProdID.ToString() + "_fp','','images/but_buy2.gif',1)\" style=\"width:76px; height:25px; cursor:pointer; \">" +
        //                                             "   <img src=\"images/but_buy1.gif\" name=\"Image" + ProdID.ToString() + "_fp\" width=\"76\" height=\"25\" border=\"0\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "','" + ProdID.ToString() + "');\"/>" +
        //                                             "</a></td></tr></table>";
        //                                }
        //                                else
        //                                {
        //                                    CartImgPath = "";
        //                                }

        //                                if (rowcolor == false)
        //                                {
        //                                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 120px; \">" + CartImgPath + "</TD>");
        //                                }
        //                                if (rowcolor == true)
        //                                {
        //                                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 120px;  \">" + CartImgPath + "</TD>");
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (rowcolor == false)
        //                                {
        //                                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 200px;   \">N/A</TD>");
        //                                }
        //                                if (rowcolor == true)
        //                                {
        //                                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"   Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">N/A</TD>");
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        strBldr.Append("</TR>");
        //    }

        //    strBldr.Append("</TABLE></td></tr></table>");
        //    //if (strBldr.ToString().Contains("<TABLE border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style><TR></TR></TABLE>"))
        //    //{
        //    //    strBldr = strBldr.Remove(0, strBldr.Length);
        //    //}
        //    return strBldr.ToString();
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PRODUCT DETAILS DYNAMICALLY ON FAMILY PAGE ***/
        /********************************************************************************/
        //public string GenerateHorizontalHTML(string _familyID, DataSet Ds)
        //{
        //    //ServiceProvider.ProductValidationServices Oservices = new TradingBell5.CatalogStudio.ServiceProvider.ProductValidationServices();





        //    DataSet dsBgDisc = new DataSet();
        //    decimal untPrice = 0;
        //    string AttrID = string.Empty;
        //  //  string HypColumn = "";
        //    int Min_ord_qty = 0;
        //  //  int Qty_avail;
        //  //  int flagtemp = 0;
        //    string _StockStatus = "NO STATUS AVAILABLE";
        //    string _AvilableQty = "0";

        //    string _Category_id = string.Empty;
        //    string _EA_Path = string.Empty;
        //    string StrPriceTable = string.Empty;
        //    DataRow[] tempPriceDr;

        //    DataTable tempPriceDt;
        //    //int ProdID;
        //    int AttrType;
        //    string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //    if (userid == "" || userid == null)
        //        userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
        //        //userid = "999";// ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

        //    string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
        //    string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
        //    string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
        //    string _parentFamily_Id = "0";
        //    string CartImgPathdisplay = string.Empty;
        //    string _ProCode = string.Empty;

        //   // if (userid == "999")
        //    //    CartImgPathdisplay = "display:none;";


        //    if (EComState == "YES")
        //        if (!objHelperServices.GetIsEcomEnabled(userid))
        //            EComState = "NO";
        //    StringBuilder strBldr = new StringBuilder();
        //    StringBuilder strBldrcost = new StringBuilder();

        //    if (HttpContext.Current.Request.QueryString["path"] != null)            
        //        _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());   

        //    if (HttpContext.Current.Request.QueryString["cid"] != null)
        //        _Category_id=HttpContext.Current.Request.QueryString["cid"] ;

        //    DsPreview = Ds;
        //    if (DsPreview.Tables[_familyID] == null)
        //        return "";


        //    DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
        //    if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
        //        _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

        //    if (_parentFamily_Id == "0")
        //        _parentFamily_Id = _familyID;

        //    strBldr.Append("<table width=\"100%\" border=\"0\" style=\" overflow:hidden;\" cellpadding=\"0\" cellspacing=\"0\"><tr> <td align=\"left\" ><div Class=\"testscroll\"><TABLE width=\"99%\" border=0 cellspacing=1 Class=\"FamilyPageTable\" cellpadding=3>");
        //    //strBldr.Append("<style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style>");

           
        //    //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
        //    //oHelper.SQLString = sSQL;
        //    //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
        //    pricecode = GetPriceCode();
        //    DisplayHeaders = true;
        //    if (DisplayHeaders == true)
        //    {
        //        strBldrcost = new StringBuilder();
        //        strBldr.Append("<TR>");
        //        for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //        {
        //            //oHelper.SQLString = "SELECT ATTRIBUTE_TYPE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_NAME= '" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'";
        //            //AttrType = oHelper.CI(oHelper.GetValue("ATTRIBUTE_TYPE").ToString());

        //          //  string attrname = "";
        //           // if (DsPreview.Tables[_familyID].Columns[j].ToString().ToUpper() != "CODE")
        //             //   attrname = DsPreview.Tables[_familyID].Columns[j].ToString();
        //            DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
        //            if (tempdr.Length > 0)
        //            {
        //                AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());
        //                if (AttrType != 3)
        //                {
        //                    if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper()=="COST")
        //                    {
        //                        strBldrcost.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;  \" >");

        //                        if (pricecode == 1)
        //                        {
        //                            strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
        //                        }
        //                        else
        //                        {
        //                            strBldrcost.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
        //                        }

        //                        strBldrcost.Append("</TD>");
        //                    }
        //                    else
        //                    {
        //                       // if( DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() != "CODE")
        //                        strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;  \" >");

        //                        if (pricecode == 1)
        //                        {
        //                            //if (DsPreview.Tables[_familyID].Columns[j].Caption == "Code")
        //                            //    strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() + (AttrType == 4 ? " Inc GST" : ""));
        //                            if (DsPreview.Tables[_familyID].Columns[j].Caption == "Code")
        //                                strBldr.Append("Order Code" + (AttrType == 4 ? " Inc GST" : ""));
        //                            else
        //                                strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
        //                           // if (DsPreview.Tables[_familyID].Columns[j].Caption == "PROD_CODE")
        //                            //    strBldr.Append("Order Code" + (AttrType == 4 ? " Inc GST" : ""));
        //                            //else
        //                            //    strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Inc GST" : ""));
        //                        }
        //                        else
        //                        {
        //                            strBldr.Append(DsPreview.Tables[_familyID].Columns[j].Caption + (AttrType == 4 ? " Ex GST" : ""));
        //                        }

        //                        strBldr.Append("</TD>");
        //                    }
        //                }                      
        //                else
        //                {
        //                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;   \" >");
        //                    strBldr.Append("</TD>");
        //                }
        //            }
        //        }
        //        if (DsPreview.Tables[_familyID].Rows.Count > 0)
        //            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width: 200px;   \" >More Info</TD>");

        //        if (strBldrcost.ToString()!="" )
        //        {
        //            if (userid == "0")
        //                userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

        //            if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0 )
        //            strBldr.Append(strBldrcost);
        //        }
        //        if (EComState.ToUpper() == "YES" && DsPreview.Tables[_familyID].Rows.Count > 0)
        //        {
        //            strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\" Class=\"FamilyPageTableHead\" style=\"width:120px;"+ CartImgPathdisplay+"\" >Cart</TD>");
        //        }
        //        strBldr.Append("</TR>");
        //    }
        //    string ValueFortag = string.Empty;
        //    bool rowcolor = false;

        //    if (_EA_Path == "" && _Category_id == "")
        //    {
        //        DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
        //        if (tmpds != null && tmpds.Tables.Count > 0)
        //        {
        //            _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
        //            string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch=Family Id=" + _familyID.ToString();
        //            _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
        //        }
        //    }



        //    for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
        //    {
               
        //        strBldr.Append("<TR>");
        //        if (rowcolor == false && i != 0)
        //        {
        //            rowcolor = true;
        //        }
        //        else if (rowcolor == true)
        //        {
        //            rowcolor = false;
        //        }
               
        //        tempPriceDr = DsPreview.Tables["ProductPrice"].Select("Product_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
        //        if(tempPriceDr.Length>0)
        //            tempPriceDt=tempPriceDr.CopyToDataTable();
        //        else       
        //            tempPriceDt=null;

        //        strBldrcost = new StringBuilder();
        //        for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //        {
                    
        //            string alignVal = "LEFT";
        //            DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
        //            if (tempdr.Length > 0)
        //            {
        //                ExtractCurrenyFormat(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATARULE"].ToString());
        //                AttrType = objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString());


        //                if (AttrType == 4 || tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_DATATYPE"].ToString().Substring(0, 3).ToUpper() == "NUM")
        //                {
        //                    alignVal = "RIGHT";
        //                }
        //                if (AttrType == 3)
        //                {
        //                    ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString();
        //                    string ValueLargeImg = string.Empty;
        //                    if (ValueFortag != string.Empty && ValueFortag != null)
        //                    {
        //                        FileInfo Fil;
        //                        string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
                               
        //                        Fil = new FileInfo(strFile + ValueFortag);
        //                        if (Fil.Exists)
        //                        {
        //                            //ValueFortag = "/prodimages" + ValueFortag;
        //                            //ValueLargeImg = ValueFortag.ToLower().Replace("_th","_Images_200");
        //                            ValueFortag = "/prodimages" + ValueFortag.Replace("\\", "/");
        //                            ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200").Replace("\\", "/");
        //                        }
        //                        else
        //                        {
        //                            ValueFortag = "/prodimages/images/noimage.gif";
        //                            ValueLargeImg = "";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ValueFortag = "/prodimages/images/noimage.gif";
        //                        ValueLargeImg = "";
        //                    }
        //                    string Popupdiv="";
        //                    if (ValueLargeImg != "")
        //                    {
        //                       // ValueLargeImg = " http://localhost:65375/" + ValueLargeImg.Replace("\\", "/");
        //                       // Popupdiv = "<div class=\"pro_img_popup\" id=\"pro_img_popup" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "\"><img alt=\"dummny img\" src=\"" + ValueLargeImg.Replace("\\", "/") + "\"></div>";
        //                        Popupdiv = "<div class=\"pro_img_popup\" style=\" visibility:hidden; \"  id=\"pro_img_popup" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "\"><img alt=\"dummny img\" src=\"" + ValueLargeImg + "\"></div>";
        //                    }
        //                    if (rowcolor == false)
        //                    {
        //                       // ValueFortag = " http://localhost:65375/" + ValueFortag.Replace("\\","/");

        //                        //strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200px;   \"><div class=\"pro_thum_outer\">" + Popupdiv + "<img src=\"" + ValueFortag + "\" onMouseOut=\"javascript:Moutimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\" onMouseOver=\"javascript:Moverimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\" style=\"max-height:50px;max-width:50px\" /></div></td>");
        //                        strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200px;   \"><div class=\"pro_thum_outer\">" + Popupdiv + "<img class=\"lazy\" data-original=\"" + ValueFortag + "\" onMouseOut=\"javascript:Moutimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\" onMouseOver=\"javascript:Moverimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\" style=\"max-height:50px;max-width:50px\" /></div></td>");
        //                    }
        //                    else if (rowcolor == true)
        //                    {
        //                       // ValueFortag = " http://localhost:65375/" + ValueFortag.Replace("\\", "/");
        //                        //strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200px;   \"><div class=\"pro_thum_outer\">" + Popupdiv + "<img src=\"" + ValueFortag + "\" onMouseOut=\"javascript:Moutimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\" onMouseOver=\"javascript:Moverimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\"  style=\"max-height:50px;max-width:50px\" /></div></td>");
        //                        strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\" Class=\"FamilyPageTableCell\" style=\"width: 200px;   \"><div class=\"pro_thum_outer\">" + Popupdiv + "<img class=\"lazy\" data-original=\"" + ValueFortag + "\" onMouseOut=\"javascript:Moutimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\" onMouseOver=\"javascript:Moverimgtag('" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "');\"  style=\"max-height:50px;max-width:50px\" /></div></td>");

        //                    }
        //                }
        //                // strBldr.Append("<TD ALIGN=\"" + alignVal + getCellString(DsPreview.Tables[_familyID].Rows[i][j].ToString()));
        //                else  //if (chkAttrType[j] == 4)
        //                {
        //                    if ((Headeroptions == "All") || (Headeroptions != "All" && i == 0))
        //                    {
        //                        if ((EmptyCondition == "Null" || EmptyCondition == "Empty" || EmptyCondition == null) && (DsPreview.Tables[_familyID].Rows[i][j].ToString() == string.Empty))
        //                        {
        //                            ValueFortag = ReplaceText;
        //                        }
        //                        else if ((DsPreview.Tables[_familyID].Rows[i][j].ToString()) == (EmptyCondition))
        //                        {
        //                            ValueFortag = ReplaceText;
        //                        }
        //                        else
        //                        {
        //                            if (Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                            {
        //                                if (AttrType == 4)
        //                                {
        //                                    //int _prodid = System.Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
        //                                    //ValueFortag = Prefix + " " + oHelper.FixDecPlace(Convert.ToDecimal(GetMyPrice(_prodid))).ToString() + " " + Suffix;                                            
        //                                    /* DB price 
        //                                    //if (tempPriceDt!=null)
        //                                    //    ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(tempPriceDt.Rows[0]["Price"].ToString())).ToString() + " " + Suffix;                                                                                        
        //                                    //else
        //                                    //    ValueFortag = Prefix + " " + "" + " " + Suffix;                                                                                        
        //                                     DB price */

        //                                    if (Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString()) > 0)
        //                                        ValueFortag = Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + Suffix;
        //                                    else
        //                                        ValueFortag = "";
                                            

        //                                }
        //                                else
        //                                {
        //                                    ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (DsPreview.Tables[_familyID].Rows[i][j].ToString().Length > 0)
        //                                {
        //                                    ValueFortag = Prefix + " " + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + " " + Suffix;
        //                                }
        //                                else
        //                                {
        //                                    ValueFortag = string.Empty;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (Isnumber(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")) == true)
        //                        {
        //                            ValueFortag = Convert.ToDouble(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")).ToString();
        //                        }
        //                        else
        //                        {
        //                            ValueFortag = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                        }
        //                    }
        //                    //if (DsPreview.Tables[_familyID].Columns[j].Caption.ToLower() == NavColumn.ToLower().ToString())
        //                    //{
        //                    //    ProdID = oHelper.CI(DsPreview.Tables[_familyID].Rows[i][0].ToString());
        //                    //    HypColumn = HypCURL.Replace("{PRODUCT_ID}", ProdID.ToString());
        //                    //    Min_ord_qty = oHelper.CI(oOrder.GetProductMinimumOrderQty(ProdID));
        //                    //    HypColumn = HypColumn.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
        //                    //    Qty_avail = oHelper.CI(oOrder.GetProductAvilableQty(ProdID));
        //                    //    HypColumn = HypColumn.Replace("{QTY_AVAIL}", Qty_avail.ToString());
        //                    //    HypColumn = HypColumn.Replace("{FAMILY_ID}", this.FamilyID.ToString());

        //                    //    ValueFortag = "<A HREF=\"" + HypColumn + "\" > " + ValueFortag + "</A>";
        //                    //}
        //                    if (AttrType == 4)
        //                    {
        //                        _StockStatus = "NO STATUS AVAILABLE";
        //                        _AvilableQty = "0";
        //                        //string _ProCode = "";
        //                        if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
        //                            _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();
        //                        if (tempPriceDt != null)
        //                        {
        //                            _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
        //                            _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
                                   
        //                        }
        //                        string _Buyer_Group = GetBuyerGroup(Convert.ToInt32 (userid));
        //                        if (Convert.ToInt32(userid) > 0)
        //                        {

        //                            dsBgDisc = GetBuyerGroupBasedDiscountDetails(_Buyer_Group);
        //                        }
        //                        else
        //                        {
        //                            dsBgDisc = GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
        //                        }

        //                        if (dsBgDisc != null)
        //                        {
        //                            if (dsBgDisc.Tables[0].Rows.Count > 0)
        //                            {
        //                                decimal DiscVal = objHelperServices.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
        //                                DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
        //                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
        //                                untPrice = objHelperServices.CDEC(DsPreview.Tables[_familyID].Rows[i][j].ToString());
        //                                bool IsBGCatProd = IsBGCatalogProduct(Convert.ToInt32(WesCatalogId), _Buyer_Group);
        //                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
        //                                {
        //                                    ValueFortag = CalculateBGDiscountPrice(untPrice, DiscVal, CalMth).ToString();

        //                                }
        //                            }
        //                        }
        //                        StrPriceTable=AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus);
        //                      //  ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" style=\"background-color:#ffffff;visibility:hidden;position:absolute\">" + AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus) + "</div><div onMouseOver=\"javascript:ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOut=\"javascript:ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" style=\"position:relative\">" + ValueFortag + "</div>";
        //                        ValueFortag = "<div id=\"pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "\" class=\"popupouterdiv2none\"><div class=\"popupaero\"></div>" + StrPriceTable + "</div><a class=\"poppricenone\" onMouseOut=\"javascript:Moutstockstatus('" + Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()) + "');\" onMouseOver=\"javascript:Moverstockstatus('" + Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()) + "');\" style=\"text-decoration:none;\">" + ValueFortag + " <br/> Price / Stock Status </a>";
        //                       // ValueFortag = "<div class=\"popupaero\"></div>";
        //                    }
        //                    if (rowcolor == false)
        //                    {
        //                        if (AttrType == 4 &&  DsPreview.Tables[_familyID].Columns[j].ToString()=="COST")
        //                            //strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell123\" style=\"width: 200px;cursor:pointer;   \" ><div class=\"pricepopup\">" + ValueFortag + " </Div></TD>");
        //                            strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"costable\" style=\"width: 200px;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   \" ><div class=\"pricepopup\">" + ValueFortag + "</Div></TD>");
        //                        else
        //                            strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \" >" + ValueFortag + "</TD>");                                
                                    

        //                    }
        //                    else if (rowcolor == true)
        //                    {
        //                        if (AttrType == 4 && DsPreview.Tables[_familyID].Columns[j].ToString() == "COST")
        //                            strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"costable\" style=\"width: 200px;cursor:pointer;border-color: -moz-use-text-color #E8E8E8 #E8E8E8 -moz-use-text-color;border-style: none solid solid none;border-width: medium 1px 1px medium;border-color:#E8E8E8;   \" ><div class=\"pricepopup\">" + ValueFortag + "</Div></TD>");
        //                            //strBldrcost.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell123\" style=\"width: 200px;cursor:pointer;   \" ><div class=\"pricepopup\">" + ValueFortag + " </Div></TD>");
        //                        else
        //                            strBldr.Append("<TD ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \" >" + ValueFortag + "</TD>");
        //                    }
        //                }
        //                //else
        //                //{
        //                //    strBldr.Append("<TD ALIGN=\"" + alignVal + "\" VALIGN=\"Middle\" style=\"width: 200px; color: Black; BACKGROUND-COLOR: white  \" >" + DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;") + "</TD>");
        //                //}

        //                //Add the Shipping and Cart Images
        //                if (j == DsPreview.Tables[_familyID].Columns.Count - 1)
        //                {

        //                    ProdID = objHelperServices.CI(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
        //                    //Boolean IsShipping = oOrder.GetProductIsShipping(ProdID);                                                        
        //                    //int IsAvailable = oPro.GetProductAvailability(ProdID);
        //                    string ShipImgPath = string.Empty;
        //                    int IsAvailable = 0;
        //                     _StockStatus = "NO STATUS AVAILABLE";
        //                     _AvilableQty = "0";
        //                    Boolean IsShipping=false;
        //                    if (tempPriceDt != null)
        //                    {
        //                        IsShipping = ((tempPriceDt.Rows[0]["IS_SHIPPING"].ToString() == "0") ? false : true);
        //                        if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "AVAILABLE")
        //                            IsAvailable = 1;
        //                        else if (tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "N/A" || tempPriceDt.Rows[0]["PRODUCT_STATUS"].ToString().ToUpper() == "DISCONTINUED")
        //                            IsAvailable = 0;
        //                        _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_", " ");
        //                        _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
        //                    }                            
                                                      
        //                    if (IsShipping == true)
        //                    {
        //                        ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("SHIPPING IMAGE").ToString();
        //                        string ShipUrl = objHelperServices.GetOptionValues("SHIP URL").ToString();
        //                        ShipImgPath = "<a href=\"" + ShipUrl + "\" style=\"text-decoration:none\"><img src=\"/" + ShipImgPath + "\" style=\"border-width:0\"></a>";
        //                    }
        //                    else if (IsShipping == false)
        //                    {
        //                        ShipImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("NO SHIPPING IMAGE").ToString();
        //                        ShipImgPath = "<img src=\"/" + ShipImgPath + "\" style=\"border-width:0\">";
        //                    }
        //                    string tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch=Family Id=" + _parentFamily_Id.ToString()));
                           
                            
        //                    //ShipImgPath = "<a href=\"pd.aspx?pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath + "\" class=\"tx_3\">" +
        //                    //                 // "<img src=\"images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>See More Details </a>";
        //                    //                 "See More Details </a>";

        //                    //aDDED BY  INDU
        //                    //*

        //                    string ORIGINALURL = "pd.aspx?pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath;
        //                    string NEWURL = string.Empty;
        //                    string HREFURL = string.Empty;


        //                    //if (HttpContext.Current.Session["PARENTFAMILY"] != null)
        //                    //{
        //                    //    NEWURL = HttpContext.Current.Session["PARENTFAMILY"].ToString() + "/" + _ProCode;

        //                    //    HREFURL = "pd.aspx?" + NEWURL;
        //                    //}
        //                    //else
        //                    //{
        //                    //    NEWURL = ORIGINALURL;
        //                    //    HREFURL = ORIGINALURL;

        //                    //}

        //                    if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
        //                    {
        //                        NEWURL = HttpContext.Current.Session["breadcrumEAPATH"].ToString() + "////" +DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString()+"="+ _ProCode;
        //                        NEWURL = objHelperServices.Cons_NewURl_bybrand(ORIGINALURL, NEWURL, "pd.aspx", "");
        //                        HREFURL = "/"+ NEWURL +"/pd/" ;
        //                    }
        //                    else
        //                    {
        //                        NEWURL = ORIGINALURL;
        //                        HREFURL = ORIGINALURL;

        //                    }

        //                    HREFURL = "/" + NEWURL + "/pd/";

        //                    ShipImgPath = "<a href=\"" + HREFURL + "\"   title=\"" + _ProCode + "\" rel=\"pd.aspx?pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath + "\" class=\"tx_3\">" +
        //                                        "<img src=\"/images/ico_details.gif\" width=\"17\" height=\"14\" border=\"0\" align=\"absmiddle\" /> <br/>Details </a>";



        //                    objHelperServices.Cons_NewURl_bybrand(ORIGINALURL, NEWURL, "pd.aspx", "");
        //                    //objHelperServices.URLRewriteToAddressBar("pd.aspx", NEWURL, ORIGINALURL, HttpContext.Current.Server.MapPath("URL_Rewrite_Prod.ini"),true);   
        //                    //*
                            
                            
                            
                            
                            
                            
                            
        //                    if (rowcolor == false)
        //                    {
        //                        strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;   \">" + ShipImgPath + "</TD>");
        //                    }
        //                    else if (rowcolor == true)
        //                    {
        //                        strBldr.Append("<TD  ALIGN=\"center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\" style=\"width: 200px;  \">" + ShipImgPath + "</TD>");
        //                    }
        //                    if (strBldrcost.ToString() != "")
        //                    {
                             
                                
        //                        if (userid == "0")
        //                            userid = ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();
        //                        if (userid != "" && userid != "0" && Convert.ToInt32(userid) > 0)
        //                        strBldr.Append(strBldrcost);
        //                    }
        //                    if (EComState.ToUpper() == "YES")
        //                    {
        //                        //Add the Cart Image
        //                        string CartImgPath = string.Empty;
                                
        //                        //ProdID = oHelper.CI(sourceTable.Rows[i - (columnTable.Columns.Count + 1)]["PRODUCT_ID"].ToString());
        //                        if (Restricted.ToUpper() == "YES")
        //                        {
        //                            CartImgPath = objHelperServices.GetOptionValues("RESTRICTED PRODUCT TEXT");
        //                            string CartUrl = objHelperServices.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
        //                            CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
        //                        }
        //                        else
        //                        {
        //                            if (IsAvailable == 1)
        //                            {
        //                                CartImgPath = objHelperServices.GetOptionValues("IMAGE PATH").ToString() + objHelperServices.GetOptionValues("CARTIMGPATH").ToString();

        //                                //Min_ord_qty = oOrder.GetProductMinimumOrderQty(ProdID);
        //                                //string _StockStatus = GetStockStatus(Convert.ToInt32(ProdID.ToString()));
        //                                 _StockStatus = "NO STATUS AVAILABLE";
        //                                 _AvilableQty = "0";
        //                                if (tempPriceDt != null)
        //                                {
        //                                    Min_ord_qty =Convert.ToInt32(tempPriceDt.Rows[0]["MIN_ORD_QTY"].ToString());
        //                                    _StockStatus = tempPriceDt.Rows[0]["PROD_STK_STATUS_DSC"].ToString().Replace("_"," ") ;
        //                                    _AvilableQty = tempPriceDt.Rows[0]["QTY_AVAIL"].ToString();
        //                                }
        //                                string CartUrl = objHelperServices.GetOptionValues("CARTURL").ToString();

        //                                CartUrl = CartUrl.Replace("{PRODUCT_ID}", ProdID.ToString());
        //                                CartUrl = CartUrl.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
        //                                CartImgPath = "<a href=\"" + CartUrl + "\" style=\"text-decoration:none\"><img src=\"/" + CartImgPath + "\" style=\"border-width:0\"></a>";

                                        
                                        
                                        
        //                                string _StockStatusTrim = _StockStatus.Trim();
        //                                bool _Tbt_Stock_Status_2 = false;

        //                                switch (_StockStatusTrim)
        //                                {
        //                                    case "IN STOCK":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "SPECIAL ORDER":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "SPECIAL ORDER PRICE &":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "DISCONTINUED":
        //                                        _Tbt_Stock_Status_2 = false;
        //                                        break;
        //                                    case "DISCONTINUED NO LONGER AVAILABLE":
        //                                        _Tbt_Stock_Status_2 = false;
        //                                        break;
        //                                    case "DISCONTINUED NO LONGER":
        //                                        _Tbt_Stock_Status_2 = false;
        //                                        break;
        //                                    case "TEMPORARY UNAVAILABLE":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "TEMPORARY UNAVAILABLE NO ETA":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "OUT OF STOCK":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    case "OUT OF STOCK ITEM WILL":
        //                                        _Tbt_Stock_Status_2 = true;
        //                                        break;
        //                                    default:
        //                                        _Tbt_Stock_Status_2 = false;
        //                                        break;
        //                                }

        //                                if (_Tbt_Stock_Status_2 == true)
        //                                {
        //                                    //CartImgPath = "<table width=\"100%\" border=\"0\" cellPadding=\"0\" cellSpacing=\"0\"><tr><td>" +
        //                                    //           "<input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" type=\"text\" size=\"1\" id=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;width: 30px;height:21px;\"   /> " +
        //                                    //         "</td><td width=\"2\">&nbsp;</td><td>" +
        //                                    //         "  <a style=\"cursor:pointer;\" valign=\"middle\"  onMouseOut=\"javascript:MM_swapImgRestore();ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onMouseOver=\"javascript:MM_swapImage('Image" + ProdID.ToString() + "_fp','','images/but_buy2.gif',1);ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\">" +
                                                
        //                                    //         "   <img src=\"images/button1.png\" name=\"Image" + ProdID.ToString() + "_fp\" width=\"56\" height=\"26\" border=\"0\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "','" + ProdID.ToString() + "');\"/>" +
        //                                    //         "</a></td></tr></table>";

        //                                    string productid = ProdID.ToString();

        //                                    CartImgPath = "<table width=\"100px\" border=\"0\" cellPadding=\"0\" cellSpacing=\"0\"><tr><td>" +

        //                                              "<div><input valign=\"middle\" name=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" onkeydown=\"return keyct(event)\"  maxlength=\"6\" type=\"text\" size=\"1\" id=\"txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "\" style=\"background-color:#FFFFFF;color: #000000;width: 30px;height:21px;float:left;\"/> " +
        //                                              "<div class=\"costable\"><div class=\"pricepopup\"><div class=\"popupouterdivnone\" id=\"popupouterdiv" + ProdID.ToString() + "\"><div class=\"popupaero\"></div>  " + StrPriceTable + "</div>" +
        //                                              "<a style=\"cursor:pointer;margin: 0 0 0 5px;\" onMouseOut=\"javascript:Mouseout('" + ProdID.ToString() + "');\" onMouseOver=\"javascript:test('" + ProdID.ToString() + "');\"  id=\"" + ProdID.ToString() + "\" valign=\"middle\" class=\"btnbuy2 button smallsiz btngreen costable\"  onClick=\"productbuy('txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "','" + ProdID.ToString() + "');\">Buy  </a></div></div></div></td></tr></table>";

                                          
     

        //                                              /*"  <a style=\"cursor:pointer;margin: 0 0 0 5px;\" valign=\"middle\" class=\"btnbuy2 button smallsiz btngreen costable\" onMouseOut=\"javascript:MM_swapImgRestore();ClosePriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" onClick=\"productbuy('txt" + ProdID.ToString() + "_" + _AvilableQty.ToString() + "_" + Min_ord_qty.ToString() + "_" + _familyID.ToString() + "','" + ProdID.ToString() + "');\" onMouseOver=\"javascript:MM_swapImage('Image" + ProdID.ToString() + "_fp','','',1);ShowPriceTable('pid" + DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString() + "')\" >Buy  </a></td></tr></table>";*/
                                                  
        //                                }
        //                                else
        //                                {
        //                                    CartImgPath = "";
        //                                }

        //                                if (rowcolor == false)
        //                                {

                                            
                                                
        //                                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 120px;"+ CartImgPathdisplay+"\">" + CartImgPath + "</TD>");
        //                                }
        //                                if (rowcolor == true)
        //                                {
        //                                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 120px; " + CartImgPathdisplay + " \">" + CartImgPath + "</TD>");
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (rowcolor == false)
        //                                {
        //                                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"  Class=\"FamilyPageTableCell\"  style=\"width: 200px; " + CartImgPathdisplay + "  \">N/A</TD>");
        //                                }
        //                                if (rowcolor == true)
        //                                {
        //                                    strBldr.Append("<TD ALIGN=\"Center\" VALIGN=\"Middle\"   Class=\"FamilyPageTableCell\" style=\"width: 200px; " + CartImgPathdisplay + " \">N/A</TD>");
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        strBldr.Append("</TR>");
        //    }

        //    strBldr.Append("</TABLE></div></td></tr></table>");
        //    //if (strBldr.ToString().Contains("<TABLE border=0 cellspacing=1 style=\"background-color:black\" cellpadding=3><style>td{font-family:arial Unicode ms;font-size:12px;}th{font-family:arial unicode ms;font-size:12px;font-weight:Bold}</style><TR></TR></TABLE>"))
        //    //{
        //    //    strBldr = strBldr.Remove(0, strBldr.Length);
        //    //}
        //    return strBldr.ToString();
        //}
        public DataSet ConvertXMLToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmlData);
                // Load the XmlTextReader from the stream
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }
        //public string GenerateHorizontalHTMLJson(string _familyID, DataSet Ds, DataSet dsPriceTableAll, DataSet EADs, string Rawurl_dy)
        //{

        //        string rtnstr = string.Empty;
        //        StringTemplateGroup _stg_container = null;
        //        StringTemplateGroup _stg_records = null;

        //        StringTemplate _stmpl_container = null;
        //        StringTemplate _stmpl_records = null;
        //        StringTemplate _stmpl_records_group = null;
        //        StringTemplate _stmpl_records1 = null;

        //        StringTemplate _stmpl_records1_group = null;


        //        //StringTemplate _stmpl_recordsrows = null;
        //        //  TBWDataList[] lstrecords = new TBWDataList[0];
        //        // TBWDataList[] lstrows = new TBWDataList[0];

        //        // StringTemplateGroup _stg_container1 = null;
        //        StringTemplateGroup _stg_records1 = null;
        //        //  TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        //        //  TBWDataList1[] lstrows1 = new TBWDataList1[0];
        //        try
        //        {

        //        DataSet dsBgDisc = new DataSet();
        //        // decimal untPrice = 0;
        //        string AttrID = string.Empty;
        //        //  string HypColumn = "";
        //        //  int Min_ord_qty = 0;
        //        //  int Qty_avail;
        //        //  int flagtemp = 0;
        //        string _StockStatus = "NO STATUS AVAILABLE";
        //        string _prod_stk_Status = "0";
        //        string _prod_stk_flag = "0";
        //        string _AvilableQty = "0";
        //        string _eta = string.Empty;
        //        string _Category_id = string.Empty;
        //        string _EA_Path = string.Empty;
        //        string StrPriceTable = string.Empty;
        //        string CATEGORY_PATH = string.Empty;
        //        DataRow[] tempPriceDr;

        //        //DataTable tempPriceDt;
        //        //int ProdID;
        //        //  int AttrType;
        //        string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //        if (userid == "" || userid == null)
        //            userid = ConfigurationManager.AppSettings["DUM_USER_ID"];
        //        //userid = "999";// ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

        //        // string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
        //        //  string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
        //        string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
        //        string _parentFamily_Id = "0";
        //        // string CartImgPathdisplay = "";
        //        string _ProCode = string.Empty;
        //        string family_name = string.Empty;
        //        if (EComState == "YES")
        //            if (!objHelperServices.GetIsEcomEnabled(userid))
        //                EComState = "NO";
        //        StringBuilder strBldr = new StringBuilder();
        //        StringBuilder strBldrcost = new StringBuilder();

        //        if (HttpContext.Current.Request.QueryString["path"] != null)
        //            _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());

        //        if (HttpContext.Current.Request.QueryString["cid"] != null)
        //            _Category_id = HttpContext.Current.Request.QueryString["cid"];

        //        DsPreview = Ds;
        //        if (DsPreview.Tables[_familyID] == null)
        //            return "";


        //        DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
        //        if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
        //            _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

        //        if (_parentFamily_Id == "0")
        //            _parentFamily_Id = _familyID;

        //        pricecode = GetPriceCode();

        //        if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
        //            family_name = _parentFamilyds.Tables[0].Rows[0]["FAMILY_NAME"].ToString();
        //        else
        //            family_name = HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"].ToString();

        //        string _SkinRootPath = string.Empty;

        //        _SkinRootPath = HttpContext.Current.Server.MapPath("~\\Templates");

        //        _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
        //        _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);

        //        string HTMLAtts = string.Empty;
        //        string HTMLAtts_group = string.Empty;
        //        string HTMLAtts_group_details = string.Empty;
        //        string HTMLProducts = string.Empty;
        //        string HTMLHeaderStr = string.Empty;
        //        string HTMLHeaderStr_group = string.Empty;
        //        // int ictrecords = 0;
        //        string costtype = string.Empty;
        //        HTMLAtts = "";
        //        string DsPreviewcaption = string.Empty;
        //        DataSet dssimilarcolumns = new DataSet();
        //        DataSet dsgetleftattr = new DataSet();
        //        try
        //        {
        //            //if (DsPreview.Tables[0].Rows.Count > 1)
        //            //{
        //            objErrorHandler.CreateLog(_familyID + "------" + _familyID);
        //                DataSet tmpds1 = objHelperDb.GetDataSetDB("exec [STP_GETFAMILY_XML] " + _familyID);
        //                DataSet dsmrgattr = new DataSet();
        //                if (tmpds1 != null && tmpds1.Tables[0].Rows.Count > 0)
        //                {

        //                    dsmrgattr = ConvertXMLToDataSet(tmpds1.Tables[0].Rows[0][0].ToString());

        //                    //DataRow[] dr = dsmrgattr.Tables["LeftRowField"].Select("Merge='Checked'");
        //                    //if (dr.Length > 0)
        //                    //{
        //                    if(dsmrgattr!=null)
        //                    {
        //                      //  DataTable dt = dr.CopyToDataTable();
        //                        DataTable dt = dsmrgattr.Tables["LeftRowField"];
        //                        string x = string.Empty;
        //                        for (int i = 0; i < dt.Rows.Count; i++)
        //                        {
        //                            if (i == 0)
        //                            {
        //                                x = dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");
        //                            }
        //                            else
        //                            {
        //                                x = x + "," + dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");
        //                            }


        //                        }
        //                     //   dssimilarcolumns = objHelperDb.GetDataSetDB("exec [STP_Attribute_name] '" + x + "'");
        //                        objErrorHandler.CreateLog("exec [STP_Attribute_name] '" + x + "'" + "------" + _familyID);
        //                        dsgetleftattr = objHelperDb.GetDataSetDB("exec [STP_Attribute_name] '" + x + "'");
        //                    }

        //                }
        //            //}
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        // stopwatch.Start();
        //        for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //        {
        //            DsPreviewcaption = string.Empty;
        //            DsPreviewcaption = DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper();
        //            if (DsPreviewcaption != "COST" && DsPreviewcaption != "CODE"
        //                && DsPreviewcaption != "TWEB IMAGE1"
        //                && DsPreviewcaption != "PRODUCT_ID"
        //                && DsPreviewcaption != "FAMILY_ID"
        //                )
        //            {

        //                _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProcellHead");
        //                if ((dsgetleftattr != null) && (dsgetleftattr.Tables.Count>0))
        //                {
        //                    if (dsgetleftattr.Tables[0].Rows.Count > 0)
        //                    {
        //                        DataRow[] dr = dsgetleftattr.Tables[0].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].Caption + "'");
        //                        if (dr.Length > 0)
        //                        {
        //                            _stmpl_records.SetAttribute("ATTRIBUTE_NAME", DsPreview.Tables[_familyID].Columns[j].Caption);
        //                            HTMLAtts = HTMLAtts + _stmpl_records.ToString();
        //                        }
        //                    }
        //                }
        //                //if (dssimilarcolumns.Tables.Count == 0 )
        //                //{
        //                //    _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProcellHead");
        //                //    _stmpl_records.SetAttribute("ATTRIBUTE_NAME", DsPreview.Tables[_familyID].Columns[j].Caption);
        //                //    HTMLAtts = HTMLAtts + _stmpl_records.ToString();
        //                //}
        //                //else
        //                //{
        //                //    DataRow[] dr = dssimilarcolumns.Tables[0].Select("Attribute_name='" + DsPreview.Tables[_familyID].Columns[j].Caption + "'");
        //                //    if (dr.Length > 0)
        //                //    {
        //                //        _stmpl_records_group = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Group_ProcellHead");
        //                //        _stmpl_records_group.SetAttribute("GROUP_ATTRIBUTE_NAME", DsPreview.Tables[_familyID].Columns[j].Caption);
        //                //        HTMLAtts_group = HTMLAtts_group + _stmpl_records_group.ToString();

        //                //    }
        //                //    else
        //                //    {
        //                //        _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProcellHead");
        //                //        _stmpl_records.SetAttribute("ATTRIBUTE_NAME", DsPreview.Tables[_familyID].Columns[j].Caption);
        //                //        HTMLAtts = HTMLAtts + _stmpl_records.ToString();

        //                //    }

        //                //}
        //            }
        //            //DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
        //            //if (tempdr.Length > 0 && objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString())!=3)
        //            //{
        //            if (DsPreviewcaption == "COST")
        //            {
        //                if (pricecode == 1)
        //                {
        //                    costtype = " Inc GST";
        //                }
        //                else
        //                {
        //                    costtype = " Ex GST";
        //                }
        //            }

        //            //}

        //        }


        //        if ((dsgetleftattr != null) && (dsgetleftattr.Tables.Count > 0))
        //        {
        //            if (dsgetleftattr.Tables[0].Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dsgetleftattr.Tables[0].Rows.Count; i++)
        //                {

        //                    DsPreviewcaption = dsgetleftattr.Tables[0].Rows[i][0].ToString();
        //                    if (DsPreviewcaption != "COST" && DsPreviewcaption != "CODE"
        //                  && DsPreviewcaption != "TWEB IMAGE1"
        //                  && DsPreviewcaption != "PRODUCT_ID"
        //                  && DsPreviewcaption != "FAMILY_ID"
        //                  )
        //                    {

        //                        _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProcellHead");
        //                        _stmpl_records.SetAttribute("ATTRIBUTE_NAME", DsPreviewcaption);
        //                        HTMLAtts = HTMLAtts + _stmpl_records.ToString();
        //                    }

        //                }
        //            }
        //        }
        //        //stopwatch.Stop();
        //        //objErrorHandler.CreateLog("First For Loop:" + "=" + stopwatch.Elapsed );
        //        _stmpl_records1 = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProrowHead");
        //        _stmpl_records1.SetAttribute("INGST", costtype);
        //        _stmpl_records1.SetAttribute("ATTRIBUTE_HEADR", HTMLAtts);
        //        HTMLHeaderStr = _stmpl_records1.ToString();




        //        bool showheader = true;
        //        if (HttpContext.Current.Session["hfprevfid"] == null)
        //        {
        //            HttpContext.Current.Session["hfprevfid"] = _familyID;
        //        }
        //        else if (HttpContext.Current.Session["hfprevfid"].ToString() != _familyID)
        //        {
        //            HttpContext.Current.Session["hfprevfid"] = _familyID;
        //        }
        //        else
        //        {
        //            showheader = false;
        //        }


        //        //  DisplayHeaders = true;
        //        //if ((showheader))
        //        //{
        //        HTMLProducts = HTMLProducts + HTMLHeaderStr + "<tbody class=\"tablebutton\" id=\"myTable\">";
        //        //}
        //        //else
        //        //{
        //        //    HTMLProducts = HTMLProducts +  "<tbody class=\"tablebutton\" id=\"myTable\">";

        //        //}
        //        string ValueFortag = string.Empty;
        //        //bool rowcolor = false;

        //        if (_EA_Path == string.Empty && _Category_id == string.Empty)
        //        {
        //            DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
        //            if (tmpds != null && tmpds.Tables.Count > 0)
        //            {
        //                _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
        //                string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch=Family Id=" + _familyID.ToString();
        //                _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
        //            }
        //        }

        //        _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
        //        _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);
        //        _stg_container = new StringTemplateGroup("main", _SkinRootPath);
        //        string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();

        //        string prodcodedesc = string.Empty;
        //        string prodedesc = string.Empty;

        //        string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));

        //        //Added on 18 July
        //        string HREFURL = string.Empty;
        //        string conspath = string.Empty;


        //        //if (HttpContext.Current.Request.RawUrl.Contains("xx"))
        //        //{
        //        //    if (_EA_Path == null && _Category_id == "")
        //        //    {
        //        //        DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
        //        //        if (tmpds != null && tmpds.Tables.Count > 0)
        //        //        {
        //        //            _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
        //        //            CATEGORY_PATH = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
        //        //            string eapath = "AllProducts////WESAUSTRALASIA////" + CATEGORY_PATH + "////UserSearch=Family Id=" + _familyID.ToString();
        //        //            _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
        //        //            string[] catpath = CATEGORY_PATH.ToString().Split(new string[] { "////" }, StringSplitOptions.None);
        //        //            CATEGORY_PATH =  (catpath.Length >= 1 ? catpath[0] : " ") + "////" +  (catpath.Length >=2 ? catpath[1] : " ") + "////";
        //        //        }
        //        //    }


        //        //    conspath = WAG_Root_Path + "////" + _parentFamily_Id + "////" + CATEGORY_PATH + "////" + family_name;


        //        //}
        //        //else
        //        //{




        //        // NEWURL = HttpContext.Current.Session["breadcrumEAPATH"].ToString() + "////" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "=" + _ProCode;
        //        string rawurl = string.Empty;

        //        if (Rawurl_dy != "")
        //        {
        //            rawurl = Rawurl_dy;
        //        }
        //        else
        //        {
        //            rawurl = HttpContext.Current.Request.RawUrl.ToString().Replace("/fl/", "");
        //        }
        //        string[] rev = rawurl.Split('/');
        //        Array.Reverse(rev);
        //        if (rev[0].Contains("wa"))
        //        {


        //            conspath = rev[2] + "////" + rev[3] + "////" + rev[4];



        //        }
        //        else
        //        {
        //            conspath = rev[1] + "////" + rev[2] + "////" + rev[3];
        //        }
        //        //}




        //        // stopwatch.Start();

        //        string templatepath = "Csfamilypage\\Prorow";
        //        string tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch=Family Id=" + _parentFamily_Id.ToString()));
        //        for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
        //        {
        //            tempPriceDr = EADs.Tables["FamilyPro"].Select("PRODUCT_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
        //            //if (tempPriceDr.Length > 0)
        //            //    tempPriceDt = tempPriceDr.CopyToDataTable();
        //            //else
        //            //   tempPriceDt = null;

        //            strBldrcost = new StringBuilder();


        //            _stmpl_records1 = _stg_records.GetInstanceOf(templatepath);

        //            _stmpl_records1.SetAttribute("PRODUCT_ID", DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
        //            _stmpl_records1.SetAttribute("FAMILY_ID", DsPreview.Tables[_familyID].Rows[i]["FAMILY_ID"].ToString());


        //            //-------------------------------- cost
        //            _StockStatus = "NO STATUS AVAILABLE";
        //            _prod_stk_Status = "0";
        //            _prod_stk_flag = "0";
        //            string ISSUBSTITUTE = "";
        //            //_stmpl_records1.SetAttribute("COST", Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + "<br/> Price / Stock Status");
        //            // _stmpl_records1.SetAttribute("COST", Prefix + " " + DsPreview.Tables[_familyID].Rows[i]["COST"].ToString() + " " + "<br/> Price / Stock Status");
        //            _stmpl_records1.SetAttribute("COST", Prefix + " " + DsPreview.Tables[_familyID].Rows[i]["COST"].ToString());
        //            if (tempPriceDr != null && tempPriceDr.Length > 0)
        //            {
        //                _StockStatus = tempPriceDr[0]["STOCK_STATUS_DESC"].ToString().Replace("_", " ");
        //                _prod_stk_Status = tempPriceDr[0]["PROD_STOCK_STATUS"].ToString();
        //                _prod_stk_flag = tempPriceDr[0]["PROD_STOCK_FLAG"].ToString();
        //                _AvilableQty = tempPriceDr[0]["QTY_AVAIL"].ToString();
        //                _eta = tempPriceDr[0]["ETA"].ToString();
        //                ISSUBSTITUTE = tempPriceDr[0]["PROD_SUBSTITUTE"].ToString();
        //            }
        //            if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
        //                _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();

        //            //  StrPriceTable = AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _prod_stk_Status, CustomerType, Convert.ToInt32(userid), _prod_stk_flag,_eta, dsPriceTableAll);
        //            // _stmpl_records1.SetAttribute("PRICE_TABLE", StrPriceTable);


        //            _stmpl_records1.SetAttribute("AVIL_QTY", _AvilableQty);
        //            _stmpl_records1.SetAttribute("MIN_ORDER_QTY", 1);




        //            // string ORIGINALURL = "pd.aspx?pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath;


        //            //if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
        //            //{
        //            //    NEWURL = HttpContext.Current.Session["breadcrumEAPATH"].ToString() + "////" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "=" + _ProCode;
        //            //    NEWURL = objHelperServices.Cons_NewURl_bybrand(ORIGINALURL, NEWURL, "pd.aspx", "");
        //            //    HREFURL = "/" + NEWURL + "/pd/";
        //            //}
        //            //else
        //            //{
        //            //    NEWURL = ORIGINALURL;
        //            //    HREFURL = ORIGINALURL;

        //            //}


        //            HREFURL = objHelperServices.SimpleURL_Str(WAG_Root_Path + "////" + _parentFamily_Id + "////" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "=" + _ProCode
        // + "////" + conspath, "pd.aspx", true);




        //            HREFURL = HREFURL + "/pd/";


        //            _stmpl_records1.SetAttribute("PARENT_FAMILY_ID", _parentFamily_Id);
        //            _stmpl_records1.SetAttribute("CAT_ID", _Category_id);
        //            _stmpl_records1.SetAttribute("EA_PATH", tempEAPath);
        //            _stmpl_records1.SetAttribute("URL_RW_PATH", HREFURL);

        //            _stmpl_records1.SetAttribute("BUY_FAMILY_ID", _familyID);
        //            // objHelperServices.SimpleURL_Str(NEWURL, "pd.aspx");
        //       // objErrorHandler.CreateLog(_StockStatus + DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString());
        //            bool _Tbt_Stock_Status_2 = false;
        //            switch (_StockStatus.Trim())
        //            {
        //                case "IN STOCK":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "SPECIAL ORDER":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "SPECIAL ORDER PRICE &":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "DISCONTINUED":
        //                    _Tbt_Stock_Status_2 = false;
        //                    break;
        //                case "DISCONTINUED NO LONGER AVAILABLE":
        //                    _Tbt_Stock_Status_2 = false;
        //                    break;
        //                case "DISCONTINUED NO LONGER":
        //                    _Tbt_Stock_Status_2 = false;
        //                    break;
        //                case "TEMPORARY UNAVAILABLE":
        //                    //   _Tbt_Stock_Status_2 = true;
        //                    //modified by indu Requirement Stock Status update date 7-Apr-2017
        //                    // _Tbt_Stock_Status_2 = true;
        //                    _Tbt_Stock_Status_2 = false;
        //                    break;
        //                case "TEMPORARY UNAVAILABLE NO ETA":
        //                    //modified by indu Requirement Stock Status update date 7-Apr-2017
        //                    //  _Tbt_Stock_Status_2 = true;
        //                    _Tbt_Stock_Status_2 = false;
        //                    break;
        //                case "OUT OF STOCK":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "OUT OF STOCK ITEM WILL":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "Please Call":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "Please_Call":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                default:
        //                    _Tbt_Stock_Status_2 = false;
        //                    break;
        //            }

        //            if ((_Tbt_Stock_Status_2))
        //            {
        //                try
        //                {

        //                    if ((_StockStatus.ToUpper().Contains("OUT OF STOCK") == true || _StockStatus.ToUpper().Contains("PLEASE CALL") == true) && ISSUBSTITUTE.ToString().Trim() != "" && _prod_stk_Status == "0" && _prod_stk_flag.ToString().Trim() == "-2")
        //                    {
        //                        // DataTable rtntbl = objProductServices.GetSubstituteProductDetails(ISSUBSTITUTE.ToString().Trim(), Convert.ToInt32(userid));
        //                        DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_ProCode, Convert.ToInt32(userid), "pd");
        //                        if (rtntbl != null && rtntbl.Rows.Count > 0)
        //                        {

        //                            bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

        //                            bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

        //                            if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
        //                            {

        //                                _stmpl_records1.SetAttribute("TBT_SUB_PRODUCT", true);
        //                                _stmpl_records1.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
        //                                string strurl = rtntbl.Rows[0]["ea_path"].ToString();
        //                                _stmpl_records1.SetAttribute("TBT_REP_EA_PATH", strurl);
        //                            }
        //                        }

        //                    }
        //                    else
        //                    {
        //                        _stmpl_records1.SetAttribute("SHOW_BUY", true);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    objErrorHandler.CreateLog(ex.ToString());
        //                }
        //            }
        //            else
        //            {
        //                try
        //                {
        //                    if (ISSUBSTITUTE.ToString().Trim() != "" && _prod_stk_Status == "0" && _prod_stk_flag.ToString().Trim() == "-2")
        //                    {
        //                        DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_ProCode.ToString().Trim(), Convert.ToInt32(userid), "pd");

        //                        if (rtntbl != null && rtntbl.Rows.Count > 0)
        //                        {

        //                            bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

        //                            bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

        //                            if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
        //                            {


        //                                _stmpl_records1.SetAttribute("TBT_SUB_PRODUCT", true);
        //                                _stmpl_records1.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
        //                                string strurl = rtntbl.Rows[0]["ea_path"].ToString();
        //                                _stmpl_records1.SetAttribute("TBT_REP_EA_PATH", strurl);
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    objErrorHandler.CreateLog(ex.ToString());
        //                }
        //            }


        //            //-------------------------------- Code

        //            _stmpl_records1.SetAttribute("PROD_CODE", DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString());

        //            //-------------------------------- Image

        //            ValueFortag = DsPreview.Tables[_familyID].Rows[i]["TWEB IMAGE1"].ToString();
        //            string ValueLargeImg = string.Empty;
        //            if (ValueFortag != string.Empty && ValueFortag != null && ValueFortag != "noimage.gif")
        //            {
        //                ValueFortag = "/prodimages" + ValueFortag.Replace("\\", "/");
        //                ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200").Replace("\\", "/");
        //                //FileInfo Fil;


        //                //Fil = new FileInfo(strFile + ValueFortag);
        //                //if (Fil.Exists)
        //                //{

        //                //    ValueFortag = "/prodimages" + ValueFortag.Replace("\\", "/");
        //                //    ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200").Replace("\\", "/");
        //                //}
        //                //else
        //                //{
        //                //    ValueFortag = "/prodimages/images/noimage.gif";
        //                //    ValueLargeImg = "";
        //                //}
        //            }
        //            else
        //            {
        //                ValueFortag = "/prodimages/images/noimage.gif";
        //                ValueLargeImg = "";
        //            }
        //            if (!HttpContext.Current.Request.RawUrl.ToLower().Contains("/mfl/"))
        //            {

        //                _stmpl_records1.SetAttribute("TWEB_LargeImg", ValueLargeImg);
        //            }

        //            _stmpl_records1.SetAttribute("TWEB_Image", ValueFortag);
        //            if ((ValueLargeImg != "") && (!HttpContext.Current.Request.RawUrl.ToLower().Contains("/mfl/")))
        //            {
        //                _stmpl_records1.SetAttribute("SHOW_DIV", true);
        //            }
        //            else
        //            {
        //                _stmpl_records1.SetAttribute("SHOW_DIV", false);
        //            }


        //            //  if( HttpContext.Current.Session["F_ALT_FNAME"].ToString() != "")
        //            //      _stmpl_records1.SetAttribute("F_ALT_FNAME", HttpContext.Current.Session["F_ALT_FNAME"].ToString());

        //            HTMLAtts = "";

        //            bool flgdescchk = false;
        //            bool flgdeschk_bbpp = false;
        //            string dsprecapcol = string.Empty;
        //            //stopwatch.Start();
        //            string teplatepath = "Csfamilypage\\Procell";
        //            string templatepathgroup = "Csfamilypage\\Group_Procell";

        //            for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //            {
        //                dsprecapcol = string.Empty;
        //                dsprecapcol = DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper();
        //                if (dsprecapcol != "COST"
        //                    && dsprecapcol != "CODE"
        //                    && dsprecapcol != "TWEB IMAGE1"
        //                    && dsprecapcol != "PRODUCT_ID"
        //                    && dsprecapcol != "FAMILY_ID"
        //                    )
        //                {
        //                    _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProcellHead");
        //                    if ((dsgetleftattr != null) && (dsgetleftattr.Tables.Count > 0))
        //                    {
        //                        if (dsgetleftattr.Tables[0].Rows.Count > 0)
        //                        {
        //                            DataRow[] dr = dsgetleftattr.Tables[0].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].Caption + "'");
        //                            if (dr.Length > 0)
        //                            {
        //                                _stmpl_records = _stg_records.GetInstanceOf(teplatepath);
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_TITLE", DsPreview.Tables[_familyID].Columns[j].Caption);

        //                                HTMLAtts = HTMLAtts + _stmpl_records.ToString();
        //                                if (dsprecapcol == "DESCRIPTION")
        //                                {
        //                                    _stmpl_records1.SetAttribute("PROD_DESCRIPTION", DsPreview.Tables[_familyID].Rows[i]["DESCRIPTION"].ToString());
        //                                    flgdescchk = true;
        //                                }
        //                                if (dsprecapcol == "DESCRIPTION" && DsPreview.Tables[_familyID].Rows[i][j].ToString() != string.Empty)
        //                                {
        //                                    prodedesc = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                                    flgdeschk_bbpp = true;
        //                                }
        //                            }
        //                        }
        //                    }
        //                    //Family Grouping
        //                  //if (  dssimilarcolumns.Tables.Count == 0 )
        //                  //  {
        //                  //      _stmpl_records = _stg_records.GetInstanceOf(teplatepath);
        //                  //      _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
        //                  //      _stmpl_records.SetAttribute("ATTRIBUTE_TITLE", DsPreview.Tables[_familyID].Columns[j].Caption);

        //                  //      HTMLAtts = HTMLAtts + _stmpl_records.ToString();
        //                  //      if (dsprecapcol == "DESCRIPTION")
        //                  //      {
        //                  //          _stmpl_records1.SetAttribute("PROD_DESCRIPTION", DsPreview.Tables[_familyID].Rows[i]["DESCRIPTION"].ToString());
        //                  //          flgdescchk = true;
        //                  //      }
        //                  //      if (dsprecapcol == "DESCRIPTION" && DsPreview.Tables[_familyID].Rows[i][j].ToString() != string.Empty)
        //                  //      {
        //                  //          prodedesc = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                  //          flgdeschk_bbpp = true;
        //                  //      }
        //                  //  }
        //                  //  else
        //                  //  {
        //                  //      DataRow[] dr = dssimilarcolumns.Tables[0].Select("Attribute_name='" + DsPreview.Tables[_familyID].Columns[j].Caption + "'");
        //                  //      if (dr.Length > 0)
        //                  //      {

        //                  //          if (i==0)
        //                  //          {
        //                  //              _stmpl_records_group = _stg_records.GetInstanceOf(templatepathgroup);
        //                  //              _stmpl_records_group.SetAttribute("GROUP_ATTRIBUTE_VALUE", DsPreview.Tables[_familyID].Rows[i][j].ToString());
        //                  //              HTMLAtts_group_details = HTMLAtts_group_details + _stmpl_records_group.ToString();
        //                  //          }

        //                  //      }
        //                  //      else
        //                  //      {
        //                  //          _stmpl_records = _stg_records.GetInstanceOf(teplatepath);
        //                  //          _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
        //                  //          _stmpl_records.SetAttribute("ATTRIBUTE_TITLE", DsPreview.Tables[_familyID].Columns[j].Caption);
        //                  //          HTMLAtts = HTMLAtts + _stmpl_records.ToString();
        //                  //          if (dsprecapcol == "DESCRIPTION")
        //                  //          {
        //                  //              _stmpl_records1.SetAttribute("PROD_DESCRIPTION", DsPreview.Tables[_familyID].Rows[i]["DESCRIPTION"].ToString());
        //                  //              flgdescchk = true;
        //                  //          }
        //                  //          if (dsprecapcol == "DESCRIPTION" && DsPreview.Tables[_familyID].Rows[i][j].ToString() != string.Empty)
        //                  //          {
        //                  //              prodedesc = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                  //              flgdeschk_bbpp = true;
        //                  //          }

        //                  //      }

        //                  //  }


        //                }

        //            }



        //            if ((dsgetleftattr != null) && (dsgetleftattr.Tables.Count > 0))
        //            {
        //                if (dsgetleftattr.Tables[0].Rows.Count > 0)
        //                {
        //                    for (int K = 0; K < dsgetleftattr.Tables[0].Rows.Count; K++)
        //                    {
        //                        if (dsprecapcol != "COST"
        //                                                   && dsprecapcol != "CODE"
        //                                                   && dsprecapcol != "TWEB IMAGE1"
        //                                                   && dsprecapcol != "PRODUCT_ID"
        //                                                   && dsprecapcol != "FAMILY_ID"
        //                                                   )
        //                        {
        //                            _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProcellHead");

        //                            //DataRow[] dr = DsPreview.Tables[_familyID].Tables[0].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].Caption + "'");
        //                            //if (dr.Length > 0)
        //                            //{
        //                                _stmpl_records = _stg_records.GetInstanceOf(teplatepath);
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", DsPreview.Tables[_familyID].Rows[i]["dsprecapcol"].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
        //                                _stmpl_records.SetAttribute("ATTRIBUTE_TITLE", dsprecapcol);

        //                                HTMLAtts = HTMLAtts + _stmpl_records.ToString();
        //                                //if (dsprecapcol == "DESCRIPTION")
        //                                //{
        //                                //    _stmpl_records1.SetAttribute("PROD_DESCRIPTION", DsPreview.Tables[_familyID].Rows[i]["DESCRIPTION"].ToString());
        //                                //    flgdescchk = true;
        //                                //}
        //                                //if (dsprecapcol == "DESCRIPTION" && DsPreview.Tables[_familyID].Rows[i][j].ToString() != string.Empty)
        //                                //{
        //                                //    prodedesc = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                                //    flgdeschk_bbpp = true;
        //                                //}
        //                            //}


        //                        }

        //                    }
        //                }
        //            }
        //            //if (HTMLAtts_group != "")
        //            //{
        //            //    _stmpl_records1_group = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Group_row");

        //            //    _stmpl_records1_group.SetAttribute("GROUPHEADER", HTMLAtts_group);
        //            //    _stmpl_records1_group.SetAttribute("GROUPDETAILS", HTMLAtts_group_details);
        //            //    HTMLHeaderStr_group = _stmpl_records1_group.ToString();


        //            //}
        //            //stopwatch.Stop();

        //            //objErrorHandler.CreateLog("For description" + "=" + stopwatch.Elapsed );

        //            if (!(flgdescchk))
        //                _stmpl_records1.SetAttribute("PROD_DESCRIPTION", family_name.Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""));


        //            if (!(flgdeschk_bbpp))
        //            {
        //                if (tempPriceDr != null && tempPriceDr.Length > 0)
        //                    prodedesc = tempPriceDr[0]["PRODUCT_DESC"].ToString();
        //            }

        //            _stmpl_records1.SetAttribute("ATTRIBUTE_VALUES", HTMLAtts);


        //            HTMLProducts = HTMLProducts + _stmpl_records1.ToString();

        //            if (prodedesc.Length > 0)
        //                prodcodedesc = prodcodedesc + _ProCode + " – " + prodedesc + "|";
        //            else
        //                prodcodedesc = prodcodedesc + _ProCode + "|";




        //        }
        //        //stopwatch.Stop();
        //        //objErrorHandler.CreateLog("Second For Loop:" + "=" + stopwatch.Elapsed);
        //        HttpContext.Current.Session["prodcodedesc"] = prodcodedesc;


        //        if ((showheader == true) || (   HttpContext.Current.Request.Browser.IsMobileDevice ==true) )
        //        {
        //            _stmpl_container = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Promain");

        //            //if (Convert.ToInt16(HttpContext.Current.Session[_familyID + "Icnt"].ToString()) == 1)
        //            //{
        //            if (HTMLHeaderStr_group != "")
        //            {
        //                _stmpl_container.SetAttribute("IS_COMMON_VALUE", HTMLHeaderStr_group);
        //                _stmpl_container.SetAttribute("PRODUCT_DETAILS_HEAD", HTMLHeaderStr_group);
        //            }

        //            _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts + " </tbody>");
        //            //}
        //            //else
        //            //{
        //            //    _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts );
        //            //}
        //        }
        //        else
        //        {

        //            _stmpl_container = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Promain_dyn");
        //            _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts.Replace("mythead", "thead_none") + " </tbody>");
        //        }
        //    }
        //    catch (Exception ex)

        //    {
        //        objErrorHandler.CreateLog("GenerateHorizontalHTMLJson---"+ex.ToString());

        //    }

        //    return _stmpl_container.ToString();

        //}

        //public string GenerateHorizontalHTMLJson(string _familyID, DataSet Ds, DataSet dsPriceTableAll, DataSet EADs, string Rawurl_dy)
        //{

        //    string rtnstr = string.Empty;
        //    StringTemplateGroup _stg_container = null;
        //    StringTemplateGroup _stg_records = null;

        //    StringTemplate _stmpl_container = null;
        //    StringTemplate _stmpl_records = null;
        //    StringTemplate _stmpl_records_group = null;
        //    StringTemplate _stmpl_records1 = null;

        //    StringTemplate _stmpl_records1_group = null;


        //    //StringTemplate _stmpl_recordsrows = null;
        //    //  TBWDataList[] lstrecords = new TBWDataList[0];
        //    // TBWDataList[] lstrows = new TBWDataList[0];

        //    // StringTemplateGroup _stg_container1 = null;
        //    StringTemplateGroup _stg_records1 = null;
        //    //  TBWDataList1[] lstrecords1 = new TBWDataList1[0];
        //    //  TBWDataList1[] lstrows1 = new TBWDataList1[0];
        //    try
        //    {

        //        DataSet dsBgDisc = new DataSet();
        //        // decimal untPrice = 0;
        //        string AttrID = string.Empty;
        //        //  string HypColumn = "";
        //        //  int Min_ord_qty = 0;
        //        //  int Qty_avail;
        //        //  int flagtemp = 0;
        //        string _StockStatus = "NO STATUS AVAILABLE";
        //        string _prod_stk_Status = "0";
        //        string _prod_stk_flag = "0";
        //        string _AvilableQty = "0";
        //        string _eta = string.Empty;
        //        string _Category_id = string.Empty;
        //        string _EA_Path = string.Empty;
        //        string StrPriceTable = string.Empty;
        //        string CATEGORY_PATH = string.Empty;
        //        DataRow[] tempPriceDr;

        //        //DataTable tempPriceDt;
        //        //int ProdID;
        //        //  int AttrType;
        //        string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //        if (userid == "" || userid == null)
        //            userid = ConfigurationManager.AppSettings["DUM_USER_ID"];
        //        //userid = "999";// ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

        //        // string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
        //        //  string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
        //        string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
        //        string _parentFamily_Id = "0";
        //        // string CartImgPathdisplay = "";
        //        string _ProCode = string.Empty;
        //        string family_name = string.Empty;
        //        if (EComState == "YES")
        //            if (!objHelperServices.GetIsEcomEnabled(userid))
        //                EComState = "NO";
        //        StringBuilder strBldr = new StringBuilder();
        //        StringBuilder strBldrcost = new StringBuilder();

        //        if (HttpContext.Current.Request.QueryString["path"] != null)
        //            _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());

        //        if (HttpContext.Current.Request.QueryString["cid"] != null)
        //            _Category_id = HttpContext.Current.Request.QueryString["cid"];

        //        DsPreview = Ds;
        //        if (DsPreview.Tables[_familyID] == null)
        //            return "";


        //        DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
        //        if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
        //            _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

        //        if (_parentFamily_Id == "0")
        //            _parentFamily_Id = _familyID;

        //        pricecode = GetPriceCode();

        //        if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
        //            family_name = _parentFamilyds.Tables[0].Rows[0]["FAMILY_NAME"].ToString();
        //        else
        //            family_name = HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"].ToString();

        //        string _SkinRootPath = string.Empty;

        //        _SkinRootPath = HttpContext.Current.Server.MapPath("~\\Templates");

        //        _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
        //        _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);

        //        string HTMLAtts = string.Empty;
        //        string HTMLAtts_group = string.Empty;
        //        string HTMLAtts_group_details = string.Empty;
        //        string HTMLProducts = string.Empty;
        //        string HTMLHeaderStr = string.Empty;
        //        string HTMLHeaderStr_group = string.Empty;
        //      bool  BindToST = true;
        //        // int ictrecords = 0;
        //        string costtype = string.Empty;
        //        HTMLAtts = "";
        //        string DsPreviewcaption = string.Empty;
        //        DataSet dssimilarcolumns = new DataSet();
        //        //try
        //        //{
        //        //    if (DsPreview.Tables[0].Rows.Count > 1)
        //        //    {
        //        //        DataSet tmpds1 = objHelperDb.GetDataSetDB("exec [STP_GETFAMILY_XML] " + _familyID);
        //        //        DataSet dsmrgattr = new DataSet();
        //        //        if (tmpds1 != null && tmpds1.Tables[0].Rows.Count > 0)
        //        //        {

        //        //            dsmrgattr = ConvertXMLToDataSet(tmpds1.Tables[0].Rows[0][0].ToString());

        //        //            DataRow[] dr = dsmrgattr.Tables["LeftRowField"].Select("Merge='Checked'");
        //        //            if (dr.Length > 0)
        //        //            {
        //        //                DataTable dt = dr.CopyToDataTable();
        //        //                string x = string.Empty;
        //        //                for (int i = 0; i < dt.Rows.Count; i++)
        //        //                {
        //        //                    if (i == 0)
        //        //                    {
        //        //                        x = dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");
        //        //                    }
        //        //                    else
        //        //                    {
        //        //                        x = x + "," + dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");
        //        //                    }


        //        //                }
        //        //                dssimilarcolumns = objHelperDb.GetDataSetDB("exec [STP_Attribute_name] '" + x + "'");
        //        //            }

        //        //        }
        //        //    }
        //        //}
        //        //    catch ( Exception ex)
        //        //{

        //        //    }
        //        // stopwatch.Start();
        //        for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //        {
        //            DsPreviewcaption = string.Empty;
        //            DsPreviewcaption = DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper();
        //            if (DsPreviewcaption != "COST" && DsPreviewcaption != "CODE"
        //                && DsPreviewcaption != "TWEB IMAGE1"
        //                && DsPreviewcaption != "PRODUCT_ID"
        //                && DsPreviewcaption != "FAMILY_ID"
        //                )
        //            {


        //                if (dssimilarcolumns.Tables.Count == 0)
        //                {
        //                    _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProcellHead");
        //                    _stmpl_records.SetAttribute("ATTRIBUTE_NAME", DsPreview.Tables[_familyID].Columns[j].Caption);
        //                    HTMLAtts = HTMLAtts + _stmpl_records.ToString();
        //                }
        //                else
        //                {
        //                    DataRow[] dr = dssimilarcolumns.Tables[0].Select("Attribute_name='" + DsPreview.Tables[_familyID].Columns[j].Caption + "'");
        //                    if (dr.Length > 0)
        //                    {
        //                        _stmpl_records_group = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Group_ProcellHead");
        //                        _stmpl_records_group.SetAttribute("GROUP_ATTRIBUTE_NAME", DsPreview.Tables[_familyID].Columns[j].Caption);
        //                        HTMLAtts_group = HTMLAtts_group + _stmpl_records_group.ToString();

        //                    }
        //                    else
        //                    {
        //                        _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProcellHead");
        //                        _stmpl_records.SetAttribute("ATTRIBUTE_NAME", DsPreview.Tables[_familyID].Columns[j].Caption);
        //                        HTMLAtts = HTMLAtts + _stmpl_records.ToString();

        //                    }

        //                }
        //            }
        //            //DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
        //            //if (tempdr.Length > 0 && objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString())!=3)
        //            //{
        //            if (DsPreviewcaption == "COST")
        //            {
        //                if (pricecode == 1)
        //                {
        //                    costtype = " Inc GST";
        //                }
        //                else
        //                {
        //                    costtype = " Ex GST";
        //                }
        //            }

        //            //}

        //        }
        //        //stopwatch.Stop();
        //        //objErrorHandler.CreateLog("First For Loop:" + "=" + stopwatch.Elapsed );
        //        _stmpl_records1 = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProrowHead");
        //        _stmpl_records1.SetAttribute("INGST", costtype);
        //        _stmpl_records1.SetAttribute("ATTRIBUTE_HEADR", HTMLAtts);
        //        HTMLHeaderStr = _stmpl_records1.ToString();




        //        bool showheader = true;
        //        if (HttpContext.Current.Session["hfprevfid"] == null)
        //        {
        //            HttpContext.Current.Session["hfprevfid"] = _familyID;
        //        }
        //        else if (HttpContext.Current.Session["hfprevfid"].ToString() != _familyID)
        //        {
        //            HttpContext.Current.Session["hfprevfid"] = _familyID;
        //        }
        //        else
        //        {
        //            showheader = false;
        //        }


        //        //  DisplayHeaders = true;
        //        //if ((showheader))
        //        //{
        //        HTMLProducts = HTMLProducts + HTMLHeaderStr + "<tbody class=\"tablebutton\" id=\"myTable\">";
        //        //}
        //        //else
        //        //{
        //        //    HTMLProducts = HTMLProducts +  "<tbody class=\"tablebutton\" id=\"myTable\">";

        //        //}
        //        string ValueFortag = string.Empty;
        //        //bool rowcolor = false;

        //        if (_EA_Path == string.Empty && _Category_id == string.Empty)
        //        {
        //            DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
        //            if (tmpds != null && tmpds.Tables.Count > 0)
        //            {
        //                _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
        //                string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch=Family Id=" + _familyID.ToString();
        //                _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
        //            }
        //        }

        //        _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
        //        _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);
        //        _stg_container = new StringTemplateGroup("main", _SkinRootPath);
        //        string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();

        //        string prodcodedesc = string.Empty;
        //        string prodedesc = string.Empty;

        //        string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));

        //        //Added on 18 July
        //        string HREFURL = string.Empty;
        //        string conspath = string.Empty;


        //        //if (HttpContext.Current.Request.RawUrl.Contains("xx"))
        //        //{
        //        //    if (_EA_Path == null && _Category_id == "")
        //        //    {
        //        //        DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
        //        //        if (tmpds != null && tmpds.Tables.Count > 0)
        //        //        {
        //        //            _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
        //        //            CATEGORY_PATH = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
        //        //            string eapath = "AllProducts////WESAUSTRALASIA////" + CATEGORY_PATH + "////UserSearch=Family Id=" + _familyID.ToString();
        //        //            _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
        //        //            string[] catpath = CATEGORY_PATH.ToString().Split(new string[] { "////" }, StringSplitOptions.None);
        //        //            CATEGORY_PATH =  (catpath.Length >= 1 ? catpath[0] : " ") + "////" +  (catpath.Length >=2 ? catpath[1] : " ") + "////";
        //        //        }
        //        //    }


        //        //    conspath = WAG_Root_Path + "////" + _parentFamily_Id + "////" + CATEGORY_PATH + "////" + family_name;


        //        //}
        //        //else
        //        //{




        //        // NEWURL = HttpContext.Current.Session["breadcrumEAPATH"].ToString() + "////" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "=" + _ProCode;
        //        string rawurl = string.Empty;

        //        if (Rawurl_dy != "")
        //        {
        //            rawurl = Rawurl_dy;
        //        }
        //        else
        //        {
        //            rawurl = HttpContext.Current.Request.RawUrl.ToString().Replace("/fl/", "");
        //        }
        //        string[] rev = rawurl.Split('/');
        //        Array.Reverse(rev);
        //        if (rev[0].Contains("wa"))
        //        {


        //            conspath = rev[2] + "////" + rev[3] + "////" + rev[4];



        //        }
        //        else
        //        {
        //            conspath = rev[1] + "////" + rev[2] + "////" + rev[3];
        //        }
        //        //}




        //        // stopwatch.Start();

        //        string templatepath = "Csfamilypage\\Prorow";
        //        string tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch=Family Id=" + _parentFamily_Id.ToString()));
        //        for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
        //        {
        //            tempPriceDr = EADs.Tables["FamilyPro"].Select("PRODUCT_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
        //            //if (tempPriceDr.Length > 0)
        //            //    tempPriceDt = tempPriceDr.CopyToDataTable();
        //            //else
        //            //   tempPriceDt = null;

        //            strBldrcost = new StringBuilder();


        //            _stmpl_records1 = _stg_records.GetInstanceOf(templatepath);

        //            _stmpl_records1.SetAttribute("PRODUCT_ID", DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
        //            _stmpl_records1.SetAttribute("FAMILY_ID", DsPreview.Tables[_familyID].Rows[i]["FAMILY_ID"].ToString());


        //            //-------------------------------- cost
        //            _StockStatus = "NO STATUS AVAILABLE";
        //            _prod_stk_Status = "0";
        //            _prod_stk_flag = "0";
        //            string ISSUBSTITUTE = "";
        //            //_stmpl_records1.SetAttribute("COST", Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + "<br/> Price / Stock Status");
        //            // _stmpl_records1.SetAttribute("COST", Prefix + " " + DsPreview.Tables[_familyID].Rows[i]["COST"].ToString() + " " + "<br/> Price / Stock Status");
        //            _stmpl_records1.SetAttribute("COST", Prefix + " " + DsPreview.Tables[_familyID].Rows[i]["COST"].ToString());
        //            if (tempPriceDr != null && tempPriceDr.Length > 0)
        //            {
        //                _StockStatus = tempPriceDr[0]["STOCK_STATUS_DESC"].ToString().Replace("_", " ");
        //                _prod_stk_Status = tempPriceDr[0]["PROD_STOCK_STATUS"].ToString();
        //                _prod_stk_flag = tempPriceDr[0]["PROD_STOCK_FLAG"].ToString();
        //                _AvilableQty = tempPriceDr[0]["QTY_AVAIL"].ToString();
        //                _eta = tempPriceDr[0]["ETA"].ToString();
        //                ISSUBSTITUTE = tempPriceDr[0]["PROD_SUBSTITUTE"].ToString();
        //            }
        //            if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
        //                _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();

        //            //  StrPriceTable = AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _prod_stk_Status, CustomerType, Convert.ToInt32(userid), _prod_stk_flag,_eta, dsPriceTableAll);
        //            // _stmpl_records1.SetAttribute("PRICE_TABLE", StrPriceTable);


        //            _stmpl_records1.SetAttribute("AVIL_QTY", _AvilableQty);
        //            _stmpl_records1.SetAttribute("MIN_ORDER_QTY", 1);




        //            // string ORIGINALURL = "pd.aspx?pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath;


        //            //if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
        //            //{
        //            //    NEWURL = HttpContext.Current.Session["breadcrumEAPATH"].ToString() + "////" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "=" + _ProCode;
        //            //    NEWURL = objHelperServices.Cons_NewURl_bybrand(ORIGINALURL, NEWURL, "pd.aspx", "");
        //            //    HREFURL = "/" + NEWURL + "/pd/";
        //            //}
        //            //else
        //            //{http://schema.org/Product
        //            //    NEWURL = ORIGINALURL;
        //            //    HREFURL = ORIGINALURL;

        //            //}

        //            //objErrorHandler.CreateLog("_procode="+_ProCode);
        //            if ((_ProCode == "") || (_ProCode ==string.Empty))

        //            {
        //                _ProCode = DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString();
        //            }
        //            //objErrorHandler.CreateLog(DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString());
        //            //objErrorHandler.CreateLog("_procode1=" + _ProCode);
        //            HREFURL = objHelperServices.SimpleURL_Str(WAG_Root_Path + "////" + _parentFamily_Id + "////" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "=" + _ProCode
        // + "////" + conspath, "pd.aspx", true);




        //            HREFURL = HREFURL + "/pd/";


        //            _stmpl_records1.SetAttribute("PARENT_FAMILY_ID", _parentFamily_Id);
        //            _stmpl_records1.SetAttribute("CAT_ID", _Category_id);
        //            _stmpl_records1.SetAttribute("EA_PATH", tempEAPath);
        //            _stmpl_records1.SetAttribute("URL_RW_PATH", HREFURL);

        //            _stmpl_records1.SetAttribute("BUY_FAMILY_ID", _familyID);
        //            // objHelperServices.SimpleURL_Str(NEWURL, "pd.aspx");
        //         //objErrorHandler.CreateLog(_StockStatus + DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString());
        //            bool _Tbt_Stock_Status_2 = false;
        //           // objErrorHandler.CreateLog(_StockStatus.Trim());
        //            switch (_StockStatus.Trim())
        //            {
        //                case "IN STOCK":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "Limited Stock, Please Call":

        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "SPECIAL ORDER":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "SPECIAL ORDER PRICE &":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "DISCONTINUED":
        //                    _Tbt_Stock_Status_2 = false;
        //                    break;
        //                case "DISCONTINUED NO LONGER AVAILABLE":
        //                    _Tbt_Stock_Status_2 = false;
        //                    break;
        //                case "DISCONTINUED NO LONGER":
        //                    _Tbt_Stock_Status_2 = false;
        //                    break;
        //                case "TEMPORARY UNAVAILABLE":
        //                    //   _Tbt_Stock_Status_2 = true;
        //                    //modified by indu Requirement Stock Status update date 7-Apr-2017
        //                    // _Tbt_Stock_Status_2 = true;
        //                    _Tbt_Stock_Status_2 = false;
        //                    break;
        //                case "TEMPORARY UNAVAILABLE NO ETA":
        //                    //modified by indu Requirement Stock Status update date 7-Apr-2017
        //                    //  _Tbt_Stock_Status_2 = true;
        //                    _Tbt_Stock_Status_2 = false;
        //                    break;
        //                case "OUT OF STOCK":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "OUT OF STOCK ITEM WILL":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "Please Call":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;
        //                case "Please_Call":
        //                    _Tbt_Stock_Status_2 = true;
        //                    break;

        //                default:
        //                    _Tbt_Stock_Status_2 = false;
        //                    break;
        //            }
        //            BindToST = true;
        //            //objErrorHandler.CreateLog("_Tbt_Stock_Status_2" + _Tbt_Stock_Status_2 + _ProCode);
        //            if ((_Tbt_Stock_Status_2))
        //            {
        //                try
        //                {

        //                    if ((_StockStatus.ToUpper().Contains("OUT OF STOCK") == true || _StockStatus.ToUpper().Contains("PLEASE CALL") == true) && ISSUBSTITUTE.ToString().Trim() != "" && _prod_stk_Status == "0" && _prod_stk_flag.ToString().Trim() == "-2")
        //                    {
        //                        // DataTable rtntbl = objProductServices.GetSubstituteProductDetails(ISSUBSTITUTE.ToString().Trim(), Convert.ToInt32(userid));
        //                        DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_ProCode, Convert.ToInt32(userid), "pd");
        //                        if (rtntbl != null && rtntbl.Rows.Count > 0)
        //                        {

        //                            bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

        //                            bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

        //                            if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
        //                            {

        //                                _stmpl_records1.SetAttribute("TBT_SUB_PRODUCT", true);
        //                                _stmpl_records1.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
        //                                string strurl = rtntbl.Rows[0]["ea_path"].ToString();
        //                                _stmpl_records1.SetAttribute("TBT_REP_EA_PATH", strurl.Replace("//", "/"));
        //                            }
        //                        }

        //                    }
        //                    else
        //                    {
        //                        _stmpl_records1.SetAttribute("SHOW_BUY", true);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    objErrorHandler.CreateLog(ex.ToString());
        //                }
        //            }
        //            else
        //            {
        //                try
        //                {
        //              //   objErrorHandler.CreateLog("ISSUBSTITUTE" + "= " + ISSUBSTITUTE + "_prod_stk_Status" + _prod_stk_Status + "_prod_stk_flag" + _prod_stk_flag + ProdID);
        //                   //&& _prod_stk_Status == "0" modified by indu on apr-5-2018
        //                    if (ISSUBSTITUTE.ToString().Trim() != ""  && _prod_stk_flag.ToString().Trim() == "-2")
        //                    {
        //                        DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_ProCode.ToString().Trim(), Convert.ToInt32(userid), "pd");

        //                        if (rtntbl != null && rtntbl.Rows.Count > 0)
        //                        {

        //                            bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

        //                            bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

        //                            objErrorHandler.CreateLog("samecodenotFound" + samecodenotFound);
        //                            objErrorHandler.CreateLog("ea_path" + rtntbl.Rows[0]["ea_path"].ToString());
        //                            if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")

        //                            {
        //                                BindToST = true;
        //                                if (_StockStatus.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
        //                                {
        //                                    // objErrorHandler.CreateLog("inside substitute prod" + rtntbl.Rows[0]["SubstuyutePid"].ToString());
        //                                    HelperDB objhelperDb = new HelperDB();
        //                                    DataSet Sqltbs = objhelperDb.GetProductPriceEA("", rtntbl.Rows[0]["SubstuyutePid"].ToString(), userid);
        //                                    if (Sqltbs != null)
        //                                    {

        //                                        string stockstaus = Sqltbs.Tables[0].Rows[0]["PROD_STK_STATUS_DSC"].ToString().Trim().ToUpper();

        //                                        if (stockstaus == "DISCONTINUED NO LONGER AVAILABLE")
        //                                        {

        //                                            BindToST = false;
        //                                        }
        //                                        else
        //                                        {
        //                                            BindToST = true;
        //                                        }
        //                                    }
        //                                }
        //                                _stmpl_records1.SetAttribute("TBT_SUB_PRODUCT", true);
        //                                _stmpl_records1.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
        //                                string strurl = rtntbl.Rows[0]["ea_path"].ToString().Replace("//", "/");
        //                                _stmpl_records1.SetAttribute("TBT_REP_EA_PATH", strurl.Replace("//", "/"));
        //                            }
        //                            else if (_StockStatus.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
        //                            {
        //                                BindToST = false;
        //                            }
        //                            else if (_StockStatus.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA" || _StockStatus.ToUpper() == "TEMPORARY_UNAVAILABLE NO ETA")
        //                            {
        //                                _stmpl_records1.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable. Please Call");
        //                            }
        //                        }
        //                    }

        //                    else
        //                    {

        //                        if ((_StockStatus.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE" || _StockStatus.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA" || _StockStatus.ToUpper() == "TEMPORARY_UNAVAILABLE NO ETA"))
        //                        {
        //                            if (_StockStatus.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
        //                            {
        //                                _stmpl_records1.SetAttribute("PRODUCT_STATUS", "Product Discontinued");
        //                                BindToST = false;
        //                                //objErrorHandler.CreateLog(ProdID + "Product Discontinued" +"Familypage"); 
        //                            }
        //                            else if (_StockStatus.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA" || _StockStatus.ToUpper() == "TEMPORARY_UNAVAILABLE NO ETA")
        //                            {
        //                                _stmpl_records1.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable. Please Call");
        //                                BindToST = true;
        //                            }
        //                            _stmpl_records1.SetAttribute("TBT_HIDE_BUY", true);
        //                        }


        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    objErrorHandler.CreateLog(ex.ToString());
        //                }
        //            }







        //            //-------------------------------- Code

        //            _stmpl_records1.SetAttribute("PROD_CODE", DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString());

        //            //-------------------------------- Image

        //            ValueFortag = DsPreview.Tables[_familyID].Rows[i]["TWEB IMAGE1"].ToString();
        //            string ValueLargeImg = string.Empty;
        //            if (ValueFortag != string.Empty && ValueFortag != null && ValueFortag != "noimage.gif")
        //            {
        //                ValueFortag = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + ValueFortag.Replace("\\", "/");
        //                ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200").Replace("\\", "/");
        //                //FileInfo Fil;


        //                //Fil = new FileInfo(strFile + ValueFortag);ob
        //                //if (Fil.Exists)
        //                //{

        //                //    ValueFortag = "/prodimages" + ValueFortag.Replace("\\", "/");
        //                //    ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200").Replace("\\", "/");
        //                //}
        //                //else
        //                //{
        //                //    ValueFortag = "/prodimages/images/noimage.gif";
        //                //    ValueLargeImg = "";
        //                //}
        //            }
        //            else
        //            {
        //                ValueFortag = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages/images/noimage.gif";
        //                ValueLargeImg = "";
        //            }
        //            if (!HttpContext.Current.Request.RawUrl.ToLower().Contains("/mfl/"))
        //            {

        //                _stmpl_records1.SetAttribute("TWEB_LargeImg", ValueLargeImg);
        //            }

        //            _stmpl_records1.SetAttribute("TWEB_Image", ValueFortag);
        //            if ((ValueLargeImg != "") && (!HttpContext.Current.Request.RawUrl.ToLower().Contains("/mfl/")))
        //            {
        //                _stmpl_records1.SetAttribute("SHOW_DIV", true);
        //            }
        //            else
        //            {
        //                _stmpl_records1.SetAttribute("SHOW_DIV", false);
        //            }


        //            //  if( HttpContext.Current.Session["F_ALT_FNAME"].ToString() != "")
        //            //      _stmpl_records1.SetAttribute("F_ALT_FNAME", HttpContext.Current.Session["F_ALT_FNAME"].ToString());

        //            HTMLAtts = "";

        //            bool flgdescchk = false;
        //            bool flgdeschk_bbpp = false;
        //            string dsprecapcol = string.Empty;
        //            //stopwatch.Start();
        //            string teplatepath = "Csfamilypage\\Procell";
        //            string templatepathgroup = "Csfamilypage\\Group_Procell";

        //            for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
        //            {
        //                dsprecapcol = string.Empty;
        //                dsprecapcol = DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper();
        //                if (dsprecapcol != "COST"
        //                    && dsprecapcol != "CODE"
        //                    && dsprecapcol != "TWEB IMAGE1"
        //                    && dsprecapcol != "PRODUCT_ID"
        //                    && dsprecapcol != "FAMILY_ID"
        //                    )
        //                {
        //                    if (dssimilarcolumns.Tables.Count == 0)
        //                    {
        //                        _stmpl_records = _stg_records.GetInstanceOf(teplatepath);
        //                        _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
        //                        _stmpl_records.SetAttribute("ATTRIBUTE_TITLE", DsPreview.Tables[_familyID].Columns[j].Caption);

        //                        HTMLAtts = HTMLAtts + _stmpl_records.ToString();
        //                        if (dsprecapcol == "DESCRIPTION")
        //                        {
        //                            _stmpl_records1.SetAttribute("PROD_DESCRIPTION", DsPreview.Tables[_familyID].Rows[i]["DESCRIPTION"].ToString());
        //                            flgdescchk = true;
        //                        }
        //                        if (dsprecapcol == "DESCRIPTION" && DsPreview.Tables[_familyID].Rows[i][j].ToString() != string.Empty)
        //                        {
        //                            prodedesc = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                            flgdeschk_bbpp = true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        DataRow[] dr = dssimilarcolumns.Tables[0].Select("Attribute_name='" + DsPreview.Tables[_familyID].Columns[j].Caption + "'");
        //                        if (dr.Length > 0)
        //                        {

        //                            if (i == 0)
        //                            {
        //                                _stmpl_records_group = _stg_records.GetInstanceOf(templatepathgroup);
        //                                _stmpl_records_group.SetAttribute("GROUP_ATTRIBUTE_VALUE", DsPreview.Tables[_familyID].Rows[i][j].ToString());
        //                                HTMLAtts_group_details = HTMLAtts_group_details + _stmpl_records_group.ToString();
        //                            }

        //                        }
        //                        else
        //                        {
        //                            _stmpl_records = _stg_records.GetInstanceOf(teplatepath);
        //                            _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
        //                            _stmpl_records.SetAttribute("ATTRIBUTE_TITLE", DsPreview.Tables[_familyID].Columns[j].Caption);
        //                            HTMLAtts = HTMLAtts + _stmpl_records.ToString();
        //                            if (dsprecapcol == "DESCRIPTION")
        //                            {
        //                                _stmpl_records1.SetAttribute("PROD_DESCRIPTION", DsPreview.Tables[_familyID].Rows[i]["DESCRIPTION"].ToString());
        //                                flgdescchk = true;
        //                            }
        //                            if (dsprecapcol == "DESCRIPTION" && DsPreview.Tables[_familyID].Rows[i][j].ToString() != string.Empty)
        //                            {
        //                                prodedesc = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
        //                                flgdeschk_bbpp = true;
        //                            }

        //                        }

        //                    }


        //                }

        //            }

        //            if (HTMLAtts_group != "")
        //            {
        //                _stmpl_records1_group = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Group_row");

        //                _stmpl_records1_group.SetAttribute("GROUPHEADER", HTMLAtts_group);
        //                _stmpl_records1_group.SetAttribute("GROUPDETAILS", HTMLAtts_group_details);
        //                HTMLHeaderStr_group = _stmpl_records1_group.ToString();


        //            }
        //            //stopwatch.Stop();

        //            //objErrorHandler.CreateLog("For description" + "=" + stopwatch.Elapsed );

        //            if (!(flgdescchk))
        //                _stmpl_records1.SetAttribute("PROD_DESCRIPTION", family_name.Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""));


        //            if (!(flgdeschk_bbpp))
        //            {
        //                if (tempPriceDr != null && tempPriceDr.Length > 0)
        //                    prodedesc = tempPriceDr[0]["PRODUCT_DESC"].ToString();
        //            }

        //            _stmpl_records1.SetAttribute("ATTRIBUTE_VALUES", HTMLAtts);

        //            if (BindToST == true)
        //            {
        //                HTMLProducts = HTMLProducts + _stmpl_records1.ToString();
        //            }

        //            if (prodedesc.Length > 0)
        //                prodcodedesc = prodcodedesc + _ProCode + " – " + prodedesc + "|";
        //            else
        //                prodcodedesc = prodcodedesc + _ProCode + "|";




        //        }
        //        //stopwatch.Stop();
        //        //objErrorHandler.CreateLog("Second For Loop:" + "=" + stopwatch.Elapsed);
        //        HttpContext.Current.Session["prodcodedesc"] = prodcodedesc;


        //        if ((showheader == true) || (HttpContext.Current.Request.Browser.IsMobileDevice == true))
        //        {
        //            _stmpl_container = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Promain");

        //            //if (Convert.ToInt16(HttpContext.Current.Session[_familyID + "Icnt"].ToString()) == 1)
        //            //{
        //            if (HTMLHeaderStr_group != "")
        //            {
        //                _stmpl_container.SetAttribute("IS_COMMON_VALUE", HTMLHeaderStr_group);
        //                _stmpl_container.SetAttribute("PRODUCT_DETAILS_HEAD", HTMLHeaderStr_group);
        //            }

        //            _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts + " </tbody>");
        //            //}
        //            //else
        //            //{
        //            //    _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts );
        //            //}
        //        }
        //        else
        //        {

        //            _stmpl_container = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Promain_dyn");
        //            _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts.Replace("mythead", "thead_none") + " </tbody>");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.CreateLog("GenerateHorizontalHTMLJson---" + ex.ToString());

        //    }

        //    return _stmpl_container.ToString();

        //}



        public string GenerateHorizontalHTMLJson(string _familyID, DataSet Ds, DataSet dsPriceTableAll, DataSet EADs, string Rawurl_dy, string famId)
        {
            //objErrorHandler.CreateLog("GenerateHorizontalHTMLJson" + _familyID);
            string rtnstr = string.Empty;
            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_container1 = null;
            StringTemplateGroup _stg_container2 = null;
            StringTemplateGroup _stg_records = null;

            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_records_group = null;
            StringTemplate _stmpl_records1 = null;

            StringTemplate _stmpl_records1_group = null;

            StringTemplate _stmpl_records2 = null;
            StringTemplate _stmpl_records3 = null;
            StringTemplate _stmpl_records4 = null;
            StringTemplate _stmpl_records5 = null;
            StringTemplate _stmpl_recordsrows = null;
            StringTemplate _stmpl_recordsrows1 = null;
            string HTMLAtts3 = string.Empty;
            string HTMLAtts5 = string.Empty;


            //StringTemplate _stmpl_recordsrows = null;
            //  TBWDataList[] lstrecords = new TBWDataList[0];
            // TBWDataList[] lstrows = new TBWDataList[0];

            // StringTemplateGroup _stg_container1 = null;
            StringTemplateGroup _stg_records1 = null;
            StringTemplateGroup _stg_records2 = null;
            StringTemplateGroup _stg_records3 = null;
            StringTemplateGroup _stg_records4 = null;
            StringTemplateGroup _stg_records5 = null;
            //  TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            //  TBWDataList1[] lstrows1 = new TBWDataList1[0];
            try
            {

                DataSet dsBgDisc = new DataSet();
                // decimal untPrice = 0;
                string AttrID = string.Empty;
                //  string HypColumn = "";
                //  int Min_ord_qty = 0;
                //  int Qty_avail;
                //  int flagtemp = 0;
                string _StockStatus = "NO STATUS AVAILABLE";
                string _prod_stk_Status = "0";
                string _prod_stk_flag = "0";
                string _AvilableQty = "0";
                string _eta = string.Empty;
                string _Category_id = string.Empty;
                string _EA_Path = string.Empty;
                string StrPriceTable = string.Empty;
                string CATEGORY_PATH = string.Empty;
                DataRow[] tempPriceDr;

                //DataTable tempPriceDt;
                //int ProdID;
                //  int AttrType;
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (userid == "" || userid == null)
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];
                //userid = "999";// ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

                // string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
                //  string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
                string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
                string _parentFamily_Id = "0";
                // string CartImgPathdisplay = "";
                string _ProCode = string.Empty;
                string family_name = string.Empty;
                if (EComState == "YES")
                    if (!objHelperServices.GetIsEcomEnabled(userid))
                        EComState = "NO";
                StringBuilder strBldr = new StringBuilder();
                StringBuilder strBldrcost = new StringBuilder();

                if (HttpContext.Current.Request.QueryString["path"] != null)
                    _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());

                if (HttpContext.Current.Request.QueryString["cid"] != null)
                    _Category_id = HttpContext.Current.Request.QueryString["cid"];

                DsPreview = Ds;
                if (DsPreview.Tables[_familyID] == null)
                    return "";


                DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
                if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                    _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

                if (_parentFamily_Id == "0")
                    _parentFamily_Id = _familyID;

                pricecode = GetPriceCode();

                if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                    family_name = _parentFamilyds.Tables[0].Rows[0]["FAMILY_NAME"].ToString();
                else
                    family_name = HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"].ToString();

                string _SkinRootPath = string.Empty;

                _SkinRootPath = HttpContext.Current.Server.MapPath("~\\Templates");

                _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);
                _stg_records2 = new StringTemplateGroup("cell", _SkinRootPath);
                _stg_records3 = new StringTemplateGroup("row", _SkinRootPath);

                string HTMLAtts = string.Empty;
                string HTMLAtts1 = string.Empty;
                string HTMLAtts_group = string.Empty;
                string HTMLAtts_group_details = string.Empty;
                string HTMLProducts = string.Empty;
                string HTMLProducts1 = string.Empty;
                string HTMLHeaderStr = string.Empty;
                string HTMLHeaderStr_group = string.Empty;
                bool BindToST = true;
                int productcnt = 0;
                // int ictrecords = 0;
                string costtype = string.Empty;
                HTMLAtts = "";
                string DsPreviewcaption = string.Empty;
                DataSet dssimilarcolumns = new DataSet();
                //try
                //{
                //    if (DsPreview.Tables[0].Rows.Count > 1)
                //    {
                //        DataSet tmpds1 = objHelperDb.GetDataSetDB("exec [STP_GETFAMILY_XML] " + _familyID);
                //        DataSet dsmrgattr = new DataSet();
                //        if (tmpds1 != null && tmpds1.Tables[0].Rows.Count > 0)
                //        {

                //            dsmrgattr = ConvertXMLToDataSet(tmpds1.Tables[0].Rows[0][0].ToString());

                //            DataRow[] dr = dsmrgattr.Tables["LeftRowField"].Select("Merge='Checked'");
                //            if (dr.Length > 0)
                //            {
                //                DataTable dt = dr.CopyToDataTable();
                //                string x = string.Empty;
                //                for (int i = 0; i < dt.Rows.Count; i++)
                //                {
                //                    if (i == 0)
                //                    {
                //                        x = dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");
                //                    }
                //                    else
                //                    {
                //                        x = x + "," + dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");
                //                    }


                //                }
                //                dssimilarcolumns = objHelperDb.GetDataSetDB("exec [STP_Attribute_name] '" + x + "'");
                //            }

                //        }
                //    }
                //}
                //    catch ( Exception ex)
                //{

                //    }
                // stopwatch.Start();
                for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                {
                    DsPreviewcaption = string.Empty;
                    DsPreviewcaption = DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper();
                    if (DsPreviewcaption != "COST" && DsPreviewcaption != "CODE"
                        && DsPreviewcaption != "TWEB IMAGE1"
                        && DsPreviewcaption != "PRODUCT_ID"
                        && DsPreviewcaption != "FAMILY_ID"
                        )
                    {


                        if (dssimilarcolumns.Tables.Count == 0)
                        {
                            _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProcellHead");
                            _stmpl_records.SetAttribute("ATTRIBUTE_NAME", DsPreview.Tables[_familyID].Columns[j].Caption);
                            HTMLAtts = HTMLAtts + _stmpl_records.ToString();
                        }
                        else
                        {
                            DataRow[] dr = dssimilarcolumns.Tables[0].Select("Attribute_name='" + DsPreview.Tables[_familyID].Columns[j].Caption + "'");
                            if (dr.Length > 0)
                            {
                                _stmpl_records_group = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Group_ProcellHead");
                                _stmpl_records_group.SetAttribute("GROUP_ATTRIBUTE_NAME", DsPreview.Tables[_familyID].Columns[j].Caption);
                                HTMLAtts_group = HTMLAtts_group + _stmpl_records_group.ToString();

                            }
                            else
                            {
                                _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProcellHead");
                                _stmpl_records.SetAttribute("ATTRIBUTE_NAME", DsPreview.Tables[_familyID].Columns[j].Caption);
                                HTMLAtts = HTMLAtts + _stmpl_records.ToString();

                            }

                        }
                    }
                    //DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                    //if (tempdr.Length > 0 && objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString())!=3)
                    //{
                    if (DsPreviewcaption == "COST")
                    {
                        if (pricecode == 1)
                        {
                            costtype = " Inc GST";
                        }
                        else
                        {
                            costtype = " Ex GST";
                        }
                    }

                    //}

                }
                //stopwatch.Stop();
                //objErrorHandler.CreateLog("First For Loop:" + "=" + stopwatch.Elapsed );
                _stmpl_records1 = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProrowHead");
                _stmpl_records1.SetAttribute("INGST", costtype);
                _stmpl_records1.SetAttribute("ATTRIBUTE_HEADR", HTMLAtts);

                bool bindheader = objHelperDb.CheckFamilyPAGE_Discontinued(_familyID.ToString());
                // objErrorHandler.CreateLog(bindheader.ToString() + _familyID);
                //if (bindheader == true)
                //{
                //    HTMLHeaderStr = _stmpl_records1.ToString();
                //}




                bool showheader = true;
                if (HttpContext.Current.Session["hfprevfid"] == null)
                {
                    HttpContext.Current.Session["hfprevfid"] = _familyID;
                }
                else if (HttpContext.Current.Session["hfprevfid"].ToString() != _familyID)
                {
                    HttpContext.Current.Session["hfprevfid"] = _familyID;
                }
                else
                {
                    showheader = false;
                }


                //  DisplayHeaders = true;
                //if ((showheader))
                //{
                HTMLProducts = HTMLProducts + HTMLHeaderStr + "<tbody class=\"tablebutton\" id=\"myTable\">";
                //}
                //else
                //{
                //    HTMLProducts = HTMLProducts +  "<tbody class=\"tablebutton\" id=\"myTable\">";

                //}
                string ValueFortag = string.Empty;
                //bool rowcolor = false;

                if (_EA_Path == string.Empty && _Category_id == string.Empty)
                {
                    DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                    if (tmpds != null && tmpds.Tables.Count > 0)
                    {
                        _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                        string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch=Family Id=" + _familyID.ToString();
                        _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
                    }
                }

                _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);
                _stg_records2 = new StringTemplateGroup("cell", _SkinRootPath);
                _stg_records3 = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();

                string prodcodedesc = string.Empty;
                string prodedesc = string.Empty;

                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));

                //Added on 18 July
                string HREFURL = string.Empty;
                string conspath = string.Empty;


                //if (HttpContext.Current.Request.RawUrl.Contains("xx"))
                //{
                //    if (_EA_Path == null && _Category_id == "")
                //    {
                //        DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                //        if (tmpds != null && tmpds.Tables.Count > 0)
                //        {
                //            _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                //            CATEGORY_PATH = tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString();
                //            string eapath = "AllProducts////WESAUSTRALASIA////" + CATEGORY_PATH + "////UserSearch=Family Id=" + _familyID.ToString();
                //            _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
                //            string[] catpath = CATEGORY_PATH.ToString().Split(new string[] { "////" }, StringSplitOptions.None);
                //            CATEGORY_PATH =  (catpath.Length >= 1 ? catpath[0] : " ") + "////" +  (catpath.Length >=2 ? catpath[1] : " ") + "////";
                //        }
                //    }


                //    conspath = WAG_Root_Path + "////" + _parentFamily_Id + "////" + CATEGORY_PATH + "////" + family_name;


                //}
                //else
                //{




                // NEWURL = HttpContext.Current.Session["breadcrumEAPATH"].ToString() + "////" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "=" + _ProCode;
                string rawurl = string.Empty;

                if (Rawurl_dy != "")
                {
                    rawurl = Rawurl_dy;
                }
                else
                {
                    rawurl = HttpContext.Current.Request.RawUrl.ToString().Replace("/fl/", "");
                }
                string[] rev = rawurl.Split('/');
                Array.Reverse(rev);
                if (!rawurl.Contains("SortProduct"))
                {
                    if (rev[0].Contains("wa"))
                    {


                        conspath = (rev.Length > 2 ? rev[2] : " ") + "////" + (rev.Length > 3 ? rev[3] : " ") + "////" + (rev.Length > 4 ? rev[4] : " ");



                    }
                    else
                    {
                        conspath = (rev.Length > 1 ? rev[1] : " ") + "////" + (rev.Length > 2 ? rev[2] : " ") + "////" + (rev.Length > 3 ? rev[3] : " ");
                    }
                }
                //}

                // stopwatch.Start();

                DsPreview.Tables[_familyID].Columns.Add("BindToST", typeof(bool));


                string templatepath = "Csfamilypage\\Prorow2";
                string templatepath1 = "Csfamilypage\\Prorow3";
                string tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch=Family Id=" + _parentFamily_Id.ToString()));
                for (int i = 0; i < DsPreview.Tables[_familyID].Rows.Count; i++)
                {
                    tempPriceDr = EADs.Tables["FamilyPro"].Select("PRODUCT_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
                    //if (tempPriceDr.Length > 0)
                    //    tempPriceDt = tempPriceDr.CopyToDataTable();
                    //else
                    //   tempPriceDt = null;

                    strBldrcost = new StringBuilder();


                    _stmpl_records1 = _stg_records.GetInstanceOf(templatepath);
                    _stmpl_records3 = _stg_records.GetInstanceOf(templatepath1);

                    _stmpl_records1.SetAttribute("PRODUCT_ID", DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                    _stmpl_records3.SetAttribute("PRODUCT_ID", DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                    _stmpl_records1.SetAttribute("FAMILY_ID", DsPreview.Tables[_familyID].Rows[i]["FAMILY_ID"].ToString());
                    _stmpl_records3.SetAttribute("FAMILY_ID", DsPreview.Tables[_familyID].Rows[i]["FAMILY_ID"].ToString());

                    _stmpl_records1.SetAttribute("PRODUCT_NAME", objProductServices.GetFamilyProductName(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"])));
                    _stmpl_records3.SetAttribute("PRODUCT_NAME", objProductServices.GetFamilyProductName(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"])));

                    //-------------------------------- cost
                    _StockStatus = "NO STATUS AVAILABLE";
                    _prod_stk_Status = "0";
                    _prod_stk_flag = "0";
                    _eta = "";
                    string ISSUBSTITUTE = "";
                    //_stmpl_records1.SetAttribute("COST", Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + "<br/> Price / Stock Status");
                    // _stmpl_records1.SetAttribute("COST", Prefix + " " + DsPreview.Tables[_familyID].Rows[i]["COST"].ToString() + " " + "<br/> Price / Stock Status");
                    _stmpl_records1.SetAttribute("COST", Prefix + "" + DsPreview.Tables[_familyID].Rows[i]["COST"].ToString());
                    _stmpl_records3.SetAttribute("COST", Prefix + "" + DsPreview.Tables[_familyID].Rows[i]["COST"].ToString());
                    if (tempPriceDr != null && tempPriceDr.Length > 0)
                    {
                        _StockStatus = tempPriceDr[0]["STOCK_STATUS_DESC"].ToString().Replace("_", " ");
                        _prod_stk_Status = tempPriceDr[0]["PROD_STOCK_STATUS"].ToString();
                        _prod_stk_flag = tempPriceDr[0]["PROD_STOCK_FLAG"].ToString();
                        _AvilableQty = tempPriceDr[0]["QTY_AVAIL"].ToString();
                        _eta = tempPriceDr[0]["ETA"].ToString();
                        ISSUBSTITUTE = tempPriceDr[0]["PROD_SUBSTITUTE"].ToString();
                    }
                    if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
                        _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();

                    //  StrPriceTable = AssemblePriceTable(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _prod_stk_Status, CustomerType, Convert.ToInt32(userid), _prod_stk_flag,_eta, dsPriceTableAll);
                    // _stmpl_records1.SetAttribute("PRICE_TABLE", StrPriceTable);


                    _stmpl_records1.SetAttribute("AVIL_QTY", _AvilableQty);
                    _stmpl_records3.SetAttribute("AVIL_QTY", _AvilableQty);
                    _stmpl_records1.SetAttribute("MIN_ORDER_QTY", 1);
                    _stmpl_records3.SetAttribute("MIN_ORDER_QTY", 1);




                    // string ORIGINALURL = "pd.aspx?pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath;


                    //if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
                    //{
                    //    NEWURL = HttpContext.Current.Session["breadcrumEAPATH"].ToString() + "////" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "=" + _ProCode;
                    //    NEWURL = objHelperServices.Cons_NewURl_bybrand(ORIGINALURL, NEWURL, "pd.aspx", "");
                    //    HREFURL = "/" + NEWURL + "/pd/";
                    //}
                    //else
                    //{http://schema.org/Product
                    //    NEWURL = ORIGINALURL;
                    //    HREFURL = ORIGINALURL;

                    //}

                    //objErrorHandler.CreateLog("_procode="+_ProCode);
                    if ((_ProCode == "") || (_ProCode == string.Empty))

                    {
                        _ProCode = DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString();
                    }
                    //objErrorHandler.CreateLog(DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString());
                    //objErrorHandler.CreateLog("_procode1=" + _ProCode);
                    HREFURL = objHelperServices.SimpleURL_Str(WAG_Root_Path + "////" + _parentFamily_Id + "////" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "=" + _ProCode
         + "////" + conspath, "pd.aspx", true);




                    HREFURL = HREFURL + "/pd/";


                    _stmpl_records1.SetAttribute("PARENT_FAMILY_ID", _parentFamily_Id);
                    _stmpl_records3.SetAttribute("PARENT_FAMILY_ID", _parentFamily_Id);
                    _stmpl_records1.SetAttribute("CAT_ID", _Category_id);
                    _stmpl_records3.SetAttribute("CAT_ID", _Category_id);
                    _stmpl_records1.SetAttribute("EA_PATH", tempEAPath);
                    _stmpl_records3.SetAttribute("EA_PATH", tempEAPath);
                    _stmpl_records1.SetAttribute("URL_RW_PATH", HREFURL);
                    _stmpl_records3.SetAttribute("URL_RW_PATH", HREFURL);

                    _stmpl_records1.SetAttribute("BUY_FAMILY_ID", _familyID);
                    _stmpl_records3.SetAttribute("BUY_FAMILY_ID", _familyID);
                    // objHelperServices.SimpleURL_Str(NEWURL, "pd.aspx");
                    //objErrorHandler.CreateLog(_StockStatus + DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString());
                    bool _Tbt_Stock_Status_2 = false;
                    string _Tbt_Stock_Status = string.Empty;
                    string _Tbt_Stock_Status_1 = string.Empty;
                    string _Tbt_Stock_Status_3 = string.Empty;
                    //string _Colorcode1 = string.Empty;
                    //string _Colorcode;
                    string _StockStatusTrim = _StockStatus.Trim();
                    string _Eta = string.Empty;
                    if (_eta != string.Empty)
                    {
                        _Eta = string.Format("<div class=\"inlineblk mob_sm_txt\"><b>ETA: </b> " + _eta.ToString() + "</div>");
                        //objErrorHandler.CreateLog(_Eta);
                    }

                    // objErrorHandler.CreateLog(_StockStatus.Trim());
                    switch (_StockStatus.Trim())
                    {
                        case "IN STOCK":
                            _Tbt_Stock_Status = "INSTOCK";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "Limited Stock, Please Call":
                            _Tbt_Stock_Status = "Limited Stock";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "SPECIAL ORDER":
                            _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                            _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "SPECIAL ORDER PRICE &":
                            _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "DISCONTINUED":
                            _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                            _Tbt_Stock_Status_2 = false;
                            break;
                        case "DISCONTINUED NO LONGER AVAILABLE":
                            _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                            _Tbt_Stock_Status_2 = false;
                            break;
                        case "DISCONTINUED NO LONGER":
                            _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                            _Tbt_Stock_Status_2 = false;
                            break;
                        case "TEMPORARY UNAVAILABLE":
                            _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                            _Tbt_Stock_Status_2 = false;
                            break;
                        case "TEMPORARY UNAVAILABLE NO ETA":
                            _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                            _Tbt_Stock_Status_2 = false;
                            break;
                        case "OUT OF STOCK":
                            _Tbt_Stock_Status_3 = "Out of stock. ";
                            _Tbt_Stock_Status_1 = " Back Order Item";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                            _Tbt_Stock_Status_3 = "Out of stock. ";
                            _Tbt_Stock_Status_1 = " Back Order Item";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "OUT OF STOCK ITEM WILL":
                            _Tbt_Stock_Status_3 = "Out of stock. ";
                            _Tbt_Stock_Status_1 = " Back Order Item";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "Please Call":
                            _Tbt_Stock_Status_3 = "Please Call";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "Please_Call":
                            _Tbt_Stock_Status_3 = "Please Call";
                            _Tbt_Stock_Status_2 = true;
                            break;

                        default:
                            _Tbt_Stock_Status = _StockStatusTrim;
                            _Tbt_Stock_Status_2 = false;
                            break;
                    }

                    string stkstatus = string.Empty;
                    stkstatus = _Tbt_Stock_Status.ToUpper();
                    string stkstatus1 = string.Empty;
                    stkstatus1 = _Tbt_Stock_Status_1.ToUpper();
                    BindToST = true;
                    //objErrorHandler.CreateLog("_Tbt_Stock_Status_2" + _Tbt_Stock_Status_2 + _ProCode +family_name);
                    if ((_Tbt_Stock_Status_2))
                    {
                        try
                        {
                            if ((_StockStatus.ToUpper().Contains("OUT OF STOCK") == true || _StockStatus.ToUpper().Contains("PLEASE CALL") == true || _StockStatus.ToUpper().Contains("SPECIAL ORDER") == true) && ISSUBSTITUTE.ToString().Trim() != "" && _prod_stk_Status == "False" && _prod_stk_flag.ToString().Trim() == "-2")
                            {
                                // DataTable rtntbl = objProductServices.GetSubstituteProductDetails(ISSUBSTITUTE.ToString().Trim(), Convert.ToInt32(userid));
                                DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_ProCode, Convert.ToInt32(userid), "pd");
                                if (rtntbl != null && rtntbl.Rows.Count > 0)
                                {

                                    bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                                    bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

                                    if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                    {

                                        _stmpl_records1.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records3.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records1.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                        _stmpl_records3.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                        string strurl = rtntbl.Rows[0]["ea_path"].ToString();
                                        _stmpl_records1.SetAttribute("TBT_REP_EA_PATH", strurl);
                                        _stmpl_records3.SetAttribute("TBT_REP_EA_PATH", strurl);
                                    }
                                }

                            }
                            else
                            {
                                _stmpl_records1.SetAttribute("SHOW_BUY", true);
                                _stmpl_records3.SetAttribute("SHOW_BUY", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            objErrorHandler.CreateLog(ex.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            // objErrorHandler.CreateLog("ISSUBSTITUTE" + "= " + ISSUBSTITUTE + "_prod_stk_Status" + _prod_stk_Status + "_prod_stk_flag" + _prod_stk_flag + ProdID);
                            //&& _prod_stk_Status == "0" modified by indu on apr-5-2018
                            if (ISSUBSTITUTE.ToString().Trim() != "" && _prod_stk_flag.ToString().Trim() == "-2")
                            {
                                DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_ProCode.ToString().Trim(), Convert.ToInt32(userid), "pd");

                                if (rtntbl != null && rtntbl.Rows.Count > 0)
                                {

                                    bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                                    bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

                                    //objErrorHandler.CreateLog("samecodenotFound" + samecodenotFound);
                                    //  objErrorHandler.CreateLog("ea_path" + rtntbl.Rows[0]["ea_path"].ToString());
                                    if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")

                                    {
                                        //  objErrorHandler.CreateLog("samecodenotFound" + samecodenotFound);
                                        BindToST = true;
                                        if (_StockStatus.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
                                        {
                                            // objErrorHandler.CreateLog("inside substitute prod" + rtntbl.Rows[0]["SubstuyutePid"].ToString());
                                            HelperDB objhelperDb = new HelperDB();
                                            DataSet Sqltbs = objhelperDb.GetProductPriceEA("", rtntbl.Rows[0]["SubstuyutePid"].ToString(), userid);
                                            if (Sqltbs != null)
                                            {

                                                string stockstaus = Sqltbs.Tables[0].Rows[0]["PROD_STK_STATUS_DSC"].ToString().Trim().ToUpper();

                                                if (stockstaus == "DISCONTINUED NO LONGER AVAILABLE")
                                                {

                                                    BindToST = false;
                                                }
                                                else
                                                {
                                                    BindToST = true;
                                                }
                                            }
                                        }
                                        _stmpl_records1.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records3.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records1.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                        _stmpl_records3.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                        string strurl = rtntbl.Rows[0]["ea_path"].ToString();
                                        _stmpl_records1.SetAttribute("TBT_REP_EA_PATH", strurl);
                                        _stmpl_records3.SetAttribute("TBT_REP_EA_PATH", strurl);
                                    }
                                    else if (_StockStatus.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
                                    {
                                        //objErrorHandler.CreateLog(_ProCode + "inside family page DISCONTINUED");
                                        BindToST = false;
                                    }
                                    else if (_StockStatus.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA" || _StockStatus.ToUpper() == "TEMPORARY_UNAVAILABLE NO ETA")
                                    {
                                        _stmpl_records1.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable. Please Call");
                                        _stmpl_records3.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable. Please Call");
                                    }
                                }
                            }

                            else
                            {

                                if ((_StockStatus.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE" || _StockStatus.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA" || _StockStatus.ToUpper() == "TEMPORARY_UNAVAILABLE NO ETA"))
                                {
                                    if (_StockStatus.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
                                    {
                                        _stmpl_records1.SetAttribute("PRODUCT_STATUS", "Product Discontinued");
                                        _stmpl_records3.SetAttribute("PRODUCT_STATUS", "Product Discontinued");
                                        BindToST = false;
                                        //objErrorHandler.CreateLog(ProdID + "Product Discontinued" +"Familypage"); 
                                    }
                                    else if (_StockStatus.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA" || _StockStatus.ToUpper() == "TEMPORARY_UNAVAILABLE NO ETA")
                                    {
                                        _stmpl_records1.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable. Please Call");
                                        _stmpl_records3.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable. Please Call");
                                        BindToST = true;
                                    }
                                    _stmpl_records1.SetAttribute("TBT_HIDE_BUY", true);
                                    _stmpl_records3.SetAttribute("TBT_HIDE_BUY", true);
                                }


                            }
                        }
                        catch (Exception ex)
                        {
                            objErrorHandler.CreateLog(ex.ToString());
                        }
                    }




                    _stmpl_records1.SetAttribute("STOCK_STATUS", _StockStatus.ToString().Trim());
                    _stmpl_records3.SetAttribute("STOCK_STATUS", _StockStatus.ToString().Trim());

                    //-------------------------------- Code

                    _stmpl_records1.SetAttribute("PROD_CODE", DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString());
                    _stmpl_records3.SetAttribute("PROD_CODE", DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString());

                    //-------------------------------- Image

                    ValueFortag = DsPreview.Tables[_familyID].Rows[i]["TWEB IMAGE1"].ToString();
                    string ValueLargeImg = string.Empty;
                    if (ValueFortag != string.Empty && ValueFortag != null && ValueFortag != "noimage.gif")
                    {
                        ValueFortag = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + ValueFortag.Replace("\\", "/");
                        ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200").Replace("\\", "/");
                        //FileInfo Fil;


                        //Fil = new FileInfo(strFile + ValueFortag);ob
                        //if (Fil.Exists)
                        //{

                        //    ValueFortag = "/prodimages" + ValueFortag.Replace("\\", "/");
                        //    ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200").Replace("\\", "/");
                        //}
                        //else
                        //{
                        //    ValueFortag = "/prodimages/images/noimage.gif";
                        //    ValueLargeImg = "";
                        //}
                    }
                    else
                    {
                        ValueFortag = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages/images/noimage.gif";
                        ValueLargeImg = "";
                    }
                    if (!HttpContext.Current.Request.RawUrl.ToLower().Contains("/mfl/"))
                    {

                        _stmpl_records1.SetAttribute("TWEB_LargeImg", ValueLargeImg);
                        _stmpl_records3.SetAttribute("TWEB_LargeImg", ValueLargeImg);
                    }

                    _stmpl_records1.SetAttribute("TWEB_Image", ValueFortag);
                    _stmpl_records3.SetAttribute("TWEB_Image", ValueFortag);
                    if ((ValueLargeImg != "") && (!HttpContext.Current.Request.RawUrl.ToLower().Contains("/mfl/")))
                    {
                        _stmpl_records1.SetAttribute("SHOW_DIV", true);
                        _stmpl_records3.SetAttribute("SHOW_DIV", true);
                    }
                    else
                    {
                        _stmpl_records1.SetAttribute("SHOW_DIV", false);
                        _stmpl_records3.SetAttribute("SHOW_DIV", false);
                    }

                    bool isSameLogic = true;

                    //objErrorHandler.CreateLog("PROD_STOCK_FLAG " + _prod_stk_flag.ToString() + "-" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                    //objErrorHandler.CreateLog("PROD_STOCK_STATUS " + _prod_stk_Status.ToString() + "-" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                    //objErrorHandler.CreateLog("STOCK_STATUS_DESC " + _StockStatus.ToString() + "-" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());

                    if (_prod_stk_flag == "-2" && _eta != "" && _eta.ToString().Trim() != "PLEASE CALL")
                    {
                        isSameLogic = true;
                    }
                    else if ((_prod_stk_flag == "-2" && _prod_stk_Status.ToString() == "0" && _StockStatus.ToString().Trim() == "OUT OF STOCK ITEM WILL BE BACK ORDERED") || (_prod_stk_flag.ToString() == "0" && _prod_stk_Status.ToString() == "1" && _StockStatus.ToString().Trim() == "Please Call") || (_prod_stk_flag.ToString() == "-2" && _prod_stk_Status.ToString() == "0" && _StockStatus.ToString().Trim() == "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED"))
                    {
                        //objErrorHandler.CreateLog("GetStockDetails");
                        isSameLogic = GetStockDetails(_stmpl_records1, _stmpl_records3, DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString(), _eta.ToString());

                    }
                    else
                    {
                        isSameLogic = true;
                    }

                    //objErrorHandler.CreateLog("isSameLogic " + isSameLogic);
                    if (isSameLogic)
                    {
                        //To display description when PROD_STOCK_FLAG is -2 and eta date not expired
                        if (_prod_stk_flag == "-2" && _eta != string.Empty && _eta.ToString().Trim() != "PLEASE CALL")
                        {
                            //string etaShipDays = string.Empty;
                            //etaShipDays = "Ships Out Approx: " + _eta.ToString();
                            _Tbt_Stock_Status_1 = "";
                            _stmpl_records1.SetAttribute("TBT_STOCK_STATUS_3", "Pre-Order");
                            _stmpl_records3.SetAttribute("TBT_STOCK_STATUS_3", "Pre-Order");
                            _Eta = string.Format("<div class=\"inlineblk mob_sm_txt\">ETA: " + _eta.ToString() + "</div>");
                        }
                        else if (!_Tbt_Stock_Status.Equals(""))
                        {
                            //_stmpl_records1.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                            //_stmpl_records3.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                            //if (_Tbt_Stock_Status == "Limited Stock")
                            //{
                            //    _stmpl_records1.SetAttribute("TBT_PLEASE_CALL", "true");
                            //    _stmpl_records3.SetAttribute("TBT_PLEASE_CALL", "true");
                            //}

                            if (_Tbt_Stock_Status == "Limited Stock")
                            {
                                int soh = 0;
                                HelperDB objHelperDB = new HelperDB();
                                string tempstr = (string)objHelperDB.GetGenericDataDB(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString(), "GET_PRODUCT_SOH", HelperDB.ReturnType.RTString);
                                if (tempstr != null && tempstr != "")
                                    soh = Convert.ToInt32(tempstr);
                                //objErrorHandler.CreateLog("soh " + soh);
                                //soh = objProductServices.GetProductSOH(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString()));

                                if (soh > 4)
                                {
                                    _stmpl_records1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                                    _stmpl_records3.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                                    _stmpl_records1.SetAttribute("TBT_ISINSTOCK", true);
                                    _stmpl_records3.SetAttribute("TBT_ISINSTOCK", true);
                                    _stmpl_records1.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                                    _stmpl_records3.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                                }
                                else
                                {
                                    _stmpl_records1.SetAttribute("TBT_PLEASE_CALL", true);
                                    _stmpl_records3.SetAttribute("TBT_PLEASE_CALL", true);
                                }
                            }
                            else
                            {
                                _stmpl_records1.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                                _stmpl_records3.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                            }
                        }
                        else
                        {
                            _stmpl_records1.SetAttribute("TBT_STOCK_STATUS_3", _Tbt_Stock_Status_3);
                            _stmpl_records3.SetAttribute("TBT_STOCK_STATUS_3", _Tbt_Stock_Status_3);
                        }
                        //_stmpl_records.SetAttribute("TBT_COLOR_CODE_1", _Colorcode1);
                        _stmpl_records1.SetAttribute("TBT_STOCK_STATUS_1", _Tbt_Stock_Status_1);
                        _stmpl_records3.SetAttribute("TBT_STOCK_STATUS_1", _Tbt_Stock_Status_1);
                        _stmpl_records1.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                        _stmpl_records3.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                        _stmpl_records1.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
                        _stmpl_records3.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
                        if (stkstatus == "INSTOCK" || stkstatus1 == "INSTOCK")
                        {
                            _stmpl_records1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                            _stmpl_records3.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                            _stmpl_records1.SetAttribute("TBT_ISINSTOCK", true);
                            _stmpl_records3.SetAttribute("TBT_ISINSTOCK", true);
                            //  objErrorHandler.CreateLog("Peoduct inside instock"); 
                            _stmpl_records1.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                            _stmpl_records3.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                        }
                        else if (stkstatus == "ITEM WILL BE BACK ORDERED" || stkstatus1 == "ITEM WILL BE BACK ORDERED")
                        {
                            _stmpl_records1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/OutOfStock");
                            _stmpl_records3.SetAttribute("TBT_STOCK_HREF", "http://schema.org/OutOfStock");
                        }
                        else if (stkstatus.Contains("SPECIAL") || stkstatus1.Contains("SPECIAL"))
                        {
                            _stmpl_records1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/LimitedAvailability");
                            _stmpl_records3.SetAttribute("TBT_STOCK_HREF", "http://schema.org/LimitedAvailability");
                        }
                        else if (!(_Tbt_Stock_Status_2))
                        {
                            _stmpl_records1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/Discontinued");
                            _stmpl_records3.SetAttribute("TBT_STOCK_HREF", "http://schema.org/Discontinued");
                        }
                        else
                        {
                            _stmpl_records1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                            _stmpl_records3.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                        }
                    }
                    //_stmpl_records.SetAttribute("TBT_STOCK_STATUS", GetStockStatus(Convert.ToInt32(cdr["PRODUCT_ID"])));
                    //  _stmpl_records.SetAttribute("TBT_PRICE_TABLE", _sPriceTable);




                    //  if( HttpContext.Current.Session["F_ALT_FNAME"].ToString() != "")
                    //      _stmpl_records1.SetAttribute("F_ALT_FNAME", HttpContext.Current.Session["F_ALT_FNAME"].ToString());


                    // To show Bulk Order price
                    OrderServices objOrderServices = new OrderServices();
                    DataSet Dsall = objOrderServices.getBulkBuyPrice(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString()), Convert.ToInt32(userid));
                    string bulkbuy = string.Empty;
                    string bulkbuymob = string.Empty;
                    string bulkbuymob1 = string.Empty;

                    if (Dsall != null && Dsall.Tables.Count > 0)
                    {
                        DataTable Sqltb = Dsall.Tables[0];

                        if (Sqltb.Rows.Count > 0)
                        {
                            bulkbuy = string.Format("<div class=\"inlineblock bulkbuy \" style=\"font-style:italic;\" ><span style=\"color:#1059a3;font-size:16px;font-weight:bold;\">Bulk Buy:</span><span style=\"color:#1059a3;font-size:16px;font-weight:bold;\"> $" + Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE1"].ToString()), 2) + " for " + Sqltb.Rows[0]["QTY"].ToString().Replace("+", "") + " or more</span></div>");
                            bulkbuymob = string.Format("<div class=\"bulkbuy a400  \" style=\"font-style:italic;\" ><span style=\"color:#1059a3;font-size:16px;font-weight:bold;\">Bulk Buy:</span><span style=\"color:#1059a3;font-size:16px;font-weight:bold;\"> $" + Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE1"].ToString()), 2) + " for " + Sqltb.Rows[0]["QTY"].ToString().Replace("+", "") + " or more</span></div>");
                            bulkbuymob1 = string.Format("<div class=\"bulkbuy ml0 b400 \" style=\"font-style:italic;\" ><span style=\"color:#1059a3;font-size:16px;font-weight:bold;\">Bulk Buy:</span><span style=\"color:#1059a3;font-size:16px;font-weight:bold;\"> $" + Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE1"].ToString()), 2) + " for " + Sqltb.Rows[0]["QTY"].ToString().Replace("+", "") + " or more</span></div>");
                            //objErrorHandler.CreateLog(bulkbuy);
                        }
                    }
                    _stmpl_records1.SetAttribute("TBT_BULK_BUY", bulkbuy);
                    _stmpl_records3.SetAttribute("TBT_BULK_BUY", bulkbuymob);
                    _stmpl_records3.SetAttribute("TBT_BULK_BUY1", bulkbuymob1);


                    HTMLAtts = "";
                    HTMLAtts1 = "";
                    bool flgdescchk = false;
                    bool flgdeschk_bbpp = false;
                    string dsprecapcol = string.Empty;
                    //stopwatch.Start();
                    string teplatepath = "Csfamilypage\\Procell1";
                    string teplatepath1 = "Csfamilypage\\Procell1";
                    string templatepathgroup = "Csfamilypage\\Group_Procell";

                    for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                    {
                        dsprecapcol = string.Empty;
                        dsprecapcol = DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper();
                        if (dsprecapcol != "COST"
                            && dsprecapcol != "CODE"
                            && dsprecapcol != "TWEB IMAGE1"
                            && dsprecapcol != "PRODUCT_ID"
                            && dsprecapcol != "FAMILY_ID"
                             && dsprecapcol != "BINDTOST"
                            )
                        {
                            if (dssimilarcolumns.Tables.Count == 0)
                            {
                                _stmpl_records = _stg_records.GetInstanceOf(teplatepath);
                                _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", HttpUtility.HtmlEncode(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", " ").Replace("\n", " ")));
                                _stmpl_records.SetAttribute("ATTRIBUTE_TITLE", textInfo.ToTitleCase(DsPreview.Tables[_familyID].Columns[j].Caption.ToLower()));
                                HTMLAtts = HTMLAtts + _stmpl_records.ToString();
                                //.Replace("\r", "<br/>").Replace("\n", "&nbsp;")
                                string attribute_value = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", " ").Replace("\n", " ");
                                //if (attribute_value.Length > 14)
                                //{
                                //    attribute_value = attribute_value.Substring(0, 14) + "...";
                                //}
                                _stmpl_records2 = _stg_records.GetInstanceOf(teplatepath1);
                                _stmpl_records2.SetAttribute("ATTRIBUTE_VALUE", HttpUtility.HtmlEncode(attribute_value));
                                _stmpl_records2.SetAttribute("ATTRIBUTE_TITLE", textInfo.ToTitleCase(DsPreview.Tables[_familyID].Columns[j].Caption.ToLower()));
                                HTMLAtts1 = HTMLAtts1 + _stmpl_records2.ToString();


                                if (dsprecapcol == "DESCRIPTION")
                                {
                                    _stmpl_records1.SetAttribute("PROD_DESCRIPTION", DsPreview.Tables[_familyID].Rows[i]["DESCRIPTION"].ToString());
                                    _stmpl_records3.SetAttribute("PROD_DESCRIPTION", DsPreview.Tables[_familyID].Rows[i]["DESCRIPTION"].ToString());
                                    flgdescchk = true;
                                }
                                if (dsprecapcol == "DESCRIPTION" && DsPreview.Tables[_familyID].Rows[i][j].ToString() != string.Empty)
                                {
                                    prodedesc = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                    flgdeschk_bbpp = true;
                                }
                            }
                            else
                            {
                                DataRow[] dr = dssimilarcolumns.Tables[0].Select("Attribute_name='" + DsPreview.Tables[_familyID].Columns[j].Caption + "'");
                                if (dr.Length > 0)
                                {

                                    if (i == 0)
                                    {
                                        _stmpl_records_group = _stg_records.GetInstanceOf(templatepathgroup);
                                        _stmpl_records_group.SetAttribute("GROUP_ATTRIBUTE_VALUE", DsPreview.Tables[_familyID].Rows[i][j].ToString());
                                        HTMLAtts_group_details = HTMLAtts_group_details + _stmpl_records_group.ToString();
                                    }

                                }
                                else
                                {
                                    _stmpl_records = _stg_records.GetInstanceOf(teplatepath);
                                    _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", HttpUtility.HtmlEncode(DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", " ").Replace("\n", " ")));
                                    _stmpl_records.SetAttribute("ATTRIBUTE_TITLE", textInfo.ToTitleCase(DsPreview.Tables[_familyID].Columns[j].Caption));
                                    HTMLAtts = HTMLAtts + _stmpl_records.ToString();
                                    //.Replace("\r", "<br/>").Replace("\n", "&nbsp;")
                                    string attribute_value = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", " ").Replace("\n", " ");
                                    //if (attribute_value.Length > 14)
                                    //{
                                    //    attribute_value = attribute_value.Substring(0, 14) + "...";
                                    //}
                                    _stmpl_records2 = _stg_records.GetInstanceOf(teplatepath1);
                                    _stmpl_records2.SetAttribute("ATTRIBUTE_VALUE", HttpUtility.HtmlEncode(attribute_value));
                                    _stmpl_records2.SetAttribute("ATTRIBUTE_TITLE", textInfo.ToTitleCase(DsPreview.Tables[_familyID].Columns[j].Caption));
                                    HTMLAtts1 = HTMLAtts1 + _stmpl_records2.ToString();


                                    if (dsprecapcol == "DESCRIPTION")
                                    {
                                        _stmpl_records1.SetAttribute("PROD_DESCRIPTION", DsPreview.Tables[_familyID].Rows[i]["DESCRIPTION"].ToString());
                                        _stmpl_records3.SetAttribute("PROD_DESCRIPTION", DsPreview.Tables[_familyID].Rows[i]["DESCRIPTION"].ToString());
                                        flgdescchk = true;
                                    }
                                    if (dsprecapcol == "DESCRIPTION" && DsPreview.Tables[_familyID].Rows[i][j].ToString() != string.Empty)
                                    {
                                        prodedesc = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                        flgdeschk_bbpp = true;
                                    }

                                }

                            }


                        }

                    }



                    if (HTMLAtts_group != "")
                    {
                        _stmpl_records1_group = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Group_row");

                        _stmpl_records1_group.SetAttribute("GROUPHEADER", HTMLAtts_group);
                        _stmpl_records1_group.SetAttribute("GROUPDETAILS", HTMLAtts_group_details);
                        HTMLHeaderStr_group = _stmpl_records1_group.ToString();


                    }
                    //stopwatch.Stop();

                    //objErrorHandler.CreateLog("For description" + "=" + stopwatch.Elapsed );

                    if (!(flgdescchk))
                    {
                        _stmpl_records1.SetAttribute("PROD_DESCRIPTION", family_name.Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""));
                        _stmpl_records3.SetAttribute("PROD_DESCRIPTION", family_name.Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""));

                    }

                    if (!(flgdeschk_bbpp))
                    {
                        if (tempPriceDr != null && tempPriceDr.Length > 0)
                            prodedesc = tempPriceDr[0]["PRODUCT_DESC"].ToString();
                    }

                    _stmpl_records1.SetAttribute("ATTRIBUTE_VALUES", HTMLAtts);
                    _stmpl_records3.SetAttribute("ATTRIBUTE_VALUES", HTMLAtts1);

                    if (BindToST == true)
                    {
                        productcnt++;
                        HTMLProducts = HTMLProducts + _stmpl_records1.ToString();
                        HTMLProducts1 = HTMLProducts1 + _stmpl_records3.ToString();
                    }

                    if (prodedesc.Length > 0)
                        prodcodedesc = prodcodedesc + _ProCode + " – " + prodedesc + "|";
                    else
                        prodcodedesc = prodcodedesc + _ProCode + "|";


                    DsPreview.Tables[_familyID].Rows[i]["BindToST"] = BindToST;

                }
                //stopwatch.Stop();
                //objErrorHandler.CreateLog("Second For Loop:" + "=" + stopwatch.Elapsed);
                HttpContext.Current.Session["prodcodedesc"] = prodcodedesc;


                string stmplrecordstem = string.Empty;
                string stmplrecordstem1 = string.Empty;

                stmplrecordstem = "Csfamilypage" + "\\" + "FilterRow";
                stmplrecordstem1 = "Csfamilypage" + "\\" + "FilterRow1";
                _stg_container1 = new StringTemplateGroup("main", _SkinRootPath);
                _stg_container2 = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records4 = new StringTemplateGroup("cell", _SkinRootPath);
                _stg_records5 = new StringTemplateGroup("cell", _SkinRootPath);

                string dsprecapcol2 = string.Empty;
                int filtercount = 0;
                List<string> filterlist = new List<string>();

                for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                {
                    int l = 1;
                    dsprecapcol2 = string.Empty;
                    dsprecapcol2 = DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper();
                    if (dsprecapcol2 != "COST"
                        && dsprecapcol2 != "CODE"
                        && dsprecapcol2 != "TWEB IMAGE1"
                        && dsprecapcol2 != "PRODUCT_ID"
                        && dsprecapcol2 != "FAMILY_ID"
                         && dsprecapcol2 != "DESCRIPTION" && dsprecapcol2 != "PROD_CODE" && dsprecapcol2 != "PRICE TYPE" && !dsprecapcol2.Trim().StartsWith("NOTE") && dsprecapcol2 != "BINDTOST")
                    {
                        //DataTable sqlDT = DsPreview.Tables[_familyID].Select("ColumnName = 'CODE'").CopyToDataTable();

                        ////row filter property
                        //sqlDT.DefaultView.RowFilter = "ColumnName = 'Foo'";
                        //sqlDT = sqlDT.DefaultView.ToTable();
                        //// DataRow[] d = DsPreview.Tables[_familyID].AsEnumerable().Where(r => r.Field<string>("ColumnName") == "CODE");
                        //DsPreview.Tables[_familyID].Defa  ultView.RowFilter =" DsPreview.Tables[_familyID].Columns[j].Caption != ''";
                        //DataTable dt = (DsPreview.Tables[_familyID].DefaultView).ToTable();
                        //DataRow[] dr = DsPreview.Tables[_familyID].Select(DsPreview.Tables[_familyID].Columns[j]+"='" + DsPreview.Tables[_familyID].Columns[j].Caption + "'");


                        _stmpl_recordsrows = _stg_container1.GetInstanceOf("Csfamilypage" + "\\" + "FilterMain");
                        _stmpl_recordsrows1 = _stg_container2.GetInstanceOf("Csfamilypage" + "\\" + "FilterMain1");
                        _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", textInfo.ToTitleCase(dsprecapcol2.ToLower()));
                        _stmpl_recordsrows1.SetAttribute("TBT_ATTRIBUTE_TITLE", textInfo.ToTitleCase(dsprecapcol2.ToLower()));
                        _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE_ID", Regex.Replace(textInfo.ToTitleCase(dsprecapcol2.ToLower()), @"[^a-zA-Z]+", ""));
                        _stmpl_recordsrows1.SetAttribute("TBT_ATTRIBUTE_TITLE_ID", Regex.Replace(textInfo.ToTitleCase(dsprecapcol2.ToLower()), @"[^a-zA-Z]+", ""));
                        string HTMLAtts2 = string.Empty;
                        string HTMLAtts4 = string.Empty;
                        List<string> firstlist = new List<string>();
                        DataRow[] dr = DsPreview.Tables[_familyID].Select("BindToST=true");
                        for (int k = 0; k < dr.Length; k++)
                        {
                            string dsprecapcol1 = string.Empty;
                            dsprecapcol1 = DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper();


                            if (dsprecapcol2 == dsprecapcol1)
                            {
                                //string attribute_value = dr[k][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                string attribute_value = dr[k][j].ToString();
                                string attribute_value2 = attribute_value;
                                if (attribute_value.Length > 20)
                                {
                                    attribute_value = attribute_value.Substring(0, 20) + "...";
                                }
                                string attribute_value1 = attribute_value.ToUpper();
                                if (!firstlist.Contains(attribute_value1) && attribute_value1.Trim().Length > 0)
                                {
                                    _stmpl_records4 = _stg_records4.GetInstanceOf(stmplrecordstem);
                                    _stmpl_records5 = _stg_records5.GetInstanceOf(stmplrecordstem1);
                                    firstlist.Add(attribute_value1);
                                    //DataRow[] dr2 = DsPreview.Tables[_familyID].Select("BindToST=true and [" + dsprecapcol1 + "] = '" + attribute_value2 + "'");
                                    DataRow[] dr2 = null;
                                    if (attribute_value2.Contains("[") == true)
                                    {
                                        string[] attval = attribute_value2.Split('[');
                                        attribute_value2 = attval[0];
                                    }
                                    if (attribute_value2.Length > 30)
                                    {
                                        //attribute_value = attribute_value2.Substring(0, 20) + "...";
                                       
                                        dr2 = DsPreview.Tables[_familyID].Select("BindToST=true and [" + dsprecapcol1 + "] LIKE '" + attribute_value2.Substring(0, 30) + "%'");

                                    }
                                    else
                                    {
                                        dr2 = DsPreview.Tables[_familyID].Select("BindToST=true and [" + dsprecapcol1 + "] = '" + attribute_value2 + "'");

                                    }
                                    _stmpl_records4.SetAttribute("ATTRIBUTE_VALUE_CNT", dr2.Length);
                                    _stmpl_records5.SetAttribute("ATTRIBUTE_VALUE_CNT", dr2.Length);
                                    _stmpl_records4.SetAttribute("ATTRIBUTE_SELECTED", false);
                                    _stmpl_records5.SetAttribute("ATTRIBUTE_SELECTED", false);
                                    _stmpl_records4.SetAttribute("ATTRIBUTE_VALUE", attribute_value);
                                    _stmpl_records5.SetAttribute("ATTRIBUTE_VALUE", attribute_value);
                                    _stmpl_records4.SetAttribute("ATTRIBUTE_NAME", HttpUtility.HtmlEncode(attribute_value.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")));
                                    _stmpl_records5.SetAttribute("ATTRIBUTE_NAME", HttpUtility.HtmlEncode(attribute_value.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")));
                                    _stmpl_records4.SetAttribute("ATTRIBUTE_TITLE", DsPreview.Tables[_familyID].Columns[j].Caption);
                                    _stmpl_records5.SetAttribute("ATTRIBUTE_TITLE", DsPreview.Tables[_familyID].Columns[j].Caption);
                                    HTMLAtts2 = HTMLAtts2 + _stmpl_records4.ToString();
                                    HTMLAtts4 = HTMLAtts4 + _stmpl_records5.ToString();
                                }
                            }



                        }

                        _stmpl_recordsrows.SetAttribute("ATTRIBUTE_VALUES", HTMLAtts2);
                        _stmpl_recordsrows1.SetAttribute("ATTRIBUTE_VALUES", HTMLAtts4);
                        if (firstlist.Count > 1)
                        {
                            filtercount++;
                            filterlist.Add(dsprecapcol2);
                            _stmpl_recordsrows.SetAttribute("FILTER_SHOW", true);
                            _stmpl_recordsrows1.SetAttribute("FILTER_SHOW", true);
                        }
                        else
                        {
                            _stmpl_recordsrows.SetAttribute("FILTER_SHOW", false);
                            _stmpl_recordsrows1.SetAttribute("FILTER_SHOW", false);
                        }
                        HTMLAtts3 += _stmpl_recordsrows.ToString();
                        HTMLAtts5 += _stmpl_recordsrows1.ToString();
                    }

                }

                
                HttpContext.Current.Session["FamilyFilterProducts"] = DsPreview;
                //objErrorHandler.CreateLog("DsPreview table count "+ DsPreview.Tables.Count);
                //objErrorHandler.CreateLog(_familyID + " rows count " + DsPreview.Tables[_familyID].Rows.Count);

                bool filtershow = false;

                if (filtercount > 0 && productcnt > 1 && _familyID.ToString().Trim() == famId.ToString().Trim())
                {
                    filtershow = true;
                    HttpContext.Current.Session["filterlist"] = filterlist;
                }

                if ((showheader == true) || (HttpContext.Current.Request.Browser.IsMobileDevice == true))
                {
                    _stmpl_container = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Promain");

                    //if (Convert.ToInt16(HttpContext.Current.Session[_familyID + "Icnt"].ToString()) == 1)
                    //{
                    if (HTMLHeaderStr_group != "")
                    {
                        _stmpl_container.SetAttribute("IS_COMMON_VALUE", HTMLHeaderStr_group);
                        _stmpl_container.SetAttribute("PRODUCT_DETAILS_HEAD", HTMLHeaderStr_group);
                    }

                    _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts + " </tbody>");
                    _stmpl_container.SetAttribute("PRODUCT_DETAILS1", HTMLProducts1);
                    _stmpl_container.SetAttribute("PRODUCT_FILTER", HTMLAtts3);
                    _stmpl_container.SetAttribute("PRODUCT_FILTER1", HTMLAtts5);
                    _stmpl_container.SetAttribute("PRODUCT_FILTER_SHOW", filtershow);
                    if (_familyID.ToString().Trim() == famId.ToString().Trim())
                    {
                        _stmpl_container.SetAttribute("FILTER_PRODUCTS", true);
                    }
                    else
                    {
                        _stmpl_container.SetAttribute("FILTER_PRODUCTS", false);
                    }
                    //}
                    //else
                    //{
                    //    _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts );
                    //}
                }
                else
                {

                    _stmpl_container = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Promain_dyn");
                    _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts.Replace("mythead", "thead_none") + " </tbody>");
                    _stmpl_container.SetAttribute("PRODUCT_DETAILS1", HTMLProducts1.Replace("mythead", "thead_none"));
                    _stmpl_container.SetAttribute("PRODUCT_FILTER", HTMLAtts3);
                    _stmpl_container.SetAttribute("PRODUCT_FILTER1", HTMLAtts5);
                    _stmpl_container.SetAttribute("PRODUCT_FILTER_SHOW", filtershow);
                    if (_familyID.ToString().Trim() == famId.ToString().Trim())
                    {
                        _stmpl_container.SetAttribute("FILTER_PRODUCTS", true);
                    }
                    else
                    {
                        _stmpl_container.SetAttribute("FILTER_PRODUCTS", false);
                    }
                }
            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog("GenerateHorizontalHTMLJson---" + ex.ToString());
                return "";
            }

            return _stmpl_container.ToString();

        }


        public string SortProductsList(string _familyID, DataSet Ds, DataSet dsPriceTableAll, DataSet EADs, string Rawurl_dy, IList<DataClass> array, string screen)
        {
            string HTMLProducts = string.Empty;
            string rtnstr = string.Empty;
            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_container1 = null;
            StringTemplateGroup _stg_container2 = null;
            StringTemplateGroup _stg_records = null;
            //var results = JsonConvert.DeserializeObject(array.ToString());

            Ds = (DataSet)HttpContext.Current.Session["FamilyFilterProducts"];

            //System.Reflection.PropertyInfo pi = array.GetType().GetProperty("option");
            //String name = (String)(pi.GetValue(array, null));

            // Type myType = array.GetType();
            //IList<DataClass> props = new List<DataClass>(myType.GetProperties());

            //foreach (DataClass prop in props)
            //{
            //    object propValue = prop.GetValue(array, null);

            //    // Do something with propValue
            //}

            int productcnt = 0;


            // Newtonsoft.Json.Linq.JObject a = Newtonsoft.Json.Linq.JObject.Parse(array);
            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_records_group = null;
            StringTemplate _stmpl_records1 = null;

            StringTemplate _stmpl_records1_group = null;

            StringTemplate _stmpl_records2 = null;
            StringTemplate _stmpl_records3 = null;
            StringTemplate _stmpl_records4 = null;
            StringTemplate _stmpl_recordsrows = null;
            string HTMLAtts3 = string.Empty;


            //StringTemplate _stmpl_recordsrows = null;
            //  TBWDataList[] lstrecords = new TBWDataList[0];
            // TBWDataList[] lstrows = new TBWDataList[0];

            // StringTemplateGroup _stg_container1 = null;
            StringTemplateGroup _stg_records1 = null;
            StringTemplateGroup _stg_records2 = null;
            StringTemplateGroup _stg_records3 = null;
            StringTemplateGroup _stg_records4 = null;
            //  TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            //  TBWDataList1[] lstrows1 = new TBWDataList1[0];
            try
            {

                DataSet dsBgDisc = new DataSet();
                // decimal untPrice = 0;
                string AttrID = string.Empty;
                //  string HypColumn = "";
                //  int Min_ord_qty = 0;
                //  int Qty_avail;
                //  int flagtemp = 0;
                string _StockStatus = "NO STATUS AVAILABLE";
                string _prod_stk_Status = "0";
                string _prod_stk_flag = "0";
                string _AvilableQty = "0";
                string _eta = string.Empty;
                string _Category_id = string.Empty;
                string _EA_Path = string.Empty;
                string StrPriceTable = string.Empty;
                string CATEGORY_PATH = string.Empty;
                DataRow[] tempPriceDr;

                //DataTable tempPriceDt;
                //int ProdID;
                //  int AttrType;
                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (userid == "" || userid == null)
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];
                //userid = "999";// ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

                // string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
                //  string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
                string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
                string _parentFamily_Id = "0";
                // string CartImgPathdisplay = "";
                string _ProCode = string.Empty;
                string family_name = string.Empty;
                if (EComState == "YES")
                    if (!objHelperServices.GetIsEcomEnabled(userid))
                        EComState = "NO";
                StringBuilder strBldr = new StringBuilder();
                StringBuilder strBldrcost = new StringBuilder();

                if (HttpContext.Current.Request.QueryString["path"] != null)
                    _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());

                if (HttpContext.Current.Request.QueryString["cid"] != null)
                    _Category_id = HttpContext.Current.Request.QueryString["cid"];

                DsPreview = Ds;
                //objErrorHandler.CreateLog("DsPreview table count" + DsPreview.Tables.Count);
                if (DsPreview == null || DsPreview.Tables.Count == 0 || DsPreview.Tables[_familyID] == null)
                    return "";


                DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
                if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                    _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

                if (_parentFamily_Id == "0")
                    _parentFamily_Id = _familyID;

                pricecode = GetPriceCode();

                if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                    family_name = _parentFamilyds.Tables[0].Rows[0]["FAMILY_NAME"].ToString();
                else
                    family_name = HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"].ToString();

                string _SkinRootPath = string.Empty;

                _SkinRootPath = HttpContext.Current.Server.MapPath("~\\Templates");

                _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);
                _stg_records2 = new StringTemplateGroup("cell", _SkinRootPath);
                _stg_records3 = new StringTemplateGroup("row", _SkinRootPath);

                string HTMLAtts = string.Empty;
                string HTMLAtts1 = string.Empty;
                string HTMLAtts_group = string.Empty;
                string HTMLAtts_group_details = string.Empty;

                string HTMLProducts1 = string.Empty;
                string HTMLHeaderStr = string.Empty;
                string HTMLHeaderStr_group = string.Empty;
                bool BindToST = true;
                // int ictrecords = 0;
                string costtype = string.Empty;
                HTMLAtts = "";
                string DsPreviewcaption = string.Empty;
                DataSet dssimilarcolumns = new DataSet();
                // stopwatch.Start();
                //DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                //if (tempdr.Length > 0 && objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString())!=3)
                //{
                //stopwatch.Stop();
                //objErrorHandler.CreateLog("First For Loop:" + "=" + stopwatch.Elapsed );
                bool bindheader = objHelperDb.CheckFamilyPAGE_Discontinued(_familyID.ToString());
                // objErrorHandler.CreateLog(bindheader.ToString() + _familyID);
                //if (bindheader == true)
                //{
                //    HTMLHeaderStr = _stmpl_records1.ToString();
                //}




                bool showheader = true;
                //if (HttpContext.Current.Session["hfprevfid"] == null)
                //{
                //    HttpContext.Current.Session["hfprevfid"] = _familyID;
                //}
                //else if (HttpContext.Current.Session["hfprevfid"].ToString() != _familyID)
                //{
                //    HttpContext.Current.Session["hfprevfid"] = _familyID;
                //}
                //else
                //{
                //    showheader = false;
                //}


                //  DisplayHeaders = true;
                //if ((showheader))
                //{
                HTMLProducts = HTMLProducts + HTMLHeaderStr + "<tbody class=\"tablebutton\" id=\"myTable\">";
                //}
                //else
                //{
                //    HTMLProducts = HTMLProducts +  "<tbody class=\"tablebutton\" id=\"myTable\">";

                //}
                string ValueFortag = string.Empty;
                //bool rowcolor = false;

                if (_EA_Path == string.Empty && _Category_id == string.Empty)
                {
                    DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                    if (tmpds != null && tmpds.Tables.Count > 0)
                    {
                        _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                        string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch=Family Id=" + _familyID.ToString();
                        _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
                    }
                }

                _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
                _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);
                _stg_records2 = new StringTemplateGroup("cell", _SkinRootPath);
                _stg_records3 = new StringTemplateGroup("row", _SkinRootPath);
                _stg_container = new StringTemplateGroup("main", _SkinRootPath);
                string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();

                string prodcodedesc = string.Empty;
                string prodedesc = string.Empty;

                string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));

                //Added on 18 July
                string HREFURL = string.Empty;
                string conspath = string.Empty;


                string rawurl = string.Empty;

                if (Rawurl_dy != "")
                {
                    rawurl = Rawurl_dy;
                }
                else
                {
                    rawurl = HttpContext.Current.Request.RawUrl.ToString().Replace("/fl/", "");
                }
                string[] rev = rawurl.Split('/');
                Array.Reverse(rev);
                if (!rawurl.Contains("SortProduct"))
                {
                    if (rev[0].Contains("wa"))
                    {


                        conspath = (rev.Length > 2 ? rev[2] : " ") + "////" + (rev.Length > 3 ? rev[3] : " ") + "////" + (rev.Length > 4 ? rev[4] : " ");



                    }
                    else
                    {
                        conspath = (rev.Length > 1 ? rev[1] : " ") + "////" + (rev.Length > 2 ? rev[2] : " ") + "////" + (rev.Length > 3 ? rev[3] : " ");
                    }
                }
                //}

                // stopwatch.Start();

                string templatepath = "Csfamilypage\\Prorow2";
                string templatepath1 = "Csfamilypage\\Prorow3";
                string tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch=Family Id=" + _parentFamily_Id.ToString()));


                DataTable dt1 = DsPreview.Tables[_familyID].Copy();
                DataRow[] dr = null;
                List<string> firstlist = new List<string>();
                List<DataClass> filterSelectedList = new List<DataClass>();
                
                IList<DataClass> array1 = new List<DataClass>();
                foreach (var item in array) // Loop through List with foreach
                {
                    var options1 = item.option;


                    if (!firstlist.Contains(item.option))
                    {
                        firstlist.Add(item.option);
                        var str = string.Empty;
                        List<string> firstlist1 = new List<string>();
                        int x = 0;
                        foreach (var item1 in array) // Loop through List with foreach
                        {
                            var options2 = item.option;

                            if (item.option == item1.option)
                            {
                                filterSelectedList.Add(new DataClass() { option = item1.option, value = item1.value });
                                objErrorHandler.CreateLog("value "+ item1.value);
                                //var vallll = filterSelectedList.Find(val => val.option.Contains("Colour"));
                                if (x == 0)
                                {
                                    str += item1.value;
                                }
                                else
                                {
                                    str += "####" + item1.value;
                                }
                                x++;
                            }

                        }
                        //objErrorHandler.CreateLog("str " + str);
                        array1.Add(new DataClass { option = options1, value = str });
                    }
                }

                DataTable dtnew = new DataTable();
                dtnew = dt1.Copy();
                foreach (var item in array1) // Loop through List with foreach
                {
                    // string[] value1 = item.value.ToString().Split(",");
                    string[] value1 = item.value.ToString().Split(new string[] { "####" }, StringSplitOptions.None);

                    string list = item.option;
                    string query = string.Empty;
                    //string[] columnssplit1 = list.Split(' ');
                    string list1 = item.option;
                    //if (columnssplit1.Length > 1)
                    //{
                    //    list1 = columnssplit1[0].ToUpper() + "_" + columnssplit1[1].ToUpper();
                    //    for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                    //    {
                    //        string dsprecap = string.Empty;
                    //        dsprecap = DsPreview.Tables[_familyID].Columns[j].Caption;
                    //        objErrorHandler.CreateLog("dsprecap " + dsprecap);
                    //        objErrorHandler.CreateLog("list " + list);
                    //        if (dsprecap.ToString().Trim().ToUpper() == list.ToString().Trim().ToUpper())
                    //        {
                    //            objErrorHandler.CreateLog("dsprecap " + dsprecap);
                    //            //dr = DsPreview.Tables[_familyID].Select(DsPreview.Tables[_familyID].Columns[j] + "='" + value + "'");
                    //            string[] columnssplit = dsprecap.Split(' ');
                    //            if (columnssplit.Length > 1)
                    //            {
                    //                dtnew.Columns[dsprecap].ColumnName = columnssplit[0] + "_" + columnssplit[1];
                    //            }
                    //        }


                    //    }
                    //}

                    //objErrorHandler.CreateLog("value1 length " + value1.Length);
                    for (int x = 0; x < value1.Length; x++)
                    {
                        string value2 = string.Empty;
                        string coloumnname = "[" + list1 + "]";
                        if (x > 0)
                        {
                            query += " or ";
                        }
                        if (value1[x].EndsWith("..."))
                        {
                            //value2 = value1[x].Substring(0, value1[x].Length - 3);
                            value2 = value1[x].Substring(0, value1[x].Length - 3).Replace("<br/>", "\r").Replace("&nbsp;", "\n");
                            query += coloumnname + " LIKE '" + value2 + "%'";
                        }
                        else
                        {
                           // value2 = value1[x];
                            value2 = value1[x].Replace("<br/>", "\r").Replace("&nbsp;", "\n"); 
                            query += coloumnname + "='" + value2 + "'";
                        }
                    }
                    //objErrorHandler.CreateLog(query);
                    dr = dtnew.Select(query);
                    // dtnew.Clear();
                    dtnew = new DataTable();
                    dtnew = dt1.Clone();
                    if (dr.Length > 0)
                        dtnew = dr.CopyToDataTable();
                }


                //string[] columnssplit1 = list.Split(' ');
                //if (columnssplit1.Length > 1)
                //{
                //    list1 = columnssplit1[0] + "_" + columnssplit1[1];
                //    for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                //    {
                //        string dsprecap = string.Empty;
                //        dsprecap = DsPreview.Tables[_familyID].Columns[j].Caption;
                //        if (dsprecap == list)
                //        {
                //            //dr = DsPreview.Tables[_familyID].Select(DsPreview.Tables[_familyID].Columns[j] + "='" + value + "'");
                //            string[] columnssplit = dsprecap.Split(' ');
                //            if (columnssplit.Length > 1)
                //            {
                //                dt1.Columns[dsprecap].ColumnName = columnssplit[0] + "_" + columnssplit[1];
                //            }
                //        }


                //    }
                //}

                //objErrorHandler.CreateLog("list1 " + list1);
                //objErrorHandler.CreateLog(HttpUtility.HtmlDecode(list1));
                //objErrorHandler.CreateLog("value " + item.value);
                //if (item.value == "all")
                //{
                //    dr = dt1.Select("FAMILY_ID='" + _familyID.ToString() + "'");
                //}
                //else if (item.value.EndsWith("..."))
                //{
                //    string value1 = item.value.Substring(0, item.value.Length - 3);
                //    dr = dt1.Select(list1 + " LIKE '" + value1 + "%'");
                //}
                //else
                //{
                //    dr = dt1.Select(list1 + "='" + value + "'");
                //}

                dr = dtnew.Select("FAMILY_ID='" + _familyID.ToString() + "' and BindToST=true");

                //objErrorHandler.CreateLog("Total Products in sort" + dr.Length);
                //dtnew.Columns.Add("BindToST", typeof(bool));

                for (int i = 0; i < dr.Length; i++)
                {
                    tempPriceDr = EADs.Tables["FamilyPro"].Select("PRODUCT_ID='" + dr[i]["PRODUCT_ID"].ToString() + "'");
                    //if (tempPriceDr.Length > 0)
                    //    tempPriceDt = tempPriceDr.CopyToDataTable();
                    //else
                    //   tempPriceDt = null;

                    strBldrcost = new StringBuilder();


                    _stmpl_records1 = _stg_records.GetInstanceOf(templatepath);
                    _stmpl_records3 = _stg_records.GetInstanceOf(templatepath1);

                    _stmpl_records1.SetAttribute("PRODUCT_ID", dr[i]["PRODUCT_ID"].ToString());
                    _stmpl_records3.SetAttribute("PRODUCT_ID", dr[i]["PRODUCT_ID"].ToString());
                    _stmpl_records1.SetAttribute("FAMILY_ID", dr[i]["FAMILY_ID"].ToString());
                    _stmpl_records3.SetAttribute("FAMILY_ID", dr[i]["FAMILY_ID"].ToString());

                    _stmpl_records1.SetAttribute("PRODUCT_NAME", objProductServices.GetFamilyProductName(Convert.ToInt32(dr[i]["PRODUCT_ID"])));
                    _stmpl_records3.SetAttribute("PRODUCT_NAME", objProductServices.GetFamilyProductName(Convert.ToInt32(dr[i]["PRODUCT_ID"])));

                    //-------------------------------- cost
                    _StockStatus = "NO STATUS AVAILABLE";
                    _prod_stk_Status = "0";
                    _prod_stk_flag = "0";
                    _eta = "";
                    string ISSUBSTITUTE = "";
                    //_stmpl_records1.SetAttribute("COST", Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(dr[i]["COST"].ToString())).ToString() + " " + "<br/> Price / Stock Status");
                    // _stmpl_records1.SetAttribute("COST", Prefix + " " + dr[i]["COST"].ToString() + " " + "<br/> Price / Stock Status");
                    _stmpl_records1.SetAttribute("COST", Prefix + "" + dr[i]["COST"].ToString().Trim());
                    _stmpl_records3.SetAttribute("COST", Prefix + "" + dr[i]["COST"].ToString().Trim());
                    if (tempPriceDr != null && tempPriceDr.Length > 0)
                    {
                        _StockStatus = tempPriceDr[0]["STOCK_STATUS_DESC"].ToString().Replace("_", " ");
                        _prod_stk_Status = tempPriceDr[0]["PROD_STOCK_STATUS"].ToString();
                        _prod_stk_flag = tempPriceDr[0]["PROD_STOCK_FLAG"].ToString();
                        _AvilableQty = tempPriceDr[0]["QTY_AVAIL"].ToString();
                        _eta = tempPriceDr[0]["ETA"].ToString();
                        ISSUBSTITUTE = tempPriceDr[0]["PROD_SUBSTITUTE"].ToString();
                    }
                    if (dr[i]["Code"] != null)
                        _ProCode = dr[i]["Code"].ToString();

                    //  StrPriceTable = AssemblePriceTable(Convert.ToInt32(dr[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _prod_stk_Status, CustomerType, Convert.ToInt32(userid), _prod_stk_flag,_eta, dsPriceTableAll);
                    // _stmpl_records1.SetAttribute("PRICE_TABLE", StrPriceTable);


                    _stmpl_records1.SetAttribute("AVIL_QTY", _AvilableQty);
                    _stmpl_records3.SetAttribute("AVIL_QTY", _AvilableQty);
                    _stmpl_records1.SetAttribute("MIN_ORDER_QTY", 1);
                    _stmpl_records3.SetAttribute("MIN_ORDER_QTY", 1);




                    // string ORIGINALURL = "pd.aspx?pid=" + dr[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath;


                    //if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
                    //{
                    //    NEWURL = HttpContext.Current.Session["breadcrumEAPATH"].ToString() + "////" + dr[i]["PRODUCT_ID"].ToString() + "=" + _ProCode;
                    //    NEWURL = objHelperServices.Cons_NewURl_bybrand(ORIGINALURL, NEWURL, "pd.aspx", "");
                    //    HREFURL = "/" + NEWURL + "/pd/";
                    //}
                    //else
                    //{http://schema.org/Product
                    //    NEWURL = ORIGINALURL;
                    //    HREFURL = ORIGINALURL;

                    //}

                    //objErrorHandler.CreateLog("_procode="+_ProCode);
                    if ((_ProCode == "") || (_ProCode == string.Empty))
                    {
                        _ProCode = dr[i]["CODE"].ToString();
                    }
                    //objErrorHandler.CreateLog(dr[i]["CODE"].ToString());
                    //objErrorHandler.CreateLog("_procode1=" + _ProCode);
                    HREFURL = objHelperServices.SimpleURL_Str(WAG_Root_Path + "////" + _parentFamily_Id + "////" + dr[i]["PRODUCT_ID"].ToString() + "=" + _ProCode
         + "////" + conspath, "pd.aspx", true);




                    HREFURL = HREFURL + "/pd/";


                    _stmpl_records1.SetAttribute("PARENT_FAMILY_ID", _parentFamily_Id);
                    _stmpl_records3.SetAttribute("PARENT_FAMILY_ID", _parentFamily_Id);
                    _stmpl_records1.SetAttribute("CAT_ID", _Category_id);
                    _stmpl_records3.SetAttribute("CAT_ID", _Category_id);
                    _stmpl_records1.SetAttribute("EA_PATH", tempEAPath);
                    _stmpl_records3.SetAttribute("EA_PATH", tempEAPath);
                    _stmpl_records1.SetAttribute("URL_RW_PATH", HREFURL);
                    _stmpl_records3.SetAttribute("URL_RW_PATH", HREFURL);

                    _stmpl_records1.SetAttribute("BUY_FAMILY_ID", _familyID);
                    _stmpl_records3.SetAttribute("BUY_FAMILY_ID", _familyID);
                    // objHelperServices.SimpleURL_Str(NEWURL, "pd.aspx");
                    //objErrorHandler.CreateLog(_StockStatus + dr[i]["CODE"].ToString());

                    bool _Tbt_Stock_Status_2 = false;
                    string _Tbt_Stock_Status = string.Empty;
                    string _Tbt_Stock_Status_1 = string.Empty;
                    string _Tbt_Stock_Status_3 = string.Empty;
                    //string _Colorcode1 = string.Empty;
                    //string _Colorcode;
                    string _StockStatusTrim = _StockStatus.Trim();
                    string _Eta = string.Empty;
                    if (_eta != string.Empty)
                    {
                        _Eta = string.Format("<div class=\"inlineblk mob_sm_txt\"><b>ETA: </b> " + _eta.ToString() + "</div>");
                       // objErrorHandler.CreateLog(_Eta);
                    }

                    // objErrorHandler.CreateLog(_StockStatus.Trim());
                    switch (_StockStatus.Trim())
                    {
                        case "IN STOCK":
                            _Tbt_Stock_Status = "INSTOCK";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "Limited Stock, Please Call":
                            _Tbt_Stock_Status = "Limited Stock";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "SPECIAL ORDER":
                            _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                            _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "SPECIAL ORDER PRICE &":
                            _Tbt_Stock_Status = "SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "DISCONTINUED":
                            _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                            _Tbt_Stock_Status_2 = false;
                            break;
                        case "DISCONTINUED NO LONGER AVAILABLE":
                            _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                            _Tbt_Stock_Status_2 = false;
                            break;
                        case "DISCONTINUED NO LONGER":
                            _Tbt_Stock_Status_3 = "DISCONTINUED NO LONGER AVAILABLE";
                            _Tbt_Stock_Status_2 = false;
                            break;
                        case "TEMPORARY UNAVAILABLE":
                            _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                            _Tbt_Stock_Status_2 = false;
                            break;
                        case "TEMPORARY UNAVAILABLE NO ETA":
                            _Tbt_Stock_Status_3 = "TEMPORARY UNAVAILABLE NO ETA";
                            _Tbt_Stock_Status_2 = false;
                            break;
                        case "OUT OF STOCK":
                            _Tbt_Stock_Status_3 = "Out of stock. ";
                            _Tbt_Stock_Status_1 = " Back Order Item";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                            _Tbt_Stock_Status_3 = "Out of stock. ";
                            _Tbt_Stock_Status_1 = " Back Order Item";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "OUT OF STOCK ITEM WILL":
                            _Tbt_Stock_Status_3 = "Out of stock. ";
                            _Tbt_Stock_Status_1 = " Back Order Item";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "Please Call":
                            _Tbt_Stock_Status_3 = "Please Call";
                            _Tbt_Stock_Status_2 = true;
                            break;
                        case "Please_Call":
                            _Tbt_Stock_Status_3 = "Please Call";
                            _Tbt_Stock_Status_2 = true;
                            break;

                        default:
                            _Tbt_Stock_Status = _StockStatusTrim;
                            _Tbt_Stock_Status_2 = false;
                            break;
                    }

                    string stkstatus = string.Empty;
                    stkstatus = _Tbt_Stock_Status.ToUpper();
                    string stkstatus1 = string.Empty;
                    stkstatus1 = _Tbt_Stock_Status_1.ToUpper();

                    BindToST = true;
                    //objErrorHandler.CreateLog("_Tbt_Stock_Status_2" + _Tbt_Stock_Status_2 + _ProCode);
                    if ((_Tbt_Stock_Status_2))
                    {
                        try
                        {

                            if ((_StockStatus.ToUpper().Contains("OUT OF STOCK") == true || _StockStatus.ToUpper().Contains("PLEASE CALL") == true || _StockStatus.ToUpper().Contains("SPECIAL ORDER") == true) && ISSUBSTITUTE.ToString().Trim() != "" && _prod_stk_Status == "False" && _prod_stk_flag.ToString().Trim() == "-2")
                            {
                                // DataTable rtntbl = objProductServices.GetSubstituteProductDetails(ISSUBSTITUTE.ToString().Trim(), Convert.ToInt32(userid));
                                DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_ProCode, Convert.ToInt32(userid), "pd");
                                if (rtntbl != null && rtntbl.Rows.Count > 0)
                                {

                                    bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                                    bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

                                    if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                    {

                                        _stmpl_records1.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records3.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records1.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                        _stmpl_records3.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                        string strurl = rtntbl.Rows[0]["ea_path"].ToString();
                                        _stmpl_records1.SetAttribute("TBT_REP_EA_PATH", strurl);
                                        _stmpl_records3.SetAttribute("TBT_REP_EA_PATH", strurl);
                                    }
                                }

                            }
                            else
                            {
                                _stmpl_records1.SetAttribute("SHOW_BUY", true);
                                _stmpl_records3.SetAttribute("SHOW_BUY", true);
                            }
                        }
                        catch (Exception ex)
                        {
                            objErrorHandler.CreateLog(ex.ToString());
                        }
                    }
                    else
                    {
                        try
                        {
                            // objErrorHandler.CreateLog("ISSUBSTITUTE" + "= " + ISSUBSTITUTE + "_prod_stk_Status" + _prod_stk_Status + "_prod_stk_flag" + _prod_stk_flag + ProdID);
                            //&& _prod_stk_Status == "0" modified by indu on apr-5-2018
                            if (ISSUBSTITUTE.ToString().Trim() != "" && _prod_stk_flag.ToString().Trim() == "-2")
                            {
                                DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_ProCode.ToString().Trim(), Convert.ToInt32(userid), "pd");

                                if (rtntbl != null && rtntbl.Rows.Count > 0)
                                {

                                    bool samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];

                                    bool samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];

                                    //objErrorHandler.CreateLog("samecodenotFound" + samecodenotFound);
                                    //  objErrorHandler.CreateLog("ea_path" + rtntbl.Rows[0]["ea_path"].ToString());
                                    if (samecodenotFound == false && rtntbl.Rows[0]["ea_path"].ToString() != "")
                                    {
                                        //  objErrorHandler.CreateLog("samecodenotFound" + samecodenotFound);
                                        BindToST = true;
                                        if (_StockStatus.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
                                        {
                                            // objErrorHandler.CreateLog("inside substitute prod" + rtntbl.Rows[0]["SubstuyutePid"].ToString());
                                            HelperDB objhelperDb = new HelperDB();
                                            DataSet Sqltbs = objhelperDb.GetProductPriceEA("", rtntbl.Rows[0]["SubstuyutePid"].ToString(), userid);
                                            if (Sqltbs != null)
                                            {

                                                string stockstaus = Sqltbs.Tables[0].Rows[0]["PROD_STK_STATUS_DSC"].ToString().Trim().ToUpper();

                                                if (stockstaus == "DISCONTINUED NO LONGER AVAILABLE")
                                                {

                                                    BindToST = false;
                                                }
                                                else
                                                {
                                                    BindToST = true;
                                                }
                                            }
                                        }
                                        _stmpl_records1.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records3.SetAttribute("TBT_SUB_PRODUCT", true);
                                        _stmpl_records1.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                        _stmpl_records3.SetAttribute("TBT_REP_NIL_CODE", rtntbl.Rows[0]["wag_product_code"].ToString());
                                        string strurl = rtntbl.Rows[0]["ea_path"].ToString();
                                        _stmpl_records1.SetAttribute("TBT_REP_EA_PATH", strurl);
                                        _stmpl_records3.SetAttribute("TBT_REP_EA_PATH", strurl);
                                    }
                                    else if (_StockStatus.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
                                    {
                                        //objErrorHandler.CreateLog(_ProCode + "inside family page DISCONTINUED");
                                        BindToST = false;
                                    }
                                    else if (_StockStatus.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA" || _StockStatus.ToUpper() == "TEMPORARY_UNAVAILABLE NO ETA")
                                    {
                                        _stmpl_records1.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable. Please Call");
                                        _stmpl_records3.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable. Please Call");
                                    }
                                }
                            }

                            else
                            {

                                if ((_StockStatus.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE" || _StockStatus.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA" || _StockStatus.ToUpper() == "TEMPORARY_UNAVAILABLE NO ETA"))
                                {
                                    if (_StockStatus.ToUpper() == "DISCONTINUED NO LONGER AVAILABLE")
                                    {
                                        _stmpl_records1.SetAttribute("PRODUCT_STATUS", "Product Discontinued");
                                        _stmpl_records3.SetAttribute("PRODUCT_STATUS", "Product Discontinued");
                                        BindToST = false;
                                        //objErrorHandler.CreateLog(ProdID + "Product Discontinued" +"Familypage"); 
                                    }
                                    else if (_StockStatus.ToUpper() == "TEMPORARY UNAVAILABLE NO ETA" || _StockStatus.ToUpper() == "TEMPORARY_UNAVAILABLE NO ETA")
                                    {
                                        _stmpl_records1.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable. Please Call");
                                        _stmpl_records3.SetAttribute("PRODUCT_STATUS", "Product Temporarily Unavailable. Please Call");
                                        BindToST = true;
                                    }
                                    _stmpl_records1.SetAttribute("TBT_HIDE_BUY", true);
                                    _stmpl_records3.SetAttribute("TBT_HIDE_BUY", true);
                                }


                            }
                        }
                        catch (Exception ex)
                        {
                            objErrorHandler.CreateLog(ex.ToString());
                        }
                    }





                    _stmpl_records1.SetAttribute("STOCK_STATUS", _StockStatus.ToString().Trim());
                    _stmpl_records3.SetAttribute("STOCK_STATUS", _StockStatus.ToString().Trim());

                    //-------------------------------- Code

                    _stmpl_records1.SetAttribute("PROD_CODE", dr[i]["CODE"].ToString());
                    _stmpl_records3.SetAttribute("PROD_CODE", dr[i]["CODE"].ToString());

                    //-------------------------------- Image

                    ValueFortag = dr[i]["TWEB IMAGE1"].ToString();
                    string ValueLargeImg = string.Empty;
                    if (ValueFortag != string.Empty && ValueFortag != null && ValueFortag != "noimage.gif")
                    {
                        ValueFortag = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages" + ValueFortag.Replace("\\", "/");
                        ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200").Replace("\\", "/");
                        //FileInfo Fil;


                        //Fil = new FileInfo(strFile + ValueFortag);ob
                        //if (Fil.Exists)
                        //{

                        //    ValueFortag = "/prodimages" + ValueFortag.Replace("\\", "/");
                        //    ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200").Replace("\\", "/");
                        //}
                        //else
                        //{
                        //    ValueFortag = "/prodimages/images/noimage.gif";
                        //    ValueLargeImg = "";
                        //}
                    }
                    else
                    {
                        ValueFortag = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages/images/noimage.gif";
                        ValueLargeImg = "";
                    }
                    if (!HttpContext.Current.Request.RawUrl.ToLower().Contains("/mfl/"))
                    {

                        _stmpl_records1.SetAttribute("TWEB_LargeImg", ValueLargeImg);
                        _stmpl_records3.SetAttribute("TWEB_LargeImg", ValueLargeImg);
                    }

                    _stmpl_records1.SetAttribute("TWEB_Image", ValueFortag);
                    _stmpl_records3.SetAttribute("TWEB_Image", ValueFortag);
                    if ((ValueLargeImg != "") && (!HttpContext.Current.Request.RawUrl.ToLower().Contains("/mfl/")))
                    {
                        _stmpl_records1.SetAttribute("SHOW_DIV", true);
                        _stmpl_records3.SetAttribute("SHOW_DIV", true);
                    }
                    else
                    {
                        _stmpl_records1.SetAttribute("SHOW_DIV", false);
                        _stmpl_records3.SetAttribute("SHOW_DIV", false);
                    }

                    bool isSameLogic = true;

                    //objErrorHandler.CreateLog("PROD_STOCK_FLAG " + _prod_stk_flag.ToString() + "-" + dr[i]["PRODUCT_ID"].ToString());
                    //objErrorHandler.CreateLog("PROD_STOCK_STATUS " + _prod_stk_Status.ToString() + "-" + dr[i]["PRODUCT_ID"].ToString());
                    //objErrorHandler.CreateLog("STOCK_STATUS_DESC " + _StockStatus.ToString() + "-" + dr[i]["PRODUCT_ID"].ToString());

                    if (_prod_stk_flag == "-2" && _eta != "" && _eta.ToString().Trim() != "PLEASE CALL")
                    {
                        isSameLogic = true;
                    }
                    else if ((_prod_stk_flag == "-2" && _prod_stk_Status.ToString() == "0" && _StockStatus.ToString().Trim() == "OUT OF STOCK ITEM WILL BE BACK ORDERED") || (_prod_stk_flag.ToString() == "0" && _prod_stk_Status.ToString() == "1" && _StockStatus.ToString().Trim() == "Please Call") || (_prod_stk_flag.ToString() == "-2" && _prod_stk_Status.ToString() == "0" && _StockStatus.ToString().Trim() == "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED"))
                    {
                        //objErrorHandler.CreateLog("GetStockDetails");
                        isSameLogic = GetStockDetails(_stmpl_records1, _stmpl_records3, dr[i]["PRODUCT_ID"].ToString(), _eta.ToString());

                    }
                    else
                    {
                        isSameLogic = true;
                    }

                    //objErrorHandler.CreateLog("isSameLogic " + isSameLogic);
                    if (isSameLogic)
                    {
                        //To display description when PROD_STOCK_FLAG is -2 and eta date not expired
                        if (_prod_stk_flag == "-2" && _eta != string.Empty && _eta.ToString().Trim() != "PLEASE CALL")
                        {
                            //string etaShipDays = string.Empty;
                            //etaShipDays = "Ships Out Approx: " + _eta.ToString();
                            _Tbt_Stock_Status_1 = "";
                            _stmpl_records1.SetAttribute("TBT_STOCK_STATUS_3", "Pre-Order");
                            _stmpl_records3.SetAttribute("TBT_STOCK_STATUS_3", "Pre-Order");
                            _Eta = string.Format("<div class=\"inlineblk mob_sm_txt\">ETA: " + _eta.ToString() + "</div>");
                        }
                        else if (!_Tbt_Stock_Status.Equals(""))
                        {
                            //_stmpl_records1.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                            //_stmpl_records3.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                            //if (_Tbt_Stock_Status == "Limited Stock")
                            //{
                            //    _stmpl_records1.SetAttribute("TBT_PLEASE_CALL", "true");
                            //    _stmpl_records3.SetAttribute("TBT_PLEASE_CALL", "true");
                            //}

                            if (_Tbt_Stock_Status == "Limited Stock")
                            {
                                int soh = 0;
                                HelperDB objHelperDB = new HelperDB();
                                string tempstr = (string)objHelperDB.GetGenericDataDB(dr[i]["PRODUCT_ID"].ToString(), "GET_PRODUCT_SOH", HelperDB.ReturnType.RTString);
                                if (tempstr != null && tempstr != "")
                                    soh = Convert.ToInt32(tempstr);
                                //objErrorHandler.CreateLog("soh " + soh);
                                //soh = objProductServices.GetProductSOH(Convert.ToInt32(dr[i]["PRODUCT_ID"].ToString()));


                                if (soh > 4)
                                {
                                    _stmpl_records1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                                    _stmpl_records3.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                                    _stmpl_records1.SetAttribute("TBT_ISINSTOCK", true);
                                    _stmpl_records3.SetAttribute("TBT_ISINSTOCK", true);
                                    _stmpl_records1.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                                    _stmpl_records3.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                                }
                                else
                                {
                                    _stmpl_records1.SetAttribute("TBT_PLEASE_CALL", true);
                                    _stmpl_records3.SetAttribute("TBT_PLEASE_CALL", true);
                                }
                            }
                            else
                            {
                                _stmpl_records1.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                                _stmpl_records3.SetAttribute("TBT_STOCK_STATUS", _Tbt_Stock_Status);
                            }
                        }
                        else
                        {
                            _stmpl_records1.SetAttribute("TBT_STOCK_STATUS_3", _Tbt_Stock_Status_3);
                            _stmpl_records3.SetAttribute("TBT_STOCK_STATUS_3", _Tbt_Stock_Status_3);
                        }
                        //_stmpl_records.SetAttribute("TBT_COLOR_CODE_1", _Colorcode1);
                        _stmpl_records1.SetAttribute("TBT_STOCK_STATUS_1", _Tbt_Stock_Status_1);
                        _stmpl_records3.SetAttribute("TBT_STOCK_STATUS_1", _Tbt_Stock_Status_1);
                        _stmpl_records1.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                        _stmpl_records3.SetAttribute("TBT_STOCK_STATUS_2", _Tbt_Stock_Status_2);
                        _stmpl_records1.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
                        _stmpl_records3.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
                        if (stkstatus == "INSTOCK" || stkstatus1 == "INSTOCK")
                        {
                            _stmpl_records1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                            _stmpl_records3.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                            _stmpl_records1.SetAttribute("TBT_ISINSTOCK", true);
                            _stmpl_records3.SetAttribute("TBT_ISINSTOCK", true);
                            //  objErrorHandler.CreateLog("Peoduct inside instock"); 
                            _stmpl_records1.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                            _stmpl_records3.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                        }
                        else if (stkstatus == "ITEM WILL BE BACK ORDERED" || stkstatus1 == "ITEM WILL BE BACK ORDERED")
                        {
                            _stmpl_records1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/OutOfStock");
                            _stmpl_records3.SetAttribute("TBT_STOCK_HREF", "http://schema.org/OutOfStock");
                        }
                        else if (stkstatus.Contains("SPECIAL") || stkstatus1.Contains("SPECIAL"))
                        {
                            _stmpl_records1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/LimitedAvailability");
                            _stmpl_records3.SetAttribute("TBT_STOCK_HREF", "http://schema.org/LimitedAvailability");
                        }
                        else if (!(_Tbt_Stock_Status_2))
                        {
                            _stmpl_records1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/Discontinued");
                            _stmpl_records3.SetAttribute("TBT_STOCK_HREF", "http://schema.org/Discontinued");
                        }
                        else
                        {
                            _stmpl_records1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                            _stmpl_records3.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                        }
                    }


                    //  if( HttpContext.Current.Session["F_ALT_FNAME"].ToString() != "")
                    //      _stmpl_records1.SetAttribute("F_ALT_FNAME", HttpContext.Current.Session["F_ALT_FNAME"].ToString());

                    // To show Bulk Order price
                    OrderServices objOrderServices = new OrderServices();
                    DataSet Dsall = objOrderServices.getBulkBuyPrice(Convert.ToInt32(dr[i]["PRODUCT_ID"].ToString()), Convert.ToInt32(userid));
                    string bulkbuy = string.Empty;
                    string bulkbuymob = string.Empty;
                    string bulkbuymob1 = string.Empty;

                    if (Dsall != null && Dsall.Tables.Count > 0)
                    {
                        DataTable Sqltb = Dsall.Tables[0];

                        if (Sqltb.Rows.Count > 0)
                        {
                            bulkbuy = string.Format("<div class=\"inlineblock bulkbuy \" style=\"font-style:italic;\" ><span style=\"color:#1059a3;font-size:16px;font-weight:bold;\">Bulk Buy:</span><span style=\"color:#1059a3;font-size:16px;font-weight:bold;\"> $" + Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE1"].ToString()), 2) + " for " + Sqltb.Rows[0]["QTY"].ToString().Replace("+", "") + " or more</span></div>");
                            bulkbuymob = string.Format("<div class=\"bulkbuy a400  \" style=\"font-style:italic;\" ><span style=\"color:#1059a3;font-size:16px;font-weight:bold;\">Bulk Buy:</span><span style=\"color:#1059a3;font-size:16px;font-weight:bold;\"> $" + Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE1"].ToString()), 2) + " for " + Sqltb.Rows[0]["QTY"].ToString().Replace("+", "") + " or more</span></div>");
                            bulkbuymob1 = string.Format("<div class=\"bulkbuy ml0 b400 \" style=\"font-style:italic;\" ><span style=\"color:#1059a3;font-size:16px;font-weight:bold;\">Bulk Buy:</span><span style=\"color:#1059a3;font-size:16px;font-weight:bold;\"> $" + Math.Round(Convert.ToDecimal(Sqltb.Rows[0]["PRICE1"].ToString()), 2) + " for " + Sqltb.Rows[0]["QTY"].ToString().Replace("+", "") + " or more</span></div>");
                            //objErrorHandler.CreateLog(bulkbuy);
                        }
                    }
                    _stmpl_records1.SetAttribute("TBT_BULK_BUY", bulkbuy);
                    _stmpl_records3.SetAttribute("TBT_BULK_BUY", bulkbuymob);
                    _stmpl_records3.SetAttribute("TBT_BULK_BUY1", bulkbuymob1);



                    HTMLAtts = "";
                    HTMLAtts1 = "";
                    bool flgdescchk = false;
                    bool flgdeschk_bbpp = false;
                    string dsprecapcol = string.Empty;
                    //stopwatch.Start();
                    string teplatepath = "Csfamilypage\\Procell1";
                    string teplatepath1 = "Csfamilypage\\Procell1";
                    string templatepathgroup = "Csfamilypage\\Group_Procell";

                    for (int j = 1; j < dtnew.Columns.Count; j++)
                    {
                        dsprecapcol = string.Empty;
                        dsprecapcol = dtnew.Columns[j].Caption.ToUpper();
                        if (dsprecapcol != "COST"
                            && dsprecapcol != "CODE"
                            && dsprecapcol != "TWEB IMAGE1"
                            && dsprecapcol != "PRODUCT_ID"
                            && dsprecapcol != "FAMILY_ID"
                             && dsprecapcol != "BINDTOST"
                            )
                        {
                            if (dssimilarcolumns.Tables.Count == 0)
                            {
                                _stmpl_records = _stg_records.GetInstanceOf(teplatepath);
                                _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", HttpUtility.HtmlEncode(dr[i][j].ToString().Replace("\r", " ").Replace("\n", " ")));
                                _stmpl_records.SetAttribute("ATTRIBUTE_TITLE", textInfo.ToTitleCase(dtnew.Columns[j].Caption.ToLower()));
                                HTMLAtts = HTMLAtts + _stmpl_records.ToString();

                                string attribute_value = dr[i][j].ToString().Replace("\r", " ").Replace("\n", " ");
                                //if (attribute_value.Length > 14)
                                //{
                                //    attribute_value = attribute_value.Substring(0, 14) + "...";
                                //}
                                _stmpl_records2 = _stg_records.GetInstanceOf(teplatepath1);
                                _stmpl_records2.SetAttribute("ATTRIBUTE_VALUE", HttpUtility.HtmlEncode(attribute_value));
                                _stmpl_records2.SetAttribute("ATTRIBUTE_TITLE", textInfo.ToTitleCase(dtnew.Columns[j].Caption.ToLower()));
                                HTMLAtts1 = HTMLAtts1 + _stmpl_records2.ToString();


                                if (dsprecapcol == "DESCRIPTION")
                                {
                                    _stmpl_records1.SetAttribute("PROD_DESCRIPTION", dr[i]["DESCRIPTION"].ToString());
                                    _stmpl_records3.SetAttribute("PROD_DESCRIPTION", dr[i]["DESCRIPTION"].ToString());
                                    flgdescchk = true;
                                }
                                if (dsprecapcol == "DESCRIPTION" && dr[i][j].ToString() != string.Empty)
                                {
                                    prodedesc = dr[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                    flgdeschk_bbpp = true;
                                }
                            }
                            else
                            {
                                DataRow[] dr1 = dssimilarcolumns.Tables[0].Select("Attribute_name='" + dtnew.Columns[j].Caption + "'");
                                if (dr1.Length > 0)
                                {

                                    if (i == 0)
                                    {
                                        _stmpl_records_group = _stg_records.GetInstanceOf(templatepathgroup);
                                        _stmpl_records_group.SetAttribute("GROUP_ATTRIBUTE_VALUE", dr[i][j].ToString());
                                        HTMLAtts_group_details = HTMLAtts_group_details + _stmpl_records_group.ToString();
                                    }

                                }
                                else
                                {
                                    _stmpl_records = _stg_records.GetInstanceOf(teplatepath);
                                    _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", HttpUtility.HtmlEncode(dr[i][j].ToString().Replace("\r", " ").Replace("\n", " ")));
                                    _stmpl_records.SetAttribute("ATTRIBUTE_TITLE", textInfo.ToTitleCase(dtnew.Columns[j].Caption.ToLower()));
                                    HTMLAtts = HTMLAtts + _stmpl_records.ToString();

                                    string attribute_value = dr[i][j].ToString().Replace("\r", " ").Replace("\n", " ");
                                    //if (attribute_value.Length > 14)
                                    //{
                                    //    attribute_value = attribute_value.Substring(0, 14) + "...";
                                    //}
                                    _stmpl_records2 = _stg_records.GetInstanceOf(teplatepath1);
                                    _stmpl_records2.SetAttribute("ATTRIBUTE_VALUE", HttpUtility.HtmlEncode(attribute_value));
                                    _stmpl_records2.SetAttribute("ATTRIBUTE_TITLE", textInfo.ToTitleCase(dtnew.Columns[j].Caption.ToLower()));
                                    HTMLAtts1 = HTMLAtts1 + _stmpl_records2.ToString();


                                    if (dsprecapcol == "DESCRIPTION")
                                    {
                                        _stmpl_records1.SetAttribute("PROD_DESCRIPTION", dr[i]["DESCRIPTION"].ToString());
                                        _stmpl_records3.SetAttribute("PROD_DESCRIPTION", dr[i]["DESCRIPTION"].ToString());
                                        flgdescchk = true;
                                    }
                                    if (dsprecapcol == "DESCRIPTION" && dr[i][j].ToString() != string.Empty)
                                    {
                                        prodedesc = dr[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                        flgdeschk_bbpp = true;
                                    }

                                }

                            }


                        }

                    }


                    if (HTMLAtts_group != "")
                    {
                        _stmpl_records1_group = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Group_row");

                        _stmpl_records1_group.SetAttribute("GROUPHEADER", HTMLAtts_group);
                        _stmpl_records1_group.SetAttribute("GROUPDETAILS", HTMLAtts_group_details);
                        HTMLHeaderStr_group = _stmpl_records1_group.ToString();


                    }
                    //stopwatch.Stop();

                    //objErrorHandler.CreateLog("For description" + "=" + stopwatch.Elapsed );

                    if (!(flgdescchk))
                    {
                        _stmpl_records1.SetAttribute("PROD_DESCRIPTION", family_name.Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""));
                        _stmpl_records3.SetAttribute("PROD_DESCRIPTION", family_name.Replace("<sub1>", " ").Replace("</sub1>", "").Replace("<sub2>", " ").Replace("</sub2>", ""));

                    }

                    if (!(flgdeschk_bbpp))
                    {
                        if (tempPriceDr != null && tempPriceDr.Length > 0)
                            prodedesc = tempPriceDr[0]["PRODUCT_DESC"].ToString();
                    }

                    _stmpl_records1.SetAttribute("ATTRIBUTE_VALUES", HTMLAtts);
                    _stmpl_records3.SetAttribute("ATTRIBUTE_VALUES", HTMLAtts1);

                    if (BindToST == true)
                    {
                        productcnt++;
                        HTMLProducts = HTMLProducts + _stmpl_records1.ToString();
                        HTMLProducts1 = HTMLProducts1 + _stmpl_records3.ToString();
                    }

                    if (prodedesc.Length > 0)
                        prodcodedesc = prodcodedesc + _ProCode + " – " + prodedesc + "|";
                    else
                        prodcodedesc = prodcodedesc + _ProCode + "|";


                    //dtnew.Rows[i]["BindToST"] = BindToST;      //commented by smith 

                }



                string stmplrecordstem = string.Empty;
                string stmplrecordstem1 = string.Empty;


                StringTemplate _stmpl_recordsrows1 = null;
                StringTemplateGroup _stg_records5 = null;
                StringTemplate _stmpl_records5 = null;

                stmplrecordstem = "Csfamilypage" + "\\" + "FilterRow";
                stmplrecordstem1 = "Csfamilypage" + "\\" + "FilterRow1";
                _stg_container1 = new StringTemplateGroup("main", _SkinRootPath);
                _stg_container2 = new StringTemplateGroup("main", _SkinRootPath);
                _stg_records4 = new StringTemplateGroup("cell", _SkinRootPath);
                _stg_records5 = new StringTemplateGroup("cell", _SkinRootPath);

                string dsprecapcol2 = string.Empty;
                int filtercount = 0;
                string HTMLAtts5 = string.Empty;
                List<string> filterlist;
                filterlist = HttpContext.Current.Session["filterlist"] as List<string>;
                DataTable dtnew1 = new DataTable();
               
                for (int j = 1; j < dtnew.Columns.Count; j++)
                {
                    int l = 1;
                    dsprecapcol2 = string.Empty;
                    dsprecapcol2 = dtnew.Columns[j].Caption.ToUpper();
                    if (filterlist.Contains(dsprecapcol2) && dsprecapcol2 != "COST"
                        && dsprecapcol2 != "CODE"
                        && dsprecapcol2 != "TWEB IMAGE1"
                        && dsprecapcol2 != "PRODUCT_ID"
                        && dsprecapcol2 != "FAMILY_ID"
                         && dsprecapcol2 != "DESCRIPTION" && dsprecapcol2 != "PROD_CODE" && dsprecapcol2 != "PRICE TYPE" && !dsprecapcol2.Trim().StartsWith("NOTE") && dsprecapcol2 != "BINDTOST")
                    {
                        //DataTable sqlDT = DsPreview.Tables[_familyID].Select("ColumnName = 'CODE'").CopyToDataTable();

                        ////row filter property
                        //sqlDT.DefaultView.RowFilter = "ColumnName = 'Foo'";
                        //sqlDT = sqlDT.DefaultView.ToTable();
                        //// DataRow[] d = DsPreview.Tables[_familyID].AsEnumerable().Where(r => r.Field<string>("ColumnName") == "CODE");
                        //DsPreview.Tables[_familyID].Defa  ultView.RowFilter =" DsPreview.Tables[_familyID].Columns[j].Caption != ''";
                        //DataTable dt = (DsPreview.Tables[_familyID].DefaultView).ToTable();
                        //DataRow[] dr = DsPreview.Tables[_familyID].Select(DsPreview.Tables[_familyID].Columns[j]+"='" + DsPreview.Tables[_familyID].Columns[j].Caption + "'");


                        _stmpl_recordsrows = _stg_container1.GetInstanceOf("Csfamilypage" + "\\" + "FilterMain");
                        _stmpl_recordsrows1 = _stg_container2.GetInstanceOf("Csfamilypage" + "\\" + "FilterMain1");
                        _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE", textInfo.ToTitleCase(dsprecapcol2.ToLower()));
                        _stmpl_recordsrows1.SetAttribute("TBT_ATTRIBUTE_TITLE", textInfo.ToTitleCase(dsprecapcol2.ToLower()));
                        _stmpl_recordsrows.SetAttribute("TBT_ATTRIBUTE_TITLE_ID", Regex.Replace(textInfo.ToTitleCase(dsprecapcol2.ToLower()), @"[^a-zA-Z]+", ""));
                        _stmpl_recordsrows1.SetAttribute("TBT_ATTRIBUTE_TITLE_ID", Regex.Replace(textInfo.ToTitleCase(dsprecapcol2.ToLower()), @"[^a-zA-Z]+", ""));
                        string HTMLAtts2 = string.Empty;
                        string HTMLAtts4 = string.Empty;
                        List<string> firstlist1 = new List<string>();
                        //DataRow[] dr1 = dtnew.Select("BindToST=true");
                        DataRow[] dr1 = null;
                        DataRow[] dr3 = null;
                        //if (dtnew.Rows.Count > 0)
                        //{
                        //    dr1 = dtnew.Select("BindToST=true");
                        //}
                        //else
                        //{
                        //    dr1 = dt1.Select("FAMILY_ID='" + _familyID.ToString() + "'");
                        //}

                        //if (filterSelectedList.Exists(val => val.option.ToString().ToUpper() == dsprecapcol2))
                        //{
                        dr1 = dt1.Select("FAMILY_ID='" + _familyID.ToString() + "' and BindToST=true");
                        //}
                        //else
                        //{
                        //    dr1 = dtnew.Select("BindToST=true");
                        //}
                        dtnew1 = dt1.Copy();
                        foreach (var item in array1) // Loop through List with foreach
                        {
                            // string[] value1 = item.value.ToString().Split(",");
                            //if (!filterSelectedList.Exists(val => val.option.ToString().ToUpper() == dsprecapcol2.ToString().ToUpper()))
                            if (!(item.option.ToString().ToUpper() == dsprecapcol2.ToString().ToUpper()))
                            {

                                string[] value1 = item.value.ToString().Split(new string[] { "####" }, StringSplitOptions.None);

                                string list = item.option;
                                string query = string.Empty;
                                //string[] columnssplit1 = list.Split(' ');
                                string list1 = item.option;

                                //objErrorHandler.CreateLog("value123 length " + value1.Length);
                                for (int x = 0; x < value1.Length; x++)
                                {
                                    string value2 = string.Empty;
                                    string coloumnname = "[" + list1 + "]";
                                    if (x > 0)
                                    {
                                        query += " or ";
                                    }
                                    if (value1[x].EndsWith("..."))
                                    {
                                        value2 = value1[x].Substring(0, value1[x].Length - 3).Replace("<br/>", "\r").Replace("&nbsp;", "\n");
                                        query += coloumnname + " LIKE '" + value2 + "%'";
                                    }
                                    else
                                    {
                                        value2 = value1[x].Replace("<br/>", "\r").Replace("&nbsp;", "\n");
                                        query += coloumnname + "='" + value2 + "'";
                                    }
                                }
                                objErrorHandler.CreateLog(query);
                                dr3 = dtnew1.Select(query);
                                // dtnew.Clear();
                                dtnew1 = new DataTable();
                                dtnew1 = dt1.Clone();
                                if (dr3.Length > 0)
                                    dtnew1 = dr3.CopyToDataTable();
                            }
                        }

                       // objErrorHandler.CreateLog("dr count " + dr1.Length);

                        for (int k = 0; k < dr1.Length; k++)
                        {
                            string dsprecapcol1 = string.Empty;
                            dsprecapcol1 = dtnew.Columns[j].Caption.ToUpper();


                            if (dsprecapcol2 == dsprecapcol1)
                            {
                                //string attr_value = dr1[k][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                string attribute_value = dr1[k][j].ToString();
                                string attribute_value2 = attribute_value;
                                if (attribute_value.Length > 20)
                                {
                                    attribute_value = attribute_value.Substring(0, 20) + "...";
                                }
                                string attribute_value1 = attribute_value.ToUpper();
                                string attr_value = attribute_value.ToUpper().ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                                if (!firstlist1.Contains(attribute_value1) && attribute_value1.Trim().Length > 0)
                                {
                                    _stmpl_records4 = _stg_records4.GetInstanceOf(stmplrecordstem);
                                    _stmpl_records5 = _stg_records5.GetInstanceOf(stmplrecordstem1);
                                    firstlist1.Add(attribute_value1);
                                    //var vallll = filterSelectedList.Find(val => val.option.Contains("Colour"));
                                    //objErrorHandler.CreateLog(dsprecapcol1);
                                    //objErrorHandler.CreateLog(attr_value);
                                    if (filterSelectedList.Exists(val => (val.option.ToString().ToUpper() == dsprecapcol1 && val.value.ToString().ToUpper() == attr_value.ToString().ToUpper())))
                                    {
                                        //objErrorHandler.CreateLog("selected");
                                        _stmpl_records4.SetAttribute("ATTRIBUTE_SELECTED", true);
                                        _stmpl_records5.SetAttribute("ATTRIBUTE_SELECTED", true);
                                    }
                                    else
                                    {
                                        _stmpl_records4.SetAttribute("ATTRIBUTE_SELECTED", false);
                                        _stmpl_records5.SetAttribute("ATTRIBUTE_SELECTED", false);
                                    }
                                    DataRow[] dr2 = null;
                                    if (filterSelectedList.Exists(val => val.option.ToString().ToUpper() == dsprecapcol2))
                                    {
                                        //dr2 = dtnew1.Select("FAMILY_ID='" + _familyID.ToString() + "' and BindToST=true and [" + dsprecapcol1 + "] = '" + attribute_value2 + "'");
                                        if (attribute_value2.Contains("[") == true)
                                        {
                                            string[] attval = attribute_value2.Split('[');
                                            attribute_value2 = attval[0];
                                        }
                                        if (attribute_value2.Length > 30 && !attribute_value2.Contains("\n"))
                                        {
                                            //attribute_value = attribute_value2.Substring(0, 20) + "...";
                                            dr2 = dtnew1.Select("FAMILY_ID='" + _familyID.ToString() + "' and BindToST=true and [" + dsprecapcol1 + "] LIKE '" + attribute_value2.Substring(0, 30) + "%'");

                                        }
                                        else
                                        {
                                            dr2 = dtnew1.Select("FAMILY_ID='" + _familyID.ToString() + "' and BindToST=true and [" + dsprecapcol1 + "] = '" + attribute_value2 + "'");

                                        }
                                    }
                                    else
                                    {
                                        //dr2 = dtnew.Select("BindToST=true and [" + dsprecapcol1 + "] = '" + attribute_value2 + "'");
                                        if (attribute_value2.Contains("[") == true)
                                        {
                                            string[] attval = attribute_value2.Split('[');
                                            attribute_value2 = attval[0];
                                        }
                                        if (attribute_value2.Length > 30)
                                        {
                                            //attribute_value = attribute_value2.Substring(0, 20) + "...";
                                            dr2 = dtnew.Select("FAMILY_ID='" + _familyID.ToString() + "' and BindToST=true and [" + dsprecapcol1 + "] LIKE '" + attribute_value2.Substring(0, 30) + "%'");

                                        }
                                        else
                                        {
                                            dr2 = dtnew.Select("FAMILY_ID='" + _familyID.ToString() + "' and BindToST=true and [" + dsprecapcol1 + "] = '" + attribute_value2 + "'");

                                        }
                                    }



                                    //dr2 = dtnew1.Select("BindToST=true and [" + dsprecapcol1 + "] = '" + attribute_value2 + "'");
                                    //if (filterSelectedList.Exists(val => val.option.ToString().ToUpper() == dsprecapcol2))
                                    //{
                                    //    dr2 = dt1.Select("FAMILY_ID='" + _familyID.ToString() + "' and BindToST=true and ["+ dsprecapcol1 + "] = '"+ attribute_value + "'");
                                    //}
                                    //else
                                    //{
                                    //    dr2 = dtnew.Select("BindToST=true and [" + dsprecapcol1 + "] = '" + attribute_value2 + "'");
                                    //}

                                    if (dr2.Length == 0)
                                    {
                                        _stmpl_records4.SetAttribute("ATTRIBUTE_DISABLED", true);
                                        _stmpl_records5.SetAttribute("ATTRIBUTE_DISABLED", true);
                                    }
                                    else
                                    {
                                        _stmpl_records4.SetAttribute("ATTRIBUTE_DISABLED", false);
                                        _stmpl_records5.SetAttribute("ATTRIBUTE_DISABLED", false);
                                    }
                                    _stmpl_records4.SetAttribute("ATTRIBUTE_VALUE_CNT", dr2.Length);
                                    _stmpl_records5.SetAttribute("ATTRIBUTE_VALUE_CNT", dr2.Length);
                                    _stmpl_records4.SetAttribute("ATTRIBUTE_VALUE", attribute_value);
                                    _stmpl_records5.SetAttribute("ATTRIBUTE_VALUE", attribute_value);
                                    _stmpl_records4.SetAttribute("ATTRIBUTE_NAME", HttpUtility.HtmlEncode(attribute_value.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")));
                                    _stmpl_records5.SetAttribute("ATTRIBUTE_NAME", HttpUtility.HtmlEncode(attribute_value.ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;")));
                                    _stmpl_records4.SetAttribute("ATTRIBUTE_TITLE", dtnew.Columns[j].Caption);
                                    _stmpl_records5.SetAttribute("ATTRIBUTE_TITLE", dtnew.Columns[j].Caption);
                                    HTMLAtts2 = HTMLAtts2 + _stmpl_records4.ToString();
                                    HTMLAtts4 = HTMLAtts4 + _stmpl_records5.ToString();
                                }
                            }

                        }

                        _stmpl_recordsrows.SetAttribute("ATTRIBUTE_VALUES", HTMLAtts2);
                        _stmpl_recordsrows1.SetAttribute("ATTRIBUTE_VALUES", HTMLAtts4);
                        //objErrorHandler.CreateLog("firstlist1 " + firstlist1);
                        if (firstlist1.Count > 0)
                        {
                            filtercount++;
                            _stmpl_recordsrows.SetAttribute("FILTER_SHOW", true);
                            _stmpl_recordsrows1.SetAttribute("FILTER_SHOW", true);
                        }
                        else
                        {
                            _stmpl_recordsrows.SetAttribute("FILTER_SHOW", false);
                            _stmpl_recordsrows1.SetAttribute("FILTER_SHOW", false);
                        }
                        HTMLAtts3 += _stmpl_recordsrows.ToString();
                        HTMLAtts5 += _stmpl_recordsrows1.ToString();
                    }

                }
                
                bool filtershow = false;
                //&& productcnt > 1  commented since the filter is not shown if it has one product
                if ((filtercount > 0 && dt1.Rows.Count > 1) || (filterSelectedList.Count > 0 && dt1.Rows.Count > 1))
                {
                    filtershow = true;
                }
                //objErrorHandler.CreateLog(filtershow.ToString());
                if ((showheader == true) || (HttpContext.Current.Request.Browser.IsMobileDevice == true))
                {
                    _stmpl_container = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Promain");

                    //if (Convert.ToInt16(HttpContext.Current.Session[_familyID + "Icnt"].ToString()) == 1)
                    //{
                    if (HTMLHeaderStr_group != "")
                    {
                        _stmpl_container.SetAttribute("IS_COMMON_VALUE", HTMLHeaderStr_group);
                        _stmpl_container.SetAttribute("PRODUCT_DETAILS_HEAD", HTMLHeaderStr_group);
                    }
                   // objErrorHandler.CreateLog(HTMLAtts3.ToString());
                    _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts + " </tbody>");
                    _stmpl_container.SetAttribute("PRODUCT_DETAILS1", HTMLProducts1);
                    _stmpl_container.SetAttribute("PRODUCT_FILTER", HTMLAtts3);
                    _stmpl_container.SetAttribute("PRODUCT_FILTER1", HTMLAtts5);
                    _stmpl_container.SetAttribute("PRODUCT_FILTER_SHOW", filtershow);
                    _stmpl_container.SetAttribute("FILTER_PRODUCTS", true);
                    //}
                    //else
                    //{
                    //    _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts );
                    //}
                    if (dtnew != null && dtnew.Rows.Count == 0 && screen == "web")
                    {
                        string str = "<div><h3 style=\"height: 25px; margin: 70px 0 0 0; color: #333;font-weight: 700;font-size: 17px; \" > No Products Found </h3><h4 style=\"float:left;font-size: 18px;font-weight: 400; color: #333;\">Select different filter options or <a  style=\"color: #4592e0;font-size: 18px;font-weight: 400;\" onclick=\"clearfilter();\">click here to reset for current filter selections</a></div>";
                        _stmpl_container.SetAttribute("PRODUCT_FILTER", str);
                        _stmpl_container.SetAttribute("PRODUCT_FILTER1", str);
                    }
                }
                else
                {

                    _stmpl_container = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Promain_dyn");
                    _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts.Replace("mythead", "thead_none") + " </tbody>");
                    _stmpl_container.SetAttribute("PRODUCT_DETAILS1", HTMLProducts1.Replace("mythead", "thead_none"));
                    _stmpl_container.SetAttribute("PRODUCT_FILTER", HTMLAtts3);
                    _stmpl_container.SetAttribute("PRODUCT_FILTER1", HTMLAtts5);
                    _stmpl_container.SetAttribute("PRODUCT_FILTER_SHOW", filtershow);
                    _stmpl_container.SetAttribute("FILTER_PRODUCTS", true);
                    if (dtnew != null && dtnew.Rows.Count == 0 && screen == "web")
                    {
                        string str = "<div><h3 style=\"height: 25px; margin: 70px 0 0 0; color: #333;font-weight: 700;font-size: 17px; \" > No Products Found </h3><h4 style=\"float:left;font-size: 18px;font-weight: 400; color: #333;\">Select different filter options or <a  style=\"color: #4592e0;font-size: 18px;font-weight: 400;\" onclick=\"clearfilter();\">click here to reset for current filter selections</a></div>";
                        _stmpl_container.SetAttribute("PRODUCT_FILTER", str);
                        _stmpl_container.SetAttribute("PRODUCT_FILTER1", str);
                    }
                }

                //stopwatch.Stop(); 
                //objErrorHandler.CreateLog("Second For Loop:" + "=" + stopwatch.Elapsed);
                HttpContext.Current.Session["prodcodedesc"] = prodcodedesc;

                //string dsprecapcol2 = string.Empty;
                //if (dtnew != null && dtnew.Rows.Count == 0 && screen == "web")
                //{
                //    string str = "<h4 class=\"text-center\" style=\"height: 25px; margin-top: 70px; \" > No Products Found </h4>";
                //    _stmpl_container.SetAttribute("PRODUCT_FILTER", str);
                //    _stmpl_container.SetAttribute("PRODUCT_FILTER1", str);
                //}
                //else if (dr != null && dr.Length == 0 && screen == "mob")
                //{
                //    return "<div class=\"mbPro_detail_empty\"><h4 class=\"text-center\" style=\"height: 25px; margin-top: 70px; \" > No Products Found </h4></div>";
                //}
                //else if (screen == "mob")
                //{
                //    return HTMLProducts1.ToString();
                //}

            }
            catch (Exception ex)
            {
                //objErrorHandler.CreateLog("GenerateHorizontalHTMLJson---" + ex.ToString());
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return "-1";
            }

            return _stmpl_container.ToString();

        }

        //private bool GetStockDetails(StringTemplate st, StringTemplate st1, string Pid, string ETA)
        //{
        //    HelperDB objhelperDb = new HelperDB();
        //    try
        //    {
        //        string user_id = string.Empty;
        //        string order_id = string.Empty;
        //        int no = 0;
        //        int availabilty = 0;
        //        string availabilty1 = string.Empty;
        //        string sqlexec = "exec SP_CHECK_STOCK_STATUS '" + Pid.ToString() + "' ";
        //        objErrorHandler.CreateLog("sqlexec " + sqlexec);
        //        DataSet Dsall = objhelperDb.GetDataSetDB(sqlexec);
        //        //objErrorHandler.CreateLog("Row Count " + Dsall.Tables[0].Rows.Count);
        //        //objErrorHandler.CreateLog("total " + Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"]);

        //        //objErrorHandler.CreateLog("SUPPLIER_ITEM_CODE 1" + Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"]);
        //        //objErrorHandler.CreateLog("SUPPLIER_ID 1" + Dsall.Tables[0].Rows[0]["SUPPLIER_ID"]);
        //        string _Eta = string.Format("<tr><td><b>ETA</b></td><td colspan=\"2\"><b>" + ETA + "</b></td></tr>");
        //        if (Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"] != null && Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString() != "")
        //        {
        //            availabilty1 = Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString();
        //            availabilty1 = availabilty1.Replace("+", "");
        //            availabilty = Convert.ToInt32(availabilty1.ToString());
        //            //objErrorHandler.CreateLog("availabilty " + availabilty);
        //        }
        //        if (Dsall.Tables[0].Rows[0]["product_id"] != null && Dsall.Tables[0].Rows[0]["product_id"].ToString() == string.Empty && Dsall.Tables[0].Rows[0]["PRODUCT_CODE"] != null && Dsall.Tables[0].Rows[0]["PRODUCT_CODE"].ToString() == string.Empty)
        //        {
        //            objErrorHandler.CreateLog("availabilty " + availabilty + " SUPPLIER_SHIP_DAYS " + Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString());
        //            if (Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"] != null && Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString() != "")
        //            {
        //                int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString());
        //                objErrorHandler.CreateLog(" shipping_time " + shipping_time);
        //                string supplier_shipping_time = string.Empty;

        //                if (shipping_time > 1)
        //                {
        //                    supplier_shipping_time = "1 - " + shipping_time + " Days";
        //                }
        //                else if (shipping_time == 1)
        //                {
        //                    supplier_shipping_time = " 1 Day";
        //                }
        //                if (shipping_time > 0 && shipping_time <= 14)

        //                {
        //                    st.SetAttribute("TBT_ISINSTOCK", true);
        //                    st1.SetAttribute("TBT_ISINSTOCK", true);
        //                    st.SetAttribute("TBT_ISINSTOCK_STAUS", "Please Call");
        //                    st1.SetAttribute("TBT_ISINSTOCK_STAUS", "Please Call");
        //                    if (ETA == "PLEASE CALL" || ETA == "")
        //                    {
        //                        st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
        //                        st1.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
        //                        st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
        //                        st1.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
        //                    }
        //                    else
        //                    {
        //                        st.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
        //                        st1.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
        //                        st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);
        //                        st1.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);

        //                    }
        //                }
        //                else if (ETA == "PLEASE CALL" || ETA == "")
        //                {
        //                    return true;

        //                }
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }


        //        }
        //        else if (availabilty > 0)
        //        {
        //            int avail_total = Convert.ToInt32(availabilty);
        //            int stock_cutoff = Convert.ToInt32(Dsall.Tables[0].Rows[0]["WEB_STOCK_CUTOFF"]);
        //            int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIPPING_TIME"].ToString());
        //            string supplier_shipping_time = string.Empty;
        //            if (shipping_time > 1)
        //            {
        //                supplier_shipping_time = "1 - " + shipping_time + " Days";
        //            }
        //            else if (shipping_time == 1)
        //            {
        //                supplier_shipping_time = " 1 Day";
        //            }
        //            if (avail_total >= stock_cutoff)
        //            {
        //                st.SetAttribute("TBT_ISINSTOCK", true);
        //                st1.SetAttribute("TBT_ISINSTOCK", true);
        //                st.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
        //                st1.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
        //                st.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
        //                st1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
        //                st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
        //                st1.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
        //                st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
        //                st1.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);

        //            }
        //            else if (avail_total < stock_cutoff)
        //            {
        //                if (ETA == "PLEASE CALL" || ETA == "")
        //                {
        //                    st.SetAttribute("TBT_PLEASE_CALL", true);
        //                    st1.SetAttribute("TBT_PLEASE_CALL", true);
        //                    st.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
        //                    st1.SetAttribute("TBT_SHIPPINGDAYS", supplier_shipping_time);
        //                }
        //                else
        //                {
        //                    st.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
        //                    st1.SetAttribute("TBT_STOCK_STATUS_ETA", _Eta);
        //                    st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);
        //                    st1.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);

        //                }
        //            }
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.CreateLog(ex.ToString());
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return true;
        //}

        private bool GetStockDetails(StringTemplate st, StringTemplate st1, string Pid, string ETA)
        {
            HelperDB objhelperDb = new HelperDB();
            try
            {
                string user_id = string.Empty;
                string order_id = string.Empty;
                int no = 0;
                int availabilty = 0;
                string availabilty1 = string.Empty;
                string sqlexec = "exec SP_CHECK_STOCK_STATUS '" + Pid.ToString() + "' ";
               // objErrorHandler.CreateLog("sqlexec " + sqlexec);
                DataSet Dsall = objhelperDb.GetDataSetDB(sqlexec);

                if (Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"] != null && Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString() != "")
                {
                    availabilty1 = Dsall.Tables[0].Rows[0]["AVAILABILTY_TOTAL"].ToString();
                    availabilty1 = availabilty1.Replace("+", "");
                    try
                    {
                        availabilty = Convert.ToInt32(availabilty1.ToString());
                    }
                    catch (Exception ex)
                    { }
                }
                if ((Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"] == null || Dsall.Tables[0].Rows[0]["SUPPLIER_ITEM_CODE"].ToString().Trim() == "") || (Dsall.Tables[0].Rows[0]["SUPPLIER_ID"].ToString() == null || Dsall.Tables[0].Rows[0]["SUPPLIER_ID"].ToString().Trim() == ""))
                {
                    return true;
                }
                if (Dsall.Tables[0].Rows[0]["product_id"] != null && Dsall.Tables[0].Rows[0]["product_id"].ToString() == string.Empty && Dsall.Tables[0].Rows[0]["PRODUCT_CODE"] != null && Dsall.Tables[0].Rows[0]["PRODUCT_CODE"].ToString() == string.Empty)
                {
                    if (Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"] != null && Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString() != "")
                    {
                        int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIP_DAYS"].ToString());
                        string supplier_shipping_time = string.Empty;

                        if (shipping_time > 1)
                        {
                            supplier_shipping_time = "1 - " + shipping_time + " Days";
                        }
                        else if (shipping_time == 1)
                        {
                            supplier_shipping_time = " 1 Day";
                        }
                        if (shipping_time > 0 && shipping_time <= 14)
                        {
                            //st.SetAttribute("TBT_ISINSTOCK", true);
                            if (ETA != "" && ETA != "PLEASE CALL")
                            {
                                st.SetAttribute("TBT_ISINSTOCK", false);
                                st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);
                                st.SetAttribute("TBT_SHOW_ETA", true);
                                st.SetAttribute("TBT_ISINSTOCK_STAUS", "Pre-Order");
                                st.SetAttribute("TBT_SHIPPINGDAYS", "ETA: " + ETA);

                                st1.SetAttribute("TBT_ISINSTOCK", false);
                                st1.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);
                                st1.SetAttribute("TBT_SHOW_ETA", true);
                                st1.SetAttribute("TBT_ISINSTOCK_STAUS", "Pre-Order");
                                st1.SetAttribute("TBT_SHIPPINGDAYS", "ETA: " + ETA);
                            }
                            else
                            {
                                st.SetAttribute("TBT_ISINSTOCK", true);
                                st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                                st.SetAttribute("TBT_ISINSTOCK_STAUS", "Please Call");
                                st.SetAttribute("TBT_SHIPPINGDAYS", "Ships out within " + supplier_shipping_time);
                                st.SetAttribute("TBT_SHOW_ETA", false);
                                st1.SetAttribute("TBT_ISINSTOCK", true);
                                st1.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                                st1.SetAttribute("TBT_ISINSTOCK_STAUS", "Please Call");
                                st1.SetAttribute("TBT_SHIPPINGDAYS", "Ships out within " + supplier_shipping_time);
                                st1.SetAttribute("TBT_SHOW_ETA", false);
                            }
                            return false;
                        }
                    }
                    //objErrorHandler.CreateLog("getstockdetails" + st.GetAttribute("TBT_SHIPPINGDAYS"));

                    return true;
                }
                else if (availabilty > 0)
                {

                    int avail_total = Convert.ToInt32(availabilty);
                    int stock_cutoff = Convert.ToInt32(Dsall.Tables[0].Rows[0]["WEB_STOCK_CUTOFF"]);
                    int shipping_time = Convert.ToInt32(Dsall.Tables[0].Rows[0]["SUPPLIER_SHIPPING_TIME"].ToString());
                    string supplier_shipping_time = string.Empty;
                    if (shipping_time > 1)
                    {
                        supplier_shipping_time = "1 - " + shipping_time + " Days";
                    }
                    else if (shipping_time == 1)
                    {
                        supplier_shipping_time = " 1 Day";
                    }
                    if (avail_total >= stock_cutoff)
                    {
                        //st.SetAttribute("TBT_ISINSTOCK", true);

                        if (ETA != "" && ETA != "PLEASE CALL")
                        {
                            st.SetAttribute("TBT_ISINSTOCK", false);
                            st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);
                            st.SetAttribute("TBT_SHOW_ETA", true);
                            st.SetAttribute("TBT_ISINSTOCK_STAUS", "Pre-Order");
                            st.SetAttribute("TBT_SHIPPINGDAYS", "ETA: " + ETA);

                            st1.SetAttribute("TBT_ISINSTOCK", false);
                            st1.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);
                            st1.SetAttribute("TBT_SHOW_ETA", true);
                            st1.SetAttribute("TBT_ISINSTOCK_STAUS", "Pre-Order");
                            st1.SetAttribute("TBT_SHIPPINGDAYS", "ETA: " + ETA);
                        }
                        else
                        {
                            st.SetAttribute("TBT_ISINSTOCK", true);
                            st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                            st.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                            st.SetAttribute("TBT_SHIPPINGDAYS", "Ships out within " + supplier_shipping_time);

                            st1.SetAttribute("TBT_ISINSTOCK", true);
                            st1.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                            st1.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                            st1.SetAttribute("TBT_SHIPPINGDAYS", "Ships out within " + supplier_shipping_time);
                        }
                        st.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                        st1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                       // objErrorHandler.CreateLog("getstockdetails1" + st.GetAttribute("TBT_SHIPPINGDAYS"));

                    }
                    else if (avail_total < stock_cutoff)
                    {
                        //  st.SetAttribute("TBT_ISINSTOCK", true);

                        if (ETA != "" && ETA != "PLEASE CALL")
                        {
                            st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);
                            st.SetAttribute("TBT_SHOW_ETA", true);
                            st.SetAttribute("TBT_ISINSTOCK_STAUS", "Pre-Order");
                            st.SetAttribute("TBT_SHIPPINGDAYS", "ETA: " + ETA);

                            st1.SetAttribute("TBT_SHOW_SHIPPINGDAYS", false);
                            st1.SetAttribute("TBT_SHOW_ETA", true);
                            st1.SetAttribute("TBT_ISINSTOCK_STAUS", "Pre-Order");
                            st1.SetAttribute("TBT_SHIPPINGDAYS", "ETA: " + ETA);
                        }
                        else
                        {
                            st.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                           // st.SetAttribute("TBT_PLEASE_CALL", true);
                            st.SetAttribute("TBT_SHIPPINGDAYS", "Ships out within " + supplier_shipping_time);

                            st1.SetAttribute("TBT_SHOW_SHIPPINGDAYS", true);
                           // st1.SetAttribute("TBT_PLEASE_CALL", true);
                            st1.SetAttribute("TBT_SHIPPINGDAYS", "Ships out within " + supplier_shipping_time);

                            int soh = 0;
                            HelperDB objHelperDB = new HelperDB();
                            string tempstr = (string)objHelperDB.GetGenericDataDB(Pid.ToString(), "GET_PRODUCT_SOH", HelperDB.ReturnType.RTString);
                            if (tempstr != null && tempstr != "")
                                soh = Convert.ToInt32(tempstr);
                            //objErrorHandler.CreateLog("soh " + soh);
                            //soh = objProductServices.GetProductSOH(Convert.ToInt32(Pid.ToString()));

                            if (soh > 4)
                            {
                                st.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                                st1.SetAttribute("TBT_STOCK_HREF", "http://schema.org/InStock");
                                st.SetAttribute("TBT_ISINSTOCK", true);
                                st.SetAttribute("TBT_ISINSTOCK", true);
                                st.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                                st1.SetAttribute("TBT_ISINSTOCK_STAUS", "In Stock");
                            }
                            else
                            {
                                st.SetAttribute("TBT_PLEASE_CALL", true);
                                st1.SetAttribute("TBT_PLEASE_CALL", true);
                            }

                        }
                    }
                    //objErrorHandler.CreateLog("getstockdetails3" + st.GetAttribute("TBT_SHIPPINGDAYS"));

                    return false;
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return true;
        }


        //modified by indu for vertical view
        public string GenerateHorizontalHTMLJsonMS(string _familyID, DataSet Ds, DataSet dsPriceTableAll, DataSet EADs, int tblcount, int rowstartcnt, int rowendcnt, string Rawurl_dy)
        {

            string rtnstr = "";
            bool verticalst = false;
            StringTemplateGroup _stg_container = null;
            StringTemplateGroup _stg_records = null;

            StringTemplate _stmpl_container = null;
            StringTemplate _stmpl_records = null;
            StringTemplate _stmpl_records1 = null;




            StringTemplate _stmpl_recordsrows = null;
            //  TBWDataList[] lstrecords = new TBWDataList[0];
            // TBWDataList[] lstrows = new TBWDataList[0];

            StringTemplateGroup _stg_container1 = null;
            StringTemplateGroup _stg_records1 = null;
            //  TBWDataList1[] lstrecords1 = new TBWDataList1[0];
            //  TBWDataList1[] lstrows1 = new TBWDataList1[0];


            DataSet dsBgDisc = new DataSet();
            decimal untPrice = 0;
            string AttrID = string.Empty;
            //  string HypColumn = "";
            int Min_ord_qty = 0;
            //  int Qty_avail;
            //  int flagtemp = 0;
            string _StockStatus = "NO STATUS AVAILABLE";
            string _prod_stk_Status = "0";
            string _prod_stk_flag = "0";
            string _AvilableQty = "0";
            string _eta = "";
            string _Category_id = "";
            string _EA_Path = "";
            string StrPriceTable = "";

            DataRow[] tempPriceDr;

            //DataTable tempPriceDt;
            //int ProdID;
            int AttrType;
            string userid = HttpContext.Current.Session["USER_ID"].ToString();
            if (userid == "" || userid == null)
                userid = ConfigurationManager.AppSettings["DUM_USER_ID"];
            //userid = "999";// ConfigurationManager.AppSettings["DUM_USER_ID"].ToString();

            string NavColumn = objHelperServices.GetOptionValues("NAVIGATIONCOLUMN").ToString();
            string HypCURL = objHelperServices.GetOptionValues("NAVIGATIONURL").ToString();
            string EComState = objHelperServices.GetOptionValues("ECOMMERCEENABLED").ToString();
            string _parentFamily_Id = "0";
            string CartImgPathdisplay = "";
            string _ProCode = "";
            string family_name = "";
            if (EComState == "YES")
                if (!objHelperServices.GetIsEcomEnabled(userid))
                    EComState = "NO";
            StringBuilder strBldr = new StringBuilder();
            StringBuilder strBldrcost = new StringBuilder();

            if (HttpContext.Current.Request.QueryString["path"] != null)
                _EA_Path = objSecurity.StringDeCrypt(HttpContext.Current.Request.QueryString["path"].ToString());

            if (HttpContext.Current.Request.QueryString["cid"] != null)
                _Category_id = HttpContext.Current.Request.QueryString["cid"];

            DsPreview = Ds;
            if (DsPreview.Tables[_familyID] == null)
                return "";


            DataSet _parentFamilyds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_FAMILY_ID_AND_PARENT", HelperDB.ReturnType.RTDataSet);
            if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                _parentFamily_Id = _parentFamilyds.Tables[0].Rows[0]["PARENT_FAMILY_ID"].ToString();

            if (_parentFamily_Id == "0")
                _parentFamily_Id = _familyID;

            pricecode = GetPriceCode();

            if (_parentFamilyds != null && _parentFamilyds.Tables.Count > 0 && _parentFamilyds.Tables[0].Rows.Count > 0)
                family_name = _parentFamilyds.Tables[0].Rows[0]["FAMILY_NAME"].ToString();
            else
                family_name = HttpContext.Current.Session["TBT_FAMILY_NAME_ALT"].ToString();

            string _SkinRootPath = string.Empty;

            _SkinRootPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["MicroTemplatePath"]);

            _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
            _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);

            string HTMLAtts = "";
            string HTMLProducts = "";
            string HTMLHeaderStr = "";
            int ictrecords = 0;
            string costtype = "";
            HTMLAtts = "";
            bool showheader = true;
            if (HttpContext.Current.Session["hfprevfid"] == null)
            {
                HttpContext.Current.Session["hfprevfid"] = _familyID;
            }
            else if (HttpContext.Current.Session["hfprevfid"].ToString() != _familyID)
            {
                HttpContext.Current.Session["hfprevfid"] = _familyID;
            }
            else
            {
                showheader = false;
            }



            for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
            {

                if (DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() != "COST" && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() != "CODE"
                    && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() != "TWEB IMAGE1"
                    && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() != "PRODUCT_ID"
                    && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() != "FAMILY_ID"
                    )
                {
                    _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProcellHead");
                    _stmpl_records.SetAttribute("ATTRIBUTE_NAME", DsPreview.Tables[_familyID].Columns[j].Caption);
                    HTMLAtts = HTMLAtts + _stmpl_records.ToString();

                }

                //DataRow[] tempdr = DsPreview.Tables["Attribute"].Select("ATTRIBUTE_NAME='" + DsPreview.Tables[_familyID].Columns[j].ToString() + "'");
                //if (tempdr.Length > 0 && objHelperServices.CI(tempdr.CopyToDataTable().Rows[0]["ATTRIBUTE_TYPE"].ToString())!=3)
                //{
                if (DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() == "COST")
                {
                    if (pricecode == 1)
                    {
                        costtype = " Inc GST";
                    }
                    else
                    {
                        costtype = " Ex GST";
                    }
                }

                //}

            }

            if (DsPreview.Tables[_familyID].Columns.Count <= 8)
            {
                _stmpl_records1 = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProrowHead");
            }
            else
            {
                _stmpl_records1 = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "ProrowHead_V");
            }

            _stmpl_records1.SetAttribute("INGST", costtype);
            _stmpl_records1.SetAttribute("ATTRIBUTE_HEADR", HTMLAtts);
            HTMLHeaderStr = _stmpl_records1.ToString();





            DisplayHeaders = true;
            //if (DisplayHeaders == true)
            //{
            //    HTMLProducts = HTMLProducts + HTMLHeaderStr;
            //}
            string ValueFortag = string.Empty;
            bool rowcolor = false;

            if (_EA_Path == "" && _Category_id == "")
            {
                DataSet tmpds = (DataSet)objHelperDb.GetGenericDataDB(WesCatalogId, _familyID, "GET_PARENT_CATEGORY_ID_PATH_INPUT_FAMID", HelperDB.ReturnType.RTDataSet);
                if (tmpds != null && tmpds.Tables.Count > 0)
                {
                    _Category_id = tmpds.Tables[0].Rows[0]["CATEGORY_ID"].ToString();
                    string eapath = "AllProducts////WESAUSTRALASIA////" + tmpds.Tables[0].Rows[0]["CATEGORY_PATH"].ToString() + "////UserSearch=Family Id=" + _familyID.ToString();
                    _EA_Path = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(eapath));
                }
            }

            _stg_records = new StringTemplateGroup("cell", _SkinRootPath);
            _stg_records1 = new StringTemplateGroup("row", _SkinRootPath);
            _stg_container = new StringTemplateGroup("main", _SkinRootPath);
            string strFile = System.Configuration.ConfigurationManager.AppSettings["ProdimgSepdomainUrl"].ToString();
            bool BindToST = true;
            string prodcodedesc = "";
            string prodedesc = "";
            //modified by indu
            //for (int i = rowstartcnt ; i < DsPreview.Tables[_familyID].Rows.Count; i++)
            //    {

            string CustomerType = objUserServices.GetCustomerType(Convert.ToInt32(userid));

            for (int i = rowstartcnt; i < rowendcnt; i++)
            {
                tempPriceDr = EADs.Tables["FamilyPro"].Select("PRODUCT_ID='" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "'");
                //if (tempPriceDr.Length > 0)
                //    tempPriceDt = tempPriceDr.CopyToDataTable();
                //else
                //   tempPriceDt = null;

                strBldrcost = new StringBuilder();

                if (DsPreview.Tables[_familyID].Columns.Count <= 8)
                {
                    _stmpl_records1 = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Prorow");
                }
                else
                {
                    _stmpl_records1 = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Prorow_V");

                }

                _stmpl_records1.SetAttribute("PRODUCT_ID", DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString());
                _stmpl_records1.SetAttribute("FAMILY_ID", DsPreview.Tables[_familyID].Rows[i]["FAMILY_ID"].ToString());


                //-------------------------------- cost
                _StockStatus = "NO STATUS AVAILABLE";
                _prod_stk_Status = "0";
                _prod_stk_flag = "0";
                //_stmpl_records1.SetAttribute("COST", Prefix + " " + objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString() + " " + "<br/> Price / Stock Status");
                try
                {
                    _stmpl_records1.SetAttribute("COST", Prefix + " " + DsPreview.Tables[_familyID].Rows[i]["COST"].ToString());
                    //_stmpl_records1.SetAttribute("COST", objHelperServices.FixDecPlace(Convert.ToDecimal(DsPreview.Tables[_familyID].Rows[i]["COST"].ToString())).ToString());
                }
                catch
                { }
                if (tempPriceDr != null && tempPriceDr.Length > 0)
                {
                    _StockStatus = tempPriceDr[0]["STOCK_STATUS_DESC"].ToString().Replace("_", " ");
                    _prod_stk_Status = tempPriceDr[0]["PROD_STOCK_STATUS"].ToString();
                    _prod_stk_flag = tempPriceDr[0]["PROD_STOCK_FLAG"].ToString();
                    _AvilableQty = tempPriceDr[0]["QTY_AVAIL"].ToString();
                    _eta = tempPriceDr[0]["ETA"].ToString();


                }
                if (DsPreview.Tables[_familyID].Rows[i]["Code"] != null)
                    _ProCode = DsPreview.Tables[_familyID].Rows[i]["Code"].ToString();

                StrPriceTable = AssemblePriceTableMS(Convert.ToInt32(DsPreview.Tables[_familyID].Rows[i]["product_id"].ToString()), pricecode, _ProCode, _StockStatus, _prod_stk_Status, CustomerType, Convert.ToInt32(userid), _prod_stk_flag, _eta, dsPriceTableAll);
                _stmpl_records1.SetAttribute("PRICE_TABLE", StrPriceTable);

                string tempEAPath = HttpUtility.UrlEncode(objSecurity.StringEnCrypt(_EA_Path + "////UserSearch=Family Id=" + _parentFamily_Id.ToString()));
                _stmpl_records1.SetAttribute("AVIL_QTY", _AvilableQty);
                _stmpl_records1.SetAttribute("MIN_ORDER_QTY", 1);




                //string ORIGINALURL = "mpd.aspx?pid=" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "&fid=" + _parentFamily_Id + "&cid=" + _Category_id + "&path=" + tempEAPath;
                //string NEWURL = string.Empty;

                //if (HttpContext.Current.Session["breadcrumEAPATH"] != null)
                //{
                //    NEWURL = HttpContext.Current.Session["breadcrumEAPATH"].ToString() + "////" + DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "=" + _ProCode;
                //    NEWURL = objHelperServices.Cons_NewURl_bybrand_MS(ORIGINALURL, NEWURL, "mpd.aspx", "");
                //    HREFURL = "/" + NEWURL + "/mpd/";
                //}
                //else
                //{
                //    NEWURL = ORIGINALURL;
                //    HREFURL = ORIGINALURL;

                //}
                string url = objHelperServices.SimpleURL_MS_Str(DsPreview.Tables[_familyID].Rows[i]["PRODUCT_ID"].ToString() + "=" + _ProCode, "pd.aspx", false);
                string HREFURL = string.Empty;
                string rawurl = string.Empty;
                if (Rawurl_dy != "")
                {
                    rawurl = Rawurl_dy;
                }
                else
                {
                    rawurl = HttpContext.Current.Request.RawUrl.ToString().Replace("/fl/", "");
                }
                string[] splURL = rawurl.Split('/');
                HREFURL = objHelperServices.AddDomainname() + splURL[1] + "/" + splURL[2] + "/" + splURL[3] + "/" + url + "/" + _parentFamily_Id + "/mpd/";
                //HREFURL = "/" + HREFURL + "/mpd/";

                _stmpl_records1.SetAttribute("PARENT_FAMILY_ID", _parentFamily_Id);
                _stmpl_records1.SetAttribute("CAT_ID", _Category_id);
                _stmpl_records1.SetAttribute("EA_PATH", tempEAPath);
                _stmpl_records1.SetAttribute("URL_RW_PATH", HREFURL);

                _stmpl_records1.SetAttribute("BUY_FAMILY_ID", _familyID);
                // objHelperServices.Cons_NewURl_bybrand_MS(ORIGINALURL, NEWURL, "mpd.aspx", "");
               // objErrorHandler.CreateLog(_StockStatus + DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString());
                bool _Tbt_Stock_Status_2 = false;
                switch (_StockStatus.Trim())
                {
                    case "IN STOCK":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "Limited Stock, Please Call":

                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER PRICE &":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "DISCONTINUED":
                        _Tbt_Stock_Status_2 = false;
                        break;
                    case "DISCONTINUED NO LONGER AVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        break;
                    case "DISCONTINUED NO LONGER":
                        _Tbt_Stock_Status_2 = false;
                        break;
                    case "TEMPORARY UNAVAILABLE":
                        //modified by indu Requirement Stock Status update date 7-Apr-2017
                       //  _Tbt_Stock_Status_2 = true;
                      _Tbt_Stock_Status_2 = false;
                        break;
                    case "TEMPORARY UNAVAILABLE NO ETA":
                        //modified by indu Requirement Stock Status update date 7-Apr-2017
           // _Tbt_Stock_Status_2 = true;
       _Tbt_Stock_Status_2 = false;
                        break;
                    case "OUT OF STOCK":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "OUT OF STOCK ITEM WILL":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "Please Call":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "Please_Call":
                        _Tbt_Stock_Status_2 = true;
                        break;
                    default:
                        _Tbt_Stock_Status_2 = false;
                        break;
                }
                BindToST = true;
                if (_Tbt_Stock_Status_2 == true)
                {
                     _stmpl_records1.SetAttribute("SHOW_BUY", true);
                        if ( tempPriceDr[0]["PROD_SUBSTITUTE"].ToString().Trim() == "" && _StockStatus.Trim() == "DISCONTINUED NO LONGER AVAILABLE")
                        {
                            BindToST = false;
                        }

                }
                else
                {
                    _stmpl_records1.SetAttribute("SHOW_BUY", false);
                }


                //-------------------------------- Code

                _stmpl_records1.SetAttribute("PROD_CODE", DsPreview.Tables[_familyID].Rows[i]["CODE"].ToString());

                //-------------------------------- Image

                ValueFortag = DsPreview.Tables[_familyID].Rows[i]["TWEB IMAGE1"].ToString();
                string ValueLargeImg = "";
                if (ValueFortag != "" && ValueFortag != null)
                {
                   // FileInfo Fil;


                   // Fil = new FileInfo(strFile + ValueFortag);
                   // if (Fil.Exists)
                   // {

                        ValueFortag = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString()+"prodimages" + ValueFortag.Replace("\\", "/");
                        ValueLargeImg = ValueFortag.ToLower().Replace("_th", "_Images_200").Replace("\\", "/");
                   // }
                    //else
                    //{
                    //    ValueFortag = "/prodimages/images/noimage.gif";
                    //    ValueLargeImg = "";
                    //}
                }
                else
                {
                    ValueFortag = System.Configuration.ConfigurationManager.AppSettings["CDNROOT"].ToString() + "prodimages/images/noimage.gif";
                    ValueLargeImg = "";
                }

                _stmpl_records1.SetAttribute("TWEB_LargeImg", ValueLargeImg);
                _stmpl_records1.SetAttribute("TWEB_Image", ValueFortag);
                if (ValueLargeImg != "")
                    _stmpl_records1.SetAttribute("SHOW_DIV", true);
                else
                    _stmpl_records1.SetAttribute("SHOW_DIV", false);


                HTMLAtts = "";

                bool flgdescchk = false;
                bool flgdeschk_bbpp = false;

                for (int j = 1; j < DsPreview.Tables[_familyID].Columns.Count; j++)
                {
                    if (DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() != "COST"
                        && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() != "CODE"
                        && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() != "TWEB IMAGE1"
                        && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() != "PRODUCT_ID"
                        && DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() != "FAMILY_ID"
                        )
                    {

                        if (DsPreview.Tables[_familyID].Columns.Count <= 8)
                        {

                            _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Procell");
                        }
                        else
                        {

                            _stmpl_records = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Procell_V");
                            verticalst = true;
                        }

                        if ((DsPreview.Tables[_familyID].Rows[i][j].ToString().Length <= 20) || (verticalst == false))
                        {
                            _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;"));
                        }
                        else if (verticalst == true)
                        {
                            string htmltitle = DsPreview.Tables[_familyID].Rows[i][j].ToString();
                            string htmlproddesc = "<a href='' style='text-decoration: none !important;' class=gray_40 data-toggle=tooltip data-placement=top   title='" + htmltitle + "'>" + DsPreview.Tables[_familyID].Rows[i][j].ToString().Substring(0, 20).Replace("\r", "<br/>").Replace("\n", "&nbsp;") + "..</a>";
                            _stmpl_records.SetAttribute("ATTRIBUTE_VALUE", htmlproddesc);
                        }
                        HTMLAtts = HTMLAtts + _stmpl_records.ToString();
                        if (DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() == "DESCRIPTION")
                        {

                            _stmpl_records1.SetAttribute("PROD_DESCRIPTION", DsPreview.Tables[_familyID].Rows[i]["DESCRIPTION"].ToString());






                            flgdescchk = true;
                        }
                        if (DsPreview.Tables[_familyID].Columns[j].Caption.ToUpper() == "DESCRIPTION" && DsPreview.Tables[_familyID].Rows[i][j].ToString() != "")
                        {
                            prodedesc = DsPreview.Tables[_familyID].Rows[i][j].ToString().Replace("\r", "<br/>").Replace("\n", "&nbsp;");
                            flgdeschk_bbpp = true;
                        }

                    }

                }

                if (flgdescchk == false)
                    _stmpl_records1.SetAttribute("PROD_DESCRIPTION", family_name);


                if (flgdeschk_bbpp == false)
                {
                    if (tempPriceDr != null && tempPriceDr.Length > 0)
                        prodedesc = tempPriceDr[0]["PRODUCT_DESC"].ToString();
                }

                _stmpl_records1.SetAttribute("ATTRIBUTE_VALUES", HTMLAtts);

                if (BindToST == true)
                {
                    HTMLProducts = HTMLProducts + _stmpl_records1.ToString();
                }

                if (prodedesc.Length > 0)
                    prodcodedesc = prodcodedesc + _ProCode + " – " + prodedesc + "|";
                else
                    prodcodedesc = prodcodedesc + _ProCode + "|";




            }
            HttpContext.Current.Session["prodcodedesc"] = prodcodedesc;
            if (DsPreview.Tables[_familyID].Columns.Count <= 8)
            {
                if (showheader)
                {
                    _stmpl_container = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Promain");
                }
                else
                {
                    _stmpl_container = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Promain_dyn_micro");
                
                }
            }
            else
            {
                _stmpl_container = _stg_records.GetInstanceOf("Csfamilypage" + "\\" + "Promain_V");
                showheader = true;
            }

            _stmpl_container.SetAttribute("PRODUCT_DETAILS", HTMLProducts);
            if (showheader)
            {
                _stmpl_container.SetAttribute("PRODUCT_DETAILS_HEAD", HTMLHeaderStr);
            }
            else
            {
               
             _stmpl_container.SetAttribute("PRODUCT_DETAILS_HEAD", HTMLHeaderStr.Replace("mythead","thead_none"));
            }
            // if (verticalst)
            //{
            //    _stmpl_container.SetAttribute("PRODUCT_DETAILS_HEAD", HTMLHeaderStr.Replace("thead_none", "mythead"));
            //}
     
            _stmpl_container.SetAttribute("PAGER_ID", tblcount);





            return _stmpl_container.ToString();

        }
        /*********************************** OLD CODE ***********************************/
        //private bool IsEcomenabled()
        //{
        //    bool retvalue = false;
        //    string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //    string websiteid = System.Configuration.ConfigurationManager.AppSettings["WEBSITEID"].ToString();
        //    string sSQL = "SELECT USER_ROLE FROM TBWC_COMPANY_BUYERS WHERE WEBSITE_ID = " + websiteid + " and USER_ID = " + userid;
        //    oHelper.SQLString = sSQL;
        //    int iROLE = oHelper.CI(oHelper.GetValue("USER_ROLE"));
        //    if (iROLE <= 3)
        //        retvalue = true;
        //    return retvalue;
        //}

        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE CURRENCY FORMAT EXTRACTION DETAILS ***/
        /********************************************************************************/
        private void ExtractCurrenyFormat(string AttributeValue)
        {
            //AppLoader.DBConnection Oocn = new DBConnection();
            string XMLstr = string.Empty;
           // DataSet dscURRENCY = new DataSet();
            //oHelper.SQLString = " SELECT ATTRIBUTE_DATARULE FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID  =" + AttributeID + " ";
            //dscURRENCY = oHelper.GetDataSet();
            Prefix = string.Empty; Suffix = string.Empty; EmptyCondition = string.Empty; ReplaceText = string.Empty; Headeroptions = string.Empty;
            //if (dscURRENCY.Tables[0].Rows.Count > 0)
            //{
                if (AttributeValue!= string.Empty)
                {
                    XMLstr = AttributeValue; //dscURRENCY.Tables[0].Rows[0].ItemArray[0].ToString();
                    XmlDocument xmlDOc = new XmlDocument();
                    xmlDOc.LoadXml(XMLstr);
                    XmlNode rootNode = xmlDOc.DocumentElement;
                    {
                        XmlNodeList xmlNodeList;
                        xmlNodeList = rootNode.ChildNodes;

                        for (int xmlNode = 0; xmlNode < xmlNodeList.Count; xmlNode++)
                        {
                            if (xmlNodeList[xmlNode].ChildNodes.Count > 0)
                            {
                                if (xmlNodeList[xmlNode].ChildNodes[0].LastChild != null)
                                {
                                    Prefix = xmlNodeList[xmlNode].ChildNodes[0].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[1].LastChild != null)
                                {
                                    Suffix = xmlNodeList[xmlNode].ChildNodes[1].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[2].LastChild != null)
                                {
                                    EmptyCondition = xmlNodeList[xmlNode].ChildNodes[2].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[3].LastChild != null)
                                {
                                    ReplaceText = xmlNodeList[xmlNode].ChildNodes[3].LastChild.Value;
                                }
                                if (xmlNodeList[xmlNode].ChildNodes[4].LastChild != null)
                                {
                                    Headeroptions = xmlNodeList[xmlNode].ChildNodes[4].LastChild.Value;
                                }

                            }
                        }
                    }



               }
            //}
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK STRING VALUE IS MATCHED WITH NUMBERS OR NOT USING REGULAR EXPRESSION VALIDATOR ***/
        /********************************************************************************/ 
        public bool Isnumber(string RefValue)
        {
            //string StrRegx = "^[0-9.]";
            //string StrRegx =@"(^-?\d\d*$)"; jai
            //string StrRegx = @"^d[0-9.]+$";
            string StrRegx = @"^[0-9]*(\.)?[0-9]+$";
            bool Retval = false;
            Regex re = new Regex(StrRegx);
            if (re.IsMatch(RefValue))
            {
                Retval = true;
            }
            else
            {
                Retval = false;
            }
            return Retval;
        }

        /*********************************** OLD CODE ***********************************/
        //private decimal GetMyPrice(int ProductID)
        //{
        //    decimal retval = 0.00M;
        //    string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //    if (!string.IsNullOrEmpty(userid))
        //    {
        //        string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
        //        oHelper.SQLString = sSQL;
        //        int pricecode = oHelper.CI(oHelper.GetValue("price_code"));

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
        //        oHelper.SQLString = strquery;
        //        retval = Math.Round(Convert.ToDecimal(oHelper.GetValue("Numeric_Value")), 2);
        //    }
        //    return retval;
        //}
        //private string GetStockStatus(int ProductID)
        //{
        //    string Retval = "NO STATUS AVAILABLE";
        //    try
        //    {
        //        string sSQL = string.Format("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = {0}", ProductID);
        //        oHelper.SQLString = sSQL;
        //        Retval = oHelper.GetValue("PROD_STK_STATUS_DSC").ToString().Replace("_", " ");
        //    }
        //    catch
        //    {
        //    }
        //    return Retval;
        //}
        /*********************************** OLD CODE ***********************************/

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //private string AssemblePriceTable(int ProductID, int _priceCode, string _ProCode, string _ProStkStatus)
        //{
        //    string _sPriceTable = "";
        //    SqlConnection oSQLCon = null;
        //    try
        //    {

        //        string userid = HttpContext.Current.Session["USER_ID"].ToString();
        //        DataSet dsPriceTable = new DataSet();
        //        //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
        //        //oHelper.SQLString = sSQL;
        //        //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
        //        //DataSet dsPriceTable = new DataSet();
        //        //oSQLCon = new SqlConnection(oCon.ConnectionString.Replace("provider=SQLOLEDB;", ""));
        //        //oSQLCon.Open();
        //        //SqlCommand oCmd = new SqlCommand("SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = @PRODUCT_ID and ATTRIBUTE_ID = 1", oSQLCon);
        //        //oCmd.Parameters.Clear();
        //        //oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
        //        //string _sCODE = oCmd.ExecuteScalar().ToString();

        //        //oCmd = new SqlCommand("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = @PRODUCT_ID", oSQLCon);
        //        //oCmd.Parameters.Clear();
        //        //oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
        //        //string stkstatus = oCmd.ExecuteScalar().ToString();
        //        int pricecode = _priceCode;
        //        string _sCODE = _ProCode;
        //        string stkstatus = _ProStkStatus;
        //        string _Tbt_Stock_Status = "";
        //        string _Tbt_Stock_Status_1 = "";
        //        bool _Tbt_Stock_Status_2 = false;
        //        string _Tbt_Stock_Status_3 = "";
        //        string _Colorcode1 = "";
        //        string _Colorcode;
        //        string StockStatus = stkstatus.Replace("_", " ");
        //        string _StockStatusTrim = StockStatus.Trim();

        //        switch (_StockStatusTrim)
        //        {
        //            case "IN STOCK":
        //                _Tbt_Stock_Status = "<span style=color:#43A246><b>INSTOCK</b></span><br>";
        //                _Tbt_Stock_Status_2 = true;
        //                break;
        //            case "SPECIAL ORDER":
        //                _Colorcode = "#43A246";
        //                _Tbt_Stock_Status_2 = true;
        //                _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
        //                break;
        //            case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
        //                _Tbt_Stock_Status_2 = true;
        //                _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
        //                break;
        //            case "SPECIAL ORDER PRICE &":
        //                _Tbt_Stock_Status_2 = true;
        //                _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
        //                break;
        //            case "DISCONTINUED":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
        //                break;
        //            case "DISCONTINUED NO LONGER AVAILABLE":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
        //                break;
        //            case "DISCONTINUED NO LONGER":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status_3 = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
        //                break;
        //            case "TEMPORARY UNAVAILABLE":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
        //                break;
        //            case "TEMPORARY UNAVAILABLE NO ETA":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
        //                break;
        //            case "OUT OF STOCK":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br>";
        //                _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246> <b>ITEM WILL BE BACK ORDERED</b> </span>";
        //                break;
        //            case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
        //                _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
        //                break;
        //            case "OUT OF STOCK ITEM WILL":
        //                _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
        //                _Tbt_Stock_Status_2 = false;
        //                _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
        //                break;
        //            default:
        //                _Colorcode = "Black";
        //                _Tbt_Stock_Status = _StockStatusTrim;
        //                break;
        //        }

        //        //SqlDataAdapter oDa = new SqlDataAdapter();
        //        //oDa.SelectCommand = new SqlCommand();
        //        //oDa.SelectCommand.Connection = oSQLCon;
        //        //oDa.SelectCommand.CommandText = "GetPriceTable";
        //        //oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
        //        //oDa.SelectCommand.Parameters.Clear();
        //        //oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
        //        //oDa.SelectCommand.Parameters.AddWithValue("@UserID", userid);
        //        //oDa.Fill(dsPriceTable, "PriceTable");
        //        dsPriceTable = objHelperDb.GetProductPriceTable(ProductID, Convert.ToInt32(userid));
        //        dsPriceTable.Tables[0].TableName = "PriceTable";

        //        _sPriceTable = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"1\" bgcolor=\"black\"><tr><td><table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
        //        _sPriceTable += "<td width=\"100\" height=\"39\" valign=\"top\" class=\"pad2\"><b>ORDER CODE:</b><br />";
        //        _sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
        //        _sPriceTable += "<td width=\"100\" valign=\"top\" class=\"pad1\"><b>STOCK STATUS</b><br />";
        //        if (_Tbt_Stock_Status != "")
        //        {
        //            _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
        //        }
        //        else
        //        {
        //            _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
        //        }
        //        _sPriceTable += "<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"table_bdr\"><tr class=\"bg_grey3\"><td><b>Qty</b></td><td><b>Cost Inc GST</b></td><td><b>Cost Ex GST</b></td></tr>";

        //        int TotalCount = 0;
        //        int RowCount = 0;

        //        if (pricecode == 3)
        //            foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
        //            {
        //                _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
        //            }
        //        else
        //        {
        //            bool bLastRow = false;
        //            TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
        //            RowCount = 0;

        //            foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
        //            {
        //                RowCount = RowCount + 1;
        //                if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
        //                {
        //                    bLastRow = true;
        //                }

        //                string _color = bLastRow ? "bg_grey31" : "bg_grey3";
        //                _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
        //            }
        //        }

        //        _sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table></td></tr></table>";
        //        //if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        _sPriceTable = "";//<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
        //        //if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
        //    }
        //    return _sPriceTable;
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PRICE TABLE DETAILS BASED ON THE MATCH CASES  ***/
        /********************************************************************************/
        public string AssemblePriceTable(int ProductID, int _priceCode, string _ProCode, string _ProStkStatus)
        {
            string _sPriceTable = string.Empty;
            SqlConnection oSQLCon = null;
            try
            {
                /*********************************** OLD CODE ***********************************/                 
              ////////  string userid = HttpContext.Current.Session["USER_ID"].ToString();
              ////////  DataSet dsPriceTable = new DataSet();
              ////////  //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
              ////////  //oHelper.SQLString = sSQL;
              ////////  //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
              ////////  //DataSet dsPriceTable = new DataSet();
              ////////  //oSQLCon = new SqlConnection(oCon.ConnectionString.Replace("provider=SQLOLEDB;", ""));
              ////////  //oSQLCon.Open();
              ////////  //SqlCommand oCmd = new SqlCommand("SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = @PRODUCT_ID and ATTRIBUTE_ID = 1", oSQLCon);
              ////////  //oCmd.Parameters.Clear();
              ////////  //oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
              ////////  //string _sCODE = oCmd.ExecuteScalar().ToString();
                
              ////////  //oCmd = new SqlCommand("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = @PRODUCT_ID", oSQLCon);
              ////////  //oCmd.Parameters.Clear();
              ////////  //oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
              ////////  //string stkstatus = oCmd.ExecuteScalar().ToString();
              ////////  int pricecode = _priceCode;
              ////////  string _sCODE = _ProCode;
              ////////  string stkstatus = _ProStkStatus;
              ////////  string _Tbt_Stock_Status = "";
              ////////  string _Tbt_Stock_Status_1 = "";
              ////////  bool _Tbt_Stock_Status_2 = false;
              ////////  string _Tbt_Stock_Status_3 = "";
              ////////  string _Colorcode1 = "";
              ////////  string _Colorcode;
              ////////  string StockStatus = stkstatus.Replace("_", " ");
              ////////  string _StockStatusTrim = StockStatus.Trim();

              ////////  switch (_StockStatusTrim)
              ////////  {
              ////////      case "IN STOCK":
              ////////          _Tbt_Stock_Status = "<span style=color:#43A246><b>INSTOCK</b></span><br>";
              ////////          _Tbt_Stock_Status_2 = true;
              ////////          break;
              ////////      case "SPECIAL ORDER":
              ////////          _Colorcode = "#43A246";
              ////////          _Tbt_Stock_Status_2 = true;
              ////////          _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
              ////////          break;
              ////////      case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
              ////////          _Tbt_Stock_Status_2 = true;
              ////////          _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
              ////////          break;
              ////////      case "SPECIAL ORDER PRICE &":
              ////////          _Tbt_Stock_Status_2 = true;
              ////////          _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
              ////////          break;
              ////////      case "DISCONTINUED":
              ////////          _Tbt_Stock_Status_2 = false;
              ////////          _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
              ////////          break;
              ////////      case "DISCONTINUED NO LONGER AVAILABLE":
              ////////          _Tbt_Stock_Status_2 = false;
              ////////          _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
              ////////          break;
              ////////      case "DISCONTINUED NO LONGER":
              ////////          _Tbt_Stock_Status_2 = false;
              ////////          _Tbt_Stock_Status_3 = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
              ////////          break;
              ////////      case "TEMPORARY UNAVAILABLE":
              ////////          _Tbt_Stock_Status_2 = false;
              ////////          _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
              ////////          break;
              ////////      case "TEMPORARY UNAVAILABLE NO ETA":
              ////////          _Tbt_Stock_Status_2 = false;
              ////////          _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
              ////////          break;
              ////////      case "OUT OF STOCK":
              ////////          _Tbt_Stock_Status_2 = false;
              ////////          _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br>";
              ////////          _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246> <b>ITEM WILL BE BACK ORDERED</b> </span>";
              ////////          break;
              ////////      case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
              ////////          _Tbt_Stock_Status_2 = false;
              ////////          _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
              ////////          _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
              ////////          break;
              ////////      case "OUT OF STOCK ITEM WILL":
              ////////          _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
              ////////          _Tbt_Stock_Status_2 = false;
              ////////          _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
              ////////          break;
              ////////      default:
              ////////          _Colorcode = "Black";
              ////////          _Tbt_Stock_Status = _StockStatusTrim;
              ////////          break;
              ////////  }

                
              ////////  dsPriceTable = objHelperDb.GetProductPriceTable(ProductID, Convert.ToInt32(  userid));
              ////////  dsPriceTable.Tables[0].TableName = "PriceTable";
 
              ////////  //_sPriceTable = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"1\" bgcolor=\"black\"><tr><td><table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
              ////////  //_sPriceTable += "<td width=\"100\" height=\"39\" valign=\"top\" class=\"table_bdr td\"><b>ORDER CODE:</b><br />";
              ////////  //_sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
              ////////  //_sPriceTable += "<td width=\"100\" valign=\"top\" class=\"pad1\"><b>STOCK STATUS</b><br />";

              ////////  //if (_Tbt_Stock_Status != "")
              ////////  //{
              ////////  //    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
              ////////  //}
              ////////  //else
              ////////  //{
              ////////  //    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
              ////////  //}
              ////////  _sPriceTable = "<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr class=\" success\">";
              ////////  _sPriceTable += "<td width=\"100\" height=\"20\" valign=\"top\" class=\"table_bdr td\"><b>ORDER CODE:</b></td><td width=\"100\" valign=\"top\" class=\"table_bdr\"><b>STOCK STATUS</b></td></tr>";
              ////////  _sPriceTable += "<tr><td>";
              ////////  _sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
              //////////  _sPriceTable += "<td width=\"100\" valign=\"top\" class=\"pad1\"><b>STOCK STATUS</b>";

              ////////  if (_Tbt_Stock_Status != "")
              ////////  {
              ////////      _sPriceTable += string.Format("<td>{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
              ////////  }
              ////////  else
              ////////  {
              ////////      _sPriceTable += string.Format("<td>{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
              ////////  }



              ////////  _sPriceTable += "<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"table_bdr\"><tr class=\"bg_grey3\"><td><b>Qty</b></td><td><b>Cost Inc GST</b></td><td><b>Cost Ex GST</b></td></tr>";

              ////////  int TotalCount = 0;
              ////////  int RowCount = 0;

              ////////  if (pricecode == 3)
              ////////      foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
              ////////      {
              ////////          _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
              ////////      }
              ////////  else
              ////////  {
              ////////      bool bLastRow = false;
              ////////      TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
              ////////      RowCount = 0;

              ////////      foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
              ////////      {
              ////////          RowCount = RowCount + 1;
              ////////          if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
              ////////          {
              ////////              bLastRow = true;
              ////////          }

              ////////          string _color = bLastRow ? "bg_grey31" : "bg_grey3";
              ////////          _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
              ////////      }
              ////////  }

              //////// // _sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table></td></tr></table>";
              ////////  _sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table>";
              ////////  _sPriceTable += "<div class=\"popupaero\"></div>";
                /*********************************** OLD CODE ***********************************/


                string userid = HttpContext.Current.Session["USER_ID"].ToString();
                if (userid == string.Empty)
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];
                DataSet dsPriceTable = new DataSet();
                //string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                //oHelper.SQLString = sSQL;
                //int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
                //DataSet dsPriceTable = new DataSet();
                //oSQLCon = new SqlConnection(oCon.ConnectionString.Replace("provider=SQLOLEDB;", ""));
                //oSQLCon.Open();
                //SqlCommand oCmd = new SqlCommand("SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = @PRODUCT_ID and ATTRIBUTE_ID = 1", oSQLCon);
                //oCmd.Parameters.Clear();
                //oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                //string _sCODE = oCmd.ExecuteScalar().ToString();

                //oCmd = new SqlCommand("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = @PRODUCT_ID", oSQLCon);
                //oCmd.Parameters.Clear();
                //oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                //string stkstatus = oCmd.ExecuteScalar().ToString();
                int pricecode = _priceCode;
                string _sCODE = _ProCode;
                string stkstatus = _ProStkStatus;
                string _Tbt_Stock_Status = string.Empty;
                string _Tbt_Stock_Status_1 = string.Empty;
                bool _Tbt_Stock_Status_2 = false;
                string _Tbt_Stock_Status_3 = string.Empty;
                string _Colorcode1 = string.Empty;
                string _Colorcode;
                string StockStatus = stkstatus.Replace("_", " ");
                string _StockStatusTrim = StockStatus.Trim();

                switch (_StockStatusTrim)
                {
                    case "IN STOCK":
                        _Tbt_Stock_Status = "<span style=\"color:#43A246;\"><b>INSTOCK</b></span><br>";
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "Limited Stock, Please Call":

                        _Tbt_Stock_Status = "<span style=\"color:#f69e1b;\"><b>Limited Stock</b></span><br>";
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER":
                        _Colorcode = "#43A246";
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=\"color:#43A246;\"><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=\"color:#43A246;\"><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "SPECIAL ORDER PRICE &":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=\"color:#43A246;\"><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "DISCONTINUED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=\"color:#ED1C24;\">DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER AVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=\"color:#ED1C24;\">DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=\"color:#ED1C24;\">DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "TEMPORARY UNAVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=\"color:#F9A023;\">TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "TEMPORARY UNAVAILABLE NO ETA":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=\"color:#F9A023;\">TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "OUT OF STOCK":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=\"color:#F9A023;\">OUT OF STOCK</span><br>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=\"color:#43A246;\"> <b>ITEM WILL BE BACK ORDERED</b> </span>";
                        break;
                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=\"color:#F9A023;\">OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=\"color:#43A246;\"><b>ITEM WILL BE BACK ORDERED</b></span>";
                        break;
                    case "OUT OF STOCK ITEM WILL":
                        _Tbt_Stock_Status_3 = "<span style=\"color:#F9A023;\">OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=\"color:#43A246;\"><b>ITEM WILL BE BACK ORDERED</b></span>";
                        break;
                    default:
                        _Colorcode = "Black";
                        _Tbt_Stock_Status = _StockStatusTrim;
                        break;
                }


                dsPriceTable = objHelperDb.GetProductPriceTable(ProductID, Convert.ToInt32(userid));
                dsPriceTable.Tables[0].TableName = "PriceTable";


                //_sPriceTable = "<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"table table-striped  table-bordered table-condensed\"><tr class=\" success\">";
                //_sPriceTable += "<td width=\"100\" height=\"20\" valign=\"top\" class=\"table_bdr td\"><b>ORDER CODE:</b></td><td width=\"100\" valign=\"top\" class=\"table_bdr\"><b>STOCK STATUS</b></td></tr>";
                //_sPriceTable += "<tr><td>";
                //_sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
              

                //if (_Tbt_Stock_Status != "")
                //{
                //    _sPriceTable += string.Format("<td>{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
                //}
                //else
                //{
                //    _sPriceTable += string.Format("<td>{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
                //}

                _sPriceTable += "<table  class=\"table table-striped  table-bordered table-condensed\" style=\"margin:0; background:#FFF\">";
                _sPriceTable += "<tr class=\"success\"><td width=\"28%\">ORDER CODE:</td><td colspan=\"2\">STOCK STATUS</td></tr>";
                _sPriceTable += string.Format("<tr><td width=\"28%\">{0}</td>", _sCODE);
                if (_Tbt_Stock_Status != "")
                {
                    _sPriceTable += string.Format("<td colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status);
                }
                else
                {
                    _sPriceTable += string.Format("<td colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status_1);
                }
                _sPriceTable += "<tr class=\"success\"><td>Qty</td><td width=\"38%\">Cost Inc GST</td><td width=\"34%\">Cost Ex GST</td></tr>";

                int TotalCount = 0;
                int RowCount = 0;

                if (pricecode == 3)
                    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                    {
                        _sPriceTable += string.Format("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                    }
                else
                {
                    bool bLastRow = false;
                    TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
                    RowCount = 0;

                    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                    {
                        RowCount = RowCount + 1;
                        if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
                        {
                            bLastRow = true;
                        }

                        string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                        _sPriceTable += string.Format("<tr><td class=\"{3}\">{0}</td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                    }
                }
                _sPriceTable += "</table>";
                // _sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table></td></tr></table>";
                //_sPriceTable += "</table><div class=\"clear\"></div>";
                //_sPriceTable += "<div class=\"popupaero\"></div>";


                
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                _sPriceTable = "";//<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                //if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
            }
            return _sPriceTable;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PRICE TABLE BASED ON THE MATCH CASES  ***/
        /********************************************************************************/
        public string AssemblePriceTable(int ProductID, int _priceCode, string _ProCode, string _ProStkStatus, string _Pro_stock_Status, string CustomerType, int user_id, string  _Pro_stock_Flag, string _Eta, DataSet Ds)
        {
            StringBuilder _sPriceTable = new StringBuilder();
            //SqlConnection oSQLCon = null;
            try
            {
                /*********************************** OLD CODE ***********************************/
                //////string userid = HttpContext.Current.Session["USER_ID"].ToString();
                //////DataSet dsPriceTable = new DataSet();
                ////////string sSQL = string.Format("select price_code from wes_customer where wes_customer_id in (select company_id from tbwc_company_buyers where user_id = {0})", userid);
                ////////oHelper.SQLString = sSQL;
                ////////int pricecode = oHelper.CI(oHelper.GetValue("price_code"));
                ////////DataSet dsPriceTable = new DataSet();
                ////////oSQLCon = new SqlConnection(oCon.ConnectionString.Replace("provider=SQLOLEDB;", ""));
                ////////oSQLCon.Open();
                ////////SqlCommand oCmd = new SqlCommand("SELECT STRING_VALUE FROM TB_PROD_SPECS WHERE PRODUCT_ID = @PRODUCT_ID and ATTRIBUTE_ID = 1", oSQLCon);
                ////////oCmd.Parameters.Clear();
                ////////oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                ////////string _sCODE = oCmd.ExecuteScalar().ToString();

                ////////oCmd = new SqlCommand("select PROD_STK_STATUS_DSC from WESTB_PRODUCT_ITEM WHERE PRODUCT_ID = @PRODUCT_ID", oSQLCon);
                ////////oCmd.Parameters.Clear();
                ////////oCmd.Parameters.AddWithValue("@PRODUCT_ID", ProductID);
                ////////string stkstatus = oCmd.ExecuteScalar().ToString();
                //////int pricecode = _priceCode;
                //////string _sCODE = _ProCode;
                //////string stkstatus = _ProStkStatus;
                //////string _Tbt_Stock_Status = "";
                //////string _Tbt_Stock_Status_1 = "";
                //////bool _Tbt_Stock_Status_2 = false;
                //////string _Tbt_Stock_Status_3 = "";
                //////string _Colorcode1 = "";
                //////string _Colorcode;
                //////string StockStatus = stkstatus.Replace("_", " ");
                //////string _StockStatusTrim = StockStatus.Trim();

                //////switch (_StockStatusTrim)
                //////{
                //////    case "IN STOCK":
                //////        _Tbt_Stock_Status = "<span style=color:#43A246><b>INSTOCK</b></span><br>";
                //////        _Tbt_Stock_Status_2 = true;
                //////        break;
                //////    case "SPECIAL ORDER":
                //////        _Colorcode = "#43A246";
                //////        _Tbt_Stock_Status_2 = true;
                //////        _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                //////        break;
                //////    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                //////        _Tbt_Stock_Status_2 = true;
                //////        _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                //////        break;
                //////    case "SPECIAL ORDER PRICE &":
                //////        _Tbt_Stock_Status_2 = true;
                //////        _Tbt_Stock_Status = "<span style=color:#43A246><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                //////        break;
                //////    case "DISCONTINUED":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                //////        break;
                //////    case "DISCONTINUED NO LONGER AVAILABLE":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                //////        break;
                //////    case "DISCONTINUED NO LONGER":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status_3 = "<span style=color:#ED1C24>DISCONTINUED NO LONGER AVAILABLE</span><br>";
                //////        break;
                //////    case "TEMPORARY UNAVAILABLE":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
                //////        break;
                //////    case "TEMPORARY UNAVAILABLE NO ETA":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status = "<span style=color:#F9A023>TEMPORARY UNAVAILABLE NO ETA</span>";
                //////        break;
                //////    case "OUT OF STOCK":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br>";
                //////        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246> <b>ITEM WILL BE BACK ORDERED</b> </span>";
                //////        break;
                //////    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
                //////        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
                //////        break;
                //////    case "OUT OF STOCK ITEM WILL":
                //////        _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
                //////        _Tbt_Stock_Status_2 = false;
                //////        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
                //////        break;
                //////    default:
                //////        _Colorcode = "Black";
                //////        _Tbt_Stock_Status = _StockStatusTrim;
                //////        break;
                //////}

                ////////SqlDataAdapter oDa = new SqlDataAdapter();
                ////////oDa.SelectCommand = new SqlCommand();
                ////////oDa.SelectCommand.Connection = oSQLCon;
                ////////oDa.SelectCommand.CommandText = "GetPriceTable";
                ////////oDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                ////////oDa.SelectCommand.Parameters.Clear();
                ////////oDa.SelectCommand.Parameters.AddWithValue("@ProductID", ProductID);
                ////////oDa.SelectCommand.Parameters.AddWithValue("@UserID", userid);
                ////////oDa.Fill(dsPriceTable, "PriceTable");
                //////if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                //////{
                //////    DataRow[] dr = Ds.Tables[0].Select("PRODUCT_ID='" + ProductID.ToString() + "'");
                //////    if (dr.Length > 0)
                //////    {
                //////        dsPriceTable.Tables.Add(dr.CopyToDataTable().Copy());
                //////        dsPriceTable.Tables[0].TableName = "PriceTable";
                //////    }
                //////}
                //////else
                //////    return "";

                ////////dsPriceTable = objHelperDb.GetProductPriceTable(ProductID, Convert.ToInt32(userid));
                ////////dsPriceTable.Tables[0].TableName = "PriceTable";

                //////_sPriceTable = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"1\" bgcolor=\"black\"><tr><td><table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                //////_sPriceTable += "<td width=\"100\" height=\"39\" valign=\"top\" class=\"pad2\"><b>ORDER CODE:</b><br />";
                //////_sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
                //////_sPriceTable += "<td width=\"100\" valign=\"top\" class=\"pad1\"><b>STOCK STATUS</b><br />";
                //////if (_Tbt_Stock_Status != "")
                //////{
                //////    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
                //////}
                //////else
                //////{
                //////    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
                //////}
                //////_sPriceTable += "<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"table_bdr\"><tr class=\"bg_grey3\"><td><b>Qty</b></td><td><b>Cost Inc GST</b></td><td><b>Cost Ex GST</b></td></tr>";

                //////int TotalCount = 0;
                //////int RowCount = 0;

                //////if (pricecode == 3)
                //////    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                //////    {
                //////        _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                //////    }
                //////else
                //////{
                //////    bool bLastRow = false;
                //////    TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
                //////    RowCount = 0;

                //////    foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                //////    {
                //////        RowCount = RowCount + 1;
                //////        if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
                //////        {
                //////            bLastRow = true;
                //////        }

                //////        string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                //////        _sPriceTable += string.Format("<tr><td class=\"{3}\"><b>{0}</b></td><td  class=\"{3}\" align=\"center\">${1:0.00}</td><td class=\"{3}\" align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                //////    }
                //////}

                //////_sPriceTable += "</table></td></tr><tr><td colspan=\"2\" height=\"4\"></td></tr></table></td></tr></table>";
                ////////if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
                /*********************************** OLD CODE ***********************************/

                string userid = HttpContext.Current.Session["USER_ID"].ToString();

                if (userid == string.Empty)
                    userid =  ConfigurationManager.AppSettings["DUM_USER_ID"];

                DataSet dsPriceTable = new DataSet();
               
                int pricecode = _priceCode;
                string _sCODE = _ProCode;
                string stkstatus = _ProStkStatus;
                string _Tbt_Stock_Status = string.Empty;
                string _Tbt_Stock_Status_1 = string.Empty;
                bool _Tbt_Stock_Status_2 = false;
                string _Tbt_Stock_Status_3 = string.Empty;
                string _Colorcode1 = string.Empty;
                string _Colorcode = string.Empty;
                string StockStatus = stkstatus.Replace("_", " ");
                string _StockStatusTrim = StockStatus.Trim();


                bool isProductReplace = true;



               // if ((_StockStatusTrim.ToUpper().Contains("OUT OF STOCK ITEM WILL BE BACK ORDERED") || _StockStatusTrim.ToUpper().Contains("SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED")))//&& CustomerType.ToLower() == "dealer"
                //    isProductReplace = false;
               // else
             //   {
                    //if (_Pro_stock_Status.ToLower() == "true" || _Pro_stock_Status.ToLower() == "1")                    
                    //    isProductReplace = false;
                    //else 
                        if (_Pro_stock_Flag == "0")
                            isProductReplace = false;
              //  }


                switch (_StockStatusTrim)
                {
                    case "IN STOCK":
                        _Tbt_Stock_Status = "<span class=\"green_clr\">INSTOCK</span>";
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "Limited Stock, Please Call":
                        _Colorcode = "#f69e1b";
                        _Tbt_Stock_Status = "<span style=\"color:#f69e1b;\"><b>Limited Stock - Please Call</b></span>";
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER":
                        _Colorcode = "#43A246";
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=\"color:#43A246;\"><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span>";
                        break;
                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=\"color:#43A246;\"><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span>";
                        break;
                    case "SPECIAL ORDER PRICE &":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=\"color:#43A246;\"><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span>";
                        break;
                    case "DISCONTINUED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=\"color:#ED1C24;\">DISCONTINUED NO LONGER AVAILABLE</span>";
                        break;
                    case "DISCONTINUED NO LONGER AVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=\"color:#ED1C24;\">DISCONTINUED NO LONGER AVAILABLE</span>";
                        break;
                    case "DISCONTINUED NO LONGER":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=\"color:#ED1C24;\">DISCONTINUED NO LONGER AVAILABLE</span>";
                        break;
                    case "TEMPORARY UNAVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=\"color:#F9A023;\">TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "TEMPORARY UNAVAILABLE NO ETA":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=\"color:#F9A023;\">TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "OUT OF STOCK":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=\"color:#F9A023;\">OUT OF STOCK</span>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=\"color:#43A246;\"> <b>ITEM WILL BE BACK ORDERED</b> </span>";
                        break;
                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=\"color:#F9A023;\">OUT OF STOCK</span>";
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=\"color:#43A246;\"><b>ITEM WILL BE BACK ORDERED</b></span>";
                        break;
                    case "OUT OF STOCK ITEM WILL":
                        _Tbt_Stock_Status_3 = "<span style=\"color:#F9A023;\">OUT OF STOCK</span>";
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=\"color:#43A246;\"><b>ITEM WILL BE BACK ORDERED</b></span>";
                        break;
                    default:
                        _Colorcode = "Black";
                        _Tbt_Stock_Status = _StockStatusTrim;
                        break;
                }

                if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = Ds.Tables[0].Select("PRODUCT_ID='" + ProductID.ToString() + "'");
                    if (dr.Length > 0)
                    {
                        dsPriceTable.Tables.Add(dr.CopyToDataTable().Copy());
                        dsPriceTable.Tables[0].TableName = "PriceTable";
                    }
                }
                else
                    return "";

              

                //_sPriceTable = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"1\" bgcolor=\"black\"><tr><td><table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                //_sPriceTable += "<td width=\"100\" height=\"39\" valign=\"top\" class=\"pad2\"><b>ORDER CODE:</b><br />";
                //_sPriceTable += string.Format("<span class=\"#00CC00\"><b>{0}</b></span></td>", _sCODE);
                //_sPriceTable += "<td width=\"100\" valign=\"top\" class=\"pad1\"><b>STOCK STATUS</b><br />";
                //if (_Tbt_Stock_Status != "")
                //{
                //    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status);
                //}
                //else
                //{
                //    _sPriceTable += string.Format("{0}</td></tr><tr><td colspan=\"2\" valign=\"top\">", _Tbt_Stock_Status_1);
                //}
                //_sPriceTable += "<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\" class=\"table_bdr\"><tr class=\"bg_grey3\"><td><b>Qty</b></td><td><b>Cost Inc GST</b></td><td><b>Cost Ex GST</b></td></tr>";



                _sPriceTable.Append("<table  class=\"table table-bordered table_bg margin_bottom_none\" >");
                _sPriceTable.Append(" <thead class=\"tool_td\"><tr><th>ORDER CODE:</th><th colspan=\"2\">STOCK STATUS</th></tr></thead>");
                _sPriceTable.Append(" <tbody class=\"tool_td\">");
                if (isProductReplace == true)
                {
                    string _catid = "", pfid = "", Ea_Path = "", wag_product_code = "", SubstuyutePid = "";
                    bool samecodesubproduct = false;
                    bool samecodenotFound = false;
                    DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_sCODE, user_id,"pd");
                    if (rtntbl != null && rtntbl.Rows.Count > 0)
                    {

                        _catid = rtntbl.Rows[0]["CatId"].ToString();
                        pfid = rtntbl.Rows[0]["Pfid"].ToString();
                      //  Ea_Path = "/"+ rtntbl.Rows[0]["Ea_Path"].ToString();
                        Ea_Path =  rtntbl.Rows[0]["Ea_Path"].ToString();
                        samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];
                        samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                        wag_product_code = rtntbl.Rows[0]["wag_product_code"].ToString();
                        SubstuyutePid = rtntbl.Rows[0]["SubstuyutePid"].ToString();
                    }
                    else
                    {
                        samecodesubproduct = true;
                        samecodenotFound = false;
                    }
                    if (samecodenotFound == false && samecodesubproduct == true)
                    {
                        _sPriceTable.AppendFormat("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _sCODE);
                        
                        if (_Tbt_Stock_Status != string.Empty)
                        {
                            _sPriceTable.AppendFormat("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status);                            
                        }
                        else
                        {
                            _sPriceTable.AppendFormat("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status_1);                            
                        }

                    }
                    else if (samecodenotFound == false && samecodesubproduct == false)
                    {
                        _sPriceTable.AppendFormat("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _sCODE);
                        _sPriceTable.AppendFormat("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", "Product Not Available.");
                        _sPriceTable.Append("<tr class=\"success\"><td colspan=\"3\"><br>RECOMMENDED REPLACEMENT<br><br></td></tr>");
                        _sPriceTable.Append("<tr><td colspan=\"3\">");
                        _sPriceTable.Append("<br>Order Code : " + "<span  style=\"color:green;font-weight: bold;\">" + wag_product_code + "</span> <br>");
                        string strurl = "ProductDetails.aspx?Pid=" + SubstuyutePid + "&amp;fid=" + pfid + "&amp;Cid=" + _catid + "&amp;path=" + Ea_Path;
                        _sPriceTable.Append("<br><a href =\"" + Ea_Path + "\" style=\"font-weight: bold; text-decoration: none; color: #1589FF;\" > View Replacement Product </a>");
                        _sPriceTable.Append("<br><br></td></tr>");

                    }
                    else
                    {
                        _sPriceTable.AppendFormat("<tr><td style=\"color:red;\"width=\"28%\">{0}</td>", _sCODE);

                        if (_Tbt_Stock_Status != string.Empty)
                        {

                            _sPriceTable.AppendFormat("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status);
                        }
                        else
                        {
                            _sPriceTable.AppendFormat("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status_1);
                        }
                        
                    }


                }
                else
                {
                    _sPriceTable.AppendFormat("<tr><td>{0}</td>", _sCODE);
                    if (_Tbt_Stock_Status != string.Empty)
                    {
                        _sPriceTable.AppendFormat("<td colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status);
                    }
                    else
                    {
                        _sPriceTable.AppendFormat("<td colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status_1);
                    }

                    if (_Eta != "")
                    {
                        _sPriceTable.AppendFormat("<tr><td><b>ETA</b></td><td colspan=\"2\"><b>" + _Eta + "</b></td></tr>");

                    }

                    _sPriceTable.AppendFormat("<tr><td>Qty</td><td>Cost Inc GST</td><td>Cost Ex GST</td></tr>");

                    int TotalCount = 0;
                    int RowCount = 0;

                    if (pricecode == 3)
                        foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                        {
                            // _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                            _sPriceTable.AppendFormat("<tr><td class=\"bg_grey3\">{0}</td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                        }
                    else
                    {
                        bool bLastRow = false;
                        TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
                        RowCount = 0;

                        foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                        {
                            RowCount = RowCount + 1;
                            if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
                            {
                                bLastRow = true;
                            }

                            string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                            _sPriceTable.AppendFormat("<tr><td>{0}</td><td>${1:0.00}</td><td>${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                        }
                    }
                }
                _sPriceTable.AppendFormat("</tbody></table>");
               // _sPriceTable += "</table><div class=\"clear\"></div>";
              //  _sPriceTable += "<div class=\"popupaero\"></div>";
               

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                _sPriceTable = new StringBuilder();
                //<table width=\"200\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" class=\"bg_grey3\"><tr>";
                //if (oSQLCon.State == ConnectionState.Open) oSQLCon.Close();
            }
            return _sPriceTable.ToString();
        }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE PRICE TABLE POPUP Microsite  ***/
        /********************************************************************************/
        public string AssemblePriceTableMS(int ProductID, int _priceCode, string _ProCode, string _ProStkStatus, string _Pro_stock_Status, string CustomerType, int user_id, string _Pro_stock_Flag, string _Eta, DataSet Ds)
        {
            StringBuilder _sPriceTable = new StringBuilder(); ;
         //   SqlConnection oSQLCon = null;
            try
            {
               

                string userid = HttpContext.Current.Session["USER_ID"].ToString();

                if (userid == string.Empty)
                    userid = ConfigurationManager.AppSettings["DUM_USER_ID"];

                DataSet dsPriceTable = new DataSet();

                int pricecode = _priceCode;
                string _sCODE = _ProCode;
                string stkstatus = _ProStkStatus;
                string _Tbt_Stock_Status = string.Empty;
                string _Tbt_Stock_Status_1 = string.Empty;
                bool _Tbt_Stock_Status_2 = false;
                string _Tbt_Stock_Status_3 = string.Empty;
                string _Colorcode1 = string.Empty;
                string _Colorcode;
                string StockStatus = stkstatus.Replace("_", " ");
                string _StockStatusTrim = StockStatus.Trim();

                bool isProductReplace = true;



                //if ((_StockStatusTrim.ToUpper().Contains("OUT OF STOCK ITEM WILL BE BACK ORDERED") || _StockStatusTrim.ToUpper().Contains("SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED"))) //&& CustomerType.ToLower() == "dealer"
                //    isProductReplace = false;
                //else
                //{
                //    if (_Pro_stock_Status.ToLower() == "true" || _Pro_stock_Status.ToLower() == "1")
                //        isProductReplace = false;
                //    else if (_Pro_stock_Flag.ToLower() == "0")
                //        isProductReplace = false;
                //}
                if (_Pro_stock_Flag == "0")
                    isProductReplace = false;

                switch (_StockStatusTrim)
                {
                    case "IN STOCK":
                        _Tbt_Stock_Status = "<span style=\"color:#43A246\"><b>INSTOCK</b></span><br>";
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "Limited Stock, Please Call":
                        _Colorcode = "#f69e1b";
                        _Tbt_Stock_Status = "<span style=\"color:#f69e1b;\"><b>Limited Stock - Please Call</b></span>";
                        _Tbt_Stock_Status_2 = true;
                        break;
                    case "SPECIAL ORDER":
                        _Colorcode = "#43A246";
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=\"color:#43A246\"><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "SPECIAL ORDER PRICE & AVAILABILITY TO BE CONFIRMED":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=\"color:#43A246\"><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "SPECIAL ORDER PRICE &":
                        _Tbt_Stock_Status_2 = true;
                        _Tbt_Stock_Status = "<span style=\"color:#43A246\"><b>SPECIAL ORDER PRICE & AVAILABILTY TO BE CONFIRMED</b></span><br>";
                        break;
                    case "DISCONTINUED":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=\"color:#ED1C24\">DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER AVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=\"color:#ED1C24\">DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "DISCONTINUED NO LONGER":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status_3 = "<span style=\"color:#ED1C24\">DISCONTINUED NO LONGER AVAILABLE</span><br>";
                        break;
                    case "TEMPORARY UNAVAILABLE":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=\"color:#F9A023\">TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "TEMPORARY UNAVAILABLE NO ETA":
                        _Tbt_Stock_Status_2 = false;
                        _Tbt_Stock_Status = "<span style=\"color:#F9A023\">TEMPORARY UNAVAILABLE NO ETA</span>";
                        break;
                    case "OUT OF STOCK":
                        _Tbt_Stock_Status_2 = false;
                       // _Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br>";
                        //_Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246> <b>ITEM WILL BE BACK ORDERED</b> </span>";
                        _Tbt_Stock_Status_3 = "<span>Contact us for ETA</span>";
                        _Tbt_Stock_Status_1 = "<span>Contact us for ETA</span>";
                        break;
                    case "OUT OF STOCK ITEM WILL BE BACK ORDERED":
                        _Tbt_Stock_Status_2 = false;
                        //_Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
                        //_Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
                        _Tbt_Stock_Status_3 = "<span>Contact us for ETA</span>";
                        _Tbt_Stock_Status_1 = "<span>Contact us for ETA</span>";
                        break;
                    case "OUT OF STOCK ITEM WILL":
                        //_Tbt_Stock_Status_3 = "<span style=color:#F9A023>OUT OF STOCK</span><br/>";
                        _Tbt_Stock_Status_2 = false;
                        //_Tbt_Stock_Status_1 = _Tbt_Stock_Status_3 + "<span style=color:#43A246><b>ITEM WILL BE BACK ORDERED</b></span>";
                        _Tbt_Stock_Status_3 = "<span>Contact us for ETA</span>";
                        _Tbt_Stock_Status_1 = "<span>Contact us for ETA</span>";
                        break;
                    default:
                        _Colorcode = "Black";
                        _Tbt_Stock_Status = _StockStatusTrim;
                        break;
                }

                if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = Ds.Tables[0].Select("PRODUCT_ID='" + ProductID.ToString() + "'");
                    if (dr.Length > 0)
                    {
                        dsPriceTable.Tables.Add(dr.CopyToDataTable().Copy());
                        dsPriceTable.Tables[0].TableName = "PriceTable";
                    }
                }
                else
                    return "";



                _sPriceTable.Append( "<table  class=\"table table-bordered table_bg margin_bottom_none\" >");
                _sPriceTable.Append( " <thead class=\"tool_td\"><tr><th>ORDER CODE:</th><th colspan=\"2\">STOCK STATUS</th></tr></thead>");
                _sPriceTable.Append( " <tbody class=\"tool_td\">");

                if (isProductReplace == true)
                {
                    string _catid = "", pfid = "", Ea_Path = "", wag_product_code = "", SubstuyutePid = "";
                    bool samecodesubproduct = false;
                    bool samecodenotFound = false;
                    DataTable rtntbl = objProductServices.GetSubstituteProductDetails(_sCODE, user_id, "mpd");
                    if (rtntbl != null && rtntbl.Rows.Count > 0)
                    {

                        _catid = rtntbl.Rows[0]["CatId"].ToString();
                        pfid = rtntbl.Rows[0]["Pfid"].ToString();
                       // Ea_Path = "/" + rtntbl.Rows[0]["Ea_Path"].ToString();
                        Ea_Path =  rtntbl.Rows[0]["Ea_Path"].ToString();
                        samecodesubproduct = (bool)rtntbl.Rows[0]["samecodesubproduct"];
                        samecodenotFound = (bool)rtntbl.Rows[0]["samecodenotFound"];
                        wag_product_code = rtntbl.Rows[0]["wag_product_code"].ToString();
                        SubstuyutePid = rtntbl.Rows[0]["SubstuyutePid"].ToString();
                    }
                    else
                    {
                        samecodesubproduct = true;
                        samecodenotFound = false;
                    }
                    if (samecodenotFound == false && samecodesubproduct == true)
                    {
                        _sPriceTable.AppendFormat("<tr><td style=\"color:red;\">{0}</td>", _sCODE);
                        
                        if (_Tbt_Stock_Status != "")
                        {
                            _sPriceTable.AppendFormat("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status);                            
                        }
                        else
                        {
                            _sPriceTable.AppendFormat("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status_1);                            
                        }
                    }
                    else if (samecodenotFound == false && samecodesubproduct == false)
                    {
                        _sPriceTable.AppendFormat("<tr><td style=\"color:red;\">{0}</td>", _sCODE);
                        _sPriceTable.AppendFormat("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", "Product Not Available.");
                        _sPriceTable.Append( "<tr><td colspan=\"3\" align=\"center\"><br>RECOMMENDED REPLACEMENT<br><br></td></tr>");
                        _sPriceTable.Append( "<tr><td colspan=\"3\" align=\"center\">");
                        _sPriceTable.Append( "<br>Order Code : " + "<span  style=\"color:green;font-weight: bold;\">" + wag_product_code + "</span> <br>");
                       // string strurl = "ProductDetails.aspx?Pid=" + SubstuyutePid + "&amp;fid=" + pfid + "&amp;Cid=" + _catid + "&amp;path=" + Ea_Path;
                        _sPriceTable.Append( "<br><a href =\"" + Ea_Path + "\" style=\"font-weight: bold; text-decoration: none; color: #1589FF;\" > View Replacement Product </a>");
                        _sPriceTable.Append( "<br><br></td></tr>");

                    }
                    else
                    {
                        _sPriceTable.AppendFormat("<tr><td style=\"color:red;\">{0}</td>", _sCODE);
                        
                        if (_Tbt_Stock_Status != "")
                        {
                            _sPriceTable.AppendFormat("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status);                            
                        }
                        else
                        {
                            _sPriceTable.AppendFormat("<td style=\"color:red;\" colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status_1);
                        }
                    }


                }
                else
                {
                    _sPriceTable.AppendFormat("<tr><td>{0}</td>", _sCODE);
                    if (_Tbt_Stock_Status != "")
                    {
                        _sPriceTable.AppendFormat("<td colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status);
                    }
                    else
                    {
                        _sPriceTable.AppendFormat("<td colspan=\"2\">{0}</td></tr>", _Tbt_Stock_Status_1);
                    }

                    if (_Eta != "")
                    {
                        if (_Eta.Contains("PLEASE CALL"))
                            _sPriceTable.AppendFormat("<tr><td><b>ETA</b></td><td colspan=\"2\"> <span style=\"text-transform: capitalize;\"> <b>" + _Eta.ToLower() + "</b> </span> </td></tr>");
                        else
                            _sPriceTable.AppendFormat("<tr><td><b>ETA</b></td><td colspan=\"2\"> <span style=\"text-transform: capitalize;\"> <b>" + " Available for shipping on " + _Eta + "</b> </span> </td></tr>");


                    }

                    _sPriceTable.Append( "<tr><td>Qty</td><td>Cost Inc GST</td><td>Cost Ex GST</td></tr>");

                    int TotalCount = 0;
                    int RowCount = 0;

                    if (pricecode == 3)
                        foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                        {
                            // _sPriceTable += string.Format("<tr><td class=\"bg_grey3\"><b>{0}</b></td><td align=\"center\">${1:0.00}</td><td align=\"center\">${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                            _sPriceTable.AppendFormat("<tr><td>{0}</td><td>${1:0.00}</td><td>${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"]);
                        }
                    else
                    {
                        bool bLastRow = false;
                        TotalCount = dsPriceTable.Tables["PriceTable"].Rows.Count;
                        RowCount = 0;

                        foreach (DataRow oDr in dsPriceTable.Tables["PriceTable"].Rows)
                        {
                            RowCount = RowCount + 1;
                            if (RowCount == TotalCount && (pricecode >= 4) && oDr["QTY"].Equals("Your Price"))  // check whether it is Last Row
                            {
                                bLastRow = true;
                            }

                            string _color = bLastRow ? "bg_grey31" : "bg_grey3";
                            _sPriceTable.AppendFormat("<tr><td><b>{0}</b></td><td>${1:0.00}</td><td>${2:0.00}</td></tr>", oDr["QTY"], oDr["Price1"], oDr["Price2"], _color);
                        }
                    }
                }
                _sPriceTable.Append("</tbody></table>");
               

            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                objErrorHandler.CreateLog();
                _sPriceTable = new StringBuilder();
            }
            return _sPriceTable.ToString();
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DISCOUNT DETAILS BASED ON THE BUYER GROUP  ***/
        /********************************************************************************/
        //public DataSet GetBuyerGroupBasedDiscountDetails(string BuyerGroup)
        //{
        //    try
        //    {
        //        //string sSQL = " SELECT DISCOUNT =";
        //        //sSQL = sSQL + " CASE";
        //        //sSQL = sSQL + " WHEN USE_PCT = 1 THEN DISC_PCT";
        //        //sSQL = sSQL + " ELSE DISC_AMT";
        //        //sSQL = sSQL + " END, VALID_DATE =";
        //        //sSQL = sSQL + " CASE";
        //        //sSQL = sSQL + " WHEN USE_PCT = 1 THEN DISC_PCT_TILL_DT";
        //        //sSQL = sSQL + " ELSE DISC_AMT_VALID_TILL_DT";
        //        //sSQL = sSQL + " END,DISC_METHOD =";
        //        //sSQL = sSQL + " CASE";
        //        //sSQL = sSQL + " WHEN USE_PCT = 1 THEN '" + DefaultBG.PERCENTAGEMETHOD.ToString() + "'";
        //        //sSQL = sSQL + " ELSE '" + DefaultBG.AMOUNTMETHOD.ToString() + "'";
        //        //sSQL = sSQL + " END FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP = '" + BuyerGroup + "'";
        //        //oHelper.SQLString = sSQL;
        //        //return oHelper.GetDataSet();
        //        return (DataSet)objHelperDb.GetGenericDataDB("", BuyerGroup, DefaultBG.PERCENTAGEMETHOD.ToString(), DefaultBG.AMOUNTMETHOD.ToString(), "GET_BUYER_GROUP_BASED_DISCOUNT", HelperDB.ReturnType.RTDataSet);
  
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
        /*** PURPOSE      : TO RETRIVE THE BUYER GROUP DETAILS ***/
        /********************************************************************************/
        //public string GetBuyerGroup(int UserID)
        //{
        //    string retVal;
        //    try
        //    {
        //        if (UserID > 0)
        //        {
        //            //string sSQL = " SELECT TBG.BUYER_GROUP FROM TBWC_BUYER_GROUP TBG,TBWC_COMPANY TC,TBWC_COMPANY_BUYERS TCB";
        //            //sSQL = sSQL + " WHERE TBG.BUYER_GROUP = TC.BUYER_GROUP";
        //            //sSQL = sSQL + " AND TC.COMPANY_ID = TCB.COMPANY_ID AND USER_ID =" + UserID;
        //            //oHelper.SQLString = sSQL;
        //            //retVal = oHelper.GetValue("BUYER_GROUP");
        //            retVal=(string)objHelperDb.GetGenericDataDB(UserID.ToString(), "GET_BUYER_GROUP", HelperDB.ReturnType.RTString);
        //        }
        //        else
        //        {
        //            retVal = DefaultBG.DEFAULTBG.ToString();
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        retVal = "";
        //    }
        //    return retVal;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE DISCOUNT PRICE FOR BUYER GROUP ***/
        /********************************************************************************/
        //public decimal CalculateBGDiscountPrice(decimal CurrentPrice, decimal DiscountValue, string DiscoundMethod)
        //{
        //    decimal retPrice = 0;
        //    try
        //    {
        //        if (DiscoundMethod == DefaultBG.PERCENTAGEMETHOD.ToString())
        //        {
        //            decimal DiscountPrice = (CurrentPrice * DiscountValue) / 100;
        //            retPrice = CurrentPrice - DiscountPrice;
        //        }
        //        else
        //        {
        //            retPrice = CurrentPrice - DiscountValue;

        //        }
        //        if (retPrice < 0)
        //        {
        //            retPrice = 0;
        //        }
        //        else
        //        {
        //            retPrice = objHelperServices.CDEC(retPrice.ToString("N2"));
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        retPrice = 0;
        //    }

        //    return retPrice;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  CHECK THE CATALOG ID BELONGS TO THE BUYER GROUP OR NOT ***/
        /********************************************************************************/
        //public bool IsBGCatalogProduct(int ProductCatalogID, string BuyerGroupName)
        //{
        //    bool retVal = false;
        //    string tempstr = string.Empty;
        //    try
        //    {
        //        //string sSQL = "SELECT CATALOG_ID FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP='" + BuyerGroupName + "'";
        //        //oHelper.SQLString = sSQL;
        //        //int BGCatalogID = oHelper.CI(oHelper.GetValue("CATALOG_ID").ToString());
        //        int BGCatalogID=0;
        //        tempstr=(string)objHelperDb.GetGenericDataDB(BuyerGroupName, "GET_BUYER_GROUP_CATALOG_ID", HelperDB.ReturnType.RTString);  
        //        if (tempstr!=null && tempstr!="")
        //            BGCatalogID = objHelperServices.CI(tempstr);

        //        if (ProductCatalogID == BGCatalogID)
        //        {
        //            retVal = true;
        //        }
        //        else
        //        {
        //            retVal = false;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        objErrorHandler.ErrorMsg = e;
        //        objErrorHandler.CreateLog();
        //        retVal = false;
        //        return retVal;
        //    }
        //    return retVal;
        //}

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  RETRIVE THE DETAILS OF FAMILY PAGE PRODUCTS ***/
        /********************************************************************************/
        public DataSet GetFamilyPageProduct(string familyid, string Option)
        {
            DataSet ds = new DataSet();
            SqlCommand objSqlCommand;
            SqlDataAdapter da;
            try
            {
                //SqlConnection Gcon = new SqlConnection();
                //Gcon.ConnectionString = conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1);
               if (familyid.StartsWith (",")==true)
                   familyid = familyid.Substring(1);

               // objSqlCommand = new SqlCommand("STP_TBWC_PICKFAMILYPAGEPRODUCT_WAGNER", objConnectionDB.GetConnection());
                objSqlCommand = new SqlCommand("STP_TBWC_PICKFAMILYPAGEPRODUCT", objConnectionDB.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.Add("@FamilyID", familyid);
                objSqlCommand.Parameters.Add("@OPTION", Option);
                da = new SqlDataAdapter(objSqlCommand);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                //objErrorHandler.CreateLog(e.ToString() + familyid + "," + Option);
            }
            finally
            {
                objSqlCommand = null;
                da = null;
            }

            return ds;
        }


        public string getleftattribute(string _fid)
        {
            string x = "";
            try
            {
                HelperDB objHelperDb = new HelperDB();
                DataSet tmpds1 = objHelperDb.GetDataSetDB("exec [STP_GETFAMILY_XML] " + _fid);
            //    objErrorHandler.CreateLog("exec [STP_GETFAMILY_XML] " + _fid);
                DataSet dsmrgattr = new DataSet();
                if (tmpds1 != null && tmpds1.Tables[0].Rows.Count > 0)
                {

                    dsmrgattr = ConvertXMLToDataSet(tmpds1.Tables[0].Rows[0][0].ToString());

                    //DataRow[] dr = dsmrgattr.Tables["LeftRowField"].Select("Merge='Checked'");
                    //if (dr.Length > 0)
                    //{
                    if ((dsmrgattr != null) && (dsmrgattr.Tables.Count > 0))
                    {
                        //  DataTable dt = dr.CopyToDataTable();
                        DataTable dt = dsmrgattr.Tables["LeftRowField"];
                        //   string x = string.Empty;
                        if (dt != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["AttrId"] != null)
                                {
                                    string[] a = dt.Rows[i]["AttrId"].ToString().Split(':');
                                    if (a.Length > 0)
                                    {
                                        if (i == 0)
                                        {
                                            // x = dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");

                                            x = a[1];
                                        }
                                        else
                                        {
                                            x = x + "," + a[1];
                                        }
                                    }




                                }


                            }
                        }
                        DataTable dtsummary = dsmrgattr.Tables["SummaryField"];

                        if (dtsummary != null)
                        {
                            for (int i = 0; i < dtsummary.Rows.Count; i++)
                            {
                                if (dtsummary.Rows[i]["AttrId"] != null)
                                {
                                    string[] a = dtsummary.Rows[i]["AttrId"].ToString().Split(':');
                                    if (a.Length > 0)
                                    {
                                        if (x =="")
                                        {
                                            // x = dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");

                                            x = a[1];
                                        }
                                        else
                                        {
                                            x = x + "," + a[1];
                                        }
                                    }




                                }


                            }
                        }

                        DataTable dtrightrow = dsmrgattr.Tables["RightRowField"];

                        if (dtrightrow != null)
                        {
                            for (int i = 0; i < dtrightrow.Rows.Count; i++)
                            {
                                if (dtrightrow.Rows[i]["AttrId"] != null)
                                {
                                    string[] a = dtrightrow.Rows[i]["AttrId"].ToString().Split(':');
                                    if (a.Length > 0)
                                    {
                                        if (x == "")
                                        {
                                            // x = dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");

                                            x = a[1];
                                        }
                                        else
                                        {
                                            x = x + "," + a[1];
                                        }
                                    }




                                }


                            }
                        }
                        DataTable dtColumnField = dsmrgattr.Tables["ColumnField"];

                        if (dtColumnField != null)
                        {
                            for (int i = 0; i < dtColumnField.Rows.Count; i++)
                            {
                                if (dtColumnField.Rows[i]["AttrId"] != null)
                                {
                                    string[] a = dtColumnField.Rows[i]["AttrId"].ToString().Split(':');
                                    if (a.Length > 0)
                                    {
                                        if (x == "")
                                        {
                                            // x = dt.Rows[i]["AttrId"].ToString().Replace("Attr:", "");

                                            x = a[1];
                                        }
                                        else
                                        {
                                            x = x + "," + a[1];
                                        }
                                    }




                                }


                            }
                        }
                        //   dssimilarcolumns = objHelperDb.GetDataSetDB("exec [STP_Attribute_name] '" + x + "'");
                        //objErrorHandler.CreateLog("exec [STP_Attribute_name] '" + x + "'" + "------" + _familyID);
                        //dsgetleftattr = objHelperDb.GetDataSetDB("exec [STP_Attribute_name] '" + x + "'");
                    }

                }
            }
            catch (Exception ex)
            {
                objErrorHandler.CreateLog(ex.ToString());
            }
           // objErrorHandler.CreateLog("x=" + x);
            return x;
        }
        public DataSet GetFamilyPageProduct_tabledesigner(string familyid, string Option, string LEFT_ATTRIBUTES)
        {
            DataSet ds = new DataSet();
            SqlCommand objSqlCommand;
            SqlDataAdapter da;
            try
            {
                //SqlConnection Gcon = new SqlConnection();
                //Gcon.ConnectionString = conStr.ConnectionString.ToString().Substring(conStr.ConnectionString.ToString().IndexOf(';') + 1);
                if (familyid.StartsWith(",") == true)
                    familyid = familyid.Substring(1);

                // objSqlCommand = new SqlCommand("STP_TBWC_PICKFAMILYPAGEPRODUCT_WAGNER", objConnectionDB.GetConnection());
                objSqlCommand = new SqlCommand("STP_TBWC_PICKFAMILYPAGEPRODUCT_TABLEDESIGNER", objConnectionDB.GetConnection());
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.Parameters.Add("@FamilyID", familyid);
                objSqlCommand.Parameters.Add("@OPTION", Option);
                objSqlCommand.Parameters.Add("@LEFT_ATTRIBUTES", Option);
                da = new SqlDataAdapter(objSqlCommand);
                da.Fill(ds);
            }
            catch (Exception e)
            {
                objErrorHandler.ErrorMsg = e;
                //objErrorHandler.CreateLog(e.ToString() + familyid + "," + Option);
            }
            finally
            {
                objSqlCommand = null;
                da = null;
            }

            return ds;
        }
        
    
    }
    /*********************************** J TECH CODE ***********************************/
    /*********************************** J TECH CODE ***********************************/
    //public class TBWDataList
    //{
    //    string _TBWDataListItem;
    //    public TBWDataList(string TBWDataListItem)
    //    {
    //        this._TBWDataListItem = TBWDataListItem;
    //    }
    //    public string TBWDataListItem { get { return _TBWDataListItem; } }
    //}
    //public class TBWDataList1
    //{
    //    string _TBWDataListItem1;
    //    public TBWDataList1(string TBWDataListItem1)
    //    {
    //        this._TBWDataListItem1 = TBWDataListItem1;
    //    }
    //    public string TBWDataListItem1 { get { return _TBWDataListItem1; } }
    //}
    //public class TBWDataList2
    //{
    //    string _TBWDataListItem2;
    //    public TBWDataList2(string TBWDataListItem2)
    //    {
    //        this._TBWDataListItem2 = TBWDataListItem2;
    //    }
    //    public string TBWDataListItem2 { get { return _TBWDataListItem2; } }
    //}
 

    /*********************************** J TECH CODE ***********************************/
}