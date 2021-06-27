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
    class EnemyShip : DrawableGameComponent
    {

        #region VARIABLES 

        #region Components

        private PrimitiveBatch pb;

        TimeSpan elapsedTime;
        TimeSpan gravityTime;

        #endregion

        #region PlayerShip Variables

        LinkedList<Vector2> enemyShip;

        public Vector2 position;
        private float scale;

        private float uiScale;

        Vector2 center;

        #endregion

        #region Thrust Variables


        public bool isThrusting = false;
        public float thrustPower;
        public float thrustPowerIncrement;

        public float MAX_THRUST_POWER = 1f;//Remake this into a cosnt
        #endregion

        #region Game State Variables


        public bool crashed = false;

        #endregion 

        #region Velocity Variables

        Vector2 UP = new Vector2(0, -1);
        Vector2 DOWN = new Vector2(0, 1);

        Vector2 velocityVector;
        Vector2 oldPosition;
        Vector2 newPosition;

        public float angle, velocity, rotation;

        float xInitialPos;
        float yInitialPos;

        public readonly int typeOfEnemyShip;
        public int size;

        #endregion

        public int score;


        #endregion

        #region Constructor
        //r.rotation - (45 * (Math.PI) / 180)), r.size - 1, r.position.X, r.position.Y
        // float enemyShipRotation, int enemyShipSize, float xPosition, float yPosition, int type, Game game  
        //Game game, int type, 
        public EnemyShip(Game game, float xPosition, float yPosition, float enemyShipRotation, int type, int enemyShipSize) : base(game)
        {
            pb = new PrimitiveBatch(game.GraphicsDevice);
            xInitialPos = xPosition;
            yInitialPos = yPosition;
            typeOfEnemyShip = type;
            rotation = (float)Math.PI / 2;
            size = enemyShipSize;
        }

        public EnemyShip(float enemyShipRotation, int enemyShipSize, float xPosition, float yPosition, int type, Game game) : base(game)
        {
            pb = new PrimitiveBatch(game.GraphicsDevice);
            xInitialPos = xPosition;
            yInitialPos = yPosition;
            typeOfEnemyShip = type;
            rotation = (float)Math.PI / 2;
            size = enemyShipSize;
        }


        #endregion

        #region Standard Monogame Methods (Initialize, LoadContent, Update, Draw)

        public override void Initialize()
        {
            position = new Vector2(xInitialPos, yInitialPos);//Note: This and Rotation will be passed in when the object is initalizes.

            oldPosition = position;
            newPosition = position;
      
            scale = 2f;
            score = 5;
      
            center = new Vector2(1, 1);

            //rotation = 0;

            enemyShip = GetEnemyShip();

            elapsedTime = TimeSpan.Zero;
            gravityTime = TimeSpan.Zero;

            thrustPower = 0;
            thrustPowerIncrement = 0.1f;

            crashed = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            VelocityCalculation();

            CheckRotation();

            RecalculatePosition(gameTime);

            CheckPosition();

            //Console.WriteLine($"EnemyShip: PositionX {position.X} PositionY {position.Y}");

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.TransparentBlack);

            pb.Begin(PrimitiveType.LineList);

            DrawEnemyShip();

            //DrawSquare();

            //DrawEnemyShipCollision();

            //pb.AddLine(new Vector2(center.X, center.Y - 10), new Vector2(center.X, center.Y + 10), Color.Violet, 5);
            //pb.AddLine(new Vector2(center.X - 10, center.Y), new Vector2(center.X + 10, center.Y), Color.Violet, 5);


            ////pb.AddVertex(new Vector2(position.X, position.Y - 10), Color.Violet);
            ////pb.AddVertex(new Vector2(position.X, position.Y + 10), Color.Violet);

            ////pb.AddVertex(new Vector2(position.X - 10, position.Y), Color.Violet);
            ////pb.AddVertex(new Vector2(position.X + 10, position.Y), Color.Violet);

            //pb.AddVertex(new Vector2(242.1367f, 275.431f), Color.Violet);
            //pb.AddVertex(new Vector2(242.1367f, 295.431f), Color.Violet);

            //pb.AddVertex(new Vector2(300, 375), Color.Violet);
            //pb.AddVertex(new Vector2(300, 300), Color.Violet);

            /*
            Laser Collision: l1.StartX (242.1367, 275.431) l1.EndX (242.1367, 295.431)
            EnemyShip Collision:  l2.StartX (300, 375) l2.EndX (300, 300)
             */
            Line2D.Intersects(new Line2D(238.5262f, 283.2247f, 238.5262f, 303.2247f),
                new Line2D(300, 375, 300, 300));



            pb.End();


            base.Draw(gameTime);
        }

        #endregion

        #region Drawing EnemyShip Methods (DrawEnemyShip, GetSquare, GetEnemyShip)

        public void DrawEnemyShip()
        {
            LinkedList<Vector2> lineList = GetEnemyShip();
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

        public void DrawSquare()
        {
            LinkedList<Vector2> lineList = GetSquare();
            Color color = new Color();

            color = Color.Red;

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

        public LinkedList<Vector2> GetEnemyShip()
        {
            LinkedList<Vector2> list = new LinkedList<Vector2>();


            #region CRASHED Lunar Lander

            //Top Of Ship
            list.AddLast(new Vector2(1 * scale  , 0 * scale  ));
            list.AddLast(new Vector2(13 * scale  , 0 * scale  ));

            list.AddLast(new Vector2(13 * scale  , 0 * scale  ));
            list.AddLast(new Vector2(13 * scale  , 9 * scale  ));

            list.AddLast(new Vector2(13 * scale  , 9 * scale  ));
            list.AddLast(new Vector2(1 * scale  , 9 * scale  ));

            list.AddLast(new Vector2(1 * scale  , 9 * scale  ));
            list.AddLast(new Vector2(1 * scale  , 0 * scale  ));

            list.AddLast(new Vector2(1 * scale  , 0 * scale  ));
            list.AddLast(new Vector2(7 * scale  , 9 * scale  ));

            list.AddLast(new Vector2(7 * scale  , 9 * scale  ));
            list.AddLast(new Vector2(13 * scale  , 0 * scale  ));

            list.AddLast(new Vector2(1 * scale  , 9 * scale  ));
            list.AddLast(new Vector2(7 * scale  , 0 * scale  ));

            list.AddLast(new Vector2(7 * scale  , 0 * scale  ));
            list.AddLast(new Vector2(13 * scale  , 9 * scale  ));

            //Middle Portion
            //////list.AddLast(new Vector2(2 * scale  , 9 * scale  ));
            //////list.AddLast(new Vector2(12 * scale  , 9 * scale  ));

            //////list.AddLast(new Vector2(12 * scale  , 9 * scale  ));
            //////list.AddLast(new Vector2(12 * scale  , 11 * scale  ));

            //////list.AddLast(new Vector2(12 * scale  , 11 * scale  ));
            //////list.AddLast(new Vector2(2 * scale  , 11 * scale  ));

            //////list.AddLast(new Vector2(2 * scale  , 11 * scale  ));
            //////list.AddLast(new Vector2(2 * scale  , 9 * scale  ));


            //Left Thruster
            list.AddLast(new Vector2(0 * scale  , 13 * scale  ));
            list.AddLast(new Vector2(0 * scale  , 12 * scale  ));

            list.AddLast(new Vector2(0 * scale  , 12 * scale  ));
            list.AddLast(new Vector2(1 * scale  , 11 * scale  ));

            list.AddLast(new Vector2(1 * scale  , 11 * scale  ));
            list.AddLast(new Vector2(3 * scale  , 11 * scale  ));

            list.AddLast(new Vector2(3 * scale  , 11 * scale  ));
            list.AddLast(new Vector2(4 * scale  , 12 * scale  ));

            list.AddLast(new Vector2(4 * scale  , 12 * scale  ));
            list.AddLast(new Vector2(4 * scale  , 13 * scale  ));

            list.AddLast(new Vector2(4 * scale  , 13 * scale  ));
            list.AddLast(new Vector2(0 * scale  , 13 * scale  ));

            list.AddLast(new Vector2(1 * scale  , 11 * scale  ));
            list.AddLast(new Vector2(1 * scale  , 13 * scale  ));

            list.AddLast(new Vector2(3 * scale  , 11 * scale  ));
            list.AddLast(new Vector2(3 * scale  , 13 * scale  ));

            //Right Thruster
            list.AddLast(new Vector2(10 * scale  , 13 * scale  ));
            list.AddLast(new Vector2(10 * scale  , 12 * scale  ));

            list.AddLast(new Vector2(10 * scale  , 12 * scale  ));
            list.AddLast(new Vector2(11 * scale  , 11 * scale  ));

            list.AddLast(new Vector2(11 * scale  , 11 * scale  ));
            list.AddLast(new Vector2(13 * scale  , 11 * scale  ));

            list.AddLast(new Vector2(13 * scale  , 11 * scale  ));
            list.AddLast(new Vector2(14 * scale  , 12 * scale  ));

            list.AddLast(new Vector2(14 * scale  , 12 * scale  ));
            list.AddLast(new Vector2(14 * scale  , 13 * scale  ));

            list.AddLast(new Vector2(14 * scale  , 13 * scale  ));
            list.AddLast(new Vector2(10 * scale  , 13 * scale  ));

            list.AddLast(new Vector2(11 * scale  , 11 * scale  ));
            list.AddLast(new Vector2(11 * scale  , 13 * scale  ));

            list.AddLast(new Vector2(13 * scale  , 11 * scale  ));
            list.AddLast(new Vector2(13 * scale  , 13 * scale  ));

            #endregion




            return list;
        }

        #endregion

        #region Converstion Methods (GetEnemyShipForCollision, ConvertEnemyShipLine2D, ConvertAngleDegrees)

        public LinkedList<Vector2> GetEnemyShipForCollision()
        {
            LinkedList<Vector2> lineList = GetEnemyShip();

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

        public LinkedList<Line2D> ConvertEnemyShipLine2D()
        {
            LinkedList<Vector2> land = GetEnemyShipForCollision();

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

        private LinkedList<Vector2> GetSquare()
        {
            LinkedList<Vector2> list = new LinkedList<Vector2>();

            list.AddLast(new Vector2(0 * scale, 0 * scale));
            list.AddLast(new Vector2(3 * scale, 0 * scale));

            list.AddLast(new Vector2(3 * scale, 0 * scale));
            list.AddLast(new Vector2(3 * scale, 3 * scale));

            list.AddLast(new Vector2(3 * scale, 3 * scale));
            list.AddLast(new Vector2(0 * scale, 3 * scale));

            list.AddLast(new Vector2(0 * scale, 3 * scale));
            list.AddLast(new Vector2(0 * scale, 0 * scale));

            list.AddLast(new Vector2(1 * scale, 3 * scale));
            list.AddLast(new Vector2(1 * scale, 0 * scale));

            list.AddLast(new Vector2(2 * scale, 3 * scale));
            list.AddLast(new Vector2(2 * scale, 0 * scale));


            return list;
        }

        public LinkedList<Line2D> EnemyShipSquareCollision()
        {
            //Vector2 center = new Vector2(.5f * scale, .5f * scale);
            LinkedList<Vector2> lineList = GetSquare();

            LinkedList<Vector2> ll = new LinkedList<Vector2>();

            foreach (Vector2 v in lineList)
            {
                float Xrotated = center.X + (v.X - center.X) *
                  (float)Math.Cos(rotation) - (v.Y - center.Y) *
                  (float)Math.Sin(rotation);

                float Yrotated = center.Y + (v.X - center.X) *
                  (float)Math.Sin(rotation) + (v.Y - center.Y) *
                  (float)Math.Cos(rotation);

                ll.AddLast(new Vector2(position.X + (Xrotated * scale),
                  position.Y + (Yrotated * scale)));
            }


            LinkedList<Line2D> lines = new LinkedList<Line2D>();

            int count = 0;
            float var1 = 0.0f;
            float var2 = 0.0f;
            float var3 = 0.0f;
            float var4 = 0.0f;

            foreach (Vector2 l in ll)
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

        public void DrawEnemyShipCollision()
        {
            LinkedList<Line2D> collision = EnemyShipSquareCollision();
            foreach (Line2D line in collision)
            {
                pb.AddVertex(new Vector2(line.StartX, line.StartY), Color.Coral);
                pb.AddVertex(new Vector2(line.EndX, line.EndY), Color.Coral);


            }
        }

        public double ConvertAngleDegrees()
        {
            return rotation * (180 / Math.PI);
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

        public void VelocityCalculation()
        {
            oldPosition = newPosition;
            newPosition = position;

            velocityVector = new Vector2(
                 (newPosition.X - oldPosition.X),
                 (newPosition.Y - oldPosition.Y));
        }



        public void RecalculatePosition(GameTime gameTime)
        {
            //if (!crashed)
            //{
            Matrix rotMatrix = Matrix.CreateRotationZ(rotation);

            Vector2 currentDirection = Vector2.Transform(UP, rotMatrix);
            currentDirection *= MAX_THRUST_POWER;
            position += currentDirection;

            //}
        }

        #endregion


        public void CheckPosition()
        {
            if (position.X > StateManager.GraphicsDevice.Viewport.Width)
            {
                position.X = 0;
            }
            if (position.X < 0)
            {
                position.X = StateManager.GraphicsDevice.Viewport.Width;
            }
            if (position.Y > StateManager.GraphicsDevice.Viewport.Height)
            {
                position.Y = 0;
            }
            if (position.Y < 0)
            {
                position.Y = StateManager.GraphicsDevice.Viewport.Height;
            }
            
        }


    }
}
