using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace MyVFX.Grass
{
    public class GrassSpawner : MonoBehaviour
    {
        //[SerializeField] private ComputeShader computeShader;
        [SerializeField] private Material planeMaterial;
        [SerializeField] private Mesh grassMesh;
        [SerializeField] private int grassResolution;
        Mesh _triangleMesh;
        
        private List<Vector2> grassPositions;
        private RenderParams renderParams;
        private ComputeBuffer computeBuffer;
        private void Start()
        {
            renderParams = new RenderParams(planeMaterial)
            {
                shadowCastingMode = ShadowCastingMode.Off,
                receiveShadows = true,
                worldBounds = new Bounds(Vector2.zero, Vector2.one * 1000)
            };
            _triangleMesh = new Mesh()
            {
                vertices = new[]
                {
                    new Vector3(-0.1f, 0),
                    new Vector3(0.1f, 0),
                    new Vector3(0, 1f)
                },
                triangles = new [] {0, 1, 2, 0, 2, 1},
                normals = new []
                {
                    Vector3.forward, 
                    Vector3.forward, 
                    Vector3.forward
                }
            };

            computeBuffer = new ComputeBuffer(grassResolution * grassResolution, 8);
            
            grassPositions = new List<Vector2>(grassResolution * grassResolution);

            for (var x = 0; x < grassResolution; x++)
                for (var y = 0; y < grassResolution; y++)
                {
                    var newPosition = new Vector2(x*x / 5f, y / 5f);
                    newPosition += new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                    grassPositions.Add(newPosition);
                }
            computeBuffer.SetData(grassPositions);
            planeMaterial.SetBuffer(Shader.PropertyToID("PositionsBuffer"), computeBuffer);
        }

        private void OnDestroy()
        {
            computeBuffer?.Dispose();
        }

        private void Update()
        {
            planeMaterial.SetVector(Shader.PropertyToID("_Position"), transform.position);
            Graphics.RenderMeshPrimitives(renderParams, grassMesh, 0, grassResolution * grassResolution);
        }
    }
}
