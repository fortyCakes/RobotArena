using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProgrammableRobot : ArenaObject
{
    public ProgramCardZone Program;
    public ArchiveMarker ArchiveMarker;

    public bool IsAlive = true;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        var archiveMarker = GameObject.Instantiate((GameObject)Resources.Load("prefabs/ArchiveMarker", typeof(GameObject)));
        
        ArchiveMarker = archiveMarker.GetComponent<ArchiveMarker>();
        ArchiveMarker.Bot = this;
        ArchiveMarker.ArenaPosition = this.ArenaPosition;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override IEnumerator FallInPit()
    {
        var duration = 0.2f;
        var time = 0f;
        var initialPosition = ArenaPosition;
        var endPosition = ArchiveMarker.ArenaPosition;
        while (time < duration)
        {
            ArenaPosition = new Vector2(Mathf.Lerp(initialPosition.x, endPosition.x, time / duration), Mathf.Lerp(initialPosition.y, endPosition.y, time / duration)).AsIntVector();
            yield return null;
            time += Time.deltaTime;
        }
        ArenaPosition = ArchiveMarker.ArenaPosition;
        foreach(var card in Program.AllCards)
        {
            card.GetComponent<CardBase>().MoveTo(arena.discard);
        }
        yield return null;
    }
}
