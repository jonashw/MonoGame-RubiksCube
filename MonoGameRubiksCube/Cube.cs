using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGameRubiksCube
{
    public class Cube
    {
        private readonly Square[] _squares;
        public CubeLayerCriteria ActiveLayer { get; private set; }
        public CubeState State { get; private set; }
        public float FreeRotateTotalAngle { get; private set; }
        public bool RotatePositive { get; private set; }
        private readonly Tween _autoTween;
        private readonly Tween _snapTween;
        private bool _workingOnNonMove = false;

        public Cube(Square[] squares)
        {
            _squares = squares;
            ActiveLayer = CubeLayerCriteria.Get(Axis.X, AxisLandmark.Max);
            State = CubeState.Ready;
            _autoTween = new Tween(Easing.CubicOut, 0, val => FreeRotateTotalAngle = val, MathHelper.PiOver2, 0.25f);
            _snapTween = new Tween(Easing.CubicOut, 0, val => FreeRotateTotalAngle = val, 0, 0.125f);
        }

        public enum CubeState
        {
            Ready,
            FreeRotating,
            Snapping,
            AutoRotating
        }

        private readonly List<Action<Move>> _observers = new List<Action<Move>>();
        public void OnMove(Action<Move> observer)
        {
            _observers.Add(observer);
        }

        private void notifyMoved()
        {
            if (!_broadcastMoves || !_observers.Any())
            {
                return;
            }
            var move = new Move(ActiveLayer, RotatePositive);
            foreach (var o in _observers)
            {
                o(move);
            }
        }

        public bool TryEnterFreeRotationMode(CubeLayerCriteria layer, bool positive = true)
        {
            if (State != CubeState.Ready && FreeRotateTotalAngle != 0)
            {
                return false;
            }
            RotatePositive = positive;
            _broadcastMoves = true;
            ActiveLayer = layer;
            State = CubeState.FreeRotating;
            return true;
        }

        public void TryStepFreeRotation(float angleInRadians)
        {
            if (State != CubeState.FreeRotating)
            {
                return;
            }
            FreeRotateTotalAngle = MathHelper.Clamp(
                FreeRotateTotalAngle + angleInRadians,
                -MathHelper.PiOver2,
                MathHelper.PiOver2);
            rotateActiveLayer();
        }

        private bool _broadcastMoves = true;
        public void TryStartAutoRotate(CubeLayerCriteria layer, bool positive = true, bool broadcast = true)
        {
            if (State != CubeState.Ready && FreeRotateTotalAngle != 0)
            {
                return;
            }
            _broadcastMoves = broadcast;
            RotatePositive = positive;
            _autoTween.Reset();
            _autoTween.SetTargetValue((positive ? 1 : -1) * MathHelper.PiOver2);
            ActiveLayer = layer;
            State = CubeState.AutoRotating;
        }

        public void Update(GameTime gameTime)
        {
            switch (State)
            {
                default:
                    return;
                case(CubeState.Snapping):
                    _snapTween.Update(gameTime);
                    if (!_snapTween.Finished)
                    {
                        rotateActiveLayer();
                        return;
                    }
                    rotateActiveLayer(true);
                    FreeRotateTotalAngle = 0;
                    State = CubeState.Ready;
                    if (_workingOnNonMove)
                    {
                        _workingOnNonMove = false;
                        break;
                    }
                    notifyMoved();
                    break;
                case(CubeState.AutoRotating):
                    _autoTween.Update(gameTime);
                    if (!_autoTween.Finished)
                    {
                        rotateActiveLayer();
                        return;
                    }
                    rotateActiveLayer(true);
                    FreeRotateTotalAngle = 0;
                    State = CubeState.Ready;
                    if (_workingOnNonMove)
                    {
                        _workingOnNonMove = false;
                        break;
                    }
                    notifyMoved();
                    break;
            }
        }

        public void TryFinishFreeRotate()
        {
            if (State != CubeState.FreeRotating)
            {
                return;
            }
            float targetRotation;
            if (Math.Abs(MathHelper.PiOver2 + FreeRotateTotalAngle) < Math.Abs(FreeRotateTotalAngle))
            {
                RotatePositive = false;
                targetRotation = -MathHelper.PiOver2;
            } else if (Math.Abs(FreeRotateTotalAngle) < Math.Abs(MathHelper.PiOver2 - FreeRotateTotalAngle))
            {
                targetRotation = 0;
                _workingOnNonMove = true;
            }
            else
            {
                RotatePositive = true;
                targetRotation = MathHelper.PiOver2;
            }
            _snapTween.Reset(FreeRotateTotalAngle);
            _snapTween.SetTargetValue(targetRotation);
            State = CubeState.Snapping;
        }

        private void rotateActiveLayer(bool crystallize = false)
        {
            var q = Quaternion.CreateFromAxisAngle(ActiveLayer.AxisVector, FreeRotateTotalAngle);
            foreach (var s in _squares.Where(ActiveLayer.IsSatisfied))
            {
                s.Rotation = q;
                if (crystallize)
                {
                    s.CrystallizePositionAndResetRotation();
                }
            }
        }

        public void Draw(ICamera camera)
        {
            foreach (var s in _squares)
            {
                s.Draw(camera);
            }
        }
    }
}