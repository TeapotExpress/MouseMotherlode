using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class StepsEmmiter : MonoBehaviour
{
    [SerializeField] private string EventPath = "event:/Walk";
 
    void PlayWalkEvent()
    {
        EventInstance Walk = RuntimeManager.CreateInstance(EventPath);
        RuntimeManager.AttachInstanceToGameObject(Walk, gameObject, GetComponent<Rigidbody2D>());
        Walk.start();
        Walk.release();

    }
}
