using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace CheddarChase.States
{
    public class PlayingState : AbstractState
    {
        public int Levens { get; set; }
        public Texture2D Background { get; set; }
        public Vector2 BackgroundPosition { get; set; } = new Vector2(0, -700);
        SpriteBatch SpriteBatch { get; set; }



        public PlayingState(Game1 game) : base(game)
        {
            Levens = 3;
            Background = game.Assets["background"];
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(Background, BackgroundPosition, Color.White);
            SpriteBatch.DrawString(game.Font, "Levens: " + Levens, new Vector2(10, 10), Color.White);
            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {


            if (Levens < 1)
            {
                game.ChangeState(new GameOverState(game));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                game.ChangeState(new PauzedState(game, this));
            }
        }
    }

}
