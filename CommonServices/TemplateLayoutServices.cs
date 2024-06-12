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
    /// <summary>
    /// Summary description for TemplateLayout
    /// </summary>
    
    public class TemplateLayoutServices
    {
        public TemplateLayoutServices()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        #region "Declaration"
        HelperDB oHelper = new HelperDB();
        ErrorHandler OErr = new ErrorHandler();
        string _CategoryID;
        string _TemplateName;
        int _Template_Rec_Id;
        string _TemplateSource;
        string _TemplateFilePath;
        int _TemplateTypes;
        bool _DefaultTemplate;
        bool _CSTemplate;
        bool _IsPaging;
        int _COlPaging;
        int _RowPaging;
        string _SortImagePath;
        string _CatImagePath;
        bool _IsEntireCatalog;
        bool _IsCompare;
        string _Editor;

        public string CategoryID
        {
            get
            {
                return _CategoryID;
            }
            set
            {
                _CategoryID = value;
            }
        }
        public string Editor
        {
            get
            {
                return _Editor;
            }
            set
            {
                _Editor = value;
            }
        }
        public string TemplateName
        {
            get
            {
                return _TemplateName;
            }
            set
            {
                _TemplateName = value;
            }
        }
        public int Template_Rec_Id
        {
            get
            {
                return _Template_Rec_Id;
            }
            set
            {
                _Template_Rec_Id = value;
            }
        }
        public string TemplateSource
        {
            get
            {
                return _TemplateSource;
            }
            set
            {
                _TemplateSource = value;
            }
        }
        public string TemplateFilePath
        {
            get
            {
                return _TemplateFilePath;
            }
            set
            {
                _TemplateFilePath = value;
            }
        }
        public int TemplateTypes
        {
            get
            {
                return _TemplateTypes;
            }
            set
            {
                _TemplateTypes = value;
            }
        }
        public bool DefaultTemplate
        {
            get
            {
                return _DefaultTemplate;
            }
            set
            {
                _DefaultTemplate = value;
            }
        }
        public bool CSTemplate
        {
            get
            {
                return _CSTemplate;
            }
            set
            {
                _CSTemplate = value;
            }
        }
        public bool IsPaging
        {
            get
            {
                return _IsPaging;
            }
            set
            {
                _IsPaging = value;
            }
        }
        public int COlPaging
        {
            get
            {
                return _COlPaging;
            }
            set
            {
                _COlPaging = value;
            }
        }
        public int RowPaging
        {
            get
            {
                return _RowPaging;
            }
            set
            {
                _RowPaging = value;
            }
        }
        public string SortImagePath
        {
            get
            {
                return _SortImagePath;
            }
            set
            {
                _SortImagePath = value;
            }
        }
        public string CatImagePath
        {
            get
            {
                return _CatImagePath;
            }
            set
            {
                _CatImagePath = value;
            }
        }
        public bool IsEntireCatalog
        {
            get
            {
                return _IsEntireCatalog;
            }
            set
            {
                _IsEntireCatalog = value;
            }
        }
        public bool IsCompare
        {
            get
            {
                return _IsCompare;
            }
            set
            {
                _IsCompare = value;
            }
        }


        #endregion

        public enum TemplateType
        {
            /// <summary>
            /// Parent Category Layout
            /// </summary>
            ParentCategoryLayout = 1,
            /// <summary>
            /// Subcategory Layout
            /// </summary>
            SubCategoryLayout = 2,
            /// <summary>
            /// Family Layout
            /// </summary>
            FamilyLayout = 3,
            /// <summary>
            /// Product Layout
            /// </summary>
            ProductLayout = 4,
            /// <summary>
            /// Family Display Layout
            /// </summary>
            FamilyList = 5,
            /// <summary>
            /// Compare Template Layout
            /// </summary>
            CompareLayout = 6
        }

        /// <summary>
        /// This is used to get the Family List using a Category ID 
        /// </summary>
        /// <param name="CatId">int</param>
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
        ///   TemplateLayoutDataAccess TLData = new TemplateLayoutDataAccess();
        ///   DataSet DSfamilyName=new DataSet();
        ///   CatalogID = OHelper.CI(Helper.WebCatGlb["DEFAULT CATALOG"].ToString());
        ///   ...
        ///   DSfamilyName = TLData.GetFamilyList(CatalogID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet GetFamilyList(int CatId)
        {
            try
            {
                DataSet DSFamily = new DataSet();
                string sSql;
                sSql = "SELECT TF.FAMILY_ID,TF.FAMILY_NAME FROM TB_FAMILY TF,TB_CATALOG_FAMILY TCF WHERE TCF.FAMILY_ID=TF.FAMILY_ID AND TCF.CATALOG_ID=" + CatId;
                oHelper.SQLString = sSql;
                DSFamily = oHelper.GetDataSet("TB_FAMILY");
                return DSFamily;
            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                return null;
            }
        }

        /// <summary>
        /// This is used to get the Family Attributes using a Category ID
        /// </summary>
        /// <param name="CatId">int</param>
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
        ///   TemplateLayoutDataAccess TLData = new TemplateLayoutDataAccess();
        ///   DataSet DSfamilyName=new DataSet();
        ///   CatalogID = OHelper.CI(Helper.WebCatGlb["DEFAULT CATALOG"].ToString());
        ///   ...
        ///   DSfamilyName = TLData.GetFamilyAttributeName(CatalogID);
        /// } 
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet GetFamilyAttributeName(int CatId)
        {
            try
            {
                DataSet DSAttribute = new DataSet();
                string SSql;
                SSql = "SELECT DISTINCT TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME FROM TB_ATTRIBUTE TA,";
                SSql = SSql + "TB_CATALOG_ATTRIBUTES TCA WHERE ATTRIBUTE_TYPE IN(7,9) AND PUBLISH2WEB=1 ";
                SSql = SSql + "AND TA.ATTRIBUTE_ID=TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID=" + CatId;
                oHelper.SQLString = SSql;
                DSAttribute = oHelper.GetDataSet();
                return DSAttribute;
            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                return null;
            }
        }

        /// <summary>
        /// This is used to get the Product Attributes using a Category ID
        /// </summary>
        /// <param name="CatId">int</param>
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
        ///   TemplateLayoutDataAccess TLData = new TemplateLayoutDataAccess();
        ///   DataSet DSfamilyName=new DataSet();
        ///   CatalogID = OHelper.CI(Helper.WebCatGlb["DEFAULT CATALOG"].ToString());
        ///   ...
        ///   DSfamilyName = TLData.GetproductAttribute(CatalogID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet GetproductAttribute(int CatId)
        {
            try
            {
                DataSet DSPAttribute = new DataSet();
                string SSql;
                SSql = "SELECT DISTINCT TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME FROM TB_ATTRIBUTE TA,";
                SSql = SSql + "TB_CATALOG_ATTRIBUTES TCA WHERE ATTRIBUTE_TYPE IN(1,2,3,4) AND PUBLISH2WEB=1 ";
                SSql = SSql + "AND TA.ATTRIBUTE_ID=TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID=" + CatId;
                oHelper.SQLString = SSql;
                DSPAttribute = oHelper.GetDataSet();
                return DSPAttribute;
            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                return null;
            }
        }

        /// <summary>
        /// This is used to create the Template for a Layout
        /// </summary>
        /// <returns>Int32</returns>
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
        ///   TemplateLayoutDataAccess TLData = new TemplateLayoutDataAccess();
        ///   DataSet DSfamilyName=new DataSet();
        ///   CatalogID = OHelper.CI(Helper.WebCatGlb["DEFAULT CATALOG"].ToString());
        ///   ...
        ///   DSfamilyName = TLData.CreateTemplate();
        /// } 
        /// </code>
        /// </example>
        [WebMethod]
        public Int32 CreateTemplate()
        {
            string SQL = "";
            int retVal = 0;
            try
            {

                string SSql = "INSERT INTO TBWC_TEMPLATES(";
                SSql = SSql + "TEMPLATE_NAME,TEMPLATE_SOURCE,TEMPLATE_FILE_PATH,TEMPLATE_TYPE,IS_DEFAULT_TEMPLATE,";
                SSql = SSql + "IS_CS_TEMPLATE,NO_OF_ROWS_INPAGE,IS_PAGING,SORT_IMAGE_PATH,CATEGORY_IMAGE_PATH,IS_APPLICABLE_ENTIRE_CATALOG,NO_OF_COLUMN_INPAGE)";
                SSql = SSql + "VALUES('" + this.TemplateName + "','" + this.TemplateSource + "','" + this.TemplateFilePath + "'," + this.TemplateTypes + ",'" + this.DefaultTemplate + "','";
                SSql = SSql + this.CSTemplate + "'," + this.RowPaging + ",'" + this.IsPaging + "','" + this.SortImagePath + "','" + this.CatImagePath + "','" + this.IsEntireCatalog + "'," + this.COlPaging + ")";
                oHelper.SQLString = SSql;
                retVal = oHelper.ExecuteSQLQuery();

                if (DefaultTemplate == true && TemplateTypes == (int)TemplateType.FamilyLayout)
                {
                    SQL = "UPDATE TBWC_TEMPLATES SET IS_DEFAULT_TEMPLATE='False' WHERE TEMPLATE_TYPE =" + (int)TemplateType.FamilyLayout;
                    SQL = SQL + "AND TEMPLATE_NAME != '" + this.TemplateName + "'";


                }
                if (DefaultTemplate == true && TemplateTypes == (int)TemplateType.ParentCategoryLayout)
                {
                    SQL = "UPDATE TBWC_TEMPLATES SET IS_DEFAULT_TEMPLATE='False' WHERE TEMPLATE_TYPE =" + (int)TemplateType.ParentCategoryLayout;
                    SQL = SQL + "AND TEMPLATE_NAME != '" + this.TemplateName + "'";
                }
                if (DefaultTemplate == true && TemplateTypes == (int)TemplateType.SubCategoryLayout)
                {
                    SQL = "UPDATE TBWC_TEMPLATES SET IS_DEFAULT_TEMPLATE='False' WHERE TEMPLATE_TYPE =" + (int)TemplateType.SubCategoryLayout;
                    SQL = SQL + "AND TEMPLATE_NAME != '" + this.TemplateName + "'";
                }
                if (SQL != "")
                {
                    oHelper.SQLString = SQL;
                    retVal = oHelper.ExecuteSQLQuery();
                }
                if (IsEntireCatalog == true && TemplateTypes == (int)TemplateType.FamilyLayout)
                {
                    SQL = "UPDATE TBWC_TEMPLATES SET IS_APPLICABLE_ENTIRE_CATALOG='FALSE' WHERE TEMPLATE_TYPE =" + (int)TemplateType.FamilyLayout;
                    SQL = SQL + "AND TEMPLATE_NAME != '" + this.TemplateName + "'";
                }
                if (IsEntireCatalog == true && TemplateTypes == (int)TemplateType.ParentCategoryLayout)
                {
                    SQL = "UPDATE TBWC_TEMPLATE SET IS_APPLICABLE_ENTIRE_CATALOG='FALSE' WHERE TEMPLATE_TYPE =" + (int)TemplateType.ParentCategoryLayout;
                    SQL = SQL + "AND TEMPLATE_NAME != '" + this.TemplateName + "'";
                }
                if (IsEntireCatalog == true && TemplateTypes == (int)TemplateType.SubCategoryLayout)
                {
                    SQL = "UPDATE TBWC_TEMPLATES SET IS_APPLICABLE_ENTIRE_CATALOG='FALSE' WHERE TEMPLATE_TYPE =" + (int)TemplateType.SubCategoryLayout;
                    SQL = SQL + "AND TEMPLATE_NAME != '" + this.TemplateName + "'";
                }


                if (SQL != "")
                {
                    oHelper.SQLString = SQL;
                    retVal = oHelper.ExecuteSQLQuery();
                }

            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                retVal = -1;
            }
            return retVal;
        }

        /// <summary>
        /// This is used to set the TemplateLayout for a particular Category 
        /// </summary>
        /// <returns>Int32</returns>
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
        ///   TemplateLayoutDataAccess TLData =  new TemplateLayoutDataAccess();
        ///   int retVal;
        ///   ...
        ///   retVal = TLData.CreateCategoryLayout();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public Int32 CreateCategoryLayout()
        {
            try
            {
                string sSQL = "INSERT INTO TBWC_CATEGORY_FAMILY_LAYOUT (CATEGORY_ID,TEMPLATE_REC_ID)";
                sSQL = sSQL + " VALUES('" + this.CategoryID + "'," + this.Template_Rec_Id + ")";
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                return -1;
            }
        }

        /// <summary>
        /// This is used to Update the TemplateLayout for a particular Category
        /// </summary>
        /// <returns>Int32</returns>
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
        ///   TemplateLayoutDataAccess TLData =  new TemplateLayoutDataAccess();
        ///   int retVal;
        ///   ...
        ///   retVal = TLData.UpdateCategorgLayout();
        /// } 
        /// </code>
        /// </example>
        [WebMethod]
        public Int32 UpdateCategorgLayout()
        {
            try
            {
                string sSQL = " UPDATE TBWC_CATEGORY_FAMILY_LAYOUT SET TEMPLATE_REC_ID=" + this.Template_Rec_Id;
                sSQL = sSQL + " WHERE CATEGORY_ID=" + this.CategoryID;
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                return -1;
            }
        }
        /// <summary>
        /// This is used to Delete the Tempalte Layout for a particular Category
        /// </summary>
        /// <returns></returns>
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
        ///   TemplateLayoutDataAccess TLData =  new TemplateLayoutDataAccess();
        ///   int retVal;
        ///   ...
        ///   retVal = TLData.DeleteCategoryLayout();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public Int32 DeleteCategoryLayout()
        {
            try
            {
                string sSQL = "DELETE FROM TBWC_CATEGORY_FAMILY_LAYOUT";
                sSQL = sSQL + " WHERE CATEGORY_ID='" + this.CategoryID + "' AND TEMPLATE_REC_ID=" + this.Template_Rec_Id + "";
                oHelper.SQLString = sSQL;
                return oHelper.ExecuteSQLQuery();
            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                return -1;
            }
        }

        /// <summary>
        /// This is used to Update the changes made to a Template Layout
        /// </summary>
        /// <returns>Int32</returns>
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
        ///   TemplateLayoutDataAccess TLData =  new TemplateLayoutDataAccess();
        ///   int retVal;
        ///   ...
        ///   retVal = TLData.UpdateTemplate();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public Int32 UpdateTemplate()
        {
            int retval = 0;
            try
            {
                string SQL = "";


                string SSql = "UPDATE TBWC_TEMPLATES SET ";
                SSql = SSql + "TEMPLATE_NAME='" + this.TemplateName + "',";
                SSql = SSql + "TEMPLATE_SOURCE='" + this.TemplateSource + "',";
                SSql = SSql + "TEMPLATE_FILE_PATH='" + this.TemplateFilePath + "',";
                SSql = SSql + "IS_DEFAULT_TEMPLATE='" + this.DefaultTemplate + "',";
                SSql = SSql + "IS_CS_TEMPLATE='" + this.CSTemplate + "',";
                SSql = SSql + "NO_OF_ROWS_INPAGE=" + this.RowPaging + ",";
                SSql = SSql + "IS_PAGING='" + this.IsPaging + "',";
                SSql = SSql + "SORT_IMAGE_PATH='" + this.SortImagePath + "',";
                SSql = SSql + "CATEGORY_IMAGE_PATH='" + this.CatImagePath + "',";
                SSql = SSql + "IS_APPLICABLE_ENTIRE_CATALOG='" + this.IsEntireCatalog + "',";
                SSql = SSql + "NO_OF_COLUMN_INPAGE=" + this.COlPaging + ",";
                SSql = SSql + " WHERE TEMPLATE_NAME='" + this.TemplateName + "'";
                oHelper.SQLString = SSql;
                retval = oHelper.ExecuteSQLQuery();
                if (DefaultTemplate == true && TemplateTypes == (int)TemplateType.FamilyLayout)
                {
                    SQL = "UPDATE TBWC_TEMPLATES SET IS_DEFAULT_TEMPLATE='False' WHERE  TEMPLATE_TYPE=" + (int)TemplateType.FamilyLayout;
                    SQL = SQL + "AND TEMPLATE_NAME != '" + this.TemplateName + "'";
                    oHelper.SQLString = SQL;
                    retval = oHelper.ExecuteSQLQuery();
                }

                if (IsEntireCatalog == true && TemplateTypes == (int)TemplateType.FamilyLayout)
                {
                    SQL = "UPDATE TBWC_TEMPLATES SET IS_APPLICABLE_ENTIRE_CATALOG='FALSE' WHERE TEMPLATE_TYPE =" + (int)TemplateType.FamilyLayout;
                    SQL = SQL + "AND TEMPLATE_NAME != '" + this.TemplateName + "'";
                    oHelper.SQLString = SQL;
                    retval = oHelper.ExecuteSQLQuery();
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
            return retval;
        }

        /// <summary>
        /// This is used to get the Category Names under a Catalog ID.
        /// <para>The parameter Template ID is used to get the Category Names which are not Already Assigned.</para>
        /// </summary>
        /// <param name="CatalogID">int</param>
        /// <param name="TemplateID">int</param>
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
        ///   TemplateLayoutDataAccess TLData =  new TemplateLayoutDataAccess();
        ///   int CatID;
        ///   int TempID;
        ///   DataSet oDS = new DataSet();
        ///   ...
        ///   oDS = TLData.GetCategoryNames(CatID,TempID);
        /// } 
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet GetCategoryNames(int CatalogID, int TemplateID)
        {
            DataSet oCatDs = new DataSet();
            string sSQL = "";
            try
            {
                if (TemplateID != 0)
                {
                    sSQL = " SELECT TC.CATEGORY_ID,CATEGORY_NAME FROM TB_CATEGORY TC,TB_CATALOG_SECTIONS TCS";
                    sSQL = sSQL + " WHERE TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID = " + CatalogID;
                    sSQL = sSQL + " AND TC.CATEGORY_ID NOT IN(SELECT CATEGORY_ID FROM TBWC_CATEGORY_FAMILY_LAYOUT  WHERE TEMPLATE_REC_ID =" + TemplateID + ")";
                }
                else
                {
                    sSQL = " SELECT TC.CATEGORY_ID,CATEGORY_NAME FROM TB_CATEGORY TC,TB_CATALOG_SECTIONS TCS";
                    sSQL = sSQL + " WHERE TC.CATEGORY_ID = TCS.CATEGORY_ID AND TCS.CATALOG_ID = " + CatalogID;
                    //sSQL = sSQL + " AND TC.CATEGORY_ID NOT IN(SELECT CATEGORY_ID FROM TBWC_CATEGORY_FAMILY_LAYOUT  WHERE TEMPLATE_REC_ID =" + TemplateID + ")";
                }
                oHelper.SQLString = sSQL;
                oCatDs = oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                oCatDs = null;
            }
            return oCatDs;
        }

        /// <summary>
        /// This is used to get the Layout for a Category using Template ID 
        /// </summary>
        /// <param name="CatID">int</param>
        /// <param name="TempId">int</param>
        /// <returns>DataSet</returns>
        /// /// <example>
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
        ///   TemplateLayoutDataAccess TLData =  new TemplateLayoutDataAccess();
        ///   int CatalogID;
        ///   int TempId;
        ///   DataSet oDS;
        ///   ...
        ///   oDS = TLData.GetCategoryLayout(CatalogID,TempID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet GetCategoryLayout(int CatID, int TempId)
        {
            try
            {
                DataSet OCatLayout = new DataSet();
                string SSql = "SELECT TC.CATEGORY_NAME,TC.CATEGORY_ID FROM TB_CATEGORY TC,TB_CATALOG_SECTIONS TCS,TBWC_CATEGORY_FAMILY_LAYOUT TCF";
                SSql = SSql + " WHERE TCS.CATEGORY_ID=TC.CATEGORY_ID AND TC.CATEGORY_ID IN(TCF.CATEGORY_ID) AND TCF.TEMPLATE_REC_ID=" + TempId + " AND TCS.CATALOG_ID=" + CatID;
                oHelper.SQLString = SSql;
                OCatLayout = oHelper.GetDataSet();
                return OCatLayout;
            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                return null;
            }
        }

        /// <summary>
        /// This is used to Get the Category ID for a Category Name
        /// </summary>
        /// <param name="CategoryName">string</param>
        /// <returns>string</returns>
        /// /// <example>
        /// <code>
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
        ///   TemplateLayoutDataAccess TLData =  new TemplateLayoutDataAccess();
        ///   string CatName;
        ///   ...
        ///   CategoryName = TLData.GetCategoryName(CatName);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GetCategoryName(string CategoryName)
        {
            try
            {
                string CatId = "";
                string sSQL = "SELECT CATEGORY_ID FROM TB_CATEGORY WHERE CATEGORY_NAME='" + CategoryName + "'";
                oHelper.SQLString = sSQL;
                CatId = oHelper.CS(oHelper.GetValue("CATEGORY_ID").ToString());
                return CatId;
            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                return null;
            }
        }

        /// <summary>
        /// This is used to Delete the Template which is Assigned to a Category
        /// </summary>
        /// <returns>Int32</returns>
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
        ///   TemplateLayoutDataAccess oTmpLayout =  new TemplateLayoutDataAccess();
        ///   ...
        ///   TLData.CategoryID = lstAssignedCategory.SelectedItem.Value;
        ///   TLData.DeleteAssignedCategories();
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public Int32 DeleteAssignedCategories()
        {
            try
            {
                int retval;
                string SSQL = "DELETE FROM TBWC_CATEGORY_FAMILY_LAYOUT";
                oHelper.SQLString = SSQL;
                retval = oHelper.ExecuteSQLQuery();
                return retval;
            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                return -1;
            }

        }
        /// <summary>
        /// This is used to get Template ID based on Template Name
        /// </summary>
        /// <param name="TemplateName">string</param>
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
        ///   TemplateLayoutDataAccess oTmpLayout =  new TemplateLayoutDataAccess();
        ///   string TempName;
        ///   int TempID;
        ///   ...
        ///   TempID = oTmpLayout.GetTemplateID(TempName);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int GetTemplateID(string TemplateName)
        {
            string TempID;
            try
            {
                string sSQL = "SELECT TEMPLATE_REC_ID FROM TBWC_TEMPLATES";
                sSQL = sSQL + " WHERE TEMPLATE_NAME='" + TemplateName + "'";
                //sSQL = sSQL + " AND TEMPLATE_TYPE='FamilyLayoutEditor'";
                oHelper.SQLString = sSQL;
                TempID = oHelper.GetValue("TEMPLATE_REC_ID");
                if (TempID == "")
                {
                    return 0;
                }
                else
                {
                    return oHelper.CI(TempID);
                }
            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                return -1;
            }
        }

        /// <summary>
        /// This is used to get Template Name based on Template Type
        /// </summary>
        /// <param name="TempType">int</param>
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
        ///   TemplateLayoutDataAccess oTmpLayout =  new TemplateLayoutDataAccess();
        ///   int Temptype;
        ///   DataSet oDSLayout = new DataSet();
        ///   ...
        ///   oDSLayout = oTmpLayout.GetTemplateNames(Temptype);
        /// }
        /// </code>
        /// </example> 

        [WebMethod]
        public DataSet GetTemplateNames(int TempType)
        {
            DataSet retDS;
            string sql = "";
            try
            {

                if (TempType == (int)TemplateType.FamilyLayout)
                {
                    sql = "SELECT TEMPLATE_NAME FROM TBWC_TEMPLATES WHERE TEMPLATE_TYPE= " + (int)TemplateType.FamilyLayout;
                }
                else if (TempType == (int)TemplateType.SubCategoryLayout)
                {
                    sql = "SELECT TEMPLATE_NAME FROM TBWC_TEMPLATES WHERE TEMPLATE_TYPE= " + (int)TemplateType.SubCategoryLayout;

                }
                else if (TempType == (int)TemplateType.ParentCategoryLayout)
                {
                    sql = "SELECT TEMPLATE_NAME FROM TBWC_TEMPLATES WHERE TEMPLATE_TYPE= " + (int)TemplateType.ParentCategoryLayout;

                }
                else if (TempType == (int)TemplateType.ProductLayout)
                {
                    sql = "SELECT TEMPLATE_NAME FROM TBWC_TEMPLATES WHERE TEMPLATE_TYPE= " + (int)TemplateType.ProductLayout;

                }
                oHelper.SQLString = sql;
                retDS = oHelper.GetDataSet();
                return retDS;

            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                retDS = null;
                return retDS;
            }
        }

        /// <summary>
        /// This is used to Get the Template HTML Source based on Template name
        /// </summary>
        /// <param name="TempType">string</param>
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
        ///   TemplateLayoutDataAccess oTmpLayout =  new TemplateLayoutDataAccess();  
        ///   string TempName;
        ///   DataSet oDSLayout = new DataSet();
        ///   ...
        ///   oDSLayout = oTmpLayout.GetTemplateSource(TempName);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet GetTemplateSource(string TempName)
        {
            try
            {

                DataSet ODS = new DataSet();
                string SSQL = "SELECT * FROM TBWC_TEMPLATES WHERE TEMPLATE_NAME=";
                SSQL = SSQL + "'" + TempName + "'";
                oHelper.SQLString = SSQL;
                ODS = oHelper.GetDataSet("TBWC_TEMPLATES");
                if (ODS.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow oDR in ODS.Tables["TBWC_TEMPLATES"].Rows)
                    {
                        this.TemplateSource = (string)oDR["TEMPLATE_SOURCE"].ToString();
                        this.IsEntireCatalog = (bool)oDR["IS_APPLICABLE_ENTIRE_CATALOG"];
                        this.IsPaging = (bool)oDR["IS_PAGING"];
                        this.RowPaging = (int)oDR["NO_OF_ROWS_INPAGE"];
                        this.COlPaging = (int)oDR["NO_OF_COLUMN_INPAGE"];
                        this.CSTemplate = (bool)oDR["IS_CS_TEMPLATE"];
                        this.DefaultTemplate = (bool)oDR["IS_DEFAULT_TEMPLATE"];
                        this.TemplateName = (string)oDR["TEMPLATE_NAME"].ToString();
                        this.TemplateTypes = (int)oDR["TEMPLATE_TYPE"];
                        this.Template_Rec_Id = (int)oDR["TEMPLATE_REC_ID"];
                    }
                }
                else
                {
                    ODS = null;
                }
                return ODS;

            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                return null;
            }

        }

        /// <summary>
        /// This is used to Get the Template HTML Source based on Template Type
        /// </summary>
        /// <param name="TempType">int</param>
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
        ///   TemplateLayoutDataAccess oTmpLayout =  new TemplateLayoutDataAccess();
        ///   int TemplateType = (int)TemplateLayout.TemplateType.FamilyList;
        ///   DataSet oDSLayout = new DataSet();
        ///   ...
        ///   oDSLayout = oTmpLayout.GetTemplateSource(TemplateType);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet GetTemplateSource(int TempType)
        {
            try
            {

                DataSet ODS = new DataSet();
                string SSQL = "SELECT * FROM TBWC_TEMPLATES WHERE TEMPLATE_TYPE=";
                SSQL = SSQL + TempType;
                oHelper.SQLString = SSQL;
                ODS = oHelper.GetDataSet("TBWC_TEMPLATES");
                if (ODS.Tables[0].Rows.Count != 0)
                {
                    foreach (DataRow oDR in ODS.Tables["TBWC_TEMPLATES"].Rows)
                    {
                        this.TemplateSource = (string)oDR["TEMPLATE_SOURCE"].ToString();
                        this.IsEntireCatalog = (bool)oDR["IS_APPLICABLE_ENTIRE_CATALOG"];
                        this.IsPaging = (bool)oDR["IS_PAGING"];
                        this.RowPaging = (int)oDR["NO_OF_ROWS_INPAGE"];
                        this.COlPaging = (int)oDR["NO_OF_COLUMN_INPAGE"];
                        this.CSTemplate = (bool)oDR["IS_CS_TEMPLATE"];
                        this.DefaultTemplate = (bool)oDR["IS_DEFAULT_TEMPLATE"];
                        this.TemplateName = (string)oDR["TEMPLATE_NAME"].ToString();
                        this.TemplateTypes = (int)oDR["TEMPLATE_TYPE"];
                        this.Template_Rec_Id = (int)oDR["TEMPLATE_REC_ID"];
                    }
                }
                else
                {
                    ODS = null;
                }
                return ODS;

            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                return null;
            }

        }
        /// <summary>
        /// This is used to get the Template HTML content from Database
        /// </summary>
        /// <param name="TempType">int</param>
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
        ///   TemplateLayoutDataAccess oTmpLayout =  new TemplateLayoutDataAccess();
        ///   int TemplateType = (int)TemplateLayout.TemplateType.FamilyList;
        ///   string TemplateSource;
        ///   ...
        ///   TemplateSource = oTmpLayout.GetTemplateHtmlSource(TemplateType);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public string GetTemplateHtmlSource(int TempType)
        {
            try
            {

                string TemplateSource = "";
                string SSQL = "SELECT TEMPLATE_SOURCE FROM TBWC_TEMPLATES WHERE TEMPLATE_TYPE=" + TempType + " AND IS_DEFAULT_TEMPLATE=1";
                oHelper.SQLString = SSQL;
                TemplateSource = oHelper.GetValue("TEMPLATE_SOURCE");
                if (TemplateSource != "" & TemplateSource != null)
                {
                    return TemplateSource;
                }
                else
                {
                    return "";
                }

            }
            catch (Exception ex)
            {
                OErr.CreateLog();
                OErr.ErrorMsg = ex;
                return "";
            }

        }

        /// <summary>
        /// This is used to get Template Source content
        /// </summary>
        /// <param name="Family_ID">int</param>
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
        ///   int FamilyID;
        ///   ...
        ///   DataSet csDs = new DataSet();
        ///   csDs = GetTemplateContent(FamilyID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        protected DataSet GetTemplateContent(int Family_ID)
        {
            string sSql = "";
            DataSet csDs;
            try
            {
                sSql = "SELECT TBWC_TEMPLATES.TEMPLATE_SOURCE, TBWC_TEMPLATES.TEMPLATE_FILE_PATH,TBWC_TEMPLATES.NO_OF_ROWS_INPAGE,TBWC_TEMPLATES.NO_OF_COLUMN_INPAGE,TBWC_TEMPLATES.IS_CS_TEMPLATE,TBWC_TEMPLATES.IS_PAGING"
                     + " FROM TBWC_CATEGORY_FAMILY_LAYOUT INNER JOIN"
                     + " TBWC_TEMPLATES ON TBWC_CATEGORY_FAMILY_LAYOUT.TEMPLATE_REC_ID = TBWC_TEMPLATES.TEMPLATE_REC_ID AND TBWC_TEMPLATES.TEMPLATE_TYPE=" + (int)TemplateType.FamilyLayout + " INNER JOIN"
                     + " TB_FAMILY ON TBWC_CATEGORY_FAMILY_LAYOUT.CATEGORY_ID = TB_FAMILY.CATEGORY_ID AND TB_FAMILY.FAMILY_ID=" + Family_ID;
                oHelper.SQLString = sSql;
                csDs = new DataSet();
                csDs = oHelper.GetDataSet();
                if (csDs == null)
                {
                    sSql = "SELECT TEMPLATE_SOURCE,TEMPLATE_FILE_PATH,NO_OF_ROWS_INPAGE,NO_OF_COLUMN_INPAGE,TBWC_TEMPLATES.IS_CS_TEMPLATE,TBWC_TEMPLATES.IS_PAGING FROM TBWC_TEMPLATES WHERE TEMPLATE_TYPE=" + (int)TemplateType.FamilyLayout
                    + " AND IS_DEFAULT_TEMPLATE=1";
                    oHelper.SQLString = sSql;
                    csDs = new DataSet();
                    csDs = oHelper.GetDataSet();
                }
            }

            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                OErr.CreateLog();
                return null;
            }
            return (csDs);
        }

        /// <summary>
        /// This is used to check any Template is assigned to Entire Catalog
        /// </summary>
        /// <param name="FamilyID">int</param>
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
        ///  TemplateLayoutDataAccess oTmpLayout =  new TemplateLayoutDataAccess();
        ///   DataSet odsTemplate = new DataSet();
        ///   FamilyID = oHelper.CI(Request["Fid"]);
        ///   ...
        ///   odsTemplate = oTmpLayout.CheckEntireCatalog(FamilyID);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public DataSet CheckEntireCatalog(int FamilyID)
        {
            string sSql = "";
            DataSet csDs;
            try
            {
                sSql = "SELECT TEMPLATE_SOURCE,TEMPLATE_FILE_PATH,NO_OF_ROWS_INPAGE,NO_OF_COLUMN_INPAGE,IS_CS_TEMPLATE,IS_PAGING FROM TBWC_TEMPLATES WHERE IS_APPLICABLE_ENTIRE_CATALOG=1 AND TEMPLATE_TYPE=" + (int)TemplateType.FamilyLayout;
                oHelper.SQLString = sSql;
                csDs = new DataSet();
                csDs = oHelper.GetDataSet();
                if (csDs == null)
                {
                    csDs = new DataSet();
                    csDs = GetTemplateContent(FamilyID);
                }
            }
            catch (Exception ex)
            {
                OErr.ErrorMsg = ex;
                OErr.CreateLog();
                return null;
            }
            return csDs;
        }

        /// <summary>
        /// This is used to Get the DefaultTemplate Count
        /// </summary>
        /// <param name="TemplateTypes">int</param>
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
        ///  int DefaultCount;
        ///  ...
        ///  DefaultCount = TLData.GetDefaultTemplateCount(TemplateType);
        /// }
        /// </code>
        /// </example>
        [WebMethod]
        public int GetDefaultTemplateCount(int TemplateTypes)
        {
            try
            {
                int Count = 0;
                string SSql = "SELECT COUNT(IS_DEFAULT_TEMPLATE) AS DEFAULTCOUNT FROM TBWC_TEMPLATES WHERE  IS_DEFAULT_TEMPLATE=1 AND TEMPLATE_TYPE=" + TemplateTypes;
                oHelper.SQLString = SSql;

                Count = oHelper.CI(oHelper.GetValue("DEFAULTCOUNT").ToString());
                return Count;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

    }
}
