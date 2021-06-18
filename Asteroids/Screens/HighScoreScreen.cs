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
    public class HighScoreScreen : GameScreen
    {
        #region Variables

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont menuFont;

        Texture2D menuImage;

        #endregion

        HighScore highScore;

        #region Constructor

        public HighScoreScreen()
        {
            LoadContent();
        }

        public HighScoreScreen(HighScore highScoreObject)
        {
            LoadContent();
            highScore = highScoreObject;
        }

        #endregion

        #region Standard MonoGame Methods (LoadContent, Update, Draw)

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(StateManager.GraphicsDevice);
            spriteFont = StateManager.Content.Load<SpriteFont>("font");
            //menuFont = StateManager.Content.Load<SpriteFont>("menu");

            //StateManager.Game.Window.Title = "Lunar Lander";

            //menuImage = StateManager.Content.Load<Texture2D>("ProgramBackground");

            //highScore.Initialize();
        }

        public override void Update(GameTime gameTime, StateManager screen, 
            GamePadState gamePadState, MouseState mouseState, 
            KeyboardState keyState, InputHandler input)
        {
            if (input.KeyboardState.WasKeyPressed(Keys.Escape))
            {
                screen.Pop();
            }

            highScore.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            highScore.Draw(gameTime);
            spriteBatch.Begin();

            //spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);

            Vector2 center = new Vector2(
                StateManager.GraphicsDevice.Viewport.Width / 2,
                StateManager.GraphicsDevice.Viewport.Height / 2);

            string msg = "Credits";

            Vector2 v = spriteFont.MeasureString(msg) / new Vector2(2,2);

            Vector2 buffer = center - v;
            
            spriteBatch.DrawString(spriteFont, msg, new Vector2(buffer.X, 150), Color.White);

            msg = "Programmed By:";
            v = spriteFont.MeasureString(msg) / new Vector2(2, 2);
            buffer = center - v;
            spriteBatch.DrawString(spriteFont, msg, new Vector2(buffer.X, 350), Color.White);

            msg = "Trenton Andrews";
            v = spriteFont.MeasureString(msg) / new Vector2(2, 2);
            buffer = center - v;
            spriteBatch.DrawString(spriteFont, msg, new Vector2(buffer.X, 500), Color.White);

            spriteBatch.End();
        }

        #endregion
    }
}
