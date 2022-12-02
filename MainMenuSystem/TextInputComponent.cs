using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SurvivalArena.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenuSystem {
    public class TextInputComponent : IComponent {
        public GameWindow gw;
        bool myBoxHasFocus = true;
        public MouseState mouseState;
        SpriteFont currentFont;

        StringBuilder myTextBoxDisplayCharacters = new StringBuilder();

        public TextInputComponent(SpriteFont currentFont, GameWindow gw) {
            this.currentFont = currentFont;
            this.gw = gw;
        }

        public void RegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method) {
            gw.TextInput += method;
        }
        public void UnRegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method) {
            gw.TextInput -= method;
        }
        // these two are textbox specific.
        public void CheckClickOnMyBox(Point mouseClick, bool isClicked, Rectangle r) {
            if (r.Contains(mouseClick) && isClicked) {
                myBoxHasFocus = !myBoxHasFocus;
                if (myBoxHasFocus)
                    RegisterFocusedButtonForTextInput(OnInput);
                else
                    UnRegisterFocusedButtonForTextInput(OnInput);
            }
        }
        public void OnInput(object sender, TextInputEventArgs e) {
            var k = e.Key;
            var c = e.Character;
            myTextBoxDisplayCharacters.Append(c);
            Console.WriteLine(myTextBoxDisplayCharacters);
        }
        public void Update(float gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                return;

            mouseState = Mouse.GetState();
            var isClicked = mouseState.LeftButton == ButtonState.Pressed;
            CheckClickOnMyBox(mouseState.Position, isClicked, new Rectangle(0, 0, 200, 200));
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(currentFont, myBoxHasFocus.ToString(), new Vector2(10, 500), Color.Yellow);
            spriteBatch.DrawString(currentFont, myTextBoxDisplayCharacters, new Vector2(10, 510), Color.Red);
        }
    }
}
