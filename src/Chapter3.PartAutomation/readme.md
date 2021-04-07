```csharp
        /// <summary>
        /// 创建一个拉伸凸台特征
        /// </summary>
        private void CreateFeatureClick(object sender, RoutedEventArgs e)
        {
            if (_swApp ==null)
            {
                MessageBox.Show("当前未连接到SolidWorks");
                return;
            }

            var doc = _swApp.Sw.ActiveDoc as IModelDoc2;
            
            //判断有没有打开文档
            if (doc == null)
            {
                MessageBox.Show("当前无活动文档");
                return;
            }

            if (doc.GetType() != (int)swDocumentTypes_e.swDocPART)
            {
                MessageBox.Show($"当前打开的不是零件，而是{(swDocumentTypes_e)doc.GetType()}");
                return;
            }

            //获取前视基准面特征
            var frontPlane = doc.GetRefPlane().First();

            frontPlane.Select2(false, 0);

            //在基准面上创建草图
            var skeMgr = doc.SketchManager;
            skeMgr.InsertSketch(true);

            var ske = skeMgr.ActiveSketch;
            //绘制矩形
            _swApp.Sw.WithToggleState(swUserPreferenceToggle_e.swSketchInference, false, () =>
              {
                  var segs = (skeMgr.CreateCenterRectangle(0, 0, 0, 0.01, 0.01, 0) as object[]).Cast<ISketchSegment>();
              });

            //doc.EditRebuild3();
            //((IFeature)ske).Select2(false, 0);

            //直接新建拉伸特征
            var featMgr = doc.FeatureManager;
            featMgr.FeatureExtrusion3(
                Sd: true,
                Flip: false,
                Dir: false,
                T1: (int)swEndConditions_e.swEndCondBlind,
                T2: (int)swEndConditions_e.swEndCondBlind,
                D1: 0.01,
                D2: 0,
                Dchk1: false,
                Dchk2: false,
                Ddir1: false,
                Ddir2: false,
                Dang1: 0,
                Dang2: 0,
                OffsetReverse1: false,
                OffsetReverse2: false,
                TranslateSurface1: false,
                TranslateSurface2: false,
                Merge: true,
                UseFeatScope: true,
                UseAutoSelect: true,
                T0: (int)swStartConditions_e.swStartSketchPlane,
                StartOffset: 0,
                FlipStartOffset: false);
        }
```

```csharp
        /// <summary>执行代码时不刷新图像区域 和 特征树</summary>
        public static void WithNoRefresh(this IModelDoc2 doc, Action action)
        {
            if (doc is null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            IModelView swView = null;
            try
            {
                swView = doc.ActiveView as IModelView;
                swView.SuppressWaitCursorDuringRedraw = true;
                swView.EnableGraphicsUpdate = false;
                doc.FeatureManager.EnableFeatureTreeWindow = false;
                doc.FeatureManager.EnableFeatureTree = false;

                doc.Lock();

                action?.Invoke();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                doc.UnLock();

                doc.FeatureManager.EnableFeatureTreeWindow = true;
                doc.FeatureManager.EnableFeatureTree = true;
                
                swView.EnableGraphicsUpdate = true;
                swView.SuppressWaitCursorDuringRedraw = false;
            }
        }
```