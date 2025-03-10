using System;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

[CreateAssetMenu(menuName = "Event Custom/On Collision Gate Event", fileName = "on_collision_gate_event")]
[EditorIcon("scriptable_event")]
public class OnCollisionGateEvent : BaseEvent<Tuple<BaseBlockPuzzle,GridSlot, BlockDirection>>
{
}