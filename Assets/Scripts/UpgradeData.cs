using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade")]
public class UpgradeData : ScriptableObject
{
    public int id;
    public string upgradeName;
    public string description;
    public Sprite icon;
    public float[] valueRange;
    public float value;
    public bool isPercentage;
    public bool isFloat;
}