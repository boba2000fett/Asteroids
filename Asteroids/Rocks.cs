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

        private Vector2 position;
        private float scale;

        private float uiScale;

        Vector2 center;

        #endregion

        #region Thrust Variables


        public bool isThrusting = false;
        public float thrustPower;
        public float thrustPowerIncrement;

        const float MAX_THRUST_POWER = 10.0f;
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

        float angle, velocity, rotation;

        float xInitialPos;
        float yInitialPos;

        public readonly bool typeOfRocks;

        #endregion

        #endregion

        #region Constructor
        public Rocks(Game game, float xPosition, float yPosition, float trajectory, bool type) : base(game)
        {
            pb = new PrimitiveBatch(game.GraphicsDevice);
            xInitialPos = xPosition;
            yInitialPos = yPosition;
            typeOfRocks = type;
            rotation = trajectory;
        }

        #endregion

        #region Standard Monogame Methods (Initialize, LoadContent, Update, Draw)

        public override void Initialize()
        {
            position = new Vector2(xInitialPos, yInitialPos);//Note: This and Rotation will be passed in when the object is initalizes.

            oldPosition = position;
            newPosition = position;

            scale = 1.5f;

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

            Console.WriteLine($"Rocks: PositionX {position.X} PositionY {position.Y}");

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.TransparentBlack);

            pb.Begin(PrimitiveType.LineList);

            DrawRocks();

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


        public LinkedList<Vector2> GetRocks()
        {
            LinkedList<Vector2> list = new LinkedList<Vector2>();

            list.AddLast(new Vector2(1 * scale, 0 * scale));
            list.AddLast(new Vector2(1 * scale, 5 * scale));

            //list.AddLast(new Vector2(0 * scale, 0 * scale));
            //list.AddLast(new Vector2(10 * scale, 0 * scale));

            //list.AddLast(new Vector2(10 * scale, 0 * scale));
            //list.AddLast(new Vector2(10 * scale, 10 * scale));

            //list.AddLast(new Vector2(10 * scale, 10 * scale));
            //list.AddLast(new Vector2(0 * scale, 10 * scale));

            //list.AddLast(new Vector2(0 * scale, 10 * scale));
            //list.AddLast(new Vector2(0 * scale, 0 * scale));



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

    }
}
