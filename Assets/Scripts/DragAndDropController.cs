using System;
using UnityEngine;

namespace DragAndDropDemo
{
    public class DragAndDropController : MonoBehaviour
    {
        public static DragAndDropController Instance;
        
        public NodeMenuItem CurrentItem = null;

        [SerializeField] private ParticleSystem splashPref;
        
        private void Awake(){
            if(Instance!=null) Destroy(Instance.gameObject);
            Instance = this;
        }

        private void Update(){
            if (CurrentItem != null ){
                var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var newPos = new Vector3(worldPos.x, worldPos.y, 0);
                if (Vector3.Distance(CurrentItem.transform.position, newPos) < 0.01f || Vector3.Distance(CurrentItem.transform.position, newPos)>10)
                    CurrentItem.transform.position = newPos;
                else{
                    CurrentItem.transform.position =
                        Vector3.Lerp(CurrentItem.transform.position, newPos, 0.1f);
                    CurrentItem.transform.LookAt(-transform.forward*0.7f+newPos);
                }
            }

            if (Input.GetMouseButtonUp(0)){
                if (CurrentItem != null){
                     CurrentItem.ConvertToNode();
                     var splash = Instantiate(splashPref, CurrentItem.transform.position,Quaternion.identity,null);
                }
            }
        }
        
    }
}