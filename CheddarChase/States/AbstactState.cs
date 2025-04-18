using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CheddarChase.States {
    public abstract class AbstractState {
        protected Game1 game;

        public AbstractState(Game1 game) {
            this.game = game;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
