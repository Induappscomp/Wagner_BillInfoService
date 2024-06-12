using System;
using System.IO;
using System.Net;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using System.Threading;

namespace TradingBell.WebCat.Helpers
{
    /*********************************** J TECH CODE ***********************************/
    public class Security
    {
        /*********************************** DECLARATION ***********************************/
        ErrorHandler objErrorHandler = new ErrorHandler();
        /*********************************** DECLARATION ***********************************/
        StreamWriter sw;
        Exception smException;
        string strlog;
        /*********************************** CONSTRUCTOR ***********************************/
        public Security()
        {
            //GetInitialDetails();
        }
        /*********************************** CONSTRUCTOR ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  ENCRYPT THE STRING USING TRIPLE DES  ***/
        /********************************************************************************/
        public string Encrypt(string Input, string key)
        {
            try
            {
                byte[] inputArray = UTF8Encoding.UTF8.GetBytes(Input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                //objErrorHandler.CreateLog();
                return "";
            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  DECRYPT THE STRING VALUE USING TRIPLE DES  ***/
        /********************************************************************************/
        public string Decrypt(string input, string key)
        {
            try
            {
                input = input.Replace(" ", "+");
                byte[] inputArray = Convert.FromBase64String(input);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return UTF8Encoding.UTF8.GetString(resultArray);
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
               // objErrorHandler.CreateLog();
                return "";
            }
        }

        ///<summary>
        ///  This is used to Decrypt the String
        /// </summary>
        /// <param name="DecryptStrValue">string</param>
        /// <returns>string</returns>
        /// <example>
        /// <code>

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  DECRYPT THE STRING   ***/
        /********************************************************************************/
        public string StringDeCrypt(string DecryptStrValue)
        {
            try
            {
                string Decryptext = Decrypt(DecryptStrValue, true,"");
                return Decryptext;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                //objErrorHandler.CreateLog();
                return null;
            }
        }
        public string StringDeCrypt_password(string DecryptStrValue)
        {
            try
            {
                string Decryptext = Decrypt_password(DecryptStrValue, true,"");
                return Decryptext;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                //objErrorHandler.CreateLog();
                return null;
            }
        }
        
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  DECRYPT THE STRING   ***/
        /********************************************************************************/
        public string StringDeCrypt(string DecryptStrValue,string key)
        {
            try
            {
                string Decryptext = Decrypt(DecryptStrValue, true, key);
                return Decryptext;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                //objErrorHandler.CreateLog();
                return null;
            }
        }
     
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  ENCRYPT THE STRING VALUE   ***/
        /********************************************************************************/
        public string StringEnCrypt(string EncryptStrValue)
        {
            try
            {
                string EncryText = Encrypt(EncryptStrValue, true,"");
                //GetEncryptengine Encryptor = new GetEncryptengine(Get_Encryption_Engine().CreateEncryptor);
                //string EncryText = Convert.ToBase64String(Transform(System.Text.Encoding.Default.GetBytes(EncryptStrValue), Encryptor()));
                return EncryText;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
               // objErrorHandler.CreateLog();
                return null;
            }
        }

        public string StringEnCrypt_password(string EncryptStrValue)
        {
            try
            {
                string EncryText = Encrypt_password(EncryptStrValue, true, "");
                //GetEncryptengine Encryptor = new GetEncryptengine(Get_Encryption_Engine().CreateEncryptor);
                //string EncryText = Convert.ToBase64String(Transform(System.Text.Encoding.Default.GetBytes(EncryptStrValue), Encryptor()));
                return EncryText;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                //objErrorHandler.CreateLog();
                return null;
            }
        }

        public string StringEnCryptWes(string EncryptStrValue)
        {
            try
            {
                string EncryText = Encrypt_password(EncryptStrValue, true, "");
                //GetEncryptengine Encryptor = new GetEncryptengine(Get_Encryption_Engine().CreateEncryptor);
                //string EncryText = Convert.ToBase64String(Transform(System.Text.Encoding.Default.GetBytes(EncryptStrValue), Encryptor()));
                return EncryText;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                // objErrorHandler.CreateLog();
                return null;
            }
        }

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  ENCRYPT THE STRING VALUE   ***/
        /********************************************************************************/
        public string StringEnCrypt(string EncryptStrValue,string key)
        {
            try
            {
                string EncryText = Encrypt(EncryptStrValue, true, key);
                //GetEncryptengine Encryptor = new GetEncryptengine(Get_Encryption_Engine().CreateEncryptor);
                //string EncryText = Convert.ToBase64String(Transform(System.Text.Encoding.Default.GetBytes(EncryptStrValue), Encryptor()));
                return EncryText;
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
               // objErrorHandler.CreateLog();
                return null;
            }
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  RETRIVE ENCRYPTION MODE,KEY DETAILS  ***/
        /********************************************************************************/
        public static System.Security.Cryptography.SymmetricAlgorithm Get_Encryption_Engine()
        {
            System.Security.Cryptography.SymmetricAlgorithm Encryption_engine;
            Encryption_engine = new System.Security.Cryptography.RijndaelManaged();
            Encryption_engine.Mode = System.Security.Cryptography.CipherMode.CBC;
            Encryption_engine.Key = Convert.FromBase64String("U1fknVDCPQWERTYGZfRqvAYCK7gFpUukYKOqsCuN8XU=");
            Encryption_engine.IV = Convert.FromBase64String("vEQWERTYRMrovjV+NXos5g==");
            return Encryption_engine;
        }
        public byte[] Transform(byte[] Source, System.Security.Cryptography.ICryptoTransform Transformer)
        {
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            System.Security.Cryptography.CryptoStream cryptographic_stream = new System.Security.Cryptography.CryptoStream(stream, Transformer, System.Security.Cryptography.CryptoStreamMode.Write);
            cryptographic_stream.Write(Source, 0, Source.Length);
            cryptographic_stream.FlushFinalBlock();
            cryptographic_stream.Close();
            return stream.ToArray();
        }


        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns></returns>
        /// 
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  ENCRYPT WITH KEY VALUE  ***/
        /********************************************************************************/
        public static string Encrypt(string toEncrypt, bool useHashing,string EnKey)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            string key = string.Empty;
            if (EnKey == string.Empty)
            {
                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                // Get the key from config file
                key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            }
            else
                key = EnKey;

            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        //Added by :Indu
        //Added on:16-Jan-2014
        //Reason:For Password Encryption
        public static string Encrypt_password(string toEncrypt, bool useHashing, string EnKey)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            string key = string.Empty;
            if (EnKey == string.Empty)
            {
                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                // Get the key from config file
                key = (string)settingsReader.GetValue("SecurityKeypassword", typeof(String));
            }
            else
                key = EnKey;

            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO  DECRYPT WITH KEY VALUE  ***/
        /********************************************************************************/
        public static string Decrypt(string cipherString, bool useHashing,string deKey)
        {
            //byte[] keyArray;
            //byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            //string key = "";
            //if (deKey == "")
            //{
            //    System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //    //Get your key from config file to open the lock!
            //    key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            //}
            //else
            //{
            //    key = deKey;
            //}
            //if (useHashing)
            //{
            //    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            //    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //    hashmd5.Clear();
            //}
            //else
            //    keyArray = UTF8Encoding.UTF8.GetBytes(key);

            //TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //tdes.Key = keyArray;
            //tdes.Mode = CipherMode.ECB;
            //tdes.Padding = PaddingMode.PKCS7;

            //ICryptoTransform cTransform = tdes.CreateDecryptor();
            //byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //tdes.Clear();
            //return UTF8Encoding.UTF8.GetString(resultArray);
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            string key = string.Empty;
            if (deKey == string.Empty)
            {
                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                //Get your key from config file to open the lock!
                key = settingsReader.GetValue("SecurityKey", typeof(String)).ToString() ;
            }
            else
            {
                key = deKey;
            }
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        public static string Decrypt_password(string cipherString, bool useHashing, string deKey)
        {
            
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            string key = string.Empty;
            if (deKey == string.Empty)
            {
                System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
                //Get your key from config file to open the lock!
                key = (string)settingsReader.GetValue("SecurityKeypassword", typeof(String));
            }
            else
            {
                key = deKey;
            }
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO REDIRECT PAGE URL INTO SECURE LAYERS ***/
        /********************************************************************************/
        public void RedirectSSL(HttpContext context, bool redirectWww, bool redirectSsl)
        {
            if (context == null)
                return;

            var redirect = false;
            var uri = context.Request.Url;
            var scheme = uri.GetComponents(UriComponents.Scheme, UriFormat.Unescaped);
            var host = uri.GetComponents(UriComponents.Host, UriFormat.Unescaped);
            var pathAndQuery = uri.GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);

            ////if (HttpContext.Current.Session != null && HttpContext.Current.Session.Count > 0)
            ////{
            ////    //if (string.IsNullOrEmpty(HttpContext.Current.Session["USER_ID"].ToString()) && Convert.ToInt32(HttpContext.Current.Session["USER_ID"]) > 0)
            ////    if (HttpContext.Current.Session["USER_ID"].ToString() != null && HttpContext.Current.Session["USER_ID"].ToString() != "" && Convert.ToInt32(HttpContext.Current.Session["USER_ID"].ToString()) > 0)
            ////    {
            ////        scheme = "https";
            ////        redirect = true;
            ////    }
            ////    else
            ////    {
            ////        if (redirectSsl == true && scheme.Equals("https") == false)
            ////        {
            ////            scheme = "https";
            ////            redirect = true;
            ////        }
            ////        if (redirectSsl == false && scheme.Equals("https") == true)
            ////        {
            ////            scheme = "http";
            ////            redirect = true;
            ////        }
            ////    }
            ////}
            ////else
            ////{
            ////    if (redirectSsl == true && scheme.Equals("https") == false)
            ////    {
            ////        scheme = "https";
            ////        redirect = true;
            ////    }
            ////    if (redirectSsl == false && scheme.Equals("https") == true)
            ////    {
            ////        scheme = "http";
            ////        redirect = true;
            ////    }

            ////}

            if ((redirectSsl) && !(scheme.Equals("https")))
            {
                scheme = "https";
                redirect = true;
            }
            if (!(redirectSsl) && (scheme.Equals("https")))
            {
                scheme = "http";
                redirect = true;
            }

            if (redirectWww && !host.StartsWith("www", StringComparison.OrdinalIgnoreCase))
            {
                host = "www." + host;
                redirect = true;
            }

            if (redirect)
            {
                context.Response.Status = "301 Moved Permanently";
                context.Response.StatusCode = 301;
                

                if ((pathAndQuery.ToLower().Contains("pl.aspx")) ||
           (pathAndQuery.ToLower().Contains("fl.aspx")) ||
            (pathAndQuery.ToLower().Contains("pd.aspx")) ||
           (pathAndQuery.ToLower().Contains("ct.aspx")) ||
           (pathAndQuery.ToLower().Contains("bb.aspx"))||
             (pathAndQuery.ToLower().Contains("ps.aspx"))
                    )
                {
                    string[] splitpathAndQuery = pathAndQuery.Split('?');
                    string newquerystring = "/" + splitpathAndQuery[1] + splitpathAndQuery[0].Replace(".aspx", "/");
                    context.Response.AddHeader("Location", scheme + "://" + host + newquerystring);
                
                }
                else
                {
                    context.Response.AddHeader("Location", scheme + "://" + host + pathAndQuery);
                   
                }
              
              
                
                //if (pathAndQuery.ToLower().Contains("ct.aspx"))
                //{
                //    pathAndQuery = pathAndQuery.ToLower().Replace("ct.aspx", "CategoryBr");
               //}
               // context.Response.AddHeader("Location", scheme + "://" + host + pathAndQuery);
            
            }
        }
        public object UrlEncode(object obj, Boolean Encrypt, Boolean urlencode)
        {
            object strreturn = "";
            strreturn = obj.ToString();
            if ((Encrypt))
                strreturn = StringEnCrypt((string)strreturn);

            if ((urlencode))
                strreturn = HttpUtility.UrlEncode((string)strreturn);
            return strreturn;
        }
        public object UrlDecode(object obj, Boolean Decrypt, Boolean urlDecode)
        {
            string strreturn = string.Empty;
            strreturn = obj.ToString();
            if ((urlDecode))
                strreturn = HttpUtility.UrlDecode(strreturn);
            if ((Decrypt))
                strreturn = StringDeCrypt(strreturn);

            return strreturn;
        }


        public Exception ErrorMsg
        {
            get
            {
                return smException;
            }
            set
            {
                smException = value;
            }
        }
        public string ExeTimelog
        {
            get
            {
                return strlog;
            }
            set
            {
                strlog = value;
            }
        }


        public void createexecutiontmielog()
        {
            try
            {
                Thread thlog = new Thread(new ThreadStart(CETLog));
                thlog.Start();

            }
            catch (Exception ex)
            {
                smException = ex;
                //oErrHand.CreateLog();
            }
        }

        public void CETLog()
        {
            try
            {
                AppDomain sPath;
                sPath = AppDomain.CurrentDomain;

                string FName = "LogTime/logtime" + DateTime.Now.ToShortDateString().Replace("/", "").Trim() + ".txt";
                string Path = sPath.BaseDirectory + FName;
                Path = Path.Replace("\\", "/");

                try
                {
                    bool rst = WriteTimeLog(Path);
                }
                catch
                {
                    FileStream fs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                    bool rst = WriteTimeLog(Path);
                }
                //if (File.Exists(Path) == false)
                //{

                //}

            }
            catch (Exception ex)
            {
                smException = ex;
                //oErrHand.CreateLog();
            }
        }

        public bool WriteTimeLog(string strPathName)
        {
            string strException = string.Empty;
            bool bReturn = false;
            try
            {
                sw = new StreamWriter(strPathName, true);
                sw.WriteLine("Estimated Time : " + strlog);
                sw.WriteLine("^^-------------------------------------------------------------------^^");
                sw.Flush();
                sw.Close();
                bReturn = true;
            }
            catch (Exception ex)
            {
                smException = ex;
                bReturn = false;
            }
            return bReturn;
        }


    }
    /*********************************** J TECH CODE ***********************************/
}