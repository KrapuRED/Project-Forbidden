using System.Collections;
using UnityEngine;
using DG.Tweening;


public class FailedPanel : Panel
{
    [SerializeField] private float speedReveal;
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private CanvasGroup contentCanvasGroup;

    public override IEnumerator AnimationOpenPanel()
    {
        // Start hidden
        panelCanvasGroup.alpha = 0f;
        contentCanvasGroup.alpha = 0f;

        // Fade in the panel background/frame first
        yield return panelCanvasGroup.DOFade(1f, speedReveal).WaitForCompletion();

        panelCanvasGroup.interactable = true;
        panelCanvasGroup.blocksRaycasts = true;
        contentCanvasGroup.interactable = true;
        contentCanvasGroup.blocksRaycasts = true;

        // Then fade in the content
        yield return contentCanvasGroup.DOFade(1f, 1).WaitForCompletion();
    }

    public override IEnumerator AnimationClosePanel()
    {
        // Fade out content first
        yield return contentCanvasGroup.DOFade(0f, 1).WaitForCompletion();

        panelCanvasGroup.interactable = false;
        panelCanvasGroup.blocksRaycasts = false;
        contentCanvasGroup.interactable = false;
        contentCanvasGroup.blocksRaycasts = false;

        // Then fade out the panel itself
        yield return panelCanvasGroup.DOFade(0f, 1).WaitForCompletion();
    }
}
