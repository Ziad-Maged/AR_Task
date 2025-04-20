using UnityEngine;

public class Spider : ARInteractableObject
{
    [SerializeField] private Renderer spiderRenderer;
    private Animator spiderAnimator;

    private void OnEnable()
    {
        spiderAnimator = GetComponent<Animator>();
    }

    protected override void SetState(State state)
    {
        base.SetState(state);
        switch (state)
        {
            case State.IDLE:
                spiderAnimator.SetTrigger("GoToIdle");
                if (spiderRenderer == null) return;
                spiderRenderer.materials[0].EnableKeyword("_EMISSION");
                spiderRenderer.materials[0].SetColor("_EmissionColor", new(0, 0, 0, 0));
                break;
            case State.ACTIVE:
                spiderAnimator.SetTrigger("StartInteraction");
                if (spiderRenderer == null) return;
                spiderRenderer.materials[0].EnableKeyword("_EMISSION");
                spiderRenderer.materials[0].SetColor("_EmissionColor", new(0.5f, 0.5f, 0.5f, 0.01f));
                break;
        }
    }
}
