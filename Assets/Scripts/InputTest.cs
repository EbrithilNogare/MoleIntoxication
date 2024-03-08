using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InputTest : MonoBehaviour
{
    Vector2 speed;

    void Update()
    {
        this.transform.position += new Vector3(speed.x, speed.y) * Time.deltaTime;
    }

    public void OnMovement(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
        speed = context.ReadValue<Vector2>();
	}

    public void OnAction(UnityEngine.InputSystem.InputAction.CallbackContext context)
	{
        if (!context.performed)
            return;

        Color targetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
            spriteRenderer.DOColor(targetColor, 0.3f);

        if (TryGetComponent(out MeshRenderer meshRenderer))
            meshRenderer.material.DOColor(targetColor, 0.3f);
    }
}
