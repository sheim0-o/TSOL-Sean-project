using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobDefeated : MonoBehaviour
{
    protected bool wait = false;
    protected Rigidbody2D rigidbody2D;
    protected Transform transform;
    protected Animator animator; 
    public virtual void Destroyed(Transform mTransform, Rigidbody2D mRigidbody2D, Animator mAnimator) { }
}
