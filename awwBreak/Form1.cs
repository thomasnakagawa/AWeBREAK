using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace awwBreak
{
    public partial class Form1 : Form
    {
        //properties
        enum Modes { None, Video, Notification };

        //TODO use a text file instead of hardcode
        String[] artisanURLs = new[]{
		    "https://www.youtube.com/watch?, v=7jWYUtQZhK0","https://www.youtube.com/watch?v=UX0qLJWQcc0", "https://www.youtube.com/watch?v=mXHbj_1A1p4", "https://www.youtube.com/watch?v=dnn-4goA7j8", "https://www.youtube.com/watch?v=yltlJEdSAHw", "https://www.youtube.com/watch?v=L3MtFGWRXAA", "https://www.youtube.com/watch?v=oAtjf6Ijmtw", "https://www.youtube.com/watch?v=wCkerYMffMo", "https://www.youtube.com/watch?v=vqSJLYobwq4", "https://www.youtube.com/watch?v=ZpCl5O6tTv8"
	    };

        int selectedTime = 1;


        //useful objects
        Random rng;
        ContextMenu contextMenu1;

        MenuItem timeRemainingItem;
        MenuItem exitButtonItem;

        //settinfs
        private Modes mode;
        private long secondsLeft;
        private long timerLength;
        private bool counting = false;
        public Form1()
        {
            InitializeComponent();
            this.Icon = awwBreak.Properties.Resources.Icon_Main;
            rng = new Random();
            mode = Modes.Notification;
            updateSelection();

            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }
        public void startWaiting()
        {
            //set the counters and start counting
            timerLength = getTimeFromForm();
            secondsLeft = timerLength;
            counting = true;

            //get the mode
            mode = getModeFromForm();

            //make and start the timer
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timerCallback);
            timer.Start();


            minimizeToTray();

            
        }

        private void minimizeToTray()
        {
            //minimize to system tray
            //TODO make it actually in the system tray
            //TODO fix the multiple icon thing
            //this.WindowState = FormWindowState.Minimized; //http://stackoverflow.com/questions/46918/whats-the-proper-way-to-minimize-to-tray-a-c-sharp-winforms-app
            //https://msdn.microsoft.com/en-us/library/system.windows.forms.notifyicon(v=vs.110).aspx
            //https://msdn.microsoft.com/en-us/library/system.windows.controls.menuitem(v=vs.110).aspx
            this.Visible = false;
            this.ShowInTaskbar = false;

            //time remaining item
            timeRemainingItem = new MenuItem();
            timeRemainingItem.Enabled = false;
            timeRemainingItem.Text = Converter.secondsToMessage(secondsLeft);

            //exit button item
            exitButtonItem = new MenuItem();
            exitButtonItem.Text = "Exit";
            exitButtonItem.Click += new System.EventHandler(this.trayExitButtonClick);


            //context menu. it contains the time and exit items
            contextMenu1 = new ContextMenu();
            contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                timeRemainingItem,
                exitButtonItem
            });

            //the notify icon. It has the context menu
            notifyIcon1 = new NotifyIcon();
            notifyIcon1.ContextMenu = contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "AWe BREAK";
            notifyIcon1.Visible = true;
            notifyIcon1.Click += new System.EventHandler(notifyIcon1_Click);

            notifyIcon1.Icon = awwBreak.Properties.Resources.Icon_Main;
            notifyIcon1.ShowBalloonTip(1, "AWe BREAK", "Your timer has started. AWe BREAK is now accessable from the system tray.", ToolTipIcon.Info);
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            //
            /*this.Visible = true;
            notifyIcon1.ContextMenu.Show(this, notifyIcon1.);
            this.Visible = false;*/
        }

        private void trayExitButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void picBoxEnter(object sender, EventArgs e)
        {
            if (sender.Equals(pictureBoxVideo))
            {
                PictureBox pb = (PictureBox)sender;
                if (mode != Modes.Video)
                    pb.Image = new Bitmap( awwBreak.Properties.Resources.mode_video_hover);
                //setInfoBox();
            }
            else if (sender.Equals(pictureBoxDefault))
            {
                PictureBox pb = (PictureBox)sender;
                if (mode != Modes.Notification)
                    pb.Image = new Bitmap(awwBreak.Properties.Resources.mode_default_hover);
                //setInfoBox();
            }
            else if (sender.Equals(pictureBox20))
            {
                PictureBox pb = (PictureBox)sender;
                if (selectedTime != 1)
                pb.Image = new Bitmap(awwBreak.Properties.Resources.time_20_hover);
                //setInfoBox();
            }
            else if (sender.Equals(pictureBox45))
            {
                PictureBox pb = (PictureBox)sender;
                if (selectedTime != 2)
                pb.Image = new Bitmap(awwBreak.Properties.Resources.time_45_hover);
                //setInfoBox();
            }
            else if (sender.Equals(pictureBoxCustom))
            {
                PictureBox pb = (PictureBox)sender;
                if (selectedTime != 3)
                pb.Image = new Bitmap(awwBreak.Properties.Resources.timer_custom_hover);
                //setInfoBox();
            }
            //Cursor.Current = Cursors.Hand;

        }

        private void picBoxLeave(object sender, EventArgs e)
        {

            if (sender.Equals(pictureBoxVideo))
            {
                PictureBox pb = (PictureBox)sender;
                if(mode != Modes.Video)
                    pb.Image = new Bitmap(awwBreak.Properties.Resources.mode_vide0);
                //setInfoBox();
            }
            else if (sender.Equals(pictureBoxDefault))
            {
                
                PictureBox pb = (PictureBox)sender;
                if (mode != Modes.Notification)
                    pb.Image = new Bitmap(awwBreak.Properties.Resources.mode_default);
                //setInfoBox();
            }
            else if (sender.Equals(pictureBox20))
            {
                PictureBox pb = (PictureBox)sender;
                if(selectedTime != 1)
                    pb.Image = new Bitmap(awwBreak.Properties.Resources.time20);
                //setInfoBox();
            }
            else if (sender.Equals(pictureBox45))
            {
                PictureBox pb = (PictureBox)sender;
                if (selectedTime != 2)
                    pb.Image = new Bitmap(awwBreak.Properties.Resources.time_45);
                //setInfoBox();
            }
            else if (sender.Equals(pictureBoxCustom))
            {
                PictureBox pb = (PictureBox)sender;
                if (selectedTime != 3)
                    pb.Image = new Bitmap(awwBreak.Properties.Resources.time_custom);
                //setInfoBox();
            }
            
        }

        private void picBoxSelect(object sender, EventArgs e)
        {
            
            if (sender.Equals(pictureBoxVideo))
            {
                PictureBox pb = (PictureBox)sender;
                pb.Image = new Bitmap(awwBreak.Properties.Resources.mode_video_selected);
                mode = Modes.Video;
                //setInfoBox();
            }
            else if (sender.Equals(pictureBoxDefault))
            {
                PictureBox pb = (PictureBox)sender;
                pb.Image = new Bitmap(awwBreak.Properties.Resources.mode_default_selected);
                mode = Modes.Notification;
                //setInfoBox();
            }
            else if (sender.Equals(pictureBox20))
            {
                PictureBox pb = (PictureBox)sender;
                pb.Image = new Bitmap(awwBreak.Properties.Resources.time_20_selected);
                selectedTime = 1;
                //setInfoBox();
            }
            else if (sender.Equals(pictureBox45))
            {
                PictureBox pb = (PictureBox)sender;
                pb.Image = new Bitmap(awwBreak.Properties.Resources.time_45_selected);
                selectedTime = 2;
                //setInfoBox();
            }
            else if (sender.Equals(pictureBoxCustom))
            {
                PictureBox pb = (PictureBox)sender;
                pb.Image = new Bitmap(awwBreak.Properties.Resources.time_custom_selected);
                selectedTime = 3;
                //setInfoBox();
            }

            updateSelection();
            
        }


        private void updateSelection()
        {
            switch (selectedTime)
            {
                case 0:
                    break;
                case 1:
                    pictureBox20.Image = new Bitmap(awwBreak.Properties.Resources.time_20_selected);
                    pictureBox45.Image = new Bitmap(awwBreak.Properties.Resources.time_45);
                    pictureBoxCustom.Image = new Bitmap(awwBreak.Properties.Resources.time_custom);
                    break;
                case 2:
                    pictureBox20.Image = new Bitmap(awwBreak.Properties.Resources.time20);
                    pictureBox45.Image = new Bitmap(awwBreak.Properties.Resources.time_45_selected);
                    pictureBoxCustom.Image = new Bitmap(awwBreak.Properties.Resources.time_custom);
                    break;
                case 3:
                    pictureBox20.Image = new Bitmap(awwBreak.Properties.Resources.time20);
                    pictureBox45.Image = new Bitmap(awwBreak.Properties.Resources.time_45);
                    pictureBoxCustom.Image = new Bitmap(awwBreak.Properties.Resources.time_custom_selected);
                    break;
            }

            switch (mode)
            {
                case Modes.None:
                    break;
                case Modes.Notification:
                    //
                    pictureBoxDefault.Image = new Bitmap(awwBreak.Properties.Resources.mode_default_selected);
                    pictureBoxVideo.Image = new Bitmap(awwBreak.Properties.Resources.mode_vide0);
                    break;
                case Modes.Video:
                    //
                    pictureBoxVideo.Image = new Bitmap(awwBreak.Properties.Resources.mode_video_selected);
                    pictureBoxDefault.Image = new Bitmap(awwBreak.Properties.Resources.mode_default);
                    break;
            }
            if (selectedTime == 3)
            {
                textBox1.Visible = true;
            }
            else
            {
                textBox1.Visible = false;
            }
        }



        private void timerCallback(object sender, EventArgs e)
        {

            if (counting)
            {
                secondsLeft = secondsLeft - 1;
            }

            timeRemainingItem.Text = Converter.secondsToMessage(secondsLeft);
            notifyIcon1.Text = "AWe BREAK";

            //timer is done.
            if (secondsLeft < 1)
            {
                counting = false;
                secondsLeft = timerLength;
                notify();
            }

            //not sure why we do this but this guy thinks its important
            //http://stackoverflow.com/questions/186084/how-do-you-add-a-timer-to-a-c-sharp-console-application
            // Force a garbage collection to occur for this demo.
            //GC.Collect();
        }

        private void testCall(object sender, EventArgs e)
        {
            plainNotification("WOrks");
        }

        /////////
        //Notifications
        //////////////////////////////
        public void notify()
        {
            if (this.CanFocus)
            {
                this.Focus();
            }
            if (mode == Modes.Video)//in video mode, it should launch the web broswer to show a random video
            {
                plainNotification("Time for a quick break!");
                this.WindowState = FormWindowState.Minimized;
                launchRandomWebpage();
            }
            else if (mode == Modes.Notification)//in plain notification mode, just notify with a box
            {
                plainNotification("Time for a quick break!");
            }
            else
            {
                plainNotification("Hello world");
            }

            this.Visible = true;
            this.ShowInTaskbar = true;
            //this.WindowState = FormWindowState.Normal;


        }
        public void launchRandomWebpage()
        {
            //Launching a URL as a process launches the default web browser.
            System.Diagnostics.Process.Start(getRandomURL());
        }
        public String getRandomURL()
        {
            int randomNo = rng.Next(0, artisanURLs.Length);
            return artisanURLs[randomNo];
        }

        public void plainNotification(String msg)
        {
            Form2 frm = new Form2();
            //MessageBox.Show(msg);
            //frm.ShowDialog();
            this.BringToFront();
        }

        public void showAbout()
        {
            //TODO about form
        }

        private Modes getModeFromForm()
        {
            return mode;
        }
        private long getTimeFromForm()
        {
            switch (selectedTime)
            {
                case 0:
                    return 0L;
                case 1:
                    //20 mins
                    return 20 * 60;
                case 2:
                    //45 mins
                    return 45 * 60;
                case 3:
                    //10 sec
                    return 10;

            }
            return 12L;
        }


        private void setInfoBox(ref Bitmap bmp)
        {
            //TODO once interface is in
            //set the picturebox to bmp
        }

        public static Point GetMousePosition()
        {
            System.Drawing.Point point = Control.MousePosition;
            return new Point(point.X, point.Y);
        }


        public void launch_button(object sender, EventArgs e)
        {
            startWaiting();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            startWaiting();
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxLaunch_ControlRemoved(object sender, ControlEventArgs e)
        {

        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Image = awwBreak.Properties.Resources.infoBox;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = awwBreak.Properties.Resources.logo_AWeBREAKS;
        }

        private void pictureBoxLaunch_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxLaunch.Image = awwBreak.Properties.Resources.launchButtonSelected;
        }

        private void pictureBoxLaunch_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxLaunch.Image = awwBreak.Properties.Resources.launchButton;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
  
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                    e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("Minutes"))
            {
                textBox1.Clear();
            }
        }
        
    }
}
