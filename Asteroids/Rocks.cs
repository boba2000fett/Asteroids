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
    class Rocks : DrawableGameComponent
    {
        #region VARIABLES 

        #region Components

        private PrimitiveBatch pb;

        TimeSpan elapsedTime;
        TimeSpan gravityTime;

        #endregion

        #region PlayerShip Variables

        LinkedList<Vector2> rocks;

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

        public readonly int typeOfRock;
        public int size;

        #endregion

        public int score;


        #endregion

        #region Constructor
        //r.rotation - (45 * (Math.PI) / 180)), r.size - 1, r.position.X, r.position.Y
        // float rockRotation, int rockSize, float xPosition, float yPosition, int type, Game game  
        //Game game, int type, 
        public Rocks(Game game, float xPosition, float yPosition, float rockRotation, int type, int rockSize) : base(game)
        {
            pb = new PrimitiveBatch(game.GraphicsDevice);
            xInitialPos = xPosition;
            yInitialPos = yPosition;
            typeOfRock = type;
            rotation = rockRotation;
            size = rockSize;
        }

        public Rocks(float rockRotation, int rockSize, float xPosition, float yPosition, int type, Game game) : base(game)
        {
            pb = new PrimitiveBatch(game.GraphicsDevice);
            xInitialPos = xPosition;
            yInitialPos = yPosition;
            typeOfRock = type;
            rotation = rockRotation;
            size = rockSize;
        }


        #endregion

        #region Standard Monogame Methods (Initialize, LoadContent, Update, Draw)

        public override void Initialize()
        {
            position = new Vector2(xInitialPos, yInitialPos);//Note: This and Rotation will be passed in when the object is initalizes.

            oldPosition = position;
            newPosition = position;
            if (size == 3)
            {
                scale = 5f;
                score = 20;
            }
            else if (size == 2)
            {
                scale = 4f;
                score = 10;
            }
            else if (size == 1)
            {
                scale = 3f;
                score = 5;
            }
            center = new Vector2(1, 1);

            //rotation = 0;

            rocks = GetRocks();

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

            //Console.WriteLine($"Rocks: PositionX {position.X} PositionY {position.Y}");

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.TransparentBlack);

            pb.Begin(PrimitiveType.LineList);

            DrawRocks();

            //DrawSquare();

            //DrawRockCollision();

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
            Rock Collision:  l2.StartX (300, 375) l2.EndX (300, 300)
             */
            Line2D.Intersects(new Line2D(238.5262f, 283.2247f, 238.5262f, 303.2247f),
                new Line2D(300, 375, 300, 300));

            #region TESTING
            float[] xTest = new float[4];
            float[] yTest = new float[4];
            GetRectanglePositions(out xTest, out yTest);


            for(int i = 0; i < xTest.Length; i++)
            {
                if (i < 3)
                {
                    pb.AddVertex(new Vector2(xTest[i], yTest[i]), Color.Violet);
                    pb.AddVertex(new Vector2(xTest[i + 1], yTest[i + 1]), Color.Violet);
                }
                else
                {
                    pb.AddVertex(new Vector2(xTest[i], yTest[i]), Color.Violet);
                    pb.AddVertex(new Vector2(xTest[0], yTest[0]), Color.Violet);
                }
                
            }
            #endregion

            pb.End();


            base.Draw(gameTime);
        }

        #endregion

        #region Drawing Rocks Methods (DrawRocks, GetSquare, GetRocks)

        public void DrawRocks()
        {
            LinkedList<Vector2> lineList = GetRocks();
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

        public LinkedList<Vector2> GetRocks()
        {
            LinkedList<Vector2> list = new LinkedList<Vector2>();
            //if(typeOfRock == 1)
            //{
            #region Rock Type #1

            //list.AddLast(new Vector2(1 * scale, 0 * scale));
            //list.AddLast(new Vector2(1 * scale, 1 * scale));

            //list.AddLast(new Vector2(1 * scale, 1 * scale));
            //list.AddLast(new Vector2(1 * scale, 2 * scale));

            //list.AddLast(new Vector2(1 * scale, 2 * scale));
            //list.AddLast(new Vector2(2 * scale, 2 * scale));

            //list.AddLast(new Vector2(2 * scale, 2 * scale));
            //list.AddLast(new Vector2(1 * scale, 3 * scale));

            //list.AddLast(new Vector2(1 * scale, 3 * scale));
            //list.AddLast(new Vector2(0 * scale, 2 * scale));

            //list.AddLast(new Vector2(0 * scale, 2 * scale));
            //list.AddLast(new Vector2(0 * scale, 1 * scale));

            //list.AddLast(new Vector2(0 * scale, 1 * scale));
            //list.AddLast(new Vector2(1 * scale, 0 * scale));

            #endregion

            #region

            list.AddLast(new Vector2(1 * scale, 0 * scale));
            list.AddLast(new Vector2(2 * scale, 0 * scale));

            list.AddLast(new Vector2(2 * scale, 0 * scale));
            list.AddLast(new Vector2(3 * scale, 1 * scale));

            list.AddLast(new Vector2(3 * scale, 1 * scale));            
            list.AddLast(new Vector2(3 * scale, 2 * scale));

            list.AddLast(new Vector2(3 * scale, 2 * scale));
            list.AddLast(new Vector2(2 * scale, 3 * scale));

            list.AddLast(new Vector2(2 * scale, 3 * scale));
            list.AddLast(new Vector2(1 * scale, 3 * scale));

            list.AddLast(new Vector2(1 * scale, 3 * scale));
            list.AddLast(new Vector2(0 * scale, 2 * scale));

            list.AddLast(new Vector2(0 * scale, 2 * scale));
            list.AddLast(new Vector2(0 * scale, 1 * scale));

            list.AddLast(new Vector2(0 * scale, 1 * scale));
            list.AddLast(new Vector2(1 * scale, 0 * scale));


            #endregion

            //}

            #region Rock Type #2 
            #endregion


            return list;
        }

        #endregion

        #region Converstion Methods (GetRocksForCollision, ConvertRocksLine2D, ConvertAngleDegrees)

        public LinkedList<Vector2> GetRocksForCollision()
        {
            LinkedList<Vector2> lineList = GetRocks();

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

        public LinkedList<Line2D> ConvertRocksLine2D()
        {
            LinkedList<Vector2> land = GetRocksForCollision();

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

            //list.AddLast(new Vector2(1 * scale, 3 * scale));
            //list.AddLast(new Vector2(1 * scale, 0 * scale));

            //list.AddLast(new Vector2(2 * scale, 3 * scale));
            //list.AddLast(new Vector2(2 * scale, 0 * scale));
            

            return list;
        }

        public LinkedList<Line2D> RocksSquareCollision()
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

        public void DrawRockCollision()
        {
            LinkedList<Line2D> collision = RocksSquareCollision();
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
        public void GetRectanglePositions(out float[] xCoordinates, out float[] yCoordinates)
        {
            LinkedList<Line2D> rectanglesLine2D = RocksSquareCollision();

            xCoordinates = new float[4];
            yCoordinates = new float[4];
            Line2D[] rectangles = rectanglesLine2D.ToArray();

            for (int i = 0; i < rectangles.Length; i++)
            {
                xCoordinates[i] = rectangles[i].StartX;
                yCoordinates[i] = rectangles[i].StartY;
            }
        }
    }
}
