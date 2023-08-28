using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Threading;

namespace slutprojekt_racing
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Defines window size in pixel dimensions.
        public static Vector2 window = new Vector2(1920, 1080);

        //Player controlled car.
        Player player = new Player();
        
        //Board, keeps track of items on track and walls.
        Board board = new Board();

        //Ghost, records your movement and plays it back.
        Ghost ghost = new Ghost();
        
        //Old keys used to prevent spamming a function by holding a key.
        Keys[] oldKeys = new Keys[0];


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //Sets correct window size and high definition graphics.
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = (int)window.X;
            _graphics.PreferredBackBufferHeight = (int)window.Y;

            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {

            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GameControl.LoadMenuContent(Content);

            // TODO: use this.Content to load your game content here
        }

        //This variable is only used to prevent a tiny bit of flickering after clicking start game.
        bool firstGameUpdate;
        protected override void Update(GameTime gameTime)
        {
            //End game with escape.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                GameControl.gameState = false;
            if (Keyboard.GetState().IsKeyDown(Keys.F11))
                _graphics.ToggleFullScreen();

            //Update code that controls the game.
            if (GameControl.gameState)
            {

                KeyboardState keyboardState = Keyboard.GetState();
                Keys[] pressedKeys = keyboardState.GetPressedKeys();

                //Updates objects.
                board.Update(player);
                player.Update(pressedKeys, board);
                ghost.Update(player, board, pressedKeys, oldKeys);

                KeyboardState oldState = Keyboard.GetState();
                oldKeys = oldState.GetPressedKeys();

                firstGameUpdate = true;

                GameControl.GameUpdate(gameTime, player, board, ghost);

            }
            //Reset code for while in the menu.
            else
            {
                firstGameUpdate = false;

                MediaPlayer.Stop();

                player.Reset();

                board.Reset();

                ghost.Reset();

                GameControl.ResetLaps(player, board, ghost);

                MouseState mouseState = Mouse.GetState();

                GameControl.MenuUpdate(Content, mouseState, player, board, ghost);
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            //Draw code for game.
            if (GameControl.gameState && firstGameUpdate)
            {

                _spriteBatch.Draw(board.track2D, board.absPos, Color.White);

                _spriteBatch.Draw(player.car.car2D, ghost.absPos, null, new Color(Color.White, 0.2f), (float)-ghost.rotation, ghost.origin, 1f, SpriteEffects.None, 0);

                _spriteBatch.Draw(player.car.car2D, player.absPos, null, Color.White, (float)-player.rotation, player.origin, 1f, SpriteEffects.None, 0);

                //This code controls the countdown after every three laps. Records movements for ghost etc.
                //Should have made a countdown and put it in there instead.
                if (GameControl.inCountDown >= 1)
                {

                    GameControl.inCountDown++;

                    if (GameControl.inCountDown < 60)
                    {
                        _spriteBatch.DrawString(GameControl.countDownFont, "3", new Vector2(window.X / 2, window.Y / 2), Color.White);
                    }
                    else if (GameControl.inCountDown < 120)
                    {
                        _spriteBatch.DrawString(GameControl.countDownFont, "2", new Vector2(window.X / 2, window.Y / 2), Color.White);
                    }
                    else if (GameControl.inCountDown < 180)
                    {
                        _spriteBatch.DrawString(GameControl.countDownFont, "1", new Vector2(window.X / 2, window.Y / 2), Color.White);
                    }
                    else
                    {
                        GameControl.inCountDown = 0;
                        MediaPlayer.Play(GameControl.marioCircuit);
                        ghost.pbStatus = true;
                        ghost.recStatus = true;
                    }

                }


            }
            //Draw code for in menu.
            else
            {
                
                _spriteBatch.Draw(GameControl.startButton2D, new Vector2((window.X - GameControl.startButton2D.Width)/2, (window.Y - GameControl.startButton2D.Height)/2), Color.White);

            }

            //_spriteBatch.Draw(hitbox, player.absPos - new Vector2(player.car.hitboxSize / 2, player.car.hitboxSize / 2), Color.White);

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);

        }
    }
}
