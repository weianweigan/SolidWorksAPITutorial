using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SolidWorks.Interop.swconst;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Xarial.XCad.SolidWorks;

namespace Chapter2.NewDocument
{
    public class MainViewModel : ViewModelBase
    {

        private ISwApplication _swApp;

        private RelayCommand _connectToSldWorksCommand;
        private string _msg;
        private RelayCommand _newPartCommand;
        private RelayCommand _newAssemblyCommand;
        private RelayCommand _newDrawingCommand;

        public string Msg { get => _msg; set => Set(ref _msg, value); }

        /// <summary>
        /// 连接SolidWorks操作
        /// </summary>
        public RelayCommand ConnectToSldWorksCommand { get => _connectToSldWorksCommand ?? (_connectToSldWorksCommand = new RelayCommand(ConectClick)); set => _connectToSldWorksCommand = value; }

        /// <summary>
        /// 新建零件操作
        /// </summary>
        public RelayCommand NewPartCommand { get => _newPartCommand ?? (_newPartCommand = new RelayCommand(NewPartClick)); set => _newPartCommand = value; }

        public RelayCommand NewAssemblyCommand { get => _newAssemblyCommand ?? (_newAssemblyCommand = new RelayCommand(NewAssemblyClick)); set => _newAssemblyCommand = value; }

        public RelayCommand NewDrawingCommand { get => _newDrawingCommand ?? (_newDrawingCommand = new RelayCommand(NewDrawingClick)); set => _newDrawingCommand = value; }

        private void NewDrawingClick()
        {
            NewSolidWorksDoc(swUserPreferenceStringValue_e.swDefaultTemplateDrawing);
        }

        private void NewAssemblyClick()
        {
            NewSolidWorksDoc(swUserPreferenceStringValue_e.swDefaultTemplateAssembly);
        }

        private void NewPartClick()
        {
            NewSolidWorksDoc(swUserPreferenceStringValue_e.swDefaultTemplatePart);
        }

        private void ConectClick()
        {
            var swProcess = Process.GetProcessesByName("SLDWORKS");
            if (!swProcess.Any())
            {
                //没有打开SolidWorksa
                Msg = "没有打开SolidWorks，请打开SolidWorks后再试";
                return;
            }

            _swApp = SwApplicationFactory.FromProcess(swProcess.First());

            Msg = _swApp.Version.ToString();
        }

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
    }
}
