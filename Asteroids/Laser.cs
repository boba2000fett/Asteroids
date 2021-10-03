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
    class Laser : DrawableGameComponent
    {        
        private PrimitiveBatch pb;

        TimeSpan elapsedTime;
        TimeSpan gravityTime;

        LinkedList<Vector2> laser;

        private Vector2 position;
        private float scale;

        private float uiScale;

        Vector2 center;        
        
        public bool isThrusting = false;
        public float thrustPower;
        public float thrustPowerIncrement;

        const float MAX_THRUST_POWER = 7.0f;
        
        public bool crashed = false;
        
        Vector2 UP = new Vector2(0, -1);
        Vector2 DOWN = new Vector2(0, 1);

        Vector2 velocityVector;
        Vector2 oldPosition;
        Vector2 newPosition;

        float angle, velocity, rotation;

        float xInitialPos;
        float yInitialPos;

        public readonly bool playerLaser;

        public Laser(Game game, float xPosition, float yPosition, float trajectory, bool type) : base(game)
        {
            pb = new PrimitiveBatch(game.GraphicsDevice);
            xInitialPos = xPosition;
            yInitialPos = yPosition;
            playerLaser = type;
            rotation = trajectory;
        }
        
        public override void Initialize()
        {          
            position = new Vector2(xInitialPos, yInitialPos);

            oldPosition = position;
            newPosition = position;

            scale = 2f;

            center = new Vector2(1, 1);

            laser = GetLaser();

            elapsedTime = TimeSpan.Zero;
            gravityTime = TimeSpan.Zero;
            
            thrustPower = 0;
            thrustPowerIncrement = 0.1f;            

            crashed = false;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            VelocityCalculation();

            CheckRotation();

            RecalculatePosition(gameTime);

            CheckPosition();

            elapsedTime += gameTime.ElapsedGameTime;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {           
            pb.Begin(PrimitiveType.LineList);

            DrawLaser();
         
            pb.End();

            base.Draw(gameTime);
        }

        public void DrawLaserCollision()
        {
            LinkedList<Line2D> collision = ConvertLaserLine2D();
            foreach (Line2D line in collision)
            {
                pb.AddVertex(new Vector2(line.StartX, line.StartY), Color.Coral);
                pb.AddVertex(new Vector2(line.EndX, line.EndY), Color.Coral);
            }
        }

        public void DrawLaser()
        {
            LinkedList<Vector2> lineList = GetLaser();
            Color color = new Color();

            if (playerLaser)
            {
                color = Color.White;
            }
            else
            {
                color = Color.Red;
            }
            
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


        public LinkedList<Vector2> GetLaser()
        {
            LinkedList<Vector2> list = new LinkedList<Vector2>();

            list.AddLast(new Vector2(1 * scale, 0 * scale));
            list.AddLast(new Vector2(1 * scale, 5 * scale));
           
            return list;
        }

        public LinkedList<Vector2> GetLaserForCollision()
        {
            LinkedList<Vector2> lineList = GetLaser();

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
            return ll;
        }

        public LinkedList<Line2D> ConvertLaserLine2D()
        {
            LinkedList<Vector2> land = GetLaserForCollision();

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

        public bool CheckLaser()
        {            
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
