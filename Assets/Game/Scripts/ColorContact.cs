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
        [Tooltip("Only respond if contacted object has this tag")]
        public SO factionId;
        [Tooltip("True: only respond if colors match, " +
                 "False: only respond if colors don't match, " +
                 "Unknown: always respond.")]
        public Trinary requireColorMatch;
        [Tooltip("Invoked if above conditions are met")]
        public UnityEvent response;
    }
}
