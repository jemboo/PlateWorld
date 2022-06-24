using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using PlateWorld.ViewModels.PlateParts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlateWorld.ViewModels.DragDrop
{
    public class SamplesDragHandler : IDragSource
    {
        private bool alreadyDropped = false;

        /// <inheritdoc />
        public void StartDrag(IDragInfo dragInfo)
        {
            this.alreadyDropped = false;
            var items = dragInfo.SourceItems.OfType<object>().ToList();
            var wrapper = new SerializableWrapper()
            {
                Items = items,
                DragDropCopyKeyState = DragDropKeyStates.ControlKey //dragInfo.DragDropCopyKeyState
            };
            dragInfo.Data = wrapper;
            dragInfo.DataFormat = DataFormats.GetDataFormat(DataFormats.Serializable);
            dragInfo.Effects = dragInfo.Data != null ? DragDropEffects.Copy | DragDropEffects.Move : DragDropEffects.None;
        }

        /// <inheritdoc />
        public bool CanStartDrag(IDragInfo dragInfo)
        {
            var src = dragInfo.SourceItem as SampleVm;
            if (src == null) return false;
            return string.IsNullOrEmpty(src.PlateName);
        }

        /// <inheritdoc />
        public void Dropped(IDropInfo dropInfo)
        {
            this.alreadyDropped = true;
        }

        /// <inheritdoc />
        public void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
        {
            if (this.alreadyDropped || dragInfo == null)
            {
                return;
            }

            //var sc = dragInfo.SourceCollection as ObservableCollection<SampleVm>;
            //var sourceItems = dragInfo.SourceItems as IEnumerable<SampleVm>;
            //var srcList = sourceItems?.ToList();

            //var sourceList = dragInfo.SourceCollection.TryGetList();
            //var items = dragInfo.SourceItems.OfType<object>().ToList();
            //if (sourceList != null)
            //{
            //    foreach (var o in items)
            //    {
            //        sourceList.Remove(o);
            //    }
            //}

            if (this.alreadyDropped || dragInfo == null)
            {
                return;
            }
            // the drag operation has finished on another app
            if (operationResult != DragDropEffects.None)
            {
                if (operationResult.HasFlag(DragDropEffects.Move))
                {
                    var sourceList = dragInfo.SourceCollection.TryGetList();
                    var items = dragInfo.SourceItems.OfType<object>().ToList();
                    if (sourceList != null)
                    {
                        foreach (var o in items)
                        {
                            //sourceList.Remove(o);
                        }
                    }

                    this.alreadyDropped = true;
                }
            }
        }

        /// <inheritdoc />
        public void DragCancelled()
        {
        }

        /// <inheritdoc />
        public bool TryCatchOccurredException(Exception exception)
        {
            return false;
        }
    }
}
