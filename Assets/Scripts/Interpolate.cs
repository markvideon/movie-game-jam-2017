using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolate {

	private float value;
	private float runtime;

	private bool complete = false;
	private float accumulatedTime = 0f;

	public Interpolate (float input_value, float run_time) {
		value = input_value;
		runtime = run_time;
	}
	
	public bool Cycle () {
		
		if (!complete) {
			value = PerformLerp (1.5f, 0f, runtime);
		} 

		return complete;
	}


	private float PerformLerp(float a, float b, float runtime) {

		accumulatedTime += Time.deltaTime;

		if (accumulatedTime < runtime ) {
			return Mathf.Lerp (a,b,accumulatedTime/runtime);
		} else {
			complete = true;
			return b;
		}
			
	}

	public void Reset() {
		accumulatedTime = 0;
	}

}
