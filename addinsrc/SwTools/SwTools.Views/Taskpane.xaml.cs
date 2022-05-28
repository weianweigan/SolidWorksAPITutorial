using System;
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
using SolidWorks.Interop.sldworks;
using SwTools.Views.Properties;
using Xarial.XCad.Base.Attributes;

namespace SwTools.Views
{
    /// <summary>
    /// Taskpane.xaml 的交互逻辑
    /// </summary>
    [Title("SwToolsLibrary")]
    [Icon(typeof(Resources), nameof(Properties.Resources.library))]
    public partial class Taskpane : UserControl
    {
        public Taskpane()
        {
            InitializeComponent();
        }

        public ISldWorks Sw { get; set; }
    }
}
