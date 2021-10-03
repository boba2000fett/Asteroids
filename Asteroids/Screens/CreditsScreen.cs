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
    public class CreditsScreen : GameScreen
    {        
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont menuFont;
        SpriteFont instructionsFont;

        Texture2D menuImage;

        HighScore highScore;
     
        public CreditsScreen()
        {
            LoadContent();
        }

        public CreditsScreen(HighScore highScoreObject)
        {
            highScore = highScoreObject;
            LoadContent();
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(StateManager.GraphicsDevice);
            spriteFont = StateManager.Content.Load<SpriteFont>("font");
            menuFont = StateManager.Content.Load<SpriteFont>("menu");
            instructionsFont = StateManager.Content.Load<SpriteFont>("InstructionsFont");

            StateManager.Game.Window.Title = "Lunar Lander";

            menuImage = StateManager.Content.Load<Texture2D>("ProgramBackground");
        }

        public override void Update(GameTime gameTime, StateManager screen, 
            GamePadState gamePadState, MouseState mouseState, 
            KeyboardState keyState, InputHandler input)
        {
            if (input.KeyboardState.WasKeyPressed(Keys.Escape))
            {
                screen.Pop();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);

            Vector2 center = new Vector2(
                StateManager.GraphicsDevice.Viewport.Width / 2,
                StateManager.GraphicsDevice.Viewport.Height / 2);

            string msg = "Credits";

            Vector2 v = spriteFont.MeasureString(msg) / new Vector2(2,2);

            Vector2 buffer = center - v;
            
            spriteBatch.DrawString(spriteFont, msg, new Vector2(buffer.X, 150), Color.White);
            
            msg = "Programmed By: Trenton Andrews";
            v = instructionsFont.MeasureString(msg) / new Vector2(2, 2);
            buffer = center - v;
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(buffer.X, 250), Color.White);

            msg = "Sources / Special Thanks";
            v = instructionsFont.MeasureString(msg) / new Vector2(2, 2);
            buffer = center - v;
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(buffer.X, 310), Color.White);

            msg = "'8 Bit Retro Funk' by David Renda, from FesliyanStudios";
            v = instructionsFont.MeasureString(msg) / new Vector2(2, 2);
            buffer = center - v;
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(buffer.X, 370), Color.White);
            
            msg = "'The Pain That Never Left' from FesliyanStudios";
            v = instructionsFont.MeasureString(msg) / new Vector2(2, 2);
            buffer = center - v;
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(buffer.X, 430), Color.White);

            msg = "Accidental Asteroid by Murphy Eliot";
            v = instructionsFont.MeasureString(msg) / new Vector2(2, 2);
            buffer = center - v;
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(buffer.X, 490), Color.White);

            msg = "Idea for Game Inspired by Asteroids by Atari";
            v = instructionsFont.MeasureString(msg) / new Vector2(2, 2);
            buffer = center - v;
            spriteBatch.DrawString(instructionsFont, msg, new Vector2(buffer.X, 550), Color.White);

            spriteBatch.End();
        }
    }
}
