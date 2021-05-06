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

namespace Asteroids
{
    class PlayerShip : DrawableGameComponent
    {
        #region VARIABLES 

        #region Components

        private PrimitiveBatch pb;

        TimeSpan elapsedTime;
        TimeSpan gravityTime;

        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;

        List<SoundEffect> soundEffects;
        public bool victorySoundPlaying;

        #endregion

        #region PlayerShip Variables

        LinkedList<Vector2> playerShip;

        public Vector2 position;
        private float scale;

        private float uiScale;

        Vector2 center;

        public int score;

        #endregion

        #region Thrust Variables

        private int lives;
        public int Lives
        {
            get
            {
                return lives;
            }
            set
            {
                if (value < 0) { lives = 0; }                
                else { lives = value; }
            }
        }

        public bool isThrusting = false;
        public float thrustPower;
        public float thrustPowerIncrement;

        const float MAX_THRUST_POWER = 3.0f;
        #endregion

        #region Game State Variables

        public bool firstLanding;


        public bool landed, crashed = false;

        int explosionCounter;

        bool gameEnded;

        #endregion 

        #region Velocity Variables

        Vector2 UP = new Vector2(0, -1);
        Vector2 DOWN = new Vector2(0, 1);

        Vector2 velocityVector;
        Vector2 oldPosition;
        Vector2 newPosition;

        float angle, velocity, rotation;

        public float Rotation
        {
            get
            {
                return rotation;
            }
        }
        

        #endregion 

        #endregion

        #region Constructor
        public PlayerShip(Game game) : base(game)
        {
            pb = new PrimitiveBatch(game.GraphicsDevice);
        }

        #endregion

        #region Standard Monogame Methods (Initialize, LoadContent, Update, Draw)

        public override void Initialize()
        {
            soundEffects = new List<SoundEffect>();

            position = new Vector2(50, 135);

            oldPosition = position;
            newPosition = position;

            scale = 1.5f;

            center = new Vector2(2, 2); 

            rotation = 0;

            playerShip = GetPlayerShip();

            lives = 2;

            elapsedTime = TimeSpan.Zero;
            gravityTime = TimeSpan.Zero;

            uiScale = 2f;

            thrustPower = 0;
            thrustPowerIncrement = 0.1f;
            

            oldKeyboardState = new KeyboardState();

            firstLanding = false;

            victorySoundPlaying = false;

            explosionCounter = 0;

            gameEnded = false;


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            soundEffects.Add(Game.Content.Load<SoundEffect>("explosion"));
            soundEffects.Add(Game.Content.Load<SoundEffect>("success"));
            soundEffects.Add(Game.Content.Load<SoundEffect>("thrust"));

            var victorySound = soundEffects[1].CreateInstance();
        }

        public override void Update(GameTime gameTime)
        {
            if (!crashed)
            {
                VelocityCalculation();

                CheckInput(gameTime);

                CheckRotation();

                RecalculatePosition(gameTime);
            }
            else
            {
                playerShip = GetPlayerShip();

                CheckInput(gameTime);
            }

            if (lives <= 0)
            {
                gameEnded = true;
            }
            oldKeyboardState = newKeyboardState;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.TransparentBlack);

            if (crashed)
            {
                GraphicsDevice.Clear(Color.Red);
            }           
            else
            {
                GraphicsDevice.Clear(Color.TransparentBlack);
            }

            pb.Begin(PrimitiveType.LineList);

            DrawPlayerShip();

            pb.End();

            #region User Interface

            DisplayTitle();
            DisplayAngle();            
            DisplayScore();
            


            if (crashed && !gameEnded)
            {
                DisplayCrashed();
            }
            if (gameEnded)
            {
                DisplayGameOver();
            }

            #endregion

            base.Draw(gameTime);
        }

        #endregion

        #region Drawing PlayerShip Methods (DrawPlayerShip, GetSquare, GetPlayerShip)

        public void DrawPlayerShip()
        {
            LinkedList<Vector2> lineList = GetPlayerShip();
            Color color = new Color();

            color = Color.White;

            foreach (Vector2 v2 in lineList)
            {
                float Xrotated = center.X + (v2.X - center.X) *
                  (float)Math.Cos(rotation) - (v2.Y - center.Y) *
                  (float)Math.Sin(rotation);

                float Yrotated = center.Y + (v2.X - center.X) *
                  (float)Math.Sin(rotation) + (v2.Y - center.Y) *
                  (float)Math.Cos(rotation);

                pb.AddVertex(new Vector2(position.X + (Xrotated * scale),
                  position.Y + (Yrotated * scale)), color);
            }
        }

