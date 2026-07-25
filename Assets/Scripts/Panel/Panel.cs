using UnityEngine;
using System.Collections;

public abstract class Panel : MonoBehaviour
{
    public abstract IEnumerator AnimationOpenPanel();
    public abstract IEnumerator AnimationClosePanel();
}
