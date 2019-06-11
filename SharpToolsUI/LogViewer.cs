using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SharpTools;

namespace SharpToolsUI
{
    public partial class LogViewer : UserControl, ILogAppender
    {
        public LogViewer()
        {
            InitializeComponent();

            PanelVisible = true;
            InfoColor = Color.White;
            DebugColor = Color.Gray;
            ErrorColor = Color.OrangeRed;
            SuccessColor = Color.PaleGreen;
            WarnColor = Color.Yellow;
            ShowLink = true;
            LineLimit = 10000;
            FollowTail = true;
        }

        [Category("Logging")]
        public bool PanelVisible
        {
            get { return controlPanel.Visible; }
            set { controlPanel.Visible = value; }
        }
        [Category("Logging")]
        public string LinkText
        {
            get { return linkLabel.Text; }
            set { linkLabel.Text = value; }
        }
        [Category("Logging")]
        public bool ShowLink
        {
            get { return linkLabel.Visible; }
            set { linkLabel.Visible = value; }
        }
        [Category("Logging")]
        public bool PanelOnTop
        {
            get { return controlPanel.Dock == DockStyle.Top; }
            set { controlPanel.Dock = value ? DockStyle.Top : DockStyle.Bottom; }
        }
        [Category("Logging")]
        public Color BackgroundColor
        {
            get { return rtb.BackColor; }
            set { rtb.BackColor = value; }
        }
        [Category("Logging")]
        public Color SuccessColor { get; set; }
        [Category("Logging")]
        public Color ErrorColor { get; set; }
        [Category("Logging")]
        public Color WarnColor { get; set; }
        [Category("Logging")]
        public Color InfoColor { get; set; }
        [Category("Logging")]
        public Color DebugColor { get; set; }
        [Category("Logging")]
        public int LineLimit { get; set; }
        [Category("Logging")]
        public bool ShowDebug
        {
            get { return showDebugCheckBox.Checked; }
            set { showDebugCheckBox.Checked = value; }
        }
        [Category("Logging")]
        public bool FollowTail
        {
            get { return followTailCheckBox.Checked; }
            set { followTailCheckBox.Checked = value; }
        }
        [Category("Logging")]
        public Font LogFont
        {
            get { return rtb.Font; }
            set { rtb.Font = value; }
        }

        public void Append(Log log)
        {
            if (rtb.InvokeRequired)
            {
                rtb.Invoke((Action)(() => { DoAppend(log); }));
            }
            else DoAppend(log);
        }

        public void Clear()
        {
            rtb.Clear();
        }

        public string AllText()
        {
            return rtb.Text;
        }

        void DoAppend(Log log)
        {
            if (ShowDebug || log.Level != LogLevel.DEBUG)
            {
                CheckLimit();
                Color color = LevelColor(log.Level);
                rtb.SelectionLength = 0; //clear selection
                rtb.SelectionStart = rtb.Text.Length;
                rtb.SelectionColor = color;
                rtb.AppendText(log.Line + "\n");
                DoFollowTail();
                if (rtb.Focused) showDebugCheckBox.Focus();
            }
        }

        Color LevelColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.SUCCESS:
                    return SuccessColor;
                case LogLevel.ERROR:
                    return ErrorColor;
                case LogLevel.WARN:
                    return WarnColor;
                case LogLevel.INFO:
                    return InfoColor;
                case LogLevel.DEBUG:
                    return DebugColor;
            }
            return Color.White;
        }

        void CheckLimit()
        {
            var ro = rtb.ReadOnly;
            try
            {
                if (LineLimit > 0)
                {
                    while (rtb.GetFirstCharIndexFromLine(LineLimit) > 0)
                    {
                        rtb.ReadOnly = false; //fails otherwise
                        rtb.SelectionLength = 0; //clear selection
                        rtb.SelectionStart = 0;
                        rtb.SelectionLength = rtb.GetFirstCharIndexFromLine(1);
                        rtb.SelectedText = "";
                    }
                }
            }
            finally
            {
                rtb.ReadOnly = ro;
            }
        }

        void DoFollowTail()
        {
            if (FollowTail)
            {
                rtb.SelectionStart = rtb.Text.Length;
                rtb.ScrollToCaret();
            }
        }

        void FollowTailCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            DoFollowTail();
        }

        void ClearButtonClick(object sender, EventArgs e)
        {
            rtb.Clear();
        }

        void LinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(linkLabel.Text);
        }
    }
}
