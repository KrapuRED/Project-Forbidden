using UnityEngine;

[CreateAssetMenu(fileName = "CharacterDataSO", menuName = "Character/CharacterDataSO")]
public class CharacterDataSO : ScriptableObject
{
    public string characterName;
    public float characterHealthAmount;
}
