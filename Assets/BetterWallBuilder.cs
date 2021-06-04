using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterWallBuilder : MonoBehaviour
{
    public GameObject wallBlock;
    public GameObject background;
    public GameObject backGroundWall;
    private Vector2 neededSize = new Vector2(.5f, .5f);
    private int wallNumber;
    void Start()
    {
        var mapType = Random.Range(0, 4);
        //Debug.Log(mapType);

        switch (mapType)
        {
            case 1:
                //Debug.Log("Calling Normal");
                NormalMap();
                break;
            case 2:
                SparseMap();
                break;
            case 3:
                DenseMap();
                break;
            case 4:
                DenserMap();
                break;
            default:
                OpenMap();
                break;


        }
        backGroundWall.SetActive(true);

    }

    //Determines which grid point to try to build from. 'Range' values will need to be adjusted if the map size changes. 
    float GridValue()
    {
        //var value = (float)(Mathf.Ceil(Random.Range(-19, 19)) * .5);
        var value = Mathf.Ceil(Random.Range(-9.5f, 8.5f));
        return value;
    }

    void NormalMap()
    {
        wallNumber = 50;
        while (wallNumber > 0)
        {
            WallSelector();

        }
        Debug.Log("Normal Map");
    }

    void SparseMap()
    {
        wallNumber = 25;
        while (wallNumber > 0)
        {
            WallSelector();
        }
        Debug.Log("Sparse Map");
    }
    void DenseMap()
    {
        wallNumber = 80;
        while (wallNumber > 0)
        {
            WallSelector();
        }
        Debug.Log("Dense Map");
    }
    void DenserMap()
    {
        wallNumber = 110;
        while (wallNumber > 0)
        {
            WallSelector();
        }
        Debug.Log("Denser Map");
    }

    void OpenMap()
    {
        //wallNumber = 0;
        //while (wallNumber > 0)
        //{
        //    var x = GridValue();
        //    var y = GridValue();
        //    var z = 0;
        //var potentialPosition = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        //if (!Physics2D.OverlapBox(potentialPosition, neededSize, 0f))
        //{
        //    Instantiate(wall, potentialPosition, Quaternion.identity);
        //    wallNumber--;

        //}
        //}
        Debug.Log("Open Map");
    }

    //Determines which style of wall will be attempted. 'Prefabs/Walls' has the models for the walls (though the prefabs themselves are not used).
    void WallSelector()
    {
        var wallType = Mathf.Ceil(Random.Range(0, 11));
        switch (wallType)
        {
            case 1:
                Build2H();
                break;
            case 2:
                Build2V();
                break;
            case 3:
                Build3H();
                break;
            case 4:
                Build3V();
                break;
            case 5:
                Build4H();
                break;
            case 6:
                Build4V();
                break;
            case 7:
                BuildCross();
                break;
            case 8:
                BuildLyingLeftL();
                break;
            case 9:
                BuildLyingRightL();
                break;
            case 10:
                BuildStandingLeftL();
                break;
            case 11:
                BuildStandingRightL();
                break;
            default:
                BuildBlock();
                break;

        }
    }

    private void BuildBlock()
    {
        var x = GridValue();
        var y = GridValue();
        var z = 0;
        var potentialPosition = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        if (!Physics2D.OverlapBox(potentialPosition, neededSize, 0f))
        {
            Instantiate(wallBlock, potentialPosition, Quaternion.identity);
            wallNumber--;

        }
    }

    //The methods below build the walls. Each individual potential block position is checked if it's clear before building the whole wall. After a wall is successfuly built, the number of blocks remaining (wallNumber) is decremented accordingly. 
    private void BuildStandingRightL()
    {
        var x = GridValue();
        var y = GridValue();
        var z = 0;
        var potentialPosition1 = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition2 = new Vector3(x + background.transform.position.x + 1, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition3 = new Vector3(x + background.transform.position.x, y + background.transform.position.y + 1, z + background.transform.position.y);
        var potentialPosition4 = new Vector3(x + background.transform.position.x, y + background.transform.position.y + 2, z + background.transform.position.y);
        if ((!Physics2D.OverlapBox(potentialPosition1, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition2, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition3, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition4, neededSize, 0f)))
        {
            Instantiate(wallBlock, potentialPosition1, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition2, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition3, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition4, Quaternion.identity);
            wallNumber -= 4;

        }
    }

    private void BuildStandingLeftL()
    {
        var x = GridValue();
        var y = GridValue();
        var z = 0;
        var potentialPosition1 = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition2 = new Vector3(x + background.transform.position.x - 1, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition3 = new Vector3(x + background.transform.position.x, y + background.transform.position.y + 1, z + background.transform.position.y);
        var potentialPosition4 = new Vector3(x + background.transform.position.x, y + background.transform.position.y + 1, z + background.transform.position.y);
        if ((!Physics2D.OverlapBox(potentialPosition1, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition2, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition3, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition4, neededSize, 0f)))
        {
            Instantiate(wallBlock, potentialPosition1, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition2, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition3, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition4, Quaternion.identity);
            wallNumber -= 4;

        }
    }

    private void BuildLyingRightL()
    {
        var x = GridValue();
        var y = GridValue();
        var z = 0;
        var potentialPosition1 = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition2 = new Vector3(x + background.transform.position.x, y + background.transform.position.y + 1, z + background.transform.position.y);
        var potentialPosition3 = new Vector3(x + background.transform.position.x - 1, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition4 = new Vector3(x + background.transform.position.x - 2, y + background.transform.position.y, z + background.transform.position.y);
        if ((!Physics2D.OverlapBox(potentialPosition1, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition2, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition3, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition4, neededSize, 0f)))
        {
            Instantiate(wallBlock, potentialPosition1, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition2, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition3, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition4, Quaternion.identity);
            wallNumber -= 4;

        }
    }

    private void BuildLyingLeftL()
    {
        var x = GridValue();
        var y = GridValue();
        var z = 0;
        var potentialPosition1 = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition2 = new Vector3(x + background.transform.position.x, y + background.transform.position.y + 1, z + background.transform.position.y);
        var potentialPosition3 = new Vector3(x + background.transform.position.x + 1, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition4 = new Vector3(x + background.transform.position.x + 2, y + background.transform.position.y, z + background.transform.position.y);
        if ((!Physics2D.OverlapBox(potentialPosition1, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition2, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition3, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition4, neededSize, 0f)))
        {
            Instantiate(wallBlock, potentialPosition1, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition2, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition3, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition4, Quaternion.identity);
            wallNumber -= 4;

        }
    }

    private void BuildCross()
    {
        var x = GridValue();
        var y = GridValue();
        var z = 0;
        var potentialPosition1 = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition2 = new Vector3(x + background.transform.position.x + 1, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition3 = new Vector3(x + background.transform.position.x - 1, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition4 = new Vector3(x + background.transform.position.x, y + background.transform.position.y + 1, z + background.transform.position.y);
        var potentialPosition5 = new Vector3(x + background.transform.position.x, y + background.transform.position.y - 1, z + background.transform.position.y);
        if ((!Physics2D.OverlapBox(potentialPosition1, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition2, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition3, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition4, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition5, neededSize, 0f)))
        {
            Instantiate(wallBlock, potentialPosition1, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition2, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition3, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition4, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition5, Quaternion.identity);
            wallNumber -= 5;

        }
    }

    private void Build4V()
    {
        var x = GridValue();
        var y = GridValue();
        var z = 0;
        var potentialPosition1 = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition2 = new Vector3(x + background.transform.position.x, y + background.transform.position.y + 1, z + background.transform.position.y);
        var potentialPosition3 = new Vector3(x + background.transform.position.x, y + background.transform.position.y - 1, z + background.transform.position.y);
        var potentialPosition4 = new Vector3(x + background.transform.position.x, y + background.transform.position.y - 2, z + background.transform.position.y);
        if ((!Physics2D.OverlapBox(potentialPosition1, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition2, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition3, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition4, neededSize, 0f)))
        {
            Instantiate(wallBlock, potentialPosition1, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition2, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition3, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition4, Quaternion.identity);
            wallNumber -= 4;

        }
    }

    private void Build4H()
    {
        var x = GridValue();
        var y = GridValue();
        var z = 0;
        var potentialPosition1 = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition2 = new Vector3(x + background.transform.position.x + 1, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition3 = new Vector3(x + background.transform.position.x - 1, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition4 = new Vector3(x + background.transform.position.x + 2, y + background.transform.position.y, z + background.transform.position.y);
        if ((!Physics2D.OverlapBox(potentialPosition1, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition2, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition3, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition4, neededSize, 0f)))
        {
            Instantiate(wallBlock, potentialPosition1, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition2, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition3, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition4, Quaternion.identity);
            wallNumber -= 4;

        }
    }

    private void Build3V()
    {
        var x = GridValue();
        var y = GridValue();
        var z = 0;
        var potentialPosition1 = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition2 = new Vector3(x + background.transform.position.x, y + background.transform.position.y + 1, z + background.transform.position.y);
        var potentialPosition3 = new Vector3(x + background.transform.position.x, y + background.transform.position.y - 1, z + background.transform.position.y);
        if ((!Physics2D.OverlapBox(potentialPosition1, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition2, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition3, neededSize, 0f)))
        {
            Instantiate(wallBlock, potentialPosition1, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition2, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition3, Quaternion.identity);
            wallNumber -= 3;

        }
    }

    private void Build3H()
    {
        var x = GridValue();
        var y = GridValue();
        var z = 0;
        var potentialPosition1 = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition2 = new Vector3(x + background.transform.position.x + 1, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition3 = new Vector3(x + background.transform.position.x -1, y + background.transform.position.y, z + background.transform.position.y);
        if ((!Physics2D.OverlapBox(potentialPosition1, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition2, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition3, neededSize, 0f)))
        {
            Instantiate(wallBlock, potentialPosition1, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition2, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition3, Quaternion.identity);
            wallNumber -= 3;

        }
    }

    private void Build2V()
    {
        var x = GridValue();
        var y = GridValue();
        var z = 0;
        var potentialPosition1 = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition2 = new Vector3(x + background.transform.position.x, y + background.transform.position.y + 1, z + background.transform.position.y);
        if ((!Physics2D.OverlapBox(potentialPosition1, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition2, neededSize, 0f)))
        {
            Instantiate(wallBlock, potentialPosition1, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition2, Quaternion.identity);
            wallNumber -= 2;

        }
    }

    private void Build2H()
    {
        var x = GridValue();
        var y = GridValue();
        var z = 0;
        var potentialPosition1 = new Vector3(x + background.transform.position.x, y + background.transform.position.y, z + background.transform.position.y);
        var potentialPosition2 = new Vector3(x + background.transform.position.x + 1, y + background.transform.position.y, z + background.transform.position.y);
        if ((!Physics2D.OverlapBox(potentialPosition1, neededSize, 0f)) && (!Physics2D.OverlapBox(potentialPosition2, neededSize, 0f)))
        {
            Instantiate(wallBlock, potentialPosition1, Quaternion.identity);
            Instantiate(wallBlock, potentialPosition2, Quaternion.identity);
            wallNumber -= 2;

        }
    }

}
