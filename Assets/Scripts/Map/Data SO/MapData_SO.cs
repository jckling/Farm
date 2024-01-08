using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData_SO", menuName = "Farm / Create MapData_SO")]
public class MapData_SO : ScriptableObject
{
    [SceneName] public string sceneName;

    public List<TileProperty> tileProperties;
}