using CheddarChase.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace CheddarChase {
    public class Game1 : Game {
        // Beheert de grafische instellingen van het spel
        public GraphicsDeviceManager Graphics { get; set; }

        // Wordt gebruikt om objecten op het scherm te tekenen
        public SpriteBatch SpriteBatch { get; set; }

        // Het lettertype dat wordt gebruikt om tekst in het spel weer te geven
        public SpriteFont Font { get; set; }

        // Een verzameling van alle geladen afbeeldingen (textures) in het spel
        public Dictionary<string, Texture2D> Assets { get; set; }

        // De huidige toestand van het spel (bijvoorbeeld Startscherm, Spelen, Pauze, Game Over)
        private AbstractState CurrentState;

        // Constructor: stelt de grafische instellingen in en wijst de contentmap toe
        public Game1() {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content"; // Map waar alle spelinhoud (zoals afbeeldingen) wordt opgeslagen
        }

        // Methode om de huidige toestand van het spel te wijzigen
        public void ChangeState(AbstractState newState) {
            CurrentState = newState;
        }

        // Wordt één keer aangeroepen bij het starten van het spel om instellingen te initialiseren
        protected override void Initialize() {
            // Stel de schermresolutie in
            Graphics.PreferredBackBufferWidth = 1080; // Breedte van het scherm
            Graphics.PreferredBackBufferHeight = 720; // Hoogte van het scherm
            Graphics.ApplyChanges(); // Pas de wijzigingen toe

            base.Initialize();
        }

        // Laadt alle benodigde inhoud (zoals afbeeldingen en lettertypen) in het spel
        protected override void LoadContent() {
            // Initialiseer de SpriteBatch voor het tekenen van objecten
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Laad alle afbeeldingen in een dictionary voor eenvoudig gebruik
            Assets = new Dictionary<string, Texture2D> {
                ["background"] = Content.Load<Texture2D>("background"), 
                ["backgroundStart"] = Content.Load<Texture2D>("backgroundStart"), 
                ["backgroundGameOver"] = Content.Load<Texture2D>("backgroundGameOver"), 
                ["backgroundWin"] = Content.Load<Texture2D>("backgroundWin"), 
                ["muis"] = Content.Load<Texture2D>("muis"),
                ["kat"] = Content.Load<Texture2D>("kat"),
                ["kaas"] = Content.Load<Texture2D>("kaas"),
                ["nietGenomenKaas"] = Content.Load<Texture2D>("nietGenomenKaas"), 
                ["leven"] = Content.Load<Texture2D>("leven"), 
                ["verlorenLeven"] = Content.Load<Texture2D>("verlorenLeven") 
            };

            // Laad het lettertype voor het weergeven van tekst
            Font = Content.Load<SpriteFont>("GameFont");

            // Stel de starttoestand van het spel in (Startscherm)
            CurrentState = new StartScreenState(this);
        }

        // Wordt elke frame aangeroepen om de spel-logica bij te werken
        protected override void Update(GameTime gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Roep de Update-methode van de huidige State aan
            CurrentState.Update(gameTime);

            base.Update(gameTime);
        }

        // Wordt elke frame aangeroepen om het spel te tekenen
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Roep de Draw-methode van de huidige State aan
            CurrentState.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}