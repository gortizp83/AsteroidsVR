using System;
using System.Collections.Generic;
using VRStandardAssets.ShootingGallery;

namespace VRStandardAssets.Common
{
    public class GameScore
    {
        private Dictionary<TargetType, int> m_ScoreMap = new Dictionary<TargetType, int>();

        public static bool operator <=(GameScore first, GameScore second)
        {
            var values = Enum.GetValues(typeof(TargetType));

            foreach (var value in values)
            {
                TargetType type = (TargetType)value;
                if (first.GetScore(type) > second.GetScore(type))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator >=(GameScore first, GameScore second)
        {
            var values = Enum.GetValues(typeof(TargetType));

            foreach (var value in values)
            {
                TargetType type = (TargetType)value;
                if (first.GetScore(type) < second.GetScore(type))
                {
                    return false;
                }
            }

            return true;
        }

        public void AddScore(TargetType targetType)
        {
            if (m_ScoreMap.ContainsKey(targetType))
            {
                m_ScoreMap[targetType]++;
            }
            else
            {
                m_ScoreMap.Add(targetType, 1);
            }
        }

        public int GetScore(TargetType targetType)
        {
            if (m_ScoreMap.ContainsKey(targetType))
            {
                return m_ScoreMap[targetType];
            }
            else
            {
                return 0;
            }
        }

        internal void SetScore(TargetType targetType, int score)
        {
            m_ScoreMap.Add(targetType, score);
        }

        public void Reset()
        {
            m_ScoreMap.Clear();
        }
    }
}