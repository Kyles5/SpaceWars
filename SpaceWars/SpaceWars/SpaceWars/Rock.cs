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
    class Rock : RockInterface
    {
        // Local Variables
        private int collisionDamage;
        private int direction; // Sign indicates direction, value indicated speed
        private String size; // Small, Medium, Large
        private Rectangle rock;

        // Constructors
        public Rock()
        {
            collisionDamage = 0;
            direction = 0;
            size = "";
            rock = new Rectangle(0, 0, 0, 0);
        }

        public Rock(int collisionDamage, int direction, String size, int x, int y, int width, int height)
        {
            this.collisionDamage = collisionDamage;
            this.direction = direction;
            this.size = size;
            rock = new Rectangle(x, y, width, height);
        }

        // Methods
        public void decrease_size()
        {
            switch (size)
            {
                case "small":
                    size = "none";
                    break;
                case "medium":
                    size = "small";
                    break;
                case "large":
                    size = "medium";
                    break;
                default:
                    break;
            }
        }

        public void move(int amt)
        {
            rock.Y += direction;
            rock.X += amt;
        }

        public int change_direction()
        {
            throw new NotImplementedException();
        }

        public void collision(Microsoft.Xna.Framework.Rectangle obj)
        {
            throw new NotImplementedException();
        }


        public Rectangle get_rock()
        {
            return rock;
        }
    }
}
