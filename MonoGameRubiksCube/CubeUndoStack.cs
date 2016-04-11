using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameRubiksCube
{
    public class CubeUndoStack
    {
        private readonly Cube _cube;
        private readonly Stack<Action> _stack = new Stack<Action>();

        public CubeUndoStack(Cube cube)
        {
            _cube = cube;
        }

        public int Count
        {
            get { return _stack.Count; }
        }

        public void RegisterMove(Move move)
        {
            _stack.Push(() => _cube.TryStartAutoRotate(move.Layer, !move.RotatePositive, false));
        }

        public void TryUndo()
        {
            if (_cube.State != Cube.CubeState.Ready || !_stack.Any())
            {
                return;
            }
            _stack.Pop()();
        }
    }
}