using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebCat.CommonServices
{

    /*********************************** J TECH CODE ***********************************/
    /// <summary>
    /// This is used to get all User's Login Details
    /// </summary>
    /// <remarks>
    /// Used to Track the User Session
    /// </remarks>
    /// <example>
    /// UserSession UserSess=new UserSession();
    /// </example>
   
    public class UserSessionServices
    {
        /*********************************** DECLARATION ***********************************/
        HelperDB objHelperDB = new HelperDB();
        UserServices objUserServices = new UserServices();
        ErrorHandler objErrorHandler = new ErrorHandler();
        /*********************************** DECLARATION ***********************************/
        /// <summary>
        /// Default Constructor
        /// </summary>
        /*********************************** CONSTRUCTOR ***********************************/
        public UserSessionServices()
        {
        }
        /*********************************** CONSTRUCTOR ***********************************/
        /// <summary>
        /// Create the Structure for manipulating User session Information
        /// </summary>
        /// <example>
        /// UserSession.UserSessionInfo oUSI ;
        /// oUSI.Session_ID;
        /// </example>
        public struct UserSessionInfo
        {
            /// <summary>
            /// Session ID
            /// </summary>
            public string Session_ID;
            /// <summary>
            ///  Last IP Address
            /// </summary>
            public string Last_IP;
            /// <summary>
            /// Session Data
            /// </summary>
            public string Session_Data;
            /// <summary>
            /// Referal URL
            /// </summary>
            public string Referal_URL;
            /// <summary>
            /// User ID
            /// </summary>
            public int User_ID;

        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to insert the track session details
        /// </summary>
        /// <param name="oUSI">UserSessionInfo</param>
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
        ///     UserSession oUS = new UserSession();
        ///     UserSession.UserSessionInfo oUSI = new UserSession.UserSessionInfo();
        ///     int TrkSession;
        ///     ...
        ///     TrkSession = oUS.TrackSession(oUSI);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO INSERT AND SAVE USER SESSION DETAILS  ***/
        /********************************************************************************/
        public int TrackSession(UserSessionInfo oUSI)
        {
            try
            {
                //string sSQL = " INSERT INTO TBWC_USER_SESSION(";
                //sSQL = sSQL + " SESSION_ID,LAST_IP_ADDRESS,";
                //sSQL = sSQL + " SESSION_DATA,";
                //sSQL = sSQL + " REFERAL_URL,CREATED_USER,";
                //sSQL = sSQL + " USER_ID) VALUES ('";
                //sSQL = sSQL + oUSI.Session_ID + "','" + oUSI.Last_IP + "','";
                //sSQL = sSQL + oUSI.Session_Data + "','";
                //sSQL = sSQL + oUSI.Referal_URL + "'," + TradingBell.Common.User._StoreAdminID + ",";
                //sSQL = sSQL + oUSI.User_ID + ")";
                //oHelper.SQLString = sSQL;
                //return oHelper.ExecuteSQLQuery();

                string sSQL = " Exec STP_TBWC_POP_SAVE_USER_SESSION '";                
                sSQL = sSQL + oUSI.Session_ID + "','" + oUSI.Last_IP + "','";
                sSQL = sSQL + oUSI.Session_Data + "','";
                sSQL = sSQL + oUSI.Referal_URL + "'," + UserServices._StoreAdminID + ",";
                sSQL = sSQL + oUSI.User_ID;

                return objHelperDB.ExecuteSQLQueryDB(sSQL);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return -1;
            }

        }

    }
    /*********************************** J TECH CODE ***********************************/
}
