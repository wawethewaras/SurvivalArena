using SurvivalArena.GameObjects;

namespace EE.StateSystem {
    public class StateComponent : IComponent {


        private State currentState;

        public void TransitionToState(State newState) {
            currentState?.OnExit();
            currentState = newState;
            currentState?.OnEnter();
        }

        public void Update(float gameTime) {
            currentState?.OnAct(gameTime);
        }
    }

    public class ShootAction {
        public float shootDelay = 3;
        public float shootDelayTimer = 3;
        public event Action ShootEvent;

        public void Shoot(float gameTime) {
            if (shootDelayTimer > 0) {
                shootDelayTimer -= gameTime;
            }
            if (shootDelayTimer <= 0) {
                ShootEvent?.Invoke();
                shootDelayTimer = shootDelay;
            }
        }
    }

    public class State {
        public event Action OnEnterEvent;
        public event Action<float> OnActEvent;
        public event Action OnExitEvent;

        public virtual void OnEnter() {
            OnEnterEvent?.Invoke();
        }
        public virtual void OnAct(float time) {
            OnActEvent?.Invoke(time);
        }
        public virtual void OnExit() {
            OnExitEvent?.Invoke();
        }
    }
    public class TransitionState : State {
        public StateComponent stateComponent;
        public List<Transition> transitions;

        public TransitionState(StateComponent stateComponent, List<Transition> transitions) {
            this.stateComponent = stateComponent;
            this.transitions = transitions;
        }

        public override void OnEnter() {
            base.OnEnter();
        }
        public override void OnAct(float time) {
            foreach (var transition in transitions) {
                if (transition.CanTransition()) {
                    stateComponent.TransitionToState(transition.TransitionState);
                    return;
                }
            }
            base.OnAct(time);
        }
        public override void OnExit() {
            base.OnExit();
        }
    }
    [System.Serializable]
    public class Transition {
        public RequirementDelegate DecisionGroup;
        public State TransitionState;

        public Transition(RequirementDelegate transitionCondition, State transitionState) {
            this.DecisionGroup = transitionCondition;
            TransitionState = transitionState;
        }
        public bool CanTransition() => DecisionGroup.Invoke();

    }

    public struct RequirementDelegate {
        EEBoolDelegate? EventToCall;

        public delegate bool EEBoolDelegate();

        public bool Invoke() {
            return EventToCall == null || EventToCall.Invoke();
        }
        public void Add(EEBoolDelegate action) {
            EventToCall += action;
        }
    
    }
}