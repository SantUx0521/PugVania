using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private Vector2 movementMultiplier = Vector2.one;
    private Vector3 lastCamPos;
    private Material material;
    private Transform camTransform;

    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        camTransform = Camera.main.transform;
        lastCamPos = camTransform.position;
    }

    private void Update()
    {
        Vector3 deltaMovement = camTransform.position - lastCamPos;
        material.mainTextureOffset += new Vector2(deltaMovement.x * movementMultiplier.x, deltaMovement.y * movementMultiplier.y);
        lastCamPos = camTransform.position;
    }
}
