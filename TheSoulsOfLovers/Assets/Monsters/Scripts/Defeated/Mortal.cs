using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Mortal : MobDefeated
{
    protected Coroutine coroutine;
    public override void Destroyed(Transform mTransform, Rigidbody2D mRigidbody2D, Animator mAnimator)
    {
        transform = mTransform;
        rigidbody2D = mRigidbody2D;
        animator = mAnimator;

        animator.Play("DestroyedStart");
        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        Coroutine("DestroyedStart", animatorStateInfo.length);
    }
    public void Coroutine(string type, float cooldownTime)
    {
        switch (type)
        {
            case "DestroyedStart":
                coroutine = StartCoroutine(coroutineDestroyedStart(cooldownTime));
                break;
        }
    }
    public IEnumerator coroutineDestroyedStart(float cooldownTime)
    {
        rigidbody2D.velocity = Vector3.zero;
        rigidbody2D.isKinematic = true;
        wait = true;
        animator.Play("DestroyedStart");
        yield return new WaitForSeconds(cooldownTime);

        PlayerPrefs playerPrefs = GameObject.FindWithTag("PlayerPrefs").GetComponent<PlayerPrefs>();
        GameData.DefeatedEnemy enemy = new GameData.DefeatedEnemy() { location=playerPrefs.location, 
            scene=playerPrefs.scene, name= transform.name, selectedTime = playerPrefs.currentTime };
        int indEnemy = playerPrefs.locations[playerPrefs.location].DefeatedEnemies.IndexOf(enemy);
        if(indEnemy==-1)
            playerPrefs.locations[playerPrefs.location].DefeatedEnemies.Add(enemy);
        Destroy(transform.gameObject);
    }
}
