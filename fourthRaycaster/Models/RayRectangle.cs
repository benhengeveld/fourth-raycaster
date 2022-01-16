using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace fourthRaycaster.Models
{
    public class RayRectangle
    {
        private Texture2D texture;
        private Rectangle rectangle;
        private Color color;
        private Rectangle textureRectangle;
        private bool visable;

        public Texture2D Texture { get => texture; set => texture = value; }
        public Rectangle Rectangle { get => rectangle; set => rectangle = value; }
        public Color Color { get => color; set => color = value; }
        public Rectangle TextureRectangle { get => textureRectangle; set => textureRectangle = value; }
        public bool Visable { get => visable; set => visable = value; }

        public RayRectangle(Texture2D texture, Rectangle rectangle, Color color, Rectangle textureRectangle, bool visable)
        {
            this.texture = texture;
            this.rectangle = rectangle;
            this.color = color;
            this.textureRectangle = textureRectangle;
            this.visable = visable;
        }
    }
}
