using UnityEngine;
using System.Collections;

public static class ArtifactPassiveSkill
{
    public delegate void PassiveSkill();

    private static void FireArtifactPassiveSkill()
    {

    }

    private static void IceArtifactPassiveSkill()
    {

    }

    private static void StoneArtifactPassiveSkill()
    {

    }

    public static PassiveSkill GetPassiveSkill(Artifact artifact)
    {
        switch(artifact)
        {
            case Artifact.Fire:
                {
                    return FireArtifactPassiveSkill;
                }

            case Artifact.Ice:
                {
                    return IceArtifactPassiveSkill;
                }

            case Artifact.Stone:
                {
                    return StoneArtifactPassiveSkill;
                }
        }
        return null;
    }
}
