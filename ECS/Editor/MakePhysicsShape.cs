using Unity.Physics.Authoring;
using UnityEditor;
using UnityEngine;

namespace FPS
{
    public class PrefabProcessor : Editor
    {
        [MenuItem("Assets/Process All Prefabs in Folder", false, 50)]
        private static void ProcessPrefabsInFolder()
        {
            // ������ ������ ��� ��������
            string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogError("������ �׸��� ������ �ƴմϴ�.");
                return;
            }

            // ���� �� ��� ������ ã��
            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

            foreach (string guid in prefabGuids)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                if (prefab != null)
                {
                    Make(prefab);
                    Debug.Log($"Processed: {prefab.name}");
                }
            }

            // ���� ���� ����
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void Make(GameObject obj)
        {
            // ���õ� ������Ʈ�� ������ ���� ǥ��
            if (obj == null)
            {
                return;
            }

            MeshCollider meshCollider = obj.GetComponent<MeshCollider>();

            if (meshCollider == null)
            {
                return;
            }


            var physicsShape = obj.AddComponent<PhysicsShapeAuthoring>();

            physicsShape.SetMesh(meshCollider.sharedMesh);
        }

        [MenuItem("Assets/Create/Remove MeshCollider from Prefabs")]
        private static void RemoveMeshColliders()
        {
            string folderPath = AssetDatabase.GetAssetPath(Selection.activeObject);

            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogError("������ �׸��� ������ �ƴմϴ�.");
                return;
            }

            string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

            foreach (string guid in prefabGuids)
            {
                string prefabPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

                if (prefabAsset != null)
                {
                    // ������ �ν��Ͻ� ����
                    GameObject prefabInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefabAsset);

                    if (prefabInstance != null)
                    {
                        MeshCollider meshCollider = prefabInstance.GetComponent<MeshCollider>();
                        if (meshCollider != null)
                        {
                            DestroyImmediate(meshCollider);
                            Debug.Log($"Removed MeshCollider from: {prefabAsset.name}");

                            // ���� ���� ����
                            PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.AutomatedAction);
                        }

                        // �ν��Ͻ� ����
                        DestroyImmediate(prefabInstance);
                    }
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}