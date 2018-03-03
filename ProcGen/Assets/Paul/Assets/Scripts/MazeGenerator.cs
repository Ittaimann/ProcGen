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
	private StringBuilder program;
	private StringBuilder save;
	private bool [,] visited;
	private int visitedCount = 0;
	private int [] dx, dy;
	private int index, saveIndex, size;
	private bool [,] inBranch;
	private bool saved = false;
	private int [,] gridToBufferIndex;
	private Direction [] directions;
	private Direction [] opposite;

	public void Awake() {
		direction = Direction.N;
		program = new StringBuilder();
		program.Append("0");
		visited = new bool[gridx, gridy];
		dx = new int[4]{0, -1, 1, 0};
		dy = new int[4]{1, 0, 0, -1};
		index = 0;
		size = 0;
		saveIndex = 0;
		gridToBufferIndex = new int[gridx, gridy];
		inBranch = new bool[gridx, gridy];
 		directions = new Direction[] {Direction.N, Direction.W, Direction.E, Direction.S};
		opposite = new Direction[4]{Direction.S, Direction.E, Direction.W, Direction.N};
	}

	public string generate() {
		int [] pos = new int[2]{Random.Range(0, gridx - 1), 
		                        Random.Range(0, gridy - 1)};
		int total = gridx * gridy;
		insertIntoBuffer('[');
		++visitedCount;
		insertIntoBuffer("--G++F");
		while(visitedCount < total) {
			pos = walk(pos[0], pos[1]);
			if(pos == null) {
				if(saved)
					reset();
				else	
					insertIntoBuffer(']');
				pos = hunt();	
			}
		}
		program.Insert(size, ']');
		return program.ToString();
	}

	private void reset() {
		insertIntoBuffer(']');
		string res = program.ToString();
		int fromHere = index;
		program = save;
		index = saveIndex;
		insertIntoBuffer(res);
		for(int i = 0; i < gridx; ++i)
			for(int j = 0; j < gridy; ++j)
				if(gridToBufferIndex[i,j] > fromHere && !inBranch[i, j])
					gridToBufferIndex[i,j] += res.Length;
				else if(inBranch[i, j])
					inBranch[i,j] = true;
		saved = false;
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
		program.Insert(index++, c);
		++size;
	}

	private void insertIntoBuffer(string s) {
		program.Insert(index, s);
		index += s.Length;
		size += s.Length;
	}

	private int[] walk(int x, int y) {
		int [] indices = permutation(4);
		int nx, ny;
		for(int i = 0; i < 4; ++i) {
			nx = x + dx[(int) directions[indices[i]]];
			ny = y + dy[(int) directions[indices[i]]];
			if (nx > -1 && nx < gridx && ny > -1 && ny < gridy && !visited[nx,ny]) {
				insertDirection(directions[indices[i]]);
				gridToBufferIndex[x,y] = index;
				if(saved)
					inBranch[x,y] = true;
				visited[x,y] = true;
				++visitedCount;
				return new int[2] {nx, ny};
			}
		}
		return null;
	}

	private int[] hunt() {
		int [] indices = permutation(4);
		int nx, ny;
		for(int i = 0; i < gridx; ++i)
			for(int j = 0; j < gridy; ++j)
				if(!visited[i,j])
					for(int d = 0; d < 4; ++d) {
						nx = i + dx[(int) directions[indices[d]]];
						ny = j + dy[(int) directions[indices[d]]];
						if (nx > -1 && nx < gridx && ny > -1 && ny < gridy && visited[nx, ny]) {
							saveIndex = index;
							save = program;
							saved = true;
							program = new StringBuilder();
							index = gridToBufferIndex[nx, ny];
							insertIntoBuffer('[');
							insertDirection(opposite[(int) directions[indices[d]]]);
							gridToBufferIndex[i,j] = index;
							insertIntoBuffer(']');
							insertIntoBuffer('[');
							visited[i, j] = true;
							inBranch[i, j] = true;
							++visitedCount;
							return new int[2]{nx, ny};	
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
