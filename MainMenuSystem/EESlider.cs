using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena;
using SurvivalArena.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenuSystem {
    public class EESlider : IUpdater, IEEDrawable{
        public Texture2D frontTexture;
        public Texture2D backTexture;
        public Vector2 position;
        public IClampedValue clampedValue;
        protected Rectangle drawArea;

        public int DrawOrder => 1;

        public EESlider(Texture2D frontTexture, Texture2D backTexture, Vector2 position, IClampedValue clampedValue) {
            this.frontTexture = frontTexture;
            this.backTexture = backTexture;
            this.position = position;
            this.clampedValue = clampedValue;
            this.drawArea = new Rectangle(0,0, frontTexture.Width, frontTexture.Height);

        }

        public void Update(float gametime) { 
            drawArea.Width = (int)((float)clampedValue.Value / clampedValue.Max * frontTexture.Width);
        }

        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(backTexture, position, Color.White);
            spriteBatch.Draw(frontTexture,position,drawArea,Color.White,0,Vector2.Zero,1f,SpriteEffects.None,1f);
        }
    }
}
