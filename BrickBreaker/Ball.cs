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

    

    public class Ball : GameObject
    {

        public float _ballSpeed = 400;
        public Vector2 _ballDirection = new Vector2(0.707f, -0.707f);
        public Ball(Game1 myGame) :
            base(myGame)
        {
            _name = "ball";
        }

        public override void Update(float deltaTime)
        {
            _position.X = MathHelper.Clamp(_position.X, 32 + _texture.Width / 2, 992 - _texture.Width / 2);
            //_position.Y = MathHelper.Clamp(_position.Y, -736 + _texture.Width / 2, 736 - _texture.Width / 2);
            //make sure that you're ADDING and not setting the position to be the result, 
            _position += _ballDirection * _ballSpeed * deltaTime;
            base.Update(deltaTime);
        }

        //look up what virtual and override mean


    }
}
