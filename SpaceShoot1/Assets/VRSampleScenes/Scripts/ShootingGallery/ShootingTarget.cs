using System;
using System.Collections;
using UnityEngine;
using VRStandardAssets.Common;
using VRStandardAssets.Utils;

namespace VRStandardAssets.ShootingGallery
{
    // This script handles a target in the shooter scenes.
    // It includes what should happen when it is hit and
    // how long before it despawns.
    public class ShootingTarget : ShootingTargetBase
    {
        public event Action<ShootingTarget> OnRemove;                   // This event is triggered when the target needs to be removed.

        [SerializeField] private int m_Score = 1;                       // This is the amount added to the users score when the target is hit.
        [SerializeField] private float m_DestroyTimeOutDuration = 2f;   // When the target is hit, it shatters.  This is how long before the shattered pieces disappear.
        [SerializeField] private GameObject m_DestroyPrefab;            // The prefab for the shattered target.
        [SerializeField] private AudioClip m_DestroyClip;               // The audio clip to play when the target shatters.
        [SerializeField] private AudioClip m_SpawnClip;                 // The audio clip that plays when the target appears.
        [SerializeField] private AudioClip m_MissedClip;                // The audio clip that plays when the target disappears without being hit.
        [SerializeField] private float m_TargetSpeed = 5;
        [SerializeField] private float m_SpawnScale = 0.5f;
        [SerializeField] protected int m_InitialLifePoints = 4;           // The number of shots the object needs to receive before exploting
        [SerializeField] private Color m_HitColor = Color.red;          // The color of the object when hit.
        [SerializeField] private Color m_InitialColor = Color.white;      // The color of the object when initialized.
        [SerializeField] private TargetType m_TargetType = TargetType.Easy;

        private Transform m_CameraTransform;                            // Used to make sure the target is facing the camera.
        private VRInteractiveItem m_InteractiveItem;                    // Used to handle the user clicking whilst looking at the target.
        private AudioSource m_Audio;                                    // Used to play the various audio clips.
        protected Renderer m_Renderer;                                    // Used to make the target disappear before it is removed.
        private MeshRenderer m_MeshRenderer;                            // Used to change the color of the target when hit.
        protected Collider m_Collider;                                    // Used to make sure the target doesn't interupt other shots happening.
        private bool m_IsEnding;                                        // Whether the target is currently being removed by another source.
        protected int m_CurrentLifePoints;
        private bool m_IgnoreHit = false;
        private bool m_IsPaused = false;
        private Vector3 m_forwardDirection = Vector3.forward;

        public TargetType Type
        {
            get
            {
                return m_TargetType;
            }
        }

        public bool IgnoreHit
        {
            get
            {
                return m_IgnoreHit;
            }

            set
            {
                m_IgnoreHit = value;
            }
        }

        protected float TargetSpeed
        {
            get
            {
                return m_TargetSpeed;
            }

            set
            {
                m_TargetSpeed = value;
            }
        }

        public Vector3 ForwardDirection
        {
            get
            {
                return m_forwardDirection;
            }

            set
            {
                m_forwardDirection = value;
            }
        }


        public void Pause()
        {
            m_IsPaused = true;
        }

        public void Resume()
        {
            m_IsPaused = false;
        }

        private void Update()
        {
            if (m_IsPaused)
                return;

            DoUpdate();
        }

        public override void DoUpdate()
        {
            this.transform.Translate(ForwardDirection * Time.deltaTime * TargetSpeed);

            this.transform.localScale = new Vector3(m_SpawnScale, m_SpawnScale, m_SpawnScale);

            if (this.transform.position.z < 0)
            {
                // The target is out of range
                StartCoroutine(MissTarget());
            }
        }

        protected void Awake()
        {
            // Setup the references.
            m_CameraTransform = Camera.main.transform;
            m_Audio = GetComponent<AudioSource> ();
            m_InteractiveItem = GetComponent<VRInteractiveItem>();
            m_Renderer = GetComponent<Renderer>();
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_Collider = GetComponent<Collider>();
            m_CurrentLifePoints = m_InitialLifePoints;
            m_MeshRenderer.material.color = m_InitialColor;
        }

