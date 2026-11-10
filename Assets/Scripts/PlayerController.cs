
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float score = 0f;
    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;
    public float maxSpeed = 5f;
    Rigidbody2D rb;
    public UIDocument uiDocument;
    private Label scoreText;
    private Button restartButton;
    public GameObject explosionEffect;
    public GameObject boosterFlame;
    public InputAction moveForward;
    public InputAction lookPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
        moveForward.Enable();
        lookPosition.Enable();
    }

    void Update()
    {
        scoreText.text = "Score: " + score;
        MovePlayer();

        if (moveForward.WasPressedThisFrame())
        {
            boosterFlame.SetActive(true);
        }
        else if (moveForward.WasReleasedThisFrame())
        {
            boosterFlame.SetActive(false);
        }
    }
    void MovePlayer()
    {
        if (moveForward.IsPressed())
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(lookPosition.ReadValue<Vector2>());
            Debug.Log("Mouse position: " + mousePos);
            scoreText.text = "Score: " + score;

            // Calculate mouse direction
            Vector2 direction = (mousePos - transform.position).normalized;

            // Move player in direction of mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}