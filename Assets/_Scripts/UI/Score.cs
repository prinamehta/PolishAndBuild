using UnityEngine;

public class Score : MonoBehaviour
{

    public int value;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            FindObjectOfType<GameManager>().AddBrick(value);
            Destroy(gameObject);
        }
    }
}
