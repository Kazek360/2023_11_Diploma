using UnityEngine;


/**
 * A scriptable object containing all basic statistics of an player
 */
[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "ScriptableObjects/Enemies/PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private int maxPsychePoints;
    [SerializeField] private int agility;
    [SerializeField] private int strength;
    [SerializeField] private int luck;
    [SerializeField] private int imagination;
}
