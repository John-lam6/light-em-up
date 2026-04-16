using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private int[] levelVariants = {0, 2, 1};
    public void LoadLevel(int level)
    {
        int variant = Random.Range(1, levelVariants[level] + 1);
        SceneManager.LoadScene("Level " + level + "." + variant);
    }
}
