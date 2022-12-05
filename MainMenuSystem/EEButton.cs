using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenuSystem {
    public class EEButton {

        Vector2 position;
        Texture2D texture2D;
        Vector2 size;
        Rectangle rectangle;
        bool isHovered = false;
        bool isClicked = false;
        public EEButton(GraphicsDeviceManager graphicsDeviceManager) {
            texture2D = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1);
            texture2D.SetData(new[] { Color.White });
            rectangle = new Rectangle(0,0, 200,200);
        }
        public void Update(float gameTime) {
            var mouseState = Mouse.GetState();
            var mousePoint = new Point(mouseState.X, mouseState.Y);

            if (rectangle.Contains(mousePoint)) {
                isHovered = true;
                isClicked = mouseState.LeftButton == ButtonState.Pressed;
            }
            else {
                isHovered = false;
                isClicked = false;
            }
            if (mouseState.LeftButton == ButtonState.Pressed) {
                Debug.WriteLine("MP: " + new Vector2(mouseState.X, mouseState.Y));
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            var color = isHovered ? Color.White : Color.Red;
            color = isClicked ? Color.Blue : color;

            spriteBatch.Draw(texture2D, rectangle, color);
        }
    }
}
