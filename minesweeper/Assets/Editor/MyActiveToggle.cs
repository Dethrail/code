using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyActiveToggle : Editor
{
    [MenuItem("GameObject/ActiveToggle _F3")] //&#a
    private static void ToggleActivationSelection()
    {
        var transforms = Selection.transforms;
        if (Selection.activeTransform != null) {
            var targetTransformState = Selection.activeTransform.gameObject.activeSelf;
            foreach (Transform t in transforms) { t.gameObject.SetActive(!targetTransformState); }
        }
        if (!EditorApplication.isPlaying) {
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        //TODO set scene dirty
    }

    [MenuItem("GameObject/Unlink prefab %j")]
    private static void BreakPrefabLink()
    {
        //string name = prefabedGO.name;
        Vector3 pos = Selection.activeTransform.position;
        Transform parent = Selection.activeTransform.parent;
        Selection.activeTransform.SetParent(null); // unparent the GO so that world transforms are preserved
        GameObject unprefabedGO = Instantiate(Selection.activeGameObject); // clears prefab link
        unprefabedGO.name = Selection.activeGameObject.name;
        unprefabedGO.SetActive(Selection.activeGameObject.activeInHierarchy);

        //DestroyImmediate(Selection.activeGameObject); // or this could just hide the old one .active = false;
        Selection.activeTransform.SetParent(parent); // unparent the GO so that world transforms are preserved
        Selection.activeTransform.position = pos;
        unprefabedGO.transform.SetParent(parent);
        unprefabedGO.transform.position = pos;
    }

    [MenuItem("Help/ClearPlayerPrefs")] //&#a
    private static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}