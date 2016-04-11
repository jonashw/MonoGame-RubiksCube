using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameRubiksCube
{
    public static class CubeFactory
    {
        public static Cube Create(GraphicsDevice graphics)
        {
            var effect = new
            {
                r = colorEffect(graphics, new Color(208, 51, 68)),
                o = colorEffect(graphics, new Color(241, 109, 51)),
                y = colorEffect(graphics, new Color(255, 233, 59)),
                g = colorEffect(graphics, new Color(66, 165, 82)),
                b = colorEffect(graphics, new Color(29, 95, 194)),
                w = colorEffect(graphics, Color.White)
            };

            var rotation = new
            {
                none = Quaternion.CreateFromAxisAngle(Vector3.UnitY, 0f),
                y180 = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.Pi),
                y090 = Quaternion.CreateFromAxisAngle(Vector3.UnitY, MathHelper.PiOver2),
                y270 = Quaternion.CreateFromAxisAngle(Vector3.UnitY, 3 * MathHelper.PiOver2),
                x090 = Quaternion.CreateFromAxisAngle(Vector3.UnitX, MathHelper.PiOver2),
                x270 = Quaternion.CreateFromAxisAngle(Vector3.UnitX, -MathHelper.PiOver2)
            };

            var faceSquarePositions = new List<Vector3>
            {
                new Vector3(-1, -1, 1),
                new Vector3( 0, -1, 1),
                new Vector3( 1, -1, 1),
                new Vector3(-1,  0, 1),
                new Vector3( 0,  0, 1),
                new Vector3( 1,  0, 1),
                new Vector3(-1,  1, 1),
                new Vector3( 0,  1, 1),
                new Vector3( 1,  1, 1)
            }.AsReadOnly();

            var squares = new[]
            {
                new {effect = effect.w, rotation = rotation.none},
                new {effect = effect.b, rotation = rotation.y090},
                new {effect = effect.y, rotation = rotation.y180},
                new {effect = effect.g, rotation = rotation.y270},
                new {effect = effect.o, rotation = rotation.x090},
                new {effect = effect.r, rotation = rotation.x270}
            }.SelectMany(face =>
                faceSquarePositions.Select(position =>
                    new Square(
                        face.effect,
                        face.rotation,
                        position))).ToArray();

            return new Cube(squares);
        }

        private static BasicEffect colorEffect(GraphicsDevice graphics, Color color)
        {
            var v = color.ToVector3();
            return new BasicEffect(graphics)
            {
                LightingEnabled = true,
                AmbientLightColor = v,
                DiffuseColor = v,
                EmissiveColor = v,
                SpecularColor = Vector3.Zero
                //TextureEnabled = true,
                //Texture = Content.Load<Texture2D>("square-star")
            };
        }
    }
}