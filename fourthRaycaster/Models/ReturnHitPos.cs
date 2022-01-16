using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace fourthRaycaster.Models
{
    public class ReturnHitPos
    {
        private bool hit;
        private Vector2 hitPos;

        public bool Hit { get => hit; set => hit = value; }
        public Vector2 HitPos { get => hitPos; set => hitPos = value; }

        public ReturnHitPos(bool hit, Vector2 hitPos)
        {
            this.hit = hit;
            this.hitPos = hitPos;
        }
    }
}
