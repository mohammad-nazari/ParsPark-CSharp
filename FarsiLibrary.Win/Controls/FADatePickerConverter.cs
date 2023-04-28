using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FarsiLibrary.Resources;
using FarsiLibrary.Utils;
using FarsiLibrary.Win.Design;
using FarsiLibrary.Win.Enums;
using FarsiLibrary.Win.Events;

namespace FarsiLibrary.Win.Controls
{
    /// <summary>
    /// A special date picker control which converts selected dates' value into other cultures.
    /// </summary>
    [ToolboxItem(true)]
    [DefaultEvent("SelectedDateTimeChanged")]
    [DefaultProperty("SelectedDateTime")]
    [Designer(typeof(FADatePickerDesigner))]
    public class FADatePickerConverter : FAContainerComboBox
    {
        #region Fields

        private FAMonthViewContainer mv;
        private Button convert;
        private int DEF_CONVERT_WIDTH = 20;
        private CalendarTypes calType;
        private Image buttonImage;
        private DateTime selectedDateTime;

	    #endregion

        #region Events

        /// <summary>
        /// Fires when SelectedDateTime property is changed.
        /// </summary>
        public event EventHandler SelectedDateTimeChanged;

        /// <summary>
        /// Fires when SelectedDateTime property is changing. Use properties of EventHandler class to specify validation of the SelectedDateTime.
        /// </summary>
        public event SelectedDateTimeChangingEventHandler SelectedDateTimeChanging;

        /// <summary>
        /// Fires when user clicks on convert button of the control.
        /// </summary>
        public event EventHandler ConvertButtonClicked;
        
        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new instance of FADatePickerConverter class.
        /// </summary>
        public FADatePickerConverter()
        {
            RightToLeftChanged += new EventHandler(OnInternalRightToLeftChanged);
            ThemeChanged += new EventHandler(OnInternalThemeChanged);
            PopupShowing += new EventHandler(OnInternalPopupShowing);
            mv = new FAMonthViewContainer(this);
            mv.MonthViewControl.SelectedDateTimeChanged += new EventHandler(OnMVSelectedDateTimeChanged);
            mv.MonthViewControl.ButtonClicked += new CalendarButtonClickedEventHandler(OnMVButtonClicked);
            FALocalizeManager.LocalizerChanged += new EventHandler(OnInternalLocalizerChanged);
            FAThemeManager.ManagerThemeChanged += new EventHandler(OnInternalManagerThemeChanged);
            convert = new Button();
            convert.Size = new Size(DEF_CONVERT_WIDTH, Height - 4);
            convert.Image = ButtonImage;
            convert.TabStop = false;
            convert.Click += new EventHandler(OnConvertButtonClick);
            convert.GotFocus += new EventHandler(OnConvertButtonFocus);
            Controls.Add(convert);

            FormatInfo = FormatInfoTypes.ShortDate;
            
            if (mv.MonthViewControl.DefaultCulture.Equals(mv.MonthViewControl.PersianCulture))
            {
                calType = CalendarTypes.Persian;
            }
            else
            {
                calType = CalendarTypes.English;
            }

            Text = FALocalizeManager.GetLocalizerByCulture(Thread.CurrentThread.CurrentUICulture).GetLocalizedString(StringID.Validation_NullText);
            UpdateButtons();
        }

        #endregion

        #region Props

        /// <summary>
        /// Determines if the MonthViewControl has selected the value.
        /// </summary>
        [DefaultValue(true)]
        [Description("Determines if the MonthViewControl has selected the value.")]
        public bool IsNull
        {
            get { return mv.MonthViewControl.IsNull; }
            set
            {
                mv.MonthViewControl.IsNull = value;
                UpdateTextValue();
            }
        }

        /// <summary>
        /// Determines which value should be changed when user scroll's mouse wheel on the <see cref="FAMonthView"/> control.
        /// </summary>
        [DefaultValue(typeof(ScrollOptionTypes), "Month")]
        [Description("Determines scrolling option of the FAMonthView control.")]
        public ScrollOptionTypes ScrollOption
        {
            get { return mv.MonthViewControl.ScrollOption; }
            set { mv.MonthViewControl.ScrollOption = value; }
        }

