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
            // Simple formula: the faster the better
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
            game.SpriteBatch.DrawString(game.Font, "YOU WIN!", new Vector2(400, 200), Color.Yellow);
            game.SpriteBatch.DrawString(game.Font, $"Time: {playTimeInSeconds:F2} seconds", new Vector2(400, 250), Color.White);
            game.SpriteBatch.DrawString(game.Font, $"Score: {score}", new Vector2(400, 300), Color.White);
            game.SpriteBatch.DrawString(game.Font, "Press Enter to return to Start", new Vector2(400, 350), Color.Gray);
            game.SpriteBatch.End();
        }
    }
}
