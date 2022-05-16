using UnityEngine;

namespace PixelCrew.Utils
{
    public class SpawnUtils
    {
        public const string ContainerName = "*** SPAWNED ***";

        public static GameObject Spawn(GameObject prefub, Vector3 position)
        {
            var container = GameObject.Find(ContainerName);
            if (container == null)
                container = new GameObject(ContainerName);

            return GameObject.Instantiate(prefub, position, Quaternion.identity, container.transform);
        }
    }
}