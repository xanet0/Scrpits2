using UnityEngine;
[CreateAssetMenu(fileName = "NewUnit", menuName = "Item")]
public class ItemData : ScriptableObject
{
    public Sprite icon;
    public GameObject prefab;
    public GameObject showInHandPrefab;
    public int id;
    public bool canBuild;
    public bool eateble;
    public int healHealth;
    
}
