using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceWars
{
    interface RockInterface
    {
        void decrease_size(); // Drop rock down 1 level
        void move(int amt); // Move the rock int the current direction
        int change_direction(); // Change the rocks direction based on what it collided with
        void collision(Rectangle obj); // Handle what happens int he event of a collision
        Rectangle get_rock();
    }
}
