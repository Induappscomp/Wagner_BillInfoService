
namespace TradingBell.WebCat.CommonServices
{
    /*********************************** J TECH CODE ***********************************/
    public class UrlRewriteServices
    {
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  BUILD URL DETAILS ***/
        /********************************************************************************/
        public static string ConstructUrl(string sCustomUrlString)
        {

            //Generating custom url            
            string sRewrittenUrl = "";
            string sPageExtention = "";
            sPageExtention = System.Convert.ToString(System.Web.HttpContext.Current.Session["PageExtention"]);
            sRewrittenUrl = System.Web.HttpContext.Current.Request.ApplicationPath + "/" + sCustomUrlString + sPageExtention;
            //sRewrittenUrl = GetPageRoot() + "/" + sCustomUrlString + sPageExtention;
            return sRewrittenUrl;
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  GETTING PAGE ROOT DETAILS ***/
        /********************************************************************************/
        //getting page root
        public static string GetPageRoot()
        {
            string sRoot;
            //checking protocol
            string sProtocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (sProtocol == null || sProtocol == "0")
                sProtocol = "http://";
            else
                sProtocol = "https://";            
            
            //getting port address
            string sPortAddress = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (sPortAddress == null || sPortAddress == "80" || sPortAddress == "443")
                sPortAddress = "";
            else
                sPortAddress = ":" + sPortAddress;

            string sApplicationPath = System.Web.HttpContext.Current.Request.ApplicationPath;
            if (sApplicationPath == "/")
                sApplicationPath = "";

            sRoot = sProtocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + sPortAddress + sApplicationPath;
            return sRoot;
        }
    }
    /*********************************** J TECH CODE ***********************************/
}