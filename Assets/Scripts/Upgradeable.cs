
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Upgradeable : MonoBehaviour
{
    public int level=0;
    [SerializeField] private GameObject MatsNeededSpace;
    private GameObject CanvasObj;

    private bool isActive=false;

    public List<LevelReq> levels;


    [Serializable]
    public class MatNeeded
    {
        public string name = "COPPER";
        public int amount = 99;
    }

    [Serializable]
    public class LevelReq
    {
        public List<MatNeeded> requiredOres;
    }

    void Awake()
    {
        CanvasObj = MatsNeededSpace.transform.root.Find("Canvas").gameObject;
    }
    void Start()
    {
        level = PlayerPrefs.GetInt(transform.name);
    }

    void ShowRequiredMaterials()
    {
        foreach(Transform b in MatsNeededSpace.transform)
        {
            Destroy(b.gameObject);
        }
        LevelReq currentLevel = levels[level];

        foreach (MatNeeded ore in currentLevel.requiredOres)
        {
            GameObject mat = Instantiate(Resources.Load("ResourceShow") as GameObject);
            mat.transform.parent = MatsNeededSpace.transform;
            mat.transform.localScale = Vector3.one;
            Debug.Log((ore.name + "icon").ToLower());
            mat.GetComponent<Image>().sprite = Resources.Load<Sprite>((ore.name + "icon").ToLower());

            Text amountTxt = mat.transform.GetChild(0).GetComponent<Text>();
            amountTxt.text = "x"+ore.amount.ToString();
            if(ore.amount > PlayerPrefs.GetInt(ore.name))
                amountTxt.color = Color.red;
        }
    }

    void Upgrade()
    {
        LevelReq currentLevel = levels[level];
        int canUpgrade = 0;

        foreach (MatNeeded ore in currentLevel.requiredOres)
        {
            if (ore.amount <= PlayerPrefs.GetInt(ore.name))
            {
                canUpgrade++;
            }
        }
        if (canUpgrade == currentLevel.requiredOres.Count)
        {
            foreach (MatNeeded ore in currentLevel.requiredOres)
            {
                PlayerPrefs.SetInt(ore.name, PlayerPrefs.GetInt(ore.name) - ore.amount);
            }
            level++;
            PlayerPrefs.SetInt(transform.name, level);
            ShowRequiredMaterials();
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.tag=="Player")
        {
            isActive=true;
            ShowRequiredMaterials();
        }
    }
    void OnTriggerExit2D(Collider2D collider2D)
    {
        if(collider2D.tag=="Player")
        {
            isActive=false;
        }
    }
    
    void Update()
    {
        CanvasObj.SetActive(isActive);

        if(isActive && Input.GetKeyDown(KeyCode.E))
        {
            Upgrade();
        }
    }
}
