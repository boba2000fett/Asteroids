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
    public class ReadyToPlayScreen : GameScreen
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont instructionsFont;

        TimeSpan elapsedTime = TimeSpan.Zero;

        Texture2D backgroundImage;

        HighScore highScore;
        
        public ReadyToPlayScreen()
        {
            LoadContent();
        }

        public ReadyToPlayScreen(HighScore highScoreObject)
        {
            LoadContent();
            highScore = highScoreObject;
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(StateManager.GraphicsDevice);
            spriteFont = StateManager.Content.Load<SpriteFont>("font");
            instructionsFont = StateManager.Content.Load<SpriteFont>("InstructionsFont");
            backgroundImage = StateManager.Content.Load<Texture2D>("InstructionsBackgroundScreen");
        }

        public override void Update(GameTime gameTime, StateManager screen, 
            GamePadState gamePadState, MouseState mouseState, 
            KeyboardState keyState, InputHandler input)
        {
            if (input.KeyboardState.WasKeyPressed(Keys.Space) ||
                input.WasPressed(0, InputHandler.ButtonType.A, Keys.A))
            {
                MediaPlayer.Stop();
                
                PlayScreen playScreen = new PlayScreen(highScore);
                screen.Pop();
                screen.Push(playScreen);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backgroundImage, new Vector2(0, 0), Color.White);

            Vector2 center = new Vector2(
                StateManager.GraphicsDevice.Viewport.Width / 2,
                StateManager.GraphicsDevice.Viewport.Height / 2);

            string msg;

            msg = "Instructions";
            Vector2 v = spriteFont.MeasureString(msg) / new Vector2(2, 2);
            spriteBatch.DrawString(spriteFont, msg, new Vector2(v.X + 10, 60), Color.White);

            #region Display Controls

            msg = "Controls";
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(260, 250), Color.White);

            msg = "Space: Thrust Ship";
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(260, 295), Color.White);

            msg = "L/R Arrows: Rotate Ship";
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(260, 340), Color.White);

            msg = "Enter : Fire Laser";
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(260, 385), Color.White);

            msg = "P: Pause Game";
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(260, 430), Color.White);

            msg = "ESC: Exit Game";
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(260, 475), Color.White);

            #endregion

            #region Display Goal

            msg = "Goal:";
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(630, 250 + 30), Color.White);

            msg = "Shoot Rocks";
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(630, 295 + 30), Color.White);

            msg = "Shoot Enemy Ships";
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(630, 340 + 30), Color.White);

            msg = "Avoid All Obstacles";
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(630, 385 + 30), Color.White);

            
            #endregion

            msg = "Press SPACE to Play!";
            v = instructionsFont.MeasureString(msg) / new Vector2(2, 2);
            Vector2 buffer = center - v;
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(buffer.X, buffer.Y + 250), Color.White);

            spriteBatch.End();
        }
    }
}
