using UnityEngine;

public class GenerateMeshBetweenPoints : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    [SerializeField]
    private GameObject startPointObject;

    [SerializeField]
    private GameObject endPointObject;

    void Update()
    {
        // Получение глобальных координат объектов
        Vector3 startPoint = startPointObject.transform.position;
        Vector3 endPoint = endPointObject.transform.position;

        // Установка позиций линии
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
}
