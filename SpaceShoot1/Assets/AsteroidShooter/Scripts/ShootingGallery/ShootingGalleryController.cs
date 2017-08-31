using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Common;
using VRStandardAssets.Utils;

namespace VRStandardAssets.ShootingGallery
{
    // This class controls the flow of the shooter games.  It
    // includes the introduction, spawning of targets and
    // outro.  Targets are spawned with a system that makes
    // spawnning more likely when there are fewer.
    public class ShootingGalleryController : MonoBehaviour
    {
        [SerializeField] private int m_IdealTargetNumber = 5;           // How many targets aim to be on screen at once.
        [SerializeField] private float m_BaseSpawnProbability = 0.7f;   // When there are the ideal number of targets, this is the probability another will spawn.
        [SerializeField] private float m_GameLength = 60f;              // Time a game lasts in seconds.
        [SerializeField] private float m_SpawnInterval = 1f;            // How frequently a target could spawn.
        [SerializeField] private float m_EndDelay = 1.5f;               // The time the user needs to wait between the outro UI and being able to continue.
        [SerializeField] private WaveSelectionController m_WaveSelectionController;     // Used to let the user pick the wave she is playing.
        [SerializeField] private SelectionRadial m_SelectionRadial;     // Used to continue past the outro.
        [SerializeField] private Reticle m_Reticle;                     // This is turned on and off when it is required and not.
        [SerializeField] private ObjectPool m_TargetObjectPool;         // The object pool that stores all the targets.
        [SerializeField] private BoxCollider m_frontSpawnCollider;      // The volume that the targets can spawn within.
        [SerializeField] private BoxCollider m_frontRightSpawnCollider;      // The volume that the targets can spawn within.
        [SerializeField] private BoxCollider m_rightSpawnCollider;      // The volume that the targets can spawn within.
        [SerializeField] private UIController m_UIController;           // Used to encapsulate the UI.
        [SerializeField] private InputWarnings m_InputWarnings;         // Tap warnings need to be on for the intro and outro but off for the game itself.
        [SerializeField] private GameConfiguration m_GameConfiguration;
        [SerializeField] private VRInput m_VRInput;

        private float m_SpawnProbability;                               // The current probability that a target will spawn at the next interval.
        private float m_ProbabilityDelta;                               // The difference to the probability caused by a target spawning or despawning.
        private List<TargetConfiguration>.Enumerator m_TargetSequence;

        private List<ShootingTarget> m_OutstandingTargets = new List<ShootingTarget>();
        private Coroutine m_WaitForWaveSalection = null;

        private GameStateMachine m_GameStateMachine = new GameStateMachine();

        // Whether or not the game is currently playing.
        public bool IsPlaying
        {
            get
            {
                return m_GameStateMachine.CurrentState == GameStateMachine.GameState.Playing;
            }
        }

        private void Awake()
        {
            SessionData.RestoreLastGameData();
        }

        private void OnEnable()
        {
            m_VRInput.OnCancel += HandleCancel;
            m_GameStateMachine.OnGameStateChanged += HandleGameStateChanged;
        }

        private void OnDisable()
        {
            m_VRInput.OnCancel -= HandleCancel;
            m_GameStateMachine.OnGameStateChanged -= HandleGameStateChanged;
        }

        private void HandleGameStateChanged(object sender, GameStateMachine.GameStateChangedEventArgs e)
        {
            switch (e.CurrentState)
            {
                case GameStateMachine.GameState.MainMenu:
                    StartCoroutine(GameMainMenu());
                    break;
                case GameStateMachine.GameState.Playing:
                    StartCoroutine(PlayGame());
                    break;
                case GameStateMachine.GameState.QuitGame:
                    Application.Quit();
                    break;
                default:
                    Debug.LogWarningFormat("Not handled state: {0}", e.CurrentState);
                    break;
            }
        }

        private void HandleCancel()
        {
            var newState = m_GameStateMachine.SetCommand(GameStateMachine.Command.CancelPressed);

        }

        private void Start()
        {
            // Set the game type for the score to be recorded correctly.
            SessionData.Restart();

            // The probability difference for each spawn is difference between 100% and the base probabilty per the number or targets wanted.
            // That means that if the ideal number of targets was 5, the base probability was 0.7 then the delta is 0.06.
            // So if there are no targets, the probability of one spawning will be 1, then 0.94, then 0.88, etc.
            m_ProbabilityDelta = (1f - m_BaseSpawnProbability) / m_IdealTargetNumber;

            m_GameStateMachine.SetCommand(GameStateMachine.Command.Begin);
        }

