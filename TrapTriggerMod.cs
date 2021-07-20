using UnityEngine;

namespace ChampionsOfForest
{
	public class TrapTriggerMod : trapTrigger
	{
		public override void TriggerLargeTrap(Collider other)
		{
			if (this.MpClientCheck && other == null && this.largeNoose)
			{
				this.switchNooseRope();
				base.Invoke("EnableCutTrigger", 1.5f);
				this.animator.enabled = true;
				this.animator.SetIntegerReflected("direction", 0);
				this.animator.SetBoolReflected("trapSpringBool", true);
			}
			else if (this.MpHostCheck && this.largeNoose)
			{
				this.SprungTrap();
			}
			if (this.sprung)
			{
				return;
			}
			this.CheckAnimReference();
			bool flag = !BoltNetwork.isRunning || this.MpHostCheck;
			if (this.hitbox && !this.hitbox.activeSelf)
			{
				this.hitbox.SetActive(true);
			}
			if (this.largeSwingingRock)
			{
				this.cutRope.SetActive(false);
				this.swingingRock.SendMessage("enableSwingingRock", false);
				base.Invoke("enableTrapReset", 3f);
			}
			if (this.largeDeadfall)
			{
				this.anim.GetComponent<Animation>().Play("trapFall");
				this.spikeTrapBlockerGo.SetActive(true);
				base.Invoke("enableTrapReset", 3f);
			}
			if (this.largeSpike)
			{
				this.anim.GetComponent<Animation>().Play("trapSpring");
				this.spikeTrapBlockerGo.SetActive(true);
				if (flag && other)
				{
					other.gameObject.SendMessageUpwards("enableController", SendMessageOptions.DontRequireReceiver);
					if (other.gameObject.CompareTag("enemyCollide"))
					{
						this.mutantSetup = other.transform.root.GetComponentInChildren<global::mutantScriptSetup>();
						if (this.mutantSetup && !this.mutantSetup.ai.creepy && !this.mutantSetup.ai.creepy_male && !this.mutantSetup.ai.creepy_fat && !this.mutantSetup.ai.creepy_baby)
						{
							other.gameObject.SendMessageUpwards("setCurrentTrap", base.gameObject, SendMessageOptions.DontRequireReceiver);
						}
					}
				}
				base.Invoke("enableTrapReset", 3f);
			}
			if (this.largeNoose)
			{
				if (flag && other)
				{
					if (other.gameObject.GetComponentInParent<EnemyProgression>()?.abilities.Count > 0)
						return;//noose traps are too strong against elites
							   //so they no longer work on them

					global::mutantHitReceiver component = other.transform.GetComponent<global::mutantHitReceiver>();
					if (other.gameObject.CompareTag("enemyCollide"))
					{
						if (component)
						{
							component.inNooseTrap = true;
							component.DisableWeaponHits(2f);
						}
						this.mutantSetup = other.transform.root.GetComponentInChildren<global::mutantScriptSetup>();
					}
					this.trappedMutants.Clear();
					this.trappedMutantMasks.Clear();
					GameObject gameObject = other.transform.root.gameObject;
					this.addTrappedMutant(gameObject);
					global::mutantScriptSetup componentInChildren = other.transform.root.GetComponentInChildren<global::mutantScriptSetup>();
					if (componentInChildren && componentInChildren.ai && componentInChildren.ai.pale)
					{
						this.FixPalePosition(componentInChildren, true);
					}
					if (base.transform.InverseTransformPoint(other.transform.position).x > 0f)
					{
						this.animator.SetIntegerReflected("direction", 0);
					}
					else
					{
						this.animator.SetIntegerReflected("direction", 1);
					}
					other.gameObject.SendMessageUpwards("setFootPivot", this.nooseFootPivot, SendMessageOptions.DontRequireReceiver);
					this.animator.enabled = true;
					this.animator.SetBoolReflected("trapSpringBool", true);
					if (this.mutantSetup)
					{
						if (!this.mutantSetup.ai.creepy && !this.mutantSetup.ai.creepy_male && !this.mutantSetup.ai.creepy_fat && !this.mutantSetup.ai.creepy_baby)
						{
							other.gameObject.SendMessageUpwards("setInNooseTrap", this.noosePivot);
						}
						other.gameObject.SendMessageUpwards("setCurrentTrap", base.gameObject);
					}
				}
				if (other)
				{
					other.gameObject.SendMessageUpwards("DieTrap", 2, SendMessageOptions.DontRequireReceiver);
				}
				this.switchNooseRope();
				base.Invoke("EnableCutTrigger", 1.5f);
				if (base.entity.IsOwner() && this.largeNoose)
				{
					base.entity.GetState<ITrapLargeState>().CanCutDown = true;
					base.entity.GetState<ITrapLargeState>().CanReset = false;
				}
				if (other && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerNet")))
				{
					base.Invoke("enableTrapReset", 2f);
				}
			}
			if (this.hitbox)
			{
				base.Invoke("disableHitbox", 1.5f);
			}
			base.transform.GetComponent<Collider>().enabled = false;
			this.SprungTag = true;
			this.PlayTriggerSFX();
		}
	}
}