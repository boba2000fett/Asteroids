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
    public class AttractScreen : GameScreen
    {
        #region Variables

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont menuFont;

        TimeSpan elapsedTime = TimeSpan.Zero;
        //Add in PlayerShip, Rocks, and Lasers and Stuff for Demo.

        HighScore highScore;

        #endregion

        #region Constructor

        public AttractScreen()
        {
            LoadContent();            
        }

        public AttractScreen(HighScore highScoreObject)
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

            StateManager.Game.Window.Title = "Asteroids";

        }

        public override void Update(GameTime gameTime, StateManager screen, 
            GamePadState gamePadState, MouseState mouseState, 
            KeyboardState keyState, InputHandler input)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (input.KeyboardState.WasKeyPressed(Keys.Space) ||
                input.WasPressed(0, InputHandler.ButtonType.A, Keys.A))
            {
                ReadyToPlayScreen readyScreen = new ReadyToPlayScreen(highScore);
                screen.Pop();
                screen.Push(readyScreen);
            }

            if (elapsedTime >= TimeSpan.FromSeconds(3))
            {
                // Switch to either High Score or Demo Screen. Maybe make a bool, and then it changes what is drawn in the Draw method.
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
