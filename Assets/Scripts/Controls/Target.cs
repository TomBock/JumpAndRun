using System;
using System.Linq;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controls
{

    public class Target : MonoBehaviour
    {
        public static void SpawnRandomlyOnMap()
        {
            var meshFilters = FindObjectsOfType <MeshFilter>();
            var loadedPrefab = Resources.Load<GameObject>("Prefabs/ExampleTarget");
           
            foreach (var meshFilter in meshFilters)
            {
                var point = Utils.GetRandomPointOnMesh(meshFilter.sharedMesh);
                var target = Instantiate(loadedPrefab, meshFilter.transform.position + point + Vector3.up * .5f, Quaternion.identity);
            }
        }

        public static void SpawnOnMap()
        {
            var loadedPrefab = Resources.Load<GameObject>("Prefabs/ExampleTarget");
            foreach (var targetSpawn in FindObjectsOfType<TargetSpawn>())
            {
                var target = Instantiate(loadedPrefab, targetSpawn.transform.position, Quaternion.identity);
            }
        }

        private void OnEnable()
        {
            PointsDisplay.instance.targets.Add(this);
            GetComponent <MeshRenderer>().material.color = Random.ColorHSV();

            var collider = GetComponent <Collider>();

            if (EnvironmentColliders.Instance.colliders.Any(x => x.bounds.Intersects(collider.bounds)))
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            PointsDisplay.instance.targets.Remove(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.gameObject.CompareTag("PlayerAttack"))
            {
                PlayerData.points++;
                AudioPlayer.Instance.PlayTargetHit(this);
                Destroy(gameObject);
            }
        }

    }

}
