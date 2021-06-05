using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is an old version of the wall builder. Not used in formal training or testing.
public class WallBuilder : MonoBehaviour
{
    public GameObject wallBlock;
    public GameObject background;
    private Vector2 neededSize = new Vector2(.5f, .5f);
    void Start()
    {
        var mapType = Mathf.Ceil(Random.Range(0, 4));

        switch (mapType)
        {
            case 1:
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

    }

    float GridValue()
    {
        var value = (float)(Mathf.Ceil(Random.Range(-19, 19)) * .5);
        return value;
    }

    void NormalMap()
    {
        int wallNumber = 50;
        while (wallNumber > 0)
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
        Debug.Log("Normal Map");
    }

    void SparseMap()
    {
        int wallNumber = 20;
        while (wallNumber > 0)
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
        Debug.Log("Sparse Map");
    }
    void DenseMap()
    {
        int wallNumber = 70;
        while (wallNumber > 0)
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
        Debug.Log("Dense Map");
    }
    void DenserMap()
    {
        int wallNumber = 90;
        while (wallNumber > 0)
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
        Debug.Log("Denser Map");
    }

    void OpenMap()
    {

        Debug.Log("Open Map");
    }

}
