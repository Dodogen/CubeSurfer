using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class OnDestroyDispatcher : MonoBehaviour
{
    [SerializeField] UnityEngine.Object _explosionParticlesHolder;
    [SerializeField] UnityEngine.Object _collectingParticlesHolder;
	private void Start()
	{
		_explosionParticlesHolder = Resources.Load("Explosion");
		_collectingParticlesHolder = Resources.Load("Collect");
	}

	public void DestroyObject(GameObject obj)
	{
        if (_explosionParticlesHolder != null)
        {
            var explosion = (GameObject)Instantiate(_explosionParticlesHolder, obj.transform.position, Quaternion.identity);

			ParticleSystemRenderer particleRenderer = explosion.GetComponent<ParticleSystemRenderer>();
			if (particleRenderer != null)
			{
				Renderer objectRenderer = obj.GetComponent<Renderer>();
				if (objectRenderer != null)
				{
					particleRenderer.material = objectRenderer.material;
				}
			}
			Destroy(explosion, 2f);
        }
        else
        {
			Debug.LogWarning("Система частиц не найдена на объекте.");
		}

		Destroy(obj);

	}

	public void CollectObjectForUI(GameObject obj, Transform uiObject)
	{
		DestroyObject(obj);
		//StartCoroutine(MoveParticlesToTarget(obj, uiObject));
	}
}
