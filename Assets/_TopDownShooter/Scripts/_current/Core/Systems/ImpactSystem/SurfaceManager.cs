using System;
using System.Collections;
using System.Collections.Generic;
using _current.Core.Systems.ImpactSystem.SurfaceEffects;
using _current.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _current.Core.Systems.ImpactSystem
{
    public class SurfaceManager : MonoBehaviour
    {
        public static SurfaceManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        [SerializeField]
        private List<SurfaceType> _surfaces = new();
        [SerializeField]
        private Surface _defaultSurface;

        private readonly Dictionary<GameObject, GameObjectPool> _objectPools = new();
        private GameObject _spawnParticle;
        [SerializeField] private ParticleSystem[] _spawnParticles;

        public void HandleImpact(GameObject hitObject, Vector3 hitPoint, Vector3 hitNormal, ImpactType impact, int triangleIndex)
        {
            if (hitObject.TryGetComponent<Terrain>(out var terrain))
            {
                List<TextureAlpha> activeTextures = GetActiveTexturesFromTerrain(terrain, hitPoint);
                foreach (TextureAlpha activeTexture in activeTextures)
                {
                    SurfaceType surfaceType = _surfaces.Find(surface => surface.Albedo == activeTexture.Texture);
                    if (surfaceType != null)
                    {
                        foreach (Surface.SurfaceImpactTypeEffect typeEffect in surfaceType.Surface.ImpactTypeEffects)
                        {
                            if (typeEffect.ImpactType == impact)
                            {
                                PlayEffects(hitPoint, hitNormal, typeEffect.SurfaceEffect, activeTexture.Alpha);
                            }
                        }
                    }
                    else
                    {
                        foreach (Surface.SurfaceImpactTypeEffect typeEffect in _defaultSurface.ImpactTypeEffects)
                        {
                            if (typeEffect.ImpactType == impact)
                            {
                                PlayEffects(hitPoint, hitNormal, typeEffect.SurfaceEffect, 1);
                            }
                        }
                    }
                }
            }
            else if (hitObject.TryGetComponent<Renderer>(out var surfaceRenderer))
            {
                Texture activeTexture = GetActiveTextureFromRenderer(surfaceRenderer, triangleIndex);

                SurfaceType surfaceType = _surfaces.Find(surface => surface.Albedo == activeTexture);
                if (surfaceType != null)
                {
                    foreach (Surface.SurfaceImpactTypeEffect typeEffect in surfaceType.Surface.ImpactTypeEffects)
                    {
                        if (typeEffect.ImpactType == impact)
                        {
                            PlayEffects(hitPoint, hitNormal, typeEffect.SurfaceEffect, 1);
                        }
                    }
                }
                else
                {
                    foreach (Surface.SurfaceImpactTypeEffect typeEffect in _defaultSurface.ImpactTypeEffects)
                    {
                        if (typeEffect.ImpactType == impact)
                        {
                            PlayEffects(hitPoint, hitNormal, typeEffect.SurfaceEffect, 1);
                        }
                    }
                }
            }
        }

        private List<TextureAlpha> GetActiveTexturesFromTerrain(Terrain terrain, Vector3 hitPoint)
        {
            Vector3 terrainPosition = hitPoint - terrain.transform.position;
            var terrainData = terrain.terrainData;
            Vector3 splatMapPosition = new Vector3(
                terrainPosition.x / terrainData.size.x,
                0,
                terrainPosition.z / terrainData.size.z
            );

            int x = Mathf.FloorToInt(splatMapPosition.x * terrainData.alphamapWidth);
            int z = Mathf.FloorToInt(splatMapPosition.z * terrain.terrainData.alphamapHeight);

            float[,,] alphaMap = terrain.terrainData.GetAlphamaps(x, z, 1, 1);

            List<TextureAlpha> activeTextures = new List<TextureAlpha>();
            for (int i = 0; i < alphaMap.Length; i++)
            {
                if (alphaMap[0, 0, i] > 0)
                {
                    activeTextures.Add(new TextureAlpha()
                    {
                        Texture = terrain.terrainData.terrainLayers[i].diffuseTexture,
                        Alpha = alphaMap[0, 0, i]
                    });
                }
            }

            return activeTextures;
        }

        private Texture GetActiveTextureFromRenderer(Renderer surfaceRenderer, int triangleIndex)
        {
            if (surfaceRenderer.TryGetComponent<MeshFilter>(out var meshFilter))
            {
                Mesh mesh = meshFilter.mesh;

                if (!mesh.isReadable)
                {
                    return null;
                }

                if (mesh.subMeshCount > 1)
                {
                    int[] hitTriangleIndices = new int[]
                    {
                        mesh.triangles[triangleIndex * 3],
                        mesh.triangles[triangleIndex * 3 + 1],
                        mesh.triangles[triangleIndex * 3 + 2]
                    };

                    for (int i = 0; i < mesh.subMeshCount; i++)
                    {
                        int[] submeshTriangles = mesh.GetTriangles(i);
                        for (int j = 0; j < submeshTriangles.Length; j += 3)
                        {
                            if (submeshTriangles[j] == hitTriangleIndices[0]
                                && submeshTriangles[j + 1] == hitTriangleIndices[1]
                                && submeshTriangles[j + 2] == hitTriangleIndices[2])
                            {
                                return surfaceRenderer.sharedMaterials[i].mainTexture;
                            }
                        }
                    }
                }
                else
                {
                    return surfaceRenderer.sharedMaterial.mainTexture;
                }
            }

            Debug.LogError($"{surfaceRenderer.name} has no MeshFilter! Using default impact effect instead of texture-specific one because we'll be unable to find the correct texture!");
            return null;
        }

        private void PlayEffects(Vector3 hitPoint, Vector3 hitNormal, SurfaceEffect surfaceEffect, float soundOffset)
        {
            foreach (SpawnObjectEffect spawnObjectEffect in surfaceEffect.SpawnObjectEffects)
            {
                if (spawnObjectEffect.Probability > Random.value)
                {
                    if (_spawnParticle == null)
                    {
                        _spawnParticle = Instantiate(spawnObjectEffect.Prefab, transform);
                        _spawnParticles = _spawnParticle.GetComponentsInChildren<ParticleSystem>();
                    }
                    _spawnParticle.gameObject.transform.position = hitPoint + hitNormal * 0.001f;
                    _spawnParticle.gameObject.transform.rotation = Quaternion.LookRotation(hitNormal);

                    _spawnParticle.transform.forward = hitNormal;
                    if (spawnObjectEffect.RandomizeRotation)
                    {
                        Vector3 offset = new Vector3(
                            Random.Range(0, 180 * spawnObjectEffect.RandomizedRotationMultiplier.x),
                            Random.Range(0, 180 * spawnObjectEffect.RandomizedRotationMultiplier.y),
                            Random.Range(0, 180 * spawnObjectEffect.RandomizedRotationMultiplier.z)
                        );

                        _spawnParticle.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + offset);
                    }

                    foreach (var particle in _spawnParticles)
                    {
                        particle.Emit(1);
                    }
                }
            }

            foreach(PlayAudioEffect playAudioEffect in surfaceEffect.PlayAudioEffects)
            {
                AudioClip clip = playAudioEffect.AudioClips[Random.Range(0, playAudioEffect.AudioClips.Count)];
                
                var pool = TryGetPool(playAudioEffect.AudioSourcePrefab.gameObject);
                AudioSource audioSource = pool.Get().GetComponent<AudioSource>();

                audioSource.transform.position = hitPoint;
                audioSource.PlayOneShot(clip, soundOffset * Random.Range(playAudioEffect.VolumeRange.x, playAudioEffect.VolumeRange.y));
                StartCoroutine(DisableGameObject(clip.length,
                    () => pool.Release(audioSource.gameObject)));
            }
        }

        private GameObjectPool TryGetPool(GameObject prefab)
        {
            if (_objectPools.TryGetValue(prefab, out var pool))
            {
                return pool;
            }

            var newPool = new GameObjectPool(prefab, transform);
            _objectPools.Add(prefab, newPool);
            return newPool;
        }

        private IEnumerator DisableGameObject(float time, Action disableAction)
        {
            yield return new WaitForSeconds(time);
            disableAction?.Invoke();
        }

        private class TextureAlpha
        {
            public float Alpha;
            public Texture Texture;
        }
    }
}