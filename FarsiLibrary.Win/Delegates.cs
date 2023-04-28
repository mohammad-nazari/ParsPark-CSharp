using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FarsiLibrary.Win.Enums;
using FarsiLibrary.Win.FAPopup;

namespace FarsiLibrary.Win.Events
{
    #region BindPopupControlEventArgs

    public class BindPopupControlEventArgs : EventArgs
    {
        #region Class members

        private Control parent;
        private IPopupControl popupControl;

        #endregion

        #region Props

        public IPopupControl BindedControl
        {
            get { return popupControl; }
            set { popupControl = value; }
        }

        public Control OwnerControl
        {
            get { return parent; }
        }

        #endregion

        #region Ctor

        private BindPopupControlEventArgs()
        {
        }

        public BindPopupControlEventArgs(Control owner)
        {
            parent = owner;
        }

        #endregion
    }

    #endregion

    #region CloseDropDownEventArgs

    public class CloseDropDownEventArgs : EventArgs
    {
        #region Fields

        private bool close;
        private Keys keyCode;

        #endregion

        #region Props

        public bool Close
        {
            get { return close; }
            set { close = value; }
        }

        public Keys KeyCode
        {
            get { return keyCode; }
        }

        #endregion

        #region Ctor

        private CloseDropDownEventArgs()
        {
        }

        public CloseDropDownEventArgs(bool close)
        {
            this.close = close;
        }

        public CloseDropDownEventArgs(bool close, Keys keycode)
        {
            this.close = close;
            keyCode = keycode;
        }

        #endregion
    }

    #endregion

    #region PopupClosedEventArgs

    /// <summary>
    /// Contains event information events.
    /// </summary>
    public class PopupClosedEventArgs : EventArgs
    {
        /// <summary>
        /// The popup form.
        /// </summary>
        private Form popup = null;

        /// <summary>
        /// Gets the popup form which is being closed.
        /// </summary>
        public Form Popup
        {
            get
            {
                return popup;
            }
        }

        /// <summary>
        /// Constructs a new instance of this class for the specified
        /// popup form.
        /// </summary>
        /// <param name="popup">Popup Form which is being closed.</param>
        public PopupClosedEventArgs(Form popup)
        {
            this.popup = popup;
        }
    }

    #endregion

    #region PopupCancelEventArgs

    /// <summary>
    /// Provides a reference to the popup form that is to be closed and 
    /// allows the operation to be cancelled.
    /// </summary>
    public class PopupCancelEventArgs : EventArgs
    {
        /// <summary>
        /// Whether to cancel the operation
        /// </summary>
        private bool cancel = false;

        /// <summary>
        /// Mouse down location
        /// </summary>
        private Point location;

        /// <summary>
        /// Popup form.
        /// </summary>
        private Form popup = null;

        /// <summary>
        /// Constructs a new instance of this class.
        /// </summary>
        /// <param name="popupForm">The popup form</param>
        /// <param name="pt">The mouse location, if any, where the
        /// mouse event that would cancel the popup occured.</param>
        public PopupCancelEventArgs(Form popupForm, Point pt)
        {
            popup = popupForm;
            location = pt;
            cancel = false;
        }

        /// <summary>
        /// Gets the popup form
        /// </summary>
        public Form Popup
        {
            get
            {
                return popup;
            }
        }

        /// <summary>
        /// Gets the location that the mouse down which would cancel this 
        /// popup occurred
        /// </summary>
        public Point CursorLocation
        {
            get
            {
                return location;
            }
        }

        /// <summary>
        /// Gets/sets whether to cancel closing the form. Set to
        /// <c>true</c> to prevent the popup from being closed.
        /// </summary>
        public bool Cancel
        {
            get
            {
                return cancel;
            }
            set
            {
                cancel = value;
            }
        }
    }

    #endregion

    #region SelectedDateRangeChangedEventArgs

    /// <summary>
    /// FarsiCalendarEvents fired by calendar controls when the currently selected Date changes.
    /// </summary>
    public class SelectedDateRangeChangedEventArgs : EventArgs
    {
        #region Fields

        private List<DateTime> selectedDates;

        #endregion

        #region Ctor

        /// <summary>
        /// Delegate fire when currentItem Date changes in the control.
        /// </summary>
        /// <param name="SelectedDates"></param>
        public SelectedDateRangeChangedEventArgs(List<DateTime> SelectedDates)
        {
            selectedDates = SelectedDates;
        }

