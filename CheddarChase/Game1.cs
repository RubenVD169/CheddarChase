using CheddarChase.States;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CheddarChase
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        public SpriteFont Font { get; set; }
        public Dictionary<string, Texture2D> Assets { get; set; }
        public Texture2D Background { get; set; }
        public Vector2 BackgroundPosition { get; set; } = new Vector2(0, 0);
        public int Levens { get; set; }

        private AbstractState CurrentState;


        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Levens = 0;

        }

        public void ChangeState(AbstractState newState)
        {
            CurrentState = newState;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            const int width = 1080;
            const int height = 720;

            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();

            this.CurrentState = new StartScreenState(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Assets = new Dictionary<string, Texture2D>();
            Background = Content.Load<Texture2D>("background");
            Font = Content.Load<SpriteFont>("GameFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CurrentState.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            SpriteBatch.Begin();
            SpriteBatch.Draw(Background, BackgroundPosition, Color.White);
            SpriteBatch.DrawString(Font, "Aantal kaasjes: " + Levens, new Vector2(10, 10), Color.White);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}