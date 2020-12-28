using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace DragAndDropDemo.Utils
{
    public class ContentSaver : MonoBehaviour
    {
        [SerializeField] private Node nodePref;
        private void Awake(){
            if(Application.platform==RuntimePlatform.WebGLPlayer)gameObject.SetActive(false);
        }

        #if PLATFORM_STANDALONE_WIN

        private Queue<Action> _asyncActions = new Queue<Action>();

        private void Update(){
            while (_asyncActions.Count>0){
                _asyncActions.Dequeue().Invoke();
            }
            _asyncActions = new Queue<Action>();
        }

        public async void Save(){
            LoadingPanel.PushTask();
            var path = Application.dataPath+"/NodesSave.json";
            var json = JsonConvert.SerializeObject(new JsonSaveFile(Workspace.Instance.GetNodes()));
            using (StreamWriter writer = File.CreateText(path)){
                await writer.WriteAsync(json);
            }
            print(json);
            LoadingPanel.PopTask();
        }

        public void ClearAll(){
            Workspace.Instance.ClearAll();
        }
        
        public async void Load(){
            LoadingPanel.PushTask();
            ClearAll();
            var path = Application.dataPath+"/NodesSave.json";
            var json = "";
            using (StreamReader reader = File.OpenText(path)){
                 json = await reader.ReadToEndAsync();
            }
            var file = JsonConvert.DeserializeObject<JsonSaveFile>(json);
            _asyncActions.Enqueue(() => { InstanceWorkspaceNodes(file.AllNodes); });
            _asyncActions.Enqueue(()=> { print(json); });
            await WaitZeroActions();
            LoadingPanel.PopTask();
        }

        private async Task WaitZeroActions(){
            while (_asyncActions.Count>0){
                await Task.Delay(25);
            }
        }
        
        public void InstanceWorkspaceNodes(NodeSave[] AllNodes){
            for (int i = 0; i < AllNodes.Length; i++){
                var save = AllNodes[i];
                var newNode = Instantiate(nodePref,Workspace.Instance.transform);
                newNode.sprite = ContentManager.Instance.GetSpriteById(save.spriteId);
                newNode.transform.position =Workspace.Instance.transform.TransformPoint(new Vector3(save.posX,
                 save.posY, save.posZ));
                newNode.Name = save.Name;
                newNode.SetColor(save.Color.GetColor());
                Workspace.Instance.AddNode(newNode);
            }
        }

        class JsonSaveFile
        {
            public NodeSave[] AllNodes;

            [JsonConstructor]
            public JsonSaveFile(NodeSave[] allNodes){
                AllNodes = allNodes;
            }

            public JsonSaveFile(Node[] allNodes){
                AllNodes = new NodeSave[allNodes.Length];
                for (int i = 0; i < allNodes.Length; i++){
                    var node = allNodes[i];
                    AllNodes[i] = new NodeSave(node.Name,ContentManager.Instance.GetSpriteId(node.sprite),Workspace.Instance.transform.InverseTransformPoint(node.transform.position),new ColorSave(node.GetColor()));
                }
            }
            
        }

        public class NodeSave
        {
            public string Name;
            public int spriteId;
            public float posX, posY, posZ;
            public ColorSave Color;

            [JsonConstructor]
            public NodeSave(string name, int spriteId, float posX, float posY, float posZ, ColorSave Color){
                this.Name = name;
                this.spriteId = spriteId;
                this.posX = posX;
                this.posY = posY;
                this.posZ = posZ;
                this.Color = Color;
            }

            public NodeSave(string name, int spriteId, Vector3 position, ColorSave color){
                this.Name = name;
                this.spriteId = spriteId;
                this.posX = position.x;
                posY = position.y;
                posZ = position.z;
                Color = color;
            }
        }
        
        public struct ColorSave
        {
            public float r, g, b, a;

            [JsonConstructor]
            public ColorSave(float r, float g, float b, float a){
                this.r = r;
                this.g = g;
                this.b = b;
                this.a = a;
            }

            public ColorSave(Color color){
                this.r = color.r;
                this.g = color.g;
                this.b = color.b;
                this.a = color.a;
            }

            public Color GetColor(){
                return new Color(r,g,b,a);
            }
        }
        
        #endif
    }
}