using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class describes one hero-spawn-location.
/// One hero-spawn-location must always consist of three positions, one for each hero.
/// </summary>
public class HeroSpawnOpportunity : MonoBehaviour
{
    [SerializeField]
    private Vector2Int[] heroSpawnPositions = new Vector2Int[3];

    public Vector2Int[] HeroSpawnPositions { get => heroSpawnPositions; }
}
