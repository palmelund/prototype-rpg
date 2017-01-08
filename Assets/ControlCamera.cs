using UnityEngine;

namespace Assets
{
    public class ControlCamera : MonoBehaviour
    {
        private const float CamSpeed = 10f;
        private const float Border = 2;

        // Update is called once per frame
        void Update()
        {
            if (Input.mousePosition.x < Border && Camera.main.transform.position.x >= 0)
            {
                transform.position += Vector3.left * Time.deltaTime * CamSpeed;
            }
            else if (Input.mousePosition.x > Screen.width - Border && Camera.main.transform.position.x <= map.Instance.Width)
            {
                transform.position += Vector3.right * Time.deltaTime * CamSpeed;
            }

            if (Input.mousePosition.y < Border && Camera.main.transform.position.y >= 0)
            {
                transform.position += Vector3.down * Time.deltaTime * CamSpeed;
            }
            if (Input.mousePosition.y > Screen.height - Border && Camera.main.transform.position.y <= map.Instance.Height)
            {
                transform.position += Vector3.up * Time.deltaTime * CamSpeed;
            }
        }
    }
}
