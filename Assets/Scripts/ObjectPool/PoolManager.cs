using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public List<GameObject> poolPrefabs;
    private List<ObjectPool<GameObject>> poolEffectList = new List<ObjectPool<GameObject>>();

    #region Event functions

    private void OnEnable()
    {
        EventHandler.ParticleEffectEvent += OnParticleEffectEvent;
    }

    private void OnDisable()
    {
        EventHandler.ParticleEffectEvent -= OnParticleEffectEvent;
    }


    private void Start()
    {
        CreatePool();
    }

    #endregion


    #region EventHandler Events

    private void OnParticleEffectEvent(ParticleEffectType effectType, Vector3 pos)
    {
        // WORKFLOW
        ObjectPool<GameObject> objPool = effectType switch
        {
            ParticleEffectType.Leaves01 => poolEffectList[0],
            ParticleEffectType.Leaves02 => poolEffectList[1],
            _ => null
        };

        GameObject obj = objPool.Get();
        obj.transform.position = pos;
        StartCoroutine(ReleaseCoroutine(objPool, obj));
    }

    #endregion

    private void CreatePool()
    {
        foreach (GameObject item in poolPrefabs)
        {
            Transform parent = new GameObject(item.name).transform;
            parent.SetParent(transform);

            ObjectPool<GameObject> newPool = new ObjectPool<GameObject>(
                () => Instantiate(item, parent),
                e => { e.SetActive(true); },
                e => { e.SetActive(false); },
                Destroy,
                true,
                10,
                20
            );

            poolEffectList.Add(newPool);
        }
    }

    private IEnumerator ReleaseCoroutine(ObjectPool<GameObject> pool, GameObject obj)
    {
        yield return new WaitForSeconds(1.5f);
        pool.Release(obj);
    }
}