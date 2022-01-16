using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace fourthRaycaster.Models
{
    public class AnimatedSprite : DrawableGameComponent
    {
        private Game1 game1;
        private SpriteBatch spriteBatch;
        private Texture2D texture;
        private Vector2 position;
        private Vector2 size;
        private Vector2 dimension;
        private int row;
        private int col;

        private List<Rectangle> frames;

        private int delay;
        private int delayCounter;
        private int frameIndex = 0;
        private bool play = false;
        private bool canBeInterpreted = false;
        private bool loop = false;

        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Size { get => size; set => size = value; }
        public int Delay { get => delay; set => delay = value; }
        public bool CanBeInterpreted { get => canBeInterpreted; set => canBeInterpreted = value; }
        public bool Loop { get => loop; set => loop = value; }

        public AnimatedSprite(Game game, Texture2D texture, Vector2 position, Vector2 size, int delay, int row, int col) : base(game)
        {
            this.game1 = (Game1)game;
            this.spriteBatch = game1._spriteBatch;
            this.texture = texture;
            this.position = position;
            this.delay = delay;
            this.row = row;
            this.col = col;
            this.size = size;

            this.dimension = new Vector2(texture.Width / col, texture.Height / row);
            CreateFrames();
        }

        /// <summary>
        /// Checks the rectangles for the frames of the sprite
        /// </summary>
        private void CreateFrames()
        {
            //Makes a new list for the frames
            frames = new List<Rectangle>();

            //Loop through the rows and columns
            for (int y = 0; y < row; y++)
            {
                for (int x = 0; x < col; x++)
                {
                    //Gets the position of the frame
                    Point framesPos = new Point((int)dimension.X * x, (int)dimension.Y * y);
                    //Makes the rectangle of the frame
                    Rectangle rec = new Rectangle(framesPos, dimension.ToPoint());
                    //Adds the rectangle to the frame list
                    frames.Add(rec);
                }
            }
        }

        /// <summary>
        /// Plays the sprites animation
        /// </summary>
        public void Play()
        {
            //If the sprite can be interpreted then restart the animation
            if (canBeInterpreted)
            {
                frameIndex = 0;
                delayCounter = 0;
            }

            //Set the sprite animation to play
            this.play = true;
        }

        /// <summary>
        /// Stops the sprites animation
        /// </summary>
        public void Stop()
        {
            //Set the animation to the first frame
            frameIndex = 0;
            delayCounter = 0;

            //Set the sprite animation to stop playing
            this.play = false;
        }

        public override void Update(GameTime gameTime)
        {
            //If the animation is playing
            if (play)
            {
                //Check the delay
                delayCounter++;
                if (delayCounter > delay)
                {
                    //Go to the next frame
                    frameIndex++;
                    //If the frame is past the amount of frames the sprite has, go to the first frame
                    if (frameIndex >= frames.Count)
                    {
                        frameIndex = 0;
                        //If the sprite is not to loop, set the play to stop
                        if (!loop)
                            play = false;
                    }
                    delayCounter = 0;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null);

            //Draw the frame
            if (frameIndex >= 0 && frameIndex < frames.Count)
            {
                Rectangle rec = new Rectangle(position.ToPoint(), size.ToPoint());
                spriteBatch.Draw(texture, rec, frames[frameIndex], Color.White); ;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
