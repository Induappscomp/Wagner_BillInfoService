using System;
using System.Web;
using System.Data;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

using TradingBell5.CatalogX;
using System.IO;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;

namespace TradingBell.WebCat.CommonServices
{
    /// <summary>
    /// Summary description for CSRender
    /// </summary>
    public class CSRenderServices 
    {
        HelperDB oHelper = new HelperDB();
        ConnectionDB oCon = new ConnectionDB();
        ErrorHandler oErr = new ErrorHandler();
        int[] AttributeIdList;
        public CSRenderServices()
        {


        }

        #region CXRelavant Functions

        //This is used to populate required Attributes(Family and Product)
        /// <summary>
        /// This is used to populate required Attributes(Family and Product)
        /// </summary>
        /// <param name="CatalogID">string</param>
        /// <param name="userId">int</param>
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
        ///   FamilyRender oFamRender = new FamilyRender();
        ///   string CatID;
        ///   int userid;
        ///   DataSet oDS = new DataSet();
        ///   ...
        ///   oDS = oFamRender.GetAttributes(CatID,userid);
        /// }
        /// </code>
        /// </example>
        //[WebMethod]
        //public DataSet GetAttributes(int CatalogID, int userId)
        //{
        //    DataSet CXAttribute = new DataSet();
        //    string sSQL = "";
        //    try
        //    {
        //        if (userId > 0)
        //        {
        //            sSQL = "SELECT TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME,ATTRIBUTE_TYPE FROM TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE "
        //            + " PUBLISH2WEB =1"
        //            + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID"
        //            + " AND TCA.CATALOG_ID =" + CatalogID
        //            + " AND TA.ATTRIBUTE_TYPE IN(1,4,3,5,7,9) AND NOT (TA.ATTRIBUTE_ID BETWEEN 480 AND 503)"
        //            + " UNION"
        //            + " SELECT ATTRIBUTE_ID,ATTRIBUTE_NAME,ATTRIBUTE_TYPE"
        //            + " FROM TB_ATTRIBUTE WHERE NOT (ATTRIBUTE_ID BETWEEN 480 AND 503) AND ATTRIBUTE_ID IN("
        //            + " SELECT PRICE_ATTRIBUTE_ID FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP IN("
        //            + " SELECT  BUYER_GROUP FROM TBWC_COMPANY WHERE COMPANY_ID IN("
        //            + " SELECT COMPANY_ID FROM TBWC_COMPANY_BUYERS WHERE USER_ID =" + userId + "))) OR ATTRIBUTE_NAME='~Restricted'";
        //        }
        //        else
        //        {
        //            sSQL = "SELECT TA.ATTRIBUTE_ID,TA.ATTRIBUTE_NAME,ATTRIBUTE_TYPE FROM TB_ATTRIBUTE TA,TB_CATALOG_ATTRIBUTES TCA WHERE "
        //                + " AND PUBLISH2WEB =1"
        //                + " AND TA.ATTRIBUTE_ID = TCA.ATTRIBUTE_ID"
        //                + " AND TCA.CATALOG_ID =" + CatalogID
        //                + " AND TA.ATTRIBUTE_TYPE IN(1,3,4,5,7,9)  AND NOT (TA.ATTRIBUTE_ID BETWEEN 480 AND 503)"
        //                + " UNION"
        //                + " SELECT ATTRIBUTE_ID,ATTRIBUTE_NAME,ATTRIBUTE_TYPE"
        //                + " FROM TB_ATTRIBUTE WHERE NOT (ATTRIBUTE_ID BETWEEN 480 AND 503) AND  ATTRIBUTE_ID IN("
        //                + " SELECT PRICE_ATTRIBUTE_ID FROM TBWC_BUYER_GROUP WHERE BUYER_GROUP ='DEFAULTBG') OR ATTRIBUTE_NAME='~Restricted'";
        //        }
        //        oHelper.SQLString = sSQL;
        //        CXAttribute = oHelper.GetDataSet();
        //        if (CXAttribute != null)
        //        {
        //            CXAttribute.Dispose();
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        oErr.ErrorMsg = Ex;
        //        oErr.CreateLog();
        //        return null;
        //    }
        //    return CXAttribute;
        //}

        //This is used to get the catalog family and product details by the way of using Catalogx
        /// <summary>
        /// This is used to get the catalog family and product details by the way of using Catalogx
        /// </summary>
        /// <param name="FamilyID">int</param>
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
        ///   FamilyRender oFamRender = new FamilyRender();
        ///   int FamId;
        ///   DataSet oDS = new DataSet();
        ///   ...
        ///   oDS = oFamRender.GetCXDs(FamId);
        /// }
        /// </code>
        /// </example>
        //[WebMethod]
        //protected DataSet GetCXDs(int CatalogID,int FamilyID,int UserID)
        //{
           
        //    int i = 0;
        //    DataSet CXAttributes=new DataSet();
        //    string Con = oCon.ConnectionString.ToString();
        //    Con = Con.Remove(0, Con.IndexOf(';') + 1);
        //    DataSet CXDs = new DataSet();
        //    try
        //    {
        //        CXAttributes = GetAttributes(CatalogID, UserID);
        //        if ((CXAttributes != null) && CXAttributes.Tables[0].Rows.Count > 0)
        //        {
        //            AttributeIdList = new int[CXAttributes.Tables[0].Rows.Count];
        //        }
        //        if (CXAttributes != null)
        //        {
        //            foreach (DataRow cxDR in CXAttributes.Tables[0].Rows)
        //            {
        //                AttributeIdList[i] = Convert.ToInt32(cxDR[0].ToString());
        //                i = i + 1;
        //            }
        //            CXAttributes.Dispose();
        //        }
        //        TradingBell5.CatalogX.CatalogXfunction oProdTable = new CatalogXfunction();
        //        CXDs = oProdTable.WebCatalogFamily(CatalogID, FamilyID, AttributeIdList, Con);
        //        if (CXDs != null)
        //        {
        //            CXDs.Dispose();
        //        }

