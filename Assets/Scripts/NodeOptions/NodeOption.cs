using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DragAndDropDemo.NodeOptions
{
    public abstract class NodeOption<TValue>: MonoBehaviour
    {
        [SerializeField] private Text title;

        private CanvasGroup _canvasGroup;
        protected Action<TValue> Action;

        private void Start(){
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void SetTitle(string value){
            title.text = value;
        }

        public abstract void SetDefaultValue(TValue value);
        
        public void SetApplyChanges(Action<TValue> action){
            Action = action;
        }

        public IEnumerator FadeOut(){
            if (_canvasGroup == null) yield break;
            for (int i = 0; i < 100; i++){
                _canvasGroup.alpha = i / 100f;
                yield return new WaitForSeconds(0.04f);
            }
            _canvasGroup.alpha = 1;
        }
        public IEnumerator FadeIn(){
            if(_canvasGroup==null) yield break;
            for (int i = 0; i < 100; i++){
                _canvasGroup.alpha =1- i / 100f;
                yield return new WaitForSeconds(0.04f);
            }
            _canvasGroup.alpha = 0;
        }
    }
}