﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Windows.Automation;
using System.Runtime.InteropServices;
using System.Windows;
using System.IO;

namespace GUIWalker
{
    class Walker
    {
        private Process pApp = null;
        private AutomationElement ModelScreen = null;
        private AutomationElement ParentWindow = null;
        private PatternManager patternManager = new PatternManager();
        private ElementPicker elemPicker = new ElementPicker();

        //stream stuff 
        static private string DesktopLoc = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\HackLog\\WalkerLog.txt";

        public void DFSwalk()
        {

            pApp = Process.Start(@"D:\Users\Dennis\Desktop\Hackathon\guitesting\WpfApplication1\WpfApplication1\bin\Debug\WpfApplication1.exe");
            while (pApp.MainWindowHandle == IntPtr.Zero)
                Thread.Sleep(1000);

            AutomationElement ParentWindow = AutomationElement.FromHandle(pApp.MainWindowHandle);
            //AutomationElementCollection DesktopChildren = AutomationElement.RootElement.FindAll(TreeScope.Children, new PropertyCondition(AutomationElement.IsEnabledProperty, true));

            Random rand = new Random();
            while (true)
            {
                AutomationElementCollection ElementCollection = ParentWindow.FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.IsEnabledProperty, true));

                AutomationElement iter = elemPicker.GetElementRandom(ElementCollection);
                if (iter == null)
                    continue;

                AutomationPattern[] validPatterns = iter.GetSupportedPatterns();
                if (validPatterns.Count() > 0)
                {
                    int patternIndex = rand.Next(0, validPatterns.Count() - 1);
                    patternManager.executePattern(iter, validPatterns[patternIndex]);



                    //Logging
                    Console.WriteLine(validPatterns[patternIndex].ProgrammaticName + " " + iter.Current.Name.ToString());
                    StreamWriter mLog = new StreamWriter(DesktopLoc, true);
                    mLog.WriteLine(iter.Current.Name + " " + iter.Current.AutomationId + " " + validPatterns[patternIndex].ProgrammaticName);
                    mLog.Close();
                }
            }

        }

    }
}
