using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using FmodForFoxes;
using System.Runtime.InteropServices;
using FmodForFoxes.Studio;

namespace BrickBreaker
{
    
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _bgTexture;
        
        //Creating game objects
        private Paddle paddle;
        private Ball ball;

        //lives of the player :P
        private int lives = 3;

        //Random variable that's used when launching the ball so that it's not predictable everytime 
        public Random _random  = new Random();

        //Int that's used to keep count of the frames so that we can do a quick fix for the collision issues with the ball and the paddle
        private int frameCount = 0;

        //blocks
        List<Block> blocks = new List<Block>();
        private int blockHit = 0;
        //this is simply used for knowing where to spawn powerups
        private Vector2 _blockPosition;

        //PowerUps
        List<PowerUp> powerUps = new List<PowerUp>();
        double _powerUpChance = 0.2;

        //fmod studio stuff
        private readonly INativeFmodLibrary _nativeLibrary;
        public Sound _sPaddleHit;
        public Sound _sPlayerDeath;
        public Sound _sBallHit;
        public Channel _channel;

        //This block layout variable defines how many of each block there are and what type
        //The color of the block is determined by the number in the array
        int[,] blockLayout = new int[,]{
           {5,5,5,5,5,5,5,5,5,5,5,5,5,5,5},
           {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
           {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
           {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
           {3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
           {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
        };

        public Game1(INativeFmodLibrary nativeLibrary)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _nativeLibrary = nativeLibrary;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.ApplyChanges();

            FmodManager.Init(_nativeLibrary, FmodInitMode.CoreAndStudio, "Content");
            
            _sPaddleHit = CoreSystem.LoadStreamedSound("pong.wav");
            _sPlayerDeath = CoreSystem.LoadStreamedSound("arcade_die.wav");
            _sBallHit = CoreSystem.LoadStreamedSound("bass_sound.wav");
            _sBallHit.Volume = 0.75f;
            
            base.Initialize();
        }

        protected override void UnloadContent()
        { 
            FmodManager.Unload();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _bgTexture = Content.Load<Texture2D>("bg");
            paddle = new Paddle(this);
            ball = new Ball(this);
            
            paddle.LoadContent();
            paddle._position = new Vector2(512, 740);
            ball.LoadContent();
            ball._position = new Vector2(512, 740 - (ball._height + paddle._height));
            ball._ballDirection = new Vector2(randomFloat(-0.999f, 0.999f), -0.707f);
            ball._ballSpeed = 400;

            //for loop to be creating the layout of blocks thats provided with the block layout variable
            //since its a 2d array that we are having to sift through, we are using 2 for loops in order to render each of the blocks
            //first for loop deals with the rows
            //Second for loop deals with the columns
            for (int i = 0; i < blockLayout.GetLength(0); i++)
                for(int j = 0; j < blockLayout.GetLength(1); j++)
                {
                    {
                        Block tempBlock = new Block((BlockColor)blockLayout[i,j], this);
                        tempBlock.LoadContent();
                        //tempBlock._position = new Vector2(64 + i * 64, 200 - 32 * j);
                        tempBlock._position = new Vector2(64 + 64 * j, 100 + 32 * i);
                        blocks.Add(tempBlock);
                    }
                }
        }

        protected void CheckCollision()
        {
            float ballRadius = ball._width/ 2;
            
            //bool removeBall = false;
            //if (ball._position.X > (paddle._position.X - ballRadius - paddle._width / 2) && 
            //    ball._position.X < (paddle._position.X + ballRadius + paddle._width / 2) && 
            //    ball._position.Y < paddle._position.Y && 
            //    ball._position.Y > (paddle._position.Y - ballRadius - paddle._height/2)) 
            //{
            //    ball._ballDirection.Y = ball._ballDirection.Y * -1;
            //}

            //if the ball is on the right side of the paddle then we will send the ball more to the right
            //This isn't a perfect implementation of it but it's what I have so far
            if (ball._position.X > (paddle._position.X - ballRadius - paddle._width / 2) &&
                ball._position.X < (paddle._position.X + ballRadius + paddle._width / 2) &&
                ball._position.Y < paddle._position.Y &&
                ball._position.Y > (paddle._position.Y - ballRadius - paddle._height / 2) && ball._position.X > paddle._position.X && frameCount <= 0)
            {
                ball._ballDirection = Vector2.Reflect(ball._ballDirection, new Vector2(0.3f, -0.981f));
                frameCount = 20;
                _channel = _sPaddleHit.Play();
            }
            //If the ball is on the left side of the paddle then we will send the ball towards the left 
            //Again not perfect but its a work in progress
            if (ball._position.X > (paddle._position.X - ballRadius - paddle._width / 2) &&
                ball._position.X < (paddle._position.X + ballRadius + paddle._width / 2) &&
                ball._position.Y < paddle._position.Y &&
                ball._position.Y > (paddle._position.Y - ballRadius - paddle._height / 2) && ball._position.X < paddle._position.X && frameCount <= 0)
            {
                ball._ballDirection = Vector2.Reflect(ball._ballDirection, new Vector2(-0.3f, -0.981f));
                frameCount = 20;
                _channel = _sPaddleHit.Play();
            }

            //These functions are just used when ball collides with a wall or ceiling
            if (MathF.Abs(ball._position.X - 32) < ballRadius)
            {
                ball._ballDirection.X = ball._ballDirection.X * -1;
                _sBallHit.Play();
            }
            if(MathF.Abs(ball._position.X - 992) < ballRadius)
            {
                //riht collision
                ball._ballDirection.X = ball._ballDirection.X * -1;
                _sBallHit.Play();
            }
            if(MathF.Abs(ball._position.Y - 32) < ballRadius)
            {
                //top collisoin
                ball._ballDirection.Y = ball._ballDirection.Y * -1;
                _sBallHit.Play();
            }
            frameCount--;

            foreach (Block b in blocks)
            {
                //This is if it hits the bottom of the block
                //LATER SEE IF YOU CAN CLEAN THIS UP, INSTEAD OF HAVING 
                if (MathF.Abs(ball._position.Y - (b._position.Y + b._height/2)) < ballRadius && 
                    ball._position.X > b._position.X - b._width/2 && 
                    ball._position.X < b._position.X + b._width/2)
                {
                    ball._ballDirection.Y = ball._ballDirection.Y * -1;
                    DestroyBlock(b); break;
                    
                }

                //hitting the left side of the blocks
                if (MathF.Abs(ball._position.X - (b._position.X - b._width/2)) < ballRadius && (b._position.Y + b._height / 2) > ball._position.Y && (b._position.Y - b._height/2 < ball._position.Y))
                {
                    ball._ballDirection.X = ball._ballDirection.X * -1;
                    DestroyBlock(b); break;
                }

                //hitting the right side of the blocks
                if (MathF.Abs(ball._position.X - (b._position.X + b._width / 2)) < ballRadius && (b._position.Y + b._height / 2) > ball._position.Y && (b._position.Y - b._height / 2 < ball._position.Y))
                {
                    ball._ballDirection.X = ball._ballDirection.X * -1;
                    DestroyBlock(b); break;
                }

                if (MathF.Abs(ball._position.Y - (b._position.Y - b._height / 2)) < ballRadius &&
                    ball._position.X > b._position.X - b._width / 2 &&
                    ball._position.X < b._position.X + b._width / 2)
                {
                    ball._ballDirection.Y = ball._ballDirection.Y * -1;
                    DestroyBlock(b); break;
                }
            }
            //Don't think I need this ccode anymore, this looks a lot cleaner, but imma keep it here just in case
            /*if(removeBall)
            {
                _sBallHit.Play();
                blocks.RemoveAt(blockHit);
                removeBall = false;
                if (_random.NextDouble() < _powerUpChance)
                {
                    SpawnPowerUp(_blockPosition);
                }
            }*/
        }

        //This function in called whenever the ball hits a block in order to destroy the block hit
        private void DestroyBlock(Block block)
        {
            _sBallHit.Play();
            blocks.RemoveAt(blocks.IndexOf(block));
            if (_random.NextDouble() <= _powerUpChance)
            {
                SpawnPowerUp(block._position);
            }
        }

        private void RemovePowerUp(PowerUp powerUp)
        {
            powerUps.Remove(powerUp);
        }

        protected void CheckGameLost()
        {
            //if the ball is below a certain height and the player has lives then the game will reset the ball 
            if(ball._position.Y > 750 + ball._width && lives > 0)
            {
                ball._position = new Vector2(512, 740 - (ball._height + paddle._height));
                ball._ballDirection = new Vector2(randomFloat(-0.999f, 0.999f), -0.707f);
                ball._ballSpeed = 400;
                lives--;
                _sPlayerDeath.Play();
            }
            //if the player has no lives then it will exit the game, 
            //later this will be a game over screen where they can restart 
            else if (lives <= 0 )
            {
                Exit();
            }
        }

        protected void SpawnPowerUp(Vector2 position)
        {
            PowerUp tempPowerUp = new PowerUp((ePowerUpName)_random.Next(3), this);
            tempPowerUp._position = position;
            tempPowerUp.LoadContent();
            tempPowerUp.BoundingRect();
            //Debug.Write(tempPowerUp._position.ToString());
            powerUps.Add(tempPowerUp);
        }

        protected void CheckForPowerUps()
        {
            Rectangle paddleRect = paddle.BoundingRect();
            Rectangle powerUpRectangle;
            for (int i = powerUps.Count - 1; i >= 0; i--)
            {
                powerUpRectangle = powerUps[i].BoundingRect();
                if (powerUpRectangle.Intersects(paddleRect))
                {
                    powerUps.RemoveAt(i);
                }
            }
        }

        private void TriggerPowerUp(PowerUp p)
        {
            if(p._name == "powerup_c")
            {
                CPowerUp();
                Debug.Write("cccccccccc");
            }
            else if(p._name == "powerup_p")
            {
                PPowerUp();
                Debug.Write("pppppppp");
            }
            else if(p._name == "powerup_b")
            {
                BPowerUp();
                Debug.Write("bbbbbbbbbb");
            }
        }

        private void PPowerUp()
        {
            Debug.WriteLine("PPPPPPPPPP");
        }

        private void CPowerUp()
        {
            Debug.WriteLine("CCCCCCC");
        }

        private void BPowerUp()
        {
            Debug.WriteLine("BBBBBBBBBBBB");
        }

        public float randomFloat(float min, float max)
        {
            double result = (_random.NextDouble() * (max - min)) + min;
            return (float) result;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            paddle.Update(deltaTime);
            ball.Update(deltaTime);
            Debug.WriteLine(ball._position);

            CheckForPowerUps();
            for (int i = powerUps.Count - 1; i >= 0; i--)
            {
                powerUps[i].Update(deltaTime);
                if (powerUps[i].remove)
                {
                    powerUps.RemoveAt(i);
                }
            }
            CheckCollision();
            CheckGameLost();
            FmodManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_bgTexture, new Vector2(0, 0), Color.White);
            paddle.Draw(_spriteBatch);
            ball.Draw(_spriteBatch);
            foreach (Block b in blocks)
            {
                b.Draw(_spriteBatch);
            }
            foreach (PowerUp p in powerUps)
            {
                p.Draw(_spriteBatch);
            }
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}