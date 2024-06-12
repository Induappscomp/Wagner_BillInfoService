using System;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    /// <summary>
    /// This is used to notify some variables to construct the order notification replace variables
    /// </summary>
    /// <remarks>These variables are used to construct the order notification replace variables.</remarks>
    /// <example>
    /// NotificationVariables oNotVar=new NotificationVariables();
    /// </example>

    public class NotificationVariablesServices
    {
        /*********************************** CONSTRUCTOR ***********************************/  
        /// <summary>
        /// Default Constructor
        /// </summary>
        public NotificationVariablesServices()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        /*********************************** CONSTRUCTOR ***********************************/  

        /*********************************** DECLARATION ***********************************/
        # region "Enum Variables"
        /// <summary>
        /// Create the Enum for the Order Receipt for Notifications
        /// </summary>
        /// <example>
        /// NotificationVariables.OrderReceipt.FROMCONTENT.ToString()
        /// </example>    

        public enum OrderReceipt
        {
            /// <summary>
            /// Sender Mail Identification
            /// </summary>
            FROMCONTENT = 1,
            /// <summary>
            /// Receiver Mail Identification
            /// </summary>
            TOCONTENT = 2,
            /// <summary>
            /// User's First Name
            /// </summary>
            FIRSTNAME = 3,
            /// <summary>
            /// Purchased Date of the Order
            /// </summary>
            ORDERDATE = 4,
            /// <summary>
            /// Order Number
            /// </summary>
            //ORDERNO = 5,
            ORDERID = 5,
            /// <summary>
            /// Build the Structure or Format of the Mail
            /// </summary>
            CONSTRUCTTABLE = 6,
            /// <summary>
            /// Display Sub Total of the Order
            /// </summary>
            SUBTOTAL = 7,
            /// <summary>
            /// Tax for the Order
            /// </summary>
            TAX = 8,
            /// <summary>
            /// Shipping Cost
            /// </summary>
            SHIPCHARGES = 9,
            /// <summary>
            /// Net Amount includes Tax + Ship Cost + SubTotal
            /// </summary>
            TOTAL = 10,
            /// <summary>
            /// Discount Amount
            /// </summary>
            DISCOUNT = 11
        }
        /// <summary>
        /// Create the Enum for the Notification List in Notifications
        /// </summary>
        /// <example>
        /// NotificationVariables.NotificationList.FORGETPASSWORD.ToString();
        /// </example>
        public enum NotificationList
        {
            /// <summary>
            /// Format for USER REGISTRATION
            /// </summary>
            USERREGISTRATION = 1,
            /// <summary>
            /// Format for ACCOUNT ACTIVATION
            /// </summary>
            ACCOUNTACTIVATE = 2,
            /// <summary>
            /// Format for ACCOUNT DEACTIVATION
            /// </summary>
            ACCOUNTDEACTIVATE = 3,
            /// <summary>
            /// Format for FORGET PASSWORD
            /// </summary>
            FORGETPASSWORD = 4,
            /// <summary>
            /// Format for RESPONSE FOR FORGET PASSWORD
            /// </summary>
            RESPONSEFORGETPASSWORD = 5,
            /// <summary>
            /// Format for NEW ORDER
            /// </summary>
            NEWORDER = 6,
            /// <summary>
            /// Format for TRANSIT ORDER
            /// </summary>
            SHIPPEDDORDER = 7,
            /// <summary>
            /// Format for CANCEL ORDER
            /// </summary>
            CANCELORDER = 8,
            /// <summary>
            /// Request the Quote
            /// </summary>
            REQUESTEDQUOTE = 9,
            /// <summary>
            /// Response for the Requested Quote
            /// </summary>
            RESPONSEDQUOTE = 10,
            /// <summary>
            /// Format for USER REGISTRATION
            /// </summary>
            CUSTOMERREGISTRATION = 11,
            /// <summary>
            /// Format for RESET PASSWORD
            /// </summary>
            RESETPASSWORD = 12
        }
        /// <summary>
        /// Create the Enum for the Quote Receipt for Notifications
        /// </summary>
        /// <example>
        /// NotificationVariables.QuoteReceipt.FROMCONTENT.ToString()
        /// </example>    
        public enum QuoteReceipt
        {
            FROMCONTENT = 1,
            TOCONTENT = 2,
            //FIRSTNAME = 3,
            FULLNAME = 3,
            QUOTEDATE = 4,
            QUOTENO = 5,
            CONSTRUCTTABLE = 6,
            PRODTOTALCOST = 7,
            TOTAL = 8
        }
        /// <summary>
        /// Create the Enum for the User Registration in Notifications
        /// </summary>
        /// <example>
        /// NotificationVariables.UserRegistration.USERID.ToString() 
        /// </example>
        public enum UserRegistration
        {
            /// <summary>
            /// User's First Name
            /// </summary>
            FIRSTNAME = 1,
            /// <summary>
            /// User's Last Name
            /// </summary>
            LASTNAME = 2,
            /// <summary>
            /// User's Identification Number
            /// </summary>
            USERID = 3,
            /// <summary>
            /// User's Password
            /// </summary>
            PASSWORD = 4,
            /// <summary>
            /// Company Address
            /// </summary>
            COMPANYADDRESS = 5,
            /// <summary>
            /// Company URL Link
            /// </summary>
            COMPANYURL = 6
        }
        /// <summary>
        /// Create the Enum for the Forgot Password in Notifications 
        /// </summary>
        /// <example>
        /// NotificationVariables.ForgotPassword.USERID.ToString()
        /// </example>
        public enum ForgotPassword
        {
            /// <summary>
            /// Users First Name + Last Name = FullName
            /// </summary>
            FULLNAME = 1,
            /// <summary>
            /// Users Identification Number
            /// </summary>
            USERID = 2,
            /// <summary>
            /// Users New Password
            /// </summary>
            NEWPASSWORD = 3

        }

        /// <summary>
        /// Create the Enum for the Forgot Password in Notifications 
        /// </summary>
        /// <example>
        /// NotificationVariables.ForgotPassword.USERID.ToString()
        /// </example>
        public enum ResetPassword
        {
            /// <summary>
            /// Users First Name + Last Name = FullName
            /// </summary>
            FULLNAME = 1,
            /// <summary>
            /// Users Identification Number
            /// </summary>
            USERID = 2,
            /// <summary>
            /// Users New Password
            /// </summary>
            NEWPASSWORD = 3,
            /// <summary>
            /// Users Reset Link
            /// </summary>
            PasswordLink = 3

        }

        /// <summary>
        /// Create the Enum for the Customer Registration in Notifications
        /// </summary>
        /// <example>
        /// NotificationVariables.CustomerRegistration
        /// </example>
        public enum CustomerRegistration
        {
            /// <summary>
            /// Customer Name
            /// </summary>
            COMPANYNAME = 1,
            /// <summary>
            /// Customer's First Name
            /// </summary>
            FIRSTNAME = 2,
            /// <summary>
            /// Customer's Last Name
            /// </summary>
            LASTNAME = 3,
            /// <summary>
            /// Customer Address
            /// </summary>
            CUSTOMERADDRESS = 4,
            /// <summary>
            /// Customer Email Link
            /// </summary>
            CUSTOMEREMAIL = 5
        }
        #endregion
        /*********************************** DECLARATION ***********************************/
    }
    /*********************************** J TECH CODE ***********************************/
}