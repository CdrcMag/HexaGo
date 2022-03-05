using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level01", menuName = "ScriptableObjects/Level", order = 1)]
public class Level01 : ScriptableObject
{
    public int id;
    public int killableEnemies;
    public GameObject[] enemies;
    public Vector2[] positions;
    public float[] multiplicatorScale;
    public Vector2 startPosPlayer = new Vector2(0f, 0f);
}
