using Microsoft.Xna.Framework;

namespace MonoGameRubiksCube
{
    public interface ICamera
    {
        Matrix View { get; }
        Matrix Projection { get; }
    }
}