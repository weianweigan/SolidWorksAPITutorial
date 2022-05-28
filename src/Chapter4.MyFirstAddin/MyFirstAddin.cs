using Chapter4.MyFirstAddin.Properties;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.SolidWorks;
using Xarial.XCad.UI.Commands;

namespace Chapter4.MyFirstAddin
{
    /*
     * 1.注册插件
     *      为了让SolidWorks知道我们插件的路径
     *      需要用到 RegAsm.exe 注册插件
     *      如何找到RegAsm.exe，C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe
     *      
     *      RegAsm.exe [程序集名称].dll /codebase
     * 
     * 2.卸载插件
     *      RegAsm.exe [程序集名称].dll /u
     *          
     * 3.xCAD 已经为我们封装了上面的注册和卸载过程，我们不需要去手动去注册和卸载；
     */

    [ComVisible(true)]
    [Title("第一个插件")]
    public class MyFirstAddin:SwAddInEx
    {
        public override void OnConnect()
        {
            var cmdGroup = CommandManager.AddCommandGroup<Commands>();
        }
    }

    public enum Commands
    {
        [Title("第一个按钮")]
        [Description("我的第一个按钮")]
        [Icon(typeof(Resource),nameof(Resource.MyFirstCommand))]
        FirstCommand
    }
}
