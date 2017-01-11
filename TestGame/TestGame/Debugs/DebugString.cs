using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Dorothy.Core;
using Microsoft.Xna.Framework.Graphics;
using Dorothy;
using Dorothy.Cameras;

namespace TestGame.Debugs
{
    class DebugString : Drawable
    {
        SpriteFont _font;
        public string Text
        {
            set;
            get;
        }
        public new Vector2 Position
        {
            set;
            get;
        }

        public DebugString()
        {
            this.Text = string.Empty;
            _font = oGame.Instance.Content.Load<SpriteFont>("font");
            oGame.Root.Add(this);
        }
        public override void Draw()
        {
            oGame.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                oGraphic.ZWriteEnable ? DepthStencilState.Default : DepthStencilState.DepthRead,
                RasterizerState.CullNone);
            oGame.SpriteBatch.DrawString(_font, Text, Position, Color.White);
            oGame.SpriteBatch.End();
        }
        protected override void GetItselfReady()
        {
            oDrawQueue.EnSecond(this);
        }
    }
}
