using CheddarChase.States;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Runtime.CompilerServices;

namespace CheddarChase {
    public class Game1 : Game {
        public GraphicsDeviceManager Graphics { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public SpriteFont Font { get; set; }
        public Dictionary<string, Texture2D> Assets { get; set; }
        public Texture2D Background { get; set; }
        public Vector2 BackgroundPosition { get; set; } = new Vector2(0, 0);
        public int cheeseAmount { get; set; }
        public int mouseLives { get; set; } = 3;
        public int catLives { get; set; } = 9;
        private Texture2D mouse;
        private Vector2 mousePosition = Vector2.Zero;
        private int mouseMovement = 5;
        private Texture2D cat;
        private Vector2 catPosition = new Vector2(500, 500);
        private float catMovement = 1f;
        private bool ActiveGame = true;
        private bool canTakeLife = true;
        private double collisionCooldown = 2.0; // seconden
        private double cooldownTimer = 0;
        private Texture2D yellowCheese;
        private Vector2 cheeseCount = new Vector2(10, 15);
        private List<Vector2> cheesePositions = new List<Vector2>();
        private TimeSpan lastSpawnedCheese = TimeSpan.Zero;
        private Vector2 newCheese;
        private Texture2D grayCheese;
        private Vector2 mouseLife = new Vector2(10, 60);
        private Vector2 catLife = new Vector2(700, 60);
        private Texture2D redHeart;
        private Texture2D grayHeart;
        private bool nextLevelText = false;
        private int currentLevel = 1;
        private double levelTimer = 0;
        private SpriteEffects mouseFlip = SpriteEffects.None;
        private SpriteEffects catFlip = SpriteEffects.None;
        private bool isCatHurt = false;
        private bool isMouseHurt = false;
        private double hurtDuration = 0.2; // seconds the cat turns red
        private double hurtTimer = 0;


        private AbstractState CurrentState;


        public Game1() {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            cheeseAmount = 0;
        }

        public void ChangeState(AbstractState newState) {
            CurrentState = newState;
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here
            const int width = 1080;
            const int height = 720;

            Graphics.PreferredBackBufferWidth = width;
            Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();

            this.CurrentState = new StartScreenState(this);

            base.Initialize();
        }

        protected override void LoadContent() {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Assets = new Dictionary<string, Texture2D>();
            Background = Content.Load<Texture2D>("background");
            mouse = Content.Load<Texture2D>("muis");
            cat = Content.Load<Texture2D>("kat");
            yellowCheese = Content.Load<Texture2D>("kaas");
            grayCheese = Content.Load<Texture2D>("nietGenomenKaas");
            redHeart = Content.Load<Texture2D>("leven");
            grayHeart = Content.Load<Texture2D>("verlorenLeven");
            Font = Content.Load<SpriteFont>("GameFont");
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            CurrentState.Update(gameTime);

            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.Up)) {
                if (mousePosition.Y - mouseMovement >= 0)
                    mousePosition.Y -= mouseMovement;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Down)) {
                if (mousePosition.Y + mouseMovement + mouse.Height <= Background.Height)
                    mousePosition.Y += mouseMovement;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (mousePosition.X - mouseMovement >= 0)
                    mousePosition.X -= mouseMovement;
                mouseFlip = SpriteEffects.FlipHorizontally;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (mousePosition.X + mouseMovement + mouse.Width <= Background.Width)
                    mousePosition.X += mouseMovement;
                mouseFlip = SpriteEffects.None;
            }
            if (mouseLives < 1) {
                ActiveGame = false;
            }

            // Calculate direction vector from cat to mouse

            Vector2 direction = mousePosition - catPosition;

            // Only move if the cat is not already at the mouse
            if (direction.Length() > 1f) {
                direction.Normalize(); // Make the vector's length 1
                catPosition += direction * catMovement; // Move cat toward mouse
            }
            if (Math.Abs(direction.X) > 0.1f)
            {
                catFlip = direction.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }

            Rectangle mouseCollisionRectangle = new Rectangle((int)mousePosition.X, (int)mousePosition.Y, mouse.Width, mouse.Height);
            Rectangle catCollisionRectangle = new Rectangle((int)catPosition.X, (int)catPosition.Y, cat.Width, cat.Height);

