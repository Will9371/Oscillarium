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
    
    private static float _hits;
    public static float hits
    {
        get => _hits;
        set
        {
            _hits = value;
            onHitsChanged?.Invoke(_hits);
        }
    }
    public static Action<float> onHitsChanged;
}