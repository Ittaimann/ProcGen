using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {
	public string sentenceName;
	public enum Direction {N, W, E, S};
	public Rules rules;

	public int gridx, gridy;
	private Direction direction;
	private bool [,] visited;
	private int visitedCount = 0;
	private int [] dx, dy;
	private Direction [] directions;
	private Direction [] opposite;
	private CaretTree tree;
	private StringBuilder lsymbols;
	private CaretTree.Node parent, head;
	private Direction [,] directionMap;

	public void Awake() {
		direction = Direction.N;
		visited = new bool[gridx, gridy];
		dx = new int[4]{0, -1, 1, 0};
		dy = new int[4]{1, 0, 0, -1};
 		directions = new Direction[4] {Direction.N, Direction.W, Direction.E, Direction.S};
		opposite = new Direction[4]{Direction.S, Direction.E, Direction.W, Direction.N};
		tree = new CaretTree();
		head = parent = null;
		lsymbols = new StringBuilder();
		directionMap = new Direction[gridx, gridy];
	}

	public string generate() {
		lsymbols.Append("++G++");
		int x = Random.Range(0, gridx - 1),
		    y = Random.Range(0, gridy - 1);
		insertDirection(direction);
		CaretTree.Node current = new CaretTree.Node(lsymbols.ToString(), x, y);
		Debug.Log("Start: " + x + " " + y);
		tree.AddNode(null, null, current);
		int total = gridx * gridy;
		while(visitedCount < total) {
			ClearBuffer();
			current = walk(current.x, current.y);
			if(current == null)
				current = hunt();	
		}
		return tree.Combine();
	}

	private int[] permutation(int range) {
		List<int> perm = new List<int>();
		List<int> nums = new List<int>();
		int size = range;
		int index = 0;
		for(int i = 0; i < range; ++i)
			nums.Add(i);
		for(int i = size - 1; i > -1; --i) {
			index = Random.Range(0, i);
			perm.Add(nums[index]);
			nums.Remove(nums[index]);
		}
		return perm.ToArray();
	}

	private void insertDirection(Direction d) {
		if(direction != d) {
			restore();
			direction = d;
			insertIntoBuffer(rules.rules[(int) d].RHS);
		}
		else
			insertIntoBuffer('F');
	}

	private void insertIntoBuffer(char c) {
		lsymbols.Append(c);
	}

	private void insertIntoBuffer(string s) {
		lsymbols.Append(s);
	}
	private void ClearBuffer() {
		lsymbols.Remove(0, lsymbols.Length);
	}
	private CaretTree.Node walk(int x, int y) {
		int [] indices = permutation(4);
		int nx, ny;
		if(visited[x,y]) {
			Debug.Assert(false);
			return null;
		}
		directionMap[x,y] = direction;
		visited[x,y] = true;
		++visitedCount;
		for(int i = 0; i < 4; ++i) {
			nx = x + dx[(int) directions[indices[i]]];
			ny = y + dy[(int) directions[indices[i]]];
			if (nx > -1 && nx < gridx && ny > -1 && ny < gridy && !visited[nx,ny]) {
				insertDirection(directions[indices[i]]);
				CaretTree.Node newNode = new CaretTree.Node(lsymbols.ToString(), nx, ny);
				Debug.Log("BeginNode: " + x + " " + y);
				Debug.Log("EndNode: " + nx + " " + ny);
				tree.AddNode(parent, head, newNode);
				return newNode;
			}
		}
		return null;
	}

	private CaretTree.Node hunt() {
		int [] indices = permutation(4);
		int nx, ny;
		for(int j = 0; j < gridy; ++j)
			for(int i = 0; i < gridx; ++i)
				if(!visited[i,j])
					for(int d = 0; d < 4; ++d) {
						nx = i + dx[(int) directions[indices[d]]];
						ny = j + dy[(int) directions[indices[d]]];
						if (nx > -1 && nx < gridx && ny > -1 && ny < gridy && visited[nx, ny]) {
							direction = directionMap[nx, ny];
							Debug.Log("Hunt: " + direction + " " + opposite[(int) directions[d]]);
							insertDirection(opposite[(int) directions[indices[d]]]);
							CaretTree.Node newNode = new CaretTree.Node(lsymbols.ToString(), i, j);
							parent = tree.GetNode(nx, ny);
							Debug.Log("Node: " + i + " " + j);
							Debug.Log("Parent: " + parent.x + " " + parent.y);
							head = newNode;
							tree.AddNode(parent, null, newNode);
							return newNode;
						}
					}
		return null;
	}

	private void restore() {
		switch(direction) {
			case Direction.N:
				return;
			case Direction.W:
				insertIntoBuffer("+");
				break;
			case Direction.E:
				insertIntoBuffer("-");
				break;
			case Direction.S:
				insertIntoBuffer("++");
				break;
		}
	}
}
