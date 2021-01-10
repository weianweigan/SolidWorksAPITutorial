using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xarial.XCad.SolidWorks;

namespace Chapter1.ConnectToSw
{
    class Program
    {
        static void Main(string[] args)
        {
            //获取运行中的SolidWorks
            var swProcess = Process.GetProcessesByName("SLDWORKS");

            //判断SolidWorks是否被打开
            if (!swProcess.Any())
            {
                Console.WriteLine("SolidWorks 没有打开");
                Console.ReadKey();

                //启动18版的SolidWorks
                var swApp = SwApplicationFactory.Create(Xarial.XCad.SolidWorks.Enums.SwVersion_e.Sw2018);
                swApp.ShowMessageBox("Hello New SolidWorks!");
            }
            else
            {
                var swApp = SwApplicationFactory.FromProcess(swProcess.First());
                swApp.ShowMessageBox("Hello SolidWorks!");
            }
 
        }
    }
}
