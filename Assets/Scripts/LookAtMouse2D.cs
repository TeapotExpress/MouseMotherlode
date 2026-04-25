using UnityEngine;

public class LookAtMouse2D : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spr;
    void Update()
    {
        // Pozycja myszy na ekranie
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Zamiana na pozycjê w œwiecie
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // Oblicz kierunek
        Vector2 direction = mouseWorldPosition - transform.position;

        // Oblicz k¹t (z radianów na stopnie)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Obróæ obiekt
        transform.rotation = Quaternion.Euler(0, 0, angle);

      
        spr.flipY = angle < -90 || angle > 90;
   
    }
}
