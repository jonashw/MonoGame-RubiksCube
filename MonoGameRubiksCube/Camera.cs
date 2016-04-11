using Microsoft.Xna.Framework;

namespace MonoGameRubiksCube
{
    public class Camera : ICamera
    {
        private readonly float _aspectRatio;
        private readonly float _viewportWidth;
        private readonly float _viewportHeight;
        public Vector3 Position;
        public Quaternion Rotation;

        public bool Orthographic = true;

        public Camera(float viewportWidth, float viewportHeight)
        {
            _viewportWidth = viewportWidth;
            _viewportHeight = viewportHeight;
            _aspectRatio = viewportWidth/_viewportHeight;
        }

        public Matrix View
        {
            get
            {
                return
                    Matrix.CreateRotationY(MathHelper.PiOver4)
                    *Matrix.CreateRotationX(-MathHelper.Pi/6f)
                    *Matrix.CreateLookAt(Position, new Vector3(0, 0, 0), Vector3.Up);
            }
        }

        private float _rotation = 0;
        public void Update(GameTime gameTime)
        {
            Rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, _rotation+=0.01f);
        }

        public Matrix Projection
        {
            get
            {
                return Orthographic
                    ? Matrix.CreateOrthographic(
                        _viewportWidth/85f,
                        _viewportHeight/85f,
                        1f,
                        100f)
                    : Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.PiOver4,
                        _aspectRatio,
                        1f,
                        100f);
            }
        }
    }
}