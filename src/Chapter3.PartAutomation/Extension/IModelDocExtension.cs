using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;

namespace Chapter3.PartAutomation.Extension
{
    public static class IModelDocExtension
    {
        /// <summary>
        /// 通过顺序获取初始基准面
        /// </summary>
        public static IEnumerable<IFeature> GetRefPlane(this IModelDoc2 doc)
        {
            //创建这个草图，需要找到一个基准面
            var feat = doc.FirstFeature() as IFeature;

            while (feat != null)
            {
                var name = feat.GetTypeName2();
                if (name == "RefPlane")
                {
                    yield return feat;
                }

                feat = feat.GetNextFeature() as IFeature;
            }
        }

        /// <summary>
        /// 在界面不刷新的情况下执行
        /// </summary>
        public static void WithNoRefresh(this IModelDoc2 doc, Action action)
        {
            if (doc is null)
            {
                throw new ArgumentNullException(nameof(doc));
            }
            var activeView = doc.ActiveView as IModelView;
            var featMgr = doc.FeatureManager;
            try
            {
                activeView.EnableGraphicsUpdate = false;

                featMgr.EnableFeatureTree = false;
                featMgr.EnableFeatureTreeWindow = false;

                action?.Invoke();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                activeView.EnableGraphicsUpdate = true;
                featMgr.EnableFeatureTree = true;
                featMgr.EnableFeatureTreeWindow = true;
            }
        }
    }

}
