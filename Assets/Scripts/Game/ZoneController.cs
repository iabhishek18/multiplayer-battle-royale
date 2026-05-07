using UnityEngine;
using Photon.Pun;

public class ZoneController : MonoBehaviourPun
{
    public static ZoneController Instance { get; private set; }

    [SerializeField] private float initialRadius = 500f;
    [SerializeField] private float finalRadius = 10f;
    [SerializeField] private float shrinkDuration = 60f;
    [SerializeField] private int damagePerSecond = 5;
    [SerializeField] private int phases = 6;

    private float currentRadius;
    private Vector3 currentCenter;
    private Vector3 targetCenter;
    private float targetRadius;
    private int currentPhase;
    private bool isShrinking;

    private void Awake() { Instance = this; }

    private void Start()
    {
        currentRadius = initialRadius;
        currentCenter = Vector3.zero;
        CalculateNextZone();
    }

    public void ShrinkZone()
    {
        if (currentPhase >= phases || isShrinking) return;
        isShrinking = true;
    }

    private void Update()
    {
        if (!isShrinking) return;

        currentRadius = Mathf.MoveTowards(currentRadius, targetRadius, (currentRadius - targetRadius) / shrinkDuration * Time.deltaTime);
        currentCenter = Vector3.MoveTowards(currentCenter, targetCenter, Vector3.Distance(currentCenter, targetCenter) / shrinkDuration * Time.deltaTime);

        if (Mathf.Approximately(currentRadius, targetRadius))
        {
            isShrinking = false;
            currentPhase++;
            CalculateNextZone();
        }
    }

    private void CalculateNextZone()
    {
        float radiusReduction = (initialRadius - finalRadius) / phases;
        targetRadius = currentRadius - radiusReduction;
        float maxOffset = currentRadius * 0.3f;
        targetCenter = currentCenter + new Vector3(Random.Range(-maxOffset, maxOffset), 0, Random.Range(-maxOffset, maxOffset));
    }

    public bool IsInsideZone(Vector3 position)
    {
        float distance = Vector3.Distance(new Vector3(position.x, 0, position.z), new Vector3(currentCenter.x, 0, currentCenter.z));
        return distance <= currentRadius;
    }

    public int GetDamage() => damagePerSecond * currentPhase;
}
