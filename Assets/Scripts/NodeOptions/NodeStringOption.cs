using System;
using UnityEngine;
using UnityEngine.UI;

namespace DragAndDropDemo.NodeOptions
{
    public class NodeStringOption : NodeOption<string>
    {
        [SerializeField] private InputField _input;

        public void OnChanged(){
            Action?.Invoke(_input.text);
        }

        public override void SetDefaultValue(string value){
            _input.text = value;
        }
    }
}