        /// <summary>
        /// Specifies if the mouse is over the control and the control is in Hot state.
        /// </summary>
        [DefaultValue(false)]
        [Browsable(false)]
        [Description("Specifies if the mouse is over the control and the control is in Hot state.")]
        public override bool IsHot
        {
            get
            {
                return base.IsHot || convert.Bounds.Contains(PointToClient(MousePosition));
            }
            set
            {
                base.IsHot = value;
            }
        }

        /// <summary>
        /// Specifies Text / Image relation on Convert button.
        /// </summary>
        [Description("Specifies Text / Image relation on Convert button.")]
        [DefaultValue(TextImageRelation.ImageBeforeText)]
        public TextImageRelation TextImageRelation
        {
            get { return convert.TextImageRelation; }
            set { convert.TextImageRelation = value; }
        }

        /// <summary>
        /// Text of the convert button.
        /// </summary>
        [DefaultValue("")]
        [Description("Text of the convert button.")]
        public string ButtonText
        {
            get 
            {
                if (convert == null) return string.Empty;
                return convert.Text; 
            }
            set
            {
                if (value == null) convert.Text = string.Empty;
                convert.Text = value;
            }
        }

        /// <summary>
        /// Width of the Convert Button.
        /// </summary>
        [Browsable(false)]
        [DefaultValue(20)]
        internal int ButtonWidth
        {
            get { return DEF_CONVERT_WIDTH; }
            set
            {
                DEF_CONVERT_WIDTH = value;
                UpdateButtons();
            }
        }

        /// <summary>
        /// Image of Convert button which shows up on Convert Button.
        /// </summary>
        [Description("Image of Convert button which shows up on Convert Button.")]
        [DefaultValue(null)]
        public Image ButtonImage
        {
            get { return buttonImage; }
            set
            {
                if (buttonImage == value)
                    return;

                if (convert == null)
                    return;

                if (value is Bitmap)
                    ((Bitmap)value).MakeTransparent();

                buttonImage = value;
                convert.Image = value;
            }
        }

        /// <summary>
        /// Calendar type of the control which changes by pressing the Convert button.
        /// </summary>
        [DefaultValue(CalendarTypes.Persian)]
        [Browsable(false)]
        public CalendarTypes CalendarType
        {
            get { return calType; }
            set
            {
                if (calType == value)
                    return;

                calType = value;
            }
        }

