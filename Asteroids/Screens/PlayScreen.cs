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
        SpriteBatch spriteBatch;
        int counter;
        bool landed;
        PlayerShip playerShip1;
        PlayerShip playerShip2;
        Laser laser;

        EnemyShip enemyShip;

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

        LinkedList<Explosion> activeExplosion;

        HighScore highScore;

        TimeSpan elapsedTime;
        TimeSpan enemyShipTime;
        TimeSpan enemyShipFireTime;        

        int randomEnemyShipSpawnTime;
        int randomEnemyShipFireTime;

        bool testHit = false;


        List<Song> songs;
        List<SoundEffect> soundEffects;

        public PlayScreen()
        {
            LoadContent();
        }

        public PlayScreen(HighScore highScoreObject)
        {
            LoadContent();
            highScore = highScoreObject;
        }

        public override void LoadContent()
        {
            songs = new List<Song>();
            soundEffects = new List<SoundEffect>();


            songs.Add(StateManager.Game.Content.Load<Song>("2020-06-18_-_8_Bit_Retro_Funk_-_www.FesliyanStudios.com_David_Renda"));
            
            soundEffects.Add(StateManager.Game.Content.Load<SoundEffect>("SoundEffects/PlayerShoot"));
            soundEffects.Add(StateManager.Game.Content.Load<SoundEffect>("SoundEffects/EnemyShoot"));
            soundEffects.Add(StateManager.Game.Content.Load<SoundEffect>("SoundEffects/EnemyShipExplosionSFX"));
            soundEffects.Add(StateManager.Game.Content.Load<SoundEffect>("SoundEffects/PlayerShipExplosionSFX"));
            soundEffects.Add(StateManager.Game.Content.Load<SoundEffect>("SoundEffects/RockExplosionSFX"));
            soundEffects.Add(StateManager.Game.Content.Load<SoundEffect>("explosion"));            

            spriteBatch = new SpriteBatch(StateManager.GraphicsDevice);

            playerShip1 = new PlayerShip(StateManager.Game, false);
            playerShip1.Initialize();

            activePlayerLasers = new LinkedList<Laser>();
            activeEnemyLasers = new LinkedList<Laser>();

            activeRocks = new LinkedList<Rocks>();

            activeExplosion = new LinkedList<Explosion>();
        
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
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(songs[0]);
            }
            else if (MediaPlayer.State == MediaState.Paused)
            {
                MediaPlayer.Resume();
            }

            if (!playerShip1.gameEnded)
            {
                if (input.KeyboardState.WasKeyPressed(Keys.P))
                {
                    PauseScreen pause = new PauseScreen();
                    screen.Push(pause);
                    MediaPlayer.Pause();
                }
                if (input.KeyboardState.WasKeyPressed(Keys.Escape))
                {
                    screen.Pop();
                    AttractScreen attract = new AttractScreen(highScore);
                    screen.Push(attract);
                    MediaPlayer.Stop();
                }


                //Check Collision
                if (!playerShip1.crashed)
                {
                    if (input.KeyboardState.WasKeyPressed(Keys.Enter))
                    {
                        if (activePlayerLasers.Count() < 1)
                        {
                            laser = new Laser(StateManager.Game, playerShip1.position.X, playerShip1.position.Y, playerShip1.Rotation, true);
                            laser.Initialize();
                            activePlayerLasers.AddLast(laser);

                            PlaySoundEffect(0);
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

                playerShip1.AddLife();
            }
            else 
            //If the Game Has ended
            {
                if (highScore.CheckScore(playerShip1.score))
                {
                    //Making Adjustments to HighScore Object to Enter High Score Screen
                    highScore.score = playerShip1.score;
                    highScore.selection = true;
                    
                    //Add HighScore Screen to Stack and Pop PlayScreen.
                    screen.Pop();
                    HighScoreScreen highScoreScreen = new HighScoreScreen(highScore);
                    screen.Push(highScoreScreen);
                    MediaPlayer.Stop();
                    return;
                }
                else
                {
                    screen.Pop();
                    AttractScreen attract = new AttractScreen(highScore);
                    screen.Push(attract);
                    MediaPlayer.Stop();
                    return;
                }
            }
            
            #region Updating Objects
            playerShip1.Update(gameTime);

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

            if(activeExplosion.Count > 0)
            {
                foreach (Explosion ex in activeExplosion)
                {
                    ex.Update(gameTime);
                }
            }

            Explosion[] activeExplosionsArray = activeExplosion.ToArray();
            for (int i = 0; i < activeExplosionsArray.Length; i++)
            {
                if (activeExplosionsArray[i].finished)
                {
                    activeExplosion.Remove(activeExplosionsArray[i]);
                }
            }
            #endregion

            #region Determine if to add Rocks or EnemyShip

            #region Add Rocks
            if (activeRocks.Count < 1)
            {
                Random rand = new Random();

                int positionX = 0;
                int positionY = 0;
                int topAndBottomBorder;
                int leftAndRightBorder;
                int verticleOrHorizontal;

                float angle = 0;
                int numberOfRocks = 6;

                if (playerShip1.score < 1000)
                {
                    numberOfRocks = rand.Next(5,8);
                }
                else if (playerShip1.score < 2000)
                {
                    numberOfRocks = rand.Next(6,10);
                }
                else if (playerShip1.score < 3000)
                {
                    numberOfRocks = rand.Next(7,12);
                }
                else if (playerShip1.score > 3000)
                {
                    numberOfRocks = rand.Next(8,15);
                }

                while (activeRocks.Count < numberOfRocks)
                {
                    #region Spawn Rocks

                    do
                    {
                        verticleOrHorizontal = rand.Next(0, 2);
                        topAndBottomBorder = rand.Next(0, 2);
                        leftAndRightBorder = rand.Next(0, 2);

                        if (verticleOrHorizontal == 0)
                        //Rock Spawns on the Top or Bottom Side of the Screen
                        {
                            if (topAndBottomBorder == 0)
                            //Top Screen
                            {
                                positionX = rand.Next(0, StateManager.GraphicsDevice.Viewport.Width);
                                positionY = 0;
                            }
                            else
                            //Bottom Screen
                            {
                                positionX = rand.Next(0, StateManager.GraphicsDevice.Viewport.Width);
                                positionY = StateManager.GraphicsDevice.Viewport.Height;
                            }
                        }
                        else
                        //Rock Spawns on the Left or Bottom Side of the Screen
                        {
                            if (leftAndRightBorder == 0)
                            //Left Side
                            {
                                positionX = 0;
                                positionY = rand.Next(0, StateManager.GraphicsDevice.Viewport.Height);

                            }
                            else
                            //Right Side
                            {
                                positionX = StateManager.GraphicsDevice.Viewport.Width;
                                positionY = rand.Next(0, StateManager.GraphicsDevice.Viewport.Height);

                            }
                        }
                    } while (
                    (Math.Abs(positionX - playerShip1.position.X) < 100) &&
                    (Math.Abs(positionY - playerShip1.position.Y) < 100));

                    angle = rand.Next(0, 361);

                    Rocks newRock = new Rocks(StateManager.Game, positionX, positionY, (float)(angle * Math.PI / 180), 1, 3);
                    newRock.Initialize();
                    activeRocks.AddLast(newRock);

                    if (playerShip1.score < 1000)
                    {
                        newRock.MAX_THRUST_POWER = 1;
                    }
                    else if (playerShip1.score < 2000)
                    {
                        newRock.MAX_THRUST_POWER = 2;
                    }
                    else if (playerShip1.score < 3000)
                    {
                        newRock.MAX_THRUST_POWER = 3;
                    }
                    else if (playerShip1.score > 3000)
                    {
                        newRock.MAX_THRUST_POWER = 5;
                    }

                    #endregion
                }
            }
            #endregion

            #region Add Enemy Ships

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

                    if (playerShip1.score < 1000)
                    {
                        enemyShip.MAX_THRUST_POWER = 1;
                    }
                    else if (playerShip1.score < 2000)
                    {
                        enemyShip.MAX_THRUST_POWER = 2;
                    }
                    else if (playerShip1.score < 3000)
                    {
                        enemyShip.MAX_THRUST_POWER = 3;
                    }
                    else if (playerShip1.score > 3000)
                    {
                        enemyShip.MAX_THRUST_POWER = 5;
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
                    
                    laser.Initialize();
                    activeEnemyLasers.AddLast(laser);
                    //Reset enemyShipFireTime
                    enemyShipFireTime = TimeSpan.Zero;

                    PlaySoundEffect(1);
                }
                else
                {
                    enemyShipFireTime += gameTime.ElapsedGameTime;
                }
            }

            #endregion

            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            #region Draw Objects
            
            playerShip1.Draw(gameTime);           
         
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

            if (activeExplosion.Count > 0)
            {
                foreach (Explosion ex in activeExplosion)
                {
                    ex.Draw(gameTime);
                }
            }

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

            #region  Check if Lasers are Still Active

            Laser[] activePlayerLasersArray = activePlayerLasers.ToArray();
            for (int i = 0; i < activePlayerLasersArray.Count(); i++)
            {
                if (activePlayerLasersArray[i].CheckLaser() == true) //Make a check in Laser so that the laser can only run for 5 seconds.
                {
                    activePlayerLasers.Remove(activePlayerLasersArray[i]);
                    activePlayerLasersArray[i].Dispose();
                }
            }
            
            #endregion

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
                            Explosion explode = new Explosion(StateManager.Game, activeEnemyShips.First.Value.position.X, activeEnemyShips.First.Value.position.Y, 0);
                            explode.Initialize();
                            activeExplosion.AddLast(explode);

                            activeEnemyShips.First.Value.Dispose();
                            activeEnemyShips.Remove(activeEnemyShips.First.Value);

                            PlaySoundEffect(2);

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
                    int index = 1;

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
                                Rocks newRock2 = new Rocks(angle1, activeRocksArray[i].size - 1, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, activeRocksArray[i].typeOfRock, StateManager.Game);
                                newRock2.Initialize();
                                activeRocks.AddLast(newRock2);
                                
                                Explosion explode = new Explosion(StateManager.Game, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, 0);
                                explode.Initialize();
                                activeExplosion.AddLast(explode);

                                if (playerShip1.score < 1000)
                                {
                                    newRock1.MAX_THRUST_POWER = 1f;
                                    newRock2.MAX_THRUST_POWER = 1f;
                                }
                                else if (playerShip1.score < 2000)
                                {
                                    newRock1.MAX_THRUST_POWER = 2f;
                                    newRock2.MAX_THRUST_POWER = 2f;
                                }
                                else if (playerShip1.score < 3000)
                                {
                                    newRock1.MAX_THRUST_POWER = 3f;
                                    newRock2.MAX_THRUST_POWER = 3f;
                                }
                                else if (playerShip1.score > 3000)
                                {
                                    newRock1.MAX_THRUST_POWER = 10f;
                                    newRock2.MAX_THRUST_POWER = 10f;
                                }

                                activeRocks.Remove(activeRocksArray[i]);
                                activeRocksArray[i].Dispose();
                            }
                            else
                            {
                                Explosion explode = new Explosion(StateManager.Game, activeRocksArray[i].position.X, activeRocksArray[i].position.Y, 0);
                                explode.Initialize();
                                activeExplosion.AddLast(explode);

                                activeRocks.Remove(activeRocksArray[i]);
                                activeRocksArray[i].Dispose();                                
                            }

                            PlaySoundEffect(4);

                            l.Dispose();
                            activePlayerLasers.Remove(l);
                            return;
                        }                        
                    }
                }
            }

            #endregion

            #endregion

            #region Enemy Laser Collision
            
            if (activeEnemyLasers.Count > 0)
            {
                //This is on both the activePlayerlaser and activeEnemyShip
                //Make a check in Laser so that the laser can only run for 5 seconds.
                if (activeEnemyLasers.First.Value.CheckLaser() == true) 
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
                            PlaySoundEffect(3);
                            activeEnemyLasers.First.Value.Dispose();
                            activeEnemyLasers.Remove(activeEnemyLasers.First.Value);
                            return;
                        }
                    }
                }
                return;

            }

            #endregion

            #region Collision between Rocks and Player Ship

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
                            PlaySoundEffect(5);
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

            if (playerShip1.score < 1000)
            {
                randomEnemyShipSpawnTime = rand.Next(50, 181);
            }
            else if (playerShip1.score < 2000)
            {
                randomEnemyShipSpawnTime = rand.Next(50, 121);
            }
            else if (playerShip1.score < 3000)
            {
                randomEnemyShipSpawnTime = rand.Next(40, 100);
            }
            else if (playerShip1.score > 3000)
            {
                randomEnemyShipSpawnTime = rand.Next(20, 70);
            }

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

        private void PlaySoundEffect(int soundEffectIndex)
        {
            switch (soundEffectIndex)
            {
                case 0:
                    var playerShoot = soundEffects[0].CreateInstance();
                    if (playerShoot.State != SoundState.Playing)
                    {
                        playerShoot.IsLooped = false;
                        playerShoot.Play();
                    }
                    return;
                case 1:
                    var enemyShoot = soundEffects[1].CreateInstance();
                    if (enemyShoot.State != SoundState.Playing)
                    {
                        enemyShoot.IsLooped = false;
                        enemyShoot.Play();
                    }
                    return;
                case 2:
                    var enemyShipExplosionSFX = soundEffects[2].CreateInstance();
                    if (enemyShipExplosionSFX.State != SoundState.Playing)
                    {
                        enemyShipExplosionSFX.IsLooped = false;
                        enemyShipExplosionSFX.Play();
                    }
                    return;
                case 3:
                    var playerShipExplosionLaserSFX = soundEffects[3].CreateInstance();
                    if (playerShipExplosionLaserSFX.State != SoundState.Playing)
                    {
                        playerShipExplosionLaserSFX.IsLooped = false;
                        playerShipExplosionLaserSFX.Play();
                    }
                    return;
                case 4:
                    var rockExplosionSFX = soundEffects[4].CreateInstance();
                    if (rockExplosionSFX.State != SoundState.Playing)
                    {
                        rockExplosionSFX.IsLooped = false;
                        rockExplosionSFX.Play();
                    }
                    return;
                case 5:
                    var playerShipExplosionRockSFX = soundEffects[5].CreateInstance();
                    if (playerShipExplosionRockSFX.State != SoundState.Playing)
                    {
                        playerShipExplosionRockSFX.IsLooped = false;
                        playerShipExplosionRockSFX.Play();
                    }
                    return;
            }
        }
    }


    class GFG
    {

        // A utility function to calculate area
        // of triangle formed by (x1, y1),
        // (x2, y2) and (x3, y3)
        static float area(float x1, float y1, float x2,
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
