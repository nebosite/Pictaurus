using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

namespace Pictaurus
{
    /// <summary>
    /// Consolidates all of the display windows
    /// </summary>
    public class ConsolidatedViewer
    {
        System.Windows.Forms.Timer newPictureTimer = new System.Windows.Forms.Timer();
        List<ScreenForm> forms = new List<ScreenForm>();
        Dictionary<Panel, ScreenForm> formLookup = new Dictionary<Panel, ScreenForm>();
        int _currentForm = 0;
        PictureViewModel viewModel = new PictureViewModel();
        DateTime nextUpdate;
        public Color BackgroundColor { get; set; }
        double _secondsBetweenChange = 5;
        int _resumeSeconds = 120;
        Font pictureFont;

        public ConsolidatedViewer()
        {
            BackgroundColor = Color.Black;
            newPictureTimer.Tick += NewPictureTimerTick;
            newPictureTimer.Interval = 200;
            newPictureTimer.Enabled = true;
            nextUpdate = DateTime.Now.AddSeconds(_secondsBetweenChange);
        }

        void NewPictureTimerTick(object sender, EventArgs e)
        {
            if (initFrames > 0)
            {
                initFrames--;
                nextUpdate = DateTime.MinValue;
            }

            if (!_paused && DateTime.Now > nextUpdate)
            {
                PanelAction(() => viewModel.GoForward(1), false);
            }

            if (_paused)
            {
                if ((DateTime.Now - viewModel.lastTurn).TotalSeconds > _resumeSeconds) Resume();
            }
        }

        private void GoNextPanel()
        {
            if (forms.Count > 0)
            {
                DrawMe(forms[_currentForm].DisplayPanel);
                _currentForm++;
                if (_currentForm >= forms.Count) _currentForm = 0;
            }
            nextUpdate = DateTime.Now.AddSeconds(_secondsBetweenChange);
        }

        internal void DrawMe(object sender)
        {
            Panel displayPanel = sender as Panel;
            ScreenForm parentForm = formLookup[displayPanel];
            Graphics g = Graphics.FromHwnd(displayPanel.Handle);

            Bitmap bitmap = parentForm.GetCurrentBitmap();
            if (bitmap == null) return;
            try
            {

                double ratioHeight = (double)displayPanel.Height / bitmap.Height;
                double masterRatio = (double)displayPanel.Width / bitmap.Width;
                if (ratioHeight < masterRatio) masterRatio = ratioHeight;
                Bitmap displayBitmap = new Bitmap(bitmap, (int)(bitmap.Width * masterRatio), (int)(bitmap.Height * masterRatio));
                //bitmap.Dispose();
                g.Clear(BackgroundColor);
                g.DrawImage(
                    displayBitmap,
                    displayPanel.Width / 2 - displayBitmap.Width / 2,
                    displayPanel.Height / 2 - displayBitmap.Height / 2);
                displayBitmap.Dispose();

                string[] parts = viewModel.CurrentPicture.Name.Split('\\');
                string labelText = "";

                int partCount = 2;
                for (int i = 0; i < partCount; i++)
                {
                    int index = parts.Length - partCount + i;
                    if (index < 0) continue;
                    if (labelText.Length > 0) labelText += "\\";
                    labelText += parts[index];
                }

                if(_paused)
                {
                    g.DrawString(labelText, pictureFont, Brushes.Black, 1, 1);
                    g.DrawString(labelText, pictureFont, Brushes.White, 0, 0);
                }
            }
            catch (System.Exception err)
            {
                Debug.WriteLine("ERROR: "+ err.ToString());
            }
        }

        internal void HandleKeyCommand(Keys key, bool shiftIsPressed, bool ctrlIsPressed, bool altIsPressed)
        {
            int stepSize = 1;
            if (shiftIsPressed) stepSize = 10;
            if (ctrlIsPressed) stepSize = 100000;

            //Debug.WriteLine(shiftIsPressed + "," + ctrlIsPressed + "," + altIsPressed);

            switch (key)
            {
                case Keys.Left:  PanelAction(() => viewModel.GoBack(stepSize), true);  break;
                case Keys.Right: PanelAction(() => viewModel.GoForward(stepSize), true); break;
                case Keys.Up: PanelAction(() => viewModel.GoUp(stepSize), true); break;
                case Keys.Down: PanelAction(() => viewModel.GoDown(stepSize), true); break;
                case Keys.P:
                case Keys.Space:
                    _resumeSeconds = 10;// key == Keys.P ? 7200 : 120; // hard pause is 2 hours, soft pause is 2 minutes
                    if (!_paused) Pause(); 
                    else Resume();
                    break;
                case Keys.C:
                    Clipboard.SetImage(forms[_currentForm].GetCurrentBitmap());
                    break;
                case Keys.Escape:
                    SaveState();
                    Application.Exit();
                    break;
            }
        }

        void PanelAction(Action action, bool pause)
        {
            try
            {
                action();
                forms[_currentForm].SetBitmap(viewModel.CurrentPicture.Name);
            }
            catch(Exception e)
            {
                // Ignore exceptions here
            }
            if (pause)
            {
                Pause();
                _resumeSeconds = 8;
            }
            GoNextPanel();
        }

        bool _paused = false;
        private void Pause()
        {
            _paused = true;
            foreach (var form in forms) form.Pause();
        }

        internal void Resume()
        {
            _paused = false;
            foreach (var form in forms) form.Resume();
        }

        internal void AddForm(ScreenForm screenForm)
        {
            if (pictureFont == null)
            {
                pictureFont = new Font("Arial", 14f);
            }
            this.forms.Add(screenForm);
            formLookup.Add(screenForm.DisplayPanel, screenForm);

        }

        internal void SaveState()
        {
            viewModel.SaveState();
        }

        int initFrames = 0;
        internal void Init()
        {
            initFrames = forms.Count;
            viewModel.GoBack(initFrames);
            
             
            //ThreadPool.QueueUserWorkItem((s) =>
            //    {
            //        for (int i = 0; i < forms.Count; i++)
            //        {
            //            PanelAction(() => viewModel.GoForward(1), false);
            //        }
            //    });

        }
    }


}