        private IEnumerator PauseGame()
        {
            m_Reticle.Hide();
            m_SelectionRadial.Hide();

            for (int i = m_OutstandingTargets.Count - 1; i >= 0; i--)
            {
                m_OutstandingTargets[i].Pause();
            }

            //StartCoroutine(InitializeGame());
            return null;
        }

        private IEnumerator PlayGame()
        {
            // Continue looping through all the phases.
            while (true)
            {
                yield return StartCoroutine(GameStartWave());
                yield return StartCoroutine(GamePlayWave());
                yield return StartCoroutine(GameEndWave());
            }
        }

        private IEnumerator GameMainMenu()
        {
            // Show VR buttons
            m_WaveSelectionController.ShowWaveButtons();

            // Wait for the intro UI to fade in.
            yield return StartCoroutine(m_UIController.ShowIntroUI());

            // Turn on the tap warnings for the selection slider.
            m_InputWarnings.TurnOnDoubleTapWarnings();
            m_InputWarnings.TurnOnSingleTapWarnings();

            // Show the reticle (since there is now a selection slider) and hide the radial.
            m_Reticle.Show();
            m_SelectionRadial.Hide();

            // Wait for the user to select a wave
            m_WaitForWaveSalection = StartCoroutine(m_WaveSelectionController.WaitForWaveSelection());
            yield return m_WaitForWaveSalection;
            m_WaitForWaveSalection = null;

            m_GameConfiguration.SetInitialWave(m_WaveSelectionController.SelectedWave);
            m_GameStateMachine.SetCommand(GameStateMachine.Command.WaveSelected);
        }

        private IEnumerator GameStartWave ()
        {
            foreach (var target in m_OutstandingTargets)
            {
                target.Resume();
            }

            UpdateLevelInfo();

            // Turn off the tap warnings since it will now be tap to fire.
            m_InputWarnings.TurnOffDoubleTapWarnings ();
            m_InputWarnings.TurnOffSingleTapWarnings ();

            // Wait for the intro UI to fade out.
            yield return StartCoroutine (m_UIController.HideIntroUI ());
        }

        private IEnumerator GamePlayWave ()
        {
            // Wait for the UI on the player's gun to fade in.
            yield return StartCoroutine(m_UIController.ShowPlayerUI());

            // Make sure the reticle is being shown.
            m_Reticle.Show ();

            // Reset the score.
            SessionData.Restart ();

            // Wait for the play updates to finish.
            yield return StartCoroutine (PlayUpdate ());
        }

        private IEnumerator GameEndWave ()
        {
            // TODO: send stats to ensure the player passed or failed the wave
            PhaseResult result = m_GameConfiguration.FinishPhase(SessionData.Score);

            SessionData.SaveGame();

            // Wait for the UI on the player's gun to fade out.
            yield return StartCoroutine(m_UIController.HidePlayerUI());

            if (!result.Pass || result.IsGameEnd)
            {
                // Hide the reticle since the radial is about to be used.
                m_Reticle.Hide();

                // In order, wait for the outro UI to fade in then wait for an additional delay.
                yield return StartCoroutine(m_UIController.ShowOutroUI(result.Message));
                yield return new WaitForSeconds(m_EndDelay);

                // Turn on the tap warnings.
                m_InputWarnings.TurnOnDoubleTapWarnings();
                m_InputWarnings.TurnOnSingleTapWarnings();

                // Wait for the radial to fill (this will show and hide the radial automatically).
                yield return StartCoroutine(m_SelectionRadial.WaitForSelectionRadialToFill());

                // The radial is now filled so stop the warnings.
                m_InputWarnings.TurnOffDoubleTapWarnings();
                m_InputWarnings.TurnOffSingleTapWarnings();
                // Wait for the outro UI to fade out.
                yield return StartCoroutine(m_UIController.HideOutroUI());
            }
            else
            {
                yield return StartCoroutine(m_UIController.ShowEndOfWaveUI(result.Message));
                yield return new WaitForSeconds(0.5f);
                yield return StartCoroutine(m_UIController.HideEndOfWaveUI());
            }
        }

