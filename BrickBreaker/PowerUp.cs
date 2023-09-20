using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{
    internal class PowerUp : GameObject
    {
        public enum ePowerUpName
        {
            powerup_c = 0,
            powerup_b,
            powerup_p
        }

        private float _fallSpeed = 9.8f;

        PowerUp(ePowerUpName powerUpName ,Game1 game)
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


            base.Update(deltaTime);
        }
    }
}
