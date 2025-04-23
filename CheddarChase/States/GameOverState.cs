using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CheddarChase.States {
    public class GameOverState : AbstractState {
        public GameOverState(Game1 game) : base(game) { }

        public override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) {                
                game.ChangeState(new StartScreenState(game));// Verander de toestand van het spel naar het Startscherm
            }
        }

        public override void Draw(GameTime gameTime) {
            game.SpriteBatch.Begin();
            // Teken de achtergrond van het Game Over-scherm
            game.SpriteBatch.Draw(game.Assets["backgroundGameOver"], Vector2.Zero, Color.White);
            // Teken de tekst in het midden van het scherm
            game.SpriteBatch.DrawString(game.Font, "Game Over", new Vector2(430, 280), Color.Red);
            game.SpriteBatch.DrawString(game.Font, "Press ENTER to Restart", new Vector2(325, 350), Color.Red);
            game.SpriteBatch.End();
        }
    }
}