using UnityEngine;

public class EditorHitObject
{
    private Vector3 hitObjectPosition;
    private float hitObjectSpawnTime;
    private int hitObjectType, hitObjectSoundType, hitObjectAnimationType;

    // Properties
    public Vector3 HitObjectPosition
    {
        get { return hitObjectPosition; }
        set { hitObjectPosition = value; }
    }

    public float HitObjectSpawnTime
    {
        get { return hitObjectSpawnTime; }
        set { hitObjectSpawnTime = value; }
    }

    public int HitObjectType
    {
        get { return hitObjectType; }
        set { hitObjectType = value; }
    }

    public int HitObjectSoundType
    {
        get { return hitObjectSoundType; }
        set { hitObjectSoundType = value; }
    }

    public int HitObjectAnimationType
    {
        get { return hitObjectAnimationType; }
        set { hitObjectAnimationType = value; }
    }
}
