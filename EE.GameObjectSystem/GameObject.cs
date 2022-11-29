using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SurvivalArena.GameObjects {
    public class GameObject : IHasPosition, IUpdater {
        public Vector2 position;

        private List<IComponent> components = new List<IComponent>();
        public GameObject parent;
        public Vector2 offSet;

        public Vector2 Position {
            get => parent != null ? parent.position + offSet : position;
            set => position = value;
        }
        public GameObject(Vector2 position) {
            this.position = position;

        }
        public GameObject(GameObject parent, Vector2 offSet) {
            position = parent.position + offSet;
            this.parent = parent;
            this.offSet = offSet;
        }
        public virtual void Update(float gameTime) {
            foreach (var component in components) {
                component.Update(gameTime);
            }
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
    public interface IHasFacingDirection {
        public bool LookingRight { get; set; }
    }
    public interface IEEDrawable {
        void Draw(SpriteBatch spriteBatch);
    }
}