        private LinkedList<Vector2> GetSquare()
        {
            LinkedList<Vector2> list = new LinkedList<Vector2>();

            list.AddLast(new Vector2(0 * scale, 0 * scale));
            list.AddLast(new Vector2(1 * scale, 0 * scale));

            list.AddLast(new Vector2(1 * scale, 0 * scale));
            list.AddLast(new Vector2(1 * scale, 1 * scale));

            list.AddLast(new Vector2(1 * scale, 1 * scale));
            list.AddLast(new Vector2(0 * scale, 1 * scale));

            list.AddLast(new Vector2(0 * scale, 1 * scale));
            list.AddLast(new Vector2(0 * scale, 0 * scale));

            return list;
        }

        public LinkedList<Vector2> GetPlayerShip()
        {
            LinkedList<Vector2> list = new LinkedList<Vector2>();

            if (!crashed)
            {
                #region Lunar Lander

                //Top Of Ship
                list.AddLast(new Vector2(0 * scale, 4 * scale));
                list.AddLast(new Vector2(2 * scale, 0 * scale));

                list.AddLast(new Vector2(2 * scale, 0 * scale));
                list.AddLast(new Vector2(4 * scale, 4 * scale));

                list.AddLast(new Vector2(4 * scale, 4 * scale));
                list.AddLast(new Vector2(0 * scale, 4 * scale));

                #endregion
            }
            else
            {
                #region CRASHED Lunar Lander

                float explosionScaled = (explosionCounter * scale);

                //Top Of Ship
                list.AddLast(new Vector2(1 * scale + (explosionScaled), 0 * scale + (explosionScaled)));
                list.AddLast(new Vector2(13 * scale + (explosionScaled), 0 * scale + (explosionScaled)));

                list.AddLast(new Vector2(13 * scale + (explosionScaled), 0 * scale - (explosionScaled)));
                list.AddLast(new Vector2(13 * scale + (explosionScaled), 9 * scale - (explosionScaled)));

                list.AddLast(new Vector2(13 * scale - (explosionScaled), 9 * scale + (explosionScaled)));
                list.AddLast(new Vector2(1 * scale - (explosionScaled), 9 * scale + (explosionScaled)));

                list.AddLast(new Vector2(1 * scale - (explosionScaled), 9 * scale - (explosionScaled)));
                list.AddLast(new Vector2(1 * scale - (explosionScaled), 0 * scale - (explosionScaled)));

                list.AddLast(new Vector2(1 * scale + (explosionScaled), 0 * scale + (explosionScaled)));
                list.AddLast(new Vector2(7 * scale + (explosionScaled), 9 * scale + (explosionScaled)));

                list.AddLast(new Vector2(7 * scale + (explosionScaled), 9 * scale - (explosionScaled)));
                list.AddLast(new Vector2(13 * scale + (explosionScaled), 0 * scale - (explosionScaled)));

                list.AddLast(new Vector2(1 * scale - (explosionScaled), 9 * scale + (explosionScaled)));
                list.AddLast(new Vector2(7 * scale - (explosionScaled), 0 * scale + (explosionScaled)));

                list.AddLast(new Vector2(7 * scale - (explosionScaled), 0 * scale - (explosionScaled)));
                list.AddLast(new Vector2(13 * scale - (explosionScaled), 9 * scale - (explosionScaled)));

                //Middle Portion
                //////list.AddLast(new Vector2(2 * scale + (explosionScaled), 9 * scale + (explosionScaled)));
                //////list.AddLast(new Vector2(12 * scale + (explosionScaled), 9 * scale + (explosionScaled)));

                //////list.AddLast(new Vector2(12 * scale + (explosionScaled), 9 * scale + (explosionScaled)));
                //////list.AddLast(new Vector2(12 * scale + (explosionScaled), 11 * scale + (explosionScaled)));

                //////list.AddLast(new Vector2(12 * scale - (explosionScaled), 11 * scale - (explosionScaled)));
                //////list.AddLast(new Vector2(2 * scale - (explosionScaled), 11 * scale - (explosionScaled)));

                //////list.AddLast(new Vector2(2 * scale - (explosionScaled), 11 * scale - (explosionScaled)));
                //////list.AddLast(new Vector2(2 * scale - (explosionScaled), 9 * scale - (explosionScaled)));


                //Left Thruster
                list.AddLast(new Vector2(0 * scale + (explosionScaled), 13 * scale + (explosionScaled)));
                list.AddLast(new Vector2(0 * scale + (explosionScaled), 12 * scale + (explosionScaled)));

                list.AddLast(new Vector2(0 * scale + (explosionScaled), 12 * scale - (explosionScaled)));
                list.AddLast(new Vector2(1 * scale + (explosionScaled), 11 * scale - (explosionScaled)));

                list.AddLast(new Vector2(1 * scale - (explosionScaled), 11 * scale + (explosionScaled)));
                list.AddLast(new Vector2(3 * scale - (explosionScaled), 11 * scale + (explosionScaled)));

                list.AddLast(new Vector2(3 * scale - (explosionScaled), 11 * scale - (explosionScaled)));
                list.AddLast(new Vector2(4 * scale - (explosionScaled), 12 * scale - (explosionScaled)));

                list.AddLast(new Vector2(4 * scale + (explosionScaled), 12 * scale + (explosionScaled)));
                list.AddLast(new Vector2(4 * scale + (explosionScaled), 13 * scale + (explosionScaled)));

                list.AddLast(new Vector2(4 * scale + (explosionScaled), 13 * scale - (explosionScaled)));
                list.AddLast(new Vector2(0 * scale + (explosionScaled), 13 * scale - (explosionScaled)));

                list.AddLast(new Vector2(1 * scale - (explosionScaled), 11 * scale + (explosionScaled)));
                list.AddLast(new Vector2(1 * scale - (explosionScaled), 13 * scale + (explosionScaled)));

                list.AddLast(new Vector2(3 * scale - (explosionScaled), 11 * scale - (explosionScaled)));
                list.AddLast(new Vector2(3 * scale - (explosionScaled), 13 * scale - (explosionScaled)));

                //Right Thruster
                list.AddLast(new Vector2(10 * scale + (explosionScaled), 13 * scale + (explosionScaled)));
                list.AddLast(new Vector2(10 * scale + (explosionScaled), 12 * scale + (explosionScaled)));

                list.AddLast(new Vector2(10 * scale + (explosionScaled), 12 * scale - (explosionScaled)));
                list.AddLast(new Vector2(11 * scale + (explosionScaled), 11 * scale - (explosionScaled)));

                list.AddLast(new Vector2(11 * scale - (explosionScaled), 11 * scale + (explosionScaled)));
                list.AddLast(new Vector2(13 * scale - (explosionScaled), 11 * scale + (explosionScaled)));

                list.AddLast(new Vector2(13 * scale - (explosionScaled), 11 * scale - (explosionScaled)));
                list.AddLast(new Vector2(14 * scale - (explosionScaled), 12 * scale - (explosionScaled)));

                list.AddLast(new Vector2(14 * scale + (explosionScaled), 12 * scale + (explosionScaled)));
                list.AddLast(new Vector2(14 * scale + (explosionScaled), 13 * scale + (explosionScaled)));

                list.AddLast(new Vector2(14 * scale + (explosionScaled), 13 * scale - (explosionScaled)));
                list.AddLast(new Vector2(10 * scale + (explosionScaled), 13 * scale - (explosionScaled)));

                list.AddLast(new Vector2(11 * scale - (explosionScaled), 11 * scale + (explosionScaled)));
                list.AddLast(new Vector2(11 * scale - (explosionScaled), 13 * scale + (explosionScaled)));

                list.AddLast(new Vector2(13 * scale - (explosionScaled), 11 * scale - (explosionScaled)));
                list.AddLast(new Vector2(13 * scale - (explosionScaled), 13 * scale - (explosionScaled)));

                #endregion

                explosionCounter += 2;
            }

            return list;
        }

