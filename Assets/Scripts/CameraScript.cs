using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;

    public float screenHeight;

    public int levelCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       screenHeight = Camera.main.orthographicSize * 2;
        levelCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > transform.position.y + screenHeight / 2)
        {
            Camera.main.transform.position += new Vector3(0, screenHeight, 0);
            levelCount += 1;
        }

        if (player.transform.position.y < transform.position.y - screenHeight / 2)
        {
            Camera.main.transform.position -= new Vector3(0, screenHeight, 0);
            levelCount -= 1;
        }
    }
}
