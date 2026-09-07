using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelCreator levelCreator;
    [SerializeField] private LevelData currentLevel;
   private void Start()
    {
        levelCreator.Build(currentLevel);
    }
}
