using System.Collections;
using System.Linq;
using UnityEngine;

namespace _Game.Legacy.CoffeeMaker2
{
    public class RespawnObject : MonoBehaviour
    {
        private Collider _collider;
        public Transform _spawnPos;
        public string[] _tags;
        void Start()
        {
            _collider = GetComponent<Collider>();
        }
        private void OnTriggerExit(Collider other)
        {
        
            Debug.Log(other.gameObject.name);
            if (_tags.Contains(other.gameObject.tag))
                StartCoroutine(RespwnObject(other.gameObject));
        }

        private IEnumerator RespwnObject(GameObject _object)
        {
            yield return new WaitForSeconds(1f);
            _object.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _object.transform.position = _spawnPos.position;
            _object.transform.rotation = new Quaternion(0,0,0,0);
            yield break;
        }
    }
}
