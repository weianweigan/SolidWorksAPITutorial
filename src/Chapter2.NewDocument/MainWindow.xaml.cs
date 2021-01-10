using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using SolidWorks.Interop.swconst;
using Xarial.XCad.SolidWorks;

namespace Chapter2.NewDocument
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
            DataContext = new MainViewModel();
        }

        /// <summary>
        /// 连接打开的SolidWorks<see cref="MainViewModel.ConectClick"/>
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        #region 新建SolidWorks文档

        /// <summary>
        /// 新建零件
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NewSolidWorksDoc(swUserPreferenceStringValue_e.swDefaultTemplatePart);
        }

        /// <summary>
        /// 新建装配体
        /// </summary>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NewSolidWorksDoc(swUserPreferenceStringValue_e.swDefaultTemplateAssembly);
        }

        /// <summary>
        /// 新建工程图
        /// </summary>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NewSolidWorksDoc(swUserPreferenceStringValue_e.swDefaultTemplateDrawing);
        }

        /// <summary>
        /// 根据SolidWorks默认配置的模板路径，新建SolidWorks文档
        /// </summary>
        /// <param name="docTemplateType">默认模板类型</param>
        private void NewSolidWorksDoc(swUserPreferenceStringValue_e docTemplateType)
        {
            if (_swApp == null)
            {
                MessageBox.Show("未连接SolidWorks");
                return;
            }

            var templatePart = _swApp.Sw.GetUserPreferenceStringValue((int)docTemplateType);

            if (!File.Exists(templatePart))
            {
                MessageBox.Show("未支配默认模板，无法新建零件");
                return;
            }

            var doc = _swApp.Sw.INewDocument2(templatePart, 0, 300d, 300d);
        }

        #endregion

        #region 打开文档

        /// <summary>
        /// 打开当前存在的零件
        /// </summary>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            OpenLocalDoc("零件1.sldprt", swDocumentTypes_e.swDocPART);
        }

        /// <summary>
        /// 打开本地文档
        /// </summary>
        private void OpenLocalDoc(string docName, swDocumentTypes_e docType)
        {
            string partDocPath = Path.Combine(
                Path.GetDirectoryName(typeof(MainWindow).Assembly.Location),
                "Model",
                docName);

            if (_swApp == null)
            {
                MessageBox.Show("未连接SolidWorks");
                return;
            }

            if (!File.Exists(partDocPath))
            {
                _swApp.ShowMessageBox($"{partDocPath} 此文件不存在");
                return;
            }

            int errors = 0;
            int warings = 0;

            var partDoc = _swApp.Sw.OpenDoc6(partDocPath, (int)docType, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref errors, ref warings);

            if (partDoc == null)
            {
                _swApp.ShowMessageBox($"{partDocPath} 打开失败，错误代码：{errors}");
            }
        }

        /// <summary>
        /// 打开装配体文档
        /// </summary>
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            OpenLocalDoc("装配体2.sldasm", swDocumentTypes_e.swDocASSEMBLY);
        }

        /// <summary>
        /// 打开工程图文档
        /// </summary>
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            OpenLocalDoc("工程图1.slddrw", swDocumentTypes_e.swDocDRAWING);
        }

        #endregion
    }
}
