using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace MonoGameRubiksCube
{
    public class CubeLayerCriteria
    {
        public readonly Func<Vector3,float> GetVectorComponent;
        public readonly float PositionComponent;
        public readonly Vector3 AxisVector;
        public readonly AxisLandmark Landmark;
        public readonly Axis Axis;

        private CubeLayerCriteria(Func<Vector3, float> getVectorComponent, Vector3 axisVector, float positionComponent, AxisLandmark landmark, Axis axis)
        {
            GetVectorComponent = getVectorComponent;
            AxisVector = axisVector;
            PositionComponent = positionComponent;
            Landmark = landmark;
            Axis = axis;
        }

        public bool IsSatisfied(Square square)
        {
            return Math.Abs(GetVectorComponent(square.Position) - PositionComponent) < 0.01f;
        }

        public static CubeLayerCriteria Get(Axis axis, AxisLandmark position)
        {
            return new CubeLayerCriteria(
                axisToVectorComponentFunc(axis),
                axisToVector(axis),
                axisLandmarkToFloat(position),
                position, axis);
        }

        public static ReadOnlyCollection<CubeLayerCriteria> AllInstances = new ReadOnlyCollection<CubeLayerCriteria>(new []
        {
            Get(Axis.X, AxisLandmark.Min),
            Get(Axis.X, AxisLandmark.Mid),
            Get(Axis.X, AxisLandmark.Max),
            Get(Axis.Y, AxisLandmark.Min),
            Get(Axis.Y, AxisLandmark.Mid),
            Get(Axis.Y, AxisLandmark.Max),
            Get(Axis.Z, AxisLandmark.Min),
            Get(Axis.Z, AxisLandmark.Mid),
            Get(Axis.Z, AxisLandmark.Max)
        });


        private static Vector3 axisToVector(Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return Vector3.UnitX;
                case Axis.Y:
                    return Vector3.UnitY;
                case Axis.Z:
                    return Vector3.UnitZ;
                default:
                    throw new ArgumentException("Unexpected axis: " + axis);
            }
        }

        private static float axisLandmarkToFloat(AxisLandmark landmark)
        {
            switch (landmark)
            {
                case AxisLandmark.Min:
                    return -1;
                case AxisLandmark.Mid:
                    return 0;
                case AxisLandmark.Max:
                    return 1;
                default:
                    throw new ArgumentException("Unexpected axis landmark: " + landmark);
            }
        }

        private static Func<Vector3,float> axisToVectorComponentFunc(Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return v => v.X;
                case Axis.Y:
                    return v => v.Y;
                case Axis.Z:
                    return v => v.Z;
                default:
                    throw new ArgumentException("Unexpected axis: " + axis);
            }
        }

        private static readonly Random _r = new Random();
        public static CubeLayerCriteria GetRandom()
        {
            var index = _r.Next(0, AllInstances.Count - 1);
            return AllInstances[index];
        }
    }
}