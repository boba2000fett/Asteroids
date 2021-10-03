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
    public class PauseScreen : GameScreen
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont menuFont;

        Texture2D backgroundImage;

        public PauseScreen()
        {
            LoadContent();            
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(StateManager.GraphicsDevice);
            spriteFont = StateManager.Content.Load<SpriteFont>("font");
            menuFont = StateManager.Content.Load<SpriteFont>("menu");

            backgroundImage = StateManager.Content.Load<Texture2D>("StarBackground");
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

            spriteBatch.Draw(backgroundImage, new Vector2(0, 0), Color.White);

            Vector2 center = new Vector2(
                StateManager.GraphicsDevice.Viewport.Width / 2,
                StateManager.GraphicsDevice.Viewport.Height / 2);

            string msg = "PAUSE";

            Vector2 v = spriteFont.MeasureString(msg) / new Vector2(2,2);

            spriteBatch.DrawString(spriteFont, msg, (center - v), Color.White);

            spriteBatch.End();
        }
    }
}
