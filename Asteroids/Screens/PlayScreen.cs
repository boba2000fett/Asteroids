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

            //lander = new Lander(StateManager.Game);
            //terrain = new Terrain(StateManager.Game);
            playerShip1 = new PlayerShip(StateManager.Game);
            playerShip1.Initialize();

            activeLasers = new LinkedList<Laser>();

            /*  Initialzize the activeRocks List
             *      Create 5 Rocks Objects around the Screen. Make sure to initialize them.
             * 
             * Initialzize the active EnemyShip List
             * 
             * 
             */

            
            activeRocks = new LinkedList<Rocks>();
            Rocks rock1 = new Rocks(StateManager.Game, 600,50, (float)(Math.PI / 180), 1,3);
            rock1.Initialize();
            activeRocks.AddLast(rock1);
            Rocks rock2 = new Rocks(StateManager.Game, 300, 300, (float)(135 * Math.PI / 180), 1, 3);
            rock2.Initialize();
            activeRocks.AddLast(rock2);

            activeEnemyShips = new LinkedList<EnemyShip>();

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
                //Console.WriteLine($"playerShip1.rotation: {playerShip1.Rotation}");
                if(activeLasers.Count() < 2)
                {
                    laser = new Laser(StateManager.Game, playerShip1.position.X, playerShip1.position.Y, playerShip1.Rotation, true);
                    laser.Initialize();
                    activeLasers.AddLast(laser);

                }

            }

            #region Checking Collision
            /* 
                
            */

            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(.00001))
            {
                playerShip1List = playerShip1.ConvertLanderLine2D();
                CheckCollisionRocks(playerShip1List);
            }



            #endregion

            /*
             * if(player runs out of lives)
             *      if(player qualifies for High Score)
             *          Send them to High Score Screen
             *      else
             *          Send them to Attract Screen
             * 
             */

            #region Updating each active Object

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
                    //e.Update(gameTime);

                }
            }
            //Iterate through Rocks Linked List
            //Iterate through EnemyShip LinkedList

            #endregion

            #region Determine if to add Rocks or EnemyShip
            /* 
                
            */
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
                    //e.Draw(gameTime);

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
            /*
              
             
               int count = 0;
               float var1 = 0.0f;
               float var2 = 0.0f;
               float var3 = 0.0f;
               float var4 = 0.0f;

              
               foreach (Rocks r in activeRocks)
                    totalLines.Add(r.ConvertRocksLine2D());//***********
                    
                    LinkedList<Line2D> lines = new LinkedList<Line2D>();

                    foreach (Vector2 l in r)
                    {
                        if (count == 0)
                        {
                            var1 = l.X;
                            var2 = l.Y;
                            count++;
                        }
                        else
                        {
                            var3 = l.X;
                            var4 = l.Y;
                            lines.AddLast(new Line2D(var1, var2, var3, var4));
                            count--;
                        }
                    }
                    return lines;
 
             */
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


                //while (tempActiveRocks.Count > 1)
                //{
                //    tempActiveRocks.RemoveLast();
                //}
                //foreach (Rocks rockStrive in activeRocks)
                //{
                //    tempActiveRocks.AddLast(rockStrive);
                //}

                Rocks[] activeRocksArray = activeRocks.ToArray();


                for(int i = 0; i < activeRocksArray.Count(); i++)
                {
                    Random rand = new Random();
                    rockCollision = activeRocksArray[i].RocksSquareCollision();
                    //int side1 = CheckAngle(activeRocksArray[i].rotation, 1);
                    //int side2 = CheckAngle(activeRocksArray[i].rotation, 2);
                    //int side3 = CheckAngle(activeRocksArray[i].rotation, 3);
                    //int side4 = CheckAngle(activeRocksArray[i].rotation, 4);

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
                                 

                                //Console.WriteLine($"Active Rock Location {location} Angle1 {angle1} Angle2 {angle2}"); 

                                if (activeRocksArray[i].size > 1)
                                {
                                    Console.WriteLine($"Before Collision Check: {activeRocks.Count}");


                                    //float rockRotation, int rockSize, float xPosition, float yPosition, int type, Game game  
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
                                    #region Old Collision
                                    //int type1 = rand.Next(1, 3);
                                    //int type2 = rand.Next(1, 3);
                                    //if (location == 1)
                                    //{

                                    //    //float rockRotation, int rockSize, float xPosition, float yPosition, int type, Game game  
                                    //    Rocks newRock1 = new Rocks(angle1, activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, activeRocksArray[i].typeOfRock, StateManager.Game);
                                    //    newRock1.Initialize();
                                    //    activeRocks.AddLast(newRock1);
                                    //    Rocks newRock2 = new Rocks(angle2, activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, activeRocksArray[i].typeOfRock, StateManager.Game);
                                    //    newRock2.Initialize();
                                    //    activeRocks.AddLast(newRock2);
                                    //    activeRocks.Remove(activeRocksArray[i]);


                                    //}
                                    //else if (location == 2)
                                    //{
                                    //    Rocks newRock1 = new Rocks((float)(activeRocksArray[i].rotation + (45 * (Math.PI) / 180)), activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, size1, StateManager.Game);
                                    //    newRock1.Initialize();
                                    //    activeRocks.AddLast(newRock1);
                                    //    Rocks newRock2 = new Rocks((float)(activeRocksArray[i].rotation + (135 * (Math.PI) / 180)), activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, size1, StateManager.Game);
                                    //    newRock2.Initialize();
                                    //    activeRocks.AddLast(newRock2);
                                    //    activeRocks.Remove(activeRocksArray[i]);
                                    //}
                                    //else if (location == 3)
                                    //{
                                    //    Rocks newRock1 = new Rocks((float)(activeRocksArray[i].rotation + (135 * (Math.PI) / 180)), activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, size1, StateManager.Game);
                                    //    newRock1.Initialize();
                                    //    activeRocks.AddLast(newRock1);
                                    //    Rocks newRock2 = new Rocks((float)(activeRocksArray[i].rotation - (135 * (Math.PI) / 180)), activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, size1, StateManager.Game);
                                    //    newRock2.Initialize();
                                    //    activeRocks.AddLast(newRock2);
                                    //    activeRocks.Remove(activeRocksArray[i]);
                                    //}
                                    //else if (location == 4)
                                    //{
                                    //    Rocks newRock1 = new Rocks((float)(activeRocksArray[i].rotation - (135 * (Math.PI) / 180)), activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, size1, StateManager.Game);
                                    //    newRock1.Initialize();
                                    //    activeRocks.AddLast(newRock1);
                                    //    Rocks newRock2 = new Rocks((float)(activeRocksArray[i].rotation - (45 * (Math.PI) / 180)), activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, size1, StateManager.Game);
                                    //    newRock2.Initialize();
                                    //    activeRocks.AddLast(newRock2);
                                    //    activeRocks.Remove(activeRocksArray[i]);
                                    //}
                                    #endregion


                                    Console.WriteLine($"After Collision Check: {activeRocks.Count}");
                                }
                                else
                                {
                                    activeRocks.Remove(activeRocksArray[i]);
                                    activeRocksArray[i].Dispose();
                                    //    Remove R component.
                                    //Remove R from tempActiveRocks.
                                }

                                activeLasers.Remove(l);
                                l.Dispose();
                            }
                        }
                    }


                }

                #region 
                ////////foreach (Rocks rock in activeRocks)
                ////////{
                ////////    Console.WriteLine($"Before {activeRocks.Count}");
                ////////    Random rand = new Random();
                ////////    rockCollision = rock.ConvertRocksLine2D();
                ////////    int side1 = CheckAngle(rock.rotation, 1);
                ////////    int side2 = CheckAngle(rock.rotation, 2);
                ////////    int side3 = CheckAngle(rock.rotation, 3);
                ////////    int side4 = CheckAngle(rock.rotation, 4);




                ////////    foreach (Line2D l1 in laserCollision)
                ////////    {
                ////////        foreach (Line2D l2 in rockCollision)
                ////////        {
                ////////            if (Line2D.Intersects(l1, l2) && l.playerLaser)
                ////////            {
                ////////                if (rock.size > 1)
                ////////                {

                ////////                    int size1 = rand.Next(1, 3);
                ////////                    int size2 = rand.Next(1, 3);
                ////////                    if (side1 == 1)
                ////////                    {
                ////////                        Console.WriteLine($"During 1 {activeRocks.Count}");

                ////////                        //float rockRotation, int rockSize, float xPosition, float yPosition, int type, Game game  
                ////////                        Rocks newRock1 = new Rocks((float)(rock.rotation - (45 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                ////////                        newRock1.Initialize();
                ////////                        tempActiveRocks.AddLast(newRock1);
                ////////                        Rocks newRock2 = new Rocks((float)(rock.rotation + (45 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                ////////                        newRock2.Initialize();
                ////////                        tempActiveRocks.AddLast(newRock2);
                ////////                        tempActiveRocks.Remove(rock);

                ////////                        Console.WriteLine($"During 2 {activeRocks.Count}");
                ////////                    }
                ////////                    else if (side2 == 2)
                ////////                    {
                ////////                        Rocks newRock1 = new Rocks((float)(rock.rotation + (45 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                ////////                        newRock1.Initialize();
                ////////                        tempActiveRocks.AddLast(newRock1);
                ////////                        Rocks newRock2 = new Rocks((float)(rock.rotation + (135 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                ////////                        newRock2.Initialize();
                ////////                        tempActiveRocks.AddLast(newRock2);
                ////////                        tempActiveRocks.Remove(rock);
                ////////                    }
                ////////                    else if (side3 == 3)
                ////////                    {
                ////////                        Rocks newRock1 = new Rocks((float)(rock.rotation + (135 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                ////////                        newRock1.Initialize();
                ////////                        tempActiveRocks.AddLast(newRock1);
                ////////                        Rocks newRock2 = new Rocks((float)(rock.rotation - (135 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                ////////                        newRock2.Initialize();
                ////////                        tempActiveRocks.AddLast(newRock2);
                ////////                        tempActiveRocks.Remove(rock);
                ////////                    }
                ////////                    else if (side4 == 4)
                ////////                    {
                ////////                        Rocks newRock1 = new Rocks((float)(rock.rotation - (135 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                ////////                        newRock1.Initialize();
                ////////                        tempActiveRocks.AddLast(newRock1);
                ////////                        Rocks newRock2 = new Rocks((float)(rock.rotation - (45 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                ////////                        newRock2.Initialize();
                ////////                        tempActiveRocks.AddLast(newRock2);
                ////////                        tempActiveRocks.Remove(rock);
                ////////                    }
                ////////                    else
                ////////                    {
                ////////                        tempActiveRocks.Remove(rock);
                ////////                        //    Remove R component.
                ////////                        //Remove R from tempActiveRocks.
                ////////                    }
                ////////                    //return;
                ////////                }
                ////////            }
                ////////        }
                ////////    }

                ////////    Console.WriteLine($"After {activeRocks.Count}");
                ////////}


                //while (activeRocks.Count > 1)
                //{
                //    activeRocks.RemoveLast();
                //}
                //foreach (Rocks rockStrive in tempActiveRocks)
                //{
                //    activeRocks.AddLast(rockStrive);
                //}
                #endregion

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

                #region Old Collision

                //    //Check Collision between Rocks and PlayerShip
                //    LinkedList<Line2D> rockCollision = new LinkedList<Line2D>();
                //foreach (Rocks r in activeRocks)
                //{ 
                //    rockCollision = r.ConvertRocksLine2D();
                //    foreach (Line2D l1 in rockCollision)
                //    {
                //        foreach (Line2D l2 in playerShip1List)
                //        {
                //            if (Line2D.Intersects(l1, l2))
                //            {
                //                playerShip1.CrashShip();
                //                return;
                //            }
                //        }
                //    }
                //}

                ////Check Collision between EnemyShip and Player
                //LinkedList<Line2D> enemyShipCollision = new LinkedList<Line2D>();
                //foreach (EnemyShip e in activeEnemyShips)
                //{
                //    //enemyShipCollision = e.ConvertEnemyShipLine2D();
                //    foreach (Line2D l1 in enemyShipCollision)
                //    {
                //        foreach (Line2D l2 in playerShip1List)
                //        {
                //            if (Line2D.Intersects(l1, l2))
                //            {
                //                playerShip1.CrashShip();
                //                return;
                //            }
                //        }
                //    }
                //}


                //LinkedList<Line2D> laserCollision = new LinkedList<Line2D>();
                //foreach (Laser l in activeLasers)
                //{
                //    if (l.CheckLaser() == true) //Make a check in Laser so that the laser can only run for 5 seconds.
                //    {
                //        activeLasers.Remove(l);
                //        return;
                //        //remove l from activeLasers
                //        //return/break; (Break from the loop)

                //    }
                //    laserCollision = l.ConvertLaserLine2D();

                //    foreach (EnemyShip e in activeEnemyShips)
                //    {
                //        //enemyShipCollision = e.ConvertEnemyShipLine2D();
                //        foreach (Line2D l1 in laserCollision)
                //        {
                //            foreach (Line2D l2 in enemyShipCollision)
                //            {
                //                if (Line2D.Intersects(l1, l2) && l.playerLaser)
                //                {
                //                    //e.DestroyShip();
                //                    //Remove e from activeEnemyShips LinkedList
                //                    activeEnemyShips.Remove(e);
                //                             //return;
                //                }
                //            }
                //        }
                //    }

                //    foreach (Rocks rock in activeRocks)
                //    {
                //        Random rand = new Random();
                //        rockCollision = rock.ConvertRocksLine2D();
                //        int side1 = CheckAngle(rock.rotation, 1);
                //        int side2 = CheckAngle(rock.rotation, 2);
                //        int side3 = CheckAngle(rock.rotation, 3);
                //        int side4 = CheckAngle(rock.rotation, 4);
                //        Rocks rockTemp;

                //        foreach (Line2D l1 in laserCollision)
                //        {
                //            foreach (Line2D l2 in rockCollision)
                //            {
                //                if (Line2D.Intersects(l1, l2) && l.playerLaser)
                //                {
                //                    if (rock.size > 1)
                //                    {
                //                        rockTemp = rock;
                //                        int size1 = rand.Next(1, 3);
                //                        int size2 = rand.Next(1, 3);
                //                        if (side1 == 1)
                //                        {

                //                            //float rockRotation, int rockSize, float xPosition, float yPosition, int type, Game game  
                //                            Rocks newRock1 = new Rocks((float)(rock.rotation - (45 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                //                            newRock1.Initialize();
                //                            activeRocks.AddLast(newRock1);
                //                            Rocks newRock2 = new Rocks((float)(rock.rotation + (45 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                //                            newRock2.Initialize();
                //                            activeRocks.AddLast(newRock2);
                //                            activeRocks.Remove(rock);
                //                        }
                //                        else if (side2 == 2)
                //                        {
                //                            Rocks newRock1 = new Rocks((float)(rock.rotation + (45 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                //                            newRock1.Initialize();
                //                            activeRocks.AddLast(newRock1);
                //                            Rocks newRock2 = new Rocks((float)(rock.rotation + (135 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                //                            newRock2.Initialize();
                //                            activeRocks.AddLast(newRock2);
                //                            activeRocks.Remove(rock);
                //                        }
                //                        else if (side3 == 3)
                //                        {
                //                            Rocks newRock1 = new Rocks((float)(rock.rotation + (135 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                //                            newRock1.Initialize();
                //                            activeRocks.AddLast(newRock1);
                //                            Rocks newRock2 = new Rocks((float)(rock.rotation - (135 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                //                            newRock2.Initialize();
                //                            activeRocks.AddLast(newRock2);
                //                            activeRocks.Remove(rock);
                //                        }
                //                        else if (side4 == 4)
                //                        {
                //                            Rocks newRock1 = new Rocks((float)(rock.rotation - (135 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                //                            newRock1.Initialize();
                //                            activeRocks.AddLast(newRock1);
                //                            Rocks newRock2 = new Rocks((float)(rock.rotation - (45 * (Math.PI) / 180)), rock.size - 1, rock.position.X, rock.position.Y, size1, StateManager.Game);
                //                            newRock2.Initialize();
                //                            activeRocks.AddLast(newRock2);
                //                            activeRocks.Remove(rock);
                //                        }
                //                        else
                //                        {
                //                            activeRocks.Remove(rock);
                //                            //    Remove R component.
                //                            //Remove R from activeRocks.
                //                        }
                //                        //return;
                //                    }
                //                }
                //            }
                //        }
                //    }

                //    foreach (Line2D l1 in laserCollision)
                //    {
                //        foreach (Line2D l2 in playerShip1List)
                //        {
                //            if (Line2D.Intersects(l1, l2) && !l.playerLaser)
                //            {
                //                playerShip1.CrashShip();
                //            }
                //        }
                //    }
                //    return;
                #endregion
                #region Planning 
                /*
                LinkedList<Line2D> rockCollision            
                foreach (Rocks r in activeRocks)
                    rockCollision = r.ConvertRocksLine2D();
                    foreach (Line2D l1 in rockCollision)
                        foreach (Line2D l2 in playerShip1List)                
                            if (Line2D.Intersects(l1, l2))                                        
                                playerShip1.CrashShip();
                                return;


                LinkedList<Line2D> enemyShipCollision            
                foreach (EnemyShip e in activeEnemyShips)
                    enemyShipCollision = e.ConvertEnemyShipLine2D();
                    foreach (Line2D l1 in enemyShipCollision
                        foreach (Line2D l2 in playerShip1List)                
                            if (Line2D.Intersects(l1, l2))                                        
                                playerShip1.CrashShip();
                                return;


                LinkedList<Line2D> laserCollision            
                foreach (Laser l in activeLasers)
                {
                    if (l.CheckLaser() == true) //Make a check in Laser so that the laser can only run for 5 seconds.
                        remove l from activeLasers
                        return/break; (Break from the loop)

                    laserCollision = l.ConvertLaserLine2D();

                    foreach (EnemyShip e in activeEnemyShips)
                        enemyShipCollision = e.ConvertEnemyShipLine2D();
                            foreach (Line2D l1 in laserCollision)
                                foreach (Line2D l2 in enemyShipCollision)                
                                    if (Line2D.Intersects(l1, l2)  && laser.playerLaser)                                        
                                        e.DestroyShip();
                                        Remove e from activeEnemyShips LinkedList
                                        //return;


                    foreach (Rocks r in activeRocks)
                        rockCollision = r.ConvertRocksLine2D();
                        int side1 = CheckCollision(r.rotation, 1);
                        int side2 = CheckCollision(r.rotation, 2);
                        int side3 = CheckCollision(r.rotation, 3);
                        int side4 = CheckCollision(r.rotation, 4);

                        foreach (Line2D l1 in rockCollision
                            foreach (Line2D l2 in playerShip1List)                
                                if (Line2D.Intersects(l1, l2) && laser.playerLaser)
                                    if(r.Size > 1)
                                        if(side1 == 1)
                                            Rocks newRock1 = new Rock(r.rotation - (45*(Math.PI)/180)) radians, r.Size - 1, r.position.X, r.position.Y)
                                            newRock1.Initialize();                            
                                            activeRocks.AddLast(newRock1);
                                            Rocks newRock2 = new Rock(r.rotation + (45*(Math.PI)/180)) radians, r.Size - 1, r.position.X, r.position.Y)
                                            newRock2.Initialize();                            
                                            activeRocks.AddLast(newRock2);
                                        else if(side2 == 2)
                                            Rocks newRock1 = new Rock(r.rotation + (45*(Math.PI)/180)) radians, r.Size - 1, r.position.X, r.position.Y)
                                            newRock1.Initialize();                            
                                            activeRocks.AddLast(newRock1);                            
                                            Rocks newRock2 = new Rock(r.rotation + (135*(Math.PI)/180)) radians, r.Size - 1, r.position.X, r.position.Y)
                                            newRock2.Initialize();                            
                                            activeRocks.AddLast(newRock2);
                                        else if(side3 == 3)                                        
                                            Rocks newRock1 = new Rock(r.rotation + (135*(Math.PI)/180)) radians, r.Size - 1, r.position.X, r.position.Y)
                                            newRock1.Initialize();                            
                                            activeRocks.AddLast(newRock1);                            
                                            Rocks newRock2 = new Rock(r.rotation - (135*(Math.PI)/180)) radians, r.Size - 1, r.position.X, r.position.Y)
                                            newRock2.Initialize();                            
                                            activeRocks.AddLast(newRock2);
                                        else if(side4 == 4)
                                            Rocks newRock1 = new Rock(r.rotation - (135 *(Math.PI)/180)) radians, r.Size - 1, r.position.X, r.position.Y)
                                            newRock1.Initialize();                            
                                            activeRocks.AddLast(newRock1);                            
                                            Rocks newRock2 = new Rock(r.rotation - (45*(Math.PI)/180)) radians, r.Size - 1, r.position.X, r.position.Y)
                                            newRock2.Initialize();                            
                                            activeRocks.AddLast(newRock2);
                                    else
                                        Remove R component. 
                                        Remove R from activeRocks.

                                    //return;


                    foreach (Line2D l1 in laserCollision)
                          foreach (Line2D l2 in playerShip1List)                
                            if (Line2D.Intersects(l1, l2) && !laser.playerLaser) 
                                playerShip1.CrashShip();
                                return;
                }


                */
                #endregion

            }
            //Check Collision between Rocks and PlayerShip
            

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


            //if (direction == 1)
            //{
            //    angle1 = degreeAngle - 45;
            //    angle2 = degreeAngle + 45;
            //}
            //else if (direction == 2)
            //{
            //    angle1 = degreeAngle + 45;
            //    angle2 = degreeAngle + 135;
            //}
            //else if (direction == 3) 
            //{
            //    angle1 = degreeAngle + 135;
            //    angle2 = degreeAngle - 135;
            //}
            //else if (direction == 4)
            //{
            //    angle1 = degreeAngle - 135;
            //    angle2 = degreeAngle - 45;
            //}

            //if (angle1 > 360)
            //    angle1 -= 360;
            //if (angle1 < 0)
            //    angle1 += 360;
            //if (angle2 > 360)
            //    angle2 -= 360;
            //if (angle2 < 0)
            //    angle2 += 360;
            //if (degreeAngle > 360)
            //    degreeAngle -= 360;
            //if (degreeAngle < 0)
            //    degreeAngle += 360;

            //if (location == 1)
            //{
            //    if (degreeAngle > angle1 || degreeAngle < angle2)
            //        return 1;
            //    else
            //        return 0;
            //}
            //else if (location == 2)
            //{
            //    if (degreeAngle > angle1 && degreeAngle < angle2)
            //        return 2;
            //    else
            //        return 0;
            //}
            //else if (location == 3)
            //{
            //    if (degreeAngle > angle1 && degreeAngle < angle2)
            //        return 3;
            //    else
            //        return 0;
            //}
            //else if (location == 4)
            //{
            //    if (degreeAngle > angle1 && degreeAngle < angle2)
            //        return 4;
            //    else
            //        return 0;
            //}
            //else
            //{
            //    return 0;
            //}




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
