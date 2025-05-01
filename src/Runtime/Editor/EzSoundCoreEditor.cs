using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Racer.EzSoundCore.Editor
{
    internal static class EzSoundCoreEditor
    {
        private static RemoveRequest _removeRequest;
        private static bool _isElementsImported;
        private const string PkgId = "com.racer.ezsoundcore";
        private const string AssetPkgId = "EzSoundCore.unitypackage";

        private const string ContextMenuPath = "Racer/EzSoundCore/";
        private const string ImportElementsContextMenuPath = ContextMenuPath + "Import Elements";
        private const string ForceImportElementsContextMenuPath = ContextMenuPath + "Import Elements(Force)";

        private const string RootPath = "Assets/EzSoundCore";
        private const string SamplesPath = "Assets/Samples/EzSoundCore";


        [MenuItem(ImportElementsContextMenuPath, false)]
        private static void ImportElements()
        {
            var packagePath = $"Packages/{PkgId}/Elements/{AssetPkgId}";

            if (File.Exists(packagePath))
                AssetDatabase.ImportPackage(packagePath, true);
            else
                EditorUtility.DisplayDialog("Missing Package File", $"{AssetPkgId} not found in the package.", "OK");
        }

        [MenuItem(ForceImportElementsContextMenuPath, false)]
        private static void ForceImportElements()
        {
            ImportElements();
        }

        [MenuItem(ImportElementsContextMenuPath, true)]
        private static bool ValidateImportElements()
        {
            _isElementsImported = AssetDatabase.IsValidFolder($"{RootPath}/Elements");
            return !_isElementsImported;
        }

        [MenuItem(ForceImportElementsContextMenuPath, true)]
        private static bool ValidateForceImportElements()
        {
            return _isElementsImported;
        }

        [MenuItem(ContextMenuPath + "Remove Package(recommended)")]
        private static void RemovePackage()
        {
            _removeRequest = Client.Remove(PkgId);
            EditorApplication.update += RemoveProgress;
        }

        private static void RemoveProgress()
        {
            if (!_removeRequest.IsCompleted) return;

            switch (_removeRequest.Status)
            {
                case StatusCode.Success:
                {
                    DirUtils.DeleteDirectory(RootPath);
                    DirUtils.DeleteDirectory(SamplesPath);
                    AssetDatabase.Refresh();

                    break;
                }
                case >= StatusCode.Failure:
                    Debug.LogWarning($"Failed to remove package: '{PkgId}'");
                    break;
            }

            EditorApplication.update -= RemoveProgress;
        }
    }

    internal static class DirUtils
    {
        public static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path)) return;

            Directory.Delete(path, true);
            DeleteEmptyMetaFiles(path);
        }

        private static void DeleteEmptyMetaFiles(string directory)
        {
            if (Directory.Exists(directory)) return;

            var metaFile = directory + ".meta";

            if (File.Exists(metaFile))
                File.Delete(metaFile);
        }
    }
}