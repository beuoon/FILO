using System.Collections.Generic;
using UnityEngine;

public class Electric : MonoBehaviour {
    private Vector3Int _position;
    public Vector3Int position {
        set { _position = value; }
        get { return _position;  }
    }

    void Start() {
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                Vector3Int pos = _position + new Vector3Int(x, y, 0);
                if (TileMgr.Instance.Waters.ContainsKey(pos) && !TileMgr.Instance.Waters[pos].ExistOriginElectric(position))
                    Electrify(pos);
            }
        }
    }
	private void OnDestroy() {
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                Vector3Int pos = _position + new Vector3Int(x, y, 0);
                if (TileMgr.Instance.Waters.ContainsKey(pos) && TileMgr.Instance.Waters[pos].ExistOriginElectric(position))
                    RemoveElectric(pos);
            }
        }
    }

	void Update() {
    }

    void Electrify(Vector3Int pos) {
        HashSet<Vector3Int> searchHistory = new HashSet<Vector3Int>();
        Queue<Vector3Int> searchQueue = new Queue<Vector3Int>();
        searchQueue.Enqueue(pos);

        while (searchQueue.Count > 0) {
            Vector3Int cp = searchQueue.Dequeue();
            searchHistory.Add(cp);
            TileMgr.Instance.Waters[cp].Electrify(position);

            for (int y = -1; y <= 1; y++) {
                for (int x = -1; x <= 1; x++) {
                    Vector3Int np = cp + new Vector3Int(x, y, 0);
                    if (!searchHistory.Contains(np) && TileMgr.Instance.Waters.ContainsKey(np))
                        searchQueue.Enqueue(np);
                }
            }
        }
	}

    void RemoveElectric(Vector3Int pos) {
        HashSet<Vector3Int> searchHistory = new HashSet<Vector3Int>();
        Queue<Vector3Int> searchQueue = new Queue<Vector3Int>();
        searchQueue.Enqueue(pos);

        while (searchQueue.Count > 0) {
            Vector3Int cp = searchQueue.Dequeue();
            searchHistory.Add(cp);
            TileMgr.Instance.Waters[cp].RemoveElectric(position);

            for (int y = -1; y <= 1; y++) {
                for (int x = -1; x <= 1; x++) {
                    Vector3Int np = cp + new Vector3Int(x, y, 0);
                    if (!searchHistory.Contains(np) && TileMgr.Instance.Waters.ContainsKey(np))
                        searchQueue.Enqueue(np);
                }
            }
        }
    }
}
