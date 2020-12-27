using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DragAndDropDemo
{
    public class WorkspaceBackground: MonoBehaviour, IPointerClickHandler
    {
        /*private void OnMouseUp(){
            print("Focus out");
            Workspace.Instance.ClearSelection();
        }*/

        public void OnPointerClick(PointerEventData eventData){
            print("Focus out");
            Workspace.Instance.ClearSelection();
        }
    }
}