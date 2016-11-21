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
    interface UserShipInterface
    {
        void decrease_health(int amt);
        void increase_health(int amt);
        void move_left(int amt);
        void move_right(int amt);
        void move_up(int amt);
        void move_down(int amt);
        void collision(Rectangle obj, int damage); // Object that collided with the enemy ship, amt of damage it causes. Static int value in the class
        void shoot_missile();
        Rectangle get_user_ship();
    }
}
