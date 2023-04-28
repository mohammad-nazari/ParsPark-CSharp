using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using FarsiLibrary.Win.Design;
using FarsiLibrary.Win.Drawing;
using FarsiLibrary.Win.Enums;

namespace FarsiLibrary.Win.BaseClasses
{
    /// <summary>
    /// Base class for all controls, which provides painting functionality bases on selected theme.
    /// </summary>
    [ToolboxItem(false)]
    public class BaseStyledControl : Control
    {
        #region Fields

        private ThemeTypes theme;
        private static FAPainterOffice2000 PainterOffice2000;
        private static FAPainterOffice2003 PainterOffice2003;
        private static FAPainterWindowsXP PainterWinXP;
        private static ToolStripProfessionalRenderer renderer;
        private int lockUpdate;

        #endregion

        #region Events

        /// <summary>
        /// Fired when current theme changes.
        /// </summary>
        public event EventHandler ThemeChanged;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates static painter objects for each of available Themes.
        /// </summary>
        static BaseStyledControl()
        {
            PainterOffice2000 = new FAPainterOffice2000();
            PainterOffice2003 = new FAPainterOffice2003();
            PainterWinXP = new FAPainterWindowsXP();
            renderer = new ToolStripProfessionalRenderer();
        }

        /// <summary>
        /// Creates a new instance of BaseStyledControl class.
        /// </summary>
        public BaseStyledControl()
        {
            // Set painting style for better performance.
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);

            FAThemeManager.ManagerThemeChanged += new EventHandler(OnInternalManagerThemeChanged);

            if (UseThemes)
                Office2003Colors.Default.Init();

            if (!DesignMode && FAThemeManager.UseGlobalThemes)
                Theme = FAThemeManager.Theme;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Locks the control for update.
        /// </summary>
        public void BeginUpdate()
        {
            lockUpdate++;
        }

        /// <summary>
        /// Removes a update lock from control.
        /// </summary>
        public void EndUpdate()
        {
            lockUpdate--;
        }

        /// <summary>
        /// Cancels all previous locks on the control. Does NOT repaint the control.
        /// </summary>
        public void CancelUpdate()
        {
            lockUpdate = 0;
        }

        /// <summary>
        /// Decides if the user is updatable or in lock mode.
        /// </summary>
        [Browsable(false)]
        public bool CanUpdate
        {
            get { return lockUpdate == 0; }
        }

        /// <summary>
        /// Invalidate and repaints the control if it is not in lock mode.
        /// </summary>
        public void Repaint()
        {
            if (CanUpdate)
                Invalidate();
        }

        /// <summary>
        /// Painter object which helps control paint itself on the screen, based on the current selected theme.
        /// </summary>
        [Browsable(false)]
        public IFAPainter Painter
        {
            get
            {
                if (!UseThemes || Theme == ThemeTypes.Office2000)
                    return PainterOffice2000;
                else if (UseThemes && Theme == ThemeTypes.Office2003)
                    return PainterOffice2003;
                else if (UseThemes && Theme == ThemeTypes.WindowsXP)
                    return PainterWinXP;
                else
                    return PainterOffice2000;
            }
        }

        protected override void OnSystemColorsChanged(EventArgs e)
        {
            base.OnSystemColorsChanged(e);
            UpdateRenderer();
            Invalidate();
        }

        protected virtual void OnThemeChanged(EventArgs e)
        {
            if (ThemeChanged != null)
                ThemeChanged(this, e);

            Repaint();
        }

        private void UpdateRenderer()
        {
            if (!UseThemes || Theme == ThemeTypes.Office2000)
            {
                renderer.ColorTable.UseSystemColors = true;
            }
            else
            {
                renderer.ColorTable.UseSystemColors = false;
            }
        }

        private void OnInternalManagerThemeChanged(object sender, EventArgs e)
        {
            Theme = FAThemeManager.Theme;
        }

        #endregion

        #region Props

        /// <summary>
        /// Displays the about form of the control when in Design-Mode.
        /// </summary>
        [DesignOnly(true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [ParenthesizePropertyName(true)]
        [Editor(typeof(AboutDialogEditor), typeof(UITypeEditor))]
        public object About
        {
            get { return null; }
        }

        [Browsable(false)]
        internal ProfessionalColorTable ColorTable
        {
            get { return renderer.ColorTable; }
        }

        [Browsable(false)]
        internal bool IsRightToLeft
        {
            get
            {
                return RightToLeft == RightToLeft.Yes;
            }
        }

        [Browsable(false)]
        internal ToolStripProfessionalRenderer ToolStripRenderer
        {
            get { return renderer; }
        }

        /// <summary>
        /// Checks if the control can paint itself using styles. Styles are only available on WindowsXP or 
        /// greater, and should be enabled by the developer, using <see cref="Application.RenderWithVisualStyles">RenderWithVisualStyles</see> property of <see cref="Application">Application</see> class.
        /// </summary>
        [Browsable(false)]
        public bool UseThemes
        {
            get
            {
                return FAThemeManager.UseThemes;
            }
        }

        /// <summary>
        /// Currently selected theme.
        /// </summary>
        [DefaultValue(typeof(ThemeTypes), "Office2000")]
        public ThemeTypes Theme
        {
            get 
            {
                if (UseThemes == false)
                    theme = ThemeTypes.Office2000;

                return theme; 
            }
            set
            {
                if (theme == value)
                    return;

                if (!UseThemes)
                    theme = ThemeTypes.Office2000;
                else
                    theme = value;

                UpdateRenderer();
                OnThemeChanged(EventArgs.Empty);
            }
        }

        #endregion

        #region Protected Methods

        protected virtual void OnThemeChanged()
        {
            if (ThemeChanged != null)
                ThemeChanged(this, EventArgs.Empty);

            Repaint();
        }

        #endregion

        #region Overrides

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (!DesignMode && Theme != FAThemeManager.Theme && FAThemeManager.UseGlobalThemes)
                Theme = FAThemeManager.Theme;
        }
        
        #endregion
    }
}
