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

        Texture2D texture2D;
        Rectangle rectangle;

        bool isHovered = false;
        bool isClicked = false;

        event Action Clicked;

        bool release = true;

        public EEButton(GraphicsDeviceManager graphicsDeviceManager, Vector2 position = new Vector2(), Action? clicked = null) {
            texture2D = new Texture2D(graphicsDeviceManager.GraphicsDevice, 1, 1);
            texture2D.SetData(new[] { Color.White });
            rectangle = new Rectangle((int)position.X, (int)position.Y, 200,200);
            if (clicked != null) {
                Clicked += clicked;
            }
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
            if (mouseState.LeftButton == ButtonState.Pressed && release) {
                Clicked?.Invoke();
                release = false;
            }
            if (mouseState.LeftButton == ButtonState.Released) {
                release = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            var color = isHovered ? Color.White : Color.Red;
            color = isClicked ? Color.Blue : color;
            var rectangleSmall = new Rectangle(rectangle.X / 2, rectangle.Y / 2, rectangle.Width / 2, rectangle.Height/2);

            spriteBatch.Draw(texture2D, rectangleSmall, color);
        }
    }
}