        private void UpdateLevelInfo()
        {
            var currentLevel = m_GameConfiguration.GetCurrentLevel();
            var currentWave = currentLevel.GetCurrentWave();
            m_TargetSequence = currentWave.TargetSequence.GetEnumerator();
            m_TargetSequence.MoveNext();

            SessionData.Level = currentLevel.LevelNumber;
            SessionData.WaveCount = currentWave.WaveNumber;
            SessionData.MinScoreToPassWave = currentWave.MinScoreToPass;
        }

        private IEnumerator PlayUpdate ()
        {
            // When the updates start, the probability of a target spawning is 100%.
            m_SpawnProbability = 1f;

            // The time remaining is the full length of the game length.
            float gameTimer = m_GameLength;

            // The amount of time before the next spawn is the full interval.
            float spawnTimer = m_SpawnInterval;

            // While there is still time remaining...
            while (gameTimer > 0f)
            {
                // ... check if the timer for spawning has reached zero.
                if (spawnTimer <= 0f)
                {
                    // If it's time to spawn, check if a spawn should happen based on the probability.
                    if (UnityEngine.Random.value < m_SpawnProbability)
                    {
                        // If a spawn should happen, restart the timer for spawning.
                        spawnTimer = m_SpawnInterval;

                        // Decrease the probability of a spawn next time because there are now more targets.
                        m_SpawnProbability -= m_ProbabilityDelta;

                        // Spawn a target.
                        Spawn (gameTimer, m_TargetSequence.Current);

                        if (!m_TargetSequence.MoveNext())
                        {
                            while(m_OutstandingTargets.Count > 0)
                            {
                                // Wait until all the targets are either destroyed or out of the players view
                                yield return null;
                            }
                            break;
                        }
                    }
                }

                // Wait for the next frame.
                yield return null;

                // Decrease the timers by the time that was waited.
                gameTimer -= Time.deltaTime;
                spawnTimer -= Time.deltaTime;
            }
        }
        
        private void Spawn (float timeRemaining, TargetConfiguration targetConfig)
        {
            // Get a reference to a target instance from the object pool.
            GameObject target = m_TargetObjectPool.GetGameObjectFromPool (targetConfig.Type);

            // Set the target's position to a random position. 
            SetTargetSpawnProperties(targetConfig.SpawnPosition, ref target);

            // Find a reference to the ShootingTarget script on the target gameobject and call it's Restart function.
            ShootingTarget shootingTarget = target.GetComponent<ShootingTarget>();
            shootingTarget.Restart(timeRemaining);

            // Subscribe to the OnRemove event.
            shootingTarget.OnRemove += HandleTargetRemoved;

            m_OutstandingTargets.Add(shootingTarget);
        }

        private void SetTargetSpawnProperties (TargetSpawnPosition spawnPosition, ref GameObject target)
        {
            BoxCollider spawnCollider;
            ShootingTarget shootingTarget = target.GetComponent<ShootingTarget>();

            switch (spawnPosition)
            {
                case TargetSpawnPosition.Front:
                    spawnCollider = m_frontSpawnCollider;
                    break;
                case TargetSpawnPosition.FrontRight:
                    spawnCollider = m_frontRightSpawnCollider;
                    break;
                case TargetSpawnPosition.Right:
                    spawnCollider = m_rightSpawnCollider;
                    break;
                default:
                    spawnCollider = m_frontSpawnCollider;
                    break;
            }

            // Find the centre and extents of the spawn collider.
            Vector3 center = spawnCollider.bounds.center;
            Vector3 extents = spawnCollider.bounds.extents;

            // Get a random value between the extents on each axis.
            float x = UnityEngine.Random.Range(center.x - extents.x, center.x + extents.x);
            float y = UnityEngine.Random.Range(center.y - extents.y, center.y + extents.y);
            float z = UnityEngine.Random.Range(center.z - extents.z, center.z + extents.z);

            target.transform.position = new Vector3(x, y, z);

            // Set the direction of the movement for the new shooting target the same as the forward direction the spawn collider is facing
            shootingTarget.ForwardDirection = spawnCollider.transform.forward;
        }

        private void HandleTargetRemoved(ShootingTarget target)
        {
            // Now that the event has been hit, unsubscribe from it.
            target.OnRemove -= HandleTargetRemoved;

            m_OutstandingTargets.Remove(target);

            // Return the target to it's object pool.
            m_TargetObjectPool.ReturnGameObjectToPool (target.gameObject);

            // Increase the likelihood of a spawn next time because there are fewer targets now.
            m_SpawnProbability += m_ProbabilityDelta;
        }
    }
}