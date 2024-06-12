using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;

using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;

namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    /// <summary>
    ///  This is used to send the Mail from the Store to all the Shoppers and Company Admin.
    /// ie.,CC,BCC
    /// </summary>
    /// <remarks>
    /// Configure the Mail (Sending Message,ParseTemplateMessage,Template contents) 
    /// </remarks>
    /// <example>
    ///  Notification oNot = new Notification();
    /// </example>
    public class NotificationServices
    {
        /*********************************** DECLARATION ***********************************/      
        # region "Declarations..."
        SqlConnection _NotifyConnection;
        private ArrayList _NotifyTo = new ArrayList();
        private string _NotifyFrom = "";
        private ArrayList _NotifyCC = new ArrayList();
        private ArrayList _NotifyBCC = new ArrayList();
        private string _NotifySubject = "";
        private string _NotifyMessage = "";
        private string _SMTPServer = "";
        private ArrayList _AttachFile = new ArrayList();
        private string _UniqueStartSymbol = "{";
        private string _UniqueEndSymbol = "}";
        private bool _NotifyIsHTML;
        private string _SQLString = "";
        private string _UserName = "";
        private string _Password = "";
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperDB objHelperDB = new HelperDB();
        /*********************************** DECLARATION ***********************************/      
        #endregion

        #region "Properties..."

        /// <summary>
        /// Set the Property for NotifyConnection
        /// </summary>
        public SqlConnection NotifyConnection
        {
            get
            {
                return _NotifyConnection;
            }
            set
            {
                _NotifyConnection = value;
            }
        }
        /// <summary>
        /// Set the Property for NotifyTo
        /// </summary>
        public ArrayList NotifyTo
        {
            get
            {
                return _NotifyTo;
            }
            set
            {
                _NotifyTo = value;
            }

        }
        /// <summary>
        /// Set the Property for NotifyFrom
        /// </summary>
        public string NotifyFrom
        {
            get
            {
                return _NotifyFrom;
            }
            set
            {
                _NotifyFrom = value;
            }

        }
        /// <summary>
        /// Set the Property for NotifyCC
        /// </summary>
        public ArrayList NotifyCC
        {
            get
            {
                return _NotifyCC;
            }
            set
            {
                _NotifyCC = value;
            }

        }
        /// <summary>
        /// Set the Property for NotifyBCC
        /// </summary>

        public ArrayList NotifyBCC
        {
            get
            {
                return _NotifyBCC;
            }
            set
            {
                _NotifyBCC = value;
            }

        }
        /// <summary>
        /// Set the Property for NotifySubject
        /// </summary>

        public string NotifySubject
        {
            get
            {
                return _NotifySubject;
            }
            set
            {
                _NotifySubject = value;
            }

        }
        /// <summary>
        /// Set the Property for NotifyMessage
        /// </summary>
        public string NotifyMessage
        {
            get
            {
                return _NotifyMessage;
            }
            set
            {
                _NotifyMessage = value;
            }

        }
        /// <summary>
        /// Set the Property for SMTPServer
        /// </summary>
        public string SMTPServer
        {
            get
            {
                return _SMTPServer;
            }
            set
            {
                _SMTPServer = value;
            }

        }

        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }

        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }

        /// <summary>
        /// Set the Property for AttachFile
        /// </summary>
        public ArrayList AttachFile
        {
            get
            {
                return _AttachFile;
            }
            set
            {
                _AttachFile = value;
            }

        }
        /// <summary>
        /// Set the Property for NotifyIsHTML
        /// </summary>
        public bool NotifyIsHTML
        {
            get
            {
                return _NotifyIsHTML;
            }
            set
            {
                _NotifyIsHTML = value;
            }

        }
        /// <summary>
        /// Set the Property for UniqueStartSymbol
        /// </summary>
        public string UniqueStartSymbol
        {
            get
            {
                return _UniqueStartSymbol;
            }
        }
        /// <summary>
        /// Set the Property for UniqueEndSymbol
        /// </summary>
        public string UniqueEndSymbol
        {
            get
            {
                return _UniqueEndSymbol;
            }
        }

        #endregion
        
        #region "Functions..."
        /// <summary>
        /// Default Constructor
        /// </summary>

        /*********************************** CONSTRUCTOR ***********************************/  
        public NotificationServices()
        {

        }
        /*********************************** CONSTRUCTOR ***********************************/

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// Used to Build the Notification Mail Structure
        /// </summary>
        /// <returns>DAtaSet</returns>
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
        ///     Notification oNot = new Notification();
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oNot.BuildNotifyInfo();
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE NOTIFICATION DETAILS  ***/
        /********************************************************************************/
        public DataSet BuildNotifyInfo()
        {
            try
            {

                DataSet dsNotify = new DataSet();
                DataTable dtNotify = new DataTable("NotifyTable");

                DataColumn oCol = new DataColumn("ColumnKey", typeof(string));
                dtNotify.Columns.Add(oCol);

                oCol = new DataColumn("ColumnValue", typeof(string));
                dtNotify.Columns.Add(oCol);

                dsNotify.Tables.Add(dtNotify);
                return dsNotify;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// Parse the Mail Message
        /// </summary>
        /// <param name="sTemplateText">string</param>
        /// <param name="dsReplace">DataSet</param>
        /// <returns>string></returns>
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
        ///     Notification oNot = new Notification();
        ///     string sTemplate;
        ///     string sTemplateText;
        ///     DataSet dsReplace = new DataSet();
        ///     ...
        ///     sTemplateText = oNot.ParseTemplateMessage(sTemplate, dsReplace);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE MESSAGE CONTENT DETAILS ***/
        /********************************************************************************/
        public string ParseTemplateMessage(string sTemplateText, DataSet dsReplace)
        {
            foreach (DataRow oRow in dsReplace.Tables[0].Rows)
            {
                sTemplateText = sTemplateText.Replace(oRow[0].ToString(), oRow[1].ToString());
            }
            return sTemplateText;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// Used to Send the Mail Message
        /// </summary>
        /// <returns>integer</returns>
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
        ///     Notification oNot = new Notification();
        ///     int retValue;
        ///     ...
        ///     retValue = oNot.SendMessage();
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO SEND THE MAIL MESSAGE  ***/
        /********************************************************************************/
        public int SendMessage()
        {
            int retValue = 0;
            try
            {
                SmtpClient oSMTP = new SmtpClient();
                MailMessage oMail = new MailMessage();
                oMail.IsBodyHtml = _NotifyIsHTML;
                oMail.Body = _NotifyMessage;
                oMail.Subject = _NotifySubject;

                for (int iCnt = 0; iCnt < _NotifyBCC.Count; iCnt++)
                {
                    if (_NotifyBCC[iCnt].ToString() != "")
                    {
                        oMail.Bcc.Add(_NotifyBCC[iCnt].ToString());
                    }
                }
                for (int iCnt = 0; iCnt < _NotifyCC.Count; iCnt++)
                {
                    if (_NotifyCC[iCnt].ToString() != "")
                    {
                        oMail.CC.Add(_NotifyCC[iCnt].ToString());
                    }
                }
                for (int iCnt = 0; iCnt < _AttachFile.Count; iCnt++)
                {
                    oMail.Attachments.Add(Attachment.CreateAttachmentFromString(_AttachFile[iCnt].ToString(), "File1"));
                }
                for (int iCnt = 0; iCnt < _NotifyTo.Count; iCnt++)
                {
                    oMail.To.Add(_NotifyTo[iCnt].ToString());
                }
                if (_NotifyFrom != "")
                {
                    oMail.From = new MailAddress(_NotifyFrom);
                }
                
                System.Net.NetworkCredential basicAuthenticationInfo = new System.Net.NetworkCredential(UserName, Password);
                oSMTP.Host = _SMTPServer;
                oSMTP.UseDefaultCredentials = true;
                oSMTP.Credentials = basicAuthenticationInfo;
                oSMTP.Send(oMail);
                retValue = 1;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retValue = -1;
            }
            return retValue;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// Returns the Dataset for the Sql Query
        /// </summary>
        /// <returns>Dateset</returns>
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
        ///     Notification oNot = new Notification();
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oNot.GetDataSet();
        /// }
        /// </code>
        /// </example>
        /// 
        /*********************************** OLD CODE ***********************************/
        /*********************************** OLD CODE ***********************************/
        //public DataSet GetDataSetDB()
        //{
        //    DataSet oDs = new DataSet();
        //    try
        //    {
        //        OleDbDataAdapter oDA = new OleDbDataAdapter(_SQLString, _NotifyConnection);

        //        oDA.Fill(oDs);
        //        if (oDs.Tables[0].Rows.Count == 0)
        //        {
        //            oDs = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        oDs = null;
        //    }
        //    return oDs;
        //}
        
        
        /// <summary>
        /// Used to return the Coulumn Value based on the Column Name
        /// </summary>
        /// <param name="ColumnName">string</param>
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
        ///     Notification oNot = new Notification();
        ///     string ColumnName;
        ///     string rValue;
        ///     ...
        ///     rValue = oNot.GetColumnValue(ColumnName);
        /// }
        /// </code>
        /// </example>
        //public string GetColumnValueDB(string ColumnName)
        //{
        //    string rValue = "";
        //    DataSet oDs = new DataSet();
        //    try
        //    {
        //        OleDbDataAdapter oDA = new OleDbDataAdapter(_SQLString, _NotifyConnection);
        //        oDA.Fill(oDs);
        //        if (oDs != null)
        //            foreach (DataRow oRow in oDs.Tables[0].Rows)
        //            {
        //                if (oRow[ColumnName] != DBNull.Value)
        //                {
        //                    rValue = oRow[ColumnName].ToString();
        //                }
        //                else
        //                {
        //                    rValue = "-1";
        //                }
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        //oErrHand.CreateLog();
        //        rValue = "-1";
        //    }
        //    return rValue;
        //}
        /// <summary>
        /// Used to get the Email Content
        /// </summary>
        /// <param name="NotificationType">String</param>
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
        ///     Notification oNot = new Notification();
        ///     string RetVal;
        ///     ...
        ///     RetVal = oNot.GetTemplateContent("Registration");
        ///     // Here "Registration" is Notification Type
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE MAIL MESSAGE CONTENT,SUBJECT,STATUS DETAILS  ***/
        /********************************************************************************/
        public string GetTemplateContent(string NotificationType)
        {
            string RetVal = "";
            DataTable objdt = new DataTable(); 
            try
            {
                //string sSQL = " SELECT SCRIPT_CONTENT FROM TBWC_EMAIL_SCRIPT ";
                //sSQL = sSQL + " WHERE SCRIPT_NAME ='" + NotificationType + "'";
                //_SQLString = sSQL;
                //RetVal = GetColumnValueDB("SCRIPT_CONTENT");
                objdt = (DataTable)objHelperDB.GetGenericDataDB(NotificationType, "GET_NOTIFICATION_EMAIL_SCRIPT", HelperDB.ReturnType.RTTable);
                if (objdt != null && objdt.Rows.Count>0)
                    RetVal = objdt.Rows[0]["SCRIPT_CONTENT"].ToString() ; 
 
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                RetVal = "-1";
            }
            return RetVal;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// Used to return the Subject of the Mail id
        /// </summary>
        /// <param name="NotificationType">string</param>
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
        ///     Notification oNot = new Notification();
        ///     string retVal;
        ///     ...
        ///     retVal = oNot.GetEmailSubject("Registration");
        ///     // Here "Registration" is the Notification Type
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE MESSAGE SUBJECT DETAILS  ***/
        /********************************************************************************/
        public string GetEmailSubject(string NotificationType)
        {
            string retVal = "";
            DataTable objdt = new DataTable(); 
            try
            {
                //string sSQL = " SELECT SCRIPT_SUBJECT FROM TBWC_EMAIL_SCRIPT ";
                //sSQL = sSQL + " WHERE SCRIPT_NAME = '" + NotificationType + "'";
                //_SQLString = sSQL;
                //retVal = GetColumnValueDB("SCRIPT_SUBJECT");
                objdt = (DataTable)objHelperDB.GetGenericDataDB(NotificationType, "GET_NOTIFICATION_EMAIL_SCRIPT", HelperDB.ReturnType.RTTable);
                if (objdt != null && objdt.Rows.Count > 0)
                    retVal = objdt.Rows[0]["SCRIPT_SUBJECT"].ToString();
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retVal = "-1";
            }
            return retVal;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// Used to Check whether Email Active or Not
        /// </summary>
        /// <param name="NotificationType">string</param>
        /// <returns>bool</returns>
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
        ///     Notification oNot = new Notification();
        ///     bool isActive = false;
        ///     ...
        ///     isActive = oNot.IsNotificationActive("Registration");
        ///     // Here "Registration" is Notification Type
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK NOTIFICATION IS ACTIVE OR NOT  ***/
        /********************************************************************************/
        public bool IsNotificationActive(string NotificationType)
        {
            bool isActive = false;
            DataSet dsNA = new DataSet();
            DataTable objdt = new DataTable(); 
            try
            {
                //string sSQL = " SELECT SCRIPT_STATUS FROM TBWC_EMAIL_SCRIPT ";
                //sSQL = sSQL + " WHERE SCRIPT_NAME = '" + NotificationType + "'";
                //_SQLString = sSQL;
                //dsNA = GetDataSetDB();

                //if (dsNA != null)
                //{
                //    foreach (DataRow row in dsNA.Tables[0].Rows)
                //    {
                //        isActive = Convert.ToBoolean(row["SCRIPT_STATUS"]);
                //    }
                //}
                objdt = (DataTable)objHelperDB.GetGenericDataDB(NotificationType, "GET_NOTIFICATION_EMAIL_SCRIPT", HelperDB.ReturnType.RTTable);
                if (objdt != null && objdt.Rows.Count > 0)
                    isActive = Convert.ToBoolean(objdt.Rows[0]["SCRIPT_STATUS"]);

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                isActive = false;
            }
            return isActive;
        }

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// Used to Send the Mail Message in the HTML Format
        /// </summary>
        /// <param name="NotificationType">String</param>
        /// <returns>bool</returns>
        /// <example>
        /// <code> 
        /// using System.IO;
        /// using System.Web;
        /// using System.Data;
        /// using TradingBell.Common;
        /// using TradingBell.WebServices;
        /// 
        /// protected void Page_Load(object sender, EventArgs e)
        /// {
        ///     Notification oNot = new Notification();
        ///     bool isHTML = false;
        ///     ... 
        ///     isHTML = oNot.IsHTMLNotification("Registration");
        ///     // Here "Registration" is Notification Type
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CHECK THE SCRIPT NOTIFICATION TYPE WEATHER ITS HTML OR NOT  ***/
        /********************************************************************************/
        public bool IsHTMLNotification(string NotificationType)
        {
            bool isHTML = false;
            DataSet dsHMTL = new DataSet();
            DataTable objdt = new DataTable(); 
            try
            {
                // Old Code commant by Jtech
                //string sSQL = " SELECT SCRIPT_ISHTML FROM TBWC_EMAIL_SCRIPT ";
                //sSQL = sSQL + " WHERE SCRIPT_NAME = '" + NotificationType + "'";
                //_SQLString = sSQL;
                //dsHMTL = GetDataSetDB();

                //if (dsHMTL != null)
                //{
                //    foreach (DataRow row in dsHMTL.Tables[0].Rows)
                //    {
                //        isHTML = Convert.ToBoolean(row["SCRIPT_ISHTML"]);
                //    }
                //}

                objdt = (DataTable)objHelperDB.GetGenericDataDB(NotificationType, "GET_NOTIFICATION_EMAIL_SCRIPT", HelperDB.ReturnType.RTTable);
                if (objdt != null && objdt.Rows.Count > 0)
                    isHTML = Convert.ToBoolean(objdt.Rows[0]["SCRIPT_ISHTML"]);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                isHTML = false;
            }
            return isHTML;
        }

        #endregion

    }
    /*********************************** J TECH CODE ***********************************/
}