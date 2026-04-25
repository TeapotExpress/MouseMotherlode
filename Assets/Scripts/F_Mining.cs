using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class F_Mining : MonoBehaviour
{
    [SerializeField] private string EventPath = "event:/Mining";
    void PlayMineEvent()
    {
        EventInstance Mine = RuntimeManager.CreateInstance(EventPath);
        RuntimeManager.AttachInstanceToGameObject(Mine, gameObject, GetComponent<Rigidbody2D>());
        Mine.start();
        Mine.release();

    }
}