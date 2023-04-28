using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using FarsiLibrary.Resources;
using FarsiLibrary.Utils;
using FarsiLibrary.Win.BaseClasses;
using FarsiLibrary.Win.Design;
using FarsiLibrary.Win.Enums;
using FarsiLibrary.Win.Events;
using PersianCalendar=FarsiLibrary.Utils.PersianCalendar;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace FarsiLibrary.Win.Controls
{
	/// <summary>
	/// MonthView control is a calendar control that displays days of a month in a view, and user can select dates in various formats.
    /// This control currently supports three cultures and calendars, both RTL and LTR rendering and displaying numeric values in correct localized format.
    /// 
    /// If you want to use the control in FA-IR culture, main entry of your application should look like this :
    /// <code>
    ///     public static void Main()
    ///     {
    ///                     Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fa-IR");
    ///                     Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
    ///          
    ///                     Application.EnableVisualStyles(); // To Support XP and Office2003 themes.
    ///                     Application.Run(new MainForm());
    ///     }
    /// </code>
    /// 
    /// If the <see cref="System.Globalization.CultureInfo"/> has Right-To-Left reading format, control renders months and days in RTL, otherwise in LTR.
	/// </summary>
	[ToolboxItem(true)]
    [Designer(typeof(FAMonthViewDesigner))]
    [DefaultEvent("SelectedDateTimeChanged")]
    [DefaultProperty("SelectedDateTime")]
    [ToolboxBitmap(typeof(FAMonthView))]
	public class FAMonthView : BaseStyledControl
	{
		#region Constants

		private const int DEF_HEADER_SIZE = 21; 
		private const int DEF_ARROW_SIZE = 3;
		private const int DEF_FOOTER_SIZE = 27;
        private const int DEF_BUTTON_WIDTH = 60;
		private const int DEF_BUTTON_HEIGHT = 23;
		private const int DEF_WEEK_DAY_HEIGHT = 20;
		private const int DEF_ROWS_MARGIN = 3;
		private const int DEF_COLUMNS_COUNT = 7;
		private const int DEF_ROWS_COUNT = 7;
		private const int DEF_TODAY_TAB_INDEX = 100;
		private const int DEF_NONE_TAB_INDEX = 101;

		#endregion

		#region Declarations and Internal Classes

		internal class ActRect
		{
			#region Fields

			private Rectangle m_rect;
			private TRectangleStatus m_state;
			private bool m_bInvalidate = true;
			private TRectangleAction m_act;
			private object m_tag;

			#endregion

			#region Class Properties

			public Rectangle Rect
			{
				get { return m_rect; }
				set { m_rect = value; }
			}

			public TRectangleStatus State
			{
				get { return m_state; }
				set
				{
					m_state = TRectangleStatus.Normal;
				}
			}

			public bool InvalidateOnChange
			{
				get { return m_bInvalidate; }
				set { m_bInvalidate = value; }
			}

			public TRectangleAction Action
			{
				get { return m_act; }
				set { m_act = value; }
			}

			public object Tag
			{
				get { return m_tag; }
				set { m_tag = value; }
			}

			public bool IsFocused
			{
				get { return (m_state & TRectangleStatus.Focused) == TRectangleStatus.Focused; }
			}

			public bool IsSelected
			{
				get { return (m_state & TRectangleStatus.Selected) == TRectangleStatus.Selected; }
			}

			public bool IsActive
			{
				get { return (m_state & TRectangleStatus.Active) == TRectangleStatus.Active; }
			}

			#endregion

			#region Ctor

			public ActRect(Rectangle rc, TRectangleStatus state, TRectangleAction act, bool invalidate)
			{
				m_rect = rc;
				m_state = state;
				m_bInvalidate = invalidate;
				m_act = act;
			}

			public ActRect(Rectangle rc, TRectangleStatus state, TRectangleAction act) : this(rc, state, act, true)
			{
			}

			public ActRect(Rectangle rc, TRectangleAction act, object tag)
			{
				m_rect = rc;
				m_state = TRectangleStatus.Normal;
				m_act = act;
				m_tag = tag;
			}

			public ActRect(Rectangle rc, TRectangleAction act) : this(rc, TRectangleStatus.Normal, act, true)
			{
			}

			public ActRect(Rectangle rc, TRectangleStatus state) : this(rc, state, TRectangleAction.None, true)
			{
			}

			public ActRect(Rectangle rc) : this(rc, TRectangleStatus.Normal, TRectangleAction.None, true)
			{
			}

			public ActRect() : this(Rectangle.Empty, TRectangleStatus.Normal, TRectangleAction.None, true)
			{
			}

			#endregion
		}

		#endregion

		#region Fields

		private Rectangle rcHeader;
		private Rectangle rcFooter;
		private Rectangle rcBody;

		private ArrayList rects = new ArrayList(100);
        private ArrayList selectedRects = new ArrayList();
        private DateTimeCollection selectedDateRange;

		private int iYear;
		private int iMonth;
		private int iDay;

		private DateTime selectedGregorianDate;
		private DateTime dtSelected;
		private PersianCalendar pc;
        private GregorianCalendar gc;
        private HijriCalendar hc;
        private Calendar customCalendar;

		private int iLastFocused = 1; 
        private bool isNull;
        private bool rectsCreated;
        private bool isPopupMode;
        private bool btnTodayActive;
        private bool btnNoneActive;
		private bool isMultiSelect = false;
        private bool showFocusRect = false;
        private bool showBorder = true;
        private StringFormat format;
        private ScrollOptionTypes scrollOption;
	    
		#endregion

		#region Events

        /// <summary>
        /// Fires when SelectedDateTime value changes.
        /// </summary>
        public event EventHandler SelectedDateTimeChanged;

        /// <summary>
        /// Fires when a date is added/removes to SelectedDateRange collection, if the control is in <see cref="IsMultiSelect"/> mode.
        /// </summary>
		public event EventHandler SelectedDateRangeChanged;

        /// <summary>
        /// Fires when Day value changes.
        /// </summary>
		public event EventHandler DayChanged;

        /// <summary>
        /// Fires when MonthValue changes.
        /// </summary>
		public event EventHandler MonthChanged;

        /// <summary>
        /// Fires when Year value changes.
        /// </summary>
		public event EventHandler YearChanged;

	    /// <summary>
	    /// Fires when current day is being printed.
	    /// </summary>
        public event CustomDrawDayEventHandler DrawCurrentDay;

	    /// <summary>
	    /// Fires when user clicks on a day, None button or Today button.
	    /// </summary>
        public event CalendarButtonClickedEventHandler ButtonClicked;
	    
		#endregion

		#region Props

	    /// <summary>
	    /// Determinces scrolling option of the FAMonthView control.
	    /// </summary>
	    [DefaultValue(typeof(ScrollOptionTypes), "Month")]
        [Description("Determinces scrolling option of the FAMonthView control.")]
	    public ScrollOptionTypes ScrollOption
	    {
            get { return scrollOption; }
	        set
	        {
	            if(scrollOption == value)
	                return;
	            
	            scrollOption = value;
	        }
	    }
	    
        [Browsable(false)]
        internal StringFormat OneLineNoTrimming
        {
            get
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                format.Trimming = StringTrimming.None;
                format.FormatFlags = StringFormatFlags.LineLimit;
                format.HotkeyPrefix = HotkeyPrefix.Show;

                return format;
            }
        }

        /// <summary>
        /// Determines if the control has not made any selection yet.
        /// </summary>
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.All)]
        public bool IsNull
        {
            get { return isNull; }
            set
            {
                if (isNull == value)
                    return;

                isNull = value;
                if (isNull)
                {
                    //also clear the selection
                    selectedDateRange.Clear();
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Gets the Day value.
        /// </summary>
        [Browsable(false)]
        public int Day
        {
            get { return iDay; }
        }

        /// <summary>
        /// Gets the Month value.
        /// </summary>
        [Browsable(false)]
        public int Month
        {
            get { return iMonth; }
        }

        /// <summary>
        /// Gets the Year value.
        /// </summary>
        [Browsable(false)]
        public int Year
        {
            get { return iYear; }
        }

        /// <summary>
        /// Arabic culture supported by the control : ("AR-SA")
        /// </summary>
        [Browsable(false)]
        public CultureInfo ArabicCulture
        {
            get { return FALocalizeManager.ArabicCulture; }
        }

        /// <summary>
        /// Invariant culture supported by the control.
        /// </summary>
        [Browsable(false)]
        public CultureInfo InvariantCulture
        {
            get { return FALocalizeManager.InvariantCulture; }
        }

        /// <summary>
        /// Persian culture supported by the control. ("FA-IR")
        /// </summary>
        [Browsable(false)]
        public CultureInfo PersianCulture
        {
            get { return FALocalizeManager.FarsiCulture; }
        }

        /// <summary>
        /// PersianCalendar instance with which controls calculates values of <see cref="PersianCulture"/>.
        /// </summary>
        [Browsable(false)]
        public PersianCalendar PersianCalendar
        {
            get { return pc; }
        }

        /// <summary>
        /// GregorianCalendar instance with which controls calculates values of <see cref="InvariantCulture"/>.
        /// </summary>
        [Browsable(false)]
        public Calendar InvariantCalendar
        {
            get { return gc; }
        }

        /// <summary>
        /// HijriCalendar instance with which controls calculates values of <see cref="ArabicCulture"/>.
        /// </summary>
        [Browsable(false)]
        public Calendar HijriCalendar
        {
            get { return hc; }
        }

        /// <summary>
        /// Default calendar of the control, based on <see cref="Thread.CurrentCulture"/> and <see cref="Thread.CurrentUICulture"/> properties.
        /// </summary>
        [Browsable(false)]
        public Calendar DefaultCalendar
        {
            get
            {
                return GetDefaultCalendar();
            }
            set
            {
                customCalendar = value;
                iYear = DefaultCalendar.GetYear(selectedGregorianDate);
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or Sets to show a border around the control.
        /// </summary>
        [DefaultValue(true)]
        [Description("Gets or Sets to show a border around the control.")]
        public bool ShowBorder
        {
            get { return showBorder; }
            set
            {
                if (showBorder == value)
                    return;

                showBorder = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or Sets to show the focus rectangle around the selected day.
        /// </summary>
        [DefaultValue(false)]
        [Description("Gets or Sets to show the focus rectangle around the selected day.")]
        public bool ShowFocusRect
        {
            get { return showFocusRect; }
            set
            {
                if (showFocusRect == value)
                    return;

                showFocusRect = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Selected values collection, if the control is in MultiSelect mode.
        /// </summary>
        [Editor(typeof(DateTimeCollectionEditor), typeof(UITypeEditor))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [TypeConverter(typeof(DateTimeConverter))]
        [Description("Selected values collection, if the control is in MultiSelect mode.")]
        public DateTimeCollection SelectedDateRange
        {
            get 
            {
                if (IsMultiSelect)
                {
                    return selectedDateRange;
                }
                else
                {
                    DateTimeCollection singleSelection = new DateTimeCollection();
                    if(!IsNull)
                        singleSelection.Add(SelectedDateTime);

                    return singleSelection;
                }
            }
        }

        /// <summary>
        /// Gets or Sets the control in MultiSelect mode.
        /// </summary>
		[DefaultValue(false)]
        [Description("Gets or Sets the control in MultiSelect mode.")]
		public bool IsMultiSelect
		{
			get
			{
				return isMultiSelect;
			}
			set
			{
				if(isMultiSelect == value)
					return;

				isMultiSelect = value;
                Repaint();
			}
		}

		/// <summary>
		/// Currently selected DateTime in calendar control.
		/// </summary>
		[Description("Currently selected DateTime instance from calendar.")]
		[RefreshProperties(RefreshProperties.All)]
        [Bindable(true)]
		[Localizable(true)]
		public DateTime SelectedDateTime
		{
			get
			{
			    return selectedGregorianDate;
			}
			set
			{
                if (value.Equals(DateTime.MinValue))
                    IsNull = true;
                else
                    IsNull = false;
			    
                if (selectedGregorianDate == value)
                    return;

				selectedGregorianDate = value;

                iDay = DefaultCalendar.GetDayOfMonth(selectedGregorianDate);
                OnDayChanged(EventArgs.Empty);

                iMonth = DefaultCalendar.GetMonth(selectedGregorianDate);
                OnMonthChanged(EventArgs.Empty);

                iYear = DefaultCalendar.GetYear(selectedGregorianDate);
                OnYearChanged(EventArgs.Empty);

                rectsCreated = false;
                Invalidate();

                OnSelectedDateTimeChanged(EventArgs.Empty);
			}
		}

        /// <summary>
        /// Size of the control that can not be changes. Control's size is fixed to 166 x 166 pixels.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Size Size
		{
			get { return base.Size; }
			set 
            {
                base.Size = new Size(166, 166); 
            }
		}

        /// <summary>
        /// Is control in popup mode?
        /// </summary>
        [Browsable(false)]
        [DefaultValue(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool IsPopupMode
        {
            get { return isPopupMode; }
            set { isPopupMode = value; }
        }

		#endregion

		#region Hidden Props

		[Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		public override string Text
		{
			get { return base.Text; }
			set { base.Text = value; }
		}

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override DockStyle Dock
        {
            get
            {
                return DockStyle.None;
            }
            set
            {
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override AnchorStyles Anchor
        {
            get
            {
                return AnchorStyles.None;
            }
            set
            {
            }
        }
	    
		#endregion

		#region Ctor && Dispose

	    /// <summary>
	    /// Creates a new instance of FAMonthView class. Initiated control could be uses in PopupMode or Normal mode, depending on the use.
	    /// </summary>
	    /// <param name="popupMode"></param>
	    public FAMonthView(bool popupMode)
	    {
	        IsPopupMode = popupMode;
	        SetStyle(ControlStyles.UserPaint, true);
	        SetStyle(ControlStyles.AllPaintingInWmPaint, true);
	        SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
	        SetStyle(ControlStyles.ResizeRedraw, true);

	        pc = new PersianCalendar();
	        gc = new GregorianCalendar();
	        hc = new HijriCalendar();

	        selectedDateRange = new DateTimeCollection();
            selectedDateRange.CollectionChanged += OnSelectionCollectionChanged;

	        base.Size = new Size(166, 166);
            base.Font = new Font("Tahoma", 8.25F);
	        SelectedDateTime = DateTime.Now;
	        dtSelected = SelectedDateTime;
	        format = new StringFormat();
            scrollOption = ScrollOptionTypes.Month;
	        isNull = true;
	        
	        FALocalizeManager.LocalizerChanged += new EventHandler(OnInternalLocalizeChanged);
	    }

	    /// <summary>
	    /// Creates a new instance of FAMonthView for normal mode usage.
	    /// </summary>
	    public FAMonthView() : this(false)
	    {
	    }

	    /// <summary>
	    /// Disposes the control.
	    /// </summary>
	    /// <param name="disposing"></param>
	    protected override void Dispose(bool disposing)
	    {
	        if (disposing)
	        {
	            if (format != null) format.Dispose();
	        }

	        base.Dispose(disposing);
	    }

	    #endregion

	    #region Paint Methods

	    protected override void OnPaint(PaintEventArgs pe)
	    {
	        if (!CanUpdate)
	            return;

	        try
	        {
	            BeginUpdate();

	            Rectangle rc = new Rectangle(0, 0, Width, Height);

	            //Active rectangles must be rebuild
	            if (rectsCreated == false)
	                rects.Clear();

	            OnDrawHeader(new PaintEventArgs(pe.Graphics, rcHeader));
	            OnDrawFooter(new PaintEventArgs(pe.Graphics, rcFooter));

	            if (rectsCreated == false)
	            {
	                rcBody = new Rectangle(rc.X, rcHeader.Bottom, rc.Width, rcFooter.Top - rcHeader.Bottom);
	                rcBody = Rectangle.Inflate(rcBody, -4, -1);
	            }

	            OnDrawBody(new PaintEventArgs(pe.Graphics, rcBody));
	            OnDrawBorder(new PaintEventArgs(pe.Graphics, rc));

	            rectsCreated = true;
	        }
	        finally
	        {
	            EndUpdate();
	        }
	    }

	    protected override void OnPaintBackground(PaintEventArgs pevent)
	    {
	        if (!CanUpdate)
	            return;

	        try
	        {
	            BeginUpdate();

	            Graphics g = pevent.Graphics;
	            Rectangle rc = new Rectangle(0, 0, Width, Height);

	            Painter.DrawFilledBackground(g, rc, false, 90f);

	            // Draw header background
	            rcHeader = new Rectangle(rc.X + 1, rc.Y + 1, rc.Width - 2, DEF_HEADER_SIZE);
	            Painter.DrawButton(g, rcHeader, string.Empty, Font, null, ItemState.Normal, false, true);

	            // Construct footer rect
	            int yBott = rc.Bottom - DEF_FOOTER_SIZE - 1;
	            rcFooter = new Rectangle(rc.X + 6, yBott, rc.Width - 12, DEF_FOOTER_SIZE);
	        }
	        finally
	        {
	            EndUpdate();
	        }
	    }

	    private void OnDrawBorder(PaintEventArgs e)
	    {
	        if (ShowBorder)
	        {
	            Rectangle border = new Rectangle(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
	            Painter.DrawBorder(e.Graphics, border, false);
	        }
	    }

	    private void OnDrawHeader(PaintEventArgs pevent)
	    {
	        Rectangle rc = pevent.ClipRectangle;
	        Rectangle rcOut = Rectangle.Inflate(rc, -6, -1);

	        PaintEventArgs ev = new PaintEventArgs(pevent.Graphics, rcOut);

	        OnDrawMonthHeader(ev);
	        OnDrawYearHeader(ev);
	    }

	    private void OnDrawMonthHeader(PaintEventArgs pevent)
	    {
	        Graphics g = pevent.Graphics;
	        Rectangle rc = pevent.ClipRectangle;

	        string strMonth = CurrentMonthName;
	        string strLongestMonth = PersianDate.PersianMonthNames.Default.Ordibehesht;

	        // Draw left arrow and add it as Active Rectangle
	        Rectangle rect = Painter.DrawArrow(g, rc, true, false, DEF_ARROW_SIZE);
	        AddActiveRect(rect, TRectangleAction.MonthDown);

	        SizeF sz = g.MeasureString(strLongestMonth, Font);
	        Rectangle rcText = new Rectangle(rect.Right + 4, rc.Y, (int) sz.Width + 20, rc.Height);
	        g.DrawString(strMonth, Font, SystemBrushes.WindowText, rcText, OneLineNoTrimming);
	        
	        // draw  right arrow and add it like Active Rectangle
	        rect = Painter.DrawArrow(g, new Rectangle(rcText.Right + 4, rc.Y, 100, rc.Height), false, false, DEF_ARROW_SIZE);
	        AddActiveRect(rect, TRectangleAction.MonthUp);
	    }

	    private void OnDrawYearHeader(PaintEventArgs pevent)
	    {
	        Graphics g = pevent.Graphics;
	        Rectangle rc = pevent.ClipRectangle;

	        string strYear = toFarsi.Convert(iYear.ToString(), DefaultCulture);

	        Rectangle rect = Painter.DrawArrow(g, new Rectangle(rc.Right - 4 - DEF_ARROW_SIZE - 2, rc.Y, DEF_ARROW_SIZE * 2, rc.Height), false, false, DEF_ARROW_SIZE);
	        AddActiveRect(rect, TRectangleAction.YearUp);

	        SizeF sz = g.MeasureString(strYear, Font);
	        Rectangle rcText = new Rectangle(rect.Left - 4 - (int) sz.Width - 8, rc.Y, (int) sz.Width + 8, rc.Height);
	        g.DrawString(strYear, Font, SystemBrushes.WindowText, rcText, OneLineNoTrimming);

	        rect = Painter.DrawArrow(g, new Rectangle(rcText.Left - 4 - DEF_ARROW_SIZE - 2, rc.Y, DEF_ARROW_SIZE * 2, rc.Height), true, false, DEF_ARROW_SIZE);
	        AddActiveRect(rect, TRectangleAction.YearDown);
	    }

	    private void OnDrawFooter(PaintEventArgs pevent)
	    {
	        Graphics g = pevent.Graphics;
	        OnDrawFooterButtons(new PaintEventArgs(g, rcFooter));
	    }

	    private void OnDrawFooterButtons(PaintEventArgs pevent)
	    {
	        Graphics g = pevent.Graphics;
	        Rectangle rc = pevent.ClipRectangle;

	        int buttonSpace = 10;
	        int margin = ((rc.Width - (DEF_BUTTON_WIDTH * 2) - buttonSpace) / 2);

	        StringFormat fmt = new StringFormat();
	        fmt.Alignment = StringAlignment.Center;
	        fmt.LineAlignment = StringAlignment.Center;
	        fmt.Trimming = StringTrimming.None;
	        fmt.FormatFlags |= StringFormatFlags.DirectionRightToLeft | StringFormatFlags.NoWrap;

	        Rectangle rcToday = new Rectangle(rc.X + margin, rc.Y + rc.Height/2 - DEF_BUTTON_HEIGHT/2, DEF_BUTTON_WIDTH, DEF_BUTTON_HEIGHT);
	        Rectangle rcNone = new Rectangle(rcToday.Right + buttonSpace, rcToday.Y, rcToday.Width, rcToday.Height);
	        AddActiveRect(rcToday, TRectangleAction.TodayBtn, DEF_TODAY_TAB_INDEX);
	        AddActiveRect(rcNone, TRectangleAction.NoneBtn, DEF_NONE_TAB_INDEX);

	        ItemState todayState = ItemState.Normal;
	        ItemState noneState = ItemState.Normal;

	        if(btnTodayActive)
	            todayState = ItemState.HotTrack;

	        if (btnNoneActive)
	            noneState = ItemState.HotTrack;

	        Painter.DrawButton(g, rcToday, FALocalizeManager.GetLocalizerByCulture(DefaultCulture).GetLocalizedString(StringID.FAMonthView_Today), Font, fmt, todayState, true, true);
	        Painter.DrawButton(g, rcNone, FALocalizeManager.GetLocalizerByCulture(DefaultCulture).GetLocalizedString(StringID.FAMonthView_None), Font, fmt, noneState, true, true);

	        fmt.Dispose();
	    }

	    private void OnDrawBody(PaintEventArgs pevent)
	    {
	        Graphics g = pevent.Graphics;
	        Rectangle rc = pevent.ClipRectangle;

	        int iColWidth = rc.Width/DEF_COLUMNS_COUNT;
	        int iRowHeight = (rc.Height - DEF_WEEK_DAY_HEIGHT) / DEF_ROWS_COUNT;

	        #region Top Separator

	        Painter.DrawSeparator(g, new Point(rc.X + 2, rc.Y + DEF_WEEK_DAY_HEIGHT - 3),
	                              new Point(rc.Right - 2, rc.Y + DEF_WEEK_DAY_HEIGHT - 3));

	        #endregion

	        #region Weekday Name

	        Rectangle rcHead = new Rectangle(rc.X, rc.Y, iColWidth, DEF_WEEK_DAY_HEIGHT - 3);
            
	        if (IsRightToLeftCulture)
	        {
	            for (int i = DEF_COLUMNS_COUNT; i > 0; i--)
	            {
	                rcHead.X = rc.Width - (i * iColWidth);
	                string strDayWeek = GetAbbrDayName(i - 1);
	                g.DrawString(strDayWeek, Font, SystemBrushes.WindowText, rcHead, OneLineNoTrimming);
	            }
	        }
	        else
	        {
	            for (int i = 0; i < DEF_COLUMNS_COUNT; i++)
	            {
	                rcHead.X = rc.X + (i * iColWidth);
	                string strDayWeek = GetAbbrDayName(i);
	                g.DrawString(strDayWeek, Font, SystemBrushes.WindowText, rcHead, OneLineNoTrimming);
	            }
	        }

	        #endregion

	        #region Calculate Month Values

	        //How many days are in DrawTab month and first day of month
	        int numDays = DefaultCalendar.GetDaysInMonth(iYear, iMonth);
            DateTime dtStartOfMonth = new DateTime(iYear, iMonth, 1, 0, 0, 0, DefaultCalendar);
            int firstDay = GetFirstDayOfWeek(dtStartOfMonth);

	        int rowNo = 1;
	        int index;

	        int iLastMonth = iMonth;
	        int iLastYear = iYear;

	        if (iMonth - 1 < 1 && iLastYear > 1)
	        {
	            iLastMonth = 12;
	            iLastYear--;
	        }
	        else if(iLastMonth - 1 > 0)
	        {
	            iLastMonth--;
	        }

	        int prevMonthDays = DefaultCalendar.GetDaysInMonth(iLastYear, iLastMonth);
	        int lastingDays = prevMonthDays - firstDay;
	        Brush brush = Brushes.Gray;
	        Rectangle rcDay;

	        if(IsRightToLeftCulture)
	            rcDay = new Rectangle(rcHead.X, rc.Y + DEF_WEEK_DAY_HEIGHT, rcHead.Width - 2, iRowHeight + 1);
	        else
	            rcDay = new Rectangle(rc.X, rc.Y + DEF_WEEK_DAY_HEIGHT, rcHead.Width - 2, iRowHeight + 1);

	        #endregion

	        #region Pre-Day Padding

	        for (int y = lastingDays; y < prevMonthDays; y++)
	        {
	            //Disabled Days
	            string disabledDay = toFarsi.Convert((y + 1).ToString(), DefaultCulture);
	            g.DrawString(disabledDay, Font, brush, rcDay, OneLineNoTrimming);

	            if (IsRightToLeftCulture)
	                rcDay.X = rcDay.X - iColWidth;
	            else
	                rcDay.X = rcDay.X + iColWidth;
	        }

	        #endregion

	        #region Current Day

	        for (int x = 1; x <= numDays; x++)
	        {
                DateTime dtInPaint = new DateTime(iYear, iMonth, x, DefaultCalendar);
	            brush = SystemBrushes.WindowText;

	            //draw weekday header names
	            string DayNo = toFarsi.Convert(x.ToString(), DefaultCulture);
	            index = x;

	            //Selected Days
	            if(IsMultiSelect && (selectedRects.Contains(index) || SelectedDateRange.Contains(dtInPaint)))
	            {
	                if(!ShowFocusRect)
	                    Painter.DrawSelectedPanel(g, rcDay);
                    else
	                    Painter.DrawFocusRect(g, rcDay);
					
	                g.DrawString(DayNo, Font, SystemBrushes.ControlText, rcDay, OneLineNoTrimming);
	                AddActiveRect(rcDay, TRectangleAction.MonthDay, index);
	            }
                else if (iDay == x) //Current Day
	            {
                    AddActiveRect(rcDay, TRectangleAction.MonthDay, index);
	                CustomDrawDayEventArgs args = new CustomDrawDayEventArgs(rcDay, g, iYear, iMonth, x, true);
	                OnDrawCurrentDay(args);
	                
                    if (!args.Handled)
                    {
                        if (!IsNull && !ShowFocusRect)
                            Painter.DrawSelectedPanel(g, rcDay);
                        else if (!IsNull && ShowFocusRect)
                            Painter.DrawFocusRect(g, rcDay);
                        else if (IsNull)
                            Painter.DrawSelectionBorder(g, rcDay);

                        g.DrawString(DayNo, Font, SystemBrushes.ControlText, rcDay, OneLineNoTrimming);
                    }

                    
	            }
	            else //Other Days
	            {
                    AddActiveRect(rcDay, TRectangleAction.MonthDay, index);
                    CustomDrawDayEventArgs args = new CustomDrawDayEventArgs(rcDay, g, iYear, iMonth, x, false);
                    OnDrawCurrentDay(args);

                    if (!args.Handled)
                    {
                        g.DrawString(DayNo, Font, brush, rcDay, OneLineNoTrimming);
                    }
	            }

	            if (IsRightToLeftCulture)
	            {
	                rcDay.X = rcDay.X - iColWidth;

	                if (rcDay.X < 0)
	                {
	                    rowNo++;
	                    rcDay.X = rcHead.X;
	                    rcDay.Y = rcDay.Y + iRowHeight + DEF_ROWS_MARGIN;
	                }
	            }
	            else
	            {
	                rcDay.X = rcDay.X + iColWidth;

	                if (rcDay.X > rc.Width - rcDay.Width)
	                {
	                    rowNo++;
	                    rcDay.X = rc.X;
	                    rcDay.Y = rcDay.Y + iRowHeight + DEF_ROWS_MARGIN;
	                }
	            }
	        }

	        #endregion

	        #region Post-Day Padding

	        //Draw next month starting days as disabled
	        int endDay;
	        brush = Brushes.Gray;

	        if (firstDay != 0)
	            endDay = numDays + 1;
	        else
	            endDay = numDays;

	        for (int i = endDay; i < 42; i++)
	        {
	            if (rowNo > 6)
	                break;

                string disabledDay = toFarsi.Convert((i - endDay + 1).ToString(), DefaultCulture);
                g.DrawString(disabledDay, Font, brush, rcDay, OneLineNoTrimming);
	            
	            if (IsRightToLeftCulture)
	            {
	                rcDay.X = rcDay.X - iColWidth;

	                if (rcDay.X < 0)
	                {
	                    rowNo++;
	                    rcDay.X = rcHead.X;
	                    rcDay.Y = rcDay.Y + iRowHeight + DEF_ROWS_MARGIN;
	                }
	            }
	            else
	            {
	                rcDay.X = rcDay.X + iColWidth;

	                if (rcDay.X > rc.Width - rcDay.Width)
	                {
	                    rowNo++;
	                    rcDay.X = rc.X;
	                    rcDay.Y = rcDay.Y + iRowHeight + DEF_ROWS_MARGIN;
	                }
	            }
	        }

	        #endregion
	    }

	    protected override void OnResize(EventArgs e)
	    {
	        if (Width < 166)
	            Width = 166;

	        if (Height < 166)
	            Height = 166;

	        rectsCreated = false;
	        Invalidate();
	    }

        private int GetFirstDayOfWeek(DateTime date)
        {
            if (DefaultCulture.Equals(FALocalizeManager.FarsiCulture))
            {
                PersianDate pd = date;
                return (int)pd.DayOfWeek;
            }
            else if (DefaultCulture.Equals(FALocalizeManager.ArabicCulture))
            {
                return (int)date.DayOfWeek;
            }
            else
            {
                return (int)date.DayOfWeek;
            }
        }

	    private void AddActiveRect(Rectangle rc, TRectangleAction action, object tag)
	    {
	        if (rectsCreated == false)
	        {
	            rects.Add(new ActRect(rc, action, tag));
	        }
	    }

	    private void AddActiveRect(Rectangle rc, TRectangleAction action)
	    {
	        if (rectsCreated == false)
	        {
	            rects.Add(new ActRect(rc, action));
	        }
	    }

	    #endregion

	    #region Overrides

	    internal void OnRecalculateRequired()
	    {
	        ResetAllRectangleStates();

	        bool bRecreate = true;
	        if (rectsCreated && bRecreate)
	            rectsCreated = false;

	        ActRect rect = FindActiveRectByTag(iDay);
	        if (iLastFocused < DEF_TODAY_TAB_INDEX)
	            iLastFocused = iDay;

	        if (rect != null)
	            rect.State |= TRectangleStatus.FocusSelect;
	    }

	    /// <summary>
	    /// Scrolls days in the view to the Left.
	    /// </summary>
	    public void ScrollDaysLeft()
	    {
	        if (iLastFocused < DEF_TODAY_TAB_INDEX)
	            SelectedDateTime = DefaultCalendar.AddDays(SelectedDateTime, -1);
	    }

	    /// <summary>
	    /// Scrolls days in the view to the Right.
	    /// </summary>
	    public void ScrollDaysRight()
	    {
	        if (iLastFocused < DEF_TODAY_TAB_INDEX)
	            SelectedDateTime = DefaultCalendar.AddDays(SelectedDateTime, 1);
	    }

	    /// <summary>
	    /// Scrolls days in the view to the Up.
	    /// </summary>
	    public void ScrollDaysUp()
	    {
	        if (iLastFocused < DEF_TODAY_TAB_INDEX)
	            SelectedDateTime = DefaultCalendar.AddDays(SelectedDateTime, -7);
	    }

	    /// <summary>
	    /// Scrolls days in the view to the Down.
	    /// </summary>
	    public void ScrollDaysDown()
	    {
	        if (iLastFocused < DEF_TODAY_TAB_INDEX)
	            SelectedDateTime = DefaultCalendar.AddDays(SelectedDateTime, 7);
	    }

	    internal void SetFocusOnNextControl()
	    {
	        ResetFocusedRectangleState();

	        if (iLastFocused < DEF_TODAY_TAB_INDEX)
	        {
	            iLastFocused = DEF_TODAY_TAB_INDEX;
	        }
	        else if (iLastFocused == DEF_TODAY_TAB_INDEX)
	        {
	            iLastFocused = DEF_NONE_TAB_INDEX;
	        }
	        else
	        {
	            Control ctrl = FindForm().GetNextControl(this, true);
	            if (ctrl != null) ctrl.Focus();
	        }
	    }

	    internal void SetFocusOnPrevControl()
	    {
	        ResetFocusedRectangleState();

	        if (iLastFocused < DEF_TODAY_TAB_INDEX)
	        {
	            iLastFocused = DEF_NONE_TAB_INDEX;
	        }
	        else if (iLastFocused == DEF_TODAY_TAB_INDEX && iDay != 0)
	        {
	            iLastFocused = iDay;

	            ActRect rc = FindActiveRectByTag(iDay);
	            if (rc != null)
	            {
	                rc.State |= TRectangleStatus.Focused | TRectangleStatus.Selected;
	            }
	        }
	        else if (iLastFocused == DEF_NONE_TAB_INDEX)
	        {
	            iLastFocused = DEF_TODAY_TAB_INDEX;
	        }
	        else
	        {
	            Control ctrl = FindForm().GetNextControl(this, false);
	            if (ctrl != null) ctrl.Focus();
	        }
	    }

	    /// <summary>
	    /// Changes the Year value to the Next Year.
	    /// </summary>
	    public void ToNextYear()
	    {
            try
            {
                SelectedDateTime = DefaultCalendar.AddYears(SelectedDateTime, 1);
                OnRecalculateRequired();
            }
	        catch(ArgumentOutOfRangeException)
	        {
	        }
	    }

	    /// <summary>
	    /// Changes the Year value to the Previous Year.
	    /// </summary>
	    public void ToPrevYear()
	    {
            try
            {
                SelectedDateTime = DefaultCalendar.AddYears(SelectedDateTime, -1);
                OnRecalculateRequired();
            }
	        catch(ArgumentOutOfRangeException)
	        {
	        }
	    }

	    /// <summary>
	    /// Changes the Month value to the Next Month.
	    /// </summary>
	    public void ToNextMonth()
	    {
            try
            {
                SelectedDateTime = DefaultCalendar.AddMonths(SelectedDateTime, 1);
                OnRecalculateRequired();
            }
	        catch(ArgumentOutOfRangeException)
	        {
	        }
	    }

	    /// <summary>
	    /// Changes the Month value to the Previous Month.
	    /// </summary>
	    public void ToPrevMonth()
	    {
            try
            {
                SelectedDateTime = DefaultCalendar.AddMonths(SelectedDateTime, -1);
                OnRecalculateRequired();
            }
	        catch(ArgumentOutOfRangeException)
	        {
	        }
	    }

	    private void RecalculateSelectionUp()
	    {
	        SelectedDateTime = dtSelected.AddDays(-7);

	        if (SelectedDateTime.Month != dtSelected.Month)
	        {
	            ScrollDaysUp();
	        }
	        else
	        {
	            ResetFocusedRectangleState();
	        }
	    }

	    private void RecalculateSelectionDown()
	    {
	        SelectedDateTime = dtSelected.AddDays(7);

	        if (SelectedDateTime.Month != dtSelected.Month) // switch to another month
	        {
	            ScrollDaysDown();
	        }
	        else
	        {
	            ResetFocusedRectangleState();
	        }
	    }

	    private void RecalculateSelectionLeft()
	    {
	        SelectedDateTime = dtSelected.AddDays(-1);

	        if (SelectedDateTime.Month != dtSelected.Month) // switch to another month
	        {
	            ScrollDaysLeft();
	        }
	        else
	        {
	            ResetFocusedRectangleState();
	        }
	    }

	    private void RecalculateSelectionRight()
	    {
	        SelectedDateTime = dtSelected.AddDays(1);

	        if (SelectedDateTime.Month != dtSelected.Month) // switch to another month
	        {
	            ScrollDaysRight();
	        }
	        else
	        {
	            ResetFocusedRectangleState();
	        }
	    }

	    private void OnRectangleClick(ActRect rc)
	    {
	        switch (rc.Action)
	        {
	            case TRectangleAction.MonthDown:
	                ToPrevMonth();
	                break;
	            case TRectangleAction.MonthUp:
	                ToNextMonth();
	                break;
	            case TRectangleAction.YearDown:
	                ToPrevYear();
	                break;
	            case TRectangleAction.YearUp:
	                ToNextYear();
	                break;
	            case TRectangleAction.TodayBtn:
	                iLastFocused = DEF_TODAY_TAB_INDEX;
	                SetTodayDay();
	                OnButtonClicked(new CalendarButtonClickedEventArgs(FAMonthViewButtons.Today));
	                break;
	            case TRectangleAction.NoneBtn:
	                iLastFocused = DEF_NONE_TAB_INDEX;
	                SetNoneDay();
                    OnButtonClicked(new CalendarButtonClickedEventArgs(FAMonthViewButtons.None));
	                break;
	            case TRectangleAction.MonthDay:
	                if (iDay == 0) return;

	                int index = (int) rc.Tag;
	                iLastFocused = index;

	                SelectedDateTime = new DateTime(iYear, iMonth, index, 0, 0, 0, DefaultCalendar);
	                IsNull = false;

	                if (IsMultiSelect)
	                {
	                    if (!isNull && !SelectedDateRange.Contains(SelectedDateTime))
	                    {
	                        SelectedDateRange.Add(SelectedDateTime);
	                        if (!selectedRects.Contains(rc.Tag))
	                            selectedRects.Add(rc.Tag);
	                    }
	                }

                    OnButtonClicked(new CalendarButtonClickedEventArgs(FAMonthViewButtons.MonthDay));
	                break;
	        }

	        Invalidate();
	    }

	    private void OnSelectionClick(ActRect rc)
	    {
	        if (rc.Action == TRectangleAction.MonthDay)
	        {
	            if (rc.IsSelected == false) 
	            {
	                rc.State |= TRectangleStatus.Selected;

	                SelectedDateTime = new DateTime(iYear, iMonth, (int)rc.Tag, 0, 0, 0, DefaultCalendar);
	                if (!isNull && !SelectedDateRange.Contains(SelectedDateTime))
	                {
	                    SelectedDateRange.Add(SelectedDateTime);

	                    if (!selectedRects.Contains(rc.Tag))
	                        selectedRects.Add(rc.Tag);
	                }
	            }
	            else
	            {
	                rc.State = (TRectangleStatus) ((int) rc.State & ~(int) TRectangleStatus.Selected);
	                selectedRects.Remove(rc.Tag);
	            }

	            iLastFocused = (int) rc.Tag;
	            isNull = false;
	        }

	        Invalidate();
	    }

        private void OnSelectionCollectionChanged(object sender, CollectionChangedEventArgs e)
        {
            ResetSelectedRectangleState();
            selectedRects.Clear();

            if (SelectedDateRange.Count > 0)
            {
                SelectedDateTime = SelectedDateRange[0];
            }
            else
            {
                isNull = true;
            }

            OnRecalculateRequired();
            Invalidate();
            OnSelectedDateRangeChanged(EventArgs.Empty);
        }

	    private void OnEnterPressed()
	    {
	        ResetSelectedRectangleState();

	        ActRect rect = FindActiveRectByTag(iLastFocused);

	        if (rect != null)
	        {
	            switch (rect.Action)
	            {
	                case TRectangleAction.TodayBtn:
	                    SetTodayDay();
	                    break;

	                case TRectangleAction.NoneBtn:
	                    SetNoneDay();
	                    break;
	            }
	        }
	    }

	    #endregion

	    #region Public Methods

	    [Browsable(false)]
	    public CultureInfo DefaultCulture
	    {
	        get
	        {
	            if (FALocalizeManager.CustomCulture != null)
                    return FALocalizeManager.CustomCulture;

	            if (Thread.CurrentThread.CurrentUICulture.Equals(FALocalizeManager.FarsiCulture))
	                return FALocalizeManager.FarsiCulture;
	            else if (Thread.CurrentThread.CurrentUICulture.Equals(FALocalizeManager.ArabicCulture))
	                return FALocalizeManager.ArabicCulture;
	            else
	                return FALocalizeManager.InvariantCulture;
	        }
	        set
	        {
                FALocalizeManager.CustomCulture = value;
	            rectsCreated = false;
                iYear = DefaultCalendar.GetYear(selectedGregorianDate);
	        }
	    }

	    private bool IsRightToLeftCulture
	    {
	        get { return DefaultCulture.TextInfo.IsRightToLeft; }
	    }

	    private Calendar GetDefaultCalendar()
	    {
	        if (customCalendar != null)
	            return customCalendar;

	        if (DefaultCulture.Equals(FALocalizeManager.FarsiCulture))
	            return pc;
	        else if (DefaultCulture.Equals(FALocalizeManager.ArabicCulture))
	            return hc;
	        else
	            return gc;
	    }

	    /// <summary>
	    /// Current Month name shown in the view.
	    /// </summary>
	    [Browsable(false)]
	    public string CurrentMonthName
	    {
	        get
	        {
	            if(DefaultCulture.Equals(FALocalizeManager.FarsiCulture))
	                return PersianDate.PersianMonthNames.Default[iMonth];
	            else 
	                return DefaultCulture.DateTimeFormat.MonthGenitiveNames[iMonth - 1].ToUpper();
	        }
	    }

	    /// <summary>
	    /// Gets the abbreviated name of the specified day.
	    /// </summary>
	    /// <param name="day"></param>
	    /// <returns></returns>
	    public string GetAbbrDayName(int day)
	    {
            if (FALocalizeManager.CustomCulture != null)
            {
                if (FALocalizeManager.CustomCulture.Equals(FALocalizeManager.FarsiCulture))
                {
                    return PersianDate.PersianWeekDayAbbr.Default[day];
                }
                else
                {
                    return FALocalizeManager.CustomCulture.DateTimeFormat.ShortestDayNames[day].ToUpper();
                }
            }
	        
	        if (DefaultCulture.Equals(FALocalizeManager.FarsiCulture))
	            return PersianDate.PersianWeekDayAbbr.Default[day];
	        else if (DefaultCulture.Equals(FALocalizeManager.ArabicCulture))
	            return FALocalizeManager.ArabicCulture.DateTimeFormat.AbbreviatedDayNames[day].Substring(2, 1);
	        else
	            return FALocalizeManager.InvariantCulture.DateTimeFormat.AbbreviatedDayNames[day].Substring(0, 1);
	    }

	    /// <summary>
	    /// Clears the selection of the control. Also clears any selected date in MultiSelect mode. 
	    /// </summary>
	    public void SetNoneDay()
	    {
	        IsNull = true;
	        SelectedDateRange.Clear();
            OnSelectedDateTimeChanged(EventArgs.Empty);
	    }

	    /// <summary>
	    /// Sets the selection value to Today.
	    /// </summary>
	    public void SetTodayDay()
	    {
	        SelectedDateRange.Clear();
            SelectedDateTime = DateTime.Now;
	        IsNull = false;
	        SelectedDateRange.Add(SelectedDateTime);
	    }

	    #endregion

	    #region Custom Events
	    
	    protected internal virtual void OnButtonClicked(CalendarButtonClickedEventArgs e)
	    {
            if (ButtonClicked != null)
                ButtonClicked(this, e);
	    }

        protected internal virtual void OnMonthChanged(EventArgs e)
	    {
	        if(MonthChanged != null)
	            MonthChanged(this, e);
	    }

        protected internal virtual void OnYearChanged(EventArgs e)
	    {
	        if(YearChanged != null)
	            YearChanged(this, e);
	    }

        protected internal virtual void OnDayChanged(EventArgs e)
	    {
	        if(DayChanged != null)
	            DayChanged(this, e);
	    }

        protected internal virtual void OnSelectedDateTimeChanged(EventArgs e)
	    {
	        if (SelectedDateTimeChanged != null)
	            SelectedDateTimeChanged(this, e);
	    }

        protected internal virtual void OnSelectedDateRangeChanged(EventArgs e)
	    {
	        if(SelectedDateRangeChanged != null)
	            SelectedDateRangeChanged(this, e);
	    }
	    
	    protected internal virtual void OnDrawCurrentDay(CustomDrawDayEventArgs e)
	    {
            if (DrawCurrentDay != null)
                DrawCurrentDay(this, e);
	    }
	    
	    #endregion

	    #region Helper Methods

	    private ActRect FindActiveRectByPoint(Point pnt)
	    {
	        foreach (ActRect rc in rects)
	        {
	            if (rc.Rect.Contains(pnt))
	                return rc;
	        }

	        return null;
	    }

	    private ActRect FindActiveRectByTag(object tag)
	    {
	        foreach (ActRect rect in rects)
	        {
	            if (rect.Tag != null && rect.Tag.Equals(tag))
	                return rect;
	        }

	        return null;
	    }

	    private void ResetActiveRectanglesState()
	    {
	        foreach (ActRect rc in rects)
	        {
	            if ((rc.State & TRectangleStatus.Active) > 0)
	            {
	                rc.State = (TRectangleStatus) ((int) rc.State & ~(int) TRectangleStatus.Active);
	            }
	        }
	    }

	    private void ResetSelectedRectangleState()
	    {
	        foreach (ActRect rc in rects)
	        {
	            if ((rc.State & TRectangleStatus.Selected) > 0)
	            {
	                rc.State = (TRectangleStatus) ((int) rc.State & ~(int) TRectangleStatus.Selected);
	            }
	        }
	    }

	    private void ResetFocusedRectangleState()
	    {
	        foreach (ActRect rc in rects)
	        {
	            if ((rc.State & TRectangleStatus.Focused) > 0)
	            {
	                rc.State = (TRectangleStatus) ((int) rc.State & ~(int) TRectangleStatus.Focused);
	            }
	        }
	    }

	    private void ResetAllRectangleStates()
	    {
	        foreach (ActRect rc in rects)
	        {
	            rc.State = TRectangleStatus.Normal;
	        }
	    }

	    #endregion

	    #region Mouse Events

	    protected override void OnMouseEnter(EventArgs e)
	    {
	        Point pnt = MousePosition;
	        pnt = PointToClient(pnt);
	        btnTodayActive = false;
	        btnNoneActive = false;

	        ResetActiveRectanglesState();

	        ActRect rect = FindActiveRectByPoint(pnt);

	        if (rect != null && rect.Action != TRectangleAction.WeekDay)
	        {
	            rect.State |= TRectangleStatus.Active;
	        }

	        if (rect != null && rect.Action == TRectangleAction.TodayBtn)
	        {
	            btnTodayActive = true;
	        }

	        if (rect != null && rect.Action == TRectangleAction.NoneBtn)
	        {
	            btnNoneActive = true;
	        }

	        Invalidate();
	    }

	    protected override void OnMouseLeave(EventArgs e)
	    {
	        btnNoneActive = false;
	        btnTodayActive = false;

	        ResetActiveRectanglesState();
	    }

	    protected override void OnMouseWheel(MouseEventArgs e)
	    {
	        if (e.Delta < 0)
	        {
	            switch(ScrollOption)
	            {
	                case ScrollOptionTypes.Day:
                        ScrollDaysLeft();
                        break;
	                    
	                case ScrollOptionTypes.Month:
	                    ToNextMonth();
	                    break;
	                    
	                case ScrollOptionTypes.Year:
	                    ToNextYear();
	                    break;
	            }
	        }
	        else
	        {
                switch (ScrollOption)
                {
                    case ScrollOptionTypes.Day:
                        ScrollDaysRight();
                        break;
                        
                    case ScrollOptionTypes.Month:
                        ToPrevMonth();
                        break;
                        
                    case ScrollOptionTypes.Year:
                        ToPrevYear();
                        break;
                }
	        }
	    }

	    protected override void OnMouseMove(MouseEventArgs e)
	    {
	        Point pnt = MousePosition;
	        pnt = PointToClient(pnt);
	        btnTodayActive = false;
	        btnNoneActive = false;

	        ResetActiveRectanglesState();

	        ActRect rect = FindActiveRectByPoint(pnt);

	        if (rect != null && rect.Action != TRectangleAction.WeekDay)
	        {
	            rect.State |= TRectangleStatus.Active;
	        }

	        if (rect != null && rect.Action == TRectangleAction.TodayBtn)
	        {
	            btnTodayActive = true;
	        }

	        if (rect != null && rect.Action == TRectangleAction.NoneBtn)
	        {
	            btnNoneActive = true;
	        }

	        Invalidate();
	    }

	    private void OnInternalLocalizeChanged(object sender, EventArgs e)
	    {
	        OnRecalculateRequired();
	        Repaint();
	    }
	    
	    internal void OnInternalMouseDown()
	    {
	        OnRecalculateRequired();

	        Point pnt = MousePosition;
	        pnt = PointToClient(pnt);
	        ActRect rect = FindActiveRectByPoint(pnt);

	        if (rect != null && rect.Action != TRectangleAction.WeekDay)
	        {
	            if (iLastFocused == DEF_NONE_TAB_INDEX)
	            {
	                rect.State |= TRectangleStatus.Pressed;
	            }
	            if (iLastFocused == DEF_TODAY_TAB_INDEX)
	            {
	                rect.State |= TRectangleStatus.Pressed;
	            }
	        }
	    }

	    internal void OnInternalMouseClick(Point location)
	    {
	        if (!IsPopupMode)
	            Focus();

	        ActRect rect = FindActiveRectByPoint(location);

	        if (rect != null && rect.Action != TRectangleAction.WeekDay)
	        {
	            ResetActiveRectanglesState();
	            ResetFocusedRectangleState();

	            // if selection begin
	            if ((ModifierKeys & (Keys.Control | Keys.Shift)) == 0)
	            {
	                selectedRects.Clear();
	                selectedDateRange.Clear();
	                ResetSelectedRectangleState();
	                OnRectangleClick(rect);
	            }
	            else
	            {
	                if (!selectedRects.Contains(rect))
	                    selectedRects.Add(rect);

	                OnSelectionClick(rect);
	            }
	        }
	    }

	    /// <summary>
	    /// Returns index of the ActRect control on click, if the HitTest is true. Returns -1 if hitting was not successfull.
	    /// </summary>
	    /// <param name="location"></param>
	    /// <returns></returns>
	    public int HitTest(Point location)
	    {
	        ActRect rect = FindActiveRectByPoint(location);
	        if (rect != null)
	        {
	            return (int)rect.Tag;
	        }
	        else
	        {
	            return -1;
	        }
	    }

	    protected override void OnMouseDown(MouseEventArgs e)
	    {
	        if(e.Button == MouseButtons.Left)
	            OnInternalMouseDown();

	        base.OnMouseDown(e);
	    }

	    protected override void OnMouseClick(MouseEventArgs e)
	    {
	        OnInternalMouseClick(e.Location);

	        base.OnMouseClick(e);
	    }

	    protected override void OnDoubleClick(EventArgs e)
	    {
	        Point pnt = MousePosition;
	        pnt = PointToClient(pnt);

	        ActRect rect = FindActiveRectByPoint(pnt);

	        if (rect != null && rect.Action != TRectangleAction.WeekDay)
	        {
	            ResetActiveRectanglesState();
	            ResetSelectedRectangleState();
	            ResetFocusedRectangleState();

	            OnRectangleClick(rect);
	        }

	        base.OnDoubleClick(e);
	    }

	    #endregion

	    #region Keyboard And Focus Event Handlers

	    protected override void OnGotFocus(EventArgs e)
	    {
	        Invalidate();
	    }

	    protected override void OnLostFocus(EventArgs e)
	    {
	        ResetFocusedRectangleState();
	        Invalidate();
	    }

	    protected override void OnKeyDown(KeyEventArgs e)
	    {
	        switch (e.Modifiers & (Keys.Alt | Keys.Control | Keys.Shift))
	        {
	                //Only Shift key is pressed
	            case Keys.Shift:
	                switch (e.KeyCode)
	                {
	                    case Keys.Tab:
	                        SetFocusOnPrevControl();
	                        break;
	                    case Keys.Down:
	                        RecalculateSelectionDown();
	                        break;
	                    case Keys.Up:
	                        RecalculateSelectionUp();
	                        break;
	                    case Keys.Left:
	                        RecalculateSelectionLeft();
	                        break;
	                    case Keys.Right:
	                        RecalculateSelectionRight();
	                        break;
	                }
	                break;

	                //Only Alt key is pressed
	            case Keys.Alt:
	                switch (e.KeyCode)
	                {
	                    case Keys.Left:
	                        ToPrevMonth();
	                        break;
	                    case Keys.Right:
	                        ToNextMonth();
	                        break;
	                    case Keys.N:
	                        SetNoneDay();
	                        break;
	                    case Keys.T:
	                        SetTodayDay();
	                        break;
	                }
	                break;

	                //Only Control key is pressed
	            case Keys.Control:
	                switch (e.KeyCode)
	                {
	                    case Keys.Up:
	                        ToNextYear();
	                        break;
	                    case Keys.Down:
	                        ToPrevYear();
	                        break;
	                }
	                break;

	            default:
	                switch (e.KeyCode)
	                {
	                    case Keys.Down:
	                        if (iLastFocused == DEF_TODAY_TAB_INDEX || iLastFocused == DEF_NONE_TAB_INDEX)
	                            SetFocusOnNextControl();
	                        else
	                            ScrollDaysDown();
	                        break;
	                    case Keys.Up:
	                        if (iLastFocused == DEF_TODAY_TAB_INDEX || iLastFocused == DEF_NONE_TAB_INDEX)
	                            SetFocusOnPrevControl();
	                        else
	                            ScrollDaysUp();
	                        break;
	                    case Keys.Left:
	                        if (iLastFocused == DEF_TODAY_TAB_INDEX || iLastFocused == DEF_NONE_TAB_INDEX)
	                            SetFocusOnPrevControl();
	                        else
	                            ScrollDaysLeft();
	                        break;
	                    case Keys.Right:
	                        if (iLastFocused == DEF_TODAY_TAB_INDEX || iLastFocused == DEF_NONE_TAB_INDEX)
	                            SetFocusOnNextControl();
	                        else
	                            ScrollDaysRight();
	                        break;
	                    case Keys.Tab:
	                        SetFocusOnNextControl();
	                        break;

	                    case Keys.Space:
	                    case Keys.Enter:
	                        OnEnterPressed();
	                        break;

	                }
	                break;
	        }

	        base.OnKeyDown(e);
	        Invalidate();
	    }

	    #endregion

	    #region Designer Methods

        /// <summary>
        /// Determines to serialize Size property or not.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeSize()
        {
            return false;
        }

        /// <summary>
        /// Determines to serialize SelectedDateTime property or not.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeSelectedDateTime()
        {
            return !SelectedDateTime.Equals(DateTime.MinValue);
        }

        /// <summary>
        /// Determines to serialize SelectedDateRange property or not.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeSelectedDateRange()
        {
            return SelectedDateRange != null && SelectedDateRange.Count > 0;
        }

        /// <summary>
        /// Determines to serialize DefaultCalendar property or not.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDefaultCalendar()
        {
            return false;
        }

        /// <summary>
        /// Determines to serialize DefaultCulture property or not.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDefaultCulture()
        {
            return false;
        }
	    
	    #endregion
	}
}