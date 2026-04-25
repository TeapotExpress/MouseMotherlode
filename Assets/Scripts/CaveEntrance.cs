using UnityEngine;
public class CaveEntrance : MonoBehaviour
{
    public GameObject UITxt;

    private bool isPlayerTouching = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            isPlayerTouching = true;
        }
       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isPlayerTouching = false;
        }
    }

    public void ChangeScene(string target)
    {
        SceneChanger.instance.SceneChange(target);
    }
    // Update is called once per frame
    void Update()
    {
        if(UITxt!=null)
            UITxt.SetActive(isPlayerTouching);

        if(isPlayerTouching && Input.GetKeyDown(KeyCode.E))
        {
            ChangeScene("Laura testy");
        }
    }
}
