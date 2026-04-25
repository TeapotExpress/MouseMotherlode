using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class CaveVolumeAdjust : MonoBehaviour
{
    public GameObject player;

    public Volume volume; // Przypisz w Inspectorze
    private Vignette vignette;

    void Start()
    {
        // Pobierz komponent Vignette z profilu
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            Debug.Log("Vignette found!");
        }
        else
        {
            Debug.LogWarning("Vignette not found in the Volume profile!");
        }
    }


    private void Update()
    {
        SetVignetteIntensity((transform.position.y/player.transform.position.y)/5);
    }
    public void SetVignetteIntensity(float intensity)
    {
        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Clamp01(intensity); // 0–1
        }
    }

    public void EnableVignette(bool enabled)
    {
        if (vignette != null)
        {
            vignette.active = enabled;
        }
    }
}
