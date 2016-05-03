using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Kaboom.Properties;

namespace Kaboom
{
    class Paddle
    {
        private Bitmap paddleImage;

        public int x;
        public int y;
        public int width = 100;
        public int height = 225;

        private int screenWidth;
        private int screenHeight;        
        private bool movingRight = false;

        public int speed = 30;
        public int life = 3;

        /// <summary>
        /// constructor to setup a new paddle
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Paddle(int width, int height, int x, int y)
        {
            screenWidth = width;
            screenHeight = height;
            this.x = x;
            this.y = y;
            paddleImage = Resources.paddle;
        }

        /// <summary>
        /// draw method for paddle
        /// 
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {            
            if (life > 0)
            {
                g.DrawImage(paddleImage, x, y);

            }

            if (life > 1)
            {
                g.DrawImage(paddleImage, x, y + (height / 3));

            }

            if (life > 2)
            {
                g.DrawImage(paddleImage, x, y + (2 * (height / 3)));

            }
           
            
            
            //SolidBrush brush = new SolidBrush(Color.Black);
            //g.FillRectangle(brush,
            //                x,
            //                y,
            //                width,
            //                height/3);
            //SolidBrush brush = new SolidBrush(Color.White);
            //g.FillRectangle(brush,
            //                x,
            //                y + (height/3),
            //                width,
            //                height/3);
            //brush = new SolidBrush(Color.Yellow);
            //g.FillRectangle(brush,
            //                x,
            //                y + (2*(height / 3)),
            //                width,
            //                height / 3);
        }

        /// <summary>
        /// moves the paddle based on booleans set in the main form by KeyDown event
        /// </summary>
        /// <param name="movment"></param>
        /// <param name="moving"></param>
        public void UpdatePosition(int movment, bool moving)
        {
            if (moving)
            {
                int newX = x + (movment * speed);
                if (newX < 0)
                    newX = 0;
                if (newX > screenWidth - width)
                    newX = screenWidth - width;
                x = newX;
            }            
        }

        public void SetXPosition(int x)
        {
            this.x = x;
        }
       
    }
}
