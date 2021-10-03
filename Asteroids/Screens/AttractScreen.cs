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
    public class AttractScreen : GameScreen
    {        
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont menuFont;

        TimeSpan elapsedTime = TimeSpan.Zero;
        
        HighScore highScore;

        AttractVisuals attractVisuals;

        PlayerShip playerShip1;
        Laser laser;

        EnemyShip enemyShip;

        //LinkedList<Laser> activeLasers;

        LinkedList<Laser> activePlayerLasers;
        LinkedList<Laser> activeEnemyLasers;

        LinkedList<Rocks> activeRocks;
        LinkedList<EnemyShip> activeEnemyShips;

        LinkedList<Line2D> playerShip1List;
        
        TimeSpan enemyShipTime;
        TimeSpan enemyShipFireTime;
        TimeSpan playerShipFireTime;
        TimeSpan playerShipRotateTime;
        TimeSpan playerShipThrustTime;

        TimeSpan collisionTime;       

        int randomEnemyShipSpawnTime;
        int randomEnemyShipFireTime;
        int randomPlayerShipFireTime;
        int randomPlayerShipRotateTime;
        int randomPlayerShipThrustTime;

        float randomPlayerAngle;

        bool testHit = false;

        bool screenSwitch = false;


        public AttractScreen()
        {
            LoadContent();            
        }

        public AttractScreen(HighScore highScoreObject)
        {
            LoadContent();
            highScore = highScoreObject;
        }
        

        public override void LoadContent()
        {
            spriteBatch = new SpriteBatch(StateManager.GraphicsDevice);
            spriteFont = StateManager.Content.Load<SpriteFont>("font");
            
            StateManager.Game.Window.Title = "Asteroids";

            attractVisuals = new AttractVisuals(StateManager.Game);

            #region PlayScreen Variables

            playerShip1 = new PlayerShip(StateManager.Game,true);
            playerShip1.Initialize();

            activePlayerLasers = new LinkedList<Laser>();
            activeEnemyLasers = new LinkedList<Laser>();

            activeRocks = new LinkedList<Rocks>();

            activeEnemyShips = new LinkedList<EnemyShip>();

            RandomEnemyShipSpawnVariables(out int test, out int test2);
            RandomEnemyShipFireVariables();

            enemyShipTime = TimeSpan.Zero;
            enemyShipFireTime = TimeSpan.Zero;
            playerShipFireTime = TimeSpan.Zero;
            collisionTime = TimeSpan.Zero;

            #endregion
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

            if (input.KeyboardState.WasKeyPressed(Keys.Escape))
            {
                screen.Pop();                
            }

            if (elapsedTime >= TimeSpan.FromSeconds(5))
            {
                // Switch to either High Score or Demo Screen. 
                elapsedTime = TimeSpan.Zero;
                if (screenSwitch)
                {
                    screenSwitch = false;
                }
                else
                {
                    screenSwitch = true;
                }
            }
            elapsedTime += gameTime.ElapsedGameTime;
            
            UpdateDemoScreen(gameTime);            
        }

        public override void Draw(GameTime gameTime)
        {            
            if (screenSwitch)
            {
                highScore.active = true;
                highScore.Draw(gameTime);
            }
            else
            {                
                attractVisuals.Draw(gameTime);

                DrawDemoScreen(gameTime);                
            }
        }

        #region PlayScreen 

        public void UpdateDemoScreen(GameTime gameTime)
        {
            if (!playerShip1.gameEnded)
            {                
                if (!playerShip1.crashed)
                {
                    //#region Mock Player Controls
                    //if (playerShipFireTime > TimeSpan.FromSeconds(randomPlayerShipFireTime))
                    //{
                    //    //Regenerate randomEnemyShipFireTime
                    //    RandomPlayerShipFire();

                    //    //Fire Laser

                    //    laser = new Laser(StateManager.Game, playerShip1.position.X, playerShip1.position.Y, playerShip1.Rotation, true);
                    //    //laser = new Laser(StateManager.Game, activeEnemyShips.First.Value.position.X, activeEnemyShips.First.Value.position.Y, (float)(Math.PI / 2), false);

                    //    laser.Initialize();
                    //    activePlayerLasers.AddLast(laser);
                    //    //Reset enemyShipFireTime
                    //    playerShipFireTime = TimeSpan.Zero;
                    //}
                    //else
                    //{
                    //    playerShipFireTime += gameTime.ElapsedGameTime;
                    //}


                    //if (playerShipRotateTime > TimeSpan.FromSeconds(randomPlayerShipFireTime))
                    //{
                    //    playerShipRotateTime = TimeSpan.Zero;
                    //    Random rand = new Random();
                    //    double degree = rand.Next(0, 360);
                    //    degree *= (Math.PI) / 180;

                    //    playerShip1.DemoRotation(degree);
                    //}
                    //else
                    //{
                    //    playerShipRotateTime += gameTime.ElapsedGameTime;
                    //}


                    //if (playerShipThrustTime > TimeSpan.FromSeconds(randomPlayerShipFireTime))
                    //{
                    //    playerShipThrustTime = TimeSpan.Zero;

                    //}
                    //else
                    //{
                    //    playerShipThrustTime += gameTime.ElapsedGameTime;
                    //}
                    //#endregion 


                    //collisionTime += gameTime.ElapsedGameTime;

                    //if (collisionTime > TimeSpan.FromSeconds(.01))
                    //{
                    //    playerShip1List = playerShip1.ConvertLanderLine2D();
                    //    CheckCollisionRocks(playerShip1List);
                    //    collisionTime = TimeSpan.Zero;
                    //}
                }
                else
                {
                    if (elapsedTime > TimeSpan.FromSeconds(1))
                    {
                        playerShip1.crashed = false;
                        playerShip1.Lives++;
                    }
                }

            }
            else //If the Game Has ended
            {
                screenSwitch = true;
            }

            //Updating Objects
            //playerShip1.Update(gameTime);

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
            }

            //If there are no ships currently active
            if (activeEnemyShips.Count < 1)
            {
                if (enemyShipTime > TimeSpan.FromSeconds(randomEnemyShipSpawnTime))
                {
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
                }

                enemyShipTime += gameTime.ElapsedGameTime;
            }
            //If there are ships currently active
            else
            {
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
                #endregion
            }
        }

        public void DrawDemoScreen(GameTime gameTime)
        {
            //playerShip1.Draw(gameTime);           
         
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
        }


        public LinkedList<LinkedList<Line2D>> ConvertRocksLine2D(LinkedList<LinkedList<Vector2>> rocks)
        {
            LinkedList<LinkedList<Line2D>> totalLines = new LinkedList<LinkedList<Line2D>>();

            return totalLines;
        }

        public void CheckCollisionRocks(LinkedList<Line2D> land)
        {
            LinkedList<Line2D> rockCollision = new LinkedList<Line2D>();
            LinkedList<Line2D> enemyShipCollision = new LinkedList<Line2D>();
            LinkedList<Line2D> laserCollision = new LinkedList<Line2D>();


            #region Player Lasers Collision

            Laser[] activePlayerLasersArray = activePlayerLasers.ToArray();
            for (int i = 0; i < activePlayerLasersArray.Count(); i++)
            {
                if (activePlayerLasersArray[i].CheckLaser() == true)
                {

                    activePlayerLasers.Remove(activePlayerLasersArray[i]);
                    activePlayerLasersArray[i].Dispose();
                }
            }


            #region Collision Between Player Laser and EnemyShip

            foreach (Laser l in activePlayerLasers)
            {
                float[] xCoordinates = new float[8];
                float[] yCoordinates = new float[8];
                laserCollision = l.ConvertLaserLine2D();

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
                            activeEnemyShips.First.Value.Dispose();
                            activeEnemyShips.Remove(activeEnemyShips.First.Value);
                            return;
                        }
                    }
                }
            }

            #endregion



            #region Collision Between Rock and Player Laser

            foreach (Laser l in activePlayerLasers)
            {
                laserCollision = l.ConvertLaserLine2D();
                
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
                            }
                            else
                            {
                                activeRocks.Remove(activeRocksArray[i]);
                                activeRocksArray[i].Dispose();                                
                            }

                            l.Dispose();
                            activePlayerLasers.Remove(l);
                            return;
                        }                        
                    }
                }
            }

            #endregion
            #endregion

            #region Enemy Laser and Player Ship Collision
            if (activeEnemyLasers.Count > 0)
            {
                if (activeEnemyLasers.First.Value.CheckLaser() == true) 
                {
                    activeEnemyLasers.First.Value.Dispose();
                    activeEnemyLasers.Remove(activeEnemyLasers.First.Value);
                    return;
                }
                laserCollision = activeEnemyLasers.First.Value.ConvertLaserLine2D();

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
            #endregion


            #region Collision between Player Ship and Rocks
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
            #endregion
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

        private void RandomPlayerShipFire()
        {
            Random rand = new Random();
            randomPlayerShipFireTime  = rand.Next(1, 6);
        }
        private void RandomPlayerShipRotate()
        {
            Random rand = new Random();            
            randomPlayerShipRotateTime = rand.Next(1, 10);            
        }

        private void RandomPlayerShipThrust()
        {
            Random rand = new Random();
            randomPlayerShipThrustTime = rand.Next(1, 6);            
        }

        #endregion
    }
}
