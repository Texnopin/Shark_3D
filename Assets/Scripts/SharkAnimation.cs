using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAnimation : MonoBehaviour
{
    [Header("��������� ����������")]
    [SerializeField] private Transform centerPoint;   // ����� ����������
    [SerializeField] private float radius = 5f;       // ������
    [SerializeField] private float speed = 30f;       // �������� (�������/���)
    [SerializeField] private bool clockwise = true;   // ����������� ��������

    [Header("��������� ��� ��������")]
    [SerializeField] private Axis orbitAxis = Axis.Y; // ��� �������� ������
    [SerializeField] private Axis facingAxis = Axis.Z; // ���, ������� ������ "��������" �����
    [SerializeField] private bool lookAtCenter = false; // �������: �������� � ����� ��� � ������� ��������

    private float currentAngle;

    public enum Axis { X, Y, Z }

    void Update()
    {
        UpdatePosition();
        UpdateFacing();
    }

    void UpdatePosition()
    {
        // ��������� ����
        currentAngle += (clockwise ? -speed : speed) * Time.deltaTime;
        currentAngle %= 360;

        float angleRad = currentAngle * Mathf.Deg2Rad;

        Vector3 offset = Vector3.zero;
        switch (orbitAxis)
        {
            case Axis.X:
                offset = new Vector3(0, Mathf.Sin(angleRad), Mathf.Cos(angleRad)) * radius;
                break;
            case Axis.Y:
                offset = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * radius;
                break;
            case Axis.Z:
                offset = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad), 0) * radius;
                break;
        }

        transform.position = centerPoint.position + offset;
    }

    void UpdateFacing()
    {
        // ������������ ����������� � ���������� (����������� ��������)
        float deltaAngle = (clockwise ? -1 : 1) * Mathf.Deg2Rad * speed * Time.deltaTime;
        float nextAngleRad = currentAngle * Mathf.Deg2Rad + deltaAngle;

        Vector3 tangentDir = Vector3.zero;
        switch (orbitAxis)
        {
            case Axis.X:
                tangentDir = new Vector3(0, Mathf.Cos(nextAngleRad), -Mathf.Sin(nextAngleRad));
                break;
            case Axis.Y:
                tangentDir = new Vector3(-Mathf.Sin(nextAngleRad), 0, Mathf.Cos(nextAngleRad));
                break;
            case Axis.Z:
                tangentDir = new Vector3(-Mathf.Sin(nextAngleRad), Mathf.Cos(nextAngleRad), 0);
                break;
        }

        // �������, ���� ����� �������� � ������� ��������
        Vector3 direction = lookAtCenter ? (centerPoint.position - transform.position).normalized : tangentDir.normalized;

        // ������� ������� ���, ����� ��� facingAxis ��������� � direction
        Quaternion targetRotation = GetRotationAlongAxis(direction);
        transform.rotation = targetRotation;
    }

    Quaternion GetRotationAlongAxis(Vector3 direction)
    {
        // ���������� ��������� ���, ������� ������ "��������" �����
        Vector3 forwardAxis;
        switch (facingAxis)
        {
            case Axis.X:
                forwardAxis = transform.right;
                break;
            case Axis.Y:
                forwardAxis = transform.up;
                break;
            case Axis.Z:
                forwardAxis = transform.forward;
                break;
            default:
                forwardAxis = transform.forward;
                break;
        }

        // ������ ��������, ����� forwardAxis ��������� � direction
        return Quaternion.LookRotation(direction, GetUpVector());
    }

    Vector3 GetUpVector()
    {
        // ����� ������ ���� ��� "�����" ��� �������� �����������
        // ��������, ��� Y-���:
        return Vector3.up;
    }

    void OnDrawGizmosSelected()
    {
        if (centerPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(centerPoint.position, radius);
        }
    }
}
