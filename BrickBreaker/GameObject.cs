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
    internal class GameObject
    {
        protected String _name = "";
        protected Texture2D _texture;
        protected Game1 _game;
        public Vector2 _position = Vector2.Zero;

        public GameObject(string name, Texture2D texture, Game1 game, Vector2 position)
        {
            _name = name;
            _texture = texture;
            _game = game;
            _position = position;
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
            batch.Begin();
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
