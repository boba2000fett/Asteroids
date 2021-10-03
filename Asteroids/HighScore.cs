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
            {"TJA","TMB","WEH","MRH","JDR","MJJ","MKV","SAD","FUN","TTG" };
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

        public HighScore(Game game) : base(game)
        {
            pb = new PrimitiveBatch(game.GraphicsDevice);
        }

        public override void Initialize()
        {
            uiScale = 5f;
            counter = 0;

            soundEffects = new List<SoundEffect>();
            soundEffects.Add(Game.Content.Load<SoundEffect>("SoundEffects/Enter"));
            soundEffects.Add(Game.Content.Load<SoundEffect>("SoundEffects/Lower"));
            soundEffects.Add(Game.Content.Load<SoundEffect>("SoundEffects/Upper"));
            soundEffects.Add(Game.Content.Load<SoundEffect>("SoundEffects/FinishName"));
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
                        
                        var upper = soundEffects[2].CreateInstance();
                        if (upper.State != SoundState.Playing)
                        {
                            upper.IsLooped = false;
                            upper.Play();
                        }
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Down))
                    {
                        counter--;
                        if (counter < 0)
                        {
                            counter = charArray.Count() - 1;
                        }

                        var lower = soundEffects[1].CreateInstance();
                        if (lower.State != SoundState.Playing)
                        {
                            lower.IsLooped = false;
                            lower.Play();
                        }
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Enter))
                    {
                        if (nameString.Length < 3)
                        {
                            nameString.Append(charArray[counter]);
                            if (nameString.Length == 3)
                            {
                                var finishName = soundEffects[3].CreateInstance();
                                if (finishName.State != SoundState.Playing)
                                {
                                    finishName.IsLooped = false;
                                    finishName.Play();
                                }

                            }
                            else
                            {
                                var enter = soundEffects[0].CreateInstance();
                                if (enter.State != SoundState.Playing)
                                {
                                    enter.IsLooped = false;
                                    enter.Play();
                                }
                            }
                            

                        }
                    }
                }

                if (nameString.Length > 2)
                {
                    ShiftScore(nameString.ToString(), score);
                    selection = false;
                    active = true;
                    nameString.Clear();
                }

                oldKeyboardState = newKeyboardState;
            }            
        }

        public void DisplayAngle()
        {
            Console.WriteLine(StateManager.GraphicsDevice.Viewport.AspectRatio);
            float center;
            
            uiScale = 2;
            Color colour = Color.White;
            
            float px = 10.0f;
            float py = 5 * (Font.TextHeight(uiScale));
            string text = "High Score";
            center = (float)(
                (StateManager.GraphicsDevice.Viewport.Width / 2) -
                ((Font.TextWidth(uiScale, text)) / 2.2f));
            Font.WriteText(pb, center, py, uiScale, colour, text);

            px = 10.0f;
            py = 15 * (Font.TextHeight(uiScale));
            text = "Name";
            center = (float)(
                (StateManager.GraphicsDevice.Viewport.Width / 2) -
                ((Font.TextWidth(uiScale, text)) / 2.2f));
            Font.WriteText(pb, center + 200, py, uiScale, colour, text);

            px = 150.0f;
            py = 15 * (Font.TextHeight(uiScale));
            text = "Score";
            center = (float)(
                (StateManager.GraphicsDevice.Viewport.Width / 2) -
                ((Font.TextWidth(uiScale, text)) / 2.2f));
            Font.WriteText(pb, center - 200, py, uiScale, colour, text);
            
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
                            center = (float)(
                                (StateManager.GraphicsDevice.Viewport.Width / 2) -
                                ((Font.TextWidth(uiScale, text)) / 2.2f));
                            Font.WriteText(pb, center - 200, py, uiScale, colour, text);

                            nameString.Remove(place - 1, 1);
                        }
                        else
                        {
                            text = nameString.ToString();
                            center = (float)(
                                (StateManager.GraphicsDevice.Viewport.Width / 2) -
                                ((Font.TextWidth(uiScale, text)) / 2.2f));
                            Font.WriteText(pb, center - 200, py, uiScale, colour, text);
                        }
                    }
                    else if (i > placeInScoreBoard)
                    {
                        text = $"{names[i - 1]}";
                        center = (float)(
                            (StateManager.GraphicsDevice.Viewport.Width / 2) -
                            ((Font.TextWidth(uiScale, text)) / 2.2f));
                        Font.WriteText(pb, center - 200, py, uiScale, colour, text);
                    }
                    else
                    {
                        text = $"{names[i]}";
                        center = (float)(
                            (StateManager.GraphicsDevice.Viewport.Width / 2) -
                            ((Font.TextWidth(uiScale, text)) / 2.2f));
                        Font.WriteText(pb, center - 200, py, uiScale, colour, text);
                    }

                    px = 150.0f;
                    if (i == placeInScoreBoard)
                    {
                        text = $"{score}";
                        center = (float)(
                            (StateManager.GraphicsDevice.Viewport.Width / 2) -
                            ((Font.TextWidth(uiScale, text)) / 2.2f));
                        Font.WriteText(pb, center + 200, py, uiScale, colour, text);
                    }
                    else if (i > placeInScoreBoard)
                    {
                        text = $"{scores[i - 1]}";
                        center = (float)(
                            (StateManager.GraphicsDevice.Viewport.Width / 2) -
                            ((Font.TextWidth(uiScale, text)) / 2.2f));
                        Font.WriteText(pb, center + 200, py, uiScale, colour, text);
                    }
                    else
                    {
                        text = $"{scores[i]}";
                        center = (float)(
                            (StateManager.GraphicsDevice.Viewport.Width / 2) -
                            ((Font.TextWidth(uiScale, text)) / 2.2f));
                        Font.WriteText(pb, center + 200, py, uiScale, colour, text);
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
                    center = (float)(
                            (StateManager.GraphicsDevice.Viewport.Width / 2) -
                            ((Font.TextWidth(uiScale, text)) / 2.2f));
                    Font.WriteText(pb, center - 200, py, uiScale, colour, text);


                    px = 150.0f;
                    text = $"{scores[i]}";
                    center = (float)(
                            (StateManager.GraphicsDevice.Viewport.Width / 2) -
                            ((Font.TextWidth(uiScale, text)) / 2.2f));
                    Font.WriteText(pb, center + 200, py, uiScale, colour, text);
                }

                #endregion
            }
        }

        public bool CheckScore(int personScore)
        {         
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
