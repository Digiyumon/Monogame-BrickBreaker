using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrickBreaker
{

    public enum BlockColor
    {
        Red = 0,
        Yellow,
        Blue,
        Green,
        Purple,
        GreyHi,
        Grey
    }

    internal class Block : GameObject
    {
        public Block(BlockColor blockColor ,Game1 myGame):
            base(myGame)
        {
            switch (blockColor)
            {
                case BlockColor.Red: 
                    _name = "block_red";
                        break;
                case BlockColor.Yellow: 
                    _name = "block_yellow";
                    break;
                case BlockColor.Blue: 
                    _name = "block_blue";
                    break;
                case BlockColor.Green:
                    _name = "block_green";
                    break;
                case BlockColor.Purple:
                    _name = "block_purple";
                    break;
                case BlockColor.GreyHi:
                    _name = "block_grey_hi";
                    break;
                case BlockColor.Grey:
                    _name = "block_grey";
                    break;
            }
        }
    }
}
