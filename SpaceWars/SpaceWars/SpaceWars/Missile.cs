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
    class Missile : MissileInterface
    {
        // Local Variables
        private int collisionDamage;
        private int direction; // Sign indicates direction, value indicates speed
        private Rectangle missile;
        private int health;

        // Constructors
        public Missile()
        {
            collisionDamage = 0;
            direction = 0;
            missile = new Rectangle(0, 0, 0, 0);
            health = 0;
        }
        public Missile(int collisionDamage, int direction, int x, int y, int width, int height)
        {
            this.collisionDamage = collisionDamage;
            this.direction = direction;
            missile = new Rectangle(x, y, width, height);
            health = 1;
        }

        // Methods
        public void move()
        {
            missile.Y -= direction;
        }

        public void collision(Microsoft.Xna.Framework.Rectangle obj)
        {
            throw new NotImplementedException();
        }


        public Rectangle get_missile()
        {
            return missile;
        }

        public int get_collision_damage()
        {
            return collisionDamage;
        }

        public int get_health()
        {
            return health;
        }

        public void decrease_health(int amt) 
        {
            health -= amt;
        }
    }
}
