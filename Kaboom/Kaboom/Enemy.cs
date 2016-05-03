using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Kaboom.Properties;

namespace Kaboom
{
    class Enemy
    {
        Bitmap robberImage;

        private int screenWidth;
        private int screenHeight;

        public int x;
        private int y;
        private int width = 50;
        private int height = 100;

        // motion controls
        private Boolean movingRight = false;
        private int maxDistanceBeforeChange = 800;
        private int distanceCounter = 0;
        private int speed = 10;
        

        public Enemy(Rectangle screen)
        {
            x = (screen.Width/2) - (width/2);
            y = (screen.Height / 5) - height;
            screenWidth = screen.Width;
            screenHeight = screen.Height;

            robberImage = Resources.robber;
        }

        public void Draw(Graphics g, bool moving)
        {
            if (moving)
            {
                UpdatePosition();
            }
            
            g.DrawImage(robberImage, x, y);
            SolidBrush brush = new SolidBrush(Color.Black);
            //g.FillRectangle(brush,
            //                x,
            //                y,
            //                width,
            //                height);
        }

        public void IncreaseSpeed()
        {
            speed += 10;
            maxDistanceBeforeChange += 100;
            distanceCounter = 0;
        }

        public void ResetSpeed()
        {
            speed = 10;
        }

        public void UpdatePosition()
        {
            Random r = new Random();
            bool directionChanged = false;

            // 1 in 8 change to switch direction
            if (r.Next(8) == 1)
            {
                movingRight = !movingRight;
                directionChanged = true;
            }
                
            // update position
            if (movingRight)
                x += speed;
            else
                x -= speed;

            // count spaces moved
            if (!directionChanged)
                distanceCounter += speed;

            // detect sides of screen
            if (x <= 0)
                movingRight = true;

            if (x >= screenWidth - width)
                movingRight = false;

            //switch is max movement reached
            if (distanceCounter > maxDistanceBeforeChange)
            {
                movingRight = !movingRight;
                distanceCounter = 0;
                maxDistanceBeforeChange = r.Next(screenWidth);
            }                
        }
    }
}
