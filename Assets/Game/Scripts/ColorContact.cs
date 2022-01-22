using System;
using UnityEngine;
using UnityEngine.Events;
using Playcraft;

public class ColorContact : MonoBehaviour
{
    [SerializeField] ColorChange color;
    [SerializeField] ContactData[] values;

    void OnTriggerEnter2D(Collider2D other)
    {
        var otherTags = other.GetComponent<CustomTags>();
        if (!otherTags) return;
    
        var otherColor = other.GetComponent<ColorChange>();
        if (!otherColor) return;

        foreach (var value in values)
        {
            var factionMatch = otherTags.HasTag(value.factionId);
            if (!factionMatch) continue;
            
            var colorMatch = otherColor.colorId == color.colorId;
            var colorCondition = RespondToMatch(colorMatch, value.requireColorMatch);
            if (colorCondition) value.response.Invoke();
            
            /*Debug.Log($"{other.name}...checking faction: {value.factionId}" +
                      $"own color: {color.colorId}, other color: {otherColor.colorId}, " +
                      $"color match: {colorMatch}, color condition: {colorCondition}, " +
                      $"faction match: {factionMatch}");*/
        }
    }
    
    bool RespondToMatch(bool match, Trinary response)
    {
        switch (response)
        {
            case Trinary.True: return match;
            case Trinary.False: return !match;
            default: return true;
        }
    }

    [Serializable]
    public struct ContactData
    {
        public SO factionId;
        public Trinary requireColorMatch;
        public UnityEvent response;
    }
}
