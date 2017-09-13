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

        {
            // Wave 1
            List<TargetConfiguration> wave1 = GenerateTargetSequence(10, TargetType.Easy);
            GameScore gameScore1 = new GameScore();
            gameScore1.SetScore(TargetType.Easy, 10);
            var wave1Config = new WaveConfiguration(waveCounter++, gameScore1, wave1);
            wave1Config.MaxPowerRingsToFill = 0;
            level1.WaveConfig.Add(wave1Config);
        }

        // Wave 2
        List<SequenceCofig> squenceConfig = new List<SequenceCofig>();
        squenceConfig.Add(new SequenceCofig(10, TargetType.Easy));
        squenceConfig.Add(new SequenceCofig(5, TargetType.Medium));
        List<TargetConfiguration> wave2 = new List<TargetConfiguration>(GenerateRandomSequence(squenceConfig));
        GameScore gameScore2 = new GameScore();
        gameScore2.SetScore(TargetType.Easy, 10);
        gameScore2.SetScore(TargetType.Medium, 5);
        WaveConfiguration wave2Configuration = new WaveConfiguration(waveCounter++, gameScore2, wave2);
        wave2Configuration.WaveTrainingConfiguration = TargetType.Medium;
        wave2Configuration.MaxPowerRingsToFill = 1;
        level1.WaveConfig.Add(wave2Configuration);

        // Wave 3
        List<SequenceCofig> targetConfig2 = new List<SequenceCofig>();
        targetConfig2.Add(new SequenceCofig(10, TargetType.Medium));
        targetConfig2.Add(new SequenceCofig(5, TargetType.Hard));
        List<TargetConfiguration> wave3 = new List<TargetConfiguration>(GenerateRandomSequence(targetConfig2));
        GameScore gameScore3 = new GameScore();
        gameScore3.SetScore(TargetType.Medium, 10);
        gameScore3.SetScore(TargetType.Hard, 5);
        level1.WaveConfig.Add(new WaveConfiguration(waveCounter++, gameScore3, wave3));

        // Wave 4
        List<SequenceCofig> sequenceConfig4 = new List<SequenceCofig>();
        sequenceConfig4.Add(new SequenceCofig(5, new TargetConfiguration(TargetType.Easy, SpawnPosition.FrontRight)));
        sequenceConfig4.Add(new SequenceCofig(5, new TargetConfiguration(TargetType.Easy, SpawnPosition.Right)));
        List<TargetConfiguration> wave4 = new List<TargetConfiguration>(GenerateRandomSequence(sequenceConfig4));
        level1.WaveConfig.Add(new WaveConfiguration(waveCounter++, wave4));

        const float c_sideSpeeSlow = 7.5f;
        // Wave 5
        List<SequenceCofig> sequenceConfig5 = new List<SequenceCofig>();
        sequenceConfig4.Add(new SequenceCofig(2, new TargetConfiguration(TargetType.Easy, SpawnPosition.Right, c_sideSpeeSlow)));
        sequenceConfig5.Add(new SequenceCofig(3, new TargetConfiguration(TargetType.Medium, SpawnPosition.FrontRight, c_sideSpeeSlow)));
        sequenceConfig5.Add(new SequenceCofig(3, new TargetConfiguration(TargetType.Medium, SpawnPosition.Right, c_sideSpeeSlow)));
        sequenceConfig5.Add(new SequenceCofig(2, new TargetConfiguration(TargetType.Hard, SpawnPosition.FrontRight, c_sideSpeeSlow)));
        List<TargetConfiguration> wave5 = new List<TargetConfiguration>(GenerateRandomSequence(sequenceConfig5));
        level1.WaveConfig.Add(new WaveConfiguration(waveCounter++, wave5));

        levels.Add(level1);
        return levels;
    }

    private static List<TargetConfiguration> GenerateTargetSequence(int targetCount, TargetType targetType, SpawnPosition spawnPosition = SpawnPosition.Front)
    {
        List<TargetConfiguration> targets = new List<TargetConfiguration>();
        
        for (int i = 0; i != targetCount; i++)
        {
            targets.Add(new TargetConfiguration(targetType, spawnPosition));
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

                sequence.Add(key, configuration.TargetConfiguration);
            }
        }

        List<TargetConfiguration> targets = new List<TargetConfiguration>();
        foreach(var item in sequence)
        {
            targets.Add(item.Value);
        }

        return targets;
    }

    private struct SequenceCofig
    {
        public SequenceCofig(int countOfItemsToCreate, TargetType targetType)
        {
            CountOfItemsToCreate = countOfItemsToCreate;
            TargetConfiguration = new TargetConfiguration(targetType);
        }

        public SequenceCofig(int countOfItemsToCreate, TargetConfiguration targetConfiguration)
        {
            CountOfItemsToCreate = countOfItemsToCreate;
            TargetConfiguration = targetConfiguration;
        }

        public TargetConfiguration TargetConfiguration;
        public int CountOfItemsToCreate;
    }
}