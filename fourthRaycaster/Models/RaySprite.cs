using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace fourthRaycaster.Models
{
    public class RaySprite : GameComponent
    {
        private Game1 game1;
        private Texture2D texture;
        private Vector2 position;
        private Color color;

        public Texture2D Texture { get => texture; set => texture = value; }
        public Vector2 Position { get => position; set => position = value; }
        public Color Color { get => color; set => color = value; }

        public RaySprite(Game game, Texture2D texture, Vector2 position, Color color) : base(game)
        {
            this.game1 = (Game1)game;
            this.texture = texture;
            this.position = position;
            this.color = color;
        }
    }
}
