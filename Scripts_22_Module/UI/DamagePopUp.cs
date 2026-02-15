using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    public float lifeTime = 2f;
    public float floatSpeed = 1f;

    private TextMeshPro text;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void Setup(int damage)
    {
        text.text = damage.ToString();
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }
}
