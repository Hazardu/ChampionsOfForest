using UnityEngine;

namespace ChampionsOfForest
{
	public class TrapHitMod : trapHit
	{
		protected override void registerTrapHit(Collider other)
		{
			if (other.GetComponent<global::creepyHitReactions>() && !this.disable && this.trigger.largeSpike)
			{
				this.sendCreepyDamageFromRoot(other);
				if (!this.disable)
				{
					base.Invoke("disableCollision", 0.8f);
				}
				return;
			}
			if (other.gameObject.CompareTag("enemyCollide") || other.gameObject.CompareTag("playerHitDetect") || other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("animalCollide"))
			{
				if (other.gameObject.CompareTag("enemyCollide"))
				{
					global::mutantHitReceiver component = other.transform.GetComponent<global::mutantHitReceiver>();
					if (component && component.inNooseTrap)
					{
						return;
					}
					global::explodeDummy component2 = other.GetComponent<global::explodeDummy>();
					if (component2)
					{
						return;
					}
				}
				if (!this.disable)
				{
					if (this.trigger.largeSpike)
					{
						other.gameObject.SendMessageUpwards("setTrapLookat", base.transform.root.gameObject, SendMessageOptions.DontRequireReceiver);
						base.gameObject.SendMessage("addTrappedMutant", other.transform.root.gameObject, SendMessageOptions.DontRequireReceiver);
						other.gameObject.SendMessageUpwards("setCurrentTrap", this.trigger.gameObject, SendMessageOptions.DontRequireReceiver);
						this.sendCreepyDamage(other);
						if (other.gameObject.CompareTag("Player"))
						{
							other.gameObject.SendMessage("HitFromTrap", 150, SendMessageOptions.DontRequireReceiver);
						}
					}
					else if (this.trigger.largeDeadfall)
					{
						if (other.gameObject.CompareTag("playerHitDetect"))
						{
							other.gameObject.SendMessageUpwards("HitFromTrap", 150, SendMessageOptions.DontRequireReceiver);
						}
						if (other.gameObject.CompareTag("enemyCollide"))
						{
							this.sendCreepyDamage(other);
						}
					}
					if (this.trigger.largeSwingingRock)
					{
						if (this.rb.velocity.magnitude > 11f)
						{
							other.gameObject.SendMessageUpwards("Hit", 1000, SendMessageOptions.DontRequireReceiver);
							//other.gameObject.SendMessageUpwards("Explosion", -1, SendMessageOptions.DontRequireReceiver);
							//other.gameObject.SendMessage("lookAtExplosion", base.transform.position, SendMessageOptions.DontRequireReceiver);
							//other.gameObject.SendMessageUpwards("DieTrap", this.trapType, SendMessageOptions.DontRequireReceiver);
						}
					}
					else
					{
						other.gameObject.SendMessageUpwards("Hit", 500, SendMessageOptions.DontRequireReceiver);
						//other.gameObject.SendMessageUpwards("DieTrap", this.trapType, SendMessageOptions.DontRequireReceiver);
						//if (other.gameObject.CompareTag("enemyCollide") && this.trigger.largeSpike)
						//{
						//    Vector3 vector = this.PutOnSpikes(other.transform.root.gameObject);
						//    other.gameObject.SendMessageUpwards("setPositionAtSpikes", vector, SendMessageOptions.DontRequireReceiver);
						//}
					}
					if (!this.disable && !this.trigger.largeSwingingRock)
					{
						base.Invoke("disableCollision", 0.8f);
					}
				}
			}
		}
	}
}