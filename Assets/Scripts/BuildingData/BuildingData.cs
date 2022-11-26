using UnityEngine;

[CreateAssetMenu(fileName = "newBuilding", menuName = "Building")]
public class BuildingData : ScriptableObject
{
    [SerializeField] private string nameBuilding;
    [SerializeField] private Vector2 sizeBuilding;
    [SerializeField] private float timeConstruction;
    [SerializeField] private Sprite spriteConstruction;
    [SerializeField] private TypeConstruction typeConstruction;
    public string NameBuilding => nameBuilding;
    public Sprite SpriteConstruction => spriteConstruction;
    public Vector2 SizeBuilding => sizeBuilding;
    public float TimeConstruction => timeConstruction;

    public TypeConstruction TypeConstruction => typeConstruction;
}
