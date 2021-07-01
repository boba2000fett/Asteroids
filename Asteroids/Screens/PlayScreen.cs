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

        EnemyShip enemyShip;

        //LinkedList<Laser> activeLasers;

        LinkedList<Laser> activePlayerLasers;
        LinkedList<Laser> activeEnemyLasers;

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
        TimeSpan enemyShipTime;
        TimeSpan enemyShipFireTime;

        int randomEnemyShipSpawnTime;
        int randomEnemyShipFireTime;

        bool testHit = false;

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

            //activeLasers = new LinkedList<Laser>();

            activePlayerLasers = new LinkedList<Laser>();
            activeEnemyLasers = new LinkedList<Laser>();

            activeRocks = new LinkedList<Rocks>();

            Rocks rock1 = new Rocks(StateManager.Game, 600, 50, (float)(Math.PI / 180), 1, 3);
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

            RandomEnemyShipSpawnVariables(out int test, out int test2);
            RandomEnemyShipFireVariables();

            enemyShipTime = TimeSpan.Zero;
            enemyShipFireTime = TimeSpan.Zero;

        }

        public override void Update(GameTime gameTime, StateManager screen,
            GamePadState gamePadState, MouseState mouseState,
            KeyboardState keyState, InputHandler input)
        {

            /*
            Laser Collision: l1.StartX (238.5262) l1.StartY (283.2247) l1.EndX (238.5262) l1.EndY (303.2247)
            Rock Collision:  l2.StartX (300) l2.StartY (375) l2.EndX (300) l2.EndY (300)
             */
            //(322.6734) l1.StartY (329.3851) l1.EndX (303.6414) l1.EndY (323.2384)
            //Console.WriteLine(Line2D.Intersects(new Line2D(322.6734f, 329.3851f, 303.6414f, 323.2384f),
            //     new Line2D(272, 304, 296, 304)));

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


                //Check Collision
                if (!playerShip1.crashed)
                {
                    //Only Active During Play
                    if (input.KeyboardState.WasKeyPressed(Keys.Enter))
                    {
                        //Console.WriteLine($"playerShip1.rotation: {playerShip1.Rotation}");
                        if (activePlayerLasers.Count() < 1)
                        {
                            //activeLasers.First.Value.Dispose();
                            //activeLasers.Remove(activeLasers.First);
                            laser = new Laser(StateManager.Game, playerShip1.position.X, playerShip1.position.Y, playerShip1.Rotation, true);
                            laser.Initialize();
                            activePlayerLasers.AddLast(laser);
                        }
                    }

                    elapsedTime += gameTime.ElapsedGameTime;

                    if (elapsedTime > TimeSpan.FromSeconds(.01))
                    {
                        playerShip1List = playerShip1.ConvertLanderLine2D();
                        CheckCollisionRocks(playerShip1List);
                        elapsedTime = TimeSpan.Zero;
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

            //Updating Objects
            playerShip1.Update(gameTime);

            //if (activePlayerLasers.Count > 0)
            //{
            //    activePlayerLasers.First.Value.Update(gameTime);
            //}

            foreach (Laser l in activePlayerLasers)
            {
                l.Update(gameTime);
            }

            if (activeEnemyLasers.Count > 0)
            {
                activeEnemyLasers.First.Value.Update(gameTime);
            }
            if (activeEnemyShips.Count > 0)
            {
                activeEnemyShips.First.Value.Update(gameTime);
            }


            if (activeRocks.Count > 0)
            {
                foreach (Rocks r in activeRocks)
                {
                    r.Update(gameTime);
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



            //If there are no ships currently active
            if (activeEnemyShips.Count < 1)
            {
                if (enemyShipTime > TimeSpan.FromSeconds(randomEnemyShipSpawnTime))
                {
                    //if (testHit)
                    //{
                        #region Instantiate New Enemy Ship Object
                        int xPos;
                        int yPos;
                        //Randomly generate which side it starts(X), and then which vertical position it starts at (Y)
                        RandomEnemyShipSpawnVariables(out xPos, out yPos);

                        EnemyShip enemyShip = new EnemyShip(StateManager.Game, 0, yPos, (float)(90 * Math.PI / 180), 1, 3);
                        enemyShip.Initialize();
                        activeEnemyShips.AddLast(enemyShip);

                        enemyShipTime = TimeSpan.Zero;

                        if (xPos == 1)
                        {
                            enemyShip.MAX_THRUST_POWER *= -1;
                        }
                        #endregion

                    //}
                    //else
                    //{
                    //    #region Testing Collision
                    //    EnemyShip enemyShip = new EnemyShip(StateManager.Game, 300, 300, (float)(90 * Math.PI / 180), 1, 3);
                    //    enemyShip.Initialize();
                    //    enemyShip.MAX_THRUST_POWER = 0;
                    //    activeEnemyShips.AddLast(enemyShip);
                    //    #endregion
                    //}



                }

                enemyShipTime += gameTime.ElapsedGameTime;
            }
            //If there are ships currently active
            else
            {
                //Add Fire Logic Here.
                /*
                If above enemyShipFireTime;
                    Regenerate enemyShipFireTime;
                    Angle Towards PlayerShip
                    int x1 = activeEnemyShips.First.Value.X;
                    int y1 = activeEnemyShips.First.Value.Y;
                    int x2 = playerShip.X;
                    int y2 = playerShip.Y;

                    m = (y2 - y1)/(x2 - x1);
                    theta = inverseTan(m);

                    Fire Laser.
                    laser = new Laser(StateManager.Game, activeEnemyShips.First.Value.position.X, activeEnemyShips.First.Value.position.Y, theta, false);
                    laser.Initialize();
                    activeLasers.AddLast(laser);
                    enemyShipTime = TimeSpan.Zero;
                    else
                    add to enemyShipFireTime;

                    */
                if (enemyShipFireTime > TimeSpan.FromSeconds(randomEnemyShipFireTime))
                {
                    //Regenerate randomEnemyShipFireTime
                    RandomEnemyShipFireVariables();
                    //Angle Towards Ship
                    float x1 = activeEnemyShips.First.Value.position.X;
                    float y1 = activeEnemyShips.First.Value.position.Y;
                    float x2 = playerShip1.position.X;
                    float y2 = playerShip1.position.Y;
                    double m = (y2 - y1) / (x2 - x1);
                    double theta = Math.Atan(m);
                    theta -= (Math.PI / 2);
                    //Fire Laser
                    
                    laser = new Laser(StateManager.Game, activeEnemyShips.First.Value.position.X, activeEnemyShips.First.Value.position.Y, (float)theta, false);
                    //laser = new Laser(StateManager.Game, activeEnemyShips.First.Value.position.X, activeEnemyShips.First.Value.position.Y, (float)(Math.PI / 2), false);
                    
                    laser.Initialize();
                    activeEnemyLasers.AddLast(laser);
                    //Reset enemyShipFireTime
                    enemyShipFireTime = TimeSpan.Zero;
                }
                else
                {
                    enemyShipFireTime += gameTime.ElapsedGameTime;
                }
            }

            #endregion

        }

        public override void Draw(GameTime gameTime)
        {
            //lander.Draw(gameTime);



            playerShip1.Draw(gameTime);
            //if (activePlayerLasers.Count > 0)
            //{
            //    activePlayerLasers.First.Value.Draw(gameTime);
            //}
            foreach (Laser l in activePlayerLasers)
            {
                l.Draw(gameTime);
            }

            if (activeEnemyLasers.Count > 0)
            {
                activeEnemyLasers.First.Value.Draw(gameTime);
            }
            if (activeEnemyShips.Count > 0)
            {
                activeEnemyShips.First.Value.Draw(gameTime);
            }

            if (activeRocks.Count > 0)
            {
                foreach (Rocks r in activeRocks)
                {
                    r.Draw(gameTime);
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
            LinkedList<Line2D> rockCollision = new LinkedList<Line2D>();
            LinkedList<Line2D> enemyShipCollision = new LinkedList<Line2D>();
            LinkedList<Line2D> laserCollision = new LinkedList<Line2D>();


            #region Player Lasers Collision
            //if (activePlayerLasers.Count > 0)
            //{
            //    if (activePlayerLasers.First.Value.CheckLaser() == true) //Make a check in Laser so that the laser can only run for 5 seconds.
            //    {
            //        activePlayerLasers.First.Value.Dispose();
            //        activePlayerLasers.Remove(activePlayerLasers.First.Value);
            //    }
            //}

         
            Laser[] activePlayerLasersArray = activePlayerLasers.ToArray();
            for (int i = 0; i < activePlayerLasersArray.Count(); i++)
            {
                if (activePlayerLasersArray[i].CheckLaser() == true) //Make a check in Laser so that the laser can only run for 5 seconds.
                {
                    
                    activePlayerLasers.Remove(activePlayerLasersArray[i]);
                    activePlayerLasersArray[i].Dispose();
                }
            }


                #region NEW Collision Between Player Laser and EnemyShip
                #region Multiple Lasers
                foreach (Laser l in activePlayerLasers)
            {
                float[] xCoordinates = new float[8];
                float[] yCoordinates = new float[8];
                laserCollision = l.ConvertLaserLine2D();

                //activePlayerLaser
                if (activeEnemyShips.Count > 0)
                {
                    enemyShipCollision = activeEnemyShips.First.Value.ConvertEnemyShipLine2D();
                    foreach (Line2D l1 in laserCollision)
                    {
                        activeEnemyShips.First.Value.GetRectanglePositions(out xCoordinates, out yCoordinates);

                        if (
                            (GFG.check(xCoordinates[0], yCoordinates[0], xCoordinates[1], yCoordinates[1], xCoordinates[2], yCoordinates[2], xCoordinates[3], yCoordinates[3], l1.StartX, l1.StartY)) ||
                            (GFG.check(xCoordinates[0], yCoordinates[0], xCoordinates[1], yCoordinates[1], xCoordinates[2], yCoordinates[2], xCoordinates[3], yCoordinates[3], l1.EndX, l1.EndX)) ||
                            (GFG.check(xCoordinates[4], yCoordinates[4], xCoordinates[5], yCoordinates[5], xCoordinates[6], yCoordinates[6], xCoordinates[7], yCoordinates[7], l1.StartX, l1.StartY)) ||
                            (GFG.check(xCoordinates[4], yCoordinates[4], xCoordinates[5], yCoordinates[5], xCoordinates[6], yCoordinates[6], xCoordinates[7], yCoordinates[7], l1.EndX, l1.EndY)
                            ))
                        {
                            //e.DestroyShip();
                            //Remove e from activeEnemyShips LinkedList
                            activeEnemyShips.First.Value.Dispose();
                            activeEnemyShips.Remove(activeEnemyShips.First.Value);
                            //testHit = true;
                            return;
                        }
                    }
                }
            }
            #endregion
            #region Old with Only 1 PlayerLaser
            //if (activePlayerLasers.Count > 0)
            //{
            //    float[] xCoordinates = new float[8];
            //    float[] yCoordinates = new float[8];
            //    laserCollision = activePlayerLasers.First.Value.ConvertLaserLine2D();

            //    //activePlayerLaser
            //    if (activeEnemyShips.Count > 0)
            //    {
            //        enemyShipCollision = activeEnemyShips.First.Value.ConvertEnemyShipLine2D();
            //        foreach (Line2D l1 in laserCollision)
            //        {                        
            //            activeEnemyShips.First.Value.GetRectanglePositions(out xCoordinates, out yCoordinates);

            //            if(
            //                (GFG.check(xCoordinates[0], yCoordinates[0], xCoordinates[1], yCoordinates[1], xCoordinates[2], yCoordinates[2], xCoordinates[3], yCoordinates[3], l1.StartX, l1.StartY))||
            //                (GFG.check(xCoordinates[0], yCoordinates[0], xCoordinates[1], yCoordinates[1], xCoordinates[2], yCoordinates[2], xCoordinates[3], yCoordinates[3], l1.EndX, l1.EndX)) ||
            //                (GFG.check(xCoordinates[4], yCoordinates[4], xCoordinates[5], yCoordinates[5], xCoordinates[6], yCoordinates[6], xCoordinates[7], yCoordinates[7], l1.StartX, l1.StartY)) ||
            //                (GFG.check(xCoordinates[4], yCoordinates[4], xCoordinates[5], yCoordinates[5], xCoordinates[6], yCoordinates[6], xCoordinates[7], yCoordinates[7], l1.EndX, l1.EndY)
            //                ))
            //            {                            
            //                //e.DestroyShip();
            //                //Remove e from activeEnemyShips LinkedList
            //                activeEnemyShips.First.Value.Dispose();
            //                activeEnemyShips.Remove(activeEnemyShips.First.Value);
            //                //testHit = true;
            //                return;
            //            }
            //        }
            //    }
            //}
            #endregion

            #endregion



            #region NEW Collision Between Rock and Player Laser
            #region New Version with Multiple Player Lasers
            foreach (Laser l in activePlayerLasers)
            {
                laserCollision = l.ConvertLaserLine2D();

                //This is on the activePlayerLaser
                //Rocks and Laser Collision
                Rocks[] activeRocksArray = activeRocks.ToArray();
                for (int i = 0; i < activeRocksArray.Count(); i++)
                {
                    float[] xCoordinates = new float[4];
                    float[] yCoordinates = new float[4];

                    Random rand = new Random();
                    rockCollision = activeRocksArray[i].RocksSquareCollision();

                    float angle1 = 0f;
                    float angle2 = 0f;
                    int location = 0;
                    int index = 1;

                    //Console.WriteLine($"Rock Rotation{activeRocksArray[i].rotation}");

                    foreach (Line2D l1 in laserCollision)
                    {
                        activeRocksArray[i].GetRectanglePositions(out xCoordinates, out yCoordinates);
                        if (
                            (GFG.PointInRectangle(new Vector2(xCoordinates[0], yCoordinates[0]), new Vector2(xCoordinates[1], yCoordinates[1]), new Vector2(xCoordinates[2], yCoordinates[2]), new Vector2(xCoordinates[3], yCoordinates[3]), new Vector2(l1.StartX, l1.StartY))) ||
                            (GFG.PointInRectangle(new Vector2(xCoordinates[0], yCoordinates[0]), new Vector2(xCoordinates[1], yCoordinates[1]), new Vector2(xCoordinates[2], yCoordinates[2]), new Vector2(xCoordinates[3], yCoordinates[3]), new Vector2(l1.EndX, l1.EndY)))
                            )
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

                            l.Dispose();
                            activePlayerLasers.Remove(l);
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Failed");
                        }

                    }
                }
            }
            #endregion
            #region OLD version with Only 1 Player Laser
            //if (activePlayerLasers.Count > 0)
            //{
            //    laserCollision = activePlayerLasers.First.Value.ConvertLaserLine2D();

            //    //This is on the activePlayerLaser
            //    //Rocks and Laser Collision
            //    Rocks[] activeRocksArray = activeRocks.ToArray();
            //    for (int i = 0; i < activeRocksArray.Count(); i++)
            //    {
            //        float[] xCoordinates = new float[4];
            //        float[] yCoordinates = new float[4];

            //        Random rand = new Random();
            //        rockCollision = activeRocksArray[i].RocksSquareCollision();

            //        float angle1 = 0f;
            //        float angle2 = 0f;
            //        int location = 0;
            //        int index = 1;

            //        //Console.WriteLine($"Rock Rotation{activeRocksArray[i].rotation}");

            //        foreach (Line2D l1 in laserCollision)
            //        {
            //            activeRocksArray[i].GetRectanglePositions(out xCoordinates, out yCoordinates);
            //            if (
            //                (GFG.PointInRectangle(new Vector2(xCoordinates[0], yCoordinates[0]), new Vector2(xCoordinates[1], yCoordinates[1]), new Vector2(xCoordinates[2], yCoordinates[2]), new Vector2(xCoordinates[3], yCoordinates[3]), new Vector2(l1.StartX, l1.StartY))) ||
            //                (GFG.PointInRectangle(new Vector2(xCoordinates[0], yCoordinates[0]), new Vector2(xCoordinates[1], yCoordinates[1]), new Vector2(xCoordinates[2], yCoordinates[2]), new Vector2(xCoordinates[3], yCoordinates[3]), new Vector2(l1.EndX, l1.EndY)))
            //                )
            //            {                        
            //                CheckAngle(activeRocksArray[i].rotation, out location, out angle1, out angle2);

            //                playerShip1.AddScore(activeRocksArray[i].score);

            //                if (activeRocksArray[i].size > 1)
            //                {
            //                    Console.WriteLine($"Before Collision Check: {activeRocks.Count}");

            //                    Rocks newRock1 = new Rocks(angle2, activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, activeRocksArray[i].typeOfRock, StateManager.Game);
            //                    newRock1.Initialize();
            //                    activeRocks.AddLast(newRock1);
            //                    newRock1.MAX_THRUST_POWER = 1f;
            //                    Rocks newRock2 = new Rocks(angle1, activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, activeRocksArray[i].typeOfRock, StateManager.Game);
            //                    newRock2.Initialize();
            //                    activeRocks.AddLast(newRock2);
            //                    newRock2.MAX_THRUST_POWER = 1f;
            //                    activeRocks.Remove(activeRocksArray[i]);
            //                    activeRocksArray[i].Dispose();

            //                    Console.WriteLine($"After Collision Check: {activeRocks.Count}");
            //                }
            //                else
            //                {
            //                    activeRocks.Remove(activeRocksArray[i]);
            //                    activeRocksArray[i].Dispose();
            //                    Console.WriteLine($"activeRocks Count{activeRocks.Count}");
            //                }

            //                activePlayerLasers.First.Value.Dispose();
            //                activePlayerLasers.Remove(activePlayerLasers.First.Value);
            //                return;
            //            }
            //            else
            //            {
            //                Console.WriteLine("Failed");
            //            }

            //        }
            //    }
            //}
            #endregion
            #endregion


            #endregion

            ////Check Collision between EnemyShip and Player Laser
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

            if (activeEnemyLasers.Count > 0)
            {
                //This is on both the activePlayerlaser and activeEnemyShip
                if (activeEnemyLasers.First.Value.CheckLaser() == true) //Make a check in Laser so that the laser can only run for 5 seconds.
                {
                    activeEnemyLasers.First.Value.Dispose();
                    activeEnemyLasers.Remove(activeEnemyLasers.First.Value);
                    return;
                }
                laserCollision = activeEnemyLasers.First.Value.ConvertLaserLine2D();

                //This one is activeEnemyLasers, because it is checking the collision between the Player and the EnemyShip
                foreach (Line2D l1 in laserCollision)
                {
                    foreach (Line2D l2 in playerShip1List)
                    {
                        if (Line2D.Intersects(l1, l2) && !activeEnemyLasers.First.Value.playerLaser)
                        {
                            playerShip1.CrashShip();
                            activeEnemyLasers.First.Value.Dispose();
                            activeEnemyLasers.Remove(activeEnemyLasers.First.Value);
                            return;
                        }
                    }
                }
                return;

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

        private void CheckAngle(float radianAngle, out int location, out float angle1, out float angle2)
        {
            float degreeAngle = radianAngle * (float)(Math.PI / 180);
            angle1 = 0;
            angle2 = 0;
            location = 0;
            int direction = 1;
            bool found = false;

            while (!found)
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
            
            ======================================================================================================
            Ideas:
            Maybe read the High Score from a .txt file, and then if there is not one available, then generate one, and update the scores of the listing. 
                This way, high scores will be kept.
                Prehaps in the Exit Command within Monogame, I can make the base function that is called 
                Whenever the ESC Key is pressed to call the function within HighScore that will write the current
                score to the file before closing. Otherwise, I can just update the file whenever a new HighScore is 
                made.

            With the EnemyShip, make them spawn between random intervals between 50 seconds and 3 minutes.
                Maybe make it so they only appear once the player is above a certain score?

            Also, with the EnemyShip, prehaps I can randomly spawn it on either side of the screen
                When the ship starts at the left side of the screen, set MAX_THRUST_POWER to positive
                When the ship starts at the right side of the screen, set MAX_THRUST_POWER to negative

            Maybe the EnemyShips shoot lasers at the PlayerShip at Random Intervals while they are on screen
                (The Intervals will be between 3 and 5 seconds)
                Also make their Lasers not last as long. Maybe you can set this in the Laser Class's 
                Update Method, and then check laserType. If positive (player), then make it the normal three seconds
                that I have been doing. If not, then make it 1 second or something. 

            ======================================================================================================

             */
            #endregion

        }


        private void ActiveRocksRemove(Rocks r)
        {
            activeRocks.Remove(r);
        }

        private void RandomEnemyShipSpawnVariables(out int lr, out int yPos)
        {
            Random rand = new Random();
            //Left and Right Position on the Screen
            lr = 0;
            //Verticle Position on the Screen
            yPos = 0;

            randomEnemyShipSpawnTime = rand.Next(50, 181);
            //randomEnemyShipSpawnTime = rand.Next(1, 5);
            //Generate whether or not its on the left side or right side
            lr = rand.Next(0, 2);
            //Generate Y position on Screen
            yPos = rand.Next(200, 501);

        }

        private void RandomEnemyShipFireVariables()
        {
            //Regenerate enemyShipFireTime
            Random rand = new Random();
            randomEnemyShipFireTime = rand.Next(1, 6);
        }
    }



    class GFG
    {

        // A utility function to calculate area
        // of triangle formed by (x1, y1),
        // (x2, y2) and (x3, y3)
        static float area(float  x1, float y1, float x2,
                          float y2, float x3, float y3)
        {
            return (float)Math.Abs((x1 * (y2 - y3) +
                                    x2 * (y3 - y1) +
                                    x3 * (y1 - y2)) / 2.0);
        }

        // A function to check whether point P(x, y)
        // lies inside the rectangle formed by A(x1, y1),
        // B(x2, y2), C(x3, y3) and D(x4, y4)
        public static bool check(float x1, float y1, float x2,
                          float y2, float x3, float y3,
                       float x4, float y4, float x, float y)
        {

            // Calculate area of rectangle ABCD
            float A = area(x1, y1, x2, y2, x3, y3) +
                      area(x1, y1, x4, y4, x3, y3);

            // Calculate area of triangle PAB
            float A1 = area(x, y, x1, y1, x2, y2);

            // Calculate area of triangle PBC
            float A2 = area(x, y, x2, y2, x3, y3);

            // Calculate area of triangle PCD
            float A3 = area(x, y, x3, y3, x4, y4);

            // Calculate area of triangle PAD
            float A4 = area(x, y, x1, y1, x4, y4);

            // Check if sum of A1, A2, A3 
            // and A4is same as A
            return (A == A1 + A2 + A3 + A4);
        }



        static bool PointInTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
        {
            // Compute vectors        
            Vector2 v0 = C - A;
            Vector2 v1 = B - A;
            Vector2 v2 = P - A;

            // Compute dot products
            float dot00 = Vector2.Dot(v0, v0);
            float dot01 = Vector2.Dot(v0, v1);
            float dot02 = Vector2.Dot(v0, v2);
            float dot11 = Vector2.Dot(v1, v1);
            float dot12 = Vector2.Dot(v1, v2);

            // Compute barycentric coordinates
            float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
            float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
            float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

            // Check if point is in triangle
            if (u >= 0 && v >= 0 && u <= 1 && v <= 1)
            { 
                return true; 
            }
            else 
            { 
                return false; 
            }
        }
    


        public static bool PointInRectangle(Vector2 X, Vector2 Y, Vector2 Z, Vector2 W, Vector2 P)
        {
            if (PointInTriangle(X, Y, Z, P)) return true;
            if (PointInTriangle(X, Z, W, P)) return true;
            return false;
        }
    }
}
