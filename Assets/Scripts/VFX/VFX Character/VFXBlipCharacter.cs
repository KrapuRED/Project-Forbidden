using DG.Tweening;
using System.Collections;
using UnityEngine;

public class VFXBlipCharacter : VfxCharacter
{
    [SerializeField] private Material flashHitMaterial;
    [SerializeField] private float duration;

    private Material _defaultMaterial;
    private bool _hasCapturedDefault;
    private Coroutine _coroutine;

    public override void AnimationIn(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null) return;

        if (!_hasCapturedDefault) // NEW — only grab it once, before any flash overwrites it
        {
            _defaultMaterial = spriteRenderer.material;
            _hasCapturedDefault = true;
        }

        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(FlashRoutine(spriteRenderer));
    }

    private IEnumerator FlashRoutine(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.material = flashHitMaterial;

        yield return new WaitForSeconds(duration);

        spriteRenderer.material = _defaultMaterial;

        _coroutine = null;
    }
}
