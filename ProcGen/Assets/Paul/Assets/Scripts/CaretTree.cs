using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CaretTree {
	public class Node {
		public string lsymbols;
		public int x;
		public int y;
		public Node next;
		public List<Node> carets;
		public Node(string lsymbols, int x, int y) {
			this.lsymbols = lsymbols;
			this.x = x;
			this.y = y;
			carets = new List<Node>();
		}

		public void insert(Node head, Node newNode) {
			if(head == null) {
				carets.Add(newNode);
				return;
			}
			foreach(Node child in carets)
				if(head == child) {
					Node node;
					for(node = head; node.next != null; node = node.next);
					node.next = newNode;	
					return;
				}
		}
	}

	Node root;

	void Awake() {
		root = null;
	}

	public Node GetNode(int x, int y) {
		Queue<Node> children = new Queue<Node>();
		for(Node node = root; node != null || children.Count != 0; node = node.next != null ? node.next : children.Count != 0 ? children.Dequeue() : null)
			if(node.x == x && node.y == y)
				return node;
			else
				foreach(Node child in node.carets)
					children.Enqueue(child);
		return null;
	}

	public void AddNode(Node parent, Node head, Node newNode) {
		if(GetNode(newNode.x, newNode.y) != null)
			return;
		if(root == null) {
			root = newNode;
			return;
		}
		if(parent == null) {
			Node node;
			for(node = root; node.next != null; node = node.next);
			node.next = newNode;
			return;
		}
		Queue<Node> children = new Queue<Node>();
		for(Node node = root; node != null || children.Count != 0; node = node.next != null ? node.next : children.Count != 0 ? children.Dequeue() : null)
			if(node.x == parent.x && node.y == parent.y) {
				node.insert(head, newNode);
				return;
			} else
				foreach(Node child in node.carets)
					children.Enqueue(child);
	}

	public string Combine() {
		StringBuilder sentence = new StringBuilder();
		Accumulator(sentence, root);
		return sentence.ToString();
	}

	void Accumulator(StringBuilder sentence, Node head) {
		for(Node node = head; node != null; node = node.next) {
			sentence.Append(node.lsymbols);
			foreach(Node child in node.carets) {
				sentence.Append("[");
				Accumulator(sentence, child);
				sentence.Append("]");
			}
		}
	}
}
