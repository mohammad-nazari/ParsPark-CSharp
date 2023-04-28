using System;
using System.ComponentModel;
using System.Threading;
using FarsiLibrary.Resources;
using FarsiLibrary.Utils;
using FarsiLibrary.Win.Design;
using FarsiLibrary.Win.Enums;
using FarsiLibrary.Win.Events;

namespace FarsiLibrary.Win.Controls
{
    /// <summary>
    /// A datepicker control which can select date in <see cref="System.Globalization.GregorianCalendar"/>, <see cref="PersianCalendar" /> and <see cref="System.Globalization.HijriCalendar"/> based on current thread's Culture and UICulture. 
    /// 
    /// To know how to display the control in other cultures and calendars, please see <see cref="FAMonthView"/> control's documentation.
    /// </summary>
    [ToolboxItem(true)]
    [DefaultEvent("SelectedDateTimeChanged")]
    [DefaultProperty("SelectedDateTime")]
    [Designer(typeof(FADatePickerDesigner))]
    [DefaultBindingProperty("SelectedDateTime")]
    public class FADatePicker : FAContainerComboBox
    {
        #region Fields

        private DateTime selectedDateTime;
        internal FAMonthViewContainer mv;

	    #endregion

        #region Events

        /// <summary>
        /// Fires when SelectedDateTime property of the control changes.
        /// </summary>
        public event EventHandler SelectedDateTimeChanged;

        /// <summary>
        /// Fires when SelectedDateTime property of the control is changing.
        /// </summary>
        public event SelectedDateTimeChangingEventHandler SelectedDateTimeChanging;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new instance of FADatePicker class.
        /// </summary>
        public FADatePicker()
        {
            mv = new FAMonthViewContainer(this);
            RightToLeftChanged += new EventHandler(OnInternalRightToLeftChanged);
            mv.MonthViewControl.SelectedDateTimeChanged += new EventHandler(OnMVSelectedDateTimeChanged);
            mv.MonthViewControl.ButtonClicked += new CalendarButtonClickedEventHandler(OnMVButtonClicked);
            FALocalizeManager.LocalizerChanged += new EventHandler(OnInternalLocalizerChanged);

            PopupShowing += new EventHandler(OnInternalPopupShowing);
            Text = FALocalizeManager.GetLocalizerByCulture(Thread.CurrentThread.CurrentUICulture).GetLocalizedString(StringID.Validation_NullText);

            FormatInfo = FormatInfoTypes.ShortDate;
        }

        #endregion

        #region Props
        
        /// <summary>
        /// Determines if the control has not made any selection yet.
        /// </summary>
        [DefaultValue(true)]
        [Description("Determines if the control has not made any selection yet.")]
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
        /// Determinces scrolling option of the FAMonthView control.
        /// </summary>
        [DefaultValue(typeof(ScrollOptionTypes), "Month")]
        [Description("Determinces scrolling option of the FAMonthView control.")]
        public ScrollOptionTypes ScrollOption
        {
            get { return mv.MonthViewControl.ScrollOption; }
            set { mv.MonthViewControl.ScrollOption = value; }
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

        private void OnInternalLocalizerChanged(object sender, EventArgs e)
        {
            UpdateTextValue();
        }

        private void OnInternalRightToLeftChanged(object sender, EventArgs e)
        {
            SetPosTextBox();
        }

        private void OnInternalPopupShowing(object sender, EventArgs e)
        {
            mv.MonthViewControl.Theme = Theme;
            ValueValidatingEventArgs args = new ValueValidatingEventArgs(Text);
            OnValueValidating(args);
        }

        protected override void OnBindingPopupControl(BindPopupControlEventArgs e)
        {
            e.BindedControl = mv;
            base.OnBindingPopupControl(e);
        }

        protected virtual void OnSelectedDateTimeChanging(SelectedDateTimeChangingEventArgs e)
        {
            e.Cancel = false;

            if (SelectedDateTimeChanging != null)
                SelectedDateTimeChanging(this, e);
        }

        protected virtual void OnSelectedDateTimeChanged(EventArgs e)
        {
            if (SelectedDateTimeChanged != null)
                SelectedDateTimeChanged(this, e);
        }

        private void OnMVSelectedDateTimeChanged(object sender, EventArgs e)
        {
            SetSelectedDateTime(mv.MonthViewControl.SelectedDateTime);
        }

        private void OnMVButtonClicked(object sender, CalendarButtonClickedEventArgs e)
        {
            HideDropDown();
        }

        private void SetSelectedDateTime(DateTime dt)
        {
            DateTime oldValue = selectedDateTime;
            DateTime newValue = dt;
            
            SelectedDateTimeChangingEventArgs changeArgs = new SelectedDateTimeChangingEventArgs(oldValue, newValue);
            OnSelectedDateTimeChanging(changeArgs);
            
            if (changeArgs.Cancel)
            {
                if(string.IsNullOrEmpty(changeArgs.Message))
                    Error.SetError(this, FALocalizeManager.GetLocalizerByCulture(Thread.CurrentThread.CurrentUICulture).GetLocalizedString(StringID.Validation_Cancel));
                else
                    Error.SetError(this, changeArgs.Message);
                return;
            }
            
            if(!string.IsNullOrEmpty(changeArgs.Message))
            {
                Error.SetError(this, changeArgs.Message);
            }
            else
            {
                Error.SetError(this, string.Empty);
            }

            selectedDateTime = changeArgs.NewValue;
            OnSelectedDateTimeChanged(EventArgs.Empty);

            UpdateTextValue();
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
                    Text = ((PersianDate) SelectedDateTime).ToString(GetFormatByFormatInfo(FormatInfo));
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
        /// Decides to serialize the SelectedDateTime property or not.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeSelectedDateTime()
        {
            return SelectedDateTime != DateTime.MinValue;
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
