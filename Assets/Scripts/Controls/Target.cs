using System;
using System.Linq;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controls
{

    public class Target : MonoBehaviour
    {
        public static void SpawnOnMap()
        {
            var meshFilters = FindObjectsOfType <MeshFilter>();

            foreach (var meshFilter in meshFilters)
            {
                var point = Utils.GetRandomPointOnMesh(meshFilter.sharedMesh);
                var loadedPrefab = Resources.Load<GameObject>("Prefabs/ExampleTarget");
                var target = Instantiate(loadedPrefab, meshFilter.transform.position + point + Vector3.up * .5f, Quaternion.identity);
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
                Destroy(gameObject);
            }
        }

    }

}
