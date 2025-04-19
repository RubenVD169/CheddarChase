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
                game.ChangeState(new StartScreenState(game));
            }
        }

        public override void Draw(GameTime gameTime) {
            game.SpriteBatch.Begin();
            game.SpriteBatch.DrawString(game.Font, "Game Over", new Vector2(400, 300), Color.Red);
            game.SpriteBatch.DrawString(game.Font, "Press ENTER to Restart", new Vector2(350, 350), Color.White);
            game.SpriteBatch.End();
        }
    }
}
