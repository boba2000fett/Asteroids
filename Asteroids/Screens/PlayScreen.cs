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

using MyLibrary;

namespace Asteroids.Screens
{
    public class PlayScreen : GameScreen
    {
        #region Variables

        SpriteBatch spriteBatch;
        int counter;
        bool landed;
        PlayerShip playerShip1;
        PlayerShip playerShip2;
        Laser laser;

        LinkedList<Laser> activeLasers;
        LinkedList<Rocks> activeRocks;
        LinkedList<EnemyShip> activeEnemyShips;

        LinkedList<Line2D> playerShip1List;
        LinkedList<Line2D> playerShip2List;

        LinkedList<LinkedList<Line2D>> activeLasersList;
        LinkedList<LinkedList<Line2D>> activeRocksList;
        LinkedList<LinkedList<Line2D>> activeEnemyShipsList;

        HighScore highScore;

        #endregion

        #region Constructor

        public PlayScreen()
        {
            LoadContent();
        }

        public PlayScreen(HighScore highScoreObject)
        {
            LoadContent();
            highScore = highScoreObject;
        }


        #endregion

        #region Standard MonoGame Methods (LoadContent, Update, Draw)


        public override void LoadContent()
        {
            //We are using StateManager to manage the States of the game, which operates like a stack
            spriteBatch = new SpriteBatch(StateManager.GraphicsDevice);

            //lander = new Lander(StateManager.Game);
            //terrain = new Terrain(StateManager.Game);
            playerShip1 = new PlayerShip(StateManager.Game);
            playerShip1.Initialize();

            activeLasers = new LinkedList<Laser>();

            //lander.Initialize();
            //terrain.Initialize();



        }

        public override void Update(GameTime gameTime, StateManager screen, 
            GamePadState gamePadState, MouseState mouseState, 
            KeyboardState keyState, InputHandler input)
        {           
            if (input.KeyboardState.WasKeyPressed(Keys.P))
            {                
                PauseScreen pause = new PauseScreen();
                screen.Push(pause);
            }
            if (input.KeyboardState.WasKeyPressed(Keys.Escape))
            {                
                screen.Pop();
                AttractScreen attract = new AttractScreen(highScore);
                screen.Push(attract);
            }

            if (input.KeyboardState.WasKeyPressed(Keys.Enter))
            {
                int YES = 69;

                Console.WriteLine($"playerShip1.rotation: {playerShip1.Rotation}");

                laser = new Laser(StateManager.Game, playerShip1.position.X, playerShip1.position.Y, playerShip1.Rotation, true);
                laser.Initialize();
                activeLasers.AddLast(laser);
            }

            /*
             * if(player runs out of lives)
             *      if(player qualifies for High Score)
             *          Send them to High Score Screen
             *      else
             *          Send them to Attract Screen
             * 
             */

            //lander.Update(gameTime);
            playerShip1.Update(gameTime);

            if (activeLasers.Count > 0)
            {
                foreach (Laser l in activeLasers)
                {
                    l.Update(gameTime);
                }
            }
            
        }

        public override void Draw(GameTime gameTime)
        {
            //lander.Draw(gameTime);


            playerShip1.Draw(gameTime);


            if (activeLasers.Count > 0)
            {
                foreach (Laser l in activeLasers)
                {
                    l.Draw(gameTime);
                }
            }
            
        }

        #endregion

    }
}
