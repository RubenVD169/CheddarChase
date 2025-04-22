using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CheddarChase.States {
    public class WinState : AbstractState {
        private double playTimeInSeconds;
        private int score;

        public WinState(Game1 game, double totalPlayTimeSeconds) : base(game) {
            playTimeInSeconds = totalPlayTimeSeconds;
            score = CalculateScore(playTimeInSeconds);
        }

        private int CalculateScore(double seconds) {
            // Simpele formule waar de score wordt berekent op basis van de tijd: hoe sneller je wint doet, hoe hoger de score.
            return Math.Max(100000 - (int)(seconds * 100), 0);
        }

        public override void Update(GameTime gameTime) {
            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Enter)) {
                game.ChangeState(new StartScreenState(game));
            }
        }

        public override void Draw(GameTime gameTime) {
            game.GraphicsDevice.Clear(Color.Black);

            game.SpriteBatch.Begin();
            game.SpriteBatch.Draw(game.Assets["backgroundWin"], Vector2.Zero, Color.White);
            game.SpriteBatch.DrawString(game.Font, "YOU WIN!", new Vector2(450, 250), Color.Yellow);
            game.SpriteBatch.DrawString(game.Font, $"Score: {score}", new Vector2(425, 300), Color.White);
            game.SpriteBatch.DrawString(game.Font, $"Time: {playTimeInSeconds:F2} seconds", new Vector2(370, 350), Color.White);
            game.SpriteBatch.DrawString(game.Font, "Press ENTER to return to Start", new Vector2(300, 400), Color.White);
            game.SpriteBatch.End();
        }
    }
}
