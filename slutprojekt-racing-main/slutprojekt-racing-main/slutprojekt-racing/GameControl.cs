using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace slutprojekt_racing
{
    
    static class GameControl
    {

        public static Texture2D startButton2D;
        public static Rectangle startButtonHitbox;

        public static Song marioCircuit;

        public static SpriteFont countDownFont;

        public static bool gameState = false;
        public static int inCountDown = 0;

        public static void LoadMenuContent(ContentManager Content)
        {

            startButton2D = Content.Load<Texture2D>("Sprites/startButton");

            countDownFont = Content.Load<SpriteFont>("SpriteFonts/Countdown");

        }

        //Update code to control button clicks in menu.
        public static void MenuUpdate(ContentManager Content, MouseState mouseState, Player player, Board board, Ghost ghost)
        {

            startButtonHitbox = new Rectangle((int)((Game1.window.X - startButton2D.Width) / 2), (int)((Game1.window.Y - startButton2D.Height) / 2), startButton2D.Width, startButton2D.Height);

            if (mouseState.LeftButton == ButtonState.Pressed)
            {

                Point mousePos = new Point(mouseState.X, mouseState.Y);
                if (startButtonHitbox.Contains(mousePos)) StartGame(Content, player, board, ghost);

            }

        }

        //Code to load all content, start game and exit menu.
        public static void StartGame(ContentManager Content, Player player, Board board, Ghost ghost)
        {

            marioCircuit = Content.Load<Song>("Music/ScruffyMarioCircuitRemix");
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(marioCircuit);
            MediaPlayer.IsRepeating = true;

            player.LoadContent(Content);
            board.LoadContent(Content);
            ghost.LoadContent(player);

            ghost.recStatus = true;

            gameState = true;

            ResetLaps(player, board, ghost);

        }

        public static void GameUpdate(GameTime gameTime, Player player, Board board, Ghost ghost)
        {

            LapsRunner(gameTime, player, board, ghost);

        }


        //This method and these variables count laps and check for player passing checkpoints to prevent cheating.
        static int lapCounter = 0;
        static bool checkpoint2 = false;
        static bool tempChecker = false;
        public static void LapsRunner(GameTime gameTime, Player player, Board board, Ghost ghost)
        {

            if (board.checkpoint1.hitbox.Intersects(player.car.hitbox) && checkpoint2 == true && tempChecker)
            {

                lapCounter++;
                checkpoint2 = false;

                tempChecker = false;

            }
            if (!board.checkpoint1.hitbox.Intersects(player.car.hitbox))
            {

                tempChecker = true;

            }

            if (board.checkpoint2.hitbox.Intersects(player.car.hitbox)) checkpoint2 = true;

            if (lapCounter == 3)
            {

                ResetLaps(player, board, ghost);

            }

        }

        //Resets lap counter and prepares for a new countdown.
        public static void ResetLaps(Player player, Board board, Ghost ghost)
        {

            MediaPlayer.Stop();

            lapCounter = 0;
            checkpoint2 = false;
            tempChecker = false;

            ghost.playback.Clear();
            ghost.playback.AddRange(ghost.recording);
            ghost.playbackCounter = 0;
            ghost.recording.Clear();
            ghost.recStatus = false;
            ghost.pbStatus = false;
            player.Reset();

            if (gameState) inCountDown = 1;

        }

    }
}
