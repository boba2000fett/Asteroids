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
            spriteFont = StateManager.Content.Load<SpriteFont>("font");
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
            //spriteBatch.Begin();
            //spriteBatch.DrawString(spriteFont, "Attract Screen", new Vector2(300f,500f), Color.White);
            //spriteBatch.End();



            spriteBatch.Begin();

            //spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);

            Vector2 center = new Vector2(
                StateManager.GraphicsDevice.Viewport.Width / 2,
                StateManager.GraphicsDevice.Viewport.Height / 2);

            string msg = "Attract Screen";

            Vector2 v = spriteFont.MeasureString(msg) / new Vector2(2, 2);

            Vector2 buffer = center - v;

            spriteBatch.DrawString(spriteFont, msg, new Vector2(buffer.X, 175), Color.White);

            //msg = "Play Game [P]";
            //v = menuFont.MeasureString(msg) / new Vector2(2, 2);
            //buffer = center - v;
            //spriteBatch.DrawString(menuFont, msg, new Vector2(buffer.X, 300), Color.White);

            //msg = "View Credits [C]";
            //v = menuFont.MeasureString(msg) / new Vector2(2, 2);
            //buffer = center - v;
            //spriteBatch.DrawString(menuFont, msg, new Vector2(buffer.X, 400), Color.White);

            //msg = "Exit Game [ESC]";
            //v = menuFont.MeasureString(msg) / new Vector2(2, 2);
            //buffer = center - v;
            //spriteBatch.DrawString(menuFont, msg, new Vector2(buffer.X, 500), Color.White);

            spriteBatch.End();


        }

        #endregion
    }
}
