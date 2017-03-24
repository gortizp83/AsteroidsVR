using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRStandardAssets.Common;
using VRStandardAssets.Utils;
using System;

namespace VRStandardAssets.ShootingGallery
{
    // This simple class encapsulates the UI for
    // the shooter scenes so that the game
    // controller need only reference one thing to
    // control the UI during the games.
    public class UIController : MonoBehaviour
    {
        [SerializeField] private UIFader m_IntroUI;     // This controls fading the UI shown during the intro.
        [SerializeField] private UIFader m_OutroUI;     // This controls fading the UI shown during the outro.
        [SerializeField] private UIFader m_PlayerUI;    // This controls fading the UI that shows around the gun that moves with the player.
        [SerializeField] private Text m_TotalScore;     // Reference to the Text component that displays the player's score at the end.
        [SerializeField] private Text m_ScoreNeeded;     // Reference to the Text component that displays the player's score at the end.
        [SerializeField] private Text m_EndOfWaveMessage;      // Reference to the Text component that displays the end of wave message
        [SerializeField] private Text m_Wave;           // Reference to the Text component that displays the current wave user is playing.
        [SerializeField] private Text m_Level;          // Reference to the Text component that displays the current level user is playing.
        [SerializeField] private Text m_WaveGoal;       // Reference to the Text component that displays the goals of the current wave.

        public IEnumerator ShowIntroUI()
        {
            m_Wave.text = SessionData.Wave.ToString();
            m_Level.text = SessionData.Level.ToString();
            m_WaveGoal.text = SessionData.CurrentWaveGoals;

            yield return StartCoroutine(m_IntroUI.InteruptAndFadeIn());
        }


        public IEnumerator HideIntroUI()
        {
            yield return StartCoroutine(m_IntroUI.InteruptAndFadeOut());
        }


        public IEnumerator ShowOutroUI()
        {
            m_TotalScore.text = SessionData.Score.ToString();
            m_EndOfWaveMessage.text = "Game is ending";

            yield return StartCoroutine(m_OutroUI.InteruptAndFadeIn());
        }

        internal IEnumerator ShowOutroUI(PhaseResult result)
        {
            m_TotalScore.text = SessionData.Score.ToString();
            m_ScoreNeeded.text = result.MinScoreToPass.ToString();
            m_EndOfWaveMessage.text = result.IsGameEnd? "Game End. Play again?" : result.Message;

            yield return StartCoroutine(m_OutroUI.InteruptAndFadeIn());
        }

        public IEnumerator HideOutroUI()
        {
            yield return StartCoroutine(m_OutroUI.InteruptAndFadeOut());
        }


        public IEnumerator ShowPlayerUI ()
        {
            yield return StartCoroutine (m_PlayerUI.InteruptAndFadeIn ());
        }


        public IEnumerator HidePlayerUI ()
        {
            yield return StartCoroutine (m_PlayerUI.InteruptAndFadeOut ());
        }
    }
}