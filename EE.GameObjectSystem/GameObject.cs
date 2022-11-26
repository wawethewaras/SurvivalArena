using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena.GameObjects {
    public class GameObject : IHasPosition, IUpdater {
        public Texture2D _texture;
        public Vector2 position;

        private List<IComponent> components = new List<IComponent>();
        public GameObject parent;
        public Vector2 offSet;

        public Vector2 Position {
            get => parent != null ? parent.position + offSet : position;
            set => position = value;
        }
        public GameObject(Texture2D texture, Vector2 position) {
            _texture = texture;
            this.position = position;

        }
        public GameObject(Texture2D texture, GameObject parent, Vector2 offSet) {
            _texture = texture;
            position = parent.position + offSet;
            this.parent = parent;
            this.offSet = offSet;
        }
        public virtual void Update(float gameTime) {
            foreach (var component in components) {
                component.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            var pos = parent != null ? parent.position + offSet : position;
            spriteBatch.Draw(_texture, pos, Color.White);
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