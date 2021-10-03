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
    class AttractVisuals : DrawableGameComponent
    {
        private PrimitiveBatch pb;
        float uiScale;

        public AttractVisuals(Game game) : base(game)
        {
            pb = new PrimitiveBatch(game.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            DisplayTitle();            
        }

        public void DisplayTitle()
        {
            Color colour = Color.White;
            string text;
            float px;
            float py;
            float center;

            text = "Asteroids";           
            py = 15 * (Font.TextHeight(uiScale));
            uiScale = 5f;
            center = (float)(
                (StateManager.GraphicsDevice.Viewport.Width / 2) -
                ((Font.TextWidth(uiScale, text)) / 3));
            Font.WriteTextSpecifyWidth(pb, center, py, uiScale, colour, text, 10, 3);
              
            text = "Press Space to Play";
            py = 35 * (Font.TextHeight(uiScale));
            uiScale = 5f;
            center = (float)(
                (StateManager.GraphicsDevice.Viewport.Width / 2) -
                ((Font.TextWidth(uiScale, text)) / 3));           
            Font.WriteTextSpecifyWidth(pb, center, py, uiScale, colour, text, 10, 3);
        }

        public override void Initialize()
        {
            uiScale = 2f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }
    }
}
