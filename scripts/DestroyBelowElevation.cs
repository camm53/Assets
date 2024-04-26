using UnityEngine;

public class DestroyBelowElevation : MonoBehaviour
{
    public float gravity = 9.8f; // Adjust gravity per object as needed

    private float destroyElevation;
    private RockSpawner rockSpawner; // Reference to the RockSpawner for repooling

    void Start()
    {
        // Set destroyElevation to the minimum y-coordinate of the parent object's collider bounds
        if (transform.parent != null && transform.parent.GetComponent<Collider>() != null)
        {
            destroyElevation = transform.parent.GetComponent<Collider>().bounds.min.y;
        }
        else
        {
           // Debug.LogError("Parent object does not have a Collider component.");
        }

        // Find the RockSpawner
        rockSpawner = FindObjectOfType<RockSpawner>();

        if (rockSpawner == null)
        {
            Debug.LogWarning("RockSpawner reference not found.");
        }
    }

    void Update()
    {
        ApplyGravity();
    }

    void ApplyGravity()
    {
        float newY = Mathf.Max(transform.position.y - gravity * Time.deltaTime, destroyElevation);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (transform.position.y <= destroyElevation)
        {
            if (rockSpawner != null && rockSpawner.IsObjectPooled(gameObject))
            {
                // Repool the object instead of destroying it
                rockSpawner.RepoolObject(gameObject);
            }
            else
            {

                gameObject.SetActive(false); // Deactivate the object instead of destroying it
            }
        }
    }
}