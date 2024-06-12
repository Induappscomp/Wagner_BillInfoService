namespace TradingBell.WebCat.EasyAsk.Impl
{
    using System;
    using System.Xml;

    internal class DisplayFormat
    {
        private static readonly string ATTR_ERROR = "EA_Error";
        private static readonly string ATTR_OUTPUT_ENGINE = "EA_OutputEngine";
        private static readonly string ATTR_PRESENTATION = "EA_Presentation";
        private static readonly int ERROR_REDIRECT = 5;
        private static readonly int ERROR_TELL_USER = 6;
        private int m_Error = 0;
        private int m_OutputEngine = 0;
        private int m_Presentation = 0;
        private static readonly int PRESENTATION_ERROR = -1;

        internal DisplayFormat(XmlNode node)
        {
            if (null != node)
            {
                this.m_OutputEngine = DOMUtilities.getIntegerAttribute(node, ATTR_OUTPUT_ENGINE);
                this.m_Presentation = DOMUtilities.getIntegerAttribute(node, ATTR_PRESENTATION);
                this.m_Error = DOMUtilities.getIntegerAttribute(node, ATTR_ERROR);
            }
        }

        public int getError()
        {
            return this.m_Error;
        }

        public int getOutputEngine()
        {
            return this.m_OutputEngine;
        }

        public int getPresentation()
        {
            return this.m_Presentation;
        }

        public bool isPresentationError()
        {
            return (this.getPresentation() == PRESENTATION_ERROR);
        }

        public bool isRedirect()
        {
            return (this.getError() == ERROR_REDIRECT);
        }
    }
}

