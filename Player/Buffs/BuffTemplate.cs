using System;

using UnityEngine;

namespace ChampionsOfForest.Player.Buffs
{
	public class BuffTemplate
	{
		public enum StackingFlags
		{
			Disabled = 0, //no multiple instances, no multiplication
			MultipleInstances = 1, //can there be multiple instances of the same buff with different values
								   //yes -> things like weapon damage buff
								   //no -> things like berserk spell active
			Accumulation = 2,//if the allowMultipleInstances property is set to false,
							 //should the current instance of the buff be overwritten or summed
			MultipleInstancesAccumulation = 3,
			OverrideToNewer = 4, //replace the previous buff instance with another?
			OverrideToLarger = 8,
			OverrideToSmaller = 16,
			OverrideToLonger = 32,
			AddToDuration = 64,
			AccumulationAdd = 130,
			AccumulationMultIncreasing = 258,
			AccumulationMultDecreasingToZero = 514,

		}
		internal delegate float BuffAction(float f);


		private readonly int id;
		private readonly BuffAction startCallback, endCallback;
		public readonly int dispellThreshold;
		public readonly bool hideAmount = true;
		public readonly bool hideDuration = true;
		private readonly Func<float, string> getAmountTextFunc;
		private readonly int textureID;
		public readonly StackingFlags stackingFlags;
		public bool isNegative => dispellThreshold > 0;


		private BuffTemplate(int id, BuffAction startCallback, BuffAction endCallback, int dispellThreshold, int textureID, StackingFlags stackingFlags, bool hideDuration, bool hideAmount, Func<float, string> getAmountTextFunc)
		{
			this.id = id;
			this.startCallback = startCallback;
			this.endCallback = endCallback;
			this.dispellThreshold = dispellThreshold;
			this.hideAmount = hideAmount;
			this.hideDuration = hideDuration;
			this.getAmountTextFunc = getAmountTextFunc;
			this.textureID = textureID;
			this.stackingFlags = stackingFlags;
		}

		//Positive buff constructor
		internal BuffTemplate(BuffManager.BuffType type, StackingFlags flags, int textureID, BuffAction startCallback, BuffAction endCallback, bool hideDuration = false, bool hideAmount = false, Func<float, string> getAmountTextFunc = null) : this((int)type, startCallback, endCallback, 0, textureID,flags, hideDuration, hideAmount, getAmountTextFunc)
		{

		}

		internal BuffTemplate(BuffManager.DebuffType type, StackingFlags flags, int textureID, BuffAction startCallback, BuffAction endCallback, int dispellThreshold = 2, bool hideDuration = false, bool hideAmount = false, Func<float, string> getAmountTextFunc = null) : this((int)type, startCallback, endCallback, dispellThreshold, textureID, flags, hideDuration, hideAmount, getAmountTextFunc)
		{

		}
		public void Start( float value)
		{
			startCallback(value);
		}

		public void End(float value)
		{
			endCallback(value);
		}
	}
}
