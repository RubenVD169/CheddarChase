using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CheddarChase.States {
    public class PlayingState : AbstractState {
        // Textures en posities voor de muis en kat
        private Texture2D mouse;
        private Vector2 mousePosition = Vector2.Zero;
        private int mouseMovement = 5;

        private Texture2D cat;
        private Vector2 catPosition = new Vector2(500, 500);
        private float catMovement = 1f;

        // Lijst met kaasposities en textures voor kaas
        private List<Vector2> cheesePositions = [];
        private Texture2D yellowCheese;
        private Texture2D grayCheese;

        // Aantal verzamelde kazen en levens van de muis en kat
        private int cheeseAmount = 0;
        private int mouseLives = 3;
        private int catLives = 9;

        // Textures voor levens (harten)
        private Texture2D redHeart;
        private Texture2D grayHeart;

        // Richtingseffecten voor de muis en kat
        private SpriteEffects mouseFlip = SpriteEffects.None;
        private SpriteEffects catFlip = SpriteEffects.None;

        // Variabelen voor schade-effecten
        private bool isCatHurt = false;
        private bool isMouseHurt = false;
        private double hurtDuration = 0.2; // Duur van het "gewond"-effect
        private double hurtTimer = 0;

        // Variabelen voor levelovergang
        private bool nextLevelText = false;
        private int currentLevel = 1;
        private double levelTimer = 0;

        // Botsingslogica
        private bool canTakeLife = true;
        private double collisionCooldown = 2.0; // Tijd waarin de kat geen leven kan afnemen
        private double cooldownTimer = 0;

        public PlayingState(Game1 game) : base(game) {
            // Laad de benodigde assets vanuit het spel
            mouse = game.Assets["muis"];
            cat = game.Assets["kat"];
            yellowCheese = game.Assets["kaas"];
            grayCheese = game.Assets["nietGenomenKaas"];
            redHeart = game.Assets["leven"];
            grayHeart = game.Assets["verlorenLeven"];
        }

        public override void Update(GameTime gameTime) {
            var keyboard = Keyboard.GetState();

            // Pauzeer het spel als de speler op 'P' drukt
            if (keyboard.IsKeyDown(Keys.P)) {
                game.ChangeState(new PauzedState(game, this));
                return;
            }

            // Beweging van de muis met pijltjestoetsen
            if (keyboard.IsKeyDown(Keys.Up) && mousePosition.Y - mouseMovement >= 0)
                mousePosition.Y -= mouseMovement;
            else if (keyboard.IsKeyDown(Keys.Down) && mousePosition.Y + mouseMovement + mouse.Height <= game.Graphics.PreferredBackBufferHeight)
                mousePosition.Y += mouseMovement;

            if (keyboard.IsKeyDown(Keys.Left)) {
                if (mousePosition.X - mouseMovement >= 0)
                    mousePosition.X -= mouseMovement;
                mouseFlip = SpriteEffects.FlipHorizontally; // Draai de muis naar links
            }
            else if (keyboard.IsKeyDown(Keys.Right)) {
                if (mousePosition.X + mouseMovement + mouse.Width <= game.Graphics.PreferredBackBufferWidth)
                    mousePosition.X += mouseMovement;
                mouseFlip = SpriteEffects.None; // Draai de muis naar rechts
            }

            // Beweging van de kat richting de muis
            Vector2 direction = mousePosition - catPosition;
            if (direction.Length() > 1f) {
                direction.Normalize(); 
                catPosition += direction * catMovement; // Beweeg de kat
                catFlip = direction.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally; // Draai de kat
            }

            // Botsingsdetectie tussen de muis en de kat
            Rectangle mouseRect = new Rectangle((int)mousePosition.X, (int)mousePosition.Y, mouse.Width, mouse.Height);
            Rectangle catRect = new Rectangle((int)catPosition.X, (int)catPosition.Y, cat.Width, cat.Height);

            if (canTakeLife && catRect.Intersects(mouseRect)) {
                mouseLives--; // Verminder het aantal levens van de muis
                isMouseHurt = true; // Activeer het "gewond"-effect
                hurtTimer = hurtDuration;
                canTakeLife = false; // Start de cooldown
                cooldownTimer = collisionCooldown;
            }
            else if (!canTakeLife) {
                cooldownTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (cooldownTimer <= 0)
                    canTakeLife = true; // Heractiveer collision
            }

            // Spawn nieuwe kaas als er minder dan 3 kazen op het scherm zijn
            if (cheesePositions.Count < 3) {
                var random = new Random();
                cheesePositions.Add(new Vector2(random.Next(20, 960), random.Next(100, 680)));
            }

            // Controleer of de muis kaas verzamelt
            for (int i = 0; i < cheesePositions.Count; i++) {
                Rectangle cheeseRect = new Rectangle((int)cheesePositions[i].X, (int)cheesePositions[i].Y, yellowCheese.Width, yellowCheese.Height);
                if (mouseRect.Intersects(cheeseRect)) {
                    cheesePositions.RemoveAt(i); // Verwijder de kaas
                    cheeseAmount++; // Verhoog het aantal verzamelde kazen
                    break;
                }
            }

            // Ga naar het volgende level als 5 kazen zijn verzameld
            if (cheeseAmount == 5) {
                cheeseAmount = 0;
                nextLevelText = true; // Toon de leveltekst
                currentLevel++;
                catLives--; // Verminder het aantal levens van de kat
                isCatHurt = true; // Activeer het "gewond"-effect voor de kat
                hurtTimer = hurtDuration;
                catMovement += 0.3f; // Verhoog de snelheid van de kat
                levelTimer = 0;
            }

            // Toon de leveltekst tijdelijk
            if (nextLevelText) {
                levelTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (levelTimer > 1)
                    nextLevelText = false;
            }

            // Beheer het "gewond"-effect
            if (isCatHurt || isMouseHurt) {
                hurtTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (hurtTimer <= 0) {
                    isCatHurt = false;
                    isMouseHurt = false;
                }
            }

            // Controleer of het spel moet eindigen
            if (mouseLives < 1) {
                game.ChangeState(new GameOverState(game)); // Ga naar de GameOverState
            }
            if (catLives < 1) {
                game.ChangeState(new WinState(game, gameTime.TotalGameTime.TotalSeconds)); // Ga naar de WinState
            }
        }

        public override void Draw(GameTime gameTime) {
            game.SpriteBatch.Begin();

            // Teken de achtergrond
            game.SpriteBatch.Draw(game.Assets["background"], Vector2.Zero, Color.White);

            // Teken de muis en kat met hun respectieve kleuren (rood als ze gewond zijn)
            Color mouseColor = isMouseHurt ? Color.Red : Color.White;
            Color catColor = isCatHurt ? Color.Red : Color.White;

            game.SpriteBatch.Draw(mouse, new Rectangle((int)mousePosition.X, (int)mousePosition.Y, mouse.Width, mouse.Height), null, mouseColor, 0f, Vector2.Zero, mouseFlip, 0f);
            game.SpriteBatch.Draw(cat, new Rectangle((int)catPosition.X, (int)catPosition.Y, cat.Width, cat.Height), null, catColor, 0f, Vector2.Zero, catFlip, 0f);

            // Teken de verzamelde kazen (linksboven)
            Vector2 kaasPos = new Vector2(10, 10);
            for (int i = 0; i < 5; i++) {
                var kaasTexture = i < cheeseAmount ? yellowCheese : grayCheese;
                game.SpriteBatch.Draw(kaasTexture, new Rectangle((int)kaasPos.X, (int)kaasPos.Y, 40, 40), Color.White);
                kaasPos.X += 50;
            }

            // Teken de levens van de muis (onder de verzamelde kazen)
            Vector2 muisLeven = new Vector2(10, 60);
            for (int i = 0; i < 3; i++) {
                var levenTexture = i < mouseLives ? redHeart : grayHeart;
                game.SpriteBatch.Draw(levenTexture, new Rectangle((int)muisLeven.X, (int)muisLeven.Y, 40, 40), Color.White);
                muisLeven.X += 50;
            }

            // Teken de levens van de kat (rechtsboven)
            Vector2 katLeven = new Vector2(635, 10);
            for (int i = 0; i < 9; i++) {
                var levenTexture = i < catLives ? redHeart : grayHeart;
                game.SpriteBatch.Draw(levenTexture, new Rectangle((int)katLeven.X, (int)katLeven.Y, 40, 40), Color.White);
                katLeven.X += 50;
            }

            // Teken de kazen op het speelveld
            foreach (var kaas in cheesePositions) {
                game.SpriteBatch.Draw(yellowCheese, new Rectangle((int)kaas.X, (int)kaas.Y, 40, 40), Color.White);
            }

            // Toon de leveltekst als het level net is verhoogd
            if (nextLevelText) {
                game.SpriteBatch.DrawString(game.Font, $"Level {currentLevel}", new Vector2(450, 300), Color.White);
            }

            game.SpriteBatch.End();
        }
    }
}