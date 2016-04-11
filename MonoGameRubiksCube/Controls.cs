using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Input;

namespace MonoGameRubiksCube
{
    public static class Controls
    {
        public static readonly LayerControlKey X1 = new LayerControlKey(Keys.W, CubeLayerCriteria.Get(Axis.X, AxisLandmark.Max));
        public static readonly LayerControlKey X2 = new LayerControlKey(Keys.E, CubeLayerCriteria.Get(Axis.X, AxisLandmark.Mid));
        public static readonly LayerControlKey X3 = new LayerControlKey(Keys.R, CubeLayerCriteria.Get(Axis.X, AxisLandmark.Min));
        public static readonly LayerControlKey Y1 = new LayerControlKey(Keys.S, CubeLayerCriteria.Get(Axis.Y, AxisLandmark.Min));
        public static readonly LayerControlKey Y2 = new LayerControlKey(Keys.D, CubeLayerCriteria.Get(Axis.Y, AxisLandmark.Mid));
        public static readonly LayerControlKey Y3 = new LayerControlKey(Keys.F, CubeLayerCriteria.Get(Axis.Y, AxisLandmark.Max));
        public static readonly LayerControlKey Z1 = new LayerControlKey(Keys.X, CubeLayerCriteria.Get(Axis.Z, AxisLandmark.Max));
        public static readonly LayerControlKey Z2 = new LayerControlKey(Keys.C, CubeLayerCriteria.Get(Axis.Z, AxisLandmark.Mid));
        public static readonly LayerControlKey Z3 = new LayerControlKey(Keys.V, CubeLayerCriteria.Get(Axis.Z, AxisLandmark.Min));

        public static ReadOnlyCollection<LayerControlKey> All = new ReadOnlyCollection<LayerControlKey>(new[]
        {
            X1, X2, X3,
            Y1, Y2, Y3,
            Z1, Z2, Z3,
        });
    }

    public class LayerControlKey
    {
        public readonly Keys Key;
        public readonly CubeLayerCriteria Layer;

        public LayerControlKey(Keys key, CubeLayerCriteria layer)
        {
            Key = key;
            Layer = layer;
        }
    }
}