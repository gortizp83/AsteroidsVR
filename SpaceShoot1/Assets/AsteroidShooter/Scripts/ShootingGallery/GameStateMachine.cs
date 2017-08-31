using System.Collections.Generic;

namespace VRStandardAssets.ShootingGallery
{
    public class GameStateMachine
    {
        public enum Command
        {
            Begin,
            CancelPressed,
            WaveSelected,
            Resume,
        }
        public enum GameState
        {
            None,
            MainMenu,
            Playing,
            Paused,
            QuitGame,
        }

        class StateTransition
        {
            readonly GameState CurrentState;
            readonly Command Command;

            public StateTransition(GameState currentState, Command command)
            {
                CurrentState = currentState;
                Command = command;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
            }
        }

        Dictionary<StateTransition, GameState> transitions;
        public GameState CurrentState { get; private set; }

        public class GameStateChangedEventArgs
        {
            public GameStateChangedEventArgs(GameState oldState, GameState currentState)
            {
                OldState = oldState;
                CurrentState = currentState;
            }
            public GameState OldState { get; private set; }
            public GameState CurrentState { get; private set; }
        }

        public delegate void GameStateChangedEventHandler(object sender, GameStateChangedEventArgs e);
        public event GameStateChangedEventHandler OnGameStateChanged;

        public GameStateMachine()
        {
            CurrentState = GameState.None;
            transitions = new Dictionary<StateTransition, GameState>
        {
            {new StateTransition(GameState.None, Command.CancelPressed), GameState.QuitGame },
            {new StateTransition(GameState.None, Command.Begin), GameState.MainMenu },
            {new StateTransition(GameState.MainMenu, Command.CancelPressed), GameState.QuitGame },
            {new StateTransition(GameState.MainMenu, Command.WaveSelected), GameState.Playing },
            {new StateTransition(GameState.Playing, Command.CancelPressed), GameState.Paused },
            {new StateTransition(GameState.Paused, Command.CancelPressed), GameState.MainMenu },
            {new StateTransition(GameState.Paused, Command.Resume), GameState.Playing }
        };
        }

        public GameState SetCommand(Command command)
        {
            GameState nextState;
            if (!transitions.TryGetValue(new StateTransition(CurrentState, command), out nextState))
            {
                var message = string.Format("There isn't any transition from the state {0} using the command {1}", CurrentState, command);
                throw new KeyNotFoundException(message);
            }

            if (CurrentState != nextState)
            {
                OnGameStateChanged(this, new GameStateChangedEventArgs(CurrentState, nextState));
                CurrentState = nextState;
            }

            return nextState;
        }
    }
}
