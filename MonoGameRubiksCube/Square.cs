using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameRubiksCube
{
    public class Square
    {
        private readonly VertexPositionNormalTexture[] _vertices;
        private readonly short[] _indices;
        public Vector3 Position;
        private readonly BasicEffect _effect;
        public Quaternion Rotation;
        private Matrix _localRotation;

        public Square(BasicEffect effect, Quaternion rotation, Vector3 position)
        {
            _effect = effect;
            _localRotation = Matrix.CreateFromQuaternion(rotation);
            Position = Vector3.Transform(position, rotation);
            Rotation = Quaternion.Identity;
            _vertices = new[]
            {
                new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.UnitZ, new Vector2(0,0)),
                new VertexPositionNormalTexture(new Vector3( 0.5f, -0.5f, 0.5f), Vector3.UnitZ, new Vector2(1,0)),
                new VertexPositionNormalTexture(new Vector3(-0.5f,  0.5f, 0.5f), Vector3.UnitZ, new Vector2(0,1)),
                new VertexPositionNormalTexture(new Vector3( 0.5f,  0.5f, 0.5f), Vector3.UnitZ, new Vector2(1,1))
            };
            _indices = new short[]
            {
                0,1,2,
                1,3,2
            };
        }

        public void CrystallizePositionAndResetRotation()
        {
            _localRotation *= Matrix.CreateFromQuaternion(Rotation);
            Position = Vector3.Transform(Position, Rotation);
            Rotation = Quaternion.Identity;
        }

        public void Draw(ICamera camera)
        {
            _effect.EnableDefaultLighting();

            _effect.View = camera.View;
            _effect.Projection = camera.Projection;
            _effect.World =
                _localRotation
                * Matrix.CreateTranslation(Position)
                * Matrix.CreateFromQuaternion(Rotation);

            foreach (var p in _effect.CurrentTechnique.Passes)
            {
                p.Apply();
                _effect.GraphicsDevice.DrawUserIndexedPrimitives(
                    PrimitiveType.TriangleList,
                    _vertices,
                    0, 4,
                    _indices,
                    0, 2,
                    VertexPositionNormalTexture.VertexDeclaration);
            }
        }
    }
}