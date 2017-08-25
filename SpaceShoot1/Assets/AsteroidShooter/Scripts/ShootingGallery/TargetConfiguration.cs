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

    public enum TargetSpawnPosition
    {
        Front,
        FrontRight,
        Right
    }

    public class TargetConfiguration
    {
        private TargetType m_type;
        private TargetSpawnPosition m_spawnPosition;

        public TargetConfiguration(TargetType type, TargetSpawnPosition spawnPosition = TargetSpawnPosition.Front)
        {
            m_type = type;
            m_spawnPosition = spawnPosition;
        }

        public TargetType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        public TargetSpawnPosition SpawnPosition
        {
            get { return m_spawnPosition; }
            set { m_spawnPosition = value; }
        }
    }
}
