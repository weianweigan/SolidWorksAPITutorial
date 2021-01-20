using SolidWorks.Interop.sldworks;
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
    }
}
