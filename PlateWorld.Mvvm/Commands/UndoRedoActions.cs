using System;

namespace PlateWorld.Mvvm.Commands
{
    public class UndoRedoActions
    {
        public UndoRedoActions(Action undo, string undoDescr, 
            Action redo, string redoDescr)
        {
            _undoAction = undo;
            _redoAction = redo;
            UndoDescr = undoDescr;
            RedoDescr = redoDescr;
        }

        Action _undoAction;
        public Action UndoAction
        {
            get { return _undoAction; }
        }

        public string UndoDescr { get; }

        Action _redoAction;
        public Action RedoAction
        {
            get { return _redoAction; }
        }
        public string RedoDescr { get; }
    }
}
