using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;

namespace Chapter3.PartAutomation.Extension
{
    public static class ISldWorksExtension
    {
        /// <summary>
        /// 在某个设置的状态下执行动作
        /// </summary>
        /// <param name="swUserPreference"><see cref="swUserPreferenceToggle_e"/> 用户设置枚举</param>
        /// <param name="desState">期望状态</param>
        /// <param name="action">执行的动作</param>
        public static void WithToggleState(this ISldWorks sw,swUserPreferenceToggle_e swUserPreference, bool desState, Action action)
        {
            //先获取用户设置的初始状态
            var sourceState = sw.GetUserPreferenceToggle((int)swUserPreference);

            //设置程序需要的状态           
            sw.SetUserPreferenceToggle((int)swUserPreference, desState);

            //执行我们需要的操作
            action?.Invoke();

            //还原用户设置
            sw.SetUserPreferenceToggle((int)swUserPreference, sourceState);

        }
    }
}