        /// <summary>
        /// Selected value of the control as a <see cref="DateTime"/> instance.
        /// </summary>
        [Bindable(true)]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.All)]
        [Description("Selected value of the control as a DateTime instance.")]
        public DateTime SelectedDateTime
        {
            get { return selectedDateTime; }
            set
            {
                if (selectedDateTime == value)
                    return;

                //Validating
                ValueValidatingEventArgs validateArgs = new ValueValidatingEventArgs(Text);//.ToString(IncludesTime, IncludesSecond));
                OnValueValidating(validateArgs);
                if (validateArgs.HasError)
                    return;

                DateTime oldValue = selectedDateTime;
                DateTime newValue = value;
                
                SelectedDateTimeChangingEventArgs changeArgs = new SelectedDateTimeChangingEventArgs(oldValue, newValue);
                OnSelectedDateTimeChanging(changeArgs);

                if (changeArgs.Cancel)
                {
                    if (string.IsNullOrEmpty(changeArgs.Message))
                        Error.SetError(this, FALocalizeManager.GetLocalizerByCulture(Thread.CurrentThread.CurrentUICulture).GetLocalizedString(StringID.Validation_Cancel));
                    else
                        Error.SetError(this, changeArgs.Message);

                    return;
                }

                if (!string.IsNullOrEmpty(changeArgs.Message))
                {
                    Error.SetError(this, changeArgs.Message);
                }
                else
                {
                    Error.SetError(this, string.Empty);
                }

                //No errors, proceed
                mv.MonthViewControl.SelectedDateTime = changeArgs.NewValue;
            }
        }

        #endregion

        #region EventHandling

        private void OnInternalManagerThemeChanged(object sender, EventArgs e)
        {
            Theme = FAThemeManager.Theme;
        }

        private void OnInternalRightToLeftChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void OnMVSelectedDateTimeChanged(object sender, EventArgs e)
        {
            SetSelectedDateTime(mv.MonthViewControl.SelectedDateTime);
        }

        private void OnMVButtonClicked(object sender, CalendarButtonClickedEventArgs e)
        {
            HideDropDown();
        }

        private void OnConvertButtonFocus(object sender, EventArgs e)
        {
            FocusNextControl();
        }

        private void OnInternalLocalizerChanged(object sender, EventArgs e)
        {
            UpdateTextValue();
        }

        private void OnInternalPopupShowing(object sender, EventArgs e)
        {
            mv.MonthViewControl.Theme = Theme;
            ValueValidatingEventArgs args = new ValueValidatingEventArgs(Text);
            OnValueValidating(args);
        }

        private void OnInternalThemeChanged(object sender, EventArgs e)
        {
            if (Theme == ThemeTypes.Office2000)
            {
                convert.FlatStyle = FlatStyle.Flat;
                convert.BackColor = SystemColors.Control;
            }
            else
            {
                convert.FlatStyle = FlatStyle.Standard;
                convert.UseVisualStyleBackColor = true;
            }
        }

        private void OnConvertButtonClick(object sender, EventArgs e)
        {
            OnConvertButtonClicked(EventArgs.Empty);
            
            if (Text == null || Text == FALocalizeManager.GetLocalizerByCulture(Thread.CurrentThread.CurrentUICulture).GetLocalizedString(StringID.Validation_NullText) || Text.Length == 0)
                return;

            if (CalendarType == CalendarTypes.Persian)
            {
                CalendarType = CalendarTypes.English;
            }
            else
            {
                CalendarType = CalendarTypes.Persian;
            }

            try
            {
                if (CalendarType == CalendarTypes.Persian)
                {
                    mv.MonthViewControl.DefaultCalendar = mv.MonthViewControl.PersianCalendar;
                    mv.MonthViewControl.DefaultCulture = mv.MonthViewControl.PersianCulture;
                    DateTime dt = SelectedDateTime;
                    
                    Text = ((PersianDate)dt).ToString(GetFormatByFormatInfo(FormatInfo));
                }
                else
                {
                    mv.MonthViewControl.DefaultCalendar = mv.MonthViewControl.InvariantCalendar;
                    mv.MonthViewControl.DefaultCulture = mv.MonthViewControl.InvariantCulture;
                    DateTime dt = SelectedDateTime;

                    Text = dt.ToString(GetFormatByFormatInfo(FormatInfo), mv.MonthViewControl.DefaultCulture);
                }
            }
            catch (FormatException)
            {
                Text = FALocalizeManager.GetLocalizerByCulture(Thread.CurrentThread.CurrentUICulture).GetLocalizedString(StringID.Validation_NullText);
                mv.MonthViewControl.SetNoneDay();
            }
        }

        protected virtual void OnSelectedDateTimeChanging(SelectedDateTimeChangingEventArgs e)
        {
            e.Cancel = false;

            if (SelectedDateTimeChanging != null)
                SelectedDateTimeChanging(this, e);
        }

        protected virtual void OnConvertButtonClicked(EventArgs e)
        {
            if (ConvertButtonClicked != null)
                ConvertButtonClicked(this, e);
        }
        
        protected override void OnBindingPopupControl(BindPopupControlEventArgs e)
        {
            e.BindedControl = mv;
            base.OnBindingPopupControl(e);
        }

        protected virtual void OnSelectedDateTimeChanged(EventArgs e)
        {
            if (SelectedDateTimeChanged != null)
                SelectedDateTimeChanged(this, e);
        }

        private void UpdateButtons()
        {
            SetPosTextBox();
            
            Rectangle r2 = GetContentRect();

            if (convert == null)
                return;

            convert.Width = DEF_CONVERT_WIDTH;
            convert.Height = Height - 4;
            convert.BringToFront();
            
            if (!IsRightToLeft)
            {
                convert.Location = new Point(r2.Left - 2, (Height - convert.Height) / 2);
            }
            else
            {
                convert.Location = new Point(r2.Width + 2, (Height - convert.Height) / 2);
            }
        }

        private void SetSelectedDateTime(DateTime dt)
        {
            DateTime oldValue = selectedDateTime;
            DateTime newValue = dt;

            SelectedDateTimeChangingEventArgs changeArgs = new SelectedDateTimeChangingEventArgs(oldValue, newValue);
            OnSelectedDateTimeChanging(changeArgs);

            if (changeArgs.Cancel)
            {
                if (string.IsNullOrEmpty(changeArgs.Message))
                    Error.SetError(this, FALocalizeManager.GetLocalizerByCulture(Thread.CurrentThread.CurrentUICulture).GetLocalizedString(StringID.Validation_Cancel));
                else
                    Error.SetError(this, changeArgs.Message);

                return;
            }

            if (!string.IsNullOrEmpty(changeArgs.Message))
            {
                Error.SetError(this, changeArgs.Message);
            }
            else
            {
                Error.SetError(this, string.Empty);
            }

            selectedDateTime = dt;
            OnSelectedDateTimeChanged(EventArgs.Empty);

            UpdateTextValue();
        }

        protected override void SetPosTextBox(Rectangle content)
        {
            try
            {
                if (UseThemes && (Theme == ThemeTypes.WindowsXP || Theme == ThemeTypes.Office2003))
                {
                    TextBox.Top = (Height - TextBox.Height + 2) / 2;
                    TextBox.Size = new Size(content.Width - 4 - DEF_CONVERT_WIDTH, content.Height);
                    TextBox.Left = content.Left;
                }
                else
                {
                    TextBox.Top = (Height + 2 - TextBox.Height) / 2;
                    TextBox.Size = new Size(content.Width - 2 - DEF_CONVERT_WIDTH, content.Height);
                    TextBox.Left = content.Left;
                }
            }
            catch { }

            if (!IsRightToLeft)
                TextBox.Left += DEF_CONVERT_WIDTH;

            Repaint();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateButtons();
        }

        /// <summary>
        /// Updates text representation of the selected value
        /// </summary>
        public override void UpdateTextValue()
        {
            if (mv.MonthViewControl.IsNull)
            {
                Text = FALocalizeManager.GetLocalizerByCulture(Thread.CurrentThread.CurrentUICulture).GetLocalizedString(StringID.Validation_NullText);
            }
            else
            {
                if (mv.MonthViewControl.DefaultCulture.Equals(mv.MonthViewControl.PersianCulture))
                {
                    Text = ((PersianDate)SelectedDateTime).ToString(GetFormatByFormatInfo(FormatInfo));
                }
                else
                {
                    Text = SelectedDateTime.ToString(GetFormatByFormatInfo(FormatInfo), mv.MonthViewControl.DefaultCulture);
                }
            }
        }

        protected override void OnValidating(CancelEventArgs e)
        {
            ValueValidatingEventArgs args = new ValueValidatingEventArgs(Text);
            OnValueValidating(args);
            e.Cancel = args.HasError;
            
            base.OnValidating(e);
        }

        protected override void OnValueValidating(ValueValidatingEventArgs e)
        {
            try
            {
                string txt = e.Value.ToString();
                if (string.IsNullOrEmpty(txt) || txt == FALocalizeManager.GetLocalizerByCulture(Thread.CurrentThread.CurrentUICulture).GetLocalizedString(StringID.Validation_NullText))
                {
                    e.HasError = false;
                }
                else if (mv.MonthViewControl.DefaultCulture.Equals(mv.MonthViewControl.PersianCulture))
                {
                    PersianDate pd = PersianDate.Parse(txt, GetFormatByFormatInfo(FormatInfo));
                    e.HasError = false;
                    mv.MonthViewControl.IsNull = false;
                    mv.MonthViewControl.SelectedDateTime = pd;
                }
                else if (mv.MonthViewControl.DefaultCulture.Equals(mv.MonthViewControl.InvariantCulture))
                {
                    DateTime dt = DateTime.Parse(txt);
                    e.HasError = false;
                    mv.MonthViewControl.IsNull = false;
                    mv.MonthViewControl.SelectedDateTime = dt;
                }
            }
            catch (Exception)
            {
                e.HasError = true;
                mv.MonthViewControl.IsNull = true;
            }
        }

        #endregion

        #region ShouldSerialize and Reset

        /// <summary>
        /// Decides to serialize SelectedDateTime or not.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeSelectedDateTime()
        {
            return SelectedDateTime != DateTime.MinValue;
        }

        /// <summary>
        /// Decides to serialize CalendarType or not.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeCalendarType()
        {
            return false;
        }

        /// <summary>
        /// Decides to serialize ButtonImage or not.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeButtonImage()
        {
            return ButtonImage != null;
        }

        /// <summary>
        /// Resets ButtonImage to default value.
        /// </summary>
        public void ResetButtonImage()
        {
            ButtonImage = null;
        }

        /// <summary>
        /// Rests SelectedDateTime to default value.
        /// </summary>
        public void ResetSelectedDateTime()
        {
            selectedDateTime = DateTime.MinValue;
            IsNull = true;
        }
        
        #endregion
    }
}
