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
        #region Variables

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont menuFont;

        #endregion


        HighScore highScore;

        #region Constructor

        public ReadyToPlayScreen()
        {
            LoadContent();
        }

        public ReadyToPlayScreen(HighScore highScoreObject)
        {
            LoadContent();
            highScore = highScoreObject;
        }

        #endregion

        #region Standard MonoGame Methods (LoadContent, Update, Draw)

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(StateManager.GraphicsDevice);
            //spriteFont = StateManager.Content.Load<SpriteFont>("font");
            //menuFont = StateManager.Content.Load<SpriteFont>("menu");


        }

        public override void Update(GameTime gameTime, StateManager screen, 
            GamePadState gamePadState, MouseState mouseState, 
            KeyboardState keyState, InputHandler input)
        {
            if (input.KeyboardState.WasKeyPressed(Keys.Space) ||
                input.WasPressed(0, InputHandler.ButtonType.A, Keys.A))
            {
                PlayScreen playScreen = new PlayScreen(highScore);
                screen.Pop();
                screen.Push(playScreen);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            spriteBatch.End();
        }

        #endregion
    }
}
