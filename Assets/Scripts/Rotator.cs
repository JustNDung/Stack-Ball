using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float speed = 100f;

    private void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
}
