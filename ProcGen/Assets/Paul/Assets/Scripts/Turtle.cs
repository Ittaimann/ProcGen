using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turtle : MonoBehaviour {
	public float width = .1f;
	public float length = 10f;
	public float angle = 90f;
	public Material groundMat, wallMat;
	private float rad_angle;
	public GameObject root;
	private GameObject tree;
	private Stack<State> state_stack;
	private State cur_state;
	void Start() {
		Init();
		DrawTree();
	}

	protected void Init() {
		rad_angle = angle * Mathf.PI / 180f;
		cur_state = new State(new Vector2(-2, -2), rad_angle);
		state_stack = new Stack<State>();
	}

	private class State {
		public Vector2 cursor;
		public float angle;
		public State (Vector2 cursor, float angle) {
			this.cursor = cursor;
			this.angle = angle;
		}
		public State clone() {
			return new State(cursor, angle);			
		}
	}

	private Color lineColor = Color.red;
	private Material lineMaterial;
	private void DrawLine(Vector2 start, Vector2 end) {
		GL.Begin(GL.LINES);
		GL.Color(Color.red);
		GL.Vertex3(start.x + 2, start.y, 0);
		GL.Vertex3(end.x + 2, end.y, 0);
		GL.End();
	}

	private void PlaceUnit(Vector2 start, Vector2 end) {
		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
		Mesh mesh = plane.GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		float radius = Vector2.Distance(start, end) / 2;
		vertices[0] = end + new Vector2(-radius, -radius);
		vertices[1] = end + new Vector2(radius, radius);
		vertices[2] = end + new Vector2(radius, -radius);
		vertices[3] = end + new Vector2(-radius, radius);
		//Create mesh
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
		MeshCollider coll = plane.GetComponent<MeshCollider>();
		coll.sharedMesh = null;
		coll.sharedMesh = mesh;
		plane.GetComponent<Renderer>().material = groundMat;
		Parent(plane);
	}
	private void PlaceBackUnit(Vector2 start, Vector2 end) {
		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
		Mesh mesh = plane.GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		float radius = Vector2.Distance(start, end) / 2;
		vertices[3] = end + new Vector2(-radius, -radius);
		vertices[2] = end + new Vector2(radius, radius);
		vertices[1] = end + new Vector2(radius, -radius);
		vertices[0] = end + new Vector2(-radius, radius);
		//Create mesh
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
		Parent(plane);
	}

	private void PlaceFrontWall(Vector2 center, Vector2 direction) {
		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
		Mesh mesh = plane.GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector3 wall_center = center + direction * length / 2;
		float radius = Mathf.Sqrt(Mathf.Pow(length / 2, 2));
		Vector3 strafe = Vector3.Cross(-direction, Vector3.back);
		wall_center += Vector3.back * length / 2;
		vertices[0] = wall_center + (strafe + Vector3.back) * radius;
		vertices[1] = wall_center + (-strafe + Vector3.forward) * radius;
		vertices[2] = wall_center + (-strafe + Vector3.back) * radius;
		vertices[3] = wall_center + (strafe + Vector3.forward) * radius;
		//Create mesh
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
		MeshCollider coll = plane.GetComponent<MeshCollider>();
		coll.sharedMesh = null;
		coll.sharedMesh = mesh;
		plane.GetComponent<Renderer>().material = wallMat;
		Parent(plane);
	}
	private void PlaceBackWall(Vector2 center, Vector2 direction) {
		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
		Mesh mesh = plane.GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector3 wall_center = center + direction * length / 2;
		Vector3 strafe = Vector3.Cross(-direction, Vector3.back);
		float radius = Mathf.Sqrt(Mathf.Pow(length / 2, 2));
		wall_center += Vector3.back * length / 2;
		vertices[3] = wall_center + (strafe + Vector3.back) * radius;
		vertices[2] = wall_center + (-strafe + Vector3.forward) * radius;
		vertices[1] = wall_center + (-strafe + Vector3.back) * radius;
		vertices[0] = wall_center + (strafe + Vector3.forward) * radius;
		//Create mesh
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
		MeshCollider coll = plane.GetComponent<MeshCollider>();
		coll.sharedMesh = null;
		coll.sharedMesh = mesh;
		plane.GetComponent<Renderer>().material = wallMat;
		Parent(plane);
	}

	void Parent(GameObject go) {
		go.transform.parent = tree.transform;
	}

	protected abstract string getSentence();
	public void DrawTree() {
		GameObject new_tree = Instantiate(root, Vector3.zero, Quaternion.identity);
		if(tree != null)
			GameObject.DestroyImmediate(tree);
		tree = new_tree;
		tree.transform.parent = transform;
		Vector2 new_pos;
		Vector2 center = Vector2.zero;
		state_stack.Push(cur_state.clone());
		foreach(char c in getSentence()) {
			switch(c) {
				case 'F':
					new_pos = cur_state.cursor + new Vector2(Mathf.Cos(cur_state.angle), Mathf.Sin(cur_state.angle)) * length;
					PlaceUnit(cur_state.cursor, new_pos);
					PlaceBackUnit(cur_state.cursor, new_pos);
					cur_state.cursor = new_pos;
					break;
				case 'G':
					cur_state.cursor += new Vector2(Mathf.Cos(cur_state.angle), Mathf.Sin(cur_state.angle)) * length;
					break;
				case '+':
					cur_state.angle += rad_angle;
					break;
				case '-':
					cur_state.angle -= rad_angle;
					break;
				case '[':
					state_stack.Push(cur_state.clone());
					break;
				case ']':
					cur_state = state_stack.Pop();
					break;
				case 'A':
				case 'L':
				case 'R':
				case 'B':
					center = cur_state.cursor - new Vector2(Mathf.Cos(cur_state.angle), Mathf.Sin(cur_state.angle)) * length;
					break;
			}
			switch(c) {
				case 'A':
					PlaceFrontWall(center, Vector2.up);
					PlaceBackWall(center, Vector2.up);
					break;
				case 'L':
					PlaceFrontWall(center, Vector2.left);
					PlaceBackWall(center, Vector2.left);
					break;
				case 'R':
					PlaceFrontWall(center, Vector2.right);
					PlaceBackWall(center, Vector2.right);
					break;
				case 'B':
					PlaceFrontWall(center, Vector2.down);
					PlaceBackWall(center, Vector2.down);
					break;
			}
		}
		tree.transform.Rotate(new Vector3(90, 0, 0));
		cur_state = state_stack.Pop();
	}
}

