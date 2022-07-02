using UnityEngine;

public class scr_CameraSetup : MonoBehaviour
{
    //Reference Global Manager Instance
    [SerializeField] private scr_GameManager ManagerInstance = scr_GameManager.instance;
    [SerializeField] private Camera MainCam;

    private bool HeightChanged = false;

    private void Awake()
    {
        MainCam = GetComponent<Camera>();
        if (ManagerInstance == null)
        {
            ManagerInstance = FindObjectOfType<scr_GameManager>();
        }
        if (ManagerInstance != null)
        {
            MainCam.orthographic = true;
            if (ManagerInstance.m_BoardHeight == 10 && ManagerInstance.m_BoardWidth <= 5)
            {
                MainCam.transform.position = new Vector3(0, 0, -10);
                MainCam.orthographicSize = 7;
                HeightChanged = true;
            }
            else if (ManagerInstance.m_BoardWidth % 2 == 0)
            {
                MainCam.transform.position = new Vector3(-0.5f, 0, -10);
            }
            if (!HeightChanged)
            {
                MainCam.orthographicSize = ManagerInstance.m_BoardWidth + (ManagerInstance.m_BoardHeight > 7 ? 1 : 0);
            }
        }
    }
}
