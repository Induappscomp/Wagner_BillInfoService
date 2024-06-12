using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using TradingBell.Common;
using TradingBell5.CatalogX;
using TradingBell.WebServices;
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebCat.CommonServices
{
    /// <summary>
    /// Summary description for FamilyListRender
    /// </summary>
    
    public class FamilyListRenderServices
    {
        #region declarations
        HelperDB oHelper = new HelperDB();
        ConnectionDB oCon = new ConnectionDB();

        ProductFamily oFam = new ProductFamily();
        ErrorHandler oErr = new ErrorHandler();
        public string CategoryID = "";
        string FamilyListTmpContent = "";
        string HeaderContent = "";
        string FamilyRowContent = "";
        string TableContent = "";
        int[] FamAttrIDList;
        DataTable oDTCatFam = new DataTable();
        DataSet oAttrListDS = new DataSet();
        TemplateLayout oTmpLayout = new TemplateLayout();
        int Pagesize;
        int _CatalogID;
        int columnDisplay;
        int FamilyCount;
        bool Ispaging = false;
        Boolean _NoImage;
        double _ImageHeight;
        double _ImageWidth;
        string _ShowFamilyLevel = "ALL";
        string _NoImageAvailableCaption = "No Image to Display";
        #endregion
        #region "Declaring Property"

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


        public Boolean NoImage
        {
            get
            {
                return _NoImage;
            }
            set
            {
                _NoImage = value;
            }
        }

        public double ImageHeight
        {
            get
            {
                return _ImageHeight;
            }
            set
            {
                _ImageHeight = value;
            }
        }


        public double ImageWidth
        {
            get
            {
                return _ImageWidth;
            }
            set
            {
                _ImageWidth = value;
            }
        }

        public string ShowFamilyLevel
        {
            get
            {
                return _ShowFamilyLevel;
            }
            set
            {
                _ShowFamilyLevel = value;
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
        #region constructor
        public FamilyListRenderServices()
        {
            //CatalogID = oHelper.GetOptionValues("DEFAULT CATALOG").ToString();
            //int TemplateType = (int)TemplateLayout.TemplateType.FamilyList;
            //DataSet oDSLayout = new DataSet();
            //oDSLayout = oTmpLayout.GetTemplateSource(TemplateType);
            //DataRow[] oDRR = oDSLayout.Tables[0].Select("IS_DEFAULT_TEMPLATE=TRUE");
            //foreach (DataRow oDR in oDRR)
            //{

            //    FamilyListTmpContent = oDR["TEMPLATE_SOURCE"].ToString();
            //    Ispaging = Convert.ToBoolean(oDR["IS_PAGING"].ToString());
            //    columnDisplay = oHelper.CI(oDR["NO_OF_COLUMN_INPAGE"].ToString());
            //    Pagesize = oHelper.CI(oDR["NO_OF_ROWS_INPAGE"].ToString());
            //}
            //if (columnDisplay == 0)
            //{
            //    columnDisplay = 1;
            //}
            //CategoryID = HttpContext.Current.Request.QueryString["Cat"].ToString();
            //HttpContext.Current.Session["Category_ID"] = CategoryID;
        }
    #endregion
        #region Methods
        /// <summary>
        /// This is used to get List of Family Attributes
        /// </summary>
        /// <returns>int[]</returns>
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
        ///   int[] FamAttrIDList = new int[500];
        ///   ...
        ///   FamAttrIDList = GetFamilyAttrList();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected int[] GetFamilyAttrList()
        {

            string sSQL = "";
            int i = 0;
            try
            {
                sSQL = "SELECT TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME,ATTRIBUTE_TYPE FROM TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE"
                                + " PUBLISH2WEB =1"
                                + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID"
                                + " AND TCA.CATALOG_ID =" + _CatalogID
                                + " AND TA.ATTRIBUTE_TYPE IN(7,9)";

                //sSQL = "SELECT TB_ATTRIBUTE.ATTRIBUTE_ID, TB_ATTRIBUTE.ATTRIBUTE_TYPE, TB_ATTRIBUTE.ATTRIBUTE_NAME"
                //       + " FROM TB_ATTRIBUTE INNER JOIN TB_FAMILY_SPECS ON TB_ATTRIBUTE.ATTRIBUTE_ID = TB_FAMILY_SPECS.ATTRIBUTE_ID AND"
                //       + " TB_FAMILY_SPECS.FAMILY_ID=" + Family_id + " AND TB_ATTRIBUTE.PUBLISH2WEB=1"
                //       + " INNER JOIN TB_CATALOG_ATTRIBUTES ON TB_ATTRIBUTE.ATTRIBUTE_ID = TB_CATALOG_ATTRIBUTES.ATTRIBUTE_ID"
                //       + " AND TB_CATALOG_ATTRIBUTES.CATALOG_ID='" + Catalog_id + "'";

                oHelper.SQLString = sSQL;
                oAttrListDS = oHelper.GetDataSet();
                if ((oAttrListDS != null) && (oAttrListDS.Tables[0].Rows.Count > 0))
                {
                    FamAttrIDList = new int[oAttrListDS.Tables[0].Rows.Count];
                }
                if (oAttrListDS != null)
                {
                    foreach (DataRow oDR in oAttrListDS.Tables[0].Rows)
                    {
                        FamAttrIDList[i] = oHelper.CI(oDR["ATTRIBUTE_ID"].ToString());
                        i = i + 1;
                    }

                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
                return null;
            }
            return FamAttrIDList;
        }

        /// <summary>
        /// This is used to Get the Family Details
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
        ///   ...
        ///   DataSet oDS = new DataSet();
        ///   oDS = GetFamilyDetailsDS();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet GetFamilyDetailsDS()
        {
            
            int[] FamAttrIDList = new int[0];
            DataSet oFamDetDS = new DataSet();
            DataTable oDT = new DataTable();
            string Con = "";
            try
            {
                Con = oCon.ConnectionString.Substring(oCon.ConnectionString.IndexOf(";") + 1).ToString();
                TradingBell5.CatalogX.CSDBProvider.Connection oFamCon = new TradingBell5.CatalogX.CSDBProvider.Connection();
                oFamCon.ConnSettings(Con);
                FamAttrIDList = GetFamilyAttrList();
                TradingBell5.CatalogX.CatalogXfunction oProdTable = new CatalogXfunction();
                TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.TB_FAMILYTableAdapter oTAFamily = new TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.TB_FAMILYTableAdapter();
                TradingBell5.CatalogX.CSDBProvider.CSDS.TB_FAMILYDataTable oDTFamily = new TradingBell5.CatalogX.CSDBProvider.CSDS.TB_FAMILYDataTable();

                oTAFamily.Fill(oDTFamily);
                oTAFamily.Dispose();
                oFamDetDS = oProdTable.WebCategoryFamily(_CatalogID, CategoryID.ToString(), FamAttrIDList, Con);
                oDT = oFamDetDS.Tables[0].Clone();

                if (ShowFamilyLevel == "PARENT")
                {
                    foreach (DataRow oDRFamDet in oFamDetDS.Tables[0].Select())
                    {
                        foreach (DataRow oDRFam in oDTFamily.Select("PARENT_FAMILY_ID=0 AND FAMILY_ID=" + oHelper.CI(oDRFamDet["FAMILY_ID"].ToString())))
                        {
                            oDT.ImportRow(oDRFamDet);
                        }
                    }
                    oFamDetDS.Tables.Clear();
                    oFamDetDS.Tables.Add(oDT);
                }
                else if (ShowFamilyLevel == "CHILD")
                {
                    foreach (DataRow oDRFamDet in oFamDetDS.Tables[0].Select())
                    {
                        foreach (DataRow oDRFam in oDTFamily.Select("PARENT_FAMILY_ID=" + oHelper.CI(oDRFamDet["FAMILY_ID"].ToString())))
                        {
                            oDT.ImportRow(oDRFamDet);
                        }
                    }
                    oFamDetDS.Tables.Clear();
                    oFamDetDS.Tables.Add(oDT);
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
                return null;
            }
            finally
            {
                oFamDetDS.Dispose();
                oDT.Dispose();
            }
            return oFamDetDS;
        }

        /// <summary>
        /// This is used to get Family with product Count
        /// </summary>
        /// <param name="CategoryID">string</param>
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
        ///   string CategoryID;
        ///   ...  
        ///   DataSet DSProdCount = new DataSet();
        ///   DSProdCount = GetFamilylistWithProdCount(CategoryID, oHelper.CI(CatalogID));
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet GetFamilylistWithProdCount(string CategoryID, int CatalogID)
        {
            try
            {
                //DataSet ODs = new DataSet();
                string sSQL = " SELECT TF.FAMILY_ID,TF.FAMILY_NAME,COUNT(TPF.PRODUCT_ID)AS PRODUCT_COUNT FROM TB_FAMILY TF, TB_PROD_FAMILY TPF, TBWC_INVENTORY TWI,TB_CATALOG_FAMILY TCF ";
                sSQL = sSQL + " WHERE TWI.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE'AND TPF.FAMILY_ID = TF.FAMILY_ID AND TF.CATEGORY_ID = N'" + CategoryID + "' AND PRODUCT_STATUS = 'AVAILABLE' ";
                sSQL = sSQL + " AND TF.FAMILY_ID = TCF.FAMILY_ID AND TCF.CATALOG_ID =" + CatalogID + " GROUP BY TF.FAMILY_ID, TF.FAMILY_NAME ORDER BY TF.FAMILY_NAME";
                //sSQL = sSQL + " UNION";
                //sSQL = sSQL + " SELECT TF.FAMILY_ID,TF.FAMILY_NAME,'0'AS PRODUCT_COUNT";
                //sSQL = sSQL + " FROM TB_FAMILY TF,TB_CATALOG_FAMILY TCF WHERE TF.CATEGORY_ID ='" + CategoryID + "' AND TF.FAMILY_ID = TCF.FAMILY_ID AND TCF.CATALOG_ID =" + CatalogID; 
                //sSQL = sSQL + " AND PARENT_FAMILY_ID = 0 AND TF.FAMILY_ID NOT IN(SELECT DISTINCT(FAMILY_ID)FROM TB_PROD_FAMILY) ORDER BY FAMILY_NAME";

                oHelper.SQLString = sSQL;
                //ODs = oHelper.GetDataSet();
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                //oErrHand.CreateLog(); 
                return null;
            }
        }

        /// <summary>
        /// This is used to find whether it is GridView or FlatView to build its Layout Content
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
        ///    string View = FindView();
        ///    string TempContent = "";
        ///    try
        ///    {
        ///        if (View == "GRID_VIEW")
        ///        {
        ///         ....
        ///        }
        ///    }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected string FindView()
        {
            string View = "";
            try
            {
                if (FamilyListTmpContent.Contains("{START_FAMILYLIST_HEADER}") && FamilyListTmpContent.Contains("{END_FAMILYLIST_HEADER}"))
                {
                    View = "GRID_VIEW";
                }
                else
                {
                    View = "FLAT_VIEW";
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
                return "";
            }
            return View;
        }

        /// <summary>
        /// This is used Break the Template content for Family content andProduct content
        /// </summary>
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
        ///   DivideTemplateContent();
        ///   DataSet oDS = new DataSet();
        ///   ...
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected void DivideTemplateContent()
        {
            string View = FindView();
            try
            {
                if (View == "GRID_VIEW")
                {
                    HeaderContent = FamilyListTmpContent.Substring(FamilyListTmpContent.IndexOf("{START_FAMILYLIST_HEADER}") + 25, FamilyListTmpContent.IndexOf("{END_FAMILYLIST_HEADER}") - (FamilyListTmpContent.IndexOf("{START_FAMILYLIST_HEADER}") + 25));
                    TableContent = FamilyListTmpContent.Remove(FamilyListTmpContent.IndexOf("{START_FAMILYLIST_HEADER}") + 25, FamilyListTmpContent.IndexOf("{END_FAMILYLIST_HEADER}") - (FamilyListTmpContent.IndexOf("{START_FAMILYLIST_HEADER}") + 25));
                    TableContent = TableContent.Remove(TableContent.IndexOf("{START_FAMILYLIST_ROW}") + 22, TableContent.IndexOf("{END_FAMILYLIST_ROW}") - (TableContent.IndexOf("{START_FAMILYLIST_ROW}") + 22));

                }
                else
                {
                    TableContent = FamilyListTmpContent.Remove(FamilyListTmpContent.IndexOf("{START_FAMILYLIST_ROW}") + 22, FamilyListTmpContent.IndexOf("{END_FAMILYLIST_ROW}") - (FamilyListTmpContent.IndexOf("{START_FAMILYLIST_ROW}") + 22));
                }
                FamilyRowContent = FamilyListTmpContent.Substring(FamilyListTmpContent.IndexOf("{START_FAMILYLIST_ROW}") + 22, FamilyListTmpContent.IndexOf("{END_FAMILYLIST_ROW}") - (FamilyListTmpContent.IndexOf("{START_FAMILYLIST_ROW}") + 22));
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
            }
        }

        //This is used to Build Image Content
        /// <summary>
        /// This is used to Build Image Content
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
        ///  ...
        ///  foreach (DataRow oDR in oParentCatDS.Tables[0].Select("Category_Id= '" + CatID + "'"))
        ///  {
        ///    if (oDR["IMAGE_FILE"].ToString().Trim() != "")
        ///     {
        ///         //ImgContent = "<IMG SRC =\"" + oDR["IMAGE_FILE"].ToString().Trim() + "\" Width =\"50px\" Height=\"50px\"/>";
        ///         ImgContent = BuildImageContent(ProdImgPath + oDR["IMAGE_FILE"].ToString().Trim());
        ///     }
        ///  }
        ///  ...
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        private string BuildImageContent(string ImageUrl)
        {
            System.Drawing.Size newVal;
            string ImageContent = "";
            try
            {
                ImageUrl = ImageUrl.Replace("\\", "/");
                string ImageZoom = ZoomImage(ImageUrl);
                if ((File.Exists(Server.MapPath(ImageUrl)) == true) && ImageUrl != null)
                {
                    System.Drawing.Image oImg = System.Drawing.Image.FromFile(Server.MapPath(ImageUrl));
                    newVal = ScaleImage(oImg.Height, oImg.Width, this.ImageWidth, this.ImageHeight);
                    ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" WIDTH=\"" + newVal.Width + "\" HEIGHT=\"" + newVal.Height + "\" style=\"border-width:0px;\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/>";
                }
                else if (ImageUrl != null)
                {
                    ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ImageWidth + "\" Height=\"" + this.ImageHeight + "\" style=\"border-width:0px;\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/>";
                }
                ImageContent = ImageUrl;
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                if (this.NoImage == true)
                {
                    ImageUrl = "images/NoImage.gif";
                    ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ImageWidth + "\" Height=\"" + this.ImageHeight + "\" style=\"border-width:0px;\" ALT=\"" + NoImageAvailableCaption + "\"/>";
                }
                else
                {
                    ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ImageWidth + "\" Height=\"" + this.ImageHeight + "\" style=\"border-width:0px;\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/>";
                }
                ImageContent = ImageUrl;
                return ImageContent;
            }
            return ImageContent;
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
        ///   newVal = ScaleImage(oImg.Height, oImg.Width, 120, 120);
        /// }
        /// </code>
        /// </example>
        public System.Drawing.Size ScaleImage(double origHeight, double origWidth, double Width, double Height)
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

        /// <summary>
        /// This is used to build the HTML content for Family content and product content
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
        ///   ...
        ///   string HtmlFamilyContent = BuildHTML();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected string BuildHTML()
        {
            CSRender oCSRender=new CSRender();
            DivideTemplateContent();
            string LinkContent = "<A HREF=\"FamilyDisplay.aspx?Fid=";
            string TempLinkContent = "";
            DataSet oDS = new DataSet();
            DataSet DSProdCount = new DataSet();
            int count = 0;
            int StartLink;
            int EndLink;
            string HtmlTextFinal = "<table id=\"tblFamilyList\" cellpadding=\"0px\" cellspacing=\"0px\" border=\"0px\"><tr></tr><tr><td>";
            string oTempFamGridCode = "";
            try
            {
                oDS = GetFamilyDetailsDS();
                FamilyCount = oDS.Tables[0].Rows.Count;
                DSProdCount = GetFamilylistWithProdCount(CategoryID,_CatalogID);
                oTempFamGridCode = FamilyRowContent;
                foreach (DataRow oDR in oDS.Tables[0].Rows)
                {
                    DataRow[] DRFamilyCounts = DSProdCount.Tables[0].Select("FAMILY_ID=" + oHelper.CI(oDR["FAMILY_ID"].ToString()));
                    DataRow[] DRSubFamsProdCount = new DataRow[0];
                    if (ShowFamilyLevel == "PARENT" && DRFamilyCounts.Length <1)
                    {
                        DataSet oDSSubFams = new DataSet();
                        oDSSubFams = oCSRender.GetSubFamilies(oHelper.CI(oDR["FAMILY_ID"].ToString()));
                        foreach (DataRow oDRSubFam in oDSSubFams.Tables[0].Select())
                        {
                            DRSubFamsProdCount = DSProdCount.Tables[0].Select("FAMILY_ID=" + oHelper.CI(oDRSubFam["SUBFAMILY_ID"].ToString()));
                        }
                    }
                    if (DRFamilyCounts.Length > 0)
                    {
                        oTempFamGridCode = FamilyRowContent;
                        count = count + 1;
                        foreach (DataColumn oDC in oDS.Tables[0].Columns)
                        {
                            if (oTempFamGridCode.Contains("{" + oDC.ColumnName.ToString() + "}"))
                            {
                                DataRow[] DRAtr = oAttrListDS.Tables[0].Select("ATTRIBUTE_TYPE=9");
                                foreach (DataRow DrAtr in DRAtr)
                                {
                                    if (oDC.ColumnName.ToLower() == DrAtr[1].ToString().ToLower())
                                    {
                                        string ImgSource = oDR[oDC.ColumnName].ToString();
                                        if (ImgSource.ToString() != null && ImgSource.ToString() != "")
                                        {

                                            ImgSource = oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + ImgSource;
                                            string ImageZoom = ZoomImage(ImgSource);
                                            ImageZoom = ImageZoom.Replace("\\", "/");
                                            string Img = BuildImageContent(ImgSource);
                                            oTempFamGridCode = oTempFamGridCode.Replace("{" + oDC.ColumnName.ToString() + "}", Img);
                                            oTempFamGridCode = oTempFamGridCode.Replace("{ZOOM}", ImageZoom);
                                        }
                                        else
                                        {
                                            if (this.NoImage == true)
                                            {
                                                string ImageUrl = "images/NoImage.gif";
                                                ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ImageWidth + "\" Height=\"" + this.ImageHeight + "\" style=\"border-width:0px;\" ALT=\"" + NoImageAvailableCaption + "\"/>";
                                                oTempFamGridCode = oTempFamGridCode.Replace("{" + oDC.ColumnName.ToString() + "}", ImageUrl);
                                            }
                                            else
                                            {
                                                oTempFamGridCode = oTempFamGridCode.Replace("{" + oDC.ColumnName.ToString() + "}", "&nbsp");
                                            }
                                        }
                                    }
                                }
                            }
                            StartLink = oTempFamGridCode.IndexOf("[LINK_START]", 0);
                            if (StartLink > -1)
                            {
                                oTempFamGridCode = oTempFamGridCode.Remove(StartLink, 12);
                                oTempFamGridCode = oTempFamGridCode.Insert(StartLink, LinkContent + oDR["FAMILY_ID"].ToString() + "\" style=\"text-decoration:none\">");
                                EndLink = oTempFamGridCode.IndexOf("[LINK_END]", StartLink);
                                if (EndLink > -1)
                                {
                                    oTempFamGridCode = oTempFamGridCode.Remove(EndLink, 10);
                                    oTempFamGridCode = oTempFamGridCode.Insert(EndLink, "</A>");
                                }

                            }
                            else
                            {
                                if (oDC.ColumnName.ToString().ToUpper() == "FAMILY_NAME")
                                {
                                    int loc = 0;
                                    string TagName = "";
                                    if (oTempFamGridCode.LastIndexOf("<TD", oTempFamGridCode.LastIndexOf("{FAMILY_NAME}")) > -1)
                                    {
                                        loc = oTempFamGridCode.LastIndexOf("<TD", oTempFamGridCode.LastIndexOf("{FAMILY_NAME}"));
                                        TagName = "TD";
                                    }
                                    else if (oTempFamGridCode.LastIndexOf("<P", oTempFamGridCode.LastIndexOf("{FAMILY_NAME}")) > -1)
                                    {
                                        loc = oTempFamGridCode.LastIndexOf("<P", oTempFamGridCode.LastIndexOf("{FAMILY_NAME}"));
                                        TagName = "P";
                                    }
                                    else if (oTempFamGridCode.LastIndexOf("<DIV", oTempFamGridCode.LastIndexOf("{FAMILY_NAME}")) > -1)
                                    {
                                        loc = oTempFamGridCode.LastIndexOf("<DIV", oTempFamGridCode.LastIndexOf("{FAMILY_NAME}"));
                                        TagName = "DIV";
                                    }
                                    if ((loc > -1) && (loc != 0))
                                    {
                                        TempLinkContent = LinkContent + oDR["FAMILY_ID"].ToString() + "\" >";
                                        loc = oTempFamGridCode.IndexOf(">", loc) + 1;
                                        oTempFamGridCode = oTempFamGridCode.Insert(loc, TempLinkContent);
                                        oTempFamGridCode = oTempFamGridCode.Replace("{" + oDC.ColumnName.ToString() + "}", oDR[oDC.ColumnName.ToString()].ToString());
                                        if (TagName == "TD")
                                        {
                                            oTempFamGridCode = oTempFamGridCode.Insert(oTempFamGridCode.IndexOf("</TD>", loc), "</A>");
                                        }
                                        else if (TagName == "P")
                                        {
                                            oTempFamGridCode = oTempFamGridCode.Insert(oTempFamGridCode.IndexOf("</P>", loc), "</A>");
                                        }
                                        else if (TagName == "DIV")
                                        {
                                            oTempFamGridCode = oTempFamGridCode.Insert(oTempFamGridCode.IndexOf("</DIV>", loc), "</A>");
                                        }
                                    }
                                    else
                                    {
                                        oTempFamGridCode = oTempFamGridCode.Replace("{" + oDC.ColumnName.ToString() + "}", LinkContent + oDR["FAMILY_ID"].ToString() + "\" >" + oDR[oDC.ColumnName].ToString() + "</A>");
                                    }
                                }
                            }

                            oTempFamGridCode = oTempFamGridCode.Replace("{" + oDC.ColumnName.ToString() + "}", oDR[oDC.ColumnName.ToString()].ToString()) + System.Environment.NewLine;
                        }

                        if (DSProdCount != null)
                        {
                            if (oTempFamGridCode.Contains("{" + DSProdCount.Tables[0].Columns[2].ColumnName + "}"))
                            {
                                DataRow[] DRFamilyCount = DSProdCount.Tables[0].Select("FAMILY_ID=" + oHelper.CI(oDR["FAMILY_ID"].ToString()));
                                foreach (DataRow DRFamily in DRFamilyCount)
                                {
                                    oTempFamGridCode = oTempFamGridCode.Replace("{" + DSProdCount.Tables[0].Columns[2].ColumnName + "}", DRFamily[2].ToString());
                                }
                            }
                            DSProdCount.Dispose();
                        }
                        else
                        {
                            oTempFamGridCode.Replace("{PRODUCT_COUNT}", "0");
                        }
                        HtmlTextFinal = HtmlTextFinal + oTempFamGridCode;
                        oTempFamGridCode = "";
                        if (count == columnDisplay)
                        {

                            HtmlTextFinal = HtmlTextFinal + "</td></tr><tr><td>";
                            count = 0;
                        }
                        else
                        {
                            HtmlTextFinal = HtmlTextFinal + "</td><td>";
                        }
                    }
                    //If sub family have a product we will display the main family
                    else if(DRSubFamsProdCount.Length >0)
                    {
                        oTempFamGridCode = FamilyRowContent;
                        count = count + 1;
                        foreach (DataColumn oDC in oDS.Tables[0].Columns)
                        {
                            if (oTempFamGridCode.Contains("{" + oDC.ColumnName.ToString() + "}"))
                            {
                                DataRow[] DRAtr = oAttrListDS.Tables[0].Select("ATTRIBUTE_TYPE=9");
                                foreach (DataRow DrAtr in DRAtr)
                                {
                                    if (oDC.ColumnName.ToLower() == DrAtr[1].ToString().ToLower())
                                    {
                                        string ImgSource = oDR[oDC.ColumnName].ToString();
                                        if (ImgSource.ToString() != null && ImgSource.ToString() != "")
                                        {

                                            ImgSource = oHelper.GetOptionValues("PRODUCT IMAGE PATH").ToString() + ImgSource;
                                            string ImageZoom = ZoomImage(ImgSource);
                                            ImageZoom = ImageZoom.Replace("\\", "/");
                                            string Img = BuildImageContent(ImgSource);
                                            oTempFamGridCode = oTempFamGridCode.Replace("{" + oDC.ColumnName.ToString() + "}", Img);
                                            oTempFamGridCode = oTempFamGridCode.Replace("{ZOOM}", ImageZoom);
                                        }
                                        else
                                        {
                                            if (this.NoImage == true)
                                            {
                                                string ImageUrl = "images/NoImage.gif";
                                                ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ImageWidth + "\" Height=\"" + this.ImageHeight + "\" style=\"border-width:0px;\" ALT=\"" + NoImageAvailableCaption + "\"/>";
                                                oTempFamGridCode = oTempFamGridCode.Replace("{" + oDC.ColumnName.ToString() + "}", ImageUrl);
                                            }
                                            else
                                            {
                                                oTempFamGridCode = oTempFamGridCode.Replace("{" + oDC.ColumnName.ToString() + "}", "&nbsp");
                                            }
                                        }
                                    }
                                }
                            }
                            StartLink = oTempFamGridCode.IndexOf("[LINK_START]", 0);
                            if (StartLink > -1)
                            {
                                oTempFamGridCode = oTempFamGridCode.Remove(StartLink, 12);
                                oTempFamGridCode = oTempFamGridCode.Insert(StartLink, LinkContent + oDR["FAMILY_ID"].ToString() + "\" style=\"text-decoration:none\">");
                                EndLink = oTempFamGridCode.IndexOf("[LINK_END]", StartLink);
                                if (EndLink > -1)
                                {
                                    oTempFamGridCode = oTempFamGridCode.Remove(EndLink, 10);
                                    oTempFamGridCode = oTempFamGridCode.Insert(EndLink, "</A>");
                                }

                            }
                            else
                            {
                                if (oDC.ColumnName.ToString().ToUpper() == "FAMILY_NAME")
                                {
                                    int loc = 0;
                                    string TagName = "";
                                    if (oTempFamGridCode.LastIndexOf("<TD", oTempFamGridCode.LastIndexOf("{FAMILY_NAME}")) > -1)
                                    {
                                        loc = oTempFamGridCode.LastIndexOf("<TD", oTempFamGridCode.LastIndexOf("{FAMILY_NAME}"));
                                        TagName = "TD";
                                    }
                                    else if (oTempFamGridCode.LastIndexOf("<P", oTempFamGridCode.LastIndexOf("{FAMILY_NAME}")) > -1)
                                    {
                                        loc = oTempFamGridCode.LastIndexOf("<P", oTempFamGridCode.LastIndexOf("{FAMILY_NAME}"));
                                        TagName = "P";
                                    }
                                    else if (oTempFamGridCode.LastIndexOf("<DIV", oTempFamGridCode.LastIndexOf("{FAMILY_NAME}")) > -1)
                                    {
                                        loc = oTempFamGridCode.LastIndexOf("<DIV", oTempFamGridCode.LastIndexOf("{FAMILY_NAME}"));
                                        TagName = "DIV";
                                    }
                                    if ((loc > -1) && (loc != 0))
                                    {
                                        TempLinkContent = LinkContent + oDR["FAMILY_ID"].ToString() + "\" >";
                                        loc = oTempFamGridCode.IndexOf(">", loc) + 1;
                                        oTempFamGridCode = oTempFamGridCode.Insert(loc, TempLinkContent);
                                        oTempFamGridCode = oTempFamGridCode.Replace("{" + oDC.ColumnName.ToString() + "}", oDR[oDC.ColumnName.ToString()].ToString());
                                        if (TagName == "TD")
                                        {
                                            oTempFamGridCode = oTempFamGridCode.Insert(oTempFamGridCode.IndexOf("</TD>", loc), "</A>");
                                        }
                                        else if (TagName == "P")
                                        {
                                            oTempFamGridCode = oTempFamGridCode.Insert(oTempFamGridCode.IndexOf("</P>", loc), "</A>");
                                        }
                                        else if (TagName == "DIV")
                                        {
                                            oTempFamGridCode = oTempFamGridCode.Insert(oTempFamGridCode.IndexOf("</DIV>", loc), "</A>");
                                        }
                                    }
                                    else
                                    {
                                        oTempFamGridCode = oTempFamGridCode.Replace("{" + oDC.ColumnName.ToString() + "}", LinkContent + oDR["FAMILY_ID"].ToString() + "\" >" + oDR[oDC.ColumnName].ToString() + "</A>");
                                    }
                                }
                            }

                            oTempFamGridCode = oTempFamGridCode.Replace("{" + oDC.ColumnName.ToString() + "}", oDR[oDC.ColumnName.ToString()].ToString()) + System.Environment.NewLine;
                        }

                        if (DSProdCount != null)
                        {
                            if (oTempFamGridCode.Contains("{" + DSProdCount.Tables[0].Columns[2].ColumnName + "}"))
                            {
                                DataRow[] DRFamilyCount = DSProdCount.Tables[0].Select("FAMILY_ID=" + oHelper.CI(oDR["FAMILY_ID"].ToString()));
                                foreach (DataRow DRFamily in DRFamilyCount)
                                {
                                    oTempFamGridCode = oTempFamGridCode.Replace("{" + DSProdCount.Tables[0].Columns[2].ColumnName + "}", DRFamily[2].ToString());
                                }
                            }
                            DSProdCount.Dispose();
                        }
                        else
                        {
                            oTempFamGridCode.Replace("{PRODUCT_COUNT}", "0");
                        }
                        HtmlTextFinal = HtmlTextFinal + oTempFamGridCode;
                        oTempFamGridCode = "";
                        if (count == columnDisplay)
                        {

                            HtmlTextFinal = HtmlTextFinal + "</td></tr><tr><td>";
                            count = 0;
                        }
                        else
                        {
                            HtmlTextFinal = HtmlTextFinal + "</td><td>";
                        }
                    }
                }
                HtmlTextFinal = HtmlTextFinal + "</table>";
                if (HtmlTextFinal.IndexOf("<tr><td>", HtmlTextFinal.Length - 16)>-1)
                {
                    int StartLoc = HtmlTextFinal.IndexOf("<tr><td>", HtmlTextFinal.Length - 16);
                    HtmlTextFinal = HtmlTextFinal.Remove(StartLoc, 8);
                }
                else if (HtmlTextFinal.IndexOf("<td>", HtmlTextFinal.Length - 12)>-1)
                {
                    int StartLoc = HtmlTextFinal.IndexOf("<td>", HtmlTextFinal.Length - 12);
                    HtmlTextFinal = HtmlTextFinal.Remove(StartLoc, 4);
                }
                int CheckIndex = 0;
                while (CheckIndex > -1)
                {
                    string ImageUrl;
                    CheckIndex = HtmlTextFinal.IndexOf("{");
                    if (CheckIndex > -1)
                    {
                        string oImgAttrName = HtmlTextFinal.Substring(CheckIndex + 1, HtmlTextFinal.IndexOf("}") - 1 - CheckIndex);
                        DataRow[] oDr = oAttrListDS.Tables[0].Select("ATTRIBUTE_TYPE =9 AND ATTRIBUTE_NAME='" + oImgAttrName + "'");
                        if (oDr.Length > 0)
                        {
                            if (this.NoImage = true)
                            {
                                ImageUrl = "images/NoImage.gif";
                                ImageUrl = "<IMG SRC=\"" + ImageUrl + "\" Width =\"" + this.ImageWidth + "\" Height=\"" + this.ImageHeight + "\" style=\"border-width:0px;\" ALT=\"" + NoImageAvailableCaption + "\"/>";
                                HtmlTextFinal = HtmlTextFinal.Replace("{" + oImgAttrName + "}", ImageUrl);
                            }
                        }
                        else
                        {
                            HtmlTextFinal = HtmlTextFinal.Remove(CheckIndex, HtmlTextFinal.IndexOf("}") + 1 - CheckIndex);
                            HtmlTextFinal = HtmlTextFinal.Insert(CheckIndex, " &nbsp;");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
                return "";
            }
            return HtmlTextFinal;
        }

        [WebMethod]
        public string BuildFamilyListHTML()
        {
            string HtmlFamilyContent = BuildHTML();
            string FinalHtml = "";
            try
            {
                TableContent = TableContent.Insert(FamilyListTmpContent.IndexOf("{START_FAMILYLIST_HEADER}") + 25, HeaderContent);
                if (FamilyCount == 0)
                {
                    HtmlFamilyContent = HtmlFamilyContent + " <TABLE WIDTH=\"100%\" CELLSPACING=\"0\" CELLPADDING=\"0\"><TR><TD WIDTH=\"100%\" ALIGN=\"CENTER\"><Font Color=\"Red\" Size=\"2\" face=Verdana><Font face=Verdana Color=\"Red\" Size=\"2\"><BR/><BR/>We do not have any products in sale right now. Please check back later.<BR/> Thank You </Font></TD></TR></TABLE>";
                    TableContent = TableContent.Insert(FamilyListTmpContent.IndexOf("{START_FAMILYLIST_ROW}") + 22, HtmlFamilyContent);
                }
                else
                {
                    TableContent = TableContent.Insert(FamilyListTmpContent.IndexOf("{START_FAMILYLIST_ROW}") + 22, HtmlFamilyContent);
                }
                TableContent = TableContent.Replace("{START_FAMILYLIST_ROW}", "");
                TableContent = TableContent.Replace("{END_FAMILYLIST_ROW}", "");
                TableContent = TableContent.Replace("{START_FAMILYLIST_HEADER}", "");
                TableContent = TableContent.Replace("{END_FAMILYLIST_HEADER}", "");
                TableContent = BuildHierarchy(TableContent);
                TableContent = BuildCatalogName(TableContent);
                if (Ispaging == true)
                {
                    if (FamilyCount > (Pagesize * columnDisplay))
                    {
                        FinalHtml = BuildTemplateHead() + TableContent + BuildTemplatePaging();
                    }
                    else
                    {
                        FinalHtml = BuildTemplateHead() + TableContent;
                    }
                }
                else
                {
                    FinalHtml = BuildTemplateHead() + TableContent + "</body></Html>";
                }
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
                return "";
            }
            return FinalHtml;
        }

        /// <summary>
        /// This is used to Build the Template Header content
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
        ///   ...
        ///   if (Ispaging == true)
        ///   {
        ///       FinalHtml = BuildTemplateHead() + TableContent + BuildTemplatePaging();
        ///   }
        ///   else
        ///   {
        ///       FinalHtml = BuildTemplateHead() + TableContent + "</body></Html>";
        ///   }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected string BuildTemplateHead()
        {
            string Tblbody = "";
            string TblHead = "";
            try
            {
                TblHead = "<script type=\"text/javascript\" src=\"TradingBellNavigation.js\"></script>";
                    //+ "<style type=\"text/css\">.pg-normal {color: black;font-weight: normal;text-decoration: none;cursor: pointer;}"
                    //+ ".pg-selected {color: black;font-weight: bold;text-decoration: underline;cursor: pointer;}</style>"
                        //+ "</head>";
                //if (ProductView == "GRID")
                //{
                Tblbody = TblHead + "<body OnLoad='initTable(\"tblFamilyList\");'>";
                //}
                //else if (ProductView == "FLAT")
                //{
                Tblbody = TblHead + "<body>";

                //}
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
                return "";
            }
            return Tblbody;
        }

        /// <summary>
        /// This is used to Build the Template content if Paging is Enabled
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
        ///   ...
        ///   if (Ispaging == true)
        ///   {
        ///       FinalHtml = BuildTemplateHead() + TableContent + BuildTemplatePaging();
        ///   }
        ///   else
        ///   {
        ///       FinalHtml = BuildTemplateHead() + TableContent + "</body></Html>";
        ///   }
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected string BuildTemplatePaging()
        {

            string Paging = "";
            string TblbodyEnd = "";
            try
            {
                Paging = "<div id=\"pageNavPosition\"></div><script type=\"text/javascript\">"
                      + "var pager = new Pager('tblFamilyList'," + Pagesize + ");"
                      + "pager.init();pager.showPageNav('pager', 'pageNavPosition');"
                      + "pager.showPage(1);</script>";
                TblbodyEnd = Paging + "</body></Html>";
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                oErr.CreateLog();
                return "";
            }
            return TblbodyEnd;
        }
        public string ZoomImage(string ImageUrl)
        {
            string ZoomImage = "Images/zoom1.gif";
            ZoomImage = "<a href=\"javascript:Zoom('" + ImageUrl + "')\"><IMG SRC=\"" + ZoomImage + "\" style=\"text-decoration:none;border-width:0px\" ALT=\"" + GetImageNameFromURL(ImageUrl) + "\"/></a>";
            return ZoomImage;
        }
        public string BuildHierarchy(string TemplateContent)
        {
            ProductFamily oFam = new ProductFamily();
            string HierarchyContent = "";
            try
            {
                int loc = 0;
                string StartTag = "";
                string EndTag = "";
                if (TemplateContent.IndexOf("{CATEGORY_HIERARCHY}") > -1)
                {
                    if (TemplateContent.ToUpper().LastIndexOf("<FONT", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}")) > -1)
                    {
                        loc = TemplateContent.ToUpper().LastIndexOf("<FONT", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}"));
                        StartTag = TemplateContent.ToUpper().Substring(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        EndTag = "</FONT>";
                        //Remove the Old Font Tag in Template
                        TemplateContent = TemplateContent.Remove(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        loc = TemplateContent.ToUpper().IndexOf("</FONT>", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}"));
                        TemplateContent = TemplateContent.Remove(loc, 7);
                    }
                    else if (TemplateContent.ToUpper().LastIndexOf("<P", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}")) > -1)
                    {
                        loc = TemplateContent.ToUpper().LastIndexOf("<P", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}"));
                        StartTag = TemplateContent.ToUpper().Substring(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        EndTag = "</P>";
                        //Remove the Old Font Tag in Template
                        TemplateContent = TemplateContent.Remove(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        loc = TemplateContent.ToUpper().IndexOf("</P>", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}"));
                        TemplateContent = TemplateContent.Remove(loc, 4);
                    }
                    else if (TemplateContent.ToUpper().LastIndexOf("<SPAN", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}")) > -1)
                    {
                        loc = TemplateContent.ToUpper().LastIndexOf("<SPAN", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}"));
                        StartTag = TemplateContent.ToUpper().Substring(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        EndTag = "</SPAN>";
                        //Remove the Old Font Tag in Template
                        TemplateContent = TemplateContent.Remove(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        loc = TemplateContent.ToUpper().IndexOf("</SPAN>", TemplateContent.LastIndexOf("{CATEGORY_HIERARCHY}"));
                        TemplateContent = TemplateContent.Remove(loc, 7);
                    }
                }
                oFam.HirearchyStyleStartTag = StartTag;
                oFam.HirearchyStyleEndTag = EndTag;
                oFam.HierarchyCharacter = " > ";
                TemplateContent = TemplateContent.Replace("{CATEGORY_HIERARCHY}", oFam.GetParentCategory(CategoryID).ToString());
                HierarchyContent = TemplateContent;
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                TemplateContent = TemplateContent.Replace("{CATEGORY_HIERARCHY}", "");
                HierarchyContent = TemplateContent;
            }
            return HierarchyContent;
            #region "Comments for another one type to replace the font tag"
            //string CatHierarchy = oFam.GetParentCategory(CatID).ToString();
            //string StrStartStyle = "<FONT COLOR=\"YELLOW\">";
            //string StrEndStyle="</FONT>";
            //loc = CatHierarchy.IndexOf("<A", loc);
            //while (loc != -1)
            //{
            //    loc = CatHierarchy.IndexOf(">", loc) + 1;
            //    CatHierarchy = CatHierarchy.Insert(loc, StrStartStyle);
            //    loc = CatHierarchy.IndexOf("</A>", loc);
            //    CatHierarchy = CatHierarchy.Insert(loc, StrEndStyle);
            //    loc = CatHierarchy.IndexOf("<A", loc);
            //}
            //return CatHierarchy;
            #endregion
        }


        public string BuildCatalogName(string TemplateContent)
        {
            string NameContent = "";
            try
            {
                int loc = 0;
                string StartTag = "";
                string EndTag = "";
                if (TemplateContent.IndexOf("{CATALOG_NAME}") > -1)
                {
                    if (TemplateContent.ToUpper().LastIndexOf("<FONT", TemplateContent.LastIndexOf("{CATALOG_NAME}")) > -1)
                    {
                        loc = TemplateContent.ToUpper().LastIndexOf("<FONT", TemplateContent.LastIndexOf("{CATALOG_NAME}"));
                        StartTag = TemplateContent.ToUpper().Substring(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        EndTag = "</FONT>";
                        //Remove the Old Font Tag in Template
                        TemplateContent = TemplateContent.Remove(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        loc = TemplateContent.ToUpper().IndexOf("</FONT>", TemplateContent.LastIndexOf("{CATALOG_NAME}"));
                        TemplateContent = TemplateContent.Remove(loc, 7);
                    }
                    else if (TemplateContent.ToUpper().LastIndexOf("<P", TemplateContent.LastIndexOf("{CATALOG_NAME}")) > -1)
                    {
                        loc = TemplateContent.ToUpper().LastIndexOf("<P", TemplateContent.LastIndexOf("{CATALOG_NAME}"));
                        StartTag = TemplateContent.ToUpper().Substring(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        EndTag = "</P>";
                        //Remove the Old Para Tag in Template
                        TemplateContent = TemplateContent.Remove(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        loc = TemplateContent.ToUpper().IndexOf("</P>", TemplateContent.LastIndexOf("{CATALOG_NAME}"));
                        TemplateContent = TemplateContent.Remove(loc, 4);
                    }
                    else if (TemplateContent.ToUpper().LastIndexOf("<SPAN", TemplateContent.LastIndexOf("{CATALOG_NAME}")) > -1)
                    {
                        loc = TemplateContent.ToUpper().LastIndexOf("<SPAN", TemplateContent.LastIndexOf("{CATALOG_NAME}"));
                        StartTag = TemplateContent.ToUpper().Substring(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        EndTag = "</SPAN>";
                        //Remove the Old Span Tag in Template
                        TemplateContent = TemplateContent.Remove(loc, (TemplateContent.IndexOf(">", loc) + 1) - loc);
                        loc = TemplateContent.ToUpper().IndexOf("</SPAN>", TemplateContent.LastIndexOf("{CATALOG_NAME}"));
                        TemplateContent = TemplateContent.Remove(loc, 7);
                    }
                }
                oFam.CatalogStyleStartTag = StartTag;
                oFam.CatalogStyleEndTag = EndTag;
                TemplateContent = TemplateContent.Replace("{CATALOG_NAME}", oFam.GetCatalogName(oHelper.CS(Session["CATALOGID"])));
                NameContent = TemplateContent;
            }
            catch (Exception ex)
            {
                oErr.ErrorMsg = ex;
                TemplateContent = TemplateContent.Replace("{CATALOG_NAME}", "");
                NameContent = TemplateContent;
            }
            return NameContent;
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

    #endregion
    }
}
