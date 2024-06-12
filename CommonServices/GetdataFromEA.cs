using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Text;
using TradingBell.WebCat.Helpers;
//Created By:Indu
//Reason:To get page title and meta keyword
namespace TradingBell.WebCat.CommonServices
{
 public    class GetmetadataFromEA
    {

       string stitle = string.Empty;
       string urlstring = string.Empty; 
        string skeyword = string.Empty;
        string productcode = string.Empty;
        string sfamily = string.Empty;
        string scategory = string.Empty;
        string sattrvalue = string.Empty;
   
        public string FetchData(DataSet ds)
        {
            try
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                   if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "category")
                      {
                          //if (i >= 0)
                          //{
                              scategory = ds.Tables[0].Rows[i]["Itemvalue"].ToString();
                              if (stitle == string.Empty)
                              {
                                  stitle = scategory;
                              }
                       //Modified by Indu on 16-july-2018
                       //To get last category level

                              else
                              {
                                  //stitle = scategory + ", " + stitle;
                                  stitle = scategory;
                              }

                              if (skeyword == string.Empty)
                              {
                                  skeyword = scategory;
                              }
                              else
                              {
                                  skeyword = skeyword + "," + scategory;

                              }   

                              if (urlstring == string.Empty)
                              {
                                 
                                  urlstring = scategory;
                              }
                              else
                              {

                                  urlstring = scategory + "/" + urlstring;
                              }

                          //}
                      }

                        else  if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "family")
                    {


                        sfamily = ds.Tables[0].Rows[i]["FamilyName"].ToString();

                        if (stitle == string.Empty)
                        {
                            stitle = sfamily;
                        }
                        else
                        {
                            stitle = sfamily + ", " + stitle;

                        }

                        if (skeyword == string.Empty)
                        {
                            skeyword = sfamily;
                        }
                        else
                        {
                            skeyword = skeyword + "," + sfamily;

                        }
                        if (urlstring == string.Empty)
                        {

                            urlstring = sfamily;
                        }
                        else
                        {

                            urlstring = sfamily + "/" + urlstring;
                        }

                    }
                else if (ds.Tables[0].Rows[i]["ItemType"].ToString().ToLower() == "product")
                    {
                        productcode = ds.Tables[0].Rows[i]["Productcode"].ToString();


                        //if (stitle == string.Empty)
                        //{
                        //    stitle = productcode;
                        //}
                        //else
                        //{
                        //    stitle = productcode + ", " + stitle;

                        //}

                        if (skeyword == string.Empty)
                        {
                            skeyword = productcode;
                        }
                        else
                        {
                            skeyword = skeyword + "," + productcode;

                        }

                        if (urlstring == string.Empty)
                        {

                            urlstring = productcode;
                        }
                        else
                        {

                            urlstring = productcode + "/" + urlstring;
                        }
                    }

                    else 
                    {
                        sattrvalue = ds.Tables[0].Rows[i]["Itemvalue"].ToString();
                        if (stitle == string.Empty)
                        {
                            stitle = sattrvalue;
                        }
                        else
                        {
                            stitle = sattrvalue + ", " + stitle;

                        }

                        if (skeyword == string.Empty)
                        {
                            skeyword = sattrvalue;
                        }
                        else
                        {
                            skeyword = skeyword + "," + sattrvalue;

                        }

                        if (urlstring == string.Empty)
                        {

                            urlstring = sattrvalue;
                        }
                        else
                        {

                            urlstring = sattrvalue + "/" + urlstring;
                        }
                    }
                   
                }
               // stitle = stitle.Replace('-', ' '); 
                return stitle + "|" + skeyword + "|" + urlstring;
            }

            catch
            {
                return string.Empty; 
            }

        }
        public string Replace_SpecialChar(string value)
        {
            StringBuilder sb_value = new StringBuilder(value);

            sb_value.Replace("<bol>", "");
            sb_value.Replace("</bol>", "");
            sb_value.Replace("<", "");
            sb_value.Replace(">", "");
            sb_value.Replace("\n", " ");
            sb_value.Replace("&", "and");
            sb_value.Replace("\r", "");
            sb_value.Replace("..", ".");
            sb_value.Replace("  ", " ");
            sb_value.Replace("<ars>g</ars>", "-");
            sb_value.Replace("@", " ");
            sb_value.Replace("$", " ");
            sb_value.Replace(">", " ");
            sb_value.Replace("<", " ");
            sb_value.Replace("^", " ");
            sb_value.Replace("#", " ");
            sb_value.Replace("<-", " ");
            sb_value.Replace("->", " ");
            sb_value.Replace("!", " ");
            sb_value.Replace("~", " ");
            sb_value.Replace("%", " ");
            sb_value.Replace("®", " ");
            sb_value.Replace(",", ", ");
            sb_value.Replace(",  ",", ");
            sb_value.Replace("?", " ");
            sb_value.Replace("`", " ");
            sb_value.Replace("arsg/ars", "-");
            sb_value.Replace("--","-");
            sb_value.Replace("amp;","");
            return sb_value.ToString();
        }
        public string Replace_SpecialChar_meta(string value)
        {
            StringBuilder sb_value = new StringBuilder(value);

            sb_value.Replace("<bol>", "");
            sb_value.Replace("</bol>", "");
            sb_value.Replace("<", "");
            sb_value.Replace(">", "");
            sb_value.Replace("\n", " ");
            sb_value.Replace("&", "and");

            sb_value.Replace("<ars>g</ars>", "-");

            sb_value.Replace("amp;", "");
            return sb_value.ToString();
        }
        public string SetTitleCount(string title)
        {
            ErrorHandler objErrorHandler = new ErrorHandler();
            string compname = " | Wagner Online Electronic Stores ";
            try
            {
                
                if (title.Length > 54)
                {
                    title = title.Substring(0, 54).ToString();


                    try
                    {
                        title = title.Substring(0, title.LastIndexOf(","));
                    }
                    catch 
                    {
                       // objErrorHandler.CreateLog("SetTitleCount"+ex.ToString()); 
                    }
                    title = title + compname;
                }
                else
                {
                    if (title.ToString().ToLower() == "personal protective equipment ppe")
                    {
                        title += " | Non Contact Thermometers, Face Masks, Hand Sanitisers";
                    }
                    title = title + compname;
                }
               
            }
            catch (Exception ex)
            {
  //objErrorHandler.CreateLog("SetTitleCount"+ex.ToString()); 
                return "";
            }
            return title;
        }

    }
}
 