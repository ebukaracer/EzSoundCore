using System;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Racer.EzSoundCore.Editor
{
    internal class EzSoundCoreEditor : EditorWindow
    {
        private static RemoveRequest _removeRequest;
        private static bool _isElementsImported;
        private const string PkgId = "com.racer.ezsoundcore";

        private const string ContextMenuPath = "Racer/EzSoundCore/";
        private const string RootPath = "Assets/EzSoundCore";
        private const string SamplesPath = "Assets/Samples/EzSoundCore";
        private static readonly string SourcePath = $"Packages/{PkgId}/Elements";
        private const string ElementsPath = RootPath + "/Elements";


        [MenuItem(ContextMenuPath + "Import Elements", false)]
        private static void ImportElements()
        {
            if (Directory.Exists(ElementsPath))
            {
                Debug.Log(
                    $"Root directory already exists: '{ElementsPath}'" +
                    "\nIf you would like to re-import, remove and reinstall this package.");
                return;
            }

            if (!Directory.Exists(SourcePath))
            {
                Debug.LogError(
                    "Source path is missing. Please ensure this package is installed correctly," +
                    $" otherwise reinstall it.\nNonexistent Path: {SourcePath}");
                return;
            }

            try
            {
                DirUtils.CreateDirectory(RootPath);
                Directory.Move(SourcePath, ElementsPath);
                DirUtils.DeleteEmptyMetaFiles(SourcePath);
                AssetDatabase.Refresh();
                _isElementsImported = AssetDatabase.IsValidFolder(ElementsPath);
                Debug.Log($"Imported successfully at '{ElementsPath}'");
            }
            catch (Exception e)
            {
                Debug.LogError(
                    $"An error occurred while importing this package's elements: {e.Message}\n{e.StackTrace}");
            }
        }


        [MenuItem(ContextMenuPath + "Import Elements", true)]
        private static bool ValidateImportElements()
        {
            _isElementsImported = AssetDatabase.IsValidFolder(ElementsPath);
            return !_isElementsImported;
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

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void MoveMetaFile(string source, string destination)
        {
            if (!File.Exists(source + ".meta")) return;

            File.Move(source + ".meta", destination + ".meta");
        }

        public static void DeleteEmptyMetaFiles(string directory)
        {
            if (Directory.Exists(directory)) return;

            var metaFile = directory + ".meta";

            if (File.Exists(metaFile))
                File.Delete(metaFile);
        }
    }
}