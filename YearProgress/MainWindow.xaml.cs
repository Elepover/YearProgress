﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;
using System.Windows.Threading;

namespace YearProgress
{
    /// <summary>
    /// MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Pre-assigned variables.
        bool _expanded = true;
        bool _shown = false;

        private void Wait(double Seconds)
        {
            DispatcherFrame Frame = new DispatcherFrame();
            Thread Thr = new Thread(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(Seconds));
                Frame.Continue = false;
            }

            );
            Thr.Start();
            Dispatcher.PushFrame(Frame);
        }

        // Events are proceed here!

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (_shown)
                return;

            _shown = true;

            // Your code here.

            while (true)
            {
                // Exit.
                if (this.IsVisible == false)
                {
                    Environment.Exit(0);
                }

                // Update date.
                DateTime Now = DateTime.Now;
                TimeSpan SpentTime = new TimeSpan(Now.DayOfYear, Now.Hour, Now.Minute, Now.Second);

                TextBlock_Date.Text = Calculations.OneToTwo(Now.Month) + "/" + Calculations.OneToTwo(Now.Day) + "/" + Now.Year + " " + Calculations.OneToTwo(Now.Hour) + ":" + Calculations.OneToTwo(Now.Minute) + ":" + Calculations.OneToTwo(Now.Second);

                // Update remaining time.
                TextBlock_RemainingDate.Text = Calculations.GetRemainingTime() + " remaining.";

                // Update spent time.
                TextBlock_PastTime.Text = Now.DayOfYear + "d/" + Calculations.OneToTwo(Now.Hour) + ":" + Calculations.OneToTwo(Now.Minute) + ":" + Calculations.OneToTwo(Now.Second);

                // Update total time.
                TextBlock_TotalTime.Text = Calculations.TotalDaysOfYear(Now.Year) + "d/00:00:00";

                // Update progressbar.
                ProgressBar_YearProgress.Maximum = Calculations.TotalDaysOfYear(Now.Year) * 24 * 60 * 60;
                ProgressBar_YearProgress.Value = SpentTime.TotalSeconds;
                ProgressBar_YearProgress.Minimum = 0;

                // Update percentage.
                TextBlock_Percentage.Text = Math.Round((SpentTime.TotalSeconds / (Calculations.TotalDaysOfYear(Now.Year) * 24 * 60 * 60)) * 100, 5) + "%";

                Wait(0.5);
            }
        }

        public void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Rect desktopWorkingArea = SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        private void Button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(DateTime.Now.Year + " has past " + TextBlock_Percentage.Text);
            Button_Copy.Content = "Copied!";
            Wait(1);
            Button_Copy.Content = "Copy";
        }

        private void Button_Expand_Click(object sender, RoutedEventArgs e)
        {
            if (_expanded == false)
            {
                Rect desktopWorkingArea = SystemParameters.WorkArea;
                this.Left = desktopWorkingArea.Right - this.Width;
                this.Top = desktopWorkingArea.Bottom - this.Height;
                _expanded = true;
            }
            else
            {
                this.Left = this.Left + this.Width - 10;
                _expanded = false;
            }
        }
    }
    public class Calculations
    {
        /// <summary>
        /// Get total days of specified year.
        /// </summary>
        /// <param name="Input">Input year.</param>
        /// <returns></returns>
        public static int TotalDaysOfYear(int Year)
        {
            int ReturnValue = 0;
            for (int Month = 1; Month <= 12; Month++){
                ReturnValue += DateTime.DaysInMonth(Year, Month);
            }
            return ReturnValue;
        }
        /// <summary>
        /// Convert 1 to 01
        /// </summary>
        /// <param name="Input">Input time.</param>
        /// <returns></returns>
        public static string OneToTwo(int Input)
        {
            if (Input.ToString().Length == 1)
            {
                return "0" + Input.ToString();
            }
            else
            {
                return Input.ToString();
            }
        }
        /// <summary>
        /// Get remaining time (string) of current year.
        /// </summary>
        /// <returns></returns>
        public static string GetRemainingTime()
        {
            DateTime NextYearFirstSec = new DateTime(DateTime.Now.Year + 1, 1, 1, 0, 0, 0);
            DateTime Now = DateTime.Now;
            TimeSpan Remaining = (NextYearFirstSec - Now);
            return Remaining.Days + "d/" + OneToTwo(Remaining.Hours) + ":" + OneToTwo(Remaining.Minutes) + ":" + OneToTwo(Remaining.Seconds);
        }
    }
}
