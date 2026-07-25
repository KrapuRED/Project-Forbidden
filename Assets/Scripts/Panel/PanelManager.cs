using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance { get; private set; }

    [SerializeField] private Transform panelContainer;
    [SerializeField] private List<Panel> panelDatas = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        panelDatas = panelContainer.GetComponentsInChildren<Panel>().ToList();

    }

    public void OpenPanel(string panelName)
    {
        StartCoroutine(OpeningPanel(panelName));
    }

    private IEnumerator OpeningPanel(string panelName)
    {
        Panel panel = panelDatas.First(t => t.name == panelName);

        yield return panel.AnimationOpenPanel();
    }

    public void ClosePanel(string panelName)
    {
        StartCoroutine(ClosingPanel(panelName));
    }

    private IEnumerator ClosingPanel(string panelName)
    {
        Panel panel = panelDatas.First(t => t.name == panelName);

        yield return panel.AnimationClosePanel();
    }
}
