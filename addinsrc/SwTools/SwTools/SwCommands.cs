using System.ComponentModel;
using SwTools.Properties;
using Xarial.XCad.Base.Attributes;
using Xarial.XCad.UI.Commands.Attributes;
using Xarial.XCad.UI.Commands.Enums;

namespace SwTools
{
    [Title("SwTools")]
    public enum SwCommands
    {
        [Title("First Command")]
        [Description("Hint text for first command")]
        [Icon(typeof(Resource), nameof(Resource.edit))]
        [CommandItemInfo(true, true, WorkspaceTypes_e.AllDocuments, true, RibbonTabTextDisplay_e.TextBelow)]        
        Cmd1,

        [Title(nameof(AddModelViewTab))]
        [Description("Add a modelview tab")]
        [Icon(typeof(Resource), nameof(Resource.edit))]
        [CommandItemInfo(true, true, WorkspaceTypes_e.AllDocuments, true, RibbonTabTextDisplay_e.TextHorizontal)]
        AddModelViewTab,

        [Title(nameof(AddFeatureManagerTab))]
        [Description("Add a feature manager tab")]
        [Icon(typeof(Resource), nameof(Resource.edit))]
        [CommandItemInfo(true,true,WorkspaceTypes_e.AllDocuments,true,RibbonTabTextDisplay_e.TextHorizontal)]
        AddFeatureManagerTab,
    }
}
