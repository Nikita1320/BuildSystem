using UnityEngine;

public enum StateBuilding
{
    Idle,
    PlacingForBuild,
    Build,
    MovingBuilding,
    UpdateBuilding,
}
public enum TypeConstruction
{
    Fortress,
    IndustrialStructure,
    Warehouse,
    DefensiveConstruction,
    Other
}
public class Building : MonoBehaviour
{
    public delegate void StartedConstruction(Building building);
    public StartedConstruction startedConstructionEvent;

    public delegate void CanceledConstruction(Building building);
    public CanceledConstruction canceledConstructionEvent;

    public delegate void EndedConstruction();
    public EndedConstruction endedConstructionEvent;

    public delegate void CanceledPlaising();
    public CanceledPlaising canceledPlaisingEvent;

    [SerializeField] private ManagerBuildingPanels managerBuildingPanels;
    [SerializeField] private BuildingData buildingData;
    [SerializeField] private Renderer meshRender;
    [SerializeField] private BuildSystem buildSystem;
    [SerializeField] private BuildTimer buildTimer;

    
    public Vector2 Size => buildingData.SizeBuilding;
    public float TimeConstruction => buildingData.TimeConstruction;
    public string NameBuilding => buildingData.NameBuilding;

    public ManagerBuildingPanels ManagerPanels => managerBuildingPanels;

    private void Start()
    {

    }
    public void Init(BuildSystem _buildSystem)
    {
        buildSystem = _buildSystem;
    }
    public void StartConstruction()
    {
        if (buildSystem.StartConstruction(this))
        {
            buildSystem.EndPlacing(this);
            buildTimer.StartTimer(buildingData.TimeConstruction);
            buildTimer.endedTimerEvent += EndConstruction;

            managerBuildingPanels.ChangeState(StateBuilding.Build);
        }
    }
    public void EndConstruction()
    {
        buildSystem.EndConstruction(this);
        buildTimer.endedTimerEvent -= EndConstruction;

        managerBuildingPanels.ChangeState(StateBuilding.Idle);
        ActivateBuilding();
    }
    public void CancelConstruction()
    {
        buildSystem.CancelConstruction(this);
        buildTimer.CancelTimer();
        buildTimer.endedTimerEvent -= EndConstruction;
    }
    public void CancelPlaicingForBuild()
    {
        buildSystem.CancelPlacingForBuild(this);
    }
    public virtual void DestroyBuilding()
    {
        buildSystem.DestroyBuilding(this);
        Destroy(gameObject);
    }
    public virtual void UpdateBuilding()// Вынести в интерфейс
    {
        if (buildSystem.StartConstruction(this))
        {

        }
    }
    public virtual void ActivateBuilding()
    {
        meshRender.material.color = Color.blue;
    }
    public virtual void DeActivateBuilding()
    {
        meshRender.material.color = Color.blue;
    }
    public void SetTransparent(bool available)
    {
        if (available)
        {
            meshRender.material.color = new Color(0, 1, 0, 0.8f);
        }
        else
        {
            meshRender.material.color = new Color(1, 0, 0, 0.8f);
        }
    }

    public void SetNormal()
    {
        meshRender.material.color = Color.white;
    }
}
