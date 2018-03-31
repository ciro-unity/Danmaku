using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class PathWalkerMixerBehaviour : PlayableBehaviour
{
	//This will iterate through the clips and forcedly call their ProcessFrame,
	//providing more data (like the current global Timeline time) so that the clip can handle bullets before and past its actual duration
     public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
		double timelineCurrentTime = (playable.GetGraph().GetResolver() as PlayableDirector).time;
		int inputCount = playable.GetInputCount();

		for (int i = 0; i < inputCount; i++)
		{
			ScriptPlayable<PathWalkerBehaviour> inputPlayable = (ScriptPlayable<PathWalkerBehaviour>)playable.GetInput(i);
			PathWalkerBehaviour input = inputPlayable.GetBehaviour();
			input.MixerProcessFrame(inputPlayable, info, playerData, timelineCurrentTime);
		}
	}
}
