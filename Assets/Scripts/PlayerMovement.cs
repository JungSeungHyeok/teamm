using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;  // 움직임의 속도

    void Update()
    {
        float moveHorizontal = 0;

        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1;
        }
        
        else if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1;
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        transform.position += movement * speed * Time.deltaTime;
    }
}
