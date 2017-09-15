using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace Pictaurus
{

    public partial class ScreenForm : Form, IMessageFilter
    {
        #region Preview API's

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion

        bool IsPreviewMode = false;
        ConsolidatedViewer _masterViewer;
        Font errorFont = new Font("Courier New", 14, FontStyle.Bold);

        public Panel DisplayPanel { get { return displayPanel; } }

        public ScreenForm(Rectangle bounds, ConsolidatedViewer masterViewer)
        {
            _masterViewer = masterViewer;
            SetupScreenSaver();
            InitializeComponent();
            this.Bounds = bounds;
            Application.AddMessageFilter(this);
            this.FormClosed += (s, e) => Application.RemoveMessageFilter(this); 
        }

        public ScreenForm(IntPtr PreviewHandle, ConsolidatedViewer masterViewer)
        {
            _masterViewer = masterViewer;
            InitializeComponent();
            //set the preview window as the parent of this window  
            SetParent(this.Handle, PreviewHandle);

            //make this a child window, so when the select screensaver dialog closes, this will also close  
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            //set our window's size to the size of our window's new parent  
            Rectangle ParentRect;
            GetClientRect(PreviewHandle, out ParentRect);
            this.Size = ParentRect.Size;

            //set our location at (0, 0)  
            this.Location = new Point(0, 0);

            IsPreviewMode = true;
            buttonClose.Visible = false;
        }

        private void SetupScreenSaver()
        {
            // Use double buffering to improve drawing performance  
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            // Capture the mouse  
            this.Capture = true;

            // Set the application to full screen mode and hide the mouse  
            //Cursor.Hide();
            //Bounds = Screen.PrimaryScreen.Bounds;
            //WindowState = FormWindowState.Maximized;
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = false;
            DoubleBuffered = true;
            //BackgroundImageLayout = ImageLayout.Stretch;
        } 

        private void ScreenForm_MouseClick(object sender, MouseEventArgs e)
        {
            //Application.Exit();
        }

        private void displayPanel_Paint(object sender, PaintEventArgs e)
        {
            _masterViewer.DrawMe(sender);
        }

        private void displayPanel_MouseClick(object sender, MouseEventArgs e)
        {
            //Application.Exit();
        }

        private void ScreenForm_KeyDown(object sender, KeyEventArgs e)
        {
            //_masterViewer.KeyDown(sender, e);
        }

        static bool shiftIsPressed;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Debug.WriteLine("POST: " + msg.Msg + "," + msg.LParam + "," + msg.WParam + "," + msg.Result + "," + keyData + "(" + (int)keyData + ")");
            //_masterViewer.HandleKeyCommand(keys
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            _masterViewer.SaveState();
            Application.Exit();
        }

        private void ScreenForm_Load(object sender, EventArgs e)
        {
            if (_masterViewer == null)
            {
                MessageBox.Show("Unable to set up display correctly.");
                Application.Exit();
            }
            else
            {
                _masterViewer.AddForm(this);
                _masterViewer.BackgroundColor = this.BackColor;
            }
        }

        const int WM_KEYUP = 0x0101;
        const int WM_KEYDOWN = 0x0100;
        const int WM_TIMER = 0x0113;
        static Dictionary<int, bool> _keyDownState = new Dictionary<int, bool>();
        static ScreenForm()
        {
            foreach (int key in Enum.GetValues(typeof(Keys)))
            {
                if(!_keyDownState.ContainsKey(key)) _keyDownState.Add(key, false);
            }
        }

        public bool PreFilterMessage(ref Message msg)
        {
            int wParam = (int)msg.WParam;
            if(msg.Msg == WM_KEYDOWN)
            {
                //Debug.WriteLine("DOWN: " + ((Keys)wParam)); 
                _keyDownState[wParam] = true;
                _masterViewer.HandleKeyCommand((Keys)wParam,
                    _keyDownState[(int)Keys.LShiftKey] | _keyDownState[(int)Keys.RShiftKey] | _keyDownState[(int)Keys.Shift] | _keyDownState[(int)Keys.ShiftKey],
                    _keyDownState[(int)Keys.Control] | _keyDownState[(int)Keys.ControlKey] | _keyDownState[(int)Keys.LControlKey] | _keyDownState[(int)Keys.RControlKey],
                    _keyDownState[(int)Keys.Alt]);

                return true;
            }
            else if(msg.Msg == WM_KEYUP)
            {
                //Debug.WriteLine("UP:   " + ((Keys)wParam));
                _keyDownState[wParam] = false;     
                return true;
            }
            else if (msg.Msg == WM_TIMER)
            {
                //Debug.WriteLine("TIMER:  " + msg.Msg + "," + msg.LParam + "," + msg.WParam + "," + msg.Result);
            }
        //}
        //    {
                
        //        Debug.WriteLine("PRE:  " + msg.Msg + "," + msg.LParam + "," + msg.WParam + "," + msg.Result);
        //        return true;
        //    }
            return false;
        }

        private void buttonResume_Click(object sender, EventArgs e)
        {
            _masterViewer.Resume();
            buttonResume.Visible = false;
        }

        internal void Resume()
        {
            buttonResume.Visible = false; 
        }

        internal void Pause()
        {
            buttonResume.Visible = true;
        }

        internal Bitmap GetCurrentBitmap()
        {
            return _currentBitmap;
        }

        Bitmap _currentBitmap;
        internal void SetBitmap(string bitmapPath)
        {
            try
            {
                if (_currentBitmap != null) _currentBitmap.Dispose();
                _currentBitmap = null;
                _currentBitmap = new Bitmap(bitmapPath);
            }
            catch (Exception e)
            {
                if (_currentBitmap == null)
                {
                    try
                    {
                        _currentBitmap = new Bitmap(1700, 800);
                        Graphics g = Graphics.FromImage(_currentBitmap);
                        g.Clear(Color.Red);
                        g.DrawString(e.ToString(), errorFont, Brushes.Yellow, 10, 10, StringFormat.GenericTypographic);
                    }
                    catch (Exception) { }
                }
            }
        }

        private void displayPanel_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void displayPanel_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
