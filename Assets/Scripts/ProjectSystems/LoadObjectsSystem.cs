using System.IO;
using TestGame.Settings;
using UnityEngine;
using Zenject;

namespace TestGame.ProjectSystems
{
    public class LoadObjectsSystem : MonoBehaviour, ISystem
    {
        [Inject]
        public void Construct()
        {
            Utilities.Logger.Log("LoadObjectsSystem Construct", LogTypes.Info);
        }

        public void Init()
        {
        }

        public T GetObjectByPath<T>(string path) where T : UnityEngine.Object
        {
            return LoadFromResources<T>(path);
        }

        private T LoadFromResources<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(ParsePath(path));
        }

        public T[] GetObjectsByPath<T>(string path) where T : UnityEngine.Object
        {
            return Resources.LoadAll<T>(ParsePath(path));
        }

        public void DeleteDirectoryRecursive(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return;
            }

            string[] files = Directory.GetFiles(directory);
            string[] dirs = Directory.GetDirectories(directory);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectoryRecursive(dir);
            }

            Directory.Delete(directory, false);
        }

        private string ParsePath(string path)
        {
            string[] parsed = path.Split('/');
            path = string.Empty;

            for (int i = 0; i < parsed.Length; i++)
            {
                path += parsed[i][0].ToString().ToUpper() + parsed[i].Substring(1, parsed[i].Length - 1);

                if (i < parsed.Length - 1)
                {
                    path += '/';
                }
            }
            return path;
        }
    }
}