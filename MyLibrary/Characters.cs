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
    class Characters
    {
        static LinkedList<Vector2> characterDraw;
        
        static public LinkedList<Vector2> GetCharacter(char character)
        {
            characterDraw = new LinkedList<Vector2>();
            switch (character)
            {                
                #region ALPHABET CHARACTERS

                #region A-E
                case 'A':
                case 'a':

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 1));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(1, 0));

                    characterDraw.AddLast(new Vector2(1, 0));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 1));
                    
                    return characterDraw;
                case 'B':
                case 'b':
                    
                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(0, 1));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(0, 2));
                    
                    return characterDraw;
                case 'C':
                case 'c':

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    return characterDraw;
                case 'D':
                case 'd':

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(1, 0));

                    characterDraw.AddLast(new Vector2(1, 0));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(0, 2));

                    return characterDraw;
                case 'E':
                case 'e':

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(1, 1));

                    return characterDraw;
                #endregion

                #region F-J
                case 'F':
                case 'f':
                    
                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(1, 1));

                    return characterDraw;
                case 'G':
                case 'g':

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(1, 1));

                    return characterDraw;
                case 'H':
                case 'h':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 1));

                    return characterDraw;
                case 'I':
                case 'i':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(1, 0));
                    characterDraw.AddLast(new Vector2(1, 2));

                    return characterDraw;
                case 'J':
                case 'j':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(1, 0));
                    characterDraw.AddLast(new Vector2(1, 2));

                    characterDraw.AddLast(new Vector2(1, 2));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 1));

                    return characterDraw;

                #endregion

                #region K-O
                case 'K':
                case 'k':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;
                case 'L':
                case 'l':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;
                case 'M':
                case 'm':
                    
                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(1, 1));

                    characterDraw.AddLast(new Vector2(1, 1));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;
                case 'N':
                case 'n':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;
                case 'O':
                case 'o':

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;

                #endregion

                #region P-T

                case 'P':
                case 'p':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 1));

                    return characterDraw;
                case 'Q':
                case 'q':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(1, 2));

                    characterDraw.AddLast(new Vector2(1, 2));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(1, 1));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;
                case 'R':
                case 'r':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;
                case 'S':
                case 's':

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(0, 1));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    return characterDraw;
                case 'T':
                case 't':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(1, 0));
                    characterDraw.AddLast(new Vector2(1, 2));

                    return characterDraw;

                #endregion

                #region U-Z

                case 'U':
                case 'u':

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;
                case 'V':
                case 'v':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(1, 2));

                    characterDraw.AddLast(new Vector2(1, 2));
                    characterDraw.AddLast(new Vector2(2, 0));

                    return characterDraw;
                case 'W':
                case 'w':

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(1, 1));

                    characterDraw.AddLast(new Vector2(1, 1));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;
                case 'X':
                case 'x':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(0, 2));

                    return characterDraw;
                case 'Y':
                case 'y':

                    characterDraw.AddLast(new Vector2(1, 1));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(1, 1));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(1, 1));
                    characterDraw.AddLast(new Vector2(1, 2));

                    return characterDraw;
                case 'Z':
                case 'z':

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;
                #endregion

                #endregion

                #region NUMERICAL CHARACTERS / PUNCTUATION / SPECIAL CHARACTERS
                
                #region 1-6

                case '1':

                    characterDraw.AddLast(new Vector2(1, 0));
                    characterDraw.AddLast(new Vector2(1, 2));
                    
                    return characterDraw;
                case '2':

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 1));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(0, 1));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    return characterDraw;
                case '3':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 1));

                    return characterDraw;
                case '4':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(0, 1));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;
                case '5':

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(0, 1));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    return characterDraw;
                case '6':

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(0, 1));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    return characterDraw;
                #endregion

                #region 7-0, '.', '>', '<'
                case '7':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;
                case '8':

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 1));

                    return characterDraw;
                case '9':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(0, 1));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    return characterDraw;
                case '0':

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(0, 2));
                    characterDraw.AddLast(new Vector2(0, 0));

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 0));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(0, 2));

                    return characterDraw;


                #endregion


                #region

                case '.':
                    characterDraw.AddLast(new Vector2(1, 1));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(2, 2));
                    characterDraw.AddLast(new Vector2(1, 2));

                    characterDraw.AddLast(new Vector2(1, 2));
                    characterDraw.AddLast(new Vector2(1, 1));
                    return characterDraw;
                case '-':

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 1));

                    return characterDraw;

                case '>':

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(0, 2));

                    return characterDraw;

                case '<':

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(0, 1));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 2));

                    return characterDraw;

                case '{'://This is an arrow pointing Left

                    characterDraw.AddLast(new Vector2(2, 0));
                    characterDraw.AddLast(new Vector2(0, 1));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(2, 2));

                    characterDraw.AddLast(new Vector2(0, 1));
                    characterDraw.AddLast(new Vector2(4, 1));

                    return characterDraw;

                case '}': //This is an arrow pointing Right

                    characterDraw.AddLast(new Vector2(0, 0));
                    characterDraw.AddLast(new Vector2(2, 1));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(0, 2));

                    characterDraw.AddLast(new Vector2(2, 1));
                    characterDraw.AddLast(new Vector2(-2, 1));

                    return characterDraw;

                #endregion

                #endregion

                #region DEFAULT/ERRORHANDLING VECTOR
                default:
                    return characterDraw;
                    #endregion
            }
        }
    }
}
