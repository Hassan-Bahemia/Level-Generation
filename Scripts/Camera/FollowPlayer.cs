using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Range(0,1)]public float Speed = .1f;
    public Transform Player;

    private void Update()
    {
        var ppX = Mathf.Round(Player.position.x * 100) / 100;
        var ppY = Mathf.Round(Player.position.y * 100) / 100;
        var pixelPerfectPosition = new Vector3(ppX, ppY, -10);

        print(ppX);

        transform.position = Vector3.Lerp(transform.position, pixelPerfectPosition, Speed);
    }
}
