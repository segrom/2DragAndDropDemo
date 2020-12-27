using System;
using System.IO;
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
        
        public void Save(){
            var path = Application.dataPath+"/NodesSave.json";
            var json = JsonConvert.SerializeObject(new JsonSaveFile(Workspace.Instance.GetNodes()));
            File.WriteAllText(path, json);
            print(json);
        }

        public void ClearAll(){
            Workspace.Instance.ClearAll();
        }
        
        public void Load(){
            ClearAll();
            var path = Application.dataPath+"/NodesSave.json";
            var json = File.ReadAllText(path);
            var file = JsonConvert.DeserializeObject<JsonSaveFile>(json);
            InstanceWorkspaceNodes(file.AllNodes);
            print(json);
        }
        
        public void InstanceWorkspaceNodes(NodeSave[] AllNodes){
            for (int i = 0; i < AllNodes.Length; i++){
                var save = AllNodes[i];
                var newNode = Instantiate(nodePref,Workspace.Instance.transform);
                newNode.sprite = ContentManager.Instance.GetSpriteById(save.spriteId);
                newNode.transform.position = new Vector3(save.posX,save.posY,save.posZ);
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
                    AllNodes[i] = new NodeSave(node.Name,ContentManager.Instance.GetSpriteId(node.sprite),node.transform.position,new ColorSave(node.GetColor()));
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