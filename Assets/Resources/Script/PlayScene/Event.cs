using UnityEngine;

public class Event {
    public enum EventType {
        FALLING_ROCK, SHORT_CIRCUIT, FLASHOVER, SMOKE
    }

    public readonly EventType type;
    public readonly Vector3Int position;
    private int leftTurn;

    public Event(EventType type, Vector3Int position, int turn) {
        this.type = type;
        this.position = position;
        this.leftTurn = turn;
	}

    public virtual void Update() {
        leftTurn--;
    }

    public bool IsSatisfied {
        get { return leftTurn <= 0; }
    }

    public static EventType StringToType(string text) {
        switch (text) {
        case "FALLING_ROCK": return EventType.FALLING_ROCK;
        case "SHORT_CIRCUIT": return EventType.SHORT_CIRCUIT;
        case "FLASHOVER": return EventType.FLASHOVER;
        case "SMOKE": return EventType.SMOKE;
        }
        return EventType.FALLING_ROCK;
    }
}