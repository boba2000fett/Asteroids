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
    public class SplashScreen : GameScreen
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        TimeSpan elapsedTime = TimeSpan.Zero;

        Texture2D splashImage;

        HighScore highScore;

        List<Song> songs;
        List<SoundEffect> soundEffects;

        float splashAlpha;

        public SplashScreen()
        {
            LoadContent();
        }

        public SplashScreen(HighScore highScoreObject)
        {
            highScore = highScoreObject;
            LoadContent();
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(StateManager.GraphicsDevice);
            spriteFont = StateManager.Content.Load<SpriteFont>("font");
        
            splashImage = StateManager.Content.Load<Texture2D>("AsteroidsSplash");

            songs = new List<Song>();
            soundEffects = new List<SoundEffect>();

            songs.Add(StateManager.Game.Content.Load<Song>("2021-06-29_-_The_Pain_That_Never_Left_-_David_Fesliyan"));

            splashAlpha = 0;
        }

        public override void Update(GameTime gameTime, StateManager screen, 
            GamePadState gamePadState, MouseState mouseState, 
            KeyboardState keyState, InputHandler input)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(songs[0], TimeSpan.FromSeconds(1));
            }
            else if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
            }

            elapsedTime += gameTime.ElapsedGameTime;

            if (input.KeyboardState.WasKeyPressed(Keys.Space) || 
                input.WasPressed(0, InputHandler.ButtonType.A, Keys.A))
            {
                MenuScreen studio = new MenuScreen(highScore);
                screen.Pop();
                screen.Push(studio);
            }
            if (elapsedTime >= TimeSpan.FromSeconds(7))
            {
                MenuScreen studio = new MenuScreen(highScore);
                screen.Pop();
                screen.Push(studio);
            }

            #region Fading in and Out Logic
            if (splashAlpha < 1.0f && elapsedTime < TimeSpan.FromSeconds(3)) 
            { 
                splashAlpha += 0.01f; 
            }
            if (splashAlpha >= 1.0f)
            {               
                splashAlpha = 1.0f;
            }
            if (splashAlpha > 0.0f && elapsedTime > TimeSpan.FromSeconds(3.5)) 
            { 
                splashAlpha -= 0.01f; 
            }
            if (splashAlpha <= 0.0f )
            {
                splashAlpha = 0.0f;                
            }
            #endregion

        }

        public override void Draw(GameTime gameTime)
        {

            StateManager.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            Vector2 center = new Vector2(
                StateManager.GraphicsDevice.Viewport.Width / 2,
                StateManager.GraphicsDevice.Viewport.Height / 2);

            string msg = "Splash Screen";

            Vector2 v = spriteFont.MeasureString(msg) / new Vector2(2, 2);

            spriteBatch.Draw(splashImage, new Vector2(0, 0), Color.White * splashAlpha);

            spriteBatch.End();
        }
    }
}