            // Collision check
            if (canTakeLife)
            {
                if (catCollisionRectangle.Intersects(mouseCollisionRectangle))
                {
                    mouseLives--;

                    // Trigger the red flash for mouse
                    isMouseHurt = true;
                    hurtTimer = hurtDuration;

                    canTakeLife = false;
                    cooldownTimer = collisionCooldown;
                }
            }
            else {
                // Aftellen tot cooldown voorbij is
                cooldownTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (cooldownTimer <= 0) {
                    canTakeLife = true;
                }
            }
            if (cheesePositions.Count < 3) {
                var random = new Random();
                newCheese = new Vector2(random.Next(20, 960), random.Next(100, 680));
                cheesePositions.Add(newCheese);
                lastSpawnedCheese = gameTime.TotalGameTime;
            }
            for (int i = 0; i < cheesePositions.Count; i++) {
                Vector2 position = cheesePositions[i];
                Rectangle cheeseCollisionRectangle = new Rectangle((int)position.X, (int)position.Y, yellowCheese.Width, yellowCheese.Height);
                if (mouseCollisionRectangle.Intersects(cheeseCollisionRectangle)) {
                    cheesePositions.RemoveAt(i);
                    cheeseAmount++;
                    cheeseCount = new Vector2(cheeseCount.X, cheeseCount.Y);
                    break; // Exit the loop after removing the cheese
                }
            }
            if (cheeseAmount == 5)
            {
                cheeseAmount = 0;
                nextLevelText = true;
                currentLevel++;
                catLives--;

                // Trigger the red flash
                isCatHurt = true;
                hurtTimer = hurtDuration;

                catMovement += 0.3f;
                levelTimer = 0;
            }

            // Update de timer altijd als de tekst actief is
            if (nextLevelText)
            {
                levelTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (levelTimer > 1) // Na 2 seconden verbergen
                {
                    nextLevelText = false;
                }
            }
            if (isCatHurt)
            {
                hurtTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (hurtTimer <= 0)
                {
                    isCatHurt = false;
                }
            }
            if (isMouseHurt)
            {
                hurtTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (hurtTimer <= 0)
                {
                    isMouseHurt = false;
                }
            }
            if (catLives < 1)
            {
                ActiveGame = false;
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (ActiveGame) {
                SpriteBatch.Begin();
                SpriteBatch.Draw(Background, BackgroundPosition, Color.White);
                Color mouseColor = isMouseHurt ? Color.Red : Color.White;
                SpriteBatch.Draw(mouse, new Rectangle((int)mousePosition.X, (int)mousePosition.Y, mouse.Width, mouse.Height), null, mouseColor, 0f, Vector2.Zero, mouseFlip, 0f);
                Color catColor = isCatHurt ? Color.Red : Color.White;
                SpriteBatch.Draw(cat, new Rectangle((int)catPosition.X, (int)catPosition.Y, cat.Width, cat.Height), null, catColor, 0f, Vector2.Zero, catFlip, 0f);

                // Draw the cheese count
                cheeseCount = new Vector2(10, 10);
                for (int i = 0; i < 5; i++) {
                    Texture2D kaasTexture = i < cheeseAmount ? yellowCheese : grayCheese;
                    SpriteBatch.Draw(kaasTexture, new Rectangle((int)cheeseCount.X, (int)cheeseCount.Y, 40, 40), Color.White);
                    cheeseCount.X += 50;
                }
                //Draw the lives of the mouse
                mouseLife = new Vector2(10, 60);
                for (int i = 0; i < 3; i++) {
                    Texture2D levenTexture = i < mouseLives ? redHeart : grayHeart;
                    SpriteBatch.Draw(levenTexture, new Rectangle((int)mouseLife.X, (int)mouseLife.Y, 40, 40), Color.White);
                    mouseLife.X += 50;
                }
                // Draw the lives of the cat
                catLife = new Vector2(635, 10);
                for (int i = 0; i < 9; i++) {
                    Texture2D levenTexture = i < catLives ? redHeart : grayHeart;
                    SpriteBatch.Draw(levenTexture, new Rectangle((int)catLife.X, (int)catLife.Y, 40, 40), Color.White);
                    catLife.X += 50;
                }


                // Cheese spawns
                for (int i = 0; i < cheesePositions.Count; i++) {
                    Vector2 position = cheesePositions[i];
                    SpriteBatch.Draw(yellowCheese, new Rectangle((int)position.X, (int)position.Y, 40, 40), Color.White);
                }
                if (nextLevelText)
                {
                    SpriteBatch.DrawString(Font, $"Level {currentLevel}", new Vector2(450, 300), Color.White);
                }
                SpriteBatch.End();
            }     
            else {
                SpriteBatch.Begin();
                SpriteBatch.DrawString(Font, "Game Over", new Vector2(400, 300), Color.White);
                SpriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}