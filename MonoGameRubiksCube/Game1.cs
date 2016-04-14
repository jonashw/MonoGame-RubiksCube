using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGameRubiksCube
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Camera _camera;
        private Cube _cube;
        private CubeUndoStack _undoStack;
        private readonly KeyboardEvents _keyboard = new KeyboardEvents();
        private SpriteFont _font;
        private bool _autoRotateMode = true;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("Consolas");

            _camera = new Camera(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height)
            {
                Position = new Vector3(0, 0.5f, -7f)
            };

            _cube = CubeFactory.Create(GraphicsDevice, Content);
            _undoStack = new CubeUndoStack(_cube);
            _cube.OnMove(_undoStack.RegisterMove);
        }

        protected override void Initialize()
        {
            _keyboard.OnPress(Keys.Enter, () => _autoRotateMode = ! _autoRotateMode);
            _keyboard.OnPress(Keys.O, () => _camera.Orthographic = !_camera.Orthographic);

            base.Initialize();
        }

        private Point _lastMousePosition; 
        private void tryStartRotate(CubeLayerCriteria layer, bool positive)
        {
            if (_autoRotateMode)
            {
                _cube.TryStartAutoRotate(layer, positive);
            }
            else
            {
                if (_cube.TryEnterFreeRotationMode(layer, positive))
                {
                    _lastMousePosition = Mouse.GetState().Position;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            var keyboardState = Keyboard.GetState();
            _keyboard.Update(keyboardState);

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                _cube.TryStepFreeRotation(-0.05f);
            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                _cube.TryStepFreeRotation(0.05f);
            }
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                _cube.TryFinishFreeRotate();
            }
            if (keyboardState.IsKeyDown(Keys.U))
            {
                _undoStack.TryUndo();
            }

            foreach (var k in Controls.All.Where(k => keyboardState.IsKeyDown(k.Key)))
            {
                tryStartRotate(k.Layer, !keyboardState.IsKeyDown(Keys.LeftShift));
            }

            _cube.Update(gameTime);

            var mouse = Mouse.GetState();
            var mousePosition = mouse.Position;
            var mouseMovement = (mousePosition - _lastMousePosition).ToVector2().Y;
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                _cube.TryFinishFreeRotate();
            }
            _lastMousePosition = mousePosition;

            _cube.TryStepFreeRotation(mouseMovement/100f);

            base.Update(gameTime);
        }

        private readonly RasterizerState _rasterizerState = new RasterizerState
        {
            CullMode = CullMode.CullClockwiseFace
        };

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.RasterizerState = _rasterizerState;

            GraphicsDevice.Clear(new Color(51, 51, 51));

            _cube.Draw(_camera);

            _spriteBatch.Begin();
            drawUiText();
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private readonly Func<Game1,string>[] _statusStringFunctions =
        {
            g => string.Format("Camera Projection: {0} ([O] to toggle)", g._camera.Orthographic ? "Orthographic" : "Perspective"),
            g => string.Format("Movement Mode = {0}; [Enter] to toggle", g._autoRotateMode ? "Auto" : "Free (use mouse / arrow keys)"),
            g => string.Format("Active Layer = {0}, {1}", g._cube.ActiveLayer.Axis, g._cube.ActiveLayer.Landmark),
            g => string.Format("Active Layer Rotation = {0}", g._cube.FreeRotateTotalAngle),
            g => string.Format("Rotation Direction: {0}", g._cube.RotatePositive ? "Positive" : "Negative"),
            g => string.Format("Cube State = {0}", g._cube.State),
        };

        private readonly Func<Game1,string>[] _controlStringFunctions =
        {
            g => string.Format("Rotate X layers: [{0}] [{1}] [{2}]", Controls.X1.Key, Controls.X2.Key, Controls.X3.Key),
            g => string.Format("Rotate Y layers: [{0}] [{1}] [{2}]", Controls.Y1.Key, Controls.Y2.Key, Controls.Y3.Key),
            g => string.Format("Rotate Z layers: [{0}] [{1}] [{2}]", Controls.Z1.Key, Controls.Z2.Key, Controls.Z3.Key),
            g => string.Format("Undo: U ({0} Left)", g._undoStack.Count)
        };

        private void drawUiText()
        {
            drawLines(
                12, 12, 24,
                _controlStringFunctions);

            drawLines(
                12,
                GraphicsDevice.Viewport.Height - 24,
                -24,
                _statusStringFunctions);
        }

        private void drawLines(float x, float y0, float yInterval, IEnumerable<Func<Game1,string>> stringFns)
        {
            var yOffset = 0f;
            foreach (var s in stringFns)
            {
                _spriteBatch.DrawString(_font, s(this), new Vector2(x, y0 + yOffset), Color.White);
                yOffset += yInterval;
            }
        }
    }
}