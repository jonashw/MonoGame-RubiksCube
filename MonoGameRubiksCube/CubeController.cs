using System;

namespace MonoGameRubiksCube
{
    public class CubeController
    {
        private readonly Cube _cube;
        private readonly CubeUndoStack _undoStack;
        private int _shuffleMovesLeft = 0;
        public CubeController(Cube cube, CubeUndoStack undoStack)
        {
            _cube = cube;
            _undoStack = undoStack;
            cube.OnMove(cubeMoved);
            State = ControllerState.Ready;
        }

        public enum ControllerState
        {
            Shuffling, Ready
        }

        private void cubeMoved(Move move)
        {
            if (State == ControllerState.Shuffling)
            {
                tryNextShuffleMove();
            }
        }

        private readonly Random _random = new Random();
        private void tryNextShuffleMove()
        {
            if (_shuffleMovesLeft <= 0)
            {
                State = ControllerState.Ready;
                return;
            }
            _shuffleMovesLeft--;
            _cube.TryStartAutoRotate(CubeLayerCriteria.GetRandom(), _random.Next(0, 100) > 50);
        }

        public ControllerState State { get; private set; }

        public void TryStartShuffle(int moveCount)
        {
            if (State == ControllerState.Shuffling)
            {
                return;
            }
            _shuffleMovesLeft = moveCount;
            State = ControllerState.Shuffling;
            tryNextShuffleMove();
        }

        public void TryStepFreeRotation(float angle)
        {
            if (State == ControllerState.Shuffling)
            {
                return;
            }
            _cube.TryStepFreeRotation(angle);
        }

        public void TryFinishFreeRotate()
        {
            if (State == ControllerState.Shuffling)
            {
                return;
            }
            _cube.TryFinishFreeRotate();
        }

        public void TryStartRotate(CubeLayerCriteria layer, bool positive)
        {
            if (State == ControllerState.Shuffling)
            {
                return;
            }
            _cube.TryStartAutoRotate(layer, positive);
        }

        public void TryUndo()
        {
            if (State == ControllerState.Shuffling)
            {
                return;
            }
            _undoStack.TryUndo();
        }
    }
}