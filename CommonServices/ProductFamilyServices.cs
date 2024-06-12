using System;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data.Common;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebCat.CommonServices
{
    /// <summary>
    /// This is used to get all the Product Family Details
    /// </summary>
    /// <remarks>
    /// Used to get the Category Family List,Family list With Product Count,
    /// Family list By SupplierName,Root Family List,Family Attributes,FamilyLayout,
    /// Family PDF or documents,Family Foot Notes....
    /// </remarks>
    /// <example>
    /// ProductFamily oPF=new ProductFamily();
    /// </example> 
    
    public class ProductFamilyServices
    {
        /// <summary>
        /// Create Enum Values for Category Display (CategoryHierarchy or  CategoryNameAlone)
        /// </summary>

        public enum CategoryDisplayAs
        {
            /// <summary>
            /// Category Hierarchy
            /// </summary>
            CategoryHierarchy = 1,
            /// <summary>
            /// Display Category Name Only
            /// </summary>
            CategoryNameAlone = 2
        }

        HelperDB oHelper = new HelperDB();
        ErrorHandler oErrHand;
        ConnectionDB oCon = new ConnectionDB();
        
        string retVal = "";
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ProductFamilyServices()
        {
            oErrHand = new ErrorHandler();
        }
        string _HirearchyStyleStartTag = "";
        string _HirearchyStyleEndTag = "";
        string _HierarchyCharacter =" >> ";
        string _CatalogStyleStartTag = "";
        string _CatalogStyleEndTag = "";
        public string HirearchyStyleStartTag
        {
            get
            {
                return _HirearchyStyleStartTag;
            }
            set
            {
                _HirearchyStyleStartTag = value;
            }
        }
        public string HirearchyStyleEndTag
        {
            get
            {
                return _HirearchyStyleEndTag;
            }
            set
            {
                _HirearchyStyleEndTag = value;
            }
        }
        public string HierarchyCharacter
        {
            get
            {
                return _HierarchyCharacter;
            }
            set
            {
                _HierarchyCharacter = value;
            }
        }
        public string CatalogStyleStartTag
        {
            get
            {
                return _CatalogStyleStartTag;
            }
            set
            {
                _CatalogStyleStartTag = value;
            }
        }
        public string CatalogStyleEndTag
        {
            get
            {
                return _CatalogStyleEndTag;
            }
            set
            {
                _CatalogStyleEndTag = value;
            }
        }
        # region "Family Related functions"
        /// <summary>
        /// This is used to get all the Family List under a Category
        /// </summary>
        /// <param name="categoryID">string</param>
        /// <param name="CatalogID">integer</param>
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     string categoryID;
        ///     int CatalogID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oPF.GetCategoryFamilyList(categoryID,CatalogID);
        /// }
        /// </code>
        /// </example> 
        //[WebMethod]
        //public DataSet GetCategoryFamilyList(string categoryID,int CatalogID)
        //{
        //    try
        //    {
        //        string sSQL = " SELECT * FROM TB_FAMILY TF,TB_CATALOG_FAMILY TCF";
        //        sSQL = sSQL + " WHERE TF.CATEGORY_ID=N'" + categoryID + "'";
        //        sSQL = sSQL + " AND TF.FAMILY_ID = TCF.FAMILY_ID AND TCF.CATALOG_ID =" + CatalogID;
        //        sSQL = sSQL + " ORDER BY FAMILY_NAME";
        //        oHelper.SQLString = sSQL;
        //        return oHelper.GetDataSet("FamilyList");
        //    }
        //    catch (Exception ex)
        //    {
        //        oErrHand.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        return null;
        //    }
        //}
        /// <summary>
        /// This is used to get the Family List with Product Count
        /// </summary>
        /// <param name="CategoryID">string</param>
        /// <param name="CatalogID">integer</param>
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     string CategoryID;
        ///     int CatalogID; 
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oPF.GetFamilylistWithProdCount(CategoryID,CatalogID);
        /// }
        /// </code>
        /// </example> 
        //[WebMethod]
        //public DataSet GetFamilylistWithProdCount(string CategoryID,int CatalogID)
        //{
        //    DataSet ODs = new DataSet();
        //    try
        //    {
        //        string sSQL = " SELECT TF.FAMILY_ID,TF.FAMILY_NAME,COUNT(TPF.PRODUCT_ID)AS PRODUCT_COUNT FROM TB_FAMILY TF, TB_PROD_FAMILY TPF, TBWC_INVENTORY TWI,TB_CATALOG_FAMILY TCF ";
        //        sSQL = sSQL + " WHERE TWI.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE'AND TPF.FAMILY_ID = TF.FAMILY_ID AND TF.CATEGORY_ID = N'" + CategoryID + "' AND PRODUCT_STATUS = 'AVAILABLE' ";
        //        sSQL = sSQL + " AND TF.FAMILY_ID = TCF.FAMILY_ID AND TCF.CATALOG_ID ="+ CatalogID +" GROUP BY TF.FAMILY_ID, TF.FAMILY_NAME "; //ORDER BY TF.FAMILY_NAME";
        //        sSQL = sSQL + " UNION";
        //        sSQL = sSQL + " SELECT TF.FAMILY_ID,TF.FAMILY_NAME,'0'AS PRODUCT_COUNT";
        //        sSQL = sSQL + " FROM TB_FAMILY TF,TB_CATALOG_FAMILY TCF WHERE TF.CATEGORY_ID ='" + CategoryID + "' AND TF.FAMILY_ID = TCF.FAMILY_ID AND TCF.CATALOG_ID =1003";
        //        sSQL = sSQL + " AND PARENT_FAMILY_ID = 0 AND TF.FAMILY_ID NOT IN(SELECT DISTINCT(FAMILY_ID)FROM TB_PROD_FAMILY) ORDER BY FAMILY_NAME";

        //        oHelper.SQLString = sSQL;
        //        ODs = oHelper.GetDataSet();
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
        /// This is used to get the Family List using Supplier Name
        /// </summary>
        /// <param name="SupplierName">string</param>
        /// <param name="CatalogID">string</param> 
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     string SupplierName;
        ///     string CatalogID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oPF.GetFamilylistBySupplierName(SupplierName,CatalogID);
        /// }
        /// </code>
        /// </example> 

        [WebMethod]
        public DataSet GetFamilylistBySupplierName(string SupplierName, string CatalogID)
        {
            try
            {
                string sSQL = " SELECT TF.FAMILY_ID,TF.FAMILY_NAME,COUNT(TP.PRODUCT_ID) AS 'PRODUCT_COUNT' FROM TB_FAMILY TF, TB_PROD_FAMILY TPF, TBWC_INVENTORY TWI,TB_PRODUCT TP,TB_CATALOG_SECTIONS TCS";
                sSQL = sSQL + " WHERE TWI.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.FAMILY_ID = TF.FAMILY_ID AND TP.PRODUCT_ID = TPF.PRODUCT_ID";
                sSQL = sSQL + " AND PRODUCT_STATUS = 'AVAILABLE' AND TPF.PUBLISH='TRUE' AND SUPPLIER_NAME ='" + SupplierName + "'";
                sSQL = sSQL + " AND TCS.CATEGORY_ID = TP.CATEGORY_ID AND CATALOG_ID ='" + CatalogID + "'";
                sSQL = sSQL + " GROUP BY TF.FAMILY_ID, TF.FAMILY_NAME ORDER BY FAMILY_NAME ";

                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>
        /// This is used to get all the parent family list
        /// </summary>
        /// <param name="categoryID">string</param>
        /// <param name="CatalogID">integer</param>
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     string categoryID;
        ///     int CatalogID;
        ///     DataTable oDT = new DataTable();
        ///     ...
        ///     oDT = oPF.GetRootFamilyList(categoryID,CatalogID);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public DataTable GetRootFamilyList(string categoryID,int CatalogID)
        {
            try
            {
                string sSQL = " SELECT * FROM TB_FAMILY TF ,TB_CATALOG_FAMILY TCF";
                sSQL = sSQL + " WHERE TF.ROOT_FAMILY = 1 AND TF.FAMILY_ID = TCF.FAMILY_ID";
                sSQL = sSQL + " AND TCF.CATALOG_ID =" + CatalogID;
                sSQL = sSQL + " AND TF.CATEGORY_ID ='" + categoryID + "'";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataTable();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>
        /// This is used to get the Category Name using Family ID
        /// </summary>
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     int FamilyID;
        ///     string retVal;
        ///     ...
        ///     retVal = oPF.CategoryName(FamilyID);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public string CategoryName(int FamilyID)
        {
            string retVal;
            try
            {
                string sSQL = "SELECT CATEGORY_ID FROM TB_FAMILY WHERE FAMILY_ID = " + FamilyID;
                oHelper.SQLString = sSQL;
                retVal = oHelper.GetValue("CATEGORY_ID");
                retVal = GetParentCategory(retVal);
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                retVal = null;
            }

            return retVal;
        }

        /// <summary>
        /// This is used to get the parent category name
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     string CategoryID;
        ///     string ParentCategory;
        ///     ...
        ///     ParentCategory = oPF.GetParentCategory(CategoryID);
        /// }
        /// </code>
        /// </example> 
        public string GetParentCategory(string CategoryID)
        {
            bool isReturnValue = false;
            try
            {

                string _ParentCategory;
                string sSQL = "SELECT CATEGORY_NAME, PARENT_CATEGORY FROM TB_CATEGORY WHERE CATEGORY_ID = '" + CategoryID + "' ";
                oHelper.SQLString = sSQL;
                _ParentCategory = oHelper.GetValue("PARENT_CATEGORY");
                if (_ParentCategory != "0" && _ParentCategory != null && _ParentCategory.ToString() != "" && isReturnValue == false)
                {
                    retVal = HierarchyCharacter + "<A HREF=\"CategoryDisplay.aspx?CatID=" + CategoryID + "\">" + HirearchyStyleStartTag + oHelper.GetValue("CATEGORY_NAME") + HirearchyStyleEndTag + "</A>" + retVal;
                    //sCategoryNames = retVal;
                    GetParentCategory(_ParentCategory);
                }
                else
                {
                    retVal = "<A HREF=\"CategoryDisplay.aspx?CatID=" + CategoryID + "\">" + HirearchyStyleStartTag + oHelper.GetValue("CATEGORY_NAME") + HirearchyStyleEndTag + "</A>" + retVal;
                }
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                retVal = null;
            }
            return retVal;
        }
        /// <summary>
        /// This is used to get all the sub family list
        /// </summary>
        /// <param name="parentID">int</param> 
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     int parentID;
        ///     DataTable oDT = new  DataTable();
        ///     ...
        ///     oDT = PF.GetSubFamilyList(parentID);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public DataTable GetSubFamilyList(int parentID)
        {
            try
            {
                string sSQL = " SELECT * FROM TB_FAMILY TF ";
                sSQL = sSQL + " WHERE TF.PARENT_FAMILY_ID =" + parentID;
                oHelper.SQLString = sSQL;
                return oHelper.GetDataTable(); ;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>
        /// This is used to get all the families list
        /// </summary>
        /// <param name="familyID">int</param> 
        /// <param name="parentID">int</param> 
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     int familyID;
        ///     int parentID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oPF.GetFamilyList(familyID,parentID);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public DataSet GetFamilyList(int familyID, int parentID)
        {
            try
            {
                string sSQL;
                if (parentID == 0)
                {
                    sSQL = " SELECT * FROM TB_FAMILY";
                    sSQL = sSQL + " WHERE PARENT_FAMILY_ID = " + familyID;
                    sSQL = sSQL + " ORDER BY FAMILY_NAME ";
                }
                else
                {
                    sSQL = " SELECT * FROM TB_FAMILY";
                    sSQL = sSQL + " WHERE PARENT_FAMILY_ID = " + parentID;
                    sSQL = sSQL + " ORDER BY FAMILY_NAME ";
                }
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet("FamilyList");
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>
        /// This is used to get all the family attributes
        /// </summary>
        /// <param name="familyID">int</param> 
        /// <param name="attributeType">int</param> 
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     int familyID;
        ///     int attributeType;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oPF.GetFamilyAttributes(familyID,attributeType);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public DataSet GetFamilyAttributes(int familyID, int attributeType)
        {
            try
            {
                string sSQL = " SELECT * FROM TB_FAMILY_ATTR_LIST";
                sSQL = sSQL + " WHERE FAMILY_ID " + familyID;
                if (attributeType != 0)
                {
                    sSQL = sSQL + " AND ATTRIBUTE_ID IN (SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_TYPE =" + attributeType;
                    sSQL = sSQL + " AND ( CREATE_BY_DEFAULT = 'Y' OR XML_ELEMENT = 'Y'))";
                }
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet("FamilyAttributes");
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>
        /// This is used to get the family specification attributes and its values
        /// </summary>
        /// <param name="familyID">int</param>
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     int familyID;
        ///     int CatalogID;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oPF.GetFamilySpecList(familyID,CatalogID);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public DataSet GetFamilySpecList(int familyID,int CatalogID)
        {
            try
            {
                string sSQL = " SELECT TA.ATTRIBUTE_ID AS ATTRIBUTEID,TA.ATTRIBUTE_NAME AS ATTRIBUTENAME,TFS.STRING_VALUE AS ATTRIBUTEVALUE";
                sSQL = sSQL + " FROM TB_FAMILY_SPECS TFS,TB_ATTRIBUTE TA ,TB_CATALOG_ATTRIBUTES TCA ";
                sSQL = sSQL + " WHERE TFS.FAMILY_ID=" + familyID + " AND TFS.STRING_VALUE <>'' ";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TFS.ATTRIBUTE_ID";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID IN(SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_TYPE =7 AND PUBLISH2WEB =1)";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet("FamilySpecList");
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        
        /// <summary>
        /// This is used to get the families images
        /// </summary>
        /// <param name="familyID">int</param> 
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     int familyID;
        ///     int CatalogID
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oPF.GetFamilyImageList(familyID,CatalogID);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public DataSet GetFamilyImageList(int familyID,int CatalogID)
        {
            try
            {
                string sSQL = " SELECT TFS.ATTRIBUTE_ID ATTRIBUTEID,TFS.OBJECT_NAME AS IMAGENAME,TFS.STRING_VALUE AS IMAGEFILE,TFS.OBJECT_TYPE AS IMAGETYPE";
                sSQL = sSQL + " FROM TB_FAMILY_SPECS TFS,TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA";
                sSQL = sSQL + " WHERE TFS.FAMILY_ID =" + familyID + " AND TFS.STRING_VALUE <>'' AND TFS.OBJECT_TYPE <>'pdf' AND TFS.OBJECT_TYPE <>'doc' ";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TFS.ATTRIBUTE_ID";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID IN(SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_TYPE =9 AND PUBLISH2WEB =1 )";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID AND TCA.CATALOG_ID =" + CatalogID;
                sSQL = sSQL + " ORDER BY TA.ATTRIBUTE_NAME";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet("FamilyImageList");
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        /// <summary>
        /// This is used to get the family count using category id
        /// </summary>
        /// <param name="categoryID">string</param> 
        /// <param name="CatalogID">integer</param>
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     string categoryID;
        ///     int CatalogID;
        ///     int countNo;
        ///     ...
        ///     countNo = oPF.GetFamilyCount(categoryID,CatalogID);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public int GetFamilyCount(string categoryID,int CatalogID)
        {
            int countNo = 0;
            try
            {
                DataSet dsSub = new DataSet();
                OleDbConnection con = new OleDbConnection(oCon.ConnectionString);
                con.Open();
                string sSQL = " SELECT COUNT(*) FROM TB_FAMILY TF,TB_CATALOG_FAMILY TCF";
                sSQL = sSQL + " WHERE TF.FAMILY_ID=TCF.FAMILY_ID AND TCF.CATALOG_ID=" + CatalogID + " AND TF.PARENT_FAMILY_ID= 0 ";
                sSQL = sSQL + " AND TF.CATEGORY_ID=N'" + categoryID + "'";
                OleDbCommand oCmd = new OleDbCommand(sSQL, con);
                countNo = oHelper.CI(oCmd.ExecuteScalar());
                con.Close();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                countNo = -1;
            }
            return countNo;
        }
        /// <summary>
        /// This is used to count the number of sub families in the parent family
        /// </summary>
        /// <param name="PrarentFamilyID">int</param> 
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     int PrarentFamilyID;
        ///     int countNo;
        ///     ...
        ///     countNo = oPF.GetSubFamilyCount(PrarentFamilyID);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public int GetSubFamilyCount(int ParentFamilyID,int CatalogID)
        {
            int countNo = 0;
            try
            {
                DataSet dsSub = new DataSet();
                OleDbConnection con = new OleDbConnection(oCon.ConnectionString);
                con.Open();
                string sSQL = " SELECT COUNT(*) FROM TB_FAMILY TF,TB_CATALOG_FAMILY TCF";
                sSQL = sSQL + " WHERE TCF.CATALOG_ID=" + CatalogID + " AND TF.FAMILY_ID=TCF.FAMILY_ID AND PARENT_FAMILY_ID=" + ParentFamilyID;
                OleDbCommand oCmd = new OleDbCommand(sSQL, con);
                countNo = oHelper.CI(oCmd.ExecuteScalar());
                con.Close();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                countNo = -1;
            }
            return countNo;
        }
        /// <summary>
        /// This is used to count the number of available families in the category
        /// </summary>
        /// <param name="CategoryID">string</param> 
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     string CategoryID;
        ///     int FamilyCount;
        ///     ...
        ///     FamilyCount = oPF.GetFamilyCountWithInventory(CategoryID);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public int GetFamilyCountWithInventory(string CategoryID,int CatalogID)
        {
            int FamilyCount = 0;
            try
            {
                string sSQL = " SELECT COUNT(DISTINCT TF.FAMILY_ID)+ ";
                sSQL = sSQL + " (SELECT COUNT(DISTINCT(TBF.FAMILY_ID)) FROM TB_FAMILY TBF ,TB_CATALOG_FAMILY TCF WHERE TBF.CATEGORY_ID = '" + CategoryID + "' AND TBF.PARENT_FAMILY_ID = 0 AND TBF.FAMILY_ID = TCF.FAMILY_ID AND TCF.CATALOG_ID =" + CatalogID + " AND TBF.FAMILY_ID NOT IN(SELECT DISTINCT(FAMILY_ID) FROM TB_PROD_FAMILY)) ";
                sSQL = sSQL + " AS FAMILY_COUNT FROM TB_FAMILY TF, TB_PROD_FAMILY TPF, TBWC_INVENTORY TWI,TB_CATALOG_FAMILY TCFF";
                sSQL = sSQL + " WHERE TWI.PRODUCT_ID = TPF.PRODUCT_ID AND TPF.PUBLISH='TRUE' AND TPF.FAMILY_ID = TF.FAMILY_ID ";
                sSQL = sSQL + " AND TF.CATEGORY_ID = N'" + CategoryID + "' AND PRODUCT_STATUS = 'AVAILABLE' AND TCFF.FAMILY_ID = TF.FAMILY_ID AND TCFF.CATALOG_ID=" + CatalogID;
                oHelper.SQLString = sSQL;
                FamilyCount = oHelper.CI(oHelper.GetValue("FAMILY_COUNT"));
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();    
                FamilyCount = -1;
            }
            return FamilyCount;
        }
        /// <summary>
        /// This is used to get the family name
        /// </summary>
        /// <param name="familyID">int</param> 
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     int familyID;
        ///     string sFamilyName;
        ///     ...
        ///     sFamilyName = oPF.GetFamilyName(familyID);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public string GetFamilyName(int familyID)
        {
            string sFamilyName = "";
            try
            {
                DataSet dsFam = new DataSet();
                string sSQL = " SELECT FAMILY_NAME FROM TB_FAMILY";
                sSQL = sSQL + " WHERE FAMILY_ID=" + familyID;
                oHelper.SQLString = sSQL;
                dsFam = oHelper.GetDataSet();
                if (dsFam != null)
                {
                    sFamilyName = dsFam.Tables[0].Rows[0].ItemArray[0].ToString();
                }
                if (dsFam != null)
                {
                    dsFam.Dispose();
                }
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
            }
            return sFamilyName;
        }
        /// <summary>
        /// This is used to get the footnotes for family
        /// </summary>
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     int FamilyID;
        ///     string retVal;
        ///     ...
        ///     retVal = oPF.FootNotes(FamilyID);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public string FootNotes(int FamilyID)
        {
            string retVal;
            try
            {
                string sSQL = "SELECT * FROM TB_FAMILY WHERE FAMILY_ID = " + FamilyID + " ";
                oHelper.SQLString = sSQL;
                retVal = oHelper.GetValue("FOOT_NOTES");
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                retVal = null;
            }
            return retVal;
        }
        /// <summary>
        /// This is used to get family attachements(PDF or DOC)
        /// </summary>
        /// <param name="familyID">int</param> 
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     int familyID;
        ///     DataSet oDS =new DataSet();
        ///     ...
        ///     oDS = oPF.GetFamilyPdf(familyID);
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public DataSet GetFamilyPdf(int familyID)
        {
            try
            {
                string sSQL = " SELECT TFS.ATTRIBUTE_ID AS ATTRIBUTEID,TA.ATTRIBUTE_NAME AS ATTRIBUTENAME,TFS.OBJECT_NAME AS IMAGENAME,STRING_VALUE AS IMAGEFILE,OBJECT_TYPE AS IMAGETYPE";
                sSQL = sSQL + " FROM TB_FAMILY_SPECS TFS,TB_ATTRIBUTE TA";
                sSQL = sSQL + " WHERE TFS.FAMILY_ID =" + familyID + " AND TFS.STRING_VALUE <>''AND TFS.OBJECT_TYPE ='pdf' ";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID = TFS.ATTRIBUTE_ID";
                sSQL = sSQL + " AND TA.ATTRIBUTE_ID IN(SELECT ATTRIBUTE_ID FROM TB_ATTRIBUTE WHERE ATTRIBUTE_TYPE =9 AND PUBLISH2WEB =1 )";
                sSQL = sSQL + " ORDER BY TA.ATTRIBUTE_NAME";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }

        }
        /// <summary>
        /// This is used to get the Family Layouts
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
        ///     ProductFamily oPF=new ProductFamily();
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oPF.GetFamilyLayout();
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        public DataSet GetFamilyLayout()
        {
            try
            {
                string sSQL = "SELECT FAMILY_ID,LAYOUT,GROUPED_COLUMNS FROM TBWC_PROD_FAMILY_LAYOUT";
                oHelper.SQLString = sSQL;
                return oHelper.GetDataSet();
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                //oErrHand.CreateLog();
                return null;
            }
        }
        [WebMethod]
        public string GetCatalogName(string CatalogID)
        {
            string retVal;
            try
            {
                string sSQL = "SELECT CATALOG_NAME FROM TB_CATALOG WHERE CATALOG_ID =" + CatalogID;
                oHelper.SQLString = sSQL;
                retVal = oHelper.GetValue("CATALOG_NAME");
                retVal = CatalogStyleStartTag + retVal + CatalogStyleEndTag;
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
                retVal = null;
            }
            return retVal;
        }

        [WebMethod]
        public int GetFamilyID(int ProductID)
        {
            int retVal=0;
            try
            {
                string sSQL = "SELECT DISTINCT FAMILY_ID FROM TB_PROD_FAMILY WHERE PRODUCT_ID =" + ProductID;
                oHelper.SQLString = sSQL;
                retVal =oHelper.CI(oHelper.GetValue("FAMILY_ID").ToString());
            }
            catch (Exception ex)
            {
                oErrHand.ErrorMsg = ex;
                oErrHand.CreateLog();
            }
            return retVal;
        }
        #endregion

    }
}
