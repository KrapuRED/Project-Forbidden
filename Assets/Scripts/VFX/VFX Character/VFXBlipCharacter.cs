using DG.Tweening;
using System.Collections;
using UnityEngine;

public class VFXBlipCharacter : VfxCharacter
{
    [SerializeField] private Color flashColor = Color.white; 
    [SerializeField] private float flashDuration = 0.1f;

    private Color _originalColor;
    private Tween _flashTween;

    public override void AnimationIn(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null) return;

        if (_flashTween != null && _flashTween.IsActive())
        {
            _flashTween.Complete();
        }

        _originalColor = spriteRenderer.color;

        // Set warna langsung ke warna flash
        spriteRenderer.color = flashColor;

        // Animasikan warna kembali ke warna asli
        _flashTween = spriteRenderer.DOColor(_originalColor, flashDuration)
            .SetEase(Ease.OutQuad);
    }
}
