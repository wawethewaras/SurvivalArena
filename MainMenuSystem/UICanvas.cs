using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenuSystem {

    public class UICanvas {
        private int width = 1280;
        private int height = 720;
        SpriteFont font;

        public UICanvas(int width, int height, SpriteFont font) {
            this.width = width;
            this.height = height;
            this.font = font;
        }
        public void DrawToRightSide(SpriteBatch spriteBatch, Vector2 offSet, string text) {

            spriteBatch.DrawString(font, text, new Vector2(width + offSet.X - text.Length, offSet.Y), Color.White);
        }
        public void DrawToCenter(SpriteBatch spriteBatch, Vector2 offSet, string text) {

            spriteBatch.DrawString(font, text, new Vector2(width/2 + offSet.X - text.Length, offSet.Y), Color.White);
        }
    }
}
