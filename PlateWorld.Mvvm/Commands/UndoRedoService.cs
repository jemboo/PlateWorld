using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace PlateWorld.Mvvm.Commands
{
    public class UndoRedoService : ObservableObject
    {
        public UndoRedoService()
        {
            _undoCommand = new RelayCommand(DoTheUndo, CanDoTheUndo);
            _redoCommand = new RelayCommand(DoTheRedo, CanDoTheRedo);
        }

        Stack<UndoRedoActions> _undoStack = new Stack<UndoRedoActions>();

        Stack<UndoRedoActions> _redoStack = new Stack<UndoRedoActions>();


        public void Push(Action undo, string undoDescr,
            Action redo, string redoDescr)
        {
            var urA = new UndoRedoActions(undo, undoDescr, redo, redoDescr);
            _undoStack.Push(urA);
            _redoStack.Clear();
            redo();
            Notify();
        }

        void Notify()
        {
            _undoCommand.NotifyCanExecuteChanged();
            _redoCommand.NotifyCanExecuteChanged();

            this.OnPropertyChanged("UndoDescr");
            this.OnPropertyChanged("RedoDescr");
        }

        public void PopUndo()
        {
            if (_undoStack.Count > 0)
            {
                var undo = _undoStack.Pop();
                Notify();
            }
        }

        #region UndoCommand

        public string UndoDescr
        {
            get 
            {
                if (_undoStack.Count > 0)
                {
                    var undo = _undoStack.Peek();
                    return undo.UndoDescr;
                }
                else
                {
                    return String.Empty;
                }
            }
        }

        RelayCommand _undoCommand;
        public ICommand UndoCommand
        {
            get 
            {
                return _undoCommand;
            }
        }

        void DoTheUndo()
        {
            if (_undoStack.Count > 0)
            {
                var undo = _undoStack.Pop();
                undo.UndoAction();
                _redoStack.Push(undo);
            }

            Notify();
        }

        bool CanDoTheUndo()
        {
            return (_undoStack.Count > 0);
        }

        #endregion


        #region RedoCommand


        public string RedoDescr
        {
            get
            {
                if (_redoStack.Count > 0)
                {
                    var redo = _redoStack.Peek();
                    return redo.RedoDescr;
                }
                else
                {
                    return String.Empty;
                }
            }
        }
        RelayCommand _redoCommand;
        public ICommand RedoCommand
        {
            get
            {
                return _redoCommand;
            }
        }

        void DoTheRedo()
        {
            if (_redoStack.Count > 0)
            {
                var redo = _redoStack.Pop();
                redo.RedoAction();
                _undoStack.Push(redo);
            }

            Notify();
        }

        bool CanDoTheRedo()
        {
            return (_redoStack.Count > 0);
        }

        #endregion
    }
}