        #endregion

        #region Props

        /// <summary>
        /// Currently selected Date in the control.
        /// </summary>
        public List<DateTime> SelectedDateRange
        {
            get { return selectedDates; }
        }

        #endregion
    }

    #endregion

    #region SelectedDateTimeChangingEventArgs

    public class SelectedDateTimeChangingEventArgs : EventArgs
    {
        private bool cancel;
        private string message;
        private DateTime oldValue;
        private DateTime newValue;

        public SelectedDateTimeChangingEventArgs(DateTime OldValue, DateTime NewValue)
        {
            oldValue = OldValue;
            newValue = NewValue;
            cancel = false;
            message = string.Empty;
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        
        public DateTime NewValue
        {
            get { return newValue; }
            set { newValue = value; }
        }

        public DateTime OldValue
        {
            get { return oldValue; }
        }

        public bool Cancel
        {
            get { return cancel; }
            set { cancel = value; }
        }
    }

    #endregion

    #region ValueValidatingEventArgs

    public class ValueValidatingEventArgs : EventArgs
    {
        private bool hasError = false;
        private object value = null;

        public ValueValidatingEventArgs(object Value)
        {
            value = Value;
        }

        public bool HasError
        {
            get { return hasError; }
            set { hasError = value; }
        }

        public object Value
        {
            get { return value; }
        }
    }

    #endregion

    #region CustomDrawDayEventArgs
    
    public class CustomDrawDayEventArgs : EventArgs
    {
        private bool handled;
        private bool isToday;
        private Rectangle r;
        private Graphics g;
        private int dayNo;
        private int yearNo;
        private int monthNo;
        
        public CustomDrawDayEventArgs(Rectangle rectangle, Graphics graphics, int year, int month, int day, bool today)
        {
            dayNo = day;
            yearNo = year;
            monthNo = month;
            r = rectangle;
            g = graphics;
            isToday = today;
            handled = false;
        }

        public bool Handled
        {
            get { return handled; }
            set { handled = value; }
        }

        public Rectangle Rectangle
        {
            get { return r; }
        }

        public Graphics Graphics
        {
            get { return g; }
        }

        public int Day
        {
            get { return dayNo; }
        }

        public int Year
        {
            get { return yearNo; }
        }

        public int Month
        {
            get { return monthNo; }
        }

        public bool IsToday
        {
            get { return isToday; }
        }
    }
    
    #endregion

    #region CalendarButtonClickedEventArgs
    
    public class CalendarButtonClickedEventArgs : EventArgs
    {
        #region Fields

        private FAMonthViewButtons button;
        
        #endregion

        #region Ctor

        public CalendarButtonClickedEventArgs(FAMonthViewButtons button)
        {
            this.button = button;
        }

        #endregion

        #region Props

        public FAMonthViewButtons Button
        {
            get { return button; }
        }

        #endregion
    }

    #endregion

    #region CollectionChangedEventArgs

    /// <summary>
    /// Specifies change type of the collection.
    /// </summary>
    public class CollectionChangedEventArgs : EventArgs
    {
        private CollectionChangeType changeType = CollectionChangeType.Other;

        public CollectionChangedEventArgs(CollectionChangeType type)
        {
            changeType = type;
        }

        public CollectionChangeType ChangeType
        {
            get { return changeType; }
        }
    }

    #endregion

    internal delegate int Hook(int ncode, IntPtr wParam, IntPtr lParam);
    public delegate void CalendarButtonClickedEventHandler(object sender, CalendarButtonClickedEventArgs e);
    public delegate void BindPopupControlEventHandler(object sender, BindPopupControlEventArgs e);
    public delegate void ValueValidatingEventHandler(object sender, ValueValidatingEventArgs e);
    public delegate void SelectedDateTimeChangingEventHandler(object sender, SelectedDateTimeChangingEventArgs e);
    public delegate void ControlChangedEventHandler(object sender, EventArgs e);
    public delegate void CloseDropDownEventHandler(object sender, CloseDropDownEventArgs e);
    public delegate void PopupClosedEventHandler(object sender, PopupClosedEventArgs e);
    public delegate void PopupCancelEventHandler(object sender, PopupCancelEventArgs e);
    public delegate void CustomDrawDayEventHandler(object sender, CustomDrawDayEventArgs e);
}
