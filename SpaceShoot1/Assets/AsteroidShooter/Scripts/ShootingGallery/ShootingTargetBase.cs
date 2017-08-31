using System;
using System.Collections;
using UnityEngine;

namespace VRStandardAssets.ShootingGallery
{
    public abstract class ShootingTargetBase : MonoBehaviour
    {
        public abstract void Restart();
        public abstract void DoUpdate();
        public abstract void TargetHit(int damage);
    }
}