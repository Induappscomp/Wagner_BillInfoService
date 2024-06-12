namespace TradingBell.WebCat.EasyAsk.Impl
{
    using TradingBell.WebCat.EasyAsk;
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using System.IO;
    using System.Data;
    internal class RemoteResults : INavigateResults
    {
        private static readonly string ATTR_ATTR_TYPE = "EA_AttrType";
        private static readonly string ATTR_ERROR = "EA_Error";
        private static readonly string ATTR_INIT_DISP_LIMIT = "EA_InitDispLimit";
        private static readonly string ATTR_IS_COMMAND = "EA_IsCommand";
        private static readonly string ATTR_IS_INIT_DISP_LIMITED = "EA_IsInitDispLimited";
        private static readonly string ATTR_OUTPUT_ENGINE = "EA_OutputEngine";
        private static readonly string ATTR_PRESENTATION = "EA_Presentation";
        private static readonly char COMMENTARY_SECTION_END = ';';
        private static readonly string[] CORRECTION_SEP = new string[] { " is " };
        private static readonly char[] LIST_SEP = new char[] { ',' };
        private List<string> m_arrangeByChoices = null;
        private AttributesInfo m_attrsInfo = null;
        private BreadCrumbTrail m_bct = null;
        private bool m_bHierarachyProcessed = false;
        private List<ICarveOut> m_carveOuts = null;
        private CategoriesInfo m_catInfo = null;
        private string m_catPath = null;
        private string m_commentary = null;
        private IList<AttributeInfo> m_commonAttributes = null;
        private DisplayFormat m_displayFormat = null;
        private XmlDocument m_doc = new XmlDocument();
        private IFeaturedProducts m_featuredProducts = null;
        private GroupedSetInfo m_groupSet = null;
        private bool m_isGrouped = false;
        private ItemDescriptions m_itemDescriptions = null;
        private IList<ItemRow> m_items = null;
        private NavigateHierarchy m_navHier = null;
        private static readonly string NODE_ATTRIB_SELECT = "////AttribSelect=";
        private static readonly string PATH_ATTRIBUTES = "/EA_Results/EA_Source/EA_Attributes";
        private static readonly string PATH_CATEGORIES = "/EA_Results/EA_Source/EA_Categories";
        private static readonly string PATH_COMMENTARY = "/EA_Results/EA_Source/EA_Commentary";
        private static readonly string PATH_COMMON_ATTRIBUTES = "/EA_Results/EA_Source/EA_CommonAttributes";
        private static readonly string PATH_DISPLAY_FORMAT = "/EA_Results/EA_Source/EA_DisplayFormat";
        private static readonly string PATH_ERROR_MSG = "/EA_Results/EA_ErrorMsg";
        private static readonly string PATH_NAVIGATE_HIERARCHY = "/EA_Results/EA_Source/EA_NavigateHierarchy/EA_NavHierNode";
        private static readonly string PATH_PURE_CAT_PATH_NODE = "/EA_Results/EA_Source/EA_NavPath/EA_NavPathNodeList/EA_NavPathNode[last()]/EA_PurePath";
        private static readonly string PATH_RETURN_CODE = "/EA_Results/EA_ReturnCode";
        private static readonly string PATH_SOURCE = "/EA_Results/EA_Source";
        private static readonly string PATH_TO_BREADCRUMB = "/EA_Results/EA_Source/EA_NavPath";
        private static readonly string PATH_TO_CARVEOUTS = "/EA_Results/EA_Source/EA_CarveOuts/EA_CarveOut";
        private static readonly string PATH_TO_FEATURED_PRODUCTS = "/EA_Results/EA_Source/EA_FeaturedProducts";
        private static readonly string PATH_TO_GROUPS = "/EA_Results/EA_Source/EA_Products/EA_Groups";
        private static readonly string PATH_TO_ITEM = "/EA_Results/EA_Source/EA_Products/EA_Item";
        private static readonly string PATH_TO_ITEM_DESC = "/EA_Results/EA_Source/EA_Products/EA_ItemDescription";
        private static readonly string PATH_TO_SOURCE = "/EA_Results/EA_Source";
        private static readonly string RELATIVE_PATH_ALL_CATS = "EA_CategoryList/EA_Category";
        private static readonly string RELATIVE_PATH_AT_TOP_NODE = "EA_AtTopNode";
        private static readonly string RELATIVE_PATH_ATTRIBUTE = "EA_Attribute";
        private static readonly string RELATIVE_PATH_ATTRIBUTE_NAME = "EA_AttributeName";
        private static readonly string RELATIVE_PATH_ATTRIBUTE_RETRIEVAL_METHOD = "EA_AttributeRetrievalMethod";
        private static readonly string RELATIVE_PATH_DISPLAY_FORMAT = "EA_DisplayFormat";
        private static readonly string RELATIVE_PATH_DISPLAY_STYLE = "EA_DisplayStyle";
        private static readonly string RELATIVE_PATH_GLOBAL_SEARCH = "EA_ProductsFromGlobalSearch";
        private static readonly string RELATIVE_PATH_INITIAL_CATS = "EA_InitialCategoryList/EA_Category";
        private static readonly string RELATIVE_PATH_INITIAL_NAME_ORDER = "EA_InitialAttrNameOrder";
        private static readonly string RELATIVE_PATH_MODIFY_QUERY = "EA_ItemsFoundByModifyingQuery";
        private static readonly string RELATIVE_PATH_PRODUCT_RETRIEVAL_METHOD = "EA_ProductRetrievalMethod";
        private static readonly string RELATIVE_PATH_QUESTION = "EA_Question";
        private static readonly string RELATIVE_PATH_REQUESTED_DISPLAY_STYLE = "EA_RequestedDisplayStyle";
        private static readonly string RELATIVE_PATH_REQUESTED_OUTPUT_ENGINE = "EA_RequestedOutputEngine";
        private static readonly string RELATIVE_PATH_SECONDARY_SEARCH = "EA_ItemsFoundWithSecondarySearch";
        private static readonly string RELATIVE_PATH_SUGGESTED_TITLE = "EA_SuggestedCategoryTitle";
        private static readonly string RELAXATION_PREFACE = "Ignored:";
        private static readonly string SPELL_CORRECTION_PREFACE = "Corrected Words:";
        private static readonly string[] splitValSep = new string[] { ";;;;" };
        private DataSet dbAdvisor = new DataSet();

        internal RemoteResults()
        {
        }
        internal void load(String url)
        {
            m_doc.Load(url);
            determineLayout();
        }
        internal void load(Stream inp)
        {
            m_doc.Load(inp);
            determineLayout();
        }
        public void SetDBAdvisor(DataSet db)
        {
            dbAdvisor = db;
        }
        public DataSet GetDBAdvisor()
        {
            return dbAdvisor;
        }
        private void determineLayout()
        {
            this.m_isGrouped = null != this.m_doc.SelectSingleNode(PATH_TO_GROUPS);
        }


        public IList<string> getArrangeByChoices()
        {
            if (null == this.m_arrangeByChoices)
            {
                this.m_arrangeByChoices = new List<string>();
                IList<INavigateCategory> list = this.getDetailedCategories();
                if (0 < list.Count)
                {
                    this.m_arrangeByChoices.Add("Category");
                }
                IList<string> list2 = this.getAttributeNames();
                foreach (string str in this.getAttributeNames())
                {
                    IList<INavigateAttribute> list3 = this.getDetailedAttributeValues(str);
                    if (1 < list3.Count)
                    {
                        this.m_arrangeByChoices.Add(str);
                    }
                    else if ((1 == list3.Count) && (list3[0].getProductCount() < this.getTotalItems()))
                    {
                        this.m_arrangeByChoices.Add(str);
                    }
                }
                this.m_arrangeByChoices.TrimExcess();
            }
            return this.m_arrangeByChoices;
        }

        public bool getAtTopNode()
        {
            return DOMUtilities.getBoolean(this.m_doc.SelectSingleNode(PATH_TO_SOURCE), RELATIVE_PATH_AT_TOP_NODE);
        }

        private IList<AttributeInfo> getAttributeInfo(XmlNode attrNode)
        {
            if (null != attrNode)
            {
                XmlNodeList list2 = attrNode.SelectNodes(RELATIVE_PATH_ATTRIBUTE);
                IList<AttributeInfo> list = new List<AttributeInfo>(list2.Count);
                foreach (XmlNode node in list2)
                {
                    list.Add(new AttributeInfo(node));
                }
                return list;
            }
            return new List<AttributeInfo>(0);
        }

        public IList<string> getAttributeNames()
        {
            return this.getAttributeNames(EasyAskConstants.ATTR_FILTER_ALL, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        }

        public IList<string> getAttributeNames(int attrFilter, int displayMode)
        {
            this.processAttributes();
            return this.m_attrsInfo.getAttributeNames(attrFilter, displayMode);
        }

        public int getAttributeRetrievalMethod()
        {
            return DOMUtilities.getInteger(this.m_doc.SelectSingleNode(PATH_TO_SOURCE), RELATIVE_PATH_ATTRIBUTE_RETRIEVAL_METHOD);
        }

        public IBreadCrumbTrail getBreadCrumbTrail()
        {
            this.processBreadCrumbTrail();
            return this.m_bct;
        }

        public IList<ICarveOut> getCarveOuts()
        {
            if (null == this.m_carveOuts)
            {
                this.m_carveOuts = new List<ICarveOut>();
                foreach (XmlNode node in this.m_doc.SelectNodes(PATH_TO_CARVEOUTS))
                {
                    this.m_carveOuts.Add(new CarveOut(this, node));
                }
                this.m_carveOuts.TrimExcess();
            }
            return this.m_carveOuts;
        }

        public string getCatPath()
        {
            if (null == m_catPath)
            {
                XmlNode purePath = m_doc.SelectSingleNode(PATH_PURE_CAT_PATH_NODE);
                if (null != purePath)
                {
                    m_catPath = purePath.InnerText;
                }
                else
                {
                    m_catPath = "All Products";
                }
            }
            return m_catPath;
            //return this.m_catPath;
        }
        public string getCatPathJson()
        {
            if (dbAdvisor != null && dbAdvisor.Tables.Count > 0 && dbAdvisor.Tables["navPathNodeList"] != null && dbAdvisor.Tables["navPathNodeList"].Rows.Count>0)
            {

                return dbAdvisor.Tables["navPathNodeList"].Rows[dbAdvisor.Tables["navPathNodeList"].Rows.Count - 1]["purePath"].ToString();
            }
            return "";
        }

        public string getCellData(int row, int col)
        {
            this.processItems();

            int num = (this.getCurrentPage() - 1) * this.getResultsPerPage();
            return this.m_items[row - num].getFormattedText(col);
        }
        public string getCellDataJson(int row, int col)
        {
            if (dbAdvisor != null && dbAdvisor.Tables.Count > 0 && dbAdvisor.Tables["Items"] != null && dbAdvisor.Tables["Items"].Rows.Count > 0)
            {
                return dbAdvisor.Tables["Items"].Rows[row][col].ToString();

            }
            return "";
        }
        public int getColumnIndex(string colName)
        {
            return this.getItemDescriptions().getColumnIndex(colName);
        }
        public int getColumnIndexJson(string colName)
        {
            if (dbAdvisor != null && dbAdvisor.Tables.Count > 0 && dbAdvisor.Tables["Items"] != null && dbAdvisor.Tables["Items"].Rows.Count > 0)
            {
                return dbAdvisor.Tables["Items"].Columns[colName.Replace(" ", "_")].Ordinal;   
                
            }
            return -1;
        }
        public string getCommentary()
        {
            if (null == this.m_commentary)
            {
                XmlNode node = this.m_doc.SelectSingleNode(PATH_COMMENTARY);
                this.m_commentary = (node == null) ? "" : node.FirstChild.Value;
            }
            return this.m_commentary;
        }
        public string getCommentaryJson()
        {
            if (dbAdvisor != null && dbAdvisor.Tables.Count > 0 && dbAdvisor.Tables["source"] != null && dbAdvisor.Tables["source"].Rows.Count > 0)
            {

                return dbAdvisor.Tables["source"].Rows[dbAdvisor.Tables["source"].Rows.Count - 1]["commentary"].ToString();
            }
            return "";
        }


        public IList<string> getCommonAttributeNames(bool onlySelected)
        {
            this.processCommonAttributes();
            List<string> list = new List<string>(this.m_commonAttributes.Count);
            foreach (AttributeInfo info in this.m_commonAttributes)
            {
                if (!(onlySelected && !this.wasAttributeSelected(info.getName())))
                {
                    list.Add(info.getName());
                }
            }
            return list;
        }

        private AttributeInfo getCommonAttrInfo(string attrName)
        {
            this.processCommonAttributes();
            foreach (AttributeInfo info in this.m_commonAttributes)
            {
                if (0 == string.Compare(attrName, info.getName(), true))
                {
                    return info;
                }
            }
            return null;
        }

        public IList<string> getCorrectedWords()
        {
            string[] strArray = this.getSpellCorrections().Split(LIST_SEP, StringSplitOptions.None);
            List<string> list = new List<string>(strArray.Length);
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] strArray2 = strArray[i].Split(CORRECTION_SEP, StringSplitOptions.None);
                list.Add(strArray2[0].Trim());
            }
            return list;
        }

        public string getCorrection(string word)
        {
            string[] strArray = this.getSpellCorrections().Split(LIST_SEP, StringSplitOptions.None);
            for (int i = 0; i < strArray.Length; i++)
            {
                string[] strArray2 = strArray[i].Split(CORRECTION_SEP, StringSplitOptions.None);
                if (0 == string.Compare(strArray2[0].Trim(), word, true))
                {
                    return strArray2[1].Trim();
                }
            }
            return null;
        }

        public int getCurrentPage()
        {
            return this.getItemDescriptions().getCurrentPage();
        }
        public int getCurrentPageJson()
        {
            if (dbAdvisor != null && dbAdvisor.Tables.Count > 0 && dbAdvisor.Tables["itemdescription"] != null && dbAdvisor.Tables["itemdescription"].Rows.Count > 0)
            {

                return Convert.ToInt32(dbAdvisor.Tables["itemdescription"].Rows[dbAdvisor.Tables["itemdescription"].Rows.Count - 1]["currentpage"].ToString());
            }
            return -1;
        }
        public IList<IDataDescription> getDataDescriptions()
        {
            return this.getItemDescriptions().getDataDescriptions();
        }

        public IList<INavigateAttribute> getDetailedAttributeValues(string attrName)
        {
            return this.getDetailedAttributeValues(attrName, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        }

        public IList<INavigateAttribute> getDetailedAttributeValues(string attrName, int displayMode)
        {
            this.processAttributes();
            return this.m_attrsInfo.getDetailedAttributeValues(attrName, displayMode);
        }

        public IList<INavigateCategory> getDetailedCategories()
        {
            return this.getDetailedCategories(EasyAskConstants.CATEGORY_DISPLAY_MODE_FULL);
        }

        public IList<INavigateCategory> getDetailedCategories(int nDisplayMode)
        {
            this.processCategories();
            return this.m_catInfo.getDetailedCategories(nDisplayMode);
        }

        public IList<INavigateAttribute> getDetailedCommonAttributeValues(string attrName)
        {
            return this.getDetailedCommonAttributeValues(attrName, EasyAskConstants.ATTR_DISPLAY_MODE_FULL);
        }

        public IList<INavigateAttribute> getDetailedCommonAttributeValues(string attrName, int displayMode)
        {
            AttributeInfo info = this.getCommonAttrInfo(attrName);
            if (null != info)
            {
                if (!((EasyAskConstants.ATTR_DISPLAY_MODE_INITIAL != displayMode) || info.getIsLimited()))
                {
                    displayMode = EasyAskConstants.ATTR_DISPLAY_MODE_FULL;
                }
                return ((EasyAskConstants.ATTR_DISPLAY_MODE_FULL == displayMode) ? info.getFullList() : info.getInitialList());
            }
            return new List<INavigateAttribute>(0);
        }

        public string getErrorMsg()
        {
            XmlNode node = this.m_doc.SelectSingleNode(PATH_ERROR_MSG);
            return ((node != null) ? node.FirstChild.InnerText : null);
        }

        public IFeaturedProducts getFeaturedProducts()
        {
            if (null == this.m_featuredProducts)
            {
                this.m_featuredProducts = new FeaturedProducts(this, this.m_doc.SelectSingleNode(PATH_TO_FEATURED_PRODUCTS));
            }
            return this.m_featuredProducts;
        }

        public int getFirstItem()
        {
            return this.getItemDescriptions().getFirstItem();
        }
        public int getFirstItemJson()
        {
            if (dbAdvisor != null && dbAdvisor.Tables.Count > 0 && dbAdvisor.Tables["itemdescription"] != null && dbAdvisor.Tables["itemdescription"].Rows.Count > 0)
            {

                return Convert.ToInt32(dbAdvisor.Tables["itemdescription"].Rows[dbAdvisor.Tables["itemdescription"].Rows.Count - 1]["firstitem"].ToString());
            }
            return -1;
        }
        public string getUserSearchValueJson()
        {
            if (dbAdvisor != null && dbAdvisor.Tables.Count > 0 && dbAdvisor.Tables["navpathnodelist"] != null && dbAdvisor.Tables["navpathnodelist"].Rows.Count > 0)
            {
                foreach(DataRow r in dbAdvisor.Tables["navpathnodelist"].Rows)
                {
                    if (r["purepath"].ToString().ToLower().Contains("usersearch") == true)
                        return r["value"].ToString();
                }
                
            }
            return "";
        }
        public IGroupedResultSet getGroupedResult()
        {
            this.processGroups();
            return this.m_groupSet;
        }

        public IList<string> getInitialDisplayList(int attrType)
        {
            this.processAttributes();
            return this.m_attrsInfo.getInitialDisplayList(attrType);
        }

        public int getInitialDispLimitForAttrNames()
        {
            this.processAttributes();
            return this.m_attrsInfo.getInitialDispLimitForAttrNames();
        }

        public int getInitialDispLimitForAttrValues(string attrName)
        {
            this.processAttributes();
            return this.m_attrsInfo.getInitialDispLimitForAttrValues(attrName);
        }

        public bool getIsCommand()
        {
            return DOMUtilities.getBooleanAttribute(this.m_doc.SelectSingleNode(PATH_TO_SOURCE), ATTR_IS_COMMAND);
        }

        public bool getIsDrillDown()
        {
            return this.getItemDescriptions().getIsDrillDown();
        }

        private ItemDescriptions getItemDescriptions()
        {
            this.processItemDescriptions();
            return this.m_itemDescriptions;
        }

        public bool getItemsFoundByModifyingQuery()
        {
            return DOMUtilities.getBoolean(this.m_doc.SelectSingleNode(PATH_TO_SOURCE), RELATIVE_PATH_MODIFY_QUERY);
        }

        public bool getItemsFoundWIthSecondarySearch()
        {
            return DOMUtilities.getBoolean(this.m_doc.SelectSingleNode(PATH_TO_SOURCE), RELATIVE_PATH_SECONDARY_SEARCH);
        }

        public int getLastItem()
        {
            return this.getItemDescriptions().getLastItem();
        }
        public int getLastItemJson()
        {
            if (dbAdvisor != null && dbAdvisor.Tables.Count > 0 && dbAdvisor.Tables["itemdescription"] != null && dbAdvisor.Tables["itemdescription"].Rows.Count > 0)
            {

                return Convert.ToInt32(dbAdvisor.Tables["itemdescription"].Rows[dbAdvisor.Tables["itemdescription"].Rows.Count - 1]["lastitem"].ToString());
            }
            return -1;
        }
        public INavigateHierarchy getNavigateHierarchy()
        {
            this.processNavigateHierarchy();
            return this.m_navHier;
        }

        public int getPageCount()
        {
            return this.getItemDescriptions().getPageCount();
        }

        public int getPageCountJson()
        {
            if (dbAdvisor != null && dbAdvisor.Tables.Count > 0 && dbAdvisor.Tables["itemdescription"] != null && dbAdvisor.Tables["itemdescription"].Rows.Count > 0)
            {

                return Convert.ToInt32(dbAdvisor.Tables["itemdescription"].Rows[dbAdvisor.Tables["itemdescription"].Rows.Count - 1]["pagecount"].ToString());
            }
            return -1;
        }
        public int getProductRetrievalMethod()
        {
            return DOMUtilities.getInteger(this.m_doc.SelectSingleNode(PATH_TO_SOURCE), RELATIVE_PATH_PRODUCT_RETRIEVAL_METHOD);
        }

        public bool getProductsFromGlobalSearch()
        {
            return DOMUtilities.getBoolean(this.m_doc.SelectSingleNode(PATH_TO_SOURCE), RELATIVE_PATH_GLOBAL_SEARCH);
        }

        public string getQuestion()
        {
            return DOMUtilities.getString(this.m_doc.SelectSingleNode(PATH_TO_SOURCE), RELATIVE_PATH_QUESTION);
        }

        public string getRedirect()
        {
            this.processDisplayFormat();
            return (this.isRedirect() ? this.getErrorMsg() : null);
        }

        public IList<string> getRelaxedTerms()
        {
            string[] strArray = this.splitCommentary(RELAXATION_PREFACE, COMMENTARY_SECTION_END).Split(LIST_SEP, StringSplitOptions.None);
            List<string> list = new List<string>(strArray.Length);
            for (int i = 0; i < strArray.Length; i++)
            {
                list.Add(strArray[i].Trim());
            }
            return list;
        }

        public int getResultsPerPage()
        {
            return this.getItemDescriptions().getResultsPerPage();
        }
        public int getResultsPerPageJson()
        {
            if (dbAdvisor != null && dbAdvisor.Tables.Count > 0 && dbAdvisor.Tables["itemdescription"] != null && dbAdvisor.Tables["itemdescription"].Rows.Count > 0)
            {

                return Convert.ToInt32(dbAdvisor.Tables["itemdescription"].Rows[dbAdvisor.Tables["itemdescription"].Rows.Count - 1]["resultperpage"].ToString());
            }
            return -1;
        }
        public int getReturnCode()
        {
            XmlNode node = this.m_doc.SelectSingleNode(PATH_RETURN_CODE);
            if (null != node)
            {
                try
                {
                    return int.Parse(node.FirstChild.InnerText);
                }
                catch (FormatException)
                {
                }
            }
            return -1;
        }

        public IResultRow getRow(int pageRow)
        {
            this.processItems();
            return this.m_items[pageRow];
        }

        public string getSortOrder()
        {
            return this.getItemDescriptions().getSortOrder();
        }

        public string getSpellCorrections()
        {
            return this.splitCommentary(SPELL_CORRECTION_PREFACE, COMMENTARY_SECTION_END);
        }

        public string getSuggestedCategoryTitle()
        {
            this.processCategories();
            return this.m_catInfo.getSuggestedCategoryTitle();
        }

        public int getTotalItems()
        {
            return this.getItemDescriptions().getTotalItems();
        }
        public int getTotalItemsJson()
        {
            if (dbAdvisor != null && dbAdvisor.Tables.Count > 0 && dbAdvisor.Tables["itemdescription"] != null && dbAdvisor.Tables["itemdescription"].Rows.Count > 0)
            {

                return Convert.ToInt32(dbAdvisor.Tables["itemdescription"].Rows[dbAdvisor.Tables["itemdescription"].Rows.Count - 1]["totalitems"].ToString());
            }
            return -1;
        }
        public bool isGroupedResult()
        {
            return this.m_isGrouped;
        }

        public bool isInitialDispLimitedForAttrNames()
        {
            this.processAttributes();
            return this.m_attrsInfo.isInitialDispLimitedForAttrNames();
        }

        public bool isInitialDispLimitedForAttrValues(string attrName)
        {
            this.processAttributes();
            return this.m_attrsInfo.isInitialDispLimitedForAttrValues(attrName);
        }

        public bool isPresentationError()
        {
            this.processDisplayFormat();
            return ((this.m_displayFormat != null) && this.m_displayFormat.isPresentationError());
        }

        public bool isRedirect()
        {
            this.processDisplayFormat();
            return ((this.m_displayFormat != null) && this.m_displayFormat.isRedirect());
        }

        //internal void load(string url)
        //{
        //    this.m_doc.Load(url);
        //    this.determineLayout();
        //}

        private void processAttributes()
        {
            if (null == this.m_attrsInfo)
            {
                this.m_attrsInfo = new AttributesInfo(this.m_doc.SelectSingleNode(PATH_ATTRIBUTES));
            }
        }

        private void processBreadCrumbTrail()
        {
            if (null == this.m_bct)
            {
                this.m_bct = new BreadCrumbTrail(this.m_doc.SelectSingleNode(PATH_TO_BREADCRUMB));
            }
        }

        private void processCategories()
        {
            if (null == this.m_catInfo)
            {
                this.m_catInfo = new CategoriesInfo(this.m_doc.SelectSingleNode(PATH_SOURCE));
            }
        }

        private void processCommonAttributes()
        {
            if (null == this.m_commonAttributes)
            {
                XmlNode attrNode = this.m_doc.SelectSingleNode(PATH_COMMON_ATTRIBUTES);
                this.m_commonAttributes = this.getAttributeInfo(attrNode);
            }
        }

        private void processDisplayFormat()
        {
            if (null == this.m_displayFormat)
            {
                this.m_displayFormat = new DisplayFormat(this.m_doc.SelectSingleNode(PATH_DISPLAY_FORMAT));
            }
        }

        private void processGroups()
        {
            if ((this.m_groupSet == null) && this.m_isGrouped)
            {
                this.m_groupSet = new GroupedSetInfo(this.m_doc.SelectSingleNode(PATH_TO_GROUPS), this);
            }
        }

        private void processItemDescriptions()
        {
            if (null == this.m_itemDescriptions)
            {
                XmlNode node = this.m_doc.SelectSingleNode(PATH_TO_ITEM_DESC);
                if (null != node)
                {
                    this.m_itemDescriptions = new ItemDescriptions(node);
                }
                else
                {
                    this.m_itemDescriptions = new ItemDescriptions();
                }
            }
        }

        private void processItems()
        {
            if (null == this.m_items)
            {
                this.m_items = new List<ItemRow>();
                if (!this.m_isGrouped)
                {
                    XmlNodeList list = this.m_doc.SelectNodes(PATH_TO_ITEM);
                    if (null != list)
                    {
                        IList<IDataDescription> desc = this.getDataDescriptions();
                        foreach (XmlNode node in list)
                        {
                            this.m_items.Add(new ItemRow(desc, node));
                        }
                    }
                }
            }
        }

        private void processNavigateHierarchy()
        {
            if (!this.m_bHierarachyProcessed)
            {
                XmlNode node = this.m_doc.SelectSingleNode(PATH_NAVIGATE_HIERARCHY);
                if (null != node)
                {
                    this.m_navHier = new NavigateHierarchy(node);
                }
                this.m_bHierarachyProcessed = true;
            }
        }

        private string splitCommentary(string key, char end)
        {
            string str = this.getCommentary();
            string str2 = "";
            int index = str.IndexOf(key);
            if (-1 < index)
            {
                str2 = str.Substring(index + key.Length);
                index = str2.IndexOf(end);
                if (-1 < index)
                {
                    str2 = str2.Substring(0, index);
                }
            }
            return str2;
        }

        private bool wasAttributeSelected(string attrName)
        {
            foreach (INavigateNode node in this.getBreadCrumbTrail().getSearchPath())
            {
                if (EasyAskConstants.NODE_TYPE_ATTRIBUTE == node.getType())
                {
                    string str = node.getPath();
                    int num = str.LastIndexOf(NODE_ATTRIB_SELECT);
                    if (0 <= num)
                    {
                        string[] strArray = str.Substring(num + NODE_ATTRIB_SELECT.Length).Split(splitValSep, StringSplitOptions.None);
                        for (int i = 0; i < strArray.Length; i++)
                        {
                            if (strArray[i].StartsWith(attrName + " = '"))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}

