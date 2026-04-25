using UnityEngine;
using System.Collections.Generic;

public class CaveBreaking : MonoBehaviour
{
    public List<Sprite> sprList;

    int indx = 0;

    private Vector3 lastPos;

    public void ResetBreaking()
    {
        indx = -1;
        transform.localScale = Vector3.zero;
        //GetComponent<SpriteRenderer>().sprite = sprList[indx];
    }

    private void Start()
    {
        ResetBreaking();
    }
    public void ChangeSprite()
    {
        transform.localScale = Vector3.one*2;
        if (lastPos!=transform.position)
        {
            ResetBreaking();
        }
        lastPos = transform.position;
        indx++;
        if (indx > sprList.Count - 1)
        {
            indx = 0;
        }
        GetComponent<SpriteRenderer>().sprite = sprList[indx];
        
    }
}
