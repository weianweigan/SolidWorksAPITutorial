using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xarial.XCad.SolidWorks;

namespace ChapterOne.ConnectToSW
{
    class Program
    {
        static void Main(string[] args)
        {
            var sldProcess = Process.GetProcessesByName("SLDWORKS");

            if (!sldProcess.Any())
            {
                Console.WriteLine("No SolidWorks opened");
            }
            else
            {
                var app = SwApplicationFactory.FromProcess(sldProcess.First());

                ISldWorks sw = app.Sw;
            }

            Console.ReadLine();
        }

    }
}
