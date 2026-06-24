using System.Collections;
using UnityEngine;

public class StackController : MonoBehaviour
{
    [SerializeField] private StackPartController[] stackParts = null;

    public void ShatterAllParts()
    {
        if (transform.parent != null)
        {
            transform.parent = null;
            FindFirstObjectByType<Ball>().IncreaseBrokenStacks();
        }

        foreach (StackPartController part in stackParts)
        {
            part.Shatter();
        }

        StartCoroutine(RemoveParts());
    }

    private IEnumerator RemoveParts()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
        
}