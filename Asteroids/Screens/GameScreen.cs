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
    public abstract class GameScreen
    {
        // Force all GameScreen derived components to call LoadContent() automatically
        // Why not make all screens drawable game components? This is a good question.
        // Component managers are typically game components and elements managed by a
        // component should only be managed by the manager
        //(i.e. not Updated() or Drawn()) automatically.
        public GameScreen()
        {

        }

        // Force all derived classes to implement these methods
        public abstract void LoadContent();

        public abstract void Update(GameTime gameTime, StateManager screen,
            GamePadState gamePadState, MouseState mouseState, 
            KeyboardState keyState, InputHandler input);

        public abstract void Draw(GameTime gameTime);
    }
}
