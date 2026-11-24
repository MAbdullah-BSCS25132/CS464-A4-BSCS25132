using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class ListenerOnPlayer : MonoBehaviour
{
    private void LateUpdate()
    {
        PlayerShip ship = PlayerShip.Instance;

        if (ship != null)
        {
            // Kaan ship par: sunai wohi dene wali awaaz jo ship ke paas ho
            transform.position = WorldPos.FromUi(ship.ShipRect);
            return;
        }

        // Menus mein (ship nahi) camera ke saath
        Camera cam = Camera.main;
        if (cam != null)
            transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0f);
    }
}