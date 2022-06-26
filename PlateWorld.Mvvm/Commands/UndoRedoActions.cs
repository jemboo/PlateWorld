using System;

namespace PlateWorld.Mvvm.Commands
{
    public class UndoRedoActions
    {
        public UndoRedoActions(Action undo, Action redo)
        {
            _undoAction = undo;
            _redoAction = redo; 
        }

        Action _undoAction;
        public Action UndoAction
        {
            get { return _undoAction; }
        }

        Action _redoAction;
        public Action RedoAction
        {
            get { return _redoAction; }
        }
    }
}
