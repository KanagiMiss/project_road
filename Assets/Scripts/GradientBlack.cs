using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientBlack : MonoBehaviour {
	public Mesh mesh;
	public Material material;
	// Use this for initialization
	void Start () {
		/*
		MeshFilter viewedModelFilter = (MeshFilter)GetComponent("MeshFilter");
		Mesh mesh = viewedModelFilter.mesh;
		Color[] colors = new Color[mesh.vertices.Length];
		Color BottomColor = Color.red;
		Color TopColor = Color.blue;
		colors[0] = BottomColor;
		colors[1] = TopColor;
		colors[2] = BottomColor;
		colors[3] = TopColor;
		mesh.colors = colors;*/
	}
	
	// Update is called once per frame
	void Update () {
		// will make the mesh appear in the scene at origin position
		Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, material, 0);
	}
}
