using System;
using System.Collections.Generic;
using VRStandardAssets.ShootingGallery;

internal class GameStatics
{
    internal static List<LevelConfiguration> GetLevels()
    {
        int levelCount = 1;
        int waveCounter = 1;

        var levels = new List<LevelConfiguration>();
        // Level 1
        LevelConfiguration level1 = new LevelConfiguration(levelCount++);
        List<TargetType> wave1 = GenerateTargetSequence(10, TargetType.Easy);
        level1.WaveConfig.Add( new WaveConfiguration(waveCounter++, 10, "destroy at last 10 asteroids", wave1));

        List<TargetConfig> targetConfig = new List<TargetConfig>();
        targetConfig.Add(new TargetConfig(10, TargetType.Easy));
        targetConfig.Add(new TargetConfig(5, TargetType.Medium));
        List<TargetType> wave2 = new List<TargetType>(GenerateRandomSequence(targetConfig));

        level1.WaveConfig.Add(new WaveConfiguration(waveCounter++, 15, "destroy at last 15 asteroids", wave2));

        levels.Add(level1);
        return levels;
    }

    private static List<TargetType> GenerateTargetSequence(int targetCount, TargetType targetType)
    {
        List<TargetType> targets = new List<TargetType>();
        
        for (int i = 0; i != targetCount; i++)
        {
            targets.Add(targetType);
        }

        return targets;
    }

    private static List<TargetType> GenerateRandomSequence(IEnumerable<TargetConfig> targetConfigList)
    {
        SortedDictionary<int, TargetType> sequence = new SortedDictionary<int, TargetType>();

        Random random = new Random();
        

        foreach (TargetConfig targetConfig in targetConfigList)
        {
            for (int i = 0; i != targetConfig.Count; i++)
            {
                int key = random.Next();

                while (sequence.ContainsKey(key))
                {
                    key = random.Next();
                }

                sequence.Add(key, targetConfig.Type);
            }
        }

        List<TargetType> targets = new List<TargetType>();
        foreach(var item in sequence)
        {
            targets.Add(item.Value);
        }

        return targets;
    }

    private class TargetConfig
    {
        public TargetConfig(int count, TargetType type)
        {
            m_count = count;
            m_type = type;
        }

        private TargetType m_type;

        public TargetType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }


        private int m_count;

        public int Count
        {
            get { return m_count; }
            set { m_count = value; }
        }

    }
}