using UnityEngine;

public class PlayerCharacter : Character
{
    private void Start()
    {
        string entityID = EntityCounterManager.Instance.GetEntityID(this);
        SetCharacterID(entityID);

        CharacterHealth.Init(CharacterData.characterHealthAmount);
    }

    public override void OnDeathCharacter()
    {
        Debug.Log($"{gameObject.name} is Dead");
        GamaManager.Instance.GameOver();
    }
}
