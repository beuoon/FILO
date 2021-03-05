using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EventMgr {
    private readonly List<Event> events = new List<Event>();

    public static readonly Object FallingRock   = Resources.Load("Prefabs/Event/Event_FallingRock");
    public static readonly Object ShortCircuit  = Resources.Load("Prefabs/Event/Event_ShortCircuit");
    public static readonly Object Flashover     = Resources.Load("Prefabs/Event/Event_Flashover");
    public static readonly Object Smoke         = Resources.Load("Prefabs/Event/Event_Smoke");

    public EventMgr(int stage) {
        LoadEvent(stage);
	}
    private void LoadEvent(int stage) {
        // XML Load
        XmlDocument doc = new XmlDocument();
        TextAsset textAsset = (TextAsset)Resources.Load("Event/Stage" + stage);
        doc.LoadXml(textAsset.text);

        XmlNodeList eventNodes = doc.SelectNodes("Events/Event");
        foreach (XmlNode eventNode in eventNodes) {
            Event.EventType type = Event.StringToType(eventNode.SelectSingleNode("Type").InnerText);

			string[] pointStr = eventNode.SelectSingleNode("Point").InnerText.Replace(" ", "").Split(',');
            int x = int.Parse(pointStr[0]);
            int y = int.Parse(pointStr[1]);
            Vector3Int point = new Vector3Int(x, y, 0);

            int turn = int.Parse(eventNode.SelectSingleNode("Turn").InnerText);

            events.Add(new Event(type, point, turn));
        }
    }

    public void TurnUpdate() {
		for (int i = 0; i < events.Count;) {
            events[i].Update();

            if (events[i].IsSatisfied) {
                CreateEventObject(events[i]);
                events.RemoveAt(i);
            }
            else
                i++;
		}
	}

    private void CreateEventObject(Event e) {
        Object obj = null;
        Vector3 pos = GameMgr.Instance.BackTile.CellToWorld(e.position);
        pos += GameMgr.Instance.BackTile.cellSize / 2.0f;

        switch (e.type) {
        case Event.EventType.FALLING_ROCK:  obj = FallingRock;   break;
        case Event.EventType.SHORT_CIRCUIT: obj = ShortCircuit;  break;
        case Event.EventType.FLASHOVER:     obj = Flashover;     break;
        case Event.EventType.SMOKE:         obj = Smoke;         break;
        }

        Object.Instantiate(obj, pos, Quaternion.identity);
    }
}
