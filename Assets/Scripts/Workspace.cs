using System;
using System.Collections.Generic;
using System.Linq;
using DragAndDropDemo.NodeOptions;
using UnityEngine;
using UnityEngine.UIElements;

namespace DragAndDropDemo
{
    public class Workspace: MonoBehaviour
    {
        public static Workspace Instance;

        [SerializeField]private List<Node> _allNodes, _selectedNodes;
        
        private Vector3? _startMovingPoint;
        private Vector3 _startMovingPosition;
        private Vector3? _startMovingGroupPoint;
        private List<Vector3> _startMovingGroupPositions;

        private void Awake(){
            if(Instance!=null) Destroy(Instance.gameObject);
            Instance = this;
            _allNodes = new List<Node>();
            _selectedNodes = new List<Node>();
        }

        public void AddNode(Node node){
            _allNodes.Add(node);
        }
        
        public void DeleteNode(Node node){
            _selectedNodes.Remove(node);
            _allNodes.Remove(node);
            Destroy(node.gameObject);
            NodeOptionsGenerator.Instance.ClearOptions();
        }
        
#region selectrion
        public void ToggleSelected(Node node){
            if( _selectedNodes.Contains(node)){
                _selectedNodes.Remove(node);
                node.Selected = false;
                return;
            }
            _selectedNodes.Add(node);
            node.Selected = true;
        }
        
        public void SetOneSelected(Node node){
            ClearSelection();
            _selectedNodes.Add(node);
            node.Selected = true;
        }
        
        public void ClearSelection(){
            var buffer = _selectedNodes.ToArray();
            foreach (Node selectedNode in buffer){
                ToggleSelected(selectedNode);
            }
            NodeOptionsGenerator.Instance.ClearOptions();
        }
#endregion

#region group_moving

        public void StartMovingGroup(Node node){
            _startMovingGroupPoint = Input.mousePosition;
            _startMovingGroupPositions = new List<Vector3>();
            foreach (Node selectedNode in _selectedNodes){
                _startMovingGroupPositions.Add(selectedNode.transform.position);
            }
        }
        
        public void StopMovingGroup(Node node){
            _startMovingGroupPoint = null;
        }
#endregion

        private void Update(){
            if (Input.GetMouseButton(2)){
                if (_startMovingPoint == null){
                    _startMovingPoint = Input.mousePosition;
                    _startMovingPosition = transform.position;
                }
                else{
                    var newPos = _startMovingPosition+ (Camera.main.ScreenToWorldPoint(Input.mousePosition) -
                                                        Camera.main.ScreenToWorldPoint(_startMovingPoint.Value));
                    transform.position = new Vector3(newPos.x,newPos.y,transform.position.z);
                }
            }

            if (Input.GetMouseButtonUp(2)){
                _startMovingPoint = null;
            }

            if (Input.mouseScrollDelta.y != 0){
                transform.localScale +=  new Vector3(Input.mouseScrollDelta.y,Input.mouseScrollDelta.y,Input.mouseScrollDelta.y)/10;
            }

            if (_startMovingGroupPoint != null){
                for(int i =0; i< _selectedNodes.Count; i++){
                    var newPos = _startMovingGroupPositions[i]+ (Camera.main.ScreenToWorldPoint(Input.mousePosition) -
                                                                 Camera.main.ScreenToWorldPoint(_startMovingGroupPoint.Value));
                    _selectedNodes[i].transform.position = new Vector3(newPos.x,newPos.y,transform.position.z);
                }
            }

            if (Input.GetKeyDown(KeyCode.Delete)){
                for (int i = 0; i < _selectedNodes.Count; i++){
                    DeleteNode(_selectedNodes[i]);
                }
            }
            
        }

        public void NodeDown(Node node){
            if (Input.GetKey(KeyCode.LeftControl)){
                ToggleSelected(node);
            }
            else{
                if (_selectedNodes.Contains(node)){
                    StartMovingGroup(node);
                }
                else{
                    SetOneSelected(node);
                    StartMovingGroup(node);
                }
            }
            NodeOptionsGenerator.Instance.ClearOptions();
            var lastSelected = _selectedNodes.LastOrDefault();
            if (lastSelected != null){
                lastSelected.ShowOptions();
            }
        }

        public Node[] GetNodes(){
            return _allNodes.ToArray();
        }

        public void SetNodes(Node[] nodes){
            for(int i = 0; i < _allNodes.Count; i++){
                DeleteNode(_allNodes[i]);
            }
            _allNodes = new List<Node>(nodes);
        }
        
        public void ClearAll(){
            var buffer = _allNodes.ToArray();
            foreach (Node node in buffer){
                DeleteNode(node);
            }
            NodeOptionsGenerator.Instance.ClearOptions();
        }
    }
}