        #endregion

        #region Display Methods (DisplayFuel, DisplayAngle, DisplayTitle, DisplayVelocity, DisplayScore, DisplayCrashed, DisplayInstructions, DisplayGameOver)

        public void DisplayAngle()
        {
            Color colour = Color.White;
            float px = 10.0f;
            float py = 5 * (Font.TextHeight(uiScale));

            string text = "ANGLE:" + string.Format("{0:D3}", (int)ConvertAngleDegrees());

            Font.WriteText(pb, px, py, uiScale, colour, text);
        }

        public void DisplayTitle()
        {
            //Color colour = Color.White;
            //string text = "Lunar Lander";
            //float px = 537.0f;
            //float py = 1 * (Font.TextHeight(uiScale));

            //float center = (float)(
            //    (StateManager.GraphicsDevice.Viewport.Width / 2) -
            //    ((Font.TextWidth(uiScale, text)) / 2));

            //Font.WriteTextSpecifyWidth(pb, px, py, uiScale, colour, text, 10, 3);
        }

        public void DisplayScore()
        {
            Color colour = Color.White;
            float px = 10.0f;
            string text;
            float py;

            text = $"Score {score}";
            py = 20 * (Font.TextHeight(uiScale));
            Font.WriteText(pb, px, py, uiScale, colour, text);
        }

