using UnityEngine;

public class Food : MonoBehaviour
{
    public BoxCollider2D gridArea;

    private void Start()
    {
        RandomizePosition();
    }

    public void RandomizePosition()
    {
        Bounds bounds = gridArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        // Redondear a posiciones de la cuadrícula!!
        transform.position = new Vector3(
            Mathf.Round(x),
            Mathf.Round(y),
            0f
        );
    }
}