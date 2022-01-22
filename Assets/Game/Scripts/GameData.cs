using System;

public static class GameData
{
    private static float _score;
    public static float score
    {
        get => _score;
        set
        {
            _score = value;
            onScoreChanged?.Invoke(_score);
        }
    }
    public static Action<float> onScoreChanged;
}