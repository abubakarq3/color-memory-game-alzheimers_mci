using UnityEngine;

public class Brush : MonoBehaviour
{
    public delegate void BrushEventHandler(Color color);
    public static event BrushEventHandler OnBrushColored;  // Evento para cambio de color del Brush
    public static event BrushEventHandler OnObjectPainted; // Evento para cuando un objeto es pintado

    public string planeTag = "ColorPlane"; // Tag de los planos
    public string brushTargetName = "brush.002"; // Nombre específico del objeto a pintar

    private Renderer brushRenderer;

    void Start()
    {
        // Busca el Renderer solo en el objeto hijo que tiene el nombre Brush.001
        Transform targetChild = FindChildByName(gameObject, brushTargetName);
        if (targetChild != null)
        {
            brushRenderer = targetChild.GetComponent<Renderer>();
        }

        if (brushRenderer == null)
        {
            Debug.LogError($"No se encontró el Renderer en el objeto {brushTargetName}. Verifica la configuración.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log($"Collision detected with object: {collision.gameObject.name}, tag: {collision.gameObject.tag}");

        // Verifica si el objeto con el que colisiona es un plano
        if (collision.gameObject.CompareTag(planeTag))
        {
            ApplyPlaneColor(collision.gameObject);
        }
        else if (!collision.gameObject.CompareTag("Plane"))
        {
            PaintObject(collision.gameObject);
        }
    }

    void ApplyPlaneColor(GameObject plane)
    {
        Renderer planeRenderer = plane.GetComponent<Renderer>();
        if (planeRenderer != null && brushRenderer != null)
        {
            brushRenderer.material.color = planeRenderer.material.color;

            // Dispara el evento OnBrushColored
            OnBrushColored?.Invoke(brushRenderer.material.color);
            Debug.Log($"Brush color changed to {brushRenderer.material.color}");
        }
    }

    void PaintObject(GameObject other)
    {
        Renderer otherRenderer = other.GetComponent<Renderer>();
        if (otherRenderer != null && brushRenderer != null)
        {
            otherRenderer.material.color = brushRenderer.material.color;

            // Dispara el evento OnObjectPainted
            OnObjectPainted?.Invoke(brushRenderer.material.color);
            Debug.Log($"Object painted with color {brushRenderer.material.color}");
        }
    }

    // Método para buscar un objeto hijo por su nombre
    Transform FindChildByName(GameObject parent, string targetName)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == targetName)
            {
                return child;
            }
        }
        return null;
    }
}