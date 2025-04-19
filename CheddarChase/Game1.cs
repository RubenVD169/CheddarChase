using CheddarChase.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CheddarChase {
    public class Game1 : Game {
        public GraphicsDeviceManager Graphics { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public SpriteFont Font { get; set; }
        public Dictionary<string, Texture2D> Assets { get; set; }

        private AbstractState CurrentState;

        public Game1() {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        public void ChangeState(AbstractState newState) {
            CurrentState = newState;
        }

        protected override void Initialize() {
            Graphics.PreferredBackBufferWidth = 1080;
            Graphics.PreferredBackBufferHeight = 720;
            Graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent() {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Assets = new Dictionary<string, Texture2D> {
                ["background"] = Content.Load<Texture2D>("background"),
                ["muis"] = Content.Load<Texture2D>("muis"),
                ["kat"] = Content.Load<Texture2D>("kat"),
                ["kaas"] = Content.Load<Texture2D>("kaas"),
                ["nietGenomenKaas"] = Content.Load<Texture2D>("nietGenomenKaas"),
                ["leven"] = Content.Load<Texture2D>("leven"),
                ["verlorenLeven"] = Content.Load<Texture2D>("verlorenLeven")
            };
            Font = Content.Load<SpriteFont>("GameFont");

            CurrentState = new StartScreenState(this);
        }

        protected override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CurrentState.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            CurrentState.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}