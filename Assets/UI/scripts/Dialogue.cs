using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOAT{

	[System.Serializable]
	public class Dialogue {

		public string title;

		[TextArea(3, 10)]
		public string[] sentences;

	}
}