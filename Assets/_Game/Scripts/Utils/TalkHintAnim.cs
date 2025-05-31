using System;
using DG.Tweening;
using UnityEngine;

namespace GoT._Game.Scripts.Utils
{
    public class TalkHintAnim : MonoBehaviour
    {
        [SerializeField] private Ease ease;
        [SerializeField] private float duration;
        [SerializeField] private float range;
        
        private Tweener _tweener;
        
        private void Awake() {
            _tweener = transform.DOMoveY( range, duration )
                .SetEase( ease )
                .SetLoops( -1, LoopType.Yoyo );
        }
        private void OnDestroy() {
            _tweener.Kill();
        }
    }
}