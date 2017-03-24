using System;
using System.Collections.Generic;
using VRStandardAssets.Common;
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

        // Wave 1
        List<TargetType> wave1 = GenerateTargetSequence(10, TargetType.Easy);
        GameScore gameScore1 = new GameScore();
        gameScore1.SetScore(TargetType.Easy, 10);
        level1.WaveConfig.Add(new WaveConfiguration(waveCounter++, gameScore1, "destroy at last 10 asteroids", wave1));

        // Wave 2
        List<TargetConfig> targetConfig = new List<TargetConfig>();
        targetConfig.Add(new TargetConfig(10, TargetType.Easy));
        targetConfig.Add(new TargetConfig(5, TargetType.Medium));
        List<TargetType> wave2 = new List<TargetType>(GenerateRandomSequence(targetConfig));
        GameScore gameScore2 = new GameScore();
        gameScore2.SetScore(TargetType.Easy, 10);
        gameScore2.SetScore(TargetType.Medium, 5);
        level1.WaveConfig.Add(new WaveConfiguration(waveCounter++, gameScore2, "destroy at last 15 asteroids", wave2));

        // Wave 3
        List<TargetConfig> targetConfig2 = new List<TargetConfig>();
        targetConfig2.Add(new TargetConfig(10, TargetType.Medium));
        targetConfig2.Add(new TargetConfig(5, TargetType.HardDouble));
        List<TargetType> wave3 = new List<TargetType>(GenerateRandomSequence(targetConfig2));
        GameScore gameScore3 = new GameScore();
        gameScore3.SetScore(TargetType.Medium, 10);
        gameScore3.SetScore(TargetType.HardDouble, 5);
        level1.WaveConfig.Add(new WaveConfiguration(waveCounter++, gameScore3, "destroy at last 15 asteroids", wave3));

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