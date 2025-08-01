using UnityEngine;

public class Utility : MonoBehaviour
{
    public static float Map(float value, float originalMin, float originalMax, float newMin, float newMax, bool clamp)
    {
        float val = newMin + (newMax - newMin) * ((value - originalMin) / (originalMax - originalMin));
        return clamp ? Mathf.Clamp(val, Mathf.Min(newMin, newMax), Mathf.Max(newMin, newMax)) : val;
    }
}
