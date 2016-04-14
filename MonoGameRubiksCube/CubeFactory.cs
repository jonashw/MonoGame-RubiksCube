using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameRubiksCube
{
    public static class CubeFactory
    {
        public static Cube Create(GraphicsDevice graphics, ContentManager content)
        {
            var effect = new
            {
                r = colorTextureEffect(graphics, content, "R", new Color(184, 0, 0)),
                o = colorTextureEffect(graphics, content, "O", new Color(254, 106, 0)),
                y = colorTextureEffect(graphics, content, "Y", new Color(255, 231, 53)),
                g = colorTextureEffect(graphics, content, "G", new Color(1, 167, 93)),
                b = colorTextureEffect(graphics, content, "B", new Color(25, 59, 255)),
                w = colorTextureEffect(graphics, content, "W", Color.White),
                wc = colorTextureEffect(graphics, content, "W-Center", Color.White),
                k = colorEffect(graphics, new Color(11,11,11))
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
                new {effect = effect.g, rotation = rotation.none},
                new {effect = effect.r, rotation = rotation.y090},
                new {effect = effect.b, rotation = rotation.y180},
                new {effect = effect.o, rotation = rotation.y270},
                new {effect = effect.y, rotation = rotation.x090},
                new {effect = effect.w, rotation = rotation.x270}
            }.SelectMany(face =>
                faceSquarePositions.Select(position =>
                    new Square(
                        face.effect == effect.w && position == new Vector3(0, 0, 1)
                            ? effect.wc
                            : face.effect,
                        face.rotation,
                        position))).ToArray();

            return new Cube(squares);
        }

        private static BasicEffect colorTextureEffect(GraphicsDevice graphics, ContentManager content, string textureNameSuffix, Color color)
        {
            var v = color.ToVector3();
            return new BasicEffect(graphics)
            {
                LightingEnabled = true,
                AmbientLightColor = v,
                DiffuseColor = v,
                EmissiveColor = Color.White.ToVector3(),
                SpecularColor = Vector3.Zero,
                TextureEnabled = true,
                Texture = content.Load<Texture2D>("Square-" + textureNameSuffix)
            };
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
            };
        }
    }
}