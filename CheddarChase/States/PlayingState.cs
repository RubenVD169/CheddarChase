using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CheddarChase.States {
    public class PlayingState : AbstractState {
        public int Levens { get; set; }

        public PlayingState(Game1 game) : base(game) {
            Levens = 3;           
        }

        public override void Draw(GameTime gameTime) {
           
        }

        public override void Update(GameTime gameTime) {
           

            if (Levens < 1) {
                game.ChangeState(new GameOverState(game));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P)) {
                game.ChangeState(new PauzedState(game, this));
            }
        }
    }

}
