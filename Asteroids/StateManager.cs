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

namespace Asteroids
{
    public class StateManager : DrawableGameComponent
    {
        // The Stack which contains the screens
        private Stack<Screens.GameScreen> screens;

        // Hold commonly used objects in a central location that is available
        // to all screens in the State Manager.
        private static ContentManager m_content;
        private static GraphicsDevice m_graphicsDevice;
        private static InputHandler m_input;
        private static Game m_game;

        public static Game Game
        {
            get { return m_game; }
            set { m_game = value; }
        }

        public static ContentManager Content
        {
            get { return m_content; }
            set { m_content = value; }
        }

        public static GraphicsDevice GraphicsDevice
        {
            get { return m_graphicsDevice; }
            set { m_graphicsDevice = value; }
        }

        public StateManager(Game game)
        : base(game)
        {
            screens = new Stack<Screens.GameScreen>();

            m_content = game.Content;
            m_graphicsDevice = game.GraphicsDevice;
            m_input = new InputHandler(game);
            m_game = game;
        }

        // insert a screen onto the stack so that it becomes
        // the screen that will be displayed.
        public void Push(Screens.GameScreen screen)
        {
            screens.Push(screen);
        }

        // remove the currently displayed screen
        public Screens.GameScreen Pop()
        {
            return screens.Pop();
        }

        protected override void LoadContent()
        {
            ContentManager content = Game.Content;
        }

        // return the top screen in the stack
        public Screens.GameScreen Top()
        {
            return screens.Peek();
        }

        public override void Update(GameTime gameTime)
        {
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();
            m_input.Update(gameTime);

            // update the current game state
            Top().Update(gameTime, this, gamePadState, mouseState, keyState, m_input);
        }

        public override void Draw(GameTime gameTime)
        {
            // draw the current game state
            Top().Draw(gameTime);
        }
    }
}