        public void DisplayCrashed()
        {
            Color colour = Color.White;

            string text = "CRASHED SHUTTLE: LOST 10 UNITS OF FUEL";

            float px = (float)(
                (StateManager.GraphicsDevice.Viewport.Width / 2) -
                ((Font.TextWidth(uiScale, text)) / 2));

            float py = (float)(
                (StateManager.GraphicsDevice.Viewport.Height / 2) +
                ((Font.TextHeight(uiScale)) / 2));
            Font.WriteText(pb, px, py, uiScale, colour, text);

            text = "Press Enter to Respawn";

            px = (float)(
                (StateManager.GraphicsDevice.Viewport.Width / 2) -
                ((Font.TextWidth(uiScale, text)) / 2));

            py += 50;

            Font.WriteText(pb, px, py, uiScale, colour, text);
        }

        public void DisplayInstructions()
        {
            Color colour = Color.White;
            float px = 600f;
            string text;
            float py;

            text = $"Controls";
            px = 920;
            py = 1 * (Font.TextHeight(uiScale));
            Font.WriteTextSpecifyWidth(pb, px, py, uiScale, colour, text, 10, 3);

            text = $"Space-Thrust";
            px = 920;
            py = 5 * (Font.TextHeight(uiScale));
            Font.WriteTextSpecifyWidth(pb, px, py, uiScale, colour, text, 10, 3);

            text = "{  }-Rotate Lander";
            px = 920;
            py = 10 * (Font.TextHeight(uiScale));
            Font.WriteTextSpecifyWidth(pb, px, py, uiScale, colour, text, 10, 3);


            text = $"P-Pause";
            px = 920;
            py = 15 * (Font.TextHeight(uiScale));
            Font.WriteTextSpecifyWidth(pb, px, py, uiScale, colour, text, 10, 3);

            text = $"ESC-Exit Game";
            px = 920;
            py = 20 * (Font.TextHeight(uiScale));
            Font.WriteTextSpecifyWidth(pb, px, py, uiScale, colour, text, 10, 3);
        }

        public void DisplayGameOver()
        {
            Color colour = Color.White;
            string text = "Out of Fuel";
            float px = (float)(
                (StateManager.GraphicsDevice.Viewport.Width / 2) -
                ((Font.TextWidth(uiScale, text)) / 2));
            float py = (float)(
                (StateManager.GraphicsDevice.Viewport.Height / 2) +
                ((Font.TextHeight(uiScale)) / 2));
            py -= 50;
            Font.WriteText(pb, px, py, uiScale, colour, text);

            text = "Game Over";
            px = (float)(
                (StateManager.GraphicsDevice.Viewport.Width / 2) -
                ((Font.TextWidth(uiScale, text)) / 2));
            py += 50;
            Font.WriteText(pb, px, py, uiScale, colour, text);

            text = "Press ESC To Exit";
            px = (float)(
                (StateManager.GraphicsDevice.Viewport.Width / 2) -
                ((Font.TextWidth(uiScale, text)) / 2));
            py += 50;
            Font.WriteText(pb, px, py, uiScale, colour, text);
        }

        #endregion

        #region Converstion Methods (GetLanderForCollision, ConvertLanderLine2D, ConvertAngleDegrees)

        public LinkedList<Vector2> GetLanderForCollision()
        {
            LinkedList<Vector2> lineList = GetPlayerShip();

            LinkedList<Vector2> ll = new LinkedList<Vector2>();

            foreach (Vector2 v in lineList)
            {
                float Xrotated = center.X + (v.X - center.X) *
                  (float)Math.Cos(angle) - (v.Y - center.Y) *
                  (float)Math.Sin(angle);

                float Yrotated = center.Y + (v.X - center.X) *
                  (float)Math.Sin(angle) + (v.Y - center.Y) *
                  (float)Math.Cos(angle);

                ll.AddLast(new Vector2(position.X + (Xrotated * scale),
                  position.Y + (Yrotated * scale)));
            }
            return ll;
        }

        public LinkedList<Line2D> ConvertLanderLine2D()
        {
            LinkedList<Vector2> land = GetLanderForCollision();

            LinkedList<Line2D> lines = new LinkedList<Line2D>();

            int count = 0;
            float var1 = 0.0f;
            float var2 = 0.0f;
            float var3 = 0.0f;
            float var4 = 0.0f;

            foreach (Vector2 l in land)
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
        }

        public double ConvertAngleDegrees()
        {
            return rotation * (180 / Math.PI);
        }

        #endregion

        #region Landing/Crashing Methods 

