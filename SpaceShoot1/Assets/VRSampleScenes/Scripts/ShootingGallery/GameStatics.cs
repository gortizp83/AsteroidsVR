using System;
using System.Collections.Generic;
using TargetType = VRStandardAssets.ShootingGallery.ShootingTarget.TargetType;

internal class GameStatics
{
    internal static List<LevelConfiguration> GetLevels()
    {
        int levelCount = 1;
        int waveCounter = 1;

        var levels = new List<LevelConfiguration>();
        // Level 1
        LevelConfiguration level1 = new LevelConfiguration(levelCount++);
        List<TargetType> sequence = new List<TargetType> { TargetType.Easy, TargetType.Easy, TargetType.Easy, TargetType.Easy, TargetType.Easy, TargetType.Easy, TargetType.Easy, TargetType.Easy, TargetType.Easy, TargetType.Easy };
        level1.WaveConfig.Add( new WaveConfiguration(waveCounter++, 10, "destroy at last 10 asteroids", sequence));

        sequence = new List<TargetType> { TargetType.Easy, TargetType.Medium, TargetType.Easy, TargetType.Hard };
        level1.WaveConfig.Add(new WaveConfiguration(waveCounter++, 15, "destroy at last 15 asteroids", sequence));

        levels.Add(level1);
        return levels;
    }
}