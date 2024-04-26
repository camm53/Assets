using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class Controladores : MonoBehaviour
{
    //  Componentes del personaje
    public Animator animator; // Animador del personaje
    public CharacterController characterController; // Controlador de movimiento del personaje
    public Transform cam; // Cámara del juego

    // Parámetros de movimiento
    public float speed = 6f; // Velocidad de movimiento del personaje
    public float turnSmoothTime = 0.1f; // Tiempo de suavizado del giro
    private float turnSmoothVelocity; // Velocidad de suavizado del giro actual
    public float jumpHeight = 7.0f; // Altura del salto
    public float gravity = 9.8f; // Gravedad
    private Vector3 playerVelocity; // Velocidad del personaje


    // Puntos de reaparición
    public Transform spawnPoint1; // Punto de reaparición inicial
    public AudioClip musiquita;
    private Vector3 initialPosition; // Posición inicial del personaje

    // Bandera para controlar la dirección del personaje
    private bool isFacingRight = true; // Indica si el personaje mira a la derecha
    

    void Start()
    {
        AudioManager.instance.PlayBackgroundMusic(musiquita);
        // Al iniciar, la posición inicial se guarda como punto de reaparición
        initialPosition = transform.position;
        
        transform.position = spawnPoint1.position;
    }

    void Update()
    {
        // Detección de suelo con un rayo (raycast)
        bool isGrounded = CheckGround(); // Comprobar si el personaje está en el suelo

        // Reiniciar la velocidad vertical cuando toca el suelo
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f; // Restablecer la velocidad vertical al tocar el suelo
            animator.SetFloat("salto", 0.0f); // Reiniciar parámetro salto al tocar el suelo
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity); // Calcular la velocidad vertical del salto
            animator.SetFloat("salto", 1.0f); // Activar salto en el Animator
        }

        float horizontal = Input.GetAxisRaw("Horizontal"); // Entrada horizontal (izquierda/derecha)
        float vertical = Input.GetAxisRaw("Vertical"); // Entrada vertical (arriba/abajo)

        // Calcular la magnitud del vector de movimiento
        float checkSpeed = new Vector2(horizontal, vertical).magnitude; // Calcular la velocidad de movimiento actual

        // Establecer el parámetro de velocidad en el Animator
        animator.SetFloat("velocidad", checkSpeed); // Indicar la velocidad de movimiento al Animator

        // Normalizar el vector de dirección
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized; // Normalizar el vector de dirección

        if (direction.magnitude >= 0.1f)
        {
            // Código de giro suave (comentado)
            // float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            // float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // Calcular el ángulo objetivo para el giro

            // Rotar el personaje según la dirección
            if (horizontal > 0 && !isFacingRight) // Girar a la derecha después de mirar a la izquierda
            {
                transform.Rotate(0f, 180f, 0f); // Voltear el personaje
                isFacingRight = true; // Indicar que el personaje mira a la derecha
            }
            else if (horizontal < 0 && isFacingRight) // Girar a la izquierda después de mirar a la derecha
            {
                transform.Rotate(0f, 180f, 0f); // Voltear el personaje
                isFacingRight = false; // Indicar que el personaje mira a la izquierda
            }

            // Mover el personaje en la dirección deseada
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // Calcular la dirección de movimiento

            characterController.Move(moveDir.normalized * speed * Time.deltaTime); // Mover el personaje
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Activar animación de ataque
            Debug.Log("¡Disparador de ataque enviado!");
            animator.SetTrigger("attackk");
            // Implementar lógica de ataque aquí (por ejemplo, infligir daño)
        }

        // Aplicar la gravedad continuamente
        playerVelocity.y -= gravity * Time.deltaTime; // Aplicar la gravedad
        characterController.Move(playerVelocity * Time.deltaTime); // Mover el personaje basado en la velocidad
    }

    // Función para revisar si el personaje está en el suelo con un rayo (raycast)
   bool CheckGround()
{
    RaycastHit hit;
    Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // Origin of the ray
    float radius = 0.5f; // Radius of the semi-sphere

    // Cast a semi-sphere downwards
    if (Physics.SphereCast(rayOrigin, radius, Vector3.down, out hit, characterController.height / 2 + 1.0f))
    {
        // Check if the collider hit is not a trigger
        if (!hit.collider.isTrigger)
        {
            return true; // The character is on the ground
        }
    }

    return false; // The character is not on the ground
}


    void OnTriggerEnter(Collider other)
    {
        // Comprobar si el personaje ha colisionado con un obstáculo
        if (other.CompareTag("Obstacle"))
        {
            // Reproducir sonido de impacto
            AudioManager.instance.PlaySoundEffect(1); 
            Respawn(); // Regresar al punto de reaparición
        }
    }

    IEnumerator CheckPlayerPositionAfterDelay(int frameCount)
{
    for (int i = 0; i < frameCount; i++)
    {
        yield return null;
    }

    Debug.Log("Player's position after " + frameCount + " frames: " + transform.position);
}

void Respawn()
{
    // Calculate the direction and distance to the spawn point
    Vector3 direction = spawnPoint1.position - transform.position;

    // Move the character to the spawn point
    characterController.Move(direction);
}
}