        private void OnDestroy()
        {
            // Ensure the event is completely unsubscribed when the target is destroyed.
            OnRemove = null;
        }

        public override void Restart (float gameTimeRemaining)
        {
            // When the target is spawned turn the visual and physical aspects on.
            m_Renderer.enabled = true;
            m_Collider.enabled = true;
            m_CurrentLifePoints = m_InitialLifePoints;
            m_MeshRenderer.material.color = m_InitialColor;

            // Since the target has just spawned, it's not ending yet.
            m_IsEnding = false;
            
            // Play the spawn clip.
            m_Audio.clip = m_SpawnClip;
            m_Audio.Play();

            // Make sure the target is facing the camera.
            transform.LookAt(m_CameraTransform);

            // Start the time out for when the game ends.
            // Note this will only come into effect if the game time remaining is less than the time out duration.
            StartCoroutine (GameOver (gameTimeRemaining));
        }
        

        private IEnumerator MissTarget()
        {
            // If by this point it's already ending, do nothing else.
            if(m_IsEnding)
                yield break;

            // Otherwise this is ending the target's lifetime.
            m_IsEnding = true;

            // Turn off the visual and physical aspects.
            m_Renderer.enabled = false;
            m_Collider.enabled = false;
            
            // Play the clip of the target being missed.
            m_Audio.clip = m_MissedClip;
            m_Audio.Play();

            // Wait for the clip to finish.
            yield return new WaitForSeconds(m_MissedClip.length);

            // Tell subscribers that this target is ready to be removed.
            if (OnRemove != null)
                OnRemove(this);
        }


        private IEnumerator GameOver (float gameTimeRemaining)
        {
            // Wait for the game to end.
            yield return new WaitForSeconds (gameTimeRemaining);

            // If by this point it's already ending, do nothing else.
            if(m_IsEnding)
                yield break;

            // Otherwise this is ending the target's lifetime.
            m_IsEnding = true;

            // Turn off the visual and physical aspects.
            m_Renderer.enabled = false;
            m_Collider.enabled = false;

            // Tell subscribers that this target is ready to be removed.
            if (OnRemove != null)
                OnRemove (this);
        }

        public IEnumerator AnimateTargetHit()
        {
            m_MeshRenderer.material.color = m_HitColor;

            // Wait for the target to disappear naturally.
            yield return new WaitForSeconds(0.1f);

            m_MeshRenderer.material.color = m_InitialColor;
        }

        public override void TargetHit(int damage)
        {
            if (IgnoreHit)
                return;

            // If it's already ending, do nothing else.
            if (m_IsEnding)
                return;

            StartCoroutine(AnimateTargetHit());

            if ((m_CurrentLifePoints -= damage) > 0)
                return;

            // Otherwise this is ending the target's lifetime.
            m_IsEnding = true;

            // Turn off the visual and physical aspects.
            m_Renderer.enabled = false;
            m_Collider.enabled = false;

            // Add to the player's score.
            SessionData.AddScore(this.GetComponent<ShootingTarget>().Type);

            PlayTargetDestroy();

            // Tell subscribers that this target is ready to be removed.
            if (OnRemove != null)
                OnRemove(this);
        }

        protected void PlayTargetDestroy()
        {
            // Play the clip of the target being hit.
            m_Audio.clip = m_DestroyClip;
            m_Audio.Play();

            // Instantiate the shattered target prefab in place of this target.
            GameObject destroyedTarget = Instantiate(m_DestroyPrefab, transform.position, transform.rotation) as GameObject;
            destroyedTarget.transform.localScale = new Vector3(m_SpawnScale, m_SpawnScale, m_SpawnScale);
            // Destroy the shattered target after it's time out duration.
            Destroy(destroyedTarget, m_DestroyTimeOutDuration);
        }
    }
}