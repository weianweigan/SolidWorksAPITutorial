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

        /// <summary>
        /// 连接SolidWorks
        /// </summary>
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
            if (_swApp == null)
            {
                MessageBox.Show("未连接SolidWorks,请先连接SolidWorks");
                return;
            }

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

            var ske = skeMgr.ActiveSketch;

            //绘制一条直线
            _swApp.Sw.WithToggleState(swUserPreferenceToggle_e.swSketchInference, false, () =>
              {
                  skeMgr.CreateLine(0, 0, 0, double.Parse(_xPostion.Text) / 1000, double.Parse(_yPostion.Text) / 1000, 0);    
              });

            //缩放选择内容到何时大小
            doc.ViewZoomToSelection();

            //退出草图
            skeMgr.InsertSketch(true);

            ((IFeature)ske).Name = "草图名称";
        }

        /// <summary>
        /// 执行拉伸凸台
        /// </summary>
        private void CreateFeatureClick(object sender, RoutedEventArgs e)
        {
            if (_swApp ==null)
            {
                MessageBox.Show("未连接SolidWorks");
                return;
            }

            var doc = _swApp.Sw.ActiveDoc as IModelDoc2;

            if (doc == null)
            {
                MessageBox.Show("没有打开的文档");
                return;
            }

            if (doc.GetType() != (int)swDocumentTypes_e.swDocPART)
            {
                MessageBox.Show("当前打开不少零件类型的文档");
                return;
            }

            doc.WithNoRefresh(() =>
            {
                //早绑定到FeatureManager
                var featMgr = doc.FeatureManager;

                //新建拉伸特征
                var frontPlane = doc.GetRefPlane().First();
                frontPlane.Select2(false, 0);

                var skeMgr = doc.SketchManager;
                skeMgr.InsertSketch(true);

                //绘制一个中心矩形
                double width = double.Parse(_rectWidth.Text) / 2;
                _swApp.Sw.WithToggleState(swUserPreferenceToggle_e.swSketchInference, false, () =>
                  {
                      skeMgr.CreateCenterRectangle(0, 0, 0, width, width, 0);
                  });


                double depth = double.Parse(_depth.Text);
                featMgr.FeatureExtrusion3(
                    Sd: true,//单向拉伸
                    Flip: false,
                    Dir: false,
                    T1: (int)swEndConditions_e.swEndCondBlind,
                    T2: (int)swEndConditions_e.swEndCondBlind,
                    D1: depth,
                    D2: 0,
                    //拔模参数
                    Dchk1: false,
                    Dchk2: false,
                    Ddir1: false,
                    Ddir2: false,
                    Dang1: 0,
                    Dang2: 0,
                    // 
                    OffsetReverse1: false,
                    OffsetReverse2: false,
                    TranslateSurface1: false,
                    TranslateSurface2: false,
                    //实体和选择
                    Merge: true,
                    UseFeatScope: true,
                    UseAutoSelect: true,
                    //起始条件
                    T0: (int)swStartConditions_e.swStartSketchPlane,
                    StartOffset: 0,
                    FlipStartOffset: false);

            });

        }

        private void GetSelectionInfo_BtnClick(object sender, RoutedEventArgs e)
        {
            if (_swApp == null)
            {
                MessageBox.Show("当前未连接SolidWorks");
                return;
            }

            var doc = _swApp.Sw.IActiveDoc2;
            if (doc == null)
            {
                _swApp.ShowMessageBox("当前未打开文档", Xarial.XCad.Base.Enums.MessageBoxIcon_e.Warning);
                return;
            }

            //获取选择管理器
            var seleMgr = doc.ISelectionManager;
            //获取选择数量
            var count = seleMgr.GetSelectedObjectCount2(-1);
            if (count < 1)
            {
                MessageBox.Show("当前没有任何选择");
            }

            var data = new List<string>(count);

            for (int i = 1; i < count+1; i++)
            {
                var mark = seleMgr.GetSelectedObjectMark(i);
                var type = seleMgr.GetSelectedObjectType3(i, mark);
                var obj = seleMgr.GetSelectedObject6(i, mark);

                //选择的位置
                var selePostioon = seleMgr.GetSelectionPoint2(i, mark) as double[];
                string info = selePostioon == null ? $"Index:{i},Mark:{mark},Type:{(swSelectType_e)type}" :
                    $"Index:{i},Mark:{mark},Type:{(swSelectType_e)type},Postion:{selePostioon[0]},{selePostioon[1]},{selePostioon[2]}";

                if (obj is IFeature feat)
                {
                    var featInfo = $"特征名称{feat.Name},特征类型：{feat.GetTypeName2()}";

                    info += featInfo;
                }

                data.Add(info);
            }

            _selectionList.ItemsSource = data;
        }
    }
}
