using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CheddarChase.States {
    public class StartScreenState : AbstractState {
        public StartScreenState(Game1 game) : base(game) { }

        public override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) {
                game.ChangeState(new PlayingState(game));
                gameTime.TotalGameTime = TimeSpan.Zero;
            }
        }

        public override void Draw(GameTime gameTime) {
            game.SpriteBatch.Begin();
            game.SpriteBatch.Draw(game.Assets["backgroundStart"], Vector2.Zero, Color.White);
            game.SpriteBatch.DrawString(game.Font, "Press ENTER to Start", new Vector2(350, 300), Color.White);
            game.SpriteBatch.End();
        }
    }
}
