using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turtle : MonoBehaviour {
	public float width = .1f;
	public float length = 10f;
	public float angle = 90f;
	private float rad_angle;
	public GameObject root;
	private GameObject tree;
	private Stack<State> state_stack;
	private State cur_state;
	private Camera camera;
	void Start() {
		Init();
	}

	protected void Init() {
		camera = GameObject.Find("Main Camera").GetComponent<Camera>();
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

	private GameObject PlaceUnit(Vector2 start, Vector2 end) {
		GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
		Mesh mesh = plane.GetComponent<MeshFilter>().mesh;
		Vector3[] vertices = mesh.vertices;
		Vector2 width = new Vector2(.1f, 0);
		int[] orders = new int[4];
		float radius = Vector2.Distance(start, end) / 2;
		vertices[0] = end + new Vector2(-radius, -radius);
		vertices[1] = end + new Vector2(radius, radius);
		vertices[2] = end + new Vector2(radius, -radius);
		vertices[3] = end + new Vector2(-radius, radius);
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
		return plane;
	}

	protected abstract string getSentence();
	public void DrawTree() {
		GameObject new_tree = Instantiate(root, Vector3.zero, Quaternion.identity);
		if(tree != null)
			GameObject.DestroyImmediate(tree);
		tree = new_tree;
		tree.transform.parent = transform;
		Vector2 new_pos;
		state_stack.Push(cur_state.clone());
		foreach(char c in getSentence())
			switch(c) {
				case 'F':
					new_pos = cur_state.cursor + new Vector2(Mathf.Cos(cur_state.angle), Mathf.Sin(cur_state.angle)) * length;
					PlaceUnit(cur_state.cursor, new_pos).transform.parent = tree.transform;
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
			}
		cur_state = state_stack.Pop();
	}
}
