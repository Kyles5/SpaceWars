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
    class UserShip : UserShipInterface
    {
        // Local Variables
        private int health;
        //private int direction; // Sign indicates direction, value indicates speed
        private Rectangle ship;

        // Constructors
        public UserShip()
        {
            health = 0;
            //direction = 0;
            ship = new Rectangle(0, 0, 0, 0);
        }

        public UserShip(int health, /*int direction,*/ int x, int y, int width, int height)
        {
            this.health = health;
            //direction = this.direction;
            ship = new Rectangle(x, y, width, height);
        }

        // Methods
        public void decrease_health(int amt)
        {
            health -= amt;
        }

        public void increase_health(int amt)
        {
            health += amt;
        }

        public void move_left(int amt)
        {
            ship.X -= amt;
        }

        public void move_right(int amt)
        {
            ship.X += amt;
        }

        public void move_up(int amt)
        {
            ship.Y += amt;
        }

        public void move_down(int amt)
        {
            ship.Y -= amt;
        }

        public void collision(Microsoft.Xna.Framework.Rectangle obj, int damage)
        {
            throw new NotImplementedException();
        }

        public void shoot_missile()
        {
            throw new NotImplementedException();
        }

        public Rectangle get_user_ship()
        {
            return ship;
        }

        public int get_health()
        {
            return health;
        }
    }
}
