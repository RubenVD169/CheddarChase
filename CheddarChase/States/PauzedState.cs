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
            if (Keyboard.GetState().IsKeyDown(Keys.P)) {
                game.ChangeState(originState);
            }
        }

        public override void Draw(GameTime gameTime) {
            originState.Draw(gameTime);
            game.SpriteBatch.Begin();
            game.SpriteBatch.DrawString(game.Font, "Game Paused", new Vector2(400, 200), Color.Yellow);
            game.SpriteBatch.End();
        }
    }
}
