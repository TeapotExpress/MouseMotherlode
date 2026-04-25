using UnityEngine;
using UnityEngine.UI;

public class TextDisplayMaterials : MonoBehaviour
{
    [Header("Nazwa surowca (np. GOLD, IRON itp.)")]
    public string resourceKey = "GOLD";

    void Update()
    {
        int value = PlayerPrefs.GetInt(resourceKey, 0);
        GetComponent<Text>().text = $"{resourceKey}: {value}";
    }
}
