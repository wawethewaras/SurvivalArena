using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGamePractice {
    internal class Game1 : Game {

        private GraphicsDeviceManager graphicsDeviceManager;

        private SpriteBatch spriteBatch = null;

        private SpriteFont? font = null;
        public Game1() : base() {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            this.Content.RootDirectory = "Content";
            this.IsFixedTimeStep = true;
            this.IsMouseVisible = true;
        }

        protected override void Initialize() {
            base.Initialize();
        }
        protected override void LoadContent() {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("FontTest");

        }
        protected override void UnloadContent() {
            base.UnloadContent();
        }
        protected override void Update(GameTime gameTime) {
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime) {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);
            this.spriteBatch.Begin();
            this.spriteBatch.DrawString(font,"Hello!", new Vector2(0,0), Color.White);
            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
