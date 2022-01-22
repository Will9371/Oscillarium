using UnityEngine;

public class GameDataAccess : MonoBehaviour
{
    public void AddToScore(int value) { GameData.score += value; }
}
