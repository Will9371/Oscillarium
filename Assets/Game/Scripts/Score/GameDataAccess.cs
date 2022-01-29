using UnityEngine;

public class GameDataAccess : MonoBehaviour
{
    public void AddToScore(int value) { GameData.score += value; }//Debug.Log("point");
    public void AddToMistakes(int value) { GameData.hits += value; }//Debug.Log("hit");
}
