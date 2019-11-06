﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;

namespace Root.Reports
{
    /// <summary>Report Object</summary>
    public class ReportBase : MarshalByRefObject
    {
        /// <summary>Formatter that belongs to this report</summary>
        private Formatter _formatter;

        /// <summary>List of all font definitions that are defined for the report</summary>
        private readonly Dictionary<String, FontDef> _dict_FontDef = new Dictionary<String, FontDef>(20);

        /// <summary>List of all font properties objects that are defined for the report</summary>
        internal readonly Dictionary<String, FontProp> dict_FontProp = new Dictionary<String, FontProp>(100);

        /// <summary>List of all line properties objects that are defined for the report</summary>
        internal readonly Hashtable ht_PenProp = new Hashtable(100);

        /// <summary>List of all line properties objects that are defined for the report</summary>
        internal readonly Hashtable ht_BrushProp = new Hashtable(100);

        /// <summary>List of all pages of this report</summary>
        private readonly ArrayList al_Page = new ArrayList(100);

        /// <summary>List of all images of this report</summary>
        internal readonly Hashtable ht_ImageData = new Hashtable(50);

        /// <summary>Title of the report</summary>
        public String sTitle;

        /// <summary>The name of the person who created the report</summary>
        public String sAuthor;

        /// <summary>Current page</summary>
        public Page page_Cur;

        internal ArrayList al_PendingTasks = new ArrayList(20);

        //----------------------------------------------------------------------------------------------------
        /// <summary>Creates a Report.</summary>
        /// <param name="formatter">Formatter to use for this report</param>
        public ReportBase(Formatter formatter)
        {
            Init(formatter);
        }

        //----------------------------------------------------------------------------------------------------
        /// <summary>Creates a report.</summary>
        public ReportBase() : this(new PdfFormatter())
        {
        }

        //----------------------------------------------------------------------------------------------------
        /// <summary>Gets the font definition hash table.</summary>
        internal Dictionary<String, FontDef> dict_FontDef
        {
            get { return _dict_FontDef; }
        }
        //----------------------------------------------------------------------------------------------------
        /// <summary>Returns an enumeration of all fonts definitions.</summary>
        internal IEnumerable enum_FontDef
        {
            get { return dict_FontDef.Values; }
        }

        //----------------------------------------------------------------------------------------------------
        /// <summary>Returns an enumeration of all pages.</summary>
        public IEnumerable enum_Page
        {
            get { return al_Page; }
        }

        //----------------------------------------------------------------------------------------------------
        /// <summary>Gets the formatter of the report</summary>
        public Formatter formatter
        {
            get { return _formatter; }
#warning this is an exception, don't use it in common, the setter method must be removed!
            set { _formatter = value; }
        }

        //----------------------------------------------------------------------------------------------------
        /// <summary>Returns the number of pages of this report.</summary>
        public Int32 iPageCount
        {
            get { return al_Page.Count; }
        }

        //----------------------------------------------------------------------------------------------------
        /// <summary>Creates the contents of the report</summary>
        internal protected virtual void Create()
        {
        }

        //----------------------------------------------------------------------------------------------------
        /// <summary>Creates a report.</summary>
        /// <param name="formatter">Formatter to use for this report</param>
        internal void Init(Formatter formatter)
        {
            if (this.formatter != null)
            {
                throw new ReportException("The formatter of a report may be set only once");
            }
            _formatter = formatter;
            formatter.report = this;
        }

        //----------------------------------------------------------------------------------------------------
        /// <summary>Registers a new page.</summary>
        internal void RegisterPage(Page page)
        {
            al_Page.Add(page);
            page_Cur = page;
        }

        //----------------------------------------------------------------------------------------------------
        /// <summary>Saves the report.</summary>
        /// <param name="sFileName">File name</param>
        public void Save(String sFileName)
        {
            FileStream stream = File.Create(sFileName);

            //Encoding.Convert(Encoding.Unicode, Encoding.ASCII, stream.  enc = new System.Text.Encoder();

            if (page_Cur == null)
            {
                Create();
            }

            try
            {
                formatter.Create(this, stream);
                foreach (Object o in al_PendingTasks)
                {
                    //          TlmBase tlmBase = (TlmBase)o;
                    throw new ReportException("Layout manager has not been closed");
                }
            }
            finally
            {
                stream.Close();
            }
        }
    }
}
