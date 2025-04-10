using UnityEngine;

public class SplineMovement : MonoBehaviour
{
    public Transform[] knots;
    public float speed = 1.0f;
    [Range(0.1f, 5f)] public float rotationSmoothness = 2f;

    public int currentKnotIndex = 0;
    private float t = 0.0f;
    public bool isMoving = false; 
    private bool automaticMode = false;
    private bool isForwardDirection = true;
    private int targetKnotIndex;

    void Update()
    {
        HandleAutomaticMovement();
        MoveAlongSpline();
    }


    public bool MoveToNextKnot()
    {
        if (currentKnotIndex >= knots.Length - 1 || isMoving) return false; // Для проигровяния звуков
        targetKnotIndex = currentKnotIndex + 1;
        isMoving = true;
        t = 0.0f;
        return true;
    }

    public bool MoveToPreviousKnot()
    {
        if (currentKnotIndex <= 0 || isMoving) return false;
        targetKnotIndex = currentKnotIndex - 1;
        isMoving = true;
        t = 1.0f;
        return true;
    }

    void HandleAutomaticMovement()
    {
        if (automaticMode && !isMoving)
        {
            if (isForwardDirection)
            {
                if (currentKnotIndex < knots.Length - 1)
                    MoveToNextKnot();
            }
            else
            {
                if (currentKnotIndex > 0)
                    MoveToPreviousKnot();
            }
        }
    }
    public void StartAutomaticMovement(bool forward)
    {
        automaticMode = true;
        isForwardDirection = forward;
        currentKnotIndex = forward ? 0 : knots.Length - 1;
        targetKnotIndex = forward ? 1 : knots.Length - 2;
        t = forward ? 0f : 1f;
        MoveAlongSpline();
    }

    public bool IsStoppedInEnd()
    {
        Debug.Log("stopped");
        Debug.Log(isMoving);
        Debug.Log(currentKnotIndex);
        return (!isMoving) && currentKnotIndex == knots.Length - 1;
    }
    public bool IsStoppedOnStart()
    {
        return !isMoving && currentKnotIndex == 0;
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

            if (automaticMode == false)
            {
                transform.rotation = Quaternion.Slerp(
                transform.rotation,
                knots[targetKnotIndex].rotation,
                Time.deltaTime * rotationSmoothness
                );
            } else
            {
                transform.rotation = transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation( knots[targetKnotIndex].position - transform.position),
                Time.deltaTime * rotationSmoothness
                );
            }
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
        for (int i = 0; i < knots.Length - 1; i++)
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