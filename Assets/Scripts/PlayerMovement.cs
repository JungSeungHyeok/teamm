using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 95f;
    public StoneControler stoneController;

    public void Update()
    {
        if (stoneController.currentState == StoneControler.ThrowState.Charging)
        {
            return;
        }

        //if (stoneController.isSpaceUpDown) // ��Ű �ٿ� �����϶� ���� �⺻�� �޽�
        //{
        //    return;
        //}

        float moveHorizontal = 0;

        // Ű���� �Է� (������)
        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1;
        }

        // ��ġ �Է�
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                // ��ġ�� UI ���� ������ �ƹ� �͵� ���� �ʴ´�.
                return;
            }

            if (touch.position.x < Screen.width / 2)
            {
                moveHorizontal = -1;
            }
            else if (touch.position.x >= Screen.width / 2)
            {
                moveHorizontal = 1;
            }
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        transform.position += movement * speed * Time.deltaTime;

        if (stoneController != null)  // StoneController���� �÷��̾��� �� ��ġ �˸�
        {
            stoneController.UpdatePreviewStonePosition(transform.position);
        }
    }
}
