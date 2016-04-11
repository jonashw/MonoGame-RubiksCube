namespace MonoGameRubiksCube
{
    public class Move
    {
        public readonly CubeLayerCriteria Layer;
        public readonly bool RotatePositive;

        public Move(CubeLayerCriteria layer, bool rotatePositive)
        {
            Layer = layer;
            RotatePositive = rotatePositive;
        }
    }
}