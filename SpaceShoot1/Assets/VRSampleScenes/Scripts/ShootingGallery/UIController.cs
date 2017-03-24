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
        [SerializeField] private Text m_EndOfWaveMessage;      // Reference to the Text component that displays the end of wave message

        public IEnumerator ShowIntroUI()
        {
            yield return StartCoroutine(m_IntroUI.InteruptAndFadeIn());
        }


        public IEnumerator HideIntroUI()
        {
            yield return StartCoroutine(m_IntroUI.InteruptAndFadeOut());
        }


        public IEnumerator ShowOutroUI()
        {
            m_EndOfWaveMessage.text = "Game is ending";

            yield return StartCoroutine(m_OutroUI.InteruptAndFadeIn());
        }

        internal IEnumerator ShowOutroUI(PhaseResult result)
        {
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