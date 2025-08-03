using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private Rigidbody2D[] rbs;
    public int currentRbIndex = 0;

    void Start()
    {
        rbs = GetComponentsInChildren<Rigidbody2D>();
    }

   public void SetRigidBodyActiveForSprite( int spriteNum ) {
        rbs[currentRbIndex].simulated = false;
        currentRbIndex = spriteNum;
        rbs[currentRbIndex].simulated = true;
    }
}
