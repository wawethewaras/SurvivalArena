using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SurvivalArena;
using SurvivalArena.GameObjects;
using SurvivalArena.HealthSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE.HealthSystem {
    public class HealthUIManager : IEEDrawable {
        SpriteFont font;
        HealthComponent healthComponent;
        Vector2 textPosition;
        Vector2 offset;
        private int drawOrder = 1;
        public int DrawOrder => drawOrder;
        public HealthUIManager(ContentManager contentManager, HealthComponent healthComponent) {
            font = contentManager.Load<SpriteFont>("FontTest");
            this.healthComponent = healthComponent;
            textPosition = new Vector2(640, 0);
            offset = new Vector2(60, 0);

        }

        public void Draw(SpriteBatch spriteBatch) {
            var text = $"Health: {healthComponent.health}";
            spriteBatch.DrawString(font, text, new Vector2(textPosition.X - text.Length - offset.X, textPosition.Y), Color.White);
        }

    }
}
