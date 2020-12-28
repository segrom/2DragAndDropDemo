using System;
using UnityEngine;

namespace DragAndDropDemo.Utils
{
    public class LoadingPanel : MonoBehaviour
    {
        private static LoadingPanel _instance;

        public static void PushTask(){
            _instance.tasks++;
            _instance.Check();
        }

        public static void PopTask(){
            _instance.tasks--;
            _instance.Check();
        }

        [SerializeField] private int tasks = 0;

        private void Awake(){
            if(_instance!=null) Destroy(_instance.gameObject);
            _instance = this;
            gameObject.SetActive(false);
        }

        private void Check(){
            if(tasks>0) gameObject.SetActive(true);
            else{
                gameObject.SetActive(false);
            }
        }
    }
}