/*
 * Copyright (c) 2015 Mehrzad Chehraz (mehrzady@gmail.com)
 * Released under the MIT License
 * http://chehraz.ir/mit_license
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
namespace Atf.UI {
   using Utility;

   using System;
   using System.ComponentModel;
   using System.Diagnostics;
   using System.Drawing;
   using System.Windows.Forms;
   using System.Windows.Forms.VisualStyles;
   [ToolboxItem(false)]
   public class DropDownControl : Control, IDropDownControl, IMessageFilter {
      #region Fields
      protected static readonly int ArrowSeparatorWidth = 1;
      private static readonly int DefaultWidth = 100;
      private bool droppedDown = false;
      private DropDownControlStyle style = DropDownControlStyle.Editable;
      private ComboBoxState arrowState = ComboBoxState.Normal;
      private Rectangle arrowBounds;
      private Rectangle childBounds;
      private Rectangle frameBounds;
      #endregion

      #region Constructors
      protected DropDownControl() {
         if (Child != null) {
            Controls.Add((Control)Child);
            AttachChildEvents();
         }
         AutoSize = true;
         BackColor = SystemColors.Window;
         ForeColor = SystemColors.WindowText;
         DoubleBuffered = true;
         MeasureBounds();
         if (Child != null) {
            SetChildBounds();
         }
         Width = DefaultWidth;
      }
      #endregion

      #region Properties
      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      [EditorBrowsable(EditorBrowsableState.Advanced)]
      public Rectangle ArrowBounds {
         get {
            return arrowBounds;
         }
         private set {
            arrowBounds = value;
         }
      }
      protected virtual ComboBoxState ArrowState {
         get {
            return arrowState;
         }
         private set {
            if (arrowState != value) {
               arrowState = value;
               Invalidate(ArrowBounds);
            }
         }
      }
      protected virtual int ArrowWidth {
         get {
            return SystemInformation.HorizontalScrollBarArrowWidth;
         }
      }
      [DefaultValue(typeof(Color), "Window")]
      public override Color BackColor {
         get {
            return base.BackColor;
         }
         set {
            if (BackColor != value) {
               base.BackColor = value;
               if (Child != null) {
                  Child.BackColor = value;
               }
            }
         }
      }
      protected virtual Size BorderSize {
         get {
            return SystemInformation.Border3DSize;
         }
      }
      protected virtual IDropDownChild Child {
         get {
            return null;
         }
      }
      protected Rectangle ChildBounds {
         get {
            return childBounds;
         }
         private set {
            childBounds = value;
         }
      }
      private bool DroppedDownInternal {
         set {
            droppedDown = value;
         }
      }
      [DefaultValue(typeof(Color), "WindowText")]
      public override Color ForeColor {
         get {
            return base.ForeColor;
         }
         set {
            if (ForeColor != value) {
               base.ForeColor = value;
            }
         }
      }
      protected virtual Rectangle FrameBounds {
         get {
            return frameBounds;
         }
         private set {
            frameBounds = value;
         }
      }
      protected virtual int PreferredHeight {
         get {
            return SizeUtil.GetCtrlHeight(Font) + 1;
         }
      }
      protected virtual IDropDownPopup Popup {
         get {
            return null;
         }
      }
      #endregion

      #region Methods
      protected virtual void AttachControlEvents() {
         GotFocus += new EventHandler(control_GotFocus);
         LostFocus += new EventHandler(control_LostFocus);
      }
      protected virtual void AttachChildEvents() {
         Child.LostFocus += new EventHandler(control_LostFocus);
      }
      protected virtual void CloseDropDown() {
         if (DroppedDown) {
            DroppedDownInternal = false;
            Popup.Close();
            Popup.LostFocus -= popUp_LostFocus;
            OnDropDownClosed(EventArgs.Empty);

            if (DropDownClosed != null)
               DropDownClosed(this, EventArgs.Empty);
         }
      }
      private void control_GotFocus(object sender, EventArgs e) {
         Invalidate();
      }
      private static ButtonState ComboBoxStateToComboButtonState(ComboBoxState cbs) {
         if (cbs == ComboBoxState.Pressed) {
            return ButtonState.Flat;
         }
         if (cbs == ComboBoxState.Disabled) {
            return ButtonState.Inactive;
         }
         return ButtonState.Normal;
      }
      private void control_LostFocus(object sender, EventArgs e) {
         if (!Popup.ContainsFocus && DroppedDown) {
            CloseDropDown();
         }
         if (Style == DropDownControlStyle.List) {
            Invalidate();
         }
      }
      protected virtual void DetachControlEvents() {
         GotFocus -= control_GotFocus;
         LostFocus -= control_LostFocus;
      }
      protected virtual void DetachChildEvents() {
         Child.LostFocus -= control_LostFocus;
      }
      protected virtual void DrawArrow(PaintEventArgs pe) {
         ComboBoxState arrowState = Enabled ? ArrowState : ComboBoxState.Disabled;
         if (ComboBoxRenderer.IsSupported && Application.RenderWithVisualStyles) {
            ComboBoxRenderer.DrawDropDownButton(pe.Graphics, ArrowBounds, arrowState);
         }
         else {
            ControlPaint.DrawComboButton(pe.Graphics, ArrowBounds,
                                         ComboBoxStateToComboButtonState(arrowState));
         }
      }
      protected virtual void DrawFrame(PaintEventArgs pe) {
         if (ComboBoxRenderer.IsSupported && Application.RenderWithVisualStyles) {
            ComboBoxRenderer.DrawTextBox(pe.Graphics, frameBounds,
                                          Enabled ? ComboBoxState.Normal : ComboBoxState.Disabled);
         }
         else {
            ControlPaint.DrawButton(pe.Graphics, frameBounds, ButtonState.Pushed);
         }
         if (Enabled) {
            using (SolidBrush brush = new SolidBrush(BackColor)) {
               pe.Graphics.FillRectangle(brush, childBounds);
            }
         }
      }
      protected virtual void DrawText(PaintEventArgs pe) {
         TextFormatFlags format = TextFormatFlags.Default;
         Rectangle chldBounds = ChildBounds;
         format |= TextFormatFlags.EndEllipsis;
         format |= TextFormatFlags.SingleLine;
         format |= TextFormatFlags.VerticalCenter;
         if (RightToLeft == RightToLeft.Yes) {
            format |= TextFormatFlags.RightToLeft;
            format |= TextFormatFlags.Right;
         }
         if (Focused && !DroppedDown && Style == DropDownControlStyle.List) {
            pe.Graphics.FillRectangle(SystemBrushes.Highlight, chldBounds);
            ControlPaint.DrawFocusRectangle(pe.Graphics, chldBounds);
            TextRenderer.DrawText(pe.Graphics, Text, Font, chldBounds,
                                  SystemColors.HighlightText, Color.Transparent, format);
         }
         else {
            Color color;
            if (Enabled) {
               using (SolidBrush brush = new SolidBrush(BackColor)) {
                  pe.Graphics.FillRectangle(brush, chldBounds);
               }
               color = ForeColor;
            }
            else {
               color = SystemColors.GrayText;
            }
            TextRenderer.DrawText(pe.Graphics, Text, Font, chldBounds, color, Color.Transparent, format);
         }
      }
      protected virtual void DropDownInternal() {
         if (Popup != null) {
            bool raiseDropDownEvent = !DroppedDown;
            DroppedDownInternal = true;
            OnDropDown(EventArgs.Empty);
            Popup.LostFocus += new EventHandler(popUp_LostFocus);
            Popup.Show(this);
            if (raiseDropDownEvent && DropDown != null) {
               DropDown(this, EventArgs.Empty);
            }
         }
      }
      public override Size GetPreferredSize(Size proposedSize) {
         Size basePreferredSize = base.GetPreferredSize(proposedSize);
         return new Size(basePreferredSize.Width, PreferredHeight);
      }
      protected virtual void MeasureBounds() {
         int controlHeight = PreferredHeight;
         Size borderSize = BorderSize;
         Size arrowSize = new Size(ArrowWidth, Height - 2 * borderSize.Height);
         Rectangle arrBounds = new Rectangle(Point.Empty, arrowSize);
         arrBounds = new Rectangle(Point.Empty, arrowSize);
         Rectangle chldBounds;
         if (RightToLeft != RightToLeft.Yes) {
            arrBounds.Offset(Width - arrowSize.Width - borderSize.Width, borderSize.Height);
            chldBounds = new Rectangle(0, 0, Width - ArrowBounds.Width - ArrowSeparatorWidth, Height);
            chldBounds.Inflate(-borderSize.Width, -borderSize.Height);
         }
         else {
            arrBounds.Offset(borderSize.Width, borderSize.Height);
            chldBounds = new Rectangle(ArrowBounds.Width + ArrowSeparatorWidth, 0,
                                                Width - ArrowBounds.Width, Height);
            chldBounds.Inflate(-borderSize.Width, -borderSize.Height);
         }
         Rectangle frmBounds = new Rectangle(0, 0, Width, Height);

         ArrowBounds = arrBounds;
         ChildBounds = chldBounds;
         FrameBounds = frmBounds;
      }
      protected virtual void OnArrowClicked(EventArgs e) {
         if (Child != null && !ContainsFocus && !((Control)Popup).ContainsFocus) {
            if (Style == DropDownControlStyle.Editable) {
               Child.Focus();
            }
         }
         DroppedDown = !DroppedDown;
      }
      protected override void OnClick(EventArgs e) {
         base.OnClick(e);
         if (Style == DropDownControlStyle.List ||
             ArrowBounds.Contains(PointToClient(Cursor.Position))) {
            OnArrowClicked(EventArgs.Empty);
         }
      }
      private void OnDropDown(EventArgs e) {
         Application.AddMessageFilter(this);
         if (Style == DropDownControlStyle.List) {
            Invalidate(childBounds);
         }
      }
      private void OnDropDownClosed(EventArgs e) {
         Application.RemoveMessageFilter(this);
         if (Style == DropDownControlStyle.List) {
            Invalidate(childBounds);
         }
      }
      protected override void OnEnabledChanged(EventArgs e) {
         base.OnEnabledChanged(e);
         if (Enabled) {
            ArrowState = ComboBoxState.Normal;
         }
         else {
         }
         Invalidate();
      }
      protected override void OnEnter(EventArgs e) {
         base.OnEnter(e);
         if (Style == DropDownControlStyle.Editable && Child != null) {
            Child.Focus();
         }
      }
      protected override void OnGotFocus(EventArgs e) {
         base.OnGotFocus(e);
         if (Style == DropDownControlStyle.Editable && Parent is ContainerControl) {
            ContainerControl container = (ContainerControl)Parent;
            container.SelectNextControl(this, false, true, true, true);
         }
      }
      protected override void OnFontChanged(EventArgs e) {
         base.OnFontChanged(e);
         MeasureBounds();
         int controlHeight = PreferredHeight;
         Height = controlHeight;
         if (Child != null) {
            SetChildBounds();
         }
      }
      protected override void OnMouseDown(MouseEventArgs e) {
         base.OnMouseDown(e);
         if (ClientRectangle.Contains(e.Location) && Style == DropDownControlStyle.List
             && !Focused) {
            Focus();
         }
         if (ArrowBounds.Contains(e.Location)) {
            ArrowState = ComboBoxState.Pressed;
         }
      }
      protected override void OnMouseLeave(EventArgs e) {
         base.OnMouseLeave(e);
         if (!DroppedDown) {
            ArrowState = ComboBoxState.Normal;
         }
      }
      protected override void OnMouseMove(MouseEventArgs e) {
         base.OnMouseMove(e);
         if (ArrowBounds.Contains(e.Location)) {
            if (ArrowState != ComboBoxState.Pressed) {
               ArrowState = ComboBoxState.Hot;
            }
         }
      }
      protected override void OnMouseUp(MouseEventArgs e) {
         base.OnMouseUp(e);
         if (ArrowBounds.Contains(e.Location) || ArrowState == ComboBoxState.Pressed) {
            ArrowState = ComboBoxState.Normal;
         }
      }
      protected override void OnPaint(PaintEventArgs pe) {
         if (pe.ClipRectangle == ArrowBounds) {
            DrawArrow(pe);
            return;
         }
         if (pe.ClipRectangle.Equals(childBounds)) {
            DrawText(pe);
            return;
         }
         DrawFrame(pe);
         DrawArrow(pe);
         DrawText(pe);
      }
      protected override void OnRightToLeftChanged(EventArgs e) {
         base.OnRightToLeftChanged(e);
         if (Child != null) {
            Child.RightToLeft = RightToLeft;
         }
         MeasureBounds();
         if (Child != null) {
            SetChildBounds();
         }
         Invalidate();
      }
      protected override void OnSizeChanged(EventArgs e) {
         base.OnSizeChanged(e);
         MeasureBounds();
         if (Child != null) {
            SetChildBounds();
         }
         Invalidate();
      }
      protected new virtual void OnStyleChanged(EventArgs e) {
         SetChildBounds();
         IDropDownChild child = Child;
         if (style == DropDownControlStyle.List) {
            if (child != null) {
               DetachChildEvents();
               if (child.Focused) {
                  Focus();
               }
               Controls.Remove((Control)child);
            }
            AttachControlEvents();
         }
         else {
            if (child != null) {
               DetachControlEvents();
               Controls.Add((Control)child);
               if (Focused) {
                  child.Focus();
               }
               AttachChildEvents();
            }
         }
      }
      protected override void OnTextChanged(EventArgs e) {
         base.OnTextChanged(e);
         Invalidate(ChildBounds);
      }
      protected override void OnTabStopChanged(EventArgs e) {
         base.OnTabStopChanged(e);
         if (Child != null) {
            Child.TabStop = TabStop;
         }
      }
      private void popUp_LostFocus(object sender, EventArgs e) {
         if (!ContainsFocus && DroppedDown) {
            CloseDropDown();
         }
         if (Style == DropDownControlStyle.List) {
            Invalidate();
         }
      }
      protected override bool ProcessDialogKey(Keys keyData) {
         switch (keyData) {
            case (Keys.Alt | Keys.Down):
               DroppedDown = !DroppedDown;
               return true;

            case Keys.Escape:
               if (DroppedDown) {
                  CloseDropDown();
                  return true;
               }
               break;
         }
         return base.ProcessDialogKey(keyData);
      }
      protected virtual void SetChildBounds() {
         IDropDownChild child = Child;
         if (child != null) {
            if (!child.Bounds.Equals(childBounds)) {
               child.SetBounds(childBounds.X, childBounds.Y, childBounds.Width, childBounds.Height);
            }
         }
      }
      #endregion

      #region IMessageFilter Members

      public bool PreFilterMessage(ref Message m) {
         if (!DroppedDown) {
            Debug.Fail("Problem!! + " + Visible.ToString());
            Application.RemoveMessageFilter(this);
            return false;
         }
         if (!IsMouseMessage(m)) {
            return false;
         }
         bool popUpContainsPoint = Popup.ContainsPoint(Cursor.Position);
         bool popUpContainsWindow = Popup.ContainsWindow(m.HWnd);
         bool thisContainsPoint = ContainsPoint(Cursor.Position);

         switch (m.Msg) {
            case NativeMethods.WM_LBUTTONDOWN:
               if (!popUpContainsPoint) {
                  CloseDropDown();
                  return true;
               }
               break;
            case NativeMethods.WM_MOUSEMOVE:
            case NativeMethods.WM_NCMOUSEMOVE:
               if (!thisContainsPoint) {
                  return true;
               }
               break;
            case NativeMethods.WM_MOUSEWHEEL:
               if (!ContainsWindow(m.HWnd)) {
                  return true;
               }
               break;
            default:
               if (!popUpContainsPoint && !popUpContainsWindow) {
                  CloseDropDown();
                  return false;
               }
               break;
         }
         return false;
      }
      private static bool IsMouseMessage(Message m) {
         bool filterMessage = false;

         if (m.Msg >= NativeMethods.WM_MOUSEFIRST && m.Msg <= NativeMethods.WM_MOUSELAST) {
            filterMessage = true;
         }
         else if (m.Msg >= NativeMethods.WM_NCLBUTTONDOWN && m.Msg <= NativeMethods.WM_NCMBUTTONDBLCLK) {
            filterMessage = true;
         }
         return filterMessage;
      }
      #endregion

      #region IDropDownControl Members
      public event EventHandler DropDown;
      public event EventHandler DropDownClosed;
      [Browsable(false)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public virtual bool DroppedDown {
         get {
            return droppedDown;
         }
         set {
            if (value && !DroppedDown) {
               DropDownInternal();
            }
            else if (!value) {
               CloseDropDown();
            }
         }
      }
      [Browsable(true)]
      [Category("Appearance")]
      [DefaultValue(DropDownControlStyle.Editable)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public DropDownControlStyle Style {
         get {
            return style;
         }
         set {
            if (style != value) {
               style = value;
               OnStyleChanged(EventArgs.Empty);
            }
         }
      }
      public bool ContainsPoint(Point point) {
         return Visible && (RectangleToScreen(Bounds).Contains(point) ||
                Popup.ContainsPoint(point));
      }
      public bool ContainsWindow(IntPtr handle) {
         return handle == Handle || NativeMethods.IsChild(Handle, handle) ||
                          Popup.ContainsWindow(handle);
      }
      public virtual Point GetDropDownLocation() {
         int y = PointToScreen(new Point(0, Height)).Y;
         if (y + Popup.Height > Screen.PrimaryScreen.WorkingArea.Height) {
            y = PointToScreen(new Point(0, 0)).Y - Popup.Height;
         }
         if (RightToLeft != RightToLeft.Yes) {
            return new Point(PointToScreen(new Point(0, 0)).X, y);
         }
         else {
            return new Point(PointToScreen(new Point(-(Popup.Width - Width), 0)).X, y);
         }
      }
      #endregion

   }
}