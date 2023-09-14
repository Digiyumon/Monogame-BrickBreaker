using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

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

        private int lives = 3;

        public Random _random  = new Random();



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
            //ball._position = new Vector2(512, 742);
            // TODO: use this.Content to load your game content here
        }

        protected void CheckCollision()
        {
            float ballRadius = ball._width/ 2;
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
            
        }

        protected void CheckGameLost()
        {
            if(ball._position.Y > 750 + ball._width && lives > 0)
            {
                ball._position = new Vector2(512, 740 - (ball._height + paddle._height));
                ball._ballDirection = new Vector2(randomFloat(-0.999f, 0.999f), -0.707f);
                ball._ballSpeed = 300;
                lives--;
            }
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
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}