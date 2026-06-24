using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelSpawner : MonoBehaviour
{
    public GameObject[] model;
    [HideInInspector]
    public GameObject[] modelPrefab = new GameObject[4];
    public GameObject winPrefab;

    private GameObject _temp1, _temp2;
    public int level = 1, addOn = 7;
    private float _i = 0;

    private void Start()
    {
        if (level > 9)
        {
            addOn = 0;
        }

        ModelSelection();
        float random = Random.value;
        for (_i = 0; _i > -level - addOn; _i -= 0.5f)
        {
            if (level <= 20) _temp1 = Instantiate(modelPrefab[Random.Range(0, 2)]);
            if (level > 20 && level <= 50) _temp1 = Instantiate(modelPrefab[Random.Range(1, 3)]);
            if (level > 50 && level <= 100) _temp1 = Instantiate(modelPrefab[Random.Range(2, 4)]);
            if (level > 100) _temp1 = Instantiate(modelPrefab[Random.Range(3, 4)]);
            _temp1.transform.position = new Vector3(0, _i - 0.01f, 0);
            _temp1.transform.eulerAngles = new Vector3(0, _i * 8, 0);

            if (Mathf.Abs(_i) >= level * 0.3f && Mathf.Abs(_i) <= level * 0.6f)
            {
                _temp1.transform.eulerAngles = new Vector3(0, _i * 8, 0);
                _temp1.transform.eulerAngles += new Vector3(0, 180, 0);
            }
            else if (Mathf.Abs(_i) >= level * 0.8f)
            {
                _temp1.transform.eulerAngles = new Vector3(0, _i * 8, 0);
                if (random > 0.75f)
                {
                    _temp1.transform.eulerAngles += new Vector3(0, 180, 0);
                }
            }
            
            _temp1.transform.parent = FindFirstObjectByType<Rotator>().transform;
        }
        
        _temp2 = Instantiate(winPrefab);
        _temp2.transform.position = new Vector3(0, _i - 0.01f, 0);
    }

    private void ModelSelection()
    {
        int randomModel = Random.Range(0, 5);

        switch (randomModel)
        {
            case 0:
                for (int i = 0; i < 4; i++) modelPrefab[i] = model[i];
                break;
            case 1:
                for (int i = 0; i < 4; i++) modelPrefab[i] = model[i + 4];
                break;
            case 2:
                for (int i = 0; i < 4; i++) modelPrefab[i] = model[i + 8];
                break;
            case 3:
                for (int i = 0; i < 4; i++) modelPrefab[i] = model[i + 12];
                break;
            case 4:
                for (int i = 0; i < 4; i++) modelPrefab[i] = model[i + 16];
                break;
            default:    
                break;
        }
    }
}