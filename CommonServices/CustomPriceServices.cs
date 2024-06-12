using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Data;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebCat.CommonServices
{
    /// <summary>
    /// This is used to get the customized price value from database for the products
    /// </summary>
    /// <example>  CustomPrice oCustPrice = new oCustPrice(); </example> 

    public class CustomPriceServices 
    {

        #region Declarations
        string ConnectionString;
        string PriceValue;
        string sSQL;
        private string Provider_Name;
        private string Data_Source;
        private string Initial_Catalog;
        private string User_Id;
        private string password;
        int CustomProduct_Id;
        string CustomAttributeID_Column;
        int CustomAttribute_Id;
        string CustomProductId_ColumnName;
        string CustomTable_Name;
        string CustomPrice_columnName;
        string DB_Name;
        OleDbConnection OconString;
        OleDbDataAdapter oDA;
        DataSet oDS;
        #endregion
        #region Properties for ConnectionString
        //[Browsable(true),Category("TradingBell"),Description("Set or Get the Name of the Provider name")]
        /// <summary>
        /// Used to Get or Set the Database Server name like Oracle or SQL Server or DB2
        /// </summary>
        public string DataBaseServersName
        {
            get
            {
                return DB_Name;
            }
            set
            {
                DB_Name = value;
            }
        }
        /// <summary>
        /// Used to Get or Set the Database Provider like SQLOLEDB or ORAOLEDB
        /// </summary>
        public string Provider
        {
            get
            {
                return Provider_Name;
            }
            set
            {
                Provider_Name = value;
            }
        }

        /// <summary>
        /// Used to Get or Set the customized Server Name
        /// </summary>
        public string DataSource
        {
            get
            {
                return Data_Source;
            }
            set
            {
                Data_Source = value;
            }
        }
        /// <summary>
        /// used to Get or Set customized Database Name
        /// </summary>
        public string InitialCatalog
        {
            get
            {
                return Initial_Catalog;
            }
            set
            {
                Initial_Catalog = value;
            }
        }
        /// <summary>
        /// Used to Get or Set Database User ID
        /// </summary>
        public string DBUserId
        {
            get
            {
                return User_Id;
            }
            set
            {
                User_Id = value;
            }
        }
        /// <summary>
        /// Used to Get or Set Database Password
        /// </summary>
        public string DBPassword
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }
        #endregion
        #region Properties for CustomPrice
        /// <summary>
        /// Used to Get or Set Custom AttributeId ColumnName
        /// </summary>
        public string CustomAttributeIdColumnName
        {
            get
            {
                return CustomAttributeID_Column;
            }
            set
            {
                CustomAttributeID_Column = value;
            }
        }
        /// <summary>
        /// Used to Get or Set Custom Price Column Name
        /// </summary>
        public string CustomPriceColumnName
        {
            get
            {
                return CustomPrice_columnName;
            }
            set
            {
                CustomPrice_columnName = value;
            }
        }
        /// <summary>
        /// Used to Get or Set Custom Table Name
        /// </summary>
        public string CustomTableName
        {
            get
            {
                return CustomTable_Name;
            }
            set
            {
                CustomTable_Name = value;
            }
        }
        /// <summary>
        /// Used to Get or Set Custom Product ID
        /// </summary>
        public int CustomProductID
        {
            get
            {
                return CustomProduct_Id;
            }
            set
            {
                CustomProduct_Id = value;
            }
        }
        /// <summary>
        /// Used to Get or Set Custom Product Id Column Name
        /// </summary>
        public string CustomProductIdColumnName
        {
            get
            {
                return CustomProductId_ColumnName;
            }
            set
            {
                CustomProductId_ColumnName = value;
            }
        }
        /// <summary>
        /// Used to Get or Set Custom Attribute Id 
        /// </summary>
        public int CustomAttributeId
        {
            get
            {
                return CustomAttribute_Id;
            }
            set
            {
                CustomAttribute_Id = value;
            }
        }
        #endregion
        public CustomPriceServices()
        {
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }
        /// <summary>
        /// This is used to get the Custom price for Products
        /// </summary>
        /// <remarks>
        /// Used to retrieve the custom price for products if Custom Price is enabled for a product 
        /// </remarks> 
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
        ///     CustomPrice oCustPrice = new oCustPrice();
        ///     string priceValue;
        ///     ...
        ///     priceValue = oCustPrice.GetCustomPrice();
        /// }
        /// </code>
        /// </example> 
        [WebMethod]
        #region Methods
        public string GetCustomPrice()
        {
            try
            {
                OconString = new OleDbConnection();
                oDS = new DataSet();
                if (DB_Name.ToLower() == "msaccess")
                {
                    ConnectionString = "Provider=" + Provider_Name + "; data source=" + Data_Source;
                }
                else if (DB_Name.ToLower() == "sqlserver")
                {
                    ConnectionString = "Provider=" + Provider_Name + "; data source=" + Data_Source + ";Initial Catalog=" + Initial_Catalog + ";User Id=" + User_Id + ";Password=" + password;
                }
                OconString.ConnectionString = ConnectionString;
                OconString.Open();

                //Build the Query
                sSQL = "SELECT " + CustomPrice_columnName + " AS PRICE FROM " + CustomTable_Name + " WHERE " + CustomProductId_ColumnName + "=" + "'" + CustomProduct_Id + "'" + " AND " + CustomAttributeIdColumnName + "=" + "'" + CustomAttribute_Id + "'";

                //Fill the Table to Dataset
                oDA = new OleDbDataAdapter(sSQL, OconString);
                oDA.Fill(oDS);
                if (oDS != null)
                {
                    foreach (DataRow oRow in oDS.Tables[0].Rows)
                    {
                        //Get the Price Value
                        PriceValue = oRow["PRICE"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                string Err = ex.Message.ToString();
                return null;
            }
            return PriceValue;
        }
        #endregion
    }

}