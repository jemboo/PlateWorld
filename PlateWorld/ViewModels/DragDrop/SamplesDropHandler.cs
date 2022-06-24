using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using PlateWorld.ViewModels.PlateParts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlateWorld.ViewModels.DragDrop
{
    public class SamplesDropHandler : IDropTarget
    {
        /// <inheritdoc />
        public void DragOver(IDropInfo dropInfo)
        {
            var wrapper = GetSerializableWrapper(dropInfo);
            if (wrapper != null && dropInfo.TargetCollection != null)
            {
                dropInfo.Effects = ShouldCopyData(dropInfo, wrapper.DragDropCopyKeyState) ? 
                    DragDropEffects.Copy : DragDropEffects.Move;
                dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
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
            var dragItems = wrapper.Items.ToList();
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
                        foreach (var o in dragItems)
                        {
                            var index = sourceList.IndexOf(o);
                            if (index != -1)
                            {
                                var srcIsWellVm = (sourceList[0].GetType() == typeof(WellVm));
                                if (srcIsWellVm)
                                {
                                    var wellVm = sourceList[index] as WellVm;
                                   // wellVm.SampleVm = null;
                                }
                                else
                                {
                                    sourceList.RemoveAt(index);

                                    // so, is the source list the destination list too ?
                                    if (destinationList != null && Equals(sourceList, destinationList) && index < insertIndex)
                                    {
                                        --insertIndex;
                                    }
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
                var cloneData = dropInfo.Effects.HasFlag(DragDropEffects.Copy) || dropInfo.Effects.HasFlag(DragDropEffects.Link);

                foreach (var o in dragItems)
                {
                    var obj2Insert = o;
                    if(o.GetType() == typeof(WellVm))
                    {
                        obj2Insert = ((WellVm)o).SampleVm;
                    }
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
                        if(obj2Insert != null)
                        {
                            destinationList.Insert(insertIndex++, obj2Insert);
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
                var method = list.GetType().GetMethod("Move", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                _ = method?.Invoke(list, new object[] { sourceIndex, destinationIndex });
            }
        }

        private static SerializableWrapper GetSerializableWrapper(IDropInfo dropInfo)
        {
            var data = dropInfo.Data;

            if (data is DataObject dataObject)
            {
                var dataFormat = DataFormats.GetDataFormat(DataFormats.Serializable);
                data = dataObject.GetDataPresent(dataFormat.Name) ? dataObject.GetData(dataFormat.Name) : data;
            }

            var wrapper = data as SerializableWrapper;
            return wrapper;
        }

        private static bool ShouldCopyData(IDropInfo dropInfo, DragDropKeyStates dragDropCopyKeyState)
        {
            // default should always the move action/effect
            if (dropInfo == null)
            {
                return false;
            }

            var copyData = ((dragDropCopyKeyState != default) && dropInfo.KeyStates.HasFlag(dragDropCopyKeyState))
                           || dragDropCopyKeyState.HasFlag(DragDropKeyStates.LeftMouseButton);
            return copyData;
        }
    }



}
