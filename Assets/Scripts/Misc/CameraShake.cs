using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{

	public class CameraShake : MonoBehaviour {

		// Transform of the camera to shake. Grabs the gameObject's transform
		// if null.
		public Transform camTransform;
		public float shakeDurationValue = 0f;
		public float shakeAmountValue = 0.7f;
		public float decreaseFactorValue = 1.0f;

		// How long the object should shake for.
		private float shakeDuration = 0f;

		// Amplitude of the shake. A larger value shakes the camera harder.
		private float shakeAmount = 0.7f;
		private float decreaseFactor = 1.0f;

		private bool shaketrue= false;

		Vector3 originalPos;

		void Awake()
		{
			if (camTransform == null)
			{
				camTransform = GetComponent(typeof(Transform)) as Transform;
			}
		}

		void OnEnable()
		{
			originalPos = camTransform.localPosition;
			EventManager.StartListening ("Player1Death", OnNeedShake);
			EventManager.StartListening ("Player2Death", OnNeedShake);
		}

		void OnDisable()
		{
			EventManager.StopListening ("Player1Death", OnNeedShake);
			EventManager.StopListening ("Player2Death", OnNeedShake);
		}

		void OnNeedShake()
		{
			shakeDuration = shakeDurationValue;
			shakeAmount = shakeAmountValue;
			decreaseFactor = decreaseFactorValue;
			shakecamera ();
		}

		void Update()
		{
			if (shaketrue) 
			{
				if (shakeDuration > 0) {
					camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

					shakeDuration -= Time.deltaTime * decreaseFactor;
				} else {
					shakeDuration = 1f;
					camTransform.localPosition = originalPos;
					shaketrue = false;
				}
			}
		}

		public void shakecamera()
		{
			shaketrue = true;
		}
	}

}