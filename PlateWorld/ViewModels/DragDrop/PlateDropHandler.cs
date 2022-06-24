using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using PlateWorld.ViewModels.PlateParts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace PlateWorld.ViewModels.DragDrop
{
    public class PlateDropHandler : IDropTarget
    {
        /// <inheritdoc />
        public void DragOver(IDropInfo dropInfo)
        {
            var wrapper = GetSerializableWrapper(dropInfo);
            if (wrapper != null && dropInfo.TargetCollection != null)
            {
                if(ShouldCopyData(dropInfo, wrapper.DragDropCopyKeyState))
                {
                    dropInfo.Effects = DragDropEffects.Copy;
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                }
                else
                {
                    dropInfo.Effects = DragDropEffects.None;
                   // dropInfo.DropTargetAdorner = DropTargetAdorners.;
                }
            }
        }

        /// <inheritdoc />
        public void Drop(IDropInfo dropInfo)
        {
            var wrapper = GetSerializableWrapper(dropInfo);
            if (wrapper == null || dropInfo.TargetCollection == null)
            {
                return;
            }

            // at this point the drag info can be null, cause the other app doesn't know it
            var insertIndex = dropInfo.UnfilteredInsertIndex;
            var destinationList = dropInfo.TargetCollection.TryGetList();
            var data = wrapper.Items.ToList();
            bool isSameCollection = false;

            var copyData = ShouldCopyData(dropInfo, wrapper.DragDropCopyKeyState);
            if (!copyData)
            {
                var sourceList = dropInfo.DragInfo?.SourceCollection?.TryGetList();
                if (sourceList != null)
                {
                    isSameCollection = sourceList.IsSameObservableCollection(destinationList);
                    if (!isSameCollection)
                    {
                        foreach (var o in data)
                        {
                            var index = sourceList.IndexOf(o);
                            if (index != -1)
                            {
                               // sourceList.RemoveAt(index);

                                // so, is the source list the destination list too ?
                                if (destinationList != null && 
                                    Equals(sourceList, destinationList) && 
                                    index < insertIndex)
                                {
                                    --insertIndex;
                                }
                            }
                        }
                    }
                }
            }

            if (destinationList != null)
            {
                var objects2Insert = new List<object>();

                // check for cloning
                var cloneData = dropInfo.Effects.HasFlag(DragDropEffects.Copy) || 
                    dropInfo.Effects.HasFlag(DragDropEffects.Link);

                foreach (var o in data)
                {
                    var obj2Insert = o;
                    if (cloneData)
                    {
                        if (o is ICloneable cloneable)
                        {
                            obj2Insert = cloneable.Clone();
                        }
                    }

                    objects2Insert.Add(obj2Insert);

                    if (!cloneData && isSameCollection)
                    {
                        var index = destinationList.IndexOf(o);
                        if (index != -1)
                        {
                            if (insertIndex > index)
                            {
                                insertIndex--;
                            }

                            Move(destinationList, index, insertIndex++);
                        }
                    }
                    else
                    {
                        var destWellVm = destinationList[insertIndex] as WellVm;
                        var srcSampleVm = obj2Insert as SampleVm;
                        if((srcSampleVm != null) && (destWellVm != null))
                        {
                            srcSampleVm.WellCoords = destWellVm.Well.WellCoords;
                            srcSampleVm.PlateName = destWellVm.PlateName;
                            destWellVm.SampleVm = srcSampleVm;
                            destWellVm.IsSelected = true;
                            srcSampleVm.Update();
                            destWellVm.Update(destWellVm);
                            return;
                        }
                        var srcWellVm = obj2Insert as WellVm;
                        if ((srcWellVm != null) && (destWellVm != null))
                        {
                            if (srcWellVm.SampleVm == null) return;
                            srcWellVm.SampleVm.WellCoords = destWellVm.Well.WellCoords;
                            srcWellVm.SampleVm.PlateName = destWellVm.PlateName;
                            destWellVm.SampleVm = srcWellVm.SampleVm;
                            srcWellVm.SampleVm = null;
                            destWellVm.IsSelected = true;
                            srcWellVm.Update(srcWellVm);
                            destWellVm.Update(destWellVm);
                            return;
                        }


                    }
                }

                DefaultDropHandler.SelectDroppedItems(dropInfo, objects2Insert);
            }
        }

        private static void Move(IList list, int sourceIndex, int destinationIndex)
        {
            if (!list.IsObservableCollection())
            {
                throw new ArgumentException("ObservableCollection<T> was expected", nameof(list));
            }

            if (sourceIndex != destinationIndex)
            {
                var method = list.GetType().GetMethod(
                    "Move", 
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                _ = method?.Invoke(list, new object[] { sourceIndex, destinationIndex });
            }
        }

        private static SerializableWrapper GetSerializableWrapper(IDropInfo dropInfo)
        {
            var data = dropInfo.Data;

            if (data is DataObject dataObject)
            {
                var dataFormat = DataFormats.GetDataFormat(DataFormats.Serializable);
                data = dataObject.GetDataPresent(dataFormat.Name) ? 
                    dataObject.GetData(dataFormat.Name) : data;
            }

            var wrapper = data as SerializableWrapper;
            return wrapper;
        }

        private static bool ShouldCopyData(IDropInfo dropInfo, DragDropKeyStates dragDropCopyKeyState)
        {
            var wellVms = new List<WellVm>();
            foreach (var oobj in dropInfo.TargetCollection)
            {
                var wvm = oobj as WellVm;
                wellVms.Add(wvm);
                if (wvm != null)
                {
                    wvm.IsSelected = false;
                }
            }
            var selWellVm = wellVms[dropInfo.InsertIndex];
            if (selWellVm != null)
            {
                if (selWellVm.SampleVm != null)
                {
                    return false;
                }
            }
            // default should always the move action/effect
            if (dropInfo == null)
            {
                return false;
            }
            selWellVm.IsSelected = true;
            return true;
        }
    }



}
