using UnityEngine;
using System.Linq;

public class CharacterVisualizer : MonoBehaviour
{
    [SerializeField] private Transform vfxContainer;
    [SerializeField] private VfxCharacter[] VFXCharacters;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Start()
    {
        VFXCharacters = vfxContainer.GetComponentsInChildren<VfxCharacter>();
    }
    public void PlayVfx(string nameVfx)
    {
        if (spriteRenderer == null)
            return;

        VfxCharacter vfx = VFXCharacters.First(v => v.name == nameVfx);

        if (vfx == null)
        {
            return;
        }

        vfx.AnimationIn(spriteRenderer);
    }
}
