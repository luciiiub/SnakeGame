using UnityEngine;

public class SnakeConnector : MonoBehaviour
{
    public Transform segment1;
    public Transform segment2;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = -1; // Detras circulos
        }
    }

    void Update()
    {
        if (segment1 != null && segment2 != null)
        {
            Vector3 midPoint = (segment1.position + segment2.position) / 2f;   // Posicionar en el punto medio entre los dos segmentos
            transform.position = midPoint;

            Vector3 direction = segment2.position - segment1.position;  // Calcula la direccion y angulo entre los segmentos
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}