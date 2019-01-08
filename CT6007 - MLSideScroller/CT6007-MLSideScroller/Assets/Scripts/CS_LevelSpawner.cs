using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_LevelSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject m_goLevelBlock;

    [SerializeField]
    private int m_iLevelWidth = 10;

    [SerializeField]
    private int m_iLevelHeight = 10;

    private List<GameObject> m_lLevelBlockList = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        int test = LevelMaps.Level1.Length;
        DrawLevel();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void DrawLevel()
    {
        for (int y = 0; y < m_iLevelHeight; y++)
        {
            for (int x = 0; x < m_iLevelWidth; x++)
            {
                if (LevelMaps.Level1[y * m_iLevelWidth + x] == 1)
                {
                    GameObject goNewLevelBlock = Instantiate(m_goLevelBlock, transform);
                    goNewLevelBlock.transform.position = transform.position;
                    goNewLevelBlock.transform.Translate(x * goNewLevelBlock.transform.localScale.x, -(y * goNewLevelBlock.transform.localScale.y), 0);
                }
            }
        }
    }
}