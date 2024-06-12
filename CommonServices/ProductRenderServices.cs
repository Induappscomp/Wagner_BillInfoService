using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using TradingBell5.CatalogX;
using System.IO;
using System.Text;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebCat.CommonServices
{
    /// <summary>
    /// Summary description for ProductRender
    /// </summary>
   
    public class ProductRenderServices
    {
        #region Declarations
        HelperDB oHelper = new HelperDB();
        ConnectionDB oCon = new ConnectionDB();
        ErrorHandler oErr = new ErrorHandler();
        Order oOrder = new Order();
        DataSet CXAttributes = new DataSet();
        CSRender oCSRender = new CSRender();
        ProductPromotion oProdPro = new ProductPromotion();
        Product oProd = new Product();
        string PriceType = "";
        string CurrencyFormat = "";
        string Restricted = "NO";
        Boolean _NoProductImage;
        double _ProductImageHeight;
        double _ProductImageWidth;
        string _NoImageAvailableCaption = "No Image to Display";

        public Boolean NoProductImage
        {
            get
            {
                return _NoProductImage;
            }
            set
            {
                _NoProductImage = value;
            }
        }

        public double ProductImageHeight
        {
            get
            {
                return _ProductImageHeight;
            }
            set
            {
                _ProductImageHeight = value;
            }
        }

        public double ProductImageWidth
        {
            get
            {
                return _ProductImageWidth;
            }
            set
            {
                _ProductImageWidth = value;
            }
        }

        public string NoImageAvailableCaption
        {
            get
            {
                return _NoImageAvailableCaption;
            }
            set
            {
                _NoImageAvailableCaption = value;
            }
        }

        #endregion

        public ProductRenderServices()
        {


        }

        //This is used to Build the Product template
        /// <summary>
        /// This is used to Build the Product template
        /// </summary>
        /// <param name="oPRODData">DataSet</param>
        /// <param name="FamilyID">int</param>
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
        ///  string ProdTemplate;
        ///  DataSet oPRODData;
        ///  int FamilyID;
        ///  ProdTemplate = BuildProductTemplate(oPRODData,FamilyID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string BuildProductTemplate(int CatalogID,int FamilyID, int UserID, DataSet oPRODData, string HeaderContent, string WOHeaderContent,int Columns,string ProductView)
        {
            string HtmlTextFinal = "";
            string WOHeaderContentRep = "";
            string sSQL = "";
            int CheckIndex;
            int SortdescIndex;
            int CheckPrice=1;
            int count=1;
            int bld_cnt = 1;
            string FirstStr = "";
            string ProductImageAttr = "";
            string ProductZoomImageAttr = "";
            BuyerGroup oBG = new BuyerGroup();
            decimal untPrice = 0;
            DataSet dsBgDisc = new DataSet();
            StringBuilder strBuildr = new StringBuilder();
            string Attrcontent = "";         
            string Sortcont = "";
            string AttrName = "";
            DataSet oPRODDataDS = new DataSet();
            //oPRODDataDS = oPRODData.Copy();
            if (HttpContext.Current.Request["SortCol"] != null)
            {
                DataSet oDsSort = new DataSet();
                string SortCol = HttpContext.Current.Request["SortCol"].ToString();
                int Sortcontent = SortCol.IndexOf("]");
                string SortcontStr = SortCol.Substring(Sortcontent + 2);
                //DataSet StringDataSet = new DataSet();
                DataSet oDSAttr = oCSRender.GetAttributes(CatalogID, UserID);
                DataRow[] oDRAttr = oDSAttr.Tables[0].Select("ATTRIBUTE_ID=1");
                string CatAttrName = oDRAttr[0]["ATTRIBUTE_NAME"].ToString();
                oDRAttr = oDSAttr.Tables[0].Select("ATTRIBUTE_TYPE=4");
                string PriceAttrName = oDRAttr[0]["ATTRIBUTE_NAME"].ToString(); AttrName = PriceAttrName;
                PriceAttrName = "[" + PriceAttrName + "]" + " " + SortcontStr;
                //int Chksplsym = CatAttrName.IndexOf("#");
                //if (Chksplsym > -1)
                //{
                //    SortCol = SortCol.Insert(Sortcontent, " ");
                //    int Addspelsym = SortCol.IndexOf(" ", 0);
                //    SortCol = SortCol.Insert(Addspelsym + 1, "#");
                //}
                if (SortCol.ToString().Trim() == PriceAttrName || SortCol.ToString().Trim() == PriceAttrName)
                {
                    //oPRODDataDS = oPRODData.Copy();
                    DataTable oResultTable = new DataTable();
                    foreach (DataColumn oDC in oPRODData.Tables[0].Columns)
                    {

                        if (oDC.ColumnName.Equals(AttrName))
                        {
                            DataColumn oNewColumn = new DataColumn(oDC.ColumnName, System.Type.GetType("System.Decimal"));
                            oResultTable.Columns.Add(oNewColumn);
                        }
                        else
                        {
                            oResultTable.Columns.Add(oDC.ColumnName.ToString(), System.Type.GetType(oDC.DataType.FullName.ToString()));
                        }
                    }
                    DataRow[] oDR = oPRODData.Tables[0].Select();
                    foreach (DataRow oDataRow in oDR)
                    {
                        oResultTable.ImportRow(oDataRow);
                    }
                    oPRODDataDS.Tables.Add(oResultTable);
                    
                    //Sorting Based on arguments columns..
                    DataRow[] oDrSort = oPRODDataDS.Tables[0].Select("", SortCol);
                    oDsSort = oPRODDataDS.Clone();
                    oPRODDataDS.Tables.Clear();

                    foreach (DataRow oDr in oDrSort)
                    {
                        oDsSort.Tables[0].ImportRow(oDr);
                    }
                    oPRODDataDS = oDsSort;
                    oResultTable.Dispose();
                }
                else
                {
                    //copy data from original DataSet to Temp DS
                    int Chksplsym = CatAttrName.IndexOf("#");//Check Attribute contain # symbol;
                    if (Chksplsym > -1)
                    {
                        SortCol = SortCol.Insert(Sortcontent, " ");
                        int Addspelsym = SortCol.IndexOf(" ", 0);
                        SortCol = SortCol.Insert(Addspelsym + 1, "#");
                    }
                    oPRODDataDS = oPRODData.Copy();
                    DataRow[] oDrSort = oPRODDataDS.Tables[0].Select("", SortCol);
                    oDsSort = oPRODDataDS.Clone();
                    oPRODDataDS.Tables.Clear();

                    foreach (DataRow oDr in oDrSort)
                    {
                        oDsSort.Tables[0].ImportRow(oDr);
                    }
                    oPRODDataDS = oDsSort;
                }
                oDsSort.Dispose();
                oDSAttr.Dispose();
            }
            else
            {
                //copy data from original DataSet to Temp DS
                oPRODDataDS = oPRODData.Copy();
            }  

            try
            {
                string HypC = "";
                string HypCURL = oHelper.GetOptionValues("NAVIGATIONURL").ToString();
                string RestrictedOption = oHelper.GetOptionValues("ENABLED RESTRICTED PRODUCT").ToString();
                string RestrictedUrl = oHelper.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                if (UserID > 0)
                {
                    sSQL = " SELECT ATTRIBUTE_NAME"
                        + " FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID IN("
                        + " SELECT PRICE_ATTRIBUTE_ID FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP IN("
                        + " SELECT  BUYER_GROUP FROM TBWC_COMPANY WHERE COMPANY_ID IN("
                        + " SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID =" + UserID + ")))";
                }
                else
                {
                    sSQL = " SELECT ATTRIBUTE_NAME"
                        + " FROM TB_ATTRIBUTE WHERE ATTRIBUTE_ID IN("
                        + " SELECT PRICE_ATTRIBUTE_ID FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP ='DEFAULTBG')";

                }
                oHelper.SQLString = sSQL;
                PriceType = oHelper.GetValue("ATTRIBUTE_NAME");
                CurrencyFormat = oHelper.GetOptionValues("CURRENCYFORMAT");
                CXAttributes = oCSRender.GetAttributes(CatalogID, UserID);
                //get sort description
                SortdescIndex = WOHeaderContent.IndexOf("{FLAT_VIEW_SORTING}", 0);
                if (SortdescIndex > 0)
                {
                    Sortcont = WOHeaderContent.Substring(0, SortdescIndex);
                    WOHeaderContent = WOHeaderContent.Remove(0, SortdescIndex);
                }
                if (ProductView == "FLAT")
                {
                    WOHeaderContent = WOHeaderContent.Trim();
                    //DataSet AttriName = new DataSet();
                    strBuildr.Append("<TABLE BORDER=\"0\" CELLPADDING =\"0\" CELLSPACING =\"0\"><TR><TD><SELECT id=\"SelSort\" style=\"font-family:Arial Unicode MS:Arial;font-size:12px;\">");
                    DataSet oDSAttr = oCSRender.GetAttributes(CatalogID, UserID);
                    DataRow[] oDRAttr = oDSAttr.Tables[0].Select("ATTRIBUTE_ID=1");
                    string CatAttrName = oDRAttr[0]["ATTRIBUTE_NAME"].ToString();
                    oDRAttr = oDSAttr.Tables[0].Select("ATTRIBUTE_TYPE=4");
                    string PriceAttrName = oDRAttr[0]["ATTRIBUTE_NAME"].ToString();
                    string[] SeparateLine = new string[] { "" };
                    string[] StrSeparator = new string[] { "{" };
                    SeparateLine = WOHeaderContent.Split(StrSeparator, StringSplitOptions.None);
                    
                    for (int i = 1; i < SeparateLine.Length; i++)
                    {
                        //if (SeparateLine[i]!= "")
                        //{
                            int endIndex = SeparateLine[i].LastIndexOf("}");
                            string IsattriName = SeparateLine[i].Substring(0, endIndex);

                            if (IsattriName.Equals(CatAttrName))
                            {
                                int chksym = CatAttrName.IndexOf("#");
                                if (chksym > -1)
                                {
                                    string splsym = CatAttrName.Replace("#", "");
                                    splsym = splsym.Trim();
                                    strBuildr.Append("<OPTION SELECTED=\"SELECTED\" VALUE= \"" + "[" + splsym + "] ASC" + "\" >Item Ascending</OPTION>");
                                    strBuildr.Append("<OPTION VALUE= \"" + "[" + splsym + "] DESC" + "\" >Item Descending</OPTION>");
                                }
                                else
                                {
                                    strBuildr.Append("<OPTION SELECTED=\"SELECTED\" VALUE= \"" + "[" + CatAttrName + "] ASC" + "\" >Item Ascending</OPTION>");
                                    strBuildr.Append("<OPTION VALUE= \"" + "[" + CatAttrName + "] DESC" + "\" >Item Descending</OPTION>");
                                }
                            }
                            if (IsattriName.Equals(PriceAttrName))
                            {
                                strBuildr.Append("<OPTION VALUE= \" " + "[" + PriceAttrName + "] ASC" + "\" >Price Low to High</OPTION>");
                                strBuildr.Append("<OPTION VALUE= \"" + "[" + PriceAttrName + "] DESC" + " \" >Price High to Low</OPTION>");
                            }
                        //}
                    }
                    strBuildr.Append("</SELECT></TD>");
                    strBuildr.Append("<TD>&nbsp<INPUT TYPE=\"button\" id =\"btnsub\"  value=\"go\" onclick=\"javascript:GetName(SelSort.value," + FamilyID + ");\" style=\"font-family:Arial Unicode MS:Arial;font-size:12px; font-weight:bold\"/></TD>");
                    strBuildr.Append("</TR></TABLE>");
                    Attrcontent = strBuildr.ToString();
                    oDSAttr.Dispose();
                }
                
                if (ProductView == "FLAT")
                {
                    int ChkCnt = 1;
                    WOHeaderContent = WOHeaderContent.Trim();
                    FirstStr = "<TABLE ID=\"tblTemplate\" cellspacing=\"0\" cellpadding=\"0\"><TR></TR><TR><TD>";
                    //FirstStr = "<TABLE ID=\"tblTemplate\" cellspacing=\"0\" cellpadding=\"0\"><TR><TD>";
                    int StartTR = WOHeaderContent.ToUpper().IndexOf("<TR", 0);
                    if (StartTR == 0)
                    {
                        WOHeaderContent = WOHeaderContent.Insert(0, "<TABLE>");
                    }
                    else
                    {
                        StartTR = WOHeaderContent.ToUpper().IndexOf("<TBODY", 0);
                        if (StartTR == 0)
                        {
                            WOHeaderContent = WOHeaderContent.Insert(0, "<TABLE>");
                        }
                    }

                    //Get the zoom attribute and image attribute
                    string TempWOHeaderContentRep = WOHeaderContent;
                    int ProdImgStartLoc = TempWOHeaderContentRep.IndexOf("{START_PRODUCT_IMAGE_ZOOM}");
                    if (ProdImgStartLoc != -1)
                    {
                        ProdImgStartLoc = ProdImgStartLoc + 26;
                    }
                    int ProdImgEndLoc = TempWOHeaderContentRep.IndexOf("{END_PRODUCT_IMAGE_ZOOM}");
                    int StartLoc = TempWOHeaderContentRep.IndexOf("{START_ZOOM_PRODUCT_ATTRIBUTE}");
                    if (StartLoc != -1)
                    {
                        StartLoc = StartLoc + 30;
                    }
                    int EndLoc = TempWOHeaderContentRep.IndexOf("{END_ZOOM_PRODUCT_ATTRIBUTE}");
                    string ProdImgAttr = "";
                    string ZoomImgAttr = "";
                    if (ProdImgStartLoc != -1 && ProdImgEndLoc != -1)
                    {
                        ProdImgAttr = TempWOHeaderContentRep.Substring(ProdImgStartLoc, (ProdImgEndLoc - ProdImgStartLoc));
                        if (ProdImgAttr.IndexOf("{") != -1)
                        {
                            ProdImgAttr = ProdImgAttr.Substring(ProdImgAttr.IndexOf("{") + 1, (ProdImgAttr.IndexOf("}") - 1) - (ProdImgAttr.IndexOf("{")));
                        }
                    }
                    if (StartLoc != -1 && EndLoc != -1)
                    {
                        ZoomImgAttr = TempWOHeaderContentRep.Substring(StartLoc, (EndLoc - StartLoc));
                        if (ZoomImgAttr.IndexOf("{") != -1)
                        {
                            ZoomImgAttr = ZoomImgAttr.Substring(ZoomImgAttr.IndexOf("{") + 1, (ZoomImgAttr.IndexOf("}") - 1) - (ZoomImgAttr.IndexOf("{")));
                        }
                    }
                    
                    foreach (DataRow Dr in oPRODDataDS.Tables[0].Rows)
                    {
                        sSQL = "SELECT PRODUCT_ID,PRODUCT_STATUS  FROM TBWC_INVENTORY WHERE PRODUCT_ID = " + oHelper.CI(Dr["PRODUCT_ID"].ToString());
                        oHelper.SQLString = sSQL;
                        int product_id = oHelper.CI(oHelper.GetValue("PRODUCT_ID"));
                        string Product_status = oHelper.GetValue("PRODUCT_STATUS");
                        if (product_id != -1 && product_id != 0 && Product_status.ToUpper() == "AVAILABLE")
                        {
                            WOHeaderContentRep = WOHeaderContent;
                            foreach (DataColumn DC in oPRODDataDS.Tables[0].Columns)
                            {
                                DataRow[] DRAtr = CXAttributes.Tables[0].Select("ATTRIBUTE_TYPE=3");
                                foreach (DataRow DrAtr in DRAtr)
                                {
                                    if (DC.ColumnName.ToLower() == DrAtr[1].ToString().ToLower())
                                    {
                                        if (WOHeaderContentRep.Contains("{" + DC.ColumnName.ToString() + "}") == true)
                                        {
                                            string ImgSource = Dr[DC.ColumnName].ToString();
                                            if (ImgSource.ToString() != null && ImgSource.ToString() != "")
                                            {
                                                ImgSource = oHelper.GetOptionValues("PRODUCT IMAGE PATH") + ImgSource;

                                                //its my content//

                                                ProductImageAttr = ProdImgAttr;
                                                ProductZoomImageAttr = ZoomImgAttr;
                                                if (DC.ColumnName.ToString() != ZoomImgAttr)
                                                {
                                                    string Img = "";
                                                    if (ProdImgStartLoc == -1 && ProdImgEndLoc == -1)
                                                    {
                                                        Img = BuildProductImageContent(ImgSource, "", false);
                                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", Img);
                                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{START_PRODUCT_IMAGE_ZOOM}", "");
                                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{END_PRODUCT_IMAGE_ZOOM}", "");
                                                    }
                                                }

                                                if (ZoomImgAttr != string.Empty && DC.ColumnName != ZoomImgAttr)
                                                {
                                                    WOHeaderContentRep = BuildProductZoomImage(oPRODDataDS, DRAtr, oHelper.CI(Dr["PRODUCT_ID"].ToString()), ProdImgAttr, ZoomImgAttr, ImgSource, WOHeaderContentRep);
                                                    StartLoc = WOHeaderContentRep.IndexOf("{START_ZOOM_PRODUCT_ATTRIBUTE}");
                                                    EndLoc = WOHeaderContentRep.IndexOf("{END_ZOOM_PRODUCT_ATTRIBUTE}") + 28;
                                                    WOHeaderContentRep = WOHeaderContentRep.Remove(StartLoc, EndLoc - StartLoc);
                                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{START_PRODUCT_IMAGE_ZOOM}", "");
                                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{END_PRODUCT_IMAGE_ZOOM}", "");
                                                }
                                                else if (ProdImgStartLoc != -1 && ZoomImgAttr == string.Empty)
                                                {
                                                    //string ImageZoom = ZoomImage(ImgSource);
                                                    string ImageZoom = BuildProductImageContent(ImgSource, ImgSource, true);
                                                    ImageZoom = ImageZoom.Replace("\\", "/");
                                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{" + ProdImgAttr + "}", ImageZoom);
                                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{START_PRODUCT_IMAGE_ZOOM}", "");
                                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{END_PRODUCT_IMAGE_ZOOM}", "");
                                                }
                                                if (ProdImgAttr == ZoomImgAttr && DC.ColumnName == ProdImgAttr)
                                                {
                                                    string ImageZoom = BuildProductImageContent(ImgSource, ImgSource, true);
                                                    ImageZoom = ImageZoom.Replace("\\", "/");
                                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{" + ProdImgAttr + "}", ImageZoom);
                                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{START_PRODUCT_IMAGE_ZOOM}", "");
                                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{END_PRODUCT_IMAGE_ZOOM}", "");
                                                    StartLoc = WOHeaderContentRep.IndexOf("{START_ZOOM_PRODUCT_ATTRIBUTE}");
                                                    EndLoc = WOHeaderContentRep.IndexOf("{END_ZOOM_PRODUCT_ATTRIBUTE}") + 28;
                                                    WOHeaderContentRep = WOHeaderContentRep.Remove(StartLoc, EndLoc - StartLoc);
                                                }
                                                //its my content//

                                                //////string ImageZoom = ZoomImage(ImgSource);
                                                //////ImageZoom = ImageZoom.Replace("\\", "/");
                                                //////string Img = BuildProductImageContent(ImgSource);
                                                //////WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", Img);
                                                //////WOHeaderContentRep=WOHeaderContentRep.Replace("{ZOOM}", ImageZoom);
                                                //////BuildProductZoomImage(oPRODData, DRAtr, "", "");
                                            }
                                            else
                                            {
                                                if (this.NoProductImage == true && DC.ColumnName != ZoomImgAttr)
                                                {
                                                    string ImageUrl = "images/NoImage.gif";
                                                    ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ProductImageWidth + "\" Height=\"" + this.ProductImageHeight + "\" ALT=\"" + NoImageAvailableCaption + "\" style=\"border-width:0\"/>";
                                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", ImageUrl);
                                                }
                                                else
                                                {
                                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", "&nbsp");
                                                }

                                            }
                                        }
                                    }
                                }
                                int PopUpLinkStart = WOHeaderContentRep.IndexOf("{" + DC.ColumnName.ToString() + "}" + "[POPUPLINK]", 0);
                                if (PopUpLinkStart > -1)
                                {
                                    PopUpLinkStart = WOHeaderContentRep.IndexOf("[POPUPLINK]", 0);
                                    HypC = DC.ColumnName.ToString();
                                }
                                else
                                {
                                    HypC = "";
                                }
                                if (DC.ColumnName.ToLower() == HypC.ToLower())
                                {
                                    WOHeaderContentRep = WOHeaderContentRep.Remove(PopUpLinkStart, 11);
                                    string HypColumn = HypCURL.Replace("{PRODUCT_ID}", product_id.ToString());
                                    int Min_ord_qty = oHelper.CI(oOrder.GetProductMinimumOrderQty(product_id));
                                    HypColumn = HypColumn.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
                                    int Qty_avail = oHelper.CI(oOrder.GetProductAvilableQty(product_id));
                                    HypColumn = HypColumn.Replace("{QTY_AVAIL}", Qty_avail.ToString());
                                    HypColumn = HypColumn.Replace("{FAMILY_ID}", FamilyID.ToString());
                                    if (DC.ColumnName.ToLower() == PriceType.ToLower())
                                    {
                                        if (UserID > 0)
                                        {

                                            dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(oBG.GetBuyerGroup(UserID));
                                        }
                                        else
                                        {
                                            dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                                        }

                                        if (dsBgDisc != null)
                                        {
                                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                                            {
                                                decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                                DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                                untPrice = oHelper.CDEC(Dr[DC.ColumnName].ToString());
                                                bool IsBGCatProd = oBG.IsBGCatalogProduct(oHelper.CI(Dr["CATALOG_ID"].ToString()), oBG.GetBuyerGroup(UserID).ToString());
                                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                                {
                                                    untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                                }
                                            }
                                        }
                                        if (untPrice > 0)
                                        {
                                            CheckPrice = 1;
                                        }
                                        else
                                        {
                                            CheckPrice = 0;
                                        }
                                        HypColumn = "<A href=\"" + HypColumn + "\" style=\"text-decoration:none\">" + CurrencyFormat + oHelper.FixDecPlace(untPrice) + "</A>";
                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", HypColumn);
                                    }
                                    else
                                    {
                                        int loc = 0;
                                        string TempLinkContent = "";
                                        string TagName = "";
                                        if (WOHeaderContentRep.LastIndexOf("<TD", WOHeaderContentRep.LastIndexOf("{" + DC.ColumnName.ToString() + "}")) > -1)
                                        {
                                            loc = WOHeaderContentRep.LastIndexOf("<TD", WOHeaderContentRep.LastIndexOf("{" + DC.ColumnName.ToString() + "}"));
                                            TagName = "TD";
                                        }
                                        else if (WOHeaderContentRep.LastIndexOf("<P", WOHeaderContentRep.LastIndexOf("{" + DC.ColumnName.ToString() + "}")) > -1)
                                        {
                                            loc = WOHeaderContentRep.LastIndexOf("<P", WOHeaderContentRep.LastIndexOf("{" + DC.ColumnName.ToString() + "}"));
                                            TagName = "P";
                                        }
                                        else if (WOHeaderContentRep.LastIndexOf("<DIV", WOHeaderContentRep.LastIndexOf("{" + DC.ColumnName.ToString() + "}")) > -1)
                                        {
                                            loc = WOHeaderContentRep.LastIndexOf("<DIV", WOHeaderContentRep.LastIndexOf("{" + DC.ColumnName.ToString() + "}"));
                                            TagName = "DIV";
                                        }
                                        if ((loc > -1) && (loc != 0))
                                        {
                                            TempLinkContent = "<A href=\"" + HypColumn + "\" style=\"text-decoration:none\">";
                                            loc = WOHeaderContentRep.IndexOf(">", loc) + 1;
                                            WOHeaderContentRep = WOHeaderContentRep.Insert(loc, TempLinkContent);
                                            WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", Dr[DC.ColumnName].ToString());
                                            if (TagName == "TD")
                                            {
                                                WOHeaderContentRep = WOHeaderContentRep.Insert(WOHeaderContentRep.IndexOf("</TD>", loc), "</A>");
                                            }
                                            else if (TagName == "P")
                                            {
                                                WOHeaderContentRep = WOHeaderContentRep.Insert(WOHeaderContentRep.IndexOf("</P>", loc), "</A>");
                                            }
                                            else if (TagName == "DIV")
                                            {
                                                WOHeaderContentRep = WOHeaderContentRep.Insert(WOHeaderContentRep.IndexOf("</DIV>", loc), "</A>");
                                            }
                                        }
                                        else
                                        {

                                            HypColumn = "<A href=\"" + HypColumn + "\" style=\"text-decoration:none\">" + Dr[DC.ColumnName].ToString() + "</A>";
                                            WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", HypColumn);
                                        }
                                    }

                                }
                                else if (DC.ColumnName.ToLower() == PriceType.ToLower())
                                {

                                    if (oProdPro.CheckPromotion(product_id))
                                    {
                                        decimal DiscAmount = oHelper.CDEC(oProdPro.GetProductPromotionDiscValue(product_id));
                                        //decimal DiscPrice = 0;
                                        untPrice = oHelper.CDEC(oProd.GetProductBasePrice(product_id));
                                        DiscAmount = (untPrice * DiscAmount) / 100;
                                        untPrice = untPrice - DiscAmount;
                                    }
                                    else
                                    {
                                        if (UserID > 0)
                                        {
                                            dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(oBG.GetBuyerGroup(UserID));
                                        }
                                        else
                                        {
                                            dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                                        }
                                        if (dsBgDisc != null)
                                        {
                                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                                            {
                                                decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                                DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                                untPrice = oHelper.CDEC(Dr[DC.ColumnName].ToString());
                                                bool IsBGCatProd = oBG.IsBGCatalogProduct(oHelper.CI(Dr["CATALOG_ID"].ToString()), oBG.GetBuyerGroup(UserID).ToString());
                                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                                {
                                                    untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                                }
                                            }
                                        }
                                    }
                                    if (untPrice > 0)
                                    {
                                        CheckPrice = 1;
                                    }
                                    else
                                    {
                                        CheckPrice = 0;
                                    }
                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", CurrencyFormat + oHelper.FixDecPlace(untPrice));
                                }
                                else if (DC.ColumnName.ToLower() == "~restricted" && RestrictedOption.ToLower() == "yes")
                                {
                                    if (Dr["~RESTRICTED"].ToString().ToUpper() == "YES")
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", Dr[DC.ColumnName].ToString());
                                        string RestrictedProdText = oHelper.GetOptionValues("RESTRICTED PRODUCT TEXT").ToString();
                                        string TempRestrictedUrl = "<A href=\"" + RestrictedUrl + "\" style=\"text-decoration:none\">" + RestrictedProdText + "</A>";
                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{CART}", TempRestrictedUrl);
                                    }
                                    else
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", Dr[DC.ColumnName.ToString()].ToString());
                                    }
                                }
                                else
                                {
                                    if (ProductImageAttr != DC.ColumnName && ProductZoomImageAttr != DC.ColumnName)
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", Dr[DC.ColumnName].ToString());
                                        if (Dr[DC.ColumnName].ToString().Length == 0)
                                        {
                                            WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", "&nbsp;");
                                        }
                                    }
                                }
                            }
                           
                            CheckIndex = 0;
                            while (CheckIndex > -1)
                            {
                                CheckIndex = WOHeaderContentRep.IndexOf("{");
                                if (CheckIndex > -1)
                                {
                                    if (WOHeaderContentRep.Substring(CheckIndex + 1, 4) == "CART" || (WOHeaderContentRep.Substring(CheckIndex + 1, 8)) == "QUANTITY")
                                    {
                                        if (CheckPrice == 1)
                                        {
                                            string CartImgPath = "";
                                            string EComState = oHelper.GetOptionValues("ECOMMERCEENABLED").ToString();
                                            if (EComState.ToUpper() == "YES")
                                            {
                                                if (Restricted.ToUpper() == "YES")
                                                {
                                                    CartImgPath = oHelper.GetOptionValues("RESTRICTED PRODUCT TEXT");
                                                    string CartUrl = oHelper.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                                                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                                                }
                                                else
                                                {
                                                    if (WOHeaderContentRep.Contains("QUANTITY"))
                                                    {
                                                        int indx = WOHeaderContentRep.IndexOf("{QUANTITY}");
                                                        int CartIndx = WOHeaderContentRep.IndexOf("{CART}");
                                                        string Qty = "", Cart = "";
                                                        int MaxQtyAvl = oOrder.GetProductAvilableQty(product_id);
                                                        int MinQty = oOrder.GetProductMinimumOrderQty(product_id);
                                                        int OrderID = oOrder.GetOrderID(UserID);
                                                        int ExistProdQty = oOrder.GetOrderItemQty(product_id, OrderID);
                                                        
                                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{QUANTITY}", "");
                                                        Qty = "<input type=\"text\" runat=\"server\" id=\"txtqty" + bld_cnt + "\" name=\"txtqty\" size=2 onKeyPress=\"NoEnter();\">";
                                                        WOHeaderContentRep = WOHeaderContentRep.Insert(indx, Qty);
                                                        Cart = "<A HREF=\"\" style=\"text-decoration:none\"><img ID=\"cmdAddtoCart\" runat=\"server\" src=\"images/cart.gif\" style=\"height:5\" SkinID=\"btnNormalSkin\" value=\"Add to cart\" OnClick=\"javascript:ValidateQty(" + MaxQtyAvl + "," + MinQty + "," + product_id + "," + UserID + "," + bld_cnt + ");return false;\" style=\"border-style:none\"></img></A>";
                                                        CartIndx = WOHeaderContentRep.IndexOf("{CART}");
                                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{CART}", "");
                                                        WOHeaderContentRep = WOHeaderContentRep.Insert(CartIndx, Cart);
                                                    }
                                                    else
                                                    {
                                                        CartImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("CARTIMGPATH").ToString();
                                                        int Min_ord_qty = oOrder.GetProductMinimumOrderQty(product_id);
                                                        string CartUrl = oHelper.GetOptionValues("CARTURL").ToString();
                                                        CartUrl = CartUrl.Replace("{PRODUCT_ID}", product_id.ToString());
                                                        CartUrl = CartUrl.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
                                                        CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + CartImgPath + "\" style=\"border-width:0\"></A>";
                                                        WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, CartImgPath);
                                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{CART}", "");
                                                    }
                                                }
                                            }
                                            
                                        }
                                        else if (CheckPrice == 0)
                                        {
                                            WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, "&nbsp;");
                                        }
                                    }
                                    else if (WOHeaderContentRep.Substring(CheckIndex + 1, 8) == "WISHLIST")
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Remove(CheckIndex, WOHeaderContentRep.IndexOf("}") + 1 - CheckIndex);
                                        if (CheckPrice == 1)
                                        {
                                            string CartImgPath = "";
                                            string EComState = oHelper.GetOptionValues("ECOMMERCEENABLED").ToString();
                                            if (EComState.ToUpper() == "YES")
                                            {
                                                if (Restricted.ToUpper() == "YES")
                                                {
                                                    CartImgPath = oHelper.GetOptionValues("RESTRICTED PRODUCT TEXT");
                                                    string CartUrl = oHelper.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                                                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                                                }
                                                else
                                                {
                                                    CartImgPath = "images/add_to_list.gif";
                                                    string CartUrl = "WishListDetails.aspx?Pid=" + product_id + "&Qty=" + 1;
                                                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + CartImgPath + "\" style=\"border-width:0\"></A>";
                                                }
                                            }
                                            WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, CartImgPath);
                                        }
                                        else if (CheckPrice == 0)
                                        {
                                            WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, "&nbsp;");
                                        }
                                    }
                                    else if (WOHeaderContentRep.Substring(CheckIndex + 1, 8) == "SHIPPING")
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Remove(CheckIndex, WOHeaderContentRep.IndexOf("}") + 1 - CheckIndex);
                                        Boolean IsShipping = oOrder.GetProductIsShipping(product_id);
                                        string ShipImgPath = "";
                                        if (IsShipping == true)
                                        {
                                            ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("SHIPPING IMAGE").ToString();
                                            string ShipUrl = oHelper.GetOptionValues("SHIP URL").ToString();
                                            ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
                                        }
                                        else if (IsShipping == false)
                                        {
                                            ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("NO SHIPPING IMAGE").ToString();
                                            ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
                                        }
                                        WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, ShipImgPath);
                                    }
                                    /* Code Newly added for product comparision */
                                    else if (WOHeaderContentRep.Substring(CheckIndex + 1, 18) == "PRODUCT_COMPARISON")
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Remove(CheckIndex, WOHeaderContentRep.IndexOf("}") + 1 - CheckIndex);
                                        WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, "<input id=\"Chkbox_" + ChkCnt + "\" type=\"checkbox\" name=\"CompareItem\" value=\"" + product_id + "\" onclick=\"CheckCompareCount(document.forms[0].CompareItem,'Chkbox_" + ChkCnt + "');\" /><a href=\"#\"" + FamilyID + " onclick=\"GetCompareItems(document.forms[0].CompareItem);\">Compare</a>");
                                        ChkCnt++;
                                    }
                                    else if (WOHeaderContentRep.Substring(CheckIndex, WOHeaderContentRep.IndexOf("}") + 1 - CheckIndex) == "{FLAT_VIEW_SORTING}")
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{FLAT_VIEW_SORTING}", Attrcontent.ToString());
                                        Attrcontent = "";
                                    }
                                    else
                                    {
                                        string ImageUrl;
                                        string oImgAttrName = WOHeaderContentRep.Substring(CheckIndex + 1, WOHeaderContentRep.IndexOf("}") - 1 - CheckIndex);
                                        DataRow[] oDr = CXAttributes.Tables[0].Select("ATTRIBUTE_TYPE =3 AND ATTRIBUTE_NAME='" + oImgAttrName + "'");
                                        if (oDr.Length > 0)
                                        {
                                            //Newly Added
                                            if (oImgAttrName == ProductZoomImageAttr)
                                            {
                                                WOHeaderContentRep = WOHeaderContentRep.Replace(ProductZoomImageAttr, "");
                                            }
                                            //Newly Added
                                            if (this.NoProductImage == true)
                                            {
                                                ImageUrl = "images/NoImage.gif";
                                                ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ProductImageWidth + "\" Height=\"" + this.ProductImageHeight + "\" ALT=\"" + NoImageAvailableCaption + "\" style=\"border-width:0\"/>";
                                                WOHeaderContentRep = WOHeaderContentRep.Replace("{" + oImgAttrName + "}", ImageUrl);
                                            }
                                        }
                                        else
                                        {
                                            WOHeaderContentRep = WOHeaderContentRep.Remove(CheckIndex, WOHeaderContentRep.IndexOf("}") + 1 - CheckIndex);
                                            WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, " &nbsp;");
                                        }
                                    }
                                }
                            }
                            WOHeaderContentRep = WOHeaderContentRep.Replace("[POPUPLINK]", "");
                            //HtmlTextFinal = HtmlTextFinal + WOHeaderContentRep + "</TABLE>";
                            HtmlTextFinal = HtmlTextFinal + WOHeaderContentRep; //+ "</TABLE>";

                            if (count<Columns)
                            {
                                HtmlTextFinal = HtmlTextFinal + "</TD><TD>";
                                count = count + 1;
                            }
                            else
                            {
                                HtmlTextFinal = HtmlTextFinal + "</TD></TR><TR><TD>";
                                count = 1;
                            }
                        }
                        bld_cnt++;
                    }
                    if (count == 1)
                    {
                        HtmlTextFinal = HtmlTextFinal + "</TABLE>";
                        //newly Added
                        //The No. Below 16 means total no of Character for "<TR><TD></TABLE>"
                        //Alternate Code to find <TD><TR> "int Pos = HtmlTextFinal.LastIndexOf("<TR><TD>", HtmlTextFinal.LastIndexOf("</TABLE>"));"
                        int Pos = HtmlTextFinal.IndexOf("<TR><TD>",HtmlTextFinal.Length-16);
                        if (Pos != -1)
                        {
                            HtmlTextFinal = HtmlTextFinal.Remove(Pos, 8);
                        }
                        //newly Added
                    }
                    else
                    {
                        HtmlTextFinal = HtmlTextFinal + "</TD></TR></TABLE>";
                    }
                    if (HtmlTextFinal == "")
                    {
                        HtmlTextFinal = "";
                    }
                    FirstStr = FirstStr + Sortcont;
                    //HtmlTextFinal = HeaderContent + FirstStr + HtmlTextFinal;
                    HtmlTextFinal = HeaderContent + FirstStr + HtmlTextFinal;
                }
                else if (ProductView == "GRID")
                {
                    int ChkCnt = 1;
                    foreach (DataRow Dr in oPRODDataDS.Tables[0].Rows)
                    {
                        sSQL = "SELECT PRODUCT_ID,PRODUCT_STATUS  FROM TBWC_INVENTORY WHERE PRODUCT_ID = " + oHelper.CI(Dr["PRODUCT_ID"].ToString());
                        oHelper.SQLString = sSQL;
                        int product_id = oHelper.CI(oHelper.GetValue("PRODUCT_ID"));
                        string Product_status = oHelper.GetValue("PRODUCT_STATUS");
                        if (product_id != -1 && product_id != 0 && Product_status.ToUpper() == "AVAILABLE")
                        {
                            WOHeaderContentRep = WOHeaderContent;
                            foreach (DataColumn DC in oPRODDataDS.Tables[0].Columns)
                            {
                                DataRow[] DRAtr = CXAttributes.Tables[0].Select("ATTRIBUTE_TYPE=3");
                                foreach (DataRow DrAtr in DRAtr)
                                {
                                    if (DC.ColumnName.ToLower() == DrAtr[1].ToString().ToLower())
                                    {
                                        if (WOHeaderContentRep.Contains("{" + DC.ColumnName.ToString() + "}") == true)
                                        {
                                            string ImgSource = Dr[DC.ColumnName].ToString();
                                            if (ImgSource.ToString() != null && ImgSource.ToString() != "")
                                            {

                                                ImgSource = oHelper.GetOptionValues("PRODUCT IMAGE PATH") + ImgSource;
                                                string ImageZoom = ZoomImage(ImgSource);
                                                ImageZoom = ImageZoom.Replace("\\", "/");
                                                string Img = BuildProductImageContent(ImgSource,"",false);
                                                WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", Img);
                                                WOHeaderContentRep = WOHeaderContentRep.Replace("{ZOOM}", ImageZoom);
                                            }
                                            else
                                            {
                                                if (this.NoProductImage == true)
                                                {
                                                    string ImageUrl = "images/NoImage.gif";
                                                    ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ProductImageWidth + "\" Height=\"" + this.ProductImageHeight + "\" ALT=\"" + NoImageAvailableCaption + "\" style=\"border-width:0\"/>";
                                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", ImageUrl);
                                                }
                                                else
                                                {
                                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", "&nbsp");
                                                }
                                            }
                                        }
                                    }
                                }
                                int PopUpLinkStart = WOHeaderContentRep.IndexOf("{" + DC.ColumnName.ToString() + "}" + "[POPUPLINK]", 0);

                                if (PopUpLinkStart > -1)
                                {
                                    PopUpLinkStart = WOHeaderContentRep.IndexOf("[POPUPLINK]", 0);
                                    HypC = DC.ColumnName.ToString();
                                }
                                if (DC.ColumnName.ToLower() == HypC.ToLower())
                                {
                                    WOHeaderContentRep = WOHeaderContentRep.Remove(PopUpLinkStart, 11);
                                    string HypColumn = HypCURL.Replace("{PRODUCT_ID}", product_id.ToString());
                                    int Min_ord_qty = oHelper.CI(oOrder.GetProductMinimumOrderQty(product_id));
                                    HypColumn = HypColumn.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
                                    int Qty_avail = oHelper.CI(oOrder.GetProductAvilableQty(product_id));
                                    HypColumn = HypColumn.Replace("{QTY_AVAIL}", Qty_avail.ToString());
                                    HypColumn = HypColumn.Replace("{FAMILY_ID}", FamilyID.ToString());
                                    if (DC.ColumnName.ToLower() == PriceType.ToLower())
                                    {
                                        if (UserID > 0)
                                        {

                                            dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(oBG.GetBuyerGroup(UserID));
                                        }
                                        else
                                        {
                                            dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                                        }

                                        if (dsBgDisc != null)
                                        {
                                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                                            {
                                                decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                                DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                                untPrice = oHelper.CDEC(Dr[DC.ColumnName].ToString());
                                                bool IsBGCatProd = oBG.IsBGCatalogProduct(oHelper.CI(Dr["CATALOG_ID"].ToString()), oBG.GetBuyerGroup(UserID).ToString());
                                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                                {
                                                    untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                                }
                                            }
                                        }
                                        if (untPrice > 0)
                                        {
                                            CheckPrice = 1;
                                        }
                                        else
                                        {
                                            CheckPrice = 0;
                                        }
                                        HypColumn = "<A href=\"" + HypColumn + "\" style=\"text-decoration:none\">" + CurrencyFormat + oHelper.FixDecPlace(untPrice) + "</A>";
                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", HypColumn);
                                    }
                                    else
                                    {
                                        int loc = 0;
                                        string TempLinkContent = "";
                                        string TagName = "";
                                        if (WOHeaderContentRep.LastIndexOf("<TD", WOHeaderContentRep.LastIndexOf("{" + DC.ColumnName.ToString() + "}")) > -1)
                                        {
                                            loc = WOHeaderContentRep.LastIndexOf("<TD", WOHeaderContentRep.LastIndexOf("{" + DC.ColumnName.ToString() + "}"));
                                            TagName = "TD";
                                        }
                                        else if (WOHeaderContentRep.LastIndexOf("<P", WOHeaderContentRep.LastIndexOf("{" + DC.ColumnName.ToString() + "}")) > -1)
                                        {
                                            loc = WOHeaderContentRep.LastIndexOf("<P", WOHeaderContentRep.LastIndexOf("{" + DC.ColumnName.ToString() + "}"));
                                            TagName = "P";
                                        }
                                        else if (WOHeaderContentRep.LastIndexOf("<DIV", WOHeaderContentRep.LastIndexOf("{" + DC.ColumnName.ToString() + "}")) > -1)
                                        {
                                            loc = WOHeaderContentRep.LastIndexOf("<DIV", WOHeaderContentRep.LastIndexOf("{" + DC.ColumnName.ToString() + "}"));
                                            TagName = "DIV";
                                        }
                                        if ((loc > -1) && (loc != 0))
                                        {
                                            TempLinkContent = "<A href=\"" + HypColumn + "\" style=\"text-decoration:none\">";
                                            loc = WOHeaderContentRep.IndexOf(">", loc) + 1;
                                            WOHeaderContentRep = WOHeaderContentRep.Insert(loc, TempLinkContent);
                                            WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", Dr[DC.ColumnName].ToString());
                                            if (TagName == "TD")
                                            {
                                                WOHeaderContentRep = WOHeaderContentRep.Insert(WOHeaderContentRep.IndexOf("</TD>", loc), "</A>");
                                            }
                                            else if (TagName == "P")
                                            {
                                                WOHeaderContentRep = WOHeaderContentRep.Insert(WOHeaderContentRep.IndexOf("</P>", loc), "</A>");
                                            }
                                            else if (TagName == "DIV")
                                            {
                                                WOHeaderContentRep = WOHeaderContentRep.Insert(WOHeaderContentRep.IndexOf("</DIV>", loc), "</A>");
                                            }
                                        }
                                        else
                                        {
                                            HypColumn = "<A href=\"" + HypColumn + "\" style=\"text-decoration:none\">" + Dr[DC.ColumnName].ToString() + "</A>";
                                            WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", HypColumn);
                                        }
                                    }
                                }
                                else if (DC.ColumnName.ToLower() == PriceType.ToLower())
                                {
                                    if (oProdPro.CheckPromotion(product_id))
                                    {
                                        decimal DiscAmount = oHelper.CDEC(oProdPro.GetProductPromotionDiscValue(product_id));
                                        //decimal DiscPrice = 0;
                                        untPrice = oHelper.CDEC(oProd.GetProductBasePrice(product_id));
                                        DiscAmount = (untPrice * DiscAmount) / 100;
                                        untPrice = untPrice - DiscAmount;
                                    }
                                    else
                                    {
                                        if (UserID > 0)
                                        {

                                            dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails(oBG.GetBuyerGroup(UserID));
                                        }
                                        else
                                        {
                                            dsBgDisc = oBG.GetBuyerGroupBasedDiscountDetails("DEFAULTBG");
                                        }

                                        if (dsBgDisc != null)
                                        {
                                            if (dsBgDisc.Tables[0].Rows.Count > 0)
                                            {
                                                decimal DiscVal = oHelper.CDEC(dsBgDisc.Tables[0].Rows[0].ItemArray[0].ToString());
                                                DateTime ValidDt = Convert.ToDateTime(dsBgDisc.Tables[0].Rows[0].ItemArray[1].ToString());
                                                string CalMth = dsBgDisc.Tables[0].Rows[0].ItemArray[2].ToString();
                                                untPrice = oHelper.CDEC(Dr[DC.ColumnName].ToString());
                                                bool IsBGCatProd = oBG.IsBGCatalogProduct(oHelper.CI(Dr["CATALOG_ID"].ToString()), oBG.GetBuyerGroup(UserID));
                                                if (ValidDt.CompareTo(DateTime.Today) >= 0 && DiscVal > 0 && IsBGCatProd == true)
                                                {
                                                    untPrice = oBG.CalculateBGDiscountPrice(untPrice, DiscVal, CalMth);
                                                }
                                            }
                                        }
                                    }
                                    if (untPrice > 0)
                                    {
                                        CheckPrice = 1;
                                    }
                                    else
                                    {
                                        CheckPrice = 0;
                                    }
                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", CurrencyFormat + oHelper.FixDecPlace(untPrice));
                                }
                                else if (DC.ColumnName.ToLower() == "~restricted" && RestrictedOption.ToLower() == "yes")
                                {
                                    if (Dr["~RESTRICTED"].ToString().ToUpper() == "YES")
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", Dr[DC.ColumnName].ToString());
                                        string RestrictedProdText = oHelper.GetOptionValues("RESTRICTED PRODUCT TEXT").ToString();
                                        string TempRestrictedUrl = "<A href=\"" + RestrictedUrl + "\" style=\"text-decoration:none\">" + RestrictedProdText + "</A>";
                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{CART}", TempRestrictedUrl);
                                    }
                                    else
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", Dr[DC.ColumnName.ToString()].ToString());
                                    }
                                }
                                else
                                {
                                    WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", Dr[DC.ColumnName].ToString());
                                    if (Dr[DC.ColumnName].ToString().Length == 0)
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{" + DC.ColumnName.ToString() + "}", "&nbsp;");
                                    }
                                }
                                bld_cnt++;
                            }
                            CheckIndex = 0;
                            while (CheckIndex > -1)
                            {
                                CheckIndex = WOHeaderContentRep.IndexOf("{");
                                if (CheckIndex > -1)
                                {
                                    if (WOHeaderContentRep.Substring(CheckIndex + 1, 4) == "CART" || (WOHeaderContentRep.Substring(CheckIndex + 1, 8)) == "QUANTITY")
                                    {
                                        if (CheckPrice == 1)
                                        {
                                            string CartImgPath = "";
                                            string EComState = oHelper.GetOptionValues("ECOMMERCEENABLED").ToString();
                                            if (EComState.ToUpper() == "YES")
                                            {
                                                if (Restricted.ToUpper() == "YES")
                                                {
                                                    CartImgPath = oHelper.GetOptionValues("RESTRICTED PRODUCT TEXT");
                                                    string CartUrl = oHelper.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                                                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                                                }
                                                else
                                                {
                                                    if (WOHeaderContentRep.Contains("QUANTITY"))
                                                    {
                                                        int indx = WOHeaderContentRep.IndexOf("{QUANTITY}");
                                                        int CartIndx = WOHeaderContentRep.IndexOf("{CART}");
                                                        string Qty = "", Cart = "";
                                                        int MaxQtyAvl = oOrder.GetProductAvilableQty(product_id);
                                                        int MinQty = oOrder.GetProductMinimumOrderQty(product_id);
                                                        int OrderID = oOrder.GetOrderID(UserID);
                                                        int ExistProdQty = oOrder.GetOrderItemQty(product_id, OrderID);

                                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{QUANTITY}", "");
                                                        Qty = "<input type=\"text\" runat=\"server\" id=\"txtqty" + bld_cnt + "\" name=\"txtqty\" size=2 onKeyPress=\"NoEnter();\">";
                                                        WOHeaderContentRep = WOHeaderContentRep.Insert(indx, Qty);
                                                        Cart = "<img ID=\"cmdAddtoCart\" runat=\"server\" src=\"images/cart.gif\" style=\"height:5\" SkinID=\"btnNormalSkin\" value=\"Add to cart\" OnClick=\"javascript:ValidateQty(" + MaxQtyAvl + "," + MinQty + "," + product_id + "," + UserID + "," + bld_cnt + ");return false;\" ></img>";
                                                        CartIndx = WOHeaderContentRep.IndexOf("{CART}");
                                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{CART}", "");
                                                        WOHeaderContentRep = WOHeaderContentRep.Insert(CartIndx, Cart);
                                                    }
                                                    else
                                                    {
                                                        CartImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("CARTIMGPATH").ToString();
                                                        int Min_ord_qty = oOrder.GetProductMinimumOrderQty(product_id);
                                                        string CartUrl = oHelper.GetOptionValues("CARTURL").ToString();
                                                        CartUrl = CartUrl.Replace("{PRODUCT_ID}", product_id.ToString());
                                                        CartUrl = CartUrl.Replace("{MIN_ORD_QTY}", Min_ord_qty.ToString());
                                                        CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + CartImgPath + "\" style=\"border-width:0\"></A>";
                                                        WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, CartImgPath);
                                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{CART}", "");
                                                    }
                                                }
                                            }

                                        }
                                        else if (CheckPrice == 0)
                                        {
                                            WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, "&nbsp;");
                                        }
                                    }
                                    else if (WOHeaderContentRep.Substring(CheckIndex + 1, 8) == "WISHLIST")
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Remove(CheckIndex, WOHeaderContentRep.IndexOf("}") + 1 - CheckIndex);
                                        if (CheckPrice == 1)
                                        {
                                            string CartImgPath = "";
                                            string EComState = oHelper.GetOptionValues("ECOMMERCEENABLED").ToString();
                                            if (EComState.ToUpper() == "YES")
                                            {
                                                if (Restricted.ToUpper() == "YES")
                                                {
                                                    CartImgPath = oHelper.GetOptionValues("RESTRICTED PRODUCT TEXT");
                                                    string CartUrl = oHelper.GetOptionValues("RESTRICTED PRODUCT URL").ToString();
                                                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\">" + CartImgPath + " </A>";
                                                }
                                                else
                                                {
                                                    CartImgPath = "images/add_to_list.gif";
                                                    string CartUrl = "WishListDetails.aspx?Pid=" + product_id + "&Qty=" + 1;
                                                    CartImgPath = "<A HREF=\"" + CartUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + CartImgPath + "\" style=\"border-width:0\"></A>";
                                                }
                                            }
                                            WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, CartImgPath);
                                        }
                                        else if (CheckPrice == 0)
                                        {
                                            WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, "&nbsp;");
                                        }
                                    }
                                    else if (WOHeaderContentRep.Substring(CheckIndex + 1, 8) == "SHIPPING")
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Remove(CheckIndex, WOHeaderContentRep.IndexOf("}") + 1 - CheckIndex);
                                        Boolean IsShipping = oOrder.GetProductIsShipping(product_id);
                                        string ShipImgPath = "";
                                        if (IsShipping == true)
                                        {
                                            ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("SHIPPING IMAGE").ToString();
                                            string ShipUrl = oHelper.GetOptionValues("SHIP URL").ToString();
                                            ShipImgPath = "<A HREF=\"" + ShipUrl + "\" style=\"text-decoration:none\"><IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\"></A>";
                                        }
                                        else if (IsShipping == false)
                                        {
                                            ShipImgPath = oHelper.GetOptionValues("IMAGE PATH").ToString() + oHelper.GetOptionValues("NO SHIPPING IMAGE").ToString();
                                            ShipImgPath = "<IMG SRC=\"" + ShipImgPath + "\" style=\"border-width:0\">";
                                        }
                                        WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, ShipImgPath);
                                    }
                                    /* Code Newly added for product comparision */
                                    else if (WOHeaderContentRep.Substring(CheckIndex + 1, 18) == "PRODUCT_COMPARISON")
                                    {
                                        WOHeaderContentRep = WOHeaderContentRep.Remove(CheckIndex, WOHeaderContentRep.IndexOf("}") + 1 - CheckIndex);
                                        WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, "<input id=\"Chkbox_" + ChkCnt + "\" type=\"checkbox\" name=\"CompareItem\" value=\"" + product_id + "\" onclick=\"CheckCompareCount(document.forms[0].CompareItem,'Chkbox_" + ChkCnt + "');\" /><a href=\"#\"" + FamilyID + " onclick=\"GetCompareItems(document.forms[0].CompareItem);\">Compare</a>");
                                        ChkCnt++;
                                    }
                                    else
                                    {
                                        string ImageUrl;
                                        string oImgAttrName = WOHeaderContentRep.Substring(CheckIndex + 1, WOHeaderContentRep.IndexOf("}") - 1 - CheckIndex);
                                        DataRow[] oDr = CXAttributes.Tables[0].Select("ATTRIBUTE_TYPE =3 AND ATTRIBUTE_NAME='" + oImgAttrName + "'");
                                        if (oDr.Length > 0)
                                        {
                                            if (this.NoProductImage == true)
                                            {
                                                ImageUrl = "images/NoImage.gif";
                                                ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ProductImageWidth + "\" Height=\"" + this.ProductImageHeight + "\" ALT=\"" + NoImageAvailableCaption + "\" style=\"border-width:0\"/>";
                                                WOHeaderContentRep = WOHeaderContentRep.Replace("{" + oImgAttrName + "}", ImageUrl);
                                            }
                                        }
                                        else
                                        {
                                            WOHeaderContentRep = WOHeaderContentRep.Remove(CheckIndex, WOHeaderContentRep.IndexOf("}") + 1 - CheckIndex);
                                            WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, " &nbsp;");
                                        }
                                    }
                                }
                            }
                            WOHeaderContentRep = WOHeaderContentRep.Replace("[POPUPLINK]", "");
                            HtmlTextFinal = HtmlTextFinal + WOHeaderContentRep;
                        }
                    }
                    if (HtmlTextFinal == "")
                    {
                        HeaderContent = "";
                    }
                    HtmlTextFinal = HeaderContent + HtmlTextFinal;
                }
            }
            catch (Exception Ex)
            {
                oErr.ErrorMsg = Ex;
                oErr.CreateLog();
                return "";
            }
            return HtmlTextFinal;
        }
        public string BuildProductZoomImage(DataSet oDSProd, DataRow[] oDRAttr,int ProdID, string ProdImageAttribute, string ZoomImageAttribute, string MainImageSrc,string TemplateContent)
        {
            try
            {
                foreach (DataRow oDRProd in oDSProd.Tables[0].Select("PRODUCT_ID=" + ProdID))
                {
                    foreach (DataColumn oDC in oDSProd.Tables[0].Columns)
                    {
                        foreach (DataRow oDR in oDRAttr)
                        {
                            if (oDC.ColumnName == oDR[1].ToString())
                            {
                                //if ((FamilyTabContent.Contains("{ZOOM}")) && (FamilyTabContent.Contains("{START_ZOOM_FAMILY_ATTRIBUTE}")) && (FamilyTabContent.Contains("{END_ZOOM_FAMILY_ATTRIBUTE}")))
                                //{
                                if (oDC.ColumnName.ToString() == ZoomImageAttribute)
                                {
                                    string ImgSource = oDRProd[oDC.ColumnName].ToString();
                                    if (ImgSource.ToString() != null && ImgSource.ToString() != "")
                                    {
                                        ImgSource = oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + ImgSource;
                                        string ImageZoom = BuildProductImageContent(MainImageSrc, ImgSource, true);
                                        //string ImageZoom = ZoomImage(ImgSource);
                                        ImageZoom = ImageZoom.Replace("\\", "/");
                                        TemplateContent = TemplateContent.Replace("{" + ProdImageAttribute + "}", ImageZoom);
                                    }
                                }

                                //}
                            }
                        }
                    }
                }
                if (TemplateContent.Contains("{" + ProdImageAttribute + "}"))
                {
                    string ImageZoom = BuildProductImageContent(MainImageSrc, MainImageSrc, true);
                    //string ImageZoom = ZoomImage(MainImageSrc);
                    ImageZoom = ImageZoom.Replace("\\", "/");
                    TemplateContent = TemplateContent.Replace("{" + ProdImageAttribute + "}", ImageZoom);
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
            }
            return TemplateContent;
        }
        //This is used to Build the Html for Product Template Head Part
        /// <summary>
        /// This is used to Build the Html for Product Template Head Part
        /// </summary>
        /// <param name="ProductView">string</param>
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
        ///   string Temp;
        ///   string ProductView;
        ///   ...
        ///   Temp = BuildProductTemplateHead(ProductView);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string BuildProductTemplateHead(string ProductView)
        {
            string Tblbody = "";
            string TblHead = "";
            try
            {
                TblHead = "<head><script type=\"text/javascript\" src=\"TradingBellNavigation.js\"></script>"
                        //+ "<style type=\"text/css\">.pg-normal {color: black;font-weight: normal;text-decoration: none;cursor: pointer;}"
                        //+ ".pg-selected {color: black;font-weight: bold;text-decoration: underline;cursor: pointer;}</style>"
                        + "</head>";
                if (ProductView == "GRID")
                {
                    Tblbody = TblHead + "<body OnLoad='initTable(\"tblTemplate\");'>";
                }
                else if (ProductView == "FLAT")
                {
                    Tblbody = TblHead + "<body>";

                }
            }
            catch (Exception Ex)
            {
                oErr.ErrorMsg = Ex;
                oErr.CreateLog();
                return "";
            }
            return Tblbody;
        }

        //This is used to Build the Html for Product Template Paging
        /// <summary>
        /// This is used to Build the Html for Product Template Paging
        /// </summary>
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
        ///  string TempPaging;
        ///  ...
        ///  TempPaging = BuildProductTemplatePaging();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string BuildProductTemplatePaging(int Pagesize)
        {

            string Paging = "";
            string TblbodyEnd = "";
            try
            {
                Paging = "<div id=\"pageNavPosition\"></div><script type=\"text/javascript\">"
                      + "var pager = new Pager('tblTemplate'," + Pagesize + ");"
                      + "pager.init();pager.showPageNav('pager', 'pageNavPosition');"
                      + "pager.showPage(1);</script>";
                TblbodyEnd = Paging; /////+ "</body></Html>";
            }
            catch (Exception Ex)
            {
                oErr.ErrorMsg = Ex;
                oErr.CreateLog();
                return "";
            }
            return TblbodyEnd;
        }

        /// <summary>
        ///  This is used to built Product Image Content
        /// </summary>
        /// <param name="ImageUrl">string</param>
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
        ///   string Img;
        ///   string ImgSource;
        ///   ...
        ///   Img = BuildProductImageContent(ImgSource);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected string BuildProductImageContent(string ImageUrl,string ZoomImageUrl,bool IsProdImgZoom)
        {
            System.Drawing.Size newVal;
            string ImageContent = "";
            try
            {
                ImageUrl = ImageUrl.Replace("\\", "/");
                if ((File.Exists(Server.MapPath(ImageUrl)) == true) && ImageUrl != null)
                {
                    System.Drawing.Image oImg = System.Drawing.Image.FromFile(Server.MapPath(ImageUrl));
                    newVal = ScaleImage(oImg.Height, oImg.Width,this.ProductImageWidth, this.ProductImageHeight);
                    if (IsProdImgZoom == true)
                    {
                        ImageUrl = "<a href=\"javascript:Zoom('" + ZoomImageUrl + "')\"><IMG SRC=\"" + ImageUrl + "\" WIDTH=\"" + newVal.Width + "\" HEIGHT=\"" + newVal.Height + "\" BORDER=\"0\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/></a>";
                    }
                    else if(IsProdImgZoom==false)
                    {
                        ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" WIDTH=\"" + newVal.Width + "\" HEIGHT=\"" + newVal.Height + "\" BORDER=\"0\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/>";
                    }
                }
                else if (ImageUrl != null)
                {
                    if (IsProdImgZoom == true)
                    {
                        ImageUrl = "<a href=\"javascript:Zoom('" + ZoomImageUrl + "')\"><IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ProductImageWidth + "\" Height=\"" + this.ProductImageHeight + "\" BORDER=\"0\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/>";
                    }
                    else if (IsProdImgZoom == false)
                    {
                        ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ProductImageWidth + "\" Height=\"" + this.ProductImageHeight + "\" BORDER=\"0\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/>";
                    }
                }
                ImageContent = ImageUrl;
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ProductImageWidth + "\" Height=\"" + this.ProductImageHeight + "\" BORDER=\"0\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/>";
                ImageContent = ImageUrl;
                return ImageContent;
            }
            return ImageContent;
        }

        public int GetProductCountByCategory(string CategoryID, int CatalogID)
        {
            int Prod_Count=0;
            try
            {
                string CategoryIDs = "";
                DataSet oDS = new DataSet();
                string sSQL = "SELECT * FROM [CATEGORY_FUNCTION](" + CatalogID + ",'" + CategoryID + "')";
                oHelper.SQLString = sSQL;
                oDS = oHelper.GetDataSet();
                if (oDS != null)
                {
                    foreach (DataRow oDR in oDS.Tables[0].Select())
                    {
                        CategoryIDs = CategoryIDs + "'" + oDR["CATEGORY_ID"].ToString() + "',";
                    }
                    CategoryIDs = CategoryIDs.Remove(CategoryIDs.LastIndexOf(","), 1);
                }
                else
                {
                    CategoryIDs = CategoryID;
                }
                sSQL = "SELECT COUNT(PRODUCT_ID) AS PROD_COUNT FROM TBWC_INVENTORY WHERE PRODUCT_ID IN("
                            + "SELECT PRODUCT_ID FROM TB_PROD_FAMILY WHERE FAMILY_ID IN("
                            + "SELECT DISTINCT FAMILY_ID FROM TB_CATALOG_FAMILY WHERE CATEGORY_ID IN("
                            + CategoryIDs + ") AND CATALOG_ID =" + oHelper.CI(CatalogID.ToString()) + "))";
                oHelper.SQLString = sSQL;
                Prod_Count =oHelper.CI(oHelper.GetValue("PROD_COUNT").ToString());
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
            }
            return Prod_Count;
        }


        //This is used to resize the Image
        /// <summary>
        /// This is used to resize the Image
        /// </summary>
        /// <param name="origHeight">double</param>
        /// <param name="origWidth">double</param>
        /// <param name="Width">double</param>
        /// <param name="Height">double</param>
        /// <returns>System.Drawing.Size</returns>
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
        ///   ...
        ///   System.Drawing.Image oImg = System.Drawing.Image.FromFile(Server.MapPath(ImageUrl));
        ///   newVal = ScaleImage(oImg.Height, oImg.Width, 180, 180);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected System.Drawing.Size ScaleImage(double origHeight, double origWidth, double Width, double Height)
        {
            System.Drawing.Size newSize = new System.Drawing.Size();
            double nWidth = Width;
            double nHeight = Height;
            double oWidth = origWidth;
            double oHeight = origHeight;
            //if (origHeight > 200 || origWidth > 200)
            if (origHeight > nHeight || origWidth > nWidth)
            {
                if (oWidth > oHeight)
                {
                    double Ratio = (double)((double)(oHeight) / (double)(oWidth));
                    double Final = (nWidth) * Ratio;
                    nHeight = (int)Final;
                }
                else
                {
                    double Ratio = (double)((double)(oWidth) / (double)(oHeight));
                    double Final = (nHeight) * Ratio;
                    nWidth = (int)Final;
                }
            }
            newSize.Height = (int)nHeight;
            newSize.Width = (int)nWidth;
            return newSize;
        }

        public string ZoomImage(string ImageUrl)
        {
            string ZoomImage = "Images/zoom1.gif";
            ZoomImage = "<a href=\"javascript:Zoom('" + ImageUrl + "')\"><IMG SRC=\"" + ZoomImage + "\" style=\"text-decoration:none;border-width:0px\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/></a>";
            return ZoomImage;
        }

        public string GetImageNameFromURL(string ImageURL)
        {
            string TempUrl = "";
            string ImgFileNameWOExt = "";
            string ImgFileNameWExt = "";
            try
            {
                TempUrl = ImageURL.Replace("\\", "/");
                if (TempUrl.IndexOf("/") > -1)
                {
                    ImgFileNameWExt = TempUrl.Substring(TempUrl.LastIndexOf("/") + 1);
                }
                if (ImgFileNameWExt.IndexOf(".") > -1)
                {
                    ImgFileNameWOExt = ImgFileNameWExt.Remove(ImgFileNameWExt.IndexOf("."));
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
            }
            return ImgFileNameWOExt;
        }

    }

}