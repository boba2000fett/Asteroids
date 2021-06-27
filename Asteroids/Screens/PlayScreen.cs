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
        LinkedList<Rocks> tempActiveRocks = new LinkedList<Rocks>();

        HighScore highScore;

        TimeSpan elapsedTime;

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

            playerShip1 = new PlayerShip(StateManager.Game);
            playerShip1.Initialize();

            activeLasers = new LinkedList<Laser>();
            
            activeRocks = new LinkedList<Rocks>();
            
            Rocks rock1 = new Rocks(StateManager.Game, 600,50, (float)(Math.PI / 180), 1,3);
            rock1.Initialize();
            activeRocks.AddLast(rock1);

            Rocks rock2 = new Rocks(StateManager.Game, 300, 300, (float)(135 * Math.PI / 180), 1, 3);
            rock2.Initialize();
            activeRocks.AddLast(rock2);

            Rocks rock3 = new Rocks(StateManager.Game, 0, 0, (float)(90 * Math.PI / 180), 1, 3);
            rock3.Initialize();
            activeRocks.AddLast(rock3);

            Rocks rock4 = new Rocks(StateManager.Game, 400, 400, (float)(300 * Math.PI / 180), 1, 3);
            rock4.Initialize();
            activeRocks.AddLast(rock4);

            Rocks rock5 = new Rocks(StateManager.Game, 10, 600, (float)(200 * Math.PI / 180), 1, 3);
            rock5.Initialize();
            activeRocks.AddLast(rock5);



            activeEnemyShips = new LinkedList<EnemyShip>();
            EnemyShip enemyShip = new EnemyShip(StateManager.Game, 300, 300, (float)(135 * Math.PI / 180), 1, 3);
            enemyShip.Initialize();
            activeEnemyShips.AddLast(enemyShip);
        }

        public override void Update(GameTime gameTime, StateManager screen, 
            GamePadState gamePadState, MouseState mouseState, 
            KeyboardState keyState, InputHandler input)
        {

            /*
            Laser Collision: l1.StartX (238.5262) l1.StartY (283.2247) l1.EndX (238.5262) l1.EndY (303.2247)
            Rock Collision:  l2.StartX (300) l2.StartY (375) l2.EndX (300) l2.EndY (300)
             */
            //Line2D.Intersects(new Line2D(238.5262f, 283.2247f, 238.5262f, 303.2247f),
            //     new Line2D(300, 375, 300, 300));

            if (!playerShip1.gameEnded)
            {
                //Only Active During Play
                if (input.KeyboardState.WasKeyPressed(Keys.P))
                {
                    PauseScreen pause = new PauseScreen();
                    screen.Push(pause);
                }
                //Only Active During Play
                if (input.KeyboardState.WasKeyPressed(Keys.Escape))
                {
                    screen.Pop();
                    AttractScreen attract = new AttractScreen(highScore);
                    screen.Push(attract);
                }

                //Only Active During Play
                if (input.KeyboardState.WasKeyPressed(Keys.Enter))
                {
                    //Console.WriteLine($"playerShip1.rotation: {playerShip1.Rotation}");
                    if (activeLasers.Count() < 1)
                    {
                        //activeLasers.First.Value.Dispose();
                        //activeLasers.Remove(activeLasers.First);
                        laser = new Laser(StateManager.Game, playerShip1.position.X, playerShip1.position.Y, playerShip1.Rotation, true);
                        laser.Initialize();
                        activeLasers.AddLast(laser);
                    }
                }

                //Check Collision
                if (!playerShip1.crashed)
                {
                    elapsedTime += gameTime.ElapsedGameTime;

                    if (elapsedTime > TimeSpan.FromSeconds(.00000000001))
                    {
                        playerShip1List = playerShip1.ConvertLanderLine2D();
                        CheckCollisionRocks(playerShip1List);
                    }
                }
            }
            else //If the Game Has ended
            {
                /*if(player runs out of lives)
                    if(player qualifies for High Score)
                        Send them to High Score Screen
                    else
                        Send them to Attract Screen */
                if (highScore.CheckScore(playerShip1.score))
                {
                    highScore.score = playerShip1.score;
                    highScore.selection = true;
                    screen.Pop();
                    HighScoreScreen highScoreScreen = new HighScoreScreen(highScore);
                    screen.Push(highScoreScreen);
                }
                else
                {
                    screen.Pop();
                    AttractScreen attract = new AttractScreen(highScore);
                    screen.Push(attract);
                }

            }
                        
         
            playerShip1.Update(gameTime);
            if (activeLasers.Count > 0)
            {
                foreach (Laser l in activeLasers)
                {
                    l.Update(gameTime);
                    
                }
            }
            if (activeRocks.Count > 0)
            {
                foreach (Rocks r in activeRocks)
                {
                    r.Update(gameTime);                   
                }
            }
            if (activeEnemyShips.Count > 0)
            {
                foreach (EnemyShip e in activeEnemyShips)
                {
                    e.Update(gameTime);

                }
            }

            #region Determine if to add Rocks or EnemyShip
            if (activeRocks.Count < 1)
            {
                Random rand = new Random();

                int positionX = 0;
                int positionY = 0;
                float angle = 0;
                while (activeRocks.Count < 6)
                {
                    do
                    {
                        positionX = rand.Next(0, StateManager.GraphicsDevice.Viewport.Width);
                        positionY = rand.Next(0, StateManager.GraphicsDevice.Viewport.Height);
                    } while (
                    (Math.Abs(positionX - playerShip1.position.X) < 100) &&
                    (Math.Abs(positionY - playerShip1.position.Y) < 100));

                    angle = rand.Next(0, 361);

                    Rocks newRock = new Rocks(StateManager.Game, positionX, positionY, (float)(angle * Math.PI / 180), 1, 3);
                    newRock.Initialize();
                    activeRocks.AddLast(newRock);

                }
                //StateManager.GraphicsDevice.Viewport.Width;
            }



            #endregion

        }

        public override void Draw(GameTime gameTime)
        {
            //lander.Draw(gameTime);


            #region Updating each active Object

            playerShip1.Draw(gameTime);
            if (activeLasers.Count > 0)
            {
                foreach (Laser l in activeLasers)
                {
                    l.Draw(gameTime);

                }
            }
            if (activeRocks.Count > 0)
            {
                foreach (Rocks r in activeRocks)
                {
                    r.Draw(gameTime);                    
                }
            }
            if (activeEnemyShips.Count > 0)
            {
                foreach (EnemyShip e in activeEnemyShips)
                {
                    e.Draw(gameTime);

                }
            }
            //Iterate through Rocks Linked List
            //Iterate through EnemyShip LinkedList

            #endregion


        }

        public LinkedList<LinkedList<Line2D>> ConvertRocksLine2D(LinkedList<LinkedList<Vector2>> rocks)
        {
            // Use the activeRocks LinkedList<Rocks>. The end result is to return a LinkedList<LinkedList<Line2D>> of those objects
            LinkedList<LinkedList<Line2D>> totalLines = new LinkedList<LinkedList<Line2D>>();
            
            return totalLines;
        }

        public void CheckCollisionRocks(LinkedList<Line2D> land)
        {
            #region New Collision
            LinkedList<Line2D> rockCollision = new LinkedList<Line2D>();
            LinkedList<Line2D> enemyShipCollision = new LinkedList<Line2D>();
            LinkedList<Line2D> laserCollision = new LinkedList<Line2D>();

            //Check Collision between EnemyShip and Player
            foreach (EnemyShip e in activeEnemyShips)
            {
                //enemyShipCollision = e.ConvertEnemyShipLine2D();
                foreach (Line2D l1 in enemyShipCollision)
                {
                    foreach (Line2D l2 in playerShip1List)
                    {
                        if (Line2D.Intersects(l1, l2))
                        {
                            playerShip1.CrashShip();
                            return;
                        }
                    }
                }
            }

            foreach (Laser l in activeLasers)
            {
                if (l.CheckLaser() == true) //Make a check in Laser so that the laser can only run for 5 seconds.
                {
                    activeLasers.Remove(l);
                    l.Dispose();
                    return;
                    //remove l from activeLasers
                    //return/break; (Break from the loop)

                }

                laserCollision = l.ConvertLaserLine2D();

                foreach (EnemyShip e in activeEnemyShips)
                {
                    //enemyShipCollision = e.ConvertEnemyShipLine2D();
                    foreach (Line2D l1 in laserCollision)
                    {
                        foreach (Line2D l2 in enemyShipCollision)
                        {
                            if (Line2D.Intersects(l1, l2) && l.playerLaser)
                            {
                                //e.DestroyShip();
                                //Remove e from activeEnemyShips LinkedList
                                activeEnemyShips.Remove(e);
                                //return;
                            }
                        }
                    }
                }



                Rocks[] activeRocksArray = activeRocks.ToArray();


                for(int i = 0; i < activeRocksArray.Count(); i++)
                {
                    Random rand = new Random();
                    rockCollision = activeRocksArray[i].RocksSquareCollision();
                    
                    float angle1 = 0f;
                    float angle2 = 0f;
                    int location = 0;
                    int index = 1;

                    //Console.WriteLine($"Rock Rotation{activeRocksArray[i].rotation}");
                    
                    foreach (Line2D l1 in laserCollision)
                    {
                        foreach (Line2D l2 in rockCollision)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                            {
                                Console.WriteLine($"Laser Collision: l1.StartX ({l1.StartX}) l1.StartY ({l1.StartY}) l1.EndX ({l1.EndX}) l1.EndY ({l1.EndY})");
                                Console.WriteLine($"Rock Collision:  l2.StartX ({l2.StartX}) l2.StartY ({l2.StartY}) l2.EndX ({l2.EndX}) l2.EndY ({l2.EndY})");
                            }

                            if (Line2D.Intersects(l1, l2) && l.playerLaser)
                            {
                                
                                CheckAngle(activeRocksArray[i].rotation, out location, out angle1, out angle2);

                                playerShip1.AddScore(activeRocksArray[i].score);

                                if (activeRocksArray[i].size > 1)
                                {
                                    Console.WriteLine($"Before Collision Check: {activeRocks.Count}");

                                    Rocks newRock1 = new Rocks(angle2, activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, activeRocksArray[i].typeOfRock, StateManager.Game);
                                    newRock1.Initialize();
                                    activeRocks.AddLast(newRock1);
                                    newRock1.MAX_THRUST_POWER = 1f;
                                    Rocks newRock2 = new Rocks(angle1, activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, activeRocksArray[i].typeOfRock, StateManager.Game);
                                    newRock2.Initialize();
                                    activeRocks.AddLast(newRock2);
                                    newRock2.MAX_THRUST_POWER = 1f;
                                    activeRocks.Remove(activeRocksArray[i]);
                                    activeRocksArray[i].Dispose();                                    

                                    Console.WriteLine($"After Collision Check: {activeRocks.Count}");
                                }
                                else
                                {
                                    activeRocks.Remove(activeRocksArray[i]);
                                    activeRocksArray[i].Dispose();
                                    Console.WriteLine($"activeRocks Count{activeRocks.Count}");
                                }

                                activeLasers.Remove(l);
                                l.Dispose();
                            }
                        }
                    }


                }


                foreach (Line2D l1 in laserCollision)
                {
                    foreach (Line2D l2 in playerShip1List)
                    {
                        if (Line2D.Intersects(l1, l2) && !l.playerLaser)
                        {
                            playerShip1.CrashShip();
                        }
                    }
                }
                return;

                #endregion

            }

            foreach (Rocks r in activeRocks)
            {
                rockCollision = r.RocksSquareCollision();
                foreach (Line2D l1 in rockCollision)
                {
                    foreach (Line2D l2 in playerShip1List)
                    {
                        if (Line2D.Intersects(l1, l2))
                        {
                            
                            playerShip1.CrashShip();
                            return;
                        }
                    }
                }
            }
        }

        #endregion
        private void CheckAngle(float radianAngle, out int location, out float angle1, out float angle2)
        {
            float degreeAngle = radianAngle * (float)(Math.PI / 180);
            angle1 = 0;
            angle2 = 0;
            location = 0;
            int direction = 1;
            bool found = false;

            while(!found)
            {
                if (direction == 1)
                {
                    angle1 = degreeAngle - 45;
                    angle2 = degreeAngle + 45;
                }
                else if (direction == 2)
                {
                    angle1 = degreeAngle + 45;
                    angle2 = degreeAngle + 135;
                }
                else if (direction == 3)
                {
                    angle1 = degreeAngle + 135;
                    angle2 = degreeAngle - 135;
                }
                else if (direction == 4)
                {
                    angle1 = degreeAngle - 135;
                    angle2 = degreeAngle - 45;
                }

                if (angle1 > 360)
                    angle1 -= 360;
                if (angle1 < 0)
                    angle1 += 360;
                if (angle2 > 360)
                    angle2 -= 360;
                if (angle2 < 0)
                    angle2 += 360;
                if (degreeAngle > 360)
                    degreeAngle -= 360;
                if (degreeAngle < 0)
                    degreeAngle += 360;


                if (degreeAngle > angle1 || degreeAngle < angle2)
                {
                    found = true;
                }
                else if (degreeAngle >= angle1 && degreeAngle <= angle2)
                {
                    found = true;
                }
                direction++;

            }

            angle1 *= (float)(180 / Math.PI);
            angle2 *= (float)(180 / Math.PI);




            #region Planning
            /*            
             \ 1 / 
             4 + 2
             / 3 \

            if (location = 1) 
                angle1 = degreeAngle - 45;
                angle2 = degreeAngle + 45;
            if (location = 2) 
                angle1 = degreeAngle + 45;
                angle2 = degreeAngle + 135;
            if (location = 3) 
                angle1 = degreeAngle + 135;
                angle2 = degreeAngle - 135;
            if (location = 4) 
                angle1 = degreeAngle - 135;
                angle2 = degreeAngle - 45;


            if(angle1 > 360)
                angle1 -= 360
            if(angle1 < 0)
                angle1 += 360
            if(angle2 > 360)
                angle2 -= 360
            if(angle2 < 0)
                angle2 += 360
            if(degreeAngle > 360)
                degreeAngle -= 360
            if(degreeAngle < 0)
                degreeAngle += 360

            if(location == 1)
                if(degreeAngle > angle1 || degreeAngle < angle2 )
                    return 1;
                else 
                    return 0;
            if(location == 2)
                if(degreeAngle > angle1 && degreeAngle < angle2 )
                    return 2;
                else 
                    return 0;
            if(location == 3)
                if(degreeAngle > angle1 && degreeAngle < angle2 )
                    return 3;
                else 
                    return 0;
            if(location == 4)
                if(degreeAngle > angle1 && degreeAngle < angle2 )
                    return 4;
                else 
                    return 0;

            return 0;                    
             */
            #endregion

        }


        private void ActiveRocksRemove(Rocks r)
        {
            activeRocks.Remove(r);
        }
    }
}
