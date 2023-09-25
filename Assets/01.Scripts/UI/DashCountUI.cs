using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DashCountUI : MonoBehaviour
{
    [SerializeField]
    private int _dashCount;

    private Image _image;
    
    private PlayerDash _playerDash;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _playerDash = GameManager.Instance.Player.transform.GetComponentCache<PlayerDash>();

        _playerDash.OnDash += OnDash;
    }
    private void OnDash(int dashCount)
    {
        Color color = _image.color;
        color.a = dashCount < _dashCount ? 0f : 1f;
        
        _image.color = color;
    }
}
