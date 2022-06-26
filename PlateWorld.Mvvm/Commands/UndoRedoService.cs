using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace PlateWorld.Mvvm.Commands
{
    public class UndoRedoService
    {
        public UndoRedoService()
        {
            _undoCommand = new RelayCommand(DoTheUndo, CanDoTheUndo);
            _redoCommand = new RelayCommand(DoTheRedo, CanDoTheRedo);
        }

        Stack<UndoRedoActions> _undoStack = new Stack<UndoRedoActions>();

        Stack<UndoRedoActions> _redoStack = new Stack<UndoRedoActions>();


        public void Push(Action undo, Action redo)
        {
            var urA = new UndoRedoActions(undo, redo);
            _undoStack.Push(urA);
            redo(); 
            _undoCommand.NotifyCanExecuteChanged();
            _redoCommand.NotifyCanExecuteChanged();
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
            _undoCommand.NotifyCanExecuteChanged();
            _redoCommand.NotifyCanExecuteChanged();
        }

        bool CanDoTheUndo()
        {
            return (_undoStack.Count > 0);
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
            _undoCommand.NotifyCanExecuteChanged();
            _redoCommand.NotifyCanExecuteChanged();
        }

        bool CanDoTheRedo()
        {
            return (_redoStack.Count > 0);
        }

    }
}
