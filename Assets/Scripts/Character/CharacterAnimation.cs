using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Character ownerCharacter;

    private Animator _animator;

    private void Start()
    {
        _animator = ownerCharacter.GetComponent<Animator>();
    }

    public void PlayWalkingAnimtion(float movement)
    {
        _animator.SetFloat("movement", movement);
    }
}
