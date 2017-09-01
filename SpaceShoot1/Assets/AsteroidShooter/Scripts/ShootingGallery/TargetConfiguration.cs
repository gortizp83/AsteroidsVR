using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRStandardAssets.ShootingGallery
{
    public enum TargetType
    {
        Easy,
        Medium,
        Hard,
        EasyDouble,
        MediumDouble,
        HardDouble,
    }

    public enum SpawnPosition
    {
        Front,
        FrontRight,
        Right
    }

    public class TargetConfiguration
    {
        public TargetConfiguration(
            TargetType type,
            SpawnPosition spawnPosition = SpawnPosition.Front,
            float? overrideTargetSpeed = null)
        {
            Type = type;
            SpawnPosition = spawnPosition;

            OverrideTargetSpeed = overrideTargetSpeed;
        }

        public TargetType Type { get; set; }

        public SpawnPosition SpawnPosition { get; set; }

        public float? OverrideTargetSpeed { get; private set; }
    }
}
