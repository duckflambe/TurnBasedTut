#define USE_INPUT_SYSTEM

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

	public static InputManager Instance { get; private set; }
	private InputActions inputActions;
	private void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("There should only be one InputManager in the scene");
			Destroy(gameObject);
			return;
		}
		Instance = this;

		inputActions = new InputActions();
		inputActions.Player.Enable();
	}

	public Vector2 GetMouseScreenPosition()
	{
#if USE_INPUT_SYSTEM
		return Mouse.current.position.ReadValue();
#else
		return Input.mousePosition;
#endif
		//return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	public bool IsMouseLeftClicked()
	{
#if USE_INPUT_SYSTEM
		return inputActions.Player.Click.WasPressedThisFrame();
#else
		return Input.GetMouseButtonDown(0);
#endif

	}

	public Vector2 GetCameraMoveVector()
	{
#if USE_INPUT_SYSTEM
		return inputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
		return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
#endif
	}

	public float GetCameraRotate()
	{
#if USE_INPUT_SYSTEM
		return inputActions.Player.CameraRotate.ReadValue<float>();
#else
		if (Input.GetKey(KeyCode.Q))
		{
			return 1f;
		}
		if (Input.GetKey(KeyCode.E))
		{
			return -1f;
		}
		return 0f;
#endif

	}

	public float GetCameraZoom()
	{
#if USE_INPUT_SYSTEM
		return inputActions.Player.CameraZoom.ReadValue<float>();
#else
		return Input.GetAxis("Mouse ScrollWheel");
#endif
	}

}
