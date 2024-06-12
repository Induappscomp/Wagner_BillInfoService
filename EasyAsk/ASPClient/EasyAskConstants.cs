namespace TradingBell.WebCat.EasyAsk
{
    using System;

    public class EasyAskConstants
    {
        public static readonly int ATTR_DISPLAY_MODE_FULL = 0;
        public static readonly int ATTR_DISPLAY_MODE_INITIAL = 1;
        public static readonly int ATTR_FILTER_ALL = (ATTR_FILTER_NORMAL | ATTR_FILTER_MERCHANDISER);
        public static readonly int ATTR_FILTER_MERCHANDISER = 2;
        public static readonly int ATTR_FILTER_NORMAL = 1;
        public static readonly int ATTR_TYPE_MERCHANDISER = 0x15;
        public static readonly int ATTR_TYPE_NORMAL = 11;
        public static readonly int CATEGORY_DISPLAY_MODE_FULL = 0;
        public static readonly int CATEGORY_DISPLAY_MODE_INITIAL = 1;
        public static readonly int NODE_TYPE_ATTRIBUTE = 2;
        public static readonly int NODE_TYPE_CATEGORY = 1;
        public static readonly int NODE_TYPE_USER_SEARCH = 3;
    }
}

