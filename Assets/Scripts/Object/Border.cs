using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{
public class Border : MonoBehaviour {
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.tag == "Enemy") {
				Destroy(other.gameObject);
			}
		}
	}
}
