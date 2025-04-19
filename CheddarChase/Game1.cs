using CheddarChase.States;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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
        public int cheeseAmount { get; set; }
        public int lives { get; set; }
        private Texture2D mouse;
        private Vector2 mousePosition = Vector2.Zero;
        private int mouseMovement = 5;
        private Texture2D cat;
        private Vector2 catPosition = new Vector2(500, 500);
        private int catMovement = 1;
        private bool ActiveGame = true;
        private bool canTakeLife = true;
        private double collisionCooldown = 2.0; // seconden
        private double cooldownTimer = 0;
        private Texture2D cheese;
        private Vector2 cheeseCount = new Vector2(10, 15);
        private List<Vector2> cheesePositions = new List<Vector2>();
        private TimeSpan lastSpawnedCheese = TimeSpan.Zero;
        private Vector2 newCheese;

        private AbstractState CurrentState;


        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            cheeseAmount = 0;
            lives = 3;
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
            mouse = Content.Load<Texture2D>("muis");
            cat = Content.Load<Texture2D>("kat");
            cheese = Content.Load<Texture2D>("kaas");
            Font = Content.Load<SpriteFont>("GameFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CurrentState.Update(gameTime);

            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (mousePosition.Y - mouseMovement >= 0)
                    mousePosition.Y -= mouseMovement;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (mousePosition.Y + mouseMovement + mouse.Height <= Background.Height)
                    mousePosition.Y += mouseMovement;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (mousePosition.X - mouseMovement >= 0)
                    mousePosition.X -= mouseMovement;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (mousePosition.X + mouseMovement + mouse.Width <= Background.Width)
                    mousePosition.X += mouseMovement;
            }
            if (lives < 1)
            {
                ActiveGame = false;
            }

            // Calculate direction vector from cat to mouse

            Vector2 direction = mousePosition - catPosition;

            // Only move if the cat is not already at the mouse
            if (direction.Length() > 1f)
            {
                direction.Normalize(); // Make the vector's length 1
                catPosition += direction * catMovement; // Move cat toward mouse
            }

            Rectangle mouseCollisionRectangle = new Rectangle((int)mousePosition.X, (int)mousePosition.Y, mouse.Width, mouse.Height);
            Rectangle catCollisionRectangle = new Rectangle((int)catPosition.X, (int)catPosition.Y, cat.Width, cat.Height);

            // Collision check
            if (canTakeLife)
            {
                if (catCollisionRectangle.Intersects(mouseCollisionRectangle))
                {
                    lives--;
                    canTakeLife = false;
                    cooldownTimer = collisionCooldown; // start cooldown
                }
            }
            else
            {
                // Aftellen tot cooldown voorbij is
                cooldownTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (cooldownTimer <= 0)
                {
                    canTakeLife = true;
                }
            }
            if (cheesePositions.Count < 3)
            {
                var random = new Random();
                newCheese = new Vector2(random.Next(20, 960), random.Next(20, 680));
                cheesePositions.Add(newCheese);
                lastSpawnedCheese = gameTime.TotalGameTime;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (ActiveGame)
            {
                SpriteBatch.Begin();
                SpriteBatch.Draw(Background, BackgroundPosition, Color.White);
                SpriteBatch.Draw(mouse, new Rectangle((int)mousePosition.X, (int)mousePosition.Y, mouse.Width, mouse.Height), Color.White);
                SpriteBatch.Draw(cat, new Rectangle((int)catPosition.X, (int)catPosition.Y, cat.Width, cat.Height), Color.White);
                SpriteBatch.Draw(cheese, new Rectangle((int)cheeseCount.X, (int)cheeseCount.Y, 40, 40), Color.White);
                SpriteBatch.DrawString(Font, "Levens: " + lives, new Vector2(10, 50), Color.White);
                for (int i = 0; i < cheesePositions.Count; i++)
                {
                    Vector2 position = cheesePositions[i];
                    SpriteBatch.Draw(cheese, new Rectangle((int)position.X, (int)position.Y, 40, 40), Color.White);
                }
                SpriteBatch.End();
            }
            else
            {
                SpriteBatch.Begin();
                SpriteBatch.DrawString(Font, "Game Over", new Vector2(400, 300), Color.White);
                SpriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}