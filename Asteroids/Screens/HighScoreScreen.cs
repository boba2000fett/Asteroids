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
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont menuFont;

        Texture2D menuImage;
        
        HighScore highScore;

        List<SoundEffect> soundEffects;

        TimeSpan elapsedTime;

        public HighScoreScreen()
        {
            LoadContent();
        }

        public HighScoreScreen(HighScore highScoreObject)
        {
            LoadContent();
            highScore = highScoreObject;
        }

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(StateManager.GraphicsDevice);
            spriteFont = StateManager.Content.Load<SpriteFont>("font");
            elapsedTime = TimeSpan.Zero;
        }

        public override void Update(GameTime gameTime, StateManager screen, 
            GamePadState gamePadState, MouseState mouseState, 
            KeyboardState keyState, InputHandler input)
        {
            if (input.KeyboardState.WasKeyPressed(Keys.Escape))
            {
                screen.Pop();
            }

            if(highScore.selection == false)
            {
                elapsedTime += gameTime.ElapsedGameTime;

                if (elapsedTime > TimeSpan.FromSeconds(3))
                {
                    screen.Pop();
                    AttractScreen attract = new AttractScreen(highScore);
                    screen.Push(attract);
                }                
            }
                       
            highScore.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            highScore.Draw(gameTime);
        }
    }
}
