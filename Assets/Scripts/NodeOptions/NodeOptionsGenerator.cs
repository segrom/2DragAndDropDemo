using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DragAndDropDemo.NodeOptions
{
    public class NodeOptionsGenerator : MonoBehaviour
    {
        public static NodeOptionsGenerator Instance;
        
        [SerializeField] private NodeStringOption nodeStringOptionPref;
        [SerializeField] private NodeColorOption nodeColorOptionPref;

        private List<GameObject> _currentOptions = new List<GameObject>();

        private void Start(){
            if(Instance!=null) Destroy(Instance);
            Instance = this;
        }

        public void AddOption(Action<string> callback, string defaultValue, string title = "String"){
            var node = Instantiate(nodeStringOptionPref,transform);
            node.SetDefaultValue(defaultValue);
            node.SetApplyChanges(callback);
            node.SetTitle(title);
            _currentOptions.Add(node.gameObject);
        }
        
        public void AddOption(Action<Color> callback, Color defaultValue, string title = "Color"){
            var node = Instantiate(nodeColorOptionPref,transform);
            node.SetDefaultValue(defaultValue);
            node.SetApplyChanges(callback);
            node.SetTitle(title);
            _currentOptions.Add(node.gameObject);
        }
        
        
        public void ClearOptions(){
            foreach (GameObject option in _currentOptions){
                Destroy(option);
            }
            _currentOptions = new List<GameObject>();
        }
        
    }
}