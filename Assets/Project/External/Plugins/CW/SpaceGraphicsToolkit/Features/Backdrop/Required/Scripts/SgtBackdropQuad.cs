using UnityEngine;

namespace SpaceGraphicsToolkit.Backdrop
{
	/// <summary>This class stores data for one quad generated by SgtBackdrop, this is typically procedurally generated.</summary>
	[System.Serializable]
	public class SgtBackdropQuad
	{
		/// <summary>Temp instance used when generating the starfield</summary>
		public static SgtBackdropQuad Temp = new SgtBackdropQuad();

		/// <summary>The coordinate index in the atlas texture.</summary>
		[Tooltip("The coordinate index in the atlas texture.")]
		public int Variant;

		/// <summary>Color tint of this quad.</summary>
		[Tooltip("Color tint of this quad.")]
		public Color Color = Color.white;

		/// <summary>Radius of this quad in local space.</summary>
		[Tooltip("Radius of this quad in local space.")]
		public float Radius;

		/// <summary>Angle of this quad in degrees.</summary>
		[Tooltip("Angle of this quad in degrees.")]
		public float Angle;

		/// <summary>Position of the quad in local space.</summary>
		[Tooltip("Position of the quad in local space.")]
		public Vector3 Position;

		/// <summary>How fast this star pulses.
		/// NOTE: This requires the starfield material's PULSE setting to be enabled.</summary>
		[Tooltip("How fast this star pulses.\n\nNOTE: This requires the starfield material's PULSE setting to be enabled.")]
		[Range(0.0f, 1.0f)]
		public float PulseSpeed = 1.0f;

		/// <summary>The pulse position will be offset by this value so they don't all pulse the same.
		/// NOTE: This requires the starfield material's PULSE setting to be enabled.</summary>
		[Tooltip("The pulse position will be offset by this value so they don't all pulse the same.\n\nNOTE: This requires the starfield material's PULSE setting to be enabled.")]
		[Range(0.0f, 1.0f)]
		public float PulseOffset;

		public void CopyFrom(SgtBackdropQuad other)
		{
			Variant     = other.Variant;
			Color       = other.Color;
			Radius      = other.Radius;
			Angle       = other.Angle;
			Position    = other.Position;
			PulseSpeed  = other.PulseSpeed;
			PulseOffset = other.PulseOffset;
		}
	}
}