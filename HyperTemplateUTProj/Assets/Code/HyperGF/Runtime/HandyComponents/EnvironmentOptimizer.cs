using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentOptimizer : MonoBehaviour
{
    [SerializeField] List<BatcherData> otherStaticMeshSetRoots;
    [SerializeField] bool shadow = true, occlusionCulling = false;
    void Awake()
    {
        if (otherStaticMeshSetRoots != null && otherStaticMeshSetRoots.Count > 0)
        {
            var otherMeshHolder = new GameObject("_Gen_OtherMeshes_Holder");
            var newRoot = otherMeshHolder.transform;
            newRoot.SetParent(transform, true);
            newRoot.localPosition = Vector3.zero;
            newRoot.localEulerAngles = Vector3.zero;
            newRoot.localScale = Vector3.one;
            for (int i = 0; i < otherStaticMeshSetRoots.Count; i++)
            {
                var oldRoot = otherStaticMeshSetRoots[i];
                CopyObjectsFromAndDisableOlds(oldRoot, newRoot);
            }
        }

        StaticBatchingUtility.Combine(gameObject);
        transform.OptimizeMeshRenderersInside(shadow, occlusionCulling);
    }

    void CopyObjectsFromAndDisableOlds(BatcherData root, Transform newRoot)
    {
        var rnds = root.Root.GetComponentsInChildren<MeshRenderer>();
        if (rnds != null && rnds.Length > 0)
        {
            for (int i = 0; i < rnds.Length; i++)
            {
                var rn = rnds[i];
                if (rn.enabled == false) { continue; }
                var curObject = rn.gameObject;
                var curTr = rn.transform;
                var newObject = new GameObject("_gen_" + curObject.name);
                var newTr = newObject.transform;
                newTr.SetParent(newRoot, true);
                newTr.position = curTr.position;
                newTr.rotation = curTr.rotation;
                newTr.localScale = curTr.lossyScale;
                var oldMFilter = rn.GetComponent<MeshFilter>();
                var oldMRender = rn.GetComponent<MeshRenderer>();
                rn.enabled = false;

                var newMFilter = newObject.AddComponent<MeshFilter>();
                newMFilter.sharedMesh = oldMFilter.sharedMesh;
                var newMRender = newObject.AddComponent<MeshRenderer>();
                newMRender.sharedMaterial = oldMRender.sharedMaterial;

                newMRender.Optimize(root.Shadow, root.OcclusionCulling);
            }
        }
    }
}
