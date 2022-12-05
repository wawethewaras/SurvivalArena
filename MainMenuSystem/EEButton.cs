using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenuSystem {
    public class EEButton {

        Vector2 position;
        Texture2D texture2D;
        Vector2 size;
        Rectangle rectangle;

        public void LoadContent(GraphicsDeviceManager graphicsDeviceManager) {
            texture2D = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1);
            texture2D.SetData(new[] { Color.White });
            rectangle = new Rectangle(0,0, 200,200);
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture2D, rectangle, Color.Chocolate);
        }
    }
}
