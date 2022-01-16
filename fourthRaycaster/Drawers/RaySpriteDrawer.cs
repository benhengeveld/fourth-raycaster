using fourthRaycaster.Models;
using fourthRaycaster.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace fourthRaycaster.Drawers
{
    public class RaySpriteDrawer : DrawableGameComponent
    {
        private Game1 game1;
        private SpriteBatch spriteBatch;
        private Player player;

        public RaySpriteDrawer(Game game) : base(game)
        {
            this.game1 = (Game1)game;
            this.spriteBatch = game1._spriteBatch;
            this.player = game1.player;
        }

        public void SortRaySprites()
        {
            List<RaySprite> raySprites = game1.raySprites;



            game1.raySprites = raySprites;
        }

        public RaySpriteReturn GetSpritesScreenPosition(Vector2 position)
        {
            float playerAngle = (float)(player.Angle * 180 / Math.PI);
            Vector2 deltaPos = position - player.Position;

            var thetaTemp = Math.Atan2(deltaPos.Y, deltaPos.X);
            thetaTemp = thetaTemp * 180 / Math.PI;
            if (thetaTemp < 0)
            {
                thetaTemp += 360;
            }

            var yTmp = playerAngle + 30 - thetaTemp;
            if (thetaTemp > 270 && playerAngle < 90)
                yTmp = playerAngle + 30 - thetaTemp + 360;
            if (playerAngle > 270 && thetaTemp < 90)
                yTmp = playerAngle + 30 - thetaTemp - 360;

            var xTmp = yTmp * game1.bounds.X / 60.0;
            xTmp = xTmp - game1.bounds.X;
            xTmp *= -1;

            float distance = game1.raycastHandler.GetDistance(player.Position, position);
            float lineHeight = (float)(game1.cubeSize * game1.bounds.Y) / distance;
            lineHeight = (float)Math.Clamp(lineHeight, 0, double.MaxValue);

            Vector2 drawSize = new Vector2(lineHeight, lineHeight);
            Vector2 drawPos = new Vector2((float)xTmp - drawSize.X / 2, game1.bounds.Y / 2 - drawSize.Y / 2);
            RaySpriteReturn raySpriteReturn = new RaySpriteReturn(drawSize, drawPos, distance);
            return raySpriteReturn;
        }

        public override void Draw(GameTime gameTime)
        {
            SortRaySprites();

            spriteBatch.Begin(SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                null);

            float[] zBuffer = game1.zBuffer;
            foreach (RaySprite raySprite in game1.raySprites)
            {
                RaySpriteReturn raySpriteReturn = GetSpritesScreenPosition(raySprite.Position);

                float sizeDif = raySprite.Texture.Width / raySpriteReturn.DrawSize.X;
                for (int x = 0; x < raySpriteReturn.DrawSize.X; x++)
                {
                    Rectangle textureRectangle = new Rectangle((int)(x * sizeDif), 0, 1, raySprite.Texture.Height);
                    Vector2 texturePos = new Vector2(raySpriteReturn.DrawPos.X + x, raySpriteReturn.DrawPos.Y);
                    Vector2 textureSize = new Vector2(1, raySpriteReturn.DrawSize.Y);
                    float layerDepth = raySpriteReturn.Distance / 10000f;

                    if ((int)(raySpriteReturn.DrawPos.X + x) < zBuffer.Length && (int)(raySpriteReturn.DrawPos.X + x) > 0)
                    {
                        if (raySpriteReturn.Distance < zBuffer[(int)(raySpriteReturn.DrawPos.X + x)])
                        {
                            spriteBatch.Draw(raySprite.Texture, new Rectangle(texturePos.ToPoint(), textureSize.ToPoint()), textureRectangle, raySprite.Color, 0, Vector2.Zero, SpriteEffects.None, layerDepth);
                        }
                    }
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
