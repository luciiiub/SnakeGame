using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    private Vector2 inputDirection = Vector2.right;
    private List<Transform> segments;
    private List<GameObject> connectors;
    private Rigidbody2D rb;
    private GameOverManager gameOverManager;
    private int score = 0;

    public Transform segmentPrefab;
    public GameObject connectorPrefab;
    public Food foodScript;

    [SerializeField] private float moveRate = 0.5f;
    [SerializeField] private float stepSize = 1f;

    private float timer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        gameOverManager = FindObjectOfType<GameOverManager>();
    }

    private void Start()
    {
        segments = new List<Transform>();
        segments.Add(transform);
        connectors = new List<GameObject>();
    }

    private void Update()
    {
        HandleInput();
        timer += Time.deltaTime;
        if (timer >= moveRate)
        {
            direction = inputDirection;
            Move();
            timer = 0f;
        }
    }

    private void HandleInput()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;
        // WASD + Flechas
        if ((keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame) && direction != Vector2.down)
            inputDirection = Vector2.up;
        else if ((keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame) && direction != Vector2.up)
            inputDirection = Vector2.down;
        else if ((keyboard.aKey.wasPressedThisFrame || keyboard.leftArrowKey.wasPressedThisFrame) && direction != Vector2.right)
            inputDirection = Vector2.left;
        else if ((keyboard.dKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame) && direction != Vector2.left)
            inputDirection = Vector2.right;
    }

    private void Move()
    {
        Vector3 previousHeadPosition = transform.position;
        Vector2 newPosition = (Vector2)transform.position + direction * stepSize;

        CheckFoodCollision(newPosition);

        rb.MovePosition(newPosition);
        transform.position = newPosition;

        for (int i = segments.Count - 1; i > 0; i--)
        {
            Vector3 temp = segments[i].position;
            segments[i].position = segments[i - 1].position;
            if (i == 1)
            {
                segments[i].position = previousHeadPosition;
            }
        }
    }

    private void CheckFoodCollision(Vector2 nextPosition)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(nextPosition, 0.5f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Food"))
            {
                Grow();
                score++;
                if (gameOverManager != null)
                {
                    gameOverManager.UpdateScore(score);
                }

                Food food = hit.GetComponent<Food>();
                if (food != null)
                {
                    food.RandomizePosition();
                }
                break;
            }
        }
    }

    private void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        if (segments.Count > 0)
        {
            segment.position = segments[segments.Count - 1].position - (Vector3)(direction * stepSize);
        }
        else
        {
            segment.position = transform.position;
        }
        segments.Add(segment);

        //Crea conector entre el nuevo segmento y el anterior!!
        if (segments.Count > 1 && connectorPrefab != null)
        {
            GameObject connector = Instantiate(connectorPrefab);
            SnakeConnector connectorScript = connector.AddComponent<SnakeConnector>();
            connectorScript.segment1 = segments[segments.Count - 2];
            connectorScript.segment2 = segments[segments.Count - 1];
            connectors.Add(connector);
        }
    }

    public void ResetState()
    {
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        //Destruir conectores
        foreach (GameObject connector in connectors)
        {
            Destroy(connector);
        }
        connectors.Clear();

        segments.Clear();
        segments.Add(this.transform);
        this.transform.position = Vector3.zero;
        direction = Vector2.right;
        inputDirection = Vector2.right;
        score = 0;
    }

    public void HideSegments()
    {
        foreach (Transform segment in segments)
        {
            if (segment != null)
            {
                segment.gameObject.SetActive(false);
            }
        }
        // Oculta conectores
        foreach (GameObject connector in connectors)
        {
            if (connector != null)
            {
                connector.SetActive(false);
            }
        }
    }

    public void ShowSegments()
    {
        foreach (Transform segment in segments)
        {
            if (segment != null)
            {
                segment.gameObject.SetActive(true);
            }
        }
        // Mostrar conectores
        foreach (GameObject connector in connectors)
        {
            if (connector != null)
            {
                connector.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Food"))
        {
            Grow();
            score++;
            if (gameOverManager != null)
            {
                gameOverManager.UpdateScore(score);
            }

            Food food = other.GetComponent<Food>();
            if (food != null)
            {
                food.RandomizePosition();
            }
            else if (foodScript != null)
            {
                foodScript.RandomizePosition();
            }
        }
        else if (other.CompareTag("Obstacle") || other.CompareTag("Wall"))
        {
            Debug.Log("Chocaste con obstáculo! Game Over!");
            if (gameOverManager != null)
            {
                gameOverManager.ShowGameOver();
            }
        }
        else if (other.CompareTag("Segment"))
        {
            if (segments.Count > 2)
            {
                Debug.Log("Te chocaste contigo mismo! Game Over!");
                if (gameOverManager != null)
                {
                    gameOverManager.ShowGameOver();
                }
            }
        }
    }
}