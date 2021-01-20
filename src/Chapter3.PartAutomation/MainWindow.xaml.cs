using SolidWorks.Interop.sldworks;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Xarial.XCad.SolidWorks;

namespace Chapter3.PartAutomation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ISwApplication _swApp;

        public MainWindow()
        {
            InitializeComponent();

            var swProcess = Process.GetProcessesByName("SLDWORKS");
            if (!swProcess.Any())
            {
                return;
            }

            _swApp = SwApplicationFactory.FromProcess(swProcess.First());

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double x = double.Parse(_xPostion.Text);
            double y = double.Parse(_yPostion.Text);

            var doc = _swApp.Sw.IActiveDoc2;
            var featMgr = doc.FeatureManager;
            var skeMgr = doc.SketchManager;

            var feat = (featMgr.GetFeatures(true) as object[])
                .Cast<IFeature>()
                .Where(p => p.Name == "上视基准面")
                .FirstOrDefault();

            if (feat != null)
            {
                feat.Select2(false, 0);
                skeMgr.InsertSketch(true);
                var ske = skeMgr.ActiveSketch;
                ske.
            }
        }
    }
}
