using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    public class Paddle : GameObject
    {

        public float _paddleSpeed = 500;
        public Paddle(Game1 myGame):
            base (myGame)
        {
            _name = "paddle";
        }

        public override void Update(float deltaTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if(keyboardState.IsKeyDown(Keys.Left))
            {
                _position.X -= _paddleSpeed * deltaTime;
            }
            else if(keyboardState.IsKeyDown(Keys.Right))
            {
                _position.X += _paddleSpeed * deltaTime;
            }
            _position.X = MathHelper.Clamp(_position.X, 32 + _texture.Width / 2, 992 - _texture.Width / 2);
            base.Update(deltaTime);
        }

        //look up what virtual and override mean


    }
}
