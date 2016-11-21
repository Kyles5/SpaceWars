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
    /*
     Basic: Does not shoot, 10 health, 5 collision damage 
     Level 2: Shoots 1 missile at player, tracks player ship, 20 health, 10 collision damage, 5 missile damage
     Level 3: Shoots 2 missiles at player, tracks player ship, 30 health, 15 collision damage, 10 missile damage  
     */


    class EnemyShip : EnemyShipInterface
    {
        // Local Variables
        private int health;
        private int collisionDamage;
        //private int direction; // Sign indicates direction, value indicates speed
        private String type;
        private Rectangle ship;

        // Constructors
        public EnemyShip() 
        {
            health = 0;
            collisionDamage = 0;
            //direction = 0;
            type = "";
            ship = new Rectangle(0, 0, 0, 0);
        }

        public EnemyShip(int health, int collisionDamage, /*int direction,*/ String type, int x, int y, int width, int height)
        {
            this.health = health;
            this.collisionDamage = collisionDamage;
            //direction = this.direction;
            this.type = type;
            ship = new Rectangle(x, y, width, height);
        }

        // Methods
        public void decrease_health(int amt)
        {
            health -= amt;
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
            ship.Y -= amt;
        }

        public void move_down(int amt)
        {
            ship.Y += amt;
        }

        public void collision(Rectangle missile, int damage)
        {
            throw new NotImplementedException();
        }

        public Rectangle get_enemy_ship()
        {
            return ship;
        }

        public String get_type()
        {
            return type;
        }

        public int get_collision_damage()
        {
            return collisionDamage;
        }

        public int get_health()
        {
            return health;
        }
    }
}
