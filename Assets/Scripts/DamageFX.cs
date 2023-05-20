using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageFX : MonoBehaviour
{
    private const int TotalPoints = 200;
    public float DecayTickRate = .25f;
    public float DecayPerTick = 1.0f;
    private Vector4[] _points;
    private Vector4[] _data;
    private int _counter;
    private Renderer _renderer;
    private bool _dataDirty;

    [SerializeField] private Material defaultAsteroidMaterial;
    [SerializeField] private Material damageFxMaterial;
    [SerializeField] private float hitTimeLimit = 10f; // The time limit after hit to keep the damageFxMaterial.
    private float lastHitTime; // The time of the last hit.
    private Material instancedDamageFxMaterial; // The instanced material.

    // Use this for initialization
    private void Start()
    {
        _points = new Vector4[TotalPoints];
        _data = new Vector4[TotalPoints];
        for (var i = 0; i < TotalPoints; i++)
        {
            _points[i] = Vector4.zero;
            _data[i] = Vector4.zero;
        }
        _renderer = GetComponent<Renderer>();
        instancedDamageFxMaterial = new Material(damageFxMaterial);
        _renderer.material = defaultAsteroidMaterial;
    }

    private void MaterialUpdateArrays(int points)
    {
        _renderer.material.SetVectorArray("_Points", _points);
        _renderer.material.SetVectorArray("_Data", _data);
        _renderer.material.SetInt("_PointsCount", points);
    }

    private void Update()
    {
        if (Time.time - lastHitTime > hitTimeLimit && _renderer.material == instancedDamageFxMaterial)
        {
            _renderer.material = defaultAsteroidMaterial;
            MaterialUpdateArrays(_counter);
        }
    }
    public void Clear()
    {
        for (var i = 0; i < _points.Length; i++)
        {
            _points[i] = Vector4.zero;
            _data[i] = Vector4.zero;
        }
        _renderer.material.SetVectorArray("_Points", _points);
        _renderer.material.SetVectorArray("_Data", _data);
    }

    public void LastHitUpdate()
    {
        lastHitTime = Time.time; // Record the time of the hit.
        if (_renderer.material != instancedDamageFxMaterial)
        {
            _renderer.material = instancedDamageFxMaterial;
            MaterialUpdateArrays(_counter);
        }
    }

    private void AddPoint(Vector3 point, float hitRadius, float dirt, float burn, float heat, float clip)
    {
        _points[_counter] = point; // VECTOR3 COORDINATE 
        _points[_counter].w = 1 - hitRadius;
        _data[_counter].x = dirt;
        _data[_counter].y = heat;
        _data[_counter].z = burn;
        _data[_counter].w = clip;
        if (_dataDirty) return;
        _dataDirty = true;
        StartCoroutine("SetData", DecayTickRate);
    }

    public void Hit(Vector3 point, float hitRadius, float dirt, float burn, float heat, float clip)
    {
            for (var i = 0; i < 1; i++)
            {
                AddPoint(point, hitRadius, dirt, burn, heat, clip);
                _counter++;
                if (_counter == TotalPoints)
                    _counter = 0;
            }
            MaterialUpdateArrays(_counter);
    }

    private IEnumerator SetData(float delay)
    {
        while (_dataDirty)
        {
            var dirty = false;
            for (var i = 0; i < TotalPoints; i++)
            {
                if (_data[i].y < Mathf.Epsilon && _data[i].z < Mathf.Epsilon) continue;
                _data[i].y = Mathf.Max(0, _data[i].y - Time.deltaTime * DecayPerTick);
                _data[i].z = Mathf.Max(0, _data[i].z - Time.deltaTime * DecayPerTick);
                dirty = true;
            }
            if (dirty)
            {
                _renderer.material.SetVectorArray("_Data", _data);
            }
            else
            {
                _dataDirty = false;
                yield return null;
            }
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }
}