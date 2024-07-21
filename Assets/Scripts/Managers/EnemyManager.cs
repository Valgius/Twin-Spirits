using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum PatrolType
{
    Patrol,
    Detect,
    Chase,
    Attack,
    Die
}

public enum EnemyType
{
    Fish,
    Frog,
    Spider,
}

public class EnemyManager : Singleton<EnemyManager>
{


}
