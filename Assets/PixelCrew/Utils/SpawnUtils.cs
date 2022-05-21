using UnityEngine;

namespace PixelCrew.Utils
{
    public class SpawnUtils
    {
        public const string ContainerName = "*** SPAWNED ***";

        public static GameObject Spawn(GameObject prefub, Vector3 position, string containerName = ContainerName)
        {
            var container = GameObject.Find(containerName);
            if (container == null)
                container = new GameObject(containerName);

            return GameObject.Instantiate(prefub, position, Quaternion.identity, container.transform);
        }
    }
}