using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data.SqlClient;
using System.Data;
using TradingBell5.CatalogX;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
/// <summary>
/// Summary description for CompareRender
/// </summary>
namespace TradingBell.WebCat.CommonServices
{
    /// <summary>
    /// Summary description for ProductRender
    /// </summary>
   
 
    public class CompareRenderServices 
    {
        TemplateLayout oTemplate = new TemplateLayout();
        DataSet oProdData = new DataSet();
        public CompareRenderServices()
        {
            
        }

         #region Declarations
        HelperDB oHelper = new HelperDB();
        ConnectionDB oCon = new ConnectionDB();
        ErrorHandler oErr = new ErrorHandler();
        Order oOrder = new Order();
        DataSet CXAttributes = new DataSet();
        CSRender oCSRender = new CSRender();
        ProductPromotion oProdPro = new ProductPromotion();
        Product oProd = new Product();

        string FamilyTabContent = "";
        string FamilyTblContent = "";
        string WOHeaderContent = "";
        string HeaderContent = "";
        string HtmlTop = "";
        string HtmlEnd = "";
        string ProdRegHtmlS = "";
        string ProdRegHtmlE = "";

        int columnDisplay;
        int Pagesize;
        int UserID;
        int FamilyStart;
        int FamilyEnd;
        int ProdHStart;
        int ProdHEnd = -1;
        int ProdRStart;
        int ProdREnd = -1;
        int ProdRegStart;
        int ProdRegEnd;
        int FamilyFirst;

        string PriceType = "";
        string CurrencyFormat = "";
        string Restricted = "NO";
        Boolean _NoProductImage;
        double _ProductImageHeight;
        double _ProductImageWidth;
        string _NoImageAvailableCaption = "No Image to Display";
        private int _CatalogID;
        string[] TDContent = new string[100];
        string[] TDContenttemp = new string[100];
        string[] _ProdIDs = new string[10];
        int ProductRow = 1;
        string RowContent = "";
        bool _ConvertVertical = false;
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
        public int CatalogID
        {
            get
            {
                return _CatalogID;
            }
            set
            {
                _CatalogID = value;

            }
        }
        public string[] ProdIDs
        {
            get
            {
                return _ProdIDs;
            }
            set
            {
                _ProdIDs = value;
            }
        }
        public bool ConvertVertical
        {
            get
            {
                return _ConvertVertical;
            }
            set
            {
                _ConvertVertical = value;
            }
        }

