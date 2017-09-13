using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Common;
using VRStandardAssets.ShootingGallery;

public class WaveConfiguration
{
    public WaveConfiguration(int waveNumber, List<TargetConfiguration> targetSequence)
    {
        WaveNumber = waveNumber;
        MinScoreToPass = CalculateMinScoreToPass(targetSequence);
        TargetSequence = targetSequence;
        InitializeWaveConfiguration();
    }

    public WaveConfiguration(int waveNumber, GameScore minScoreToPass, List<TargetConfiguration> targetSequence)
    {
        WaveNumber = waveNumber;
        MinScoreToPass = minScoreToPass;
        TargetSequence = targetSequence;
        InitializeWaveConfiguration();
    }

    private void InitializeWaveConfiguration()
    {
        WaveTrainingConfiguration = null;
        MaxPowerRingsToFill = 2;
    }

    public int WaveNumber { get; set; }

    public GameScore MinScoreToPass { get; set; }

    public List<TargetConfiguration> TargetSequence { get; set; }

    public TargetType? WaveTrainingConfiguration {get; set;}

    public int MaxPowerRingsToFill { get; set; }

    private GameScore CalculateMinScoreToPass(List<TargetConfiguration> targetSequence)
    {
        GameScore score = new GameScore();
        Dictionary<TargetType, int> targetTypeToCountMap = new Dictionary<TargetType, int>();

        foreach(var config in targetSequence)
        {
            if (targetTypeToCountMap.ContainsKey(config.Type))
            {
                targetTypeToCountMap[config.Type]++;
            }
            else
            {
                targetTypeToCountMap[config.Type] = 1;
            }
        }

        foreach (var targetTypeCount in targetTypeToCountMap)
        {
            score.SetScore(targetTypeCount.Key, targetTypeCount.Value);
        }

        return score;
    }
}