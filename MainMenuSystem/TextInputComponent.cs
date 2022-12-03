using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SurvivalArena.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MainMenuSystem {
    public class TextInputComponent : IComponent {
        public GameWindow gw;
        bool myBoxHasFocus = false;
        public MouseState mouseState;
        SpriteFont currentFont;

        StringBuilder myTextBoxDisplayCharacters = new StringBuilder();
        public Vector2 Position = new Vector2(10, 510);
        public TextInputComponent(SpriteFont currentFont, GameWindow gw) {
            this.currentFont = currentFont;
            this.gw = gw;
            RegisterFocusedButtonForTextInput(OnInput);

        }
        public string GetText() => myTextBoxDisplayCharacters.ToString();
        public void ResetText() => myTextBoxDisplayCharacters.Clear();

        public void RegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method) {
            gw.TextInput += method;
        }
        public void UnRegisterFocusedButtonForTextInput(System.EventHandler<TextInputEventArgs> method) {
            gw.TextInput -= method;
        }

        public void OnInput(object sender, TextInputEventArgs e) {
            var k = e.Key;
            var c = e.Character;
            if (isAlphaNumeric(c.ToString())) {
                myTextBoxDisplayCharacters.Append(c);
            }
        }
        public void Update(float gameTime) {
            if (Keyboard.GetState().IsKeyDown(Keys.Back) && myBoxHasFocus) {
                myTextBoxDisplayCharacters.Remove(myTextBoxDisplayCharacters.Length - 1, 1);
                myBoxHasFocus = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Back)) {
                myBoxHasFocus = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.DrawString(currentFont, myTextBoxDisplayCharacters, Position, Color.Red);
        }

        public static bool isAlphaNumeric(string strToCheck) {
            Regex rg = new Regex(@"^[a-zA-Z0-9\s,]*$");
            return rg.IsMatch(strToCheck);
        }
    }
}
