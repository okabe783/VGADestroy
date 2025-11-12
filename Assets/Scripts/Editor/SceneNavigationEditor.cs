#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace UnityEditor
{
    public static class SceneNavigationEditor
    {
        [MenuItem("Scene/TitleScene")]
        public static void Scene0()
        {
            EditorSceneManager.SaveOpenScenes();
            OpenScene(0);
        }

        [MenuItem("Scene/GameScene")]
        public static void Scene01()
        {
            EditorSceneManager.SaveOpenScenes();
            OpenScene(1);
        }

        [MenuItem("Scene/ResultScene")]
        public static void Scene02()
        {
            EditorSceneManager.SaveOpenScenes();
            OpenScene(2);
        }

        private static void OpenScene(int sceneIndex)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
            if (!string.IsNullOrEmpty(scenePath))
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }
    }
}
#endif