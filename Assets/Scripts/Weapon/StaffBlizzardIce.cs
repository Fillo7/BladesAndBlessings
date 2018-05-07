using UnityEngine;

public class StaffBlizzardIce : MonoBehaviour
{
    private float timer = 0.0f;
    private float duration = 0.0f;
    private bool active = false;

    void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        if (!active)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= duration)
        {
            active = false;
            timer = 0.0f;
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void EnableIce(float duration)
    {
        this.duration = duration;
        GetComponent<MeshRenderer>().enabled = true;
        active = true;
    }
}
