using System.Linq;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    public Rigidbody rigidbody;
    public GrabController grabController;
    public string[] filterTags;

    private bool _isSnapped;

    void Start()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (filterTags.Contains(other.tag) && grabController.grabbedObject != null &&
            other.attachedRigidbody == grabController.grabbedObject)
        {
            grabController.SetActiveSnapPoint(this);
            _isSnapped = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == grabController.grabbedObject)
        {
            grabController.ClearActiveSnapPoint(this);
            _isSnapped = false;
        }
    }

    public bool IsSnapped()
    {
        return _isSnapped;
    }
}