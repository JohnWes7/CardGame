using System.Collections;
using UnityEngine;

namespace Shaper2D
{
    public class DemoInstantiator : MonoBehaviour
    {
        public Shaper2D prefab;
        public float delayBetweenInstantiations = 0.1f;
        public int numberOfInstantiations = 1000;
    
        private void Start()
        {
            StartCoroutine(InstantiatePrefabs());
        }
    
        private IEnumerator InstantiatePrefabs()
        {
            for (int i = 0; i < numberOfInstantiations; i++)
            {
                var randomPosition = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
                var shaper2d = Instantiate(prefab, transform.position + randomPosition, Quaternion.identity);
                shaper2d.starrines = Random.Range(0f, 1f);
                shaper2d.innerRadius = Random.Range(0.5f, 2f);
                shaper2d.outerRadius = Random.Range(shaper2d.innerRadius + 0.3f, shaper2d.innerRadius + 2f);
                shaper2d.sectorCount = Random.Range(3, 20);
                shaper2d.rotation = Random.Range(0, 360);
                shaper2d.innerColor = Random.ColorHSV();
                shaper2d.outerColor = Random.ColorHSV();
                //Forcing Shaper2D to apply changes in the same frame.
                //Otherwise, it will detect them automatically and apply them in the next frame.
                shaper2d.ForceApplyChanges();
                yield return new WaitForSeconds(delayBetweenInstantiations);
            }
        }
    }
}
