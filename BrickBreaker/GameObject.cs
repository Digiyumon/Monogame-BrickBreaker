using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    public class GameObject
    {
        protected String _name = "";
        protected Texture2D _texture;
        protected Game1 _game;
        public Vector2 _position = Vector2.Zero;

        public GameObject(Game1 myGame)
        {
            //_name = name;
            //_texture = texture;
            _game = myGame;
            //_position = position;
        }
        public virtual void LoadContent()
        {
            if (_name != "")
            {
                _texture = _game.Content.Load<Texture2D>(_name);
            }
        }
        public virtual void Update(float deltaTime)
        {

        }
        
        public virtual void Draw(SpriteBatch batch)
        {
            //we don't need this batch.begin here because we are simply calling this function in the main Game1.cs class 
            //we already have started the spritebatch in the other file so we can't "begin" the spritebatch again since it never ended :/
            //batch.Begin();
            if(_texture != null )
            {
                Vector2 drawPosition = _position;
                drawPosition.X -= _texture.Width/2;
                drawPosition.Y -= _texture.Height/2;
                batch.Draw(_texture,drawPosition, Color.White);
            }
        }
     }
 }
