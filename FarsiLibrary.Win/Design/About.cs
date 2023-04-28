using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Timer=System.Timers.Timer;

namespace FarsiLibrary.Win.Design
{
    /// <summary>
    /// About form
    /// </summary>
    [Browsable(false)]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class About : Form
    {
        private Timer m_fadeinTimer;
        private Button btnClose;
        private Label label1;
        private Label label2;
        private ListBox lst;
        private bool fade;
        private bool m_fadeInFlag = false;

        public About(bool doFade) : this()
        {
            if(doFade)
            {
                fade = doFade;
                m_fadeinTimer = new Timer();
                m_fadeinTimer.Elapsed += new ElapsedEventHandler(m_fadeinTimer_Elapsed);
            }
        }
        
        /// <summary>
        /// Default constructor for <bottom>About</bottom> form.
        /// </summary>
        public About()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Loads default values on form startup.
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!DesignMode && fade)
            {
                m_fadeInFlag = true;
                Opacity = 0;
                m_fadeinTimer.Enabled = true;
            }
        }

        /// <summary>
        /// Run when user closes the form.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (e.Cancel)
                return;

            if(!fade)
                return;
            
            if (Opacity > 0)
            {
                m_fadeInFlag = false;
                m_fadeinTimer.Enabled = true;
                e.Cancel = true;
            }
        }

        private void InitializeComponent()
        {
            btnClose = new Button();
            label1 = new Label();
            label2 = new Label();
            lst = new ListBox();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.Anchor = (((AnchorStyles.Bottom | AnchorStyles.Left)));
            btnClose.Location = new Point(8, 152);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(75, 24);
            btnClose.TabIndex = 0;
            btnClose.Text = "خروج";
            btnClose.Click += new EventHandler(btnClose_Click);
            // 
            // label1
            // 
            label1.Location = new Point(24, 32);
            label1.Name = "label1";
            label1.Size = new Size(368, 23);
            label1.TabIndex = 1;
            label1.Text = "کاری از : هادی اسکندری (H.Eskandari@GMail.Com)";
            // 
            // label2
            // 
            label2.Location = new Point(24, 8);
            label2.Name = "label2";
            label2.Size = new Size(368, 23);
            label2.TabIndex = 2;
            label2.Text = "کنترل های فارسی NET.";
            // 
            // lst
            // 
            lst.ItemHeight = 14;
            lst.Location = new Point(16, 56);
            lst.Name = "lst";
            lst.RightToLeft = RightToLeft.No;
            lst.Size = new Size(376, 88);
            lst.TabIndex = 3;
            // 
            // About
            // 
            AutoScaleBaseSize = new Size(6, 15);
            ClientSize = new Size(408, 182);
            ControlBox = false;
            Controls.Add(lst);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnClose);
            Font = new Font("Tahoma", 9F, FontStyle.Regular,GraphicsUnit.Point, 0);
            Name = "About";
            RightToLeft = RightToLeft.Yes;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "درباره...";
            Load += new EventHandler(About_Load);
            ResumeLayout(false);
        }

        private void m_fadeinTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(!fade)
            {
                m_fadeinTimer.Enabled = false;
                return;
            }
            
            // How should we fade?
            if (m_fadeInFlag == false)
            {
                Opacity -= (m_fadeinTimer.Interval/500.0);

                // Should we continue to fade?
                if (Opacity > 0)
                    m_fadeinTimer.Enabled = true;
                else
                {
                    m_fadeinTimer.Enabled = false;
                    Close();
                } // End else we should close the form.
            } // End if we should fade in.
            else
            {
                Opacity += (m_fadeinTimer.Interval/500.0);
                m_fadeinTimer.Enabled = (Opacity < 1.0);
                m_fadeInFlag = (Opacity < 1.0);
            } // End else we should fade out.
        }

        private void About_Load(object sender, EventArgs e)
        {
            Assembly[] assemblies = Thread.GetDomain().GetAssemblies();
            lst.Items.Clear();

            foreach (Assembly asm in assemblies)
            {
                if (asm.GetName().Name.StartsWith("FarsiLibrary"))
                {
                    string itemName = asm.GetName().Name + " " + asm.GetName().Version.ToString();
                    if (!lst.Items.Contains(itemName))
                        lst.Items.Add(itemName);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}