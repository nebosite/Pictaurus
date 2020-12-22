using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

namespace Pictaurus
{
    /// <summary>
    /// http://www.dreamincode.net/forums/topic/74297-making-a-c%23-screen-saver/
    /// </summary>
    static class Program
    {
        enum Action
        {
            Configure,
            Preview,
            ScreenSaver,
            Debug
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Kick off reading the repository in the background
            PictureRepository rep = PictureRepository.Instance;
           // MessageBox.Show("Attach your debugger now");

            Action action = Action.ScreenSaver;
            if (args.Length > 0)
            {
               string firstArg = args[0].ToLower();
               if(firstArg.StartsWith("/c")) action = Action.Configure;
               if(firstArg.StartsWith("/p")) action = Action.Preview; 
               if(firstArg.StartsWith("/d")) action = Action.Debug; 
            }
            
            //MessageBox.Show("Action was" + action + Environment.NewLine + string.Join(",", args));

            switch (action)
            {
                case Action.Preview:
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        ConsolidatedViewer masterViewer = new ConsolidatedViewer();
                        masterViewer.Init();
                        Application.Run(new ScreenForm(new IntPtr(long.Parse(args[1])), masterViewer));
                    }
                    break;
                case Action.Configure:
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new SettingsForm());
                    break;
                case Action.Debug:
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);

                        Screen otherScreen = null;

                        foreach (Screen screen in Screen.AllScreens)
                        {
                            if (!screen.Primary) otherScreen = screen;
                        }

                        if (otherScreen == null) otherScreen = Screen.AllScreens[0];

                        ConsolidatedViewer masterViewer = new ConsolidatedViewer();
                        Rectangle bounds = new Rectangle(otherScreen.Bounds.X + otherScreen.Bounds.Width / 4,
                            otherScreen.Bounds.Y + otherScreen.Bounds.Height / 8,
                            otherScreen.Bounds.Width / 3,
                            otherScreen.Bounds.Height / 3);
                        ScreenForm screenForm = new ScreenForm(bounds, masterViewer);
                        screenForm.Show();

                        bounds = new Rectangle(otherScreen.Bounds.X + otherScreen.Bounds.Width / 4,
                            otherScreen.Bounds.Y + otherScreen.Bounds.Height / 2,
                            otherScreen.Bounds.Width / 3,
                            otherScreen.Bounds.Height / 3);
                        screenForm = new ScreenForm(bounds, masterViewer);
                        screenForm.Show();

                        masterViewer.Init();
                        Application.Run();
                    }
                    break;
                case Action.ScreenSaver:
                default:
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        ConsolidatedViewer masterViewer = new ConsolidatedViewer();
                        foreach (Screen screen in Screen.AllScreens)
                        {
                            //creates a form just for that screen and passes it the bounds of that screen  
                            ScreenForm screenForm = new ScreenForm(screen.Bounds, masterViewer);
                            screenForm.Show();
                        }
                        masterViewer.Init(); 
                        Application.Run();
                    }
                    break;
            }
        }
    }
}
