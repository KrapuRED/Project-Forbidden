using UnityEngine;
using System.Collections.Generic;

public class HealthData
{
    public string ownerHealthData;
    public float currentHealthData;
}

public class HealthManager : MonoBehaviour
{
    public static HealthManager Instance { get; private set; }

    [SerializeField] private List<HealthData> healthDatas = new (); // able see in Inspector
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

    public void AssignHealth()
    {
        //Assign Character / CharacterHealth to Manager
    }

    public void OnTakeDamage()
    {
        //Calaculate  Character / CharacterHealth Health
    }

    public void OnDeath()
    {
        //Remove Character / CharacterHealth from Manager

    }
}
