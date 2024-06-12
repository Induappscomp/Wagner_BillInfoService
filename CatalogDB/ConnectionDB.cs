using System;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Security.Cryptography;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebCat.CatalogDB
{

#region OLD CODE TRADING BELL
    //private string smConnectionString;
    //    SqlConnection smConnection;        
    //    Hashtable tbGlobals = new Hashtable();
    //    IDbTransaction mTransaction;
    //    ErrorHandler objErrorHandler = new ErrorHandler(); 
    //    Security objSecurity = new Security();

    //    public string ConnectionString
    //    {
    //        get
    //        {
    //            return smConnectionString;
    //        }
    //        set
    //        {
    //            ConnectionString = smConnectionString;
    //        }
    //    }
    //    private string GetSqlConnectionString()
    //    {
    //        string constring = null;
    //        if (HttpContext.Current.Session != null)
    //        {
    //            if (HttpContext.Current.Session["DBConnection"] == null)
    //            {
    //                constring = objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
    //                HttpContext.Current.Session["DBConnection"] = constring;
    //            }
    //            else
    //                constring = (string)HttpContext.Current.Session["DBConnection"];
    //        }

    //        smConnectionString = constring;
    //        return smConnectionString;
    //    }

    //    private SqlConnection GetSqlConnection()
    //    {
    //        //string constring=null;
    //        //if (HttpContext.Current.Session != null)
    //        //{
    //        //    if (HttpContext.Current.Session["DBConnection"] == null)
    //        //    {
    //        //        constring = objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
    //        //        HttpContext.Current.Session["DBConnection"] = constring;
    //        //    }
    //        //    else
    //        //        constring = (string)HttpContext.Current.Session["DBConnection"];
    //        //}
    //        //constring =
    //        smConnectionString = GetSqlConnectionString(); 
    //        return  new SqlConnection();        
    //    }
    //    # region "Initialization Functions "
    //    /// <summary>
    //    /// Establishing Connection String      
    //    /// </summary>    
    //    /// 

       

    //    public ConnectionDB()
    //    {           
    //        string constring;
    //        try
    //        {
    //            //string s=objSecurity.StringEnCrypt(@"Data Source=P3-SDSIP\SQLEXPRESS;Initial Catalog=TB_WESTEST;User Id=tbadmin;Password=data2go");
    //            //string s1=objSecurity.StringEnCrypt(@"Data Source=P3-SDSIP\SQLEXPRESS;Initial Catalog=TB_WESTEST;User Id=tbadmin;Password=data2go;persist security info=True;packet size=4096");
    //            //if (HttpContext.Current.Session!=null && HttpContext.Current.Session["DBConnection"] == null)
    //            //{
    //            //    constring = objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
    //            //    HttpContext.Current.Session["DBConnection"] = constring;
    //            //}
    //            //else
    //            //    constring = (string)HttpContext.Current.Session["DBConnection"];

    //            //smConnectionString = constring;                
    //            //smConnection = new SqlConnection();    
    //            smConnection=GetSqlConnection(); 
               
    //        }
    //        catch (Exception ex)
    //        {
    //            objErrorHandler.ErrorMsg = ex;
    //            objErrorHandler.CreateLog();
    //        }
            
    //    }
    
    //    #endregion

    //    # region "Connection Related Functions"
    //    /// <summary>
    //    /// This is used to open the OleDbConnection   
    //    /// </summary>
    //    /// <example>
    //    /// <code>
    //    /// using System;
    //    /// using System.IO;
    //    /// using System.Web;
    //    /// using System.Data;
    //    /// 
    //    /// using TradingBell.WebServices;
    //    /// 
    //    /// protected void Page_Load(object sender, EventArgs e)
    //    /// {
    //    ///     Connection oCon = new Connection();
    //    ///     ...
    //    ///     OleDbCommand oCmd = new OleDbCommand(sSQL, oCon.GetConnection());
    //    /// }
    //    /// </code>
    //    /// </example>
    //    /// <returns>OleDbConnection</returns>

    //    public SqlConnection GetConnection()
    //    {
    //        objErrorHandler = new ErrorHandler();
    //        try
    //        {
    //            if (smConnection == null)
    //            {
    //                smConnection = GetSqlConnection();
    //            }
    //            if (smConnectionString == null || smConnectionString == "")
    //            {
    //                smConnectionString = GetSqlConnectionString();
    //            }
    //            if (smConnection.State == ConnectionState.Closed)
    //            {
    //                smConnection.ConnectionString = smConnectionString;                     
    //                smConnection.Open();                      
    //            }
  
    //        }
    //        catch (Exception ex)
    //        {
    //            objErrorHandler.ErrorMsg = ex;
    //            objErrorHandler.CreateLog();
    //        }
    //        return smConnection;
    //    }
       
    //    /// <summary>
    //    /// This is used to Get Connection using Connection String     
    //    /// </summary>
    //    /// <param name="sConnectionString">string</param>
    //    /// <example>
    //    /// <code>
    //    /// using System;
    //    /// using System.IO;
    //    /// using System.Web;
    //    /// using System.Data;
    //    /// 
    //    /// using TradingBell.WebServices;
    //    /// 
    //    /// protected void Page_Load(object sender, EventArgs e)
    //    /// {
    //    ///     Connection oCon = new Connection();
    //    ///     string sConnectionString;
    //    ///     ...
    //    ///     OleDbCommand oCmd = new OleDbCommand(sSQL, oCon.GetConnection(sConnectionString));
    //    /// }
    //    /// </code>
    //    /// </example>
    //    /// <returns>OleDbConnection</returns>

    //    //public SqlConnection GetConnection(string sConnectionString)
    //    //{
    //    //    objErrorHandler = new ErrorHandler();
    //    //    try
    //    //    {
    //    //        smConnectionString = sConnectionString;
    //    //        OpenConnection();
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        objErrorHandler.ErrorMsg = ex;
    //    //        objErrorHandler.CreateLog();
    //    //    }
    //    //    return smConnection;
    //    //}
     
    //    /// <summary>
    //    /// This is used to Open the Connection     
    //    /// </summary>
    //    /// <param></param>
    //    /// <returns>True or False</returns>
    //    /// <example>
    //    /// <code>
    //    /// using System;
    //    /// using System.IO;
    //    /// using System.Web;
    //    /// using System.Data;
    //    /// 
    //    /// using TradingBell.WebServices;
    //    /// 
    //    /// protected void Page_Load(object sender, EventArgs e)
    //    /// {
    //    ///     Connection oCon = new Connection();
    //    ///     bool isConnected;
    //    ///     ...
    //    ///     isConnected = oCon.OpenConnection();
    //    /// }
    //    /// </code>
    //    /// </example>
    //    //public bool OpenConnection()
    //    //{
    //    //    bool isConnected = false;
    //    //    objErrorHandler = new ErrorHandler();
    //    //    try
    //    //    {
    //    //        //if (!(smConnection.State == System.Data.ConnectionState.Open))
    //    //        //{
                    
    //    //            smConnection.ConnectionString = smConnectionString;
    //    //             smConnection.Open();
    //    //            isConnected = true;
    //    //        //}
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        objErrorHandler.ErrorMsg = ex;
    //    //        isConnected = false;
    //    //        objErrorHandler.CreateLog();
    //    //    }
    //    //    return isConnected;
    //    //}
    //    /// <summary>
    //    /// This is used to close the Connection      
    //    /// </summary>
    //    /// <returns>True or False</returns>
    //    /// <example>
    //    /// <code>
    //    /// using System;
    //    /// using System.IO;
    //    /// using System.Web;
    //    /// using System.Data;
    //    /// 
    //    /// using TradingBell.WebServices;
    //    /// 
    //    /// protected void Page_Load(object sender, EventArgs e)
    //    /// {
    //    ///     Connection oCon = new Connection(); 
    //    ///     bool isClose;
    //    ///     ...
    //    ///     isClose = oCon.CloseConnection();
    //    /// } 
    //    /// </code>
    //    /// </example>
    //    public bool CloseConnection( )
    //    {
            
    //        bool isClose;
    //        try
    //        {
    //            if (smConnection.State == ConnectionState.Open)
    //            {
    //                smConnection.Close();
    //                smConnection.Dispose(); 
    //                smConnection = null;
    //            }
    //            isClose = true;
    //        }
    //        catch (Exception ex)
    //        {
    //            objErrorHandler.ErrorMsg = ex;
    //            isClose = false;
    //        }
    //        return isClose;
    //    }
      
    //    #endregion

    //}
#endregion
    /*********************************** J TECH CODE ***********************************/
    public delegate System.Security.Cryptography.ICryptoTransform GetEncryptengine();
    /// <summary>
    /// Establishing the Connection.      
    /// <para>  
    /// Make Connection.
    /// </para>
    /// </summary>
    /// <remarks>
    /// Used to Connect the Database 
    /// </remarks>
    /// <example>
    /// Connection oCon = new Connection();
    /// </example>

    public class ConnectionDB
    {
      
        /*********************************** DECLARATION ***********************************/
        private string smConnectionString;
        SqlConnection smConnection;        
        Hashtable tbGlobals = new Hashtable();
        IDbTransaction mTransaction;
        ErrorHandler objErrorHandler = new ErrorHandler(); 
        Security objSecurity = new Security();

        public string ConnectionString
        {
            get
            {
                return smConnectionString;
            }
            set
            {
                ConnectionString = smConnectionString;
            }
        }
        /*********************************** DECLARATION ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE DATABASE CONNECTION STRING ***/
        /********************************************************************************/

        private string GetSqlConnectionString()
        {
            string constring = null;
            try
            {
                if (HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session["DBConnection"] == null)
                    {
                        constring = objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
                        HttpContext.Current.Session["DBConnection"] = constring;
                    }
                    else
                        constring = (string)HttpContext.Current.Session["DBConnection"];
                }
                else 
                {
                    constring = objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
               
                }
            }
            catch (Exception ex)
            {
                constring = objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
            }

            smConnectionString = constring;
            return smConnectionString;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE DATABASE CONNECTION USING CONNECTION STRING ***/
        /********************************************************************************/
        private SqlConnection GetSqlConnection()
        {
            //string constring=null;
            //if (HttpContext.Current.Session != null)
            //{
            //    if (HttpContext.Current.Session["DBConnection"] == null)
            //    {
            //        constring = objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
            //        HttpContext.Current.Session["DBConnection"] = constring;
            //    }
            //    else
            //        constring = (string)HttpContext.Current.Session["DBConnection"];
            //}
            //constring =
            smConnectionString = GetSqlConnectionString(); 
            return  new SqlConnection();        
        }
        # region "Initialization Functions "
        /// <summary>
        /// Establishing Connection String      
        /// </summary>    
        /// 


        /*********************************** CONSTRUCTOR WITH CONNECTION STRING ***********************************/
        public ConnectionDB()
        {           
           // string constring;
            try
            {
                //string s=objSecurity.StringEnCrypt(@"Data Source=P3-SDSIP\SQLEXPRESS;Initial Catalog=TB_WESTEST;User Id=tbadmin;Password=data2go");
                //string s1=objSecurity.StringEnCrypt(@"Data Source=P3-SDSIP\SQLEXPRESS;Initial Catalog=TB_WESTEST;User Id=tbadmin;Password=data2go;persist security info=True;packet size=4096");
                //if (HttpContext.Current.Session!=null && HttpContext.Current.Session["DBConnection"] == null)
                //{
                //    constring = objSecurity.StringDeCrypt(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
                //    HttpContext.Current.Session["DBConnection"] = constring;
                //}
                //else
                //    constring = (string)HttpContext.Current.Session["DBConnection"];

                //smConnectionString = constring;                
                //smConnection = new SqlConnection();    
                smConnection=GetSqlConnection(); 
               
            }
            catch (Exception ex)
            {
               // objErrorHandler.ErrorMsg = ex;
              //  objErrorHandler.CreateLog();
            }
            
        }
        /*********************************** CONSTRUCTOR WITH CONNECTION STRING ***********************************/
        #endregion

        # region "Connection Related Functions"
        /// <summary>
        /// This is used to open the OleDbConnection   
        /// </summary>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// 
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Connection oCon = new Connection();
        ///     ...
        ///     OleDbCommand oCmd = new OleDbCommand(sSQL, oCon.GetConnection());
        /// }
        /// </code>
        /// </example>
        /// <returns>OleDbConnection</returns>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE DATABASE CONNECTION USING CONNECTION STRING ***/
        /********************************************************************************/
        public SqlConnection GetConnection()
        {
            objErrorHandler = new ErrorHandler();
            try
            {
             
                if (smConnection == null)
                {
                    smConnection = GetSqlConnection();
                }
                if (smConnectionString == null || smConnectionString == "")
                {
                    smConnectionString = GetSqlConnectionString();
                }
                if (smConnection.State == ConnectionState.Closed)
                {
                    smConnection.ConnectionString = smConnectionString;                     
                    smConnection.Open();                      
                }
  
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return smConnection;
        }
       
        /// <summary>
        /// This is used to Get Connection using Connection String     
        /// </summary>
        /// <param name="sConnectionString">string</param>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// 
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Connection oCon = new Connection();
        ///     string sConnectionString;
        ///     ...
        ///     OleDbCommand oCmd = new OleDbCommand(sSQL, oCon.GetConnection(sConnectionString));
        /// }
        /// </code>
        /// </example>
        /// <returns>OleDbConnection</returns>

        //public SqlConnection GetConnection(string sConnectionString)
        //{
        //    objErrorHandler = new ErrorHandler();
        //    try
        //    {
        //        smConnectionString = sConnectionString;
        //        OpenConnection();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //    }
        //    return smConnection;
        //}
     
        /// <summary>
        /// This is used to Open the Connection     
        /// </summary>
        /// <param></param>
        /// <returns>True or False</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// 
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Connection oCon = new Connection();
        ///     bool isConnected;
        ///     ...
        ///     isConnected = oCon.OpenConnection();
        /// }
        /// </code>
        /// </example>
        //public bool OpenConnection()
        //{
        //    bool isConnected = false;
        //    objErrorHandler = new ErrorHandler();
        //    try
        //    {
        //        //if (!(smConnection.State == System.Data.ConnectionState.Open))
        //        //{
                    
        //            smConnection.ConnectionString = smConnectionString;
        //             smConnection.Open();
        //            isConnected = true;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        isConnected = false;
        //        objErrorHandler.CreateLog();
        //    }
        //    return isConnected;
        //}
        /// <summary>
        /// This is used to close the Connection      
        /// </summary>
        /// <returns>True or False</returns>
        /// <example>
        /// <code>
        /// using System;
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// 
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Connection oCon = new Connection(); 
        ///     bool isClose;
        ///     ...
        ///     isClose = oCon.CloseConnection();
        /// } 
        /// </code>
        /// </example>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK DATABASE CONNECTION IS CLOSED OR NOT ***/
        /********************************************************************************/

        public bool CloseConnection( )
        {
            
            bool isClose;
            try
            {
                if (smConnection.State == ConnectionState.Open)
                {
                    smConnection.Close();
                    //smConnection.Dispose(); 
                    smConnection = null;
                }
                isClose = true;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                isClose = false;
            }
            return isClose;
        }
      
        #endregion

    }
    /*********************************** J TECH CODE ***********************************/
}