        #endregion

        
        public string GenerateTemplateHtml(int FamilyID, int userId, string HtmlContent)
        {
            string GeneratedHtml = "";
            UserID = userId;
            DivideHTMLContent(HtmlContent);
            oProdData = oCSRender.GetCXProducts(CatalogID, FamilyID, userId);
            GeneratedHtml = BuildProductTemplate(CatalogID, FamilyID, userId, oProdData, HeaderContent, WOHeaderContent);
            return GeneratedHtml;
        }
        
        
        protected void DivideHTMLContent(string HtmlContent)
        {
            try
            {
                FamilyTabContent = "";
                if (HtmlContent.IndexOf("{START", 0) > -1)
                {
                    HtmlTop = HtmlContent.Substring(0, HtmlContent.IndexOf("{START", 0));
                }
                FamilyStart = HtmlContent.IndexOf("{START_FAMILY_REGION}", 0);
                if (FamilyStart > -1)
                {
                    FamilyEnd = HtmlContent.IndexOf("{END_FAMILY_REGION}", FamilyStart);
                }
                ProdRegStart = HtmlContent.IndexOf("{START_PRODUCT_REGION}", 0);
                if (ProdRegStart > -1)
                {
                    ProdRegEnd = HtmlContent.IndexOf("{END_PRODUCT_REGION}", 0);
                }
                ProdRStart = HtmlContent.IndexOf("{START_PRODUCT_ROW}", 0);
                if (ProdRStart > -1)
                {
                    ProdREnd = HtmlContent.IndexOf("{END_PRODUCT_ROW}", ProdRStart);
                }
                ProdHStart = HtmlContent.IndexOf("{START_PRODUCT_HEADER}", 0);
                if (ProdHStart > -1)
                {
                    ProdHEnd = HtmlContent.IndexOf("{END_PRODUCT_HEADER}", ProdHStart);
                    HeaderContent = HtmlContent.Substring((ProdHStart + 22), (ProdHEnd - (ProdHStart + 22)));
                    ProdRegHtmlS = HtmlContent.Substring(ProdRegStart + 22, ProdHStart - (ProdRegStart + 22));
                }
                else
                {
                    HeaderContent = "";
                    if (ProdRStart > -1)
                    {
                        ProdRegHtmlS = HtmlContent.Substring(ProdRegStart + 22, ProdRStart - (ProdRegStart + 22));
                    }
                }

                if (ProdRegHtmlS.ToUpper().IndexOf("<TABLE", 0) > -1)
                {
                    ProdRegHtmlS = ProdRegHtmlS.ToUpper().Replace("<TABLE", "<TABLE ID=\"tblTemplate\" cellspacing=\"0\" cellpadding=\"0\"");
                }
                if (ProdREnd > -1)
                {
                    ProdRegHtmlE = HtmlContent.Substring(ProdREnd + 17, (ProdRegEnd - (ProdREnd + 17)));
                    WOHeaderContent = HtmlContent.Substring(ProdRStart + 19, (ProdREnd - (ProdRStart + 19)));
                }
                if (FamilyStart < ProdRStart)
                {
                    FamilyFirst = 1;
                    HtmlEnd = HtmlContent.Substring(ProdRegEnd + 20);
                }
                else
                {
                    FamilyFirst = 0;
                    HtmlEnd = HtmlContent.Substring(FamilyEnd + 20);
                }
                if (FamilyStart > -1)
                {
                    FamilyTblContent = HtmlContent.Substring(FamilyStart + 21, FamilyEnd - (FamilyStart + 21));
                }
                if (ProdRegHtmlS.Contains("{VERTICAL_TABLE}"))
                {
                    ProdRegHtmlS = ProdRegHtmlS.Replace("{VERTICAL_TABLE}", "");
                }
            }
            catch (Exception Ex)
            {
                oErr.ErrorMsg = Ex;
                oErr.CreateLog();
            }
        }

        
        public string BuildProductTemplate(int CatalogID,int FamilyID, int UserID, DataSet oPRODData, string HeaderContent, string WOHeaderContent)
        {
            string HtmlTextFinal = "";
            string WOHeaderContentRep = "";
            string ProductView = "GRID";
            string sSQL = "";
            int CheckIndex;
            int CheckPrice = 1;
            int bld_cnt = 1;
            BuyerGroup oBG = new BuyerGroup();
            decimal untPrice = 0;
            DataSet dsBgDisc = new DataSet();
            StringBuilder strBuildr = new StringBuilder();
            string AttrName = "";
            DataSet oPRODDataDS = new DataSet();
            try
            {
                //copy data from original DataSet to Temp DS
                oPRODDataDS = oPRODData.Clone();
                int cnt = 0;
                while (cnt < _ProdIDs.Length)
                {
                    foreach (DataRow oDR in oPRODData.Tables[0].Select("PRODUCT_ID=" + _ProdIDs[cnt]))
                    {
                        oPRODDataDS.Tables[0].ImportRow(oDR);
                        cnt++;
                    }
                }
                //foreach (DataRow oDR in oPRODData.Tables[0].Select())
                //{
                //    if ((cnt < _ProdIDs.Length) && (oDR["PRODUCT_ID"].ToString() == _ProdIDs[cnt].ToString()))
                //    {
                //        oPRODDataDS.Tables[0].ImportRow(oDR);
                //        cnt++;
                //    }
                //}
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
                
               
                if (ProductView == "GRID")
                {
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
                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{ADD_TO_CART}", TempRestrictedUrl);
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
                               
                            }
                            CheckIndex = 0;
                            while (CheckIndex > -1)
                            {
                                CheckIndex = WOHeaderContentRep.IndexOf("{");
                                if (CheckIndex > -1)
                                {
                                    if (WOHeaderContentRep.Substring(CheckIndex + 1, 11) == "ADD_TO_CART" || (WOHeaderContentRep.Substring(CheckIndex + 1, 8)) == "QUANTITY")
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
                                                        int CartIndx = WOHeaderContentRep.IndexOf("{ADD_TO_CART}");
                                                        string Qty = "", Cart = "";
                                                        int MaxQtyAvl = oOrder.GetProductAvilableQty(product_id);
                                                        int MinQty = oOrder.GetProductMinimumOrderQty(product_id);
                                                        int OrderID = oOrder.GetOrderID(UserID);
                                                        int ExistProdQty = oOrder.GetOrderItemQty(product_id, OrderID);

                                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{QUANTITY}", "");
                                                        Qty = "<input type=\"text\" runat=\"server\" id=\"txtqty" + bld_cnt + "\" name=\"txtqty\" size=2 onKeyPress=\"NoEnter();\">";
                                                        WOHeaderContentRep = WOHeaderContentRep.Insert(indx, Qty);
                                                        Cart = "<A HREF=\"\" style=\"text-decoration:none\"><img ID=\"cmdAddtoCart\" runat=\"server\" src=\"images/cart.gif\" style=\"height:15\" SkinID=\"btnNormalSkin\" value=\"Add to cart\" OnClick=\"javascript:ValidateQty(" + MaxQtyAvl + "," + MinQty + "," + product_id + "," + UserID + "," + bld_cnt + ");return false;\" style=\"border-style:none\"></img></A>";
                                                        CartIndx = WOHeaderContentRep.IndexOf("{ADD_TO_CART}");
                                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{ADD_TO_CART}", "");
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
                                                        WOHeaderContentRep = WOHeaderContentRep.Replace("{ADD_TO_CART}", "");
                                                    }
                                                }
                                            }

                                        }
                                        else if (CheckPrice == 0)
                                        {
                                            WOHeaderContentRep = WOHeaderContentRep.Insert(CheckIndex, "&nbsp;");
                                        }
                                    }
                                    else if (WOHeaderContentRep.Substring(CheckIndex + 1, 15) == "ADD_TO_WISHLIST")
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

