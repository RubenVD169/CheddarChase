using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CheddarChase.States {
    public class StartScreenState : AbstractState {
        public StartScreenState(Game1 game) : base(game) {
        }

        public override void Draw(GameTime gameTime) {
            
        }

        public override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) {
                game.ChangeState(new PlayingState(game));
            }
        }
    }
}
