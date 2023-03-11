using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    //one line clear = 100
    //tetris = 800
    public static int width = 10;
    public static int height = 17;
    public static Transform[,] grid = new Transform[width, height];
    
    [SerializeField]
    private static int _rowCounter = 0;

    [SerializeField]
    private static GameManager _gm;

    private void Awake()
    {
        _gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gm == null)
        {
            Debug.LogError("The GameManager is NULL");
        }
    }

    //private void Update()
    //{
    //    for (int i = 0; i < width; i++)
    //    {
    //        for (int j = 0; j < height; j++)
    //        {
    //            if (grid[i, j] != null)
    //            {
    //                arrayLayout.rows[i].row[j] = true;
    //            }
    //            else
    //            {
    //                arrayLayout.rows[i].row[j] = false;
    //            }
                
    //        }
    //    }
    //}

    public static Vector2 roundVector2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static bool insideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    public static void deleteRow(int j)
    {
        for (int i = 0; i < width; i++)
        {
            Destroy(grid[i, j].gameObject);
            grid[i, j] = null;
        }
        _rowCounter++;
        Debug.Log("Row Counter: " + _rowCounter);
        Debug.Log("Level Requirement: " + _gm._levelReq[_gm._level]);

        if (_rowCounter >= _gm._levelReq[_gm._level])
        {
            _rowCounter = 0;
            _gm._level++;
            FindObjectOfType<UIManager>().UpdateLevel(_gm._level);
        }
    }

    public static void decreaseRow(int j)
    {
        for (int i = 0; i < width; i++)
            if (grid[i, j] != null)
            {
                grid[i, j - 1] = grid[i, j];
                grid[i, j] = null;

                grid[i, j - 1].position += new Vector3(0, -1, 0);
            }
    }

    public static void decreaseRowAbove(int j)
    {
        for (int i = j; i < height; i++)
        {
            decreaseRow(i);
        }
    }


    public static bool isRowFull(int j)
    {
        for (int i = 0; i < width; i++)
        {
            if (grid[i, j] == null)
            {
                return false;
            }
        }
        return true;
    }

    public static int deleteFullRows()
    {
        int deletedRows = 0;
        for (int j = 0; j < height; j++)
        {
            if (isRowFull(j))
            {
                deletedRows++;
                deleteRow(j);
                decreaseRowAbove(j + 1);
                j--;

            }
        }
        return deletedRows;
    }

    public void showArray()
    {
        for (int i = 0; i < width; i++) 
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < height; j++)
            {
                sb.Append(grid[i, j]);
            }
            Debug.Log(sb);
            sb.Clear();
        }
    }
}
