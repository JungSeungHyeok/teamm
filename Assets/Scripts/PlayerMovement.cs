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

        //if (stoneController.isSpaceUpDown) // 겟키 다운 상태일때 리턴 기본값 펄스
        //{
        //    return;
        //}

        float moveHorizontal = 0;

        // 키보드 입력 (디버깅용)
        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1;
        }

        // 터치 입력
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                // 터치가 UI 위에 있으면 아무 것도 하지 않는다.
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

        if (stoneController != null)  // StoneController에게 플레이어의 새 위치 알림
        {
            stoneController.UpdatePreviewStonePosition(transform.position);
        }
    }
}