                            if (_ConvertVertical == true)
                            {
                                RowContent = WOHeaderContentRep.ToLower();
                                int sl, el;
                                if (RowContent.Contains("<tr") && RowContent.Contains("<td"))
                                {
                                    sl = RowContent.IndexOf("<tr");
                                    el = RowContent.IndexOf("<td", sl);
                                    RowContent = RowContent.Substring(0, el);
                                    WOHeaderContentRep = WOHeaderContentRep.ToLower().Replace(RowContent, "");
                                    WOHeaderContentRep = WOHeaderContentRep.ToLower().Replace("</tr>", "");
                                    TDContent = Regex.Split(WOHeaderContentRep.ToLower(), "<td");
                                    for (int ct = 0; ct < TDContent.Length; ct++)
                                    {
                                        if (ProductRow == oPRODDataDS.Tables[0].Rows.Count && TDContent[ct] != string.Empty)
                                        {
                                            TDContenttemp[ct] = TDContenttemp[ct] + "<td" + TDContent[ct] + "</tr>";
                                        }
                                        else if (TDContent[ct] != string.Empty)
                                        {
                                            TDContenttemp[ct] = TDContenttemp[ct] + "<td" + TDContent[ct];
                                        }
                                    }
                                    ProductRow++;
                                }
                                //TDContenttemp = TDContent;
                            }
                            else
                            {
                                HtmlTextFinal = HtmlTextFinal + WOHeaderContentRep;
                            }
                        }
                        bld_cnt++;
                    }
                    if (HtmlTextFinal == string.Empty && _ConvertVertical == false)
                    {
                        HeaderContent = "";
                    }
                    if (_ConvertVertical == true)
                    {
                        RowContent = HeaderContent.ToLower();
                        int sloc, eloc;
                        if (RowContent.Contains("<tr") && RowContent.Contains("<td"))
                        {
                            sloc = RowContent.IndexOf("<tr");
                            eloc = RowContent.IndexOf("<td", sloc);
                            RowContent = RowContent.Substring(0, eloc);
                            HeaderContent = HeaderContent.ToLower().Replace(RowContent, "");
                            HeaderContent = HeaderContent.ToLower().Replace("</tr>", "");
                            TDContent = Regex.Split(HeaderContent.ToLower(), "<td");
                            for (int ct = 0; ct < TDContent.Length; ct++)
                            {
                                if (TDContenttemp[ct] != null && TDContent[ct] != null)
                                {
                                    TDContent[ct] = RowContent + "<td" + TDContent[ct] + TDContenttemp[ct];
                                    HtmlTextFinal = HtmlTextFinal + TDContent[ct];
                                }
                            }
                        }
                        HtmlTextFinal = ProdRegHtmlS + HtmlTextFinal + ProdRegHtmlE;
                    }
                    else
                    {
                        HtmlTextFinal = ProdRegHtmlS + HeaderContent + HtmlTextFinal + ProdRegHtmlE;
                    }
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
                                if (oDC.ColumnName.ToString() == ZoomImageAttribute)
                                {
                                    string ImgSource = oDRProd[oDC.ColumnName].ToString();
                                    if (ImgSource.ToString() != null && ImgSource.ToString() != "")
                                    {
                                        ImgSource = oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + ImgSource;
                                        string ImageZoom = BuildProductImageContent(MainImageSrc, ImgSource, true);
                                        ImageZoom = ImageZoom.Replace("\\", "/");
                                        TemplateContent = TemplateContent.Replace("{" + ProdImageAttribute + "}", ImageZoom);
                                    }
                                }
                            }
                        }
                    }
                }
                if (TemplateContent.Contains("{" + ProdImageAttribute + "}"))
                {
                    string ImageZoom = BuildProductImageContent(MainImageSrc, MainImageSrc, true);
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
        
        
        protected System.Drawing.Size ScaleImage(double origHeight, double origWidth, double Width, double Height)
        {
            System.Drawing.Size newSize = new System.Drawing.Size();
            double nWidth = Width;
            double nHeight = Height;
            double oWidth = origWidth;
            double oHeight = origHeight;
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

