using UnityEngine;

public class SplineMovement : MonoBehaviour
{
    public Transform[] knots;
    public float speed = 1.0f;
    [Range(0.1f, 5f)] public float rotationSmoothness = 2f;

    private int currentKnotIndex = 0;
    private float t = 0.0f;
    private bool isMoving = false; 
    private int targetKnotIndex;

    void Update()
    {
        HandleInput();
        MoveAlongSpline();
    }

    void HandleInput()
    {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.A) && currentKnotIndex < knots.Length - 1) MoveToNextKnot();
            else if (Input.GetKeyDown(KeyCode.D) && currentKnotIndex > 0) MoveToPreviousKnot();
        }
    }

    public void MoveToNextKnot()
    {
        targetKnotIndex = currentKnotIndex + 1;
        isMoving = true;
        t = 0.0f;
    }

    public void MoveToPreviousKnot()
    {
        targetKnotIndex = currentKnotIndex - 1;
        isMoving = true;
        t = 1.0f;
    }

    void MoveAlongSpline()
    {
        if (isMoving)
        {
            // Обновляем параметр t
            if (targetKnotIndex > currentKnotIndex)
                t += Time.deltaTime * speed; // Движение вперед
            else
                t -= Time.deltaTime * speed; // Движение назад

            // Завершение движения
            if ((t >= 1.0f && targetKnotIndex > currentKnotIndex) || (t <= 0.0f && targetKnotIndex < currentKnotIndex))
            {
                t = Mathf.Clamp(t, 0.0f, 1.0f);
                currentKnotIndex = targetKnotIndex;
                isMoving = false;
            }

            int p0, p1, p2, p3;
            GetSplinePoints(out p0, out p1, out p2, out p3);

            Vector3 newPos = GetCatmullRomPosition(t, knots[p0].position, knots[p1].position, knots[p2].position, knots[p3].position);
            transform.position = newPos;

            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            knots[targetKnotIndex].rotation,
            Time.deltaTime * rotationSmoothness
        );
        }
    }

    void GetSplinePoints(out int p0, out int p1, out int p2, out int p3)
    {
        p1 = Mathf.Min(currentKnotIndex, targetKnotIndex);
        p2 = Mathf.Max(currentKnotIndex, targetKnotIndex);

        p0 = (p1 - 1 + knots.Length) % knots.Length;
        p3 = (p2 + 1) % knots.Length;
    }

    Vector3 GetCatmullRomPosition(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // Формула Catmull-Rom сплайна
        float t2 = t * t;
        float t3 = t2 * t;
        return 0.5f * (
            (2 * p1) +
            (-p0 + p2) * t +
            (2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 +
            (-p0 + 3 * p1 - 3 * p2 + p3) * t3
        );
    }

    void OnDrawGizmos()
    {
        if (knots == null || knots.Length < 2) return;

        Gizmos.color = Color.white;
        for (int i = 0; i < knots.Length; i++)
        {
            int p0 = Mathf.Max(i - 1, 0);
            int p1 = i;
            int p2 = i + 1;
            int p3 = Mathf.Min(i + 2, knots.Length - 1);

            Vector3 prevPos = knots[p1].position;
            for (int j = 1; j <= 20; j++)
            {
                float t = j / 20.0f;
                Vector3 newPos = GetCatmullRomPosition(t, knots[p0].position, knots[p1].position, knots[p2].position, knots[p3].position);
                Gizmos.DrawLine(prevPos, newPos);
                prevPos = newPos;
            }
        }
    }
}