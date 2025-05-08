using System.Linq;
using _Game.Scripts.Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Legacy.CoffeeMaker2
{
    public class SnapPoint : MonoBehaviour
    {
        public Rigidbody rigidbody;
        [FormerlySerializedAs("grabController")] public DragAndDropController dragAndDropController;
        public string[] filterTags;

        private bool _isSnapped;

        void Start()
        {

        }

        void OnTriggerStay(Collider other)
        {
            if (filterTags.Contains(other.tag) && dragAndDropController.grabbedObject != null &&
                other.attachedRigidbody == dragAndDropController.grabbedObject)
            {
                dragAndDropController.SetActiveSnapPoint(this);
                _isSnapped = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.attachedRigidbody == dragAndDropController.grabbedObject)
            {
                dragAndDropController.ClearActiveSnapPoint(this);
                _isSnapped = false;
            }
        }

        public bool IsSnapped()
        {
            return _isSnapped;
        }
    }
}