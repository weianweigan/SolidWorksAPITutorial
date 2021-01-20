using SolidWorks.Interop.sldworks;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Xarial.XCad.SolidWorks;
using SolidWorks.Interop.swconst;
using System.Collections;
using System.Collections.Generic;
using System;
using Chapter3.PartAutomation.Extension;

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
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var swProcess = Process.GetProcessesByName("SLDWORKS");
            if (!swProcess.Any())
            {
                msg.Text = "没有打开SolidWorks，请打开SolidWorks后再试";
                return;
            }

            _swApp = SwApplicationFactory.FromProcess(swProcess.First());

            msg.Text = _swApp.Version.ToString();
        }

        /// <summary>
        /// 新建一个草图并且绘制一条直线
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //获取当前打开的文档
            var doc = _swApp.Sw.IActiveDoc2;

            //判断一下执行条件
            if (doc == null)
            {
                MessageBox.Show("没有打开的文档");
                return;
            }

            if (doc.GetType() != (int)swDocumentTypes_e.swDocPART)
            {
                MessageBox.Show("当前打开的不是零件");
                return;
            }

            //获取前视基准面
            IFeature refFeat = doc.GetRefPlane().FirstOrDefault();

            //选中这个基准面
            refFeat.Select2(false, 0);

            //在这个基准面上插入一个草图            
            var skeMgr = doc.SketchManager;
            skeMgr.InsertSketch(true);

            //绘制一条直线
            _swApp.Sw.WithToggleState(swUserPreferenceToggle_e.swSketchInference, false, () =>
              {
                  skeMgr.CreateLine(0, 0, 0, double.Parse(_xPostion.Text) / 1000, double.Parse(_yPostion.Text) / 1000, 0);    
              });

            //缩放选择内容到何时大小
            doc.ViewZoomToSelection();

            //退出草图
            skeMgr.InsertSketch(true);
        }

    }
}
