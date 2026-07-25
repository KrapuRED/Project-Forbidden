using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class EntityCounter
{
    public string entityName;
    public string entityID;
    public CharacterType entityType;
    public Transform entityPosition;
}

public class EntityCounterManager : MonoBehaviour
{
    public static EntityCounterManager Instance { get; private set; }

    [Header("Counter Entity Manager Config")]
    [SerializeField] private List<EntityCounter> entityCounterList = new List<EntityCounter>();

    private Dictionary<CharacterType, int> _entityCounters = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void AssignToCounter(Character character, string newEntityID)
    {
        EntityCounter newEntity = new EntityCounter
        {
            entityName = character.name,
            entityID = newEntityID,
            entityType = character.CharacterType,
            entityPosition = character.transform
        };

        entityCounterList.Add(newEntity);
        Debug.Log($"Succes add {newEntityID} to entityCounterList");
    }

    public string GetEntityID(Character character)
    {
        string entityID = string.Empty;

        if (!_entityCounters.ContainsKey(character.CharacterType))
            _entityCounters[character.CharacterType] = 0;

        int entityCounter = _entityCounters[character.CharacterType];
        _entityCounters[character.CharacterType]++;

        switch (character.CharacterType)
        {
            case CharacterType.Player:
                entityID = $"PL";
                break;

            case CharacterType.EnemyMinion:
                entityID = $"EM_{entityCounter}";
                break;
        }

        AssignToCounter(character, entityID);

        return entityID;
    }

    public void RemoveEntityFormCounterByID(string entityID)
    {
        int removedCount = entityCounterList.RemoveAll(entity => entity.entityID == entityID);

        if (removedCount > 0)
        {
            Debug.Log($"[EntityCounterManager] Berhasil menghapus entity dengan ID: {entityID}");
        }
        else
        {
            Debug.LogWarning($"[EntityCounterManager] Entity dengan ID: {entityID} tidak ditemukan!");
        }
    }

    public EntityCounter GetEntityByID(string entityID)
    {
        foreach (var entity in entityCounterList)
        {
            if (entity.entityID == entityID)
                return entity;
        }

        return null;
    }
}
