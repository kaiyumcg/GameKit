using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[DefaultExecutionOrder(-8000)]
public class PreloadHandler : MonoBehaviour
{
    [SerializeField] List<AssetReferenceGameObject> additionalPrefabDependencies;

    List<GameObject> dependencyPrefabs = null;
    List<AsyncOperationHandle<GameObject>> depHandles = default;
    static PreloadHandler instance;
    bool allDependencyLoaded = false;

    public static List<GameObject> DependencyPrefabs { get { return instance.dependencyPrefabs; } }
    public bool AllDependencyLoaded { get { return allDependencyLoaded; } set { allDependencyLoaded = value; } }
    public static PreloadHandler Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            DestroyImmediate(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        if (depHandles != null && depHandles.Count > 0)
        {
            for (int i = 0; i < depHandles.Count; i++)
            {
                var dep = depHandles[i];
                if (dep.IsValid())
                {
                    Addressables.Release(dep);
                    depHandles[i] = default;
                }
            }
        }
    }

    public IEnumerator LoadAdditionalDependencies()
    {
        int completedCount = 0;
        int validTotalCount = 0;
        if (additionalPrefabDependencies != null && additionalPrefabDependencies.Count > 0)
        {
            for (int i = 0; i < additionalPrefabDependencies.Count; i++)
            {
                var aref = additionalPrefabDependencies[i];
                if (aref == null) { continue; }
                if (aref.IsValid()) { validTotalCount++; }
            }
        }

        if (additionalPrefabDependencies != null && additionalPrefabDependencies.Count > 0)
        {
            for (int i = 0; i < additionalPrefabDependencies.Count; i++)
            {
                var aref = additionalPrefabDependencies[i];
                StartCoroutine(GranularSingleAssetLoader(aref, i, (obj, handle, id) =>
                {
                    depHandles[id] = handle;
                    dependencyPrefabs[id] = obj;
                    completedCount++;
                }));
            }
        }

        while (completedCount < validTotalCount) { yield return null; }
        allDependencyLoaded = true;
    }

    IEnumerator GranularSingleAssetLoader(AssetReferenceGameObject assetRef, int index, System.Action<GameObject, AsyncOperationHandle<GameObject>, int> OnComplete)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(assetRef);
        handle.Completed += Handle_Completed;
        yield return null;

        void Handle_Completed(AsyncOperationHandle<GameObject> handle_obj)
        {
            if (handle_obj.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject obj = handle_obj.Result;
                OnComplete?.Invoke(obj, handle_obj, index);
            }
        }
    }
}