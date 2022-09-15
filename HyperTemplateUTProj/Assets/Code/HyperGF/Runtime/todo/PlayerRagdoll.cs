using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100000)]
public class PlayerRagdoll : MonoBehaviour
{
    [SerializeField, DebugView] List<Rigidbody> rigidbodies;
    [SerializeField, DebugView] List<Collider> colliders;

#if UNITY_EDITOR
    //[EasyButtons.Button("Get Ragdoll data")]
    //todo do context menu and search for easy button and remove them with that
    //todo make UI particle effect prefab with sample sprite
    private void GetRagdollData()
    {
        var dirtyObjects = new List<UnityEngine.Object>();

        var cols = GetComponentsInChildren<Collider>();
        colliders = new List<Collider>(); 
        colliders.AddRange(cols);
        colliders.ExForEach((c) => { c.enabled = false; dirtyObjects.Add(c); dirtyObjects.Add(c.gameObject); });


        var rgds = GetComponentsInChildren<Rigidbody>();
        rigidbodies = new List<Rigidbody>(); 
        rigidbodies.AddRange(rgds);
        rigidbodies.ExForEach((r) => { r.velocity = Vector3.zero; r.isKinematic = true; dirtyObjects.Add(r); dirtyObjects.Add(r.gameObject); });

        dirtyObjects.ExForEach((o) => { UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(o); });
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }
#endif


    private void Awake()
    {
        colliders.ExForEach((c) => { c.enabled = false; });
        rigidbodies.ExForEach((r) => { r.velocity = Vector3.zero; r.isKinematic = true; });
    }

    public void StartRagdoll()
    {
        GetComponent<Animator>().enabled = false;
        transform.SetParent(null, true);
        foreach (var c in colliders) { c.enabled = true; }
        foreach (var r in rigidbodies) { r.isKinematic = false; }
    }

#if UNITY_EDITOR
    //[EasyButtons.Button("Remove Ragdoll")]
    public void RemoveRagdoll()
    {
        var dirtyObjects = new List<UnityEngine.Object>();

        var cols = GetComponentsInChildren<Collider>();
        cols.ExForEach((c) => { dirtyObjects.Add(c.gameObject); DestroyImmediate(c); });

        var joints = GetComponentsInChildren<CharacterJoint>();
        joints.ExForEach((c) => { dirtyObjects.Add(c.gameObject); DestroyImmediate(c); });

        var rgds = GetComponentsInChildren<Rigidbody>();
        rgds.ExForEach((c) => { dirtyObjects.Add(c.gameObject); DestroyImmediate(c); });

        dirtyObjects.Add(gameObject);

        dirtyObjects.ExForEach((o) => { UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(o); });
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
    }
#endif
}
