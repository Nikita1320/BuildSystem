using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerBuildingPanels : MonoBehaviour
{
    public delegate void ChangedState();
    public ChangedState changedStateEvent;

    [SerializeField] GameObject canvasBuilding;

    [SerializeField] private StateBuilding[] statesBuilding;
    [SerializeField] private StateBuilding currentStateBuilding;
    
    [SerializeField] private ActionPanel[] gamePanelBuilding;
    [SerializeField] private ActionPanel currentActivePanel;

    private Dictionary<StateBuilding, ActionPanel> gamePanels = new Dictionary<StateBuilding, ActionPanel>();

    public StateBuilding CurrentState => currentStateBuilding;

    private void Start()
    {
        for (int i = 0; i < statesBuilding.Length; i++)
        {
            gamePanels.Add(statesBuilding[i], gamePanelBuilding[i]);
            Debug.Log(gamePanels[statesBuilding[i]]);
        }
        ChangeState(StateBuilding.PlacingForBuild);
    }
    public void ChangeState(StateBuilding _stateBuilding)
    {
        DeActivatePanel();
        currentStateBuilding = _stateBuilding;
    }

    public void ActivatePanel(bool value = true)
    {
        canvasBuilding.SetActive(true);
        if (currentActivePanel != null)
        {
            currentActivePanel.gameObject.SetActive(false);
        }
        Debug.Log(currentStateBuilding.ToString());
        Debug.Log(gamePanels[currentStateBuilding]);
        currentActivePanel = gamePanels[currentStateBuilding];
        currentActivePanel.Activate(value);
    }
    public void DeActivatePanel()
    {
        Debug.Log("DeActivatePanel");
        if (currentActivePanel)
        {
            currentActivePanel.gameObject.SetActive(false);
        }
        canvasBuilding.SetActive(false);
        currentActivePanel = null;
    }
}
