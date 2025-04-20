using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CheddarChase.States {
    public class PauzedState : PlayingState {
        private PlayingState originState;

        public PauzedState(Game1 game, PlayingState playingState) : base(game) {
            originState = playingState;
        }

        public override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) {
                game.ChangeState(originState);
            }
        }

        public override void Draw(GameTime gameTime) {
            originState.Draw(gameTime);
            game.SpriteBatch.Begin();
            game.SpriteBatch.DrawString(game.Font, "Game Paused", new Vector2(400, 280), Color.White);
            game.SpriteBatch.DrawString(game.Font, "Press Enter to continue", new Vector2(325, 350), Color.White);
            game.SpriteBatch.End();
        }
    }
}
