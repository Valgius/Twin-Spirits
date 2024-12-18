using UnityEngine;

public class WaterFlowInstantiate : MonoBehaviour
{
    public GameObject waterFlowPrefab;
    public Transform startPoint;
    public Transform endPoint;
    public float instantiateDelay = 1;
    private float instantiateTimer;

    public float lifetime = 1f;
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        if (instantiateTimer > 0)
        {
            instantiateTimer -= Time.deltaTime;
        }
        else
        {
            InstantiateWater();
        }
    }

    public void InstantiateWater()
    {
        GameObject waterFlow = Instantiate(waterFlowPrefab, startPoint);
        waterFlow.GetComponent<MoveWaterAsset>()?.Initialize(endPoint.position, lifetime, speed);
        instantiateTimer = instantiateDelay;
    }
}
