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
        private PrimitiveBatch pb;

        TimeSpan elapsedTime;
        TimeSpan gravityTime;

        LinkedList<Vector2> enemyShip;

        public Vector2 position;
        private float scale;

        private float uiScale;

        Vector2 center;

        public bool isThrusting = false;
        public float thrustPower;
        public float thrustPowerIncrement;

        public float MAX_THRUST_POWER = 1f;//Remake this into a cosnt
        
        public bool crashed = false;

        
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

        public int score;

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

        public override void Initialize()
        {
            position = new Vector2(xInitialPos, yInitialPos);//Note: This and Rotation will be passed in when the object is initalizes.

            oldPosition = position;
            newPosition = position;
      
            scale = 2f;
            score = 5;
      
            center = new Vector2(1, 1);

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

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            pb.Begin(PrimitiveType.LineList);

            DrawEnemyShip();

            Line2D.Intersects(new Line2D(238.5262f, 283.2247f, 238.5262f, 303.2247f),
                new Line2D(300, 375, 300, 300));

            pb.End();


            base.Draw(gameTime);
        }

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

            #region Enemy Ship Vectors

            //Outline of Ship
            list.AddLast(new Vector2(3 * scale, 0 * scale));
            list.AddLast(new Vector2(4 * scale, 1 * scale));

            list.AddLast(new Vector2(4 * scale, 1 * scale));
            list.AddLast(new Vector2(4 * scale, 9 * scale));

            list.AddLast(new Vector2(4 * scale, 9 * scale));
            list.AddLast(new Vector2(3 * scale, 10 * scale));

            list.AddLast(new Vector2(3 * scale, 10 * scale));
            list.AddLast(new Vector2(2 * scale, 9 * scale));

            list.AddLast(new Vector2(2 * scale, 9 * scale));
            list.AddLast(new Vector2(2 * scale, 8 * scale));

            list.AddLast(new Vector2(2 * scale, 8 * scale));
            list.AddLast(new Vector2(1 * scale, 6 * scale));

            list.AddLast(new Vector2(1 * scale, 6 * scale));
            list.AddLast(new Vector2(1 * scale, 4 * scale));

            list.AddLast(new Vector2(1 * scale, 4 * scale));
            list.AddLast(new Vector2(2 * scale, 2 * scale));

            list.AddLast(new Vector2(2 * scale, 2 * scale));
            list.AddLast(new Vector2(2 * scale, 1 * scale));

            list.AddLast(new Vector2(2 * scale, 1 * scale));
            list.AddLast(new Vector2(3 * scale, 0 * scale));

            list.AddLast(new Vector2(2 * scale, 8 * scale));
            list.AddLast(new Vector2(2 * scale, 2 * scale));

            //Internal Design
            list.AddLast(new Vector2(2 * scale, 1 * scale));
            list.AddLast(new Vector2(4 * scale, 5 * scale));

            list.AddLast(new Vector2(4 * scale, 5 * scale));
            list.AddLast(new Vector2(2 * scale, 9 * scale));

            list.AddLast(new Vector2(2 * scale, 3 * scale));
            list.AddLast(new Vector2(3 * scale, 5 * scale));

            list.AddLast(new Vector2(3 * scale, 5 * scale));
            list.AddLast(new Vector2(2 * scale, 7 * scale));           
            #endregion

            return list;
        }

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

            list.AddLast(new Vector2(2 * scale, 0 * scale));
            list.AddLast(new Vector2(4 * scale, 0 * scale));

            list.AddLast(new Vector2(4 * scale, 0 * scale));
            list.AddLast(new Vector2(4 * scale, 10 * scale));

            list.AddLast(new Vector2(4 * scale, 10 * scale));
            list.AddLast(new Vector2(2 * scale, 10 * scale));

            list.AddLast(new Vector2(2 * scale, 10 * scale));
            list.AddLast(new Vector2(2 * scale, 0 * scale));
            

            //Box around Cockpit
            list.AddLast(new Vector2(1 * scale, 2 * scale));
            list.AddLast(new Vector2(2 * scale, 2 * scale));

            list.AddLast(new Vector2(2 * scale, 2 * scale));
            list.AddLast(new Vector2(2 * scale, 8 * scale));

            list.AddLast(new Vector2(2 * scale, 8 * scale));
            list.AddLast(new Vector2(1 * scale, 8 * scale));

            list.AddLast(new Vector2(1 * scale, 8 * scale));
            list.AddLast(new Vector2(1 * scale, 2 * scale));

            return list;
        }

        public LinkedList<Line2D> EnemyShipSquareCollision()
        {
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
            Matrix rotMatrix = Matrix.CreateRotationZ(rotation);

            Vector2 currentDirection = Vector2.Transform(UP, rotMatrix);
            currentDirection *= MAX_THRUST_POWER;
            position += currentDirection;
        }

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
            LinkedList<Line2D> rectanglesLine2D = EnemyShipSquareCollision();

            xCoordinates = new float[8];
            yCoordinates = new float[8];
            Line2D[] rectangles = rectanglesLine2D.ToArray();
            
            for (int i = 0; i < rectangles.Length; i++)
            {
                xCoordinates[i] = rectangles[i].StartX;
                yCoordinates[i] = rectangles[i].StartY;
            }
        }
    }
}
