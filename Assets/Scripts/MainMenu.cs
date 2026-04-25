using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject creditsSplash;
    private bool creditsScreenOn = false;
    private string EventPath = string.Empty;
    public void buttonHoverSFX()
    {
        EventInstance Hover = RuntimeManager.CreateInstance(EventPath);
        RuntimeManager.AttachInstanceToGameObject(Hover, gameObject, GetComponent<Rigidbody2D>());
        Hover.start();
        Hover.release();
    }



    //Buttons Pressed
    public void StartButtonPressed()
    {
        SceneManager.LoadScene(1);
    }
    public void CredditsButtonPressed()
    {
        if (!creditsScreenOn)
        {
            creditsScreenOn = true;
            creditsSplash.GetComponent<Animator>().SetTrigger("Open");
        }
    }

    public void CloseCredits()
    {
        creditsScreenOn = false;
        creditsSplash.GetComponent<Animator>().SetTrigger("Close");
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
        if (Application.isEditor && Application.isPlaying)
        {
            Debug.Log("Exit function called in editor - I would exit on application");
        }
    }
}
