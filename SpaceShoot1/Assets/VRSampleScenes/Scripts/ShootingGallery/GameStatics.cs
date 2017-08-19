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
        List<TargetConfiguration> wave1 = GenerateTargetSequence(10, TargetType.Easy);
        GameScore gameScore1 = new GameScore();
        gameScore1.SetScore(TargetType.Easy, 10);
        level1.WaveConfig.Add(new WaveConfiguration(waveCounter++, gameScore1, wave1));

        // Wave 2
        List<SequenceCofig> squenceConfig = new List<SequenceCofig>();
        squenceConfig.Add(new SequenceCofig(10, TargetType.Easy));
        squenceConfig.Add(new SequenceCofig(5, TargetType.Medium));
        List<TargetConfiguration> wave2 = new List<TargetConfiguration>(GenerateRandomSequence(squenceConfig));
        GameScore gameScore2 = new GameScore();
        gameScore2.SetScore(TargetType.Easy, 10);
        gameScore2.SetScore(TargetType.Medium, 5);
        level1.WaveConfig.Add(new WaveConfiguration(waveCounter++, gameScore2, wave2));

        // Wave 3
        List<SequenceCofig> targetConfig2 = new List<SequenceCofig>();
        targetConfig2.Add(new SequenceCofig(10, TargetType.Medium));
        targetConfig2.Add(new SequenceCofig(5, TargetType.Hard));
        List<TargetConfiguration> wave3 = new List<TargetConfiguration>(GenerateRandomSequence(targetConfig2));
        GameScore gameScore3 = new GameScore();
        gameScore3.SetScore(TargetType.Medium, 10);
        gameScore3.SetScore(TargetType.Hard, 5);
        level1.WaveConfig.Add(new WaveConfiguration(waveCounter++, gameScore3, wave3));

        levels.Add(level1);
        return levels;
    }

    private static List<TargetConfiguration> GenerateTargetSequence(int targetCount, TargetType targetType)
    {
        List<TargetConfiguration> targets = new List<TargetConfiguration>();
        
        for (int i = 0; i != targetCount; i++)
        {
            targets.Add(new TargetConfiguration(targetType));
        }

        return targets;
    }

    private static List<TargetConfiguration> GenerateRandomSequence(IEnumerable<SequenceCofig> squenceConfigurations)
    {
        SortedDictionary<int, TargetConfiguration> sequence = new SortedDictionary<int, TargetConfiguration>();

        Random random = new Random();
        

        foreach (SequenceCofig configuration in squenceConfigurations)
        {
            for (int i = 0; i != configuration.CountOfItemsToCreate; i++)
            {
                int key = random.Next();

                while (sequence.ContainsKey(key))
                {
                    key = random.Next();
                }

                sequence.Add(key, new TargetConfiguration(configuration.Type));
            }
        }

        List<TargetConfiguration> targets = new List<TargetConfiguration>();
        foreach(var item in sequence)
        {
            targets.Add(item.Value);
        }

        return targets;
    }

    private class SequenceCofig
    {
        public SequenceCofig(int count, TargetType type)
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

        public int CountOfItemsToCreate
        {
            get { return m_count; }
            set { m_count = value; }
        }

    }
}