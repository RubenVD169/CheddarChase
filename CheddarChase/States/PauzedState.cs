using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CheddarChase.States {
    public class PauzedState : PlayingState {
        // Houdt een referentie bij naar de oorspronkelijke PlayingState
        private PlayingState originState;

        // Constructor: slaat de oorspronkelijke PlayingState op
        public PauzedState(Game1 game, PlayingState playingState) : base(game) {
            originState = playingState; // Bewaar de huidige speeltoestand
        }

        public override void Update(GameTime gameTime) {
            // Controleer of de Enter-toets is ingedrukt
            if (Keyboard.GetState().IsKeyDown(Keys.Enter)) {
                // Keer terug naar de oorspronkelijke PlayingState
                game.ChangeState(originState);
            }
        }

        public override void Draw(GameTime gameTime) {
            // Teken de oorspronkelijke PlayingState (achtergrond en spelobjecten)
            originState.Draw(gameTime);

            // Begin met tekenen van de pauzetekst
            game.SpriteBatch.Begin();
            game.SpriteBatch.DrawString(game.Font, "Game Paused", new Vector2(400, 280), Color.White);
            game.SpriteBatch.DrawString(game.Font, "Press ENTER to continue", new Vector2(325, 350), Color.White);
            game.SpriteBatch.End();
        }
    }
}