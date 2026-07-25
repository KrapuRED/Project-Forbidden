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
        if (isDead) return;

        isDead = true;
        Debug.Log($"{gameObject.name} is Dead");
        GameManager.Instance.GameOver();
    }
}