        public bool CheckLanding()
        {
            if ((ConvertAngleDegrees() > 355 || ConvertAngleDegrees() < 5) && Math.Abs(velocity) < 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CrashShip()
        {
            crashed = true;

            thrustPower = 0.0f;
            velocity = 0;
            explosionCounter = 0;

            Lives--;

            var instance = soundEffects[0].CreateInstance();
            if (instance.State != SoundState.Playing)
            {
                instance.IsLooped = false;
                instance.Play();
            }
        }


        #endregion

        #region Calculation/Input Methods (CheckRotation, AddScore, VelocityCalculation, CheckInput, RecalculatePosition)

        public void CheckRotation()
        {
            if (rotation > ((float)(2 * Math.PI)))
            {
                rotation -= ((float)(2 * Math.PI));
            }
            else if (rotation < 0)
            {
                rotation += ((float)(2 * Math.PI));
            }
            else if (rotation == ((float)(2 * Math.PI)))
            {
                rotation = 0f;
            }
        }

        public void AddScore(int addedScore)
        {
            
            score += addedScore;
        }

        public void VelocityCalculation()
        {
            oldPosition = newPosition;
            newPosition = position;

            velocityVector = new Vector2(
                 (newPosition.X - oldPosition.X),
                 (newPosition.Y - oldPosition.Y));
        }

        public void CheckInput(GameTime gameTime)
        {
            var instance = soundEffects[2].CreateInstance();

            newKeyboardState = Keyboard.GetState();

            if (!crashed)
            {
                #region Input when Ship is Not Crashed

                if (newKeyboardState.IsKeyDown(Keys.Space) && lives > 0)
                {
                    if (landed)
                    {
                        landed = false;
                        thrustPower = MAX_THRUST_POWER;
                        victorySoundPlaying = false;
                    }

                    thrustPower += thrustPowerIncrement;

                    if (thrustPower > MAX_THRUST_POWER)
                    {
                        thrustPower = MAX_THRUST_POWER;
                    }

                    elapsedTime += gameTime.ElapsedGameTime;

                    if (elapsedTime > TimeSpan.FromSeconds(1))
                    {
                        elapsedTime -= TimeSpan.FromSeconds(1);

                    }

                    if (instance.State != SoundState.Playing)
                    {
                        instance.IsLooped = false;
                        instance.Play();
                    }
                }
                else
                {
                    //Take a Look at this. Maybe add an If statement above that checks if the Space Bar was previously pressed. If it was, then set thrustPower to 0.
                    //thrustPower -= thrustPowerIncrement / 5;

                    //if (thrustPower < 0)
                    //{
                    //    thrustPower = 0;
                    //}
                }
                //LOOK INTO THIS AS WELL
                if (Keyboard.GetState().IsKeyUp(Keys.Space))
                {
                    thrustPower -= thrustPowerIncrement / 5;

                    if (thrustPower < 0)
                    {
                        thrustPower = 0;
                    }
                }

                if (newKeyboardState.IsKeyUp(Keys.Space) &&
                        instance.State == SoundState.Playing)
                {
                    instance.Stop();
                }

                #region Rotational Controls

                if (!crashed && !landed)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        rotation -= 0.1f;

                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        rotation += 0.1f;
                    }
                }

                #endregion

                #endregion
            }
            else
            {
                #region Input when Ship IS Crashed

                if (newKeyboardState.IsKeyDown(Keys.Enter) && !gameEnded)
                {
                    position = new Vector2(50, 135);

                    oldPosition = position;
                    newPosition = position;

                    rotation = 0;
                    crashed = false;

                }

                #endregion
            }
        }

        public void RecalculatePosition(GameTime gameTime)
        {
            if (!landed)
            {
                Matrix rotMatrix = Matrix.CreateRotationZ(rotation);

                Vector2 currentDirection = Vector2.Transform(UP, rotMatrix);
                currentDirection *= thrustPower;
                position += currentDirection;

                //Console.WriteLine($"PLAYERSHIP rotation {rotation}__________________________________________________");

                //position += DOWN * velocity;

                //if (thrustPower <= 0)
                //{
                //    gravityTime += gameTime.ElapsedGameTime;

                //    if (gravityTime > TimeSpan.FromSeconds(.1))
                //    {
                //        //To Do: Add in Terminal Velocity                        
                //        gravityTime -= TimeSpan.FromSeconds(.1);
                //        velocity += (gravityConstant / 10);
                //    }
                //}
                //else
                //{
                //    gravityTime = TimeSpan.Zero;

                //    velocity = gravityConstant;
                //}
            }
        }

        #endregion
    }
}
