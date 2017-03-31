using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.ShootingGallery;
using VRStandardAssets.Utils;

public class MultiTarget : ShootingTarget {

    private new void Awake()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).GetComponent<ShootingTarget>().IgnoreHit = true;
            //var item = this.transform.GetChild(i).GetComponent<VRInteractiveItem>();
            //item.OnDown += HandleDown;
        }

        base.Awake();
    }

    public override void TargetHit(int damage)
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            var target = this.transform.GetChild(i).GetComponent<ShootingTarget>();
            StartCoroutine(target.AnimateTargetHit());
        }

        if (--m_CurrentLifePoints > 0)
            return;

        // Turn off the visual and physical aspects.
        m_Renderer.enabled = false;
        m_Collider.enabled = false;

        base.PlayTargetDestroy();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            float speedDeviation = 0.70f;
            float speedMuliplier = 2f;
            var child = this.transform.GetChild(i);
            child.GetComponent<ShootingTarget>().IgnoreHit = false;
            var newTargetSpeed = Random.Range(TargetSpeed * (1 - speedDeviation), TargetSpeed * (1 + speedDeviation));
            child.GetComponent<ShootingTarget>().TargetSpeed = newTargetSpeed * speedMuliplier;

            var item = child.GetComponent<VRInteractiveItem>();
            //item.OnDown -= HandleDown;
        }
    }
	
	// Update is called once per frame
	void Update () {
        base.DoUpdate();
	}
}