        //    }
        //    catch (Exception Ex)
        //    {
        //        oErr.ErrorMsg = Ex;
        //        oErr.CreateLog();
        //        return null;
        //    }
        //    return (CXDs);
        //}

        //This is used to filter the family details from catalogx
        /// <summary>
        /// This is used to filter the family details from catalogx
        /// </summary>
        /// <param name="FamilyID">int</param>
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
        ///   FamilyRender oFamRender = new FamilyRender();
        ///   int FamId;
        ///   DataSet oDS = new DataSet();
        ///   ...
        ///   oDS = oFamRender.GetCXFamilies(FamId);
        /// }
        /// </code>
        /// </example>
        //[WebMethod]
        //public DataSet GetCXFamilies(int CatalogID,int FamilyID,int UserID)
        //{
        //    DataSet CXDsFamily = new DataSet();
        //    DataSet CXDs=new DataSet();
        //    try
        //    {
        //        CXDsFamily = GetCXDs(CatalogID,FamilyID,UserID);
        //        //if (CXDsFamily != null)
        //        //{
        //        //    //DataRow[] CXDrFamily = CXDsFamily.Tables["ProductFamily"].Select("FAMILY_ID=" + FamilyID);
        //        //    DataRow[] CXDrFamily = CXDsFamily.Tables["Family Details"].Select("FAMILY_ID=" + FamilyID);
        //        //    if (CXDrFamily.Length == 0)
        //        //    {
        //        //        sSqlStr = "SELECT FAMILY_ID,FAMILY_NAME,CATEGORY_ID FROM TB_FAMILY WHERE FAMILY_ID=" + FamilyID;
        //        //        oHelper.SQLString = sSqlStr;
        //        //        CXDsFamily = oHelper.GetDataSet();
        //        //        CXDrFamily = CXDsFamily.Tables[0].Select();
        //        //    }
        //        //    DataTable oTable = new DataTable();
        //        //    foreach (DataColumn oDC in CXDsFamily.Tables["Family Details"].Columns)
        //        //    {
        //        //        oTable.Columns.Add(oDC.ColumnName, oDC.DataType);
        //        //    }
        //        //    foreach (DataRow oDR in CXDrFamily)
        //        //    {
        //        //        oTable.ImportRow(oDR);
        //        //    }
        //        //    CXDsFamily.Clear();
        //        //    CXDs.Tables.Add(oTable);
        //        //}
        //        //else
        //        //{
        //        //    CXDs = null;
        //        //}
        //    }
        //    catch (Exception Ex)
        //    {
        //        oErr.ErrorMsg = Ex;
        //        oErr.CreateLog();
        //        return null;
        //    }
        //    return CXDsFamily;
        //    //return CXDs;
        //}

        //This is used to filter the product details
        /// <summary>
        /// This is used to filter the product details
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
        ///   FamilyRender oFamRender = new FamilyRender();
        ///   int FamId;
        ///   DataSet oDS = new DataSet();
        ///   ...
        ///   oDS = oFamRender.GetCXProducts(FamId);
        /// }
        /// </code>
        /// </example>

        //[WebMethod]
        //public DataSet GetCXProducts(int CatalogID,int FamilyID,int UserID)
        //{

        //    DataSet CXDsProduct = new DataSet();
        //    DataSet CXDs = new DataSet();
        //    try
        //    {
        //        CXDsProduct = GetCXDs(CatalogID,FamilyID,UserID);
        //        if (CXDsProduct != null)
        //        {
        //            DataRow[] CXDrProduct = CXDsProduct.Tables["ProductTable"].Select("FAMILY_ID=" + FamilyID);
        //            DataTable oTable = CXDsProduct.Tables["ProductTable"].Clone();
        //            foreach (DataRow oDR in CXDrProduct)
        //            {
        //                oTable.ImportRow(oDR);
        //            }
        //            CXDsProduct.Clear();
        //            CXDs.Tables.Add(oTable);
        //            oTable.Dispose();
        //            CXDsProduct.Dispose();
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        oErr.ErrorMsg = Ex;
        //        //oErr.CreateLog();
        //        return null;
        //    }
        //    return CXDs;
        //}

        //[WebMethod]
        //public DataSet GetSubFamilies(int FamilyID)
        //{
        //    DataSet oDS = new DataSet();
        //    try
        //    {
        //        DataTable oDT = new DataTable();
        //        TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.TB_SUBFAMILYTableAdapter oTASubFam = new TradingBell5.CatalogX.CSDBProvider.CSDSTableAdapters.TB_SUBFAMILYTableAdapter();
        //        TradingBell5.CatalogX.CSDBProvider.CSDS.TB_SUBFAMILYDataTable oDTSubFam = new TradingBell5.CatalogX.CSDBProvider.CSDS.TB_SUBFAMILYDataTable();
        //        oTASubFam.Fill(oDTSubFam);
        //        oTASubFam.Dispose();
        //        if (oDTSubFam != null)
        //        {
        //            oDT = oDTSubFam.Clone();
        //            DataRow[] oDR = oDTSubFam.Select("FAMILY_ID= " + FamilyID);
        //            foreach (DataRow oDRS in oDR)
        //            {
        //                oDT.ImportRow(oDRS);
        //            }
        //            oDTSubFam.Dispose();
        //        }
        //        oDS.Tables.Add(oDT);
        //        oDS.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        oErr.ErrorMsg = ex;
        //    }
        //    return oDS;
        //}
        #endregion


    }

}