using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena {
    public class GameObject : IHasPosition {
        protected Texture2D _texture;
        public Vector2 position;
        public ColliderComponent colliderComponent;

        private List<IComponent> components = new List<IComponent>();

        public Vector2 Position { get => position; set => position = value; }
        public GameObject(Texture2D texture, Vector2 position) {
            _texture = texture;
            this.position = position;

        }
        public virtual void Update(float gameTime) {
            foreach (var component in components) {
                component.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(_texture, position, Color.White);
        }

        public void AddComponent(IComponent component) {
            components.Add(component);
        }
    }

    public interface IComponent {
        void Update(float gameTime);
    }
    public interface IHasPosition {
        public Vector2 Position { get; set; }

    }
}