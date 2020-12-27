using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DragAndDropDemo.NodeOptions
{
    public class NodeColorOption: NodeOption<Color>
    {
        [SerializeField]private List<ColorView> views;

        public override void SetDefaultValue(Color value){
            var view = views.FirstOrDefault(v => v.color == value);
            view.button.interactable = false;
        }

        public void OnClick(int order){
            var view = views[order];
            Action?.Invoke(view.color);
            foreach (ColorView colorView in views){
                colorView.button.interactable = true;
            }
            view.button.interactable = false;
        }
    }

    [Serializable]
    public class ColorView
    {
        public Button button;
        public Color color;
    }
}