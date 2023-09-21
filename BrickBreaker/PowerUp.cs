using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{

    public enum ePowerUpName
    {
        powerup_c = 0,
        powerup_b,
        powerup_p
    } 

    internal class PowerUp : GameObject
    {


        private float _fallSpeed = 100f;
        public bool remove = false;

         public PowerUp(ePowerUpName powerUpName ,Game1 game)
            : base(game)
        {
            switch (powerUpName)
            {
                case ePowerUpName.powerup_c:
                    _name = "powerup_c";
                    break;
                case ePowerUpName.powerup_b:
                    _name = "powerup_b";
                    break;
                case ePowerUpName.powerup_p:
                    _name = "powerup_p";
                    break;
            } 
        }
        public override void Update(float deltaTime)
        {
            _position.Y += _fallSpeed * deltaTime;
            if (_position.Y > 760)
            {
                remove = true;
            }
            base.Update(deltaTime);
        }
    }
}
