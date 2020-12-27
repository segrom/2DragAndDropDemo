using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace DragAndDropDemo
{
    public class ContentManager : MonoBehaviour
    {
        public static ContentManager Instance;
        
        [SerializeField]private Sprite[] contentSprites;
        [SerializeField] private NodeMenuItem itemPref;
        [SerializeField] private Canvas canvas;
        private List<NodeMenuItem> _items;

        private void Start(){
            Instance = this;
            _items = new List<NodeMenuItem>();
            foreach (Sprite sprite in contentSprites){
                if(sprite==null) continue;
                NodeMenuItem newItem = Instantiate(itemPref,transform);
                newItem.image.sprite = sprite;
                newItem.canvas = canvas;
                _items.Add(newItem);
            }

            GetComponent<RectTransform>().SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal ,
                (itemPref.GetComponent<RectTransform>().rect.width + 10) * contentSprites.Length);
        }

        public int GetSpriteId(Sprite sprite){
            for (int id = 0; id < contentSprites.Length; id++){
                if (contentSprites[id] == sprite) return id;
            }
            return -1;
        }

        public Sprite GetSpriteById(int id){
            return contentSprites[id];
        }
    }
}