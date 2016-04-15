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
            var config = new
            {
                r = new {colorId = SquareColorId.Red,    effect = textureColorEffect(graphics, content, "Square-R",        new Color(184, 0, 0))},
                o = new {colorId = SquareColorId.Orange, effect = textureColorEffect(graphics, content, "Square-O",        new Color(254, 106, 0))},
                y = new {colorId = SquareColorId.Yellow, effect = textureColorEffect(graphics, content, "Square-Y",        new Color(255, 231, 53))},
                g = new {colorId = SquareColorId.Green,  effect = textureColorEffect(graphics, content, "Square-G",        new Color(1, 167, 93))},
                b = new {colorId = SquareColorId.Blue,   effect = textureColorEffect(graphics, content, "Square-B",        new Color(25, 59, 255))},
                w = new {colorId = SquareColorId.White,  effect = textureColorEffect(graphics, content, "Square-W",        Color.White)},
                wc =new {colorId = SquareColorId.White,  effect = textureColorEffect(graphics, content, "Square-W-Center", Color.White)}
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
                new {config = config.g, rotation = rotation.none},
                new {config = config.r, rotation = rotation.y090},
                new {config = config.b, rotation = rotation.y180},
                new {config = config.o, rotation = rotation.y270},
                new {config = config.y, rotation = rotation.x090},
                new {config = config.w, rotation = rotation.x270}
            }.SelectMany(face =>
                faceSquarePositions.Select(position =>
                    face.config == config.w && position == new Vector3(0, 0, 1)
                        ? new Square(
                            config.wc.colorId,
                            config.wc.effect,
                            face.rotation,
                            position)
                        : new Square(
                            face.config.colorId,
                            face.config.effect,
                            face.rotation,
                            position)
                    )).ToArray();

            return new Cube(squares);
        }

        private static BasicEffect textureColorEffect(GraphicsDevice graphics, ContentManager content, string textureName, Color color)
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
                Texture = content.Load<Texture2D>(textureName)
            };
        }
    }
}