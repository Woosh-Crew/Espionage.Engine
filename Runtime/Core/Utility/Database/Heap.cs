using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Array representing a binary tree allowing us to make quick comparisons
/// </summary>
public class Heap<T> where T : IHeapItem<T> {
	
    /// <summary>
    /// An Array of generics
    /// </summary>
	public T[] Items;

    /// <summary>
    /// The current number of items in the heap
    /// </summary>
	private int _currentItemCount;
	
    /// <summary>
    /// Initializes the heap to a given size
    /// </summary>
	public Heap(int maxHeapSize) {
        //Initializes array of type T
		Items = new T[maxHeapSize];
	}
	
    /// <summary>
    /// Adds an item to the heap
    /// </summary>
	public void Add(T item) {
        //Set the added items HeapIndex to the current number of items
		item.HeapIndex = _currentItemCount;
        //Set the index at the current count to the added item
		Items[_currentItemCount] = item;
        //Place the item in the appropriate place within the heap
		SortUp(item);
        //Increment the number of items in the heap
		_currentItemCount++;
	}

	/// <summary>
	/// Returns and removes first item from the heap
	/// </summary>
	public T RemoveFirst() {
		//Get refernce to first item
		T firstItem = Items[0];
		//Reduce count of items
		_currentItemCount--;
		//Take item at the end of the heap and put it into the first place in the items array
		Items[0] = Items[_currentItemCount];
		Items[0].HeapIndex = 0;
		//Resort the heap
		SortDown(Items[0]);
		return firstItem;
	}

	/// <summary>
	/// Updates a given item in the heap
	/// </summary>
	public void UpdateItem(T item) {
		SortUp(item);
	}

	/// <summary>
	/// The number of items in the heap
	/// </summary>
	public int Count {
		get {
			return _currentItemCount;
		}
	}

	/// <summary>
	/// Checks to see if the heap contains a specified item
	/// </summary>
	public bool Contains(T item) {
		return Equals(Items[item.HeapIndex], item);
	}

	/// <summary>
	/// Sorts the array from the top of the heap to the bottom
	/// </summary>
	void SortDown(T item) {
		while (true) {
			//Get the items left child
			int childIndexLeft = item.HeapIndex * 2 + 1;
			//Get the items right child
			int childIndexRight = item.HeapIndex * 2 + 2;
			//Variable for holding where want to swap to
			int swapIndex = 0;

			//Check to see if the given item has at least one child
			if (childIndexLeft < _currentItemCount) {
				//Set swap index to the left child
				swapIndex = childIndexLeft;
				
				//If the item has a child on the right
				if (childIndexRight < _currentItemCount) {
					//Check to see if the right index has a higher priority than the left
					if (Items[childIndexLeft].CompareTo(Items[childIndexRight]) < 0) {
						swapIndex = childIndexRight;
					}
				}

				//Check if the parent has a lower priority than its highest priority child, then swap them
				if (item.CompareTo(Items[swapIndex]) < 0) {
					Swap (item,Items[swapIndex]);
				}
				else {
					//If the Item has a higher priority than both of its children
					return;
				}

			}
			else {
				//if the given item has no children
				return;
			}

		}
	}
	
    /// <summary>
    /// Sorts an item upward in the heap so it is in the appropriate position
    /// </summary>
	void SortUp(T item) {

        //Formula for parent index of a 'node' in the tree is (n-1)/2
		int parentIndex = (item.HeapIndex-1)/2;
		
		while (true) {
            //Get a reference to our parent node
			T parentItem = Items[parentIndex];
            
			//Make use of icomparable to compare the two items
			if (item.CompareTo(parentItem) > 0) {
				Swap (item,parentItem);
			}
			else {
				break;
			}

			//Rset the parent index so item can be compared to new parent
			parentIndex = (item.HeapIndex-1)/2;
		}
	}
	
	/// <summary>
	/// Swaps two items in an array
	/// </summary>
	void Swap(T itemA, T itemB) {
		//PErform the swap
		Items[itemA.HeapIndex] = itemB;
		Items[itemB.HeapIndex] = itemA;
		//Store temp itemA index
		int itemAIndex = itemA.HeapIndex;
		//Swap the items heap indices
		itemA.HeapIndex = itemB.HeapIndex;
		itemB.HeapIndex = itemAIndex;
	}
}

/// <summary>
/// An interface which implements a 'HeapIndex' value used for heap comparisons
/// </summary>
public interface IHeapItem<T> : IComparable<T> {
	public int HeapIndex {
		get;
		set;
	}
}
