using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.Common;
using TradingBell.WebCat.CatalogDB;
using TradingBell.WebCat.Helpers;
namespace TradingBell.WebCat.CommonServices
{ 
    /*********************************** J TECH CODE ***********************************/
    /// <summary>
    /// This is used to get all the Country and State Details
    /// </summary>
    /// <remarks>
    /// Used to get the Countries List,States List,Company Names,Tax for each State...
    /// </remarks>
    /// <example>
    /// Country oCountry = new Country();
    /// </example>

    public class CountryServices 
    {
        /*********************************** DECLARATION ***********************************/      
        HelperDB objHelperDB = new HelperDB();
        CountryDB objCountryDB = new CountryDB();
        ErrorHandler objErrorHandler = new ErrorHandler();
        HelperServices objHelperServices = new HelperServices();
        /*********************************** DECLARATION ***********************************/      
        #region "Tax Related Functions"

        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is Used to get the Countries list
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
        ///     Country oCountry = new Country();
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oCountry.GetCountries();
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/
        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public DataSet GetCountries()
        //{
        //    string sSQL;
        //    DataSet dsOD = new DataSet();
        //    try
        //    {
        //        //sSQL = "SELECT COUNTRY_NAME,COUNTRY_CODE FROM TBWC_COUNTRY ORDER BY COUNTRY_NAME";
        //        //oHelper.SQLString = sSQL;
        //        //return oHelper.GetDataSet("CountryDetails");
        //        dsOD = (DataSet)objCountryDB.GetGenericDataDB("", "GET_COUNTRIES", CountryDB.ReturnType.RTDataSet);
        //        if (dsOD != null)
        //            dsOD.Tables[0].TableName = "CountryDetails";
        //        else
        //            dsOD = null;

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        return null;
        //    }
        //    return dsOD;
        //}

        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE COUNTRY NAME AND COUNTRY CODE DETAILS  ***/
        /********************************************************************************/
        public DataSet GetCountries()
        {
           // string sSQL;
            DataSet dsOD = new DataSet();
            try
            {
                //sSQL = "SELECT COUNTRY_NAME,COUNTRY_CODE FROM TBWC_COUNTRY ORDER BY COUNTRY_NAME";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet("CountryDetails");
                dsOD = (DataSet)objCountryDB.GetGenericDataDB("", "GET_COUNTRIES", CountryDB.ReturnType.RTDataSet);
                if (dsOD != null)
                    dsOD.Tables[0].TableName = "CountryDetails";
                else
                    dsOD = null;
                     
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
            return dsOD;
        }
      
        /*********************************** OLD CODE ***********************************/
        /// <summary>
        /// This is used to get the Country name using Country Code
        /// </summary>
        /// <param name="CountryCode">Sring</param>
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
        ///     Country oCountry = new Country();
        ///     string CountCode;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oCountry.GetCountryName(CountCode);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/

        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public DataSet GetCountryName(string CountryCode)
        //{
        //    string sSQL;
        //    DataSet dsOS = new DataSet();
        //    try
        //    {
        //        //sSQL = "SELECT COUNTRY_NAME FROM TBWC_COUNTRY WHERE COUNTRY_CODE ='" + CountryCode + "'";
        //        //oHelper.SQLString = sSQL;
        //        //return oHelper.GetDataSet();
        //        return (DataSet)objCountryDB.GetGenericDataDB(CountryCode, "GET_COUNTRY_NAME", CountryDB.ReturnType.RTDataSet);

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        return null;
        //    }

        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE COUNTRY NAME  ***/
        /********************************************************************************/
        public DataSet GetCountryName(string CountryCode)
        {
          //  string sSQL;
            DataSet dsOS = new DataSet();
            try
            {
                //sSQL = "SELECT COUNTRY_NAME FROM TBWC_COUNTRY WHERE COUNTRY_CODE ='" + CountryCode + "'";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objCountryDB.GetGenericDataDB(CountryCode, "GET_COUNTRY_NAME", CountryDB.ReturnType.RTDataSet);

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
        /// This is used to get all States using Country Code
        /// </summary>
        /// <param name="CountryCode">string</param>
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
        ///     Country oCountry = new Country();
        ///     string CountryCode;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oCountry.GetStates("CountryCode");
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/
        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public DataSet GetStates(string CountryCode)
        //{
        //    string sSQL;
        //    try
        //    {
        //        //sSQL = "SELECT STATE_NAME,STATE_CODE,COUNTRY_CODE FROM TBWC_STATE WHERE COUNTRY_CODE ='" + CountryCode + "'";
        //        //oHelper.SQLString = sSQL;
        //        //return oHelper.GetDataSet();
        //        return (DataSet)objCountryDB.GetGenericDataDB(CountryCode, "GET_COUNTRY_STATE", CountryDB.ReturnType.RTDataSet);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        return null;
        //    }

        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE STATE NAME,STATE CODE,COUNTRY CODE DETAILS  ***/
        /********************************************************************************/
        public DataSet GetStates(string CountryCode)
        {
          //  string sSQL;
            try
            {
                //sSQL = "SELECT STATE_NAME,STATE_CODE,COUNTRY_CODE FROM TBWC_STATE WHERE COUNTRY_CODE ='" + CountryCode + "'";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objCountryDB.GetGenericDataDB(CountryCode, "GET_COUNTRY_STATE", CountryDB.ReturnType.RTDataSet);
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
        /// This is used to get the State Name using State Code
        /// </summary>
        /// <param name="StateCode">string</param>
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
        ///     Country oCountry = new Country();
        ///     string StateCode;
        ///     DataSet oDS = new DataSet();
        ///     ...
        ///     oDS = oCountry.GetStateName(StateCode);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/
        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public DataSet GetStateName(string StateCode)
        //{
        //    string sSQL;
        //    try
        //    {
        //        //sSQL = "SELECT STATE_NAME FROM TBWC_STATE WHERE STATE_CODE ='" + StateCode + "'";
        //        //oHelper.SQLString = sSQL;
        //        //return oHelper.GetDataSet();
        //        return (DataSet)objCountryDB.GetGenericDataDB(StateCode, "GET_STATE_NAME", CountryDB.ReturnType.RTDataSet);

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        return null;
        //    }
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE STATE NAME ***/
        /********************************************************************************/
        public DataSet GetStateName(string StateCode)
        {
           // string sSQL;
            try
            {
                //sSQL = "SELECT STATE_NAME FROM TBWC_STATE WHERE STATE_CODE ='" + StateCode + "'";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet();
                return (DataSet)objCountryDB.GetGenericDataDB(StateCode, "GET_STATE_NAME", CountryDB.ReturnType.RTDataSet);

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
        /// This is used to get the State Tax Value based on State Code
        /// </summary>
        /// <param name="CountryCode">string</param>
        /// <param name="StateCode">string</param>
        /// <returns>decimal</returns>
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
        ///     Country oCountry = new Country();
        ///     string CountryCode;
        ///     string StateCode;
        ///     decimal retVal;
        ///     ...
        ///     retVal = oCountry.GetStateTax(CountryCode, StateCode);
        /// }
        /// </code>
        /// </example>
        /*********************************** OLD CODE ***********************************/
        /*********************************** OLD CODE TRADING BELL ***********************************/
        //public decimal GetStateTax(string CountryCode, string StateCode)
        //{
        //    string sSQL;
        //    decimal retVal = 0;
        //    string tempstr = "";
        //    try
        //    {
        //        //sSQL = "SELECT TAX_AMOUNT FROM TBWC_TAX WHERE STATE_CODE ='" + StateCode + "'and COUNTRY_CODE ='" + CountryCode + "'";
        //        //oHelper.SQLString = sSQL;
        //        //retVal = oHelper.CDEC(oHelper.GetValue("TAX_AMOUNT"));
        //        tempstr = (string)objCountryDB.GetGenericDataDB("", StateCode, CountryCode, "GET_STATE_TAX", CountryDB.ReturnType.RTString);
        //        if (tempstr != null && tempstr != "")
        //            retVal = objHelperServices.CDEC(tempstr);


        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorHandler.ErrorMsg = ex;
        //        objErrorHandler.CreateLog();
        //        retVal = 0;
        //    }
        //    return retVal;
        //}
        /*********************************** OLD CODE TRADING BELL ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE STATE TAX AMOUNT DETAILS  ***/
        /********************************************************************************/
        public decimal GetStateTax(string CountryCode, string StateCode)
        {
          //  string sSQL;
            decimal retVal=0;
            string tempstr = "";
            try
            {
                //sSQL = "SELECT TAX_AMOUNT FROM TBWC_TAX WHERE STATE_CODE ='" + StateCode + "'and COUNTRY_CODE ='" + CountryCode + "'";
                //oHelper.SQLString = sSQL;
                //retVal = oHelper.CDEC(oHelper.GetValue("TAX_AMOUNT"));
                tempstr = (string)objCountryDB.GetGenericDataDB("", StateCode, CountryCode, "GET_STATE_TAX", CountryDB.ReturnType.RTString);
                if (tempstr != null && tempstr != "")
                    retVal = objHelperServices.CDEC(tempstr);  


            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                retVal = 0;
            }
            return retVal;
        }
        /*********************************** OLD CODE ***********************************/
        //:::                                                                         :::
        //:::  This routine calculates the distance between two points (given the     :::
        //:::  latitude/longitude of those points). It is being used to calculate     :::
        //:::  the distance between two ZIP Codes or Postal Codes using our           :::
        //:::  ZIPCodeWorld(TM) and PostalCodeWorld(TM) products.                     :::
        //:::                                                                         :::
        //:::  Definitions:                                                           :::
        //:::    South latitudes are negative, east longitudes are positive           :::
        //:::                                                                         :::
        //:::  Passed to function:                                                    :::
        //:::    lat1, lon1 = Latitude and Longitude of point 1 (in decimal degrees)  :::
        //:::    lat2, lon2 = Latitude and Longitude of point 2 (in decimal degrees)  :::
        //:::    unit = the unit you desire for results                               :::
        //:::           where: 'M' is statute miles                                   :::
        //:::                  'K' is kilometers (default)                            :::
        //:::                  'N' is nautical miles                                  :::
        //:::                                                                         :::
        //:::  United States ZIP Code/ Canadian Postal Code databases with latitude   :::
        //:::  & longitude are available at http://www.zipcodeworld.com               :::
        //:::                                                                         :::
        //:::                                                                         :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        /*********************************** OLD CODE ***********************************/

        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO GET THE DISTANCE BETWEEN ZIP CODES  ***/
        /********************************************************************************/
        public double GetDistanceUsingZip(string ZipCode)
        {
            double Distance=0;
            try
            {
                //string sSQL = "SELECT * FROM TBWC_ZIPCODES WHERE ZIPCODE='" + ZipCode + "'";
                //oHelper.SQLString = sSQL;
                DataSet oDSZip = new DataSet();
                //oDSZip = oHelper.GetDataSet();
                oDSZip = (DataSet)objCountryDB.GetGenericDataDB(ZipCode, "GET_ZIPCODE", CountryDB.ReturnType.RTDataSet);
                if (oDSZip != null)
                {
                    double DestLatitude = objHelperServices.CD(oDSZip.Tables[0].Rows[0]["Latitude"].ToString());
                    double DestLongitude = objHelperServices.CD(oDSZip.Tables[0].Rows[0]["Longitude"].ToString());
                    double SourceLatitude = 38.69067;
                    double SourceLongitude = -77.221509;
                    Distance = FindDistance(SourceLatitude, SourceLongitude, DestLatitude, DestLongitude, 'M');
                }
            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
            }
            return Distance;
        }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO FINDOUT THE DISTANCE USING LATITUDE AND LONGITUDE VALUES  ***/
        /********************************************************************************/
        private double FindDistance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CONVERT DECIMAL DEGREES TO RADIANS  ***/
        /********************************************************************************/
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO CONVERT RADIANS TO DECIMAL DEGREES ***/
        /********************************************************************************/
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }


        /*********************************************************************************/
        /*** ORGANIZATION : J TECH ***/
        /*** PURPOSE      : TO RETRIVE THE COUNTRY LIST   ***/
        /********************************************************************************/
        public DataSet GET_COUNTRIES_LIST(string str)
        {
            // string sSQL;
            DataSet dsOD = new DataSet();
            try
            {
                //sSQL = "SELECT COUNTRY_NAME,COUNTRY_CODE FROM TBWC_COUNTRY ORDER BY COUNTRY_NAME";
                //oHelper.SQLString = sSQL;
                //return oHelper.GetDataSet("CountryDetails");
                dsOD = (DataSet)objCountryDB.GetGenericDataDB(str, "GET_COUNTRIES_LIST", CountryDB.ReturnType.RTDataSet);
                if (dsOD != null)
                    dsOD.Tables[0].TableName = "Country";
                else
                    dsOD = null;

            }
            catch (Exception ex)
            {
                objErrorHandler.ErrorMsg = ex;
                objErrorHandler.CreateLog();
                return null;
            }
            return dsOD;
        }
      

        #endregion



    }
    /*********************************** J TECH CODE ***********************************/
}

