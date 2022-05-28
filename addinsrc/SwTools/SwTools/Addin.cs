using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SwTools.Views;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.Extensions;
using Xarial.XCad.UI.Commands;

namespace SwTools
{
    [ComVisible(true)]
    [Title("SwTools")]
    public class Addin : Xarial.XCad.SolidWorks.SwAddInEx
    {
        public override void OnConnect()
        {
            CommandManager.AddCommandGroup<SwCommands>().CommandClick += Addin_CommandClick; ;
            CreateTaskpane();
        }

        private void Addin_CommandClick(SwCommands spec)
        {
            switch (spec)
            {
                case SwCommands.Cmd1:
                    {
                        var window = new TestWindow(Application.WindowHandle);
                        window.Show();
                    }
                    break;
                case SwCommands.AddFeatureManagerTab:
                    {
                        AddFeatureMgrTab();
                    }
                    break;
                case SwCommands.AddModelViewTab:
                    {
                        AddModelViewTab();
                    }                    
                    break;
            }   
        }

        private void AddModelViewTab()
        {
            var activeDoc = Application.Documents.Active;
            if (activeDoc == null)
            {
                return;
            }

            var featMgrTab = CreateDocumentTab<ModelViewTab>(activeDoc);
            featMgrTab.Control.Sw = Application.Sw;
        }

        private void AddFeatureMgrTab()
        {
            var activeDoc = Application.Documents.Active;
            if (activeDoc == null)
            {
                return;
            }

            var featMgrTab = CreateFeatureManagerTab<FeatMgrTab>(activeDoc);
            featMgrTab.Control.Sw = Application.Sw;
        }

        private void CreateTaskpane()
        {
            var taskpane = this.CreateTaskPane<Taskpane>();
            taskpane.Control.Sw = Application.Sw;
        }

        public override void OnDisconnect()
        {
            base.OnDisconnect();
        }
    }
}
