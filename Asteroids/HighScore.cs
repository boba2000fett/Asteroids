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
    public class HighScore : DrawableGameComponent
    {
        private string[] names =
            {"TJA","TMB","WEH","MRH","JDR","MJJ","MJK","SAD","FUN","TTG" };
        private int[] scores = 
            {1000, 700, 500, 200, 100, 50, 20, 10, 0, 0 };

        private string name;
        public int score;


        private PrimitiveBatch pb;

        TimeSpan elapsedTime;
        TimeSpan gravityTime;

        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;

        List<SoundEffect> soundEffects;
        public bool victorySoundPlaying;


        LinkedList<Vector2> playerShip;

        public Vector2 position;
        private float scale;

        private float uiScale;

        Vector2 center;


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

        public bool firstLanding;


        public bool landed, crashed = false;

        int explosionCounter;

        public bool gameEnded;

        Vector2 UP = new Vector2(0, -1);
        Vector2 DOWN = new Vector2(0, 1);

        Vector2 velocityVector;
        Vector2 oldPosition;
        Vector2 newPosition;

        float angle, velocity, rotation;


        char[] charArray =
            {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q',
                'r','s','t','u','v','w','x','y','z','1','2','3','4','5','6','7','8','9','0'};

        StringBuilder nameString = new StringBuilder();

        int counter;

        public bool active;

        public bool selection;

        public float Rotation
        {
            get
            {
                return rotation;
            }
        }


        //public HighScore()
        //{
            
        //}

        public HighScore(Game game) : base(game)
        {
            pb = new PrimitiveBatch(game.GraphicsDevice);

            //names = new string[10];
            //scores = new int[10];

            //To Do: Maybe initalize everything to default values? So make default names with default scores.

        }

        public override void Initialize()
        {

            uiScale = 5f;
            counter = 0;

        }

        public override void Draw(GameTime gameTime)
        {
            if (active || selection)
            {
                DisplayAngle();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (selection)
            {
                base.Update(gameTime);
                newKeyboardState = Keyboard.GetState();


                if (oldKeyboardState != newKeyboardState)
                {
                    if (newKeyboardState.IsKeyDown(Keys.Up))
                    {
                        counter++;
                        if (counter > charArray.Count() - 1)
                        {
                            counter = 0;
                        }
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Down))
                    {
                        counter--;
                        if (counter < 0)
                        {
                            counter = charArray.Count() - 1;
                        }
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Enter))
                    {
                        if (nameString.Length < 3)
                        {
                            nameString.Append(charArray[counter]);
                        }
                        else
                        {
                            //ShiftScore(nameString.ToString(), score);
                            //selection = false;
                            //active = true;
                        }
                    }
                }


                if (nameString.Length > 2)
                {
                    ShiftScore(nameString.ToString(), score);
                    selection = false;
                    active = true;
                }

                oldKeyboardState = newKeyboardState;

                Console.WriteLine($"counter {counter}");
                Console.WriteLine($"nameString {nameString.ToString()}");
                Console.WriteLine($"Current Element/charArray[counter]: {charArray[counter]}");
                Console.WriteLine();
                Console.WriteLine();
            }
            
        }

        public void DisplayAngle()
        {
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();

            Console.WriteLine(StateManager.GraphicsDevice.Viewport.AspectRatio);

            
            uiScale = 1;
            Color colour = Color.White;
            float px = 10.0f;
            float py = 5 * (Font.TextHeight(uiScale));
            string text = "High Score Screen";
            Font.WriteText(pb, px, py, uiScale, colour, text);

            px = 10.0f;
            py = 15 * (Font.TextHeight(uiScale));
            text = "Name";
            Font.WriteText(pb, px, py, uiScale, colour, text);

            px = 150.0f;
            py = 15 * (Font.TextHeight(uiScale));
            text = "Score";
            Font.WriteText(pb, px, py, uiScale, colour, text);
            //scores.Length

            if (selection)
            {

                #region Selecting Name
                int placeInScoreBoard = ReturnPlace();

                for (int i = 0; i < scores.Length; i++)
                {

                    py = (25 + (i * 10)) * (Font.TextHeight(uiScale));

                    px = 10.0f;
                    if (i == placeInScoreBoard)
                    {
                        if (nameString.Length < 3)
                        {
                            nameString.Append(charArray[counter]);

                            int place = nameString.Length;

                            text = nameString.ToString();
                            Font.WriteText(pb, px, py, uiScale, colour, text);
                            nameString.Remove(place - 1, 1);
                        }
                        else
                        {
                            text = nameString.ToString();
                            Font.WriteText(pb, px, py, uiScale, colour, text);
                        }
                    }
                    else if (i > placeInScoreBoard)
                    {
                        text = $"{names[i - 1]}";
                        Font.WriteText(pb, px, py, uiScale, colour, text);
                    }
                    else
                    {
                        text = $"{names[i]}";
                        Font.WriteText(pb, px, py, uiScale, colour, text);
                    }

                    px = 150.0f;
                    if (i == placeInScoreBoard)
                    {
                        text = $"{score}";
                        Font.WriteText(pb, px, py, uiScale, colour, text);
                    }
                    else if (i > placeInScoreBoard)
                    {
                        text = $"{scores[i - 1]}";
                        Font.WriteText(pb, px, py, uiScale, colour, text);
                    }
                    else
                    {
                        text = $"{scores[i]}";
                        Font.WriteText(pb, px, py, uiScale, colour, text);
                    }
                }

                #endregion

            }
            else if (active)
            {
                #region Only Displaying High Score

                for (int i = 0; i < scores.Length; i++)
                {
                    py = (25 + (i * 10)) * (Font.TextHeight(uiScale));

                    px = 10.0f;
                    text = $"{names[i]}";
                    Font.WriteText(pb, px, py, uiScale, colour, text);

                    px = 150.0f;
                    text = $"{scores[i]}";
                    Font.WriteText(pb, px, py, uiScale, colour, text);
                }

                py = (25 + (10 * 10)) * (Font.TextHeight(uiScale));
                py = 30;
                px = 150.0f;
                text = $"Active";
                Font.WriteText(pb, px, py, uiScale, colour, text);
                

                #endregion
            }



        }

        /*
        Planning for drawing the High Score
        Controls: Press Up and Down to transition between characters
        Press Enter to Validate the Character

        Thinking:
        ----*if (Up Key is Pressed)
            Increment Counter Variable
            If (larger than Max amount of Characters)
                counter = 0
        ----*if(Down Key is Pressed)
            Decrement Counter Variable
                If (less than Max amount of Characters)
                    counter = 0
        ----*if (Enter Key is Pressed)
            Append the current char[cpimter] element to the Player's Name List      
            

        ----*Have Char Arraay which has all the available characters.
        
        When Drawinvg the Score Screen Above:
        In Loop
            If (At Players Curremt New Place In Scorebaord)
                Draw Current Characters in the Array
                Draw the current charpcounter[ tjat the player is current;y se;ectimng
                
            If (No Characters Currently in Placer Array)
                




         */



        public bool CheckScore(int personScore)
        {
            /* Iterate through the two arrays that comprise the HighScore.
             * Mainly, you are going to loop through the scores[] array, and if the personScore is larger than
             * any of the elements inside that one array, then you will call a seperate method that will insert that number into
             * the array and shift everything else down. NOTE: Make sure to do this for both arrays (names[] and scores[]) 
             * Then, return true.
             * If that number is not found, then return false. This means that the player's score did not qualify for a high score.
             * 
             * 
             * 
             */

            for (int i = 0; i < scores.Length; i++)
            {
                if (personScore > scores[i])
                {
                    return true;
                }
            }


            return false;
        }

        public int ReturnPlace()
        {
            for (int i = 0; i < scores.Length; i++)
            {
                if (this.score > scores[i])
                {
                    return i;
                }
            }
            return 0;
        }


        public void ShiftScore(string personName, int personScore)
        {
            int intTemp;
            int i;
            int counter = -1;

            string stringTemp;

            //Do Loop to find where the new personScore replaces a spot at the High Score List (set this equal to counter)
            for (i = 0; i < scores.Length; i++)
            {
                if (personScore > scores[i])
                {
                    counter = i;
                    break;
                }
            }

            for (i = counter; i < scores.Length; i++)
            {
                intTemp = scores[i];
                scores[i] = personScore;
                personScore = intTemp;

                stringTemp = names[i];
                names[i] = personName;
                personName = stringTemp;

            }



        }
    }
}
