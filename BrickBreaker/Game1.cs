﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.ApplyChanges();

            

            base.Initialize();
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
            ball._position = new Vector2(512, 740);

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
            

            Debug.WriteLine(paddle._width);
        }

        protected void CheckCollision()
        {
            float ballRadius = ball._width/ 2;
            
            bool removeBall = false;
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
            }

            //These functions are just used when ball collides with a wall or ceiling
            if (MathF.Abs(ball._position.X - 32) < ballRadius)
            {
                ball._ballDirection.X = ball._ballDirection.X * -1;
            }
            if(MathF.Abs(ball._position.X - 992) < ballRadius)
            {
                //riht collision
                ball._ballDirection.X = ball._ballDirection.X * -1;
            }
            if(MathF.Abs(ball._position.Y - 32) < ballRadius)
            {
                //top collisoin
                ball._ballDirection.Y = ball._ballDirection.Y * -1;
            }
            frameCount--;

            foreach (Block b in blocks)
            {
                //This is if it hits the bottom of the block
                if (MathF.Abs(ball._position.Y - (b._position.Y + b._height/2)) < ballRadius && 
                    ball._position.X > b._position.X - b._width/2 && 
                    ball._position.X < b._position.X + b._width/2)
                {
                    ball._ballDirection.Y = ball._ballDirection.Y * -1;
                    blockHit = blocks.IndexOf(b);
                    removeBall = true; break;
                }

                //hitting the left side of the blocks
                if (MathF.Abs(ball._position.X - (b._position.X - b._width/2)) < ballRadius && (b._position.Y + b._height / 2) > ball._position.Y && (b._position.Y - b._height/2 < ball._position.Y))
                {
                    ball._ballDirection.X = ball._ballDirection.X * -1;
                    blockHit = blocks.IndexOf(b);
                    removeBall = true; break;
                }

                //hitting the right side of the blocks
                if (MathF.Abs(ball._position.X - (b._position.X + b._width / 2)) < ballRadius && (b._position.Y + b._height / 2) > ball._position.Y && (b._position.Y - b._height / 2 < ball._position.Y))
                {
                    ball._ballDirection.X = ball._ballDirection.X * -1;
                    blockHit = blocks.IndexOf(b);
                    removeBall = true; break;
                }

                if (MathF.Abs(ball._position.Y - (b._position.Y - b._height / 2)) < ballRadius &&
                    ball._position.X > b._position.X - b._width / 2 &&
                    ball._position.X < b._position.X + b._width / 2)
                {
                    ball._ballDirection.Y = ball._ballDirection.Y * -1;
                    blockHit = blocks.IndexOf(b);
                    removeBall = true; break;
                }
            }
            if(removeBall)
            {
                Debug.WriteLine(blockHit.ToString());
                blocks.RemoveAt(blockHit);
                removeBall = false;
            }
           
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
            }
            //if the player has no lives then it will exit the game, 
            //later this will be a game over screen where they can restart 
            else if (lives <= 0 )
            {
                Exit();
            }
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
            CheckCollision();
            CheckGameLost();
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
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}