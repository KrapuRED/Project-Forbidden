using UnityEngine;

public class PlayerCharacter : Character
{
    public override void OnDeathCharacter()
    {
        Debug.Log($"{gameObject.name} is Dead");

    }
}
