using UnityEngine;

public class PlayerHand : CardZone
{
    public ProgramCardZone PlayerProgram;

    private ArenaController arena;

    public void Start()
    {
        arena = FindObjectOfType<ArenaController>();
    }

    public override void OnCardClick(GameObject gameObject)
    {
        if (!arena.isExecuting)
        {
            gameObject.GetComponent<CardBase>().MoveTo(PlayerProgram);
        }
    }
}

