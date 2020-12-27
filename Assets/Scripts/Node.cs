using System;
using System.Collections;
using DragAndDropDemo.NodeOptions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DragAndDropDemo
{
    public class Node : MonoBehaviour
    {
        public Sprite sprite;
        [SerializeField] private GameObject border;
        [SerializeField] private TextMesh nameText;

        public string Name{
            get => _name;
            set{
                nameText.text = value;
                _name = value;
            }
        }

        private string _name;
        public bool Selected{
            get => _selected;
            set{
                border.SetActive(value);
                nameText.gameObject.SetActive(value);
                _selected = value;
            }
        }
        
        private bool _selected= false;
        private SpriteRenderer _sr;

        private void Awake(){
            _sr =  GetComponent<SpriteRenderer>();

        }

        private void Start(){ 
            _sr.sprite = sprite;
        }
        

        private void OnMouseDown(){
            Workspace.Instance.NodeDown(this);
        }
        private void OnMouseUp(){
            Workspace.Instance.StopMovingGroup(Selected?null:this);
        }
        
        public void ShowOptions(){
            NodeOptionsGenerator.Instance.AddOption(value => { Name = value;},Name,"Name");
            NodeOptionsGenerator.Instance.AddOption(value => { _sr.color = value;},_sr.color);
        }

        public Color GetColor(){
            return _sr.color;
        }

        public void SetColor(Color color){
            _sr.color = color;
        }
    }
}