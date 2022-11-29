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
        public HealthUIManager(ContentManager contentManager, HealthComponent healthComponent) {
            font = contentManager.Load<SpriteFont>("FontTest");
            this.healthComponent = healthComponent;
            textPosition = new Vector2(1210, 0);
        }

        public void Draw(SpriteBatch spriteBatch) {
            var text = $"Health: {healthComponent.health}";
            spriteBatch.DrawString(font, text, textPosition, Color.White);
        }

    }
}
