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

        public event Action Clicked;

        bool release = true;

        public EEButton(Texture2D texture2D, Vector2 position = new Vector2(), Action? clicked = null) {
            this.texture2D = texture2D;
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture2D.Width, texture2D.Height);
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
            if (isClicked && release) {
                Clicked?.Invoke();
                release = false;
            }
            if (mouseState.LeftButton == ButtonState.Released) {
                release = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            var color = isHovered ? Color.Gray : Color.White;
            color = isClicked ? Color.Black : color;
            var rectangleSmall = new Rectangle((int)(rectangle.X / MainMenuManager.screenScaleWitdh), (int)(rectangle.Y / MainMenuManager.screenScaleHeight), (int)(rectangle.Width / MainMenuManager.screenScaleWitdh), (int)(rectangle.Height/ MainMenuManager.screenScaleHeight));

            spriteBatch.Draw(texture2D, rectangleSmall, color);
        }
    }
}
