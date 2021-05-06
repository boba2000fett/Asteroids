using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MyLibrary
{    
    public class Font
    {
        #region Variables

        public static Color color;
        public static float Width = 16f;

        #endregion

        #region Retrieval Methods (GetChar, TextWidth, TextHeight)

        private static LinkedList<Vector2> GetChar(char ch)
        {
            LinkedList<Vector2> g = Characters.GetCharacter(ch);
            
            return g;
        }

        public static float TextWidth(float scale, string text)
        {
            if ((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio / scale) > 10)
            {
                scale = 10;
            }

            LinkedList<Vector2> lineList = new LinkedList<Vector2>();
            LinkedList<Vector2> charLines = null;

            Font.color = color;

            float charWidth = 16.0f * scale;
            float lineWidth = 4.0f * scale;
            float lineHeight = 3.0f * scale;

            float dx = 0.0f;
            float dy = 0.0f;
            float xPos = 0.0f;
            float yPos = 0.0f;

            int i = 0;

            int x = 0;
            int y = 0;

            float xInitial = x;
            float yInitial = y;



            for (i = 0; i < text.Length; i++)
            {
                charLines = Characters.GetCharacter(text.ToCharArray()[i]);

                foreach (Vector2 v in charLines)
                {
                    dx = v.X;
                    dy = v.Y;

                    xPos = x + i * charWidth;
                    yPos = y;

                    lineList.AddLast(new Vector2(
                        xPos + dx * lineWidth,
                        yPos + dy * lineHeight));

                }
            }

            return xPos;
        }

        public static float TextHeight(float scale)
        {
            return 3.0f * scale;
        }

        #endregion

        #region WriteText Methods (WriteText, WriteTextSpecifyWidth)

        public static void WriteText(PrimitiveBatch pb, float x, float y, float scale,
            Color color, string text)
        {
            
            if ((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio / scale) > 10)                
            {
                scale = 10;
            }

            LinkedList<Vector2> lineList = new LinkedList<Vector2>();
            LinkedList<Vector2> charLines = null;

            Font.color = color;
           
            float charWidth = 16.0f * scale;
            float lineWidth = 4.0f * scale;
            float lineHeight = 3.0f * scale;

            float dx = 0.0f;
            float dy = 0.0f;
            float xPos = 0.0f;
            float yPos = 0.0f;
            
            int i = 0;

            float xInitial = x;
            float yInitial = y;


            for (i = 0; i < text.Length; i++)
            {
                charLines = Characters.GetCharacter(text.ToCharArray()[i]);
                
                foreach (Vector2 v in charLines)
                {
                    dx = v.X;
                    dy = v.Y;
                                        
                    xPos = x + i * charWidth;
                    yPos = y;
                    
                    lineList.AddLast(new Vector2(
                        xPos + dx * lineWidth,
                        yPos + dy * lineHeight));

                }
            }

            pb.Begin(PrimitiveType.LineList);

            foreach (Vector2 v in lineList)
            {
                if (v.X < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width &&
                    v.Y < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
                {                    
                    pb.AddVertex(v, color);
                }                
            }

            pb.End();
        }

        public static void WriteTextSpecifyWidth(PrimitiveBatch pb, float x, float y, float scale,
            Color color, string text, float charWidthTemp, float lineWidthTemp)
        {

            if ((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio / scale) > 10)
            {
                scale = 10;
            }

            LinkedList<Vector2> lineList = new LinkedList<Vector2>();
            LinkedList<Vector2> charLines = null;

            Font.color = color;

            float charWidth = charWidthTemp * scale;//16
            float lineWidth = lineWidthTemp * scale;  //4
            float lineHeight = 3.0f * scale;

            float dx = 0.0f;
            float dy = 0.0f;
            float xPos = 0.0f;
            float yPos = 0.0f;

            int i = 0;

            float xInitial = x;
            float yInitial = y;


            for (i = 0; i < text.Length; i++)
            {
                charLines = Characters.GetCharacter(text.ToCharArray()[i]);

                foreach (Vector2 v in charLines)
                {
                    dx = v.X;
                    dy = v.Y;

                    xPos = x + i * charWidth;
                    yPos = y;

                    lineList.AddLast(new Vector2(
                        xPos + dx * lineWidth,
                        yPos + dy * lineHeight));

                }
            }

            pb.Begin(PrimitiveType.LineList);

            foreach (Vector2 v in lineList)
            {
                if (v.X < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width &&
                    v.Y < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
                {
                    pb.AddVertex(v, color);
                }
            }

            pb.End();
        }

        #endregion
    }
}
