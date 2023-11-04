using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace com.impactional.chess
{
    public class finderMulti : MonoBehaviour
    {
		public static GameObject RayHitFromScreen(Vector3 hitPosition)
		{
			Ray ray = converterMulti.ScreenPointToRay(hitPosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, float.PositiveInfinity, GameManagerMulti.Instance.CLickableMask))
			{
				return hit.transform.gameObject;
			}

			return null;
		}

		public static T RayHitFromScreen<T>(Vector3 hitPosition)
		{
			GameObject go = RayHitFromScreen(hitPosition);
			if (go == null) return default(T);
			object o = go.GetComponent(typeof(T));
			if (o == null) return default(T);
			return (T)Convert.ChangeType(o, typeof(T));
		}

		public static IClickable IClickableRayHitFromScreen(Vector3 hitPosition)
		{
			Ray ray = converterMulti.ScreenPointToRay(hitPosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, float.PositiveInfinity, GameManagerMulti.Instance.CLickableMask))
			{
				return hit.transform.gameObject.GetComponent(typeof(IClickable)) as IClickable;
			}

			return null;
		}
	}

}