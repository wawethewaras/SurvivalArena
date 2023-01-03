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
    public class HasPositionWithOfSet : IHasPosition {
        public IHasPosition hasPosition;
        public IHasFacingDirection hasFacingDirection;

        public Vector2 OffSet;

        public HasPositionWithOfSet(IHasPosition hasPosition, IHasFacingDirection hasFacingDirection, Vector2 offSet) {
            this.hasPosition = hasPosition;
            this.hasFacingDirection = hasFacingDirection;
            OffSet = offSet;
        }

        public Vector2 Position { get => hasFacingDirection != null ? PositionWithFacingDirection : hasPosition.Position + OffSet; set => hasPosition.Position = value; }

        public Vector2 PositionWithFacingDirection => hasPosition.Position + (hasFacingDirection.LookingRight ? OffSet : -OffSet);

    }

    public interface IHasFacingDirection {
        public bool LookingRight { get; set; }
    }
    public interface IEEDrawable {
        public int DrawOrder { get; }
        void Draw(SpriteBatch spriteBatch);
    }

    public interface IClampedValue {
        int Max { get; }
        int Value { get; }

    }
}