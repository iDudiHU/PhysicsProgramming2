using UnityEngine;

public class ImpactSurfaceFX : MonoBehaviour
{
    public float HitRadius = 0.1f;
    public float Dirt = 1f;
    public float Burn = 1f;
    public float Heat = 1f;
    public float Clip = .0f;
    public Transform ImpactFX;
    public float ImpactSize = 0.3f;

    public void ApplyVFX(DamageFX dfx, RaycastHit hitInfo)
    {
        if (dfx != null)
		{
            dfx.Hit(dfx.transform.InverseTransformPoint(hitInfo.point), HitRadius, Dirt, Burn, Heat, Clip);
		}
        if (!ImpactFX) return;
        var fx = Instantiate(ImpactFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        fx.localScale = Vector3.one * HitRadius + Vector3.one * ImpactSize;
    }
}