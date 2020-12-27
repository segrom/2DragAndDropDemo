using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace DragAndDropDemo
{
    public class NodeMenuItem : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Node nodePref;
        public Image image;
        public Canvas canvas;

        private void Awake(){
            image = GetComponent<Image>();
        }

        public void OnPointerDown(PointerEventData eventData){
            var newItem = Instantiate(this,canvas.transform);
            DragAndDropController.Instance.CurrentItem = newItem;
            print(newItem);
        }

        public void ConvertToNode(){
            var newNode = Instantiate(nodePref,Workspace.Instance.transform);
            newNode.sprite = image.sprite;
            newNode.transform.position = transform.position;
            newNode.Name = newNode.transform.name;
            Workspace.Instance.AddNode(newNode);
            Destroy(gameObject);
        }
    }
}