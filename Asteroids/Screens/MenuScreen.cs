using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Asteroids.Screens
{
    public class MenuScreen : GameScreen
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont menuFont;

        Texture2D menuImage;

        HighScore highScore;

        List<Song> songs;
        List<SoundEffect> soundEffects;

        public MenuScreen()
        {
            LoadContent();
        }

        public MenuScreen(HighScore highScoreObject)
        {
            highScore = highScoreObject;
            LoadContent();
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(StateManager.GraphicsDevice);
            spriteFont = StateManager.Content.Load<SpriteFont>("font");
            menuFont = StateManager.Content.Load<SpriteFont>("menu");
            
            menuImage = StateManager.Content.Load<Texture2D>("AsteroidsMenu");

            songs = new List<Song>();
            soundEffects = new List<SoundEffect>();

            songs.Add(StateManager.Game.Content.Load<Song>("2021-06-29_-_The_Pain_That_Never_Left_-_David_Fesliyan"));
        }

        public override void Update(GameTime gameTime, StateManager screen, 
            GamePadState gamePadState, MouseState mouseState, 
            KeyboardState keyState, InputHandler input)
        {
            if (input.KeyboardState.WasKeyPressed(Keys.P))
                //This goes to the Attract Screen
            {
                MediaPlayer.Stop();

                AttractScreen instructions = new AttractScreen(highScore);
                screen.Push(instructions);
            }
            if (input.KeyboardState.WasKeyPressed(Keys.C))
                //This goes to the Credits
            {
                CreditsScreen credits = new CreditsScreen();
                screen.Push(credits);
            }
            if (input.KeyboardState.WasKeyPressed(Keys.Escape))
                //This exits the game
            {
                Environment.Exit(0);
            }

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(songs[0],TimeSpan.FromSeconds(16.65));
            }
            else if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);

            Vector2 center = new Vector2(
                StateManager.GraphicsDevice.Viewport.Width / 2,
                StateManager.GraphicsDevice.Viewport.Height / 2);

            string msg = "Main Menu";

            Vector2 v = spriteFont.MeasureString(msg) / new Vector2(2,2);

            Vector2 buffer = center - v;
            
            spriteBatch.DrawString(spriteFont, msg, new Vector2(buffer.X, 175), Color.White);

            msg = "Play Game [P]";
            v = menuFont.MeasureString(msg) / new Vector2(2, 2);
            buffer = center - v;
            spriteBatch.DrawString(menuFont, msg, new Vector2(buffer.X, 300), Color.White);

            msg = "View Credits [C]";
            v = menuFont.MeasureString(msg) / new Vector2(2, 2);
            buffer = center - v;
            spriteBatch.DrawString(menuFont, msg, new Vector2(buffer.X, 400), Color.White);

            msg = "Exit Game [ESC]";
            v = menuFont.MeasureString(msg) / new Vector2(2, 2);
            buffer = center - v;
            spriteBatch.DrawString(menuFont, msg, new Vector2(buffer.X, 500), Color.White);

            spriteBatch.End();
        }
    }
}
